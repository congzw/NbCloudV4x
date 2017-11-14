using System;
using System.Configuration;
using System.Data.SqlClient;

namespace NbCloud.Common.Db
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public interface IMyDbConfigHelper
    {
        /// <summary>
        /// 指定的连接名是否存在
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        bool ExistConnectionString(string connName);

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        string GetConnectionString(string connName);

        /// <summary>
        /// FindDbNameFromConnectionString
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        string FindDbNameFromConnectionString(string connString);
    }

    public class MyDbConfigHelper : IMyDbConfigHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyDbConfigHelper> _resolve = () => ResolveAsSingleton.Resolve<MyDbConfigHelper, IMyDbConfigHelper>();
        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static Func<IMyDbConfigHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public bool ExistConnectionString(string connName)
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[connName];
            if (connectionStringSetting == null)
            {
                //没有找到
                return false;
            }
            return !string.IsNullOrWhiteSpace(connectionStringSetting.ConnectionString);
        }

        public string GetConnectionString(string connName)
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[connName];
            if (connectionStringSetting == null)
            {
                //没有找到
                throw new Exception(string.Format("没有从配置中找到名为{0}的数据库连接！", connName));
            }
            return connectionStringSetting.ConnectionString;
        }
        
        public string FindDbNameFromConnectionString(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new ArgumentException("必须指定connString");
            }

            //如果配置名相符，就将数据库连接字符串中的InitialCatalog取出，作为DbName
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connString);
            var dbName = sqlConnectionStringBuilder.InitialCatalog;
            return dbName;
        }
    }
}
