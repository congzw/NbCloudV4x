using System;

namespace NbCloud.BaseLib.Tentants
{
    public class Tenant
    {
        public string Id { get; set; }
        /// <summary>
        /// 唯一名
        /// </summary>
        public string UniqueName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }
    }
}