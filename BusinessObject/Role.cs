using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role description is required!!")]
        [MinLength(2, ErrorMessage = "Role description must have at least 2 characters!!")]
        [MaxLength(50, ErrorMessage = "Role description is limited to 50 characters!!")]
        [Display(Name = "Role Description")]
        public string RoleDesc { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
