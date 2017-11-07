using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSymbology.Form
{
    public class frmStringEdit:System.Windows.Forms.Form
    {
        public static int FormWidth = 27;

        private DevComponents.DotNetBar.Controls.TextBoxX txtValue;
        private string m_EditType;
        private IEditItem m_EditItem;

        public frmStringEdit(IEditItem _EditItem, string _EditValue, System.Drawing.Point _Location, int _Width,string editType)
        {
            InitializeComponent();
            m_EditType = editType;
            this.Size = new System.Drawing.Size(_Width, FormWidth);
            this.txtValue.Size = new System.Drawing.Size(this.Width, 21);
            this.Location = _Location;
            txtValue.LostFocus += new EventHandler(StringEdit_LostFocus);
            m_EditItem = _EditItem;
            txtValue.Text = _EditValue;
        }
    
        private void InitializeComponent()
        {
            this.txtValue = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // txtValue
            // 
            // 
            // 
            // 
            this.txtValue.Border.Class = "TextBoxBorder";
            this.txtValue.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtValue.Location = new System.Drawing.Point(0, 3);
            this.txtValue.Margin = new System.Windows.Forms.Padding(0);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(150, 21);
            this.txtValue.TabIndex = 0;
            this.txtValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textbox_KeyUp);
            // 
            // frmStringEdit
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(150, 27);
            this.Controls.Add(this.txtValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStringEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        private void textbox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    txtValue.LostFocus -= new EventHandler(StringEdit_LostFocus);
                    m_EditItem.DoAfterEdit(txtValue.Text.Replace("\r\n", ""), DialogResult.OK, m_EditType);
                    this.Close();
                    break;
                case Keys.Escape:
                    m_EditItem.DoAfterEdit(null, DialogResult.Cancel, m_EditType);
                    this.Close();
                    break;
            }
        }

        private void StringEdit_LostFocus(object sender, EventArgs e)
        {
            m_EditItem.DoAfterEdit(txtValue.Text.Replace("\r\n", ""), DialogResult.OK, m_EditType);
            this.Close();
        }
    }
}
