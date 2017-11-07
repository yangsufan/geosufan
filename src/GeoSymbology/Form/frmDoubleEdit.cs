using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoSymbology.Form
{
    public partial class frmDoubleEdit : System.Windows.Forms.Form
    {
        public static int FormWidth = 27;

        private IEditItem m_EditItem;
        private string m_EditType;
        public frmDoubleEdit(IEditItem _EditItem, double _EditValue, Point _Location, int _Width,string editType)
        {
            m_EditType = editType;
            InitEditor(_EditItem, _EditValue, _Location, _Width, -1.7976931348623157E+308, 1.7976931348623157E+308);
        }

        public frmDoubleEdit(IEditItem _EditItem, double _EditValue, Point _Location, int _Width,double _MinValue,double _MaxValue)
        {
            InitEditor(_EditItem, _EditValue, _Location, _Width, _MinValue, _MaxValue);
        }

        private void InitEditor(IEditItem _EditItem, double _EditValue, Point _Location, int _Width, double _MinValue, double _MaxValue)
        {
            InitializeComponent();
            this.Size = new Size(_Width, FormWidth);
            this.doubleInput.Size = new Size(this.Width, 21);
            this.doubleInput.MinValue = _MinValue;
            this.doubleInput.MaxValue = _MaxValue;
            this.Location = _Location;
            doubleInput.LostFocus += new EventHandler(DoubleEdit_LostFocus);
            m_EditItem = _EditItem;
            doubleInput.Value = _EditValue;
        }

        private void doubleInput_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    doubleInput.LostFocus -= new EventHandler(DoubleEdit_LostFocus);
                    m_EditItem.DoAfterEdit(doubleInput.Value, DialogResult.OK,m_EditType);
                    this.Close();
                    break;
                case Keys.Escape:
                    m_EditItem.DoAfterEdit(null, DialogResult.Cancel, m_EditType);
                    this.Close();
                    break;
            }
        }

        private void DoubleEdit_LostFocus(object sender, EventArgs e)
        {
            m_EditItem.DoAfterEdit(doubleInput.Value, DialogResult.OK, m_EditType);
            this.Close();
        }
    }
}