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

        public async Task<Result<OfferMessage>> CommitAsync(ApplicationDbContext context, OfferUpload offer, UserContext user, bool isUserGrant)
        {
            var response = new Result<OfferMessage>();
            try
            {
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
                IUserRepository userRepository = new UserRepository(context);
                Handler requestOffer = new RequestOffer(generalRepo, generalRepo, shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, isUserGrant);
                Handler commitOffer = new CommitOffer(generalRepo, generalRepo, shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, isUserGrant);
                Handler escalationOffer = new EscalationOffer(generalRepo, generalRepo, shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo, userRepository, isUserGrant);

                requestOffer.SetSuccessor(escalationOffer);
                //  commitOffer.SetSuccessor(escalationOffer);
                escalationOffer.SetSuccessor(commitOffer);
                var messages = await requestOffer.HandleRequest(offer, user);

                await context.SaveChangesAsync();
                if (messages != null && messages.Users != null && messages.NotifyItem != null && messages.Users.Any())
                {
                    response.Model = new OfferMessage();
                    response.Model.MessageClient = messages.MessageClient;
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

        public async Task<Result<OfferMessage>> CancelAsync(ApplicationDbContext context, OfferUpload offer, UserContext user)
        {
            var response = new Result<OfferMessage>();
            try
            {
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                IShipComapnyRepository shipComapnyRepository = new ShipComapnyRepository(context);
                IUserRepository userRepository = new UserRepository(context);
                Handler cancelClient = new CanceClient(generalRepo, generalRepo, shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo, userRepository);
                Handler cancelOffer = new CancelOffer(generalRepo, generalRepo, shipComapnyRepository, offerRepository, shippingRepository, generalRepo, generalRepo, userRepository);
                cancelClient.SetSuccessor(cancelOffer);
                var messages = await cancelClient.HandleRequest(offer, user);

                await context.SaveChangesAsync();
                if (messages != null && messages.Users != null && messages.NotifyItem != null && messages.Users.Any())
                {
                    response.Model = new OfferMessage();
                    response.Model.MessageClient = messages.MessageClient;
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