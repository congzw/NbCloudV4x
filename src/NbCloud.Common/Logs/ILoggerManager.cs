using System;

namespace NbCloud.Common.Logs
{
    public interface ILoggerManager
    {
        ILogger GetLogger(string name);
        ILogger GetLogger<T>();
        ILogger GetLogger(Type type);
        ILogger GetLogger();
    }
}