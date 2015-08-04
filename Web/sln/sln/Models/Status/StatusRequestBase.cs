using sln.Contract;
using sln.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models.Status
{
    public class StatusRequest : StatusRequestBase
    {
        public StatusRequest(StatusRequestBase requestBase)
        {
            this.Ship = requestBase.Ship;
            this.UserContext = requestBase.UserContext;
            this.AssignTo = requestBase.AssignTo;
        }
       // public IUserContext UserContext { get; set; }
        //public Shipping Ship { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public DateTime CurrentDate { get { return DateTime.Now; } }
        public int Status { get; set; }
        public int NotifyType { get; set; }
        public Guid StatusShipping { get; set; }

    }
    public class StatusRequestBase
    {
        public IUserContext UserContext { get; set; }
        public Shipping Ship { get; set; }
        public string AssignTo { get; set; }
    }
}