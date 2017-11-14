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
    }
}