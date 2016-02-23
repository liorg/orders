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
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep, IUserRepository userRepository, ILocationRepository locationRepostory, bool isUserGrant) :
            base(bussinessClosureRepository, slaRepository,
           shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep,userRepository,locationRepostory, isUserGrant)
        {

        }
        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == (int)OfferVariables.OfferStateCode.Request)
            {
                var messageClient = "";
                
                var ship = await _shippingRepository.GetShipIncludeFollowsUsers(offer.Id);
                var offerModel = await _offerRepository.GetOfferAndHisChilds(offer.OfferId);
                var bodyMessage = " בקשת ההזמנה אושרה עבור " + ship.Name;
                var titleMessage = "הזמנה אושרה";
                HashSet<Guid> users = new HashSet<Guid>();
                FollowByLogic follow = new FollowByLogic(_shippingRepository);
                OrderLogic logic = new OrderLogic(_offerRepository, _shippingRepository, _offerPrice, _orgDetailRep, _userRepository, _locationRepostory);
                CalcService sla = new CalcService(_bussinessClosureRepository, _slaRepository,_orgDetailRep);
                var usersfollow = follow.GetUsersByShip(ship);
                foreach (var usrFollow in usersfollow)
                {
                    users.Add(usrFollow);
                }
                var isNeedConfirm = logic.IsNeedEsclationPrice(offer, ship, offerModel);
                if (isNeedConfirm)
                {
                    logic.SetEsclationStatus(offer, ship, offerModel);
                    messageClient = " מחייב אישור למחיר חריג";

                    if (user.GrantUserId.HasValue)
                        users.Add(user.GrantUserId.Value);
                    bodyMessage += " שים לב ! יש חובה לאשר מחיר חריג זה לפני ההזמנה";
                }
                else
                {
                    
                    logic.ChangeStatusOffer((int)OfferVariables.OfferStateCode.End, offer, user, ship, offerModel);
                    logic.SetCompanyHandler(ship, offer.ShippingCompanyId);
                    sla.SetSla(ship);

                    var request = new StatusRequestBase();
                    request.Ship = ship;
                    request.UserContext = user;
                    StatusLogic statusLogic = new StatusLogic(_shippingRepository);
                    statusLogic.ConfirmRequest2(request);

                }
                logic.Update(ship);

                var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
                var path = "/Offer/OrderItem?shipId=" + offer.Id.ToString();

              
              
                var urlMessage = url + path;


                return await SetNotification(users, urlMessage, titleMessage, bodyMessage, messageClient, offer.Id);
            }
            else if (successor != null)
             return await successor.HandleRequest(offer, user);
            
            return await Task.FromResult<MessageForUsers>(null);
        }
    }
}