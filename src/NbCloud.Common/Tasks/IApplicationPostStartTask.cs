namespace NbCloud.Common.Tasks
{
    /// <summary>
    /// During Or After ApplicationStart Task
    /// </summary>
    public interface IApplicationPostStartTask : IAutoTask
    {
        /// <summary>
        /// Execute During Or After ApplicationStart Task
        /// </summary>
        void Execute();
    }
}