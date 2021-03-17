using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using HZH_Controls;
using HZH_Controls.Controls;
using HZH_Controls.Forms;
using MachineryProcessingDemo.helper;
using Microsoft.Extensions.Configuration;
using QualityCheckDemo;

namespace MachineryProcessingDemo.Forms
{
    public partial class MainPanel : FrmBase
    {
        public MainPanel(long? staffId, string staffCode, string staffName)
        {
            InitializeComponent();
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            EmployeeIDTxt.Text = staffCode;
            EmployeeNameTxt.Text = staffName;
        }
        private static int a = 0;
        private static long? _staffId;
        private static string _staffCode;
        private static string _staffName;
        private static C_ProductProcessing _cProductProcessing;
        private static string _workshopId;
        private static string _workshopCode;
        private static string _workshopName;
        private static string _equipmentId;
        private static string _equipmentCode;
        private static string _equipmentName;
        //图标间宽度
        private static int _widthX = 400;
        //tuple计数器
        private static int _tupleI = 0;
        //全局静态只读tuple
        private static readonly List<Tuple<string, string>> MuneList = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("工量具", "E_icon_tools"),
            // new Tuple<string, string>("工量具", "A_fa_wrench"),
            // new Tuple<string, string>("扫描枪状态", "A_fa_check_circle"),
            new Tuple<string, string>("扫描枪状态", "E_icon_check_alt2"),
            // new Tuple<string, string>("上线", "E_arrow_carrot_2up_alt"),
            // new Tuple<string, string>("程序文件", "A_fa_file_text"),
            new Tuple<string, string>("程序文件", "A_fa_list_alt"),
            new Tuple<string, string>("作业指导", "A_fa_github"),
            // new Tuple<string, string>("自检项录入", "A_fa_edit"),
            // new Tuple<string, string>("自检项录入", "E_icon_pencil_edit"),
            // new Tuple<string, string>("下线", "E_arrow_carrot_2down_alt2"),
            new Tuple<string, string>("强制下线", "A_fa_stack_overflow"),
            new Tuple<string, string>("强制下线", "E_icon_error_circle_alt"),
            new Tuple<string, string>("切换账号", "E_arrow_left_right_alt"),
            new Tuple<string, string>("退出", "A_fa_power_off"),
            // new Tuple<string, string>("人员信息", "A_fa_address_card_o"),
        };

