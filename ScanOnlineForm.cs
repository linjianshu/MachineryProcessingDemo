using AutoMapper;
using HZH_Controls;
using HZH_Controls.Forms;
using System;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WorkPlatForm.Public_Classes;

namespace MachineryProcessingDemo
{
    public partial class ScanOnlineForm : FrmWithOKCancel1
    {
        public ScanOnlineForm(long? staffId, string staffCode, string staffName)
        {
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            InitializeComponent();
        }

        public ScanOnlineForm(long? staffId, string staffCode, string staffName, string productBornCode)
        {
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            _productBornCode = productBornCode;
            InitializeComponent();
            ProductIDTxt.Text = _productBornCode;
        }

        private static long? _staffId;
        private static string _staffCode;
        private static string _staffName;
        private static string _productBornCode;
        private static string _strProductType = "产品";
        private static APS_ProcedureTask _currentTask;
        public Action RegetProcedureTasksDetails;
        public Action<string, string, string, string> DisplayInfoToMainPanel;
        public Action ChangeBgColor;
        private void ScanOnline_Load(object sender, EventArgs e)
        {
            var tuple = new Tuple<string, string>("扫码上线", "A_fa_cube");
            var icon1 = (FontIcons)Enum.Parse(typeof(FontIcons), tuple.Item2);
            var pictureBox1 = new PictureBox
            {
                AutoSize = false,
                Size = new Size(40, 40),
                ForeColor = Color.FromArgb(255, 77, 59),
                Image = FontImages.GetImage(icon1, 40, Color.FromArgb(255, 77, 59)),
                Location = new Point(this.Size.Width / 2 - 20, 30)
            };
            panel3.Controls.Add(pictureBox1);

            if (serialPortTest.IsOpen) { serialPortTest.Close(); }
            string portName = ConfigAppSettingsHelper.ReadSetting("PortName");
            string baudRate = ConfigAppSettingsHelper.ReadSetting("BaudRate");
            serialPortTest.Dispose();//释放扫描枪所有资源
            serialPortTest.PortName = portName;
            serialPortTest.BaudRate = int.Parse(baudRate);
            try
            {
                if (!serialPortTest.IsOpen)
                {
                    serialPortTest.Open();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// 扫描枪扫描调用方法
        /// </summary>
        /// <param name="serialPort"></param>
        /// <returns></returns>
        private static string GetDataFromSerialPort(SerialPort serialPort)
        {
            Thread.Sleep(300);
            byte[] buffer = new byte[serialPort.BytesToRead];
            string receiveString = "";
            try
            {
                serialPort.Read(buffer, 0, buffer.Length);
                foreach (var t in buffer)
                {
                    receiveString += (char)t;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (receiveString.Length > 2)
            {
                receiveString = receiveString.Substring(0, receiveString.Length - 1);
            }
            return receiveString;
        }

        //清空窗体textbox 解开只读限制
        private void ClearTxt()
        {
            BeginInvoke(new Action(() =>
            {
                ProductIDTxt.Clear();
                ProductIDTxt.ReadOnly = false;
                ProductNameTxt.Clear();
                ProductNameTxt.ReadOnly = false;
                _productBornCode = "";
            }));
        }
        private void serialPortTest_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            ClearTxt();
            var receivedData = GetDataFromSerialPort(serialPortTest);

            if (CheckProductBornCode(receivedData))
            {
                CheckTaskValidity();
            }
        }

        private bool CheckProductBornCode(string receivedData)
        {
            using (var context = new Model())
            {
                BeginInvoke(new Action(() =>
                {
                    ProductIDTxt.Text = receivedData;
                    ProductIDTxt.ReadOnly = true;
                }));

                if (_strProductType.Contains("产品"))
                {
                    //在计划产品出生证表中根据产品出生证来获取产品名称(要添加一个状态)
                    var aPlanProductInflammations = context.A_PlanProductInfomation.FirstOrDefault(s =>
                        s.ProductBornCode == receivedData && s.IsAvailable == true);

                    if (aPlanProductInflammations != null)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            ProductNameTxt.Text = aPlanProductInflammations.ProductName;
                            ProductNameTxt.ReadOnly = true;
                        }));
                        _productBornCode = receivedData;
                        return true;
                    }
                    else
                    {
                        BeginInvoke(new Action((() =>
                            FrmDialog.ShowDialog(this, "产品出生证不正确", "出生证不正确"))));
                        // MessageBox.Show("产品出生证不正确");
                        ClearTxt();
                        _productBornCode = "";
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public bool CheckTaskValidity()
        {
            using (var context = new Model())
            {
                {
                    //在工序任务明细表中根据产品出生证和设备编号以及任务完成状态  判断有无加工任务
                    var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                        s.ProductBornCode == _productBornCode && s.EquipmentID == 1 && s.TaskState == 1);
                    if (apsProcedureTaskDetail == null)
                    {
                        BeginInvoke(new Action((() =>
                            FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                        ClearTxt();
                        return false;
                    }

                    //在工序任务表中根据设备编号和任务状态和 开始时间排序得到优先级最高的工序任务
                    // 待完成的工序编号(弃用)
                    var apsProcedureTasks = context.APS_ProcedureTask.Where(s => s.EquipmentID == 1 && s.TaskState == 2)
                        .OrderBy(s => s.StartTime).FirstOrDefault();
                    // &&s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode


                    if (apsProcedureTasks != null)
                    {
                        _currentTask = apsProcedureTasks;
                        //判断是否已经加工过了
                        var procedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                            s.ProductBornCode == ProductIDTxt.Text.Trim() &&
                            s.ProcedureCode == apsProcedureTasks.ProcedureCode);

                        if (procedureTaskDetail.TaskState == 3)
                        {
                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "该工序任务已完成", "提示"))));
                            ClearTxt();
                            return false;
                        }

                        //在工序任务明细表中 根据 任务表id==明细表工序任务id 获得优先级最高的工序任务下的产品出生证集合
                        var list = context.APS_ProcedureTaskDetail.Where(s => s.TaskTableID == apsProcedureTasks.ID)
                            .Select(s => s.ProductBornCode).ToList();

                        //判断扫码的产品出生证是否在该集合里
                        if (!list.Contains(_productBornCode))
                        {
                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,请重新扫码", "生产顺序异常"))));
                            ClearTxt();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        BeginInvoke(new Action((() =>
                            FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                        ClearTxt();
                        return false;
                    }
                }
            }
        }
        public bool CheckTaskValidity(string procedureCode)
        {
            using (var context = new Model())
            {
                {
                    //在工序任务明细表中根据产品出生证和设备编号以及任务完成状态  判断有无加工任务
                    var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                        s.ProductBornCode == _productBornCode && s.EquipmentID == 1 && s.TaskState == 1);
                    if (apsProcedureTaskDetail == null)
                    {
                        BeginInvoke(new Action((() =>
                            FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                        ClearTxt();
                        return false;
                    }

                    //在工序任务表中根据设备编号和任务状态和 开始时间排序得到优先级最高的工序任务
                    // 待完成的工序编号(弃用)
                    var apsProcedureTasks = context.APS_ProcedureTask.Where(s => s.EquipmentID == 1 && s.TaskState == 2)
                        .OrderBy(s => s.StartTime).FirstOrDefault();
                    // &&s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode

                    //判断工序号对不对应,  防止出现同一个产品但是先加工了op02的情况    
                    if (apsProcedureTasks.ProcedureCode!=procedureCode)
                    {
                        FrmDialog.ShowDialog(this, "该任务不是推荐生产工序顺序,请重新扫码", "生产工序顺序异常");
                        return false; 
                    }

                    if (apsProcedureTasks != null)
                    {
                        _currentTask = apsProcedureTasks;
                        //判断是否已经加工过了
                        var procedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                            s.ProductBornCode == ProductIDTxt.Text.Trim() &&
                            s.ProcedureCode == apsProcedureTasks.ProcedureCode);

                        if (procedureTaskDetail.TaskState == 3)
                        {
                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "该工序任务已完成", "提示"))));
                            ClearTxt();
                            return false;
                        }

                        //在工序任务明细表中 根据 任务表id==明细表工序任务id 获得优先级最高的工序任务下的产品出生证集合
                        var list = context.APS_ProcedureTaskDetail.Where(s => s.TaskTableID == apsProcedureTasks.ID)
                            .Select(s => s.ProductBornCode).ToList();

                        //判断扫码的产品出生证是否在该集合里
                        if (!list.Contains(_productBornCode))
                        {
                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,请重新扫码", "生产顺序异常"))));
                            ClearTxt();
                            return false;
                        }

                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        BeginInvoke(new Action((() =>
                            FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                        // FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务");
                        // MessageBox.Show("该加工中心暂无当前产品加工任务");
                        ClearTxt();
                        return false;
                    }
                }
            }
        }

        private void DisplayInfo(string strData)
        {
            var action = new Action((() =>
            {
                ProductIDTxt.Text = strData;
                ProductIDTxt.ReadOnly = true;
            }));
            BeginInvoke(action);
        }

 
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            _strProductType = comboBox1.Text;
            ClearTxt();
        }
        private bool IsChecked()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                var _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == _productBornCode);
                //在工序任务明细表中根据产品出生证/有效性/工序号 获得元数据
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.ProductBornCode == _cProductProcessing.ProductBornCode && s.IsAvailable == true &&
                    s.ProcedureCode == _cProductProcessing.ProcedureCode);
                if (apsProcedureTaskDetail.IsChecked == 1)
                {
                    return true;
                }
                return false;
            }
        }

