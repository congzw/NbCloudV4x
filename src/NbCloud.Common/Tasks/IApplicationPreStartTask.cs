namespace NbCloud.Common.Tasks
{
    /// <summary>
    /// Before ApplicationStart Task
    /// </summary>
    public interface IApplicationPreStartTask : IAutoTask
    {
        /// <summary>
        /// Before ApplicationStart Task
        /// </summary>
        void Execute();
    }
}