using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ad_Insights_DataAccessLayer
{
    public class Transactions
    {
        [Key]
        [Required]
        public int TransactionID { get; set; }

        [Required]
        public int ProductID { get; set; }
        
        public ProductDetail ProductDetail { get; set; }

        [Required]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required]
        [RegularExpression("^[1-9]*$", ErrorMessage = "Quantity must be positive numeric")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]
        public DateTime TransactionDate { get; set; }

     public List<Transactions> GetTransaction { get; set; }
    }
}
