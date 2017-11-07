using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SysCommon;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;

namespace GeoDBIntegration
{
    /// <summary>
    /// 栅格数据建库　　陈亚飞添加　
    /// </summary>
    public partial class FrmSetRasterDB : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace pworkSpace = null;
        DevComponents.AdvTree.Node m_ProjectNode = null;  //当前数据库工程节点
        string m_sDBType = "";                              //数据库类型：高程数据库、影像数据库
        private long m_iProjectID = -1;
        //相关参数
        private string dbtypeStr = "";                  //栅格数据类型
        private string pResampleStr = "";                 //重采样
        private string pCompressionStr = "";              //压缩类型
        private string pPyramidStr = "";                //金字塔 
        private string pTileHStr = "";                     //瓦片高度
        private string pTileWStr = "";                    //瓦片宽度
        private string pBandStr = "";                     //波段
        private string pDTName = "";                    //数据集名称
        public string s1dbtypeStr { get { return this.dbtypeStr; } }
        public string s2pResampleStr { get { return this.pResampleStr; } }
        public string s3pCompressionStr { get { return this.pCompressionStr; } }
        public string s4pPyramidStr { get { return this.pPyramidStr; } }
        public string s5pTileHStr { get { return this.pTileHStr; } }
        public string s6pTileWStr { get { return this.pTileWStr; } }
        public string s7pBandStr { get { return this.pBandStr; } }
        public string s8pDTName { get { return this.pDTName; } }
        public FrmSetRasterDB(long in_ProjectID,string DbType)
        {
            InitializeComponent();
            #region 初始化界面信息

            // m_Hook = pHook;
            cmbRasterType.Items.Clear();
            cmbRasterSpaRef.Items.Clear();
            cmbGeoSpaRef.Items.Clear();
            cmbRasterPixeType.Items.Clear();

            //cmbRasterType.Items.AddRange(new object[] { "DOM", "DEM" });//"DLG",
            cmbRasterType.Items.AddRange(new object[] { "栅格编目", "栅格数据集" });
            cmbRasterType.SelectedIndex = 0;
          
            object[] spaRef = new object[] { "西安高斯117度(3度带)", "西安高斯120度(3度带)", "西安高斯123度(3度带)" };
            cmbRasterSpaRef.Items.AddRange(spaRef);
            //cmbRasterSpaRef.SelectedIndex = 0;
            cmbGeoSpaRef.Items.AddRange(spaRef);
            //cmbGeoSpaRef.SelectedIndex = 0;

            cmbResampleType.Items.Clear();
            cmbCompression.Items.Clear();

            cmbResampleType.Items.AddRange(new object[] { "邻近法", "双线性内插法", "立方卷积法" });
            cmbResampleType.SelectedIndex = 0;

            cmbCompression.Items.AddRange(new object[] { "LZ77", "JPEG", "JPEG2000", "PackBits", "LZW" });
            cmbCompression.SelectedIndex = 0;

            cmbRasterPixeType.Items.AddRange(new object[] { "PT_UCHAR", "PT_UNKNOWN", "PT_U1", "PT_U2", "PT_U4", "PT_CHAR", 
                "PT_USHORT", "PT_SHORT", "PT_ULONG", "PT_LONG","PT_FLOAT","PT_DOUBLE","PT_COMPLEX","PT_DCOMPLEX" });
            cmbRasterPixeType.SelectedIndex = 0;

            tileH.Text = "128";
            tileW.Text = "128";
            txtBand.Text = "1";
            rbcatalog.Checked = true;
            #endregion
            m_iProjectID = in_ProjectID;
            m_sDBType = DbType;
        }

