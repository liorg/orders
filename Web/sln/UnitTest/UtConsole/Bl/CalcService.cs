using Michal.Project.Contract.DAL;
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

        public static TimeSpan AddMins(this TimeSpan ts, int addMins)
        {
            TimeSpan fromMin = TimeSpan.FromMinutes(addMins);
            var result = ts.Add(fromMin);
            return result;
        }
    }
    public class CalcService
    {
        readonly IBussinessClosureRepository _bussinessClosureRepository;

        public CalcService(IBussinessClosureRepository bussinessClosureRepository)
        {
            _bussinessClosureRepository = bussinessClosureRepository;
        }

        public DateTime Calc(Guid company, int timeleftOnMin, DateTime maxEndDate)
        {
            var data = _bussinessClosureRepository.GetByShipCompany(company);
            var dt = DateTime.Now;
           
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

                        var maxDt = minTimeOnly.AddMins(timeleftOnMin);
                        if (maxDt <= sepicialDay.EndTime)
                        {
                            dt = dt.Date.Add(maxDt);
                            hasFound = true;
                            continue;
                        }

                        TimeSpan getmin = maxDt - sepicialDay.EndTime;
                        timeleftOnMin = getmin.Minutes;

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
                        if (!hasFound)
                            break;

                        //if (minTimeOnly < bcitem.StartTime)
                        //    minTimeOnly = sepicialDay.StartTime;
                        if (sepicialDay.StartTime > minTimeOnly)
                            minTimeOnly = sepicialDay.StartTime;

                        var maxDt = minTimeOnly.AddMins(timeleftOnMin);
                        if (maxDt <= bcitem.EndTime)
                        {
                            dt = dt.Add(maxDt);
                            hasFound = true;
                            continue;
                        }
                        else
                        {
                            TimeSpan getmin = maxDt - bcitem.EndTime;
                            timeleftOnMin = getmin.Minutes;
                            minTimeOnly = maxDt;
                        }
                    }
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