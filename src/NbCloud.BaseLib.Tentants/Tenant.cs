using System.Collections.Generic;
using System.Web;
using NbCloud.Common;

namespace NbCloud.BaseLib.Tentants
{
    public class Tenant
    {
        /// <summary>
        /// 唯一名
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

                //var theOne = tenantHolder.Tenants.SingleOrDefault(x => x.UniqueName.Equals(tenantContext.UniqueName, StringComparison.OrdinalIgnoreCase));
                var theOne = tenantHolder.Get(tenantContext.UniqueName);
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

        public static Tenant TryGetMatchTenant(this ITenantHolder tenantHolder)
        {
            return tenantHolder.TryGetMatchTenant(HttpContext.Current);
        }
    }
}
