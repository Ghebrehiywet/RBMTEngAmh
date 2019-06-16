namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Source_Language",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        WordPOSType = c.Int(nullable: false),
                        WordType = c.Int(nullable: false),
                        RootWord = c.String(nullable: false),
                        RegularNoun = c.String(),
                        IrregularNoun = c.String(),
                        IrregularPluralNoun = c.String(),
                        RegularVerb = c.String(),
                        IrregularVerb = c.String(),
                        IrregularPastVerb = c.String(),
                        IrregularPPVerb = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Target_Language",
                c => new
                    {
                        id = c.Long(nullable: false),
                        sourceId = c.Long(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Source_Language", t => t.id)
                .ForeignKey("dbo.Source_Language", t => t.sourceId)
                .Index(t => t.id)
                .Index(t => t.sourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Target_Language", "sourceId", "dbo.Source_Language");
            DropForeignKey("dbo.Target_Language", "id", "dbo.Source_Language");
            DropIndex("dbo.Target_Language", new[] { "sourceId" });
            DropIndex("dbo.Target_Language", new[] { "id" });
            DropTable("dbo.Target_Language");
            DropTable("dbo.Source_Language");
        }
    }
}
