using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiGum.Sdk.Generator
{
    public static   class JavaScriptGenerator
    {

       public static void WriteFile()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

            var sb = new StringBuilder();
            sb.AppendLine("module.exports = {");
         
            JArray apps;

            using (var httpClient = new HttpClient())
            {

                var httpResponse = httpClient.GetAsync("https://api.apigum.com/v1/apps/published");

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                apps = JArray.Parse(response);

            }




            foreach (var app in apps)
            {

                sb.AppendLine($"{FormatItem(app["Name"].ToString())}: {{");
                sb.AppendLine("Integrations: {");


                JArray integrations;

                using (var httpClient = new HttpClient())
                {
                    var httpResponse = httpClient.GetAsync($"https://api.apigum.com/v1/apps/{app["SafeName"].ToString()}/integrations");

                    var statusCode = httpResponse.Result.StatusCode;
                    var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                    integrations = JArray.Parse(JObject.Parse(response).SelectToken("Integrations").ToString());

                }






                foreach (var integration in integrations)
                    sb.AppendLine($"{FormatItem(integration["Description"].ToString(), true)}: \"{integration["IntegrationId"].ToString()}\",");

                sb.AppendLine("},");


                sb.AppendLine("Keys: {");
                JArray Keys;

                using (var httpClient = new HttpClient())
                {

                    var httpResponse = httpClient.GetAsync($"https://api.apigum.com/v1/apps/{app["Id"].ToString()}");

                    var statusCode = httpResponse.Result.StatusCode;
                    var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                    Keys = JArray.Parse(JObject.Parse(response).SelectToken("AppKeys").ToString());

                }

                foreach (var key in Keys)
                    sb.AppendLine($"{ System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(FormatItem(key["Name"].ToString()))}: \"{key["Name"].ToString()}\",");

                sb.AppendLine("},");

                sb.AppendLine($"AppId: \"{app["Id"].ToString()}\"");
                sb.AppendLine("},");
            }

            sb.AppendLine("}");
   
            System.IO.File.WriteAllText(@"index.js", sb.ToString());



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
