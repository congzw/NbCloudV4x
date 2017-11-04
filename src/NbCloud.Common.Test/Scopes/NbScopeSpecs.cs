using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Scopes
{
    [TestClass]
    public class MyScopeSpecs
    {
        [TestMethod]
        public void Null_Args_ShouldThrowEx()
        {
            AssertHelper.ShouldThrows<ArgumentNullException>(() =>
            {
                var nbScope = new NbScope(null);
            });
        }

        [TestMethod]
        public void Dispose_ShouldBySetting_False()
        {
            var emptyTrancationManager = new EmptyTrancationManager();
            bool disposedInvoked = false;
            emptyTrancationManager.DisposeInvoking = manager => disposedInvoked = true;
            using (var nbScope = new NbScope(emptyTrancationManager) { ShouldReleaseTrancationManager = false })
            {
            }
            disposedInvoked.ShouldFalse();
        }

        [TestMethod]
        public void Dispose_ShouldBySetting_True()
        {
            var emptyTrancationManager = new EmptyTrancationManager();
            bool disposedInvoked = false;
            emptyTrancationManager.DisposeInvoking = manager => disposedInvoked = true;

            using (var nbScope = new NbScope(emptyTrancationManager) { ShouldReleaseTrancationManager = true })
            {
            }

            disposedInvoked.ShouldTrue();
        }
    }
}
