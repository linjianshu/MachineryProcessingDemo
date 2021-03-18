using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using HZH_Controls;
using HZH_Controls.Forms;
using MachineryProcessingDemo.helper;
using Microsoft.Extensions.Configuration;
using QualityCheckDemo;

namespace MachineryProcessingDemo.Forms
{
    public partial class ScanOfflineForm : FrmWithOKCancel1
    {
        public ScanOfflineForm(string productBornCode, long? staffId, string staffCode, string staffName)
        {
            _productBornCode = productBornCode;
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            InitializeComponent();
        }
        private static string _workshopId;
        private static string _workshopCode;
        private static string _workshopName;
        private static string _equipmentId;
        private static string _equipmentCode;
        private static string _equipmentName;
        private static long? _staffId;
        private static string _staffCode;
        private static string _staffName;
        private static string _productBornCode;
        private static C_ProductProcessing _cProductProcessing;
        public Action ChangeBgColor;
        public Action ClearMainPanelTxt;
        public Action RegetProcedureTasksDetails;
        public Action HideLabel;
        public Action ResetProductPhoto; 

        private void ScanOfflineForm_Load(object sender, EventArgs e)
        {
            var addXmlFile = new ConfigurationBuilder().SetBasePath(GlobalClass.Xml)
                .AddXmlFile("config.xml");
            var configuration = addXmlFile.Build();
            _workshopId = configuration["WorkshopID"];
            _workshopCode = configuration["WorkshopCode"];
            _workshopName = configuration["WorkshopName"];
            _equipmentId = configuration["EquipmentID"];
            _equipmentCode = configuration["EquipmentCode"];
            _equipmentName = configuration["EquipmentName"];
            DataFill();
        }

        private void DataFill()
        {
            var tuple = new Tuple<string, string>("扫码下线", "A_fa_cube");
            FontIcons icon1 = (FontIcons)Enum.Parse(typeof(FontIcons), tuple.Item2);
            var pictureBox1 = new PictureBox
            {
                AutoSize = false,
                Size = new Size(40, 40),
                ForeColor = Color.FromArgb(255, 77, 59),
                Image = FontImages.GetImage(icon1, 40, Color.FromArgb(255, 77, 59)),
                Location = new Point(this.Size.Width / 2 - 20, 15)
            };
            panel3.Controls.Add(pictureBox1);

            using (var context = new Model())
            {
                var cProductProcessing = context.C_ProductProcessing.First(s => s.ProductBornCode == _productBornCode);
                BeginInvoke(new Action(() =>
                {
                    ProductIDTxt.Text = _productBornCode;
                    ProductIDTxt.ReadOnly = true;
                    ProductNameTxt.Text = cProductProcessing.ProductName;
                    ProductNameTxt.ReadOnly = true;
                }));
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "不良下机")
            {
                BadReasonLbl.Visible = true;
                richTextBox1.Visible = true;
            }
            else
            {
                BadReasonLbl.Visible = false;
                richTextBox1.Visible = false;
            }
        }

