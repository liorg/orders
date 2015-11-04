﻿using Michal.Project.Dal;
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
                var userContext = HttpContext.Current.GetOwinContext().Authentication;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                var organid = cache.GetOrg(context);
                var distances = cache.GetDistancesPerOrg(context, organid); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                var priceList = cache.GetPriceList(context);
                var shiptypeLists = cache.GetShipType(context);
                var discountLists = cache.GetDiscount(context);
                var products = cache.GetProducts(context, organid);
                var user = new UserContext(userContext);
                decimal? priceValue = null;
                PriceList price = null;
                OfferClient offerClient = new OfferClient();

                var discounts = new List<OfferClientItem>();
                foreach (var discount in discountLists)
                {
                    discounts.Add(new OfferClientItem
                    {
                        Amount = 1,
                        Desc = discount.Desc,
                        Id = discount.DiscountId,
                        IsDiscount = true,
                        IsPresent = false,
                        Name = discount.Name,
                        ProductPrice = null,
                        StatusRecord = 1,


                    });
                }

                offerClient.Distance = new List<OfferClientItem>();
                foreach (var distance in distances)
                {
                    priceValue = null;
                    price = priceList.Where(p => p.ObjectId == distance.DistanceId && p.ObjectTypeCode == (int)ObjectTypeCode.Distance).FirstOrDefault();
                    if (price != null)
                        priceValue = price.PriceValue;

                    offerClient.Distance.Add(new OfferClientItem
                    {
                        Amount = 1,
                        Desc = distance.Desc,
                        Id = distance.DistanceId,
                        IsDiscount = false,
                        IsPresent = false,
                        Name = distance.Name,
                        ProductPrice = priceValue,
                        StatusRecord = 1
                    });
                }

                offerClient.ShipType = new List<OfferClientItem>();
                foreach (var shiptypeItem in shiptypeLists)
                {
                    priceValue = null;
                    price = priceList.Where(p => p.ObjectId == shiptypeItem.ShipTypeId && p.ObjectTypeCode == (int)ObjectTypeCode.ShipType).FirstOrDefault();
                    if (price != null)
                        priceValue = price.PriceValue;

                    offerClient.ShipType.Add(new OfferClientItem
                    {
                        Amount = 1,
                        Desc = shiptypeItem.Desc,
                        Id = shiptypeItem.ShipTypeId,
                        IsDiscount = false,
                        IsPresent = false,
                        Name = shiptypeItem.Name,
                        ProductPrice = priceValue,
                        StatusRecord = 1
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
                        StatusRecord = 1
                    });
                }
                //var demo = new OfferDemo();

                offerClient.Items = new List<OfferItem>();
                if (offerId == Guid.Empty)
                {

                    offerClient.Discounts = (from d in discountLists
                                             join sd in discounts
                                             on d.DiscountId equals sd.Id
                                             where d.IsSweeping == false
                                             select sd).ToList();
                    var discountsSweep = discountLists.Where(ds => ds.IsSweeping == true).ToList();
                    foreach (var discountSweep in discountsSweep)
                    {
                        offerClient.Items.Add(new OfferItem
                        {
                            Id = Guid.NewGuid(),
                            IsDiscount = true,
                            IsPresent = false,
                            Desc=discountSweep.Desc,
                            Name = discountSweep.Name,
                            ObjectId = discountSweep.DiscountId,
                            ObjectIdType = (int)ObjectTypeCode.Discount,
                            ProductPrice = null,
                            Amount = 1
                        });
                    }
                    //discounts.Where(d => discountLists.Contains(ds => d.Id).Any());
                    var ship = await context.Shipping.Include(ic => ic.ShippingItems).FirstOrDefaultAsync(shp => shp.ShippingId == shipid);
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
                            ObjectId = item.Product.ProductId,
                            ObjectIdType = (int)ObjectTypeCode.Product,
                            ProductPrice = priceValue,
                            Amount = Convert.ToInt32(item.Quantity)
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
                        ObjectIdType = (int)ObjectTypeCode.ShipType
                    });

                    priceValue = null;
                    price = priceList.Where(p => p.ObjectId == ship.Distance.DistanceId && p.ObjectTypeCode == (int)ObjectTypeCode.Distance).FirstOrDefault();
                    if (price != null)
                    {
                        priceValue = price.PriceValue;
                    }
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
                        ObjectIdType = (int)ObjectTypeCode.ShipType
                    });
                }

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


    }
}