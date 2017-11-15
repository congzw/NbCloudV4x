using System;
using System.Runtime.InteropServices;

namespace NbCloud.Common.Demos
{
    //https://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=ZH-CN&k=k(CA1063);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5)&rd=true
    public class DisposeResourceDemo : IDisposable
    {
        private IntPtr _nativeResource = Marshal.AllocHGlobal(100);
        private IDisposable _managedResource = null;

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~DisposeResourceDemo()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_managedResource != null)
                {
                    _managedResource.Dispose();
                    _managedResource = null;
                }
            }
            // free native resources if there are any.
            if (_nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_nativeResource);
                _nativeResource = IntPtr.Zero;
            }
        }
    }
}
