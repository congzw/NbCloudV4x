using NbCloud.Common;
using NbCloud.Common.Ioc;
using NbCloud.Common.Tasks;

namespace NbCloud.BaseLib.Traces.RunningRecords
{
    public class RunningRecordTask : IApplicationPostStartTask, IApplicationStopTask
    {
        public int Priority()
        {
            return this.Priority_Default_0();
        }

        void IApplicationPostStartTask.Execute()
        {
            var runningRecordService = CoreServiceProvider.LocateService<IRunningRecordService>();
            runningRecordService.Start();
        }

        void IApplicationStopTask.Execute()
        {
            var runningRecordService = CoreServiceProvider.LocateService<IRunningRecordService>();
            runningRecordService.Stop();
        }
    }
}