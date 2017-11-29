using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class MyIniFileHelperSpecs
    {
        const string IniContent = @"
# comments
a=123.
b=1234 

; last modified 1 April 2001 by John Doe
[owner]
name=John Doe
organization=Acme Widgets Inc.


[database]
; use IP address in case network name resolution is not working
Server=192.0.2.62
port=143
file=payroll.dat";

        [TestMethod]
        public void Desc_All()
        {

            var myIniFileHelper = new MyIniFileHelper();
            var loadIniContentAsFlat = myIniFileHelper.LoadIniContentAsFlat(IniContent);
            foreach (var item in loadIniContentAsFlat.Items)
            {
                foreach (var item2 in item.Value)
                {
                    Debug.WriteLine("{0} = {1}", item2.Key, item2.Value);
                }
            }
        }

        [TestMethod]
        public void GetItemValue_Key_Should_Has_Value()
        {
            var myIniFileHelper = new MyIniFileHelper();
            var loadIniContentAsFlat = myIniFileHelper.LoadIniContentAsFlat(IniContent);

            AssertHelper.ShouldThrows<ArgumentException>(() =>
            {
                loadIniContentAsFlat.GetItemValue("");
            });
            AssertHelper.ShouldThrows<ArgumentException>(() =>
            {
                loadIniContentAsFlat.GetItemValue(null);
            });
        }

        [TestMethod]
        public void GetItemValue_Should_OK()
        {

            var myIniFileHelper = new MyIniFileHelper();
            var loadIniContentAsFlat = myIniFileHelper.LoadIniContentAsFlat(IniContent);

            loadIniContentAsFlat.GetItemValue("a").ShouldEqual("123.");
            loadIniContentAsFlat.GetItemValue("b").ShouldEqual("1234");
            loadIniContentAsFlat.GetItemValue("xxx", "XXX").ShouldEqual("XXX");
            loadIniContentAsFlat.GetItemValue("name", "XXX").ShouldEqual("John Doe");
            loadIniContentAsFlat.GetItemValue("Name", "XXX").ShouldEqual("John Doe");
        }


        [TestMethod]
        public void GetSectionItemValue_Key_Should_Has_Value()
        {
            var myIniFileHelper = new MyIniFileHelper();
            var loadIniContentAsFlat = myIniFileHelper.LoadIniContentAsFlat(IniContent);

            AssertHelper.ShouldThrows<ArgumentException>(() =>
            {
                loadIniContentAsFlat.GetItemValue("","");
            });
            AssertHelper.ShouldThrows<ArgumentException>(() =>
            {
                loadIniContentAsFlat.GetItemValue("",null);
            });
        }
        [TestMethod]
        public void GetSectionItemValue_Should_OK()
        {

            var myIniFileHelper = new MyIniFileHelper();
            var loadIniContentAsFlat = myIniFileHelper.LoadIniContentAsFlat(IniContent);

            loadIniContentAsFlat.GetSectionItemValue("","a").ShouldEqual("123.");
            loadIniContentAsFlat.GetSectionItemValue("","b").ShouldEqual("1234");
            loadIniContentAsFlat.GetSectionItemValue("owner", "xxx", "XXX").ShouldEqual("XXX");
            loadIniContentAsFlat.GetSectionItemValue("owner", "Name", "XXX").ShouldEqual("John Doe");
            loadIniContentAsFlat.GetSectionItemValue("Owner", "name", "XXX").ShouldEqual("John Doe");
        }
    }
}
