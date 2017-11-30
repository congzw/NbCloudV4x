using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.PlugIns
{
    [TestClass]
    public class PlugInManagerSpecs
    {
        [TestMethod]
        public void IsRegistered_Should_Diff_Name()
        {
            var plugInManager = new PlugInManager();
            var module1 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module1);

            plugInManager.IsRegistered(module1).ShouldTrue();

            var module2 = new PlugIn("ModuleB", "某模块", "1.0.0.0");
            plugInManager.IsRegistered(module2).ShouldFalse();

            var module3 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.IsRegistered(module3).ShouldTrue();
        }

        [TestMethod]
        public void IsRegistered_Should_Diff_Version()
        {
            var plugInManager = new PlugInManager();
            var module1 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module1);

            plugInManager.IsRegistered(module1).ShouldTrue();

            var module2 = new PlugIn("ModuleA", "某模块", "1.1.0.0");
            plugInManager.IsRegistered(module2).ShouldFalse();

            var module3 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.IsRegistered(module3).ShouldTrue();
        }

        [TestMethod]
        public void Register_Should_Diff_Name()
        {
            var plugInManager = new PlugInManager();
            var module1 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module1);
            
            var module2 = new PlugIn("ModuleB", "某模块", "1.0.0.0");
            plugInManager.Register(module2);
            plugInManager.PlugInList.Count.ShouldEqual(2);

            var module3 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module3);
            plugInManager.PlugInList.Count.ShouldEqual(2);
        }

        [TestMethod]
        public void Register_Should_Diff_Version()
        {
            var plugInManager = new PlugInManager();
            var module1 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module1);

            var module2 = new PlugIn("ModuleA", "某模块", "1.1.0.0");
            plugInManager.Register(module2);
            plugInManager.PlugInList.Count.ShouldEqual(2);
            
            var module3 = new PlugIn("ModuleA", "某模块", "1.0.0.0");
            plugInManager.Register(module3);

            plugInManager.PlugInList.Count.ShouldEqual(2);
        }
    }
}
