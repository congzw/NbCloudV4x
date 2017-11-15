using System;
using Ninject.Infrastructure.Disposal;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public class NinjectAmbientScope : AmbientScope, INotifyWhenDisposed
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!IsDisposed)
            {
                this.IsDisposed = true;
                if (this.Disposed != null)
                {
                    this.Disposed(this, EventArgs.Empty);
                }
            }
        }
        public bool IsDisposed { get; private set; }
        public event EventHandler Disposed;
    }
}
