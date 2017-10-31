using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NbCloud.Common.Tenants
{
    public class Tenant
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string UniqueName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }
    }
    
    public interface ITenantHolder : ISingletonDependency
    {
        IList<Tenant> Tenants { get; }
        void Add(Tenant tenant);
        void Add(IList<Tenant> tenants);
        void Remove(Tenant tenant);
        void Update(Tenant tenant);
        Tenant Get(string uniqueName);
    }

    public static class TenantHolderExtension
    {
        public static Tenant TryGetMatchTenant(this ITenantHolder tenantHolder, HttpContextBase httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    return null;
                }

                var tenantContextHelper = TenantContextHelper.Resolve();
                var tenantContext = tenantContextHelper.GetCurrent(httpContext);
                if (tenantContext.IsEmpty())
                {
                    return null;
                }

                var theOne = tenantHolder.Tenants.SingleOrDefault(x => x.UniqueName.Equals(tenantContext.UniqueName, StringComparison.OrdinalIgnoreCase));
                return theOne;
            }
            catch (HttpException ex)
            {
                UtilsLogger.LogMessage(typeof(TenantHolderExtension), ex.Message);
                return null;
            }
        }

        public static Tenant TryGetMatchTenant(this ITenantHolder tenantHolder, HttpContext httpContext)
        {
            return tenantHolder.TryGetMatchTenant(new HttpContextWrapper(httpContext));
        }
    }

    //https://www.codeproject.com/articles/848111/multi-tenancy-system-with-separate-databases-in-mv
    
    //http://nocture.dk/2015/02/12/subdomain-based-multitenancy-asp-net-mvc-5/

    //IIS Express config: %USERPROFILE%\My Documents\IISExpress\config\applicationhost.config
    //<site name="NbCloud.Web(13)" id="472">
    //    <application path="/" applicationPool="Clr4IntegratedAppPool">
    //        <virtualDirectory path="/" physicalPath="D:\WS_Local\NbCloudV4x\src\NbCloud.Web" />
    //    </application>
    //    <bindings>
    //        <binding protocol="http" bindingInformation="*:25437:localhost" />
    //        <binding protocol="http" bindingInformation="*:25437:tenant1.localhost" />
    //        <binding protocol="http" bindingInformation="*:25437:tenant2.localhost" />
    //    </bindings>
    //</site>

    ////C:\Windows\System32\drivers\etc\hosts
    //127.0.0.1	localhost
    //127.0.0.1	tenant1.localhost.com
    //127.0.0.1	tenant2.localhost.com

    //public class TenantActionFilter : ActionFilterAttribute, IActionFilter
    //{
    //    public void OnActionExecuting(ActionExecutingContext filterContext) 
    //    { 
    //        var fullAddress = filterContext.HttpContext.Request.Headers["Host"].Split('.'); 
    //        if (fullAddress.Length < 2) 
    //        { 
    //            filterContext.Result = new HttpStatusCodeResult(404); //or redirect filterContext.Result = new RedirectToRouteResult(..);
    //        } 

    //        var tenantSubdomain = fullAddress[0]; 

    //        // Lookup tenant id (preferably use a cache) 
    //        var tenantId = ... filterContext.RouteData.Values.Add("tenant", tenantId);
    //        base.OnActionExecuting(filterContext); 
    //    }
    //}
}
