using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure;
using Woben.Web.Cloud;

namespace Woben.Web.Controllers
{
    [Authorize(Roles="Administrator")]
    public class StorageManagerController : ApiController
    {
        private ICloudBlobManager _cloudBlobManager;


        public StorageManagerController()
        {
            string connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");

            _cloudBlobManager = new CloudBlobManager();
            _cloudBlobManager.Initialize(connectionString);
        }

        // GET api/<controller>
        public IEnumerable<BlobInfo> Get()
        {
            return _cloudBlobManager.GetListOfBlobsFromContainer(Settings.RootContainerName);            
        }
    }
}