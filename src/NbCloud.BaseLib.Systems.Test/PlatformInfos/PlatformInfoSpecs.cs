using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.BaseLib.Systems.PlatformInfos
{
    [TestClass]
    public class PlatformInfoSpecs
    {
        [TestMethod]
        public void SetSystemVersionInner_Null_ShouldThrows()
        {
            var platformInfo = new PlatformInfo();
            AssertHelper.ShouldThrows<ArgumentNullException>(() =>
            {
                platformInfo.SetSystemVersionInner(null);
            });
        }
        [TestMethod]
        public void SetSystemVersionInner_ShouldReplace()
        {
            var platformInfo = new PlatformInfo();
            platformInfo.SetSystemVersionInner(new Version(1,2,3,4));
            platformInfo.SystemVersionInner.ShouldEqual(new Version(1, 2, 3, 4).ToString());
        }
    }
}
