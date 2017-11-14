using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class MyConfigHelperSpecs
    {
        private static readonly string NotExistKey = "Whatever";
        private static readonly string ExistKey = "Foo";
        private static readonly string ExistKeyValue = "FooValue";
        private static readonly string ExistKeyForBool = "FooBoolean";

        [TestMethod]
        public void GetAppSettingValue_Exist_Should_Return_Setting()
        {
            var helper = new MyConfigHelper();
            helper.GetAppSettingValue(ExistKey, "ABC").ShouldNotNull().ShouldEqual(ExistKeyValue);
        }

        [TestMethod]
        public void GetAppSettingValue_NotExist_Should_Return_Default()
        {
            var helper = new MyConfigHelper();
            helper.GetAppSettingValue(NotExistKey, "ABC").ShouldNotNull().ShouldEqual("ABC");
            helper.GetAppSettingValue(NotExistKey, "").ShouldNotNull().ShouldEqual("");
            helper.GetAppSettingValue(NotExistKey, null).ShouldNull().ShouldEqual(null);
        }


        [TestMethod]
        public void GetAppSettingValueAsBool_Exist_Should_Return_Setting()
        {
            var helper = new MyConfigHelper();
            helper.GetAppSettingValueAsBool(ExistKeyForBool, false).ShouldTrue();
        }

        [TestMethod]
        public void GetAppSettingValueAsBool_NotExist_Should_Return_Default()
        {
            var helper = new MyConfigHelper();
            helper.GetAppSettingValueAsBool(NotExistKey, false).ShouldFalse();
        }
    }
}
