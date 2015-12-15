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
        
        public async Task AddOwner(Shipping ship, Guid userid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.ToString());
           
            ship.FollowsBy.Add(user);
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
    
        public void Add(Shipping ship)
        {
            _context.Shipping.Add(ship);
        }

        public async Task<Shipping> GetShipIncludeFollowsUsers(Guid shipId)
        {
            return await _context.Shipping.Include(ic => ic.FollowsBy).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
      
        }

        public async Task<XbzCounter> GetCounter(Guid orgid)
        {
           
            XbzCounter counter = null;
            long increa = 0;
            counter = await _context.XbzCounter.Where(o=>o.Organizations_OrgId.HasValue&& o.Organizations_OrgId.Value==orgid).Take(1).OrderByDescending(o => o.LastNumber).FirstOrDefaultAsync();
            if (counter != null)
            {
                increa = counter.LastNumber;
                increa++;
                counter.LastNumber = increa;
                _context.Entry<XbzCounter>(counter).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                counter = new XbzCounter();
                counter.XbzCounterId = Guid.NewGuid();
                counter.IsActive = true;
                counter.Organizations_OrgId = orgid;
                counter.LastNumber = increa++;
                _context.Entry<XbzCounter>(counter).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
            return counter;
        }

        public async Task<IEnumerable<ShippingItem>> GetShipitems(Guid shipId)
        {
            var shippingItems = await _context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product != null && s.Product.IsCalculatingShippingInclusive == false).ToListAsync();
            return shippingItems;   
        }
    }
}