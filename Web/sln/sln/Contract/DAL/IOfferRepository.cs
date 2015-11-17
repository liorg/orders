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
        Task Create(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips);
        Task ChangeStatus(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips, bool deleteChildrens);
    }
}
