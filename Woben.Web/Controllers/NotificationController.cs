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
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

using Woben.Domain.Model;
using Woben.Data;
using Woben.Web.Helpers;
using Woben.Web.Models;

namespace Woben.Web.Controllers
{
    [Authorize]
    public class NotificationController : ODataController
    {
        private WobenDbContext db = new WobenDbContext();

        private ApplicationUserManager usermanager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return usermanager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                usermanager = value;
            }
        }

        // GET odata/Notification
        [Queryable]
        public IQueryable<Notification> GetNotification()
        {
            return db.Notifications;
        }

        // GET odata/Notification(5)
        [Queryable]
        public SingleResult<Notification> GetNotification([FromODataUri] int key)
        {
            return SingleResult.Create(db.Notifications.Where(notification => notification.MessageId == key));
        }

        // PUT odata/Notification(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Notification notification)
        {

            notification.UpdatedDate = DateTime.UtcNow;
            notification.UpdatedBy = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != notification.MessageId)
            {
                return BadRequest();
            }

            db.Entry(notification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(notification);
        }

        // POST odata/Notification
        public async Task<IHttpActionResult> Post(Notification notification)
        {

            notification.CreatedDate = DateTime.UtcNow;
            notification.UpdatedDate = DateTime.UtcNow;
            notification.CreatedBy = User.Identity.Name;
            notification.UpdatedBy = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notifications.Add(notification);

            // Check is the user don´t have phone numbers
            var user = db.Users.Find(User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(user.PhoneNumber))
            {
                user.PhoneNumber = notification.PhoneNumber;
                user.PhoneNumberConfirmed = true;
            }

            await db.SaveChangesAsync();


            // Send notifications to admin users
            var product = await db.Products.FindAsync(notification.ProductId);

            string body = ViewRenderer.RenderView("~/Views/Mailer/Notification.cshtml", notification);

            var role = await db.Roles.Where(r => r.Name == "Administrator").FirstAsync();

            var adminUsers = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));

            foreach (var adminUser in adminUsers.ToList())
            {
                await UserManager.SendEmailAsync(adminUser.Id, "Se ha recibido una nueva notificación acerca del producto '" + product.Name + "'", body);
            }

            return Created(notification);
        }

        // PATCH odata/Notification(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Notification> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Notification notification = await db.Notifications.FindAsync(key);
            if (notification == null)
            {
                return NotFound();
            }
            
            notification.UpdatedDate = DateTime.UtcNow;
            notification.UpdatedBy = User.Identity.Name;

            patch.Patch(notification);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(notification);
        }

        // DELETE odata/Notification(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Notification notification = await db.Notifications.FindAsync(key);
            if (notification == null)
            {
                return NotFound();
            }

            db.Notifications.Remove(notification);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Notification(5)/Product
        [Queryable]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(db.Notifications.Where(m => m.MessageId == key).Select(m => m.Product));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(int key)
        {
            return db.Notifications.Count(e => e.MessageId == key) > 0;
        }
    }
}
