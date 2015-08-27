using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Michal.Project.Models;
using Michal.Project.Helper;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Michal.Project.Bll;
using Kipodeal.Helper.Cache;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Data;
using Michal.Project.Providers;

namespace Michal.Project.Controllers
{
    [Authorize]
    public class ExcelController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Export(string nextMonth, string prevMonth)
        {
            using (var context = new ApplicationDbContext())
            {
                Guid draf=Guid.Parse(Status.Draft);
                var user = new UserContext(AuthenticationManager);
                Guid orgId = Guid.Empty;
                if (User.IsInRole(HelperAutorize.RoleAdmin) || User.IsInRole(HelperAutorize.RoleRunner))
                    orgId = Guid.Empty; //user.OrgId;
                //We load the data
                var fromMonth = DateTime.Today.AddMonths(-1).Date;
                var toMonth = DateTime.Today.AddMonths(1).Date;
                if (!String.IsNullOrEmpty(nextMonth))
                    toMonth = DateTime.ParseExact(nextMonth, "yyyy-MM-dd", null);

                if (!String.IsNullOrEmpty(prevMonth))
                    fromMonth = DateTime.ParseExact(prevMonth, "yyyy-MM-dd", null);

                var shippingsQuery = (from s in context.Shipping
                                      join u in context.Users
                                      on s.OwnerId.Value.ToString() equals u.Id
                                      where s.OwnerId.HasValue == true
                                      && s.StatusShipping != null && s.StatusShipping.StatusShippingId != draf
                                      && s.CreatedOn > fromMonth && s.CreatedOn <= toMonth
                                      select new
                                      {
                                          Name = s.Name,
                                          OwnerFirstName = u.FirstName,
                                          OwnerLastName = u.LastName,
                                          Department = u.Department,
                                          Price = s.ActualPrice,
                                          Status = s.StatusShipping.Desc,

                                         FromStreet=s.AddressFrom,
                                          FromNum=s.AddressNumFrom,
                                          FromCity = s.CityFromName,

                                          ToStreet = s.AddressTo,
                                          ToNum = s.AddressNumTo,
                                          ToCity = s.CityToName,

                                      }).ToList();
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable(toMonth.ToString("MM")));
                var dt = ds.Tables[0];
                dt.Columns.Add(new DataColumn("מספר משלוח", typeof(string)));
                dt.Columns.Add(new DataColumn("שם מלא", typeof(string)));
                dt.Columns.Add(new DataColumn("מחלקה", typeof(string)));
                dt.Columns.Add(new DataColumn("סטאטוס", typeof(string)));
                dt.Columns.Add(new DataColumn("מחיר", typeof(decimal)));
                dt.Columns.Add(new DataColumn("ממקום", typeof(string)));
                dt.Columns.Add(new DataColumn("למקום", typeof(string)));
                foreach (var ship in shippingsQuery)
                {
                    var row = dt.NewRow();
                    row["מספר משלוח"] = ship.OwnerFirstName + " " + ship.OwnerLastName;
                    row["שם מלא"] = ship.Name;
                    row["מחלקה"] = ship.Department;
                    row["סטאטוס"] = ship.Status;
                    row["מחיר"] = ship.Price;
                    row["ממקום"] = ship.FromStreet + " " + ship.FromNum + " " + ship.FromCity;
                    row["למקום"] = ship.ToStreet + " " + ship.ToNum + " " + ship.ToCity;

                    dt.Rows.Add(row);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    ExcelProvider.ExportDSToExcel(ds, ms);
                    var fName = string.Format("דוח חודשי-{0}.xlsx", toMonth);

                    byte[] fileContents = ms.ToArray();

                    return File(fileContents, "application/vnd.ms-excel", fName);
                }
            }
        }
    }
}