﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Logs.Log4Net
{
    [TestClass]
    public class Log4NetLoggerManagerSpecs
    {
        [TestMethod]
        public void GetLogger_ByName_Should_OK()
        {
            var loggerManager = new Log4NetLoggerManager();

            var logger = loggerManager.GetLogger("NbCloud.Demo");
            logger.ShouldNotNull();
            logger.Name.ShouldEqual("NbCloud.Demo");
            logger.LogSelfInvoke();
        }

        [TestMethod]
        public void GetLogger_ByType_Should_OK()
        {
            var loggerManager = new Log4NetLoggerManager();

            var logger = loggerManager.GetLogger(this.GetType());
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(this.GetType().Name);
            logger.LogSelfInvoke();
        }

        [TestMethod]
        public void GetLogger_ByGenericType_Should_OK()
        {
            var loggerManager = new Log4NetLoggerManager();

            var logger = loggerManager.GetLogger<Log4NetLoggerManagerSpecs>();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(this.GetType().Name);
            logger.LogSelfInvoke();
        }

        [TestMethod]
        public void GetLogger_Default_Should_OK()
        {
            var loggerManager = new Log4NetLoggerManager();

            var logger = loggerManager.GetLogger();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(LoggerConfig.Resolve().DefaultLoggerName);
            logger.LogSelfInvoke();
        }

        [TestMethod]
        public void GetLogger_LoggerType_Should_OK()
        {
            var loggerManager = new Log4NetLoggerManager();

            var logger = loggerManager.GetLogger();
            logger.ShouldNotNull();
            logger.Name.ShouldEqual(LoggerConfig.Resolve().DefaultLoggerName);
            logger.LogSelfInvoke();
        }
    }
}