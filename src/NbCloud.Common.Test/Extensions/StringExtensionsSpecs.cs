﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Extensions
{
    [TestClass]
    public class StringExtensionsSpecs
    {
        [TestMethod]
        public void JoinToString_Null_Should_Ok()
        {
            IList<string> test = null;
            test.JoinToString().ShouldEqual("");
            IList<int> test2 = null;
            test2.JoinToString().ShouldEqual("");
        }

        [TestMethod]
        public void JoinToString_Empty_Should_Ok()
        {
            new string[] { }.JoinToString().ShouldEqual("");
            new int[] { }.JoinToString().ShouldEqual("");
        }

        [TestMethod]
        public void JoinToString_Should_Ok()
        {
            new string[] { "1", "2", "3" }.JoinToString().ShouldEqual("1,2,3");
            new int[] { 1, 2, 3 }.JoinToString().ShouldEqual("1,2,3");
        }

        [TestMethod]
        public void JoinToString_With_Separator_Should_Ok()
        {
            new string[] { "1", "2", "3" }.JoinToString("-").ShouldEqual("1-2-3");
            new int[] { 1, 2, 3 }.JoinToString("-").ShouldEqual("1-2-3");
        }
    }
}
