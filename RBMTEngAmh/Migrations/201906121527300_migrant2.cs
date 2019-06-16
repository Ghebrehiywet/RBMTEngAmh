namespace RBMTEngAmh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrant2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Source_Language", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Target_Language", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Target_Language", "Gender");
            DropColumn("dbo.Source_Language", "Gender");
        }
    }
}
