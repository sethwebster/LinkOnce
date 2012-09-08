namespace LinkOnce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        Destination = c.String(nullable: false),
                        ShortUrl = c.String(nullable: false),
                        OptionalEmailAddress = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUsed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LinkId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Links");
        }
    }
}
