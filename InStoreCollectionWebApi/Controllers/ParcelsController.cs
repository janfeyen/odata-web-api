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
    builder.EntitySet<Parcel>("Parcels");
    builder.EntitySet<ParcelGroup>("ParcelGroups"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ParcelsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/Parcels
        [EnableQuery]
        public IQueryable<Parcel> GetParcels()
        {
            return db.Parcels;
        }

        // GET: odata/Parcels(5)
        [EnableQuery]
        public SingleResult<Parcel> GetParcel([FromODataUri] int key)
        {
            return SingleResult.Create(db.Parcels.Where(parcel => parcel.Id == key));
        }

        // PUT: odata/Parcels(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Parcel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Parcel parcel = db.Parcels.Find(key);
            if (parcel == null)
            {
                return NotFound();
            }

            patch.Put(parcel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(parcel);
        }

        // POST: odata/Parcels
        public IHttpActionResult Post(Parcel parcel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Parcels.Add(parcel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ParcelExists(parcel.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(parcel);
        }

        // PATCH: odata/Parcels(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Parcel> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Parcel parcel = db.Parcels.Find(key);
            if (parcel == null)
            {
                return NotFound();
            }

            patch.Patch(parcel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(parcel);
        }

        // DELETE: odata/Parcels(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Parcel parcel = db.Parcels.Find(key);
            if (parcel == null)
            {
                return NotFound();
            }

            db.Parcels.Remove(parcel);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Parcels(5)/ParcelGroup
        [EnableQuery]
        public SingleResult<ParcelGroup> GetParcelGroup([FromODataUri] int key)
        {
            return SingleResult.Create(db.Parcels.Where(m => m.Id == key).Select(m => m.ParcelGroup));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParcelExists(int key)
        {
            return db.Parcels.Count(e => e.Id == key) > 0;
        }
    }
}
