using Apigum.NuGet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ApiGum.NuGet.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }



        static void WriteFile() {

            var sb = new StringBuilder();
            sb.AppendLine("namespace Apigum.NuGet {");
            sb.AppendLine("public class Apps {");

            JArray apps;

            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                //var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                var httpResponse = httpClient.GetAsync("https://api.apigum.com/v1/apps/published");

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                apps = JArray.Parse(response);

            }




            foreach (var app in apps)
            {
                var safename = JObject.Parse(app.ToString())["SafeName"].ToString();

                sb.AppendLine("public static class {app-name}".Replace("{app-name}", safename ));
                sb.AppendLine("public static class Integrations {");

                JArray integrations;

                using (var httpClient = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                    //var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                    var httpResponse = httpClient.GetAsync($"https://api.apigum.com/v1/apps/{safename}/integrations");

                    var statusCode = httpResponse.Result.StatusCode;
                    var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                    integrations = JArray.Parse(response);

                }



                foreach (var integration in integrations)
                {
                    sb.AppendLine("public static string {integration-title} = \"{integration-key}\";".Replace("{integration-title}", "").Replace("{integration-key}", ""));
                }

                sb.AppendLine("}//end of integrations");
                sb.AppendLine("public static string AppId = \"{app-key}\";");
                sb.AppendLine("} // end of app");
            }

            sb.AppendLine("}");
            sb.AppendLine("}");

        }


    }
}
