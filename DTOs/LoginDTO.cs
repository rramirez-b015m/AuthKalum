using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KalumAuthManagement.DTOs
{
    public class LoginDTO
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username {get;set;}
        [Required]
        [JsonPropertyName("password")]
        public string Password {get; set;}
    }
}