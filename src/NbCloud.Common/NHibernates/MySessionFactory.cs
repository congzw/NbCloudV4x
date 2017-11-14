using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NbCloud.Common.Logs;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;

namespace NbCloud.Common.NHibernates
{
    public interface IMySessionFactory : IResolveAsSingleton
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="connName"></param>
        /// <param name="rebulidSchema"></param>
        /// <param name="mapperAssemblies"></param>
        /// <param name="sessionFactoryKey"></param>
        void InitSessionFactory(string connName, bool rebulidSchema, IList<Assembly> mapperAssemblies, string sessionFactoryKey = "");
        /// <summary>
        /// 获取某个已经初始化过的实例
        /// </summary>
        /// <param name="sessionFactoryKey"></param>
        /// <returns></returns>
        ISessionFactory GetSessionFactory(string sessionFactoryKey = "");
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="sessionFactoryKey"></param>
        /// <returns></returns>
        ISession OpenSession(string sessionFactoryKey = "");
    }

    public class MySessionFactory : IMySessionFactory
    {
        #region for di extensions

        private static Func<IMySessionFactory> _resolve = () => ResolveAsSingleton.Resolve<MySessionFactory, IMySessionFactory>();
        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static Func<IMySessionFactory> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
        
        public void InitSessionFactory(string connName, bool rebulidSchema, IList<Assembly> mapperAssemblies, string sessionFactoryKey = "")
        {
            if (string.IsNullOrWhiteSpace(connName))
            {
                throw new ArgumentException("必须指定connName");
            }

            string connStr = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            InitSessionFactory(MsSqlConfiguration.MsSql2005.ConnectionString(connStr), m =>
            {
                //解决同类名映射问题
                m.FluentMappings.Conventions.Setup(x => x.Add(AutoImport.Never()));

                if (mapperAssemblies.Count > 0)
                {
                    //需要在站架子统一注册的mapper
                    foreach (var mapperAssembly in mapperAssemblies)
                    {
                        m.FluentMappings.AddFromAssembly(mapperAssembly);
                    }
                }
            }
                , rebulidSchema, sessionFactoryKey);
        }
        public ISessionFactory GetSessionFactory(string sessionFactoryKey = "")
        {
            if (SessionFactories.ContainsKey(sessionFactoryKey))
            {
                return SessionFactories[sessionFactoryKey];
            }
            return null;
        }
        public ISession OpenSession(string sessionFactoryKey = "")
        {
            var session = SessionFactories[sessionFactoryKey].OpenSession();
            var message = string.Format(">>>>>>>>>>> open new session [{2}] => <{0},{1}>", session.GetType().Name, session.GetHashCode(), sessionFactoryKey);
            LogMessage(message);
            return session;
        }
        
        #region helpers

        /// <summary>
        /// 全局统一SessionFactory初始化，一般在Application_Start方法中调用。
        /// 一次调用初始化一个SessionFactory
        /// </summary>
        /// <param name="dbCfg">NH数据库配置</param>
        /// <param name="mappings">NH映射配置</param>
        /// <param name="buildSchema">是否自动创建或重新创建数据库表，默认不创建</param>
        /// <param name="sessionFactoryKey">SessionFactory名，标识不同的SessionFactory, 默认名为Empty</param>
        /// <param name="script"></param>
        /// <param name="export"></param>
        /// <param name="justDrop"></param>
        public void InitSessionFactory(IPersistenceConfigurer dbCfg, Action<MappingConfiguration> mappings, bool buildSchema = false, string sessionFactoryKey = "", bool script = true, bool export = true, bool justDrop = false)
        {
            if (SessionFactories.ContainsKey(sessionFactoryKey))
                throw new ArgumentException("Duplicate Session Factory name");

            var cfg = Fluently.Configure()
                .Database(dbCfg)
                .Mappings(mappings)
                .BuildConfiguration();

            if (buildSchema)
            {
                BuildSchema(cfg, script, export, justDrop);
            }
            else
            {
                //自动补齐表和列
                Updatechema(cfg, script);
            }

            SessionFactories[sessionFactoryKey] = cfg.BuildSessionFactory();
        }

        protected void BuildSchema(Configuration configuration, bool script = true, bool export = true, bool justDrop = false)
        {
            //new SchemaUpdate(configuration).Execute(script, true);
            new SchemaExport(configuration).Execute(script, export, justDrop);
        }
        protected void Updatechema(Configuration configuration, bool script = true)
        {
            new SchemaUpdate(configuration).Execute(script, true);
        }

        protected static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();

        protected void LogMessage(string message)
        {
            MyLogHelper.Resolve().Debug("[MyNHibernateHelper]=> " + message);
        }

        #endregion
    }
}
