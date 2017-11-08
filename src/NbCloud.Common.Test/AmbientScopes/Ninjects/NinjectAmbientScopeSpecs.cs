using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;
using Ninject;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    [TestClass]
    public class NinjectAmbientScopeSpecs
    {
        [TestMethod]
        public void InAmbientScope_SameScope_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();

                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var check = kernel.Get<UowInvokeCheck>();

                using (var scope = new NinjectAmbientScope())
                {
                    //singleton should unique
                    var check2 = kernel.Get<UowInvokeCheck>();
                    check2.ShouldSame(check);

                    var uow = kernel.Get<MockUow>();
                    uow.ShouldNotNull();
                    
                    var uow2 = kernel.Get<MockUow>();
                    uow2.ShouldNotNull();
                    uow2.ShouldSame(uow);
                    
                    Task.Run(() =>
                    {
                        //singleton should unique
                        var check3 = kernel.Get<UowInvokeCheck>();
                        check3.ShouldSame(check);

                        var uowTask = kernel.Get<MockUow>();
                        uowTask.ShouldNotNull();
                        uowTask.ShouldSame(uow);
                    }).Wait();
                }
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(1);
            }
        }
        
        [TestMethod]
        public void InAmbientScope_CrossScope_Should_NotSame()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();
                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var check = kernel.Get<UowInvokeCheck>();

                using (var scopeOuter = new NinjectAmbientScope())
                {
                    //singleton should unique
                    var check2 = kernel.Get<UowInvokeCheck>();
                    check2.ShouldSame(check);

                    var uow = kernel.Get<MockUow>();
                    uow.ShouldNotNull();

                    var uow2 = kernel.Get<MockUow>();
                    uow2.ShouldNotNull();
                    uow2.ShouldSame(uow);

                    Task.Run(() =>
                    {
                        //singleton should unique
                        var uowTask = kernel.Get<MockUow>();
                        uowTask.ShouldNotNull();
                        uowTask.ShouldSame(uow);

                        //singleton should unique
                        var check3 = kernel.Get<UowInvokeCheck>();
                        check3.ShouldSame(check);
                    }).Wait();
                }
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(1);


                using (var scope = new NinjectAmbientScope())
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
                }
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(2);
            }
        }

        [TestMethod]
        public void InAmbientScope_NestedScope_Should_NotSame()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<UowInvokeCheck>().ToSelf().InSingletonScope();
                kernel.Bind<MockUow>().ToSelf().InAmbientScope();

                var check = kernel.Get<UowInvokeCheck>();

                using (var scopeOuter = new NinjectAmbientScope())
                {
                    //singleton should unique
                    var check2Outer = kernel.Get<UowInvokeCheck>();
                    check2Outer.ShouldSame(check);

                    var uowOuter = kernel.Get<MockUow>();
                    uowOuter.ShouldNotNull();

                    var uow2Outer = kernel.Get<MockUow>();
                    uow2Outer.ShouldNotNull();
                    uow2Outer.ShouldSame(uowOuter);

                    Task.Run(() =>
                    {
                        //singleton should unique
                        var uowTaskOuter = kernel.Get<MockUow>();
                        uowTaskOuter.ShouldNotNull();
                        uowTaskOuter.ShouldSame(uowOuter);

                        //singleton should unique
                        var check3Outer = kernel.Get<UowInvokeCheck>();
                        check3Outer.ShouldSame(check);
                    }).Wait();
                    
                    using (var scope = new NinjectAmbientScope())
                    {
                        //singleton should unique
                        var check2 = kernel.Get<UowInvokeCheck>();
                        check2.ShouldSame(check);

                        var uow = kernel.Get<MockUow>();
                        uow.ShouldNotNull();

                        var uow2 = kernel.Get<MockUow>();
                        uow2.ShouldNotNull();
                        uow2.ShouldSame(uow);

                        Task.Run(() =>
                        {
                            //singleton should unique
                            var uowTask = kernel.Get<MockUow>();
                            uowTask.ShouldNotNull();
                            uowTask.ShouldSame(uow);

                            //singleton should unique
                            var check3 = kernel.Get<UowInvokeCheck>();
                            check3.ShouldSame(check);
                        }).Wait();
                    }
                    //should disposed here!
                    check.IsInvoked.ShouldTrue();
                    check.InvokedCount.ShouldEqual(1);
                }
                //should disposed here!
                check.IsInvoked.ShouldTrue();
                check.InvokedCount.ShouldEqual(2);
            }
        }
    }

    public class UowInvokeCheck
    {
        public bool IsInvoked { get; set; }
        public int InvokedCount { get; set; }
    }

    public class MockUow : IDisposable
    {
        private readonly UowInvokeCheck _uowInvokeCheck;

        public MockUow(UowInvokeCheck uowInvokeCheck)
        {
            _uowInvokeCheck = uowInvokeCheck;
        }

        public void Dispose()
        {
            _uowInvokeCheck.IsInvoked = true;
            _uowInvokeCheck.InvokedCount++;
        }
    }
}
