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
    builder.EntitySet<Shop>("Shops");
    builder.EntitySet<CollectionPoint>("CollectionPoints"); 
    builder.EntitySet<ShopRegistration>("ShopRegistrations"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ShopsController : ODataController
    {
        private InStoreCollectionEntities db = new InStoreCollectionEntities();

        // GET: odata/Shops
        [EnableQuery]
        public IQueryable<Shop> GetShops()
        {
            return db.Shops;
        }

        // GET: odata/Shops(5)
        [EnableQuery]
        public SingleResult<Shop> GetShop([FromODataUri] int key)
        {
            return SingleResult.Create(db.Shops.Where(shop => shop.Id == key));
        }

        // PUT: odata/Shops(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Shop> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Shop shop = db.Shops.Find(key);
            if (shop == null)
            {
                return NotFound();
            }

            patch.Put(shop);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shop);
        }

        // POST: odata/Shops
        public IHttpActionResult Post(Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shops.Add(shop);
            db.SaveChanges();

            return Created(shop);
        }

        // PATCH: odata/Shops(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Shop> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Shop shop = db.Shops.Find(key);
            if (shop == null)
            {
                return NotFound();
            }

            patch.Patch(shop);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shop);
        }

        // DELETE: odata/Shops(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Shop shop = db.Shops.Find(key);
            if (shop == null)
            {
                return NotFound();
            }

            db.Shops.Remove(shop);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Shops(5)/CollectionPoint
        [EnableQuery]
        public SingleResult<CollectionPoint> GetCollectionPoint([FromODataUri] int key)
        {
            return SingleResult.Create(db.Shops.Where(m => m.Id == key).Select(m => m.CollectionPoint));
        }

        // GET: odata/Shops(5)/ShopRegistrations
        [EnableQuery]
        public IQueryable<ShopRegistration> GetShopRegistrations([FromODataUri] int key)
        {
            return db.Shops.Where(m => m.Id == key).SelectMany(m => m.ShopRegistrations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShopExists(int key)
        {
            return db.Shops.Count(e => e.Id == key) > 0;
        }
    }
}
