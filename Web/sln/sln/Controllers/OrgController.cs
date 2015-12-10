using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Michal.Project.Models;
using Michal.Project.Helper;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Michal.Project.Bll;
using Kipodeal.Helper.Cache;

namespace Michal.Project.Controllers
{
    // [Authorize]
    public class OrgController : Controller
    {

        public ActionResult CreateShipByOrg()
        {
            using (var context = new ApplicationDbContext())
            {
                ViewLogic view = new ViewLogic();
                //  UserContext user = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                ViewBag.Orgs = cache.GetOrgs(context).ToList();


            }
            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Details(Guid? orgid)
        {
            OrgVm orgVm = new OrgVm();

            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);

                var org = repository.GetOrgEntity();
                orgVm.Name = org.Name;
                orgVm.Id = org.OrgId;
                orgVm.PriceException = org.PriceValueException.GetValueOrDefault().ToString();
                orgVm.Address = new AddressEditorViewModel();
                orgVm.Address.City = org.AddressOrg.CityName;
                orgVm.Address.ExtraDetail = org.AddressOrg.ExtraDetail;
                orgVm.Address.Num = org.AddressOrg.StreetNum;
                orgVm.Address.Street = org.AddressOrg.StreetName;

                orgVm.Desc = String.IsNullOrWhiteSpace(org.Name) ? General.Empty : org.Name;
            }
            return View(orgVm);
        }

        public ActionResult GetDistances(Guid? orgid)
        {
            var distancesView = new DistancesView();

            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                var org = repository.GetOrgEntity();
                distancesView.Name = org.Name;
                distancesView.Id = org.OrgId;
                distancesView.Items = calc.GetDistancesItems(org.OrgId);

            }
            return View(distancesView);
        }
        public ActionResult GetProducts(Guid? orgid)
        {
            var distancesView = new ProductsView();
            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                var org = repository.GetOrgEntity();
                distancesView.Name = org.Name;
                distancesView.Id = org.OrgId;
                distancesView.Items = calc.GetProducts(org.OrgId);
            }
            return View(distancesView);
        }
    }
}