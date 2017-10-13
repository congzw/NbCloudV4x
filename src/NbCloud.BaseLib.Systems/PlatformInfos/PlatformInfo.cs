using System;

namespace NbCloud.BaseLib.Systems.PlatformInfos
{
    /// <summary>
    /// 平台信息
    /// </summary>
    public class PlatformInfo
    {
        public PlatformInfo()
        {
            SystemName = "纳博云平台";
            SystemNameOem = SystemName;
            Description = "纳博云平台";
            SystemVersionInner = "4.0.0.0";
            SystemVersion = "NbCloud V4";
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 系统名称（OEM）
        /// </summary>
        public string SystemNameOem { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 系统版本（内部）
        /// </summary>
        public string SystemVersionInner { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string SystemVersion { get; set; }

        /// <summary>
        /// Reset Version
        /// </summary>
        /// <param name="version"></param>
        public void SetSystemVersionInner(Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }
            SystemVersionInner = version.ToString();
        }
    }
}
