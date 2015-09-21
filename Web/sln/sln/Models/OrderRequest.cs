using Michal.Project.Dal;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderRequest
    {
        //int? viewType,bool? viewAll, int? currentPage
      //Shipping shipping, List<Runner> runners
        public Shipping Shipping { get; set; }
        public List<Runner> Runners { get; set; }
        public UserContext UserContext { get; set; }

    }

    //public class ViewListsResponse
    //{
    //    public IEnumerable<ShippingVm> shippingsQuery { get; set; }
    //    public string StatusDesc { get; set; }
    //}
}