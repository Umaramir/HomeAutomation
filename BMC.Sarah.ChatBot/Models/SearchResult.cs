using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMC.Sarah.ChatBot.Models
{
    public class SearchResult
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        public SearchResultHit[] Value { get; set; }
    }
}