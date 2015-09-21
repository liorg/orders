using Michal.Project.Dal;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Michal.Project.Models
{
    public class ViewListsRequest
    {
        //int? viewType,bool? viewAll, int? currentPage
        public int? ViewType { get; set; }
        public bool? ViewAll { get; set; }
        public int? CurrentPage { get; set; }
        public IQueryable<Shipping> shippingsQuery { get; set; }
        public Michal.Project.Contract.IUserContext UserContext { get; set; }
        public List<ViewItem> View { get; set; }
        public IPrincipal User { get; set; }
    }

    public class ViewListsResponse
    {
        public IEnumerable<ShippingVm> shippingsQuery { get; set; }
        public string StatusDesc { get; set; }
    }
}