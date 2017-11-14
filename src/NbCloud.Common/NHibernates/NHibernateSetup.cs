using System;
using System.Collections.Generic;
using System.Reflection;
using NbCloud.Common.Db;
using NbCloud.Common.Logs;

namespace NbCloud.Common.NHibernates
{
    public class NHibernateSetup
    {
        private readonly IMyLogHelper _myLogHelper;
        private readonly IMySqlScriptHelper _mySqlScriptHelper;
        private readonly IMySessionFactory _mySessionFactory;

        public NHibernateSetup(
             IMySqlScriptHelper mySqlScriptHelper
            , IMySessionFactory mySessionFactory)
        {
            _myLogHelper = MyLogHelper.Resolve();
            _mySqlScriptHelper = mySqlScriptHelper;
            _mySessionFactory = mySessionFactory;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public void AutoCreateDatabase()
        {
            Log("====AutoCreateDatabase Begin====");
            var myDbConfigHelper = MyDbConfigHelper.Resolve();
            var myProjectHelper = MyProjectHelper.Resolve();
            var connName = myDbConfigHelper.GuessDefaultConnName();
            Log("GetConnName: " + connName);

            var connString = myDbConfigHelper.GetConnectionString(connName);
            Log("GetConnectionString: " + connString);

            var dbName = myDbConfigHelper.FindDbNameFromConnectionString(connString);
            Log("FindDbNameFromConnectionString: " + dbName);

            var appAssemblies = myProjectHelper.LoadAppAssemblies();
            CreateDbIfNecessary(connName, dbName, connString, appAssemblies);
            Log("====AutoCreateDatabase End====");
        }

        public void AutoCreateDatabase(string connName, string dbName, string connString, IList<Assembly> appAssemblies)
        {
            Log("====AutoCreateDatabase Begin====");
            Log("ConnName: " + connName);
            Log("ConnectionString: " + connString);
            Log("DbName: " + dbName);
            CreateDbIfNecessary(connName, dbName, connString, appAssemblies);
            Log("====AutoCreateDatabase End====");
        }

        private void CreateDbIfNecessary(string connName, string dbName, string connString, IList<Assembly> appAssemblies)
        {
            var dbExist = _mySqlScriptHelper.CheckDbExist(connString, dbName).Success;
            Log(string.Format("CheckDbExist {0}: {1}", dbName, dbExist));
            if (!dbExist)
            {
                Log("Create New Database: " + dbName);
                _mySqlScriptHelper.CreateDbIfNotExist(connString, dbName);
                Log("Create New Database Done. ");
            }

            Log("InitSessionFactory: " + connName);
            _mySessionFactory.InitSessionFactory(connName, !dbExist, appAssemblies);
        }

        private void Log(string message)
        {
            _myLogHelper.Debug(string.Format("[{0}] => {1}", typeof(NHibernateSetup).Name, message));
        }
    }
}
