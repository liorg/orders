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

        //public async Task<ActionResult> Index(int offerType, int state)//state=>1=New,2=InProccess, 3=End,4=Cancel
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        List<OfferVm> offers = new List<OfferVm>();
        //        var user = new UserContext(AuthenticationManager);
        //        Guid orgId = Guid.Empty;
        //        MemeryCacheDataService cache = new MemeryCacheDataService();

        //        orgId = cache.GetOrg(context);
        //        return View(offers);
        //    }
        //}

        public async Task<ActionResult> CreateOrder(Guid shipId)
        {
            using (var context = new ApplicationDbContext())
            {
                OrderDetail order = new OrderDetail();
                var user = new UserContext(AuthenticationManager);
                MemeryCacheDataService cache = new MemeryCacheDataService();
                var ship = await context.Shipping.Include(s => s.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
                order.Id = shipId;
                order.OfferId = Guid.Empty;
                order.Name = ship.Name;
                order.Title = "בקשת הזמנה";
                var org = cache.GetOrgEntity(context);
                var companies = cache.GetShippingCompaniesByOrgId(context, org.OrgId);
                if (companies.Any())
                {
                    order.ShippingCompanyId = companies.First().ShippingCompanyId;
                }


                order.ShippingItems = new List<ShippingItemVm>();
                foreach (var shipItem in ship.ShippingItems)
                    order.ShippingItems.Add(new ShippingItemVm { ProductName = shipItem.Product.Name, Total = Convert.ToInt32(shipItem.Quantity) });

                order.SigTypeText = "ללא חזרה";
                if (ship.SigBackType.HasValue)
                    order.SigTypeText = cache.GetBackOrder().Where(ds => ds.Key == ship.SigBackType.Value).Select(s => s.Value).FirstOrDefault();

                order.DirectionText = cache.GetDirection().Where(d => d.Key == ship.Direction).Select(s => s.Value).FirstOrDefault();
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



                Guid orgId = cache.GetOrg(context);
                return View(order);
            }
        }

        public async Task<ActionResult> OrderItem(Guid shipId)
        {
            using (var context = new ApplicationDbContext())
            {
                OrderDetail order = new OrderDetail();
                var user = new UserContext(AuthenticationManager);
                //  MemeryCacheDataService cache = new MemeryCacheDataService();
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo);

                var ship = await logic.GetShipAsync(shipId); //context.Shipping.Include(s => s.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
                order.Id = shipId;
                order.OfferId = ship.OfferId.GetValueOrDefault();
                var offer = await logic.GetOfferAsync(order.OfferId);
                if (offer != null)
                {
                    order.StateCode = offer.StatusCode;
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



                Guid orgId = generalRepo.GetOrg();
                return View(order);
            }
        }
    }
}