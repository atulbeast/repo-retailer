using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using Retailer.Models.DataModel;

namespace Retailer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public string OTP { get; set; }
        public string MobileNumber { get; set; }
        public bool MobileNumberConfirmed { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("RetailerConnection", throwIfV1Schema: false)
        {
          //  Database.SetInitializer(new ApplicationInitializer());
        }
        public DbSet<Address> Address { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<LineItem> LineItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<AppImage> AppImage { get; set; }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //public class ApplicationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        //{
        //    protected override void Seed(ApplicationDbContext context)
        //    {
        //        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //        var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

        //        // Create Admin Role
        //        string[] roleNames = { "Admin", "Consumer",  "Retailer", "FOS"};
        //        IdentityResult roleResult;
        //        foreach (string roleName in roleNames)
        //        {// Check to see if Role Exists, if not create it
        //            if (!RoleManager.RoleExists(roleName))
        //            {
        //                roleResult = RoleManager.Create(new IdentityRole(roleName));
        //                if (roleName == "Admin")
        //                {
        //                    var store = new UserStore<ApplicationUser>(context);
        //                    var manager = new UserManager<ApplicationUser>(store);
        //                    var user = new ApplicationUser { UserName = "admin" };
        //                    manager.Create(user, "retailer@123");
        //                    manager.AddToRole(user.Id, "Admin");
        //                }
        //            }
        //        }

                
        //    }
        //}
        
    }
}