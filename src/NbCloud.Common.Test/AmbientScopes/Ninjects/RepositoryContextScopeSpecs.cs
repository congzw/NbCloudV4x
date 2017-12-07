using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;
using Ninject;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    [TestClass]
    public class RepositoryContextScopeSpecs
    {
        [TestMethod]
        public void InAmbientScope_Session_Should_OK()
        {
            using (var kernel = new StandardKernel())
            {
                InitKernel(kernel);

                using (var scope = new NinjectAmbientScope())
                {
                    var session = kernel.Get<IMockSession>();
                    session.ShouldNotNull();

                    var session2 = kernel.Get<IMockSession>();
                    session2.ShouldNotNull();
                    session2.ShouldSame(session);

                    Task.Run(() =>
                    {
                        var sessionTask = kernel.Get<IMockSession>();
                        sessionTask.ShouldNotNull();
                        sessionTask.ShouldSame(session);
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void InAmbientScope_NhRepositoryContext_Should_OK()
        {
            using (var kernel = new StandardKernel())
            {
                InitKernel(kernel);

                using (var scope = new NinjectAmbientScope())
                {
                    var nhRepos = kernel.Get<IMockNhRepositoryContext>();
                    nhRepos.ShouldNotNull();

                    var nhRepos2 = kernel.Get<IMockNhRepositoryContext>();
                    nhRepos2.ShouldNotNull();
                    nhRepos2.ShouldSame(nhRepos);

                    Task.Run(() =>
                    {
                        var nhReposTask = kernel.Get<IMockNhRepositoryContext>();
                        nhReposTask.ShouldNotNull();
                        nhReposTask.ShouldSame(nhRepos);
                    }).Wait();
                }
            }
        }

        [TestMethod]
        public void InAmbientScope_RepositoryContext_Should_OK()
        {
            using (var kernel = new StandardKernel())
            {
                InitKernel(kernel);

                using (var scope = new NinjectAmbientScope())
                {
                    var repos = kernel.Get<IMockRepositoryContext>();
                    repos.ShouldNotNull();

                    var repos2 = kernel.Get<IMockRepositoryContext>();
                    repos2.ShouldNotNull();
                    repos2.ShouldSame(repos);

                    Task.Run(() =>
                    {
                        var reposTask = kernel.Get<IMockRepositoryContext>();
                        reposTask.ShouldNotNull();
                        reposTask.ShouldSame(repos);
                    }).Wait();
                }
            }
        }
        
        [TestMethod]
        public void InAmbientScope_AllContext_Should_OK()
        {
            using (var kernel = new StandardKernel())
            {
                InitKernel(kernel);

                using (var scope = new NinjectAmbientScope())
                {
                    var nhRepos = kernel.Get<IMockNhRepositoryContext>();
                    nhRepos.ShouldNotNull();

                    var repos = kernel.Get<IMockRepositoryContext>();
                    repos.ShouldNotNull();
                    repos.ShouldSame(nhRepos);

                    var txManager = kernel.Get<IMockTransactionManager>();
                    txManager.ShouldNotNull();
                    txManager.ShouldSame(nhRepos);

                    var sql = kernel.Get<IMockSql>();
                    sql.ShouldNotNull();
                    sql.ShouldSame(nhRepos);
                    
                    Task.Run(() =>
                    {
                        var nhReposTask = kernel.Get<IMockNhRepositoryContext>();
                        nhReposTask.ShouldNotNull();
                        
                        var reposTask = kernel.Get<IMockRepositoryContext>();
                        reposTask.ShouldNotNull();
                        reposTask.ShouldSame(nhReposTask);
                        
                        var txManagerTask = kernel.Get<IMockTransactionManager>();
                        txManagerTask.ShouldNotNull();
                        txManagerTask.ShouldSame(nhRepos);

                        var sqlTask = kernel.Get<IMockSql>();
                        sqlTask.ShouldNotNull();
                        sqlTask.ShouldSame(nhRepos);
                        
                        //outer same
                        nhReposTask.ShouldSame(nhRepos);
                        reposTask.ShouldSame(repos);

                    }).Wait();
                }
            }
        }


        private void InitKernel(IKernel kernel)
        {
            kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();

            kernel.Bind<IMockSession>().ToMethod(x => GetSession()).InAmbientOrRequestScope().OnDeactivation(onDeactivation);
            kernel.Bind<IMockNhRepositoryContext>().To<MockNhRepositoryContext>().InScope(ctx => ctx.Kernel.Get<IMockSession>());

            kernel.Bind<IMockRepositoryContext>().ToMethod(ctx => ctx.Kernel.Get<IMockNhRepositoryContext>());
            kernel.Bind<IMockTransactionManager>().ToMethod(ctx => ctx.Kernel.Get<IMockNhRepositoryContext>());
            kernel.Bind<IMockSql>().ToMethod(ctx => ctx.Kernel.Get<IMockNhRepositoryContext>());
        }
        private IMockSession GetSession()
        {
            var openSession = new MockSession();
            UtilsLogger.LogMessage(">>>>>>>> open new session => " + openSession.ObjectInfo());
            return openSession;
        }
        private void onDeactivation(IMockSession session)
        {
            UtilsLogger.LogMessage(">>>>>>>> dispose session => " + session.ObjectInfo());
        }
    }

}
