using BusinessObject;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eBookStoreWebAPI
{
    public static class DbInitializer
    {
        public static void Initialize(eStoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            IEnumerable<User> users = new List<User>()
            {
                new User()
                {
                    EmailAddress = "test@estore.com",
                    Password = "123456",
                    FirstName = "Test",
                    LastName = "Test"
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
