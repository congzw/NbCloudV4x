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

        public virtual void Dispose()
        {
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
    }

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
}
