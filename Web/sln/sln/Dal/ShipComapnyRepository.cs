using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Michal.Project.Dal
{
    public class ShipComapnyRepository : IShipComapnyRepository
    {
        ApplicationDbContext _context;
        public ShipComapnyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShippingCompany> GetAsync(Guid companyId)
        {
            return await _context.ShippingCompany.FirstOrDefaultAsync(c => c.ShippingCompanyId == companyId);

        }
    }
}