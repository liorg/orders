using Michal.Project.DataModel;
using Michal.Project.Models;
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
        Task AddOwnerFollowBy(Shipping ship, Guid userid);
        Task AddOwner(Shipping ship, Guid userid);
        void Update(Shipping ship);
        Task<Shipping> GetShipIncludeFollowsUsers(Guid shipId);
        void Add(Shipping ship);
        Task<XbzCounter> GetCounter(Guid orgid);
        Task<IEnumerable<ShippingItem>> GetShipitems(Guid shipId);
        Task<IEnumerable<ShippingVm>> GetShippingByUserId(Guid userId);
        Task<Shipping> GetShipTimelines(Guid shipId);
    }
}
