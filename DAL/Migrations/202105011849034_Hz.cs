namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hz : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.InsuranceOrders", newName: "OrderInsurances");
            DropPrimaryKey("dbo.OrderInsurances");
            AddPrimaryKey("dbo.OrderInsurances", new[] { "Order_Id", "Insurance_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrderInsurances");
            AddPrimaryKey("dbo.OrderInsurances", new[] { "Insurance_Id", "Order_Id" });
            RenameTable(name: "dbo.OrderInsurances", newName: "InsuranceOrders");
        }
    }
}
