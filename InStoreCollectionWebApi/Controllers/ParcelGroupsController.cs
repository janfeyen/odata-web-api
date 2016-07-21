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
    builder.EntitySet<ParcelGroup>("ParcelGroups");
    builder.EntitySet<CollectionPoint>("CollectionPoints"); 
    builder.EntitySet<Parcel>("Parcels"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ParcelGroupsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/ParcelGroups
        [EnableQuery]
        public IQueryable<ParcelGroup> GetParcelGroups()
        {
            return db.ParcelGroups;
        }

        // GET: odata/ParcelGroups(5)
        [EnableQuery]
        public SingleResult<ParcelGroup> GetParcelGroup([FromODataUri] int key)
        {
            return SingleResult.Create(db.ParcelGroups.Where(parcelGroup => parcelGroup.Id == key));
        }

        // PUT: odata/ParcelGroups(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ParcelGroup> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ParcelGroup parcelGroup = db.ParcelGroups.Find(key);
            if (parcelGroup == null)
            {
                return NotFound();
            }

            patch.Put(parcelGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelGroupExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(parcelGroup);
        }

        // POST: odata/ParcelGroups
        public IHttpActionResult Post(ParcelGroup parcelGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ParcelGroups.Add(parcelGroup);
            db.SaveChanges();

            return Created(parcelGroup);
        }

        // PATCH: odata/ParcelGroups(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ParcelGroup> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ParcelGroup parcelGroup = db.ParcelGroups.Find(key);
            if (parcelGroup == null)
            {
                return NotFound();
            }

            patch.Patch(parcelGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelGroupExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(parcelGroup);
        }

        // DELETE: odata/ParcelGroups(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ParcelGroup parcelGroup = db.ParcelGroups.Find(key);
            if (parcelGroup == null)
            {
                return NotFound();
            }

            db.ParcelGroups.Remove(parcelGroup);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/ParcelGroups(5)/CollectionPoint
        [EnableQuery]
        public SingleResult<CollectionPoint> GetCollectionPoint([FromODataUri] int key)
        {
            return SingleResult.Create(db.ParcelGroups.Where(m => m.Id == key).Select(m => m.CollectionPoint));
        }

        // GET: odata/ParcelGroups(5)/Parcels
        [EnableQuery]
        public IQueryable<Parcel> GetParcels([FromODataUri] int key)
        {
            return db.ParcelGroups.Where(m => m.Id == key).SelectMany(m => m.Parcels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParcelGroupExists(int key)
        {
            return db.ParcelGroups.Count(e => e.Id == key) > 0;
        }
    }
}
