using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace NbCloud.Common.Db
{
    /// <summary>
    /// 数据库相关
    /// </summary>
    public interface IMySqlScriptHelper
    {
        /// <summary>
        /// 检测数据库是否存在
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        MessageResult CheckDbExist(string connStr, string dbName);

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        MessageResult CreateDbIfNotExist(string connStr, string dbName);
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        MessageResult DropDbIfExist(string connString, string dbName);
    }

    public class MySqlScriptHelper: IMySqlScriptHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMySqlScriptHelper> _resolve = () => ResolveAsSingleton.Resolve<MySqlScriptHelper, IMySqlScriptHelper>();
        public static Func<IMySqlScriptHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public MessageResult CheckDbExist(string connStr, string dbName)
        {
            MessageResult mr = new MessageResult();
            string script = string.Format(
@"USE [master]
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}')
BEGIN
    SELECT 0
END
ELSE
BEGIN
    SELECT 1
END", dbName);

            string newConnStr = connStr.ToLower().Replace(dbName.ToLower(), "master");
            using (var sqlCon = new SqlConnection(newConnStr))
            {
                var cmd = sqlCon.CreateCommand();
                cmd.Connection = sqlCon;
                ExecuteNonQuerySqlWithGo(cmd, script);
                int count = (int)ExecuteScriptScalar(sqlCon, script);
                mr.Success = count > 0;
                mr.Message = string.Format("数据库[{0}]{1}存在", dbName, count > 0 ? "已" : "不");
                mr.Data = count;
                return mr;
            }
        }
        public MessageResult CreateDbIfNotExist(string connStr, string dbName)
        {
            string script = string.Format(
@"USE [master]
GO
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}')
BEGIN
CREATE DATABASE [{0}] 
END", dbName);

            string newConnStr = connStr.Replace(dbName, "master");
            MessageResult mr = RunScript(newConnStr, script);
            return mr;
        }
        public MessageResult DropDbIfExist(string connStr, string dbName)
        {
            string script = string.Format(
@"USE [master]
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}')
BEGIN
Drop DATABASE [{0}] 
END", dbName);

            string newConnStr = connStr.Replace(dbName, "master");
            MessageResult mr = RunScript(newConnStr, script);
            return mr;
        }


        private MessageResult RunScript(string connString, string scriptSql, string rollbackScript = null)
        {
            using (var sqlCon = new SqlConnection(connString))
            {
                MessageResult mr = ExecuteScript(sqlCon, scriptSql);
                if (!mr.Success && !string.IsNullOrWhiteSpace(rollbackScript))
                {
                    mr = ExecuteScript(sqlCon, rollbackScript);
                    mr.Success = false;
                    mr.Message = "执行回滚操作完毕";
                }
                return mr;
            }
        }
        private MessageResult ExecuteScript(IDbConnection sqlCon, string scriptSql)
        {
            MessageResult mr = new MessageResult();
            if (sqlCon == null)
            {
                mr.Success = false;
                mr.Message = "数据库连接不能空";
                return mr;
            }

            if (string.IsNullOrWhiteSpace(scriptSql))
            {
                mr.Success = false;
                mr.Message = "scriptSql不能空";
                return mr;
            }

            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] sqls = regex.Split(scriptSql);

            var cmd = sqlCon.CreateCommand();
            cmd.Connection = sqlCon;
            sqlCon.Open();
            foreach (string line in sqls)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        mr.Success = false;
                        mr.Message = ex.Message;
                        return mr;
                    }
                }
            }
            sqlCon.Close();

            mr.Success = true;
            mr.Message = "执行完毕";
            return mr;
        }
        private void ExecuteNonQuerySqlWithGo(IDbCommand cmd, string scriptSql)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }

            if (string.IsNullOrWhiteSpace(scriptSql))
            {
                throw new ArgumentNullException("scriptSql");
            }

            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] sqls = regex.Split(scriptSql);

            bool needClosed = false;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                needClosed = true;
                cmd.Connection.Open();
            }

            foreach (string line in sqls)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }

            if (needClosed)
            {
                cmd.Connection.Close();
            }
        }
        private object ExecuteScriptScalar(IDbConnection sqlCon, string scriptSql)
        {
            object result = null;
            if (sqlCon == null)
            {
                throw new ArgumentNullException("sqlCon");
            }

            if (string.IsNullOrWhiteSpace(scriptSql))
            {
                throw new ArgumentNullException("scriptSql");
            }

            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] sqls = regex.Split(scriptSql);
            var cmd = sqlCon.CreateCommand();
            sqlCon.Open();

            //执行并返回最后一个Scalar
            foreach (var sql in sqls)
            {
                string line = sql;
                if (!string.IsNullOrEmpty(line))
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    result = cmd.ExecuteScalar();
                }
            }

            sqlCon.Close();
            return result;
        }
    }
}
