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

        public async Task ExcuteAsync(ApplicationDbContext context, OfferUpload offer, UserContext user)
        {

            IOfferRepository offerRepository = new OfferRepository(context);
            IShippingRepository shippingRepository = new ShippingRepository(context);
            GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
            INotificationRepository notificationRepository = new NotificationRepository(context);
            IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
            Handler requestOffer = new RequestOffer(shipComapnyRepository,notificationRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
            Handler commitOffer = new CommitOffer(shipComapnyRepository,notificationRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
            requestOffer.SetSuccessor(commitOffer);

            await requestOffer.HandleRequest(offer, user);
         
        }
    }
}