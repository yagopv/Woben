using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;

using Microsoft.AspNet.Identity.EntityFramework;
using Woben.Domain.Model;

namespace Woben.Data
{
    public class WobenDbContext : IdentityDbContext<UserProfile>
    {
        public WobenDbContext()
                    : base("WobenConnection")
        {            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WobenDbContext, Migrations.Configuration>());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Feature> Features { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = false;
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Very bad idea not doing this :)
            //http://stackoverflow.com/questions/19474662/map-tables-using-fluent-api-in-asp-net-mvc5-ef6
            base.OnModelCreating(modelBuilder);
        }
    }
}
