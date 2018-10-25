using System.Collections.Generic;
using Apigum.NuGet.Model;

namespace Apigum.NuGet.Helpers
{
    public static class AppHelper
    {

        public static App Configure(string appId, Dictionary<string, string> keys)
        {
            return new App() { AppId = appId, Keys = keys };
        }


    }
}
