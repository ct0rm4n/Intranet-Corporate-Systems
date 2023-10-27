using System.Web;
using System.Web.Mvc;

namespace IntraNet.Mod.SGR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
