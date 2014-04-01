using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woben.Web.Cloud
{
    public class BlobInfo
    {
        public string ParentContainerName { get; set; }
        public Uri Uri { get; set; }        
        public string Filename { get; set; }
        public long FileLengthKilobytes { get; set; }
    }
}
