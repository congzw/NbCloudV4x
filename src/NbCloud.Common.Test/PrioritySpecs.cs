using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common
{
    [TestClass]
    public class PrioritySpecs
    {
        [TestMethod]
        public void SortByPriority_Should_Ok()
        {
            var tasks = new List<IDemoPriority>
            {
                new DemoPriority(),
                new Demo1Priority(),
                new Demo2Priority(),
                new Demo3Priority(),
                new Demo4Priority()
            };

            var orderedTasks = tasks.SortByPriority().ToList();
            orderedTasks[0].GetType().Name.ShouldEqual(typeof(Demo1Priority).Name);
            orderedTasks[1].GetType().Name.ShouldEqual(typeof(Demo2Priority).Name);
            orderedTasks[2].GetType().Name.ShouldEqual(typeof(DemoPriority).Name);
            orderedTasks[3].GetType().Name.ShouldEqual(typeof(Demo3Priority).Name);
            orderedTasks[4].GetType().Name.ShouldEqual(typeof(Demo4Priority).Name);
        }
    }

    #region mocks

    public interface IDemoPriority : IPriority
    {

    }
    public class DemoPriority : IDemoPriority
    {
        public int Priority()
        {
            return this.Priority_Default_0();
        }
    }
    public class Demo1Priority : IDemoPriority
    {
        public int Priority()
        {
            return this.Priority_Minus_10000();
        }
    }
    public class Demo2Priority : IDemoPriority
    {
        public int Priority()
        {
            return this.Priority_Minus_10000() + 1;
        }
    }
    public class Demo3Priority : IDemoPriority
    {
        public int Priority()
        {
            return this.Priority_Plus_10000();
        }
    }
    public class Demo4Priority : IDemoPriority
    {
        public int Priority()
        {
            return this.Priority_Plus_10000() + 1;
        }
    }

    #endregion
}
