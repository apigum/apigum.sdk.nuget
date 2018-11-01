using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiGum.Sdk.Generator
{
    public static   class CSharpGenerator
    {

       public static void WriteFile()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

            var sb = new StringBuilder();
            sb.AppendLine("namespace Apigum.Sdk.Generation {");
            sb.AppendLine("public class Apps {");

            JArray apps;

            using (var httpClient = new HttpClient())
            {

                //var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                var httpResponse = httpClient.GetAsync("https://api.apigum.com/v1/apps/published");

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                apps = JArray.Parse(response);

            }




            foreach (var app in apps)
            {


                sb.AppendLine("public static class {app-name} {".Replace("{app-name}", FormatItem(app["Name"].ToString())));
                sb.AppendLine("public static class Integrations {");

                JArray integrations;

                using (var httpClient = new HttpClient())
                {
                    var httpResponse = httpClient.GetAsync($"https://api.apigum.com/v1/apps/{app["SafeName"].ToString()}/integrations");

                    var statusCode = httpResponse.Result.StatusCode;
                    var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                    integrations = JArray.Parse(JObject.Parse(response).SelectToken("Integrations").ToString());

                }






                foreach (var integration in integrations)
                {
                    sb.AppendLine("public const string {integration-title} = \"{integration-key}\";"
                        .Replace("{integration-title}", FormatItem(integration["Description"].ToString(), true))
                        .Replace("{integration-key}", integration["IntegrationId"].ToString()));
                }

                sb.AppendLine("}");


                sb.AppendLine("public static class Keys {");
                JArray Keys;

                using (var httpClient = new HttpClient())
                {

                    var httpResponse = httpClient.GetAsync($"https://api.apigum.com/v1/apps/{app["Id"].ToString()}");

                    var statusCode = httpResponse.Result.StatusCode;
                    var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                    Keys = JArray.Parse(JObject.Parse(response).SelectToken("AppKeys").ToString());

                }

                foreach (var key in Keys)
                {
                    sb.AppendLine($"public const string { System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(FormatItem(key["Name"].ToString()))} = \"{key["Name"].ToString()}\";");
                }

                sb.AppendLine("}");

                sb.AppendLine("public const string AppId = \"{app-key}\";".Replace("{app-key}", app["Id"].ToString()));
                sb.AppendLine("}");
            }

            sb.AppendLine("}");
            sb.AppendLine("}");


            System.IO.File.WriteAllText(@"Apps.cs", sb.ToString());



        }


        private static string FormatItem(string val, bool isUpperCase = false)
        {
            var rgx = new Regex("[^a-zA-Z0-9]");
            var result = rgx.Replace(val, "_");

            if (isUpperCase)
                result = result.ToUpper();

            if (Regex.IsMatch(result, @"^\d"))
                return "_" + result;
            else
                return result;
        }


    }
}
