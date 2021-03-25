using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using HZH_Controls;
using HZH_Controls.Forms;
using MachineryProcessingDemo.helper;
using MachineryProcessingDemo.Properties;
using Microsoft.Extensions.Configuration;

namespace MachineryProcessingDemo.Forms
{
    public partial class UserLoginForm : FrmBase
    {
        private static string _workshopId;
        private static string _workshopCode;
        private static string _workshopName;
        private static string _equipmentId;
        private static string _equipmentCode;
        private static string _equipmentName;

        public UserLoginForm()
        {
            InitializeComponent();
            this.IsFullSize = false;
            PwdTxt.PasswordChar = '*';
        }

        private void UserLoginForm_Load(object sender, EventArgs e)
        {

            using (var context = new Model())
            {
                // FileStream fs = new FileStream("E:\\project\\visual Studio Project\\MachineryProcessingDemo\\Resources\\timi.png", FileMode.Open);
                // byte[] imgSourse = new byte[fs.Length];
                // fs.Read(imgSourse, 0, imgSourse.Length);
                // fs.Close();
                //
                // var aProductBase = new A_ProductBase();
                // var timi = Resources.timi;
                // var memoryStream = new MemoryStream();
                // timi.Save(memoryStream,ImageFormat.Png);
                // var buffer = memoryStream.ToArray();
                //
                //
                // string sql = "update A_ProductBase set Image=@Image where ID=11";
                // var sqlParameter = new SqlParameter("@Image", buffer);
                // var executeSqlCommand = context.Database.ExecuteSqlCommand(sql,new []{sqlParameter});
                //
                // aProductBase.Image = buffer;
                // context.A_ProductBase.Add(aProductBase);
                // context.SaveChanges(); 
            }
            var addXmlFile = new ConfigurationBuilder().SetBasePath(GlobalClass.Xml)
                .AddXmlFile("config.xml");
            var configuration = addXmlFile.Build();
            _workshopId = configuration["WorkshopID"];
            _workshopCode = configuration["WorkshopCode"];
            _workshopName = configuration["WorkshopName"];
            _equipmentId = configuration["EquipmentID"];
            _equipmentCode = configuration["EquipmentCode"];
            _equipmentName = configuration["EquipmentName"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = "";
            var md5 = MD5.Create();
            var computeHash = md5.ComputeHash(Encoding.UTF8.GetBytes(PwdTxt.ToString()));
            foreach (var b in computeHash)
            {
                pwd += b.ToString();
            }

            using (var context = new Model())
            {
                // var connectionState = context.Database.Connection.State;
                // if (connectionState==ConnectionState.Closed||connectionState==ConnectionState.Broken)
                // {
                //     FrmDialog.ShowDialog(this, "服务器异常,请校正服务器状态后重试"); 
                //     return; 
                // }
                var cStaffBaseInformation = context.C_StaffBaseInformation.FirstOrDefault(s => s.Account == AccountTxt.Text);
                if (cStaffBaseInformation != null)
                {
                    if (cStaffBaseInformation.Password == pwd)
                    {
                        FrmTips.ShowTips(null, "登陆成功,欢迎使用!", 2000, false, ContentAlignment.BottomCenter, null,
                            TipsSizeMode.Medium, new Size(300, 50), TipsState.Success);
                        // FrmDialog.ShowDialog(this, "登陆成功,欢迎使用!", "登陆成功");
                        this.Hide();

                        string strIp = "";
                        foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                        {
                            strIp = ipAddress.ToString();
                        }

                        if (!cStaffBaseInformation.Account.Contains("admin"))
                        {
                            var cEquipmentInfomation = context.C_EquipmentInfomation.FirstOrDefault(s => s.IsAvailable == true && s.IsEnable == true && s.EquipmentCode.ToString() == _equipmentCode);
                            _equipmentId = cEquipmentInfomation.ID.ToString();

                            int.TryParse(_equipmentId, out var result);
                            //在登陆过程表中插入数据
                            var cLoginInProcessing = new C_LoginInProcessing()
                            {
                                StaffCode = cStaffBaseInformation.StaffCode,
                                StaffID = cStaffBaseInformation.StaffID,
                                StaffName = cStaffBaseInformation.StaffName,
                                OnlineTime = context.GetServerDate(),
                                //设备id???
                                EquipmentID = result,
                                EquipmentName = _equipmentName,
                                IP = strIp,
                                Remarks = "测试数据"
                            };

                            context.C_LoginInProcessing.Add(cLoginInProcessing);
                            context.SaveChanges();
                        }

                        new MainPanel(cStaffBaseInformation.StaffID, cStaffBaseInformation.StaffCode, cStaffBaseInformation.StaffName).Show();
                    }
                    else
                    {
                        // FrmTips.ShowTips(this, "密码错误,请重试!", 1000, false, ContentAlignment.MiddleCenter, null,
                        //     TipsSizeMode.Medium, new Size(300, 50), TipsState.Error);
                        FrmDialog.ShowDialog(this, "密码错误,请重试!", "登陆失败");
                    }
                }
                else
                {
                    FrmDialog.ShowDialog(this, "该用户不存在!", "登陆失败");
                }
            }
        }
    }
}
