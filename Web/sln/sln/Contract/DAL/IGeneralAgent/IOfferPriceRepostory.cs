using Michal.Project.Dal;
using System;
using System.Collections.Generic;
namespace Michal.Project.Contract.DAL
{
    public interface IOfferPriceRepostory
    {
        System.Collections.Generic.List<Michal.Project.DataModel.Discount> GetDiscount();
        System.Collections.Generic.List<Michal.Project.DataModel.PriceList> GetPriceList();
        System.Collections.Generic.List<Michal.Project.DataModel.Product> GetProducts( Guid orgId);
        System.Collections.Generic.List<Michal.Project.DataModel.ProductSystem> GetProductsSystem();
        List<KeyValuePair<int, string>> GetBackOrder();
        List<KeyValuePair<int, string>> GetDirection();
     }
}
