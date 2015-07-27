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
                if (Status == 2)
                  return "danger";
                if (Status == 3)
                    return "success";
                return "default";
            }
        }

        public string Icon
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "fa-pencil";
                    case 2:
                        return "fa-rocket";
                    default:
                        return "fa-rocket";

                }
            }
        }

    }
}