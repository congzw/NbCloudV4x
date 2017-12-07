using System;
using NbCloud.TestLib;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public interface IMockSession
    {

    }
    public class MockSession : IMockSession, IDisposable
    {
        public void Dispose()
        {
            UtilsLogger.LogMessage(">>>>>>>> Mock Session Disposed(): " + this.ObjectInfo());
        }
    }
    public interface IMockTransactionManager
    {

    }
    public interface IMockSql
    {
    }
    public interface IMockRepositoryContext : IMockTransactionManager, IMockSql, IDisposable
    {

    }
    public abstract class MockRepositoryContextBase : IMockRepositoryContext
    {
        public void Dispose()
        {

        }
    }
    public interface IMockNhRepositoryContext : IMockRepositoryContext
    {
    }
    public class MockNhRepositoryContext : MockRepositoryContextBase, IMockNhRepositoryContext
    {
        private readonly IMockSession _session;

        public MockNhRepositoryContext(IMockSession session)
        {
            _session = session;
        }
    }
}
