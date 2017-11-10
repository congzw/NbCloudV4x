namespace NbCloud.Common.Data
{
    /// <summary>
    /// 指示Uow当销毁时，发生异常，是否应该吞并异常，用于特殊场景事件（例如备份、还原数据库，网站重启等）
    /// </summary>
    public class RepositoryContextDisposeHelper
    {
        /// <summary>
        /// 指示Uow当销毁时，发生异常，是否应该吞并异常，用于特殊场景事件（例如备份、还原数据库，网站重启等）
        /// 例如，备份还原操作，数据库连接会被强制断开，页面显示错误
        /// 默认为False
        /// </summary>
        public static bool HideDisposingException { get; set; }
    }
}
