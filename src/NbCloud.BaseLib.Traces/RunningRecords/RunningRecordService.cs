using System;
using System.Linq;
using NbCloud.Common;
using NbCloud.Common.Data;
using NbCloud.Common.Web;

namespace NbCloud.BaseLib.Traces.RunningRecords
{
    public interface IRunningRecordService : IDependency
    {
        /// <summary>
        /// start application
        /// </summary>
        void Start();
        /// <summary>
        /// stop application
        /// </summary>
        void Stop();
    }

    public class RunningRecordService : IRunningRecordService
    {
        private readonly IRepository<RunningRecordEntity, Guid> _repository;

        public RunningRecordService(IRepository<RunningRecordEntity,Guid> repository)
        {
            _repository = repository;
        }

        public void Start()
        {
            //todo date now!
            var runningRecordEntity = new RunningRecordEntity();
            runningRecordEntity.ApplicationName = MyHostHelper.Resolve().GetApplicationName();
            runningRecordEntity.StartAt = UtilsDateTime.GetTime();
            _repository.Create(runningRecordEntity);
        }

        public void Stop()
        {
            var runningRecordEntity = _repository.Table.OrderByDescending(x => x.StartAt).FirstOrDefault(x => x.StopAt == null);
            if (runningRecordEntity != null)
            {
                runningRecordEntity.StopAt = UtilsDateTime.GetTime();
                _repository.Update(runningRecordEntity);
            }
        }
    }
}
