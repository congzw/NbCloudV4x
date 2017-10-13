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
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void SingleThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            var resolveDemoTestResult = ObjectIntanceTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldNotSame();
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
        }

        int multiThreadTaskCount = 20;
        [TestMethod]
        public void MultiThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton<ResolveDemo>.Resolve
                , ResolveAsSingleton<ResolveDemo>.Resolve
                , true);
        }

        [TestMethod]
        public void MultiThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            ObjectIntanceTestResult.RunTestInMultiTasks(multiThreadTaskCount
                , ResolveAsSingleton<ResolveDemo>.Resolve
                , ResolveAsSingleton<ResolveDemo>.Resolve
                , false);
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
        }
        
        #region test helper
        public class ResolveDemo : IResolveAsSingleton
        {
        }
        #endregion
    }
}
