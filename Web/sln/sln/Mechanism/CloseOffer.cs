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
    internal class CloseOffer : Handler
    {
        public CloseOffer(IBussinessClosureRepository bussinessClosureRepository, ISlaRepository slaRepository, IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep, IUserRepository userRepository, ILocationRepository locationRepostory, bool isUserGrant) :
            base(bussinessClosureRepository, slaRepository,
             shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep,userRepository,locationRepostory , isUserGrant)
        {

        }

        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == (int)OfferVariables.OfferStateCode.Close)
            {
                var messageClient = "";

                var ship = await _shippingRepository.GetShipIncludeItems(offer.Id); //context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == offer.Id);
                var offerModel = await _offerRepository.GetOfferAndHisChilds(offer.OfferId);
                FollowByLogic follow = new FollowByLogic(_shippingRepository);
                OrderLogic logic = new OrderLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep, _userRepository, _locationRepostory);
               
                HashSet<Guid> users = new HashSet<Guid>();
                var usersfollow = follow.GetUsersByShip(ship);
               
                logic.ChangeStatusOffer((int)OfferVariables.OfferStateCode.Close, offer, user, ship, offerModel);

                logic.Update(ship);
                var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
                var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();

                var titleMessage = "הזמנה  סגורה";
                var bodyMessage = " בקשת אישור הזמנה חריגה עבור " + ship.Name;
                var urlMessage = url + path;
                return await SetNotification(users, urlMessage, titleMessage, bodyMessage, messageClient);
            }
            else if (successor != null)
            {
                return await successor.HandleRequest(offer, user);
            }
            return await Task.FromResult<MessageForUsers>(null);
        }
    }

}