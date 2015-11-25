using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtConsole.Mock
{
    public class MockData : IBussinessClosureRepository
    {
        List<IBussinessClosure> _data;
        public MockData()
        {
            _data = new List<IBussinessClosure>();

            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = 0,
                EndTime = TimeSpan.Parse("12:45"),
                StartTime = TimeSpan.Parse("09:15"),
                SpecialDate = null,
                Year = 2015,
                IsDayOff = false,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });
            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = 0,
                StartTime = TimeSpan.Parse("14:45"),
                EndTime = TimeSpan.Parse("15:35"),
                SpecialDate = DateTime.Now.Date,
                Year = 2015,
                IsDayOff = true,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });
            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = 0,
                StartTime = TimeSpan.Parse("8:45"),
                EndTime = TimeSpan.Parse("14:45"),
                SpecialDate = DateTime.Now.Date.AddDays(1),
                Year = 2015,
                IsDayOff = false,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });
        }
        /*
         תאריך ספציפי
         תרחיש 1 תאריך ב
         
         */

        public List<IBussinessClosure> GetByShipCompany(Guid shipCopanyId)
        {
            return _data;
        }
    }
}
