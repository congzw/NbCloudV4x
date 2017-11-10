using NbCloud.Common.Data;

namespace NbCloud.Common.AmbientScopes.Plugins
{
    public class AmbientScopeTrancation : IAmbientScopeTrancation
    {
        private readonly ITransactionManager _transactionManager;

        public AmbientScopeTrancation(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public void Commit()
        {
            _transactionManager.Commit();
        }

        public void Cancel()
        {
            _transactionManager.Cancel();
        }
    }

    #region with no trancation

    //public class EmptyAmbientScopeTrancation : IAmbientScopeTrancation
    //{
    //    public void Commit()
    //    {
    //    }

    //    public void Cancel()
    //    {
    //    }
    //}

    #endregion
}
