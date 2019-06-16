namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VERBS",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        reg_stem = c.String(),
                        irreg_stem = c.String(),
                        irreg_past = c.String(),
                        irret_PP = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VERBS");
        }
    }
}
