namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ins : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Insurances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InsuranceOrders",
                c => new
                    {
                        Insurance_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Insurance_Id, t.Order_Id })
                .ForeignKey("dbo.Insurances", t => t.Insurance_Id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .Index(t => t.Insurance_Id)
                .Index(t => t.Order_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InsuranceOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.InsuranceOrders", "Insurance_Id", "dbo.Insurances");
            DropIndex("dbo.InsuranceOrders", new[] { "Order_Id" });
            DropIndex("dbo.InsuranceOrders", new[] { "Insurance_Id" });
            DropTable("dbo.InsuranceOrders");
            DropTable("dbo.Insurances");
        }
    }
}
