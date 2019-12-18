using Newtonsoft.Json;
using TaskManagerAPI.Models.JsonConverters;

namespace TaskManagerAPI.Models.FE.APIRequests
{
    public class LoginRequest
    {
        [JsonConverter(typeof(TrimAndLowerCase))]
        public string Email { get; set; }

        [JsonConverter(typeof(Trim))]
        public string Password { get; set; }
    }
}