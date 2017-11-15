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

            Log("====MyBootstrapper TaskConfig.PreStartTasks() Begin====");
            TaskConfig.PreStartTasks();
            Log("====MyBootstrapper TaskConfig.PreStartTasks() End====");
        }

        public static void PostStart()
        {
            Log("====MyBootstrapper TaskConfig.PostStartTasks() Begin====");
            TaskConfig.PostStartTasks();
            Log("====MyBootstrapper TaskConfig.PostStartTasks() End====");
        }

        public static void Stop()
        {
            Log("====MyBootstrapper TaskConfig.StopTasks() Begin====");
            TaskConfig.StopTasks();
            Log("====MyBootstrapper TaskConfig.StopTasks() End====");
        }

        private static void Log(string message)
        {
            MyLogHelper.Resolve().Debug(message);
        }
    }
}
