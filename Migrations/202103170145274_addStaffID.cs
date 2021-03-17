namespace MachineryProcessingDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStaffID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo._InfomationPushProcessing", "StaffID", c => c.Int());
            AlterColumn("dbo._StaffBaseInformation", "Reserve1", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo._StaffBaseInformation", "Reserve1", c => c.String(maxLength: 50));
            DropColumn("dbo._InfomationPushProcessing", "StaffID");
            DropColumn("dbo.APS_ProcedureTask", "ProcedureType");
            DropTable("dbo.KitProcessingDocument");
        }
    }
}
