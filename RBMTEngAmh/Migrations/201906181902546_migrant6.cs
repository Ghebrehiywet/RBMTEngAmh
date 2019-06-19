namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant6 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.NOUNS");
            DropTable("dbo.VERBS");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VERBS",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Regular_stem = c.String(),
                        Irregular_stem = c.String(),
                        Irregular_past = c.String(),
                        Irregular_PP = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.NOUNS",
                c => new
                    {
                        id = c.Short(nullable: false, identity: true),
                        Regular = c.String(),
                        Irregular = c.String(),
                        IrregularPlural = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
    }
}
