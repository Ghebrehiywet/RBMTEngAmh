namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant41 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Target_Language");
            AddColumn("dbo.Target_Language", "TargetLangId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Target_Language", "TargetLangId");
            DropColumn("dbo.Target_Language", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Target_Language", "id", c => c.Long(nullable: false, identity: true));
            DropPrimaryKey("dbo.Target_Language");
            DropColumn("dbo.Target_Language", "TargetLangId");
            AddPrimaryKey("dbo.Target_Language", "id");
        }
    }
}
