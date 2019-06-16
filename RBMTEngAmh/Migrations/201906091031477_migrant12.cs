namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VERBS", "Regular_stem", c => c.String());
            AddColumn("dbo.VERBS", "Irregular_stem", c => c.String());
            AddColumn("dbo.VERBS", "Irregular_past", c => c.String());
            AddColumn("dbo.VERBS", "Irregular_PP", c => c.String());
            DropColumn("dbo.VERBS", "reg_stem");
            DropColumn("dbo.VERBS", "irreg_stem");
            DropColumn("dbo.VERBS", "irreg_past");
            DropColumn("dbo.VERBS", "irret_PP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VERBS", "irret_PP", c => c.String());
            AddColumn("dbo.VERBS", "irreg_past", c => c.String());
            AddColumn("dbo.VERBS", "irreg_stem", c => c.String());
            AddColumn("dbo.VERBS", "reg_stem", c => c.String());
            DropColumn("dbo.VERBS", "Irregular_PP");
            DropColumn("dbo.VERBS", "Irregular_past");
            DropColumn("dbo.VERBS", "Irregular_stem");
            DropColumn("dbo.VERBS", "Regular_stem");
        }
    }
}
