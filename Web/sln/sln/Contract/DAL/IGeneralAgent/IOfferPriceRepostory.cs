using Michal.Project.Dal;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
namespace Michal.Project.Contract.DAL
{
    public interface IOfferPriceRepostory
    {
        List<Discount> GetDiscount();
        List<PriceList> GetPriceList();
        List<Product> GetProducts( Guid orgId);
        List<ProductSystem> GetProductsSystem();
        List<KeyValuePair<int, string>> GetBackOrder();
        List<KeyValuePair<int, string>> GetDirection();
     }
}
