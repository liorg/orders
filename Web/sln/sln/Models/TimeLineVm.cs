using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class TimeLineVm
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Desc { get; set; }
        public Guid TimeLineId { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    public class TimeLinedDetailVm : TimeLineVm
    {
        public int ProgressBar { get; set; }

        public double StatusPresent
        {
            get
            {
                //       orderModel.ShippingVm.StatusPresent = shipping.StatusShipping.OrderDirection == 0 ? 0 : (double)(shipping.StatusShipping.OrderDirection /(double) Status.Max) * 100;
                if (ProgressBar == 0) return 0;

                return (double)(ProgressBar /(double) Michal.Project.Helper.Status.Max) * 100;
            }
        }
    }


}