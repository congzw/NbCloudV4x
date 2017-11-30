using System;
using System.Collections.Generic;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugIn
    {
        string Code { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
        ///// <summary>
        ///// 依赖的组件
        ///// </summary>
        //string Dependency { get; set; }
    }

    public class PlugIn : IPlugIn
    {
        public PlugIn(string code, string name, string version)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("code");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException("version");
            }
            Version parseVersion;
            var tryParse = System.Version.TryParse(version, out parseVersion);
            if (!tryParse)
            {
                throw new ArgumentException("非法的version：" + version);
            }
            Code = code;
            Name = name;
            Version = version;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
    
    public class PlugInList : List<IPlugIn>
    {
    }
}
