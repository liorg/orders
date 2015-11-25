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
                DayOfWeek = (int)DayOfWeek.Wednesday,
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
                DayOfWeek = (int)DayOfWeek.Wednesday,
                EndTime = TimeSpan.Parse("16:45"),
                StartTime = TimeSpan.Parse("13:00"),
                SpecialDate = null,
                Year = 2015,
                IsDayOff = false,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });
            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = (int)DayOfWeek.Thursday,
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
                DayOfWeek = (int)DayOfWeek.Friday,
                EndTime = TimeSpan.Parse("12:45"),
                StartTime = TimeSpan.Parse("09:15"),
                SpecialDate = null,
                Year = 2015,
                IsDayOff = true,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });

            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = (int)DayOfWeek.Saturday,
                EndTime = TimeSpan.Parse("12:45"),
                StartTime = TimeSpan.Parse("09:15"),
                SpecialDate = null,
                Year = 2015,
                IsDayOff = true,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });

            _data.Add(new MockBussinessClosure
            {
                BussinessClosureId = Guid.NewGuid(),
                DayOfWeek = (int)DayOfWeek.Sunday,
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
                DayOfWeek = (int)DayOfWeek.Monday,
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
                DayOfWeek = (int)DayOfWeek.Tuesday,
                EndTime = TimeSpan.Parse("12:45"),
                StartTime = TimeSpan.Parse("09:15"),
                SpecialDate = null,
                Year = 2015,
                IsDayOff = false,
                Name = "",
                ShippingCompany = Guid.NewGuid()
            });

            //_data.Add(new MockBussinessClosure
            //{
            //    BussinessClosureId = Guid.NewGuid(),
            //    DayOfWeek = 0,
            //    StartTime = TimeSpan.Parse("14:45"),
            //    EndTime = TimeSpan.Parse("15:35"),
            //    SpecialDate = DateTime.Now.Date,
            //    Year = 2015,
            //    IsDayOff = true,
            //    Name = "",
            //    ShippingCompany = Guid.NewGuid()
            //});
            //_data.Add(new MockBussinessClosure
            //{
            //    BussinessClosureId = Guid.NewGuid(),
            //    DayOfWeek = 0,
            //    StartTime = TimeSpan.Parse("8:45"),
            //    EndTime = TimeSpan.Parse("14:45"),
            //    SpecialDate = DateTime.Now.Date.AddDays(1),
            //    Year = 2015,
            //    IsDayOff = false,
            //    Name = "",
            //    ShippingCompany = Guid.NewGuid()
            //});
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
