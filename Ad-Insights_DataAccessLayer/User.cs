using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Ad_Insights_DataAccessLayer
{
    public class User
    {

        [Key]
        [Required]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Enter a valid EmailId")]
        [EmailAddress]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string EmailID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter a valid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Status { get; set; }

        public string Role { get; set; }


        public ICollection<Transactions> GetTransactions { get; set; }
    }
}
