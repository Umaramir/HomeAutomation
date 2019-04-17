using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMC.Sarah.ChatBot.Models
{
    public class SearchFacets
    {
        [JsonProperty("category@odata.type")]
        public string CategoryOdataType { get; set; }

        public Category[] Category { get; set; }
    }
}