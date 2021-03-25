
namespace MachineryProcessingDemo.Forms
{
    partial class WorkerGauge
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            serialPort1.Close();
            serialPort1.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ProductType = new System.Windows.Forms.Label();
            this.BadReasonLbl = new System.Windows.Forms.Label();
            this.ToolingIdTxt = new System.Windows.Forms.TextBox();
            this.ToolingNameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ToolingTypeTxt = new System.Windows.Forms.TextBox();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.BadReasonLbl);
            this.panel3.Controls.Add(this.ProductType);
            this.panel3.Controls.Add(this.ToolingTypeTxt);
            this.panel3.Controls.Add(this.ToolingNameTxt);
            this.panel3.Controls.Add(this.ToolingIdTxt);
            this.panel3.Size = new System.Drawing.Size(553, 250);
            // 
            // ProductType
            // 
            this.ProductType.AutoSize = true;
            this.ProductType.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ProductType.ForeColor = System.Drawing.Color.DodgerBlue;
            this.ProductType.Location = new System.Drawing.Point(154, 81);
            this.ProductType.Name = "ProductType";
            this.ProductType.Size = new System.Drawing.Size(112, 26);
            this.ProductType.TabIndex = 8;
            this.ProductType.Text = "工量具编号:";
            // 
            // BadReasonLbl
            // 
            this.BadReasonLbl.AutoSize = true;
            this.BadReasonLbl.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BadReasonLbl.ForeColor = System.Drawing.Color.DodgerBlue;
            this.BadReasonLbl.Location = new System.Drawing.Point(155, 142);
            this.BadReasonLbl.Name = "BadReasonLbl";
            this.BadReasonLbl.Size = new System.Drawing.Size(112, 26);
            this.BadReasonLbl.TabIndex = 8;
            this.BadReasonLbl.Text = "工量具名称:";
            // 
            // ToolingIdTxt
            // 
            this.ToolingIdTxt.BackColor = System.Drawing.Color.White;
            this.ToolingIdTxt.Location = new System.Drawing.Point(270, 78);
            this.ToolingIdTxt.Name = "ToolingIdTxt";
            this.ToolingIdTxt.Size = new System.Drawing.Size(143, 32);
            this.ToolingIdTxt.TabIndex = 1;
            // 
            // ToolingNameTxt
            // 
            this.ToolingNameTxt.BackColor = System.Drawing.Color.White;
            this.ToolingNameTxt.Location = new System.Drawing.Point(270, 139);
            this.ToolingNameTxt.Name = "ToolingNameTxt";
            this.ToolingNameTxt.Size = new System.Drawing.Size(143, 32);
            this.ToolingNameTxt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(154, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "工量具类型:";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // ToolingTypeTxt
            // 
            this.ToolingTypeTxt.BackColor = System.Drawing.Color.White;
            this.ToolingTypeTxt.Location = new System.Drawing.Point(270, 198);
            this.ToolingTypeTxt.Name = "ToolingTypeTxt";
            this.ToolingTypeTxt.Size = new System.Drawing.Size(143, 32);
            this.ToolingTypeTxt.TabIndex = 3;
            // 
            // WorkerGauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumAquamarine;
            this.ClientSize = new System.Drawing.Size(553, 374);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "WorkerGauge";
            this.Text = "ScanOfflineForm";
            this.Title = "工量具绑定";
            this.Load += new System.EventHandler(this.ScanOfflineForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label ProductType;
        private System.Windows.Forms.Label BadReasonLbl;
        private System.Windows.Forms.TextBox ToolingNameTxt;
        private System.Windows.Forms.TextBox ToolingIdTxt;
        private System.Windows.Forms.Label label1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox ToolingTypeTxt;
    }
}