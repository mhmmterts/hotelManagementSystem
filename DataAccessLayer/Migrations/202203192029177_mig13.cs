namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                {
                    CustomerID = c.Int(nullable: false, identity: true),
                    CustomerName = c.String(maxLength: 50),
                    Mail = c.String(),
                    Password = c.String(maxLength: 50),
                    PhoneNumber = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.CustomerID);

        }

        public override void Down()
        {
            DropTable("dbo.Customers");
        }
    }

}
