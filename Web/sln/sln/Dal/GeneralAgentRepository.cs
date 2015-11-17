using Michal.Project.Contract.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Dal
{
    public class GeneralAgentRepository : IOfferPriceRepostory, IOrgDetailRepostory
    {
        ApplicationDbContext _context;
        
        public GeneralAgentRepository(ApplicationDbContext context)
        {
            _context = context;//ok
           
        }

      

        public List<DataModel.Discount> GetDiscount()
        {
            MemeryCacheDataService memory=new MemeryCacheDataService();
            return memory.GetDiscount(_context);
        }

        public List<DataModel.PriceList> GetPriceList()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetPriceList(_context);
        }

        public List<DataModel.ProductSystem> GetProductsSystem()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetProductsSystem(_context);
        }

      

        public List<DataModel.Distance> GetDistancesPerOrg(Guid orgId)
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetDistancesPerOrg(_context, orgId);
        }

        public List<DataModel.ShippingCompany> GetShippingCompaniesByOrgId(Guid orgId)
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetShippingCompaniesByOrgId(_context, orgId);
        }

        public List<DataModel.Organization> GetOrgs()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetOrgs(_context);
        }

        public List<DataModel.ShipType> GetShipType()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetShipType(_context);
        }

        public Guid GetOrg()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetOrg(_context);
        }

        public DataModel.Organization GetOrgEntity()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetOrgEntity(_context);
        }


        public List<DataModel.Product> GetProducts(Guid orgId)
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetProducts(_context, orgId);
        }
    }
}