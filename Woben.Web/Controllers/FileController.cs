using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

using Microsoft.WindowsAzure;

using Woben.Web.Cloud;
using Woben.Web.Helpers;
using System.Configuration;


namespace Woben.Web.Controllers
{
    [Authorize(Roles="Administrator")]
    public class FileController : ApiController
    {
        private ICloudBlobManager _cloudBlobManager;
        private readonly JavaScriptSerializer _js = new JavaScriptSerializer { MaxJsonLength = 41943040 };

        public FileController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;

            _cloudBlobManager = new CloudBlobManager();
            _cloudBlobManager.Initialize(connectionString);
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string filename)
        {            

            BlobInfo blobInfo = new BlobInfo();

            blobInfo.ParentContainerName = Settings.RootContainerName;
            blobInfo.Filename = filename;

            Stream stream = _cloudBlobManager.DownloadBlob(blobInfo);

            stream.Position = 0;

            WebAPIStreamResponseHelper responseHelper = new WebAPIStreamResponseHelper();

            return responseHelper.returnStreamResponseInformingMimeType(filename, stream);
        }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            return UploadFile(HttpContext.Current);
        }

        [HttpPut]
        public HttpResponseMessage Put()
        {
            return UploadFile(HttpContext.Current);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string filename)
        {
            _cloudBlobManager.DeleteBlob(Settings.RootContainerName, filename);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        private HttpResponseMessage UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            return WriteJsonIframeSafe(context, statuses);
        }

        private HttpResponseMessage WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(_js.Serialize(statuses.ToArray()))
            };
            if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            }
            return response;
        }

        // Upload partial file
        private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
        {

            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;
           
            _cloudBlobManager.StoreFileInAzureStorage(Settings.RootContainerName,  Guid.NewGuid() + fileName, inputStream);
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {            
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];

                var name = Guid.NewGuid() + file.FileName;

                _cloudBlobManager.StoreFileInAzureStorage(Settings.RootContainerName, name, file.InputStream);

                string fullName = Path.GetFileName(name);
                statuses.Add(new FilesStatus(fullName, file.ContentLength));
            }
        }


    }
}