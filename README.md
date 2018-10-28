# ApiGum.Sdk

[![NuGet](https://img.shields.io/nuget/v/apigum.svg)](https://www.nuget.org/packages/apigum/)

ApiGum SDK is a .Net library for managing integrations between popular cloud applications like Twilio, SendGrid, Shopify and others.

## Installation

 - ApiGum SDK is available as a [NuGet Package](https://www.nuget.org/packages/apigum.sdk/).
 - In your Visual Studio click on *Tools -> NuGet Package Manager -> Package Manager Console* and enter the following

	PM> Install-Package ApiGum.Sdk

## Usage

 - Log into your [apigum.com](apigum.com) account to obtain your API Key.  
 - You'll also need to obtain the relevant application keys. For example secret key for Stripe or Subdomain and Api Key for Freshdesk.
 - This library makes calls to the [apigum REST API](https://api.apigum.com/help).  

## Namespaces
All examples here are written as if you've added the following using statements to your file...

```cs
using Apigum.Sdk;
using Apigum.Sdk.Generation;
using Apigum.Sdk.Helpers;
```

### Setup
```cs
  //obtain api key at https://account.apigum.com/api
  var integration = new Integration(new Guid("your api key"));

  //intructions for obtaining credentials for Stripe & Freshdesk can be found on vendor sites or apigum integration page:
  //for example https://www.apigum.com/Integrations/6c6c6398-b628-450d-9faf-667d89113ed5

  //set up Freshdesk credentials
  var FreshdeskCrentials = new Dictionary<string, string>();
  FreshdeskCrentials.Add(Apps.Freshdesk.Keys.Apikey, "your Freshdesk api key");
  FreshdeskCrentials.Add(Apps.Freshdesk.Keys.Subdomain, "your Freshdesk subdomain");

  //set up Stripe credentials
  var StripeCrentials = new Dictionary<string, string>();
  StripeCrentials.Add(Apps.Stripe.Keys.Secretkey, "your Stripe secret key");
```

### Create Integration

```cs
  var freshdesk = AppHelper.Configure(Apps.Freshdesk.AppId, FreshdeskCrentials);
  var stripe = AppHelper.Configure(Apps.Stripe.AppId, StripeCrentials);

  //save integration id for later use
  var integrationId = Integration.Create(freshdesk, stripe,
                      Apps.Freshdesk.Integrations.CREATE_FRESHDESK_CONTACT_FOR_NEW_STRIPE_CUSTOMERS);

  //You may clone other integrations on apigum.com by using the id (last part) in the URL:
  //e.g.: https://www.apigum.com/Integrations/{integration-id}
```

### Update Integration

```cs
    var script = System.IO.File.ReadAllText("integration.js");
    var integrationId = new Guid("<Integration Id>");
    Integration.UpdateScript(integrationId, script);            
```

#### Sample integration.js
```js
//Integration code for => "Create Freshdesk contact for new Stripe customers"
  var freshdesk={};

  function setElements(stripe) {
      freshdesk.name = stripe.description;
      freshdesk.email = stripe.email;

  }

  function template() {
      return `{
    "name": "${freshdesk.name}",
    "email": "${freshdesk.email}",
    "other_emails": []
  }`;
  }

  module.exports = function (context, events) {

      let actions = [];

      for (let event of events.body) {
          setElements(event);
          actions.push(template());
      }

      context.res = {
          body: actions
      };

      context.done();
  };            
```

### Delete Integration
```cs
  Integration.Delete(new Guid("<Integration Id>"));
```

### Start Running
```cs
  //by default integrations start running when created
  //this method may be used if integration has been stopped.
  Integration.Publish(new Guid("<Integration Id>"));
```

### Stop Running
```cs
  //suspends integration data synchronization
  Integration.Unpublish(new Guid("<Integration Id>"));
```

For product information please visit our site at https://www.apigum.com
