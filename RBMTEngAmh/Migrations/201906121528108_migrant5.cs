namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Translation_Rules", "AmharicRules", c => c.String(nullable: false));
            DropColumn("dbo.Translation_Rules", "AnharicRules");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Translation_Rules", "AnharicRules", c => c.String(nullable: false));
            DropColumn("dbo.Translation_Rules", "AmharicRules");
        }
    }
}
