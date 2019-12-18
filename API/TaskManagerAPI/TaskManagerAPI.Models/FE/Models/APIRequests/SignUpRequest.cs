using Newtonsoft.Json;
using TaskManagerAPI.Models.JsonConverters;

namespace TaskManagerAPI.Models.FE.APIRequests
{
    public class SignUpRequest
    {
        [JsonConverter(typeof(Trim))]
        public string FullName { get; set; }

        [JsonConverter(typeof(TrimAndLowerCase))]
        public string Email { get; set; }

        [JsonConverter(typeof(Trim))]
        public string Password { get; set; }
    }
}