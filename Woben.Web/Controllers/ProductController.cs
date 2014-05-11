using System;
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
    [Authorize(Roles="Administrator")]
    public class ProductController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Product
        [Queryable]
        public IQueryable<Product> GetProduct()
        {
            return db.Products;
        }

        // GET odata/Product(5)
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(product => product.ProductId == key));
        }

        // PUT odata/Product(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product product)
        {
            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != product.ProductId)
            {
                return BadRequest();
            }

            if (product.Tags != null)
            {
                // Add new tags
                foreach (var tag in product.Tags.ToList())
                {
                    if (tag.TagId == 0)
                    {
                        tag.SetUrlReference();
                        db.Tags.Add(tag);
                    }
                    else if (tag.TagId == -1)
                    {
                        db.Tags.Remove(tag);
                    }
                }
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }            

            return Updated(product);
        }

        // POST odata/Product
        public async Task<IHttpActionResult> Post(Product product)
        {
            product.CreatedDate = DateTime.UtcNow;
            product.UpdatedDate = DateTime.UtcNow;
            product.CreatedBy = User.Identity.Name;
            product.UpdatedBy = User.Identity.Name;
            product.SetUrlReference();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Created(product);
        }

        // PATCH odata/Product(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // DELETE odata/Product(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Product product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Product(5)/Category
        [Queryable]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Products.Where(m => m.ProductId == key).Select(m => m.Category));
        }

        // GET odata/Product(5)/Tags
        [Queryable]
        public IQueryable<Tag> GetTags([FromODataUri] int key)
        {
            return db.Products.Where(m => m.ProductId == key).SelectMany(m => m.Tags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return db.Products.Count(e => e.ProductId == key) > 0;
        }

    }
}
