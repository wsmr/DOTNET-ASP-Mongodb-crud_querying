using DiyawannaSupBackend.Api.Models.DTOs;
using DiyawannaSupBackend.Api.Models.Entities;

namespace DiyawannaSupBackend.Api.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<User> RegisterAsync(RegisterRequest request);
        // Task<LoginResponse> RefreshTokenAsync(string token);
    }
}
