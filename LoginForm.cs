using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Forms;

namespace MachineryProcessingDemo
{
    public partial class LoginForm : FrmWithOKCancel1
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            var control = Controls.Find("btnOK",true)[0];
            if (control is HZH_Controls.Controls.UCBtnExt ucBtnExt )
            {
                ucBtnExt.BtnText = "登陆"; 
            }
        }

        protected override void DoEnter()
        {
            MessageBox.Show("helloworld");
        }
    }
}
