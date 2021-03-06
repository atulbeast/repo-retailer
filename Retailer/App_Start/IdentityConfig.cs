﻿using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Retailer.Models;
using System.Linq;
using Retailer.Common;
namespace Retailer
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public async Task<ApplicationUser> OTPCheck(string MobileNumber, string OTP)
        {
            ApplicationUser user;
            using(ApplicationDbContext db=new ApplicationDbContext())
            {
                user= db.Users.FirstOrDefault(x => x.OTP == OTP && x.MobileNumber==MobileNumber);
                if (user != null)
                {
                    user.OTP = "";
                    if (!user.MobileNumberConfirmed)
                        user.MobileNumberConfirmed = true;
                    await db.SaveChangesAsync();
                }
            }
            //var manager = new ApplicationUserManager();
            //var item=manager.Users.fin
            return user;
        }

        public async Task<ApplicationUser> IfExistSendOTP(string MobileNumber)
        {
            ApplicationUser user;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                user = db.Users.FirstOrDefault(x => x.MobileNumber == MobileNumber);
                if (user != null)
                {
                    user.OTP = Utils.GenerateRandomNo();
                    await db.SaveChangesAsync();
                    await Utils.SendSMS(MobileNumber, "Your OTP for Retailer App is " + user.OTP);
                }
            }
            
            return user;
        }


        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            ///It is based on the same context as the ApplicationUserManager
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}
