using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ad_Insights.ViewModel
{
    public class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
    }
}