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
    public class MessageController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Message
        [Queryable]
        public IQueryable<Message> GetMessage()
        {
            return db.Messages;
        }

        // GET odata/Message(5)
        [Queryable]
        public SingleResult<Message> GetMessage([FromODataUri] int key)
        {
            return SingleResult.Create(db.Messages.Where(message => message.MessageId == key));
        }

        // PUT odata/Message(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Message message)
        {

            message.UpdatedDate = DateTime.UtcNow;
            message.UpdatedBy = User.Identity.Name;


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != message.MessageId)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // POST odata/Message
        public async Task<IHttpActionResult> Post(Message message)
        {
            message.CreatedDate = DateTime.UtcNow;
            message.UpdatedDate = DateTime.UtcNow;
            message.CreatedBy = User.Identity.Name;
            message.UpdatedBy = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Messages.Add(message);
            await db.SaveChangesAsync();

            return Created(message);
        }

        // PATCH odata/Message(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Message> patch)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = await db.Messages.FindAsync(key);

            if (message == null)
            {
                return NotFound();
            }

            message.UpdatedDate = DateTime.UtcNow;
            message.UpdatedBy = User.Identity.Name;

            patch.Patch(message);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // DELETE odata/Message(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Message message = await db.Messages.FindAsync(key);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int key)
        {
            return db.Messages.Count(e => e.MessageId == key) > 0;
        }
    }
}
