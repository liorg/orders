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
            this.ActualStartDate = requestBase.ActualStartDate;
            this.ActualEndDate = requestBase.ActualEndDate;
            this.TimeWaitStartSend = requestBase.TimeWaitStartSend;
            this.TimeWaitEndSend = requestBase.TimeWaitEndSend;
            this.TimeWaitStartGet = requestBase.TimeWaitStartGet;
            this.TimeWaitEndGet = requestBase.TimeWaitEndGet;
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
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        public DateTime? TimeWaitStartSend { get; set; }//when send package (last session)
        public DateTime? TimeWaitEndSend { get; set; }//when send package (last session)

        public DateTime? TimeWaitStartGet { get; set; } //when get package
        public DateTime? TimeWaitEndGet { get; set; }//when get package
    }

}