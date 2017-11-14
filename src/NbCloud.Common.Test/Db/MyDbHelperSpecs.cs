using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Db
{
    [TestClass]
    public class MyDbConfigHelperSpecs
    {

        private static readonly string BadConnName = "BadConn";
        private static readonly string ConnName = "DemoConn";
        private static readonly string DbName = "DemoDb";

        [TestMethod]
        public void ExistConnectionString_Exist_Should_True()
        {
            var helper = new MyDbConfigHelper();
            helper.ExistConnectionString(ConnName).ShouldTrue();
        }

        [TestMethod]
        public void ExistConnectionString_NotExist_Should_False()
        {
            var helper = new MyDbConfigHelper();
            helper.ExistConnectionString(BadConnName).ShouldFalse();
        }

        [TestMethod]
        public void GetConnectionString_Exist_Should_NotNull()
        {
            var helper = new MyDbConfigHelper();
            helper.GetConnectionString(ConnName).ShouldNotNull();
        }

        [TestMethod]
        public void GetConnectionString_NotExist_Should_Ex()
        {
            var helper = new MyDbConfigHelper();
            AssertHelper.ShouldThrows<Exception>(() =>
            {
                helper.GetConnectionString(BadConnName);
            });
        }

        [TestMethod]
        public void FindDbNameFromConnectionString_Exist_Should_Find()
        {
            var helper = new MyDbConfigHelper();
            var connectionString = helper.GetConnectionString(ConnName);
            helper.FindDbNameFromConnectionString(connectionString).ShouldNotNull().ShouldEqual(DbName);
        }
        
        [TestMethod]
        public void FindDbNameFromConnectionString_ArgNull_Should_Ex()
        {
            var helper = new MyDbConfigHelper();
            AssertHelper.ShouldThrows<Exception>(() =>
            {
                helper.FindDbNameFromConnectionString(null);
            });
        }
    }
}
