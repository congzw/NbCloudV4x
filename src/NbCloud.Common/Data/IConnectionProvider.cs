namespace NbCloud.Common.Data
{
    /// <summary>
    /// 提供请求的数据库连接
    /// </summary>
    public interface IDbConnectionProvider : IDependency
    {
        /// <summary>
        /// 获取当前上下文的数据库连接字符串
        /// </summary>
        /// <returns></returns>
        string GetDbConnectionString();

        /// <summary>
        /// 获取指定的数据库连接字符串
        /// </summary>
        /// <param name="dbConnectionName"></param>
        /// <returns></returns>
        string GetDbConnectionString(string dbConnectionName);

        /// <summary>
        /// 尝试猜测默认的数据库连接名称
        /// </summary>
        /// <returns></returns>
        string TryGetDefaultDbConnectionName();
    }
    
    /// <summary>
    /// 多租户提供数据库链接
    /// </summary>
    public class DbConnectionProvider : IDbConnectionProvider
    {
        #region IConnectionProvider 成员

        public string GetDbConnectionString()
        {
            //a tenant DbConnectionProvider could replace this default impl
            #region e.g.
            //var tenant = CoreServiceProvider.LocateService<Tenant>();
            //if (tenant == null || tenant.Name == Tenant.DefaultName)
            //{
            //    return null;
            //}
            //return tenant.DataConnectionString;
            #endregion
            var dbConnectionName = TryGetDefaultDbConnectionName();
            return GetDbConnectionString(dbConnectionName);
        }

        public string GetDbConnectionString(string dbConnectionName)
        {
            throw new System.NotImplementedException();
        }

        public string TryGetDefaultDbConnectionName()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
