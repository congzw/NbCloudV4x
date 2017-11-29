//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Web.Compilation;
//using NbCloud.Common.Tasks;

//namespace NbCloud.Common.PlugIns
//{
//    public class ApplicationPreStartTask : IApplicationPreStartTask
//    {
//        public int Priority()
//        {
//            return this.Priority_Minus_10000();
//        }

//        public void Execute()
//        {
//            //for demo todo fix
//            //var folderPath = @"D:\WS_Local\NbCloudV4x\src\NbCloud.Web\Areas\Demo\bin\Debug";
//            var folderPath = @"D:\WS_Local\NbCloudV4x\src\NbCloud.Web.Area.Demo\bin\Debug";
//            var assemblyFiles = Directory
//                  .EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories)
//                  .Where(s => s.EndsWith("NbCloud.Web.Area.Demo.dll"));
//            MyPlugInManager.LoadPluginAssemblies(assemblyFiles);
//        }
//    }

//    public class MyPlugInManager
//    {
//        public static void LoadPluginAssemblies(IEnumerable<string> assemblyFiles)
//        {
//            var plugInAssemblies = assemblyFiles.Select(Assembly.LoadFile).ToList();
//            foreach (var plugInAssembly in plugInAssemblies)
//            {
//                BuildManager.AddReferencedAssembly(plugInAssembly);
//            }
//        }
//    }

//    public interface INbPluginManager
//    {
//        void LoadAssemblies(IEnumerable<string> assemblyFiles);
//    }
//}
