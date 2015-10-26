using Michal.Project.Dal;
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
            var userContext = HttpContext.Current.GetOwinContext().Authentication;

            var user = new UserContext(userContext);

            OfferClient offerClient = new OfferClient();
            var demo = new OfferDemo();
            //offerClient.AddItems = demo.AddItems;
            //offerClient.Discounts = demo.Discounts;

            offerClient.Items = new List<OfferItem>();
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = " פריט מסוים",
                Desc = "מתוך המערכת פריט",
                IsDiscount = false,
                IsPresent = false,
                PriceValue = 1,
                StatusRecord = 1,
                ObjectId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                ObjectIdType = 1
            });
            offerClient.Items.Add(new OfferItem
            {
                Id = Guid.NewGuid(),
                Name = " פריט מסוים2",
                Desc = "מתוך המערכת פריט2",
                IsDiscount = false,
                IsPresent = true,
                PriceValue = null,
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
                PriceValue = 10,
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
            // var responseBody = "var claimsClient={};";
            // var loginExt = "var loginex='"+KipodealConfig.GetSingelton().ExtrnalLogin+"';";
            //try
            //{
            //    using (var unitofwork = new UnitOfWorkV3(new RequestDbContextStore(String.Empty), new StaticHttpContextPerRequestStore()))
            //    {
            //        var autorizeManager = new AutorizeUIManager();
            //        var user = unitofwork.CurrentUserContext;
            //        if (user != null && user.UserId.HasValue && user.UserId.Value != Guid.Empty)
            //        {
            //            var getAutrizeClient = autorizeManager.GetAutorizeByPageFileClientSide(unitofwork, pagefile);
            //            var data = JsonConvert.SerializeObject(getAutrizeClient.Result);
            //            responseBody = @"var claimsClient = " + data + ";";
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    responseBody = "var claimsClient={};";

            //}
            //responseBody += loginExt;
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