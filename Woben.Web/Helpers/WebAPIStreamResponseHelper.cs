using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Woben.Web.Helpers
{
    public class WebAPIStreamResponseHelper
    {
        // U Test candidate
        public  HttpResponseMessage returnStreamResponseInformingMimeType(string fileName, Stream stream)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(stream);

            string fileExtension = Path.GetExtension(fileName);

            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue(FileExtensionToMimeType.GetMimeType(fileExtension));


            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return result;

        }
      
    }
}