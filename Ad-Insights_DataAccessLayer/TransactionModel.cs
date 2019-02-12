using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad_Insights_DataAccessLayer
{
    public class TransactionModel
    {
        public string ProductName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Quantity { get; set; }
        public DateTime Transaction { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public int TransactionID { get; set; }
        public Transactions transactions { get; set; }
    }
}
