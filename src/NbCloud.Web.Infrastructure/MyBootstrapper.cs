using NbCloud.Common.Logs;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NbCloud.Web.MyBootstrapper), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(NbCloud.Web.MyBootstrapper), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NbCloud.Web.MyBootstrapper), "Stop")]
namespace NbCloud.Web
{
    public class MyBootstrapper
    {
        public static void PreStart()
        {
            Log("====MyBootstrapper NHibernateConfig.Setup() Start====");
            NHibernateConfig.Setup();
            Log("====MyBootstrapper NHibernateConfig.Setup() End====");

            Log("====MyBootstrapper NinjectConfig.Setup() Start====");
            NinjectConfig.Setup();
            Log("====MyBootstrapper NinjectConfig.Setup() End====");

            Log("====MyBootstrapper TaskConfig.RunPreStartTasks() Begin====");
            TaskConfig.RunPreStartTasks();
            Log("====MyBootstrapper TaskConfig.RunPreStartTasks() End====");
        }

        public static void PostStart()
        {
            Log("====MyBootstrapper TaskConfig.RunPostStartTasks() Begin====");
            TaskConfig.RunPostStartTasks();
            Log("====MyBootstrapper TaskConfig.RunPostStartTasks() End====");
        }

        public static void Stop()
        {
            Log("====MyBootstrapper TaskConfig.RunStopTasks() Begin====");
            TaskConfig.RunStopTasks();
            Log("====MyBootstrapper TaskConfig.RunStopTasks() End====");
        }

        private static void Log(string message)
        {
            MyLogHelper.Resolve().Debug(message);
        }
    }
}
