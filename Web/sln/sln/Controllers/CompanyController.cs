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

namespace Michal.Project.Controllers
{
    // [Authorize]
    public class CompanyController : Controller
    {

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Details(Guid? companyid)
        {
            CompanyVm companyVm = new CompanyVm();

            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);

                var company = calc.GetCompany(orgId, companyid);
                companyVm.Name = company.Name;
                companyVm.Id = company.ShippingCompanyId;
                companyVm.ContractTel = String.IsNullOrWhiteSpace(company.ContactTel) ? "אין פרטים" : company.ContactTel;
                companyVm.ContactFullName = String.IsNullOrWhiteSpace(company.ContactFullName) ? "אין פרטים" : company.ContactFullName;
               
                companyVm.Address = new AddressEditorViewModel();
                companyVm.Address.City = company.AddressCompany.CityName;
                companyVm.Address.ExtraDetail = company.AddressCompany.ExtraDetail;
                companyVm.Address.Num = company.AddressCompany.StreetNum;
                companyVm.Address.Street = company.AddressCompany.StreetName;

                companyVm.Desc= String.IsNullOrWhiteSpace( company.Desc)?"חברת שליחיות":company.Desc;
            }
            return View(companyVm);
        }

        public ActionResult BussinessClosure(Guid companyid)
        {
            var bussinessClosureView = new BussinessClosureView();
            bussinessClosureView.Id = companyid;
            using (var context = new ApplicationDbContext())
            {

                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                bussinessClosureView.Items = calc.GetBussinessClosure(companyid);
                var company = calc.GetCompany(orgId, companyid);
                bussinessClosureView.Name = company.Name;
                return View(bussinessClosureView);
            }
        }

        public ActionResult DaysOff(Guid companyid)
        {
            var bussinessClosureView = new BussinessClosureView();
            bussinessClosureView.Id = companyid;
            using (var context = new ApplicationDbContext())
            {

                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                bussinessClosureView.Items = calc.GetDayOff(companyid);
                var company = calc.GetCompany(orgId, companyid);
                bussinessClosureView.Name = company.Name;
                return View(bussinessClosureView);
            }
        }
        public ActionResult Sla(Guid companyid)
        {
            SlaView sla = new SlaView();
            sla.Id = companyid;
            using (var context = new ApplicationDbContext())
            {

                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                sla.Items = calc.Slas(orgId, companyid);
                var company = calc.GetCompany(orgId, companyid);
                sla.Name = company.Name;
                return View(sla);
            }
        }



    }
}