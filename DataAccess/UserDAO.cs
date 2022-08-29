using BusinessObject;
using eBookStoreLibrary;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();

        private UserDAO()
        {

        }

        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var db = new eStoreContext();
            return await db.Users
                .Include(user => user.Publisher)
                .Include(user => user.Role)
                .ToListAsync();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var db = new eStoreContext();
            return await db.Users
                .Include(user => user.Publisher)
                .Include(user => user.Role)
                .SingleOrDefaultAsync(user => user.UserId == userId);
        }

        public async Task<User> GetUserAsync(string email)
        {
            var db = new eStoreContext();
            return await db.Users
                .Include(user => user.Publisher)
                .Include(user => user.Role)
                .SingleOrDefaultAsync(user => user.EmailAddress.Equals(email.ToLower()));
        }

        public User GetDefaultUser()
        {
            return JsonConvert.DeserializeObject<User>(eBookStoreApiConfiguration.DefaultAccount);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var db = new eStoreContext();
            IEnumerable<User> users = await db.Users.ToListAsync();
            users = users.Prepend(GetDefaultUser());
            if (email == null)
            {
                email = string.Empty;
            }
            if (password == null)
            {
                password = string.Empty;
            }
            return users.SingleOrDefault(user => user.EmailAddress.ToLower().Equals(email.ToLower())
                                && user.Password.Equals(password));
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            await CheckUserAsync(newUser);
            if (await GetUserAsync(newUser.EmailAddress) != null)
            {
                throw new ApplicationException("Email Address is existed!!");
            }
            var db = new eStoreContext();
            await db.Users.AddAsync(newUser);
            await db.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            if (await GetUserAsync(updatedUser.UserId) == null)
            {
                throw new ApplicationException("User does not exist!!");
            }
            await CheckUserAsync(updatedUser);
            var db = new eStoreContext();
            db.Users.Update(updatedUser);
            await db.SaveChangesAsync();
            return updatedUser;
        }

        public async Task DeleteUserAsync(int userId)
        {
            User deletedUser = await GetUserAsync(userId);
            if (deletedUser == null)
            {
                throw new ApplicationException("User does not exist!!");
            }
            var db = new eStoreContext();
            db.Users.Remove(deletedUser);
            await db.SaveChangesAsync();
        }

        private async Task CheckUserAsync(User user)
        {

            user.EmailAddress.IsEmail("Email is invalid!!");

            user.Password.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "User Password is required!!",
                minLength: 6,
                minLengthErrorMessage: "User Password must have at least 6 characters!!",
                maxLength: 30,
                maxLengthErrorMessage: "User Password is limited to 30 characters!!"
                );

            user.LastName.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "User Last name is required!!",
                minLength: 2,
                minLengthErrorMessage: "User Last name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "User Last name is limited to 255 characters!!"
                );

            user.FirstName.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "User First name is required!!",
                minLength: 2,
                minLengthErrorMessage: "User First name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "User First name is limited to 255 characters!!"
                );

            user.MiddleName.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "User Middle name is required!!",
                minLength: 2,
                minLengthErrorMessage: "User Middle name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "User Middle name is limited to 255 characters!!"
                );

            user.Source.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "User Source must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "User Source is limited to 500 characters!!"
                );

            if (user.HireDate > DateTime.Now)
            {
                throw new ApplicationException("User Hire date must not be after today!!");
            }

            var db = new eStoreContext();
            if (await db.Roles.FindAsync(user.RoleId) == null)
            {
                throw new ApplicationException("User Role does not exist!!");
            }
            if (await db.Publishers.FindAsync(user.PublisherId) == null)
            {
                throw new ApplicationException("User Publisher does not exist!!");
            }
        }
    }
}
