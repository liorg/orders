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
    internal class CommitOffer : Handler
    {
        public CommitOffer(IBussinessClosureRepository bussinessClosureRepository, ISlaRepository slaRepository, IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(bussinessClosureRepository, slaRepository,
           shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }
        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == (int)OfferVariables.OfferStateCode.Request)
            {
                var ship = await _shippingRepository.GetShipIncludeFollowsUsers(offer.Id);
                var offerModel = await _offerRepository.GetOfferAndHisChilds(offer.OfferId);

                FollowByLogic follow = new FollowByLogic(_shippingRepository);
                OrderLogic logic = new OrderLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep);
                CalcService sla = new CalcService(_bussinessClosureRepository, _slaRepository);

                var request = new StatusRequestBase();
                request.Ship = ship;
                request.UserContext = user;
                StatusLogic statusLogic = new StatusLogic(_shippingRepository);
                statusLogic.ConfirmRequest2(request);

                logic.ChangeStatusOffer((int)OfferVariables.OfferStateCode.End, offer, user, ship, offerModel);
                logic.SetCompanyHandler(ship, offer.ShippingCompanyId);
                sla.SetSla(ship);
                logic.Update(ship);

                var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
                var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();

                var titleMessage = "הזמנה אושרה";
                var bodyMessage = " בקשת ההזמנה אושרה עבור " + ship.Name;
                var urlMessage = url + path;
                var users = follow.GetUsersByShip(ship);

                return await SetNotification(users, urlMessage, titleMessage, bodyMessage);
            }
            else if (successor != null)
             return await successor.HandleRequest(offer, user);
            
            return await Task.FromResult<MessageForUsers>(null);
        }
    }
}