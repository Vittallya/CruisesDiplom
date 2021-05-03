namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class start2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Tour_Id", "dbo.Tours");
            DropIndex("dbo.Orders", new[] { "Tour_Id" });
            RenameColumn(table: "dbo.Orders", name: "Tour_Id", newName: "TourId");
            AlterColumn("dbo.Orders", "TourId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "TourId");
            AddForeignKey("dbo.Orders", "TourId", "dbo.Tours", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "TourId", "dbo.Tours");
            DropIndex("dbo.Orders", new[] { "TourId" });
            AlterColumn("dbo.Orders", "TourId", c => c.Int());
            RenameColumn(table: "dbo.Orders", name: "TourId", newName: "Tour_Id");
            CreateIndex("dbo.Orders", "Tour_Id");
            AddForeignKey("dbo.Orders", "Tour_Id", "dbo.Tours", "Id");
        }
    }
}
