using HZH_Controls;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MachineryProcessingDemo
{
    public class UCTestGridTable_CustomCellIcon : UserControl, HZH_Controls.Controls.IDataGridViewCustomCell
    {
        public UCTestGridTable_CustomCellIcon()
        {
            InitializeComponent();
        }
        private APS_ProcedureTaskDetail m_object = null;
        public object DataSource
        {
            get
            {
                return m_object;
            }
        }
        public void SetBindSource(object obj)
        {
            if (obj is APS_ProcedureTaskDetail apsProcedureTaskDetail)
            {
                m_object = (APS_ProcedureTaskDetail)obj;
                using (var context = new Model())
                {
                    var apsProcedureTask = context.APS_ProcedureTask.FirstOrDefault(s => s.ID == apsProcedureTaskDetail.TaskTableID && s.IsAvailable == true);
                    if (apsProcedureTask != null)
                    {
                        var imageBytes = context.A_ProductBase.FirstOrDefault(s => s.ProductCode == apsProcedureTask.ProductCode && s.IsAvailable == true)?.Image;
                        var memoryStream = new MemoryStream(imageBytes);
                        var fromStream = Image.FromStream(memoryStream);
                        this.BackgroundImage = fromStream;
                        this.BackgroundImageLayout = ImageLayout.Zoom; 
                    }
                }
                // string strIcon = "E_icon_star";
                // ForeColor = Color.FromArgb(255, 77, 59);
                // FontIcons icon1 = (FontIcons)Enum.Parse(typeof(FontIcons), strIcon);
                // // Image = FontImages.GetImage(icon1, 32, Color.FromArgb(255, 77, 59));
                // this.BackgroundImage = FontImages.GetImage(icon1, 32, Color.FromArgb(255, 77, 59));
                // // this.BackgroundImage = Properties.Resources.rowicon;
            }
        }


        public event HZH_Controls.Controls.DataGridViewRowCustomEventHandler RowCustomEvent;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UCTestGridTable_CustomCellIcon
            // 
            this.Name = "UCTestGridTable_CustomCellIcon";
            this.Size = new System.Drawing.Size(37, 38);
            this.ResumeLayout(false);

        }
    }
}
