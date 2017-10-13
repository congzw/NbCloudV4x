using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbCloud.Common
{
    [TestClass]
    public class ResolveAsSingletonSpecs
    {
        [TestMethod]
        public void SingleThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
            var resolveDemoTestResult = ResolveDemoTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldSame();
        }

        [TestMethod]
        public void SingleThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            var resolveDemoTestResult = ResolveDemoTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
            resolveDemoTestResult.ShouldNotSame();
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
        }

        int multiThreadTaskCount = 20;
        [TestMethod]
        public void MultiThread_Singleton_ShouldSame()
        {
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
            List<Task> tasks = new List<Task>();
            ConcurrentBag<ResolveDemoTestResult> testResults = new ConcurrentBag<ResolveDemoTestResult>();
            for (int i = 0; i < multiThreadTaskCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var resolveDemoTestResult = ResolveDemoTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
                    testResults.Add(resolveDemoTestResult);
                }));
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var testResult in testResults)
            {
                testResult.ShouldSame();
            }
        }

        [TestMethod]
        public void MultiThread_Transient_ShouldNotSame()
        {
            ResolveAsSingleton<ResolveDemo>.SetFactoryFunc(() => new ResolveDemo());
            List<Task> tasks = new List<Task>();
            ConcurrentBag<ResolveDemoTestResult> testResults = new ConcurrentBag<ResolveDemoTestResult>();
            for (int i = 0; i < multiThreadTaskCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var resolveDemoTestResult = ResolveDemoTestResult.Create(ResolveAsSingleton<ResolveDemo>.Resolve(), ResolveAsSingleton<ResolveDemo>.Resolve());
                    testResults.Add(resolveDemoTestResult);
                }));
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var testResult in testResults)
            {
                testResult.ShouldNotSame();
            }
            ResolveAsSingleton<ResolveDemo>.ResetFactoryFunc();
        }


        #region test helper
        public class ResolveDemo : IResolveAsSingleton
        {
        }

        public class ResolveDemoTestResult
        {
            public static ResolveDemoTestResult Create(params ResolveDemo[] items)
            {
                var resolveDemoTestResult = new ResolveDemoTestResult();
                resolveDemoTestResult.CreateInThreadId = Thread.CurrentThread.ManagedThreadId;
                resolveDemoTestResult.Item1 = items[0];
                resolveDemoTestResult.Item2 = items[1];
                return resolveDemoTestResult;
            }

            public ResolveDemo Item1 { get; set; }
            public ResolveDemo Item2 { get; set; }
            public int CreateInThreadId { get; set; }

            public void ShouldSame()
            {
                var isOkMessage = Item1 == Item2 ? "OK" : "- !!! K.O.";
                var message = string.Format("[{0}](Thread:{1}): {2} == {3}", isOkMessage, CreateInThreadId.ToString("000"), Item1.GetHashCode(), Item2.GetHashCode());
                Console.WriteLine(message);
                Assert.AreSame(Item1, Item2);
            }

            public void ShouldNotSame()
            {
                var isOkMessage = Item1 != Item2 ? "OK" : "- !!! K.O.";
                var message = string.Format("[{0}](Thread:{1}): {2} != {3}", isOkMessage, CreateInThreadId.ToString("000"), Item1.GetHashCode(), Item2.GetHashCode());
                Console.WriteLine(message);
                Assert.AreNotSame(Item1, Item2);
            }
        }
        #endregion
    }
}
