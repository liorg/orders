using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface IShippingRepository
    {
        Task CreateNewOrder(Guid modifiedId, Guid shipId, Guid companyShip, Guid offerId, Guid statusId);
        Task<Shipping> GetShip(Guid shipId);
        Task<Shipping> GetShipIncludeItems(Guid shipId);

        void Update(Shipping ship);
    }
}
