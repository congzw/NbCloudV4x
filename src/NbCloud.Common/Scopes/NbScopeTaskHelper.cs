using System;
using System.Threading.Tasks;

namespace NbCloud.Common.Scopes
{
    public interface INbScopeTaskHelper
    {
        #region 同步

        /// <summary>
        /// 在事务中同步运行任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        void Run(Action action);

        /// <summary>
        /// 在事务中同步运行任务，只记录异常，不抛出
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        void TryRun(Action action);

        /// <summary>
        /// 在事务中同步运行任务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        TResult Run<TResult>(Func<TResult> func);

        /// <summary>
        /// 在事务中同步运行任务，只记录异常，不抛出
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        TResult TryRun<TResult>(Func<TResult> func);

        #endregion

        #region 异步
        
        /// <summary>
        /// 在事务中异步运行任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task RunAsync(Action action);

        /// <summary>
        /// 在事务中异步运行任务，只记录异常，不抛出
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task TryRunAsync(Action action);

        /// <summary>
        /// 在事务中异步运行任务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<TResult> RunAsync<TResult>(Func<TResult> func);

        /// <summary>
        /// 在事务中异步运行任务，只记录异常，不抛出
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<TResult> TryRunAsync<TResult>(Func<TResult> func);

        #endregion
    }
    
    public class NbScopeTaskHelper : INbScopeTaskHelper
    {
        private readonly INbScopeResolver _resolver;

        public NbScopeTaskHelper(INbScopeResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// 在事务中同步运行任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void Run(Action action)
        {
            _run(action, true);
        }

        /// <summary>
        /// 在事务中同步运行任务，只记录异常，不抛出
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void TryRun(Action action)
        {
            _run(action, false);
        }

        /// <summary>
        /// 在事务中同步运行任务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult Run<TResult>(Func<TResult> func)
        {
            return _run(func, true);
        }

        /// <summary>
        /// 在事务中同步运行任务，只记录异常，不抛出
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public TResult TryRun<TResult>(Func<TResult> func)
        {
            return _run(func, false);
        }
        
        #region 异步

        /// <summary>
        /// 在事务中异步运行任务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task RunAsync(Action action)
        {
            await Task.Run(() => _run(action, true));
        }
        /// <summary>
        /// 在事务中异步运行任务，只记录异常，不抛出
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task TryRunAsync(Action action)
        {
            await Task.Run(() => _run(action, false));
        }

        /// <summary>
        /// 在事务中异步运行任务
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<TResult> RunAsync<TResult>(Func<TResult> func)
        {
            return await Task.Run(() => _run(func, true));
        }
        /// <summary>
        /// 在事务中异步运行任务，只记录异常，不抛出
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<TResult> TryRunAsync<TResult>(Func<TResult> func)
        {
            return await Task.Run(() => _run(func, false));
        }

        #endregion

        //helpers
        private void _run(Action action, bool throwEx)
        {
            using (var scope = _resolver.CreateNewScope())
            {
                bool shouldRollBack = false;
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    shouldRollBack = true;
                    LogException(ex);
                    if (throwEx)
                    {
                        throw;
                    }
                }
                finally
                {
                    var nbScopeTrancation = scope.TrancationManager;
                    if (nbScopeTrancation != null)
                    {
                        if (shouldRollBack)
                        {
                            nbScopeTrancation.Cancel();
                        }
                        else
                        {
                            nbScopeTrancation.Commit();
                        }
                    }
                    _resolver.ReleaseScope(scope);
                }
            }
        }
        private TResult _run<TResult>(Func<TResult> func, bool throwEx)
        {
            using (var scope = _resolver.CreateNewScope())
            {
                TResult result = default(TResult);
                bool shouldRollBack = false;
                try
                {
                    result = func.Invoke();
                }
                catch (Exception ex)
                {
                    shouldRollBack = true;
                    LogException(ex);
                    if (throwEx)
                    {
                        throw;
                    }
                }
                finally
                {
                    var nbScopeTrancation = scope.TrancationManager;
                    if (nbScopeTrancation != null)
                    {
                        if (shouldRollBack)
                        {
                            nbScopeTrancation.Cancel();
                        }
                        else
                        {
                            nbScopeTrancation.Commit();
                        }
                    }
                    _resolver.ReleaseScope(scope);
                }
                return result;
            }
        }
        private void LogException(Exception ex)
        {
            UtilsLogger.LogMessage("[NbScopeTaskHelper] => Exception: " + ex.Message);
        }

    }
}