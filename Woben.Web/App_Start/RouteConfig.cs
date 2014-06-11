using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http.OData.Builder;

using Newtonsoft.Json.Serialization;

using Woben.Domain.Model;
using Woben.Web.Filters;

namespace Woben.Web
{
    public static class RouteConfig
    {
        public static void RegisterWebApiRoutes(HttpConfiguration config)
        {
            //Use web api routing
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove Xml formatters. This means when we visit an endpoint from a browser,
            // Instead of returning Xml, it will return Json.
            // More information from Dave Ward: http://jpapa.me/P4vdx6
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Configure json camelCasing per the following post: http://jpapa.me/NqC2HH
            // Here we configure it to write JSON property names with camel casing
            // without changing our server-side data model:
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();            

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Product");
            builder.EntitySet<Category>("Category");
            builder.EntitySet<Tag>("Tag");
            builder.EntitySet<Feature>("Feature");
            builder.EntitySet<Message>("Message");
            builder.EntitySet<Notification>("Notification");
            builder.EntitySet<Image>("Image");

            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());

        }

        public static void RegisterMVCRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Sitemap", 
                url: "sitemap",
                defaults: new { controller = "Sitemap", action = "Sitemap" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}