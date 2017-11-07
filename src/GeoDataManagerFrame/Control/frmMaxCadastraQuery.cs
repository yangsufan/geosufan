using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geometry;
//ZQ 20110922   街坊最大地籍号查询
namespace GeoDataManagerFrame
{
    public partial class frmMaxCadastraQuery : DevComponents.DotNetBar.Office2007Form
    {
        private AxMapControl m_MapControl;
        private string _LinBanCodeFieldName = "";
        private string _XzqFieldName = "";
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
        public frmMaxCadastraQuery(IFeatureClass JFFeatureClass, IFeatureClass ZDFeatureClass, AxMapControl pMapControl, IWorkspace pWorkspace)
        {
            InitializeComponent();
            CommnClass.m_JFFeatureClass = JFFeatureClass;
            CommnClass.m_ZDFeatureClass = ZDFeatureClass;
            CommnClass.m_cmbBoxCountry = "";
            CommnClass.m_cmbBoxVillage = "";
            CommnClass.m_cmbBoxNeighbour = "";
            CommnClass.m_JFLike = "%";
            CommnClass.m_ZDLike = "%";
            CommnClass.m_Workspace = pWorkspace;
            m_MapControl = pMapControl;
            CommnClass.m_ZDLike = CommnClass.GetDescriptionOfWorkspace((ZDFeatureClass as IDataset).Workspace);
            CommnClass.m_JFLike = CommnClass.GetDescriptionOfWorkspace((JFFeatureClass as IDataset).Workspace);
            CommnClass.SetcmbBoxCountryVale(cmbBoxCountry);  //wgf 20111014 从数据中获取修改为从行政区字典中获取

            //?/????
           
        }
        public frmMaxCadastraQuery( IFeatureClass ZDFeatureClass,string LinBanCodeField,string XzqField, AxMapControl pMapControl, IWorkspace pWorkspace)
        {
            InitializeComponent();
            _LinBanCodeFieldName = LinBanCodeField;
            _XzqFieldName = XzqField;
            CommnClass._DJHFieldName = LinBanCodeField;
            CommnClass._JFDMFieldName = XzqField;
            CommnClass._JFMCFieldName = XzqField;
            CommnClass.m_JFFeatureClass = null;
            CommnClass.m_ZDFeatureClass = ZDFeatureClass;
            CommnClass.m_cmbBoxCountry = "";
            CommnClass.m_cmbBoxVillage = "";
            CommnClass.m_cmbBoxNeighbour = "";
            CommnClass.m_JFLike = "%";
            CommnClass.m_ZDLike = "%";
            CommnClass.m_Workspace = pWorkspace;
            m_MapControl = pMapControl;
            CommnClass.m_ZDLike = CommnClass.GetDescriptionOfWorkspace((ZDFeatureClass as IDataset).Workspace);
            //CommnClass.m_JFLike = CommnClass.GetDescriptionOfWorkspace((JFFeatureClass as IDataset).Workspace);
            CommnClass.SetcmbBoxCountryVale(cmbBoxCountry);  //wgf 20111014 从数据中获取修改为从行政区字典中获取

            //?/????

        }

        private void cmbBoxCountry_TextChanged(object sender, EventArgs e)
        {
            ComboBoxItem pComboBoxItem = cmbBoxCountry.SelectedItem as ComboBoxItem;
            CommnClass.m_cmbBoxCountry = pComboBoxItem.Tag.ToString();
            CommnClass.m_cmbBoxVillage = "";
            CommnClass.m_cmbBoxNeighbour = "";
            cmbBoxVillage.Items.Clear();
            cmbBoxNeighbour.Items.Clear();
            CommnClass.SetcmbBoxVillageVale(cmbBoxVillage); //wgf 20111014 从数据中获取修改为从行政区字典中获取
            //??
           
        }

        private void cmbBoxVillage_TextChanged(object sender, EventArgs e)
        {
            ComboBoxItem pComboBoxItem = cmbBoxVillage.SelectedItem as ComboBoxItem;
            CommnClass.m_cmbBoxVillage = pComboBoxItem.Tag.ToString();
            CommnClass.m_cmbBoxNeighbour = "";
            cmbBoxNeighbour.Items.Clear();
            CommnClass.SetcmbBoxNeighbourVale(cmbBoxNeighbour); //wgf 20111014 从数据中获取修改为从行政区字典中获取
           
        }
        private void cmbBoxNeighbour_TextChanged(object sender, EventArgs e)
        {
            ComboBoxItem pComboBoxItem = cmbBoxNeighbour.SelectedItem as ComboBoxItem;
            CommnClass.m_cmbBoxNeighbour = pComboBoxItem.Tag.ToString();
           
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            error.Dispose();
            this.Dispose();

        }

        private void vTreeResult_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("最大林斑号查询");
                }
                if (vTreeResult.SelectedNode.Level == 3)
                {
                    IFeature pFeature = CommnClass.Query(vTreeResult.SelectedNode.Text);
                    if (pFeature == null)
                    {
                        MessageBox.Show("未找到该行政区林斑号信息！", "提示！");
                        return;
                    }
                    ///ZQ 20111020 定位范围扩大1.5倍
                    IEnvelope pExtent = pFeature.Extent;
                    SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
                    //先刷新，后闪烁问题
                    m_MapControl.Extent = pExtent;
                    m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                    m_MapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    m_MapControl.FlashShape(pFeature.ShapeCopy, 3, 200, null);
                }
            }
            catch { }
        }

        private void btt_Click(object sender, EventArgs e)
        {
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("最大林斑号查询");
            }
            error.Clear();
            if (CommnClass.m_cmbBoxCountry == "") 
            {
                error.SetError(cmbBoxCountry, "县级行政区代码不能为空！");
                return; 
            }

            //进度条
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在查询，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            if (CommnClass.m_cmbBoxVillage == "" && CommnClass.m_cmbBoxNeighbour == "")
            {
                CommnClass.SetvTreeCountry(vTreeResult);
                vTreeResult.ExpandAll();
            }
            else if (CommnClass.m_cmbBoxNeighbour == "")
            {
                CommnClass.SetvTreeVillage(vTreeResult);
                vTreeResult.ExpandAll();
            }
            else
            {
                IFeature pFeature = CommnClass.Query(CommnClass.m_cmbBoxNeighbour);
                if (pFeature == null)
                {
                    pgss.Close();
                    MessageBox.Show("未找到行政区林斑号信息！", "提示！");
                    return;
                }
                CommnClass.SetLinBanTreeNeighbour(vTreeResult);
                vTreeResult.ExpandAll();
                ///ZQ 20111020 定位范围扩大1.5倍
                IEnvelope pExtent = pFeature.Extent;
                SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
                //先刷新，后闪烁问题
                m_MapControl.Extent = pExtent;
                m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                m_MapControl.ActiveView.ScreenDisplay.UpdateWindow();
                m_MapControl.FlashShape(pFeature.ShapeCopy, 3, 200, null);
             
            }
            ///ZQ  20111109  modfiy 修改点击查询，界面最小化
            Application.DoEvents();
            pgss.Close();

           
        }

        private void cmbBoxNeighbour_MouseClick(object sender, MouseEventArgs e)
        {
            //CommnClass.SetcmbBoxNeighbourVale(cmbBoxNeighbour);
        }    
      
    }
}
