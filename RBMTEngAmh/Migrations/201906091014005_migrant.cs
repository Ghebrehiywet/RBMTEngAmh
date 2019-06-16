namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant : DbMigration
    {
        public override void Up()
        {
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
        
        public override void Down()
        {
            DropTable("dbo.NOUNS");
        }
    }
}
