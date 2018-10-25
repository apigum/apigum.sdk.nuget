using Apigum.NuGet.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Apigum.NuGet
{



    public class Integration
    {

        public Integration(Guid apiKey)
        {
            //store key in memory for api calls

           // IntegrationHelper.Fill();
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

                var httpContent = new StringContent(JsonConvert.SerializeObject(integration), Encoding.UTF8, "application/json");
                var httpResponse = httpClient.PostAsync($"https://api.apigum.com/v1/integrations/{integrationId}/copy", httpContent);

                var statusCode = httpResponse.Result.StatusCode;
                var response = httpResponse.Result.Content.ReadAsStringAsync().Result;

                if (statusCode != HttpStatusCode.OK)
                    throw new Exception("response");

                return Guid.Parse(response);

            }


        }


        public void UpdateScript(Guid integrationId, string script)
        {
            //create new update script api
            throw new NotImplementedException();
        }

        public void Delete(Guid integrationId)
        {
            //call delete api
            throw new NotImplementedException();
        }

        public void ClearCache(Guid integrationId)
        {
            //call clear cache api
            throw new NotImplementedException();
        }

        public void Publish(Guid integrationId)
        {
            //call publish api
            throw new NotImplementedException();
        }

        public void Unpublish(Guid integrationId)
        {
            //call publish api
            throw new NotImplementedException();
        }



    }
}
