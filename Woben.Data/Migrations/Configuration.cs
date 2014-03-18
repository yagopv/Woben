namespace Woben.Data.Migrations
{
    using Woben.Domain.Model;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Woben.Data.WobenDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Woben.Data.WobenDbContext context)
        {
            UserManager<UserProfile> UserManager = new UserManager<UserProfile>(new UserStore<UserProfile>(context));
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                        
            if (!RoleManager.RoleExists("Administrator"))
            {
                RoleManager.Create(new IdentityRole("Administrator"));

            }

            if (!RoleManager.RoleExists("User"))
            {
                RoleManager.Create(new IdentityRole("User"));

            }

            if (UserManager.FindByName("admin") == null)
            {
                var user = new UserProfile() { UserName = "admin",  Email = "admin@mydomain.com", EmailConfirmed = true };
                var result = UserManager.Create(user, "admin1234");
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id,"Administrator" );
                }                
            }
        }
    }
}
