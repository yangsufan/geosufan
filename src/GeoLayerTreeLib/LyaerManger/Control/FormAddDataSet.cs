using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;

namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormAddDataSet : DevComponents.DotNetBar.Office2007Form
    {
        bool isUpdate = false;                  //判断当前是否为更新
        public string _DataSetname = "";
        public string _DataSetScrip = "";
        public string _FrmTitle = "添加图层组";
        public FormAddDataSet()
        {
            InitializeComponent();
        }

        public FormAddDataSet(string DataSetName, string DataSetScrip)
        {
            InitializeComponent();
            _DataSetname = DataSetName;
            _DataSetScrip = DataSetScrip;
            isUpdate = true;
        }
        public FormAddDataSet(string DataSetName, string DataSetScrip, string FrmTitle)
        {
            InitializeComponent();
            _DataSetname = DataSetName;
            _DataSetScrip = DataSetScrip;
            _FrmTitle = FrmTitle;
            isUpdate = true;
        }

        private void btnAddDataSet_Click(object sender, EventArgs e)
        {
            _DataSetname = this.txtDataSet.Text;
            _DataSetScrip = this.txtComment.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormAddDataSet_Load(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                this.Text = _FrmTitle;
                btnAddDataSet.Text = "修改";
                this.txtDataSet.Text = _DataSetname;
                this.txtComment.Text = _DataSetScrip;
            }
            else
            {
                this.Text = _FrmTitle;
                btnAddDataSet.Text = "添加";
            }
            this.txtDataSet.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}