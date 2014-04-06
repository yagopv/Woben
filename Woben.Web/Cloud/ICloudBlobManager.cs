using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woben.Web.Cloud
{
    public interface ICloudBlobManager
    {
        void Initialize(string connectionstring);
        void StoreFileInAzureStorage(string blobContainer, string filename, Stream stream);
        IEnumerable<BlobInfo> GetListOfBlobsFromContainer(string containerName);
        Stream DownloadBlob(BlobInfo blobInfo);
        bool DeleteBlob(string containername, string file);
    }
}
