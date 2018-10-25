using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Apigum.NuGet.Helpers;

namespace Apigum.NuGet.Tests
{
    [TestClass]
    public class IntegrationsTests
    {

        public Integration Integration { get; set; }
        private Dictionary<string,string> FreshdeskCrentials { get; set; }
        private Dictionary<string, string> StripeCrentials { get; set; }


        public IntegrationsTests()
        {
            //set up credentials
            FreshdeskCrentials.Add("apikey", "your freshdesk api key");
            FreshdeskCrentials.Add("subdomain", "your freshdesk subdomain");

            //set up credentials
            StripeCrentials.Add("secretkey", "your stripe secret key");
            
            //apigum apikey available at https://account.apigum.com/api
            var apiKey = new Guid("6125e0b5-3920-4d05-8bbd-52264ce5c8ba"); 
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
            var integrationId = new Guid("A6CFEA44-71CF-4F84-AA85-92046B9D446F");
            Integration.UpdateScript(integrationId, script);
        }


        [TestMethod]
        public void DeleteIntegration()
        {
            var script = System.IO.File.ReadAllText("integration.js");
            var integrationId = new Guid("A6CFEA44-71CF-4F84-AA85-92046B9D446F");
            Integration.Delete(integrationId);
        }


        [TestMethod]
        public void PublishIntegration()
        {
            var integrationId = new Guid("A6CFEA44-71CF-4F84-AA85-92046B9D446F");
            Integration.Publish(integrationId);
        }


        [TestMethod]
        public void UnpublishIntegration()
        {
            var integrationId = new Guid("A6CFEA44-71CF-4F84-AA85-92046B9D446F");
            Integration.Unpublish(integrationId);
        }

    }
}
