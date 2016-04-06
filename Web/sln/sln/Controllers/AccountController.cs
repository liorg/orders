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
using Michal.Project.Models.View;
using Michal.Project.Agent;
using Michal.Project.Mechanism.Sync.User;
using Michal.Project.Fasade;

namespace Michal.Project.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ApplicationDbContext DBContext;

        public AccountController()
            : this(null)
        {

        }

        public AccountController(UserManager<ApplicationUser> userManager = null)
        {
            DBContext = new ApplicationDbContext();

            if (userManager == null)
                userManager = new ApplicationUserManager(DBContext);


            UserManager = userManager;
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }

        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public async Task<ActionResult> Index(int? currentPage)
        {
            var userContext = new UserContext(AuthenticationManager);
            IQueryable<ApplicationUser> usersQuery;
            //var users = DBContext.Users;
            GeneralAgentRepository repository = new GeneralAgentRepository(DBContext);
            var org = repository.GetOrgEntity();

            var inMemo = new List<EditUserViewModel>();
;
            usersQuery = DBContext.Users.Where(u => u.IsClientUser == true);
            var total = await usersQuery.CountAsync();
            int page = currentPage.HasValue ? currentPage.Value : 1;
            var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

            var usersData = await usersQuery.OrderByDescending(ord => ord.UserName).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();


            foreach (var user in usersData)
            {
                var edit = new EditUserViewModel(user);
                inMemo.Add(edit);
            }
            UsersView usersView = new UsersView();
            usersView.Name = org.Name;
            usersView.Id = org.OrgId;
            usersView.Items = inMemo;
            usersView.ClientViewType = ClientViewType.Users;
            usersView.CurrentPage = page;
            usersView.MoreRecord = hasMoreRecord;
            usersView.Title = "משתמשים";
            usersView.Total = total;

            return View(usersView);
        }


        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public async Task<ActionResult> GetShippers(int? currentPage)
        {
            var userContext = new UserContext(AuthenticationManager);
            IQueryable<ApplicationUser> usersQuery;
            GeneralAgentRepository repository = new GeneralAgentRepository(DBContext);
            var orgId = repository.GetOrg();
            CalcService calc = new CalcService(repository, repository, repository);

            //var users = DBContext.Users;
            var inMemo = new List<EditUserViewModel>();

            //if (!User.IsInRole(HelperAutorize.RoleAdmin))
            //    usersQuery = DBContext.Users.Where(u => u.Organization_OrgId.HasValue && u.Organization_OrgId.Value == userContext.OrgId);
            //else
            usersQuery = DBContext.Users.Where(u => u.IsClientUser == false);
            var total = await usersQuery.CountAsync();
            int page = currentPage.HasValue ? currentPage.Value : 1;
            var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);

            var usersData = await usersQuery.OrderByDescending(ord => ord.UserName).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).ToListAsync();


            foreach (var user in usersData)
            {
                var edit = new EditUserViewModel(user);
                inMemo.Add(edit);
            }
            UsersView usersView = new UsersView();
            var company = calc.GetCompany(orgId, null);
            usersView.Id = company.ShippingCompanyId;
            usersView.Name = company.Name;
            usersView.Items = inMemo;
            usersView.ClientViewType = ClientViewType.Users;
            usersView.CurrentPage = page;
            usersView.MoreRecord = hasMoreRecord;
            usersView.Title = "משתמשים";
            usersView.Total = total;

            return View(usersView);
        }



        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager, HelperAutorize.RunnerManager)]
        public async Task<ActionResult> Edit(string id, ManageMessageId? Message = null)
        {
            // var Db = new ApplicationDbContext();
            //  using (var context = new ApplicationDbContext())
            {
                var context = DBContext;
                var grantUsers = await context.Users.Where(u => u.IsActive == true && u.Roles.Any(r => r.Role != null && (r.Role.Name == HelperAutorize.ApprovalExceptionalBudget || r.Role.Name == HelperAutorize.RoleOrgManager))

                    ).Select((s) => new
                    {
                        Id = s.Id,
                        Title = s.FirstName + " " + s.LastName + "(" + s.EmpId + ")"
                    })
                        .ToListAsync();

                var user = await context.Users.FirstAsync(u => u.Id == id);
                //var user = context.Users.First(u => u.Id == id);
                var model = new EditUserViewModel(user);
                if (user.Roles != null && user.Roles.Any())
                {
                    foreach (var item in user.Roles)
                    {
                        if (item.Role != null)
                        {
                            if (item.Role.Name == Helper.HelperAutorize.RoleAdmin)
                                model.IsAdmin = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleUser)
                                model.IsCreateOrder = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleRunner)
                                model.IsRunner = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleOrgManager)
                                model.IsOrgMangager = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleAccept)
                                model.IsAcceptOrder = true;
                            if (item.Role.Name == Helper.HelperAutorize.ApprovalExceptionalBudget)
                                model.IsApprovalExceptionalBudget = true;
                        }
                    }
                }
                ViewBag.GrantUsers = new SelectList(grantUsers, "Id", "Title");

                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");
                ViewBag.MessageId = Message;
                return View(model);
            }
        }

        public async Task<ActionResult> Profile(string id)
        {
            // var Db = new ApplicationDbContext();
            //  using (var context = new ApplicationDbContext())
            {
                if (String.IsNullOrWhiteSpace(id))
                {
                    id = User.Identity.GetUserId();
                }
                var context = DBContext;
                var grantUsers = await context.Users.Where(u => u.IsActive == true && u.Roles.Any(r => r.Role != null && (r.Role.Name == HelperAutorize.ApprovalExceptionalBudget || r.Role.Name == HelperAutorize.RoleOrgManager))

                    ).Select((s) => new
                    {
                        Id = s.Id,
                        Title = s.FirstName + " " + s.LastName + "(" + s.EmpId + ")"
                    })
                        .ToListAsync();

                var user = await context.Users.FirstAsync(u => u.Id == id);
                //var user = context.Users.First(u => u.Id == id);
                var model = new UserDetail(user);
                if (user.Roles != null && user.Roles.Any())
                {
                    foreach (var item in user.Roles)
                    {
                        if (item.Role != null)
                        {
                            if (item.Role.Name == Helper.HelperAutorize.RoleAdmin)
                                model.IsAdmin = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleUser)
                                model.IsCreateOrder = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleRunner)
                                model.IsRunner = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleOrgManager)
                                model.IsOrgMangager = true;
                            if (item.Role.Name == Helper.HelperAutorize.RoleAccept)
                                model.IsAcceptOrder = true;
                            if (item.Role.Name == Helper.HelperAutorize.ApprovalExceptionalBudget)
                                model.IsApprovalExceptionalBudget = true;
                        }
                    }
                }
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager, HelperAutorize.RunnerManager)]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            //if (ModelState.IsValid)
            {

                {
                    var id = model.UserId.ToString();
                    var viewLogic = new ViewLogic();
                    MemeryCacheDataService cache = new MemeryCacheDataService();
                    
                    var context = DBContext;
                    var org=cache.GetOrgEntity(context);
                    var orgid = org.OrgId;
                    LocationAgent location = new LocationAgent(cache);
                    var user = await context.Users.FirstAsync(u => u.Id == id);
                    // Update the user data:
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.IsActive = model.IsActive;
                    user.EmpId = model.EmpId;
                    user.Tel = model.Tel;
                    user.GrantUserManager = model.GrantUserManager;
                    user.IsClientUser = model.IsClientUser;
                    user.Department = model.Department;
                    if (user.IsClientUser)
                    {
                        await location.SetLocationAsync(model.Address, user.AddressUser);

                        user.AddressUser.ExtraDetail = model.Address.ExtraDetail;
                    }
                    else
                    {
                        if (model.CompanyId == Guid.Empty)
                        {
                            var defaultCompany = cache.GetShippingCompaniesByOrgId(context, orgid).FirstOrDefault();
                            model.CompanyId = defaultCompany != null ? defaultCompany.ShippingCompanyId : Guid.Empty;
                        }
                             user.ShippingCompany_ShippingCompanyId = model.CompanyId;
                    }
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;

                    viewLogic.SetViewerUserByRole(model, user);

                    await context.SaveChangesAsync();

                    if (user.Roles != null && user.Roles.Any())
                    {
                        var cloneRules = new List<string>();
                        user.Roles.Where(rw => rw.Role != null && !String.IsNullOrEmpty(rw.Role.Name)).ToList().ForEach(rr => cloneRules.Add(rr.Role.Name));

                        foreach (var role in cloneRules)
                            await UserManager.RemoveFromRoleAsync(model.UserId, role);
                    }

                    if (model.IsAdmin) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAdmin);// "Admin");

                    if (model.IsCreateOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleUser);//, "User");

                    if (model.IsOrgMangager) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleOrgManager);//"OrgManager");

                    if (model.IsRunner) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleRunner);// "Runner");

                    if (model.IsAcceptOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAccept);// "Accept");

                    if (model.IsApprovalExceptionalBudget) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.ApprovalExceptionalBudget);//// "ApprovalExceptionalBudget");

                    if (model.IsClientUser)
                        return RedirectToAction("Index", "Account");
                    else
                        return RedirectToAction("GetShippers", "Account");

                   SyncManager syncManager = new SyncManager();
                   await syncManager.Push(new UserUpdateData(context, model));

                    
                    
                    //return RedirectToAction("Index");

                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(string id = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new SelectUserRolesViewModel(user);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserRoles(user.Id);
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddUserToRole(user.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // using (ApplicationDbContext db = new ApplicationDbContext())
            //  {
            //   ViewBag.Orgs = new SelectList(db.Organization.ToList(), "OrgId", "Name");

            //  }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    MemeryCacheDataService cache = new MemeryCacheDataService();
                    var orgs = cache.GetOrgs(context); //await context.Organization.ToListAsync();
                    //  ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                    //  var org = orgs.Where(o => o.OrgId == model.OrgId).FirstOrDefault();
                    var orgId = cache.GetOrg(context);
                    var org = orgs.Where(o => o.OrgId == orgId).FirstOrDefault();
                    var userName = model.UserName;
                    if (!model.IsDeliveryBoy)
                    {
                        userName = model.UserName + "@" + org.Domain;
                    }
                    //if (org.Name != General.OrgWWW)
                    //{
                    //    userName = model.UserName + "@" + org.Domain;
                    //}
                    var user = await UserManager.FindAsync(userName, model.Password);
                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe, org);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "שם משתמש או סיסמא לא תקינים");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager, HelperAutorize.RunnerManager)]
        public async Task<ActionResult> Register(bool? isSupplier = false)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.IsClientUser = true;
            model.Address = new AddressEditorViewModel();
            //using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var db = DBContext;

                var grantUsers = await db.Users.Where(u => u.IsActive == true && u.Roles.Any(r => r.Role != null && (r.Role.Name == HelperAutorize.ApprovalExceptionalBudget || r.Role.Name == HelperAutorize.RoleOrgManager))

                   ).Select((s) => new
                   {
                       Id = s.Id,
                       Title = s.FirstName + " " + s.LastName + "(" + s.EmpId + ")"
                   })
                       .ToListAsync();

                MemeryCacheDataService cache = new MemeryCacheDataService();

                var orgs = cache.GetOrgs(db);
                var orgid = cache.GetOrg(db);

                var org = orgs.Where(o => o.OrgId == orgid).FirstOrDefault();
                if (isSupplier.HasValue && isSupplier.Value)
                {
                    var defaultCompany = cache.GetShippingCompaniesByOrgId(db, orgid).FirstOrDefault();
                    model.CompanyId = defaultCompany != null ? defaultCompany.ShippingCompanyId : Guid.Empty;
                    var orgWWW = orgs.Where(w => w.Name == General.OrgWWW).FirstOrDefault();
                    model.OrgId = orgWWW.OrgId;
                    model.IsClientUser = false;
                    //   model.c
                }

                if (org != null)
                {
                    model.Address.ExtraDetail = org.AddressOrg.ExtraDetail;
                    model.Address.City = org.AddressOrg.CityName;
                    model.Address.Citycode = org.AddressOrg.CityCode;
                    model.Address.CitycodeOld = model.Address.Citycode;
                    model.Address.IsSensor = org.AddressOrg.IsSensor;
                    model.Address.Lat = org.AddressOrg.Lat;
                    model.Address.Lng = org.AddressOrg.Lng;
                    model.Address.Num = org.AddressOrg.StreetNum;
                    model.Address.Street = org.AddressOrg.StreetName;
                    model.Address.Streetcode = org.AddressOrg.StreetCode;
                    model.Address.StreetcodeOld = model.Address.Streetcode;
                    model.Address.UId = org.AddressOrg.UID;
                }
                ViewBag.GrantUsers = new SelectList(grantUsers, "Id", "Title");

                ViewBag.Orgs = new SelectList(db.Organization.ToList(), "OrgId", "Name");

            }
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager, HelperAutorize.RunnerManager)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //using (var context = new ApplicationDbContext())
            {
                var context = DBContext;
                MemeryCacheDataService cache = new MemeryCacheDataService();
                LocationAgent location = new LocationAgent(cache);
                var viewLogic = new ViewLogic();

                if (!User.IsInRole(Helper.HelperAutorize.RoleAdmin))
                {
                    Guid orgId = Guid.Empty;
                    ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        if (claim.Type == ClaimTypes.GroupSid)
                        {
                            orgId = Guid.Parse(claim.Value);

                            break;
                        }
                    }
                    model.OrgId = orgId;
                }

                var orgs = await context.Organization.ToListAsync();
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                var org = orgs.Where(o => o.OrgId == model.OrgId).FirstOrDefault();
                var userName = model.UserName;
                if (org.Name != General.OrgWWW)
                    userName = model.UserName + "@" + org.Domain;

                //if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = userName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        IsActive = true,
                        EmpId = model.EmpId,
                        Organization_OrgId = model.OrgId,
                        Tel = model.Tel,
                        GrantUserManager = model.GrantUserManager,
                        Department = model.Department,
                        IsClientUser = model.IsClientUser
                    };
                    user.AddressUser = new Address();
                    if (user.IsClientUser)
                    {
                        await location.SetLocationAsync(model.Address, user.AddressUser);
                        user.AddressUser.ExtraDetail = model.Address.ExtraDetail;
                    }
                    else
                    {
                        if (model.CompanyId != Guid.Empty)
                            user.ShippingCompany_ShippingCompanyId = model.CompanyId;
                    }
                    viewLogic.SetViewerUserByRole(model, user);


                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (model.IsAdmin) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAdmin);

                    if (model.IsCreateOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleUser);

                    if (model.IsOrgMangager) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleOrgManager);

                    if (model.IsRunner) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleRunner);

                    if (model.IsAcceptOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAccept);

                    if (model.IsApprovalExceptionalBudget) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.ApprovalExceptionalBudget);


                    if (result.Succeeded)
                    {
                        if (model.IsClientUser)
                            return RedirectToAction("Index", "Account");
                        else
                            return RedirectToAction("GetShippers", "Account");
                    }
                    else
                        AddErrors(result);

                }

                return View(model);
            }
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    UserManager = null;
                }
                if (DBContext != null)
                {
                    DBContext.Dispose();
                    DBContext = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent, Organization org)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            Helper.JobType jobType = Helper.JobType.Client;
            string jobTitle = Helper.JobTitle.Client;
            var claimAllRoles = identity.Claims.Where(ccl => ccl.Type == ClaimTypes.Role).AsEnumerable();
            foreach (var claimRole in claimAllRoles)
            {
                if (claimRole != null && !String.IsNullOrWhiteSpace(claimRole.Value))
                {
                    if (claimRole.Value == Helper.HelperAutorize.RoleAdmin)
                    {
                        jobType = Helper.JobType.Admin;
                        jobTitle = Helper.JobTitle.Admin;
                        break;
                    }
                    if (claimRole.Value == Helper.HelperAutorize.RoleRunner)
                    {
                        jobType = Helper.JobType.Runner;
                        jobTitle = Helper.JobTitle.DeliveryBoy;
                    }
                }
            }
            HelperSecurity.SetClaims(identity, user, org, jobType, jobTitle);

            //identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
            //identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

            //identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            //identity.AddClaim(new Claim(ClaimTypes.GroupSid, org.OrgId.ToString()));
            //identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
            //identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));

            //identity.AddClaim(new Claim(CustomClaimTypes.ShowAllView, user.ViewAll.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.DefaultView, user.DefaultView.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.Tel, user.Tel.ToString()));

            //identity.AddClaim(new Claim(CustomClaimTypes.City, user.AddressUser.CityName.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.CityCode, user.AddressUser.CityCode.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.Street, user.AddressUser.StreetName.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.StreetCode, user.AddressUser.StreetCode.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.Num, user.AddressUser.StreetNum.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.External, String.IsNullOrEmpty(user.AddressUser.ExtraDetail) ? "" : user.AddressUser.ExtraDetail.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.UID, user.AddressUser.UID.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.Lat, user.AddressUser.Lat.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.Lng, user.AddressUser.Lng.ToString()));
            //identity.AddClaim(new Claim(CustomClaimTypes.GrantUser, user.GrantUserManager.GetValueOrDefault().ToString()));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        
        private async Task SignInAsyncOld(ApplicationUser user, bool isPersistent, Organization org)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            Helper.JobType jobType = Helper.JobType.Client;
            string jobTitle = Helper.JobTitle.Client;
            var claimAllRoles = identity.Claims.Where(ccl => ccl.Type == ClaimTypes.Role).AsEnumerable();
            foreach (var claimRole in claimAllRoles)
            {
                if (claimRole != null && !String.IsNullOrWhiteSpace(claimRole.Value))
                {
                    if (claimRole.Value == Helper.HelperAutorize.RoleAdmin)
                    {
                        jobType = Helper.JobType.Admin;
                        jobTitle = Helper.JobTitle.Admin;
                        break;
                    }
                    if (claimRole.Value == Helper.HelperAutorize.RoleRunner)
                    {
                        jobType = Helper.JobType.Runner;
                        jobTitle = Helper.JobTitle.DeliveryBoy;
                    }
                }
            }
            identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
            identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, org.OrgId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));

            identity.AddClaim(new Claim(CustomClaimTypes.ShowAllView, user.ViewAll.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.DefaultView, user.DefaultView.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Tel, user.Tel.ToString()));

            identity.AddClaim(new Claim(CustomClaimTypes.City, user.AddressUser.CityName.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.CityCode, user.AddressUser.CityCode.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Street, user.AddressUser.StreetName.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.StreetCode, user.AddressUser.StreetCode.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Num, user.AddressUser.StreetNum.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.External, String.IsNullOrEmpty(user.AddressUser.ExtraDetail) ? "" : user.AddressUser.ExtraDetail.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.UID, user.AddressUser.UID.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Lat, user.AddressUser.Lat.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Lng, user.AddressUser.Lng.ToString()));
           


            identity.AddClaim(new Claim(CustomClaimTypes.GrantUser, user.GrantUserManager.GetValueOrDefault().ToString()));

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        // public CustomPasswordValidator PasswordValidator { get; set; }
    }
}