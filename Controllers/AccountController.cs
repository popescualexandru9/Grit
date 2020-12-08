using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Grit.Models;
using System.Net.Mail;
using Grit.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Grit.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;

        #region providedRegion
        public AccountController()
        {
            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            _context = new ApplicationDbContext();
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = await UserManager.FindByEmailAsync(model.Email);
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await UserManager.AddToRoleAsync(user.Id, "Member");
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    MailAddress addr = new MailAddress(model.Email);
                    string emailUser = addr.User;
                    int index = addr.Host.LastIndexOf(".");
                    string emailHost = addr.Host.Substring(0, index);
                    
                   
                    return RedirectToAction("FillInfo", "Account", new { emailUser = emailUser , emailHost = emailHost});
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        #region MyRegion

        [Route("Account/FillInfo/{emailUser?}/{emailHost?}")]
        public ActionResult FillInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/FillInfo/{emailUser}/{emailHost}")]
        public async Task<ActionResult> FillInfo(FillInfoViewModel model, string emailUser, string emailHost)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = emailUser + "@" + emailHost + ".com";
            var user = await UserManager.FindByNameAsync(email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by email.");
            }


            // Format weight and height to always have 2 decimals
  
            model.DailyWeight = decimal.Parse(model.DailyWeight.ToString("F"));
            model.Height = decimal.Parse(model.Height.ToString("F"));

            decimal weight = Math.Round(model.DailyWeight, 2);

            // Search for weight in database and store its id
            int weightEntityId;
            var weightEntity = _context.Weights.SingleOrDefault(x => x.Weigth == weight);
            
            if (weightEntity == null)
            {
                // If not found, create new weight
                WeightController weightController = new WeightController();
                weightEntityId = weightController.Create(weight);
            }
            else
                weightEntityId = weightEntity.Id;
            

            // Asign values to user
            user.DailyWeight_Id = weightEntityId;
            user.Height = Math.Round(model.Height, 2);
            user.Birthdate = model.Birthdate;
            user.Gender = model.Gender;
            user.SignUpDate = DateTime.Now;

            UserManager.Update(user);
            return RedirectToAction("Index", "Home");
        }

        [Authorize (Roles = "Employee, Admin")]
        [Route("Account/Members")]
        public ActionResult Members()
        {
            var users = _context.Users.ToList();
            IList<string> roles = new List<string>();
            foreach ( var user in users)
            {
                roles.Add(UserManager.GetRoles(user.Id).FirstOrDefault());
            }

            var membersModel = new MembersViewModel
            {
                Users =  users,
                RoleName = roles
            };

            return View(membersModel);
        }

        public ActionResult DeleteMember(string id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(id));
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by id.");
            }

            _context.Users.Attach(user);
            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Members", "Account");

        }

        [Authorize(Roles = "Employee, Admin")]
        public ActionResult MemberDetails(string id, string status)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(id));

            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by id.");
            }
   
            var detailsModel = new MemberDetailsViewModel
            {
                User = user,
                Weight = _context.Weights.SingleOrDefault(x => x.Id == user.DailyWeight_Id),
                RoleName = UserManager.GetRoles(user.Id).FirstOrDefault()
            };
            return View(detailsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<ActionResult> MemberDetails(MemberDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.User.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by email.");
            }

            // Format weight and height to always have 2 decimals
            decimal weight = decimal.Parse(model.Weight.Weigth.ToString("F"));
            decimal height = decimal.Parse((model.User.Height??0).ToString("F"));

            weight = Math.Round(weight, 2);

            // Search for weight in database and store its id
            int weightEntityId;
            var weightEntity = _context.Weights.SingleOrDefault(x => x.Weigth == weight);

            if (weightEntity == null)
            {
                // If not found, create new weight
                WeightController weightController = new WeightController();
                weightEntityId = weightController.Create(weight);
            }
            else
                weightEntityId = weightEntity.Id;


            // Asign values to user
            user.UserName = model.User.UserName;
            user.DailyWeight_Id = weightEntityId;
            user.Height = Math.Round(height, 2);
            user.Birthdate = model.User.Birthdate;
            user.Gender = model.User.Gender;

            UserManager.Update(user);
            return RedirectToAction("MemberDetails", "Account", new {id = user.Id, status="ok"});
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole(string id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(id));
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by id.");
            }

            var detailsModel = new MemberDetailsViewModel
            {
                User = user,
                Weight = _context.Weights.SingleOrDefault(x => x.Id == user.DailyWeight_Id),
                RoleName = UserManager.GetRoles(user.Id).FirstOrDefault()
            };

            return View(detailsModel);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeRole(MemberDetailsViewModel model, string previousRole)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id.Equals(model.User.Id));
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new HttpException(404, "Can't find user by id.");
            }
            var result = await UserManager.RemoveFromRoleAsync(user.Id, previousRole);
            await UserManager.AddToRoleAsync(user.Id, model.RoleName);
            return RedirectToAction("Members", "Account");
        }

        #endregion
    }
}