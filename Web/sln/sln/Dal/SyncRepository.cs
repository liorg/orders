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



        public async Task DeleteUnused(Contract.View.ISync sync)
        {
           var items= await _context.SyncTable.Where(d => d.UserId == sync.CurrentUserId).ToListAsync();
           foreach (var item in items)
           {
               _context.Entry<SyncTable>(item).State = EntityState.Deleted;
           }
        }
    }
}

