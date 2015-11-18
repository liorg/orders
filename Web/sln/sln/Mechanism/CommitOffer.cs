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
    internal class CommitOffer : Handler
    {
        public CommitOffer(IShipComapnyRepository shipComapnyRepository, INotificationRepository notificationRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(shipComapnyRepository, notificationRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }
        public override async Task HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == 2)
            {

            }
            else if (successor != null)
            {
                await successor.HandleRequest(offer,user);
            }
        }
    }
}