using System.Collections.Generic;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugIn
    {
        string Code { get; set; }
        string Name { get; set; }
        string Version { get; set; }
        string Description { get; set; }
    }

    public class PlugIn : IPlugIn
    {
        public PlugIn(string code, string name, string version)
        {
            //todo null check
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
