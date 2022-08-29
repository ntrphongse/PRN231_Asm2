using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BookAuthor
    {
        [Key]
        [ForeignKey("Author")]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [Key]
        [ForeignKey("Book")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        //[Required]
        [Display(Name = "Author Order")]
        public int AuthorOrder { get; set; }

        [Required]
        [Display(Name = "Royality Percentage")]
        public int RoyalityPercentage { get; set; }

        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }
    }
}
