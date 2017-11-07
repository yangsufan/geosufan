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
using ESRI.ArcGIS.Geodatabase;

namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormAddFolder : DevComponents.DotNetBar.Office2007Form
    {
        bool isUpdate = false;                  //判断当前是否为更新
        public string _Foldername = "";
        public string _FolderScrip = "";
        private string _FolderTitle = "添加专题";
        public string _Scale = "";
        public string _DataType = "";
        public string _Year = "";
        public string _XZQCode = "";
        public string _DIRType = "";
        private IWorkspace _TmpWorkspace = null;
        private Dictionary<string, string> _DicDIRType = null;

        public FormAddFolder(IWorkspace TmpWorkspace)
        {
            InitializeComponent();
            _TmpWorkspace = TmpWorkspace;
        }

        public FormAddFolder(string FolderName,string FolderScrip)
        {
            InitializeComponent();
            _Foldername = FolderName;
            _FolderScrip = FolderScrip;
            isUpdate = true;
        }
        public FormAddFolder(IWorkspace TmpWorkspace,string FolderName, string FolderScrip,string strScale,string strDataType,string strYear,string strDIRType,string FolderTitle)
        {
            InitializeComponent();
            _TmpWorkspace = TmpWorkspace;
            _Foldername = FolderName;
            _FolderScrip = FolderScrip;
            _FolderTitle = FolderTitle;
            _Scale = strScale;
            _DataType = strDataType;
            //deleted by chulili 20110921 第一级专题节点没有行政区
            //_XZQCode = XZQCode;
            _Year = strYear;
            _DIRType = strDIRType;
            isUpdate = true;
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            _Foldername = this.txtFolder.Text;
            _FolderScrip = this.txtComment.Text;
            _Scale = this.txtScale.Text;
            _DataType = this.cmbDataType.Text;
            //deleted by chulili 20110921 第一级专题节点没有行政区
            //_XZQCode = SysCommon.ModXZQ.GetXzqCode(_TmpWorkspace,cmbXZQ.Text);
            _Year = cmbYear.Text;
            _DIRType = SysCommon.ModSysSetting.GetCodeOfName (_TmpWorkspace,"专题类型表",cmbDIRType.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormAddFolder_Load(object sender, EventArgs e)
        {
            if (_TmpWorkspace != null)
            {
                SysGisTable sysTable = new SysGisTable(_TmpWorkspace);
                Exception eError = null;
                List<Dictionary<string, object>> ListConfig = sysTable.GetRows("年度代码表", "", out eError);
                if (ListConfig != null)
                {
                    foreach (Dictionary<string, object> DicYear in ListConfig)
                    {
                        if (DicYear.ContainsKey("CODE"))
                        {
                            cmbYear.Items.Add(DicYear["CODE"].ToString());
                        }
                    }
                }
                ListConfig = null;
                //deleted by chulili 20110921 第一级专题节点没有行政区
                //ListConfig = sysTable.GetRows("行政区字典表", "XZJB='3'", out eError);
                //foreach (Dictionary<string, object> DicXZQ in ListConfig)
                //{
                //    if (DicXZQ.ContainsKey("NAME"))
                //    {
                //        this.cmbXZQ.Items.Add(DicXZQ["NAME"].ToString());
                //    }
                //}
                //ListConfig = null;
                ListConfig = sysTable.GetRows("专题类型表", "", out eError);
                if (ListConfig != null)
                {
                    foreach (Dictionary<string, object> DicDIRType in ListConfig)
                    {
                        if (DicDIRType.ContainsKey("NAME"))
                        {
                            cmbDIRType.Items.Add(DicDIRType["NAME"].ToString());
                        }
                    }
                }
                ListConfig = null;
                sysTable = null;
            }
            this.cmbDataType.Items.Add("DLG");
            this.cmbDataType.Items.Add("DOM");
            this.cmbDataType.Items.Add("DEM");
            if (isUpdate)
            {
                this.Text = _FolderTitle;
                btnAddFolder.Text = "修改";
                this.txtFolder.Text = _Foldername;
                this.txtComment.Text = _FolderScrip;
                this.txtScale.Text = _Scale;
                for (int i = 0; i < cmbDataType.Items.Count ; i++)
                {
                    if (cmbDataType.Items[i].ToString() == _DataType)
                    {
                        cmbDataType.SelectedIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < cmbDIRType.Items.Count; i++)
                {
                    if (cmbDIRType.Items[i].ToString() == SysCommon.ModSysSetting.GetNameOfCode (_TmpWorkspace,"专题类型表",_DIRType))
                    {
                        cmbDIRType.SelectedIndex = i;
                        break;
                    }
                }
                //deleted by chulili 20110921 第一级专题节点没有行政区
                //for (int i = 0; i < cmbXZQ.Items.Count; i++)
                //{
                //    if (cmbXZQ.Items[i].ToString() == SysCommon.ModXZQ.GetXzqName(_TmpWorkspace,_XZQCode))
                //    {
                //        cmbXZQ.SelectedIndex = i;
                //        break;
                //    }
                //}
                for (int i = 0; i < cmbYear.Items.Count; i++)
                {
                    if (cmbYear.Items[i].ToString() == _Year )
                    {
                        cmbYear.SelectedIndex = i;
                        break;
                    }
                }
                if (!cmbYear.Text.Equals(_Year))
                {
                    cmbYear.Text = _Year;
                }
            }
            else
            {
                this.Text =_FolderTitle;
                btnAddFolder.Text = "添加";
                cmbDataType.SelectedIndex = 0;
            }
            this.txtFolder.Focus();
        }

        private void txtScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

    }
}