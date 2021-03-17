using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using HZH_Controls.Forms;
using MachineryProcessingDemo.helper;
using Microsoft.Extensions.Configuration;
using QualityCheckDemo;

namespace MachineryProcessingDemo.Forms
{
    public partial class SelfCheckItemForm : FrmWithOKCancel1
    {
        public SelfCheckItemForm(string productBornCode, long? staffId, string staffCode, string staffName)
        {
            InitializeComponent();
            _productBornCode = productBornCode;
            _staffId = staffId;
            _staffCode = staffCode;
            _staffName = staffName;
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
        private static List<A_ProcedureSelfCheckingConfig> _aProcedureSelfCheckingConfigs;
        public static List<string> _txtNameList = new List<string>();
        public static List<string> _mbNameList = new List<string>();
        public Action ChangeBgColor;
        private void SelfCheckItemForm_Load(object sender, EventArgs e)
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
            using (var context = new Model())
            {
                //在产品加工过程表中根据产品出生证  获取元数据
                _cProductProcessing = context.C_ProductProcessing.FirstOrDefault(s => s.ProductBornCode == _productBornCode);

                if (_cProductProcessing == null)
                {
                    FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                    return;
                }
                var procedureIdInt16 = Convert.ToInt16(_cProductProcessing.ProcedureID);

                //在工序自检项配置表中根据工序主键/是否启用/有效性  获得自检项数据
                //类型转换问题  数据库设计有误吗???
                _aProcedureSelfCheckingConfigs = context.A_ProcedureSelfCheckingConfig.Where(s =>
                    s.ProcedureID == procedureIdInt16 &&
                    s.IsEnable == true && s.IsAvailable == true).OrderByDescending(s => s.IsRequired).ToList();

                //动态加载txt和lbl控件
                int tabIndex = 1;
                int localLblY = 23;
                int localTextBoxY = 20;
                foreach (var aProcedureSelfCheckingConfig in _aProcedureSelfCheckingConfigs)
                {
                    var label = new Label();
                    if (tabIndex % 3 == 2)
                    {
                        label.Location = new Point(310, localLblY);
                    }
                    else if (tabIndex%3==0)
                    {
                        label.Location = new Point(550, localLblY);
                    }
                    else
                    {
                        label.Location = new Point(80, localLblY);
                    }
                    label.Size = new Size(70, 26);
                    label.Text = aProcedureSelfCheckingConfig.ItemName + ':';
                    label.ForeColor = Color.Black;
                    label.TabIndex = tabIndex;
                    label.BackColor = Color.Transparent;
                    label.Font = new Font("微软雅黑", 10.8F, FontStyle.Bold,
                        GraphicsUnit.Point, ((byte)(134)));
                    label.Name = aProcedureSelfCheckingConfig.ItemCode + "lbl";
                    panel3.Controls.Add(label);

                  
                    if (tabIndex % 3 == 0)
                    {
                        localLblY += 60;
                    }
                    // localLblY += 60;

                    var textBox = new TextBox();
                    if (tabIndex % 3 == 2)
                    {
                        textBox.Location = new Point(390, localTextBoxY);
                    }
                    else if (tabIndex % 3 == 0)
                    {
                        textBox.Location = new Point(620, localTextBoxY);
                    }
                    else
                    {
                        textBox.Location = new Point(160, localTextBoxY);
                    }
                    // textBox.Location = new Point(130, localTextBoxY);
                    textBox.Size = new Size(130, 32);
                    textBox.Name = aProcedureSelfCheckingConfig.ItemCode + "txt";
                    panel3.Controls.Add(textBox);

                    if (tabIndex % 3 == 0)
                    {
                        localTextBoxY += 60;
                    }
                    // localTextBoxY += 60;
                    _txtNameList.Add(aProcedureSelfCheckingConfig.ItemCode + "txt");

                    tabIndex++;


                    if (aProcedureSelfCheckingConfig.IsRequired != null && (bool)aProcedureSelfCheckingConfig.IsRequired)
                    {
                        panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "lbl"}"].Text =
                            panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "lbl"}"].Text
                            .Insert(panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "lbl"}"].Text.Length - 1, "*");
                        panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "lbl"}"].ForeColor = Color.Red;
                        panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "txt"}"].Tag = "*";
                        _mbNameList.Add(aProcedureSelfCheckingConfig.ItemCode);
                    }

