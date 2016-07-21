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
    builder.EntitySet<StoreRegistration>("StoreRegistrations");
    builder.EntitySet<DeviceInfo>("DeviceInfoes"); 
    builder.EntitySet<Store>("Stores"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StoreRegistrationsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/StoreRegistrations
        [EnableQuery]
        public IQueryable<StoreRegistration> GetStoreRegistrations()
        {
            return db.StoreRegistrations;
        }

        // GET: odata/StoreRegistrations(5)
        [EnableQuery]
        public SingleResult<StoreRegistration> GetStoreRegistration([FromODataUri] int key)
        {
            return SingleResult.Create(db.StoreRegistrations.Where(storeRegistration => storeRegistration.Id == key));
        }

        // PUT: odata/StoreRegistrations(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<StoreRegistration> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreRegistration storeRegistration = db.StoreRegistrations.Find(key);
            if (storeRegistration == null)
            {
                return NotFound();
            }

            patch.Put(storeRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreRegistrationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storeRegistration);
        }

        // POST: odata/StoreRegistrations
        public IHttpActionResult Post(StoreRegistration storeRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreRegistrations.Add(storeRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StoreRegistrationExists(storeRegistration.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(storeRegistration);
        }

        // PATCH: odata/StoreRegistrations(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<StoreRegistration> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreRegistration storeRegistration = db.StoreRegistrations.Find(key);
            if (storeRegistration == null)
            {
                return NotFound();
            }

            patch.Patch(storeRegistration);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreRegistrationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storeRegistration);
        }

        // DELETE: odata/StoreRegistrations(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            StoreRegistration storeRegistration = db.StoreRegistrations.Find(key);
            if (storeRegistration == null)
            {
                return NotFound();
            }

            db.StoreRegistrations.Remove(storeRegistration);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StoreRegistrations(5)/DeviceInfo
        [EnableQuery]
        public SingleResult<DeviceInfo> GetDeviceInfo([FromODataUri] int key)
        {
            return SingleResult.Create(db.StoreRegistrations.Where(m => m.Id == key).Select(m => m.DeviceInfo));
        }

        // GET: odata/StoreRegistrations(5)/Store
        [EnableQuery]
        public SingleResult<Store> GetStore([FromODataUri] int key)
        {
            return SingleResult.Create(db.StoreRegistrations.Where(m => m.Id == key).Select(m => m.Store));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreRegistrationExists(int key)
        {
            return db.StoreRegistrations.Count(e => e.Id == key) > 0;
        }
    }
}
