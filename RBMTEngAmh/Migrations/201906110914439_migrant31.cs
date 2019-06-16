namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant31 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Target_Language", "id", "dbo.Source_Language");
            DropIndex("dbo.Target_Language", new[] { "id" });
            DropPrimaryKey("dbo.Target_Language");
            AddColumn("dbo.Target_Language", "WordPOSType", c => c.Int(nullable: false));
            AddColumn("dbo.Target_Language", "WordType", c => c.Int(nullable: false));
            AddColumn("dbo.Target_Language", "RootWord", c => c.String(nullable: false));
            AddColumn("dbo.Target_Language", "IrregularPluralNoun", c => c.String());
            AddColumn("dbo.Target_Language", "IrregularPastVerb", c => c.String());
            AddColumn("dbo.Target_Language", "IrregularPPVerb", c => c.String());
            AlterColumn("dbo.Target_Language", "id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Target_Language", "id");
            DropColumn("dbo.Source_Language", "RegularNoun");
            DropColumn("dbo.Source_Language", "IrregularNoun");
            DropColumn("dbo.Source_Language", "RegularVerb");
            DropColumn("dbo.Source_Language", "IrregularVerb");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Source_Language", "IrregularVerb", c => c.String());
            AddColumn("dbo.Source_Language", "RegularVerb", c => c.String());
            AddColumn("dbo.Source_Language", "IrregularNoun", c => c.String());
            AddColumn("dbo.Source_Language", "RegularNoun", c => c.String());
            DropPrimaryKey("dbo.Target_Language");
            AlterColumn("dbo.Target_Language", "id", c => c.Long(nullable: false));
            DropColumn("dbo.Target_Language", "IrregularPPVerb");
            DropColumn("dbo.Target_Language", "IrregularPastVerb");
            DropColumn("dbo.Target_Language", "IrregularPluralNoun");
            DropColumn("dbo.Target_Language", "RootWord");
            DropColumn("dbo.Target_Language", "WordType");
            DropColumn("dbo.Target_Language", "WordPOSType");
            AddPrimaryKey("dbo.Target_Language", "id");
            CreateIndex("dbo.Target_Language", "id");
            AddForeignKey("dbo.Target_Language", "id", "dbo.Source_Language", "id");
        }
    }
}
