namespace TaskManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProgress : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Progress", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Progress", c => c.Byte(nullable: false));
        }
    }
}
