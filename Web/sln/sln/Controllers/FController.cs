using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Michal.Project.Bll;
using System.Linq.Expressions;
using Michal.Project.Models.View;

namespace Michal.Project.Controllers
{
    [Authorize]
    public class FController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //public async Task<ActionResult> IndexOld(int? currentPage, string nextDay, string prevDay)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var user = new UserContext(AuthenticationManager);
        //        Guid orgId = Guid.Empty;
        //        MemeryCacheDataService cache = new MemeryCacheDataService();

        //        if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
        //            orgId = Guid.Empty; //user.OrgId;
        //        List<Shipping> shippings = new List<Shipping>();
        //        var from = DateTime.Today.AddDays(-1).Date;
        //        var to = DateTime.Today.AddDays(1).Date;

        //        if (!String.IsNullOrEmpty(nextDay))
        //            to = DateTime.ParseExact(nextDay, "yyyy-MM-dd", null);

        //        if (!String.IsNullOrEmpty(prevDay))
        //            from = DateTime.ParseExact(prevDay, "yyyy-MM-dd", null);


        //        var shippingsQuery = context.Shipping.Where(s => s.FollowsBy.Any(gf => gf.Id == user.UserId.ToString()) && (s.CreatedOn > from &&
        //            s.CreatedOn <= to)).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

        //        int page = currentPage.HasValue ? currentPage.Value : 1;
        //        var total = await shippingsQuery.CountAsync();
        //        var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

        //        shippings = await shippingsQuery.OrderByDescending(ord => ord.ModifiedOn).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();
        //        var model = new List<ShippingVm>();
        //        foreach (var ship in shippings)
        //        {

        //            var u = new ShippingVm();
        //            u.Id = ship.ShippingId;
        //            u.Status = ship.StatusShipping.Desc;
        //            u.Name = ship.Name;
        //            u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
        //            u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
        //            u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
        //            u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
        //            u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
        //            u.NumFrom = ship.AddressNumFrom;
        //            u.NumTo = ship.AddressNumTo;
        //            u.SreetFrom = ship.AddressFrom;
        //            u.SreetTo = ship.AddressTo;
        //            u.TelTarget = ship.TelTarget;
        //            u.NameTarget = ship.NameTarget;


        //            model.Add(u);

        //        }

        //        bool isToday = to.Date == DateTime.Now.AddDays(1).Date;


        //        ViewBag.Total = total;
        //        ViewBag.CurrentPage = page;
        //        ViewBag.MoreRecord = hasMoreRecord;
        //        ViewBag.FromDay = from.ToString("yyyy-MM-dd");
        //        ViewBag.ToDay = to.ToString("yyyy-MM-dd");
        //        ViewBag.IsToday = isToday;
        //        ViewBag.Title = "מעקב המשלוחים שלי לתאריך " + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");

        //        return View(model);
        //    }
        //}

        public async Task<ActionResult> Index(int? currentPage, string nextDay, string prevDay)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                MemeryCacheDataService cache = new MemeryCacheDataService();

                if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
                    orgId = Guid.Empty; //user.OrgId;
                List<Shipping> shippings = new List<Shipping>();
                var from = DateTime.Today.AddDays(-1).Date;
                var to = DateTime.Today.AddDays(1).Date;

                if (!String.IsNullOrEmpty(nextDay))
                    to = DateTime.ParseExact(nextDay, "yyyy-MM-dd", null);

                if (!String.IsNullOrEmpty(prevDay))
                    from = DateTime.ParseExact(prevDay, "yyyy-MM-dd", null);


                var shippingsQuery = context.Shipping.Where(s => s.FollowsBy.Any(gf => gf.Id == user.UserId.ToString()) && (s.CreatedOn > from &&
                    s.CreatedOn <= to)).AsQueryable();// && (!showAll && view.GetOnlyMyRecords(s,user))).AsQueryable();//)).AsQueryable();

                int page = currentPage.HasValue ? currentPage.Value : 1;
                var total = await shippingsQuery.CountAsync();
                var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

                shippings = await shippingsQuery.OrderByDescending(ord => ord.ModifiedOn).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();
                var shippingsItems = new List<ShippingVm>();
                foreach (var ship in shippings)
                {

                    var u = new ShippingVm();
                    u.Id = ship.ShippingId;
                    u.Status = ship.StatusShipping.Desc;
                    u.Name = ship.Name;
                    u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
                    u.ShipTypeIdName = ship.ShipType != null ? ship.ShipType.Name : "";
                    u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
                    u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
                    u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
                    u.NumFrom = ship.AddressNumFrom;
                    u.NumTo = ship.AddressNumTo;
                    u.SreetFrom = ship.AddressFrom;
                    u.SreetTo = ship.AddressTo;
                    u.TelTarget = ship.TelTarget;
                    u.NameTarget = ship.NameTarget;
                    shippingsItems.Add(u);

                }

                bool isToday = to.Date == DateTime.Now.AddDays(1).Date;

                //ViewBag.Total = total;
                //ViewBag.CurrentPage = page;
                //ViewBag.MoreRecord = hasMoreRecord;
                //ViewBag.FromDay = from.ToString("yyyy-MM-dd");
                //ViewBag.ToDay = to.ToString("yyyy-MM-dd");
                //ViewBag.IsToday = isToday;
                //ViewBag.Title = "מעקב המשלוחים שלי לתאריך " + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");


                FollowsView followsView = new FollowsView();
                followsView.Items = shippingsItems.AsEnumerable();
                followsView.ClientViewType = ClientViewType.Follows;

                followsView.Total = total;
                followsView.CurrentPage = page;
                followsView.MoreRecord = hasMoreRecord;
                followsView.Title = "מעקב המשלוחים שלי לתאריך " + " " + to.Date.AddMinutes(-1).ToString("dd/MM/yyyy");

                followsView.FromDay = from.ToString("yyyy-MM-dd");
                followsView.ToDay = to.ToString("yyyy-MM-dd");
                followsView.IsToday = to.Date == DateTime.Now.AddDays(1).Date;

                return View(followsView);
            }
        }

    }
}
