using Michal.Project.Contract.DAL;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class OfferLogic
    {
        IOfferRepository _offerRepository;
        public OfferLogic(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }
        public async Task Edit(OfferUpload request)
        {

            //if (request.StateCode == 1)
            //{
                
            //}
        }
    }
}