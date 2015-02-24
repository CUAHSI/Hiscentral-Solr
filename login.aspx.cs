using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using Microsoft.AspNet.Membership.OpenAuth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Globalization;
using System.Linq;
using WebSite1;
using System.Threading;

public partial class login : System.Web.UI.Page {
    public string ReturnUrl { get; set; }

    protected void Page_Load(object sender, EventArgs e) {
        try {

            if (IsPostBack) {
                var provider = Request.Form["provider"];
                if (provider == null) {
                    return;
                }
                // Request a redirect to the external login provider
                string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/ExternalLoginLandingPage.aspx?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
                var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
                // Add xsrf verification when linking accounts
                if (Context.User.Identity.IsAuthenticated) {
                    properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
                }
                Context.GetOwinContext().Authentication.Challenge(properties, provider);
                Response.StatusCode = 401;
                Response.End();

            } 
            
            else if (!IsPostBack && String.IsNullOrEmpty(Request.Url.Query)) {
                var url = Request.Url.AbsoluteUri + "?ReturnUrl=" + Request.ApplicationPath + "/login.aspx";
                Response.Redirect(url);
            }


            //if (User.Identity.IsAuthenticated) {
            //    lblLogin.Text = "You are signed in as " + User.Identity.Name;
            //} else {
            //    lblLogin.Text = "You are not signed in!";
            //}
            //if (IsPostBack) {
            //    var provider = Request.Form["provider"];
            //    if (provider == null) {
            //        return;
            //    }
            //    var token = Request.QueryString["s"] ?? String.Empty;   //If token found, we want to associate an existing user's gmail address to his login

            //    //var redirectUrl = String.IsNullOrEmpty(token) ? "~/ExternalLoginLandingPage.aspx" : "~/ExternalLoginLandingPage.aspx?token=" + token;
            //    string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/ExternalLoginLandingPage.aspx?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
            //    var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };

            //    HttpContext.Current.GetOwinContext().Authentication.Challenge(properties, provider);

            //    //OpenAuth.RequestAuthentication(provider, redirectUrl);
        }  catch (ThreadAbortException) {
            //Do nothing
        } catch (Exception ex) {
            //throw new Exception(ex.Message);
            var error = ex.Message;
        }

    }   

    
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e) {

    }

    public IEnumerable<string> GetProviderNames() {
     //return HttpContext.Current.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        var result = new List<string>();
        var providers = HttpContext.Current.GetOwinContext().Authentication.GetAuthenticationTypes().Select(t => t.AuthenticationType).First();
        result.Add(providers);
        return result;
    }

    //public IEnumerable<ProviderDetails> GetProviderNames()
    //{
    //    return OpenAuth.AuthenticationClients.GetAll();
    //}   

}


    //public class ApplicationUser : IdentityUser
    //{
    //}

    // public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection")
    //    {
    //    }
    //}

    //public class UserManager : UserManager<ApplicationUser>
    //{
    //    public UserManager()
    //        : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
    //    {
    //    }
    //}

    //public static class IdentityHelper {
    //    // Used for XSRF when linking external logins
    //    public const string XsrfKey = "XsrfId";

    //    public static void SignIn(UserManager manager, ApplicationUser user, bool isPersistent) {
    //        IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
    //        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
    //        var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
    //        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
    //    }

    //    public const string ProviderNameKey = "providerName";
    //    public static string GetProviderNameFromRequest(HttpRequest request) {
    //        return request[ProviderNameKey];
    //    }

    //    public static string GetExternalLoginRedirectUrl(string accountProvider) {
    //        return "/ExternalLoginLandingPage.aspx?" + ProviderNameKey + "=" + accountProvider;
    //    }

    //    private static bool IsLocalUrl(string url) {
    //        return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
    //    }

    //    public static void RedirectToReturnUrl(string returnUrl, HttpResponse response) {
    //        if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl)) {
    //            response.Redirect(returnUrl);
    //        } else {
    //            response.Redirect("~/");
    //        }
    //    }
    //}

    //protected void Login1_LoginError(object sender, EventArgs e)
    //{

    //    //There was a problem logging in the user

    //    //'See if this user exists in the database
    //    MembershipUser userInfo = Membership.GetUser(Login1.UserName);

    //    if (userInfo == null)
    //    {
    //        //'The user entered an invalid username...
    //        LoginErrorDetails.Text = "There is no user in the database with the username " + Login1.UserName;
    //    }
    //    else
    //    {
    //        //'See if the user is locked out or not approved
    //        if (!userInfo.IsApproved)
    //        {
    //            LoginErrorDetails.Text = "Your account has not yet been approved by the site's administrators. Please try again later...";
    //        }
    //        else if (userInfo.IsLockedOut)
    //        {
    //            LoginErrorDetails.Text = "Your account has been locked out because of a maximum number of incorrect login attempts. You will NOT be able to login until you contact a site administrator and have your account unlocked.";
    //        }
    //        else
    //        {
    //            //'The password was incorrect (don't show anything, the Login control already describes the problem)
    //            LoginErrorDetails.Text = String.Empty;
    //        }
    //    }

    //}
