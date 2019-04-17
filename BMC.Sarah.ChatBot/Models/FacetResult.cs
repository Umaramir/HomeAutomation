using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMC.Sarah.ChatBot.Models
{
    public class FacetResult
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonProperty("@search.facets")]
        public SearchFacets Facets { get; set; }

        public SearchResultHit[] Value { get; set; }
    }
}