using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class MyProjectHelperSpecs
    {
        [TestMethod]
        public void GetProjectPrefix_Should_Ok()
        {
            var helper = new MyProjectHelper();
            helper.GetProjectPrefix().ShouldEqual("NbCloud");
        }

        [TestMethod]
        public void GetBinDirectory_Should_NotNull()
        {
            var helper = new MyProjectHelper();
            helper.GetBinDirectory().Log().ShouldNotNull();
        }

        [TestMethod]
        public void LoadAppAssemblies_Should_NotNull()
        {
            var helper = new MyProjectHelper();
            helper.LoadAppAssemblies().Log().ShouldNotNull();
        }

        [TestMethod]
        public void LoadAppAssemblies_Should_Not_Include_Excludes()
        {
            var excludeFileName = "NbCloud.Common.Test.dll";
            var helper = new MyProjectHelper();
            var loadAppAssemblies = helper.LoadAppAssemblies(null, null, new[] {excludeFileName});
            loadAppAssemblies.Log();
            foreach (var loadAppAssembly in loadAppAssemblies)
            {
                loadAppAssembly.GetName().Name.ShouldNotEqual(excludeFileName);
            }
        }
    }
}
