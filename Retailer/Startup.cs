using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Retailer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(Retailer.Startup))]

namespace Retailer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Create Admin Role
            string[] roleNames = { "Admin", "Retailer", "Consumer", "FOS" };
            IdentityResult roleResult;
            foreach (string roleName in roleNames)
            {// Check to see if Role Exists, if not create it
                if (!RoleManager.RoleExists(roleName))
                {
                    roleResult = RoleManager.Create(new IdentityRole(roleName));
                    if (roleName == "Admin")
                    {
                        var store = new UserStore<ApplicationUser>(context);
                        var manager = new UserManager<ApplicationUser>(store);
                        var user = new ApplicationUser { UserName = "admin" };
                        manager.Create(user, "retailer@123");
                        manager.AddToRole(user.Id, "Admin");
                    }
                }
            }
        }
    }
}
