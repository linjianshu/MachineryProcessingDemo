namespace MachineryProcessingDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.A_CutterDemand",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProjectID = c.String(maxLength: 50),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        CutterType = c.String(maxLength: 50),
                        CutterName = c.String(maxLength: 50),
                        Specification = c.String(maxLength: 100),
                        DemandNum = c.Int(),
                        Cstate = c.Int(),
                        ProLeadTime = c.DateTime(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_KitBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ApplianceType = c.Int(),
                        KitCode = c.String(maxLength: 50),
                        KitName = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_MaterialProgramDemand",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        Type = c.Int(),
                        DemandNum = c.Int(),
                        Cstate = c.Int(),
                        ProLeadTime = c.DateTime(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_OrderBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderCode = c.String(maxLength: 50),
                        OrderName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 50),
                        CustomerID = c.Long(nullable: false),
                        CustomerCode = c.String(maxLength: 50),
                        CustomerName = c.String(maxLength: 50),
                        State = c.Int(),
                        OrderType = c.Int(),
                        Priority = c.Int(),
                        TechnicalDirector = c.String(maxLength: 50),
                        ManufacturingDirector = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_PlanProductInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        PlanBaseID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductBornCode = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        Type = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 6),
                        Unit = c.String(maxLength: 10),
                        DeadLine = c.DateTime(),
                        SubPriority = c.Int(),
                        State = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcedureBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        GroupCode = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureIndex = c.Int(),
                        StandardTaskSpan = c.String(maxLength: 50),
                        IsProgramDemand = c.Boolean(),
                        IsMaterialDemand = c.Boolean(),
                        TimeUnit = c.Int(),
                        IsNeedCheck = c.Boolean(),
                        CheckFrequency = c.Int(),
                        CheckTime = c.Decimal(precision: 18, scale: 6),
                        MaxNgNumber = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcedureCutterConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureIndex = c.Int(),
                        CutterType = c.String(maxLength: 50),
                        CutterName = c.String(maxLength: 50),
                        Specification = c.String(maxLength: 100),
                        LeadTime = c.Decimal(precision: 18, scale: 6),
                        TimeUnit = c.Int(),
                        IsAvailable = c.Int(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcedureEquipmentConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureIndex = c.Int(),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentGroupName = c.String(maxLength: 50),
                        EquipmentGroupCode = c.String(maxLength: 50),
                        SpecialEquipmentCode = c.String(maxLength: 50),
                        SpecialEquipmentTag = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcedureSelfCheckingConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcedureID = c.Long(),
                        ItemCode = c.String(maxLength: 50),
                        ItemName = c.String(maxLength: 50),
                        UpperLimit = c.Decimal(precision: 18, scale: 6),
                        LowerLimit = c.Decimal(precision: 18, scale: 6),
                        StandardValue = c.Decimal(precision: 18, scale: 6),
                        Unit = c.String(maxLength: 50),
                        IsEnable = c.Boolean(),
                        IsRequired = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcedureStaffSkillConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureIndex = c.Int(),
                        SkillType = c.String(maxLength: 50),
                        SkillRank = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcessModelBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ModelName = c.String(maxLength: 50),
                        ModelCode = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProcessProcedureBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcessID = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        ProcedureType = c.Int(),
                        FormerProcedureType = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProductBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProductType = c.Int(),
                        ProductSpeCode = c.String(maxLength: 50),
                        Image = c.String(maxLength: 100),
                        StandardCost = c.Decimal(precision: 18, scale: 6),
                        PackingNum = c.Int(),
                        Description = c.String(maxLength: 100),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProductProcedureBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductModelID = c.Long(),
                        ModelID = c.Long(),
                        GroupID = c.Long(),
                        GroupCode = c.String(maxLength: 50),
                        GroupName = c.String(maxLength: 50),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureDescription = c.String(maxLength: 200),
                        ProcedureIndex = c.Int(),
                        IsProgramDemand = c.Boolean(),
                        IsMaterialDemand = c.Boolean(),
                        ProcedureType = c.Int(),
                        LastProcedureMode = c.Int(),
                        StandardTaskSpan = c.String(maxLength: 50),
                        TimeUnit = c.String(maxLength: 50),
                        IsNeedCheck = c.Boolean(),
                        CheckFrequency = c.Int(),
                        CheckTime = c.Decimal(precision: 18, scale: 6),
                        MaxNgNumber = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProjectInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        ProductID = c.Long(),
                        ProductName = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProjectName = c.String(maxLength: 50),
                        ProjectCode = c.String(maxLength: 50),
                        Type = c.Int(),
                        Num = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 6),
                        Unit = c.String(maxLength: 10),
                        DeadLine = c.DateTime(),
                        SubPriority = c.Int(),
                        State = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 1),
                        Reserve2 = c.String(maxLength: 1),
                        Reserve3 = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProjectPlanInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanCode = c.String(maxLength: 50),
                        Status = c.Int(),
                        Type = c.Int(),
                        Batch = c.Int(),
                        PlanNo = c.Int(),
                        HasFirstPiece = c.Int(),
                        DeadLine = c.DateTime(),
                        SubPriority = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_ProjectProcessModel",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ModelID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.A_WorkerTaskConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TaskTableID = c.Long(),
                        StaffID = c.Long(),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        IsFirstTask = c.Int(),
                        TaskIndex = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.APS_ProcedureTask",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Marks = c.Int(),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanCode = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        WorkerCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        IndentLevel = c.Int(),
                        SortOrder = c.Int(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ProgressPercent = c.Decimal(precision: 18, scale: 6),
                        PredecessorIndices = c.Int(),
                        Description = c.String(maxLength: 200),
                        Priority = c.Int(),
                        ProductNumber = c.Int(),
                        Batch = c.Int(),
                        Tag = c.String(maxLength: 50),
                        TaskState = c.Int(),
                        IsChecked = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.String(maxLength: 50),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.APS_ProcedureTaskDetail",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TaskTableID = c.Long(),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureIndex = c.Int(),
                        ProductBornCode = c.String(maxLength: 50),
                        TaskState = c.Int(),
                        IsChecked = c.Int(),
                        IsInspect = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.String(maxLength: 50),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        ProcedureType = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.APS_ResLineTime",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProcessID = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        Priority = c.Int(),
                        Tag = c.String(maxLength: 50),
                        AccessibleTime = c.DateTime(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Base_BackupJob",
                c => new
                    {
                        BackupId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ServerName = c.String(maxLength: 50, unicode: false),
                        DbName = c.String(maxLength: 50, unicode: false),
                        JobName = c.String(maxLength: 200, unicode: false),
                        Mode = c.String(maxLength: 50, unicode: false),
                        StartTime = c.String(maxLength: 50, unicode: false),
                        FilePath = c.String(maxLength: 200, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.String(maxLength: 20, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.BackupId);
            
            CreateTable(
                "dbo.Base_Button",
                c => new
                    {
                        ButtonId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Icon = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        JsEvent = c.String(maxLength: 50, unicode: false),
                        ActionEvent = c.String(maxLength: 200, unicode: false),
                        Split = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ButtonId);
            
            CreateTable(
                "dbo.Base_ButtonPermission",
                c => new
                    {
                        ButtonPermissionId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ModuleButtonId = c.String(nullable: false, maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ButtonPermissionId);
            
            CreateTable(
                "dbo.Base_CodeRule",
                c => new
                    {
                        CodeRuleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.CodeRuleId);
            
            CreateTable(
                "dbo.Base_CodeRuleDetail",
                c => new
                    {
                        CodeRuleDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CodeRuleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        SortCode = c.Int(nullable: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Consted = c.String(maxLength: 50, unicode: false),
                        AutoReset = c.Int(),
                        FixLength = c.Int(),
                        FormatStr = c.String(maxLength: 50, unicode: false),
                        StepValue = c.Int(),
                        InitValue = c.Int(),
                        FLength = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        FType = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodeRuleDetailId, t.CodeRuleId, t.SortCode });
            
            CreateTable(
                "dbo.Base_CodeRuleSerious",
                c => new
                    {
                        CodeSeriousId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CodeRuleId = c.String(maxLength: 50, unicode: false),
                        UserId = c.String(maxLength: 50, unicode: false),
                        ValueType = c.Int(),
                        NowValue = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        LastUpdateDate = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CodeSeriousId);
            
            CreateTable(
                "dbo.Base_Company",
                c => new
                    {
                        CompanyId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        ShortName = c.String(maxLength: 50, unicode: false),
                        Nature = c.String(maxLength: 50, unicode: false),
                        Manager = c.String(maxLength: 50, unicode: false),
                        Contact = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Fax = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        ProvinceId = c.String(maxLength: 50, unicode: false),
                        Province = c.String(maxLength: 50, unicode: false),
                        CityId = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        CountyId = c.String(maxLength: 50, unicode: false),
                        County = c.String(maxLength: 50, unicode: false),
                        Address = c.String(maxLength: 50, unicode: false),
                        AccountInfo = c.String(maxLength: 200, unicode: false),
                        Postalcode = c.String(maxLength: 200, unicode: false),
                        Web = c.String(maxLength: 200, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Base_DataDictionary",
                c => new
                    {
                        DataDictionaryId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        ParentId = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsTree = c.Int(),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DataDictionaryId);
            
            CreateTable(
                "dbo.Base_DataDictionaryDetail",
                c => new
                    {
                        DataDictionaryDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        DataDictionaryId = c.String(maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 200, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DataDictionaryDetailId);
            
            CreateTable(
                "dbo.Base_DataScopePermission",
                c => new
                    {
                        DataScopePermissionId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ResourceId = c.String(maxLength: 50, unicode: false),
                        Condition = c.String(unicode: false, storeType: "text"),
                        ConditionJson = c.String(unicode: false, storeType: "text"),
                        ScopeType = c.String(maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DataScopePermissionId);
            
            CreateTable(
                "dbo.Base_Department",
                c => new
                    {
                        DepartmentId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        ShortName = c.String(maxLength: 50, unicode: false),
                        Nature = c.String(maxLength: 50, unicode: false),
                        Manager = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Fax = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Base_Email",
                c => new
                    {
                        EmailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Theme = c.String(maxLength: 200, unicode: false),
                        ThemeColour = c.String(maxLength: 50, unicode: false),
                        Content = c.String(unicode: false, storeType: "text"),
                        Addresser = c.String(maxLength: 50, unicode: false),
                        SendDate = c.DateTime(),
                        IsAccessory = c.Int(),
                        Priority = c.Int(),
                        Receipt = c.Int(),
                        IsDelayed = c.Int(),
                        DelayedTime = c.DateTime(),
                        State = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.EmailId);
            
            CreateTable(
                "dbo.Base_EmailAccessory",
                c => new
                    {
                        EmailAccessoryId = c.String(nullable: false, maxLength: 50, unicode: false),
                        EmailId = c.String(maxLength: 50, unicode: false),
                        FileName = c.String(maxLength: 50, unicode: false),
                        FilePath = c.String(maxLength: 200, unicode: false),
                        FileSize = c.String(maxLength: 50, unicode: false),
                        FileType = c.String(maxLength: 50, unicode: false),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmailAccessoryId)
                .ForeignKey("dbo.Base_Email", t => t.EmailId, cascadeDelete: true)
                .Index(t => t.EmailId);
            
            CreateTable(
                "dbo.Base_EmailAddressee",
                c => new
                    {
                        EmailAddresseeId = c.String(nullable: false, maxLength: 50, unicode: false),
                        EmailId = c.String(maxLength: 50, unicode: false),
                        AddresseeId = c.String(maxLength: 50, unicode: false),
                        AddresseeName = c.String(maxLength: 50, unicode: false),
                        AddresseeIdState = c.Int(),
                        IsRead = c.Int(),
                        ReadCount = c.Int(),
                        ReadDate = c.DateTime(),
                        EndReadDate = c.DateTime(),
                        Highlight = c.Int(),
                        Backlog = c.Int(),
                        CreateDate = c.DateTime(),
                        DeleteMark = c.Int(),
                    })
                .PrimaryKey(t => t.EmailAddresseeId)
                .ForeignKey("dbo.Base_Email", t => t.EmailId, cascadeDelete: true)
                .Index(t => t.EmailId);
            
            CreateTable(
                "dbo.Base_EmailCategory",
                c => new
                    {
                        EmailCategoryId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.EmailCategoryId);
            
            CreateTable(
                "dbo.Base_Employee",
                c => new
                    {
                        EmployeeId = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserId = c.String(maxLength: 50, unicode: false),
                        Photograph = c.String(maxLength: 50, unicode: false),
                        IDCard = c.String(maxLength: 50, unicode: false),
                        Age = c.Int(),
                        BankCode = c.String(maxLength: 50, unicode: false),
                        OfficeCornet = c.String(maxLength: 50, unicode: false),
                        OfficePhone = c.String(maxLength: 50, unicode: false),
                        OfficeZipCode = c.String(maxLength: 50, unicode: false),
                        OfficeAddress = c.String(maxLength: 200, unicode: false),
                        OfficeFax = c.String(maxLength: 50, unicode: false),
                        Education = c.String(maxLength: 50, unicode: false),
                        School = c.String(maxLength: 50, unicode: false),
                        GraduationDate = c.DateTime(),
                        Major = c.String(maxLength: 50, unicode: false),
                        Degree = c.String(maxLength: 50, unicode: false),
                        WorkingDate = c.DateTime(),
                        HomeZipCode = c.String(maxLength: 50, unicode: false),
                        HomeAddress = c.String(maxLength: 200, unicode: false),
                        HomePhone = c.String(maxLength: 50, unicode: false),
                        HomeFax = c.String(maxLength: 50, unicode: false),
                        Province = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        Area = c.String(maxLength: 50, unicode: false),
                        NativePlace = c.String(maxLength: 50, unicode: false),
                        Party = c.String(maxLength: 50, unicode: false),
                        Nation = c.String(maxLength: 50, unicode: false),
                        Nationality = c.String(maxLength: 50, unicode: false),
                        Duty = c.String(maxLength: 50, unicode: false),
                        WorkingProperty = c.String(maxLength: 50, unicode: false),
                        Competency = c.String(maxLength: 50, unicode: false),
                        EmergencyContact = c.String(maxLength: 50, unicode: false),
                        IsDimission = c.Int(),
                        DimissionDate = c.DateTime(),
                        DimissionCause = c.String(maxLength: 200, unicode: false),
                        DimissionWhither = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Base_ExcelImport",
                c => new
                    {
                        ImportId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        ImportName = c.String(maxLength: 50, unicode: false),
                        ImportTable = c.String(maxLength: 50, unicode: false),
                        ImportTableName = c.String(maxLength: 50, unicode: false),
                        ImportFileName = c.String(maxLength: 200, unicode: false),
                        ErrorHanding = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ImportId);
            
            CreateTable(
                "dbo.Base_ExcelImportDetail",
                c => new
                    {
                        ImportDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ImportId = c.String(maxLength: 50, unicode: false),
                        ColumnName = c.String(maxLength: 50, unicode: false),
                        FieldName = c.String(maxLength: 50, unicode: false),
                        ForeignTable = c.String(maxLength: 200, unicode: false),
                        BackField = c.String(maxLength: 200, unicode: false),
                        CompareField = c.String(maxLength: 200, unicode: false),
                        AttachCondition = c.String(maxLength: 200, unicode: false),
                        DataType = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        FieldRemark = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ImportDetailId);
            
            CreateTable(
                "dbo.Base_FormAttribute",
                c => new
                    {
                        FormAttributeId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        PropertyName = c.String(maxLength: 50, unicode: false),
                        ControlId = c.String(maxLength: 50, unicode: false),
                        ControlType = c.String(maxLength: 50, unicode: false),
                        ControlStyle = c.String(maxLength: 50, unicode: false),
                        ControlValidator = c.String(maxLength: 50, unicode: false),
                        ImportLength = c.Int(),
                        DefaultVlaue = c.String(maxLength: 50, unicode: false),
                        AttributesProperty = c.String(unicode: false),
                        DataSourceType = c.Int(),
                        DataSource = c.String(unicode: false),
                        ControlColspan = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FormAttributeId);
            
            CreateTable(
                "dbo.Base_FormAttributeValue",
                c => new
                    {
                        AttributeValueId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ObjectParameterJson = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.AttributeValueId);
            
            CreateTable(
                "dbo.Base_GroupUser",
                c => new
                    {
                        GroupUserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        DepartmentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.GroupUserId);
            
            CreateTable(
                "dbo.Base_InterfaceManage",
                c => new
                    {
                        InterfaceId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Action = c.String(maxLength: 50, unicode: false),
                        Constraints = c.String(unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.InterfaceId);
            
            CreateTable(
                "dbo.Base_InterfaceManageParameter",
                c => new
                    {
                        InterfaceParameterId = c.String(nullable: false, maxLength: 50, unicode: false),
                        InterfaceId = c.String(maxLength: 50, unicode: false),
                        Field = c.String(maxLength: 50, unicode: false),
                        FieldMemo = c.String(maxLength: 200, unicode: false),
                        FieldType = c.String(maxLength: 50, unicode: false),
                        FieldMaxLength = c.Int(),
                        AllowNull = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                    })
                .PrimaryKey(t => t.InterfaceParameterId)
                .ForeignKey("dbo.Base_InterfaceManage", t => t.InterfaceId, cascadeDelete: true)
                .Index(t => t.InterfaceId);
            
            CreateTable(
                "dbo.Base_Language",
                c => new
                    {
                        LanguageId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        BusinessCode = c.String(maxLength: 50, unicode: false),
                        BusinessName = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(unicode: false, storeType: "text"),
                        FullValue = c.String(maxLength: 200, unicode: false),
                        Note = c.String(unicode: false),
                        LanguageType = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.Base_Module",
                c => new
                    {
                        ModuleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Icon = c.String(maxLength: 50, unicode: false),
                        Location = c.String(maxLength: 200, unicode: false),
                        Target = c.String(maxLength: 50, unicode: false),
                        Level = c.Int(),
                        Isexpand = c.Int(),
                        AllowButton = c.Int(),
                        AllowView = c.Int(),
                        AllowForm = c.Int(),
                        Authority = c.Int(),
                        DataScope = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ModuleId);
            
            CreateTable(
                "dbo.Base_ModulePermission",
                c => new
                    {
                        ModulePermissionId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ModulePermissionId);
            
            CreateTable(
                "dbo.Base_NetworkFile",
                c => new
                    {
                        NetworkFileId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FolderId = c.String(maxLength: 50, unicode: false),
                        FileName = c.String(maxLength: 200, unicode: false),
                        FilePath = c.String(maxLength: 200, unicode: false),
                        FileSize = c.String(maxLength: 50, unicode: false),
                        FileExtensions = c.String(maxLength: 50, unicode: false),
                        FileType = c.String(maxLength: 50, unicode: false),
                        Icon = c.String(maxLength: 50, unicode: false),
                        Sharing = c.Int(),
                        SharingFolderId = c.String(maxLength: 50, unicode: false),
                        SharingCreateDate = c.DateTime(),
                        SharingEndDate = c.DateTime(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.NetworkFileId);
            
            CreateTable(
                "dbo.Base_NetworkFolder",
                c => new
                    {
                        FolderId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        FolderName = c.String(maxLength: 200, unicode: false),
                        IsPublic = c.Int(),
                        Sharing = c.Int(),
                        SharingFolderId = c.String(maxLength: 50, unicode: false),
                        SharingCreateDate = c.DateTime(),
                        SharingEndDate = c.DateTime(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FolderId);
            
            CreateTable(
                "dbo.Base_ObjectUserRelation",
                c => new
                    {
                        ObjectUserRelationId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        UserId = c.String(maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ObjectUserRelationId);
            
            CreateTable(
                "dbo.Base_PhoneNote",
                c => new
                    {
                        PhoneNoteId = c.String(nullable: false, maxLength: 50, unicode: false),
                        PhonenNumber = c.String(maxLength: 50, unicode: false),
                        SendContent = c.String(maxLength: 200, unicode: false),
                        SendTime = c.DateTime(),
                        SendStatus = c.String(maxLength: 50, unicode: false),
                        DeviceName = c.String(maxLength: 200, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.PhoneNoteId);
            
            CreateTable(
                "dbo.Base_Post",
                c => new
                    {
                        PostId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        DepartmentId = c.String(maxLength: 50, unicode: false),
                        RoleId = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.PostId);
            
            CreateTable(
                "dbo.Base_ProvinceCity",
                c => new
                    {
                        ProvinceCityId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ProvinceCityId);
            
            CreateTable(
                "dbo.Base_QueryRecord",
                c => new
                    {
                        QueryRecordId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        ConditionJson = c.String(unicode: false),
                        ResourceShare = c.Int(),
                        NextDefault = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.QueryRecordId);
            
            CreateTable(
                "dbo.Base_Roles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        Code = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Base_Shortcuts",
                c => new
                    {
                        ShortcutsId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ShortcutsId);
            
            CreateTable(
                "dbo.Base_Supplies",
                c => new
                    {
                        SupplierId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Telephone = c.String(nullable: false, maxLength: 50, unicode: false),
                        Fax = c.String(nullable: false, maxLength: 50, unicode: false),
                        AccountsMethod = c.String(nullable: false, maxLength: 50, unicode: false),
                        LinkMan = c.String(nullable: false, maxLength: 50, unicode: false),
                        LeadingOfficialId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Address = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            CreateTable(
                "dbo.Base_SysLog",
                c => new
                    {
                        SysLogId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        LogType = c.String(maxLength: 50, unicode: false),
                        IPAddress = c.String(maxLength: 50, unicode: false),
                        IPAddressName = c.String(maxLength: 200, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        DepartmentId = c.String(maxLength: 50, unicode: false),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(unicode: false),
                        Status = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.SysLogId);
            
            CreateTable(
                "dbo.Base_SysLogDetail",
                c => new
                    {
                        SysLogDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        SysLogId = c.String(maxLength: 50, unicode: false),
                        PropertyName = c.String(maxLength: 50, unicode: false),
                        PropertyField = c.String(maxLength: 50, unicode: false),
                        NewValue = c.String(unicode: false),
                        OldValue = c.String(maxLength: 50, unicode: false),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SysLogDetailId)
                .ForeignKey("dbo.Base_SysLog", t => t.SysLogId, cascadeDelete: true)
                .Index(t => t.SysLogId);
            
            CreateTable(
                "dbo.Base_User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyId = c.String(maxLength: 50, unicode: false),
                        DepartmentId = c.String(maxLength: 50, unicode: false),
                        InnerUser = c.Int(),
                        Code = c.String(maxLength: 50, unicode: false),
                        Account = c.String(maxLength: 50, unicode: false),
                        Password = c.String(maxLength: 50, unicode: false),
                        Secretkey = c.String(maxLength: 50, unicode: false),
                        RealName = c.String(maxLength: 50, unicode: false),
                        Spell = c.String(maxLength: 200, unicode: false),
                        Gender = c.String(maxLength: 50, unicode: false),
                        Birthday = c.DateTime(),
                        Mobile = c.String(maxLength: 50, unicode: false),
                        Telephone = c.String(maxLength: 50, unicode: false),
                        OICQ = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        ChangePasswordDate = c.DateTime(),
                        OpenId = c.Int(),
                        LogOnCount = c.Int(),
                        FirstVisit = c.DateTime(),
                        PreviousVisit = c.DateTime(),
                        LastVisit = c.DateTime(),
                        AuditStatus = c.String(maxLength: 50, unicode: false),
                        AuditUserId = c.String(maxLength: 50, unicode: false),
                        AuditUserName = c.String(maxLength: 50, unicode: false),
                        AuditDateTime = c.DateTime(),
                        Online = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Base_View",
                c => new
                    {
                        ViewId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        FieldName = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        ShowName = c.String(maxLength: 50, unicode: false),
                        ColumnWidth = c.Int(),
                        TextAlign = c.String(maxLength: 50, unicode: false),
                        AllowShow = c.Int(),
                        AllowDerive = c.Int(),
                        CustomSwitch = c.String(unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ViewId);
            
            CreateTable(
                "dbo.Base_ViewPermission",
                c => new
                    {
                        ViewPermissionId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ViewId = c.String(nullable: false, maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ViewPermissionId);
            
            CreateTable(
                "dbo.Base_ViewWhere",
                c => new
                    {
                        ViewWhereId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ControlType = c.String(maxLength: 50, unicode: false),
                        ControlDefault = c.String(maxLength: 200, unicode: false),
                        ControlSource = c.String(maxLength: 200, unicode: false),
                        FieldName = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 50, unicode: false),
                        ShowName = c.String(maxLength: 50, unicode: false),
                        AllowShow = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        ControlCustom = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.ViewWhereId);
            
            CreateTable(
                "dbo.Base_ViewWherePermission",
                c => new
                    {
                        ViewWherePermissionId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ModuleId = c.String(maxLength: 50, unicode: false),
                        ViewId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ViewWhereDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        SortCode = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ViewWherePermissionId);
            
            CreateTable(
                "dbo.__MigrationHistory",
                c => new
                    {
                        MigrationId = c.String(nullable: false, maxLength: 150),
                        ContextKey = c.String(nullable: false, maxLength: 300),
                        Model = c.Binary(nullable: false),
                        ProductVersion = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => new { t.MigrationId, t.ContextKey });
            
            CreateTable(
                "dbo._BBdbR_CntlPntBase",
                c => new
                    {
                        CntlPntId = c.Long(nullable: false, identity: true),
                        CntlPntCd = c.String(maxLength: 50),
                        CntlPntNm = c.String(maxLength: 50),
                        CntlPntTyp = c.String(maxLength: 50),
                        CntlPntSort = c.Int(),
                        Enabled = c.String(maxLength: 50),
                        CreTm = c.DateTime(),
                        CreCd = c.String(maxLength: 50),
                        CreNm = c.String(maxLength: 50),
                        MdfTm = c.DateTime(),
                        MdfCd = c.String(maxLength: 50),
                        MdfNm = c.String(maxLength: 50),
                        Rem = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.CntlPntId);
            
            CreateTable(
                "dbo._BWuE_CntlLogicDoc",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ControlPointID = c.Long(),
                        Sort = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        StartTime = c.DateTime(),
                        FinishTime = c.DateTime(),
                        Remarks = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._BWuE_CntlLogicPro",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ControlPointID = c.Long(),
                        Sort = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        StartTime = c.DateTime(),
                        FinishTime = c.DateTime(),
                        Remarks = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._Calendar",
                c => new
                    {
                        StaffID = c.Long(nullable: false, identity: true),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        StaffSex = c.String(maxLength: 50),
                        Birth = c.DateTime(),
                        IDCard = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Wechat = c.String(maxLength: 50),
                        StaffImage = c.Binary(storeType: "image"),
                        ImageType = c.String(maxLength: 50),
                        Account = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        SkillType = c.String(maxLength: 50),
                        SkillGrade = c.String(maxLength: 50),
                        MarryStatue = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StaffID);
            
            CreateTable(
                "dbo._CheckProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        CheckReportPath = c.String(maxLength: 500),
                        CheckType = c.Int(),
                        EquipmentID = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        Offline_type = c.Int(),
                        CheckState = c.Int(),
                        StartCheckTime = c.DateTime(),
                        EndCheckTime = c.DateTime(),
                        CheckStaff = c.String(maxLength: 50),
                        CheckResult = c.String(maxLength: 500),
                        CauseDescription = c.String(maxLength: 500),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._CheckProcessingDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        CheckReportPath = c.String(maxLength: 500),
                        CheckType = c.Int(),
                        EquipmentID = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        Offline_type = c.Int(),
                        CheckState = c.Int(),
                        StartCheckTime = c.DateTime(),
                        EndCheckTime = c.DateTime(),
                        CheckStaff = c.String(maxLength: 50),
                        CheckResult = c.String(maxLength: 500),
                        CauseDescription = c.String(maxLength: 500),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._CheckTask",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        TaskState = c.Int(),
                        CheckReason = c.Int(),
                        CheckType = c.Int(),
                        EquipmentID = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        WorkerCode = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ClassRankInformation",
                c => new
                    {
                        ClassRankID = c.Long(nullable: false, identity: true),
                        DetailCode = c.String(maxLength: 50),
                        ShiftID = c.Long(),
                        RestStartTime = c.DateTime(),
                        RestEndTime = c.DateTime(),
                        WorkStartTime = c.DateTime(),
                        WorkEndTime = c.DateTime(),
                        TimeType = c.Int(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ClassRankID);
            
            CreateTable(
                "dbo._CommingMaterial",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CommingMaterialCode = c.String(maxLength: 50),
                        CommingMateriaName = c.String(maxLength: 50),
                        CommingMaterialCount = c.Int(),
                        CommingMaterialTime = c.DateTime(),
                        ContactsName = c.String(maxLength: 50),
                        ContactsPhone = c.String(maxLength: 50),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._CommingMaterialDetail",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        CommingMaterialID = c.Long(),
                        MaterialCode = c.String(maxLength: 50),
                        MaterielType = c.Int(),
                        MaterielState = c.Int(),
                        CheckReportPath = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._CompanyBaseInformation",
                c => new
                    {
                        CompanyID = c.Long(nullable: false, identity: true),
                        CompanyCode = c.String(maxLength: 50),
                        CompanyName = c.String(maxLength: 50),
                        Nature = c.String(maxLength: 50),
                        ArtificialPerson = c.String(maxLength: 50),
                        Contacts = c.String(maxLength: 50),
                        Telephone = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Address = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo._DeliveryDetail",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        DeliveryID = c.Long(),
                        OrderID = c.Long(),
                        ProjectID = c.Long(),
                        PlanID = c.Long(),
                        ProductBornCode = c.Long(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._DeliveryTask",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        DeliveryCode = c.String(maxLength: 50),
                        DeliverTime = c.DateTime(),
                        StaffCode = c.String(maxLength: 50),
                        CompanyID = c.Long(),
                        ContactsName = c.String(maxLength: 50),
                        ContactsPhone = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentAlarmDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentModel = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        AlarmID = c.Long(),
                        AlarmCode = c.String(maxLength: 50),
                        AlarmName = c.String(maxLength: 50),
                        AlarmCategory = c.String(maxLength: 50),
                        AlarmType = c.String(maxLength: 50),
                        AlarmCause = c.String(maxLength: 50),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        RelieveCause = c.String(maxLength: 500),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        StaffID = c.Long(),
                        StaffCode = c.String(maxLength: 50),
                        SolverID = c.Long(),
                        SolverCode = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentAlarmInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentModel = c.String(maxLength: 50),
                        AlarmCode = c.String(maxLength: 50),
                        AlarmName = c.String(maxLength: 50),
                        AlarmCategory = c.String(maxLength: 50),
                        AlarmType = c.String(maxLength: 50),
                        AlarmDescription = c.String(maxLength: 500),
                        AlarmImage = c.Binary(storeType: "image"),
                        ImageType = c.Int(),
                        IsEnable = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentAlarmProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentModel = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        AlarmID = c.Long(),
                        AlarmCode = c.String(maxLength: 50),
                        AlarmName = c.String(maxLength: 50),
                        AlarmCategory = c.String(maxLength: 50),
                        AlarmType = c.String(maxLength: 50),
                        AlarmCause = c.String(maxLength: 50),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        RelieveCause = c.String(maxLength: 500),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureID = c.Long(),
                        ProcedureCode = c.String(maxLength: 50),
                        StaffID = c.Long(),
                        StaffCode = c.String(maxLength: 50),
                        SolverID = c.Long(),
                        SolverCode = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentGroupBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        WorkshopID = c.Long(),
                        EquipmentGroupCode = c.String(maxLength: 1, unicode: false),
                        EquipmentGroupName = c.String(maxLength: 1, unicode: false),
                        GroupDescription = c.String(maxLength: 200, unicode: false),
                        EquipmentNum = c.Int(),
                        EquipmentType = c.Long(),
                        EquipmentModel = c.String(maxLength: 1, unicode: false),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 1, unicode: false),
                        Reserve2 = c.String(maxLength: 1, unicode: false),
                        Reserve3 = c.String(maxLength: 1, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCode = c.String(maxLength: 50, unicode: false),
                        EquipmentGroupID = c.Long(),
                        EquipmentGroupCode = c.String(maxLength: 50, unicode: false),
                        EquipmentName = c.String(maxLength: 50, unicode: false),
                        EquipmentCategory = c.String(maxLength: 50, unicode: false),
                        EquipmentType = c.String(maxLength: 50, unicode: false),
                        EquipmentModel = c.String(maxLength: 50, unicode: false),
                        EquipmentImage = c.Binary(storeType: "image"),
                        EquipmentMaker = c.String(maxLength: 50, unicode: false),
                        EquipmentLife = c.String(maxLength: 50, unicode: false),
                        EquipmentMakeTime = c.DateTime(),
                        MaxIdleTime = c.Decimal(precision: 18, scale: 0),
                        IsMonitor = c.Boolean(),
                        TimeUnit = c.String(maxLength: 10, unicode: false),
                        EquipmentDescription = c.String(maxLength: 500, unicode: false),
                        MaintenanceCycle = c.Int(),
                        LeadTime = c.Int(),
                        IsEnable = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 50, unicode: false),
                        Reserve2 = c.String(maxLength: 50, unicode: false),
                        Reserve3 = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentStateDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentModel = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        EquipmentState = c.Int(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ContinueTime = c.Decimal(precision: 18, scale: 0),
                        Model = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._EquipmentStateProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EquipmentCategory = c.String(maxLength: 50),
                        EquipmentType = c.String(maxLength: 50),
                        EquipmentModel = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        EquipmentState = c.Int(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ContinueTime = c.Decimal(precision: 18, scale: 0),
                        Model = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._FactoryBaseInformation",
                c => new
                    {
                        FactoryID = c.Long(nullable: false, identity: true),
                        FactoryCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        FactoryName = c.String(nullable: false, maxLength: 50, unicode: false),
                        FactoryDescription = c.String(maxLength: 200, unicode: false),
                        CompanyID = c.Long(nullable: false),
                        IsAvailable = c.Boolean(),
                        SolversID = c.Long(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 50, unicode: false),
                        Reserve2 = c.String(maxLength: 50, unicode: false),
                        Reserve3 = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FactoryID);
            
            CreateTable(
                "dbo._FileFloderInformation",
                c => new
                    {
                        FileFloderID = c.Long(nullable: false, identity: true),
                        FileFloderCode = c.String(maxLength: 50),
                        FileFloderName = c.String(maxLength: 50),
                        ParentID = c.Long(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        DataSourcePath = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.FileFloderID);
            
            CreateTable(
                "dbo._FileInformation",
                c => new
                    {
                        FileID = c.Long(nullable: false, identity: true),
                        FileCode = c.String(maxLength: 50),
                        FileType = c.String(maxLength: 50),
                        FileEdition = c.String(maxLength: 200),
                        FileSize = c.String(maxLength: 200),
                        FileStoragePath = c.String(maxLength: 500),
                        UploadPcMac = c.String(maxLength: 100),
                        UploadPcIP = c.String(maxLength: 100),
                        UploadPcName = c.String(maxLength: 50),
                        FileData = c.String(maxLength: 1000),
                        FileName = c.String(maxLength: 500),
                        FileFloderID = c.Long(),
                        FileFloderCode = c.String(maxLength: 50),
                        FileFloderName = c.String(maxLength: 500),
                        IsEnable = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.FileID);
            
            CreateTable(
                "dbo._InfomationPushBase",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PushCode = c.String(maxLength: 50),
                        PushCategory = c.String(maxLength: 50),
                        PushRank = c.String(maxLength: 50),
                        IntervalTime = c.Decimal(precision: 18, scale: 6),
                        TimeUnit = c.String(maxLength: 50),
                        IsEnable = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._InfomationPushDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PushID = c.String(maxLength: 50),
                        PushCategory = c.String(maxLength: 50),
                        InitPushRankPushRank = c.String(maxLength: 50),
                        PushContent = c.String(maxLength: 50),
                        CreateType = c.String(maxLength: 50),
                        PushState = c.Int(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastPushTime = c.DateTime(),
                        CurrentPushRank = c.String(maxLength: 50),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._InfomationPushProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PushID = c.String(maxLength: 50),
                        PushCategory = c.String(maxLength: 50),
                        InitPushRankPushRank = c.String(maxLength: 50),
                        PushContent = c.String(maxLength: 50),
                        CreateType = c.String(maxLength: 50),
                        PushState = c.Int(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastPushTime = c.DateTime(),
                        CurrentPushRank = c.String(maxLength: 50),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._InfomationPushStaffConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PushID = c.String(maxLength: 50),
                        StaffID = c.String(maxLength: 50),
                        IsEnable = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._KitDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        KitID = c.Long(),
                        KitCode = c.String(maxLength: 50),
                        KitName = c.String(maxLength: 50),
                        ApplianceType = c.Int(),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        PositionType = c.Int(),
                        PositionID = c.Long(),
                        PositionCode = c.String(maxLength: 50),
                        PositionName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._KitProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        KitID = c.Long(),
                        KitCode = c.String(maxLength: 50),
                        KitName = c.String(maxLength: 50),
                        ApplianceType = c.Int(),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        PositionType = c.Int(),
                        PositionID = c.Long(),
                        PositionCode = c.String(maxLength: 50),
                        PositionName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._LoginInDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        IsSubmit = c.Boolean(),
                        StaffCode = c.String(maxLength: 50),
                        StaffID = c.Long(),
                        StaffName = c.String(maxLength: 50),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        EquipmentID = c.Long(),
                        EquipmentName = c.String(maxLength: 50),
                        IP = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._LoginInProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        IsSubmit = c.Boolean(),
                        StaffCode = c.String(maxLength: 50),
                        StaffID = c.Long(),
                        StaffName = c.String(maxLength: 50),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        EquipmentID = c.Long(),
                        EquipmentName = c.String(maxLength: 50),
                        IP = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProcedureFirstDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50, unicode: false),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.Long(),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        ProcedureFirstStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProcedureFirstRecord",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.Long(),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        ProcedureFirstStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProductDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductBornCode = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        Type = c.Int(),
                        DeadLine = c.DateTime(),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProductProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        WorkshopID = c.Long(),
                        WorkshopCode = c.String(maxLength: 50),
                        WorkshopName = c.String(maxLength: 50),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        Offline_type = c.Int(),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        CauseDescription = c.String(maxLength: 500),
                        Online_type = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProductProcessingDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        WorkshopID = c.Long(),
                        WorkshopCode = c.String(maxLength: 50),
                        WorkshopName = c.String(maxLength: 50),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        Offline_type = c.Int(),
                        OnlineTime = c.DateTime(),
                        OfflineTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                        CauseDescription = c.String(maxLength: 500),
                        Online_type = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ProductQualityData",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        CheckType = c.Int(),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        CheckStaffCode = c.String(maxLength: 50),
                        CheckStaffName = c.String(maxLength: 50),
                        ItemID = c.Long(),
                        ItemCode = c.String(maxLength: 50),
                        ItemName = c.String(maxLength: 50),
                        UpperLimit = c.Decimal(precision: 18, scale: 6),
                        LowerLimit = c.Decimal(precision: 18, scale: 6),
                        StandardValue = c.Decimal(precision: 18, scale: 6),
                        Unit = c.String(maxLength: 10),
                        CollectValue = c.Decimal(precision: 18, scale: 6),
                        CheckReportPath = c.String(maxLength: 500),
                        CheckResult = c.Int(),
                        CreateTime = c.DateTime(),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ReformDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OriginalOrderID = c.Long(),
                        OriginalOrderCode = c.String(maxLength: 50),
                        OriginalOrderName = c.String(maxLength: 50),
                        OriginalProjectID = c.Long(),
                        OriginalProjectCode = c.String(maxLength: 50),
                        OriginalProjectName = c.String(maxLength: 50),
                        OriginalPlanID = c.Long(),
                        OriginalPlanCode = c.String(maxLength: 50),
                        OriginalPlanName = c.String(maxLength: 50),
                        OriginalWorkmanshipCode = c.String(maxLength: 50),
                        OriginalWorkmanshipID = c.Long(),
                        ProductBornCode = c.String(maxLength: 50),
                        OriginalProductID = c.Long(),
                        OriginalProductCode = c.String(maxLength: 50),
                        OriginalProductName = c.String(maxLength: 50),
                        NewOrderID = c.Long(),
                        NewOrderCode = c.String(maxLength: 50),
                        NewOrderName = c.String(maxLength: 50),
                        NewProjectCode = c.String(maxLength: 50),
                        NewProjectID = c.Long(),
                        NewPlanCode = c.String(maxLength: 50),
                        NewPlanID = c.Long(),
                        NewWorkmanshipCode = c.String(maxLength: 50),
                        NewWorkmanshipID = c.Long(),
                        NewProductBornCode = c.String(maxLength: 1),
                        NewProductID = c.Long(),
                        NewProductCode = c.String(maxLength: 50),
                        NewProductName = c.String(maxLength: 50),
                        ReformStaffID = c.Long(),
                        ReformStaffCode = c.String(maxLength: 50),
                        ReformStaffName = c.String(maxLength: 50),
                        CauseDescription = c.String(maxLength: 500),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._ShiftBaseInformation",
                c => new
                    {
                        ShiftID = c.Long(nullable: false, identity: true),
                        ShiftCode = c.String(maxLength: 50),
                        ShiftName = c.String(maxLength: 50),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ShiftID);
            
            CreateTable(
                "dbo._ShiftTeamConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ClassRankID = c.Long(),
                        TeamID = c.Long(),
                        IsDefault = c.Boolean(),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._StaffBaseInformation",
                c => new
                    {
                        StaffID = c.Long(nullable: false, identity: true),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        StaffSex = c.String(maxLength: 50),
                        Birth = c.DateTime(),
                        IDCard = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Wechat = c.String(maxLength: 50),
                        DingID = c.String(maxLength: 50),
                        StaffImage = c.Binary(storeType: "image"),
                        ImageType = c.String(maxLength: 50),
                        Account = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        SkillType = c.String(maxLength: 50),
                        SkillGrade = c.String(maxLength: 50),
                        MarryStatue = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.StaffID);
            
            CreateTable(
                "dbo._SubmitWorktimeInfomation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        StaffID = c.Long(),
                        StaffCode = c.String(maxLength: 50),
                        StaffName = c.String(maxLength: 50),
                        EquipmentID = c.Long(),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        OrderName = c.String(maxLength: 50),
                        ProjectCode = c.String(maxLength: 50),
                        ProjectName = c.String(maxLength: 50),
                        PlanCode = c.String(maxLength: 50),
                        PlanName = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ApprovalStatus = c.Int(),
                        SubmitWorktime = c.Decimal(precision: 18, scale: 6),
                        ActualWorktime = c.Decimal(precision: 18, scale: 6),
                        TimeUnit = c.String(maxLength: 10),
                        ReviewerCode = c.String(maxLength: 50),
                        ReviewerName = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._TeamBaseInformation",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TeamCode = c.String(maxLength: 50),
                        TeamName = c.String(maxLength: 50),
                        TeamType = c.String(maxLength: 50),
                        Describe = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._TeamStaffConfig",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        TeamCode = c.String(maxLength: 50),
                        TeamName = c.String(maxLength: 50),
                        TeamType = c.String(maxLength: 50),
                        Describe = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._TurnoverWarehouseDocument",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ShelvesCode = c.String(maxLength: 50),
                        ShelvesName = c.String(maxLength: 50),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        PlanName = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        InStaffID = c.Long(),
                        InStaffCode = c.String(maxLength: 50),
                        InStaffName = c.String(maxLength: 50),
                        DeliveryTime = c.DateTime(),
                        OutStaffID = c.Long(),
                        OutStaffCode = c.String(maxLength: 50),
                        OutStaffName = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._TurnoverWarehouseProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ShelvesCode = c.String(maxLength: 50),
                        ShelvesName = c.String(maxLength: 50),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        PlanName = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        InStaffID = c.Long(),
                        InStaffCode = c.String(maxLength: 50),
                        InStaffName = c.String(maxLength: 50),
                        DeliveryTime = c.DateTime(),
                        OutStaffID = c.Long(),
                        OutStaffCode = c.String(maxLength: 50),
                        OutStaffName = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 200),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo._WorkshopBaseInformation",
                c => new
                    {
                        WorkshopID = c.Long(nullable: false, identity: true),
                        FactoryID = c.Long(),
                        WorkshopCode = c.String(maxLength: 50),
                        WorkshopName = c.String(maxLength: 50),
                        WorkshopType = c.String(maxLength: 50),
                        SolversID = c.Long(),
                        WorkshopAddress = c.String(maxLength: 50),
                        WorkshopPhone = c.String(maxLength: 50),
                        WorkshopDescription = c.String(maxLength: 50),
                        IsAvailable = c.Boolean(),
                        CreateTime = c.DateTime(),
                        CreatorID = c.Long(),
                        LastModifiedTime = c.DateTime(),
                        ModifierID = c.Long(),
                        Remarks = c.String(maxLength: 200, unicode: false),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.WorkshopID);
            
            CreateTable(
                "dbo.D_DEVICE_STATUS",
                c => new
                    {
                        RECORD_ID = c.String(nullable: false, maxLength: 50),
                        DEVICE_ID = c.String(maxLength: 50),
                        DEVICE_CODE = c.String(maxLength: 50),
                        DEVICE_NAME = c.String(maxLength: 50),
                        ADDRESS_ID = c.String(maxLength: 50),
                        STATUS_TYPE = c.String(maxLength: 50),
                        STATUS_CONTENT = c.String(maxLength: 50),
                        STATUS_DESC = c.String(maxLength: 50),
                        START_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        DURATION = c.Int(),
                        STATUS_REMARK = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RECORD_ID);
            
            CreateTable(
                "dbo.DataCollect",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        WorkCenterCode = c.String(maxLength: 50, unicode: false),
                        SerialCode = c.String(maxLength: 50, unicode: false),
                        AddressName = c.String(maxLength: 50, unicode: false),
                        AddressValue = c.String(maxLength: 100, unicode: false),
                        CollectTime = c.DateTime(),
                        HasRead = c.Boolean(),
                        ErrorMsg = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DefectiveProductProcessing",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        OrderID = c.Long(),
                        OrderCode = c.String(maxLength: 50),
                        ProductID = c.Long(),
                        ProductCode = c.String(maxLength: 50),
                        ProductName = c.String(maxLength: 50),
                        ProjectID = c.Long(),
                        ProjectCode = c.String(maxLength: 50),
                        PlanID = c.Long(),
                        PlanCode = c.String(maxLength: 50),
                        ProductBornCode = c.String(maxLength: 50),
                        ProcedureCode = c.String(maxLength: 50),
                        ProcedureName = c.String(maxLength: 50),
                        ProcedureID = c.String(maxLength: 50),
                        State = c.Int(),
                        EquipmentID = c.String(maxLength: 50),
                        EquipmentCode = c.String(maxLength: 50),
                        EquipmentName = c.String(maxLength: 50),
                        OnlineStaffID = c.Long(),
                        OnlineStaffCode = c.String(maxLength: 50),
                        OnlineStaffName = c.String(maxLength: 50),
                        OfflineStaffID = c.Long(),
                        OfflineStaffCode = c.String(maxLength: 50),
                        OfflineStaffName = c.String(maxLength: 50),
                        CauseDescription = c.String(maxLength: 500),
                        Reserve1 = c.String(maxLength: 50),
                        Reserve2 = c.String(maxLength: 50),
                        Reserve3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.P_DEVICE_ALARM",
                c => new
                    {
                        RECORD_ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        DEVICE_ID = c.String(maxLength: 50, unicode: false),
                        DEVICE_CODE = c.String(maxLength: 50, unicode: false),
                        DEVICE_NAME = c.String(maxLength: 50, unicode: false),
                        ADDRESS_ID = c.String(maxLength: 50, unicode: false),
                        ALARM_TYPE = c.String(maxLength: 50, unicode: false),
                        ALARM_CONTENT = c.String(maxLength: 50, unicode: false),
                        ALARM_DESC = c.String(maxLength: 50, unicode: false),
                        ALARM_REMARK = c.String(maxLength: 50, unicode: false),
                        START_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        DURATION = c.Int(),
                    })
                .PrimaryKey(t => t.RECORD_ID);
            
            CreateTable(
                "dbo.P_DEVICE_STATUS",
                c => new
                    {
                        RECORD_ID = c.String(nullable: false, maxLength: 50),
                        DEVICE_ID = c.String(maxLength: 50),
                        DEVICE_CODE = c.String(maxLength: 50),
                        DEVICE_NAME = c.String(maxLength: 50),
                        ADDRESS_ID = c.String(maxLength: 50),
                        STATUS_TYPE = c.String(maxLength: 50),
                        STATUS_CONTENT = c.String(maxLength: 50),
                        STATUS_DESC = c.String(maxLength: 50),
                        START_TIME = c.DateTime(),
                        END_TIME = c.DateTime(),
                        DURATION = c.Int(),
                        STATUS_REMARK = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RECORD_ID);
            
            CreateTable(
                "dbo.PettyCash",
                c => new
                    {
                        PettyCashId = c.String(nullable: false, maxLength: 50, unicode: false),
                        DepartmentId = c.String(maxLength: 50, unicode: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Keeper = c.String(maxLength: 50, unicode: false),
                        KeepType = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.PettyCashId);
            
            CreateTable(
                "dbo.POOrder",
                c => new
                    {
                        POOrderId = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentId = c.String(maxLength: 50, unicode: false),
                        BillNo = c.String(maxLength: 50, unicode: false),
                        BillDate = c.DateTime(),
                        Method = c.String(maxLength: 20, unicode: false),
                        Clearing = c.String(maxLength: 50, unicode: false),
                        ClearingTime = c.DateTime(),
                        Currency = c.String(maxLength: 50, unicode: false),
                        ExchangeRate = c.Decimal(precision: 18, scale: 2),
                        SupplierId = c.String(maxLength: 50, unicode: false),
                        FetchAdd = c.String(maxLength: 200, unicode: false),
                        SalesmanId = c.String(maxLength: 50, unicode: false),
                        Salesman = c.String(maxLength: 50, unicode: false),
                        POOrderType = c.Int(),
                        Cancellation = c.Int(),
                        CreateDepartmentId = c.String(maxLength: 50, unicode: false),
                        CreateDepartmentName = c.String(maxLength: 50, unicode: false),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        CreateDate = c.DateTime(),
                        IsSubmit = c.Int(),
                        DeleteMark = c.Int(),
                        ModifyDepartmentId = c.String(maxLength: 50, unicode: false),
                        ModifyDepartmentName = c.String(maxLength: 50, unicode: false),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        AuditStatus = c.String(maxLength: 50, unicode: false),
                        AuditStatusName = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.POOrderId);
            
            CreateTable(
                "dbo.POOrderEntry",
                c => new
                    {
                        POOrderEntryId = c.String(nullable: false, maxLength: 50, unicode: false),
                        POOrderId = c.String(maxLength: 50, unicode: false),
                        BatchNo = c.String(maxLength: 50, unicode: false),
                        ItemId = c.String(maxLength: 50, unicode: false),
                        ItemCode = c.String(maxLength: 50, unicode: false),
                        ItemName = c.String(maxLength: 200, unicode: false),
                        ItemModel = c.String(maxLength: 50, unicode: false),
                        UnitId = c.String(maxLength: 50, unicode: false),
                        Qty = c.Decimal(precision: 18, scale: 2),
                        Price = c.Decimal(precision: 18, scale: 2),
                        PriceAmount = c.Decimal(precision: 18, scale: 2),
                        PlusPrice = c.Decimal(precision: 18, scale: 2),
                        PlusPriceAmount = c.Decimal(precision: 18, scale: 2),
                        CESS = c.Decimal(precision: 18, scale: 2),
                        CESSAmount = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(maxLength: 200, unicode: false),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.POOrderEntryId);
            
            CreateTable(
                "dbo.RS_ORGE_STRU",
                c => new
                    {
                        RECORD_ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        PARENT_ORGE = c.String(maxLength: 50, unicode: false),
                        CHILD_ORGE = c.String(maxLength: 50, unicode: false),
                        TYPE = c.String(maxLength: 50, unicode: false),
                        ENABLED = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.RECORD_ID);
            
            CreateTable(
                "dbo.VW_LOT_BIRTHCODE",
                c => new
                    {
                        产品出生证 = c.String(nullable: false, maxLength: 50, unicode: false),
                        产品名称 = c.String(maxLength: 50, unicode: false),
                        产品编号 = c.String(maxLength: 50, unicode: false),
                        订单编号 = c.String(maxLength: 50, unicode: false),
                        订单名称 = c.String(maxLength: 50, unicode: false),
                        计划开始时间 = c.DateTime(),
                        计划结束时间 = c.DateTime(),
                        开始执行时间 = c.DateTime(),
                        执行完成时间 = c.DateTime(),
                        执行状态 = c.String(maxLength: 50, unicode: false),
                        产品状态 = c.String(maxLength: 50, unicode: false),
                        生产线 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.产品出生证);
            
            CreateTable(
                "dbo.VW_LOT_BIRTHCODE_ASS",
                c => new
                    {
                        产品出生证 = c.String(nullable: false, maxLength: 50, unicode: false),
                        产品名称 = c.String(maxLength: 50, unicode: false),
                        产品编号 = c.String(maxLength: 50, unicode: false),
                        是否返修 = c.String(maxLength: 50, unicode: false),
                        返修次数 = c.Int(),
                        返修代码 = c.String(maxLength: 50, unicode: false),
                        返修原因 = c.String(maxLength: 50, unicode: false),
                        订单编号 = c.String(maxLength: 50, unicode: false),
                        订单名称 = c.String(maxLength: 50, unicode: false),
                        计划开始时间 = c.DateTime(),
                        计划结束时间 = c.DateTime(),
                        开始执行时间 = c.DateTime(),
                        执行完成时间 = c.DateTime(),
                        执行状态 = c.String(maxLength: 50, unicode: false),
                        产品状态 = c.String(maxLength: 50, unicode: false),
                        生产线 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.产品出生证);
            
            CreateTable(
                "dbo.WF_FlowCCRole",
                c => new
                    {
                        FlowCCRoleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowMainId = c.String(maxLength: 50, unicode: false),
                        ControlName = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FlowCCRoleId);
            
            CreateTable(
                "dbo.WF_FlowLine",
                c => new
                    {
                        FlowLineId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowMainId = c.String(maxLength: 50, unicode: false),
                        FlowLineCode = c.String(maxLength: 50, unicode: false),
                        SourceNodeId = c.String(maxLength: 50, unicode: false),
                        GoNodeId = c.String(maxLength: 50, unicode: false),
                        ConditionType = c.Int(),
                        ConditionString = c.String(unicode: false),
                        ConditionJson = c.String(unicode: false),
                        ConditionValueJson = c.String(unicode: false),
                        CreateInclude = c.String(unicode: false),
                        CreateWithout = c.String(unicode: false),
                        ExcuteInclude = c.String(unicode: false),
                        ExcuteWithout = c.String(unicode: false),
                        UserMethod = c.String(maxLength: 200, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FlowLineId);
            
            CreateTable(
                "dbo.WF_FlowMain",
                c => new
                    {
                        FlowMainId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowCode = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 200, unicode: false),
                        RunWay = c.String(maxLength: 50, unicode: false),
                        FlowJson = c.String(unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        LinkUrl = c.String(maxLength: 200, unicode: false),
                        FrmType = c.String(maxLength: 50, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        RunSQL = c.String(unicode: false),
                        IsBill = c.Int(),
                        IsJoinTable = c.Int(),
                        IsCC = c.Int(),
                        CCRole = c.String(unicode: false),
                        ValidDays = c.Decimal(precision: 18, scale: 0),
                        VersionNo = c.Int(),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(storeType: "smalldatetime"),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(storeType: "smalldatetime"),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FlowMainId);
            
            CreateTable(
                "dbo.WF_FlowNodeRole",
                c => new
                    {
                        FlowNodeRoleId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowNodeId = c.String(maxLength: 50, unicode: false),
                        ObjectId = c.String(maxLength: 50, unicode: false),
                        ObjectType = c.Int(),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FlowNodeRoleId);
            
            CreateTable(
                "dbo.WF_FrmAttr",
                c => new
                    {
                        FrmAttrId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FrmMainId = c.String(maxLength: 50, unicode: false),
                        ControlName = c.String(maxLength: 50, unicode: false),
                        BindFeild = c.String(maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 50, unicode: false),
                        DefVal = c.String(maxLength: 200, unicode: false),
                        ControlType = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FrmAttrId);
            
            CreateTable(
                "dbo.WF_FrmDetail",
                c => new
                    {
                        FrmDetailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FrmAttrId = c.String(maxLength: 50, unicode: false),
                        ControlName = c.String(maxLength: 50, unicode: false),
                        BindFeild = c.String(maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 50, unicode: false),
                        DefVal = c.String(maxLength: 200, unicode: false),
                        ControlType = c.String(maxLength: 50, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FrmDetailId);
            
            CreateTable(
                "dbo.WF_FrmMain",
                c => new
                    {
                        FrmMainId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FrmCode = c.String(maxLength: 50, unicode: false),
                        FullName = c.String(maxLength: 200, unicode: false),
                        Category = c.String(maxLength: 50, unicode: false),
                        FrmVersion = c.String(maxLength: 50, unicode: false),
                        FrmTable = c.String(maxLength: 50, unicode: false),
                        PrimaryKey = c.String(maxLength: 50, unicode: false),
                        Heading = c.String(maxLength: 200, unicode: false),
                        FrmHtml = c.String(unicode: false, storeType: "text"),
                        FrmEventJson = c.String(unicode: false, storeType: "text"),
                        FrmDept = c.String(maxLength: 50, unicode: false),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        SortCode = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        FrmType = c.Int(),
                        FrmURL = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FrmMainId);
            
            CreateTable(
                "dbo.WF_FrmNodeRelation",
                c => new
                    {
                        FrmNodeRelationId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowNodeId = c.String(maxLength: 50, unicode: false),
                        FrmTable = c.String(maxLength: 50, unicode: false),
                        FrmField = c.String(maxLength: 50, unicode: false),
                        DisplayMark = c.Int(),
                        OperType = c.Int(),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FrmNodeRelationId);
            
            CreateTable(
                "dbo.WF_Instance",
                c => new
                    {
                        InstanceId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowMainId = c.String(maxLength: 50, unicode: false),
                        FrmMainId = c.String(maxLength: 50, unicode: false),
                        TaskId = c.String(maxLength: 50, unicode: false),
                        FirstStepId = c.String(maxLength: 50, unicode: false),
                        StartUserId = c.String(maxLength: 50, unicode: false),
                        CurrentStepId = c.String(maxLength: 50, unicode: false),
                        FlowStatus = c.Int(),
                        CompleteTime = c.DateTime(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        FlowTitle = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.InstanceId);
            
            CreateTable(
                "dbo.WF_NodeButtonRelation",
                c => new
                    {
                        NodeButtonRelationId = c.String(nullable: false, maxLength: 50, unicode: false),
                        FlowNodeId = c.String(maxLength: 50, unicode: false),
                        ButtonId = c.String(maxLength: 50, unicode: false),
                        IsBack = c.Int(),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.NodeButtonRelationId);
            
            CreateTable(
                "dbo.WF_Task",
                c => new
                    {
                        FlowTaskId = c.String(nullable: false, maxLength: 50, unicode: false),
                        InstanceId = c.String(maxLength: 50, unicode: false),
                        FlowMainId = c.String(maxLength: 50, unicode: false),
                        FrmMainId = c.String(maxLength: 50, unicode: false),
                        TaskId = c.String(maxLength: 50, unicode: false),
                        StepId = c.String(maxLength: 50, unicode: false),
                        ExcuteObjectId = c.String(unicode: false),
                        ExcuteObjectName = c.String(unicode: false),
                        TastStartTime = c.DateTime(),
                        TaskEndTime = c.DateTime(),
                        BeforeStepId = c.String(maxLength: 50, unicode: false),
                        LastStepId = c.String(maxLength: 50, unicode: false),
                        LastStepUserId = c.String(unicode: false),
                        ExcuteUserId = c.String(maxLength: 50, unicode: false),
                        ExcuteTime = c.DateTime(),
                        ExcuteResult = c.Int(),
                        Remark = c.String(maxLength: 200, unicode: false),
                        Enabled = c.Int(),
                        DeleteMark = c.Int(),
                        CreateDate = c.DateTime(),
                        CreateUserId = c.String(maxLength: 50, unicode: false),
                        CreateUserName = c.String(maxLength: 50, unicode: false),
                        ModifyDate = c.DateTime(),
                        ModifyUserId = c.String(maxLength: 50, unicode: false),
                        ModifyUserName = c.String(maxLength: 50, unicode: false),
                        FlowTitle = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.FlowTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Base_SysLogDetail", "SysLogId", "dbo.Base_SysLog");
            DropForeignKey("dbo.Base_InterfaceManageParameter", "InterfaceId", "dbo.Base_InterfaceManage");
            DropForeignKey("dbo.Base_EmailAddressee", "EmailId", "dbo.Base_Email");
            DropForeignKey("dbo.Base_EmailAccessory", "EmailId", "dbo.Base_Email");
            DropIndex("dbo.Base_SysLogDetail", new[] { "SysLogId" });
            DropIndex("dbo.Base_InterfaceManageParameter", new[] { "InterfaceId" });
            DropIndex("dbo.Base_EmailAddressee", new[] { "EmailId" });
            DropIndex("dbo.Base_EmailAccessory", new[] { "EmailId" });
            DropTable("dbo.WF_Task");
            DropTable("dbo.WF_NodeButtonRelation");
            DropTable("dbo.WF_Instance");
            DropTable("dbo.WF_FrmNodeRelation");
            DropTable("dbo.WF_FrmMain");
            DropTable("dbo.WF_FrmDetail");
            DropTable("dbo.WF_FrmAttr");
            DropTable("dbo.WF_FlowNodeRole");
            DropTable("dbo.WF_FlowMain");
            DropTable("dbo.WF_FlowLine");
            DropTable("dbo.WF_FlowCCRole");
            DropTable("dbo.VW_LOT_BIRTHCODE_ASS");
            DropTable("dbo.VW_LOT_BIRTHCODE");
            DropTable("dbo.RS_ORGE_STRU");
            DropTable("dbo.POOrderEntry");
            DropTable("dbo.POOrder");
            DropTable("dbo.PettyCash");
            DropTable("dbo.P_DEVICE_STATUS");
            DropTable("dbo.P_DEVICE_ALARM");
            DropTable("dbo.DefectiveProductProcessing");
            DropTable("dbo.DataCollect");
            DropTable("dbo.D_DEVICE_STATUS");
            DropTable("dbo._WorkshopBaseInformation");
            DropTable("dbo._TurnoverWarehouseProcessing");
            DropTable("dbo._TurnoverWarehouseDocument");
            DropTable("dbo._TeamStaffConfig");
            DropTable("dbo._TeamBaseInformation");
            DropTable("dbo._SubmitWorktimeInfomation");
            DropTable("dbo._StaffBaseInformation");
            DropTable("dbo._ShiftTeamConfig");
            DropTable("dbo._ShiftBaseInformation");
            DropTable("dbo._ReformDocument");
            DropTable("dbo._ProductQualityData");
            DropTable("dbo._ProductProcessingDocument");
            DropTable("dbo._ProductProcessing");
            DropTable("dbo._ProductDocument");
            DropTable("dbo._ProcedureFirstRecord");
            DropTable("dbo._ProcedureFirstDocument");
            DropTable("dbo._LoginInProcessing");
            DropTable("dbo._LoginInDocument");
            DropTable("dbo._KitProcessing");
            DropTable("dbo._KitDocument");
            DropTable("dbo._InfomationPushStaffConfig");
            DropTable("dbo._InfomationPushProcessing");
            DropTable("dbo._InfomationPushDocument");
            DropTable("dbo._InfomationPushBase");
            DropTable("dbo._FileInformation");
            DropTable("dbo._FileFloderInformation");
            DropTable("dbo._FactoryBaseInformation");
            DropTable("dbo._EquipmentStateProcessing");
            DropTable("dbo._EquipmentStateDocument");
            DropTable("dbo._EquipmentInfomation");
            DropTable("dbo._EquipmentGroupBase");
            DropTable("dbo._EquipmentAlarmProcessing");
            DropTable("dbo._EquipmentAlarmInfomation");
            DropTable("dbo._EquipmentAlarmDocument");
            DropTable("dbo._DeliveryTask");
            DropTable("dbo._DeliveryDetail");
            DropTable("dbo._CompanyBaseInformation");
            DropTable("dbo._CommingMaterialDetail");
            DropTable("dbo._CommingMaterial");
            DropTable("dbo._ClassRankInformation");
            DropTable("dbo._CheckTask");
            DropTable("dbo._CheckProcessingDocument");
            DropTable("dbo._CheckProcessing");
            DropTable("dbo._Calendar");
            DropTable("dbo._BWuE_CntlLogicPro");
            DropTable("dbo._BWuE_CntlLogicDoc");
            DropTable("dbo._BBdbR_CntlPntBase");
            DropTable("dbo.__MigrationHistory");
            DropTable("dbo.Base_ViewWherePermission");
            DropTable("dbo.Base_ViewWhere");
            DropTable("dbo.Base_ViewPermission");
            DropTable("dbo.Base_View");
            DropTable("dbo.Base_User");
            DropTable("dbo.Base_SysLogDetail");
            DropTable("dbo.Base_SysLog");
            DropTable("dbo.Base_Supplies");
            DropTable("dbo.Base_Shortcuts");
            DropTable("dbo.Base_Roles");
            DropTable("dbo.Base_QueryRecord");
            DropTable("dbo.Base_ProvinceCity");
            DropTable("dbo.Base_Post");
            DropTable("dbo.Base_PhoneNote");
            DropTable("dbo.Base_ObjectUserRelation");
            DropTable("dbo.Base_NetworkFolder");
            DropTable("dbo.Base_NetworkFile");
            DropTable("dbo.Base_ModulePermission");
            DropTable("dbo.Base_Module");
            DropTable("dbo.Base_Language");
            DropTable("dbo.Base_InterfaceManageParameter");
            DropTable("dbo.Base_InterfaceManage");
            DropTable("dbo.Base_GroupUser");
            DropTable("dbo.Base_FormAttributeValue");
            DropTable("dbo.Base_FormAttribute");
            DropTable("dbo.Base_ExcelImportDetail");
            DropTable("dbo.Base_ExcelImport");
            DropTable("dbo.Base_Employee");
            DropTable("dbo.Base_EmailCategory");
            DropTable("dbo.Base_EmailAddressee");
            DropTable("dbo.Base_EmailAccessory");
            DropTable("dbo.Base_Email");
            DropTable("dbo.Base_Department");
            DropTable("dbo.Base_DataScopePermission");
            DropTable("dbo.Base_DataDictionaryDetail");
            DropTable("dbo.Base_DataDictionary");
            DropTable("dbo.Base_Company");
            DropTable("dbo.Base_CodeRuleSerious");
            DropTable("dbo.Base_CodeRuleDetail");
            DropTable("dbo.Base_CodeRule");
            DropTable("dbo.Base_ButtonPermission");
            DropTable("dbo.Base_Button");
            DropTable("dbo.Base_BackupJob");
            DropTable("dbo.APS_ResLineTime");
            DropTable("dbo.APS_ProcedureTaskDetail");
            DropTable("dbo.APS_ProcedureTask");
            DropTable("dbo.A_WorkerTaskConfig");
            DropTable("dbo.A_ProjectProcessModel");
            DropTable("dbo.A_ProjectPlanInfomation");
            DropTable("dbo.A_ProjectInfomation");
            DropTable("dbo.A_ProductProcedureBase");
            DropTable("dbo.A_ProductBase");
            DropTable("dbo.A_ProcessProcedureBase");
            DropTable("dbo.A_ProcessModelBase");
            DropTable("dbo.A_ProcedureStaffSkillConfig");
            DropTable("dbo.A_ProcedureSelfCheckingConfig");
            DropTable("dbo.A_ProcedureEquipmentConfig");
            DropTable("dbo.A_ProcedureCutterConfig");
            DropTable("dbo.A_ProcedureBase");
            DropTable("dbo.A_PlanProductInfomation");
            DropTable("dbo.A_OrderBase");
            DropTable("dbo.A_MaterialProgramDemand");
            DropTable("dbo.A_KitBase");
            DropTable("dbo.A_CutterDemand");
        }
    }
}
