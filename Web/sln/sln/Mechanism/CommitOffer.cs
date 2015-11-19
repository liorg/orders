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
        public CommitOffer(IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(shipComapnyRepository, offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }
        public override async Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user)
        {
            if (offer.StateCode == 2)
            {
                return await Task.FromResult<MessageForUsers>(null);
            }
            else if (successor != null)
            {
                return await successor.HandleRequest(offer,user);
            }
            return await Task.FromResult<MessageForUsers>(null);
        }
    }
}