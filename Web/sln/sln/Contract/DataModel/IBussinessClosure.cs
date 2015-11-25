using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface IBussinessClosure
    {
         Guid BussinessClosureId { get; set; }

         int DayOfWeek { get; set; }

         TimeSpan StartTime { get; set; }
         TimeSpan EndTime { get; set; }

         bool IsDayOff { get; set; }

         DateTime? SpecialDate { get; set; }

   
         int Year { get; set; }

         string Name { get; set; }
         Guid ShippingCompany { get; set; }
    }
}
