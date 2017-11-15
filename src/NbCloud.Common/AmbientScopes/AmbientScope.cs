using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;

namespace NbCloud.Common.AmbientScopes
{
    public class AmbientScope : IDisposable
    {
        private static readonly string _scopeStackKey = "AmbientScopeStack";
        internal static ConcurrentStack<IDisposable> ScopeStack
        {
            get
            {
                return CallContext.LogicalGetData(_scopeStackKey) as ConcurrentStack<IDisposable>;
            }
            set
            {
                CallContext.LogicalSetData(_scopeStackKey, value);
            }
        }

        public AmbientScope()
        {
            if (ScopeStack == null)
            {
                ScopeStack = new ConcurrentStack<IDisposable>();
            }
            ScopeStack.Push(this);
        }

        public static IDisposable Current
        {
            get
            {
                if (ScopeStack == null || ScopeStack.IsEmpty)
                {
                    return null;
                }
                IDisposable result;
                ScopeStack.TryPeek(out result);
                return result;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~AmbientScope()
        {
            Dispose(false);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            //本类内部没有封装任何托管或非托管资源，所以无需额外处理，直接维护正确的容器结构即可
            if (!_disposed)
            {
                if (disposing)
                {
                    //处理托管资源
                    if (!ScopeStack.IsEmpty)
                    {
                        IDisposable result;
                        ScopeStack.TryPop(out result);
                    }
                    if (ScopeStack.IsEmpty)
                    {
                        CallContext.FreeNamedDataSlot(_scopeStackKey);
                    }
                }
                //处理非托管资源
            }
            _disposed = true;
        }

    }

    #region ImmutableStack Impls
    
    //public class AmbientScope : IDisposable
    //{
    //    private static readonly string _scopeStackKey = "AmbientScopeStack";
    //    internal static ImmutableStack<IDisposable> ScopeStack
    //    {
    //        get
    //        {
    //            return CallContext.LogicalGetData(_scopeStackKey) as ImmutableStack<IDisposable>;
    //        }
    //        set
    //        {
    //            CallContext.LogicalSetData(_scopeStackKey, value);
    //        }
    //    }

    //    public AmbientScope()
    //    {
    //        if (ScopeStack == null)
    //        {
    //            ScopeStack = ImmutableStack.Create<IDisposable>(this);
    //        }
    //        else
    //        {
    //            ScopeStack = ScopeStack.Push(this);
    //        }
    //    }

    //    public static IDisposable Current
    //    {
    //        get
    //        {
    //            if (ScopeStack == null || ScopeStack.IsEmpty)
    //            {
    //                return null;
    //            }
    //            return ScopeStack.Peek();
    //        }
    //    }

    //    public virtual void Dispose()
    //    {
    //        if (!ScopeStack.IsEmpty)
    //        {
    //            ScopeStack = ScopeStack.Pop();
    //        }
    //        if (ScopeStack.IsEmpty)
    //        {
    //            CallContext.FreeNamedDataSlot(_scopeStackKey);
    //        }
    //    }
    //}

    #endregion
}
