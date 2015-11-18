using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface IOfferRepository
    {
        void Create(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips);
        Task ChangeStatusAsync(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips, bool deleteChildrens);

        Task<RequestShipping> GetOffer(Guid requestShippingId);
        Task<RequestShipping> GetOfferAndHisChilds(Guid requestShippingId);
    }
}
