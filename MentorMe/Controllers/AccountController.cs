﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MentorMe.Filters;
using MentorMe.Models;
using Facebook;
using Facebook.Reflection;

namespace MentorMe.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
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
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                using (var context = new UsersContext())
                {
                    Session["UserName"] = model.UserName;
                    var oUser = context.UserProfiles.Where(x => x.UserName == model.UserName).FirstOrDefault();
                    if (oUser != null)
                    {
                        if (context.PersonalDetails.Where(x => x.UserId == oUser.UserId).Any())
                        {
                            if (context.Education.Any(x => x.UserId == oUser.UserId))
                            {
                                if (context.AreaOfIntrests.Any(x => x.UserId == oUser.UserId))
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("Interest", "Detail");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Education", "Detail");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Personal", "Detail");
                        }
                    }
                }
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }


        [HttpGet]
        public ActionResult LogOff(string returnUrl)
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home"); 
        }
        
        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                if (Session["provider"] != null)
                {
                    string oProvider = Session["provider"].ToString();
                    if (oProvider == "facebook")
                    {
                        var oFacebook = new FacebookClient();
                        var accessToken = Session["accesstoken"].ToString();
                        Session.Clear();
                        var callLogOutkUrl = new Uri(Url.RouteUrl("Default", new { Action = "Login" }, Request.Url.Scheme));
                        var logoutUrl = oFacebook.GetLogoutUrl(new
                        {
                            access_token = accessToken,
                            next = callLogOutkUrl
                        });
                        WebSecurity.Logout();
                        return new RedirectResult(logoutUrl.AbsoluteUri);
                    }
                }

                WebSecurity.Logout();
            }
            catch (Exception ex)
            {
                using (UsersContext db = new UsersContext())
                {
                    db.ErrorsLog.Add(new ErrorLog { ExceptionMessage = ex.Message, ExceptionStackTrace = ex.StackTrace, ErrorLogDate = DateTime.Now.ToString() });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
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
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber
                    });
                    WebSecurity.Login(model.UserName, model.Password);
                    Session["UserName"] = model.UserName;
                    using (var context = new UsersContext())
                    {
                        var oUser = context.UserProfiles.Where(x => x.UserName == model.UserName).FirstOrDefault();
                        if (oUser != null)
                        {
                            if (context.PersonalDetails.Where(x => x.UserId == oUser.UserId).Any())
                            {
                                if (context.Education.Any(x => x.UserId == oUser.UserId))
                                {
                                    if (context.AreaOfIntrests.Any(x => x.UserId == oUser.UserId))
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Interest", "Detail");
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("Education", "Detail");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Personal", "Detail");
                            }
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Index", "Home");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Index", "Home");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            try
            {
                GooglePlusClient.RewriteRequest();
                string firstname = string.Empty;
                string lastname = string.Empty;
                string email = string.Empty;

                AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
                if (!result.IsSuccessful)
                {
                    return RedirectToAction("ExternalLoginFailure");
                }

                if (result.ExtraData.ContainsKey("accesstoken"))
                {
                    Session["accesstoken"] = result.ExtraData["accesstoken"].ToString();
                    Session["provider"] = result.Provider.ToLower();
                    if (result.Provider.ToLower() == "linkedin")
                    {
                        firstname = result.ExtraData["firstname"].ToString();
                        lastname = result.ExtraData["lastname"].ToString();
                        email = result.ExtraData["email"].ToString();
                    }
                    else if (result.Provider.ToLower() == "googleplus")
                    {
                        firstname = result.ExtraData["name"].ToString();
                        lastname = result.ExtraData["family_name"].ToString();
                        email = result.ExtraData["email"].ToString();
                    }
                    else if (result.Provider.ToLower() == "facebook")
                    {
                        firstname = result.ExtraData["firstname"].ToString();
                        lastname = result.ExtraData["lastname"].ToString();
                        email = result.ExtraData["email"].ToString();
                    }
                }

                //User name already register check with websecurity(checking point-redirect to main page)
                if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                {
                    Session["UserName"] = email;
                    using (var context = new UsersContext())
                    {
                        var oUser = context.UserProfiles.Where(x => x.UserName == email).FirstOrDefault();
                        if (oUser != null)
                        {
                            if (context.PersonalDetails.Where(x => x.UserId == oUser.UserId).Any())
                            {
                                if (context.Education.Any(x => x.UserId == oUser.UserId))
                                {
                                    if (context.AreaOfIntrests.Any(x => x.UserId == oUser.UserId))
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Interest", "Detail");
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("Education", "Detail");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Personal", "Detail");
                            }
                        }
                    }
                    return RedirectToLocal(returnUrl);
                }

                if (User.Identity.IsAuthenticated)
                {
                    // If the current user is logged in add the new account
                    OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                    return RedirectToLocal(returnUrl);
                }
                else
                {

                    // User is new, ask for their desired membership name
                    string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                    ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                    ViewBag.ReturnUrl = returnUrl;
                    return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, FirstName = firstname, LastName = lastname, Email = email, ExternalLoginData = loginData });
                }
            }
            catch (Exception ex)
            {
                using (UsersContext db = new UsersContext())
                {
                    db.ErrorsLog.Add(new ErrorLog { ExceptionMessage = ex.Message, ExceptionStackTrace = ex.StackTrace, ErrorLogDate = DateTime.Now.ToString() });
                    db.SaveChanges();
                }
            }
            return RedirectToLocal(returnUrl);
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;
            try
            {
                if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
                {
                    return RedirectToAction("Manage");
                }

                if (ModelState.IsValid)
                {
                    // Insert a new user into the database
                    using (UsersContext db = new UsersContext())
                    {
                        UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                        // Check if user already exists
                        if (user == null)
                        {
                            if (Session["provider"] != null)
                            {
                                if (Session["provider"].ToString().ToLower() == "linkedin" || Session["provider"].ToString().ToLower() == "googleplus" || Session["provider"].ToString().ToLower() == "facebook")
                                {
                                    db.UserProfiles.Add(new UserProfile { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Provider = provider });
                                }
                                else
                                {
                                    // Insert name into the profile table
                                    db.UserProfiles.Add(new UserProfile { UserName = model.UserName, Provider = provider });
                                }
                            }
                            else
                            {
                                // Insert name into the profile table
                                db.UserProfiles.Add(new UserProfile { UserName = model.UserName, Provider = provider });
                            }
                            db.SaveChanges();

                            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                            OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                            using (var context = new UsersContext())
                            {
                                Session["UserName"] = model.UserName;
                                var oUser = context.UserProfiles.Where(x => x.UserName == model.UserName).FirstOrDefault();
                                if (oUser != null)
                                {
                                    if (context.PersonalDetails.Where(x => x.UserId == oUser.UserId).Any())
                                    {
                                        if (context.Education.Any(x => x.UserId == oUser.UserId))
                                        {
                                            if (context.AreaOfIntrests.Any(x => x.UserId == oUser.UserId))
                                            {
                                                return RedirectToAction("Index", "Home");
                                            }
                                            else
                                            {
                                                return RedirectToAction("Interest", "Detail");
                                            }
                                        }
                                        else
                                        {
                                            return RedirectToAction("Education", "Detail");
                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("Personal", "Detail");
                                    }
                                }
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            var oProvider = db.UserProfiles.Where(x => x.UserName.ToLower() == model.UserName.ToLower()).Select(x => x.Provider).FirstOrDefault();
                            if (oProvider == null)
                            {
                                ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                            }
                            else
                            {
                                ModelState.AddModelError("UserName", "User name already register using '" + oProvider.ToString() + "' provider.");
                            }
                        }
                    }
                }

                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                using (UsersContext db = new UsersContext())
                {
                    db.ErrorsLog.Add(new ErrorLog { ExceptionMessage = ex.Message, ExceptionStackTrace = ex.StackTrace, ErrorLogDate = DateTime.Now.ToString() });
                    db.SaveChanges();
                }
            }
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
