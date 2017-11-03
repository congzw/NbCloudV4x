//using System;
//using System.Globalization;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http.Controllers;
//using System.Web.Mvc;
//using NbCloud.Common;
//using Ninject.Modules;
//using Ninject.Web.WebApi.FilterBindingSyntax;

//namespace NbCloud.BaseLib.Traces.Demos
//{
//    public class ApiTraceFilter : System.Web.Http.Filters.IActionFilter
//    {
//        public bool AllowMultiple
//        {
//            get { return true; }
//        }
//        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
//        {
//            var message = string.Format(CultureInfo.InvariantCulture, "[ApiTraceFilter Executing] {0}.{1}",
//                actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
//                actionContext.ActionDescriptor.ActionName);
//            UtilsLogger.LogMessage(message);
            
//            var result = await continuation();

//            var message2 = string.Format(CultureInfo.InvariantCulture, "[ApiTraceFilter Executed] {0}.{1}",
//                actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
//                actionContext.ActionDescriptor.ActionName);
//            UtilsLogger.LogMessage(message2);
//            return result;
//        }
//    }

//    public class ApiTraceFilterModule : NinjectModule
//    {
//        public override void Load()
//        {
//            this.BindHttpFilter<ApiTraceFilter>(System.Web.Http.Filters.FilterScope.Global);
//        }
//    }
//}
