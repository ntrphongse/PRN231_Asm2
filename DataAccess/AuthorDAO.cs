using BusinessObject;
using eBookStoreLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AuthorDAO
    {
        private static AuthorDAO instance = null;
        private static readonly object instanceLock = new object();

        private AuthorDAO()
        {

        }

        public static AuthorDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AuthorDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            var db = new eStoreContext();
            return await db.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int authorId)
        {
            var db = new eStoreContext();
            return await db.Authors.FindAsync(authorId);
        }

        public async Task<Author> AddAuthorAsync(Author newAuthor)
        {
            CheckAuthor(newAuthor);
            var db = new eStoreContext();
            await db.Authors.AddAsync(newAuthor);
            await db.SaveChangesAsync();

            return newAuthor;
        }

        public async Task<Author> UpdateAuthorAsync(Author updatedAuthor)
        {
            if (await GetAuthorAsync(updatedAuthor.AuthorId) == null)
            {
                throw new ApplicationException("Author does not exist!!");
            }
            CheckAuthor(updatedAuthor);
            var db = new eStoreContext();
            db.Authors.Update(updatedAuthor);
            await db.SaveChangesAsync();
            return updatedAuthor;
        }

        public async Task DeleteAuthorAsync(int authorId)
        {
            Author deletedAuthor = await GetAuthorAsync(authorId);
            if (deletedAuthor == null)
            {
                throw new ApplicationException("Author does not exist!!");
            }
            var db = new eStoreContext();
            db.Authors.Remove(deletedAuthor);
            await db.SaveChangesAsync();
        }

        private void CheckAuthor(Author author)
        {
            author.LastName.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "Author Last name is required!!",
                minLength: 2,
                minLengthErrorMessage: "Author Last name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Author Last name is limited to 255 characters!!"
                );

            author.FirstName.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Author First name must have at least 2 characters!!",
                maxLength: 255,
                maxLengthErrorMessage: "Author First name is limited to 255 characters!!"
                );

            if (!string.IsNullOrEmpty(author.Phone))
            {
                author.Phone.IsPhoneNumber("Phone number is invalid!!");
            }

            author.Address.StringValidate(
                allowEmpty: true,
                emptyErrorMessage: "",
                minLength: 2,
                minLengthErrorMessage: "Author Address must have at least 2 characters!!",
                maxLength: 500,
                maxLengthErrorMessage: "Author Address is limited to 500 characters!!"
                );

            author.EmailAddress.IsEmail("Email is invalid!!");
        }
    }
}
