namespace TaskManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Todoes", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Todoes", new[] { "ProjectId" });
            RenameColumn(table: "dbo.Projects", name: "UserId", newName: "FromUserId");
            AddColumn("dbo.Projects", "Status", c => c.String());
            AddColumn("dbo.Projects", "Progress", c => c.Byte(nullable: false));
            AddColumn("dbo.Projects", "Deadline", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "ToUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Projects", "FromUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Projects", "FromUserId");
            CreateIndex("dbo.Projects", "ToUserId");
            AddForeignKey("dbo.Projects", "ToUserId", "dbo.AspNetUsers", "Id");
            DropTable("dbo.Todoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Todoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Projects", "ToUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ToUserId" });
            DropIndex("dbo.Projects", new[] { "FromUserId" });
            AlterColumn("dbo.Projects", "FromUserId", c => c.String(maxLength: 128));
            DropColumn("dbo.Projects", "ToUserId");
            DropColumn("dbo.Projects", "Deadline");
            DropColumn("dbo.Projects", "Progress");
            DropColumn("dbo.Projects", "Status");
            RenameColumn(table: "dbo.Projects", name: "FromUserId", newName: "UserId");
            CreateIndex("dbo.Todoes", "ProjectId");
            CreateIndex("dbo.Projects", "UserId");
            AddForeignKey("dbo.Todoes", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
