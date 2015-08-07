using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class TimeLineVm
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Desc { get; set; }
        public Guid TimeLineId { get; set; }
        public DateTime CreatedOn { get; set; }

        public string Circle
        {
            get
            {
                if (Status == Helper.TimeStatus.New)
                    return "default";
                if (Status == Helper.TimeStatus.ApporvallRequest || Status == Helper.TimeStatus.AcceptByRunner || Status == Helper.TimeStatus.AcceptByClient)
                    return "success";
                if (Status == Helper.TimeStatus.CancelByAdmin || Status == Helper.TimeStatus.Cancel || Status == Helper.TimeStatus.NoAcceptByClient || Status == Helper.TimeStatus.PrevStep)
                    return "danger";
                if (Status == Helper.TimeStatus.Confirm || Status == Helper.TimeStatus.Arrived || Status == Helper.TimeStatus.Close || Status == Helper.TimeStatus.ChangePrice)
                    return "info";
                return "default";
            }
        }

        public string Icon
        {
            get
            {
                switch (Status)
                {
                    case Helper.TimeStatus.New:
                        return "fa-pencil";
                    case Helper.TimeStatus.ApporvallRequest:
                        return "fa-paper-plane";
                    case Helper.TimeStatus.Confirm:
                        return "fa-thumbs-o-up";
                    case Helper.TimeStatus.CancelByAdmin:
                        return "fa-thumbs-o-down";
                    case Helper.TimeStatus.Cancel:
                        return "fa-times";
                    case Helper.TimeStatus.AcceptByRunner:
                        return "fa-road";
                    case Helper.TimeStatus.Arrived:
                        return "fa-map-marker";
                    case Helper.TimeStatus.NoAcceptByClient:
                        return "fa-hand-rock-o";
                    case Helper.TimeStatus.AcceptByClient:
                        return "fa-gift";  
                    default:
                        return "fa-rocket";

                }
            }
        }

    }
}