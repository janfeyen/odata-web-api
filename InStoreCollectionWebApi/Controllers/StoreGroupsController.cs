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
    builder.EntitySet<StoreGroup>("StoreGroups");
    builder.EntitySet<Store>("Stores"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StoreGroupsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/StoreGroups
        [EnableQuery]
        public IQueryable<StoreGroup> GetStoreGroups()
        {
            return db.StoreGroups;
        }

        // GET: odata/StoreGroups(5)
        [EnableQuery]
        public SingleResult<StoreGroup> GetStoreGroup([FromODataUri] int key)
        {
            return SingleResult.Create(db.StoreGroups.Where(storeGroup => storeGroup.Id == key));
        }

        // PUT: odata/StoreGroups(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<StoreGroup> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreGroup storeGroup = db.StoreGroups.Find(key);
            if (storeGroup == null)
            {
                return NotFound();
            }

            patch.Put(storeGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGroupExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storeGroup);
        }

        // POST: odata/StoreGroups
        public IHttpActionResult Post(StoreGroup storeGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoreGroups.Add(storeGroup);
            db.SaveChanges();

            return Created(storeGroup);
        }

        // PATCH: odata/StoreGroups(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<StoreGroup> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoreGroup storeGroup = db.StoreGroups.Find(key);
            if (storeGroup == null)
            {
                return NotFound();
            }

            patch.Patch(storeGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreGroupExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storeGroup);
        }

        // DELETE: odata/StoreGroups(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            StoreGroup storeGroup = db.StoreGroups.Find(key);
            if (storeGroup == null)
            {
                return NotFound();
            }

            db.StoreGroups.Remove(storeGroup);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StoreGroups(5)/Stores
        [EnableQuery]
        public IQueryable<Store> GetStores([FromODataUri] int key)
        {
            return db.StoreGroups.Where(m => m.Id == key).SelectMany(m => m.Stores);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreGroupExists(int key)
        {
            return db.StoreGroups.Count(e => e.Id == key) > 0;
        }
    }
}
