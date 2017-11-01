using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common;
using NbCloud.TestLib;

namespace NbCloud.BaseLib.Tentants
{
    [TestClass]
    public class TenantHolderSpecs
    {
        private Func<List<Tenant>> _loadFunc = () => new List<Tenant>() { new Tenant() { Id = "1", UniqueName = "tenant1", Name = "租户1", DbConnectionString = "foo" } };

        [TestMethod]
        public void GetByUniqueName_Null_ShouldNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            tenantHolder.GetByUniqueName("tenant-x").ShouldNull();
        }

        [TestMethod]
        public void GetByUniqueName_NotExist_ShouldNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            tenantHolder.GetByUniqueName("tenant-x").ShouldNull();
        }

        [TestMethod]
        public void GetByUniqueName_Exist_ShouldNotNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            tenantHolder.GetByUniqueName("tenant1").ShouldNotNull();
        }

        [TestMethod]
        public void TryGetMatchTenant_HttpContextBaseNull_ShouldNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            var tryGetMatchTenant = tenantHolder.TryGetMatchTenant((HttpContextBase)null);
            tryGetMatchTenant.ShouldNull();
        }

        [TestMethod]
        public void TryGetMatchTenant_HttpContextNull_ShouldNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            var tryGetMatchTenant = tenantHolder.TryGetMatchTenant((HttpContext)null);
            tryGetMatchTenant.ShouldNull();
        }

        [TestMethod]
        public void TryGetMatchTenant_Null_ShouldNull()
        {
            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_loadFunc);
            var tryGetMatchTenant = tenantHolder.TryGetMatchTenant();
            tryGetMatchTenant.ShouldNull();
        }

        [TestMethod]
        public void TryGetMatchTenant_Reset_ShouldReturnByFunc()
        {
            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.SetFactoryFunc(() => new MockTenantContextHelper());

            Func<List<Tenant>> _newLoadFunc = () => new List<Tenant>() {
                new Tenant() { Id = "1", UniqueName = "tenant1", Name = "租户1", DbConnectionString = "foo" },
                new Tenant() { Id = "2", UniqueName = "tenant2", Name = "租户2", DbConnectionString = "foo" }
            };

            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_newLoadFunc);

            var tryGetMatchTenant = tenantHolder.TryGetMatchTenant();
            tryGetMatchTenant.ShouldNotNull();
            tryGetMatchTenant.UniqueName.ShouldEqual("tenant1");

            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.ResetFactoryFunc();
        }

        [TestMethod]
        public void TryGetMatchTenant_Reset_ShouldReturnByFunc2()
        {
            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.SetFactoryFunc(() => new MockTenantContextHelper());

            Func<List<Tenant>> _newLoadFunc = () => new List<Tenant>() {
                //new Tenant() { Id = "1", UniqueName = "tenant1", Name = "租户1", DbConnectionString = "foo" },
                new Tenant() { Id = "2", UniqueName = "tenant2", Name = "租户2", DbConnectionString = "foo" }
            };

            var tenantHolder = new TenantHolder();
            tenantHolder.ReloadAll(_newLoadFunc);

            var tryGetMatchTenant = tenantHolder.TryGetMatchTenant();
            tryGetMatchTenant.ShouldNull();

            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.ResetFactoryFunc();
        }

    }
}
