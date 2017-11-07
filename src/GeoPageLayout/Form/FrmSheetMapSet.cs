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
    /// 日期：2011.05.24
    /// 说明：地图标准图幅出图设置界面小比例尺
    /// </summary>
    public partial class FrmSheetMapSet : DevComponents.DotNetBar.Office2007Form
    {
        private int ScaleSet = 0;
        public Dictionary<string, string> MapTextElements = null;
        public string SheetMapNO = "";
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
        //通过标准图幅按钮访问的构造函数
        public FrmSheetMapSet(int scale,string strscale)
        {
            InitializeComponent();
           
            InitMapTextElements();
            ScaleSet = scale;
            int scaleindex = cBoxScale.FindStringExact(scale.ToString());

            if (scaleindex > -1)
            {
                
                cBoxScale.SelectedIndex = scaleindex;
               
            }
            if (strscale != "")
            {
                txtMapNO.Text = txtMapNO.Text.Remove(3, 1);
                txtMapNO.Text = txtMapNO.Text.Insert(3, strscale);
            }
            Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志

        }
        //用图幅号构造函数
        public FrmSheetMapSet(int scale,string strscale,string strMapNo)
        {
            InitializeComponent();

            InitMapTextElements();
            ScaleSet = scale;
            int scaleindex = cBoxScale.FindStringExact(scale.ToString());

            if (scaleindex > -1)
            {

                cBoxScale.SelectedIndex = scaleindex;

            }
            if (strMapNo != "")
            {
                txtMapNO.Text = strMapNo;
                
            }
            Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志

        }
        private void InitMapTextElements()
        {
            MapTextElements = new Dictionary<string, string>();
            //from mxd template
            MapTextElements.Add("图名", "");
            MapTextElements.Add("G50G005005", "");
            MapTextElements.Add("密级", "");
            //MapTextElements.Add("G50G004004", "");
            //MapTextElements.Add("G50G004005", "");
            //MapTextElements.Add("G50G004006", "");
            //MapTextElements.Add("G50G005004", "");
            //MapTextElements.Add("G50G005006", "");
            //MapTextElements.Add("G50G006004", "");
            //MapTextElements.Add("G50G006005", "");
            //MapTextElements.Add("G50G006006", "");
            MapTextElements.Add("1995年5月XXX测图。", "");
            MapTextElements.Add("坐标系", "");
            MapTextElements.Add("1985国家高程基准，", "");
            MapTextElements.Add("等高距为1米。", "");
            MapTextElements.Add("1996年版图式。", "");
            MapTextElements.Add("附注：", "");
            MapTextElements.Add("测绘机关全称", "");
            MapTextElements.Add("比例尺", "");
            MapTextElements.Add("比例尺2", "");
            //MapTextElements.Add("553.75左下", "");
            //MapTextElements.Add("385.50左下", "");
            //MapTextElements.Add("553.75右下", "");
            //MapTextElements.Add("385.75右下", "");
            //MapTextElements.Add("554.00左上", "");
            //MapTextElements.Add("385.50左上", "");
            //MapTextElements.Add("554.00右上", "");
            //MapTextElements.Add("385.75右上", "");
            MapTextElements.Add("图名1", "");
            MapTextElements.Add("图号1", "");
            MapTextElements.Add("图名2", "");
            MapTextElements.Add("图号2", "");
            MapTextElements.Add("图名3", "");
            MapTextElements.Add("图号3", "");
            MapTextElements.Add("图名4", "");
            MapTextElements.Add("图号4", "");
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Plugin.LogTable.Writelog("取消标准分幅制图");//xisheng 2011.07.09 增加日志
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["图名"]=txtTitle.Text;
            MapTextElements["G50G005005"]=txtMapNO.Text;
            MapTextElements["密级"]=cBoxSecret.Text;
            //MapTextElements["G50G004004"]=txtJTBWN.Text;
            //MapTextElements["G50G004005"]=txtJTBN.Text;
            //MapTextElements["G50G004006"]=txtJTBEN.Text;
            //MapTextElements["G50G005004"]=txtJTBW.Text;
            //MapTextElements["G50G005006"]=txtJTBE.Text;
            //MapTextElements["G50G006004"]=txtJTBWS.Text;
            //MapTextElements["G50G006005"]=txtJTBS.Text;
            //MapTextElements["G50G006006"]=txtJTBES.Text;
            MapTextElements["1995年5月XXX测图。"]=txtTime.Text;
            MapTextElements["坐标系"]=cBoxCoordinate.Text;
            MapTextElements["1985国家高程基准，"]=cBoxElevation.Text+"，";
            MapTextElements["等高距为1米。"]="等高距为"+txtContourIntvl.Text+"米";
            MapTextElements["1996年版图式。"]=txtVersionYear.Text+"年版图式。";
            MapTextElements["附注："]="附注："+txtAnno.Text;
            MapTextElements["测绘机关全称"]=txtCartoGroup.Text;
            MapTextElements["比例尺"]="1:"+cBoxScale.Text;
            MapTextElements["比例尺2"]="1:"+cBoxScale.Text;
            //MapTextElements["553.75左下"]=.Text;
            //MapTextElements["385.50左下"]=.Text;
            //MapTextElements["553.75右下"]=.Text;
            //MapTextElements["385.75右下"]=.Text;
            //MapTextElements["554.00左上"]=.Text;
            //MapTextElements["385.50左上"]=.Text;
            //MapTextElements["554.00右上"]=.Text;
            //MapTextElements["385.75右上"]=.Text;
            MapTextElements["图名1"]=txtTitle.Text;
            MapTextElements["图号1"]=txtMapNO.Text;
            MapTextElements["图名2"]=txtTitle.Text;
            MapTextElements["图号2"]=txtMapNO.Text;
            MapTextElements["图名3"]=txtTitle.Text;
            MapTextElements["图号3"]=txtMapNO.Text;
            MapTextElements["图名4"]=txtTitle.Text;
            MapTextElements["图号4"]=txtMapNO.Text;
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMapNO_TextChanged(object sender, EventArgs e)
        {
            if (txtMapNO.Text.Length == 10 || txtMapNO.Text.Length==12)
            {
                string[] astrMapNo=txtMapNO.Text.Split(' ');//若图幅号带空格 则去空格
                string realMapNo="";
                foreach(string str in astrMapNo)
                {
                    realMapNo += str;
                }
                string[] nborTh=new string[8];
                GeoDrawSheetMap.basPageLayout.CalculateFigureforme(realMapNo, ScaleSet.ToString(), ref nborTh);
                if (nborTh.Length != 0)
                {
                    txtJTBWN.Text = nborTh[0];
                    txtJTBN.Text = nborTh[1];
                    txtJTBEN.Text = nborTh[2];
                    txtJTBW.Text = nborTh[3];
                    txtJTBE.Text = nborTh[4];
                    txtJTBWS.Text = nborTh[5];
                    txtJTBS.Text = nborTh[6];
                    txtJTBES.Text = nborTh[7];
                }
               
            }
           
        }
       
    }
}
