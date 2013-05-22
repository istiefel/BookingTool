namespace BookingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false, maxLength: 250),
                        DateBookedUtc = c.DateTime(nullable: false),
                        DateCreatedUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartialBookings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DatePaidUtc = c.DateTime(),
                        BookingId = c.Int(nullable: false),
                        Sender = c.String(nullable: false),
                        Recipient = c.String(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PartialBookings", new[] { "BookingId" });
            DropForeignKey("dbo.PartialBookings", "BookingId", "dbo.Bookings");
            DropTable("dbo.Products");
            DropTable("dbo.PartialBookings");
            DropTable("dbo.Bookings");
        }
    }
}
