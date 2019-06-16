namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant44 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Translation_Rules", "EnglishRules", c => c.String(nullable: false));
            AlterColumn("dbo.Translation_Rules", "AnharicRules", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Translation_Rules", "AnharicRules", c => c.String());
            AlterColumn("dbo.Translation_Rules", "EnglishRules", c => c.String());
        }
    }
}
