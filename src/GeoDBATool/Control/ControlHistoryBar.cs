using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using System.Xml;

namespace GeoDBATool
{
    public partial class ControlHistoryBar : UserControl
    {
        private AxMapControl ArcGisMapControl;
        private IMapControlDefault Mapcontrol;
        private ITOCControlDefault Toccontrol;
        private IMap _map;               //一定将map设为全局变量否则IActiveViewEvents_Event相关事件无法响应
        private System.Windows.Forms.Timer _timer;
        private DevComponents.DotNetBar.Bar BarHistoryDataCompare;
        private DevComponents.DotNetBar.DotNetBarManager MainDotNetBarManager;
        private DevComponents.AdvTree.AdvTree m_ProTree = null;   //cyf 20110705 add
            
        public DevComponents.DotNetBar.Bar BarHistoryManage
        {
            get { return this.BarHistory; }
        }

        public ControlHistoryBar(AxMapControl arcGisMapControl, ITOCControlDefault tocControl,DevComponents.AdvTree.AdvTree pProTree, DevComponents.DotNetBar.Bar barHistory, DevComponents.DotNetBar.DotNetBarManager dotNetBarManager)
        {
            InitializeComponent();
            InitialFrm();
            ArcGisMapControl = arcGisMapControl;
            Mapcontrol = ArcGisMapControl.Object as IMapControlDefault;
            m_ProTree = pProTree;//cyf 20110705 add 
            if (m_ProTree == null) return;//cyf 20110705 add 
            Toccontrol = tocControl;
            BarHistoryDataCompare = barHistory;
            MainDotNetBarManager = dotNetBarManager;
            _map = Mapcontrol.Map;
            ((IActiveViewEvents_Event)_map).ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(LayerControl_ItemDeleted); 
        }
//added by chulili 20110719 根据数据集加载历史数据
        public void AddHistoryDataByFD(string[] strTemp,string strType)
        {
            Mapcontrol.ClearLayers();
            Toccontrol.Update();
            Application.DoEvents();

            //加载历史数据
            Exception err = null;
            SysCommon.Gis.SysGisDataSet sourceSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            switch (strType)
            {
                case "SDE":
                    sourceSysGisDataSet.SetWorkspace(strTemp[0], strTemp[1], strTemp[2], strTemp[3], strTemp[4], strTemp[5], out err);
                    break;
                case "PDB":
                    sourceSysGisDataSet.SetWorkspace(strTemp[2], SysCommon.enumWSType.PDB, out err);
                    break;
                case "GDB":
                    sourceSysGisDataSet.SetWorkspace(strTemp[2], SysCommon.enumWSType.GDB, out err);
                    break;
            }

            if (err != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("连接数据库失败", "原因:" + err.Message);
                return;
            }
            //cyf 20110706 add
            DevComponents.AdvTree.Node ProjectNode = new DevComponents.AdvTree.Node();
            ProjectNode = m_ProTree.SelectedNode;
            while (ProjectNode.Parent != null)
            {
                ProjectNode = ProjectNode.Parent;
            }
            //cyf 20110625 add:
            DevComponents.AdvTree.Node DBNode = new DevComponents.AdvTree.Node(); //数据库树节点
            //获取数据库节点
            DBNode = m_ProTree.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode.DataKeyString != "DB")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库节点失败!");
                return;
            }

