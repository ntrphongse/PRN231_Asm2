using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthor>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Book Title is required!!")]
        [MinLength(2, ErrorMessage = "Book Title must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Book Title is limited to 255 characters!!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Book Type is required!!")]
        [MinLength(2, ErrorMessage = "Book Type must have at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Book Type is limited to 50 characters!!")]
        public string Type { get; set; }

        [ForeignKey("Publisher")]
        [Required(ErrorMessage = "Book Publisher is required!!")]
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Book Price is required!!!")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Book Price must be a positive number!!")]
        public decimal Price { get; set; }

        [MinLength(2, ErrorMessage = "Book Advance must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Book Advance is limited to 50 characters!!")]
        public string Advance { get; set; }

        [MinLength(2, ErrorMessage = "Book Royalty must have at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Book Royalty is limited to 50 characters!!")]
        public string Royalty { get; set; }

        [Required(ErrorMessage = "Year to date Sales is required!!")]
        [Range(0, int.MaxValue, ErrorMessage = "Year to date Sales has to be a positive integer!")]
        [Display(Name = "Year to Date Sales")]
        public int YtdSales { get; set; }

        [MinLength(2, ErrorMessage = "Book Notes must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "Book Notes is limited to 255 characters!!")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Book Published Date is required!!")]
        [Display(Name = "Published Date")]
        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }

        public Publisher Publisher { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
