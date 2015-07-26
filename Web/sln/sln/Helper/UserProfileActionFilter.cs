using sln.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Helper
{
    public class UserProfileActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAdmin = false;
            filterContext.Controller.ViewBag.IsAuthenticated = filterContext.RequestContext.HttpContext.Request.IsAuthenticated;// MembershipService.IsAuthenticated;

            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.IsAdmin = filterContext.RequestContext.HttpContext.User.IsInRole(HelperAutorize.RoleAdmin);
            }

            MemeryCacheDataService views = new MemeryCacheDataService();
            filterContext.Controller.ViewBag.Views = views.GetView().ToList();

        }

    }
    //public class LayoutInjecterAttribute : ActionFilterAttribute
    //{
    //    private readonly string _masterName;
    //    public LayoutInjecterAttribute(string masterName)
    //    {
    //        _masterName = masterName;
    //    }

    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        base.OnActionExecuted(filterContext);
    //        var result = filterContext.Result as ViewResult;
    //        if (result != null)
    //        {
    //            result.MasterName = _masterName;
    //            }
    //    }
    //}
}