namespace NbCloud.Common.Tasks
{
    public interface IApplicationStopTask : IAutoTask
    {
        /// <summary>
        /// Before ApplicationStart Task
        /// </summary>
        void Execute();
    }
}