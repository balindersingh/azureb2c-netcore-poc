using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureB2CDemoApp.Models
{
    
    public class CosmosDBOptions
    {
        public string DatabaseEndpoint { get; set; }
        public string DatabaseName { get; set; }
        public string SecretKey { get; set; }
    }
}
