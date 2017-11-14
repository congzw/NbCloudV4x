using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Web
{
    [TestClass]
    public class MyHostHelperSpecs
    {
        [TestMethod]
        public void GetMachineName_Should_NotNull()
        {
            var hostHelper = new MyHostHelper();
            hostHelper.GetMachineName().ShouldNotNull();
        }

        [TestMethod]
        public void GetApplicationName_NonWeb_Should_Null()
        {
            var hostHelper = new MyHostHelper();
            hostHelper.GetApplicationName().ShouldNull();
        }

        [TestMethod]
        public void GetApplicationPhysicalPath_NonWeb_Should_Null()
        {
            var hostHelper = new MyHostHelper();
            hostHelper.GetApplicationPhysicalPath().ShouldNull();
        }
    }
}
