using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NbCloud.TestLib;

namespace NbCloud.Common.Scopes
{
    [TestClass]
    public class NbScopeTaskHelperSpecs
    {
        [TestMethod]
        public void Run_ScopeCount_ShouldOk()
        {
            var resolver = new NbScopeResolver(() => new Mock<INbScope>().Object);
            var nbScopeTaskHelper = new NbScopeTaskHelper(resolver);
            nbScopeTaskHelper.Run(() =>
            {
                var currentScopes = resolver.GetAllCurrentScopes();
                var currentScopes2 = resolver.GetAllCurrentScopes();
                currentScopes.ShouldNotNull();
                currentScopes2.ShouldNotNull();
                currentScopes.Count.ShouldEqual(1);
                currentScopes2.Count.ShouldEqual(1);

                var currentScope = resolver.GetCurrentScope();
                var currentScope2 = resolver.GetCurrentScope();
                currentScope.ShouldNotNull();
                currentScope2.ShouldNotNull();
                currentScope2.ShouldSame(currentScope);
            });

            resolver.GetAllCurrentScopes().Count.ShouldEqual(0);
            resolver.GetCurrentScope().ShouldNull();
        }
        
        [TestMethod]
        public void Run_CreateAndRelease_ShouldOk()
        {
            var mockScope = new Mock<INbScope>();
            var mockResolver = new Mock<INbScopeResolver>();
            mockResolver.Setup(x => x.CreateNewScope()).Returns(mockScope.Object);
   
            mockResolver.Setup(x => x.ReleaseScope(It.IsAny<INbScope>()));
            var nbScopeTaskHelper = new NbScopeTaskHelper(mockResolver.Object);
            nbScopeTaskHelper.Run(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Once);
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Once);
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));
            

            nbScopeTaskHelper.Run(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(2));
        }
        
        [TestMethod]
        public void Nested_Run_ScopeCount_ShouldOk()
        {
            var resolver = new NbScopeResolver(() => new Mock<INbScope>().Object);
            var nbScopeTaskHelper = new NbScopeTaskHelper(resolver);
            nbScopeTaskHelper.Run(() =>
            {
                var currentScopes = resolver.GetAllCurrentScopes();
                var currentScopes2 = resolver.GetAllCurrentScopes();
                currentScopes.ShouldNotNull();
                currentScopes2.ShouldNotNull();
                currentScopes.Count.ShouldEqual(1);
                currentScopes2.Count.ShouldEqual(1);

                var currentScope = resolver.GetCurrentScope();
                var currentScope2 = resolver.GetCurrentScope();
                currentScope.ShouldNotNull();
                currentScope2.ShouldNotNull();
                currentScope2.ShouldSame(currentScope);
                
                nbScopeTaskHelper.Run(() =>
                {
                    var currentScopesInner = resolver.GetAllCurrentScopes();
                    currentScopesInner.ShouldNotNull();
                    currentScopesInner.Count.ShouldEqual(2);

                    var currentScopeInner = resolver.GetCurrentScope();
                    currentScopeInner.ShouldNotNull();
                    currentScopeInner.ShouldNotSame(currentScope);
                });

                var allCurrentScopes = resolver.GetAllCurrentScopes();
                allCurrentScopes.Count.ShouldEqual(1);
                var nbScope = resolver.GetCurrentScope();
                nbScope.ShouldSame(currentScope);
                nbScope.ShouldSame(currentScope2);
            });

            resolver.GetAllCurrentScopes().Count.ShouldEqual(0);
            resolver.GetCurrentScope().ShouldNull();
        }

        [TestMethod]
        public void Nested_Run_CreateAndRelease_ShouldOk()
        {
            var mockScope = new Mock<INbScope>();
            var mockResolver = new Mock<INbScopeResolver>();
            mockResolver.Setup(x => x.CreateNewScope()).Returns(mockScope.Object);

            mockResolver.Setup(x => x.ReleaseScope(It.IsAny<INbScope>()));
            var nbScopeTaskHelper = new NbScopeTaskHelper(mockResolver.Object);
            nbScopeTaskHelper.Run(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(1));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));
                
                nbScopeTaskHelper.Run(() =>
                {
                    mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                    mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));
                });

                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task RunAsync_ScopeCount_ShouldOk()
        {
            var resolver = new NbScopeResolver(() => new Mock<INbScope>().Object);
            var nbScopeTaskHelper = new NbScopeTaskHelper(resolver);
            await nbScopeTaskHelper.RunAsync(() =>
            {
                var currentScopes = resolver.GetAllCurrentScopes();
                var currentScopes2 = resolver.GetAllCurrentScopes();
                currentScopes.ShouldNotNull();
                currentScopes2.ShouldNotNull();
                currentScopes.Count.ShouldEqual(1);
                currentScopes2.Count.ShouldEqual(1);

                var currentScope = resolver.GetCurrentScope();
                var currentScope2 = resolver.GetCurrentScope();
                currentScope.ShouldNotNull();
                currentScope2.ShouldNotNull();
                currentScope2.ShouldSame(currentScope);
            });

            resolver.GetAllCurrentScopes().Count.ShouldEqual(0);
            resolver.GetCurrentScope().ShouldNull();
        }

        [TestMethod]
        public async Task RunAsync_CreateAndRelease_ShouldOk()
        {
            var mockScope = new Mock<INbScope>();
            var mockResolver = new Mock<INbScopeResolver>();
            mockResolver.Setup(x => x.CreateNewScope()).Returns(mockScope.Object);

            mockResolver.Setup(x => x.ReleaseScope(It.IsAny<INbScope>()));
            var nbScopeTaskHelper = new NbScopeTaskHelper(mockResolver.Object);
            await nbScopeTaskHelper.RunAsync(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Once);
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(1));
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));

            await nbScopeTaskHelper.RunAsync(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Nested_RunAsync_ScopeCount_ShouldOk()
        {
            var resolver = new NbScopeResolver(() => new Mock<INbScope>().Object);
            var nbScopeTaskHelper = new NbScopeTaskHelper(resolver);
            await nbScopeTaskHelper.RunAsync(() =>
            {
                var currentScopes = resolver.GetAllCurrentScopes();
                var currentScopes2 = resolver.GetAllCurrentScopes();
                currentScopes.ShouldNotNull();
                currentScopes2.ShouldNotNull();
                currentScopes.Count.ShouldEqual(1);
                currentScopes2.Count.ShouldEqual(1);

                var currentScope = resolver.GetCurrentScope();
                var currentScope2 = resolver.GetCurrentScope();
                currentScope.ShouldNotNull();
                currentScope2.ShouldNotNull();
                currentScope2.ShouldSame(currentScope);

                nbScopeTaskHelper.Run(() =>
                {
                    var currentScopesInner = resolver.GetAllCurrentScopes();
                    currentScopesInner.ShouldNotNull();
                    //如果嵌套异步任务，则不能保证执行顺序为如下断言
                    currentScopesInner.Count.ShouldEqual(2);

                    var currentScopeInner = resolver.GetCurrentScope();
                    currentScopeInner.ShouldNotNull();
                    currentScopeInner.ShouldNotSame(currentScope);
                });

                var allCurrentScopes = resolver.GetAllCurrentScopes();
                allCurrentScopes.Count.ShouldEqual(1);
                var nbScope = resolver.GetCurrentScope();
                nbScope.ShouldSame(currentScope);
                nbScope.ShouldSame(currentScope2);
            });

            resolver.GetAllCurrentScopes().Count.ShouldEqual(0);
            resolver.GetCurrentScope().ShouldNull();
        }

        [TestMethod]
        public async Task Nested_RunAsync_CreateAndRelease_ShouldOk()
        {
            var mockScope = new Mock<INbScope>();
            var mockResolver = new Mock<INbScopeResolver>();
            mockResolver.Setup(x => x.CreateNewScope()).Returns(mockScope.Object);

            mockResolver.Setup(x => x.ReleaseScope(It.IsAny<INbScope>()));
            var nbScopeTaskHelper = new NbScopeTaskHelper(mockResolver.Object);
            await nbScopeTaskHelper.RunAsync(() =>
            {
                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(1));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));

                var runAsync = nbScopeTaskHelper.RunAsync(() =>
                {
                    mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                    mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(0));
                });
                runAsync.Wait();

                mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
                mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(1));
            });

            mockResolver.Verify(x => x.CreateNewScope(), Times.Exactly(2));
            mockResolver.Verify(x => x.ReleaseScope(It.IsAny<INbScope>()), Times.Exactly(2));
        }
    }
}
