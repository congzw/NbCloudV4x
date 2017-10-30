using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class ResolveAsSingletonSpecs
    {
        [TestMethod]
        public void SingleThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.ResetFactoryFunc();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void SingleThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldNotSame();
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.ResetFactoryFunc();
        }

        int multiThreadTaskCount = 20;
        [TestMethod]
        public void MultiThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.ResetFactoryFunc();
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve
                , ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve
                , true);
        }

        [TestMethod]
        public void MultiThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve
                , ResolveAsSingleton<ResolveDemo, IResolveDemo>.Resolve
                , false);
            ResolveAsSingleton<ResolveDemo, IResolveDemo>.ResetFactoryFunc();
        }
        
        #region test helper

        public interface IResolveDemo
        {
             
        }
        public class ResolveDemo : IResolveAsSingleton, IResolveDemo
        {
        }
        #endregion
    }
}
