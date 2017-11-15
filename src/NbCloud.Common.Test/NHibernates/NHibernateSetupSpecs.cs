﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common.Db;
using NbCloud.TestLib;

namespace NbCloud.Common.NHibernates
{
    //这个是测试数据库的自动创建，按需执行即可
    //[TestClass]
    public class NHibernateSetupSpecs
    {
        private static readonly string ConnName = "DemoConn";
        private static readonly string DbName = "DemoDb";

        [TestMethod]
        public void InitDatabase_Should_Init()
        {
            var myDbConfigHelper = MyDbConfigHelper.Resolve();
            var connectionString = myDbConfigHelper.GetConnectionString(ConnName);
            var mySqlScriptHelper = MySqlScriptHelper.Resolve();
            mySqlScriptHelper.DropDbIfExist(connectionString, DbName);
            mySqlScriptHelper.CheckDbExist(connectionString, DbName).Success.ShouldFalse();
            var nHibernateSetup = new NHibernateSetup(mySqlScriptHelper, MySessionFactory.Resolve());
            nHibernateSetup.AutoCreateDatabase();
            mySqlScriptHelper.CheckDbExist(connectionString, DbName).Success.ShouldTrue();
        }
    }
}