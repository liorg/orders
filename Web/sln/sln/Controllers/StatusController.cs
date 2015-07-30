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

        public async Task<ActionResult> RemoveItem(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                var title = "הזמנה בוטלה ע''י" + " " + user.FullName;
                var currentDate = DateTime.Now;
                var text = title;
                if (ship != null)
                {
                    ship.IsActive = false;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.NotifyType = Helper.Notification.Error;
                    ship.NotifyText = text;
                    ship.StatusShipping_StatusShippingId = Guid.Parse(Helper.Status.Cancel);

                    TimeLine tl = new TimeLine
                    {
                        Name = title,
                        Desc = text,
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.Cancel,
                    };
                    ship.TimeLines.Add(tl);
                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index","s");
            }
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
                var text = "הזמנה אושרה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm");
                
                if (ship != null)
                {
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;
                    ship.NotifyText = text;
                    ship.NotifyType = Helper.Notification.Info;

                    TimeLine tl = new TimeLine
                    {
                        Name = "הזמנה אושרה" + "של " + user.FullName + " (" + user.EmpId + ")",
                        Desc = text,
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
                if (!String.IsNullOrEmpty(assignTo))
                {
                    ship.GrantRunner = Guid.Parse(assignTo);
                    grantToText = cache.GetRunners(context).Where(run => run.Id == assignTo).Select(run2 => run2.FullName).FirstOrDefault();

                }
                else
                {
                    ship.GrantRunner = userid;
                    grantToText = user.FullName;
                }
                var currentDate = DateTime.Now;
                if (ship != null)
                {
                    var title = "הזמנה אושרה ע'' חברת השליחות" + " ע''י " + user.FullName + " (" + user.EmpId + ")";
                    var text = title +System.Environment.NewLine+" " + "הזמנה אושרה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm") + " והועברה לשליח" + " " + grantToText;
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;
                    ship.ApprovalShip = userid;
                    ship.NotifyText = text;
                    ship.NotifyType = Helper.Notification.Info;
        
                    TimeLine tl = new TimeLine
                    {
                        Name = title,
                        Desc = text,
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.Confirm,
                        StatusShipping_StatusShippingId = approval
                    };
                    ship.TimeLines.Add(tl);
                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }

        public async Task<ActionResult> Accept(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                Guid approval = Guid.Parse(Helper.Status.AcceptByRunner);
                MemeryCacheDataService cache = new MemeryCacheDataService();
               
                var currentDate = DateTime.Now;
                if (ship != null)
                {
                    var title = "הזמנה  התקבלה " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")";
                    var text = title + System.Environment.NewLine + " " + " מספר הזמנה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm") ;
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;
                    ship.ApprovalShip = userid;
                    ship.NotifyText = text;
                    ship.NotifyType = Helper.Notification.Info;

                    TimeLine tl = new TimeLine
                    {
                        Name = title,
                        Desc = text,
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.AcceptByRunner,
                        StatusShipping_StatusShippingId = approval
                    };
                    ship.TimeLines.Add(tl);
                }
                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }

        public async Task<ActionResult> CancelRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);
                Guid approval = Guid.Parse(Helper.Status.CancelByAdmin);
                var currentDate = DateTime.Now;
                var text = "הזמנה לא מאושרת" + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm");
                if (ship != null)
                {
                    ship.ApprovalRequest = user.UserId;
                    ship.ModifiedOn = currentDate;
                    ship.ModifiedBy = user.UserId;
                    ship.StatusShipping_StatusShippingId = approval;
                    ship.NotifyText = text;
                    ship.NotifyType = Helper.Notification.Error;
                    TimeLine tl = new TimeLine
                    {
                        Name = "הזמנה לא מאושרת" + "של " + user.FullName + " (" + user.EmpId + ")",
                        Desc = text,
                        CreatedBy = userid,
                        CreatedOn = currentDate,
                        ModifiedBy = userid,
                        ModifiedOn = currentDate,
                        TimeLineId = Guid.NewGuid(),
                        IsActive = true,
                        Status = TimeStatus.CancelByAdmin,
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