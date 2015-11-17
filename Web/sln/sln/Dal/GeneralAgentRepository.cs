using Michal.Project.Contract.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Dal
{
    public class GeneralAgentRepository : IOfferPriceRepostory, IOrgDetailRepostory
    {
        public GeneralAgentRepository()
        {

        }

        public List<DataModel.Discount> GetDiscount(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.PriceList> GetPriceList(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Product> GetProducts(ApplicationDbContext context, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ProductSystem> GetProductsSystem(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Discount> GetDiscount()
        {
            throw new NotImplementedException();
        }

        public List<DataModel.PriceList> GetPriceList()
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Product> GetProducts()
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ProductSystem> GetProductsSystem()
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Distance> GetDistancesPerOrg(ApplicationDbContext context, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ShippingCompany> GetShippingCompaniesByOrgId(ApplicationDbContext context, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Organization> GetOrgs(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ShipType> GetShipType(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public Guid GetOrg(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public DataModel.Organization GetOrgEntity(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Distance> GetDistancesPerOrg(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ShippingCompany> GetShippingCompaniesByOrgId(Guid orgId)
        {
            throw new NotImplementedException();
        }

        public List<DataModel.Organization> GetOrgs()
        {
            throw new NotImplementedException();
        }

        public List<DataModel.ShipType> GetShipType()
        {
            throw new NotImplementedException();
        }

        public Guid GetOrg()
        {
            throw new NotImplementedException();
        }

        public DataModel.Organization GetOrgEntity()
        {
            throw new NotImplementedException();
        }


        public List<DataModel.Product> GetProducts(Guid orgId)
        {
            throw new NotImplementedException();
        }
    }
}