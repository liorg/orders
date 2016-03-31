using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Michal.Project.Models;
using Michal.Project.Helper;
using Michal.Project.Contract.View;
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
            var ship = await GetShip(shipId);
            ship.ShippingCompany_ShippingCompanyId = companyShip;
            ship.OfferId = offerId;
            ship.ModifiedBy = modifiedId;
            ship.ModifiedOn = DateTime.Now;
        }

        public async Task<Shipping> GetShip(Guid shipId)
        {
            return await _context.Shipping.FindAsync(shipId);
        }

        public async Task<Shipping> GetShipTimelines(Guid shipId)
        {
            return await _context.Shipping.Include(tl => tl.TimeLines).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);

        }
        
        public async Task<Shipping> GetShipIncludeItems(Guid shipId)
        {
            return await _context.Shipping.Include(f=>f.FollowsBy).Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipId);
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
                ship = await _context.Shipping.Include(f => f.FollowsBy).Where(s => s.ShippingId == ship.ShippingId).FirstAsync();
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
            counter = await _context.XbzCounter.Where(o => o.Organizations_OrgId.HasValue && o.Organizations_OrgId.Value == orgid).Take(1).OrderByDescending(o => o.LastNumber).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<ShippingVm>> GetShippingByUserId(Guid userId)
        {

            var shippings = await (from s in _context.Shipping
                                   // join u in _context.Users
                                   // on s.OwnerId.Value.ToString() equals u.Id
                                   where s.GrantRunner.HasValue && s.IsInProccess && s.GrantRunner == userId
                                   orderby s.WalkOrder
                                   select new ShippingVm
                                   {
                                       ActualEndDateDt = s.ActualStartDate,
                                       ActualStartDateDt = s.ActualEndDate,
                                       SlaEndTime = s.SlaTime,
                                       DistanceName = s.Distance.Name,
                                       DistanceValue = s.DistanceText,
                                       Id = s.ShippingId,
                                       Name = s.Name,
                                       TelSource = s.TelSource,
                                       TelTarget = s.TelTarget,
                                       ShipTypeIdName = s.ShipType.Name,
                                       WalkOrder = s.WalkOrder,
                                       Status = s.StatusShipping.Desc,
                                       NameSource = s.NameSource,
                                       NameTarget = s.NameTarget,

                                       TargetAddress = new AddressEditorViewModel { Lat = s.Target.Lat, Lng = s.Target.Lng, City = s.Target.CityName, Citycode = s.Target.CityName, ExtraDetail = s.Target.ExtraDetail, Num = s.Target.StreetNum, Street = s.Target.StreetCode, Streetcode = s.Target.StreetName },
                                       SourceAddress = new AddressEditorViewModel { Lat = s.Source.Lat, Lng = s.Source.Lng, City = s.Source.CityName, Citycode = s.Source.CityName, ExtraDetail = s.Source.ExtraDetail, Num = s.Source.StreetNum, Street = s.Source.StreetCode, Streetcode = s.Source.StreetName }
                                   }).ToListAsync();
            return shippings;
        }

        public async Task<IEnumerable<ItemSync<MobileShipVm>>> GetShippingSyncByUserId(Guid userId,
            string deviceid, string clientid, bool isForceAll = true)
        {
            List<ItemSync<MobileShipVm>> items = new List<ItemSync<MobileShipVm>>();

            var shippings = await (from s in _context.Shipping
                                   // join u in _context.Users
                                   // on s.OwnerId.Value.ToString() equals u.Id
                                   where s.GrantRunner.HasValue && s.IsInProccess && s.GrantRunner == userId
                                   orderby s.WalkOrder
                                   select new MobileShipVm
                                   {
                                       ActualEndDateDt = s.ActualStartDate,
                                       ActualStartDateDt = s.ActualEndDate,
                                       SlaEndTime = s.SlaTime,
                                       Id = s.ShippingId,
                                       Name = s.Name,
                                       TelSource = s.TelSource,
                                       TelTarget = s.TelTarget,
                                       ShipTypeIdName = s.ShipType.Name,
                                       WalkOrder = s.WalkOrder,
                                       Status = s.StatusShipping.Desc,
                                       NameSource = s.NameSource,
                                       NameTarget = s.NameTarget,

                                       TargetAddress = new AddressEditorViewModel { Lat = s.Target.Lat, Lng = s.Target.Lng, City = s.Target.CityName, Citycode = s.Target.CityName, ExtraDetail = s.Target.ExtraDetail, Num = s.Target.StreetNum, Street = s.Target.StreetCode, Streetcode = s.Target.StreetName },
                                       SourceAddress = new AddressEditorViewModel { Lat = s.Source.Lat, Lng = s.Source.Lng, City = s.Source.CityName, Citycode = s.Source.CityName, ExtraDetail = s.Source.ExtraDetail, Num = s.Source.StreetNum, Street = s.Source.StreetCode, Streetcode = s.Source.StreetName }
                                   }).ToListAsync();
            foreach (var shipping in shippings)
            {
               
              SyncTable  syncRecord = new SyncTable
                    {
                        SyncStatus = SyncStatus.SyncFromServer,
                        ObjectId = shipping.Id,
                        ObjectTableCode = ObjectTableCode.SHIP,
                        UserId = userId,
                        SyncStateRecord = SyncStateRecord.No,
                        LastUpdateRecord = DateTime.Now
                    };

              items.Add(new ItemSync<MobileShipVm>
                {
                    Model = shipping,
                    LastUpdateRecord = syncRecord.LastUpdateRecord,
                    ObjectId = syncRecord.ObjectId,
                    ObjectTableCode = syncRecord.ObjectTableCode,
                    SyncStateRecord = syncRecord.SyncStateRecord,
                    SyncStatus = syncRecord.SyncStatus
                });
            }
            return items;
        }

        public void AddRecordTableAsync(Guid userid, ISyncItem syncRequest)
        {
            SyncTable tablSync = new SyncTable();
            tablSync.SyncTableId = Guid.NewGuid();
            tablSync.LastUpdateRecord = syncRequest.LastUpdateRecord;
            tablSync.ObjectId = syncRequest.ObjectId;
            tablSync.ObjectTableCode = syncRequest.ObjectTableCode;
            tablSync.SyncStateRecord = syncRequest.SyncStateRecord;
            tablSync.SyncStatus = syncRequest.SyncStatus;
            tablSync.CreatedOn = DateTime.Now;
            tablSync.ModifiedOn = DateTime.Now;
            tablSync.UserId = userid;
            _context.SyncTable.Add(tablSync); 
        }


    }
}