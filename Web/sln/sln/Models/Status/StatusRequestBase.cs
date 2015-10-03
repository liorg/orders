using Michal.Project.Contract;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models.Status
{
    public class StatusRequest : StatusRequestBase
    {
        public StatusRequest(StatusRequestBase requestBase)
        {
            this.Ship = requestBase.Ship;
            this.UserContext = requestBase.UserContext;
            this.AssignTo = requestBase.AssignTo;
            this.EndDesc = requestBase.EndDesc;
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
        public string EndDesc { get; set; }
    }
}