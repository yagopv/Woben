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
    builder.EntitySet<Feature>("Feature");
    builder.EntitySet<Product>("Products"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FeatureController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        // GET odata/Feature
        [Queryable]
        public IQueryable<Feature> GetFeature()
        {
            return db.Features;
        }

        // GET odata/Feature(5)
        [Queryable]
        public SingleResult<Feature> GetFeature([FromODataUri] int key)
        {
            return SingleResult.Create(db.Features.Where(feature => feature.FeatureId == key));
        }

        // PUT odata/Feature(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != feature.FeatureId)
            {
                return BadRequest();
            }

            db.Entry(feature).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(feature);
        }

        // POST odata/Feature
        public async Task<IHttpActionResult> Post(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Features.Add(feature);
            await db.SaveChangesAsync();

            return Created(feature);
        }

        // PATCH odata/Feature(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Feature> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Feature feature = await db.Features.FindAsync(key);
            if (feature == null)
            {
                return NotFound();
            }

            patch.Patch(feature);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(feature);
        }

        // DELETE odata/Feature(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Feature feature = await db.Features.FindAsync(key);
            if (feature == null)
            {
                return NotFound();
            }

            db.Features.Remove(feature);
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

        private bool FeatureExists(int key)
        {
            return db.Features.Count(e => e.FeatureId == key) > 0;
        }
    }
}
