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
        protected readonly INotificationRepository _notificationRepository;
        protected readonly IShipComapnyRepository _shipComapnyRepository;
        protected Handler successor;
        public Handler(IShipComapnyRepository shipComapnyRepository, INotificationRepository notificationRepository, IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep)
        {
            _shipComapnyRepository = shipComapnyRepository;
            _notificationRepository = notificationRepository;
            _offerRepository = offerRepository; _shippingRepository = shippingRepository; _offerPrice = offerPrice; _orgDetailRep = orgDetailRep;
        }
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract Task HandleRequest(OfferUpload offer, UserContext user);
    }

}