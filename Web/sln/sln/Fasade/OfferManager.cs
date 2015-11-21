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

        public async Task<Result> CommitAsync(ApplicationDbContext context, OfferUpload offer, UserContext user)
        {
            var response = new Result();
            try
            {
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
                Handler requestOffer = new RequestOffer(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
                Handler commitOffer = new CommitOffer(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
                requestOffer.SetSuccessor(commitOffer);
                var messages = await requestOffer.HandleRequest(offer, user);

                await context.SaveChangesAsync();
                if (messages != null && messages.Users != null && messages.NotifyItem != null && messages.Users.Any())
                {
                    NotificationManager manager = new NotificationManager();
                    await manager.SendItemsAsync(context, messages);
                }
            
            }
            catch (Exception e)
            {
                response.IsError = true;
                response.ErrDesc = "נוצרה שגיאה";
                response.ErrCode = e.Message;
            }
            return response;
           
        }

        public async Task<Result> CancelAsync(ApplicationDbContext context, OfferUpload offer, UserContext user)
        {
            var response = new Result();
            try
            {
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
                Handler cancelClient = new CanceClient(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
                Handler cancelOffer = new CancelOffer(shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo);
                cancelClient.SetSuccessor(cancelOffer);
                var messages = await cancelClient.HandleRequest(offer, user);

                await context.SaveChangesAsync();
                if (messages != null && messages.Users != null && messages.NotifyItem != null && messages.Users.Any())
                {
                    NotificationManager manager = new NotificationManager();
                    await manager.SendItemsAsync(context, messages);
                }

            }
            catch (Exception e)
            {
                response.IsError = true;
                response.ErrDesc = "נוצרה שגיאה";
                response.ErrCode = e.Message;
            }
            return response;

        }
    }
}