        /// <summary>
        /// Get Raster Pixel Type
        /// </summary>
        /// <returns></returns>
        private rstPixelType GetPixelType()
        {
            rstPixelType pPixelType = rstPixelType.PT_UCHAR;
            switch (cmbRasterPixeType.Text.Trim())
            {
                case "PT_UNKNOWN":
                    pPixelType = rstPixelType.PT_UNKNOWN;
                    break;
                case "PT_U1":
                    pPixelType = rstPixelType.PT_U1;
                    break;
                case "PT_U2":
                    pPixelType = rstPixelType.PT_U2;
                    break;
                case "PT_U4":
                    pPixelType = rstPixelType.PT_U4;
                    break;
                case "PT_UCHAR":
                    pPixelType = rstPixelType.PT_UCHAR;
                    break;
                case "PT_CHAR":
                    pPixelType = rstPixelType.PT_CHAR;
                    break;
                case "PT_USHORT":
                    pPixelType = rstPixelType.PT_USHORT;
                    break;
                case "PT_SHORT":
                    pPixelType = rstPixelType.PT_SHORT;
                    break;
                case "PT_ULONG":
                    pPixelType = rstPixelType.PT_ULONG;
                    break;
                case "PT_LONG":
                    pPixelType = rstPixelType.PT_LONG;
                    break;
                case "PT_FLOAT":
                    pPixelType = rstPixelType.PT_FLOAT;
                    break;
                case "PT_DOUBLE":
                    pPixelType = rstPixelType.PT_DOUBLE;
                    break;
                case "PT_COMPLEX":
                    pPixelType = rstPixelType.PT_COMPLEX;
                    break;
                case "PT_DCOMPLEX":
                    pPixelType = rstPixelType.PT_DCOMPLEX;
                    break;
                default:
                    break;
            }
            return pPixelType;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            Exception err = null;
            #region 检查设置是否完备
            if (string.IsNullOrEmpty(txt_RasterName.Text))
            {
                MessageBox.Show("请输入栅格数据库的名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txt_RasterName.Focus();
                return;
            }
            //是否选择建库的类型
            if (rbcatalog.Checked == false && rbdataset.Checked == false)
            {
                MessageBox.Show("请选择栅格库的类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //服务器连接设置检查         
            if (rbdataset.Checked)//　cmbRasterType.Text.Trim() == "栅格数据集")
            {
                //栅格数据集设置检查

                if (cmbCompression.Text.Trim() == "")
                {
                    MessageBox.Show("请选择压缩类型!","系统提示",MessageBoxButtons.OK ,MessageBoxIcon.Information );
                    return;
                }
                if (cmbResampleType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择重采样类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    if (txtPyramid.Text.Trim() != "")
                    {
                        Convert.ToInt32(txtPyramid.Text.Trim());
                    }

                    Convert.ToInt32(tileH.Text.Trim());
                    Convert.ToInt32(tileW.Text.Trim());
                    Convert.ToInt32(txtBand.Text.Trim());
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请填写有效的数字!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //labelXErr.Text = "请填写有效的数字！";
                    return;
                }
            }             
            #endregion
            //给变量赋值
            if (rbcatalog.Checked)
            {
                dbtypeStr = "栅格编目";
            }
            else if (rbdataset.Checked)
            {
                dbtypeStr = "栅格数据集";
            }
            pResampleStr = cmbResampleType.Text.Trim();
            pCompressionStr = cmbCompression.Text.Trim();
            pPyramidStr = txtPyramid.Text.Trim();
            pTileHStr = tileH.Text.Trim();
            pTileWStr = tileW.Text.Trim();
            pBandStr = txtBand.Text.Trim();
            pDTName = txt_RasterName.Text.Trim();
            #region 刷新栅格数据库xml的参数设置   陈亚飞添加 20101011
            //根据不同的数据库类型读取不同的xml文件
            string pXmlPath = "";
            if (m_sDBType == enumInterDBType.高程数据库.ToString())
            {
                pXmlPath = ModuleData.v_DEMProjectXml;
            }
            else if (m_sDBType == enumInterDBType.影像数据库.ToString())
            {
                pXmlPath = ModuleData.v_ImageProjectXml;
            }


            if (!File.Exists(pXmlPath)) return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pXmlPath);
            //工程节点
            System.Xml.XmlNode Projectnode = xmlDoc.SelectSingleNode("工程管理/工程[@编号='" + m_iProjectID + "']");
            if (Projectnode == null) return;
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            //内容节点
            XmlNode ConNode = ProjectNodeElement.FirstChild;
            //遍历所有内容子节点，设置栅格数据库参数信息
            foreach (XmlNode dbNode in ConNode.ChildNodes)
            {
                XmlElement DbTypeEle = dbNode as XmlElement;
                string sVisible = DbTypeEle.GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //设置数据库节点的“存储类型”属性
                DbTypeEle.SetAttribute("存储类型", dbtypeStr);

                //库体节点
                XmlElement ProjectUserDSEle = DbTypeEle.FirstChild.FirstChild as XmlElement;
                //设置库体名称属性
                ProjectUserDSEle.SetAttribute("名称", pDTName);

                //设置栅格数据参数
                System.Xml.XmlElement rasterParaEle = DbTypeEle.SelectSingleNode(".//栅格参数设置") as System.Xml.XmlElement;
                rasterParaEle.SetAttribute("重采样类型", pResampleStr);
                rasterParaEle.SetAttribute("压缩类型", pCompressionStr);
                rasterParaEle.SetAttribute("金字塔", pPyramidStr);
                rasterParaEle.SetAttribute("瓦片高度", pTileHStr);
                rasterParaEle.SetAttribute("瓦片宽度", pTileWStr);
                rasterParaEle.SetAttribute("波段", pBandStr);

                break;
            }
            xmlDoc.Save(pXmlPath);
            #endregion

            this.DialogResult = DialogResult.OK;
            //MessageBox.Show("创建成功！");
            this.Close();
        }

        private void cmbRasterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRasterType.Text == "栅格编目")
            {
                //txtBand.Enabled = false;
                groupBox2.Enabled = false;
            }
            else if (cmbRasterType.Text == "栅格数据集")
            {
                //txtBand.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void rbcatalog_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbdataset.Checked)
                this.groupBox2.Enabled = true;
            else
                this.groupBox2.Enabled = false;
        }

        private void rbdataset_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbdataset.Checked)
                this.groupBox2.Enabled = true;
            else
                this.groupBox2.Enabled = false;
        }

    }
}

