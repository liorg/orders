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
using Michal.Project.Fasade;
using Michal.Project.Bll;
using Michal.Project.Contract.DAL;


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
        public async Task<HttpResponseMessage> GetOffer(Guid shipid, Guid offerId, Guid shippingCompanyId)
        {
            using (var context = new ApplicationDbContext())
            {
                bool allowRemove = false;
                //    bool allowAdd = false;
                bool allowEdit = false;
                var userContext = HttpContext.Current.GetOwinContext().Authentication;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                   (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
                {
                    allowRemove = true;
                    //  allowAdd = true;
                    allowEdit = true;
                }
                IOfferRepository offerRepository = new OfferRepository(context);
                IShippingRepository shippingRepository = new ShippingRepository(context);
                GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
                var user = new UserContext(userContext);
                OrderLogic logic = new OrderLogic(offerRepository, shippingRepository, generalRepo, generalRepo);

                var ship = await shippingRepository.GetShipIncludeItems(shipid);
                OfferClient offerClient = logic.GetOfferClient(allowRemove, allowEdit, ship, shippingCompanyId, user);
                if (offerId == Guid.Empty)
                {
                    logic.AppendNewOffer(offerClient, ship, allowRemove, allowEdit);
                }
                else
                {
                    var offer = await offerRepository.GetOfferAndHisChilds(offerId);
                    logic.AppendCurrentOffer(offerClient, ship, offer, allowRemove, allowEdit);
                }
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
        [Route("CommitOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> CommitOffer([FromBody] OfferUpload offer)
        {
            var result = new Result();
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            using (var context = new ApplicationDbContext())
            {
                OfferManager offermanager = new OfferManager();
                result=await offermanager.CommitAsync(context, offer, user);

            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [Route("CancelOffer")]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> CancelOffer([FromBody] OfferUpload offer)
        {
            var result = new Result();
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            using (var context = new ApplicationDbContext())
            {
                OfferManager offermanager = new OfferManager();
                result = await offermanager.CancelAsync(context, offer, user);

            }
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<Result>(result,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
    }
}
