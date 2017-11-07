using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using SysCommon.Gis;

namespace GeoDBATool
{
    public partial class FrmBatchUpdate : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppGISRef v_AppGIS;
        EnumOperateType v_OpeType;
        private System.Windows.Forms.Timer _timer;
        private IGeometry m_Geometry = null;

        IFeatureLayer m_FeaLayer = null;
        public FrmBatchUpdate(Plugin.Application.IAppGISRef pAppGIS)
        {
            InitializeComponent();
            v_AppGIS = pAppGIS;

        }
        private void FrmBatchUpdate_Load(object sender, EventArgs e)
        {
            comboBoxUptDataType.Items.Add("ESRI个人数据库(*.mdb)");
            comboBoxUptDataType.Items.Add("ESRI文件数据库(*.gdb)");

            comboBoxUptRangeType.Items.Add("ShapeFile(*.shp)");

            comboBoxUptRangeType.SelectedIndex = 0;
            comboBoxUptDataType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            
            //判断源数据是否连接
            if (this.textBoxUptRange.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置更新范围数据连接");
                return;
            }
            if (this.textBoxUptData.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置更新数据连接");
                return;
            }
            
            ClsBatchUpdate pClsBatchUpdate = new ClsBatchUpdate();
            //从数据树图中获取现势库和历史库节点
            DevComponents.AdvTree.Node pCurNode = pClsBatchUpdate.GetNodeOfProjectTree(v_AppGIS.ProjectTree, "DB", "现势库");
            DevComponents.AdvTree.Node pHisNode = pClsBatchUpdate.GetNodeOfProjectTree(v_AppGIS.ProjectTree, "DB", "历史库");
            if (pCurNode == null || pHisNode==null)
            {
                return;
            }

            //获取现势库连接
            XmlElement elementTemp = (pCurNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            IWorkspace pCurWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            if (pCurWorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接现势库失败!");
                return;
            }

            //获取历史库连接
            elementTemp = (pHisNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;            
            IWorkspace pHisWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            if (pHisWorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接历史库失败!");
                return;
            }

            //获取更新数据库连接
            SysCommon.Gis.SysGisDataSet pUptSysGisDT = new SysCommon.Gis.SysGisDataSet();//更新库连接
            SysCommon.enumWSType pUptType = SysCommon.enumWSType.PDB;
            if (this.textBoxUptData.Tag == "PDB")
            {
                pUptType = SysCommon.enumWSType.PDB;
            }
            else if(this.textBoxUptData.Tag =="GDB")
            {
                pUptType = SysCommon.enumWSType.GDB;
            }
            Exception ERR0 = null;
            pUptSysGisDT.SetWorkspace(this.textBoxUptData.Text, pUptType, out ERR0);
            IWorkspace pUptWorkSpace = pUptSysGisDT.WorkSpace;
            //获取更新范围
            IGeometry pUptGeometry = null;
            pUptGeometry = SysCommon.ModPublicFun.GetPolyGonFromFile(this.textBoxUptRange.Text);
            this.Hide();
            FrmProcessBar frmbar = new FrmProcessBar();
            frmbar.Show();
            if (pUptGeometry != null)
            {
                pClsBatchUpdate.DoBatchUpdate(pCurWorkSpace, pHisWorkSpace, pUptWorkSpace, pUptGeometry, pCurNode, pHisNode,frmbar);
            }
            frmbar.Close();
            this.Close();
        }

        

        private void buttonUptRange_Click(object sender, EventArgs e)
        {
            if (comboBoxUptRangeType.Tag == "PDB")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "选择更新范围数据";
                OpenFile.Filter = "MDB数据(*.mdb)|*.mdb";
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxUptRange.Text = OpenFile.FileName;
                }
            }
            else if(comboBoxUptRangeType.Tag=="GDB")
            {
                FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                
                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件");
                        return;
                    }
                    this.textBoxUptRange.Text = pFolderBrowser.SelectedPath;
                }
            }
            else if(comboBoxUptRangeType.Tag=="SHP")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "选择更新范围数据";
                OpenFile.Filter = "SHP数据(*.shp)|*.shp";
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxUptRange.Text = OpenFile.FileName;
                }
            }
        }


        private void btnUptData_Click(object sender, EventArgs e)
        {
            if (comboBoxUptDataType.Tag == "PDB")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "选择更新数据";
                OpenFile.Filter = "MDB数据(*.mdb)|*.mdb";
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxUptData.Text = OpenFile.FileName;
                }
            }
            else if (comboBoxUptDataType.Tag == "GDB")
            {
                FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();

                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件");
                        return;
                    }
                    this.textBoxUptData.Text = pFolderBrowser.SelectedPath;
                }
            }
            //else if (comboBoxUptRangeType.Tag == "SHP")
            //{
            //    OpenFileDialog OpenFile = new OpenFileDialog();
            //    OpenFile.Title = "选择更新数据";
            //    OpenFile.Filter = "SHP数据(*.shp)|*.shp";
            //    if (OpenFile.ShowDialog() == DialogResult.OK)
            //    {
            //        this.textBoxUptData.Text = OpenFile.FileName;
            //    }
            //}
        }

        private void comboBoxUptRangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUptRangeType.Text == "ESRI个人数据库(*.mdb)")
            {
                comboBoxUptRangeType.Tag = "PDB";
            }
            else if (comboBoxUptRangeType.Text == "ESRI文件数据库(*.gdb)")
            {
                comboBoxUptRangeType.Tag = "GDB";
            }
            else if (comboBoxUptRangeType.Text == "ShapeFile(*.shp)")
            {
                comboBoxUptRangeType.Tag = "SHP";
            }
            else if (comboBoxUptRangeType.Text == "ArcSDE(For Oracle)")
            {
                comboBoxUptRangeType.Tag = "SDE";
            }
        }

        private void comboBoxUptDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUptDataType.Text == "ESRI个人数据库(*.mdb)")
            {
                comboBoxUptDataType.Tag = "PDB";
            }
            else if (comboBoxUptDataType.Text == "ESRI文件数据库(*.gdb)")
            {
                comboBoxUptDataType.Tag = "GDB";
            }
            else if (comboBoxUptDataType.Text == "ShapeFile(*.shp)")
            {
                comboBoxUptDataType.Tag = "SHP";
            }
            else if (comboBoxUptDataType.Text == "ArcSDE(For Oracle)")
            {
                comboBoxUptDataType.Tag = "SDE";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}