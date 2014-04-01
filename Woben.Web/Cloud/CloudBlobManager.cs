using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Woben.Web.Cloud
{
    public class CloudBlobManager : ICloudBlobManager
    {
        private CloudStorageAccount _storageAccount = null;

        public void Initialize(string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public void StoreFileInAzureStorage(string containerName, string filename, System.IO.Stream stream)
        {
            CloudBlobContainer container = GetBlobContainerCreateIfDoesntExists(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStream(stream);
        }

        public IEnumerable<BlobInfo> GetListOfBlobsFromContainer(string containerName)
        {
            List<BlobInfo> blobInfos = new List<BlobInfo>();
            
            CloudBlobContainer container = GetBlobContainerCreateIfDoesntExists(containerName);

            // Only traverse first level no directories, no pages
            foreach (IListBlobItem item in container.ListBlobs(null))
            {
                if (item.GetType() == typeof (CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob) item;
                    
                    BlobInfo blobInfo = new BlobInfo();

                    blobInfo.Uri      = blob.Uri;
                    blobInfo.Filename = blob.Name;
                    blobInfo.ParentContainerName = blob.Parent.Container.Name;
                    blobInfo.FileLengthKilobytes = ConvertBytesToKilobytes(blob.Properties.Length);

                    blobInfos.Add(blobInfo);
                }
            }

            return blobInfos;
        }

        public Stream DownloadBlob(BlobInfo blobInfo)
        {
            Stream stream = new MemoryStream();
            CloudBlobContainer container = GetBlobContainerCreateIfDoesntExists(blobInfo.ParentContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobInfo.Filename);

            blockBlob.DownloadToStream(stream);

            return stream;
        }

        private CloudBlobContainer GetBlobContainerCreateIfDoesntExists(string containerName)
        {
            CloudBlobContainer container = null;
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();

            return container;
        }

        private long ConvertBytesToKilobytes(long byteslength)
        {
            return (byteslength / 1024);
        }
    }
}
