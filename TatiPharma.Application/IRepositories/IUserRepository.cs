using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Application.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByUsernameAsync(string username);
    }
}
