using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
namespace Michal.Project.Dal
{
    public class ShippingRepository : IShippingRepository
    {
        ApplicationDbContext _context;
        public ShippingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNewOrder(Guid modifiedId, Guid shipId, Guid companyShip, Guid offerId, Guid statusId)
        {
            var ship=await GetShip(shipId);
            ship.ShippingCompany_ShippingCompanyId = companyShip;
            ship.OfferId = offerId;
            ship.ModifiedBy = modifiedId;
            ship.ModifiedOn = DateTime.Now;
        }

        public async Task<Shipping> GetShip(Guid shipId)
        {
            return await _context.Shipping.FindAsync(shipId);
        }

        public async Task<Shipping> GetShipIncludeItems(Guid shipId)
        {
            return await _context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
        }

        public async Task AddOwnerFollowBy(Shipping ship, Guid userid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.ToString());
            if (ship.FollowsBy == null || ship.FollowsBy.Count == 0)
            {
                ship= await _context.Shipping.Include(f => f.FollowsBy).Where(s => s.ShippingId == ship.ShippingId).FirstAsync();
            }
            ship.FollowsBy.Add(user);
        }

        public void Update(Shipping ship)
        {
            _context.Entry<Shipping>(ship).State = EntityState.Modified;
        }


        public async Task<Shipping> GetShipIncludeFollowsUsers(Guid shipId)
        {
            return await _context.Shipping.Include(ic => ic.FollowsBy).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
      
        }
    }
}