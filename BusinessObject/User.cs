using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email Address is required!!")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email is invalid!!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "User Password is required!!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "User Password must have at least 6 characters!!")]
        [MaxLength(30, ErrorMessage = "User Password is limited to 30 characters!!")]
        public string Password { get; set; }

        [MinLength(2, ErrorMessage = "User Source must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "User Source is limited to 255 characters!!")]
        public string Source { get; set; }

        [Required(ErrorMessage = "User First name is required!!")]
        [Display(Name = "First Name")]
        [MinLength(2, ErrorMessage = "User First name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "User First name is limited to 255 characters!!")]
        public string FirstName { get; set; }
        
        [Display(Name = "Middle Name")]
        [MinLength(2, ErrorMessage = "User Middle name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "User Middle name is limited to 255 characters!!")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "User Last name is required!!")]
        [Display(Name = "Last Name")]
        [MinLength(2, ErrorMessage = "User Last name must have at least 2 characters!!")]
        [MaxLength(255, ErrorMessage = "User Last name is limited to 255 characters!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Hire Date is required!!")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [ForeignKey("Role")]
        [Display(Name = "Role")]
        [Required(ErrorMessage = "User Role is required!!")]
        public int RoleId { get; set; }

        [ForeignKey("Publisher")]
        [Display(Name = "Publisher")]
        [Required(ErrorMessage = "User Publisher is required!!")]
        public int PublisherId { get; set; }

        public Publisher Publisher { get; set; }
        public Role Role { get; set; }
    }
}
