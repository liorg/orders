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
    /// <summary>
    /// The 'Handler' abstract class
    /// </summary>
    abstract class Handler
    {
        protected readonly IOfferRepository _offerRepository;
        protected readonly IShippingRepository _shippingRepository;
        protected readonly IOfferPriceRepostory _offerPrice;
        protected readonly IOrgDetailRepostory _orgDetailRep;
        protected readonly IShipComapnyRepository _shipComapnyRepository;
        protected readonly IBussinessClosureRepository _bussinessClosureRepository;
        protected readonly ISlaRepository _slaRepository;
        protected readonly bool _isUserGrant;
        protected readonly IUserRepository _userRepository;
        protected Handler successor;
        public Handler(IBussinessClosureRepository bussinessClosureRepository,ISlaRepository slaRepository,
            
            IShipComapnyRepository shipComapnyRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep,
            IUserRepository userRepository,bool isUserGrant=false)
        {
            _bussinessClosureRepository = bussinessClosureRepository;
            _slaRepository = slaRepository;
            _shipComapnyRepository = shipComapnyRepository;
            _offerRepository = offerRepository; _shippingRepository = shippingRepository; _offerPrice = offerPrice; _orgDetailRep = orgDetailRep;
            _userRepository = userRepository;
            _isUserGrant = isUserGrant;
        }
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract Task<MessageForUsers> HandleRequest(OfferUpload offer, UserContext user);

        public async Task<MessageForUsers> SetNotification(IEnumerable<Guid> users, string url, string title, string body,string messageClient)
        {
            MessageForUsers result = new MessageForUsers();
            result.MessageClient = messageClient;
            result.NotifyItem = new NotifyItem
            {
                Title = title,
                Body = body,
                Url = url
            };
            result.Users = users.ToList();
            return await Task.FromResult<MessageForUsers>(result);
        }
    }

}