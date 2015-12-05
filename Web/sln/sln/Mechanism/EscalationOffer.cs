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
    internal class EscalationOffer : Handler
    {
        public EscalationOffer(IBussinessClosureRepository bussinessClosureRepository, ISlaRepository slaRepository, IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep, IUserRepository userRepository, bool isUserGrant) :
            base(bussinessClosureRepository, slaRepository,
             shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep,userRepository, isUserGrant)
        {

        }

        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == (int)OfferVariables.OfferStateCode.ConfirmException)
            {
                var messageClient = "";

                var ship = await _shippingRepository.GetShipIncludeItems(offer.Id); //context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == offer.Id);
                var offerModel = await _offerRepository.GetOfferAndHisChilds(offer.OfferId);
                FollowByLogic follow = new FollowByLogic(_shippingRepository);
                OrderLogic logic = new OrderLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep, _userRepository);
               
                HashSet<Guid> users = new HashSet<Guid>();
                var usersfollow = follow.GetUsersByShip(ship);
                if (!_isUserGrant)
                {
                    messageClient = "חובה מאשר הזמנה עבור הזמנה חריגה";
                    return await SetNotification(usersfollow, "", "חובה מאשר עבור הזמנה חריגה ", "חובה מאשר הזמנה חריג", messageClient);
                }
                logic.ChangeStatusOffer((int)OfferVariables.OfferStateCode.Request, offer, user, ship, offerModel);

                logic.SetApprovalPriceException(offer, user, ship, offerModel);
                logic.Update(ship);
                var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
                var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();

                var titleMessage = "הזמנה חריגה אושרה";
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