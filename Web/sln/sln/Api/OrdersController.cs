using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Michal.Project.Api
{
    //[Authorize]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {  
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            bool isadmin=User.IsInRole(Helper.HelperAutorize.RoleAdmin);
            bool isroleUser = User.IsInRole(Helper.HelperAutorize.RoleUser);
            return Ok(Order.CreateOrders(user));
        }

        [Route("GetValid")]
        public IHttpActionResult GetValid()
        {
            if (!User.Identity.IsAuthenticated) 
                return Unauthorized(new AuthenticationHeaderValue("hey","unt"));
            var userContext = HttpContext.Current.GetOwinContext().Authentication;
            var user = new UserContext(userContext);
            bool isadmin = User.IsInRole(Helper.HelperAutorize.RoleAdmin);
            bool isroleUser = User.IsInRole(Helper.HelperAutorize.RoleUser);
            return Ok(Order.CreateOrders(user));
        }
    } 

    #region Helpers

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }
        public string UserName { get; set; }

        public static List<Order> CreateOrders( UserContext userContext)
        {
            List<Order> OrderList = new List<Order> 
            {
                new Order {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true ,UserName=userContext.FullName},
                new Order {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false ,UserName=userContext.FullName},
                new Order {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false  ,UserName=userContext.FullName},
                new Order {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false ,UserName=userContext.FullName},
                new Order {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true ,UserName=userContext.FullName}
            };

            return OrderList;
        }
    }

    #endregion
}