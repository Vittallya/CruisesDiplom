namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cabins : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cabins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deck = c.Int(nullable: false),
                        LaynerId = c.Int(nullable: false),
                        CabinType = c.Int(nullable: false),
                        AdultCount = c.Int(nullable: false),
                        ChildCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Layners", t => t.LaynerId, cascadeDelete: true)
                .Index(t => t.LaynerId);
            
            CreateTable(
                "dbo.Placements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        IsClient = c.Boolean(nullable: false),
                        Fio = c.String(),
                        Pasport = c.String(),
                        CabinId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cabins", t => t.CabinId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.CabinId);
            
            AddColumn("dbo.Clients", "Pasport", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Placements", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Placements", "CabinId", "dbo.Cabins");
            DropForeignKey("dbo.Cabins", "LaynerId", "dbo.Layners");
            DropIndex("dbo.Placements", new[] { "CabinId" });
            DropIndex("dbo.Placements", new[] { "OrderId" });
            DropIndex("dbo.Cabins", new[] { "LaynerId" });
            DropColumn("dbo.Clients", "Pasport");
            DropTable("dbo.Placements");
            DropTable("dbo.Cabins");
        }
    }
}
