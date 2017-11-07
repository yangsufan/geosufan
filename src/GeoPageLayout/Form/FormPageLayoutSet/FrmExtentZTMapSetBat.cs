using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110908
    /// 说明：地图范围专题图出图设置界面
    /// </summary>
    public partial class FrmExtentZTMapSetBat : DevComponents.DotNetBar.Office2007Form
    {
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public Dictionary<string, string> MapTextElements = null;
      
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
        //是否生成图例
        public bool HasLegend
        {
            get { return checkBoxTuli.Checked; }
        }
        //是否添加指北针
        public bool HasNorthArrow
        {
            get { return chkNorthArrow.Checked; }
        }
        //加引线
        public bool HasBootLine
        {
            get { return cBoxHasBootLine.Checked; }
        }
        //全部字段
        public IList<string> LstFields
        {
            get;
            set;
        }
        //用户选择的字段
        public IList<string> LstResFields
        {
            get;
            set;
        }
        //用户选择的主标题字段
        public string ResTitleField
        {
            get;
            set;
        }
        //用户选择的副标题字段
        public string ResSubHeadFields
        {
            get;
            set;
        }
        //是否显示副标题
        public bool HasSubHead
        {
            get { return cBoxSubHead.Checked;}
        }
        //全部面层
        public IList<ILayer> LstPolygonLyrs
        {
            get;
            set;
        }
        //用户选择的面层
        public IList<ILayer> LstResPolygonLyrs
        {
            get;
            set;
        }
        //用户选择的面层
        public IFeature  ExtentFeature
        {
            get;
            set;
        }
        //通过标准图幅按钮访问的构造函数
        public FrmExtentZTMapSetBat()
        {
            InitializeComponent();
           
            InitMapTextElements();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置出图信息");//xisheng 2011.07.09 增加日志
            }
            string orgName = ModGetData.OrgName();//XX市
            if (orgName != "")
                rTxtRightLower.Text=rTxtRightLower.Text.Replace("XX", orgName);
        }

        private void InitMapTextElements()
        {
            MapTextElements = new Dictionary<string, string>();
            //from mxd template
            MapTextElements.Add("主题", "");
            MapTextElements.Add("副题", "");
            MapTextElements.Add("左下角标注", "");
            MapTextElements.Add("右下角标注", "");
            MapTextElements.Add("比例尺", "");
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消范围项目专题图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["主题"]=rTxtTitle.Text;
            //MapTextElements["副题"]=txtMapNO.Text;
            MapTextElements["左下角标注"] = rTxtLeftLower.Text;
            MapTextElements["右下角标注"] = rTxtRightLower.Text;
            MapTextElements["比例尺"]="1:"+txtScale.Text;

        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSetLabel_Click(object sender, EventArgs e)
        {
            //用户设置标注字段
            frmSetLabel fmSetLabel = new frmSetLabel(LstFields);
            if(fmSetLabel.ShowDialog(this)==DialogResult.OK)
               LstResFields = fmSetLabel.ResLst;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //用户设置面符号显示
            frmSetPolygonSymbol fmSetLabel = new frmSetPolygonSymbol(LstPolygonLyrs);
            if (fmSetLabel.ShowDialog(this) == DialogResult.OK)
                LstResPolygonLyrs = fmSetLabel.ResLst;
        }

        private void cBoxSubHead_CheckedChanged(object sender, EventArgs e)
        {
            //用户设置副标题字段
            if (cBoxSubHead.Checked)
            {
                FrmSubHeadFieldSet fmSHFS = new FrmSubHeadFieldSet(LstFields);
                if (fmSHFS.ShowDialog(this) == DialogResult.OK)
                    ResSubHeadFields = fmSHFS.ResField;
            }
        }

        private void btnTitleFd_Click(object sender, EventArgs e)
        {
            //用户设置主标题字段
            if (ExtentFeature!=null)
            {
                FrmSubHeadFieldSet fmSHFS = new FrmSubHeadFieldSet(LstFields);
                if (fmSHFS.ShowDialog(this) == DialogResult.OK)
                {
                    ResTitleField = fmSHFS.ResField;
                    int idx = ExtentFeature.Fields.FindField(ResTitleField);
                    if (idx != -1)
                    {
                        string val = ExtentFeature.get_Value(idx).ToString();
                        if (val != "")
                            rTxtTitle.Text = val;
                    }
                }
            }

        }

        private void FrmExtentZTMapSetBat_Load(object sender, EventArgs e)
        {
            //用户设置主标题字段
            if (ExtentFeature != null)
            {
                int idx = ExtentFeature.Fields.FindField("XMMC");
                if (idx != -1)
                {
                    string val = ExtentFeature.get_Value(idx).ToString();
                    if (val != "")
                        rTxtTitle.Text = val;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
