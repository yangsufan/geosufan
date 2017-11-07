using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Xml;
using ESRI.ArcGIS.SystemUI;

/*-----------------------------------------------------------
 added by xisheng 20110805 缓冲坐标窗体文件 frmSelectBuffer.cs
 -----------------------------------------------------------*/
namespace GeoDataCenterFunLib
{
    public partial class frmSelectBuffer : DevComponents.DotNetBar.Office2007Form
    {

        private AxMapControl m_pMapControl = null;
        public  IMapControlDefault _pMapControl = null;
        private frmBufferSet m_frmBufferSet = null;
        private frmQuery m_frmQuery;
        private Form m_mainFrm;
        private enumQueryMode m_enumQueryMode;
        ICommand mCommand=null;
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
        public frmSelectBuffer()
        {
            InitializeComponent();
        }
        public SysCommon.BottomQueryBar QueryBar
        {
            get;
            set;
        }

        public frmSelectBuffer(AxMapControl axmapcontrol, Form mainform)
        {
            InitializeComponent();
            m_pMapControl = axmapcontrol;
            m_mainFrm = mainform;
            m_enumQueryMode = enumQueryMode.Visiable;
           // InitiaComboBox();//屏蔽加载列表框图层 xisheng 20111119
            comboBoxELayers.Text = "点击选择查询图层";
            //SetTool(); 选择图层后再选择要素
            this.TopMost = true;
            m_pMapControl.OnMouseUp += new IMapControlEvents2_Ax_OnMouseUpEventHandler(m_pMapControl_OnMouseup);
            
        }
        private void m_pMapControl_OnMouseup(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            labelX3.Text = "选中了" + m_pMapControl.Map.SelectionCount + "个要素";
        }

        private void SetTool()
        {
            if (mCommand == null)
            {
                mCommand = new ControlsSelectFeaturesToolClass();
            }
            mCommand.OnCreate(m_pMapControl.Object);
            m_pMapControl.CurrentTool = mCommand as ITool;
        }