                    //在产品质量数据表中根据 订单号项目号计划好产品号出生证工序号检验类型(自检)及检测项编号  获得特定数据(检测实际值)
                    var cProductQualityData = context.C_ProductQualityData.FirstOrDefault(s =>
                        s.OrderID == _cProductProcessing.OrderID && s.ProjectID == _cProductProcessing.ProjectID && s.PlanID == _cProductProcessing.PlanID
                        && s.ProductID == _cProductProcessing.ProductID && s.ProductBornCode == _cProductProcessing.ProductBornCode &&
                        s.ProcedureID == _cProductProcessing.ProcedureID &&
                        s.CheckType == (decimal?)CheckType.SelfCheck &&
                        s.ItemCode == aProcedureSelfCheckingConfig.ItemCode);

                    if (cProductQualityData != null && cProductQualityData.CollectValue != null)
                        panel3.Controls[$"{aProcedureSelfCheckingConfig.ItemCode + "txt"}"].Text = cProductQualityData.CollectValue.ToString();
                }
            }
        }

        private void AddCntLogicPro()
        {
            //添加控制点过程信息
            using (var context = new Model())
            {
                var cBWuECntlLogicPro = new C_BWuE_CntlLogicPro();
                cBWuECntlLogicPro.ProductBornCode = _productBornCode;
                cBWuECntlLogicPro.ProcedureCode = _cProductProcessing.ProcedureCode;
                cBWuECntlLogicPro.ControlPointID = 2;
                cBWuECntlLogicPro.Sort = "2";
                cBWuECntlLogicPro.EquipmentCode = _equipmentCode;
                cBWuECntlLogicPro.State = "1";
                cBWuECntlLogicPro.StartTime = context.GetServerDate();

                context.C_BWuE_CntlLogicPro.Add(cBWuECntlLogicPro);
                context.SaveChanges();
            }
        }

        protected override void DoEnter()
        {
            if (_cProductProcessing == null)
            {
                FrmDialog.ShowDialog(this, "未检测到上线产品", "警告");
                return;
            }

            //添加控制点过程信息
            AddCntLogicPro();

            //数据校验
            if (DataCheck())
            {
                //录入自检项数据
                InputSelfItem();

                //控制点转档
                // CntLogicTurn();
            }
        }

        private void CntLogicTurn()
        {
            using (var context = new Model())
            {
                //在控制点过程表中 根据产品出生证 工序编号 控制点id 设备编号(需要修改) 查到相关集合
                var cBWuECntlLogicPros = context.C_BWuE_CntlLogicPro.Where(s =>
                        s.ProductBornCode == _productBornCode && s.ProcedureCode == _cProductProcessing.ProcedureCode
                                                              && s.ControlPointID == 2 && s.EquipmentCode == _equipmentCode)
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

        private void InputSelfItem()
        {
            //判断产品质量数据表中有没有原始数据
            //如果有的话就是更新操作
            using (var context = new Model())
            {
                //在产品质量数据表中根据产品出生证和工序号/检验结果为空 来获得录入过的数据
                var cProductQualityDatas = context.C_ProductQualityData.Where(s =>
                    s.ProductBornCode == _productBornCode && s.ProcedureCode == _cProductProcessing.ProcedureCode
                    && s.CheckResult == null).OrderBy(s => s.ItemID).ToList();
                if (cProductQualityDatas.Count > 0)
                {
                    foreach (var cProductQualityData in cProductQualityDatas)
                    {
                        var trim = panel3.Controls[$"{cProductQualityData.ItemCode}txt"].Text.Trim();
                        decimal.TryParse(trim, out var txtDecimalValue);
                        if (string.IsNullOrEmpty(trim))
                        {
                            cProductQualityData.CollectValue = null;
                        }
                        else
                        {
                            cProductQualityData.CollectValue = txtDecimalValue;
                        }

                        cProductQualityData.CreateTime = context.GetServerDate();
                    }
                    context.SaveChanges();
                    FrmDialog.ShowDialog(this, "自检项录入成功", "录入成功");
                    ChangeBgColor();
                    Close();
                }
                else
                {
                    //构建产品加工过程表和产品质量数据表的映射关系
                    var mapperConfiguration = new MapperConfiguration(cfg =>
                        cfg.CreateMap<C_ProductProcessing, C_ProductQualityData>());

                    //构建工序自检项配置表和产品质量数据表的映射关系
                    var mapperConfiguration1 = new MapperConfiguration(cfg =>
                        cfg.CreateMap<A_ProcedureSelfCheckingConfig, C_ProductQualityData>());

                    foreach (var aProcedureSelfCheckingConfig in _aProcedureSelfCheckingConfigs)
                    {
                        //automapper
                        var mapper1 = mapperConfiguration1.CreateMapper();
                        var cProductQualityData = mapper1.Map<C_ProductQualityData>(aProcedureSelfCheckingConfig);
                        //automapper
                        var mapper = mapperConfiguration.CreateMapper();
                        var productQualityData = mapper.Map<C_ProductProcessing, C_ProductQualityData>(_cProductProcessing, cProductQualityData);

                        //录入值
                        var txtValue = panel3.Controls.Find(aProcedureSelfCheckingConfig.ItemCode + "txt", false).First().Text.Trim();
                        var tryParse = decimal.TryParse(txtValue, out var txtDecimalValue);
                        if (string.IsNullOrEmpty(txtValue))
                        {
                            productQualityData.CollectValue = null;
                        }
                        else if (tryParse)
                        {
                            productQualityData.CollectValue = txtDecimalValue;
                        }
                        else
                        {
                            FrmDialog.ShowDialog(this, "输入数据格式不正确", "格式异常");
                            return;
                        }

                        //(补充信息)
                        productQualityData.CheckType = (int?)CheckType.SelfCheck;
                        productQualityData.ItemID = aProcedureSelfCheckingConfig.ID;
                        productQualityData.CheckStaffCode = _staffCode;
                        productQualityData.CheckStaffName = _staffName;
                        productQualityData.CreateTime = context.GetServerDate();

                        context.C_ProductQualityData.Add(productQualityData);
                    }

                    context.SaveChanges();
                    FrmDialog.ShowDialog(this, "自检项录入成功", "录入成功");
                    ChangeBgColor();
                    Close();
                }
            }
        }

        private bool DataCheck()
        {
            bool isOk = false;

            foreach (var aProcedureSelfCheckingConfig in _aProcedureSelfCheckingConfigs)
            {
                //录入值
                var txtValue = panel3.Controls.Find(aProcedureSelfCheckingConfig.ItemCode + "txt", false).First().Text.Trim();
                var tryParse = decimal.TryParse(txtValue, out var txtDecimalValue);
                if (tryParse || string.IsNullOrEmpty(txtValue))
                {
                    isOk = true;
                }
                else
                {
                    FrmDialog.ShowDialog(this, "输入数据格式不正确", "格式异常");
                    var control = panel3.Controls.Find(aProcedureSelfCheckingConfig.ItemCode + "txt", false).First();
                    var textBox = (TextBox)control;
                    //文本框撤销  树儿nb
                    textBox.Undo();
                    return false;
                }
            }
            return isOk;
        }
    }
}
