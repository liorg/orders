using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Bll
{
    public static class ExBu
    {

        public static TimeSpan GetTime(this DateTime dt)
        {
            TimeSpan min = dt.TimeOfDay;
            TimeSpan minOnly = new TimeSpan(min.Ticks - (min.Ticks % 600000000));
            return minOnly;
        }
        public static DateTime NextDay(this DateTime dt)
        {
            return dt.AddDays(1).Date;
        }

        public static TimeSpan AddMins(this TimeSpan ts, double addMins)
        {
            TimeSpan fromMin = TimeSpan.FromMinutes(addMins);
            var result = ts.Add(fromMin);
            return result;
        }
    }
    
    public class CalcService
    {
        const int MAX_DAYS = 30;

        readonly IBussinessClosureRepository _bussinessClosureRepository;
        readonly ISlaRepository _slaRepository;
        readonly IOrgDetailRepostory _orgDetailRepostory;
        public CalcService(IBussinessClosureRepository bussinessClosureRepository,
            ISlaRepository slaRepository, IOrgDetailRepostory orgDetailRepostory
            )
        {
            _bussinessClosureRepository = bussinessClosureRepository;
            _slaRepository = slaRepository;
            _orgDetailRepostory=orgDetailRepostory;
        }
        public void SetSla(ISlaValue sla)
        {
            Guid shipCopanyId = sla.ShippingCompany_ShippingCompanyId.GetValueOrDefault();
            Guid orgid = sla.Organization_OrgId.GetValueOrDefault();
            Guid distanceId = sla.Distance_DistanceId.GetValueOrDefault();
            Guid shipTypeId = sla.ShipType_ShipTypeId.GetValueOrDefault();

            var result = GetSla(shipCopanyId, orgid, distanceId, shipTypeId, sla.ActualStartDate);
            sla.SlaTime = result;
        }
      
        public DateTime GetSla(Guid shipCopanyId, Guid orgid, Guid distanceId, Guid shipTypeId, DateTime? starttime = null)
        {
            var minutes = _slaRepository.FindSlaOnMinute(shipCopanyId, orgid, distanceId, shipTypeId);
            var result = Calc(shipCopanyId, minutes, DateTime.Now.AddDays(MAX_DAYS), starttime);
            return result;
        }

        public DateTime Calc(Guid company, double timeleftOnMin, DateTime maxEndDate, DateTime? starttime = null)
        {
            var data = _bussinessClosureRepository.GetByShipCompany(company);

            var dt = starttime.HasValue ? starttime.Value : DateTime.Now;

            maxEndDate = maxEndDate.NextDay();
            bool hasFound = false;
            while (dt < maxEndDate && !hasFound)
            {
                TimeSpan minTimeOnly = dt.GetTime();
                hasFound = false;
                var items = data.Where(d => ((d.DayOfWeek == (int)dt.DayOfWeek) || (d.SpecialDate != null && d.SpecialDate.Value.Date == dt.Date))).AsEnumerable();
                if (items.Any())
                {
                    var sepicialDay = items.Where(sp => sp.SpecialDate != null).FirstOrDefault();
                    if (sepicialDay != null)
                    {
                        if (sepicialDay.IsDayOff)
                        {
                            dt = dt.NextDay();
                            continue;
                        }
                        if (sepicialDay.StartTime > minTimeOnly)
                            minTimeOnly = sepicialDay.StartTime;

                        if (minTimeOnly >= sepicialDay.EndTime)
                        {
                            dt = dt.NextDay();
                            continue;
                        }

                        var maxDt = minTimeOnly.AddMins(timeleftOnMin);
                        if (maxDt <= sepicialDay.EndTime)
                        {
                            dt = dt.Date.Add(maxDt);
                            hasFound = true;
                            continue;
                        }

                        TimeSpan getmin = maxDt - sepicialDay.EndTime;
                        timeleftOnMin = getmin.TotalMinutes;

                        dt = dt.NextDay();
                        continue;
                    }
                    if (items.Any(a => a.IsDayOff))
                    {
                        dt = dt.NextDay();
                        continue;
                    }

                    foreach (var bcitem in items.OrderBy(o => o.StartTime))
                    {
                        if (bcitem.StartTime > minTimeOnly)
                            minTimeOnly = bcitem.StartTime;

                        if (minTimeOnly >= bcitem.EndTime)
                            continue;

                        var maxDt = minTimeOnly.AddMins(timeleftOnMin);
                        if (maxDt <= bcitem.EndTime)
                        {
                            dt = dt.Add(maxDt);
                            hasFound = true;
                            break;
                        }
                        else
                        {
                            TimeSpan getmin = maxDt - bcitem.EndTime;
                            timeleftOnMin = getmin.TotalMinutes;
                            minTimeOnly = maxDt;
                            continue;
                        }
                    }//end loop for this day
                    if (!hasFound)
                    {
                        dt = dt.NextDay();
                        continue;
                    }
                }//end   if (items.Any())
                else
                    dt = dt.NextDay(); //next day if not found on table businessClosure

            }
            if (!hasFound)
                throw new ArgumentException("no found sla");

            return dt;
        }

        public List<SlaItem> Slas(Guid orgid,Guid companyid)
        {
            List<SlaItem> slas = new List<SlaItem>();
            var data = _slaRepository.GetAllSla(orgid, companyid);
            foreach (var item in data)
            {
                SlaItem slaItem = new SlaItem();
                slaItem.Desc = item.Name;
                slaItem.Distance = item.Distance.Name;
                slaItem.ShipType = item.ShipType.Name;
                slaItem.Mins = item.Mins.ToString();
                slas.Add(slaItem);
            }
            return slas;
        }

        public ShippingCompany GetCompany(Guid orgid, Guid? companyid)
        {
            return _orgDetailRepostory.GetShippingCompaniesByOrgId(orgid).Where(c => !companyid.HasValue || c.ShippingCompanyId == companyid.Value ).FirstOrDefault();
        }

        public List<BussinessClosureItem> GetBussinessClosure(Guid companyid)
        {
            var items = new List<BussinessClosureItem>();
            var data = _bussinessClosureRepository.GetByShipCompany(companyid).Where(d => !d.SpecialDate.HasValue || (d.SpecialDate.HasValue && !d.IsDayOff));
            foreach (var item in data)
            {
                var bussinessItem = new BussinessClosureItem();
                bussinessItem.Name = item.Name;
                bussinessItem.Start = item.StartTime.ToString();
                bussinessItem.End = item.EndTime.ToString();
                bussinessItem.DateSpiceial = item.SpecialDate.HasValue ? item.SpecialDate.Value.ToString("dd/MM/yyyy") : "";
                bussinessItem.IsDateOff = item.IsDayOff;

                items.Add(bussinessItem);
            }
            return items;
        }

        public List<BussinessClosureItem> GetDayOff(Guid companyid)
        {
            var items = new List<BussinessClosureItem>();
            var data = _bussinessClosureRepository.GetByShipCompany(companyid).Where(d=>d.SpecialDate.HasValue && d.IsDayOff) ;
            foreach (var item in data)
            {
                var bussinessItem = new BussinessClosureItem();
                bussinessItem.Name = item.Name;
                bussinessItem.Start = item.StartTime.ToString();
                bussinessItem.End = item.EndTime.ToString();
                bussinessItem.DateSpiceial = item.SpecialDate.HasValue ? item.SpecialDate.Value.ToString("dd/MM/yyyy") : "";
                bussinessItem.IsDateOff = item.IsDayOff;

                items.Add(bussinessItem);
            }
            return items;
        }
    }
}
/*

 Sunday = 0,
        //
        // Summary:
        //     Indicates Monday.
        Monday = 1,
        //
        // Summary:
        //     Indicates Tuesday.
        Tuesday = 2,
        //
        // Summary:
        //     Indicates Wednesday.
        Wednesday = 3,
        //
        // Summary:
        //     Indicates Thursday.
        Thursday = 4,
        //
        // Summary:
        //     Indicates Friday.
        Friday = 5,
        //
        // Summary:
        //     Indicates Saturday.
        Saturday = 6,
*/