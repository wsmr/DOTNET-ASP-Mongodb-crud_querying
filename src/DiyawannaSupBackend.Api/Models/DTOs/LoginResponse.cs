namespace DiyawannaSupBackend.Api.Models.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public long ExpiresIn { get; set; } // In milliseconds
        public string Message { get; set; }
    }
}
