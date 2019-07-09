using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Models.JsonConverters;

namespace TaskManagerAPI.Models.APIRequests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [JsonConverter(typeof(TrimAndLowerCase))]
        public string Email { get; set; }
        [Required]
        [JsonConverter(typeof(TrimAndLowerCase))]
        public string Password { get; set; }
    }
}
