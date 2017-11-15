using System.Linq;
using NbCloud.Common.AmbientScopes;
using NbCloud.Common.Ioc;
using NbCloud.Common.Tasks;

namespace NbCloud.Web
{
    public class TaskConfig
    {
        public static void RunPreStartTasks()
        {
            var ambientScopeTaskHelper = CoreServiceProvider.LocateService<AmbientScopeTaskHelper>();
            ambientScopeTaskHelper.Run(() =>
            {
                var applicationTasks = CoreServiceProvider.Current.GetAllInstances<IApplicationPreStartTask>().ToList().OrderBy(x => x.Priority()).ToList();
                foreach (var applicationTask in applicationTasks)
                {
                    applicationTask.Execute();
                }
            });
        }
        public static void RunStartTasks()
        {
            var ambientScopeTaskHelper = CoreServiceProvider.LocateService<AmbientScopeTaskHelper>();
            ambientScopeTaskHelper.Run(() =>
            {
                var applicationTasks = CoreServiceProvider.Current.GetAllInstances<IApplicationStartTask>().ToList().OrderBy(x => x.Priority()).ToList();
                foreach (var applicationTask in applicationTasks)
                {
                    applicationTask.Execute();
                }
            });
        }
        public static void RunPostStartTasks()
        {
            var ambientScopeTaskHelper = CoreServiceProvider.LocateService<AmbientScopeTaskHelper>();
            ambientScopeTaskHelper.Run(() =>
            {
                var applicationTasks = CoreServiceProvider.Current.GetAllInstances<IApplicationPostStartTask>().ToList().OrderBy(x => x.Priority()).ToList();
                foreach (var applicationTask in applicationTasks)
                {
                    applicationTask.Execute();
                }
            });
        }
        public static void RunStopTasks()
        {
            var ambientScopeTaskHelper = CoreServiceProvider.LocateService<AmbientScopeTaskHelper>();
            ambientScopeTaskHelper.Run(() =>
            {
                var applicationTasks = CoreServiceProvider.Current.GetAllInstances<IApplicationStopTask>().ToList().OrderBy(x => x.Priority()).ToList();
                foreach (var applicationTask in applicationTasks)
                {
                    applicationTask.Execute();
                }
            });
        }
    }
}
