using System.ComponentModel.DataAnnotations;

namespace DiyawannaSupBackend.Api.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int Age { get; set; }
        public string University { get; set; }
        public string School { get; set; }
        public string Work { get; set; }
    }
}
