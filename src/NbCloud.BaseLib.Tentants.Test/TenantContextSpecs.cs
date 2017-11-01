using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common;
using NbCloud.TestLib;

namespace NbCloud.BaseLib.Tentants
{
    [TestClass]
    public class TenantContextSpecs
    {
        [TestMethod]
        public void GetCurrent_HttpContextBaseNull_ShouldReturnEmpty()
        {
            var tenantContextHelper = TenantContextHelper.Resolve();
            var tenantContext = tenantContextHelper.GetCurrent((HttpContextBase)null);
            tenantContext.ShouldNotNull();
            tenantContext.IsEmpty().ShouldTrue();
        }
        [TestMethod]
        public void GetCurrent_HttpContextNull_ShouldReturnEmpty()
        {
            var tenantContextHelper = TenantContextHelper.Resolve();
            var tenantContext = tenantContextHelper.GetCurrent((HttpContext)null);
            tenantContext.ShouldNotNull();
            tenantContext.IsEmpty().ShouldTrue();
        }
        [TestMethod]
        public void GetCurrent_Null_ShouldReturnEmpty()
        {
            var tenantContextHelper = TenantContextHelper.Resolve();
            var tenantContext = tenantContextHelper.GetCurrent();
            tenantContext.ShouldNotNull();
            tenantContext.IsEmpty().ShouldTrue();
        }
        [TestMethod]
        public void GetCurrent_Reset_ShouldReturnByFunc()
        {
            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.SetFactoryFunc(() => new MockTenantContextHelper());
            
            var tenantContextHelper = TenantContextHelper.Resolve();
            var tenantContext = tenantContextHelper.GetCurrent();
            tenantContext.ShouldNotNull();
            tenantContext.IsEmpty().ShouldFalse();
            tenantContext.UniqueName.ShouldEqual("tenant1");

            ResolveAsSingleton<TenantContextHelper, ITenantContextHelper>.ResetFactoryFunc();
        }
    }

    #region mock helper
    
    public class MockTenantContextHelper : ITenantContextHelper
    {
        public TenantContext GetCurrent(HttpContextBase httpContext)
        {
            return new TenantContext() { UniqueName = "tenant1" };
        }
    }

    #endregion
}
