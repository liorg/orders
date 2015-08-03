using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class ViewItem
    {
        public int StatusId { get; set; }
        public string StatusDesc { get; set; }

        public Func<sln.DataModel.Shipping, sln.Contract.IUserContext, bool> GetOnlyMyRecords { get; set; }

        //public Func<sln.Contract.IUserContext, bool> GetDefaultView { get; set; }

        //public bool IsDefaultAdmin { get; set; }
        //public bool IsDefaultOrgManager { get; set; }
        //public bool IsDefaultAccept { get; set; }
        //public bool IsDefaultUser { get; set; }
        //public bool IsDefaultRunner { get; set; }

        //public bool IsVisbleForAdmin { get; set; }
        //public bool IsVisbleForOrgManager { get; set; }
        //public bool IsVisbleForAccept { get; set; }
        //public bool IsVisbleForUser { get; set; }
        //public bool IsVisbleForRunner { get; set; }
    }
}