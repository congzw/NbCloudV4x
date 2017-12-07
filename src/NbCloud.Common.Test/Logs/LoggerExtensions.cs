namespace NbCloud.Common.Logs
{
    public static class LoggerExtensions
    {
        public static void DebugInvoke(this ILogger logger)
        {
            var invokeMethodBase = StackTraceHelper.GetInvokeMethodBase(2);
            logger.Debug(invokeMethodBase.Name);
        }
    }
}
