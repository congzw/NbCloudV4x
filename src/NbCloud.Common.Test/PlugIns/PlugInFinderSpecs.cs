using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.PlugIns
{
    [TestClass]
    public class PlugInFinderSpecs
    {
        [TestMethod]
        public void FindAllPlugIns_Should_OK()
        {
            var plugInFinder = new PlugInFinder();
            plugInFinder.PlugInBaseFolderResolver = () => AppDomain.CurrentDomain.BaseDirectory + "\\PlugIns\\Mocks";

            var plugIns = plugInFinder.FindAllPlugIns();

            plugIns.ShouldNotNull();
            plugIns.Count.ShouldEqual(2);
            plugIns[0].Code.ShouldEqual("ModuleA");
            plugIns[1].Code.ShouldEqual("ModuleB");
        }
    }
}
