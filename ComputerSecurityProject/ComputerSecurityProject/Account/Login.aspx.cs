using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using ComputerSecurityProject.Models;
using System.Linq;
using System.Configuration;

namespace ComputerSecurityProject.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                
                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);

                switch (result)
                {
                    case SignInStatus.Success:
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        break;
                    case SignInStatus.LockedOut:
                        Response.Redirect("/Account/Lockout");
                        break;
                    case SignInStatus.RequiresVerification:
                        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                        Request.QueryString["ReturnUrl"],
                                                        RememberMe.Checked),
                                          true);
                        break;
                    case SignInStatus.Failure:
                    default:
                        FailureText.Text = "Invalid login attempt";
                        ErrorMessage.Visible = true;
                        break;
                }
                // The code for sql injection
                //using (var context = new Entities())
                //{ // ' or 'a'='a
                //  // APjaHotK+i1M1/6RYibTX87U8l8GgBMpTQOY0eaFelnJ6zTgVblhudk+nxLOrHpnpQ==
                //    var email = Email.Text;
                //    var password = Password.Text;
                //    var user = context.AspNetUsers.SqlQuery("select * FROM aspnetusers where Email = '" + email + "' AND PasswordHash = '" + password + "'").ToList<AspNetUser>();
                //    if (user.Count != 0)
                //    {
                //        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //    }
                //    else
                //    {
                //        FailureText.Text = "Invalid login attempt";
                //        ErrorMessage.Visible = true;
                //    }
                //}
            }
        }
    }
}