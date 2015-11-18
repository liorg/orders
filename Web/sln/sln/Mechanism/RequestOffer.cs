using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism
{
  
    /// <summary>
    /// The 'RequestOffer' class
    /// </summary>
    internal class RequestOffer : Handler
    {
        public RequestOffer(IShipComapnyRepository shipComapnyRepository, INotificationRepository notificationRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(shipComapnyRepository, notificationRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }
        public override async Task HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == 1)
            {
                var ship = await _shippingRepository.GetShipIncludeItems(offer.Id); //context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == offer.Id);
                var managerShip = await _shipComapnyRepository.GetAsync(offer.ShippingCompanyId);

                await SendNotification(offer, ship, managerShip, user);
                OfferLogic logic = new OfferLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep);

                logic.Create(offer, user, ship, managerShip);
                //request.RequestShippingId
                
                //offer.StateCode = 2;
            }
            else if (successor != null)
            {
                await successor.HandleRequest(offer,user);
            }
        }

        async Task SendNotification(OfferUpload offer, Shipping ship, ShippingCompany managerShip, UserContext user)
        {
            var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
            var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();
            var orderName = ship.Name;
            //NotificationManager manager = new NotificationManager();
            var notifyItem = new NotifyItem
            {
                Title = "הזמנה חדשה",
                Body = " בקשת אישור הזמנה עבור " + orderName,
                Url = url + path
            };
            await _notificationRepository.SendAsync(user.UserId, notifyItem);
            await _notificationRepository.SendAsync(managerShip.ManagerId, notifyItem);
        }
    }

}