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
    builder.EntitySet<CollectionPoint>("CollectionPoints");
    builder.EntitySet<ParcelGroup>("ParcelGroups"); 
    builder.EntitySet<Shop>("Shops"); 
    builder.EntitySet<Store>("Stores"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CollectionPointsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/CollectionPoints
        [EnableQuery]
        public IQueryable<CollectionPoint> GetCollectionPoints()
        {
            return db.CollectionPoints;
        }

        // GET: odata/CollectionPoints(5)
        [EnableQuery]
        public SingleResult<CollectionPoint> GetCollectionPoint([FromODataUri] int key)
        {
            return SingleResult.Create(db.CollectionPoints.Where(collectionPoint => collectionPoint.Id == key));
        }

        // PUT: odata/CollectionPoints(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<CollectionPoint> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionPoint collectionPoint = db.CollectionPoints.Find(key);
            if (collectionPoint == null)
            {
                return NotFound();
            }

            patch.Put(collectionPoint);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionPointExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(collectionPoint);
        }

        // POST: odata/CollectionPoints
        public IHttpActionResult Post(CollectionPoint collectionPoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CollectionPoints.Add(collectionPoint);
            db.SaveChanges();

            return Created(collectionPoint);
        }

        // PATCH: odata/CollectionPoints(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<CollectionPoint> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionPoint collectionPoint = db.CollectionPoints.Find(key);
            if (collectionPoint == null)
            {
                return NotFound();
            }

            patch.Patch(collectionPoint);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionPointExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(collectionPoint);
        }

        // DELETE: odata/CollectionPoints(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            CollectionPoint collectionPoint = db.CollectionPoints.Find(key);
            if (collectionPoint == null)
            {
                return NotFound();
            }

            db.CollectionPoints.Remove(collectionPoint);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/CollectionPoints(5)/ParcelGroups
        [EnableQuery]
        public IQueryable<ParcelGroup> GetParcelGroups([FromODataUri] int key)
        {
            return db.CollectionPoints.Where(m => m.Id == key).SelectMany(m => m.ParcelGroups);
        }

        // GET: odata/CollectionPoints(5)/Shops
        [EnableQuery]
        public IQueryable<Shop> GetShops([FromODataUri] int key)
        {
            return db.CollectionPoints.Where(m => m.Id == key).SelectMany(m => m.Shops);
        }

        // GET: odata/CollectionPoints(5)/Stores
        [EnableQuery]
        public IQueryable<Store> GetStores([FromODataUri] int key)
        {
            return db.CollectionPoints.Where(m => m.Id == key).SelectMany(m => m.Stores);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CollectionPointExists(int key)
        {
            return db.CollectionPoints.Count(e => e.Id == key) > 0;
        }
    }
}
