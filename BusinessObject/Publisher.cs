using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
            Users = new HashSet<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Publisher Name is required!!")]
        [Display(Name = "Publisher Name")]
        [MinLength(2, ErrorMessage = "Publisher Name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Publisher Name is limited to 255 characters!!")]
        public string PublisherName { get; set; }

        [Required(ErrorMessage = "Publisher City is required!!")]
        [MinLength(2, ErrorMessage = "Publisher City must have at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Publisher City is limited to 100 characters!!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Publisher State is required!!")]
        [MinLength(2, ErrorMessage = "Publisher State must have at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Publisher State is limited to 100 characters!!")]
        public string State { get; set; }

        [Required(ErrorMessage = "Publisher Country is required!!")]
        [MinLength(2, ErrorMessage = "Publisher Country must have at least 2 characters!!")]
        [MaxLength(100, ErrorMessage = "Publisher Country is limited to 100 characters!!")]
        public string Country { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
