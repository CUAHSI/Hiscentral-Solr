//using Microsoft.AspNet.Membership.OpenAuth;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite1;
using Microsoft.AspNet.Identity;

namespace hiscentral {
    public partial class ExternalLoginLandingPage : System.Web.UI.Page {

        protected string ProviderName {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderAccountKey {
            get { return (string)ViewState["ProviderAccountKey"] ?? String.Empty; }
            private set { ViewState["ProviderAccountKey"] = value; }
        }

        protected void Page_Load() {
            // Process the result from an auth provider in the request
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName)) {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack) {
                var manager = new UserManager();
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null) {
                    Response.Redirect("~/Login.aspx");
                }
                var user = manager.Find(loginInfo.Login);
                if (user != null) {
                    IdentityHelper.SignIn(manager, user, isPersistent: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                } else if (User.Identity.IsAuthenticated) {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null) {
                        Response.Redirect("~/Login.aspx");
                    }

                    var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                    if (result.Succeeded) {
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    } else {
                        AddErrors(result);
                        return;
                    }
                } else {
                    ///userName.Text = loginInfo.DefaultUserName;
                }
            }
        }

        protected void LogIn_Click(object sender, EventArgs e) {
            CreateAndLoginUser();
        }

        protected void cancel_Click(object sender, EventArgs e) {
            RedirectToReturnUrl();
        }

        private void CreateAndLoginUser() {
            if (!IsValid) {
                return;
            }
            var manager = new UserManager();
           var userName =  Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            var user = new ApplicationUser() { UserName = userName.Email };
            IdentityResult result = manager.Create(user);
            if (result.Succeeded) {
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null) {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
                result = manager.AddLogin(user.Id, loginInfo.Login);
                if (result.Succeeded) {
                    IdentityHelper.SignIn(manager, user, isPersistent: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    return;
                }
            }
            AddErrors(result);
        }

        private void RedirectToReturnUrl() {
            var returnUrl = Request.QueryString["ReturnUrl"];
            Response.Redirect(returnUrl);
            //var returnUrl = Request.QueryString["ReturnUrl"];
            //if (!String.IsNullOrEmpty(returnUrl) && Context.GetOwinContext().Authentication. OpenAuth.IsLocalUrl(returnUrl)) {
            //    Response.Redirect(returnUrl);
            //} else {
            //    Response.Redirect("~/");
            //}
        }

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }
        //    protected string ProviderName {
        //        get { return (string)ViewState["ProviderName"] ?? String.Empty; }
        //        private set { ViewState["ProviderName"] = value; }
        //    }

        //    protected string ProviderDisplayName {
        //        get { return (string)ViewState["ProviderDisplayName"] ?? String.Empty; }
        //        private set { ViewState["ProviderDisplayName"] = value; }
        //    }

        //    protected string ProviderUserId {
        //        get { return (string)ViewState["ProviderUserId"] ?? String.Empty; }
        //        private set { ViewState["ProviderUserId"] = value; }
        //    }

        //    protected string ProviderUserName {
        //        get { return (string)ViewState["ProviderUserName"] ?? String.Empty; }
        //        private set { ViewState["ProviderUserName"] = value; }
        //    }

        //    protected void AssociateExistingLoginToGmailAddress(string gmailAddress) {
        //        //If a token is supplied, try and associate the gmail address with an existing username
        //        var token = Request.QueryString["token"] ?? String.Empty;
        //        if (!String.IsNullOrEmpty(token)) {
        //            //var gmailAddress = ProviderUserName;
        //            var login = DataAccess.AssociateSuppliedTokenToExistingLogin(token);
        //            if (!String.IsNullOrEmpty(login)) {
        //                DataAccess.UpdateGmailAddress(login, gmailAddress);
        //            } else {
        //                throw new Exception("ERROR! Invalid token supplied - unable to associate " + ProviderUserName + " to an existing login in our database. Please contact support!");
        //            }
        //        }
        //    }

        //    protected void Page_Load() {


        //        if (!IsPostBack) {
        //            ProcessProviderResult();
        //        }
        //    }

        //    protected void logIn_Click(object sender, EventArgs e) {
        //        CreateAndLoginUser();
        //    }

        //    protected void cancel_Click(object sender, EventArgs e) {
        //        RedirectToReturnUrl();
        //    }

        //    private void ProcessProviderResult() {
        //        // Process the result from an auth provider in the request
        //        ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

        //        if (String.IsNullOrEmpty(ProviderName)) {
        //            Response.Redirect(FormsAuthentication.LoginUrl);
        //        }

        //        // Build the redirect url for OpenAuth verification
        //        var redirectUrl = "~/ExternalLoginLandingPage.aspx";
        //        var returnUrl = Request.QueryString["ReturnUrl"];
        //        if (!String.IsNullOrEmpty(returnUrl)) {
        //            redirectUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
        //        }

        //        // Verify the OpenAuth payload
        //        var authResult = OpenAuth.VerifyAuthentication(redirectUrl);



        //        ProviderDisplayName = OpenAuth.GetProviderDisplayName(ProviderName);

        //        if (!authResult.IsSuccessful) {
        //            Title = "External login failed";
        //            userNameForm.Visible = false;

        //            ModelState.AddModelError("Provider", String.Format("External login {0} failed.", ProviderDisplayName));

        //            // To view this error, enable page tracing in web.config (<system.web><trace enabled="true"/></system.web>) and visit ~/Trace.axd
        //            Trace.Warn("OpenAuth", String.Format("There was an error verifying authentication with {0})", ProviderDisplayName), authResult.Error);
        //            return;
        //        }
        //        AssociateExistingLoginToGmailAddress(authResult.UserName);
        //        // User has logged in with provider successfully
        //        // Check if user is already registered locally
        //        if (OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false)) {
        //            Session["gmail"] = authResult.UserName;

        //            RedirectToReturnUrl();
        //        }

        //        // Store the provider details in ViewState
        //        ProviderName = authResult.Provider;
        //        ProviderUserId = authResult.ProviderUserId;
        //        ProviderUserName = authResult.UserName;
        //        Session["gmail"] = ProviderUserName ?? "email_unavailable";
        //        // Strip the query string from action
        //        Form.Action = ResolveUrl(redirectUrl);

        //        if (User.Identity.IsAuthenticated) {
        //            // User is already authenticated, add the external login and redirect to return url
        //            OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
        //            RedirectToReturnUrl();
        //        } else {
        //            // User is new, ask for their desired membership name
        //            //userName.Text = authResult.UserName;
        //            //userName.Text = String.Empty;
        //        }
        //    }

        //    private void CreateAndLoginUser() {
        //        if (!IsValid) {
        //            return;
        //        }

        //        var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, ProviderUserName);
        //        if (!createResult.IsSuccessful) {

        //            ModelState.AddModelError("UserName", createResult.ErrorMessage);

        //        } else {
        //            // User created & associated OK
        //            if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false)) {
        //                RedirectToReturnUrl();
        //            }
        //        }
        //    }

        //    private void RedirectToReturnUrl() {
        //        var returnUrl = Request.QueryString["ReturnUrl"];
        //        if (!String.IsNullOrEmpty(returnUrl) && OpenAuth.IsLocalUrl(returnUrl)) {
        //            Response.Redirect(returnUrl);
        //        } else {
        //            Response.Redirect("~/");
        //        }
        //    }
        //}
    }
}

//-----------------------
//public partial class ExternalLoginLandingPage : System.Web.UI.Page
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
//}