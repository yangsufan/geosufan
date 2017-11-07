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
    public partial class FrmImpLandUseReport : DevComponents.DotNetBar.Office2007Form
    {
        Dictionary<string, string> DicXZQ = new Dictionary<string, string>();
        //public string _XZQcode = "";
        //public string _XZQmc = "";
        public string _Year = "";
        public string _AreaUnit = "";
        public bool _chkTDLY = false;
        public bool _chkZTGH = false;
        public int _FractionNum = 2;
        public string _ResultPath = "";
        private string _LayerTreeXmlPath = "";
        public FrmImpLandUseReport()
        {
            InitializeComponent();
        }
        public FrmImpLandUseReport(string XmlPath)
        {
            InitializeComponent();
            _LayerTreeXmlPath = XmlPath;
        }

        private void buttonXOK_Click(object sender, EventArgs e)
        {
            //if (cmbXZQ.Text.Equals(""))
            //{
            //    MessageBox.Show("请选择面积单位！");
            //    return;
            //}
            //_XZQmc = cmbXZQ.Text;
            //_XZQcode = DicXZQ[_XZQmc];
            if (!chkTDLY.Checked && !chkZTGH.Checked)
            {
                MessageBox.Show("请选择分析类型！");
                return;
            }
            if (cmbAreaUnit.Text.Equals(""))
            {
                MessageBox.Show("请选择面积单位！");
                return;
            }
            if (chkTDLY.Checked && cmbYear.Text.Equals(""))
            {
                MessageBox.Show("请选择统计年度！");
                return;
            }
            ModStatReport._Statistic_Year = cmbYear.Text;
            ModStatReport._Statistic_AreaUnit = cmbAreaUnit.Text;
            ModStatReport._Statistic_TDLY = chkTDLY.Checked;
            ModStatReport._Statistic_ZTGH = chkZTGH.Checked;
            ModStatReport._Statistic_Fractionnum = txtFractionNum.Text;
            ModStatReport._ResultPath_Imp = textBoxResultPath.Text;
            _AreaUnit = cmbAreaUnit.Text;
            _Year = cmbYear.Text;
            _chkTDLY = chkTDLY.Checked;
            _chkZTGH = chkZTGH.Checked;
            _FractionNum = int.Parse(txtFractionNum.Text);
            //if (textBoxResultPath.Text.Equals("默认路径"))
            //{
            //    _ResultPath = Application.StartupPath + @"\..\OutputResults\统计成果\" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
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
            textBoxResultPath.Text = ModStatReport._ResultPath_Imp;

            txtFractionNum.Text = ModStatReport._Statistic_Fractionnum;
            chkTDLY.Checked = ModStatReport._Statistic_TDLY;
            if (!chkTDLY.Checked)
            {
                cmbYear.Enabled = false;
            }
            chkZTGH.Checked = ModStatReport._Statistic_ZTGH;
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
            //cmbXZQ.Items.Add("韶关市"); DicXZQ.Add("韶关市", "4402");
            //cmbXZQ.Items.Add("仁化县"); DicXZQ.Add("仁化县", "440224");
            //cmbXZQ.Items.Add("南雄市"); DicXZQ.Add("南雄市", "440282");
            //cmbXZQ.Items.Add("武江区"); DicXZQ.Add("武江区", "440203");
            //cmbXZQ.Items.Add("始兴县"); DicXZQ.Add("始兴县", "440222");
            //cmbXZQ.Items.Add("新丰县"); DicXZQ.Add("新丰县", "440233");
            //cmbXZQ.Items.Add("浈江区"); DicXZQ.Add("浈江区", "440204");
            //cmbXZQ.Items.Add("翁源县"); DicXZQ.Add("翁源县", "440229");
            //cmbXZQ.Items.Add("乐昌市"); DicXZQ.Add("乐昌市", "440281");
            //cmbXZQ.Items.Add("曲江区"); DicXZQ.Add("曲江区", "440205");
            //cmbXZQ.Items.Add("乳源瑶族自治县"); DicXZQ.Add("乳源瑶族自治县", "440232");
            
            if (File.Exists(_LayerTreeXmlPath))
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(_LayerTreeXmlPath);
                XmlNodeList pDIRList = pXmlDoc.SelectNodes("//DIR [@DIRType='TDLY']");
                if (pDIRList != null)
                {
                    foreach (XmlNode pDIRNode in pDIRList)
                    {
                        XmlElement pDIREle = pDIRNode as XmlElement;
                        if (pDIREle != null)
                        {
                            if (pDIREle.HasAttribute("Year"))
                            {
                                string strYear=pDIREle.GetAttribute("Year");
                                try
                                {
                                    int iYear = int.Parse(strYear);
                                    if (iYear > 2008)
                                    {
                                        if (!cmbYear.Items.Contains(strYear))
                                        {
                                            cmbYear.Items.Add(strYear);
                                        }
                                    }
                                }
                                catch(Exception err)
                                {}
                            }
                        }
                    }
                }
                pXmlDoc = null;

            }
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

        private void chkTDLY_CheckValueChanged(object sender, EventArgs e)
        {
            if (chkTDLY.Checked)
            {
                cmbYear.Enabled = true;
            }
            else
            {
                cmbYear.Enabled = false;
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