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
using sln.Models;
using sln.Helper;
using sln.Dal;
using sln.DataModel;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace sln.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        public AccountController()
            : this(new ApplicationUserManager())
        {

        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }

        //  [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            using (var Db = new ApplicationDbContext())
            {
                var users = Db.Users;
                var model = new List<EditUserViewModel>();
                foreach (var user in users)
                {
                    var u = new EditUserViewModel(user);
                    model.Add(u);
                }
                return View(model);
            }
        }

        // [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.Id == id);
            var model = new EditUserViewModel(user);

            ViewBag.MessageId = Message;
            return View(model);
        }


        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.Id == model.UserId);

                // Update the user data:
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [LayoutInjecterAttribute("~/Views/Shared/_Layout.cshtml")]
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
        [LayoutInjecterAttribute("~/Views/Shared/_Layout.cshtml")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var orgs = await context.Organization.ToListAsync();
                    ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                    var org = orgs.Where(o => o.OrgId == model.OrgId).FirstOrDefault();
                    var userName = model.UserName;
                    if (org.Name != "www")
                    {
                        userName = model.UserName + "@" + org.Domain;
                    }
                    var user = await UserManager.FindAsync(userName, model.Password);
                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe, org);
                        return RedirectToLocal(returnUrl);
                     //   return RedirectToAction("Index","S");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
            }
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [LayoutInjecterAttribute("~/Views/Shared/_Layout.cshtml")]
        public ActionResult Register()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ViewBag.Orgs = new SelectList(db.Organization.ToList(), "OrgId", "Name");

            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [LayoutInjecterAttribute("~/Views/Shared/_Layout.cshtml")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var orgs = await db.Organization.ToListAsync();
                ViewBag.Orgs = new SelectList(orgs, "OrgId", "Name");
                var org = orgs.Where(o => o.OrgId == model.OrgId).FirstOrDefault();
                var userName = model.UserName;
                if (org.Name != "www")
                {
                    userName = model.UserName + "@" + org.Domain;
                }
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = userName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        IsActive = true,
                        Organization_OrgId = model.OrgId
                    };

                    var result = await UserManager.CreateAsync(user, model.Password);

                    IdentityManager manager = new IdentityManager();
                    if (model.IsAdmin)
                        manager.AddUserToRole(UserManager,user.Id, "Admin");
                    
                    if (model.IsCreateOrder)
                      manager.AddUserToRole(UserManager, user.Id, "User");
                    
                    if (model.IsOrgMangager)
                      manager.AddUserToRole(UserManager, user.Id, "OrgManager");
                    
                    if (model.IsRunner)
                      manager.AddUserToRole(UserManager, user.Id, "Runner");
                    
                    if (model.IsAcceptOrder)
                      manager.AddUserToRole(UserManager, user.Id, "Accept");
                    
                   
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false,org: org);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        AddErrors(result);
                    }
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

        //
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

        //
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
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent,Organization org)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.GroupSid, org.OrgId.ToString()));

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

        public CustomPasswordValidator PasswordValidator { get; set; }
    }
}