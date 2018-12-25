using System.Web;
using System.Web.Mvc;

namespace Tickets_Sieviertsev_Vadym_IS62
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
