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
    public class OrderLogic
    {
        readonly IOfferRepository _offerRepository;
        readonly IShippingRepository _shippingRepository;
        readonly IOfferPriceRepostory _offerPrice;
        readonly IOrgDetailRepostory _orgDetailRep;

        public OrderLogic(IOfferRepository offerRepository, IShippingRepository shippingRepository, IOfferPriceRepostory offerPrice, IOrgDetailRepostory orgDetailRep)
        {
            _offerRepository = offerRepository;
            _shippingRepository = shippingRepository;
            _offerPrice = offerPrice;
            _orgDetailRep = orgDetailRep;
        }

        public OfferClient GetOfferClient(bool allowRemove, bool allowEdit, Shipping ship, Guid shippingCompanyId, UserContext user)
        {
            OfferClient offerClient = new OfferClient();
            offerClient.ObjectIdExcpetionPriceId = Guid.Parse(ProductType.ObjectIdExcpetionPrice);
            //bool isPresent = false;
            int qunitityType = 0;
            var organid = _orgDetailRep.GetOrg();
            var distances = _orgDetailRep.GetDistancesPerOrg(organid);
            var priceList = _offerPrice.GetPriceList();
            var shiptypeLists = _orgDetailRep.GetShipType();
            var discountLists = _offerPrice.GetDiscount();
            var products = _offerPrice.GetProducts(organid);
            var productsSystem = _offerPrice.GetProductsSystem();

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

            return offerClient;
        }

        public void AppendNewOffer(OfferClient offerClient, Shipping ship, bool allowRemove, bool allowEdit)
        {
            int qunitityType = 0;
            decimal? priceValue;
            PriceList price;
            offerClient.StateCode = (int)OfferVariables.OfferStateCode.New;
            var discountLists = _offerPrice.GetDiscount();
            var priceList = _offerPrice.GetPriceList();
            var productsSystem = _offerPrice.GetProductsSystem();
            var discounts = GetDiscounts(discountLists, priceList, allowRemove, allowEdit);

            offerClient.HasDirty = true;
            offerClient.Discounts = (from d in discountLists
                                     join sd in discounts
                                     on d.DiscountId equals sd.Id
                                     where d.IsSweeping == false
                                     select sd).ToList();

            offerClient.DirtyDiscounts = (from d in discountLists
                                          join sd in discounts
                                          on d.DiscountId equals sd.Id
                                          where d.IsSweeping == true
                                          select sd).ToList();

            foreach (var discountSweep in offerClient.DirtyDiscounts)
            {
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    IsDiscount = true,
                    IsPresent = discountSweep.IsPresent,
                    Desc = discountSweep.Desc,
                    StatusRecord = 1,
                    Name = discountSweep.Name,
                    ObjectId = discountSweep.Id,
                    ObjectIdType = (int)ObjectTypeCode.Discount,
                    ProductPrice = discountSweep.ProductPrice,
                    Amount = 1,
                    AllowEdit = allowEdit,
                    AllowRemove = allowRemove,
                    QuntityType = discountSweep.QuntityType
                });

            }

            foreach (var item in ship.ShippingItems)
            {
                priceValue = null;
                var price2 = priceList.Where(p => p.ObjectId == item.Product.ProductId && p.ObjectTypeCode == (int)ObjectTypeCode.Product).FirstOrDefault();
                if (price2 != null)
                    priceValue = price2.PriceValue;

                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    IsDiscount = false,
                    IsPresent = false,
                    Name = item.Product.Name,
                    Desc = item.Product.Desc,
                    ObjectId = item.Product.ProductId,
                    ObjectIdType = (int)ObjectTypeCode.Product,
                    ProductPrice = priceValue,
                    StatusRecord = 1,
                    Amount = Convert.ToInt32(item.Quantity),
                    AllowEdit = allowEdit,
                    AllowRemove = allowRemove,
                    QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
                });
            }
            priceValue = null;
            price = priceList.Where(p => p.ObjectId == ship.ShipType.ShipTypeId && p.ObjectTypeCode == (int)ObjectTypeCode.ShipType).FirstOrDefault();
            if (price != null)
            {
                priceValue = price.PriceValue;
            }
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = ship.ShipType.Name,
                Desc = ship.ShipType.Desc,
                IsDiscount = false,
                IsPresent = false,
                ProductPrice = priceValue,
                StatusRecord = 1,
                Amount = 1,
                ObjectId = ship.ShipType.ShipTypeId,
                ObjectIdType = (int)ObjectTypeCode.ShipType,
                AllowEdit = allowEdit,
                QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin,
                AllowRemove = false
            });

            priceValue = null;
            price = priceList.Where(p => p.ObjectId == ship.Distance.DistanceId && p.ObjectTypeCode == (int)ObjectTypeCode.Distance).FirstOrDefault();
            if (price != null)
                priceValue = price.PriceValue;

            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = ship.Distance.Name,
                Desc = ship.Distance.Desc,
                IsDiscount = false,
                IsPresent = false,
                ProductPrice = priceValue,
                StatusRecord = 1,
                Amount = 1,
                ObjectId = ship.Distance.DistanceId,
                ObjectIdType = (int)ObjectTypeCode.Distance,
                AllowEdit = allowEdit,
                AllowRemove = false,
                QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
            });

            priceValue = null; qunitityType = 0;
            var twSetId = Guid.Parse(ProductSystemIds.TimeWaitSet);
            var timeWaitSetProduct = productsSystem.Where(ps => ps.ProductSystemId == twSetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();

            price = priceList.Where(p => p.ObjectId == twSetId && p.ObjectTypeCode == (int)ObjectTypeCode.ProductSystem).FirstOrDefault();
            if (price != null)
            {
                priceValue = price.PriceValue;
                qunitityType = price.QuntityType;
            }
            offerClient.TimeWaitSetProductId = timeWaitSetProduct.ProductSystemId;

            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                IsDiscount = false,
                IsPresent = false,
                Desc = timeWaitSetProduct.Name,
                StatusRecord = 1,
                Name = timeWaitSetProduct.Name,
                ObjectId = timeWaitSetProduct.ProductSystemId,
                ObjectIdType = (int)ObjectTypeCode.ProductSystem,
                ProductPrice = priceValue,
                Amount = ProductSystemIds.MinAmountTimeWaitInMIn,
                AllowEdit = allowEdit,
                AllowRemove = allowRemove,
                QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
            });

            priceValue = null; qunitityType = 0;
            var twGetId = Guid.Parse(ProductSystemIds.TimeWaitGet);
            var timeWaitGetProduct = productsSystem.Where(ps => ps.ProductSystemId == twGetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();

            price = priceList.Where(p => p.ObjectId == twGetId && p.ObjectTypeCode == (int)ObjectTypeCode.ProductSystem).FirstOrDefault();
            if (price != null)
            {
                priceValue = price.PriceValue;
                qunitityType = price.QuntityType;
            }
            offerClient.TimeWaitGetProductId = timeWaitGetProduct.ProductSystemId;
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                IsDiscount = false,
                IsPresent = false,
                Desc = timeWaitSetProduct.Name,
                StatusRecord = 1,
                Name = timeWaitSetProduct.Name,
                ObjectId = timeWaitGetProduct.ProductSystemId,
                ObjectIdType = (int)ObjectTypeCode.ProductSystem,
                ProductPrice = priceValue,
                Amount = ProductSystemIds.MinAmountTimeWaitInMIn,
                AllowEdit = allowEdit,
                AllowRemove = allowRemove,
                QuntityType = qunitityType == 0 ? General.Unit : General.UnitMin
            });

        }

        public void AppendCurrentOffer(OfferClient offerClient, Shipping ship, RequestShipping offer, bool allowRemove, bool allowEdit)
        {
            offerClient.StateCode = offer.StatusCode;
            var discountLists = _offerPrice.GetDiscount();
            var priceList = _offerPrice.GetPriceList();
            var productsSystem = _offerPrice.GetProductsSystem();
            var discounts = GetDiscounts(discountLists, priceList, allowRemove, allowEdit);
            offerClient.OfferId = offer.RequestShippingId;
            offerClient.HasDirty = false;

            offerClient.DirtyDiscounts = new List<OfferClientItem>();
            foreach (var item in offer.RequestItemShip)
            {
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    IsDiscount = item.IsDiscount,
                    IsPresent = item.PriceValueType==2,
                    Name = item.Name,
                    Desc = item.Desc,
                    ObjectId = item.ObjectTypeId.GetValueOrDefault(),
                    ObjectIdType = item.ObjectTypeIdCode.GetValueOrDefault(),
                    ProductPrice = item.ProductValue,
                    StatusRecord = 2,
                    Amount = item.Amount,
                    AllowEdit = allowEdit,
                    AllowRemove = allowRemove,
                    QuntityType = item.QuntityType == 0 ? General.Unit : General.UnitMin
                });
                if (item.IsDiscount)
                {
                    offerClient.DirtyDiscounts.Add(new OfferClientItem
                    {
                        AllowRemove = allowRemove,
                        Amount = item.Amount,
                        AllowEdit = allowEdit,
                        Desc = item.Desc,
                        Id = item.ObjectTypeId.GetValueOrDefault(),
                        IsDiscount = true,
                        IsPresent = item.PriceValueType == 2,
                        Name = item.Name,
                        // PriceValue = item.PriceValue,
                        ProductPrice = item.ProductValue,
                        QuntityType = item.QuntityType == 0 ? General.Unit : General.UnitMin,
                        StatusRecord = 2
                    });
                }
            }
            offerClient.Discounts = (from d in discounts
                                     where !offerClient.Items.Any(o => o.ObjectIdType == (int)ObjectTypeCode.Discount && o.ObjectId == d.Id)
                                     select d).ToList();


            var twSetId = Guid.Parse(ProductSystemIds.TimeWaitSet);
            var timeWaitSetProduct = productsSystem.Where(ps => ps.ProductSystemId == twSetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();
            offerClient.TimeWaitSetProductId = timeWaitSetProduct.ProductSystemId;

            var twGetId = Guid.Parse(ProductSystemIds.TimeWaitGet);
            var timeWaitGetProduct = productsSystem.Where(ps => ps.ProductSystemId == twGetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();
            offerClient.TimeWaitGetProductId = timeWaitGetProduct.ProductSystemId;
        }

        public List<OfferClientItem> GetDiscounts(List<Discount> discountLists, List<PriceList> priceList, bool allowRemove, bool allowEdit)
        {
            bool isPresent = false;
            int qunitityType = 0;
            decimal? priceValue;
            PriceList price;
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
            return discounts;
        }

        public async Task<Shipping> GetShipAsync(Guid shipId)
        {
            return await _shippingRepository.GetShipIncludeItems(shipId);
        }

        public async Task<RequestShipping> GetOfferAsync(Guid offerId)
        {
            if (offerId == null || offerId == Guid.Empty)
                return null;
            return await _offerRepository.GetAsync(offerId);
        }

        public string GetTitle(RequestShipping offer)
        {
            var stateCode = 1;
            if (offer != null)
                stateCode = offer.StatusCode;

            switch (stateCode)
            {
                case 1:
                    return "בקשת הזמנה חדשה";
                case 2:
                    return "אישור הזמנה סופי";
                default:
                    return "אישור הזמנה סופי";

            }
        }

        public RequestShipping Create(OfferUpload offer, UserContext user, Shipping ship, ShippingCompany managerShip)
        {
            var dt = DateTime.Now;
            Guid requestShippingId = Guid.NewGuid();
            RequestShipping request = new RequestShipping();
            request.RequestShippingId = requestShippingId;
            request.StatusCode = (int)OfferVariables.OfferStateCode.Request;//next status
            request.StatusReasonCode = 1;
            request.ShippingCompany_ShippingCompanyId = managerShip.ShippingCompanyId;
            request.Shipping_ShippingId = ship.ShippingId;
            request.ModifiedBy = user.UserId;
            request.ModifiedOn = DateTime.Now;
            request.CreatedBy = user.UserId;
            request.CreatedOn = DateTime.Now;

            List<RequestItemShip> items = FillItems(offer, requestShippingId, user);

            ship.ShippingCompany_ShippingCompanyId = managerShip.ShippingCompanyId;
            ship.OfferId = requestShippingId;
            ship.ModifiedBy = user.UserId;
            ship.ModifiedOn = DateTime.Now;

            _shippingRepository.Update(ship);
            _offerRepository.Create(request, items);


            return request;
        }

        public bool IsNeedEsclationPrice(OfferUpload offer,Shipping ship,RequestShipping request )
        {
            var org = _orgDetailRep.GetOrgEntity();
            if ((offer.AddExceptionPrice || (org.PriceValueException.HasValue && org.PriceValueException.Value >= offer.Total)) && !ship.ApprovalPriceException.HasValue)
            {
               // request.StatusCode = (int)OfferVariables.OfferStateCode.ConfirmException;//next status
                return true;
            }
            return false;
        }
        public void SetEsclationStatus(OfferUpload offer, Shipping ship, RequestShipping request)
        {
            request.StatusCode = (int)OfferVariables.OfferStateCode.ConfirmException;//next status
                
        }
        public void ChangeStatusOffer(int status, OfferUpload offerRequest, UserContext user, Shipping ship, RequestShipping offer)
        {
            List<RequestItemShip> items = new List<RequestItemShip>();
            if (offerRequest.HasDirty)
            {
                items = FillItems(offerRequest, offer.RequestShippingId, user);
            }
            offer.StatusCode = status;//next status
            offer.StatusReasonCode = 1;
            offer.ModifiedBy = user.UserId;
            offer.ModifiedOn = DateTime.Now;

            _offerRepository.ChangeStatus(offer, items, offerRequest.HasDirty);
            
        }
        public void SetApprovalPriceException(OfferUpload offerRequest, UserContext user, Shipping ship, RequestShipping offer)
        {
            ship.ApprovalPriceException = user.UserId;
        }
        public List<RequestItemShip> FillItems(OfferUpload offerRequest, Guid requestShippingId, UserContext user)
        {
            DateTime dt = DateTime.Now;
            List<RequestItemShip> items = new List<RequestItemShip>();
            if (offerRequest.HasDirty)
            {
                foreach (var dataItem in offerRequest.DataItems)
                {
                    items.Add(new RequestItemShip
                    {
                        Amount = dataItem.Amount,
                        CreatedBy = user.UserId,
                        CreatedOn = dt,
                        Desc = dataItem.Desc,
                        IsActive = true,
                        ModifiedBy = user.UserId,
                        ModifiedOn = dt,
                        Name = dataItem.Name,
                        ObjectTypeId = dataItem.ObjectId,
                        ObjectTypeIdCode = dataItem.ObjectIdType,
                        PriceValue = dataItem.PriceValue,
                        PriceValueType = dataItem.IsPresent ? 2 : 1,
                        RequestItemShipId = Guid.NewGuid(),
                        RequestShipping_RequestShippingId = requestShippingId,
                        ProductValue = dataItem.ProductPrice,
                        IsDiscount = dataItem.IsDiscount,
                        QuntityType = dataItem.QuntityType == General.UnitMin ? 1 : 0

                    });
                }
            }
            return items;

        }

        public void SetCompanyHandler(Shipping ship, Guid companyHandler)
        {
            ship.ShippingCompany_ShippingCompanyId = companyHandler;
        }

        public void Update(Shipping ship)
        {
            _shippingRepository.Update(ship);
        }
    }
}