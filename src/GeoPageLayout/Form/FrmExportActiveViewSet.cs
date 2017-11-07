using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SysCommon.Error;
namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.28
    /// 说明：地图视图输出设置窗体
    /// </summary>
    public partial class FrmExportActiveViewSet : DevComponents.DotNetBar.Office2007Form
    {
        private string curMapName="";
        public FrmExportActiveViewSet(string incurMapName)
        {
            InitializeComponent();
            isOK = false;
            curMapName = incurMapName;
        }

        private void btnSaveDl_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PDF|*.pdf|EPS|*.eps|TIFF|*.tiff|JPEG|*.jpg";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.FileName = curMapName;
            if(saveFileDialog1.ShowDialog()!=DialogResult.OK)
                return;
            if (saveFileDialog1.FileName == "")
                return;
            txtFileName.Text=saveFileDialog1.FileName;
        }
        public string FileName
        {
            get { return txtFileName.Text; }
        }
        public int  Resolution
        {
            get { return Convert.ToInt32(txtResolution.Text); }
        }
        public int Retio
        {
            get { return Convert.ToInt32(cBoxRatio.Text); }
        }
        public bool isOK
        {
            get;
            set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtResolution.Text == "" || txtFileName.Text == "" || cBoxRatio.Text == "")
                return;
            try
            {
                int r = Convert.ToInt32(txtResolution.Text);
                int ra = Convert.ToInt32(cBoxRatio.Text);

            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
                return;
            }
            isOK = true;
            this.Close();
        }

        private void FrmExportActiveViewSet_Load(object sender, EventArgs e)
        {
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            string fExt=Path.GetExtension(txtFileName.Text);
            switch (fExt)
            {
                case ".tiff":case ".jpg": case ".bmp": case ".png":
                    labelRatio.Visible = false;
                    cBoxRatio.Visible = false;
                    txtResolution.Text = "96";
                    break;
                default:
                    labelRatio.Visible = true;
                    cBoxRatio.Visible = true;
                    txtResolution.Text = "300";
                    break;
            }

        }

    }
}