        #region 改变图层是否可选状态
        /// <summary>
        /// 改变图层是否可选状态 added by xisheng 20110731
        /// </summary>
        private void ChangeSelectAble(bool flag, string layername)
        {
            for (int i = 0; i < m_pMapControl.Map.LayerCount; i++)
            {
                if (m_pMapControl.Map.get_Layer(i) is IGroupLayer)
                {
                    ICompositeLayer pLayer = m_pMapControl.Map.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < pLayer.Count; j++)
                    {
                        ILayer pFLayer = pLayer.get_Layer(j) as ILayer;
                        if (pFLayer != null)
                        {
                            if (pFLayer is IFeatureLayer)
                            {
                                IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
                                if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
                                {
                                    if (pFeatureLayer.Name != layername)
                                        pFeatureLayer.Selectable = flag;
                                    else
                                    {
                                        pFeatureLayer.Selectable = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ILayer pFLayer = m_pMapControl.Map.get_Layer(i) as ILayer;
                    if (pFLayer == null)
                    {
                        return;
                    }
                    if (pFLayer is IFeatureLayer)
                    {
                        IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
                        if (pFeatureLayer.Name != layername)
                            pFeatureLayer.Selectable = flag;
                        else
                        {
                            pFeatureLayer.Selectable = true;
                        }
                    }

                }
            }
        }
        #endregion


        #region 将地图中的图层加载到列表中
        /// <summary>
        /// 将地图中的图层加载到列表中 added by xisheng 20110731
        /// </summary>
        private void InitiaComboBox()
        {
            for (int i = 0; i < m_pMapControl.Map.LayerCount; i++)
            {
                if (m_pMapControl.Map.get_Layer(i) is IGroupLayer)
                {
                    ICompositeLayer pLayer = m_pMapControl.Map.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < pLayer.Count; j++)
                    {
                        ILayer pFLayer = pLayer.get_Layer(j) as ILayer;
                        if (pFLayer != null)
                        {
                            if (pFLayer is IFeatureLayer)
                            {
                                IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
                                if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
                                {
                                    comboBoxELayers.Items.Add(pFeatureLayer.Name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ILayer pFLayer = m_pMapControl.Map.get_Layer(i) as ILayer;
                    if (pFLayer != null)
                    {
                        if (pFLayer is IFeatureLayer)
                        {
                            IFeatureLayer pFeatureLayer = pFLayer as IFeatureLayer;
                            if (GetIsQuery(pFeatureLayer))//判断是否可查询 0802
                            {
                                comboBoxELayers.Items.Add(pFeatureLayer.Name);
                            }
                        }
                    }
                }

                if (comboBoxELayers.Items.Count > 0)
                {
                    comboBoxELayers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region 查询该图层是否可查询
        /// <summary>
        /// 查询该图层是否可查询 added by xisheng 20110802
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns></returns>
        public bool GetIsQuery(IFeatureLayer layer)
        {
            ILayerGeneralProperties pLayerGenPro = layer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (strNodeXml.Equals(""))
            {
                return true;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//AboutShow");
            if (pxmlnode == null)
            {
                pXmldoc = null;
                return true;
            }
            string strNodeKey = pxmlnode.Attributes["IsQuery"].Value.ToString();
            if (strNodeKey.Trim().ToUpper() == "FALSE")
            {
                pXmldoc = null;
                return false;
            }
            else
            {
                pXmldoc = null;
                return true;
            }

        }
        #endregion


        //改变图层选择方式 屏蔽之前代码 xisheng 20111119
        private void comboBoxELayers_SelectedIndexChanged(object sender, EventArgs e)
        {
           // ChangeSelectAble(false, comboBoxELayers.SelectedItem.ToString());
        }

        private void frmSelectBuffer_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChangeSelectAble(true, "");
            ESRI.ArcGIS.SystemUI.ICommand pCommand =new ControlsClearSelectionCommandClass();
            pCommand.OnCreate(m_pMapControl.Object);
            pCommand.OnClick();
            //m_pMapControl.Map.ClearSelection();
            m_pMapControl.CurrentTool = null;
            //m_pMapControl.ActiveView.PartialRefresh(,null,null);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (comboBoxELayers.Text.Trim() == "" || comboBoxELayers.Text.Trim() == "点击选择查询图层")
            {
                System.Windows.Forms.MessageBox.Show("请先选择图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<IGeometry> lstGeometrys = new List<IGeometry>();
              IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeomtryCol = (IGeometryCollection)pGeometryBag;
            IMap pMap = m_pMapControl.Map;
            object obj = System.Reflection.Missing.Value;
            //IGeometry pTempGeo = null;
            if (pMap.SelectionCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("请先选择要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Visible = false;
            m_pMapControl.CurrentTool = null;
            ChangeSelectAble(true, "");
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(this.Text);//xisheng 日志记录 0928;
            }
            IActiveView pAv = pMap as IActiveView;
            ISelection pSelection = pMap.FeatureSelection;
            IEnumFeature pEnumFea = pSelection as IEnumFeature;
            IFeature pFea = pEnumFea.Next();
            while (pFea != null)
            {
                if (pFea.Shape != null)
                {
                    lstGeometrys.Add(pFea.Shape);
                    pGeomtryCol.AddGeometry(pFea.Shape,ref obj,ref obj);
                }
                pFea = pEnumFea.Next();
            }
            pGeometryBag.Project(pMap.SpatialReference);

            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuery(_pMapControl, m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }
            ///ZQ 2011 1129 已经不再使用Element绘制
            //清除上次的所有元素
            //(m_pMapControl.Map as IGraphicsContainer).DeleteAllElements();
            if (m_frmBufferSet != null)
            {
                SysCommon.ScreenDraw.list.Remove(m_frmBufferSet.BufferSetAfterDraw); 
                m_frmBufferSet.setBufferGeometry(null);
                m_frmBufferSet = null;
            }
            m_frmBufferSet = new frmBufferSet(pGeometryBag as IGeometry, m_pMapControl.Map, m_frmQuery);
            m_frmBufferSet.FormClosed += new FormClosedEventHandler(frmBufferSet_FormClosed);
            IGeometry pGeometry = m_frmBufferSet.GetBufferGeometry();
            if (pGeometry == null || m_frmBufferSet.Res == false)
            { this.Close();  return; }

           // m_frmQuery.Show();
            ///ZQ 20111119  modify
           // m_frmQuery.FillData(m_pMapControl.ActiveView.FocusMap, pGeometry,m_frmBufferSet.pesriSpatialRelEnum);
            
            //更改查询结果显示方式
            //ygc 2012-8-10
            QueryBar.m_pMapControl = _pMapControl;
            QueryBar.EmergeQueryData(m_pMapControl.ActiveView.FocusMap, pGeometry, m_frmBufferSet.pesriSpatialRelEnum);
            try
            {
                DevComponents.DotNetBar.Bar pBar = QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                if (pBar != null)
                {
                    pBar.AutoHide = false;
                    //pBar.SelectedDockTab = 1;
                    int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                    pBar.SelectedDockTab = tmpindex;
                }
            }
            catch
            { }
            this.Close();
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
            if (m_frmBufferSet!=null) m_frmBufferSet.setBufferGeometry(null);//added by chulili 20110731
           // SetTool();
            //m_pMapControl.ActiveView.Refresh();
        }

        private void frmBufferSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmBufferSet.setBufferGeometry(null);//added by chulili 20110731
            //SetTool();
            ///ZQ 2011 1129 modify
            //m_pMapControl.Map.ClearSelection();
            //m_pMapControl.ActiveView.Refresh();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //选择图层后 重新选择要素
        private void comboBoxELayers_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxELayers.Text.Trim() == "" || comboBoxELayers.Text.Trim() == "点击选择查询图层")
                return;
            bttSelectTool.Enabled = true;
            ChangeSelectAble(false, comboBoxELayers.Text);
        }

        private void comboBoxELayers_Click(object sender, EventArgs e)
        {
            SysCommon.SelectLayerByTree frm = new SysCommon.SelectLayerByTree(1, Plugin.ModuleCommon.TmpWorkSpace,Plugin.ModuleCommon.ListUserdataPriID );
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.m_NodeKey.Trim() != "")
                {
                    IFeatureLayer pfeaturelayer = new FeatureLayerClass();
                    IFeatureClass pFeatureClass = pfeaturelayer.FeatureClass;
                    pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath, frm.m_NodeKey);
                    if (pFeatureClass != null)
                    {
                        comboBoxELayers.Text = frm.m_NodeText;
                    }
                }
            }
        }

        private void bttSelectTool_Click(object sender, EventArgs e)
        {
            SetTool();
        }

    
    }

}
