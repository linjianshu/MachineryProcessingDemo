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
    public partial class WorkerGauge : FrmWithOKCancel1
    {
        public WorkerGauge(long? staffId, string staffCode, string staffName)
        {
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
        private static Base_DataDictionaryDetail _baseDataDictionaryDetail;
        private static KitProcessingDocument _kitProcessingDocument;
        public Action RegetAction; 
        private void ScanOfflineForm_Load(object sender, EventArgs e)
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

            DataFill();

            if (serialPort1.IsOpen) { serialPort1.Close(); }
            string portName = ConfigAppSettingsHelper.ReadSetting("PortName");
            string baudRate = ConfigAppSettingsHelper.ReadSetting("BaudRate");
            serialPort1.Dispose();//释放扫描枪所有资源
            serialPort1.PortName = portName;
            serialPort1.BaudRate = int.Parse(baudRate);
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.Open();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void DataFill()
        {
            var tuple = new Tuple<string, string>("工量具", "E_icon_tools");
            FontIcons icon2 = (FontIcons)Enum.Parse(typeof(FontIcons), tuple.Item2);

            var pictureBox2 = new PictureBox
            {
                AutoSize = false,
                Size = new Size(40, 40),
                ForeColor = Color.FromArgb(255, 77, 59),
                Image = FontImages.GetImage(icon2, 36, Color.FromArgb(255, 77, 59)),
                Location = new Point(this.Size.Width / 2 - 20, 15)
            };
            panel3.Controls.Add(pictureBox2);
        }

        protected override void DoEnter()
        {
            if (string.IsNullOrEmpty(ToolingIdTxt.Text) || string.IsNullOrEmpty(ToolingNameTxt.Text) || string.IsNullOrEmpty(ToolingTypeTxt.Text))
            {
                FrmDialog.ShowDialog(this, "工量具信息不准确,请重新扫码");
                return;
            }
            var dialogResult = FrmDialog.ShowDialog(this, "是否确认绑定?", "确认", true);
            if (dialogResult == DialogResult.OK)
            {
                BindingTooling();
                FrmDialog.ShowDialog(this, "绑定成功!");
                Close();
            }
            serialPort1.Close();
        }

        protected override void DoEsc()
        {
            serialPort1.Close();
            Close();
        }

        private void BindingTooling()
        {
            using (var context = new Model())
            {
                //判断是否已经绑定过了 最好在扫码的时候就做一下判断
                //有的话就转档 , 没有的话就直接加进来

                //在过程表中找 ,如果有就转档 , 如果没有就算了
                var firstOrDefault = context.C_KitProcessing.FirstOrDefault(s => s.KitCode==_kitProcessingDocument.KitBornCode);
                if (firstOrDefault!=null)
                {
                    var mapperConfiguration1 = new MapperConfiguration(cfg =>
                        cfg.CreateMap<C_KitProcessing, C_KitDocument>());
                    var mapper1 = mapperConfiguration1.CreateMapper();
                    var cKitDocument = mapper1.Map<C_KitDocument>(firstOrDefault);
                    context.C_KitDocument.Add(cKitDocument);
                    context.C_KitProcessing.Remove(firstOrDefault);
                    context.SaveChanges();
                }

                //在可用的工量具产品表中检索 和 映射
                var mapperConfiguration = new MapperConfiguration(cfg =>
                    cfg.CreateMap<KitProcessingDocument, C_KitProcessing>());
                var mapper = mapperConfiguration.CreateMapper();
                var cKitProcessing = mapper.Map<C_KitProcessing>(_kitProcessingDocument);

                cKitProcessing.KitID = _kitProcessingDocument.ID;
                cKitProcessing.KitCode = _kitProcessingDocument.KitBornCode;
                cKitProcessing.ApplianceType = _kitProcessingDocument.ApplicanceType;
                //这里需要修改
                int.TryParse(_equipmentId, out var result); 
                cKitProcessing.PositionID = result;
                cKitProcessing.PositionCode = _equipmentCode;
                cKitProcessing.PositionName =_equipmentName;
                cKitProcessing.PositionType = (int?) PositionType.Machining;
                cKitProcessing.StaffName = _staffName;
                cKitProcessing.StaffCode = _staffCode;
                context.C_KitProcessing.Add(cKitProcessing);
                context.SaveChanges();
            }
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var receivedData = GetDataFromSerialPort(serialPort1);

            EnrichTextbox(receivedData);
        }

        private void EnrichTextbox(string receivedData)
        {
            using (var context = new Model())
            {
                var kitProcessingDocument = context.KitProcessingDocument.FirstOrDefault(s =>
                    s.KitBornCode == receivedData && s.IsAvailable == true);
                if (kitProcessingDocument != null)
                {
                    _kitProcessingDocument = kitProcessingDocument;
                    string kitType = _kitProcessingDocument.ApplicanceType == 1 ? "工具" : "量具";
                    BeginInvoke(new Action((() =>
                    {
                        ToolingIdTxt.Text = _kitProcessingDocument.KitBornCode;
                        ToolingNameTxt.Text = _kitProcessingDocument.KitName;
                        ToolingTypeTxt.Text = kitType;
                    })));
                }
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
    }
}
