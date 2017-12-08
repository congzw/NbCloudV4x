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
            logger.Name.ShouldEqual("Foo");
            logger.DebugInvoke();
        }

        [TestMethod]
        public void GetLogger_ByType_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger(this.GetType());
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(this.GetType().Name);
            logger.DebugInvoke();
        }

        [TestMethod]
        public void GetLogger_ByGenericType_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger<LoggerManagerSpecs>();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(this.GetType().Name);
            logger.DebugInvoke();
        }
        
        [TestMethod]
        public void GetLogger_Default_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(LoggerConfig.Resolve().DefaultLoggerName);
            logger.DebugInvoke();
        }

        [TestMethod]
        public void GetLogger_LoggerType_Should_OK()
        {
            var loggerManager = new LoggerManager();

            var logger = loggerManager.GetLogger();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(LoggerConfig.Resolve().DefaultLoggerName);
            logger.DebugInvoke();
        }
    }
}
