using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NbCloud.Common.Collections.Extensions;
using NbCloud.Common.Extensions;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInManager
    {
        /// <summary>
        /// �Ƿ�ע���
        /// </summary>
        /// <param name="plugIn"></param>
        /// <returns></returns>
        bool IsRegistered(IPlugIn plugIn);

        /// <summary>
        /// ע����
        /// </summary>
        /// <param name="plugIn"></param>
        void Register(IPlugIn plugIn);

        /// <summary>
        /// ����б�
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