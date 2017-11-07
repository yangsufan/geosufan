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
    /// 说明：宗地图出图设置界面
    /// </summary>
    public partial class FrmZDMapSet : DevComponents.DotNetBar.Office2007Form
    {
        public Dictionary<string, string> MapTextElements = null;
      
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
        //通过标准图幅按钮访问的构造函数
        public FrmZDMapSet()
        {
            InitializeComponent();
           
            InitMapTextElements();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置出图信息");//xisheng 2011.07.09 增加日志
            }

        }

        private void InitMapTextElements()
        {
            MapTextElements = new Dictionary<string, string>();
            //from mxd template
            MapTextElements.Add("主题", "");
            //MapTextElements.Add("副题", "");
            MapTextElements.Add("左下角标注", "");
            MapTextElements.Add("左下角标注2", "");
            MapTextElements.Add("右下角标注", "");
            MapTextElements.Add("比例尺", "");
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消宗地图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["主题"]=rTxtTitle.Text;
            //MapTextElements["副题"]=txtMapNO.Text;
            MapTextElements["左下角标注"] = rTxtLeftLower.Text;
            MapTextElements["左下角标注2"] = txtCartoGroup.Text;
            MapTextElements["右下角标注"] = rTxtRightLower.Text;
            MapTextElements["比例尺"]="1:"+txtScale.Text;

        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rTxtLeftLower_TextChanged(object sender, EventArgs e)
        {

        }

        private void rTxtRightLower_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmZDMapSet_Load(object sender, EventArgs e)
        {
            string orgName = ModGetData.OrgName();//XX市
            if (orgName != "")
                txtCartoGroup.Text = txtCartoGroup.Text.Replace("XX", orgName);
        }

    }
}
