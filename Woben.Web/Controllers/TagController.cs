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
    [Authorize(Roles = "Administrator")]
    public class TagController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Tag
        [Queryable]
        public IQueryable<Tag> GetTag()
        {
            return db.Tags;
        }

        // GET odata/Tag(5)
        [Queryable]
        public SingleResult<Tag> GetTag([FromODataUri] int key)
        {
            return SingleResult.Create(db.Tags.Where(tag => tag.TagId == key));
        }

        // PUT odata/Tag(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != tag.TagId)
            {
                return BadRequest();
            }

            db.Entry(tag).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tag);
        }

        // POST odata/Tag
        public async Task<IHttpActionResult> Post(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tags.Add(tag);
            await db.SaveChangesAsync();

            return Created(tag);
        }

        // PATCH odata/Tag(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Tag> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tag tag = await db.Tags.FindAsync(key);
            if (tag == null)
            {
                return NotFound();
            }

            patch.Patch(tag);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tag);
        }

        // DELETE odata/Tag(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Tag tag = await db.Tags.FindAsync(key);
            if (tag == null)
            {
                return NotFound();
            }

            db.Tags.Remove(tag);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Tag(5)/Product
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Tags.Where(m => m.TagId == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TagExists(int key)
        {
            return db.Tags.Count(e => e.TagId == key) > 0;
        }
    }
}
