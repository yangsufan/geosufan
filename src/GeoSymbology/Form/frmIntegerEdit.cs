using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.Form
{
    public class frmIntegerEdit:System.Windows.Forms.Form
    {
        private DevComponents.Editors.IntegerInput integerInput;
        private object m_OriginObject;
        public frmIntegerEdit(object _OriginObject)
        {
            InitializeComponent();
            this.integerInput.Size = new System.Drawing.Size(this.Width, 21);
            integerInput.LostFocus += new EventHandler(IntegerInput_LostFocus);
            integerInput.KeyUp+=new System.Windows.Forms.KeyEventHandler(IntegerInput_KeyUp);
            m_OriginObject = _OriginObject;
            GetValue();
        }
        private void InitializeComponent()
        {
            this.integerInput = new DevComponents.Editors.IntegerInput();
            ((System.ComponentModel.ISupportInitialize)(this.integerInput)).BeginInit();
            this.SuspendLayout();
            // 
            // integerInput
            // 
            // 
            // 
            // 
            this.integerInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.integerInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.integerInput.Location = new System.Drawing.Point(0, 3);
            this.integerInput.Name = "integerInput";
            this.integerInput.ShowUpDown = true;
            this.integerInput.Size = new System.Drawing.Size(150, 21);
            this.integerInput.TabIndex = 0;
            // 
            // frmIntegerEdit
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(150, 27);
            this.Controls.Add(this.integerInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIntegerEdit";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.integerInput)).EndInit();
            this.ResumeLayout(false);

        }

        private void IntegerInput_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Enter:
                    integerInput.LostFocus -= new EventHandler(IntegerInput_LostFocus);
                    SetValue();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    break;
                case System.Windows.Forms.Keys.Escape:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }

        private void GetValue()
        {
            if (m_OriginObject is DevComponents.AdvTree.Cell)
            {
                DevComponents.AdvTree.Cell cell = m_OriginObject as DevComponents.AdvTree.Cell;
                integerInput.Value = (int)cell.Tag;
                cell.Tag = cell.Text.Split('-')[0];
            }
            else if (m_OriginObject is System.Windows.Forms.ListViewItem.ListViewSubItem)
            {
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem =
                    m_OriginObject as System.Windows.Forms.ListViewItem.ListViewSubItem;
                integerInput.Value = (int)subItem.Tag;
                subItem.Tag = subItem.Text.Split('-')[0];
            }
        }

        private void SetValue()
        {
            if (m_OriginObject is DevComponents.AdvTree.Cell)
            {
                DevComponents.AdvTree.Cell cell = m_OriginObject as DevComponents.AdvTree.Cell;
                cell.Text = cell.Tag.ToString() + "-" + integerInput.Value.ToString();
                cell.Tag = integerInput.Value;
                cell.HostedControl = null;
            }
            else if (m_OriginObject is System.Windows.Forms.ListViewItem.ListViewSubItem)
            {
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem =
                    m_OriginObject as System.Windows.Forms.ListViewItem.ListViewSubItem;
                subItem.Text = subItem.Tag.ToString() + "-" + integerInput.Value.ToString();
                subItem.Tag = integerInput.Value;
            }
        }

        private void IntegerInput_LostFocus(object sender, EventArgs e)
        {
            SetValue();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
