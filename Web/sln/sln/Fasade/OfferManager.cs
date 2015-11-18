using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Fasade
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
        protected Handler successor;
        public Handler(IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep)
        {
            _offerRepository = offerRepository; _shippingRepository = shippingRepository; _offerPrice = offerPrice; _orgDetailRep = orgDetailRep;
        }
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(int status);
    }


    /// <summary>
    /// The 'RequestOffer' class
    /// </summary>
    class RequestOffer : Handler
    {
        public RequestOffer(IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep):
            base(offerRepository,shippingRepository,offerPrice,orgDetailRep)
        {
           
        }
        public override void HandleRequest(int status)
        {
            if (status==1)
            {
                
            }
            else if (successor != null)
            {
                successor.HandleRequest(status);
            }
        }
    }

    class CommitOffer : Handler
    {
        public CommitOffer(IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep) :
            base(offerRepository, shippingRepository, offerPrice, orgDetailRep)
        {

        }
        public override void HandleRequest(int status)
        {
            if (status == 1)
            {

            }
            else if (successor != null)
            {
                successor.HandleRequest(status);
            }
        }
    }
    public class OfferManager
    {
        public OfferManager()
        {

        }

        public void Excute(ApplicationDbContext context ){
            IOfferRepository offerRepository = new OfferRepository(context);
            IShippingRepository shippingRepository = new ShippingRepository(context);
            GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
            Handler requestOffer = new RequestOffer(offerRepository, shippingRepository, generalRepo, generalRepo);
            Handler commitOffer = new CommitOffer(offerRepository, shippingRepository, generalRepo, generalRepo);
            requestOffer.SetSuccessor(commitOffer);
         
        }
        //public async Task CreateOffer()
        //{

        //}
        //public async Task Dismiss()
        //{

        //}
        //public async Task Commit()
        //{

        //}

    }
}