using NbCloud.Common.Db;
using NbCloud.Common.NHibernates;

namespace NbCloud.Web
{
    public class NHibernateConfig
    {
        public static void Setup()
        {
            var mySqlScriptHelper = MySqlScriptHelper.Resolve();
            var mySessionFactory = MySessionFactory.Resolve();
            var nHibernateSetup = new NHibernateSetup(mySqlScriptHelper, mySessionFactory);
            nHibernateSetup.AutoCreateDatabase();
        }
    }
}
