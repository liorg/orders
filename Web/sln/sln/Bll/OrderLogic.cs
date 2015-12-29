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
        readonly IOfferPriceRepostory _offerPriceRepository;
        readonly IOrgDetailRepostory _orgDetailRepsitory;
        readonly IUserRepository _userRepository;
        readonly ILocationRepository _locationRepostory;

        public OrderLogic(IOfferRepository offerRepository,
            IShippingRepository shippingRepository, IOfferPriceRepostory offerPriceRepository,
            IOrgDetailRepostory orgDetailRepsitory, IUserRepository userRepository,
            ILocationRepository locationRepostory)
        {
            _offerRepository = offerRepository;
            _shippingRepository = shippingRepository;
            _offerPriceRepository = offerPriceRepository;
            _orgDetailRepsitory = orgDetailRepsitory;
            _userRepository = userRepository;
            _locationRepostory = locationRepostory;
        }

        public async Task<IEnumerable<ShippingItemVm>> GetItemsShip(Guid shipId)
        {
          
            var shippingItems = await _shippingRepository.GetShipitems(shipId); //await context.ShippingItem.Where(s => s.IsActive == true && s.Shipping_ShippingId == shipId && s.Product != null && s.Product.IsCalculatingShippingInclusive == false).ToListAsync();
        
            var model = new List<ShippingItemVm>();
            foreach (var shipItem in shippingItems)
            {

                var u = new ShippingItemVm();
                u.Id = shipItem.ShippingItemId;
                u.ProductName = shipItem.Product != null ? shipItem.Product.Name : "";
                u.Name = shipItem.Name;
                u.Total = Convert.ToInt32(shipItem.Quantity);
                model.Add(u);
            }
            return model;
        }

        public async Task<ShippingVm> OnPreCreateShip(UserContext userContext)
        {
            var model = new ShippingVm();
            var org = _orgDetailRepsitory.GetOrgEntity();
            var counter = await _shippingRepository.GetCounter(org.OrgId);
            var increa = counter.LastNumber;
            model.Number = String.Format(org.Perfix + "-{0}", increa.ToString().PadLeft(5, '0'));
            model.FastSearch = increa;

            model.SourceAddress = new AddressEditorViewModel();
            model.SourceAddress.City = userContext.Address.CityName;
            model.SourceAddress.Citycode = userContext.Address.CityCode;
            model.SourceAddress.CitycodeOld = userContext.Address.CityCode;
            model.SourceAddress.Street = userContext.Address.StreetName;
            model.SourceAddress.Streetcode = userContext.Address.StreetCode;
            model.SourceAddress.StreetcodeOld = userContext.Address.StreetCode;
            model.SourceAddress.ExtraDetail = userContext.Address.ExtraDetail;
            model.SourceAddress.Num = userContext.Address.StreetNum;
            model.SourceAddress.UId = userContext.Address.UID;

            model.SourceAddress.Lat = userContext.Address.Lat;
            model.SourceAddress.Lng = userContext.Address.Lng;
            model.Direction = 0;//send
            model.TelSource = userContext.Tel;
            model.NameSource = userContext.FullName;

            return model;
        }

        public async Task<Guid> OnPostCreateShip(ShippingVm shippingVm, UserContext userContext)
        {
            shippingVm.StatusId = Guid.Parse(Helper.Status.Draft);
            var org = _orgDetailRepsitory.GetOrgEntity();
            var shipping = new Shipping();

            var userid = userContext.UserId;
            shipping.ShippingId = Guid.NewGuid();
            shipping.FastSearchNumber = shippingVm.FastSearch;
            shipping.Name = shippingVm.Number;
            shipping.SigBackType = shippingVm.SigBackType;
            shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
            shipping.Direction = shippingVm.Direction;

            var currentDate = DateTime.Now;
            shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
            shipping.CreatedOn = currentDate;
            shipping.CreatedBy = userid;
            shipping.ModifiedOn = currentDate;
            shipping.ModifiedBy = userid;
            shipping.OwnerId = userid;
            shipping.IsActive = true;
            shipping.NotifyType = (int)AlertStyle.Warning;
            shipping.NotifyText = Notification.MessageConfirm;

            shipping.Recipient = shippingVm.Recipient;
            shipping.TelSource = shippingVm.TelSource; // userContext.Tel;
            shipping.TelTarget = shippingVm.TelTarget;
            shipping.NameSource = shippingVm.NameSource;// userContext.FullName;
            shipping.NameTarget = shippingVm.NameTarget;

            await _locationRepostory.SetLocationAsync(shippingVm.TargetAddress, shipping.Target);
            await _locationRepostory.SetLocationAsync(shippingVm.SourceAddress, shipping.Source);
            shipping.Target.ExtraDetail = shippingVm.TargetAddress.ExtraDetail;
            shipping.Source.ExtraDetail = shippingVm.SourceAddress.ExtraDetail;
            shipping.Organization_OrgId = org.OrgId;
            if (_locationRepostory.IsChangedCity(shippingVm.SourceAddress) || _locationRepostory.IsChangedCity(shippingVm.TargetAddress))
            {
                await _locationRepostory.SetDistance(shipping.Source, shipping.Target, shipping);
                var distance = FindDistance(shipping, org);
                shipping.Distance_DistanceId = distance.DistanceId;
            }

            TimeLine timeline = new TimeLine
            {
                Name = "הזמנה חדשה" + "של " + userContext.FullName + " מספר עובד - " + userContext.EmpId + "",
                Desc = "הזמנה חדשה שנוצרה" + " " + shipping.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy HH:mm"),
                CreatedBy = userid,
                CreatedOn = currentDate,
                ModifiedBy = userid,
                ModifiedOn = currentDate,
                TimeLineId = Guid.NewGuid(),
                IsActive = true,
                Status = TimeStatus.New,
                StatusShipping_StatusShippingId = shippingVm.StatusId
            };
            shipping.TimeLines.Add(timeline);
            await _shippingRepository.AddOwner(shipping, userContext.UserId);

            _shippingRepository.Add(shipping);
            return shipping.ShippingId;
        }

        public async Task OnPostUpdateShip(ShippingVm shippingVm, UserContext userContext)
        {
            var shipping = await _shippingRepository.GetShip(shippingVm.Id);
            var org = _orgDetailRepsitory.GetOrgEntity();
            shipping.ShipType_ShipTypeId = shippingVm.ShipTypeId;
            shipping.Distance_DistanceId = shippingVm.DistanceId;
            shipping.SigBackType = shippingVm.SigBackType;
            shipping.FastSearchNumber = shippingVm.FastSearch;
            shipping.StatusShipping_StatusShippingId = shippingVm.StatusId;
            shipping.ModifiedOn = DateTime.Now;
            shipping.ModifiedBy = userContext.UserId;
            shipping.IsActive = true;
            shipping.Direction = shippingVm.Direction;
            shipping.Recipient = shippingVm.Recipient;
            shipping.TelTarget = shippingVm.TelTarget;
            shipping.NameTarget = shippingVm.NameTarget;

            shipping.TelSource = shippingVm.TelSource;
            shipping.NameSource = shippingVm.NameSource;

            await _locationRepostory.SetLocationAsync(shippingVm.TargetAddress, shipping.Target);
            await _locationRepostory.SetLocationAsync(shippingVm.SourceAddress, shipping.Source);
            shipping.Target.ExtraDetail = shippingVm.TargetAddress.ExtraDetail;
            shipping.Source.ExtraDetail = shippingVm.SourceAddress.ExtraDetail;
            if (shippingVm.DistanceId == shippingVm.DistanceIdState)
            {
                if (_locationRepostory.IsChangedCity(shippingVm.SourceAddress) || _locationRepostory.IsChangedCity(shippingVm.TargetAddress))
                {
                    await _locationRepostory.SetDistance(shipping.Source, shipping.Target, shipping);

                    var distance = FindDistance(shipping, org);
                    shipping.Distance_DistanceId = distance.DistanceId;

                }
            }
            _shippingRepository.Update(shipping);
            // context.Entry<Shipping>(shipping).State = EntityState.Modified;
        }

        public async Task<ShippingVm> OnPreUpdateShip(Guid id, UserContext userContext)
        {
            var shipping = await _shippingRepository.GetShip(id);
            var model = new ShippingVm();
            model.Number = shipping.Name;
            model.Name = shipping.Name;
            model.SigBackType = shipping.SigBackType.GetValueOrDefault();
            model.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
            model.DistanceIdState = model.DistanceId;
            model.ShipTypeId = shipping.ShipType_ShipTypeId.GetValueOrDefault();
            model.DistanceValue = String.IsNullOrEmpty(shipping.DistanceText) ? General.Empty : shipping.DistanceText;
            model.FastSearch = shipping.FastSearchNumber;
            model.Id = shipping.ShippingId;

            model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

            model.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : General.Empty;
            model.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            model.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

            model.Recipient = shipping.Recipient;
            model.TelTarget = shipping.TelTarget;
            model.NameTarget = shipping.NameTarget;

            model.TelSource = shipping.TelSource;
            model.NameSource = shipping.NameSource;

            model.SourceAddress = new AddressEditorViewModel();
            model.SourceAddress.City = shipping.Source.CityName;
            model.SourceAddress.Citycode = shipping.Source.CityCode;
            model.SourceAddress.CitycodeOld = shipping.Source.CityCode;
            model.SourceAddress.Street = shipping.Source.StreetName;
            model.SourceAddress.Streetcode = shipping.Source.StreetCode;
            model.SourceAddress.StreetcodeOld = shipping.Source.StreetCode;
            model.SourceAddress.ExtraDetail = shipping.Source.ExtraDetail;
            model.SourceAddress.Num = shipping.Source.StreetNum;

            model.TargetAddress = new AddressEditorViewModel();
            model.TargetAddress.City = shipping.Target.CityName;
            model.TargetAddress.Citycode = shipping.Target.CityCode;
            model.TargetAddress.CitycodeOld = shipping.Target.CityCode;
            model.TargetAddress.Street = shipping.Target.StreetName;
            model.TargetAddress.Streetcode = shipping.Target.StreetCode;
            model.TargetAddress.StreetcodeOld = shipping.Target.StreetCode;
            model.TargetAddress.ExtraDetail = shipping.Target.ExtraDetail;
            model.TargetAddress.Num = shipping.Target.StreetNum;

            model.Direction = shipping.Direction;

            if (shipping.StatusShipping_StatusShippingId.HasValue)
            {
                if (shipping.StatusShipping_StatusShippingId.Value == Guid.Parse(Helper.Status.Draft))
                {
                    shipping.NotifyType = (int)AlertStyle.Warning;//Notification.Warning;
                    shipping.NotifyText = Notification.MessageConfirm;
                }
            }
            return model;
        }

        public Distance FindDistance(Shipping shipping, Organization org)
        {
            var distances = _orgDetailRepsitory.GetDistancesPerOrg(org.OrgId);
            Distance distance = null;
            if (shipping.DistanceValue != 0)
            {
                distance = (from d in distances
                            where d.FromDistance.HasValue && d.FromDistance.Value < shipping.DistanceValue && d.ToDistance.HasValue && d.ToDistance.Value >= shipping.DistanceValue
                            select d).FirstOrDefault();
                if (distance != null)
                    return distance;

            }

            DefaultShip defaultShip = new DefaultShip();
            var defaultDistance = defaultShip.items.Where(t => t.Item1 == DefaultShip.DType.Distance).FirstOrDefault();
            distance = distances.Where(dd => dd.DistanceId == defaultDistance.Item3).FirstOrDefault();

            return distance;
        }

        public OfferClient GetOfferClient(bool allowRemove, bool allowEdit, Shipping ship, Guid shippingCompanyId, UserContext user)
        {
            OfferClient offerClient = new OfferClient();
            bool isPresent = false;
            offerClient.ObjectIdExcpetionPriceId = Guid.Parse(ProductType.ObjectIdExcpetionPrice);

            int qunitityType = 0;
            var organid = _orgDetailRepsitory.GetOrg();
            var distances = _orgDetailRepsitory.GetDistancesPerOrg(organid);
            var priceList = _offerPriceRepository.GetPriceList();
            var shiptypeLists = _orgDetailRepsitory.GetShipType();
            var discountLists = _offerPriceRepository.GetDiscount();
            var products = _offerPriceRepository.GetProducts(organid);
            var productsSystem = _offerPriceRepository.GetProductsSystem();

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
                offerClient.TimeWaitSend = resultTimeWaitSend.TotalMinutes > ProductSystemIds.MinAmountTimeWaitInMIn ? resultTimeWaitSend.TotalMinutes : ProductSystemIds.MinAmountTimeWaitInMIn;
            }

            if (ship.TimeWaitEndGet.HasValue && ship.TimeWaitStartSGet.HasValue)
            {
                var resultTimeWaitGet = ship.TimeWaitEndGet.Value - ship.TimeWaitStartSGet.Value;
                offerClient.TimeWaitGet = resultTimeWaitGet.TotalMinutes > ProductSystemIds.MinAmountTimeWaitInMIn ? resultTimeWaitGet.TotalMinutes : ProductSystemIds.MinAmountTimeWaitInMIn;
            }


            qunitityType = 0;
            offerClient.Distances = new List<OfferClientItem>();
            foreach (var distance in distances)
            {
                priceValue = null;
                price = priceList.Where(p => p.ObjectId == distance.DistanceId && p.ObjectTypeCode == (int)ObjectTypeCode.Distance).FirstOrDefault();
                if (price != null)
                {
                    priceValue = price.PriceValue;
                    isPresent = price.PriceValueType == 2 ? true : false;
                }
                offerClient.Distances.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = distance.Desc,
                    Id = distance.DistanceId,
                    IsDiscount = false,
                    IsPresent = isPresent,//false,
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
                    isPresent = price.PriceValueType == 2 ? true : false;
                }

                offerClient.ShipTypes.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = shiptypeItem.Desc,
                    Id = shiptypeItem.ShipTypeId,
                    IsDiscount = false,
                    IsPresent = isPresent,//false,
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
                {
                    priceValue = price.PriceValue;
                    isPresent = price.PriceValueType == 2 ? true : false;
                }
                offerClient.Products.Add(new OfferClientItem
                {
                    Amount = 1,
                    Desc = productItem.Desc,
                    Id = productItem.ProductId,
                    IsDiscount = false,
                    IsPresent =isPresent,// false,
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
            bool isPresent=false;
            offerClient.StateCode = (int)OfferVariables.OfferStateCode.New;
            var discountLists = _offerPriceRepository.GetDiscount();
            var priceList = _offerPriceRepository.GetPriceList();
            var productsSystem = _offerPriceRepository.GetProductsSystem();
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
                {
                    priceValue = price2.PriceValue;
                    isPresent = price2.PriceValueType == 2 ? true : false;
                }

                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    IsDiscount = false,
                    IsPresent = isPresent,//false,
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
                isPresent = price.PriceValueType == 2 ? true : false;
            }
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = ship.ShipType.Name,
                Desc = ship.ShipType.Desc,
                IsDiscount = false,
                IsPresent =isPresent,// false,
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
            {
                priceValue = price.PriceValue;
                isPresent = price.PriceValueType == 2 ? true : false;
            }
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = ship.Distance.Name,
                Desc = ship.Distance.Desc,
                IsDiscount = false,
                IsPresent = isPresent,//false,
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
            offerClient.IsAddExceptionPrice = false;
            offerClient.AddExceptionPrice = offerClient.AddExceptionPrice;
            var discountLists = _offerPriceRepository.GetDiscount();
            var priceList = _offerPriceRepository.GetPriceList();
            var productsSystem = _offerPriceRepository.GetProductsSystem();
            var discounts = GetDiscounts(discountLists, priceList, allowRemove, allowEdit);
            offerClient.OfferId = offer.RequestShippingId;
            offerClient.HasDirty = false;

            var twSetId = Guid.Parse(ProductSystemIds.TimeWaitSet);
            var timeWaitSetProduct = productsSystem.Where(ps => ps.ProductSystemId == twSetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();
            offerClient.TimeWaitSetProductId = timeWaitSetProduct.ProductSystemId;

            var twGetId = Guid.Parse(ProductSystemIds.TimeWaitGet);
            var timeWaitGetProduct = productsSystem.Where(ps => ps.ProductSystemId == twGetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();
            offerClient.TimeWaitGetProductId = timeWaitGetProduct.ProductSystemId;

            offerClient.DirtyDiscounts = new List<OfferClientItem>();
            foreach (var item in offer.RequestItemShip)
            {
                if (item.ObjectTypeId == Guid.Parse(ProductType.ObjectIdExcpetionPrice))
                    offerClient.IsAddExceptionPrice = true;

                // for close 
                if (offerClient.StateCode == (int)OfferVariables.OfferStateCode.End)
                {
                    if (item.ObjectTypeId == offerClient.TimeWaitSetProductId
                        && item.ObjectTypeIdCode == (int)ObjectTypeCode.ProductSystem)
                        item.Amount = (int)offerClient.TimeWaitSend;

                    else if (item.ObjectTypeId == offerClient.TimeWaitGetProductId
                        && item.ObjectTypeIdCode == (int)ObjectTypeCode.ProductSystem)
                        item.Amount = (int)offerClient.TimeWaitGet;

                }
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    IsDiscount = item.IsDiscount,
                    IsPresent = item.PriceValueType == 2,
                    Name = item.Name,
                    Desc = item.Desc,
                    ObjectId = item.ObjectTypeId.GetValueOrDefault(),
                    ObjectIdType = item.ObjectTypeIdCode.GetValueOrDefault(),
                    ProductPrice = item.ProductValue,
                    StatusRecord = item.StatusRecord,
                    Amount = item.Amount,
                    AllowEdit = allowEdit,
                    AllowRemove = allowRemove,
                    QuntityType = item.QuntityType == 0 ? General.Unit : General.UnitMin,
                   
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
                        ProductPrice = item.ProductValue,
                        QuntityType = item.QuntityType == 0 ? General.Unit : General.UnitMin,
                        StatusRecord = item.StatusRecord
                    });
                }
            }
            offerClient.Discounts = (from d in discounts
                                     where !offerClient.Items.Any(o => o.ObjectIdType == (int)ObjectTypeCode.Discount && o.ObjectId == d.Id)
                                     select d).ToList();



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
                case 7:
                    return "אישור הזמנה חריגה";
                case 3:
                    return "פרטי הזמנה";
                case 10:
                    return " ההזמנה סגורה";
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

            request.Price = offer.Price.GetValueOrDefault();
            request.Total = offer.Total.GetValueOrDefault();
            request.DiscountPrice = offer.DiscountPrice.GetValueOrDefault();

            List<RequestItemShip> items = FillItems(offer, requestShippingId, user);

            ship.ShippingCompany_ShippingCompanyId = managerShip.ShippingCompanyId;
            ship.OfferId = requestShippingId;
            ship.ModifiedBy = user.UserId;
            ship.ModifiedOn = DateTime.Now;

            _shippingRepository.Update(ship);
            _offerRepository.Create(request, items);


            return request;
        }

        public bool IsNeedEsclationPrice(OfferUpload offer, Shipping ship, RequestShipping request)
        {
            var org = _orgDetailRepsitory.GetOrgEntity();
            if ((offer.IsAddExceptionPrice || (org.PriceValueException.HasValue && org.PriceValueException.Value <= offer.Total)) && !ship.ApprovalPriceException.HasValue)
                return true;

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

            offer.Price = offerRequest.Price.GetValueOrDefault();
            offer.Total = offerRequest.Total.GetValueOrDefault();
            offer.DiscountPrice = offerRequest.DiscountPrice.GetValueOrDefault();


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
                        QuntityType = dataItem.QuntityType == General.UnitMin ? 1 : 0,
                        StatusRecord=dataItem.StatusRecord

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

        public async Task<UserLink> GetCreator(Shipping ship)
        {
            var userLink = await _userRepository.GetUserLink(ship.CreatedBy);
            return userLink;
        }

        public async Task<UserLink> GetApproval(Shipping ship)
        {
            var userLink = await _userRepository.GetUserLink(ship.ApprovalRequest);
            return userLink;
        }

        public async Task<UserLink> GetApprovalException(Shipping ship)
        {
            var userLink = await _userRepository.GetUserLink(ship.ApprovalPriceException);
            return userLink;
        }

        public async Task<UserLink> GetApprovalShip(Shipping ship)
        {
            var userLink = await _userRepository.GetUserLink(ship.ApprovalShip);
            return userLink;
        }

        public void SetPriceOnCloseShip(Shipping ship,OfferUpload offerClose)
        {
            ship.Price = offerClose.ClosedPrice.GetValueOrDefault();
            ship.ActualPrice = offerClose.ClosedTotal.GetValueOrDefault();
            ship.DiscountPrice = offerClose.DiscountPrice.GetValueOrDefault();
        }
    }
}