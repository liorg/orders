﻿using System;
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
        public ActionResult Index()
        {
            //  using (var context = new ApplicationDbContext())
            // {
            var userContext = new UserContext(AuthenticationManager);
            IEnumerable<ApplicationUser> usersData;
            var users = DBContext.Users;
            var model = new List<EditUserViewModel>();
            if (!User.IsInRole(HelperAutorize.RoleAdmin))
                usersData = users.Where(u => u.Organization_OrgId.HasValue && u.Organization_OrgId.Value == userContext.OrgId).ToList();
            else
                usersData = users.ToList();

            foreach (var user in usersData)
            {
                var edit = new EditUserViewModel(user);
                model.Add(edit);
            }
            return View(model);
            // }
        }

        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public async Task<ActionResult> Edit(string id, ManageMessageId? Message = null)
        {
            // var Db = new ApplicationDbContext();
            //  using (var context = new ApplicationDbContext())
            {
                var context = DBContext;
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
                        }
                    }
                }
                ViewBag.Orgs = new SelectList(context.Organization.ToList(), "OrgId", "Name");
                ViewBag.MessageId = Message;
                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            //if (ModelState.IsValid)
            {

                {
                    var id = model.UserId.ToString();
                    var viewLogic = new ViewLogic();

                    var context = DBContext;
                    var user = await context.Users.FirstAsync(u => u.Id == id);
                    // Update the user data:
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.IsActive = model.IsActive;
                    user.EmpId = model.EmpId;
                    user.Tel = model.Tel;
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

                    if (model.IsAdmin) await UserManager.AddToRoleAsync(user.Id, "Admin");

                    if (model.IsCreateOrder) await UserManager.AddToRoleAsync(user.Id, "User");

                    if (model.IsOrgMangager) await UserManager.AddToRoleAsync(user.Id, "OrgManager");

                    if (model.IsRunner) await UserManager.AddToRoleAsync(user.Id, "Runner");

                    if (model.IsAcceptOrder) await UserManager.AddToRoleAsync(user.Id, "Accept");

                    return RedirectToAction("Index");

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
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ViewBag.Orgs = new SelectList(db.Organization.ToList(), "OrgId", "Name");

            }
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
                    ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                    var org = orgs.Where(o => o.OrgId == model.OrgId).FirstOrDefault();
                    var userName = model.UserName;
                    if (org.Name != General.OrgWWW)
                    {
                        userName = model.UserName + "@" + org.Domain;
                    }
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


        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public ActionResult Register()
        {
            //using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var db = DBContext;
                ViewBag.Orgs = new SelectList(db.Organization.ToList(), "OrgId", "Name");

            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RolesAttribute(HelperAutorize.RoleAdmin, HelperAutorize.RoleOrgManager)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //using (var context = new ApplicationDbContext())
            {
                var context = DBContext;
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

                if (ModelState.IsValid)
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
                        Tel = model.Tel
                    };
                    viewLogic.SetViewerUserByRole(model, user);


                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (model.IsAdmin) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAdmin);

                    if (model.IsCreateOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleUser);

                    if (model.IsOrgMangager) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleOrgManager);

                    if (model.IsRunner) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleRunner);

                    if (model.IsAcceptOrder) await UserManager.AddToRoleAsync(user.Id, Helper.HelperAutorize.RoleAccept);

                    if (result.Succeeded)
                        return RedirectToAction("Index", "Account");
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
            identity.AddClaim(new Claim(CustomClaimTypes.JobTitle, jobTitle));
            identity.AddClaim(new Claim(CustomClaimTypes.JobType, ((int)jobType).ToString()));

            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, org.OrgId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, String.IsNullOrEmpty(user.EmpId) ? "אן מספר עובד" : user.EmpId));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.FirstName + " " + user.LastName));


            identity.AddClaim(new Claim(CustomClaimTypes.ShowAllView, user.ViewAll.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.DefaultView, user.DefaultView.ToString()));
            identity.AddClaim(new Claim(CustomClaimTypes.Tel, user.Tel.ToString()));

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