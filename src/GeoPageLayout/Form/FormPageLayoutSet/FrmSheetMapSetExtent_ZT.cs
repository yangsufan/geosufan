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
    /// 说明：地图标准图幅专题图出图设置界面
    /// </summary>
    public partial class FrmSheetMapSetExtent_ZT : DevComponents.DotNetBar.Office2007Form
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
            get { return cBoxSubHead.Checked; }
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
        
        public IFeature ExtentFeature
        {
            get;
            set;
        }
        //用户选择的面层
        public IFeatureClass Jhtb
        {
            get;
            set;
        }

        //通过标准图幅按钮访问的构造函数
        public FrmSheetMapSetExtent_ZT(int scale,string strscale)
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
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置标准图幅信息");//xisheng 2011.07.09 增加日志
            }

        }
        //用图幅号构造函数
        public FrmSheetMapSetExtent_ZT(int scale, string strscale, string strMapNo)
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
            MapTextElements.Add("图幅号", "");
            MapTextElements.Add("副题", "");
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
            MapTextElements.Add("新旧图幅号", "");
            MapTextElements.Add("新图幅号", "");
            txtMapNO.Tag = "new";
 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消标准分幅制图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            MapTextElements["主题"]=txtTitle.Text;
            MapTextElements["图幅号"]=txtMapNO.Text;
            if (cBoxSubHead.Checked)
            {
                MapTextElements["副题"] = txtSubTitle.Text;
            }
            if (chkBoxSecurity.Checked)
            {
                MapTextElements["密级"] = cBoxSecret.Text;
            }
            MapTextElements["JTB1"] = txtJTBWN.Text;
            MapTextElements["JTB2"] = txtJTBN.Text;
            MapTextElements["JTB3"] = txtJTBEN.Text;
            MapTextElements["JTB4"] = txtJTBW.Text;
            MapTextElements["JTB5"] = txtJTBE.Text;
            MapTextElements["JTB6"] = txtJTBWS.Text;
            MapTextElements["JTB7"] = txtJTBS.Text;
            MapTextElements["JTB8"] = txtJTBES.Text;
            MapTextElements["1995年5月XXX测图。"]=txtTime.Text;
            MapTextElements["坐标系"]=cBoxCoordinate.Text;
            MapTextElements["1985国家高程基准，"]=cBoxElevation.Text+"，";
            MapTextElements["等高距为1米。"]="等高距为"+txtContourIntvl.Text+"米";
            //MapTextElements["1996年版图式。"]=txtVersionYear.Text+"年版图式。";
            //MapTextElements["附注："]="附注："+txtAnno.Text;
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
            MapTextElements["新旧图幅号"] = txtMapNO.Tag.ToString();
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMapNO_TextChanged(object sender, EventArgs e)
        {
            if (txtMapNO.Tag.ToString() == "new")
            {
                if (txtMapNO.Text.Length == 10 || txtMapNO.Text.Length == 12)
                {
                    string[] astrMapNo = txtMapNO.Text.Split(' ');//若图幅号带空格 则去空格
                    string realMapNo = "";
                    foreach (string str in astrMapNo)
                    {
                        realMapNo += str;
                    }
                    string[] nborTh = new string[8];
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
            else
            {
                txtJTBWN.Text = getValueOfFeature("MAPNUMBER='" + txtJTBWN.Text.Insert(3," ").Insert(5," ") + "'", "MAPNUMBER_OLD");
                txtJTBN.Text = getValueOfFeature("MAPNUMBER='" + txtJTBN.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBEN.Text = getValueOfFeature("MAPNUMBER='" + txtJTBEN.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBW.Text = getValueOfFeature("MAPNUMBER='" + txtJTBW.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBE.Text = getValueOfFeature("MAPNUMBER='" + txtJTBE.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBWS.Text = getValueOfFeature("MAPNUMBER='" + txtJTBWS.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBS.Text = getValueOfFeature("MAPNUMBER='" + txtJTBS.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
                txtJTBES.Text = getValueOfFeature("MAPNUMBER='" + txtJTBES.Text.Insert(3, " ").Insert(5, " ") + "'", "MAPNUMBER_OLD");
 
            }
           
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }

        private void FrmSheetMapSet_ZT_Load(object sender, EventArgs e)
        {
            string orgName = ModGetData.OrgName();//XX市
            if (orgName != "")
                txtCartoGroup.Text=txtCartoGroup.Text.Replace("XX", orgName);
        }

        private void chkBoxSecurity_CheckedChanged(object sender, EventArgs e)
        {
            cBoxSecret.Enabled = chkBoxSecurity.Checked;
        }

        private void cBoxSubHead_CheckedChanged(object sender, EventArgs e)
        {
            //用户设置副标题字段
            if (cBoxSubHead.Checked)
            {
                txtSubTitle.Enabled = true;
                if (txtMapNO.Tag.ToString() == "new")
                {
                    if (txtMapNO.Text.Contains(" "))
                       txtSubTitle.Text = getValueOfFeature("MAPNUMBER='" + txtMapNO.Text + "'", "MAPNAME");
                    else
                        txtSubTitle.Text = getValueOfFeature("MAPNUMBER='" + txtMapNO.Text.Insert(3," ").Insert(5," ") + "'", "MAPNAME");
                }
                else
                    txtSubTitle.Text = getValueOfFeature("MAPNUMBER_OLD='" + txtMapNO.Text + "'", "MAPNAME");
            }
            else
                txtSubTitle.Enabled = false;
        }

        private void btnShowPgSym_Click(object sender, EventArgs e)
        {
            //用户设置面符号显示
            frmSetPolygonSymbol fmSetLabel = new frmSetPolygonSymbol(LstPolygonLyrs);
            if (fmSetLabel.ShowDialog(this) == DialogResult.OK)
                LstResPolygonLyrs = fmSetLabel.ResLst;
        }

        private void btnSetLabel_Click(object sender, EventArgs e)
        {
            //用户设置标注字段
            frmSetLabel fmSetLabel = new frmSetLabel(LstFields);
            if (fmSetLabel.ShowDialog(this) == DialogResult.OK)
                LstResFields = fmSetLabel.ResLst;
        }

        private void cBoxHasBootLine_CheckedChanged(object sender, EventArgs e)
        {

        }
        //根据某字段获取另一字段的值
        private string getValueOfFeature(string whereclause,string desField)
        {
            string res = "";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = whereclause;
            ITable pTable = Jhtb as ITable;
            ICursor pCursor = pTable.Search(pQF, false);
            IRow pRow = pCursor.NextRow();
            int fdx = pTable.FindField(desField);
            if (fdx!=-1 && pRow != null)
            {
                res = pRow.get_Value(fdx).ToString();
 
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            pCursor = null;
            pRow = null;
            return res;
 
        }

        private void btnTFH_Click(object sender, EventArgs e)
        {
            if (Jhtb != null && txtMapNO.Text != "")
            {
                if (btnTFH.Text == "旧图幅号")
                {
                    txtMapNO.Tag = "old";
                    if (txtMapNO.Text.Contains(" "))
                       txtMapNO.Text = getValueOfFeature("MAPNUMBER='" + txtMapNO.Text + "'", "MAPNUMBER_OLD");
                    else
                       txtMapNO.Text = getValueOfFeature("MAPNUMBER='" + txtMapNO.Text.Insert(3," ").Insert(5," ") + "'", "MAPNUMBER_OLD");
                    btnTFH.Text = "新图幅号";
                }
                else
                {
                    txtMapNO.Tag = "new";
                    txtMapNO.Text = getValueOfFeature("MAPNUMBER_OLD='" + txtMapNO.Text + "'", "MAPNUMBER");
                    btnTFH.Text = "旧图幅号";
 
                }
            }
        }
       
    }
}
