using System;

namespace NbCloud.Common.Scopes
{
    public interface INbScopeTrancationManager : IDisposable
    {
        /// <summary>
        /// 立刻触发事务处理。根据当前上下文，提交或者回滚当前事务，然后开启一个新的事务
        /// </summary>
        void RequireNew();
        /// <summary>
        /// 标示为提交，销毁实例时触发实际的操作
        /// </summary>
        void Commit();
        /// <summary>
        /// 标示为取消，销毁实例时触发实际的操作
        /// </summary>
        void Cancel();
    }

    public class EmptyTrancationManager : INbScopeTrancationManager
    {
        public void RequireNew()
        {
            RequireNewInvoked = true;
        }

        public void Commit()
        {
            CommitInvoked = true;
        }

        public void Cancel()
        {
            CancelInvoked = true;
        }
        public void Dispose()
        {
            if (DisposeInvoking != null)
            {
                DisposeInvoking(this);
            }
        }

        public bool RequireNewInvoked { get; set; }
        public bool CommitInvoked { get; set; }
        public bool CancelInvoked { get; set; }
        public Action<INbScopeTrancationManager> DisposeInvoking { get; set; }

    }
}