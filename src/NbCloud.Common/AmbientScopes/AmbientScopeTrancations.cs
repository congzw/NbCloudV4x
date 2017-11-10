﻿namespace NbCloud.Common.AmbientScopes
{
    public interface IAmbientScopeTrancation
    {
        /// <summary>
        /// 标示为提交，销毁实例时触发实际的操作
        /// </summary>
        void Commit();
        /// <summary>
        /// 标示为取消，销毁实例时触发实际的操作
        /// </summary>
        void Cancel();
    }
}
