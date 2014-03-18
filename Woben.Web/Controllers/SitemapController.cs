using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Woben.Web.Results;
using Woben.Web.SEO;
using System.IO;
using Woben.Data;

namespace Woben.Web.Controllers
{
    public class SitemapController : Controller
    {
        WobenDbContext db = new WobenDbContext();
        //
        // GET: /Sitemap/
        public ActionResult Sitemap()
        {
            var url = System.Web.HttpContext.Current.Request.Url;
            var leftpart = url.Scheme + "://" + url.Authority;

            // Static Urls
            var sitemapItems = new List<SitemapItem>
            {                
                new SitemapItem(leftpart + "/home/index", changeFrequency: SitemapChangeFrequency.Weekly, priority: 1.0),
                new SitemapItem(leftpart + "/home/products", changeFrequency: SitemapChangeFrequency.Daily, priority: 1.0),
                new SitemapItem(leftpart + "/home/help", changeFrequency: SitemapChangeFrequency.Weekly, priority: 1.0),
                new SitemapItem(leftpart + "/home/about", changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0),
                new SitemapItem(leftpart + "/account/login", changeFrequency: SitemapChangeFrequency.Monthly, priority: 0.5),
                new SitemapItem(leftpart + "/account/register", changeFrequency: SitemapChangeFrequency.Monthly, priority: 0.5)
            };

            var products = db.Products.Where(p => p.IsPublished);

            foreach (var product in products)
            {
                sitemapItems.Add(new SitemapItem(leftpart + "/" + product.CreatedBy + "/" + product.Category.Name + "/" + product.UrlCodeReference, changeFrequency: SitemapChangeFrequency.Weekly, priority: 0.8));
            }

            return new SitemapResult(sitemapItems);
        }
	}
}