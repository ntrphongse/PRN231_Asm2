using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        User GetDefaultUser();
        Task<User> GetUserAsync(int userId);
        Task<User> AddUserAsync(User newUser);
        Task<User> UpdateUserAsync(User updatedUser);
        Task DeleteUserAsync(int userId);
        Task<User> LoginAsync(string email, string password);
    }
}
