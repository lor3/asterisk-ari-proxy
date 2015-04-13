using Newtonsoft.Json;

namespace AriProxy.Service
{
    internal class Command
    {
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    internal class CommandResult
    {
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("response_body")]
        public string ResponseBody { get; set; }
    }
}
