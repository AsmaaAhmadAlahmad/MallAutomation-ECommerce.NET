using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public required string  email { get; set; }

        [Required]
       
        public string Password { get; set; }
    }
}
