//using System.Globalization;
//using System.Web.Mvc;
//using NbCloud.Common;
//using Ninject.Web.Mvc.FilterBindingSyntax;

//namespace NbCloud.BaseLib.Traces.Demos
//{
//    public class MvcTraceFilter : System.Web.Mvc.IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var message = string.Format(CultureInfo.InvariantCulture, "[RequestTraceFilter OnActionExecuting] {0}.{1}",
//                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
//                filterContext.ActionDescriptor.ActionName);

//            UtilsLogger.LogMessage(message);
//        }

//        public void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            var message = string.Format(CultureInfo.InvariantCulture, "[RequestTraceFilter OnActionExecuted] {0}.{1}",
//                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
//                filterContext.ActionDescriptor.ActionName);

//            UtilsLogger.LogMessage(message);
//        }
//    }

//    public class RequestTraceBindModule : NinjectModule
//    {
//        public override void Load()
//        {
//            this.BindFilter<MvcTraceFilter>(FilterScope.Action, 0);
//        }
//    }
//}
