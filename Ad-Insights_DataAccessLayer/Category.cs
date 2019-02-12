using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Ad_Insights_DataAccessLayer
{
    public class Category
    {
        [Key]
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }


        public ICollection<ProductDetail> GetProductDetails { get; set; }
    }
}
