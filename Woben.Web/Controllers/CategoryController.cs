﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Woben.Domain.Model;
using Woben.Data;

namespace Woben.Web.Controllers
{
    [Authorize]
    public class CategoryController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Category
        [Queryable]
        [Authorize]
        public IQueryable<Category> GetCategory()
        {
            return db.Categories;
        }

        // GET odata/Category(5)
        [Queryable]
        [Authorize]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Categories.Where(category => category.CategoryId == key));
        }

        // PUT odata/Category(5)
        [Authorize(Roles="Administrator")]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Category category)
        {
            category.SetUrlReference();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // POST odata/Category
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(Category category)
        {
            category.SetUrlReference();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Created(category);
        }

        // PATCH odata/Category(5)
        [AcceptVerbs("PATCH", "MERGE")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Category> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = await db.Categories.FindAsync(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Patch(category);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // DELETE odata/Category(5)
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Category category = await db.Categories.FindAsync(key);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Image(5)/Products
        [Queryable]
        [Authorize]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Categories.Where(m => m.CategoryId == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int key)
        {
            return db.Categories.Count(e => e.CategoryId == key) > 0;
        }
    }
}
