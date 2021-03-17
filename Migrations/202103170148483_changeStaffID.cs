namespace MachineryProcessingDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeStaffID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo._InfomationPushProcessing", "StaffID", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo._InfomationPushProcessing", "StaffID", c => c.Int());
        }
    }
}
