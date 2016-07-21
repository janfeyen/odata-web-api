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
    builder.EntitySet<DeviceInfo>("DeviceInfoes");
    builder.EntitySet<ShopRegistration>("ShopRegistrations"); 
    builder.EntitySet<StoreRegistration>("StoreRegistrations"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DeviceInfoesController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/DeviceInfoes
        [EnableQuery]
        public IQueryable<DeviceInfo> GetDeviceInfoes()
        {
            return db.DeviceInfoes;
        }

        // GET: odata/DeviceInfoes(5)
        [EnableQuery]
        public SingleResult<DeviceInfo> GetDeviceInfo([FromODataUri] int key)
        {
            return SingleResult.Create(db.DeviceInfoes.Where(deviceInfo => deviceInfo.Id == key));
        }

        // PUT: odata/DeviceInfoes(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<DeviceInfo> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceInfo deviceInfo = db.DeviceInfoes.Find(key);
            if (deviceInfo == null)
            {
                return NotFound();
            }

            patch.Put(deviceInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceInfoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(deviceInfo);
        }

        // POST: odata/DeviceInfoes
        public IHttpActionResult Post(DeviceInfo deviceInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DeviceInfoes.Add(deviceInfo);
            db.SaveChanges();

            return Created(deviceInfo);
        }

        // PATCH: odata/DeviceInfoes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<DeviceInfo> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeviceInfo deviceInfo = db.DeviceInfoes.Find(key);
            if (deviceInfo == null)
            {
                return NotFound();
            }

            patch.Patch(deviceInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceInfoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(deviceInfo);
        }

        // DELETE: odata/DeviceInfoes(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            DeviceInfo deviceInfo = db.DeviceInfoes.Find(key);
            if (deviceInfo == null)
            {
                return NotFound();
            }

            db.DeviceInfoes.Remove(deviceInfo);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DeviceInfoes(5)/ShopRegistration
        [EnableQuery]
        public SingleResult<ShopRegistration> GetShopRegistration([FromODataUri] int key)
        {
            return SingleResult.Create(db.DeviceInfoes.Where(m => m.Id == key).Select(m => m.ShopRegistration));
        }

        // GET: odata/DeviceInfoes(5)/StoreRegistration
        [EnableQuery]
        public SingleResult<StoreRegistration> GetStoreRegistration([FromODataUri] int key)
        {
            return SingleResult.Create(db.DeviceInfoes.Where(m => m.Id == key).Select(m => m.StoreRegistration));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceInfoExists(int key)
        {
            return db.DeviceInfoes.Count(e => e.Id == key) > 0;
        }
    }
}
