using InStoreCollectionWebApi.Extensions;
using InStoreCollectionWebApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

//using System.Web.OData.Builder;
//using System.Web.OData.Extensions;


namespace InStoreCollectionWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<CollectionPoint>("CollectionPoints");
            builder.EntitySet<DeviceInfo>("DeviceInfoes");
            builder.EntitySet<Parcel>("Parcels");
            builder.EntitySet<ParcelGroup>("ParcelGroups");
            builder.EntitySet<Shop>("Shops");
            builder.EntitySet<ShopRegistration>("ShopRegistrations");
            builder.EntitySet<Store>("Stores");
            builder.EntitySet<StoreGroup>("StoreGroups");
            builder.EntitySet<StoreRegistration>("StoreRegistrations");

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel(), new EntityFrameworkBatchHandler(GlobalConfiguration.DefaultServer));

            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}