        protected override void DoEnter()
        {
            if (CheckProductBornCode(ProductIDTxt.Text))
            {
                if (CheckTaskValidity())
                {
                    AddCntLogicPro();
                    //判断首件
                    bool hasFirstRecord = HasFirstRecord();

                    //齐套确认
                    if (KittingConfirm())
                    {
                        //操作人员确认
                        if (WorkerConfirm())
                        { 
                            //转档  工序任务明细表=>产品加工过程表
                            ProcessTurnArchives();

                            //判断是否抽检
                            var isChecked = IsChecked();
                            if (!hasFirstRecord&&isChecked)
                            {
                                AddFirstRecord();
                            }
                         
                            //完善工序任务明细表中的数据 诸如任务状态 ; 修改人修改时间
                            PerfectApsDetail();
                            //完善计划产品出生证表
                            PerfectPlanProductInfo();
                            //转档  
                            CntLogicTurn();
                            //刷新界面数据
                            RegetProcedureTasksDetails();
                        }
                    }
                }
            }
        }
        public void PerfectPlanProductInfo()
        {
            using (var context = new Model())
            {
                var aPlanProductInflammations = context.A_PlanProductInfomation.FirstOrDefault(s =>
                    s.ProductBornCode == _productBornCode && s.State == 0 && s.IsAvailable == true &&
                    s.PlanCode == _currentTask.PlanCode);
                if (aPlanProductInflammations != null)
                {
                    aPlanProductInflammations.State = 1; //状态:执行中
                    aPlanProductInflammations.LastModifiedTime = DateTime.Now;
                    aPlanProductInflammations.ModifierID = _staffId;
                }
                context.SaveChanges();
            }
        }

