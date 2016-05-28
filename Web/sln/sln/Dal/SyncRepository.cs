using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Michal.Project.Helper;
using Michal.Project.Contract.View;
namespace Michal.Project.Dal
{
    public class SyncRepository : ISyncRepository
    {
        ApplicationDbContext _context;

        public SyncRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SyncTable>> GetSyn(Guid userId, Guid objectId, int objectTableCode)
        {
           return await _context.SyncTable.Where(d => d.UserId == userId && d.SyncStatus==SyncStatus.SyncFromServer && d.ObjectId == objectId && d.ObjectTableCode == objectTableCode).ToListAsync();
        }



        public async Task DeleteUnused(ISyncItem sync)
        {
           var noTable= ObjectTableCode.NONE;
            var items = await _context.SyncTable.Where(d => d.UserId == sync.UserId 
                 && (sync.DeviceId == null  ||sync.DeviceId == "" || d.DeviceId==sync.DeviceId)
                 && (sync.ClientId == null || sync.ClientId == "" || d.ClientId == sync.ClientId)
                 && (sync.ObjectTableCode == noTable || d.ObjectTableCode == sync.ObjectTableCode)
                 && (sync.ObjectId == Guid.Empty || d.ObjectId == sync.ObjectId)
                 ).ToListAsync();
           foreach (var item in items)
           {
               _context.Entry<SyncTable>(item).State = EntityState.Deleted;
           }
        }


        public async Task SyncOn(Contract.View.ISyncItem sync)
        {
            var items = await _context.SyncTable.Where(d => d.UserId == sync.UserId && 
                d.ObjectId == sync.ObjectId && d.ObjectTableCode == sync.ObjectTableCode).ToListAsync();
           // if (!items.Any())
            {
                _context.SyncTable.Add(new SyncTable
                {
                    ClientId = sync.ClientId,
                    UserId = sync.UserId,
                    DeviceId = sync.DeviceId,
                    IsActive = true,
                    LastUpdateRecord = sync.LastUpdateRecord,
                    ObjectId = sync.ObjectId,
                    ObjectTableCode = sync.ObjectTableCode,
                    SyncStateRecord = sync.SyncStateRecord,
                    SyncStatus = sync.SyncStatus,
                    SyncTableId = Guid.NewGuid()
                });
            }
        }
    }
}

