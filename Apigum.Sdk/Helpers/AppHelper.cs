using System.Collections.Generic;
using Apigum.Sdk.Model;

namespace Apigum.Sdk.Helpers
{
    public static class AppHelper
    {

        public static App Configure(string appId, Dictionary<string, string> keys)
        {
            return new App() { AppId = appId, Keys = keys };
        }


    }
}
