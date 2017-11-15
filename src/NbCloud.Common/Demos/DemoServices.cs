using System;
using NbCloud.Common.Data;
using NbCloud.Common.Data.Model;
using NbCloud.Common.Data.Provider.Nhibernate;
using NbCloud.Common.Logs;

namespace NbCloud.Common.Demos
{
    public interface IDemoService : ITransientDependency
    {
        string Hello();
    }

    public class DemoServices : IDemoService
    {
        private readonly IRepository<DemoEntity, Guid> _repository;
        private readonly IRepository<DemoEntity, Guid> _repository2;
        //public string Hello()
        //{
        //    var message = string.Format("Service:{0}"
        //        , this.GetHashCode());
        //    LogMessage(message);
        //    return message;
        //}
        public DemoServices(IRepository<DemoEntity, Guid> repository, IRepository<DemoEntity, Guid> repository2)
        {
            _repository = repository;
            _repository2 = repository2;
        }

        public string Hello()
        {
            var repositoryContext = _repository.Context as NhRepositoryContext;
            if (repositoryContext == null)
            {
                throw new InvalidOperationException("repositoryContext is null");
            }

            var message = string.Format("Session:{0}, Context:{1}, Repository:{2}+{3}, Service:{4}"
                , repositoryContext.DbContext.GetHashCode()
                , repositoryContext.GetHashCode()
                , _repository.GetHashCode()
                , _repository2.GetHashCode()
                , this.GetHashCode());
            LogMessage(message);
            return message;
        }

        private void LogMessage(string message)
        {
            MyLogHelper.Resolve().Debug(this.GetType(), message);
        }
    }
    public class DemoEntity : NbEntity<DemoEntity>
    {

    }
}
