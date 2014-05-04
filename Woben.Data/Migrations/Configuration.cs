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
            //Seed User and Roles

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

            // Seed Categories
            var cat1 = new Category() {
                 Name = "Categoria 1",
                 Description = "Descripción Categoría 1",
                 UrlCodeReference = "categoria-1"
            };
            var cat2 = new Category()
            {
                Name = "Categoria 2",
                Description = "Descripción Categoría 2",
                UrlCodeReference = "categoria-2"
            };
            var cat3 = new Category()
            {
                Name = "Categoria 3",
                Description = "Descripción Categoría 3",
                UrlCodeReference = "categoria-3"
            };
            var cat4 = new Category()
            {
                Name = "Categoria 4",
                Description = "Descripción Categoría 4",
                UrlCodeReference = "categoria-4"
            };

            context.Categories.AddOrUpdate(cat1, cat2, cat3, cat4);

            context.SaveChanges();

            var product1 = new Product()
            {
                 Name = "Producto 1",
                 Description = "Descripción producto 1",
                 Markdown = "",
                 Html = "",
                 CreatedBy = "admin",
                 UpdatedBy ="admin",
                 CreatedDate = DateTime.UtcNow,
                 UpdatedDate = DateTime.UtcNow,
                 CategoryId = cat1.CategoryId,
                 ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                 IsPublished = true,
                 UrlCodeReference = "producto-1"                  
            };

            var product2 = new Product()
            {
                Name = "Producto 2",
                Description = "Descripción producto 2",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-2"
            };

            var product3 = new Product()
            {
                Name = "Producto 3",
                Description = "Descripción producto 3",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-3"
            };

            var product4 = new Product()
            {
                Name = "Producto 4",
                Description = "Descripción producto 4",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-4"
            };

            var product5 = new Product()
            {
                Name = "Producto 5",
                Description = "Descripción producto 5",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-5"
            };

            var product6 = new Product()
            {
                Name = "Producto 6",
                Description = "Descripción producto 6",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-6"
            };

            var product7 = new Product()
            {
                Name = "Producto 7",
                Description = "Descripción producto 7",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-7"
            };

            var product8 = new Product()
            {
                Name = "Producto 8",
                Description = "Descripción producto 8",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-8"
            };

            var product9 = new Product()
            {
                Name = "Producto 9",
                Description = "Descripción producto 9",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-9"
            };

            var product10 = new Product()
            {
                Name = "Producto 10",
                Description = "Descripción producto 10",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-10"
            };

            var product11 = new Product()
            {
                Name = "Producto 11",
                Description = "Descripción producto 11",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-11"
            };

            var product12 = new Product()
            {
                Name = "Producto 12",
                Description = "Descripción producto 12",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-12"
            };

            var product13 = new Product()
            {
                Name = "Producto 13",
                Description = "Descripción producto 13",
                Markdown = "",
                Html = "",
                CreatedBy = "admin",
                UpdatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                CategoryId = cat1.CategoryId,
                ImageUrl = "http://www.google.es/imgres?imgurl=http%3A%2F%2Ftcnandroid.com%2Fwp-content%2Fthemes%2Fflavor%2Fimages%2Fplaceholder-320.png&imgrefurl=http%3A%2F%2Ftcnandroid.com%2F%3Ftag%3Denemigo-de-whatsapp&h=205&w=320&tbnid=W5-FwaOhalYWqM%3A&zoom=1&docid=oGV2O34jJdSMqM&ei=F2xmU4XMD6aU0AXdvoGgCA&tbm=isch&ved=0CEIQMyg6MDo4yAE&iact=rc&uact=3&dur=1212&page=10&start=234&ndsp=29",
                IsPublished = true,
                UrlCodeReference = "producto-13"
            };

            context.Products.AddOrUpdate(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10, product11, product12, product13);

            context.SaveChanges();
        }
    }
}
