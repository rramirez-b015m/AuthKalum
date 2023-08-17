using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KalumAuthManagement.Models
{

    public class UserInfo
    {
        [JsonPropertyName("id")]
        public string Id {get; set;} 
         [JsonPropertyName("username")]
        public string Username {get; set;}
         [JsonPropertyName("normalizedUserName")]
        public string NormalizedUsername {get; set;}
        [JsonPropertyName("email")]
        public string Email   {get; set;}
         [JsonPropertyName("password")]
        public string Password{get; set;}
         [JsonPropertyName("roles")]
         [Required(ErrorMessage ="Es necesario asignar un rol al usuario")]
        public List<string> Roles {get; set;}

    }
}