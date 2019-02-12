using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Ad_Insights_DataAccessLayer
{
    public class Ad_insightContext : DbContext
    {
        public Ad_insightContext() : base ("AdInsightConnectionString")
        {
           

        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}

        public DbSet<User> Users { get; set; }

        public int CategoryID { get; set; }
        public DbSet<Category> Categories { get; set; }

        public int ProductID { get; set; }
        public DbSet<ProductDetail> ProductDetail { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        //public System.Data.Entity.DbSet<Ad_Insights_DataAccessLayer.UserView> UserViews { get; set; }
    }
}
