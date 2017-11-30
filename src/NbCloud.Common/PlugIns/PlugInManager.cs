using System.Linq;
using NbCloud.Common.Collections.Extensions;
using NbCloud.Common.Extensions;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInManager
    {
        /// <summary>
        /// 模块是否注册
        /// </summary>
        /// <param name="plugIn"></param>
        /// <returns></returns>
        bool IsRegistered(IPlugIn plugIn);

        /// <summary>
        /// 模块注册
        /// </summary>
        /// <param name="plugIn"></param>
        void Register(IPlugIn plugIn);

        /// <summary>
        /// 所有的插件列表
        /// </summary>
        PlugInList PlugInList { get; }
    }

    public class PlugInManager : IPlugInManager
    {
        public PlugInManager()
        {
            PlugInList = new PlugInList();
        }

        public PlugInList PlugInList { get; private set; }

        public void Register(IPlugIn plugIn)
        {
            var isRegistered = IsRegistered(plugIn);
            if (isRegistered)
            {
                return;
            }
            PlugInList.AddIfNotContains(plugIn);
        }

        public bool IsRegistered(IPlugIn plugIn)
        {
            return PlugInList.Any(x => x.Code.NbEquals(plugIn.Code) && x.Version.NbEquals(plugIn.Version));
        }
    }
}