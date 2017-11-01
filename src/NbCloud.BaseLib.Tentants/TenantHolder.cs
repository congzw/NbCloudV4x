using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NbCloud.Common;

namespace NbCloud.BaseLib.Tentants
{
    public interface ITenantHolder : ISingletonDependency
    {
        IList<Tenant> Tenants { get; }
        Tenant GetByUniqueName(string uniqueName);
        void ReloadAll(Func<IList<Tenant>> reloadFunc);
    }

    public class TenantHolder : ITenantHolder
    {
        public TenantHolder()
        {
            Tenants = new List<Tenant>();
        }

        public IList<Tenant> Tenants { get; private set; }

        public Tenant GetByUniqueName(string uniqueName)
        {
            if (string.IsNullOrWhiteSpace(uniqueName))
            {
                return null;
            }
            var theOne = Tenants.SingleOrDefault(x => x.UniqueName.Equals(uniqueName, StringComparison.OrdinalIgnoreCase));
            return theOne;
        }

        public void ReloadAll(Func<IList<Tenant>> reloadFunc)
        {
            if (reloadFunc == null)
            {
                throw new InvalidOperationException("ReloadTenantsFunc不能为空");
            }
            Tenants = reloadFunc();
        }
    }

    public static class TenantHolderExtension
    {
        public static Tenant TryGetMatchTenant(this ITenantHolder tenantHolder, HttpContextBase httpContext)
        {
            try
            {
                var tenantContextHelper = TenantContextHelper.Resolve();
                var tenantContext = tenantContextHelper.GetCurrent(httpContext);
                if (tenantContext.IsEmpty())
                {
                    return null;
                }

                var theOne = tenantHolder.GetByUniqueName(tenantContext.UniqueName);
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
            if (httpContext == null)
            {
                return tenantHolder.TryGetMatchTenant((HttpContextBase)null);
            }
            return tenantHolder.TryGetMatchTenant(new HttpContextWrapper(httpContext));
        }

        public static Tenant TryGetMatchTenant(this ITenantHolder tenantHolder)
        {
            return tenantHolder.TryGetMatchTenant(HttpContext.Current);
        }
    }
}
