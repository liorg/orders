using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using Michal.Project.Models.Status;
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
        public RequestOffer(IBussinessClosureRepository bussinessClosureRepository, ISlaRepository slaRepository, IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(bussinessClosureRepository,slaRepository,
            shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }

        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == (int)OfferVariables.OfferStateCode.New)
            {
                var ship = await _shippingRepository.GetShipIncludeItems(offer.Id); //context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == offer.Id);
                var managerShip = await _shipComapnyRepository.GetAsync(offer.ShippingCompanyId);
                HashSet<Guid> users = new HashSet<Guid>();
                if (ship.OwnerId.HasValue)
                {
                    users.Add(ship.OwnerId.Value);
                }
                if (managerShip.ManagerId != null && managerShip.ManagerId.Value == Guid.Empty)
                {

                    users.Add(managerShip.ManagerId.Value);
                }
                users.Add(user.UserId);

                OrderLogic logic = new OrderLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic(_shippingRepository);
                statusLogic.ApprovalRequest2(request);

                logic.Create(offer, user, ship, managerShip);

                FollowByLogic follow = new FollowByLogic(_shippingRepository);
                foreach (var userID in users)
                {
                    await follow.AddOwnerFollowBy(ship, userID);
                }

                var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
                var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();

                var titleMessage = "הזמנה חדשה";
                var bodyMessage = " בקשת אישור הזמנה עבור " + ship.Name;
                var urlMessage = url + path;
                return await SetNotification(users, urlMessage, titleMessage, bodyMessage);
            }
            else if (successor != null)
            {
                return await successor.HandleRequest(offer, user);
            }
            return await Task.FromResult<MessageForUsers>(null);
        }
    }

}