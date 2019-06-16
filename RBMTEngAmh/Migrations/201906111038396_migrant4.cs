namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Target_Language", "sourceId", "dbo.Source_Language");
            DropIndex("dbo.Target_Language", new[] { "sourceId" });
            AlterColumn("dbo.Target_Language", "sourceId", c => c.Long(nullable: false));
            CreateIndex("dbo.Target_Language", "sourceId");
            AddForeignKey("dbo.Target_Language", "sourceId", "dbo.Source_Language", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Target_Language", "sourceId", "dbo.Source_Language");
            DropIndex("dbo.Target_Language", new[] { "sourceId" });
            AlterColumn("dbo.Target_Language", "sourceId", c => c.Long());
            CreateIndex("dbo.Target_Language", "sourceId");
            AddForeignKey("dbo.Target_Language", "sourceId", "dbo.Source_Language", "id");
        }
    }
}
