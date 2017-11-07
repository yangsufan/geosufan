using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110923
    /// 说明：地图森林资源现状辖区专题图出图设置界面
    /// </summary>
    public partial class FrmTDLYMapSet_XQT : DevComponents.DotNetBar.Office2007Form
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
        private int ScaleSet = 0;
        public Dictionary<string, string> MapTextElements = null;
        public string SheetMapNO = "";
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
        public bool HasLegend
        {
            get { return checkBoxTuli.Checked; }
        }
        public string XZQname
        {
            get;
            set;
        }
        //通过标准图幅按钮访问的构造函数
        public FrmTDLYMapSet_XQT(int scale,string strscale)
        {
            InitializeComponent();
           
            InitMapTextElements();
            ScaleSet = scale;
            int scaleindex = cBoxScale.FindStringExact(scale.ToString());

            if (scaleindex > -1)
            {
                
                cBoxScale.SelectedIndex = scaleindex;
               
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志
            }

        }
        //用图幅号构造函数
        public FrmTDLYMapSet_XQT(int scale, string strscale, string strMapNo)
        {
            InitializeComponent();

            InitMapTextElements();
            ScaleSet = scale;
            int scaleindex = cBoxScale.FindStringExact(scale.ToString());

            if (scaleindex > -1)
            {

                cBoxScale.SelectedIndex = scaleindex;

            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志
            }

        }
        private void InitMapTextElements()
        {
            MapTextElements = new Dictionary<string, string>();
            //from mxd template
            MapTextElements.Add("主题", "");
            MapTextElements.Add("密级", "");
            MapTextElements.Add("左下角标注", "");
            MapTextElements.Add("单位", "");
            MapTextElements.Add("比例尺", "");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消标准辖区制图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["主题"]=txtTitle.Text;
            MapTextElements["左下角标注"] = rTxtLeftLower.Text;
            if (chkBoxSecurity.Checked)
            {
                MapTextElements["密级"] = cBoxSecret.Text;
            }
            MapTextElements["单位"]=txtCartoGroup.Text;
            MapTextElements["比例尺"]="1:"+cBoxScale.Text;
     
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmTDLYMapSet_XQT_Load(object sender, EventArgs e)
        {
            if (XZQname != "")
                txtTitle.Text = XZQname + txtTitle.Text;
            string orgName = ModGetData.OrgName();//XX市
            if (orgName != "")
                txtCartoGroup.Text=txtCartoGroup.Text.Replace("XX", orgName);
        }

        private void chkBoxSecurity_CheckedChanged(object sender, EventArgs e)
        {
            cBoxSecret.Enabled = chkBoxSecurity.Checked;
        }

      
       
    }
}