        private void MainPanel_Load(object sender, EventArgs e)
        {
            //var addXmlFile = new ConfigurationBuilder().SetBasePath("E:\\project\\visual Studio Project\\MachineryProcessingDemo")
            var addXmlFile = new ConfigurationBuilder().SetBasePath(GlobalClass.Xml)
                .AddXmlFile("config.xml");
            var configuration = addXmlFile.Build();
            _workshopId = configuration["WorkshopID"];
            _workshopCode = configuration["WorkshopCode"];
            _workshopName = configuration["WorkshopName"];
            _equipmentId = configuration["EquipmentID"];
            _equipmentCode = configuration["EquipmentCode"];
            _equipmentName = configuration["EquipmentName"];

            //使用hzh控件自带的图标库 tuple
            //解析tuple 加载顶部菜单栏 绑定事件
            var workersGaugeLabel = GenerateLabel();
            workersGaugeLabel.Click += OpenWorkerGauge;

            var scanGunStateLabel = GenerateLabel();
            scanGunStateLabel.DoubleClick += lbl_DoubleClick;

            // var onlineLabel = GenerateLabel();
            // onlineLabel.Click += OpenScanOnlineForm;

            var programFilesLabel = GenerateLabel();
            programFilesLabel.Click += OpenProgramFile;

            var workInstructionLabel = GenerateLabel();
            workInstructionLabel.Click += OpenWorkInstruction;

            // var selfCheckItemInputLabel = GenerateLabel();
            // selfCheckItemInputLabel.Click += OpenSelfCheckItemForm;

            // var offlineLabel = GenerateLabel();
            // offlineLabel.Click += OpenScanOfflineForm;

            var forceOfflineLabel = GenerateLabel();
            forceOfflineLabel.Click += OpenForceOfflineForm;

            var forceOfflineLabel1 = GenerateLabel();
            forceOfflineLabel1.Click += OpenForceOfflineForm;

            var switchAccountLabel = GenerateLabel();
            switchAccountLabel.Click += OpenLoginForm;

            var exitLabel = GenerateLabel();
            exitLabel.Click += CloseForms;

            // 加载人员信息图标
            var tuple1 = new Tuple<string, string>("人员信息", "A_fa_address_card_o");
            var icon1 = (FontIcons)Enum.Parse(typeof(FontIcons), tuple1.Item2);
            var pictureBox1 = new PictureBox
            {
                AutoSize = false,
                Size = new Size(240, 160),
                ForeColor = Color.FromArgb(255, 77, 59),
                Image = FontImages.GetImage(icon1, 64, Color.FromArgb(255, 77, 59)),
                Location = new Point(110, 20)
            };
            PersonnelInfoPanel.Controls.Add(pictureBox1);


            //修改自定义控件label.text文本
            CompletedTask.label1.Text = " 已完成任务";
            ProductionTaskQueue.label1.Text = "生产任务队列";

            InitialDidTasks();

            ucSignalLamp1.LampColor = new Color[] { Color.Green };
            ucSignalLamp2.LampColor = new Color[] { Color.Red };

            InialToDoTasks();

            DrawLabel();

            //读取配置文件里的数据库连接字符串
            // var builder = new ConfigurationBuilder().SetBasePath("E:\\project\\visual Studio Project\\MachineryProcessingDemo").AddJsonFile("Setting.json");
            // var configuration = builder.Build();
            // var configurationSection = configuration.GetSection("_EquipmentID");
            // var s1 = configurationSection["key1"];

            //获取当前加工中心的生产任务(已上线)
            using (var context = new Model())
            {
                var cProductProcessing = context.C_ProductProcessing
                    .FirstOrDefault(s => s.WorkshopCode == _workshopCode && s.OnlineTime != null);
                if (cProductProcessing != null)
                {
                    var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                        s.ProductBornCode == cProductProcessing.ProductBornCode && s.IsAvailable == true);

                    if (apsProcedureTaskDetail != null && apsProcedureTaskDetail.ProcedureType == (decimal?)ProcedureType.Tooling)
                    {
                        ProductionStatusInfoPanel.Controls.Find("control003", false).First().Hide();
                        var control = ProductionStatusInfoPanel.Controls.Find("control002", false).First();
                        control.Click -= SelfCheckItemEvent;
                        control.Click += ProductOfflineEvent;
                        control.Text = "产品下线";
                        ProductionStatusInfoPanel.Controls[1].Dispose();
                    }
                    ProductIDTxt.Text = cProductProcessing.ProductBornCode;
                    ProductIDTxt.ReadOnly = true;
                    ProductNameTxt.Text = cProductProcessing.ProductName;
                    ProductNameTxt.ReadOnly = true;
                    CurrentProcessTxt.Text = cProductProcessing.ProcedureName;
                    CurrentProcessTxt.ReadOnly = true;
                    OnlineTimeTxt.Text = cProductProcessing.OnlineTime.ToString();
                    OnlineTimeTxt.ReadOnly = true;
                    ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                        Color.MediumSeaGreen;
                }

                if (!string.IsNullOrEmpty(ProductIDTxt.Text))
                {
                    //在产品加工过程表中根据产品出生证  获取元数据
                    _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());
                }
            }

            //初始化判断自检项完成与否
            SelfCheckColorJudge();

            timer1.Enabled = true;
        }

        //打开工量具配置界面
        private void OpenWorkerGauge(object sender, EventArgs e)
        {
            var workerGauge = new WorkerGauge(_staffId, _staffCode, _staffName);
            workerGauge.Location = new Point(panel10.Width / 2 - workerGauge.Width / 2, 10);
            workerGauge.FormBorderStyle = FormBorderStyle.None;
            workerGauge.AutoSize = false;
            workerGauge.AutoScaleMode = AutoScaleMode.None;
            workerGauge.AutoScaleMode = AutoScaleMode.Font;
            workerGauge.TopLevel = false;
            workerGauge.BackColor = Color.FromArgb(200, 200, 247);
            workerGauge.Name = "workerGauge";
            //查到面板里有没有扫码上线窗体,如果有的话就dispose掉防止串口占用
            var find = panel10.Controls.Find("scanOnlineForm", false).FirstOrDefault();
            find?.Dispose();

            panel10.Controls.Clear();

            panel10.Controls.Add(workerGauge);
            workerGauge.Show();
        }

        private void DrawLabel()
        {
            ProductionStatusInfoPanel.Controls.Clear();

            // 加载箭头图标
            var tuple2 = new Tuple<string, string>("Arrow", "A_fa_arrow_down");
            var icon2 = (FontIcons)Enum.Parse(typeof(FontIcons), tuple2.Item2);
            int localY = 72;
            for (var i = 0; i < 2; i++)
            {
                ProductionStatusInfoPanel.Controls.Add(new PictureBox()
                {
                    AutoSize = false,
                    Size = new Size(40, 40),
                    ForeColor = Color.FromArgb(255, 77, 59),
                    Image = FontImages.GetImage(icon2, 40, Color.FromArgb(255, 77, 59)),
                    Location = new Point(270, localY)
                });
                localY += 98;
            }

            //初始化生产状态信息面板
            using (var context = new Model())
            {
                //这里需要配置修改xml
                var cBBdbRCntlPntBases = context.C_BBdbR_CntlPntBase.Where(s =>
                        s.CntlPntTyp == "1" && s.Enabled == "1")
                    .OrderBy(s => s.CntlPntSort).ToList();

                int localLblY = 25;
                foreach (var cBBdbRCntlPntBase in cBBdbRCntlPntBases)
                {
                    var label = new Label()
                    {
                        Location = new Point(239, localLblY),
                        Size = new Size(112, 39),
                        Name = cBBdbRCntlPntBase.CntlPntCd,
                        BackColor = Color.LightSlateGray,
                        Font = new Font("微软雅黑", 10.8F, FontStyle.Bold,
                            GraphicsUnit.Point, ((byte)(134))),
                        Text = cBBdbRCntlPntBase.CntlPntNm,
                        TextAlign = ContentAlignment.MiddleCenter,
                    };
                    if (label.Name.Equals("control001"))
                    {
                        label.Click += ProductOnlineEvent;
                    }
                    else if (label.Name.Equals("control002"))
                    {
                        label.Click += SelfCheckItemEvent;
                    }
                    else if (label.Name.Equals("control003"))
                    {
                        label.Click += ProductOfflineEvent;
                    }
                    ProductionStatusInfoPanel.Controls.Add(label);
                    localLblY += 96;
                }
            }
        }

        private void OpenForceOfflineForm(object sender, EventArgs e)
        {
            panel10.Controls.Clear();
            if (string.IsNullOrEmpty(ProductIDTxt.Text))
            {
                FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                return;
            }
            var dialogResult = FrmDialog.ShowDialog(this,
                $"确定将产品[{ProductIDTxt.Text.Trim()}]强制下线吗,一旦强制下线将不可撤回,请慎重操作", "强制下线", true);
            if (dialogResult == DialogResult.OK)
            {
                //不判断自检项是否必填 如果存在自检项控制点就直接他喵的转档
                SelfItemCntLogicTurn("强制下线类型");

                //添加下线的逻辑控制点
                AddCntLogicProOffline("强制下线类型");

                //apsdetail状态修改以及完善apsdetail最后修改时间呀什么的
                PerfectApsDetail();

                //控制点转档
                OfflineCntLogicTurn("强制下线类型");

                //完善产品质量数据表
                PerfectProductQD();

                //修改aps工序任务表里的信息,例如修改完成状态,更新执行进度
                PerfectApsProcedureTask();

                if (IsLastProcedureEeceptQC())
                {
                    //如果是除了质检外的末道工序 生成产品档案表 师兄,末道工序转到产品档案表里是指的是机加工序的末道工序还是整个产品而言的末道工序???
                    GenerateProductDoc();
                }

                //如果被收录进首件记录表, 则需要删掉
                DelFirstRecord();

                //产品加工过程表转档
                ProductProcessingDocTurn();

                ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                    Color.OrangeRed;
                InialToDoTasks();
                InitialDidTasks();
            }
        }

        private void DelFirstRecord()
        {
            using (var context = new Model())
            {
                //在首件记录过程表中根据计划号/产品号/产品出生证/工序号/设备号/首件状态 判断是否存在数据
                var cProcedureFirstRecords = context.C_ProcedureFirstRecord.FirstOrDefault(s =>
                    s.PlanID == _cProductProcessing.PlanID && s.ProductID == _cProductProcessing.ProductID &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                    s.EquipmentID == _cProductProcessing.EquipmentID && s.ProcedureFirstStatus == 0);
                //存在就删掉,不存在就算了
                if (cProcedureFirstRecords != null)
                {
                    context.C_ProcedureFirstRecord.Remove(cProcedureFirstRecords);
                    context.SaveChanges();
                }
            }
        }
        //如果是整个生产环节的末道工序的话,就要转到产品档案表中
        private void GenerateProductDoc()
        {
            using (var context = new Model())
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<C_ProductProcessing, C_ProductDocument>());
                var mapper = mapperConfiguration.CreateMapper();
                var cProcedureFirstDocument = mapper.Map<C_ProductDocument>(_cProductProcessing);

                //需要修改   正常/报废
                cProcedureFirstDocument.Type = -1;
                cProcedureFirstDocument.OfflineTime = context.GetServerDate(); ;
                cProcedureFirstDocument.IsAvailable = true;
                cProcedureFirstDocument.CreateTime = context.GetServerDate();
                cProcedureFirstDocument.CreatorID = _staffId;
                cProcedureFirstDocument.LastModifiedTime = context.GetServerDate();
                cProcedureFirstDocument.ModifierID = _staffId;
                //在产品加工档案表中根据产品出生证/设备编号/计划号 按下线时间排序 获得该产品首道工序上线时间
                var cProductProcessingDocument = context.C_ProductProcessingDocument.Where(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim()
                                                                                                && s.EquipmentID == _cProductProcessing.EquipmentID && s.PlanCode == _cProductProcessing.PlanCode
                                                                                                && s.PlanID == _cProductProcessing.PlanID)
                    .OrderBy(s => s.OfflineTime).FirstOrDefault();

                cProcedureFirstDocument.OnlineTime = cProductProcessingDocument.OnlineTime;
                context.C_ProductDocument.Add(cProcedureFirstDocument);
                context.SaveChanges();
            }
        }
        //判断是否是除质检外的末道工序
        private bool IsLastProcedureEeceptQC()
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
        //产品加工档案表转档(强制下机部分)
        private void ProductProcessingDocTurn()
        {
            using (var context = new Model())
            {
                _cProductProcessing.OfflineStaffID = _staffId;
                _cProductProcessing.OfflineStaffCode = _staffCode;
                _cProductProcessing.OfflineStaffName = _staffName;
                _cProductProcessing.Offline_type = (int?)ProductProcessingOfflineType.Force; //强制下机
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
        //完善aps工序任务表 进度/修改人/修改时间/状态
        private void PerfectApsProcedureTask()
        {
            using (var context = new Model())
            {
                //在工序任务表里 根据订单号/计划号/项目号/产品号/工序号/设备号获得元数据
                var apsProcedureTask = context.APS_ProcedureTask.First(s =>
                    s.OrderID == _cProductProcessing.OrderID && s.PlanCode == _cProductProcessing.PlanCode &&
                    s.ProjectCode == _cProductProcessing.ProjectCode &&
                    s.ProductCode == _cProductProcessing.ProductCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode &&
                    s.EquipmentID == _cProductProcessing.EquipmentID);

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
                    apsProcedureTask.TaskState = 1; //已完成呜呜呜
                }

                context.SaveChanges();
            }
        }
        //完善产品质量数据表信息  下线人员姓名/编号/id
        private void PerfectProductQD()
        {
            using (var context = new Model())
            {
                var cProductQualityDatas = context.C_ProductQualityData.Where(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.PlanID == _cProductProcessing.PlanID && s.OfflineStaffID == null && s.ProcedureCode == _cProductProcessing.ProcedureCode).ToList();
                if (cProductQualityDatas.Any())
                {
                    foreach (var cProductQualityData in cProductQualityDatas)
                    {
                        cProductQualityData.OfflineStaffCode = _staffCode;
                        cProductQualityData.OfflineStaffID = _staffId;
                        cProductQualityData.OfflineStaffName = _staffName;
                    }
                    context.SaveChanges();
                }
            }
        }
        //产品下线逻辑转档(强制下线)
        private void OfflineCntLogicTurn(string remark = "")
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
                    cBWuECntlLogicPros[0].Remarks = remark;

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
        //完善apsdetail表 任务状态/修改人/修改时间
        private void PerfectApsDetail()
        {
            using (var context = new Model())
            {
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.EquipmentID == _cProductProcessing.EquipmentID &&
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode && s.IsAvailable == true &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.InExcecution);
                apsProcedureTaskDetail.TaskState = (int?)ApsProcedureTaskDetailState.Completed;//已完成
                apsProcedureTaskDetail.ModifierID = _staffId.ToString();
                apsProcedureTaskDetail.LastModifiedTime = context.GetServerDate();
                context.Entry(apsProcedureTaskDetail).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        //初始化未完成任务列表

        public void InialToDoTasks()
        {
            //  自定义表格 装载图片等资源
            List<DataGridViewColumnEntity> lstColumns1 = new List<DataGridViewColumnEntity>
            {
                new DataGridViewColumnEntity()
                {
                    DataField = "ProductBornCode", HeadText = "产品出生证", Width = 30, WidthType = SizeType.Percent
                },
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve2", HeadText = "工序名称", Width = 15, WidthType = SizeType.Percent
                },
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve3", HeadText = "预计开始时间", Width = 30, WidthType = SizeType.Percent,
                },
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve1", HeadText = "工序状态", Width = 25, WidthType = SizeType.Percent
                }
            };
            ucDataGridView2.Columns = lstColumns1;
            ucDataGridView2.ItemClick += UcDataGridView2_ItemClick;

            //拿到待加工产品排序集合
            var apsProcedureTaskDetails = GetToDoProcedureTask();
            ucDataGridView2.DataSource = apsProcedureTaskDetails;
            ucDataGridView2.HeadTextColor = Color.DarkOrange; 
            
        }

        //初始化已完成任务列表
        public void InitialDidTasks()
        {
            // 自定义表格 装载图片等资源
            List<DataGridViewColumnEntity> lstColumns = new List<DataGridViewColumnEntity>
            {
                // new DataGridViewColumnEntity()
                // {
                //     Width = 10,
                //     WidthType = SizeType.Percent,
                //     CustomCellType = typeof(UCTestGridTable_CustomCellIcon),
                //     HeadText = "产品图片"
                // },
                new DataGridViewColumnEntity()
                {
                    DataField = "ProductBornCode", HeadText = "产品出生证", Width = 35, WidthType = SizeType.Percent
                },
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve2", HeadText = "工序名称", Width = 15, WidthType = SizeType.Percent
                },
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve1", HeadText = "自检结果", Width = 20, WidthType = SizeType.Percent
                } ,
                new DataGridViewColumnEntity()
                {
                    DataField = "Reserve3", HeadText = "工序状态", Width = 25, WidthType = SizeType.Percent
                }
            };

            var didProcedureTask = GetDidProcedureTask();
            // lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Age", HeadText = "年龄", Width = 50, WidthType = SizeType.Percent });
            // lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Birthday", HeadText = "生日", Width = 50, WidthType = SizeType.Percent, Format = (a) => { return ((DateTime)a).ToString("yyyy-MM-dd"); } });
            // lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Sex", HeadText = "性别", Width = 50, WidthType = SizeType.Percent, Format = (a) => { return ((int)a) == 0 ? "女" : "男"; } });
            // lstColumns.Add(new DataGridViewColumnEntity() { Width = 155, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCell) });
            ucDataGridView1.Columns = lstColumns;
            // this.ucDataGridView1.IsShowCheckBox = true;

            ucDataGridView1.DataSource = didProcedureTask;
        }

        //单击事件 产品上线
        private void UcDataGridView2_ItemClick(object sender, DataGridViewEventArgs e)
        {
            ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                Color.LightSlateGray;
            ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                Color.LightSlateGray;
            ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                Color.LightSlateGray;

            var controls = panel10.Controls.Find("scanOnlineForm", false);
            if (controls.Any())
            {
                controls[0].Dispose();
            }

            if (!HasExitProductTask())
            {
                ProductNameTxt.Clear();
                ProductIDTxt.Clear();
                CurrentProcessTxt.Clear();
                OnlineTimeTxt.Clear();

                var dataGridViewRow = ucDataGridView2.SelectRow;
                var dataSource = dataGridViewRow.DataSource;
                if (dataSource is APS_ProcedureTaskDetail apsProcedureTaskDetail)
                {
                    //如果是工装工序的话走这里
                    if (apsProcedureTaskDetail.ProcedureType == (decimal?)ProcedureType.Tooling)
                    {
                        if (a == 0)
                        {
                            ProductionStatusInfoPanel.Controls.Find("control003", false).First().Hide();
                            var control = ProductionStatusInfoPanel.Controls.Find("control002", false).First();
                            control.Click -= SelfCheckItemEvent;
                            control.Click += ProductOfflineEvent;
                            control.Text = "产品下线";
                            ProductionStatusInfoPanel.Controls[1].Hide();
                            a++;
                        }

                        var dialogResult1 = FrmDialog.ShowDialog(this, $"确定上线选中产品[{apsProcedureTaskDetail.ProductBornCode}]吗", "产品上线", true);
                        if (dialogResult1 == DialogResult.OK)
                        {
                            var scanOnlineForm = new ScanOnlineForm(_staffId, _staffCode, _staffName, apsProcedureTaskDetail.ProductBornCode, _workshopId, _workshopCode, _workshopName, _equipmentId, _equipmentCode, _equipmentName)

                            {
                                DisplayInfoToMainPanel = (s1, s2, s3, s4) =>
                                {
                                    ProductIDTxt.Text = s1;
                                    ProductNameTxt.Text = s2;
                                    CurrentProcessTxt.Text = s3;
                                    OnlineTimeTxt.Text = s4;
                                },
                                ChangeBgColor = () =>
                                {
                                    ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                                        Color.MediumSeaGreen;
                                    ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                                        Color.LightSlateGray;
                                }
                            };
                            if (scanOnlineForm.CheckTaskValidity(apsProcedureTaskDetail.ProcedureCode))
                            {
                                scanOnlineForm.AddCntLogicPro();
                                if (scanOnlineForm.WorkerConfirm())
                                {
                                    //转档  工序任务明细表=>产品加工过程表
                                    scanOnlineForm.ProcessTurnArchives();
                                    //完善工序任务明细表中的数据 诸如任务状态 ; 修改人修改时间
                                    scanOnlineForm.PerfectApsDetail();
                                    //完善计划产品出生证表
                                    scanOnlineForm.PerfectPlanProductInfo();
                                    //转档  
                                    scanOnlineForm.CntLogicTurn();
                                    FrmDialog.ShowDialog(this, $"产品{ProductIDTxt.Text}上线成功!", "上线成功");
                                    InialToDoTasks();
                                }
                            }
                        }
                        return;
                    }
                    //如果不是工装工序
                    if (a == 1)
                    {
                        ProductionStatusInfoPanel.Controls.Find("control003", false).First().Show();
                        var control = ProductionStatusInfoPanel.Controls.Find("control002", false).First();
                        control.Click += SelfCheckItemEvent;
                        control.Click -= ProductOfflineEvent;
                        control.Text = "自检项录入";
                        ProductionStatusInfoPanel.Controls[1].Show();
                        a = 0;
                    }
                    var dialogResult = FrmDialog.ShowDialog(this, $"确定上线选中产品[{apsProcedureTaskDetail.ProductBornCode}]吗", "产品上线", true);
                    if (dialogResult == DialogResult.OK)
                    {
                        var scanOnlineForm = new ScanOnlineForm(_staffId, _staffCode, _staffName, apsProcedureTaskDetail.ProductBornCode, _workshopId, _workshopCode, _workshopName, _equipmentId, _equipmentCode, _equipmentName)
                        {
                            DisplayInfoToMainPanel = (s1, s2, s3, s4) =>
                            {
                                ProductIDTxt.Text = s1;
                                ProductNameTxt.Text = s2;
                                CurrentProcessTxt.Text = s3;
                                OnlineTimeTxt.Text = s4;
                            },
                            ChangeBgColor = () =>
                            {
                                ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                                    Color.MediumSeaGreen;
                                ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                                    Color.LightSlateGray;
                                ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                                    Color.LightSlateGray;
                            }
                        };

                        if (scanOnlineForm.CheckTaskValidity(apsProcedureTaskDetail.ProcedureCode))
                        {
                            scanOnlineForm.AddCntLogicPro();
                            //先判断一下本产品出生证的有没有待检验的前序质检任务没做
                            var hasSelfQcTask = scanOnlineForm.HasSelfQcTask();
                            //如果确实有前序质检任务未完成, 就干死他
                            if (hasSelfQcTask)
                            {
                                return;
                            }

                            //判断首件
                            bool hasFirstRecord = scanOnlineForm.HasFirstRecord(out var abortMessage);
                            if (abortMessage.Equals("y"))
                            {
                                return;
                            }
                             if (abortMessage.Equals("y and change"))
                            {
                                FrmDialog.ShowDialog(this, "当前工序的产品加工中心已修改,请上线其他产品");
                                return;
                            }

                            //齐套确认
                            if (scanOnlineForm.KittingConfirm())
                            {
                                //操作人员确认
                                if (scanOnlineForm.WorkerConfirm())
                                {
                                    //转档  工序任务明细表=>产品加工过程表
                                    _cProductProcessing =  scanOnlineForm.ProcessTurnArchives();
                                    //判断是否抽检
                                    var isChecked = scanOnlineForm.IsChecked();
                                    if (!hasFirstRecord && isChecked)
                                    {
                                        scanOnlineForm.AddFirstRecord();
                                    }
                                    //如果有首件记录并且抽检为是的话
                                    else if ((hasFirstRecord && isChecked) || (!hasFirstRecord && !isChecked))
                                    {
                                        //获取紧前产品
                                        var cProductProcessingDocument = scanOnlineForm.GetPrevProduct();
                                        if (cProductProcessingDocument != null)
                                        {
                                            //根据紧前产品获得紧前产品的apsdetail
                                            var apsProcedureTaskDetail1 = scanOnlineForm.GetPrevApsDetail(cProductProcessingDocument);
                                            if (apsProcedureTaskDetail1.IsInspect == 1)
                                            {
                                                //apsdetail获得送检结果
                                                var cCheckProcessingDocument = scanOnlineForm.GetCheckResult(apsProcedureTaskDetail1);

                                                if (cCheckProcessingDocument == null)
                                                {
                                                    scanOnlineForm.BackProcessTurnArchives();
                                                    FrmDialog.ShowDialog(this, "该工序紧前产品未质检,请先执行质检任务!");
                                                    BeginInvoke(new Action((() =>
                                                    {
                                                        ProductIDTxt.Clear();
                                                        ProductIDTxt.ReadOnly = false;
                                                        ProductNameTxt.Clear();
                                                        ProductNameTxt.ReadOnly = false;
                                                        CurrentProcessTxt.Clear();
                                                        CurrentProcessTxt.ReadOnly = false;
                                                        OnlineTimeTxt.Clear();
                                                        OnlineTimeTxt.ReadOnly = false;
                                                        ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                                                            Color.LightSlateGray;
                                                    })));
                                                    return;
                                                }

                                                if (cCheckProcessingDocument != null)
                                                {
                                                    if (cCheckProcessingDocument.Offline_type == (int?)OfflineType.NG)
                                                    {
                                                        bool changeEquipmentOrNot = scanOnlineForm.ChangeEquipmentOrNot();
                                                        //如果确实要改变加工中心的话 , 就不做下面的任何操作了
                                                        if (changeEquipmentOrNot)
                                                        {
                                                            //可能还要做什么修改aps表,修改apsdetail表, 改那个设备编号什么的
                                                            //另外还要删除上面做的任何一步,例如删除控制点的添加/删除产品加工过程表里的该元组
                                                            DelProductProcessing();
                                                            DelCntPointProcessing();
                                                            ClearTextAndResetLabelColor();
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //完善工序任务明细表中的数据 诸如任务状态 ; 修改人修改时间
                                    scanOnlineForm.PerfectApsDetail();
                                    //完善计划产品出生证表
                                    scanOnlineForm.PerfectPlanProductInfo();
                                    //转档  
                                    scanOnlineForm.CntLogicTurn();
                                    FrmDialog.ShowDialog(this, $"产品{ProductIDTxt.Text}上线成功!", "上线成功");
                                    InialToDoTasks();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                    Color.MediumSeaGreen;
                SelfCheckColorJudge();
            }
        }

        private void ClearTextAndResetLabelColor()
        {
            BeginInvoke(new Action((() =>
            {
                ProductIDTxt.Clear();
                ProductIDTxt.ReadOnly = false;
                ProductNameTxt.Clear();
                ProductNameTxt.ReadOnly = false;
                CurrentProcessTxt.Clear();
                CurrentProcessTxt.ReadOnly = false;
                OnlineTimeTxt.Clear();
                OnlineTimeTxt.ReadOnly = false;
                ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                    Color.LightSlateGray;
            }))); 
        }

        private void DelCntPointProcessing()
        {
            using (var context = new Model())
            {
                var cBWuECntlLogicPro = context.C_BWuE_CntlLogicPro.FirstOrDefault(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode);
                context.C_BWuE_CntlLogicPro.Remove(cBWuECntlLogicPro);
                context.SaveChanges(); 
            }
        }

        private void DelProductProcessing()
        {
            using (var context = new Model())
            {
                context.Entry(_cProductProcessing).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private List<APS_ProcedureTaskDetail> GetDidProcedureTask()
        {
            var procedureTaskDetails = new List<APS_ProcedureTaskDetail>();
            using (var context = new Model())
            {
                //在工序任务表中根据设备编号和 开始时间排序得到优先级最高的工序任务
                var apsProcedureTasks = context.APS_ProcedureTask
                    .Where(s => s.EquipmentID.ToString() == _equipmentId && s.IsAvailable == true)
                    .OrderBy(s => s.StartTime).ToList();

                foreach (var apsProcedureTask in apsProcedureTasks)
                {
                    //在工序明细表中 根据tasktableid/设备号/工序号/有效性/任务状态 获取已完成的任务集合(排序)
                    var apsProcedureTaskDetails = context.APS_ProcedureTaskDetail.Where(s =>
                        s.EquipmentID == apsProcedureTask.EquipmentID &&
                        s.ProcedureCode == apsProcedureTask.ProcedureCode && s.IsAvailable == true &&
                        s.TaskTableID == apsProcedureTask.ID &&
                        s.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed)
                        .OrderBy(s => s.LastModifiedTime);

                    foreach (var apsProcedureTaskDetail in apsProcedureTaskDetails)
                    {
                        //令apsdetail的reserve2 字段作为工序名称来搞
                        //在产品工序基础表中根据产品号/有效性  获得工序名称
                        apsProcedureTaskDetail.Reserve2 = context.A_ProductProcedureBase.Where(s =>
                                s.IsAvailable == true && s.ProductID == apsProcedureTask.ProductID &&
                                s.PlanCode == apsProcedureTask.PlanCode &&
                                s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode).Select(s => s.ProcedureName)
                            .FirstOrDefault();

                        var cProductProcessingDocument = context.C_ProductProcessingDocument.FirstOrDefault(s =>
                            s.ProductBornCode == apsProcedureTaskDetail.ProductBornCode &&
                            s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode);

                        apsProcedureTaskDetail.Reserve1 =
                            cProductProcessingDocument.Offline_type == (decimal?)ProductProcessingOfflineType.Normal
                                ? "正常下机" : cProductProcessingDocument.Offline_type == (decimal?)ProductProcessingOfflineType.Force
                                    ? "强制下机" : "不良下机";

                        if (apsProcedureTaskDetail.IsInspect == 1)
                        {
                            //获得三坐标检验任务
                            var cCheckTask = context.C_CheckTask.FirstOrDefault(s =>
                                s.ProductBornCode == apsProcedureTaskDetail.ProductBornCode &&
                                s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode && s.IsAvailable == true);
                            if (cCheckTask == null)
                            {
                                apsProcedureTaskDetail.Reserve3 = "加工完毕";
                                continue;
                            }
                            apsProcedureTaskDetail.Reserve3 =
                                cCheckTask.TaskState == (decimal?)CheckTaskState.NotOnline ? "待送检(三坐标)" :
                                cCheckTask.TaskState == (decimal?)CheckTaskState.InExecution ? "正在送检(三坐标)" :
                                "送检完毕(三坐标)";
                        }
                        else
                        {
                            apsProcedureTaskDetail.Reserve3 = "加工完毕";
                        }
                    }
                    procedureTaskDetails.AddRange(apsProcedureTaskDetails);
                }
            }
            return procedureTaskDetails;
        }

        private List<APS_ProcedureTaskDetail> GetToDoProcedureTask()
        {
            var procedureTaskDetails = new List<APS_ProcedureTaskDetail>();
            using (var context = new Model())
            {
                //在工序任务表中根据设备编号和任务状态和 开始时间排序得到优先级最高的工序任务
                var apsProcedureTasks = context.APS_ProcedureTask
                    .Where(s => s.EquipmentID.ToString() == _equipmentId &&
                                s.TaskState == (decimal?)ApsProcedureTaskState.ToDo && s.IsAvailable == true)
                    .OrderBy(s => s.StartTime).ToList();
                foreach (var apsProcedureTask in apsProcedureTasks)
                {
                    //在工序任务明细表中 根据tasktableid/设备号/工序号/有效性/任务状态 获取待完成的任务集合(排序)
                    var apsProcedureTaskDetails = context.APS_ProcedureTaskDetail.Where(s =>
                            s.EquipmentID == apsProcedureTask.EquipmentID &&
                            s.ProcedureCode == apsProcedureTask.ProcedureCode && s.TaskState != (decimal?)ApsProcedureTaskDetailState.Completed &&
                            s.IsAvailable == true && s.TaskTableID == apsProcedureTask.ID).OrderByDescending(s => s.TaskState)
                        .ToList();

                    foreach (var apsProcedureTaskDetail in apsProcedureTaskDetails)
                    {
                        //reserve3 作为转换后的日期格式显示
                        if (apsProcedureTaskDetail.CreateTime != null)
                            apsProcedureTaskDetail.Reserve3 = apsProcedureTaskDetail.CreateTime.Value.ToString("g");

                        //令apsdetail的reserve2 字段作为工序名称来搞
                        //在产品工序基础表中根据产品号/有效性  获得工序名称
                        apsProcedureTaskDetail.Reserve2 = context.A_ProductProcedureBase.Where(s =>
                                s.IsAvailable == true && s.ProductID == apsProcedureTask.ProductID &&
                                s.PlanCode == apsProcedureTask.PlanCode &&
                                s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode).Select(s => s.ProcedureName)
                            .FirstOrDefault();

                        //判断当前产品当前工序的状态 待加工/正在加工/加工完毕/待送检(三坐标/手检)/正在送检(三坐标/手检)/送检完毕(三坐标/手检)
                        if (apsProcedureTaskDetail.TaskState == (decimal?)ApsProcedureTaskDetailState.NotOnline)
                        {
                            apsProcedureTaskDetail.Reserve1 = "待加工";
                        }
                        else if (apsProcedureTaskDetail.TaskState ==
                                 (decimal?)ApsProcedureTaskDetailState.InExcecution)
                        {
                            apsProcedureTaskDetail.Reserve1 = "正在加工";
                        }
                        else if (apsProcedureTaskDetail.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed)
                        {
                            if (apsProcedureTaskDetail.IsInspect != 1)
                            {
                                apsProcedureTaskDetail.Reserve1 = "加工完毕";
                            }
                            else
                            {
                                //获得三坐标检验任务
                                var cCheckTask = context.C_CheckTask.FirstOrDefault(s =>
                                    s.ProductBornCode == apsProcedureTaskDetail.ProductBornCode &&
                                    s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode && s.IsAvailable == true);
                                apsProcedureTaskDetail.Reserve1 =
                                    cCheckTask.TaskState == (decimal?)CheckTaskState.NotOnline ? "待送检(三坐标)" :
                                    cCheckTask.TaskState == (decimal?)CheckTaskState.InExecution ? "正在送检(三坐标)" :
                                    "送检完毕(三坐标)";
                            }
                        }
                    }
                    procedureTaskDetails.AddRange(apsProcedureTaskDetails);
                }
            }
            return procedureTaskDetails;
        }
        //自检项是否已完成
        private void SelfCheckColorJudge()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());
                if (_cProductProcessing != null)
                {
                    if (context.C_ProductQualityData.Any(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim() && s.ProcedureCode ==
                                                                      _cProductProcessing.ProcedureCode
                                                                      && s.CheckType == (decimal?)CheckType.SelfCheck && s.CollectValue != null))
                    {
                        ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor
                            = Color.MediumSeaGreen;
                    }
                }
            }
        }
        //判断是否存在加工任务
        private bool HasExitProductTask()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据加工中心编号和上线时间非空  判断本加工中心是否有正在处理的生产任务 
                var cProductProcessing = context.C_ProductProcessing
                    .FirstOrDefault(s => s.WorkshopCode == _workshopCode && s.OnlineTime != null);
                if (cProductProcessing != null)
                {
                    FrmDialog.ShowDialog(this, "当前已有正在处理的生产任务,请完成", "已有生产任务");
                    return true;
                }
                return false;
            }
        }

        //产品上线标签点击事件
        private void ProductOnlineEvent(object sender, EventArgs e)
        {
            panel10.Controls.Find("workerGauge",false).FirstOrDefault()?.Dispose();
            var find = panel10.Controls.Find("scanOnlineForm", false);
            if (find.Any())
            {
                return;
            }
            ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                Color.LightSlateGray;
            ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                Color.LightSlateGray;
            ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                Color.LightSlateGray;
            var exitProductTask = HasExitProductTask();
            if (!exitProductTask)
            {
                ProductNameTxt.Clear();
                ProductIDTxt.Clear();
                CurrentProcessTxt.Clear();
                OnlineTimeTxt.Clear();
                var scanOnlineForm = new ScanOnlineForm(_staffId, _staffCode, _staffName, _workshopId, _workshopCode, _workshopName, _equipmentId, _equipmentCode, _equipmentName)
                {
                    DisplayInfoToMainPanel = (s1, s2, s3, s4) =>
                    {
                        ProductIDTxt.Text = s1;
                        ProductNameTxt.Text = s2;
                        CurrentProcessTxt.Text = s3;
                        OnlineTimeTxt.Text = s4;
                    },
                    ChangeBgColor = () =>
                    {
                        ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                                Color.MediumSeaGreen;
                        ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                            Color.LightSlateGray;
                        ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                            Color.LightSlateGray;
                    },
                    RegetProcedureTasksDetails = InialToDoTasks,
                    ClearMainPanelTxt = () =>
                     {
                         ProductIDTxt.Clear();
                         ProductIDTxt.ReadOnly = false;
                         ProductNameTxt.Clear();
                         ProductNameTxt.ReadOnly = false;
                         CurrentProcessTxt.Clear();
                         CurrentProcessTxt.ReadOnly = false;
                         OnlineTimeTxt.Clear();
                         OnlineTimeTxt.ReadOnly = false;
                         ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                             Color.LightSlateGray;
                     }
                };
                var controls = scanOnlineForm.Controls.Find("lblTitle", false).First();
                controls.Visible = false;
                scanOnlineForm.Location = new Point(panel10.Width / 2 - scanOnlineForm.Width / 2, 0);
                scanOnlineForm.FormBorderStyle = FormBorderStyle.None;
                scanOnlineForm.AutoSize = false;
                scanOnlineForm.AutoScaleMode = AutoScaleMode.None;
                scanOnlineForm.Size = new Size(553, panel10.Height);
                scanOnlineForm.AutoScaleMode = AutoScaleMode.Font;
                scanOnlineForm.TopLevel = false;
                scanOnlineForm.BackColor = Color.FromArgb(247, 247, 247);
                scanOnlineForm.ForeColor = Color.FromArgb(66, 66, 66);
                scanOnlineForm.ChangeLabel = new Action((() =>
                  {
                      if (a == 0)
                      {
                          BeginInvoke(new Action((() =>
                          {
                              ProductionStatusInfoPanel.Controls.Find("control003", false).First().Hide();
                              var control = ProductionStatusInfoPanel.Controls.Find("control002", false).First();
                              control.Click -= SelfCheckItemEvent;
                              control.Click += ProductOfflineEvent;
                              control.Text = "产品下线";
                              ProductionStatusInfoPanel.Controls[1].Dispose();
                              a++;
                          })));

                      }
                  }));
                scanOnlineForm.ResetLabel = () =>
                {
                    if (a == 1)
                    {
                        BeginInvoke(new Action((() =>
                        {
                            ProductionStatusInfoPanel.Controls.Find("control003", false).First().Show();
                            var control = ProductionStatusInfoPanel.Controls.Find("control002", false).First();
                            control.Click += SelfCheckItemEvent;
                            control.Click -= ProductOfflineEvent;
                            control.Text = "自检项录入";
                            ProductionStatusInfoPanel.Controls[1].Show();
                            a = 0;
                        })));
                    }
                };
                panel10.Controls.Add(scanOnlineForm);
                scanOnlineForm.Show();
            }
            else
            {
                ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                    Color.MediumSeaGreen;
                SelfCheckColorJudge();
            }
        }
        //自检项配置标签点击事件
        private void SelfCheckItemEvent(object sender, EventArgs e)
        {
            panel10.Controls.Clear();
            if (string.IsNullOrEmpty(ProductIDTxt.Text))
            {
                FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                return;
            }

            var selfCheckItemForm = new SelfCheckItemForm(ProductIDTxt.Text.Trim(),
               _staffId, _staffCode, _staffCode);
            var controls = selfCheckItemForm.Controls.Find("lblTitle", false).First();
            controls.Visible = false;
            selfCheckItemForm.Location = new Point(0, 0);
            selfCheckItemForm.FormBorderStyle = FormBorderStyle.None;
            selfCheckItemForm.AutoSize = false;
            selfCheckItemForm.AutoScaleMode = AutoScaleMode.None;
            selfCheckItemForm.Size = new Size(panel10.Width, panel10.Height);
            selfCheckItemForm.AutoScaleMode = AutoScaleMode.Font;
            selfCheckItemForm.TopLevel = false;
            selfCheckItemForm.BackColor = Color.FromArgb(247, 247, 247);
            selfCheckItemForm.ForeColor = Color.FromArgb(66, 66, 66);

            selfCheckItemForm.ChangeBgColor = () =>
                ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor = Color.MediumSeaGreen;

            panel10.Controls.Add(selfCheckItemForm);
            selfCheckItemForm.Show();
        }
        //产品下线标签点击事件
        private void ProductOfflineEvent(object sender, EventArgs e)
        {
            panel10.Controls.Clear();
            var label = (Label)sender;
            if (label.BackColor == Color.MediumSeaGreen) return;
            if (string.IsNullOrEmpty(ProductIDTxt.Text))
            {
                FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                return;
            }

            //如果control003隐藏了,就说明是工装工序
            if (ProductionStatusInfoPanel.Controls.Find("control003", false).FirstOrDefault()?.Visible == false)
            {
                OpenScanOfflineForm(out var isOk);
                if (isOk)
                {
                    AddCntLogicProOffline();
                }
                return;
            }
            using (var context = new Model())
            {
                if (!context.C_ProductQualityData.Any(s => s.ProductBornCode == ProductIDTxt.Text.Trim() && s.OfflineStaffID == null))
                {
                    FrmDialog.ShowDialog(this, "未执行自检项录入", "提示");
                    return;
                }
            }
            //自检项配置与否
            if (SelfItemCheck())
            {
                //自检项控制点转档
                SelfItemCntLogicTurn();

                //打开产品下线窗体
                OpenScanOfflineForm(out var isOk);

                if (isOk)
                {
                    //增加产品下线控制点
                    AddCntLogicProOffline();
                }
            }
        }
        //添加产品下线控制点
        private void AddCntLogicProOffline(string remark = "")
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());
                var cBWuECntlLogicPro = new C_BWuE_CntlLogicPro
                {
                    ProductBornCode = ProductIDTxt.Text.Trim(),
                    ProcedureCode = _cProductProcessing?.ProcedureCode,
                    ControlPointID = 3,
                    Sort = "3",
                    EquipmentCode = _equipmentCode,
                    State = "1",
                    StartTime = context.GetServerDate(),
                    Remarks = remark
                };

                context.Entry(cBWuECntlLogicPro).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        //打开产品下线窗体
        private void OpenScanOfflineForm(out bool isOk)
        {
            var scanOfflineForm = new ScanOfflineForm(ProductIDTxt.Text.Trim(), _staffId, _staffCode, _staffName)
            {
                ChangeBgColor = () =>
                {
                    ProductionStatusInfoPanel.Controls.Find("control003", false).First().BackColor =
                        Color.MediumSeaGreen;
                    ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor =
                        Color.MediumSeaGreen;
                }
                   ,
                ClearMainPanelTxt = () =>
                {
                    ProductIDTxt.Clear();
                    CurrentProcessTxt.Clear();
                    ProductNameTxt.Clear();
                    OnlineTimeTxt.Clear();
                },
                RegetProcedureTasksDetails = () =>
                {
                    InitialDidTasks();
                    InialToDoTasks();
                },
                HideLabel = () =>
                 {
                     DrawLabel();
                 }
            };
            var controls = scanOfflineForm.Controls.Find("lblTitle", false).First();
            controls.Visible = false;
            scanOfflineForm.Location = new Point(panel10.Width / 2 - scanOfflineForm.Width / 2, 0);
            scanOfflineForm.FormBorderStyle = FormBorderStyle.None;
            scanOfflineForm.AutoSize = false;
            scanOfflineForm.AutoScaleMode = AutoScaleMode.None;
            scanOfflineForm.Size = new Size(553, panel10.Height);
            scanOfflineForm.AutoScaleMode = AutoScaleMode.Font;
            scanOfflineForm.TopLevel = false;
            scanOfflineForm.BackColor = Color.FromArgb(247, 247, 247);
            scanOfflineForm.ForeColor = Color.FromArgb(66, 66, 66);
            panel10.Controls.Add(scanOfflineForm);
            scanOfflineForm.Show();
            isOk = true;
        }

        //判断自检项配置与否
        private bool SelfItemCheck()
        {
            bool isOk = true;
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());
                //在产品质量数据表中根据产品出生证和工序名称以及检验结果为空  查询实际值为空的数据
                var cProductQualityDatas = context.C_ProductQualityData.Where(s =>
                    s.ProductBornCode == ProductIDTxt.Text.Trim() && s.ProcedureName == CurrentProcessTxt.Text.Trim() &&
                    s.CollectValue == null && s.CheckResult == null).ToList();

                foreach (var cProductQualityData in cProductQualityDatas)
                {
                    //在工序自检项配置表中根据  实际值为空的数据判断 是不是必填项
                    var aProcedureSelfCheckingConfig = context.A_ProcedureSelfCheckingConfig.FirstOrDefault(s =>
                        s.ItemCode == cProductQualityData.ItemCode && s.IsEnable == true && s.IsAvailable == true &&
                        s.IsRequired == true && s.ProcedureID.ToString() == _cProductProcessing.ProcedureID);
                    if (aProcedureSelfCheckingConfig != null)
                    {
                        FrmDialog.ShowDialog(this, $"{aProcedureSelfCheckingConfig.ItemName + " 为必填项,请填写."}", "必填项空缺");
                        return false;
                    }
                }
                return isOk;
            }
        }
        //自检项控制点转档
        private void SelfItemCntLogicTurn(string remark = "")
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == ProductIDTxt.Text.Trim());

                //在控制点过程表中 根据产品出生证 工序编号 控制点id 设备编号(需要修改) 查到相关集合
                var cBWuECntlLogicPros = context.C_BWuE_CntlLogicPro.Where(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim() && s.ProcedureCode == _cProductProcessing.ProcedureCode
                                                              && s.ControlPointID == 2 && s.EquipmentCode == _equipmentCode)
                    .OrderByDescending(s => s.StartTime).ToList();
                //判断过程表中有无数据  如果没有(说明已经转档过了) 那就不操作了弟弟
                if (cBWuECntlLogicPros.Any())
                {
                    cBWuECntlLogicPros[0].State = "2";
                    cBWuECntlLogicPros[0].FinishTime = context.GetServerDate();
                    cBWuECntlLogicPros[0].Remarks = remark;

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

        private void OpenWorkInstruction(object sender, EventArgs e)
        {
            //这里需要修改 根据指定的路径规则 查找作业指导书
            System.Diagnostics.Process.Start("C:\\Users\\Sweetie\\Desktop\\车间级制造执行系统IDEF图_第一层+第二层+第三层 - v1.9.vsdx");
        }

        private void OpenProgramFile(object sender, EventArgs e)
        {
            //这里需要修改 根据指定的路径规则 查找并下载机床程序文件
            System.Diagnostics.Process.Start("C:\\Users\\Sweetie\\Desktop\\20-033班审批材料补交\\2020170277-肖锴.pdf");
        }

        private void OpenLoginForm(object sender, EventArgs e)
        {
            // FrmInputs frm = new FrmInputs("盛鼎---加工中心001",
            //     new string[] { "账号", "密码", "身份证号", "住址" },
            //     new Dictionary<string, HZH_Controls.TextInputType>() { { "电话", HZH_Controls.TextInputType.Regex }, { "身份证号", HZH_Controls.TextInputType.Regex } },
            //     new Dictionary<string, string>() { { "电话", "^1\\d{0,10}$" }, { "身份证号", "^\\d{0,18}$" } },
            //     new Dictionary<string, KeyBoardType>() { { "电话", KeyBoardType.UCKeyBorderNum }, { "身份证号", KeyBoardType.UCKeyBorderNum } },
            //     new List<string>() { "账号", "密码", "身份证号" });
            // frm.ShowDialog(this);
            new UserLoginForm().Show();

            C_LoginInProcessing cLoginInProcessing;
            using (var context = new Model())
            {
                cLoginInProcessing = context.C_LoginInProcessing.FirstOrDefault(s =>
                    s.StaffCode == EmployeeIDTxt.Text && s.EquipmentID.ToString() == _equipmentId &&
                    s.OfflineTime == null);

                if (cLoginInProcessing != null) cLoginInProcessing.OfflineTime = context.GetServerDate();
                context.SaveChanges();
            }

            if (cLoginInProcessing != null)
            {
                LoginUserTurnArchives(cLoginInProcessing);
            }
            a = 0;
            _tupleI = 0;
            _widthX = 400;
            Close();
        }

        private void CloseForms(object sender, EventArgs e)
        {
            C_LoginInProcessing cLoginInProcessing;
            using (var context = new Model())
            {
                cLoginInProcessing = context.C_LoginInProcessing.FirstOrDefault(s =>
                    s.StaffCode == EmployeeIDTxt.Text && s.EquipmentID.ToString() == _equipmentId && s.OfflineTime == null);
                if (cLoginInProcessing != null) cLoginInProcessing.OfflineTime = context.GetServerDate();
                context.SaveChanges();
            }
            if (cLoginInProcessing != null)
            {
                LoginUserTurnArchives(cLoginInProcessing);
            }
            Application.Exit();
        }

        private void OpenScanOfflineForm(object sender, EventArgs e) => new ScanOfflineForm(ProductIDTxt.Text.Trim(), _staffId, _staffCode, _staffName).Show();

        private void OpenSelfCheckItemForm(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ProductIDTxt.Text))
            {
                FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                // MessageBox.Show("未检测到上线产品");
                return;
            }

            var selfCheckItemForm = new SelfCheckItemForm(ProductIDTxt.Text.Trim(), _staffId, _staffCode, _staffCode);
            selfCheckItemForm.ChangeBgColor = () =>
                ProductionStatusInfoPanel.Controls.Find("control002", false).First().BackColor = Color.MediumSeaGreen;
            selfCheckItemForm.Show();
        }

        private void OpenScanOnlineForm(object sender, EventArgs e)
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据加工中心编号和上线时间非空  判断本加工中心是否有正在处理的生产任务 
                var cProductProcessing = context.C_ProductProcessing
                    .FirstOrDefault(s => s.WorkshopCode == "001" && s.OnlineTime != null);
                if (cProductProcessing != null)
                {
                    FrmDialog.ShowDialog(this, "当前已有正在处理的生产任务,请完成", "已有生产任务");
                }
                else
                {
                    var scanOnlineForm = new ScanOnlineForm(_staffId, _staffCode, _staffName, _workshopId, _workshopCode, _workshopName, _equipmentId, _equipmentCode, _equipmentName)
                    {
                        DisplayInfoToMainPanel = (s1, s2, s3, s4) =>
                        {
                            ProductIDTxt.Text = s1;
                            ProductIDTxt.ReadOnly = true;
                            ProductNameTxt.Text = s2;
                            ProductNameTxt.ReadOnly = true;
                            CurrentProcessTxt.Text = s3;
                            CurrentProcessTxt.ReadOnly = true;
                            OnlineTimeTxt.Text = s4;
                            OnlineTimeTxt.ReadOnly = true;
                        },
                        ChangeBgColor = () =>
                        {
                            ProductionStatusInfoPanel.Controls.Find("control001", false).First().BackColor =
                                Color.MediumSeaGreen;
                            ProductionStatusInfoPanel.Controls.
                                Find("control002", false).First().BackColor = Color.MediumSeaGreen;
                        }
                            ,
                        RegetProcedureTasksDetails = () =>
                        {
                            InialToDoTasks();
                        }
                        // ChangeBgColor = () => ProductOnlineLbl.BackColor = Color.MediumSeaGreen
                    };
                    scanOnlineForm.Show();
                }
            }
        }

        private void lbl_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("hello world");
        }

        //代码生成label 
        private Label GenerateLabel()
        {
            var icon = (FontIcons)Enum.Parse(typeof(FontIcons), MuneList[_tupleI].Item2);
            var label = new Label
            {
                AutoSize = false,
                Size = new Size(90, 60),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.BottomCenter,
                ImageAlign = ContentAlignment.TopCenter,
                Margin = new Padding(5),
                Text = MuneList[_tupleI].Item1,
                Image = FontImages.GetImage(icon, 32, Color.White),
                Location = new Point(_widthX, 0),
                Font = new Font("微软雅黑", 12, FontStyle.Bold)
            };
            FirstTitlePanel.Controls.Add(label);
            _widthX += 90;
            _tupleI++;
            return label;
        }

        public void LoginUserTurnArchives(C_LoginInProcessing cLoginInProcessing)
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<C_LoginInProcessing, C_LoginInDocument>());
            var mapper = mapperConfiguration.CreateMapper();
            var cLoginInDocument = mapper.Map<C_LoginInDocument>(cLoginInProcessing);
            using (var context = new Model())
            {
                //此处应该优化成事务操作 保证acid原则
                context.C_LoginInDocument.Add(cLoginInDocument);
                context.SaveChanges();
                context.Entry(cLoginInProcessing).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ServerStateWatch();
            var apsProcedureTaskDetails = GetDidProcedureTask();
            ucDataGridView1.DataSource = apsProcedureTaskDetails;
            var toDoProcedureTask = GetToDoProcedureTask();
            ucDataGridView2.DataSource = toDoProcedureTask;
        }

        private void ServerStateWatch()
        {
            using (var context  =new Model())
            {
                var exists = context.Database.Exists();
                if (!exists)
                {
                    changeSignalColor();
                }
                else
                {
                    ResetSignalColor();
                }
                //     var connectionState = context.Database.Connection.State;
                //     if (connectionState == ConnectionState.Closed || connectionState == ConnectionState.Broken)
                //     {
                //         FrmDialog.ShowDialog(this, "服务器异常,请校正服务器状态后重试");
                //         return;
                //     }
            }
        }

        private void ResetSignalColor()
        {
            ucSignalLamp1.LampColor = new[] {Color.Green}; 
        }

        public void changeSignalColor()
        {
            ucSignalLamp1.LampColor=new []{Color.Red };
        }
    }

    public class TestGridModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public List<TestGridModel> Childrens { get; set; }
    }
}
