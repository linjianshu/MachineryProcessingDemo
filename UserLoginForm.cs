using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Forms;

namespace MachineryProcessingDemo
{
    public partial class UserLoginForm : FrmBase
    {
        public UserLoginForm()
        {
            InitializeComponent();
            this.IsFullSize = false;
            PwdTxt.PasswordChar = '*'; 
        }

        private void UserLoginForm_Load(object sender, EventArgs e)
        {

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
                var cStaffBaseInformation = context.C_StaffBaseInformation.FirstOrDefault(s => s.Account==AccountTxt.Text);
                if (cStaffBaseInformation!=null)
                {
                    if (cStaffBaseInformation.Password==pwd)
                    {
                        FrmDialog.ShowDialog(this, "登陆成功,欢迎使用!", "登陆成功");
                        // MessageBox.Show("登陆成功,欢迎使用!");
                        this.Hide();

                        string strIp = ""; 
                        foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                        {
                                strIp = ipAddress.ToString(); 
                        }
                        //在登陆过程表中插入数据
                        var cLoginInProcessing = new C_LoginInProcessing()
                        {
                            StaffCode = cStaffBaseInformation.StaffCode,
                            StaffID = cStaffBaseInformation.StaffID,
                            StaffName = cStaffBaseInformation.StaffName,
                            OnlineTime = DateTime.Now,
                            //设备id???
                            EquipmentID = 1,
                            EquipmentName = "车车中心",
                            IP = strIp,
                            Remarks = "测试数据"
                        };

                        context.C_LoginInProcessing.Add(cLoginInProcessing);
                        context.SaveChanges(); 

                        new MainPanel(cLoginInProcessing.StaffID,cLoginInProcessing.StaffCode,cLoginInProcessing.StaffName).Show();
                    }
                    else
                    {
                        FrmDialog.ShowDialog(this, "密码错误,请重试!", "登陆失败");
                        // MessageBox.Show("密码错误,请重试!"); 
                    }
                }
                else
                {
                    FrmDialog.ShowDialog(this, "该用户不存在!", "登陆失败");
                    // MessageBox.Show("该用户不存在!");
                }
            }
        }
    }
}
