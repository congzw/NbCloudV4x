using System;
using System.Collections.Generic;

namespace NbCloud.Common
{
    public interface IMyIniHelper
    {
        /// <summary>
        /// 加载文件内容
        /// </summary>
        /// <param name="content"></param>
        IniFlatContent LoadIniContentAsFlat(string content);
    }

    public class MyIniFileHelper : IMyIniHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyIniHelper> _resolve = () => ResolveAsSingleton.Resolve<MyIniFileHelper, IMyIniHelper>();
        public static Func<IMyIniHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public IniFlatContent LoadIniContentAsFlat(string content)
        {
            return new IniFlatContent(content);
        }
    }
    
    #region dto
    
    public class IniFlatContent
    {
        public StringComparison Comparer { get; set; }
        public IDictionary<string, IDictionary<string, string>> Items { get; set; }

        public IniFlatContent(string content)
        {
            Comparer = StringComparison.OrdinalIgnoreCase;
            Items = Gini.ParseHash(content);
        }

        public string GetItemValue(string key, string defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key不能为空");
            }
            var result = defaultValue;
            foreach (var section in Items)
            {
                foreach (var sectionItem in section.Value)
                {
                    if (key.Equals(sectionItem.Key, Comparer))
                    {
                        result = sectionItem.Value;
                        break;
                    }
                }
            }
            return result;
        }

        public string GetSectionItemValue(string sectionKey, string key, string defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key不能为空");
            }
            var result = defaultValue;
            foreach (var section in Items)
            {
                if (section.Key.Equals(sectionKey, Comparer))
                {
                    foreach (var sectionItem in section.Value)
                    {
                        if (key.Equals(sectionItem.Key, Comparer))
                        {
                            result = sectionItem.Value;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }

    #endregion
}
