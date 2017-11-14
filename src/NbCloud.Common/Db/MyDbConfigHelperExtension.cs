namespace NbCloud.Common.Db
{
    public static class MyDbConfigHelperExtension
    {
        /// <summary>
        /// FindDbNameFromConnection
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string FindDbNameFromConnection(this IMyDbConfigHelper helper, string connName)
        {
            var connectionString = helper.GetConnectionString(connName);
            var dbName = helper.FindDbNameFromConnectionString(connectionString);
            return dbName;
        }

        /// <summary>
        /// 自动猜测默认数据库连接的名字
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="defaultValue">如果没有指定默认值，则以程序的前缀为默认名称</param>
        /// <returns></returns>
        public static string GuessDefaultConnName(this IMyDbConfigHelper helper, string defaultValue = null)
        {
            var myConfigHelper = MyConfigHelper.Resolve();
            if (!string.IsNullOrWhiteSpace(defaultValue))
            {
                return GetConnName(myConfigHelper, defaultValue);
            }

            var projectPrefix = MyProjectHelper.Resolve().GetProjectPrefix();
            return GetConnName(myConfigHelper, projectPrefix);
        }

        private static string Config_Common_ConnName = "Config.Common.ConnName";
        private static string GetConnName(IMyConfigHelper helper, string defaultValue)
        {
            var connName = helper.GetAppSettingValue(Config_Common_ConnName, defaultValue);
            return connName;
        }
    }
}