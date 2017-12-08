using System;

namespace NbCloud.Common.Logs
{
    public interface ILoggerManager
    {
        string Name { get; set; }
        ILogger GetLogger(string name);
        ILogger GetLogger(Type type);
    }
}