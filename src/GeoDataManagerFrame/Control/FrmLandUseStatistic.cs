using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
namespace GeoDataManagerFrame
{
    public partial class FrmLandUseStatistic : DevComponents.DotNetBar.Office2007Form
    {
        Dictionary<string, string> DicXZQ = new Dictionary<string, string>();
        public string _XZQcode = "";
        public string _XZQmc = "";
        public string _Year = "";
        public string _AreaUnit = "";
        private string _LayerTreeXmlPath = "";
        public int _FractionNum = 2;
        public string _ResultPath = "";
        public FrmLandUseStatistic()
        {
            InitializeComponent();
        }
        public FrmLandUseStatistic(string XmlPath)
        {
            InitializeComponent();
            _LayerTreeXmlPath = XmlPath;
        }

        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (cmbXZQ.Text.Equals(""))
            {
                MessageBox.Show("请选择统计区域！");
                return;
            }
            if(cmbAreaUnit.Text.Equals(""))
            {
                MessageBox.Show("请选择面积单位！");
                return;
            }
            if (cmbYear.Text.Equals(""))
            {
                MessageBox.Show("请选择统计年度！");
                return;
            }
            ModStatReport._Statistic_XZQ = cmbXZQ.Text;
            ModStatReport._Statistic_AreaUnit = cmbAreaUnit.Text;
            ModStatReport._Statistic_Year = cmbYear.Text;
            ModStatReport._Statistic_Fractionnum = txtFractionNum.Text;
            ModStatReport._ResultPath_LandUse = textBoxResultPath.Text;
            _AreaUnit = cmbAreaUnit.Text;
            _XZQmc = cmbXZQ.Text;
            _XZQcode = DicXZQ[_XZQmc];

            _Year = cmbYear.Text;
            _FractionNum = int.Parse(txtFractionNum.Text);
            //if (textBoxResultPath.Text.Equals("默认路径"))
            //{
            //    _ResultPath = Application.StartupPath + @"\..\OutputResults\统计成果\LandUseStatistic";
            //}
            //else
            //{
                _ResultPath = textBoxResultPath.Text;
            //}
            this.DialogResult = DialogResult.OK;
        }

