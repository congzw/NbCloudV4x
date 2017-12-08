using System.Diagnostics;
using System.Reflection;

namespace NbCloud.Common
{
    public class StackTraceHelper
    {
        public static MethodBase GetInvokeMethodBase(int index)
        {
            var stackTrace = new StackTrace();
            var mi = stackTrace.GetFrame(index).GetMethod();
            return mi;
        }
    }
}
