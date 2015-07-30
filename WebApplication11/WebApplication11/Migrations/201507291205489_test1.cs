namespace WebApplication11.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        client = c.String(nullable: false, maxLength: 128),
                        jobNumber = c.String(),
                        jobName = c.String(),
                        due = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.client);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Jobs");
        }
    }
}
