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

        //    public  ActionResult bussinessClosure((Guid companyid)
        //    {
        //}
        public ActionResult Details(Guid? companyid)
        {
            CompanyVm coompanyVm = new CompanyVm();
           
            using (var context = new ApplicationDbContext())
            {
                UserContext user = new UserContext(AuthenticationManager);
                GeneralAgentRepository repository = new GeneralAgentRepository(context);
                var orgId = repository.GetOrg();
                CalcService calc = new CalcService(repository, repository, repository);
                
                var company = calc.GetCompany(orgId, companyid);
                coompanyVm.Name = company.Name;
                coompanyVm.Id = company.ShippingCompanyId;
               
            }
            return View(coompanyVm);
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