using Microsoft.Owin.Security;
using sln.Bll;
using sln.Dal;
using sln.DataModel;
using sln.Helper;
using sln.Models;
using sln.Models.Status;
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
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.RemoveOrder(request);

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

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.ApprovalRequest(request);

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
                Func<string,string> func=(assig=> cache.GetRunners(context).Where(run => run.Id == assig).Select(run2 => run2.FullName).FirstOrDefault());

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                request.AssignTo = assignTo;

                StatusLogic statusLogic = new StatusLogic();
                statusLogic.ConfirmRequest(request, func);

                //var grantToText = "";
                //if (!String.IsNullOrEmpty(assignTo))
                //{
                //    ship.GrantRunner = Guid.Parse(assignTo);
                //    grantToText = cache.GetRunners(context).Where(run => run.Id == assignTo).Select(run2 => run2.FullName).FirstOrDefault();

                //}
                //else
                //{
                //    ship.GrantRunner = userid;
                //    grantToText = user.FullName;
                //}
                //var currentDate = DateTime.Now;
                //if (ship != null)
                //{
                //    var title = "הזמנה אושרה ע'' חברת השליחות" + " ע''י " + user.FullName + " (" + user.EmpId + ")";
                //    var text = title +System.Environment.NewLine+" " + "הזמנה אושרה " + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm") + " והועברה לשליח" + " " + grantToText;
                //    ship.ApprovalRequest = user.UserId;
                //    ship.ModifiedOn = currentDate;
                //    ship.ModifiedBy = user.UserId;
                //    ship.StatusShipping_StatusShippingId = approval;
                //    ship.ApprovalShip = userid;
                //    ship.NotifyText = text;
                //    ship.NotifyType = Helper.Notification.Info;
        
                //    TimeLine tl = new TimeLine
                //    {
                //        Name = title,
                //        Desc = text,
                //        CreatedBy = userid,
                //        CreatedOn = currentDate,
                //        ModifiedBy = userid,
                //        ModifiedOn = currentDate,
                //        TimeLineId = Guid.NewGuid(),
                //        IsActive = true,
                //        Status = TimeStatus.Confirm,
                //        StatusShipping_StatusShippingId = approval
                //    };
                //    ship.TimeLines.Add(tl);
                //}
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

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Accept(request);

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

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Accept(request);

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

        public async Task<ActionResult> Arrived(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Arrived(request);

                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("ShipView", "S", new  { id = id });
            }
        }

        public async Task<ActionResult> TakeOk(string id,string recipient,string desc)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Take(request, desc, recipient);

                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }

        public async Task<ActionResult> NoTake(string id, string desc)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.FindAsync(shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Arrived(request);

                context.Entry<Shipping>(ship).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "S");
            }
        }
    }
}