using System.Web;
using NbCloud.Common;

namespace NbCloud.BaseLib.Tentants
{
    public class TenantContext
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string UniqueName { get; set; }

        public bool IsEmpty()
        {
            return this == Empty;
        }
        static TenantContext()
        {
            Empty = new TenantContext();
        }
        public static TenantContext Empty { get; private set; }
    }

    public interface ITenantContextHelper
    {
        TenantContext GetCurrent(HttpContextBase httpContext);
    }

    public class TenantContextHelper : ITenantContextHelper, IResolveAsSingleton
    {
        #region ioc helpers

        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static ITenantContextHelper Resolve()
        {
            return ResolveAsSingleton.Resolve<TenantContextHelper, ITenantContextHelper>();
        }

        #endregion
        
        private static string tenantKey = "tenant";
        public TenantContext GetCurrent(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                return TenantContext.Empty;
            }

            var tenantCache = httpContext.Items[tenantKey] as TenantContext;
            if (tenantCache == null)
            {
                tenantCache = TryCreateTenantContext(httpContext);
                httpContext.Items[tenantKey] = tenantCache;
            }
            return tenantCache;
        }

        private TenantContext TryCreateTenantContext(HttpContextBase httpContext)
        {
            //默认: subdomain => tenant
            //Host:tenant1.localhost
            //Host:tenant2.localhost
            var fullHost = httpContext.Request.Headers["Host"].Split('.');
            if (fullHost.Length < 2)
            {
                return TenantContext.Empty;
            }
            var subdomain = fullHost[0];
            //httpContext.Request.RequestContext.RouteData.Values[tenantKey] = subdomain;
            return new TenantContext() { UniqueName = subdomain };
        }
    }

    public static class TenantContextProviderExtension
    {
        public static TenantContext GetCurrent(this ITenantContextHelper helper)
        {
            return helper.GetCurrent(HttpContext.Current);
        }

        public static TenantContext GetCurrent(this ITenantContextHelper helper, HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return helper.GetCurrent(null);
            }
            return helper.GetCurrent(new HttpContextWrapper(httpContext));
        }
    }
}
