namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant43 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translation_Rules",
                c => new
                    {
                        RuleId = c.Int(nullable: false, identity: true),
                        EnglishRules = c.String(),
                        AnharicRules = c.String(),
                    })
                .PrimaryKey(t => t.RuleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Translation_Rules");
        }
    }
}
