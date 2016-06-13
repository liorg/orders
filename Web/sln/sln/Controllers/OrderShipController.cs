using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Michal.Project.Bll;
using System.Linq.Expressions;
using Michal.Project.Models.View;
using Michal.Project.Contract.DAL;
using Michal.Project.Fasade;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Dal;
using Michal.Project.Agent;
using System.Threading.Tasks;

namespace Michal.Project.Controllers
{
    public class OrderShipController : Controller
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

        public async Task<ActionResult> Create()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Distance> distances = new List<Distance>();
                UserContext userContext = new UserContext(AuthenticationManager);
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);

                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                var model = await logic.OnPreCreateShip(userContext);

                var shiptypes = generalRepo.GetShipType();

                ViewBag.OrderNumber = model.Name;

                var orgs = generalRepo.GetOrgs();
                var sigBacks = generalRepo.GetBackOrder();
                var directions = generalRepo.GetDirection();

                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                ViewBag.ShipTypes = new SelectList(shiptypes, "ShipTypeId", "Name");
                ViewBag.SigBacks = new SelectList(sigBacks, "Key", "Value");
                ViewBag.Directions = new SelectList(directions, "Key", "Value");

                var org = generalRepo.GetOrgEntity();
                var organid = org.OrgId;
                generalRepo.GetDistancesPerOrg(organid);
                distances = generalRepo.GetDistancesPerOrg(organid);
                model.OrgId = organid;

                ViewBag.Distance = new SelectList(distances, "DistanceId", "Name");
                return View(model);
            }
        }

    }
}