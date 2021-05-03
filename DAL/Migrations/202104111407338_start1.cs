namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class start1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "ImageName");
        }
    }
}
