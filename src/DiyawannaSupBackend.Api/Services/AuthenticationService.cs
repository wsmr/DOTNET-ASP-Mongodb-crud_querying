using DiyawannaSupBackend.Api.Exceptions;
using DiyawannaSupBackend.Api.Models.DTOs;
using DiyawannaSupBackend.Api.Models.Entities;
using DiyawannaSupBackend.Api.Repositories;
using DiyawannaSupBackend.Api.Security;
using BCrypt.Net; // Install BCrypt.Net-Next NuGet package

namespace DiyawannaSupBackend.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new AuthenticationException("Invalid username or password.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse
            {
                Success = true,
                Token = token,
                Username = user.Username,
                ExpiresIn = new JwtSettings().ExpirationMinutes * 60 * 1000, // Convert minutes to milliseconds
                Message = "Login successful"
            };
        }

        public async Task<User> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException($"User with username \'{request.Username}\' already exists.");
            }

            var newUser = new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Age = request.Age,
                University = request.University,
                School = request.School,
                Work = request.Work,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(newUser);
            return newUser;
        }
    }
}
