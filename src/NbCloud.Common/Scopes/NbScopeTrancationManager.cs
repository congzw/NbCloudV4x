using System;

namespace NbCloud.Common.Scopes
{
    public interface INbScopeTrancationManager : IDisposable
    {
        /// <summary>
        /// ���̴������������ݵ�ǰ�����ģ��ύ���߻ع���ǰ����Ȼ����һ���µ�����
        /// </summary>
        void RequireNew();
        /// <summary>
        /// ��ʾΪ�ύ������ʵ��ʱ����ʵ�ʵĲ���
        /// </summary>
        void Commit();
        /// <summary>
        /// ��ʾΪȡ��������ʵ��ʱ����ʵ�ʵĲ���
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