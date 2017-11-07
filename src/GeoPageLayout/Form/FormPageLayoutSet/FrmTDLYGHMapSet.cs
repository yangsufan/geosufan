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
    /// 日期：20110908
    /// 说明：地图标准图幅专题图出图设置界面
    /// </summary>
    public partial class FrmTDLYGHMapSet : DevComponents.DotNetBar.Office2007Form
    {
        private int ScaleSet = 0;
        public Dictionary<string, string> MapTextElements = null;
        public string SheetMapNO = "";
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
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
        public FrmTDLYGHMapSet()
        {
            InitializeComponent();
           
            InitMapTextElements();

            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置总体规划图信息");//xisheng 2011.07.09 增加日志
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
            MapTextElements.Add("规划标题", "");
            MapTextElements.Add("左下角标注", "");
            MapTextElements.Add("右下角标注", "");
            MapTextElements.Add("比例尺", "");
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消总体规划制图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["主题"]=txtTitle.Text;
            MapTextElements["规划标题"] = txtMapNO.Text;
            MapTextElements["左下角标注"] = rTxtLeftLower.Text;
            MapTextElements["右下角标注"] = rTxtRightLower.Text;
            MapTextElements["比例尺"]="1:"+cBoxScale.Text;

        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmTDLYGHMapSet_Load(object sender, EventArgs e)
        {
            if (XZQname != "")
            {
                txtMapNO.Text = XZQname + txtMapNO.Text;
                txtTitle.Text = XZQname + txtTitle.Text;
            }
        }

    }
}
