using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AutoMapper;
using HZH_Controls;
using HZH_Controls.Forms;
using Microsoft.Extensions.Configuration;
using QualityCheckDemo;
using WorkPlatForm.Public_Classes;

namespace MachineryProcessingDemo.Forms
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

        public ScanOnlineForm(long? staffId, string staffCode, string staffName, string workshopId, string workshopCode, string workshopName,
            string equipmentId, string equipmentCode, string equipmentName)
        {
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            _workshopId = workshopId;
            _workshopCode = workshopCode;
            _workshopName = workshopName;
            _equipmentId = equipmentId;
            _equipmentCode = equipmentCode;
            _equipmentName = equipmentName;
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
        public ScanOnlineForm(long? staffId, string staffCode, string staffName, string productBornCode, string workshopId, string workshopCode, string workshopName,
            string equipmentId, string equipmentCode, string equipmentName)
        {
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
            _productBornCode = productBornCode;
            InitializeComponent();
            ProductIDTxt.Text = _productBornCode;
            _workshopId = workshopId;
            _workshopCode = workshopCode;
            _workshopName = workshopName;
            _equipmentId = equipmentId;
            _equipmentCode = equipmentCode;
            _equipmentName = equipmentName;
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
        private static string _strProductType = "产品";
        private static APS_ProcedureTask _currentTask;
        public Action RegetProcedureTasksDetails;
        public Action<string, string, string, string> DisplayInfoToMainPanel;
        public Action ChangeBgColor;
        public Action ClearMainPanelTxt;
        public Action ChangeLabel;
        public Action ResetLabel;
        private void ScanOnline_Load(object sender, EventArgs e)
        {
            var addXmlFile = new ConfigurationBuilder().SetBasePath("E:\\project\\visual Studio Project\\MachineryProcessingDemo")
                .AddXmlFile("config.xml");
            var configuration = addXmlFile.Build();
            _workshopId = configuration["WorkshopID"];
            _workshopCode = configuration["WorkshopCode"];
            _workshopName = configuration["WorkshopName"];
            _equipmentId = configuration["EquipmentID"];
            _equipmentCode = configuration["EquipmentCode"];
            _equipmentName = configuration["EquipmentName"];

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

        //端口接收到数据的事件
        private void serialPortTest_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            ClearTxt();
            var receivedData = GetDataFromSerialPort(serialPortTest);

            if (CheckProductBornCode(receivedData))
            {
                CheckTaskValidity();
            }
        }

        //校验出生证正确与否
        private bool CheckProductBornCode(string receivedData)
        {
            using (var context = new Model())
            {

                if (_strProductType.Contains("产品"))
                {
                    ResetLabel();
                    //在计划产品出生证表中根据产品出生证来获取产品名称(要添加一个状态)
                    var aPlanProductInflammations = context.A_PlanProductInfomation.FirstOrDefault(s =>
                        s.ProductBornCode == receivedData && s.IsAvailable == true);

                    if (aPlanProductInflammations != null)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            ProductNameTxt.Text = aPlanProductInflammations.ProductName;
                            ProductNameTxt.ReadOnly = true;
                            ProductIDTxt.Text = receivedData;
                            ProductIDTxt.ReadOnly = true;
                        }));
                        _productBornCode = receivedData;
                        return true;
                    }
                    BeginInvoke(new Action((() =>
                        FrmDialog.ShowDialog(this, "产品出生证不正确", "出生证不正确"))));
                    ClearTxt();
                    _productBornCode = "";
                    return false;
                }
                ChangeLabel();
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                    s.ProductBornCode == receivedData && s.IsAvailable == true && s.ProcedureType == (decimal?)ProcedureType.Tooling);
                if (apsProcedureTaskDetail == null)
                {

                    BeginInvoke(new Action((() => FrmDialog.ShowDialog(this, "工量具出生证不正确", "出生证不正确"))));
                    return false;
                }

                BeginInvoke(new Action(() =>
                {
                    // ProductNameTxt.Text = aPlanProductInflammations.ProductName;
                    // ProductNameTxt.ReadOnly = true;
                    ProductIDTxt.Text = receivedData;
                    ProductIDTxt.ReadOnly = true;
                    _productBornCode = receivedData;
                }));
                return true;
            }
        }

        protected override void DoEsc()
        {
            serialPortTest.Close();
            Close();
            base.DoEsc();
        }

        //校核产品出生证加工完了与否/有无加工任务/是否推荐顺序
        private bool CheckTaskValidity()
        {
            using (var context = new Model())
            {
                //在工序任务明细表中根据产品出生证和设备编号以及任务完成状态  判断有无加工任务
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                    s.ProductBornCode == _productBornCode && s.EquipmentID.ToString() == _equipmentId &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.NotOnline);
                if (apsProcedureTaskDetail == null)
                {
                    BeginInvoke(new Action((() =>
                        FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                    ClearTxt();
                    return false;
                }

                //在工序任务表中根据设备编号和任务状态和 开始时间排序得到优先级最高的工序任务
                // 待完成的工序编号(弃用)
                var apsProcedureTasks = context.APS_ProcedureTask.Where(s =>
                        s.EquipmentID.ToString() == _equipmentId &&
                        s.TaskState == (decimal?)ApsProcedureTaskState.ToDo)
                    .OrderBy(s => s.StartTime).FirstOrDefault();
                // &&s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode

                if (apsProcedureTasks != null)
                {
                    _currentTask = apsProcedureTasks;
                    //判断是否已经加工过了
                    var procedureTaskDetail = context.APS_ProcedureTaskDetail.FirstOrDefault(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim() &&
                        s.ProcedureCode == apsProcedureTasks.ProcedureCode);
                    //新修改的
                    if (procedureTaskDetail == null)
                    {
                        // BeginInvoke(new Action((() =>
                        var dialogResult = FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,是否强制上线", "生产顺序异常", true);
                        if (dialogResult == DialogResult.OK)
                        {
                            if (_staffName == "admin")
                            {
                                _currentTask = context.APS_ProcedureTask.FirstOrDefault(s => s.IsAvailable == true && s.EquipmentID.ToString() == _equipmentId &&
                                                                                             s.TaskState == (decimal?)ApsProcedureTaskState.ToDo && s.ID == apsProcedureTaskDetail.TaskTableID);
                                return true;
                            }
                            FrmDialog.ShowDialog(this, "当前账户权限不足,强制上线失败!");
                        }

                        ClearTxt();
                        return false;
                    }
                    if (procedureTaskDetail.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed)
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
                        var dialogResult = FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,是否强制上线", "生产顺序异常", true);
                        if (dialogResult == DialogResult.OK)
                        {
                            if (_staffName == "admin")
                            {
                                _currentTask = context.APS_ProcedureTask.FirstOrDefault(s => s.IsAvailable == true && s.EquipmentID.ToString() == _equipmentId &&
                                                                                             s.TaskState == (decimal?)ApsProcedureTaskState.ToDo && s.ID == apsProcedureTaskDetail.TaskTableID);
                                return true;
                            }
                            FrmDialog.ShowDialog(this, "当前账户权限不足,强制上线失败!");
                        }

                        ClearTxt();
                        return false;
                    }
                    return true;
                }
                BeginInvoke(new Action((() =>
                    FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                ClearTxt();
                return false;
            }
        }
        //校核产品出生证加工完了与否/有无加工任务/是否推荐顺序(主窗体直接调用)
        public bool CheckTaskValidity(string procedureCode)
        {
            using (var context = new Model())
            {
                //在工序任务明细表中根据产品出生证和设备编号以及任务完成状态  判断有无加工任务
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.ProductBornCode == _productBornCode && s.EquipmentID.ToString() == _equipmentId &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.NotOnline);
                if (apsProcedureTaskDetail == null)
                {
                    BeginInvoke(new Action((() =>
                        FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                    ClearTxt();
                    return false;
                }

                //在工序任务表中根据设备编号和任务状态和 开始时间排序得到优先级最高的工序任务
                // 待完成的工序编号(弃用)
                var apsProcedureTasks = context.APS_ProcedureTask.Where(s =>
                        s.EquipmentID.ToString() == _equipmentId &&
                        s.TaskState == (decimal?)ApsProcedureTaskState.ToDo && s.IsAvailable == true)
                    .OrderBy(s => s.StartTime).FirstOrDefault();
                // &&s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode

                //判断工序号对不对应,  防止出现同一个产品但是先加工了op02的情况    
                if (apsProcedureTasks?.ProcedureCode != procedureCode)
                {
                    var dialogResult = FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,是否强制上线", "生产顺序异常", true);
                    if (dialogResult == DialogResult.OK)
                    {
                        if (_staffName == "admin")
                        {
                            _currentTask = context.APS_ProcedureTask.FirstOrDefault(s => s.IsAvailable == true && s.EquipmentID.ToString() == _equipmentId &&
                                                                                         s.TaskState == (decimal?)ApsProcedureTaskState.ToDo &&
                                                                                         s.ProcedureCode == procedureCode);
                            return true;
                        }
                        FrmDialog.ShowDialog(this, "当前账户权限不足,强制上线失败!");
                    }

                    ClearTxt();
                    return false;
                    // FrmDialog.ShowDialog(this, "该任务不是推荐生产工序顺序,请重新扫码", "生产工序顺序异常");
                    // return false;
                }

                if (apsProcedureTasks != null)
                {
                    _currentTask = apsProcedureTasks;
                    //判断是否已经加工过了
                    var procedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                        s.ProductBornCode == ProductIDTxt.Text.Trim() &&
                        s.ProcedureCode == apsProcedureTasks.ProcedureCode);

                    if (procedureTaskDetail.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed)
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
                        var dialogResult = FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,是否强制上线", "生产顺序异常", true);
                        if (dialogResult == DialogResult.OK)
                        {
                            if (_staffName == "admin")
                            {
                                _currentTask = context.APS_ProcedureTask.FirstOrDefault(s =>
                                    s.IsAvailable == true && s.EquipmentID.ToString() == _equipmentId &&
                                                                                             s.TaskState == (decimal?)ApsProcedureTaskState.ToDo &&
                                                                                             s.ProcedureCode == procedureCode);
                                return true;
                            }
                            FrmDialog.ShowDialog(this, "当前账户权限不足,强制上线失败!");
                        }

                        ClearTxt();
                        return false;

                        // FrmDialog.ShowDialog(this, "该任务不是推荐生产顺序,请重新扫码", "生产顺序异常");
                        // ClearTxt();
                        // return false;
                    }
                    return true;
                }
                BeginInvoke(new Action((() =>
                    FrmDialog.ShowDialog(this, "该加工中心暂无当前产品加工任务", "暂无加工任务"))));
                ClearTxt();
                return false;
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
        public bool IsChecked()
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
                    //判断是否是工装工序
                    if (_currentTask.ProcedureType == (decimal?)ProcedureType.Tooling)
                    {
                        if (WorkerConfirm())
                        {
                            //转档  工序任务明细表=>产品加工过程表
                            ProcessTurnArchives();
                            //完善工序任务明细表中的数据 诸如任务状态 ; 修改人修改时间
                            PerfectApsDetail();
                            //完善计划产品出生证表
                            PerfectPlanProductInfo();
                            //转档  
                            CntLogicTurn();
                            FrmDialog.ShowDialog(this, $"产品{ProductIDTxt.Text}上线成功!", "上线成功");
                            //刷新界面数据
                            RegetProcedureTasksDetails();
                            return;
                        }
                    }

                    //先判断一下本产品出生证的有没有待检验的前序质检任务没做
                    var hasSelfQcTask = HasSelfQcTask();
                    //如果确实有前序质检任务未完成, 就干死他
                    if (hasSelfQcTask)
                    {
                        return;
                    }
                    //判断首件
                    bool hasFirstRecord = HasFirstRecord(out var abortMessage);
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
                    if (KittingConfirm())
                    {
                        //操作人员确认
                        if (WorkerConfirm())
                        {
                            //转档  工序任务明细表=>产品加工过程表
                            ProcessTurnArchives();

                            //判断是否抽检
                            var isChecked = IsChecked();
                            if (!hasFirstRecord && isChecked)
                            {
                                AddFirstRecord();
                            }
                            //如果有首件记录并且抽检为是的话
                            else if ((hasFirstRecord && isChecked) || (!hasFirstRecord && !isChecked))
                            {
                                //获取紧前产品
                                var cProductProcessingDocument = GetPrevProduct();
                                if (cProductProcessingDocument != null)
                                {
                                    //根据紧前产品获得紧前产品的apsdetail
                                    var apsProcedureTaskDetail = GetPrevApsDetail(cProductProcessingDocument);
                                    if (apsProcedureTaskDetail.IsChecked == 1)
                                    {
                                        //apsdetail获得送检结果
                                        var cCheckProcessingDocument = GetCheckResult(apsProcedureTaskDetail);

                                        if (cCheckProcessingDocument == null)
                                        {
                                            //应该要回滚前面做的   工序任务明细表=>产品加工过程表
                                            BackProcessTurnArchives();
                                            FrmDialog.ShowDialog(this, "该工序紧前产品未质检,请先执行质检任务!");
                                            ClearMainPanelTxt();
                                            return;
                                        }
                                        if (cCheckProcessingDocument != null)
                                        {
                                            if (cCheckProcessingDocument.Offline_type == (int?)OfflineType.NG)
                                            {
                                                bool changeEquipmentOrNot = ChangeEquipmentOrNot();
                                                //如果确实要改变加工中心的话 , 就不做下面的任何操作了
                                                if (changeEquipmentOrNot)
                                                {
                                                    BackProcessTurnArchives();
                                                    DelCntPointProcessing();
                                                    //可能还要做什么修改aps表,修改apsdetail表, 改那个设备编号什么的
                                                    //另外还要删除上面做的任何一步,例如删除控制点的添加/删除产品加工过程表里的该元组
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //完善工序任务明细表中的数据 诸如任务状态 ; 修改人修改时间
                            PerfectApsDetail();
                            //完善计划产品出生证表
                            PerfectPlanProductInfo();
                            //转档  
                            CntLogicTurn();
                            FrmDialog.ShowDialog(this, $"产品{ProductIDTxt.Text}上线成功!", "上线成功");
                            //刷新界面数据
                            RegetProcedureTasksDetails();
                        }
                    }
                }
            }
        }
        private void DelCntPointProcessing()
        {
            using (var context = new Model())
            {
                var cBWuECntlLogicPro = context.C_BWuE_CntlLogicPro.FirstOrDefault(s =>
                    s.ProductBornCode == _productBornCode &&
                    s.ProcedureCode == _currentTask.ProcedureCode);
                context.C_BWuE_CntlLogicPro.Remove(cBWuECntlLogicPro);
                context.SaveChanges();
            }
        }


        //回滚 删除过程表信息
        public void BackProcessTurnArchives()
        {
            using (var context = new Model())
            {
                var cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s =>
                    s.EquipmentID == _currentTask.EquipmentID && s.ProductBornCode == _productBornCode &&
                    s.ProcedureCode == _currentTask.ProcedureCode);
                context.C_ProductProcessing.Remove(cProductProcessing);
                context.SaveChanges();
            }
        }

        //判断是否有本产品自身的质检未做完的(三坐标)
        public bool HasSelfQcTask()
        {
            using (var context = new Model())
            {
                var any = context.C_CheckTask.Any(s =>
                    s.CheckType == (decimal?)CheckType.ThreeCoordinate && s.IsAvailable == true && s.TaskState == (decimal?)CheckTaskState.NotOnline &&
                    s.ProductBornCode == _productBornCode && s.PlanCode == _currentTask.PlanCode);
                if (any)
                {
                    FrmDialog.ShowDialog(this, "该产品前序质检任务未完成,请先完成前序质检任务");
                }
                return any;
            }
        }

        //判断主管是否需要在上件NG的时候更改设备加工工件
        public bool ChangeEquipmentOrNot()
        {
            using (var context = new Model())
            {
                var cInfomationPushProcessing = new C_InfomationPushProcessing
                {
                    //这里需要改动 这里我不知道要推送给哪个主管
                    StaffID = 1,
                    PushCategory = "等级异常",
                    InitPushRankPushRank = "1",
                    PushContent = "机加环节人员操作等级未达要求",
                    CreateType = "现场发起",
                    PushState = 1,
                    CreateTime = context.GetServerDate(),
                    CreatorID = _staffId
                };
                context.C_InfomationPushProcessing.Add(cInfomationPushProcessing);
                context.SaveChanges();
            }
            return false;
        }


        public C_CheckProcessingDocument GetCheckResult(APS_ProcedureTaskDetail apsProcedureTaskDetail)
        {
            using (var context = new Model())
            {
                //在检验档案表中根据计划号/产品出生证/工序号/检验类型(三坐标) 获得数据
                var cCheckProcessingDocument = context.C_CheckProcessingDocument.FirstOrDefault(s =>
                    s.PlanCode == _currentTask.PlanCode &&
                    s.ProductBornCode == apsProcedureTaskDetail.ProductBornCode &&
                    s.ProcedureCode == apsProcedureTaskDetail.ProcedureCode &&
                    s.CheckType == (int?)CheckType.ThreeCoordinate);
                return cCheckProcessingDocument;
            }
        }

        public APS_ProcedureTaskDetail GetPrevApsDetail(C_ProductProcessingDocument cProductProcessingDocument)
        {
            using (var context = new Model())
            {
                //在aps工序明细表根据设备编号/有效性/产品出生证/工序编号/任务状态(已完成) 获得紧前产品的apsdetail
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.EquipmentID == cProductProcessingDocument.EquipmentID
                    && s.IsAvailable == true &&
                    s.ProductBornCode == cProductProcessingDocument
                        .ProductBornCode && s.ProcedureCode == cProductProcessingDocument.ProcedureCode &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.Completed);
                return apsProcedureTaskDetail;
            }
        }

        public C_ProductProcessingDocument GetPrevProduct()
        {
            using (var context = new Model())
            {
                //在产品加工档案表中 根据设备号/计划号/产品id/工序号/下线时间排序 获取紧前产品
                var cProductProcessingDocument = context.C_ProductProcessingDocument.Where(s =>
                    s.EquipmentID.ToString() == _equipmentId && s.ProductID == _currentTask.ProductID &&
                    s.ProcedureCode == _currentTask.ProcedureCode && s.PlanCode == _currentTask.PlanCode
                ).OrderByDescending(s => s.OfflineTime).FirstOrDefault();
                return cProductProcessingDocument;
            }
        }

        //完善计划产品出生证表中的状态(任务状态/修改时间/修改人)
        public void PerfectPlanProductInfo()
        {
            using (var context = new Model())
            {
                //在计划产品出生证表中根据产品出生证/上线状态/有效性/计划编号 获得元数据
                var aPlanProductInflammations = context.A_PlanProductInfomation.FirstOrDefault(s =>
                    s.ProductBornCode == _productBornCode && s.State == (decimal?)PlanProductInfoState.NotOnline && s.IsAvailable == true &&
                    s.PlanCode == _currentTask.PlanCode);
                if (aPlanProductInflammations != null)
                {
                    aPlanProductInflammations.State = (int?)PlanProductInfoState.InExcecution; //状态:执行中
                    aPlanProductInflammations.LastModifiedTime = context.GetServerDate();
                    aPlanProductInflammations.ModifierID = _staffId;
                }
                context.SaveChanges();
            }
        }

        //完善aps工序明细表 任务状态/修改人/修改时间
        public void PerfectApsDetail()
        {
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证获取元数据
                var cProductProcessing = context.C_ProductProcessing.First(s => s.ProductBornCode == _productBornCode);
                //在aps工序明细表中根据产品出生证/设备编号/工序编号/有效性/任务状态(未上线)
                var apsProcedureTaskDetail = context.APS_ProcedureTaskDetail.First(s =>
                    s.EquipmentID == cProductProcessing.EquipmentID &&
                    s.ProductBornCode == cProductProcessing.ProductBornCode &&
                    s.ProcedureCode == cProductProcessing.ProcedureCode && s.IsAvailable == true &&
                    s.TaskState == (decimal?)ApsProcedureTaskDetailState.NotOnline);
                apsProcedureTaskDetail.TaskState = (int?)ApsProcedureTaskDetailState.InExcecution; //正在进行中
                apsProcedureTaskDetail.LastModifiedTime = context.GetServerDate();
                apsProcedureTaskDetail.ModifierID = _staffId.ToString();
                context.SaveChanges();
            }
        }

        //产品上线的控制点转档
        public void CntLogicTurn()
        {
            using (var context = new Model())
            {
                //在控制点过程表中 根据产品出生证 工序编号 控制点id 设备编号(需要修改) 查到相关集合
                var cBWuECntlLogicPros = context.C_BWuE_CntlLogicPro.Where(s =>
                        s.ProductBornCode == _productBornCode && s.ProcedureCode == _currentTask.ProcedureCode
                                                              && s.ControlPointID == 1 && s.EquipmentCode == _equipmentCode)
                    .OrderByDescending(s => s.StartTime).ToList();
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

        //添加产品上线逻辑控制点
        public void AddCntLogicPro()
        {
            //录入进控制点过程表  哪个产品在哪个工序哪个控制点正在进行
            using (var context = new Model())
            {
                var cBWuECntlLogicPro = new C_BWuE_CntlLogicPro();
                cBWuECntlLogicPro.ProductBornCode = _productBornCode;
                cBWuECntlLogicPro.ProcedureCode = _currentTask.ProcedureCode;
                cBWuECntlLogicPro.ControlPointID = 1;
                cBWuECntlLogicPro.Sort = "1";
                cBWuECntlLogicPro.EquipmentCode = _equipmentCode;
                cBWuECntlLogicPro.State = "1";
                cBWuECntlLogicPro.StartTime = context.GetServerDate();

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
                    var dialogResult = FrmDialog.ShowDialog(this, "加工人员与规定人员不一致,是否继续生产", "人员不匹配", true);
                    if (dialogResult == DialogResult.OK)
                    {
                        if (!IsHighLevel())
                        {
                            var cInformationPushProcessing = new C_InfomationPushProcessing
                            {
                                //这里需要改动
                                // PushID = "push001",
                                PushCategory = "等级异常",
                                InitPushRankPushRank = "1",
                                PushContent = "机加环节人员操作等级未达要求",
                                CreateType = "现场发起",
                                PushState = 1,
                                CreateTime = context.GetServerDate(),
                                CreatorID = _staffId
                            };

                            context.C_InfomationPushProcessing.Add(cInformationPushProcessing);
                            context.SaveChanges();

                            BeginInvoke(new Action((() =>
                                FrmDialog.ShowDialog(this, "很抱歉,您的操作等级未能达到要求,已将消息推送至主管", "等级异常"))));
                            ClearTxt();
                            return false;
                        }
                        return true;
                    }
                    ClearTxt();
                    return false;
                }
                return true;
            }
        }


        private bool IsHighLevel()
        {
            bool b = false;
            using (var context = new Model())
            {
                var cStaffBaseInformation = context.C_StaffBaseInformation.FirstOrDefault(s =>
                    s.StaffCode == _staffCode && s.StaffName == _staffName && s.IsAvailable == true);
                int.TryParse(cStaffBaseInformation?.SkillGrade, out var result);
                var strings = _currentTask.WorkerCode.Split(',');
                foreach (var s1 in strings)
                {
                    var staffBaseInformation = context.C_StaffBaseInformation.FirstOrDefault(s => s.StaffCode == s1 && s.IsAvailable == true);
                    if (staffBaseInformation != null)
                    {
                        int.TryParse(staffBaseInformation.SkillGrade, out var result1);
                        if (result < result1)
                        {
                            return false;
                        }
                        if (result >= result1)
                        {
                            b = true;
                        }
                    }
                }
            }
            //这里需要改动
            return b;
        }

        //齐套确认
        public bool KittingConfirm()
        {
            using (var context = new Model())
            {
                //先从工序基础表中 根据工序编号判断是否有齐套要求  ????当前产品生产绑定在特定的计划下 是否只需要查找该计划是否齐套就ok了????
                var aProcedureBase = context.A_ProcedureBase.FirstOrDefault(s => s.ProcedureCode == _currentTask.ProcedureCode);

                var aMaterialProgramDemand = context.A_MaterialProgramDemand.Where(s =>
                    s.ProjectID == _currentTask.ProjectID && s.PlanCode == _currentTask.PlanCode).ToList();

                var materialProgramAll = aMaterialProgramDemand.All(s => s.Cstate == (decimal?)CstateState.Yes);

                var aCutterDemands = context.A_CutterDemand.Where(s =>
                    s.ProjectCode == _currentTask.ProjectCode && s.PlanCode == _currentTask.PlanCode).ToList();

                var cutterAll = aCutterDemands.All(s => s.Cstate == (decimal?)CstateState.Yes);

                if (!materialProgramAll || !cutterAll)
                {
                    DialogResult dialogResult;
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
                    return false;
                }
                return true;
            }
        }

        public bool HasFirstRecord(out string abortMessage)
        {
            using (var context = new Model())
            {
                var cProcedureFirstRecords = context.C_ProcedureFirstDocument.Where
                (s => s.PlanCode == _currentTask.PlanCode &&
                      s.EquipmentID.ToString() == _equipmentId && s.ProductID == _currentTask.ProductID
                      && s.ProcedureCode == _currentTask.ProcedureCode).ToList();
                if (cProcedureFirstRecords.Any())
                {
                    if (cProcedureFirstRecords.Any(s => s.ProcedureFirstStatus == 0))
                    {
                        FrmDialog.ShowDialog(this, "该工序首件未质检,请先执行质检任务");
                        abortMessage = "y";
                        return true;
                    }

                    var any = cProcedureFirstRecords.Any(s => s.ProcedureFirstStatus == (int)ProcedureFirstStatus.Qualified);
                    if (any)
                    {
                        abortMessage = "n";
                        //如果存在 但是有一条是合格的
                        return true;
                    }
                    //上件ng的话就把这件当作首件的逻辑
                    bool changeEquipmentOrNot = ChangeEquipmentOrNot();
                    //如果确实要改变加工中心的话 , 就不做下面的任何操作了
                    if (changeEquipmentOrNot)
                    {
                        BackProcessTurnArchives();
                        DelCntPointProcessing();
                        //可能还要做什么修改aps表,修改apsdetail表, 改那个设备编号什么的/计划号可能也会修改/删除控制点呀什么的
                        abortMessage = "y and change";
                        return false;
                    }
                    //不修改加工中心的话 就把下一件继续当作首件,并且继续在该加工中心进行加工
                    abortMessage = "no";
                    return false;

                    //如果所有的都不是合格的   那么就要添加首件记录  前提是先判断一下是否要转移加工设备
                    // AddFirstRecord();
                    // return false;
                }
                abortMessage = "n";
                //记得补充代码
                //如果一条也没有的话  就要添加首件记录
                // AddFirstRecord();
                return false;
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
                cProcedureFirstRecord.EquipmentName = _equipmentName;

                context.C_ProcedureFirstRecord.Add(cProcedureFirstRecord);
                context.SaveChanges();
            }
        }

        public C_ProductProcessing ProcessTurnArchives()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.CreateMap<APS_ProcedureTask, C_ProductProcessing>());
            var mapper = mapperConfiguration.CreateMapper();
            var cProductProcessing = mapper.Map<C_ProductProcessing>(_currentTask);
            int.TryParse(_workshopId, out var a);
            cProductProcessing.WorkshopID = a;
            cProductProcessing.WorkshopCode = _workshopCode;
            cProductProcessing.WorkshopName = _workshopName;

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
                cProductProcessing.EquipmentName = _equipmentName;
                cProductProcessing.OnlineStaffID = _staffId;
                cProductProcessing.OnlineStaffCode = _staffCode;
                cProductProcessing.OnlineStaffName = _staffName;
                cProductProcessing.OnlineTime = context.GetServerDate();

                //上线类型判断

                context.C_ProductProcessing.Add(cProductProcessing);
                context.SaveChanges();

                DisplayInfoToMainPanel(cProductProcessing.ProductBornCode, cProductProcessing.ProductName,
                    cProductProcessing.ProcedureName, cProductProcessing.OnlineTime.ToString());
                ChangeBgColor();

                Close();
                Dispose();
                return cProductProcessing;
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
