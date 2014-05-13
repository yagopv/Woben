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
    /*
    Para agregar una ruta para este controlador, combine estas instrucciones al método Register de la clase WebApiConfig. Tenga en cuenta que las direcciones URL de OData distinguen mayúsculas de minúsculas.

    using System.Web.Http.OData.Builder;
    using Woben.Domain.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Message>("Message");
    builder.EntitySet<Product>("Products"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
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

        // GET odata/Message(5)/Product
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Messages.Where(m => m.MessageId == key).Select(m => m.Product));
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
