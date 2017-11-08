using System;
using Ninject.Infrastructure.Disposal;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public class NinjectAmbientScope : AmbientScope, INotifyWhenDisposed
    {
        public override void Dispose()
        {
            base.Dispose();
            this.IsDisposed = true;
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }
        public bool IsDisposed { get; private set; }
        public event EventHandler Disposed;
    }
}
