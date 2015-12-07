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
using Michal.Project.Models.View;
using Michal.Project.Contract.DAL;
using Michal.Project.Agent;


namespace Michal.Project.Controllers
{
    [Authorize]
    public class OfferController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Calc()
        {
            OrderDetail order = new OrderDetail();
            order.Id = Guid.Empty;
            order.Name = "מחשבון דמו";
            order.OfferId = Guid.Empty;
            DefaultShip logic = new DefaultShip();
            var company = logic.items.Where(t => t.Item1 == DefaultShip.DType.DefaultCompany).FirstOrDefault();
            order.ShippingCompanyId = company.Item3;
            return View(order);
        }

        public async Task<ActionResult> OrderItem(Guid shipId)
        {
            using (var context = new ApplicationDbContext())
            {
                OrderDetail order = new OrderDetail();
                var user = new UserContext(AuthenticationManager);
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IUserRepository userRepository = new UserRepository(context);
                ILocationRepository locationRepository = new LocationRepository(context, new GoogleAgent());
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, locationRepository);

                var ship = await logic.GetShipAsync(shipId); 
                order.Id = shipId;
                order.OfferId = ship.OfferId.GetValueOrDefault();
                var offer = await logic.GetOfferAsync(order.OfferId);
                if (offer != null)
                {
                    order.StateCode = offer.StatusCode;
                   
                  //  order.ad
                }
                else
                {
                    order.StateCode = (int)OfferVariables.OfferStateCode.New;
                }
                order.Name = ship.Name;

                order.Title = logic.GetTitle(offer);
                var org = generalRepo.GetOrgEntity();
                var companies = generalRepo.GetShippingCompaniesByOrgId(org.OrgId);
                if (companies.Any())
                {
                    order.ShippingCompanyId = companies.First().ShippingCompanyId;
                }

                order.ShippingItems = new List<ShippingItemVm>();
                foreach (var shipItem in ship.ShippingItems)
                    order.ShippingItems.Add(new ShippingItemVm { ProductName = shipItem.Product.Name, Total = Convert.ToInt32(shipItem.Quantity) });

                order.SigTypeText = "ללא חזרה";
                if (ship.SigBackType.HasValue)
                    order.SigTypeText = generalRepo.GetBackOrder().Where(ds => ds.Key == ship.SigBackType.Value).Select(s => s.Value).FirstOrDefault();

                order.DirectionText = generalRepo.GetDirection().Where(d => d.Key == ship.Direction).Select(s => s.Value).FirstOrDefault();
                order.DistanceText = ship.Distance != null ? ship.Distance.Name : "";
                order.ShipTypeText = ship.ShipType != null ? ship.ShipType.Name : "";

                order.TargetAddress = new AddressEditorViewModel();
                order.TargetAddress.City = ship.Target.CityName;
                order.TargetAddress.Citycode = ship.Target.CityCode;
                order.TargetAddress.ExtraDetail = ship.Target.ExtraDetail;
                order.TargetAddress.Num = ship.Target.StreetNum;
                order.TargetAddress.Street = ship.Target.StreetName;
                order.TargetAddress.Streetcode = ship.Target.StreetCode;


                order.SourceAddress = new AddressEditorViewModel();
                order.SourceAddress.City = ship.Source.CityName;
                order.SourceAddress.Citycode = ship.Source.CityCode;
                order.SourceAddress.ExtraDetail = ship.Source.ExtraDetail;
                order.SourceAddress.Num = ship.Source.StreetNum;
                order.SourceAddress.Street = ship.Source.StreetName;
                order.SourceAddress.Streetcode = ship.Source.StreetCode;

                order.Approval =await logic.GetApproval(ship);
                order.ApprovalPriceException = await logic.GetApprovalException(ship);
                order.Creator = await logic.GetCreator(ship);
                order.ApprovalShipping = await logic.GetApprovalShip(ship);

                Guid orgId = generalRepo.GetOrg();
                return View(order);
            }
        }
    }
}