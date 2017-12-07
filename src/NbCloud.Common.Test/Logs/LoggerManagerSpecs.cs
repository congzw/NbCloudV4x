using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Logs
{
    [TestClass]
    public class LoggerManagerSpecs
    {
        [TestMethod]
        public void GetLogger_ByName_Should_OK()
        {
            var loggerManager = new LoggerManager();
            var logger = loggerManager.GetLogger("Foo");
            logger.ShouldNotNull();
            logger.Category.ShouldEqual("Foo");
            logger.DebugInvoke();
        }

        [TestMethod]
        public void GetLogger_ByType_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger(this.GetType());
            logger.ShouldNotNull();
            logger.Category.ShouldEqual(this.GetType().Name);
            logger.DebugInvoke();
        }

        [TestMethod]
        public void GetLogger_ByGenericType_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger<LoggerManagerSpecs>();
            logger.ShouldNotNull();
            logger.Category.ShouldEqual(this.GetType().Name);
            logger.DebugInvoke();
        }
        
        [TestMethod]
        public void GetLogger_Default_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger();
            logger.ShouldNotNull();
            logger.Category.ShouldEqual("");
            logger.DebugInvoke();
        }
    }
}
