using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ComputerSecurityProject.Models;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace ComputerSecurityProject
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var apiKey = "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";
            var from = "shayush11@gmail.com";
            var mailMessage = new MailMessage(from, message.Destination, message.Subject, message.Body);
            mailMessage.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587)))
            {
                var creds = new NetworkCredential
                {
                    UserName = "apikey",
                    Password = apiKey
                };
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = creds;
                smtpClient.Send(mailMessage);
                return Task.FromResult(0);
            }
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            var passwordLength = int.Parse(ConfigurationManager.AppSettings["PasswordLength"]);
            var requireNonLetterOrDigit = ConfigurationManager.AppSettings["RequireNonLetterOrDigit"] == "true";
            var requireDigit = ConfigurationManager.AppSettings["RequireDigit"] == "true";
            var requireLowercase = ConfigurationManager.AppSettings["RequireLowercase"] == "true";
            var requireUppercase = ConfigurationManager.AppSettings["RequireUppercase"] == "true";

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = passwordLength,
                RequireNonLetterOrDigit = requireNonLetterOrDigit,
                RequireDigit = requireDigit,
                RequireLowercase = requireLowercase,
                RequireUppercase = requireUppercase,
            };

            var maxFailedAccessAttemptsBeforeLockout = int.Parse(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]);

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = maxFailedAccessAttemptsBeforeLockout;

            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
