using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using InStoreCollectionWebApi.Models;

namespace InStoreCollectionWebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using InStoreCollectionWebApi.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ShopRegistration>("ShopRegistrations");
    builder.EntitySet<DeviceInfo>("DeviceInfoes"); 
    builder.EntitySet<Shop>("Shops"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ShopRegistrationsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/ShopRegistrations
        [EnableQuery]
        public IQueryable<ShopRegistration> GetShopRegistrations()
        {
            return db.ShopRegistrations;
        }

        // GET: odata/ShopRegistrations(5)
        [EnableQuery]
        public SingleResult<ShopRegistration> GetShopRegistration([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShopRegistrations.Where(shopRegistration => shopRegistration.Id == key));
        }

        // PUT: odata/ShopRegistrations(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ShopRegistration> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShopRegistration shopRegistration = db.ShopRegistrations.Find(key);
            if (shopRegistration == null)
            {
                return NotFound();
            }

            patch.Put(shopRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopRegistrationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shopRegistration);
        }

        // POST: odata/ShopRegistrations
        public IHttpActionResult Post(ShopRegistration shopRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShopRegistrations.Add(shopRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ShopRegistrationExists(shopRegistration.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(shopRegistration);
        }

        // PATCH: odata/ShopRegistrations(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ShopRegistration> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShopRegistration shopRegistration = db.ShopRegistrations.Find(key);
            if (shopRegistration == null)
            {
                return NotFound();
            }

            patch.Patch(shopRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopRegistrationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shopRegistration);
        }

        // DELETE: odata/ShopRegistrations(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ShopRegistration shopRegistration = db.ShopRegistrations.Find(key);
            if (shopRegistration == null)
            {
                return NotFound();
            }

            db.ShopRegistrations.Remove(shopRegistration);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ShopRegistrations(5)/DeviceInfo
        [EnableQuery]
        public SingleResult<DeviceInfo> GetDeviceInfo([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShopRegistrations.Where(m => m.Id == key).Select(m => m.DeviceInfo));
        }

        // GET: odata/ShopRegistrations(5)/Shop
        [EnableQuery]
        public SingleResult<Shop> GetShop([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShopRegistrations.Where(m => m.Id == key).Select(m => m.Shop));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShopRegistrationExists(int key)
        {
            return db.ShopRegistrations.Count(e => e.Id == key) > 0;
        }
    }
}
