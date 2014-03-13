using System.Collections.Generic;
using System.Xml.Linq;

namespace Woben.Web.SEO
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
