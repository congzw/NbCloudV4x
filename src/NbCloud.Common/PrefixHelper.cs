namespace NbCloud.Common
{
    public class PrefixHelper
    {
        static PrefixHelper()
        {
            //todo read from config
            Prefix = "NbCloud";
        }
        public static string Prefix { get; set; }
    }
}
