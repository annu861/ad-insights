using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ad_Insights_DataAccessLayer
{
    public class ProductDetail
    {
        [Key]
        [Required]
        public int ProductID { get; set; }
       
        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public List<ProductDetail> GetProductDetails { get; set; }
        public Category ProductCategory { get; set; }
        public ICollection<Transactions> GetTransactions { get; set; }
    }
}
