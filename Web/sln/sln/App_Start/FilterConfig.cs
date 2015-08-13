using Michal.Project.Helper;
using System.Web;
using System.Web.Mvc;

namespace Michal.Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserProfileActionFilter());
        }
    }
}
