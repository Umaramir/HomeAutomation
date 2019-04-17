using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMC.Sarah.ChatBot.Models
{
    public class SearchResultHit
    {
        [JsonProperty("@search.score")]
        public float SearchScore { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
    }
}