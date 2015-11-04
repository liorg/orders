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
using System.Web;
using System.Web.Http;



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
        public HttpResponseMessage GetOffer(Guid shipid, Guid offerId)
        {
            using (var context = new ApplicationDbContext())
            {
                var userContext = HttpContext.Current.GetOwinContext().Authentication;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                var organid = cache.GetOrg(context);
                var distances = cache.GetDistancesPerOrg(context, organid); //await context.Distance.Where(s => s.Organizations.Any(e => e.OrgId == orgId)).ToListAsync();
                var priceList = cache.GetPriceList(context);
                var shiptypeLists = cache.GetShipType(context);
                var products = cache.GetProducts(context,organid);
                var user = new UserContext(userContext);

                OfferClient offerClient = new OfferClient();

                offerClient.Distance = new List<OfferClientItem>();
                foreach (var distance in distances)
                {
                    decimal? priceValue=null;
                    var  price=priceList.Where(p=>p.ObjectId==distance.DistanceId && p.ObjectTypeCode==(int)ObjectTypeCode.Distance).FirstOrDefault();
                    if(price!=null){
                    priceValue=price.PriceValue;
                    }

                    offerClient.Distance.Add(new OfferClientItem
                    {
                        Amount = 1,
                        Desc = distance.Desc,
                        Id = distance.DistanceId,
                        IsDiscount = false,
                        IsPresent = false,
                        Name = distance.Name,
                        ProductPrice= priceValue,
                        StatusRecord = 1
                    });
                }

                offerClient.ShipType = new List<OfferClientItem>();
                foreach (var shiptypeItem in shiptypeLists)
                {
                    decimal? priceValue = null;
                    var price = priceList.Where(p => p.ObjectId == shiptypeItem.ShipTypeId && p.ObjectTypeCode == (int)ObjectTypeCode.ShipType).FirstOrDefault();
                    if (price != null)
                    {
                        priceValue = price.PriceValue;
                    }

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
                    decimal? priceValue = null;
                    var price = priceList.Where(p => p.ObjectId == productItem.ProductId && p.ObjectTypeCode == (int)ObjectTypeCode.Product).FirstOrDefault();
                    if (price != null)
                    {
                        priceValue = price.PriceValue;
                    }

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
                var demo = new OfferDemo();
             
                offerClient.Items = new List<OfferItem>();
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    Name = " פריט מסוים",
                    Desc = "מתוך המערכת פריט",
                    IsDiscount = false,
                    IsPresent = false,
                    ProductPrice = 1,
                    StatusRecord = 1,
                    Amount = 23,
                    ObjectId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    ObjectIdType = 1
                });
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    Name = " פריט מסוים2",
                    Desc = "מתוך המערכת פריט2",
                    IsDiscount = false,
                    IsPresent = false,
                    ProductPrice = null,
                    StatusRecord = 1,
                    ObjectId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    ObjectIdType = 1
                });
                offerClient.Items.Add(new OfferItem
                {
                    Id = Guid.NewGuid(),
                    Name = "הנחה שניה",
                    Desc = "מתוך המערכת הנחה שניה",
                    IsDiscount = true,
                    IsPresent = true,
                    Amount = 1,
                    ProductPrice = 10,
                    StatusRecord = 1,
                    ObjectId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    ObjectIdType = 2
                });

                offerClient.Discounts = (from d in demo.Discounts
                                         where !offerClient.Items.Any(o => o.ObjectIdType == 2 && o.ObjectId == d.Id)
                                         select d).ToList();

                offerClient.AddItems = (from d in demo.AddItems
                                        where !offerClient.Items.Any(o => o.ObjectIdType == 1 && o.ObjectId == d.Id)
                                        select d).ToList();
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