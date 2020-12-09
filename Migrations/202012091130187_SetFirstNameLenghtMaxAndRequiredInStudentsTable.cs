namespace Diary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetFirstNameLenghtMaxAndRequiredInStudentsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "FirstName", c => c.String(nullable: false, maxLength: 100));

            // Sql("Insert .... ");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "FirstName", c => c.String());
        }
    }
}
