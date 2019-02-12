namespace Ad_Insights_DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductDetail1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductDetails", "ProductDetail_ProductID", c => c.Int());
            CreateIndex("dbo.ProductDetails", "ProductDetail_ProductID");
            AddForeignKey("dbo.ProductDetails", "ProductDetail_ProductID", "dbo.ProductDetails", "ProductID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductDetails", "ProductDetail_ProductID", "dbo.ProductDetails");
            DropIndex("dbo.ProductDetails", new[] { "ProductDetail_ProductID" });
            DropColumn("dbo.ProductDetails", "ProductDetail_ProductID");
        }
    }
}
