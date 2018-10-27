using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Apigum.Sdk.Helpers;
using Apigum.Sdk.Generation;


namespace Apigum.Sdk.Tests
{
    [TestClass]
    public class IntegrationsTests
    {

        public Integration Integration { get; set; }
        private Dictionary<string,string> FreshdeskCrentials { get; set; }
        private Dictionary<string, string> StripeCrentials { get; set; }


        public IntegrationsTests()
        {

            FreshdeskCrentials = new Dictionary<string, string>();
            StripeCrentials = new Dictionary<string, string>();

            //set up credentials
            FreshdeskCrentials.Add("apikey", "your freshdesk api key");
            FreshdeskCrentials.Add("subdomain", "your freshdesk subdomain");

            //set up credentials
            StripeCrentials.Add("secretkey", "your stripe secret key");
                        

            //obtain api key at https://account.apigum.com/api
            var apiKey = new Guid("your api key"); 
            Integration = new Integration(apiKey);
        }

        [TestMethod]
        public void CreateIntegrationTest()
        {
            var freshdesk = AppHelper.Configure(Apps.Freshdesk.AppId, FreshdeskCrentials);
            var stripe = AppHelper.Configure(Apps.Stripe.AppId, StripeCrentials);
            
            var integrationId = Integration.Create(freshdesk, stripe, 
                Apps.Freshdesk.Integrations.CREATE_FRESHDESK_CONTACT_FOR_NEW_STRIPE_CUSTOMERS);
            
            //save integration id for later use
        }


        [TestMethod]
        public void UpdateIntegrationScript()
        {
            var script = System.IO.File.ReadAllText("integration.js");
            var integrationId = new Guid("C0C052A5-5F5B-41F7-9390-D091F66D9B75");
            Integration.UpdateScript(integrationId, script);
        }


        [TestMethod]
        public void DeleteIntegration()
        {
            var script = System.IO.File.ReadAllText("integration.js");
            var integrationId = new Guid("C0C052A5-5F5B-41F7-9390-D091F66D9B75");
            Integration.Delete(integrationId);
        }


        [TestMethod]
        public void Start()
        {
            var integrationId = new Guid("C0C052A5-5F5B-41F7-9390-D091F66D9B75");
            Integration.Publish(integrationId);
        }


        [TestMethod]
        public void Stop()
        {
            var integrationId = new Guid("C0C052A5-5F5B-41F7-9390-D091F66D9B75");
            Integration.Unpublish(integrationId);
        }

    }
}
