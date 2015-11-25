using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtConsole.Mock
{
    public class MockBussinessClosure : IBussinessClosure
    {
        public Guid BussinessClosureId
        { get; set; }

        public int DayOfWeek
        { get; set; }

        public TimeSpan StartTime
        { get; set; }

        public TimeSpan EndTime
        { get; set; }

        public bool IsDayOff
        { get; set; }

        public DateTime? SpecialDate
        { get; set; }

        public int Year
        { get; set; }

        public string Name
        { get; set; }

        public Guid ShippingCompany { get; set; }

    }
}
