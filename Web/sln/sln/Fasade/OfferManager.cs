using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.Mechanism;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Fasade
{



    public class OfferManager
    {
        public OfferManager()
        {

        }

        public async Task CommitAsync(ApplicationDbContext context, OfferUpload offer, UserContext user)
        {
            IOfferRepository offerRepository = new OfferRepository(context);
            IShippingRepository shippingRepository = new ShippingRepository(context);
            GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
            IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
            Handler requestOffer = new RequestOffer(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
            Handler commitOffer = new CommitOffer(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
            requestOffer.SetSuccessor(commitOffer);
            await context.SaveChangesAsync();

            var messages = await requestOffer.HandleRequest(offer, user);
            if (messages != null && messages.Users != null && messages.NotifyItem != null && messages.Users.Any())
            {
                NotificationManager manager = new NotificationManager();
                await manager.SendItemsAsync(context, messages);
            }
        }
    }
}