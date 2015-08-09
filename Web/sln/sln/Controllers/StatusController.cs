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
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

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

                return RedirectToAction("Index", "s");
            }
        }

        public async Task<ActionResult> ApprovalRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var ship = await context.Shipping.Include(fb=>fb.FollowsBy).FirstOrDefaultAsync(s=>s.ShippingId== shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.ApprovalRequest(request);

                context.Entry<Shipping>(ship).State = EntityState.Modified;

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }

        public async Task<ActionResult> ConfirmRequest(string id, string assignTo)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
               // var ship = await context.Shipping.FindAsync(shipId);
                var ship = await context.Shipping.Include(fb => fb.FollowsBy).FirstOrDefaultAsync(s => s.ShippingId == shipId);

                Guid approval = Guid.Parse(Helper.Status.Confirm);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                Func<string, string> func = (assig => cache.GetRunners(context).Where(run => run.Id == assig).Select(run2 => run2.FullName).FirstOrDefault());

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                request.AssignTo = assignTo;

                StatusLogic statusLogic = new StatusLogic();
                statusLogic.ConfirmRequest(request, func);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                if (!String.IsNullOrEmpty(assignTo))
                {
                    await followLogic.AppendOwnerFollowBy(ship, new UserContext{ UserId =Guid.Parse(assignTo) }, context.Users); 
                }
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }

        public async Task<ActionResult> Accept(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                // var ship = await context.Shipping.FindAsync(shipId);
                var ship = await context.Shipping.Include(fb => fb.FollowsBy).FirstOrDefaultAsync(s => s.ShippingId == shipId);


                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Accept(request);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }

        public async Task<ActionResult> CancelRequest(string id)
        {
            using (var context = new ApplicationDbContext())
            {

                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                // var ship = await context.Shipping.FindAsync(shipId);
                var ship = await context.Shipping.Include(fb => fb.FollowsBy).FirstOrDefaultAsync(s => s.ShippingId == shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Accept(request);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }

       
        public async Task<ActionResult> Arrived(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                // var ship = await context.Shipping.FindAsync(shipId);
                var ship = await context.Shipping.Include(fb => fb.FollowsBy).FirstOrDefaultAsync(s => s.ShippingId == shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Arrived(request);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }

        [HttpPost]
        public async Task<ActionResult> TakeOk(string takeOkId, string recipient, string freeText)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(takeOkId);
                var ship = await context.Shipping.FindAsync(shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.Take(request, freeText, recipient);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }
        [HttpPost]
        public async Task<ActionResult> NoTake(string noTakeOkId, string desc)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid userid = Guid.Empty;
                UserContext user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(noTakeOkId);
                // var ship = await context.Shipping.FindAsync(shipId);
                var ship = await context.Shipping.Include(fb => fb.FollowsBy).FirstOrDefaultAsync(s => s.ShippingId == shipId);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic();
                statusLogic.NoTake(request, desc);

                FollowLogic followLogic = new FollowLogic();
                await followLogic.AppendOwnerFollowBy(ship, user, context.Users);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "F");
            }
        }
    }
}