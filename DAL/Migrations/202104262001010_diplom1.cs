namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diplom1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Layners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descr = c.String(),
                        ImageName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tours", "LaynerId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "DaysCount", c => c.Int(nullable: false));
            CreateIndex("dbo.Tours", "LaynerId");
            AddForeignKey("dbo.Tours", "LaynerId", "dbo.Layners", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tours", "LaynerId", "dbo.Layners");
            DropIndex("dbo.Tours", new[] { "LaynerId" });
            DropColumn("dbo.Tours", "DaysCount");
            DropColumn("dbo.Tours", "LaynerId");
            DropTable("dbo.Layners");
        }
    }
}
