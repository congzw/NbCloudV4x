using System;
using System.Collections.Generic;

namespace NbCloud.Common.Scopes
{
    public interface INbScope : IDisposable
    {
        bool ShouldReleaseTrancationManager { get; set; }
        INbScopeTrancationManager TrancationManager { get; set; }
    }
    
    public class NbScope : INbScope
    {
        public bool ShouldReleaseTrancationManager { get; set; }
        public INbScopeTrancationManager TrancationManager { get; set; }

        public NbScope(INbScopeTrancationManager nbScopeTrancationManager)
        {
            if (nbScopeTrancationManager == null)
            {
                throw new ArgumentNullException("nbScopeTrancationManager");
            }
            TrancationManager = nbScopeTrancationManager;
            LogMessage("Ctor()");
        }

        public void Dispose()
        {
            LogMessage("Dispose()");
            if (TrancationManager != null)
            {
                if (ShouldReleaseTrancationManager)
                {
                    TrancationManager.Dispose();
                }
                TrancationManager = null;
            }
        }

        private void LogMessage(string message)
        {
            UtilsLogger.LogMessage(string.Format("[NbScope]<{0}> => {1}", this.GetHashCode(), message));
        }
    }
}
