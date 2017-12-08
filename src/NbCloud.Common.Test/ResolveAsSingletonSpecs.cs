using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common.Ioc;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class ResolveAsSingletonSpecs
    {
        private static readonly object Lock = new object();

        [TestInitialize]
        public void MyTestInitialize()
        {
            Monitor.Enter(Lock);
            (Thread.CurrentThread.ManagedThreadId + " > Enter").Log();
            
            #region desc
            
            //C# 4 translate lock(){}
            //bool lockWasTaken = false;
            //var temp = obj;
            //try
            //{
            //    Monitor.Enter(temp, ref lockWasTaken);
            //    // body
            //}
            //finally
            //{
            //    if (lockWasTaken)
            //    {
            //        Monitor.Exit(temp);
            //    }
            //}

            #endregion
        }
        [TestCleanup]
        public void MyTestCleanup()
        {
            (Thread.CurrentThread.ManagedThreadId + " > Exit").Log();
            Monitor.Exit(Lock);
        }


        [TestMethod]
        public void SingleThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>(), ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>());
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void SingleThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton.SetFactoryFunc<ResolveDemo, IResolveDemo>(() => new ResolveDemo());
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>(), ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>());
            resolveDemoTestResult.ShouldNotSame();
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
        }

        int multiThreadTaskCount = 200;
        [TestMethod]
        public void MultiThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , true);
        }

        [TestMethod]
        public void MultiThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton.SetFactoryFunc<ResolveDemo, IResolveDemo>(() => new ResolveDemo());
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>
                , false);
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
        }
        
        [TestMethod]
        public void Use_Di_And_Register_Should_Return_Di_First()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
            ResolveAsSingleton.SetResolve(null);

            var mockServiceLocator = new MockServiceLocator();
            ResolveAsSingleton.SetResolve((type) => mockServiceLocator.GetService(type));
            var resolveDemo = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            var resolveDemo2 = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            resolveDemo.Desc.ShouldEqual("FromMockServiceLocator");
            resolveDemo2.Desc.ShouldEqual("FromMockServiceLocator");
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldNotSame();
        }
        
        [TestMethod]
        public void Use_Di_Not_Register_Should_Return_Di_First()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveUnknownDemo, IResolveUnknownDemo>();
            ResolveAsSingleton.SetResolve(null);

            var mockServiceLocator = new MockServiceLocator();
            ResolveAsSingleton.SetResolve((type) => mockServiceLocator.GetService(type));
            var resolveDemo = ResolveAsSingleton.Resolve<ResolveUnknownDemo, IResolveUnknownDemo>();
            var resolveDemo2 = ResolveAsSingleton.Resolve<ResolveUnknownDemo, IResolveUnknownDemo>();
            resolveDemo.Desc.ShouldNull();
            resolveDemo.Desc.ShouldNull();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldSame();
        }
        
        [TestMethod]
        public void Not_Use_Di_Should_Return_Default_First()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveDemo, IResolveDemo>();
            ResolveAsSingleton.SetResolve(null);

            var resolveDemo = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            var resolveDemo2 = ResolveAsSingleton.Resolve<ResolveDemo, IResolveDemo>();
            resolveDemo.Desc.ShouldNull();
            resolveDemo2.Desc.ShouldNull();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void Not_Use_Di_Should_Return_Default_First2()
        {
            ResolveAsSingleton.ResetFactoryFunc<ResolveUnknownDemo, IResolveUnknownDemo>();
            ResolveAsSingleton.SetResolve(null);

            var resolveDemo = ResolveAsSingleton.Resolve<ResolveUnknownDemo, IResolveUnknownDemo>();
            var resolveDemo2 = ResolveAsSingleton.Resolve<ResolveUnknownDemo, IResolveUnknownDemo>();
            resolveDemo.Desc.ShouldNull();
            resolveDemo.Desc.ShouldNull();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(resolveDemo, resolveDemo2);
            resolveDemoTestResult.ShouldSame();
        }
        
        #region test helper

        public interface IResolveUnknownDemo
        {
            string Desc { get; set; }
        }
        public class ResolveUnknownDemo : IResolveAsSingleton, IResolveUnknownDemo
        {
            public string Desc { get; set; }
        }

        public interface IResolveDemo
        {
            string Desc { get; set; }
        }
        public class ResolveDemo : IResolveAsSingleton, IResolveDemo
        {
            public string Desc { get; set; }
        }
        public class MockServiceLocator :  ServiceLocatorImplBase
        {
            protected override object DoGetInstance(Type serviceType, string key)
            {
                if (serviceType != typeof(IResolveDemo))
                {
                    return null;
                }
                return new ResolveDemo() { Desc = "FromMockServiceLocator" };
            }

            protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
            {
                if (serviceType != typeof(IResolveDemo))
                {
                    yield return null;
                }
                yield return new ResolveDemo() { Desc = "FromMockServiceLocator" };
            }

            public override bool IsRegistered(Type type)
            {
                if (type != typeof(IResolveDemo))
                {
                    return false;
                }
                return true;
            }

            public override bool IsRegistered<T>()
            {
                if (typeof(T) != typeof(IResolveDemo))
                {
                    return false;
                }
                return true;
            }
        }
        #endregion
    }
}
