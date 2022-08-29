using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Author
    {
        public Author()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Author Last name is required!!")]
        [Display(Name = "Last name")]
        [MinLength(2, ErrorMessage = "Author Last name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Author Last name is limited to 255 characters!!")]
        public string LastName { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Author First name is required!!")]
        [MinLength(2, ErrorMessage = "Author First name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Author First name is limited to 255 characters!!")]
        public string FirstName { get; set; }

        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Phone number is invalid!!")]
        public string Phone { get; set; }

        [MinLength(2, ErrorMessage = "Author Address must have at least 2 characters!!")]
        [MaxLength(500, ErrorMessage = "Author Address is limited to 500 characters!!")]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required!!")]
        [EmailAddress(ErrorMessage = "Email is invalid!!")]
        public string EmailAddress { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
