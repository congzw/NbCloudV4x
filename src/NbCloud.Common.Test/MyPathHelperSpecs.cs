using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class MyPathHelperSpecs
    {
        [TestMethod]
        public void GetBinDirectory_Should_OK()
        {
            var helper = new MyPathHelper();
            var binDirectory = helper.GetBinDirectory();
            binDirectory.ShouldNotNull();
            binDirectory.ShouldEqual(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        public void JoinPath_Should_OK()
        {
            var helper = new MyPathHelper();
            helper.JoinPath("A", "B").ShouldEqual("A\\B");
        }
        
    }
}
