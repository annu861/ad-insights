namespace Ad_Insights_DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addViewModel8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Quantity = c.Int(nullable: false),
                        Transaction = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        TransactionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transactions", t => t.TransactionID, cascadeDelete: true)
                .Index(t => t.TransactionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViewModels", "TransactionID", "dbo.Transactions");
            DropIndex("dbo.ViewModels", new[] { "TransactionID" });
            DropTable("dbo.ViewModels");
        }
    }
}
