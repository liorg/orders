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
                if (Status == Helper.TimeStatus.ApporvallRequest)
                    return "success";
                if (Status == Helper.TimeStatus.CancelByAdmin || Status==Helper.TimeStatus.Cancel)
                    return "danger";
                if (Status == Helper.TimeStatus.Confirm)
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
                        
                    default:
                        return "fa-rocket";

                }
            }
        }

    }
}