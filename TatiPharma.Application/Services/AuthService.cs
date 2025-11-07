using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;
using TatiPharma.Application.IRepositories;

namespace TatiPharma.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, JwtService jwtService, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.UserName);

            if (user == null || user.Password != request.Password)
            {
                return ApiResponse<LoginResponseDto>.ErrorResult(
                    new List<string> { "Invalid username or password" });
            }

            var roleName = user.Role?.RollName ?? "User";
            var token = _jwtService.GenerateToken(user, roleName);

            var response = new LoginResponseDto
            {
                UserId = user.BintUserId,
                UserName = user.UserName ?? string.Empty,
                RoleName = roleName,
                Token = token
            };

            return ApiResponse<LoginResponseDto>.SuccessResult(response, "Login successful");
        }
    }
}
