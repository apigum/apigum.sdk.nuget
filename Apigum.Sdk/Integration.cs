using Apigum.Sdk.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Apigum.Sdk
{



    public class Integration
    {

        public Guid ApiKey { get; set; }
       // private const string baseApi = "http://localhost/integration";
        private const string baseApi = "https://api.apigum.com/";


        public Integration(Guid apiKey)
        {
            ApiKey = apiKey;
        }

        public bool UpdateCredentials(Guid apiKey, Dictionary<string, string> credentials)
        {
            throw new NotImplementedException();
        }

        
        public Guid Create(App triggerApp, App actionApp , string integrationId)
        {
            

            
            
            // set trigger
            var trigger = new IntegrationCopyRequest.IntegrationCopyItem();
            trigger.AppId = Guid.Parse(triggerApp.AppId);
            

            foreach (var item in triggerApp.Keys)
            {
                trigger.KeyValuePairs.Add(new KeyValuePair() { Name = item.Key, Value = item.Value });
            }


            // set action
            var action = new IntegrationCopyRequest.IntegrationCopyItem();
            action.AppId = Guid.Parse(actionApp.AppId);
            
            foreach (var item in actionApp.Keys)
            {
                action.KeyValuePairs.Add(new KeyValuePair() { Name = item.Key, Value = item.Value });
            }


            var integration = new IntegrationCopyRequest();
            integration.TriggerKeys.Add(trigger);
            integration.ActionKeys.Add(action);


            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;
                
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{ApiKey.ToString()}:X")));

                var httpContent = new StringContent(JsonConvert.SerializeObject(integration), Encoding.UTF8, "application/json");
                var httpResponse = httpClient.PostAsync($"{baseApi}/v1/integrations/{integrationId}/copy", httpContent);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception(response);

                return Guid.Parse(response.Replace("\"", ""));

            }


        }


        public bool UpdateScript(Guid integrationId, string script)
        {

            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{ApiKey.ToString()}:X")));

                var httpContent = new StringContent(JsonConvert.SerializeObject(new IntegrationScriptRequest() { Code=script }), 
                    Encoding.UTF8, "application/json");
                var httpResponse = httpClient.PutAsync($"{baseApi}/v1/integrations/{integrationId}/code", httpContent);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception(response);

                return true;

            }
        }

        public bool Delete(Guid integrationId)
        {
            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{ApiKey.ToString()}:X")));

                var httpResponse = httpClient.DeleteAsync($"{baseApi}/v1/integrations/{integrationId}");

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception(response);

                return true;

            }
        }

        public void ClearCache(Guid integrationId)
        {
            //call clear cache api
            throw new NotImplementedException();
        }

        public bool Publish(Guid integrationId)
        {
            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{ApiKey.ToString()}:X")));
                                
                var httpResponse = httpClient.PutAsync($"{baseApi}/v1/integrations/{integrationId}/publish", null);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception(response);

                return true;

            }
        }

        public bool Unpublish(Guid integrationId)
        {
            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{ApiKey.ToString()}:X")));

                var httpResponse = httpClient.PutAsync($"{baseApi}/v1/integrations/{integrationId}/unpublish", null);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception(response);

                return true;

            }
        }



    }
}
