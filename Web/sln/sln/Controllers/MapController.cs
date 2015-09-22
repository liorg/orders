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


namespace Michal.Project.Controllers
{
    public class MapController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Show(string id)
        {
            OrderViewBase orderModel = new OrderViewBase();
            using (var context = new ApplicationDbContext())
            {
                var user = new UserContext(AuthenticationManager);
                Guid shipId = Guid.Parse(id);
                var shipping = await context.Shipping.FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
                orderModel.ShippingVm = new ShippingVm();
                orderModel.ShippingVm.Id = shipId;
                orderModel.ShippingVm.Name = shipping.Name;
                orderModel.ShippingVm.NameSource = shipping.NameSource;
                orderModel.ShippingVm.NameTarget = shipping.NameTarget;
                orderModel.ShippingVm.TelSource = shipping.TelSource;
                orderModel.ShippingVm.TelTarget = shipping.TelTarget;
                orderModel.ShippingVm.Recipient = shipping.Recipient;

                orderModel.ShippingVm.SourceAddress = new AddressEditorViewModel();
                orderModel.ShippingVm.SourceAddress.City = shipping.Source.CityName;
                orderModel.ShippingVm.SourceAddress.Street = shipping.Source.StreetName;
                orderModel.ShippingVm.SourceAddress.Num = shipping.Source.StreetNum;
                orderModel.ShippingVm.SourceAddress.ExtraDetail = shipping.Source.ExtraDetail;

                orderModel.ShippingVm.TargetAddress = new AddressEditorViewModel();
                orderModel.ShippingVm.TargetAddress.City = shipping.Target.CityName;
                orderModel.ShippingVm.TargetAddress.Street = shipping.Target.StreetName;
                orderModel.ShippingVm.TargetAddress.Num = shipping.Target.StreetNum;
                orderModel.ShippingVm.TargetAddress.ExtraDetail = shipping.Target.ExtraDetail;

                orderModel.Location = new Location();
                orderModel.Location.TargetLat = shipping.Target.Lat;
                orderModel.Location.TargetLng = shipping.Target.Lng;
                orderModel.Location.TargetName = "כתובת המקבל:" + orderModel.ShippingVm.TargetAddress.ToString();

                orderModel.Location.SourceLat = shipping.Source.Lat;
                orderModel.Location.SourceLng = shipping.Source.Lng;
                orderModel.Location.SourceName = "כתובת השולח:" + orderModel.ShippingVm.SourceAddress.ToString();

            }
            return View(orderModel);
        }  
    }
}