        protected override void DoEnter()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据 
                _cProductProcessing =
                    context.C_ProductProcessing.Where(s => s.ProductBornCode == _productBornCode)
                        .OrderByDescending(s => s.OnlineTime).FirstOrDefault();

                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                    s.IsAvailable == true && s.ProductBornCode == _productBornCode &&
                    s.TaskState != (decimal?)ApsProcedureTaskDetailState.Completed && s.ProcedureType == (decimal?)ProcedureType.Tooling);
                if (apsProcedureTaskDetail != null)
                {
                    //如果是不良下机则生成过程检验任务
                    if (comboBox1.Text.Trim().Equals("不良下机"))
                    {
                        GenerateProcessTask((int)CheckReason.Bad);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                    }

                    //apsdetail状态修改以及完善apsdetail最后修改时间呀什么的
                    PerfectApsDetail();

                    //控制点转档
                    OfflineCntLogicTurn();
                    //修改aps工序任务表里的信息,例如修改完成状态,更新执行进度
                    PerfectApsProcedureTask();

                    if (comboBox1.Text.Trim().Equals("正常下机"))
                    {
                        //也给工量具档案表转个档好惹
                        KitProcessingDocTurn();
                    }

                    //产品加工过程表转档
                    ProductProcessingDocTurn();

                    FrmDialog.ShowDialog(this, "产品下线成功", "提示");
                    ChangeBgColor();
                    // ClearMainPanelTxt();
                    RegetProcedureTasksDetails();
                    HideLabel();
                    Close();
                    return;
                }
            }
            var isChecked = IsChecked();
            //判断紧前产品是否送检以及送检结果是否生成
            if (IsCheckResult())
            {
                //判断是否是机加工工序的末道工序以及生成手动检验任务
                if (IsLastProcedureAndGenerateManualTask())
                {
                    // 生成除了本工序外其他的所有未检验的机加工工序三坐标任务(除了强制下线之外的)
                    GenerateOtherThreeTasks();
                    //标识除了本工序外其他的所有未检验的机加工工序  送检(除了强制下线之外的)
                    RemarkOtherInspect();

                    //如果是不良下机则生成过程检验任务
                    if (comboBox1.Text.Trim().Equals("不良下机"))
                    {
                        GenerateProcessTask((int)CheckReason.Bad);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                        //首件记录转档
                        FirstTurnDoc();
                    }

                    if (isChecked)
                    {
                        //生成本出生证本工序的过程检验任务
                        GenerateProcessTask((int)CheckReason.LastProcedure);
                        //标识本出生证本工序的送检
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                    }

                    if (IsLastProcedureEeceptQc())
                    {
                        //如果是除了质检外的末道工序 生成产品档案表 师兄,末道工序转到产品档案表里是指的是机加工序的末道工序还是整个产品而言的末道工序???
                        GenerateProductDoc();
                    }

                    //如果是调试件但又没有分配抽检  理论上就不应该出现在那个首件记录表里边 这时候要将首件记录过程表中的记录删掉而不是转档
                    if (!isChecked)
                    {
                        DelFirstRecord();
                    }
                }
                else
                {
                    //判断产品是否是抽检 只有抽检的情况下才能确定
                    //我们分开来考虑：① 该工序抽检：1.首件2.达到送检频率3.不良下机4.返修5 上件质检结果为NG 的也要送检
                    // ②：该工序不抽检：不良下机，返修

                     if (_cProductProcessing.Online_type == (decimal?)ProductProcessingOnlineType.Repair)
                    {
                        GenerateProcessTask((int)CheckReason.Repair);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                    }
                    //如果是不良下机则生成过程检验任务
                     else if (comboBox1.Text.Trim().Equals("不良下机"))
                    {
                        GenerateProcessTask((int)CheckReason.Bad);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                        //首件记录转档
                        FirstTurnDoc();
                    }
                    //如果是调试件则生成过程检验任务
                    else if (isChecked && IsFirstRecord())
                    {
                        GenerateProcessTask((int)CheckReason.First);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                        //首件记录转档
                        FirstTurnDoc();
                    }
                    //如果是调试件但又没有分配抽检  理论上就不应该出现在那个首件记录表里边 这时候要将首件记录过程表中的记录删掉而不是转档
                    else if (!isChecked && IsFirstRecord())
                    {
                        DelFirstRecord();
                    }
                    //如果是返修上线则生成过程检验任务
                    
                    //如果上件产品送检结果为NG则生成过程检验任务
                    else if (isChecked && IsNotGood())
                    {
                        GenerateProcessTask((int)CheckReason.LastPieceNotGood);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                    }
                    //是否达到抽检频率
                    else if (isChecked && IsMeetFrequency())
                    {
                        GenerateProcessTask((int)CheckReason.MeetFrequency);
                        RemarkInspect();
                        FrmDialog.ShowDialog(this, "请将此 产品送至质检中心,等待进一步检验结果!");
                    }
                }
                //apsdetail状态修改以及完善apsdetail最后修改时间呀什么的
                PerfectApsDetail();

                //控制点转档
                OfflineCntLogicTurn();
                //计划产品信息状态修改(不应该出现,应该出现在某道产品末道工序的最后一个产品上 可以用goto语句搞一搞)
                //理论上这边也要有的,因为我们并不能确定机加工是不是就是末道工序了,所以需要添加一下

                // PlanProductInfoStateChange();
                //完善产品质量数据表
                PerfectProductQD();

                //修改aps工序任务表里的信息,例如修改完成状态,更新执行进度
                PerfectApsProcedureTask();

                //产品加工过程表转档
                ProductProcessingDocTurn();
                //ResetProductPhoto(); 
                FrmDialog.ShowDialog(this, "产品下线成功", "提示");
                ChangeBgColor();
                RegetProcedureTasksDetails();
            }
            Close();
        }

        //针对工装环节 , 如果正常下机的话 不仅需要在产品档案表中存有,还需要在工量具加工档案表中存有
        //但不能出现在产品档案表中, 需要将产品和工量具区分开来
        private void KitProcessingDocTurn()
        {
            using (var context = new Model())
            {
                var kitProcessingDocument = new KitProcessingDocument();
                kitProcessingDocument.IsAvailable = true;
                kitProcessingDocument.LastModifiedTime = context.GetServerDate();
                kitProcessingDocument.ModifierID = _staffId;
                kitProcessingDocument.CreateTime = context.GetServerDate();
                kitProcessingDocument.CreatorID = _staffId;
                //如何判定工量具类型
                kitProcessingDocument.ApplicanceType = 1;
                kitProcessingDocument.KitBornCode = _cProductProcessing.ProductBornCode;
                kitProcessingDocument.KitName = _cProductProcessing.ProductName;
                context.KitProcessingDocument.Add(kitProcessingDocument);
                context.SaveChanges();
            }
        }

        //删除首件 首件添加是在产品上线的时候添加的, 产品上线的时候并不知道产品有没有送检,如果没有送检,就要删除首件约束
        private void DelFirstRecord()
        {
            using (var context = new Model())
            {
                var cProcedureFirstRecords = context.C_ProcedureFirstRecord.FirstOrDefault(s =>
                    s.PlanID == _cProductProcessing.PlanID && s.ProductID == _cProductProcessing.ProductID &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                    s.EquipmentID == _cProductProcessing.EquipmentID && s.ProcedureFirstStatus == 0);

                if (cProcedureFirstRecords != null)
                {
                    context.C_ProcedureFirstRecord.Remove(cProcedureFirstRecords);
                    context.SaveChanges();
                }
            }
        }

        //判断是否是除了质检之外的末道工序
        private bool IsLastProcedureEeceptQc()
        {
            using (var context = new Model())
            {
                //在产品工序基础表里 根据项目号/计划号/产品号/有效性/工序类型(去掉)  对工序索引进行排序 获得末道工序信息
                var aProductProcedureBase = context.A_ProductProcedureBase.Where(s =>
                        s.ProjectID == _cProductProcessing.ProjectID && s.PlanID == _cProductProcessing.PlanID
                                                                     && s.ProductID == _cProductProcessing.ProductID
                                                                     && s.IsAvailable == true)
                    .OrderByDescending(s => s.ProcedureIndex).First();
                //判断末道工序是不是当前工序
                if (aProductProcedureBase.ProcedureID.ToString() == _cProductProcessing.ProcedureID)
                {
                    return true;
                }
                return false;
            }
        }

        private void GenerateProductDoc()
        {
            using (var context = new Model())
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<C_ProductProcessing, C_ProductDocument>());
                var mapper = mapperConfiguration.CreateMapper();
                var cProcedureFirstDocument = mapper.Map<C_ProductDocument>(_cProductProcessing);
                //需要修改 正常还是报废
                cProcedureFirstDocument.Type = -1;
                cProcedureFirstDocument.OfflineTime = context.GetServerDate();
                cProcedureFirstDocument.IsAvailable = true;
                cProcedureFirstDocument.CreateTime = context.GetServerDate();
                cProcedureFirstDocument.CreatorID = _staffId;
                cProcedureFirstDocument.LastModifiedTime = context.GetServerDate();
                cProcedureFirstDocument.ModifierID = _staffId;

                var cProductProcessingDocument = context.C_ProductProcessingDocument.Where(s => s.ProductBornCode == _productBornCode
                                                                                                 && s.PlanCode == _cProductProcessing.PlanCode && s.PlanID == _cProductProcessing.PlanID)
                    .OrderBy(s => s.OfflineTime).FirstOrDefault();
                cProcedureFirstDocument.OnlineTime = cProductProcessingDocument.OnlineTime;
                context.C_ProductDocument.Add(cProcedureFirstDocument);
                context.SaveChanges();
            }
        }

        //标注除了机加末道工序外的分配抽检但是没有送检且不是强制下机的三坐标送检任务
        private void RemarkOtherInspect()
        {
            using (var context = new Model())
            {
                //获取到有抽检/没送检/不是强制下机的
                var dbRawSqlQuery = context.Database.SqlQuery<APS_ProcedureTaskDetail>(
                    "SELECT\r\n\tAPS_ProcedureTaskDetail.*\r\nFROM\r\n\tdbo.APS_ProcedureTaskDetail,\r\n\tdbo._ProductProcessingDocument\r\nWHERE\r\n\tAPS_ProcedureTaskDetail.EquipmentID = _ProductProcessingDocument.EquipmentID AND\r\n\tAPS_ProcedureTaskDetail.ProductBornCode = _ProductProcessingDocument.ProductBornCode AND\r\n\tAPS_ProcedureTaskDetail.ProcedureCode = _ProductProcessingDocument.ProcedureCode AND\r\n\t_ProductProcessingDocument.Offline_type = 1\r\nand \r\nAPS_ProcedureTaskDetail.ischecked = 1\r\nand APS_ProcedureTaskDetail.IsAvailable = 1");
                var procedureTaskDetails = dbRawSqlQuery.ToList();

                if (procedureTaskDetails.Any())
                {
                    var apsProcedureTaskDetails = procedureTaskDetails.Where(s =>
                        s.IsAvailable == true && s.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed &&
                        s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                        s.EquipmentID == _cProductProcessing.EquipmentID && s.IsInspect != 1 && s.IsChecked == 1).ToList();
                    foreach (var apsProcedureTaskDetail in apsProcedureTaskDetails)
                    {
                        context.Entry(apsProcedureTaskDetail).State = EntityState.Modified;
                        //标识为送检
                        apsProcedureTaskDetail.IsInspect = 1;
                        apsProcedureTaskDetail.LastModifiedTime = context.GetServerDate();
                        apsProcedureTaskDetail.ModifierID = _staffId.ToString();

                    }
                    context.SaveChanges();
                }
            }
        }

        private void FirstTurnDoc()
        {
            using (var context = new Model())
            {
                var cProcedureFirstRecord = context.C_ProcedureFirstRecord.FirstOrDefault(s =>
                    s.PlanID == _cProductProcessing.PlanID && s.ProductID == _cProductProcessing.ProductID &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                    s.EquipmentID == _cProductProcessing.EquipmentID);

                //有就转档, , 没有就告辞
                if (cProcedureFirstRecord != null)
                {
                    var mapperConfiguration = new MapperConfiguration(cfg =>
           cfg.CreateMap<C_ProcedureFirstRecord, C_ProcedureFirstDocument>());
                    var mapper = mapperConfiguration.CreateMapper();
                    var cProcedureFirstDocument = mapper.Map<C_ProcedureFirstDocument>(cProcedureFirstRecord);
                    context.C_ProcedureFirstRecord.Remove(cProcedureFirstRecord);
                    context.C_ProcedureFirstDocument.Add(cProcedureFirstDocument);
                }

                context.SaveChanges();
            }
        }

        private void PerfectApsProcedureTask()
        {
            using (var context = new Model())
            {
                //最近新修改过的 apsdetail表中根据出生证/有效性/工序编号/设备号/状态/ 按照创建时间排序 获得最新的
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.Where(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode && s.IsAvailable == true
                                                                             && s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                                                                             s.EquipmentCode == _cProductProcessing.EquipmentCode &&
                                                                             s.TaskState == (decimal?) ApsProcedureTaskDetailState.Completed)
                    .OrderByDescending(s=>s.CreateTime).FirstOrDefault();

                var apsProcedureTask = context.APS_ProcedureTask.First(s =>
                    s.ID == apsProcedureTaskDetail.TaskTableID && s.IsAvailable == true); 


                // //在工序任务表里 根据订单号/计划号/项目号/产品号/工序号/设备号获得元数据
                // var apsProcedureTask = context.APS_ProcedureTask.First(s =>
                //     s.OrderID == _cProductProcessing.OrderID && s.PlanCode == _cProductProcessing.PlanCode &&
                //     s.ProjectCode == _cProductProcessing.ProjectCode &&
                //     s.ProductCode == _cProductProcessing.ProductCode &&
                //     s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                //     s.EquipmentID == _cProductProcessing.EquipmentID);

                var count = context.APS_ProcedureTaskDetail.Count(s =>
                    s.EquipmentID == _cProductProcessing.EquipmentID && s.TaskTableID == apsProcedureTask.ID &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed && s.IsAvailable == true);

                decimal progressPercent = (decimal)((double)count / apsProcedureTask.ProductNumber);

                apsProcedureTask.ProgressPercent = progressPercent;
                apsProcedureTask.ModifierID = _staffId.ToString();
                apsProcedureTask.LastModifiedTime = context.GetServerDate();

                if (progressPercent == 1)
                {
                    apsProcedureTask.EndTime = context.GetServerDate();
                    apsProcedureTask.TaskState = (int?)ApsProcedureTaskState.Completed; //已完成呜呜呜
                }
                context.SaveChanges();
            }
        }

        private void PerfectProductQD()
        {
            using (var context = new Model())
            {
                var cProductQualityDatas = context.C_ProductQualityData.Where(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.PlanID == _cProductProcessing.PlanID && s.OfflineStaffID == null &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                    s.Online_type == _cProductProcessing.Online_type).ToList();

                foreach (var cProductQualityData in cProductQualityDatas)
                {
                    cProductQualityData.OfflineStaffCode = _staffCode;
                    cProductQualityData.OfflineStaffID = _staffId;
                    cProductQualityData.OfflineStaffName = _staffName;
                }
                context.SaveChanges();
            }
        }

        private void PlanProductInfoStateChange()
        {
            using (var context = new Model())
            {
                var aPlanProductInfomation = context.A_PlanProductInfomation.FirstOrDefault(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode && s.IsAvailable == true && s.State == (decimal?)PlanProductInfoState.InExcecution);
                if (aPlanProductInfomation != null)
                {
                    aPlanProductInfomation.State = (int?)PlanProductInfoState.Completed;
                    aPlanProductInfomation.LastModifiedTime = context.GetServerDate();
                    aPlanProductInfomation.ModifierID = _staffId;
                }
                context.SaveChanges();
            }
        }

        private void OfflineCntLogicTurn()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());

                //在控制点过程表中 根据产品出生证 工序编号 控制点id 设备编号(需要修改) 查到相关集合
                var cBWuECntlLogicPros = context.C_BWuE_CntlLogicPro.Where(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim() && s.ProcedureCode == _cProductProcessing.ProcedureCode
                                                                      && s.ControlPointID == 3 && s.EquipmentCode == _equipmentCode)
                    .OrderByDescending(s => s.StartTime).ToList();
                //判断过程表中有无数据  如果没有(说明已经转档过了) 那就不操作了弟弟
                if (cBWuECntlLogicPros.Any())
                {
                    cBWuECntlLogicPros[0].State = "2";
                    cBWuECntlLogicPros[0].FinishTime = context.GetServerDate();

                    //遍历  添加后删除过程表中所有选中数据
                    foreach (var cBWuECntlLogicPro in cBWuECntlLogicPros)
                    {
                        var mapperConfiguration = new MapperConfiguration(cfg =>
                            cfg.CreateMap<C_BWuE_CntlLogicPro, C_BWuE_CntlLogicDoc>());
                        var mapper = mapperConfiguration.CreateMapper();
                        var cBWuECntlLogicDoc = mapper.Map<C_BWuE_CntlLogicDoc>(cBWuECntlLogicPro);

                        context.C_BWuE_CntlLogicDoc.Add(cBWuECntlLogicDoc);
                        context.C_BWuE_CntlLogicPro.Remove(cBWuECntlLogicPro);
                    }
                    context.SaveChanges();
                }
            }
        }

        private void PerfectApsDetail()
        {
            using (var context = new Model())
            {
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.EquipmentID == _cProductProcessing.EquipmentID &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode && s.IsAvailable == true && s.TaskState == (decimal?)ApsProcedureTaskDetailState.InExecution);
                apsProcedureTaskDetail.TaskState = (int?)ApsProcedureTaskDetailState.Completed;//已完成
                apsProcedureTaskDetail.ModifierID = _staffId.ToString();
                apsProcedureTaskDetail.LastModifiedTime = context.GetServerDate();
                context.Entry(apsProcedureTaskDetail).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private void ProductProcessingDocTurn()
        {
            using (var context = new Model())
            {
                _cProductProcessing.OfflineStaffID = _staffId;
                _cProductProcessing.OfflineStaffCode = _staffCode;
                _cProductProcessing.OfflineStaffName = _staffName;

                if (comboBox1.Text.Trim().Contains("正常"))
                {
                    _cProductProcessing.Offline_type = (int?)ProductProcessingOfflineType.Normal;
                }
                else
                {
                    _cProductProcessing.Offline_type = (int?)ProductProcessingOfflineType.Bad;//不良
                    _cProductProcessing.CauseDescription = richTextBox1.Text.Trim();
                }
                _cProductProcessing.OfflineTime = context.GetServerDate();
                context.Entry(_cProductProcessing).State = EntityState.Deleted;

                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<C_ProductProcessing, C_ProductProcessingDocument>());
                var mapper = mapperConfiguration.CreateMapper();
                var cProductProcessingDocument = mapper.Map<C_ProductProcessingDocument>(_cProductProcessing);
                context.C_ProductProcessingDocument.Add(cProductProcessingDocument);

                context.SaveChanges();
            }
        }

        private bool IsChecked()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());
                //在工序任务明细表中根据产品出生证/有效性/工序号 获得元数据
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode && s.IsAvailable == true &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode&&s.TaskState==(decimal?) ApsProcedureTaskDetailState.InExecution);
                if (apsProcedureTaskDetail.IsChecked == 1)
                {
                    return true;
                }
                return false;
            }
        }

        private void RemarkInspect()
        {
            using (var context = new Model())
            {
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode && s.IsAvailable == true
                    && s.TaskState == (decimal?)ApsProcedureTaskDetailState.InExecution);
                apsProcedureTaskDetail.IsInspect = 1;
                context.SaveChanges();
            }
        }

        private bool IsMeetFrequency()
        {
            using (var context = new Model())
            {
                //先在产品工序基础表里 根据产品号/计划号/工序号/有效性  拿到抽检频率
                var aProductProcedureBase = context.A_ProductProcedureBase.First(s =>
                    s.PlanID == _cProductProcessing.PlanID && s.ProductID == _cProductProcessing.ProductID &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode && s.IsAvailable == true);

                //在产品加工档案表里 根据设备号/产品号/计划号/工序号/下线时间排序 拿到(抽检频率)的集合
                var cProductProcessingDocuments = context.C_ProductProcessingDocument
                    .Where(s => s.PlanID == _cProductProcessing.PlanID &&
                                s.ProductID == _cProductProcessing.ProductID &&
                                s.ProcedureID == _cProductProcessing.ProcedureID
                                && s.EquipmentID == _cProductProcessing.EquipmentID).OrderByDescending(s => s.OfflineTime)
                    .Take((int)(aProductProcedureBase.CheckFrequency)).ToList();

                //如果是1 , 就要一直送检
                if ((int)aProductProcedureBase.CheckFrequency == 1)
                {
                    return true;
                }

                //如果take的数量小于送检频率,那就没有判断的必要 , 因为生产才刚刚开始
                if (cProductProcessingDocuments.Count == (int)(aProductProcedureBase.CheckFrequency))
                {
                    cProductProcessingDocuments.Reverse();

                    //保证累加
                    var count = context.Database.SqlQuery<APS_ProcedureTaskDetail>("SELECT\n\tAPS_ProcedureTaskDetail.*\nFROM\n\tdbo._ProductProcessingDocument,\n\tdbo.APS_ProcedureTaskDetail\nWHERE\n\tAPS_ProcedureTaskDetail.IsInspect = 1 AND\n\t_ProductProcessingDocument.ProductBornCode = APS_ProcedureTaskDetail.ProductBornCode AND\n\t_ProductProcessingDocument.ProcedureCode = APS_ProcedureTaskDetail.ProcedureCode AND\n\t_ProductProcessingDocument.EquipmentID = APS_ProcedureTaskDetail.EquipmentID AND\n\tAPS_ProcedureTaskDetail.IsAvailable = 1").Count();
                    if (count > 1)
                    {
                        return false;
                    }
                    // context.APS_ProcedureTaskDetail.Where(s=>s.IsInspect==1&&s.IsAvailable==1&&)

                    // if (cProductProcessingDocuments.Count(s => s == cProductProcessingDocument) > 0)
                    // {
                    // return false;
                    // }

                    bool isForce = true;
                    //遍历上述集合,根据产品出生证在工序任务明细表中查找  当前工序是否已经被送检过了 
                    foreach (var cProductProcessingDocument in cProductProcessingDocuments)
                    {
                        var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                            s.IsInspect == 1 && s.ProductBornCode == cProductProcessingDocument.ProductBornCode &&
                            s.ProcedureCode == cProductProcessingDocument.ProcedureCode);
                        //如果查出来了
                        if (apsProcedureTaskDetail != null)
                        {
                            isForce = false;
                            var findIndex = cProductProcessingDocuments.FindIndex(s => s == cProductProcessingDocument);
                            if (findIndex == 0)
                            {
                                //当前任务就需要送检
                                return true;
                            }
                            return false;
                            // //如果都没有发现index  , 说明可能是因为强制下机导致的
                            // if (findIndex==-1)
                            // {
                            //     return true; 
                            // }
                        }
                        continue;
                    }

                    if (isForce)
                    {
                        //如果为空就说明没有发现送检的
                        //则说明可能是因为强制下机导致的, 所以要送检
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsFirstRecord()
        {
            using (var context = new Model())
            {
                var cProcedureFirstRecord = context.C_ProcedureFirstRecord.FirstOrDefault(s =>
                    s.ProductBornCode == _productBornCode && s.ProcedureCode == _cProductProcessing.ProcedureCode);
                if (cProcedureFirstRecord != null)
                {
                    return true;
                }
                return false;
            }
        }

        private bool IsNotGood()
        {
            using (var context = new Model())
            {
                //在产品加工档案表中 根据设备号/计划号/产品id/工序号/下线时间排序 获取紧前产品
                var cProductProcessingDocument = context.C_ProductProcessingDocument
                    .Where(s => s.EquipmentID.ToString() == _equipmentId && s.ProcedureID == _cProductProcessing.ProcedureID &&
                                s.PlanID == _cProductProcessing.PlanID).OrderByDescending(s => s.OfflineTime)
                    .FirstOrDefault();

                //在检验档案表里判断紧前产品下机类型是否是ng
                var cCheckProcessingDocument = context.C_CheckProcessingDocument.FirstOrDefault(s =>
                    s.Offline_type == (decimal?)OfflineType.NG && s.ProductBornCode == cProductProcessingDocument.ProductBornCode);
                if (cCheckProcessingDocument != null)
                {
                    return true;
                }
                return false;
            }
        }

        private void GenerateProcessTask(int checkReason, int checktype = 2)
        {
            using (var context = new Model())
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<C_ProductProcessing, C_CheckTask>());
                var mapper = mapperConfiguration.CreateMapper();
                var cCheckTask = mapper.Map<C_CheckTask>(_cProductProcessing);

                cCheckTask.ProcedureID = _cProductProcessing.ProcedureID;
                cCheckTask.TaskState = (int?)CheckTaskState.NotOnline;
                //原因
                cCheckTask.CheckReason = checkReason;//最后一道工序
                cCheckTask.CheckType = checktype;//3 手检
                //如果是首件那就没有工序之分
                if (checktype == (int)CheckType.Manual)
                {
                    cCheckTask.ProcedureCode = "";
                    cCheckTask.ProcedureName = "";
                    cCheckTask.ProcedureID = "";
                }
                cCheckTask.IsAvailable = true;
                cCheckTask.CreateTime = context.GetServerDate();
                cCheckTask.CreatorID = _staffId;
                cCheckTask.LastModifiedTime = context.GetServerDate();
                cCheckTask.ModifierID = _staffId;

                //根据任务状态 拿到apsdetail
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                    s.IsAvailable == true && s.TaskState == (decimal?) ApsProcedureTaskDetailState.InExecution &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode);


                //拿到apsdetail所对应的id 用来和生成的检验任务一一对应
                cCheckTask.ApsDetailID = apsProcedureTaskDetail.ID;  

                //需要修改如果是手检的话要换人啦兄弟
                if (checktype == 3)
                {
                    cCheckTask.WorkerCode = DistributeCheckTask("手检").StaffCode + ',';
                }
                else
                {
                    cCheckTask.WorkerCode = DistributeCheckTask().StaffCode + ',';
                }

                context.C_CheckTask.Add(cCheckTask);
                context.SaveChanges();
            }
        }

        private bool IsLastProcedureAndGenerateManualTask()
        {
            using (var context = new Model())
            {
                //在产品工序基础表里 根据项目号/计划号/产品号/有效性/工序类型  对工序索引进行排序 获得末道工序信息
                var aProductProcedureBase = context.A_ProductProcedureBase.Where(s =>
                                                                                    s.ProjectID == _cProductProcessing.ProjectID && s.PlanID == _cProductProcessing.PlanID
                                                                                    && s.ProductID == _cProductProcessing.ProductID && s.ProcedureType == (decimal?)ProcedureType.Machining
                                                                                    && s.IsAvailable == true)
                    .OrderByDescending(s => s.ProcedureIndex).First();
                //判断末道工序是不是当前工序
                if (aProductProcedureBase.ProcedureID.ToString() == _cProductProcessing.ProcedureID)
                {
                    GenerateProcessTask((int)CheckReason.LastProcedure, (int)CheckType.Manual);
                    return true;
                }
                return false;
            }
        }

        private void GenerateOtherThreeTasks()
        {
            using (var context = new Model())
            {
                //获取到有抽检/没送检/不是强制下机的
                var dbRawSqlQuery = context.Database.SqlQuery<APS_ProcedureTaskDetail>("SELECT\r\n\tAPS_ProcedureTaskDetail.*\r\nFROM\r\n\tdbo.APS_ProcedureTaskDetail,\r\n\tdbo._ProductProcessingDocument\r\nWHERE\r\n\tAPS_ProcedureTaskDetail.EquipmentID = _ProductProcessingDocument.EquipmentID AND\r\n\tAPS_ProcedureTaskDetail.ProductBornCode = _ProductProcessingDocument.ProductBornCode AND\r\n\tAPS_ProcedureTaskDetail.ProcedureCode = _ProductProcessingDocument.ProcedureCode AND\r\n\t_ProductProcessingDocument.Offline_type = 1\r\nand \r\nAPS_ProcedureTaskDetail.ischecked = 1\r\nand APS_ProcedureTaskDetail.IsAvailable = 1");
                var procedureTaskDetails = dbRawSqlQuery.ToList();

                if (procedureTaskDetails.Any())
                {
                    //在工序任务明细表中根据产品出生证/有效性/以及没有送检/但有抽检   获得一系列该产品未送检的工序信息
                    var apsProcedureTaskDetails = procedureTaskDetails.Where(s =>
                        s.ProductBornCode == _cProductProcessing.ProductBornCode
                                                                                           && s.IsAvailable == true &&
                        s.IsInspect != 1 && s.IsChecked == 1 &&
                        s.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed).ToList();
                    //遍历 然后插入三坐标到检验任务表中
                    foreach (var apsProcedureTaskDetail in apsProcedureTaskDetails)
                    {
                        var mapperConfiguration = new MapperConfiguration(cfg =>
                            cfg.CreateMap<C_ProductProcessing, C_CheckTask>());
                        var mapper = mapperConfiguration.CreateMapper();
                        var cCheckTask = mapper.Map<C_CheckTask>(_cProductProcessing);

                        var aProcedureBase = context.A_ProcedureBase.First(s =>
                            s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode && s.IsAvailable == true);

                        cCheckTask.ProcedureID = aProcedureBase.ID.ToString();
                        cCheckTask.ProcedureName = aProcedureBase.ProcedureName;
                        cCheckTask.TaskState = (int?)CheckTaskState.NotOnline;
                        cCheckTask.ProcedureCode = apsProcedureTaskDetail.ProcedureCode;

                        //原因
                        cCheckTask.CheckReason = (int?)CheckReason.LastProcedure;
                        cCheckTask.CheckType = (int?)CheckType.ThreeCoordinate;
                        cCheckTask.IsAvailable = true;
                        cCheckTask.CreateTime = context.GetServerDate();
                        cCheckTask.CreatorID = _staffId;
                        cCheckTask.LastModifiedTime = context.GetServerDate();
                        cCheckTask.ModifierID = _staffId;

                        //分配任务给质检人员
                        cCheckTask.WorkerCode = DistributeCheckTask().StaffCode + ',';

                        context.C_CheckTask.Add(cCheckTask);

                        // var mapperConfiguration1 = new MapperConfiguration(cfg =>
                        //     cfg.CreateMap<C_CheckTask, C_CheckTask>());
                        // var mapper1 = mapperConfiguration1.CreateMapper();
                        // var cCheckTask1 = mapper1.Map<C_CheckTask>(cCheckTask);
                        // cCheckTask1.CheckType = 3;
                        //
                        // context.C_CheckTask.Add(cCheckTask1);
                    }
                }
                context.SaveChanges();
            }
        }

        private C_StaffBaseInformation DistributeCheckTask(string SkillType = "三坐标")
        {
            using (var context = new Model())
            {
                //在人员基础信息表里查找三坐标和质检两类人员  这里生成的是三坐标任务 所以检索三坐标人员
                var staffBaseInformation = context.C_StaffBaseInformation
                    .Where(s => s.SkillType == SkillType && s.IsAvailable == true).OrderBy(s => s.Reserve1).FirstOrDefault();
                if (staffBaseInformation.Reserve1 == null)
                {
                    staffBaseInformation.Reserve1 = 0;
                }
                staffBaseInformation.Reserve1 = staffBaseInformation.Reserve1 + 1;
                context.SaveChanges();
                return staffBaseInformation;
            }
        }

        private bool IsCheckResult()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据 
                _cProductProcessing =
                    context.C_ProductProcessing.Where(s => s.ProductBornCode == _productBornCode)
                        .OrderByDescending(s => s.OnlineTime).FirstOrDefault();
                //在产品加工档案表中 根据设备号/计划号/产品id/工序号/下线时间排序 获取紧前产品
                var cProductProcessingDocument = context.C_ProductProcessingDocument.Where(s => s.EquipmentID.ToString() == _equipmentId &&
                                                                                                s.ProductID == _cProductProcessing.ProductID && s.ProcedureID == _cProductProcessing
                                                                                                    .ProcedureID && s.PlanID == _cProductProcessing.PlanID
                ).OrderByDescending(s => s.OfflineTime).FirstOrDefault();
                if (cProductProcessingDocument != null)
                {
                    var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                        s.ProductBornCode == cProductProcessingDocument.ProductBornCode && s.ProcedureCode == cProductProcessingDocument.ProcedureCode
                        && s.IsAvailable == true && s.EquipmentID == _cProductProcessing.EquipmentID&&s.TaskState==(decimal?) ApsProcedureTaskDetailState.Completed
                    );
                    //如果是送检的话
                    if (apsProcedureTaskDetail.IsInspect == 1)
                    {
                        //在产品检验档案表中根据出生证/检验类型(三坐标)/工序编号/计划号/设备编号 获取产品检验档案数据
                        var cCheckProcessingDocument = context.C_CheckProcessingDocument.FirstOrDefault(s =>
                            s.ProductBornCode == cProductProcessingDocument.ProductBornCode && s.CheckType == (decimal?)CheckType.ThreeCoordinate
                             && s.ProcedureCode == cProductProcessingDocument.ProcedureCode &&
                            s.PlanID == cProductProcessingDocument.PlanID);

                        if (cCheckProcessingDocument.Offline_type == null)
                        {
                            FrmDialog.ShowDialog(this, "紧前产品送检结果未生成", "提示");
                            return false;
                        }

                        // //在产品质量数据表中判断是否有结果了(这里要检索的是质检类型为三坐标的质检任务是否有结果了)
                        // var cProductQualityDatas = context.C_ProductQualityData.Where(s =>
                        //     s.ProductBornCode == cProductProcessingDocument.ProductBornCode && s.ProductID == _cProductProcessing.ProductID
                        //     && s.PlanID == _cProductProcessing.PlanID && s.ProcedureID == _cProductProcessing.ProcedureID && s.CheckType == (int?)CheckType.ThreeCoordinate).ToList();
                        // if (cProductQualityDatas.Any())
                        // {
                        //     foreach (var cProductQualityData in cProductQualityDatas)
                        //     {
                        //         if (cProductQualityData.CheckResult == null)
                        //         {
                        //             FrmDialog.ShowDialog(this, "紧前产品送检结果未生成", "提示");
                        //             return false;
                        //         }
                        //     }
                        // }
                        // else
                        // {
                        //     //如果为空 , 就说明该工序的三坐标质检任务没有结果 
                        //     FrmDialog.ShowDialog(this, "紧前产品送检结果未生成", "提示");
                        //     return false;
                        // }
                    }

                    // //如果不送检的话 就要判断是否有不良下机
                    // if (cProductProcessingDocument.Offline_type == 3)
                    // {
                    //     //在产品质量数据表中判断是否有结果了
                    //     var cProductQualityData = context.C_ProductQualityData.First(s =>
                    //         s.ProductBornCode == cProductProcessingDocument.ProductBornCode);
                    //     if (cProductQualityData.CheckResult == null)
                    //     {
                    //         FrmDialog.ShowDialog(this, "紧前产品送检结果未生成", "提示");
                    //         return false;
                    //     }
                    // }
                    //
                    // //在检验档案表里 根据紧前产品的工序号/下线时间排序获得检验数据
                    // var cCheckProcessingDocument = context.C_CheckProcessingDocument.Where(s =>
                    //         s.ProductBornCode == cProductProcessingDocument.ProductBornCode
                    //         && s.ProcedureCode == cProductProcessingDocument.ProcedureCode)
                    //     .OrderByDescending(s => s.OfflineTime).FirstOrDefault();
                    // //如果不送检的话 就要判断是否有返修
                    // if (cCheckProcessingDocument != null)
                    // {
                    //     if (cCheckProcessingDocument.Offline_type == 3)
                    //     {
                    //         //在产品质量数据表中判断是否有结果了
                    //         var cProductQualityData = context.C_ProductQualityData.First(s =>
                    //             s.ProductBornCode == cProductProcessingDocument.ProductBornCode);
                    //         if (cProductQualityData.CheckResult == null)
                    //         {
                    //             FrmDialog.ShowDialog(this, "紧前产品送检结果未生成", "提示");
                    //             return false;
                    //         }
                    //     }
                    // }
                    return true;
                }
                return true;
            }
        }
    }
}
