using Michal.Project.Dal;
using Michal.Project.Helper;
using Michal.Project.Models;
using Michal.Project.Models.NSStreet;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using Michal.Project.DataModel;
using Michal.Project.Fasade;


namespace Michal.Project.Api
{
    [Authorize]
    [RoutePrefix("api/OfferService")]
    public class OfferServiceController : ApiController
    {

        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.CreateOrders());
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("GetOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> GetOffer(Guid shipid, Guid offerId)
        {
            using (var context = new ApplicationDbContext())
            {
                string unitMin = "דק'";
                string unit = "יח'";
                bool allowRemove = false;
                //    bool allowAdd = false;
                bool allowEdit = false;
                bool isPresent = false;
                int qunitityType = 0;
                var userContext = HttpContext.Current.GetOwinContext().Authentication;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                   (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
                {
                    allowRemove = true;
                    //  allowAdd = true;
                    allowEdit = true;
                }
                var organid = cache.GetOrg(context);
                var distances = cache.GetDistancesPerOrg(context, organid);
                var priceList = cache.GetPriceList(context);
                var shiptypeLists = cache.GetShipType(context);
                var discountLists = cache.GetDiscount(context);
                var products = cache.GetProducts(context, organid);
                var productsSystem = cache.GetProductsSystem(context);

                var ship = await context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipid);
             
                var statusShip = ship.StatusShipping_StatusShippingId.GetValueOrDefault();

                var user = new UserContext(userContext);
                decimal? priceValue = null;
                PriceList price = null;
                OfferClient offerClient = new OfferClient();
                offerClient.Id = ship.ShippingId;
                offerClient.Name = ship.Name;
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
                        QuntityType = qunitityType == 0 ? unit : unitMin
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
                        QuntityType = qunitityType == 0 ? unit : unitMin
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
                        QuntityType = qunitityType == 0 ? unit : unitMin
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
                        QuntityType = qunitityType == 0 ? unit : unitMin,
                        AllowRemove = false
                    });
                }

                offerClient.Items = new List<OfferItem>();
                if (offerId == Guid.Empty)
                {
                    offerClient.HasDirty = true;
                    offerClient.Discounts = (from d in discountLists
                                             join sd in discounts
                                             on d.DiscountId equals sd.Id
                                             where d.IsSweeping == false
                                             select sd).ToList();

                    //var discountsSweep = discountLists.Where(ds => ds.IsSweeping == true).ToList();

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
                            QuntityType = qunitityType == 0 ? unit : unitMin
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
                        QuntityType = qunitityType == 0 ? unit : unitMin,
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
                        QuntityType = qunitityType == 0 ? unit : unitMin
                    });
                }
                priceValue = null; qunitityType = 0;
                var twSetId = Guid.Parse(ProductSystemIds.TimeWaitSet);
                var timeWaitSetProduct = productsSystem.Where(ps => ps.ProductSystemId == twSetId && ps.ProductTypeKey == (int)ProductSystemIds.ProductSystemType.TimeWait).FirstOrDefault();

                price = priceList.Where(p => p.ObjectId == twSetId && p.ObjectTypeCode == (int)ObjectTypeCode.ProductSystem).FirstOrDefault();
                if (price != null)
                {
                    priceValue = price.PriceValue;
                    qunitityType = price.QuntityType;
                }

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
                    QuntityType = qunitityType == 0 ? unit : unitMin
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
                    QuntityType = qunitityType == 0 ? unit : unitMin
                });

                //offerClient.Discounts = (from d in demo.Discounts
                //                         where !offerClient.Items.Any(o => o.ObjectIdType == (int)ObjectTypeCode.Discount && o.ObjectId == d.Id)
                //                         select d).ToList();

                var data = JsonConvert.SerializeObject(offerClient);
                var responseBody = @"var offerClient = " + data + ";";

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/javascript")
                };

                response.Headers.CacheControl = new CacheControlHeaderValue();
                response.Headers.CacheControl.NoStore = true;
                return response;
            }
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [Route("EditOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> EditOffer([FromBody] OfferUpload offer)
        {
            var result = new Result<Guid>();
            result.Model = Guid.NewGuid();
            var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
            var path = "/Offer/CreateOrder?shipId="+offer.Id.ToString();
             var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            using (var context = new ApplicationDbContext())
            {
                var ship = await context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == offer.Id);
                var orderName=ship.Name;
                NotificationManager manager = new NotificationManager();
                var notifyItem = new NotifyItem
                {
                    Title = "הזמנה חדשה",
                    Body = " אישור הזמנה" + orderName,
                    Url = url+path
                };
               await manager.Send(context, user,notifyItem);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result<Guid>>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

    }
}