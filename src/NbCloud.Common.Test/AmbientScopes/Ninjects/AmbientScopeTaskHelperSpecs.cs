using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NbCloud.Common.Scopes;
using NbCloud.TestLib;
using Ninject;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    [TestClass]
    public class AmbientScopeTaskHelperSpecs
    {
        [TestMethod]
        public void Run_InSingletonScope_ShouldOk()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<TrancationInvokeCheck>().ToSelf().InSingletonScope();

                kernel.Bind<AmbientScope>().To<NinjectAmbientScope>();
                kernel.Bind<IAmbientScopeTrancation>().To<MockAmbientScopeTrancation>();
                //Func<IAmbientScopeTrancation> trancationResolver, Func<AmbientScope> ambientScopeResolver
                //kernel.Bind<IAmbientScopeTaskHelper>().To<AmbientScopeTaskHelper>().InSingletonScope();
                kernel.Bind<IAmbientScopeTaskHelper>().ToMethod(
                    ctx =>
                    {
                        return new AmbientScopeTaskHelper(
                            () => ctx.Kernel.Get<IAmbientScopeTrancation>());
                    }).InSingletonScope();

                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var ambientScopeTaskHelper = kernel.Get<IAmbientScopeTaskHelper>();

                var txCheck = kernel.Get<TrancationInvokeCheck>();

                ambientScopeTaskHelper.TryRun(() =>
                {
                    //singleton should unique
                    kernel.Get<TrancationInvokeCheck>().ShouldSame(txCheck);
                    Task.Run(() =>
                    {
                        //singleton should unique
                        kernel.Get<TrancationInvokeCheck>().ShouldSame(txCheck);
                    }).Wait();
                });
            }
        }

        [TestMethod]
        public void Run_InAmbientScope_ShouldOk()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<TrancationInvokeCheck>().ToSelf().InSingletonScope();
                kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();

                kernel.Bind<AmbientScope>().To<NinjectAmbientScope>();
                kernel.Bind<IAmbientScopeTrancation>().To<MockAmbientScopeTrancation>();
                //Func<IAmbientScopeTrancation> trancationResolver, Func<AmbientScope> ambientScopeResolver
                //kernel.Bind<IAmbientScopeTaskHelper>().To<AmbientScopeTaskHelper>().InSingletonScope();
                kernel.Bind<IAmbientScopeTaskHelper>().ToMethod(
                    ctx =>
                    {
                        return new AmbientScopeTaskHelper(
                            () => ctx.Kernel.Get<IAmbientScopeTrancation>());
                    }).InSingletonScope();

                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var ambientScopeTaskHelper = kernel.Get<IAmbientScopeTaskHelper>();

                var txCheck = kernel.Get<TrancationInvokeCheck>();
                var check = kernel.Get<UowInvokeCheck>();

                ambientScopeTaskHelper.TryRun(() =>
                {
                    var uow = kernel.Get<MockUow>();
                    uow.ShouldNotNull();

                    var uow2 = kernel.Get<MockUow>();
                    uow2.ShouldNotNull();
                    uow2.ShouldSame(uow);

                    Task.Run(() =>
                    {
                        var uowTask = kernel.Get<MockUow>();
                        uowTask.ShouldNotNull();
                        uowTask.ShouldSame(uow);
                    }).Wait();
                });
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(1);
                txCheck.CommitInvoked.ShouldTrue();
            }
        }
        
        [TestMethod]
        public void Run_InAmbientScope_Nested_ShouldOk()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<TrancationInvokeCheck>().ToSelf().InSingletonScope();
                kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();

                kernel.Bind<AmbientScope>().To<NinjectAmbientScope>();
                kernel.Bind<IAmbientScopeTrancation>().To<MockAmbientScopeTrancation>();
                //Func<IAmbientScopeTrancation> trancationResolver, Func<AmbientScope> ambientScopeResolver
                //kernel.Bind<IAmbientScopeTaskHelper>().To<AmbientScopeTaskHelper>().InSingletonScope();
                kernel.Bind<IAmbientScopeTaskHelper>().ToMethod(
                    ctx =>
                    {
                        return new AmbientScopeTaskHelper(
                            () => ctx.Kernel.Get<IAmbientScopeTrancation>());
                    }).InSingletonScope();

                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var ambientScopeTaskHelper = kernel.Get<IAmbientScopeTaskHelper>();

                var txCheck = kernel.Get<TrancationInvokeCheck>();
                var check = kernel.Get<UowInvokeCheck>();

                ambientScopeTaskHelper.TryRun(() =>
                {
                    var uowOuter = kernel.Get<MockUow>();
                    uowOuter.ShouldNotNull();

                    var uow2Outer = kernel.Get<MockUow>();
                    uow2Outer.ShouldNotNull();
                    uow2Outer.ShouldSame(uowOuter);

                    Task.Run(() =>
                    {
                        var uowTaskOuter = kernel.Get<MockUow>();
                        uowTaskOuter.ShouldNotNull();
                        uowTaskOuter.ShouldSame(uowOuter);
                    }).Wait();


                    ambientScopeTaskHelper.TryRun(() =>
                    {
                        var uow = kernel.Get<MockUow>();
                        uow.ShouldNotNull();

                        var uow2 = kernel.Get<MockUow>();
                        uow2.ShouldNotNull();
                        uow2.ShouldSame(uow);

                        Task.Run(() =>
                        {
                            var uowTask = kernel.Get<MockUow>();
                            uowTask.ShouldNotNull();
                            uowTask.ShouldSame(uow);
                        }).Wait();
                    });
                    //should disposed here!
                    check.IsInvoked.ShouldTrue();
                    check.InvokedCount.ShouldEqual(1);
                    txCheck.CommitInvoked.ShouldTrue();

                });
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(2);
                txCheck.CommitInvoked.ShouldTrue();
            }
        }
    }

    public class TrancationInvokeCheck
    {
        public bool RequireNewInvoked { get; set; }
        public bool CommitInvoked { get; set; }
        public bool CancelInvoked { get; set; }
    }

    public class MockAmbientScopeTrancation : IAmbientScopeTrancation
    {
        private readonly TrancationInvokeCheck _check;

        public MockAmbientScopeTrancation(TrancationInvokeCheck check)
        {
            _check = check;
        }


        public void RequireNew()
        {
            _check.RequireNewInvoked = true;
        }

        public void Commit()
        {
            _check.CommitInvoked = true;
        }

        public void Cancel()
        {
            _check.CancelInvoked = true;
        }
    }
}
