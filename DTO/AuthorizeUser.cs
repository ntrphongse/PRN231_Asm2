using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AuthorizeUser : User
    {
        public AuthorizeUser(User user)
        {
            EmailAddress = user.EmailAddress;
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public AuthorizeUser()
        {

        }
        public string AuthorizeRole { get; set; }
    }
}
