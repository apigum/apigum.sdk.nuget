
using System;
using System.Collections.Generic;


namespace Apigum.Sdk
{
    public class IntegrationCopyRequest
    {

        public IntegrationCopyRequest()
        {
            TriggerKeys = new List<IntegrationCopyItem>();
            ActionKeys = new List<IntegrationCopyItem>();
        }

        public List<IntegrationCopyItem> TriggerKeys { get; set; }

        public List<IntegrationCopyItem> ActionKeys { get; set; }
        

        public class IntegrationCopyItem
        {

            public IntegrationCopyItem()
            {
                KeyValuePairs = new List<KeyValuePair>();
            }

            public Guid AppId { get; set; }

            public List<KeyValuePair> KeyValuePairs { get; set; }

        }


    }
}
