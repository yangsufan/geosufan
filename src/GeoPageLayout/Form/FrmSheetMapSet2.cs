using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SysCommon.Error;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图标准图幅出图设置界面大比例尺
    /// </summary>
    public partial class FrmSheetMapSet2 : DevComponents.DotNetBar.Office2007Form
    {
        private int ScaleSet = 0;
        public Dictionary<string, string> MapTextElements = null;
        public string SheetMapNO = "";
        public bool isChecked = false;
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
        public FrmSheetMapSet2(int scale)
        {
            InitializeComponent();
           
            InitMapTextElements();
            ScaleSet = scale;
            int scaleindex = cBoxScale.FindStringExact(scale.ToString());

            if (scaleindex > -1)
            {
                
                cBoxScale.SelectedIndex = scaleindex;
               
            
            }
            Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志
            //if (strscale != "")
            //{
            //    txtMapNO.Text = txtMapNO.Text.Remove(3, 1);
            //    txtMapNO.Text = txtMapNO.Text.Insert(3, strscale);
            //}

        }
          //用图幅号构造函数
        public FrmSheetMapSet2(int scale,string strscale,string strMapNo)
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
            MapTextElements.Add("测量员", "");
            MapTextElements.Add("绘图员", "");
            MapTextElements.Add("检查员", "");
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Plugin.LogTable.Writelog("取消标准分幅制图");//xisheng 2011.07.09 增加日志
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtGWX.Text == "X" || txtGWY.Text == "XX" || txtGWX.Text == "" || txtGWY.Text == "" || txtGWX.Text == "0" || txtGWY.Text == "0")
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请设置图幅号的高位数！");
                return;
            }
            MapTextElements["图名"]=txtTitle.Text;
            MapTextElements["G50G005005"]=txtMapNO.Text;
            MapTextElements["密级"]=cBoxSecret.Text;
            MapTextElements["G50G004004"] = txtJTBWN.Text;
            MapTextElements["G50G004005"] = txtJTBN.Text;
            MapTextElements["G50G004006"] = txtJTBEN.Text;
            MapTextElements["G50G005004"] = txtJTBW.Text;
            MapTextElements["G50G005006"] = txtJTBE.Text;
            MapTextElements["G50G006004"] = txtJTBWS.Text;
            MapTextElements["G50G006005"] = txtJTBS.Text;
            MapTextElements["G50G006006"] = txtJTBES.Text;
            MapTextElements["1995年5月XXX测图。"]=txtTime.Text;
            MapTextElements["坐标系"]=cBoxCoordinate.Text;
            MapTextElements["1985国家高程基准，"]=cBoxElevation.Text+"，";
            MapTextElements["等高距为1米。"]="等高距为"+txtContourIntvl.Text+"米";
            MapTextElements["1996年版图式。"]=txtVersionYear.Text+"年版图式。";
            MapTextElements["附注："] = (txtCLY.Text=="")?"":"附注："+txtCLY.Text;
            MapTextElements["测绘机关全称"]=txtCartoGroup.Text;
            MapTextElements["比例尺"]="1:"+cBoxScale.Text;
            MapTextElements["比例尺2"]="1:"+cBoxScale.Text;
        
            MapTextElements["553.75左下"]=txtMapNO.Text.Trim().Split('-')[0];
            MapTextElements["385.50左下"]=txtMapNO.Text.Trim().Split('-')[1];
            MapTextElements["553.75右下"]=txtJTBE.Text.Trim().Split('-')[0];
            MapTextElements["385.75右下"]=txtJTBE.Text.Trim().Split('-')[1];
            MapTextElements["554.00左上"]=txtJTBN.Text.Trim().Split('-')[0];
            MapTextElements["385.50左上"]=txtJTBN.Text.Trim().Split('-')[1];
            MapTextElements["554.00右上"]=txtJTBEN.Text.Trim().Split('-')[0];
            MapTextElements["385.75右上"]=txtJTBEN.Text.Trim().Split('-')[1];
            MapTextElements["图名1"]=txtTitle.Text;
            MapTextElements["图号1"]=txtMapNO.Text;
            MapTextElements["图名2"]=txtTitle.Text;
            MapTextElements["图号2"]=txtMapNO.Text;
            MapTextElements["图名3"]=txtTitle.Text;
            MapTextElements["图号3"]=txtMapNO.Text;
            MapTextElements["图名4"]=txtTitle.Text;
            MapTextElements["图号4"]=txtMapNO.Text;
            MapTextElements["测量员"] = "测量员：" + txtCLY.Text;
            MapTextElements["绘图员"] = "绘图员：" + txtHTY.Text;
            MapTextElements["检查员"] = "检查员：" + txtJCHY.Text;
            isChecked = true;
            this.Close();
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMapNO_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            if (txtGWX.Text == "X" || txtGWY.Text == "XX" || txtGWX.Text == "" || txtGWY.Text == "" || txtGWX.Text == "0" || txtGWY.Text == "0")
                return;
            if (txtMapNO.Text.Trim().Length ==11)
            {
                double off_dis = 0.5 * ScaleSet / 1000;
                string[] MidXY = txtMapNO.Text.Trim().Split('-');
                string FullX = txtGWX.Text + MidXY[0];
                string FullY = txtGWY.Text + MidXY[1];
                double dFullX = Convert.ToDouble(FullX) ;
                double dFullY = Convert.ToDouble(FullY) ;
                int lenGWX=txtGWX.Text.Trim().Length;
                int lenGWY=txtGWY.Text.Trim().Length;

                txtJTBWN.Text =(dFullX + off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY - off_dis).ToString("F").Substring(lenGWY);//F是为了格式化为ddd.dd默认两位小数
                txtJTBN.Text = (dFullX + off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY).ToString("F").Substring(lenGWY);
                txtJTBEN.Text = (dFullX+ off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY + off_dis).ToString("F").Substring(lenGWY);
                txtJTBW.Text = dFullX.ToString("F").Substring(lenGWX) + "-" + (dFullY - off_dis).ToString("F").Substring(lenGWY);
                txtJTBE.Text = dFullX.ToString("F").Substring(lenGWX) + "-" + (dFullY + off_dis).ToString("F").Trim().Substring(lenGWY);
                txtJTBWS.Text = (dFullX - off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY - off_dis).ToString("F").Substring(lenGWY);
                txtJTBS.Text = (dFullX - off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY).ToString("F").Substring(lenGWY);
                txtJTBES.Text = (dFullX - off_dis).ToString("F").Substring(lenGWX) + "-" + (dFullY + off_dis).ToString("F").Substring(lenGWY);
                btnOK.Enabled = true;
               
            }
        }
        public string GWX
        {
            get { return txtGWX.Text; }
            set { txtGWX.Text = value; }
        }
        public string GWY
        {
            get { return txtGWY.Text; }
            set { txtGWY.Text = value; }
        }

        private void txtGWX_TextChanged(object sender, EventArgs e)
        {
            txtMapNO_TextChanged(sender, e);
        }

        private void txtGWY_TextChanged(object sender, EventArgs e)
        {
            txtMapNO_TextChanged(sender, e);
        }

        private void FrmSheetMapSet2_Load(object sender, EventArgs e)
        {
            txtMapNO_TextChanged(sender, e);
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }
       
    }
}
