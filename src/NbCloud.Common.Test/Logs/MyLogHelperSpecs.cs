using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common.Logs.Log4Net;

namespace NbCloud.Common.Logs
{
    [TestClass]
    public class MyLogHelperSpecs
    {
        [TestMethod]
        public void Debug_Should_OK()
        {
            var myLogHelper = new MyLogHelper();
            myLogHelper.Debug("Foo");
        }

        [TestMethod]
        public void Debug_With_Log4Net_Should_OK()
        {
            var myLogHelper = new MyLogHelper();
            myLogHelper.LoggerManagerFunc = ()=> new Log4NetLoggerManager();
            myLogHelper.Debug("Foo");
        }
    }
}