            DevComponents.AdvTree.Node DtSetNode = new DevComponents.AdvTree.Node(); //数据集树节点
            if (DBNode.Text == "现势库" || DBNode.Text == "历史库") //.DataKeyString == "现势库"
            {
                //获取数据集节点
                DtSetNode = m_ProTree.SelectedNode;
                while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "FD")
                {
                    DtSetNode = DtSetNode.Parent;
                }
                if (DtSetNode.DataKeyString != "FD")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集节点失败!");
                    return;
                }
            }
            //end
            //added by chulili 20110719
            XmlElement elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            IWorkspace TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;

            SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
            IFeatureDataset featureDataset = null;        //数据集  
            IGroupLayer pGroupLayer = new GroupLayerClass();
            if (m_ProTree.SelectedNode.DataKeyString == "FD")
            {
                featureDataset = sysGisDataset.GetFeatureDataset(m_ProTree.SelectedNode.Text, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败，请检查该数据集是否存在!");
                    return;
                }
                pGroupLayer.Name = m_ProTree.SelectedNode.Text + "_" + ProjectNode.Text;
            }
            else if (m_ProTree.SelectedNode.DataKeyString == "FC")
            {
                featureDataset = sysGisDataset.GetFeatureDataset(DtSetNode.Text, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败!");
                    return;
                }
                pGroupLayer.Name = DtSetNode.Text + "_" + ProjectNode.Text;
            }
            List<IDataset> lstDataset = sysGisDataset.GetFeatureClass(featureDataset);
            //end added by chulili
            //List<string> lstNames = sourceSysGisDataSet.GetFeatureClassNames();
            XmlElement feaclsElem = null;
            try { feaclsElem = (m_ProTree.SelectedNode.Tag as XmlElement).SelectSingleNode(".//图层名") as XmlElement; }
            catch { }

            foreach (IDataset dataset in lstDataset)
            {
                IFeatureClass pFeatureClass = dataset as IFeatureClass;
                if (pFeatureClass == null) continue;
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                if (pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatureLayer = new FDOGraphicsLayerClass();
                }
                pFeatureLayer.FeatureClass = pFeatureClass;

                pFeatureLayer.Name = dataset.Name;
                if (m_ProTree.SelectedNode.DataKeyString == "FC")
                {
                    //加载指定的图层
                    if (m_ProTree.SelectedNode.Text != pFeatureLayer.Name) continue;
                }
                else if (m_ProTree.SelectedNode.DataKeyString == "FD")
                {
                    if (feaclsElem != null)
                    {
                        if (!feaclsElem.GetAttribute("名称").Contains(pFeatureLayer.Name))
                        {
                            //若不具备数据权限，则不进行加载
                            continue;
                        }
                    }

                }
                pGroupLayer.Add(pFeatureLayer as ILayer);
            }
            Mapcontrol.Map.AddLayer(pGroupLayer);
            SysCommon.Gis.ModGisPub.LayersCompose(Mapcontrol);

            InitialSliderItem(Mapcontrol);
        }

        public void AddHistoryData(string[] strTemp,string strType)
        {
            Mapcontrol.ClearLayers();
            Toccontrol.Update();
            Application.DoEvents();

            //加载历史数据
            Exception err = null;
            SysCommon.Gis.SysGisDataSet sourceSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            switch (strType)
            {
                case "SDE":
                    sourceSysGisDataSet.SetWorkspace(strTemp[0], strTemp[1], strTemp[2], strTemp[3], strTemp[4], strTemp[5], out err);
                    break;
                case "PDB":
                    sourceSysGisDataSet.SetWorkspace(strTemp[2], SysCommon.enumWSType.PDB, out err);
                    break;
                case "GDB":
                    sourceSysGisDataSet.SetWorkspace(strTemp[2], SysCommon.enumWSType.GDB, out err);
                    break;
            }
            string WsUsername = strTemp[3];//记录用户名 席胜20111020
            if (err != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("连接数据库失败", "原因:" + err.Message);
                return;
            }
            //cyf 20110706 add
            DevComponents.AdvTree.Node ProjectNode = new DevComponents.AdvTree.Node();
            ProjectNode = m_ProTree.SelectedNode;
            while (ProjectNode.Parent != null)
            {
                ProjectNode = ProjectNode.Parent;
            }
            //cyf 20110625 add:
            DevComponents.AdvTree.Node DBNode = new DevComponents.AdvTree.Node(); //数据库树节点
            //获取数据库节点
            DBNode = m_ProTree.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode.DataKeyString != "DB")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库节点失败!");
                return;
            }

            DevComponents.AdvTree.Node DtSetNode = new DevComponents.AdvTree.Node(); //数据集树节点
            if (DBNode.Text == "现势库" || DBNode.Text == "历史库") //.DataKeyString == "现势库"
            {
                //获取数据集节点
                DtSetNode = m_ProTree.SelectedNode;
                while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "FD")
                {
                    DtSetNode = DtSetNode.Parent;
                }
                if (DtSetNode.DataKeyString != "FD")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集节点失败!");
                    return;
                }
            }
            //end

            //List<string> lstNames = sourceSysGisDataSet.GetFeatureClassNames();
            //if (lstNames == null) return;
            //foreach (string name in lstNames)
            //{
            //    if (name.EndsWith("_GOH"))
            //    {
            //        IFeatureClass featCls = sourceSysGisDataSet.GetFeatureClass(name, out err);
            //        IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            //        if (featCls.FeatureType == esriFeatureType.esriFTAnnotation)
            //        {
            //            pFeatureLayer = new FDOGraphicsLayerClass();
            //        }

            //        pFeatureLayer.FeatureClass = featCls;
            //        //cyf 20110706 modify:不去掉图层名
            //        //if (strTemp[3].Trim() != "")
            //        //{
            //        //    if ((featCls as IDataset).Name.ToUpper().Contains(strTemp[3].Trim().ToUpper()))
            //        //    {
            //        //        //SDE,图层名带用户名，应该去掉
            //        //        pFeatureLayer.Name = (featCls as IDataset).Name.Substring(strTemp[3].Trim().Length+1);
            //        //    }
            //        //}
            //        //else
            //        //{
            //            pFeatureLayer.Name = (featCls as IDataset).Name;
            //        //}
            //        //end
            //        //cyf 20110705 add
            //        //加载具备权限的图层
            //        XmlElement feaclsElem = null;
            //        try { feaclsElem = (m_ProTree.SelectedNode.Tag as XmlElement).SelectSingleNode(".//图层名") as XmlElement; } catch { }
					
            //        if (feaclsElem != null)
            //        {
            //            if (!feaclsElem.GetAttribute("名称").Contains(pFeatureLayer.Name))
            //            {
            //                //若不具备数据权限，则不进行加载
            //                continue;
            //            }
            //        }
            //        //end
            //        Mapcontrol.Map.AddLayer(pFeatureLayer as ILayer);
            //    }
            //}

            //lstNames = sourceSysGisDataSet.GetAllFeatureDatasetNames();
            //foreach (string name in lstNames)
            //{
            //    if (!name.EndsWith("_GOH"))
            //    {
            //        continue;
            //    }
            //    //cyf 20110706 modify
            //    IGroupLayer pGroupLayer = new GroupLayerClass();
            //    IFeatureDataset featureDataset = null;        //数据集
            //    if (m_ProTree.SelectedNode.DataKeyString == "FD")
            //    {
            //        featureDataset = sourceSysGisDataSet.GetFeatureDataset(m_ProTree.SelectedNode.Text, out err);
            //        if (err != null)
            //        {
            //            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败，请检查历史库是否存在!");
            //            return;
            //        }
            //        pGroupLayer.Name = m_ProTree.SelectedNode.Text + "_" + ProjectNode.Text;
            //    }
            //    else if (m_ProTree.SelectedNode.DataKeyString == "FC")
            //    {
            //        featureDataset = sourceSysGisDataSet.GetFeatureDataset(DtSetNode.Text, out err);
            //        if (err != null)
            //        {
            //            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败!");
            //            return;
            //        }
            //        pGroupLayer.Name = DtSetNode.Text + "_" + ProjectNode.Text;
            //    }
            //    //end
                IGroupLayer pGroupLayer =   new GroupLayerClass();
                pGroupLayer.Name = m_ProTree.SelectedNode.Text + "_" + ProjectNode.Text;
                IFeatureDataset pFeatureDataset = sourceSysGisDataSet.GetFeatureDataset(DtSetNode.Text, out err);
                IEnumDataset pEnumDs = pFeatureDataset.Subsets;
                pEnumDs.Reset();
                IDataset pDs = pEnumDs.Next();
                //ModDBOperator.WriteLog("while start");
                while (pDs != null)
                {
                    IFeatureClass featCls = pDs as IFeatureClass;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    if (featCls.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        pFeatureLayer = new FDOGraphicsLayerClass();
                    }
                    pFeatureLayer.FeatureClass = featCls;
                 //去用户名 xisheng 0906
                    if (strTemp[3].Trim() != "")
                    {
                        if (pDs.Name.ToUpper().Contains(strTemp[3].Trim().ToUpper()))
                        {
                            //SDE,图层名带用户名，应该去掉
                            pFeatureLayer.Name = pDs.Name.Substring(strTemp[3].Trim().Length + 1);
                        }
                    }
                    else
                    {
                        pFeatureLayer.Name = pDs.Name;
                    }
                    //end
                    //pFeatureLayer.Name = pDs.Name;
                    //cyf 20110705 add
                    //加载具备权限的图层
                    XmlElement feaclsElem = null;
                    if (m_ProTree.SelectedNode.DataKeyString == "FC")
                    {
                        //加载指定的图层
                        if (m_ProTree.SelectedNode.Text != pFeatureLayer.Name)
                        {
                            pDs = pEnumDs.Next();
                            continue;
                        }
                        else
                        {
                            pGroupLayer.Add(pFeatureLayer as ILayer);
                            break;
                        }
                    }
                    else if (m_ProTree.SelectedNode.DataKeyString == "FD")
                    {
                        //数据集节点
                        try { feaclsElem = (m_ProTree.SelectedNode.Tag as XmlElement).SelectSingleNode(".//图层名") as XmlElement; }
                        catch { }

                        if (feaclsElem != null)
                        {
                            if (!feaclsElem.GetAttribute("名称").Contains(pFeatureLayer.Name))
                            {
                                //若不具备数据权限，则不进行加载
                                pDs = pEnumDs.Next();
                                continue;
                            }
                        }
                    }
                    //end
                    //cyf 20110706 modify
                    pGroupLayer.Add(pFeatureLayer as ILayer);
                    //Mapcontrol.Map.AddLayer(pFeatureLayer as ILayer);
                    //end
                    pDs = pEnumDs.Next();
                }
                //ModDBOperator.WriteLog("while end");
                Mapcontrol.Map.AddLayer(pGroupLayer as ILayer);
                Mapcontrol.ActiveView.Refresh();

            //ModDBOperator.WriteLog("LayersCompose start");
            SysCommon.Gis.ModGisPub.LayersCompose(Mapcontrol);
            //写日志
            //ModDBOperator.WriteLog("InitialSliderItem start");
            InitialSliderItem(Mapcontrol);
            //写日志
            //ModDBOperator.WriteLog("InitialSliderItem end");
        }

        private void InitialFrm()
        {
            BarHistory.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            BarHistory.DockOrientation = DevComponents.DotNetBar.eOrientation.Horizontal;
            BarHistory.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            BarHistory.Dock = System.Windows.Forms.DockStyle.Top;
            BarHistory.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            BarHistory.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;

            sliderItem.Enabled = false;
            comboBoxItem.Enabled = false;
            btnCompare.Enabled = false;
            btnSelFeatures.Enabled = false;
            btnSelFeaturesByCondition.Enabled = false;
            btnBrowse.Enabled = false;
            btnRenderHistoryData.Enabled = false;
            btnStract.Enabled = false;
            //利用计时器刷新mapcontrol
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 800;
            _timer.Enabled = false;
            _timer.Tick += new EventHandler(Timer_Tick);
        }
        
        //利用计时器刷新mapcontrol
        private void Timer_Tick(object sender, EventArgs e)
        {
            GeoUtilities.ControlsSelFeature pControlsSelFeature = Mapcontrol.CurrentTool as GeoUtilities.ControlsSelFeature;
            if (pControlsSelFeature == null)
            {
                btnBrowse.Enabled = false;
            }
            else
            {
                btnBrowse.Enabled = true;
            }
        }

        private void btnAddHistoryData_Click(object sender, EventArgs e)
        {
            frmDBPropertySet newFrm = new frmDBPropertySet("历史库连接设置");
            newFrm.ShowDialog(this);

            if (newFrm.Res)
            {
                string[] strTemp = newFrm.GetPropertySetStr.Split('|');
                string strType = newFrm.DBType;

                AddHistoryData(strTemp, strType);
            }
        }

        private void LayerControl_ItemDeleted(object Item)
        {
            InitialSliderItem(Mapcontrol);
        }
        private void InitialSliderItem(IMapControlDefault mapcontrol)
        {
            comboBoxItem.Items.Clear();
            if (mapcontrol.Map.LayerCount == 0)
            {
                sliderItem.Enabled = false;
                comboBoxItem.Enabled = false;
                btnCompare.Enabled = false;
                btnSelFeatures.Enabled = false;
                btnSelFeaturesByCondition.Enabled = false;
                btnBrowse.Enabled = false;
                btnRenderHistoryData.Enabled = false;
                btnStract.Enabled = false;
                Mapcontrol.CurrentTool = null;
                _timer.Enabled = false;
                return;
            }

            ArrayList arrayListFromDate = new ArrayList();
            //计算各历史图层的fromdate字段唯一值
            for (int i = 0; i < mapcontrol.Map.LayerCount; i++)
            {
                //cyf 20110706 add:
                ILayer mLayer = mapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComLayer.get_Layer(j) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        ITable table = featLay.FeatureClass as ITable;
                        if (table == null) continue;
                        if (table.Fields.FindField("FromDate") < 0 || table.Fields.FindField("ToDate") < 0) continue;

                        IDataStatistics statistics = new DataStatisticsClass();
                        statistics.Cursor = table.Search(null, false);
                        statistics.Field = "FromDate";
                        //ModDBOperator.WriteLog(featLay.Name +"UniqueValues start");
                        IEnumerator enumDate = statistics.UniqueValues;
                        //ModDBOperator.WriteLog("UniqueValues end");
                        enumDate.Reset();
                        while (enumDate.MoveNext())
                        {
                            if (!arrayListFromDate.Contains(enumDate.Current))
                            {
                                arrayListFromDate.Add(enumDate.Current);
                            }
                        }
                    }
                }//end
                else
                {
                    IFeatureLayer featLay = mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    ITable table = featLay.FeatureClass as ITable;
                    if (table == null) continue;
                    if (table.Fields.FindField("FromDate") < 0 || table.Fields.FindField("ToDate") < 0) continue;

                    IDataStatistics statistics = new DataStatisticsClass();
                    statistics.Cursor = table.Search(null, false);
                    statistics.Field = "FromDate";
                    IEnumerator enumDate = statistics.UniqueValues;
                    enumDate.Reset();
                    while (enumDate.MoveNext())
                    {
                        if (!arrayListFromDate.Contains(enumDate.Current))
                        {
                            arrayListFromDate.Add(enumDate.Current);
                        }
                    }
                }
            }

            if (arrayListFromDate.Count== 0)
            {
                sliderItem.Enabled = false;
                comboBoxItem.Enabled = false;
                btnCompare.Enabled = false;
                btnSelFeatures.Enabled = false;
                btnSelFeaturesByCondition.Enabled = false;
                btnBrowse.Enabled = false;
                btnRenderHistoryData.Enabled = false;
                btnStract.Enabled = false;
                Mapcontrol.CurrentTool = null;
                _timer.Enabled = false;
                return;
            }

            //组合形成时间段
            sliderItem.Enabled = true;
            comboBoxItem.Enabled = true;
            btnCompare.Enabled = true;
            btnSelFeatures.Enabled = true;
            btnSelFeaturesByCondition.Enabled = true;
            btnRenderHistoryData.Enabled = true;
            btnStract.Enabled = true;
            _timer.Enabled = true;
            arrayListFromDate.Sort();
            for (int i = 0; i < arrayListFromDate.Count; i++)
            {
                comboBoxItem.Items.Add(arrayListFromDate[i]);
            }
            comboBoxItem.SelectedIndex = arrayListFromDate.Count-1;
            sliderItem.Maximum = arrayListFromDate.Count;
            sliderItem.Minimum = 1;
            sliderItem.Value = arrayListFromDate.Count;
            sliderItem.Tag = arrayListFromDate;
            
        }

        private void sliderItem_ValueChanged(object sender, EventArgs e)
        {
            comboBoxItem.SelectedIndex = sliderItem.Value - 1;

            ChangeLayersDef();
        }

        private void ChangeLayersDef()
        {
            for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
            {
                //cyf 20110706 add
                ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComlayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComlayer.get_Layer(j) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        if (featLay.FeatureClass.Fields.FindField("FromDate") < 0 || featLay.FeatureClass.Fields.FindField("ToDate") < 0) continue;
                        IFeatureLayerDefinition featLayDef = featLay as IFeatureLayerDefinition;
                        featLayDef.DefinitionExpression = "FromDate<='" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "' and ToDate>'" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "'";
            
                    }
                }//end
                else
                {
                    IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    if (featLay.FeatureClass.Fields.FindField("FromDate") < 0 || featLay.FeatureClass.Fields.FindField("ToDate") < 0) continue;
                    IFeatureLayerDefinition featLayDef = featLay as IFeatureLayerDefinition;
                    featLayDef.DefinitionExpression = "FromDate<='" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "' and ToDate>'" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "'";
                }
            }

            Mapcontrol.ActiveView.Refresh();
        }
        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            sliderItem.Value = comboBoxItem.SelectedIndex + 1;
            ChangeLayersDef();
        }

        private void btnRenderHistoryData_Click(object sender, EventArgs e)
        {
            if (!btnRenderHistoryData.Checked)
            {
                for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
                {
                    //cyf 20110706 add
                    ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                    if (mLayer is IGroupLayer)
                    {
                        ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                        for (int j = 0; j < pComlayer.Count; j++)
                        {
                            IFeatureLayer featLay = pComlayer.get_Layer(j) as IFeatureLayer;
                            if (featLay == null) continue;
                            if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                            if (featLay.FeatureClass.Fields.FindField("State") < 0) continue;

                            try
                            {
                                //先将原来的符号保存起来
                                if (!ModData.m_DicFeaLayerRender.ContainsKey(featLay))
                                {
                                    IGeoFeatureLayer mGeoFeaLayer = featLay as IGeoFeatureLayer;
                                    if (mGeoFeaLayer != null)
                                    {
                                        IFeatureRenderer pFeaRender = mGeoFeaLayer.Renderer;
                                        if (pFeaRender != null)
                                        {
                                            ModData.m_DicFeaLayerRender.Add(featLay, pFeaRender);
                                        }
                                    }
                                }
                            } catch
                            { }

                            SetDataUpdateSymbol(featLay, "State");
                        }
                    }//end
                    else
                    {
                        IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        if (featLay.FeatureClass.Fields.FindField("State") < 0) continue;

                        try
                        {
                            //先将原来的符号保存起来
                            if (!ModData.m_DicFeaLayerRender.ContainsKey(featLay))
                            {
                                IGeoFeatureLayer mGeoFeaLayer = featLay as IGeoFeatureLayer;
                                if (mGeoFeaLayer != null)
                                {
                                    IFeatureRenderer pFeaRender = mGeoFeaLayer.Renderer;
                                    if (pFeaRender != null)
                                    {
                                        ModData.m_DicFeaLayerRender.Add(featLay, pFeaRender);
                                    }
                                }
                            }
                        } catch
                        { }

                        SetDataUpdateSymbol(featLay, "State");
                    }
                }
            }
            else
            {
                //取消渲染
                if (ModData.m_DicFeaLayerRender == null || ModData.m_DicFeaLayerRender.Count == 0) return;
                //遍历图层，回复默认符号
                for (int i = 0; i <  Mapcontrol.Map.LayerCount; i++)
                {
                    //cyf 20110706 add
                    ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                    if (mLayer is IGroupLayer)
                    {
                         ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                         for (int j = 0; j < pComlayer.Count; j++)
                         {
                             IFeatureLayer pFeaLayer = pComlayer.get_Layer(j) as IFeatureLayer;
                             if (ModData.m_DicFeaLayerRender.ContainsKey(pFeaLayer))
                             {
                                 //设置默认符号
                                 IGeoFeatureLayer pGeoFeaLayer = pFeaLayer as IGeoFeatureLayer;
                                 if (pGeoFeaLayer != null)
                                 {
                                     pGeoFeaLayer.Renderer = ModData.m_DicFeaLayerRender[pFeaLayer];
                                 }
                             }
                         }
                    }//end
                    else
                    {
                        IFeatureLayer pFeaLayer = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                        if (ModData.m_DicFeaLayerRender.ContainsKey(pFeaLayer))
                        {
                            //设置默认符号
                            IGeoFeatureLayer pGeoFeaLayer = pFeaLayer as IGeoFeatureLayer;
                            if (pGeoFeaLayer != null)
                            {
                                pGeoFeaLayer.Renderer = ModData.m_DicFeaLayerRender[pFeaLayer];
                            }
                        }
                    }
                }
            }

            btnRenderHistoryData.Checked = !btnRenderHistoryData.Checked;

            Mapcontrol.ActiveView.Refresh();
            Toccontrol.Update();
        }

        /// <summary>
        /// 渲染更新数据 渲染依据字段 日志记录表.state 1-新建,2-修改,3-删除
        /// </summary>
        /// <param name="pFeatureLayer"></param>
        /// <param name="strFieldName"></param>
        private void SetDataUpdateSymbol(IFeatureLayer pFeatureLayer, string strFieldName)
        {
            if (pFeatureLayer == null || strFieldName == string.Empty) return;
            Dictionary<string, string> dicFieldValue = new Dictionary<string, string>();
            Dictionary<string, ISymbol> dicFieldSymbol = new Dictionary<string, ISymbol>();
            
            ISymbol pSymbol = null;
            dicFieldValue.Add("1", "新建");
            pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 35, 255, 255);
            dicFieldSymbol.Add("1", pSymbol);

            dicFieldValue.Add("2", "修改");
            pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 38, 254, 7);
            dicFieldSymbol.Add("2", pSymbol);

            dicFieldValue.Add("3", "删除");
            pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 254, 7, 7); ;
            dicFieldSymbol.Add("3", pSymbol);

            dicFieldValue.Add("0", "未变化");
            pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 178, 178, 178); ;
            dicFieldSymbol.Add("0", pSymbol);

            SysCommon.Gis.ModGisPub.SetLayerUniqueValueRenderer(pFeatureLayer, strFieldName, dicFieldValue, dicFieldSymbol, false);
        }
        private ISymbol CreateSymbol(esriGeometryType pGeometryType, int intR, int intG, int intB)
        {
            ISymbol pSymbol = null;
            ISimpleLineSymbol pSimpleLineSymbol = null;
            IColor pColor = SysCommon.Gis.ModGisPub.GetRGBColor(intR, intG, intB);
            switch (pGeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = SysCommon.Gis.ModGisPub.GetRGBColor(156, 156, 156);
                    pSimpleLineSymbol.Width = 0.01;
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    pSimpleFillSymbol.Outline = pSimpleLineSymbol;
                    pSimpleFillSymbol.Color = pColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol = pSimpleFillSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpleMarkerSymbol.Color = pColor;
                    pSimpleMarkerSymbol.Size = 2;
                    pSymbol = pSimpleMarkerSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = pColor;
                    pSimpleLineSymbol.Width = 0.1;
                    pSymbol = pSimpleLineSymbol as ISymbol;
                    break;
            }

            return pSymbol;
        }

        private void btnStract_Click(object sender, EventArgs e)
        {
            ArrayList arrayListFromDate = sliderItem.Tag as ArrayList;
            if (arrayListFromDate == null) return;
            frmStractHistorySet newFrm = new frmStractHistorySet(Mapcontrol.Map, arrayListFromDate,comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString(),false);
            newFrm.ShowDialog(this);
        }

        private void btnSelFeatures_Click(object sender, EventArgs e)
        {
            GeoUtilities.ControlsSelFeature controlsEditSelFeature = new GeoUtilities.ControlsSelFeature();

            ITool tool = controlsEditSelFeature as ITool;
            ICommand cmd = tool as ICommand;
            cmd.OnCreate(Mapcontrol);
            Mapcontrol.Map.ClearSelection();
            Mapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, Mapcontrol.ActiveView.FullExtent);
            Mapcontrol.CurrentTool = tool;

            //获得要选择的图层
            List<ILayer> layerList = new List<ILayer>();//可以选择的图层列表
            for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
            {
                //cyf 20110706 add
                ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComlayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComlayer.get_Layer(j) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        layerList.Add(featLay);
                    }
                }//end
                else
                {
                    IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    layerList.Add(featLay);
                }
            }
            controlsEditSelFeature.LayerList = layerList;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (Mapcontrol.Map.SelectionCount == 0) return;
            List<IFeatureLayer> layerList = new List<IFeatureLayer>();
            for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
            {
                 //cyf 20110706 add
                ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComlayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComlayer.get_Layer(j) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        IFeatureSelection fealSel = featLay as IFeatureSelection;
                        if (fealSel.SelectionSet.Count != 0)
                        {
                            layerList.Add(featLay);
                        }
                    }
                }//end
                else
                {
                    IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    IFeatureSelection fealSel = featLay as IFeatureSelection;
                    if (fealSel.SelectionSet.Count != 0)
                    {
                        layerList.Add(featLay);
                    }
                }
            }

            if (layerList.Count == 0) return;

            frmBrowseHistorySelFeatures newFrm = new frmBrowseHistorySelFeatures(layerList);
            newFrm.Show();
        }

        //对比查看各个版本的历史数据
        private void btnCompare_Click(object sender, EventArgs e)
        {
            BarHistoryDataCompare.Items.Clear();
            BarHistoryDataCompare.Hide();

            MainDotNetBarManager.RightDockSite.Controls.Add(BarHistoryDataCompare);
            MainDotNetBarManager.RightDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(BarHistoryDataCompare, 664, 126)))}, DevComponents.DotNetBar.eOrientation.Vertical);
            BarHistoryDataCompare.Size = new System.Drawing.Size(362, 228);

            DevComponents.DotNetBar.DockContainerItem dockItemHistoryData = new DevComponents.DotNetBar.DockContainerItem("dockItemHistoryData0", "历史数据对比浏览");
            DevComponents.DotNetBar.PanelDockContainer PanelTipHistoryData = new DevComponents.DotNetBar.PanelDockContainer();
            frmArcgisMapControl newFrmArcgisMapControl = new frmArcgisMapControl();
            newFrmArcgisMapControl.ArcGisMapControl.Dock = DockStyle.Fill;
            PanelTipHistoryData.Controls.Add(newFrmArcgisMapControl.ArcGisMapControl);
            dockItemHistoryData.Control = PanelTipHistoryData;
            BarHistoryDataCompare.Items.Add(dockItemHistoryData);
            BarHistoryDataCompare.Show();
            BarHistoryDataCompare.AutoHide = true;
                        
            ArrayList arrayListFromDate = sliderItem.Tag as ArrayList;
            if (arrayListFromDate == null) return;
            frmSelHistoryDataVersion newFrm = new frmSelHistoryDataVersion(arrayListFromDate, ArcGisMapControl, BarHistoryDataCompare,false);
            newFrm.ShowDialog(this);
        }

        private void btnSelFeaturesByCondition_Click(object sender, EventArgs e)
        {
            GeoUtilities.FrmSQLQuery frmSQL = new GeoUtilities.FrmSQLQuery(Mapcontrol, false);
            frmSQL.ShowDialog(this);

            if (frmSQL.DialogResult == DialogResult.OK)
            {
                GeoUtilities.ControlsSelFeature controlsEditSelFeature = new GeoUtilities.ControlsSelFeature();

                ITool tool = controlsEditSelFeature as ITool;
                ICommand cmd = tool as ICommand;
                cmd.OnCreate(Mapcontrol);
                Mapcontrol.CurrentTool = tool;

                //获得要选择的图层
                List<ILayer> layerList = new List<ILayer>();//可以选择的图层列表
                for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
                {  //cyf 20110706 add
                    ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                    if (mLayer is IGroupLayer)
                    {
                        ICompositeLayer pComlayer = mLayer as ICompositeLayer;
                        for (int j = 0; j < pComlayer.Count; j++)
                        {
                            IFeatureLayer featLay = pComlayer.get_Layer(j) as IFeatureLayer;
                            if (featLay == null) continue;
                            if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                            layerList.Add(featLay);
                        }
                    }//end
                    else
                    {
                        IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        layerList.Add(featLay);
                    }
                }
                controlsEditSelFeature.LayerList = layerList;
            }
        }
    }
}
