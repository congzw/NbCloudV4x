namespace NbCloud.Common.Scopes
{
    public class MockNbScope : INbScope
    {
        public void Dispose()
        {
            DisposeInvoked = true;
        }

        public void RequireNew()
        {
            RequireNewInvoked = true;
        }

        public void Commit()
        {
            CommitInvoked = true;
        }

        public void Cancel()
        {
            CancelInvoked = true;
        }

        public bool DisposeInvoked { get; set; }
        public bool RequireNewInvoked { get; set; }
        public bool CommitInvoked { get; set; }
        public bool CancelInvoked { get; set; }

        public bool ShouldReleaseTrancationManager { get; set; }
        public INbScopeTrancationManager TrancationManager { get; set; }
    }
    public interface IMockSession
    {
    }
    public class MockSession : IMockSession
    {
    }
    public interface IMockRepository
    {
    }
    public class MockRepository : IMockRepository
    {
    }
    public interface IMockRepositoryContext
    {
    }
    public class MockRepositoryContext : IMockRepositoryContext
    {
    }
    public class HelloWorldService
    {
        public IMockRepositoryContext Context { get; private set; }
        private readonly IMockRepository _repository;

        public HelloWorldService(IMockRepositoryContext context, IMockRepository repository)
        {
            Context = context;
            _repository = repository;
        }
    }
}
