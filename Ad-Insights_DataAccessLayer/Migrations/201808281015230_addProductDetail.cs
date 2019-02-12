namespace Ad_Insights_DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductDetails", "ProductPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductDetails", "ProductPrice", c => c.Double(nullable: false));
        }
    }
}
