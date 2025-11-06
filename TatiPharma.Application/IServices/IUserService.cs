using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;

namespace TatiPharma.Application.IServices
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDto>>> GetAllUsersAsync();
    }
}
