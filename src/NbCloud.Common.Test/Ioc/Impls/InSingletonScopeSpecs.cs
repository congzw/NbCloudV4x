using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;
using Ninject;

namespace NbCloud.Common.Ioc.Impls
{
    [TestClass]
    public class InSingletonScopeSpecs
    {
        [TestMethod]
        public void SameThread_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();
                var instance1 = kernel.Get<object>();
                var instance2 = kernel.Get<object>();
                Assert.AreSame(instance1, instance2);
                Debug.WriteLine("{0}:{1}", instance1.GetHashCode(), instance2.GetHashCode());
            }
        }
        
        [TestMethod]
        public void ThreadStart_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();

                var instance1 = kernel.Get<object>();
                object instance2 = null;
                var thread = new Thread(() =>
                {
                    instance2 = kernel.Get<object>();
                });
                thread.Start();
                thread.Join();

                instance1.ShouldSame(instance2);
            }
        }

        [TestMethod]
        public void TaskRun_Should_Same()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Bind<object>().ToSelf().InSingletonScope();

                for (int i = 0; i < 100; i++)
                {
                    var instance1 = kernel.Get<object>();
                    object instance2 = null;
                    Task.Run(() =>
                    {
                        instance2 = kernel.Get<object>();
                    }).Wait();

                    instance1.ShouldSame(instance2);
                }
            }
        }
    }
}
