using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Grit.Models.Training
{
    public class FeedCalendarApi
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("result")]
        public IList<DayCalendar> Result { get; set; }
    }

    public class DayCalendar
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("class")]
        public string Kind { get; set; }
        [JsonProperty("start")]
        public string Start { get; set; }
        [JsonProperty("end")]
        public string End { get; set; }


    }
}