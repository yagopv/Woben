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
    public class ImageController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Image
        [Queryable]
        [Authorize]
        public IQueryable<Image> GetImage()
        {
            return db.Images;
        }

        // GET odata/Image(5)
        [Queryable]
        [Authorize]
        public SingleResult<Image> GetImage([FromODataUri] int key)
        {
            return SingleResult.Create(db.Images.Where(image => image.ImageId == key));
        }

        // PUT odata/Image(5)
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != image.ImageId)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(image);
        }

        // POST odata/Image
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(image);
            await db.SaveChangesAsync();

            return Created(image);
        }

        // PATCH odata/Image(5)
        [AcceptVerbs("PATCH", "MERGE")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Image> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Image image = await db.Images.FindAsync(key);
            if (image == null)
            {
                return NotFound();
            }

            patch.Patch(image);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(image);
        }

        // DELETE odata/Image(5)
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Image image = await db.Images.FindAsync(key);
            if (image == null)
            {
                return NotFound();
            }

            db.Images.Remove(image);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Feature(5)/Product
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Features.Where(m => m.FeatureId == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int key)
        {
            return db.Images.Count(e => e.ImageId == key) > 0;
        }
    }
}