        private void buttonXQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmImpLandUseReport_Load(object sender, EventArgs e)
        {
            textBoxResultPath.Text = ModStatReport._ResultPath_LandUse;
            txtFractionNum.Text = ModStatReport._Statistic_Fractionnum;
            //统计的面积单位 初始化
            cmbAreaUnit.Items.Add("平方米");
            cmbAreaUnit.Items.Add("亩");
            cmbAreaUnit.Items.Add("公顷");
            cmbAreaUnit.SelectedIndex = 0;
            if (ModStatReport._Statistic_AreaUnit != "")
            {
                for (int i = 0; i < cmbAreaUnit.Items.Count; i++)
                {
                    if (cmbAreaUnit.Items[i].ToString() == ModStatReport._Statistic_AreaUnit)
                    {
                        cmbAreaUnit.SelectedIndex = i;
                        break;
                    }
                }
            }
            //统计区域初始化
//            140202--大同城区
//140203--大同矿区
//140211--大同市南郊区
//140212--大同市新荣区
//140221--阳高县
//140222--天镇县
//140223--广灵县
//140224--灵丘县
//140225--浑源县
//140226--左云县
//140227--大同县


            cmbXZQ.Items.Add("大同城区"); DicXZQ.Add("大同城区", "140202");
            cmbXZQ.Items.Add("大同矿区"); DicXZQ.Add("大同矿区", "140203");
            cmbXZQ.Items.Add("大同市南郊区"); DicXZQ.Add("大同市南郊区", "140211");
            cmbXZQ.Items.Add("大同市新荣区"); DicXZQ.Add("大同市新荣区", "140212");
            cmbXZQ.Items.Add("阳高县"); DicXZQ.Add("阳高县", "140221");
            cmbXZQ.Items.Add("天镇县"); DicXZQ.Add("天镇县", "140222");
            cmbXZQ.Items.Add("广灵县"); DicXZQ.Add("广灵县", "140223");
            cmbXZQ.Items.Add("灵丘县"); DicXZQ.Add("灵丘县", "140224");
            cmbXZQ.Items.Add("浑源县"); DicXZQ.Add("浑源县", "140225");
            cmbXZQ.Items.Add("左云县"); DicXZQ.Add("左云县", "140226");
            cmbXZQ.Items.Add("大同县"); DicXZQ.Add("大同县", "140227");
            cmbXZQ.SelectedIndex = 1;
            if (ModStatReport._Statistic_XZQ != "")
            {
                for (int i = 0; i < cmbXZQ.Items.Count; i++)
                {
                    if (cmbXZQ.Items[i].ToString() == ModStatReport._Statistic_XZQ)
                    {
                        cmbXZQ.SelectedIndex = i;
                        break;
                    }
                }
            }
            cmbYear.Items.Add("2009");
            cmbYear.Items.Add("2010");
            cmbYear.Items.Add("2011");
            cmbYear.Items.Add("2012");
            //DicXZQ.Add("韶关市", "4402");
            //DicXZQ.Add("仁化县", "440224");
            //DicXZQ.Add("南雄市", "440282");
            //DicXZQ.Add("武江区", "440203");
            //DicXZQ.Add("始兴县", "440222");
            //DicXZQ.Add("新丰县", "440233");
            //DicXZQ.Add("浈江区", "440204");
            //DicXZQ.Add("翁源县", "440229");
            //DicXZQ.Add("乐昌市", "440281");
            //DicXZQ.Add("曲江区", "440205");
            //DicXZQ.Add("乳源瑶族自治县", "440232");

            //DicXZQ.Add("4402","韶关市");
            //DicXZQ.Add("440224","仁化县");
            //DicXZQ.Add("440282","南雄市");
            //DicXZQ.Add("440203","武江区");
            //DicXZQ.Add("440222","始兴县");
            //DicXZQ.Add("440233","新丰县");
            //DicXZQ.Add("440204","浈江区");
            //DicXZQ.Add("440229","翁源县");
            //DicXZQ.Add("440281","乐昌市");
            //DicXZQ.Add("440205","曲江区");
            //DicXZQ.Add("440232","乳源瑶族自治县");
            //if (File.Exists(_LayerTreeXmlPath))
            //{
            //    XmlDocument pXmlDoc = new XmlDocument();
            //    pXmlDoc.Load(_LayerTreeXmlPath);
            //    XmlNodeList pDIRList = pXmlDoc.SelectNodes("//DIR [@DIRType='TDLY']");
            //    if (pDIRList != null)
            //    {
            //        foreach (XmlNode pDIRNode in pDIRList)
            //        {
            //            XmlElement pDIREle = pDIRNode as XmlElement;
            //            if (pDIREle != null)
            //            {
            //                if (pDIREle.HasAttribute("Year"))
            //                {
            //                    string strYear=pDIREle.GetAttribute("Year");
            //                    try
            //                    {
            //                        int iYear = int.Parse(strYear);
            //                        if (iYear > 2008)
            //                        {
            //                            if (!cmbYear.Items.Contains(strYear))
            //                            {
            //                                cmbYear.Items.Add(strYear);
            //                            }
            //                        }
            //                    }
            //                    catch(Exception err)
            //                    {}
            //                }
            //                //if (pDIREle.HasAttribute("XZQCode"))
            //                //{
            //                //    string strXZQcode = pDIREle.GetAttribute("XZQCode");
            //                //    string strXZQName = DicXZQ[strXZQcode];
            //                //    if (!cmbXZQ.Items.Contains(strXZQName))
            //                //    {
            //                //        cmbXZQ.Items.Add(strXZQName);
            //                //    }
            //                //}
            //            }
            //        }
            //    }
            //    pXmlDoc = null;

            //}
                if (cmbYear.Items.Count > 0)
                {
                    cmbYear.SelectedIndex = 0;
                }
                if (ModStatReport._Statistic_Year != "")
                {
                    for (int i = 0; i < cmbYear.Items.Count; i++)
                    {
                        if (cmbYear.Items[i].ToString() == ModStatReport._Statistic_Year)
                        {
                            cmbYear.SelectedIndex = i;
                            break;
                        }
                    }
                }
        }

        private void txtFractionNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
            if (pFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBoxResultPath.Text = pFolderBrowser.SelectedPath;
                pFolderBrowser = null;
            }
        }
    }
}