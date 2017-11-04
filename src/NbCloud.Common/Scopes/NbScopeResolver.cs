using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NbCloud.Common.Scopes
{
    public interface INbScopeResolver
    {
        IList<INbScope> GetAllCurrentScopes();
        INbScope GetCurrentScope();
        INbScope CreateNewScope();
        void ReleaseScope(INbScope scope);
    }

    public class NbScopeResolver : INbScopeResolver
    {
        private readonly Func<INbScope> _createScopeFunc;

        public NbScopeResolver(Func<INbScope> createScopeFunc)
        {
            _createScopeFunc = createScopeFunc;
        }

        public IList<INbScope> GetAllCurrentScopes()
        {
            var wrappers = GetCurrentThreadScopeWrappers();
            return wrappers.Select(x => x.NbScope).ToList();
        }

        public INbScope GetCurrentScope()
        {
            var allCurrentScopes = GetAllCurrentScopes();
            return allCurrentScopes.LastOrDefault();;
        }

        public INbScope CreateNewScope()
        {
            var nbScope = _createScopeFunc();
            var wrappers = GetCurrentThreadScopeWrappers();
            wrappers.Add(new NbScopeWrapper(nbScope));
            return nbScope;
        }

        public void ReleaseScope(INbScope scope)
        {
            var wrappers = GetCurrentThreadScopeWrappers();
            var theOne = wrappers.SingleOrDefault(x => x.NbScope == scope);
            if (theOne != null)
            {
                wrappers.Remove(theOne);
                theOne.Dispose();
            }
        }

        #region scopes

        private readonly ConcurrentDictionary<int, IList<NbScopeWrapper>> _scopeWrapperDic = new ConcurrentDictionary<int, IList<NbScopeWrapper>>();
        private IList<NbScopeWrapper> GetCurrentThreadScopeWrappers()
        {
            var managedThreadId = Thread.CurrentThread.ManagedThreadId;
            if (!_scopeWrapperDic.ContainsKey(managedThreadId))
            {
                _scopeWrapperDic[managedThreadId] = new List<NbScopeWrapper>();
            }
            return _scopeWrapperDic[managedThreadId];
        }

        #endregion
        
        internal class NbScopeWrapper : IDisposable
        {
            public NbScopeWrapper(INbScope nbScope)
            {
                NbScope = nbScope;
            }
            public INbScope NbScope { get; set; }
            public void Dispose()
            {
                if (NbScope != null)
                {
                    NbScope.Dispose();
                    NbScope = null;
                }
            }
        }
    }
}