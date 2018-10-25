
using System.Net;
using System.Net.Http;
using System.Text;

namespace Apigum.NuGet
{
    public static class RestHelper
    {

        public static RestResponse Post(string request, string url)
        {

        

            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;
                
                var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                var httpResponse = httpClient.PostAsync(url, httpContent);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                return new RestResponse() { StatusCode = statusCode, Response = response };

            }





        }





    }


    public class RestResponse
    {
        public string Response { get; set; }

        public HttpStatusCode StatusCode { get; set; }

    }



}