        public void PerfectApsDetail()
        {
            using (var context = new Model())
            {
                var cProductProcessing = context.C_ProductProcessing.First(s => s.ProductBornCode == _productBornCode);
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.EquipmentID == cProductProcessing.EquipmentID &&
                    s.ProductBornCode == cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == cProductProcessing.ProcedureCode && s.IsAvailable == true && s.TaskState == 1);
                apsProcedureTaskDetail.TaskState = 2; //正在进行中
                apsProcedureTaskDetail.LastModifiedTime = DateTime.Now;
                apsProcedureTaskDetail.ModifierID = _staffId.ToString();
                context.SaveChanges();
            }
        }

        public void CntLogicTurn()
        {
            using (var context = new Model())
            {
                //在控制点过程表中 根据产品出生证 工序编号 控制点id 设备编号(需要修改) 查到相关集合
                var cBWuECntlLogicPros = context.C_BWuE_CntlLogicPro.Where(s =>
                        s.ProductBornCode == _productBornCode && s.ProcedureCode == _currentTask.ProcedureCode
                                                              && s.ControlPointID == 1 && s.EquipmentCode == "1")
                    .OrderByDescending(s => s.StartTime).ToList();
                cBWuECntlLogicPros[0].State = "2";
                cBWuECntlLogicPros[0].FinishTime = DateTime.Now;

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

        public void AddCntLogicPro()
        {
            //录入进控制点过程表  哪个产品在哪个工序哪个控制点正在进行
            using (var context = new Model())
            {
                var cBWuECntlLogicPro = new C_BWuE_CntlLogicPro();
                cBWuECntlLogicPro.ProductBornCode = _productBornCode;
                cBWuECntlLogicPro.ProcedureCode = _currentTask.ProcedureCode;
                cBWuECntlLogicPro.ControlPointID = 1;
                cBWuECntlLogicPro.Sort = 1.ToString();
                cBWuECntlLogicPro.EquipmentCode = 1.ToString();
                cBWuECntlLogicPro.State = 1.ToString();
                cBWuECntlLogicPro.StartTime = DateTime.Now;

                context.C_BWuE_CntlLogicPro.Add(cBWuECntlLogicPro);
                context.SaveChanges();
            }
        }

        //操作成员确认
        public bool WorkerConfirm()
        {
            using (var context = new Model())
            {
                var strings = _currentTask.WorkerCode.Split(',');
                var contains = strings.Contains(_staffCode);
                if (!contains)
                {
                    var dialogResult = (DialogResult)Invoke(new Func<DialogResult>((() =>
                        FrmDialog.ShowDialog(this, "加工人员与规定人员不一致,是否继续生产",
                            "人员不匹配", true))));
                    // var dialogResult = MessageBox.Show("加工人员与规定人员不一致,是否继续生产", "人员不匹配", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        if (!IsHighLevel())
                        {
                            var cInformationPushProcessing = new C_InfomationPushProcessing
                            {
                                //这里需要改动
                                PushID = "push001",
                                PushCategory = "真的很紧急",
                                InitPushRankPushRank = "非常重要的等级呢亲",
                                PushContent = "想玩高级设备",
                                CreateType = "现场发起的",
                                PushState = 1,
                                CreateTime = DateTime.Now,
                                CreatorID = _staffId
                            };

                            context.C_InfomationPushProcessing.Add(cInformationPushProcessing);
                            context.SaveChanges();

                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "很抱歉,您的操作等级未能达到要求,已将消息推送至主管", "等级异常"))));
                            ClearTxt();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        ClearTxt();
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }


        private bool IsHighLevel()
        {
            //这里需要改动
            return true;
        }

        public bool KittingConfirm()
        {
            using (var context = new Model())
            {
                //先从工序基础表中 根据工序编号判断是否有齐套要求  ????当前产品生产绑定在特定的计划下 是否只需要查找该计划是否齐套就ok了????
                var aProcedureBase = context.A_ProcedureBase.FirstOrDefault(s => s.ProcedureCode == _currentTask.ProcedureCode);

                var aMaterialProgramDemand = context.A_MaterialProgramDemand.Where(s =>
                    s.ProjectID == _currentTask.ProjectID && s.PlanCode == _currentTask.PlanCode).ToList();

                var materialProgramAll = aMaterialProgramDemand.All(s => s.Cstate == 1);

                var aCutterDemands = context.A_CutterDemand.Where(s =>
                    s.ProjectCode == _currentTask.ProjectCode && s.PlanCode == _currentTask.PlanCode).ToList();

                var cutterAll = aCutterDemands.All(s => s.Cstate == 1);

                if (!materialProgramAll || !cutterAll)
                {
                    DialogResult dialogResult = DialogResult.None;
                    if (!materialProgramAll && !cutterAll)
                    {
                        if (!this.IsHandleCreated)
                        {
                            dialogResult = FrmDialog.ShowDialog(this, "物料程序和刀具均未齐套,是否继续生产?", "未齐套", true);
                        }
                        else
                        {
                            dialogResult = (DialogResult)Invoke(new Func<DialogResult>((() =>
                            FrmDialog.ShowDialog(this, "物料程序和刀具均未齐套,是否继续生产?", "未齐套", true))));
                        }
                    }
                    else if (!materialProgramAll)
                    {
                        if (!this.IsHandleCreated)
                        {
                            dialogResult = FrmDialog.ShowDialog(this, "物料或程序未齐套,是否继续生产?", "未齐套", true);
                        }
                        else
                        {
                            dialogResult = (DialogResult)Invoke(new Func<DialogResult>((() =>
                                FrmDialog.ShowDialog(this, "物料或程序未齐套,是否继续生产?", "未齐套", true))));
                        }
                    }
                    else
                    {
                        if (!this.IsHandleCreated)
                        {
                            dialogResult = FrmDialog.ShowDialog(this, "刀具未齐套,是否继续生产?", "未齐套", true);
                        }
                        else
                        {
                            dialogResult = (DialogResult)Invoke(new Func<DialogResult>((() =>
                            FrmDialog.ShowDialog(this, "刀具未齐套,是否继续生产?", "未齐套", true))));
                        }
                    }

                    if (dialogResult == DialogResult.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool HasFirstRecord()
        {
            using (var context = new Model())
            {

                var cProcedureFirstRecords = context.C_ProcedureFirstDocument.Where
                (s => s.PlanCode == _currentTask.PlanCode &&
                      s.EquipmentID == 1 && s.ProductID == _currentTask.ProductID
                      && s.ProcedureCode == _currentTask.ProcedureCode).ToList();
                if (cProcedureFirstRecords.Any())
                {
                    
                    MessageBox.Show("质检还没做所以要手动修改转档后的首件记录状态 , 你修改了喵");
                    
                    var any = cProcedureFirstRecords.Any(s => s.ProcedureFirstStatus == 1);
                    if (any)
                    {
                        //如果存在 但是有一条是合格的
                        return true;
                    }
                    else
                    {
                        //如果所有的都不是合格的   那么就要添加首件记录
                        // AddFirstRecord();
                        return false;
                    }
                }
                else
                {
                    //记得补充代码
                    //如果一条也没有的话  就要添加首件记录
                    // AddFirstRecord();
                    return false;
                }
            }
        }

        public void AddFirstRecord()
        {
            using (var context = new Model())
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<APS_ProcedureTask, C_ProcedureFirstRecord>());
                var mapper = mapperConfiguration.CreateMapper();
                var cProcedureFirstRecord = mapper.Map<C_ProcedureFirstRecord>(_currentTask);
                cProcedureFirstRecord.ProductBornCode = _productBornCode;

                //在计划产品出生证表中 根据计划编号获得计划id 并赋值
                var aPlanProductInformation = context.A_PlanProductInfomation.FirstOrDefault(s =>
                    s.PlanCode == _currentTask.PlanCode);
                if (aPlanProductInformation != null) cProcedureFirstRecord.PlanID = aPlanProductInformation.PlanID;

                //在产品工序基本表中根据工序编号获得工序id和名称   这里是否需要外加计划id项目id和产品id才能查得到????
                var aProductProcedureBase = context.A_ProductProcedureBase.FirstOrDefault(s =>
                    s.ProcedureCode == _currentTask.ProcedureCode && s.PlanCode == _currentTask.PlanCode && s.ProjectCode == _currentTask.ProjectCode
                    && s.ProductCode == _currentTask.ProductCode);
                if (aProductProcedureBase != null)
                {
                    cProcedureFirstRecord.ProcedureName = aProductProcedureBase.ProcedureName;
                    cProcedureFirstRecord.ProcedureID = aProductProcedureBase.ProcedureID;
                }

                //记得修改
                cProcedureFirstRecord.EquipmentName = "车床ca1601";

                context.C_ProcedureFirstRecord.Add(cProcedureFirstRecord);
                context.SaveChanges();
            }
        }

        public void ProcessTurnArchives()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.CreateMap<APS_ProcedureTask, C_ProductProcessing>());
            var mapper = mapperConfiguration.CreateMapper();
            var cProductProcessing = mapper.Map<C_ProductProcessing>(_currentTask);
            cProductProcessing.WorkshopID = 1;
            cProductProcessing.WorkshopCode = "001";
            cProductProcessing.WorkshopName = "加工中心001";

            using (var context = new Model())
            {
                //在计划产品出生证表中 根据计划编号/有效性/产品出生证   获得计划id 并赋值
                var aPlanProductInformation = context.A_PlanProductInfomation.FirstOrDefault(s =>
                    s.PlanCode == _currentTask.PlanCode && s.IsAvailable == true && s.ProductBornCode == _productBornCode);
                if (aPlanProductInformation != null) cProductProcessing.PlanID = aPlanProductInformation.PlanID;

                cProductProcessing.ProductBornCode = _productBornCode;

                //在产品工序基本表中根据工序编号/有效性/产品号/计划号          获得工序id和名称   这里是否需要外加计划id项目id和产品id才能查得到????
                var aProductProcedureBase = context.A_ProductProcedureBase.FirstOrDefault(s =>
                    s.ProcedureCode == _currentTask.ProcedureCode && s.IsAvailable == true && s.PlanCode == _currentTask.PlanCode
                    && s.ProductID == _currentTask.ProductID);

                //?????类型不匹配????
                if (aProductProcedureBase != null)
                {
                    cProductProcessing.ProcedureID = aProductProcedureBase.ProcedureID.ToString();
                    cProductProcessing.ProcedureName = aProductProcedureBase.ProcedureName;
                }

                //记得修改
                cProductProcessing.EquipmentName = "车床ca1601";
                cProductProcessing.OnlineStaffID = _staffId;
                cProductProcessing.OnlineStaffCode = _staffCode;
                cProductProcessing.OnlineStaffName = _staffName;
                cProductProcessing.OnlineTime = DateTime.Now;

                //上线类型判断

                context.C_ProductProcessing.Add(cProductProcessing);
                context.SaveChanges();

                FrmDialog.ShowDialog(this, $"产品{ProductIDTxt.Text}上线成功!", "上线成功");

                DisplayInfoToMainPanel(cProductProcessing.ProductBornCode, cProductProcessing.ProductName,
                    cProductProcessing.ProcedureName, cProductProcessing.OnlineTime.ToString());
                ChangeBgColor();

                Close();
                Dispose();
            }
        }

        private void ProductIDTxt_DoubleClick(object sender, EventArgs e)
        {
            if (CheckProductBornCode(ProductIDTxt.Text.Trim()))
            {
                CheckTaskValidity();
            }
        }
    }
}
