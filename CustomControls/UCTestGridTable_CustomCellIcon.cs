using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls;
using MachineryProcessingDemo.Forms;

namespace MachineryProcessingDemo
{
    public class UCTestGridTable_CustomCellIcon : UserControl, HZH_Controls.Controls.IDataGridViewCustomCell
    {
        public UCTestGridTable_CustomCellIcon()
        {
            InitializeComponent();
        }
        private TestGridModel m_object = null;
        public object DataSource
        {
            get
            {
                return m_object;
            }
        }
        public void SetBindSource(object obj)
        {
            if (obj is TestGridModel)
            {
                m_object = (TestGridModel)obj;
                string strIcon = "E_icon_star";
                ForeColor = Color.FromArgb(255, 77, 59);
                FontIcons icon1 = (FontIcons)Enum.Parse(typeof(FontIcons), strIcon);
                // Image = FontImages.GetImage(icon1, 32, Color.FromArgb(255, 77, 59));
                this.BackgroundImage = FontImages.GetImage(icon1, 32, Color.FromArgb(255, 77, 59));
                // this.BackgroundImage = Properties.Resources.rowicon;
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
