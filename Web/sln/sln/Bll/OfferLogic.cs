using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class OfferLogic
    {
        IOfferRepository _offerRepository;
        public OfferLogic(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task SetOfferClient(OfferClient offerClient, bool allowRemove,  bool allowEdit, Guid shipId, Guid shippingCompanyId, IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep, UserContext user)
        {

            bool isPresent = false;
            int qunitityType = 0;
            var organid = orgDetailRep.GetOrg();
            var distances = orgDetailRep.GetDistancesPerOrg(organid);
            var priceList = offerPrice.GetPriceList();
            var shiptypeLists = orgDetailRep.GetShipType();
            var discountLists = offerPrice.GetDiscount();
            var products = offerPrice.GetProducts(organid);
            var productsSystem = offerPrice.GetProductsSystem();

            var ship = await shippingRepository.GetShipIncludeItems(shipId);
            var statusShip = ship.StatusShipping_StatusShippingId.GetValueOrDefault();

            decimal? priceValue = null;
            PriceList price = null;

            offerClient.Id = ship.ShippingId;
            offerClient.Name = ship.Name;
            offerClient.ShippingCompanyId = shippingCompanyId;
            offerClient.TimeWaitGet = ProductSystemIds.MinAmountTimeWaitInMIn;// 15;
            offerClient.TimeWaitSend = ProductSystemIds.MinAmountTimeWaitInMIn;// 15;
            offerClient.StatusId = ship.StatusShipping_StatusShippingId.GetValueOrDefault();
            
            if (ship.TimeWaitStartSend.HasValue && ship.TimeWaitEndSend.HasValue)
            {
                var resultTimeWaitSend = ship.TimeWaitEndSend.Value - ship.TimeWaitStartSend.Value;
                offerClient.TimeWaitSend = resultTimeWaitSend.Minutes > ProductSystemIds.MinAmountTimeWaitInMIn ? resultTimeWaitSend.Minutes : ProductSystemIds.MinAmountTimeWaitInMIn;
            }
           
            if (ship.TimeWaitEndGet.HasValue && ship.TimeWaitStartSGet.HasValue)
            {
                var resultTimeWaitGet = ship.TimeWaitEndGet.Value - ship.TimeWaitStartSGet.Value;
                offerClient.TimeWaitGet = resultTimeWaitGet.Minutes > ProductSystemIds.MinAmountTimeWaitInMIn ? resultTimeWaitGet.Minutes : ProductSystemIds.MinAmountTimeWaitInMIn;
            }

            var discounts = new List<OfferClientItem>();
            foreach (var discount in discountLists)
            {
                priceValue = null;
                isPresent = false;
                qunitityType = 0;
                price = priceList.Where(p => p.ObjectId == discount.DiscountId && p.ObjectTypeCode == (int)ObjectTypeCode.Discount).FirstOrDefault();
                if (price != null)
                {
                    priceValue = price.PriceValue.HasValue ? (decimal?)price.PriceValue.Value * -1 : null;
                    isPresent = price.PriceValueType == 2 ? true : false;
                    qunitityType = price.QuntityType;
                }

                discounts.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = discount.Desc,
                    Id = discount.DiscountId,
                    IsDiscount = true,
                    IsPresent = isPresent,
                    Name = discount.Name,
                    ProductPrice = priceValue,
                    StatusRecord = 1,
                    AllowEdit = allowEdit,
                    AllowRemove = allowRemove,
                    QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
                });
            }
            qunitityType = 0;
            offerClient.Distances = new List<OfferClientItem>();
            foreach (var distance in distances)
            {
                priceValue = null;
                price = priceList.Where(p => p.ObjectId == distance.DistanceId && p.ObjectTypeCode == (int)ObjectTypeCode.Distance).FirstOrDefault();
                if (price != null)
                    priceValue = price.PriceValue;

                offerClient.Distances.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = distance.Desc,
                    Id = distance.DistanceId,
                    IsDiscount = false,
                    IsPresent = false,
                    Name = distance.Name,
                    ProductPrice = priceValue,
                    StatusRecord = 1,
                    AllowEdit = allowEdit,
                    AllowRemove = false,
                    QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
                });
            }

            offerClient.ShipTypes = new List<OfferClientItem>();
            foreach (var shiptypeItem in shiptypeLists)
            {
                priceValue = null;
                price = priceList.Where(p => p.ObjectId == shiptypeItem.ShipTypeId && p.ObjectTypeCode == (int)ObjectTypeCode.ShipType).FirstOrDefault();
                if (price != null)
                {
                    priceValue = price.PriceValue;
                }

                offerClient.ShipTypes.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = shiptypeItem.Desc,
                    Id = shiptypeItem.ShipTypeId,
                    IsDiscount = false,
                    IsPresent = false,
                    Name = shiptypeItem.Name,
                    ProductPrice = priceValue,
                    StatusRecord = 1,
                    AllowEdit = allowEdit,
                    AllowRemove = false,
                    QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
                });
            }
            offerClient.Products = new List<OfferClientItem>();
            foreach (var productItem in products)
            {
                priceValue = null;
                price = priceList.Where(p => p.ObjectId == productItem.ProductId && p.ObjectTypeCode == (int)ObjectTypeCode.Product).FirstOrDefault();
                if (price != null)
                    priceValue = price.PriceValue;

                offerClient.Products.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = productItem.Desc,
                    Id = productItem.ProductId,
                    IsDiscount = false,
                    IsPresent = false,
                    Name = productItem.Name,
                    ProductPrice = priceValue,
                    StatusRecord = 1,
                    AllowEdit = allowEdit,
                    QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin,
                    AllowRemove = false
                });
            }

            offerClient.Items = new List<OfferItem>();
        }

        public async Task Edit(OfferUpload request)
        {

            //if (request.StateCode == 1)
            //{

            //}
        }
    }
}