using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> AddUserAsync(User newUser)
            => await UserDAO.Instance.AddUserAsync(newUser);

        public async Task DeleteUserAsync(int userId)
            => await UserDAO.Instance.DeleteUserAsync(userId);

        public async Task<User> GetUserAsync(int userId)
            => await UserDAO.Instance.GetUserAsync(userId);

        public async Task<IEnumerable<User>> GetUsersAsync()
            => await UserDAO.Instance.GetUsersAsync();

        public async Task<User> UpdateUserAsync(User updatedUser)
            => await UserDAO.Instance.UpdateUserAsync(updatedUser);

        public async Task<User> LoginAsync(string email, string password)
            => await UserDAO.Instance.LoginAsync(email, password);

        public User GetDefaultUser()
            => UserDAO.Instance.GetDefaultUser();
    }
}
