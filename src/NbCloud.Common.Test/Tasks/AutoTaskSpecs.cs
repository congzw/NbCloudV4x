using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.TestLib;

namespace NbCloud.Common.Tasks
{
    [TestClass]
    public class AutoTaskSpecs
    {
        [TestMethod]
        public void SortByPriority_Should_Ok()
        {
            var tasks = new List<IAutoTask>
            {
                new Demo4Task(),
                new Demo2Task(),
                new Demo1Task(),
                new Demo3Task(),
                new Demo5Task()
            };

            var orderedTasks = tasks.SortByPriority().ToList();
            orderedTasks[0].GetType().Name.ShouldEqual(typeof(Demo1Task).Name);
            orderedTasks[1].GetType().Name.ShouldEqual(typeof(Demo2Task).Name);
            orderedTasks[2].GetType().Name.ShouldEqual(typeof(Demo3Task).Name);
            orderedTasks[3].GetType().Name.ShouldEqual(typeof(Demo4Task).Name);
            orderedTasks[4].GetType().Name.ShouldEqual(typeof(Demo5Task).Name);
        }

        [TestMethod]
        public void SortByPriorityAndCast_Should_Ok()
        {
            var tasks = new List<IAutoTask>
            {
                new Demo4Task(),
                new Demo2Task(),
                new Demo1Task(),
                new Demo3Task(),
                new Demo5Task()
            };

            var beginTasks = tasks.Where(x => x is IRequestBeginTask).SortByPriority().Cast<IRequestBeginTask>().ToList();
            beginTasks.Count.ShouldEqual(3);
            beginTasks[0].GetType().Name.ShouldEqual(typeof(Demo1Task).Name);
            beginTasks[1].GetType().Name.ShouldEqual(typeof(Demo2Task).Name);
            beginTasks[2].GetType().Name.ShouldEqual(typeof(Demo5Task).Name);

            var endTasks = tasks.Where(x => x is IRequestEndTask).SortByPriority().Cast<IRequestEndTask>().ToList();
            endTasks.Count.ShouldEqual(3);
            endTasks[0].GetType().Name.ShouldEqual(typeof(Demo3Task).Name);
            endTasks[1].GetType().Name.ShouldEqual(typeof(Demo4Task).Name);
            endTasks[2].GetType().Name.ShouldEqual(typeof(Demo5Task).Name);
        }
    }

    #region mocks

    public class Demo1Task : IRequestBeginTask
    {
        public int Priority()
        {
            return this.Priority_Minus_10000();
        }

        public void Execute()
        {
            Debug.WriteLine(this.GetType().Name);
        }
    }

    public class Demo2Task : IRequestBeginTask
    {
        public int Priority()
        {
            return this.Priority_Minus_10000() +1;
        }

        public void Execute()
        {
            Debug.WriteLine(this.GetType().Name);
        }
    }

    public class Demo3Task : IRequestEndTask
    {
        public int Priority()
        {
            return this.Priority_Default_0();
        }

        public void Execute()
        {
            Debug.WriteLine(this.GetType().Name);
        }
    }

    public class Demo4Task : IRequestEndTask
    {
        public int Priority()
        {
            return this.Priority_Plus_10000() - 1;
        }

        public void Execute()
        {
            Debug.WriteLine(this.GetType().Name);
        }
    }
    
    public class Demo5Task : IRequestBeginTask, IRequestEndTask
    {
        public int Priority()
        {
            return this.Priority_Plus_10000();
        }
        
        void IRequestBeginTask.Execute()
        {
            Debug.WriteLine("IRequestBeginTask => " + this.GetType().Name);
        }

        void IRequestEndTask.Execute()
        {
            Debug.WriteLine("IRequestEndTask => " + this.GetType().Name);
        }
    }

    #endregion
}
