using Microsoft.Owin.Security;
using sln.Dal;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sln.Controllers
{
    public class StatusController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ApprovalRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                Guid approval = Guid.Parse(Helper.Status.ApporvallRequest);
                var currentDate = DateTime.Now;
                if (ship != null)
                {
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;

                    TimeLine tl = new TimeLine
                    {
                        Name = "הזמנה אושרה" + "של " + user.FullName + " (" + user.EmpId + ")",
                        Desc = "הזמנה אושרה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm"),
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.ApporvallRequest,
                        StatusShipping_StatusShippingId = approval
                    };
                    ship.TimeLines.Add(tl);
                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }

        public async Task<ActionResult> ConfirmRequest(string id, string assignTo)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                Guid approval = Guid.Parse(Helper.Status.Confirm);
                MemeryCacheDataService cache = new MemeryCacheDataService();

                var grantToText = "";
                var currentDate = DateTime.Now;
                if (ship != null)
                {
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;
                    ship.ApprovalShip = userid;
                    if (String.IsNullOrEmpty(assignTo))
                    {
                        ship.GrantRunner = Guid.Parse(assignTo);
                        grantToText = cache.GetRunners(context).Where(run => run.Id == assignTo).Select(run2=>run2.FullName).FirstOrDefault(); 

                    }
                    else
                    {
                        ship.GrantRunner = userid;
                        grantToText = user.FullName;
                    }
                    TimeLine tl = new TimeLine
                    {
                        Name = "הזמנה אושרה ע'' חברת השליחות" + "של " + user.FullName + " (" + user.EmpId + ")",
                        Desc = "הזמנה אושרה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm") +" והועברה לשליח"+ " "+grantToText,
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.ApporvallRequest,
                        StatusShipping_StatusShippingId = approval
                    };
                    ship.TimeLines.Add(tl);
                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}