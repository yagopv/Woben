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
    [Authorize]
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
        [Authorize(Roles = "Administrator")]
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
                // Add new Tags
                foreach (var tag in product.Tags.ToList())
                {
                    if (tag.TagId == 0)
                    {
                        tag.SetUrlReference();
                        tag.Identity = Guid.NewGuid();
                        db.Tags.Add(tag);
                    }
                    else if (tag.TagId == -1)
                    {
                        var originalTag = await db.Tags.Where(t => t.Identity == tag.Identity && t.ProductId == t.ProductId).FirstAsync();
                        if (originalTag != null)
                        {
                            product.Tags.Remove(tag);
                            db.Tags.Remove(originalTag);
                        }
                        
                    }
                }
            }

            if (product.Features != null)
            {
                // Add new Features
                foreach (var feature in product.Features.ToList())
                {
                    if (feature.FeatureId == 0)
                    {
                        feature.Identity = Guid.NewGuid();
                        db.Features.Add(feature);
                    }
                    else if (feature.FeatureId == -1)
                    {
                        var originalFeature = await db.Features.Where(f => f.Identity == feature.Identity && f.ProductId == f.ProductId).FirstAsync();
                        if (originalFeature != null)
                        {
                            product.Features.Remove(feature);
                            db.Features.Remove(originalFeature);
                        }
                    }
                }
            }

            if (product.Images != null)
            {
                // Add new tags
                foreach (var image in product.Images.ToList())
                {
                    if (image.ImageId == 0)
                    {
                        image.Identity = Guid.NewGuid();
                        db.Images.Add(image);
                    }
                    else if (image.ImageId == -1)
                    {
                        var originalImage = await db.Images.Where(i => i.Identity == image.Identity).Include(i => i.Products).FirstAsync();
                        if (originalImage != null)
                        {
                            product.Images.Remove(image);                            
                        }
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

            return Ok(product);
        }

        // POST odata/Product
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

            product.UpdatedDate = DateTime.UtcNow;
            product.UpdatedBy = User.Identity.Name;

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
        [Authorize(Roles = "Administrator")]
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
