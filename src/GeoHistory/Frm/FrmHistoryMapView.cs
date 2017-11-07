using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using GeoHistory.Control;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Data.OleDb;
using System.IO;

namespace GeoHistory
{
    public partial class FrmHistoryMapView : DevComponents.DotNetBar.Office2007Form
    {
        public IEnvelope MainMapExtent = null;

        private AxMapControl MapMain = null;
        private UCHistoryMap m_pMainXucHis = null;
        private DevComponents.AdvTree.AdvTree _ProjectTree = null;
        private IFeatureClass _OldFeatureClass = null;
        private IFeatureClass _NewFeatureClass = null;
        private string _MdbPath = Application.StartupPath + "\\..\\Bin";
        private string _MdbName = "TmpResMdb.mdb";
        private string _PrjFileName = Application.StartupPath + "\\..\\Prj\\Xian 1980 3 Degree GK CM 114E.prj";
        private string _IntersectName = "TmpIntersect1";
        private SysCommon.CProgress  _Progress = null;
        public FrmHistoryMapView(IEnvelope _pEnv, IMap _pMap,DevComponents.AdvTree.AdvTree pProjectTree)
        {
            InitializeComponent();
            _ProjectTree = pProjectTree;
            MainMapExtent = _pEnv;
            //ucHistoryMap1.AxMapCtrlHis.Map = new MapClass();
            //ucHistoryMap2.AxMapCtrlHis.Map = new MapClass();
            //ucHistoryMap1.AxMapCtrlHis.Map = _pMap;
            //initControls();
            xTabHis.SelectedIndex = 0;//日期段初始显示
        }
        private void initControls()
        {
            //    LoadMap();
            MapMain = ucHistoryMap1.AxMapCtrlHis;
            ucHistoryMap1.AxMapCtrlHis.Extent = MainMapExtent;
            ucHistoryMap1.AxMapCtrlHis.Map.Name = "地图1";
            IMapControlDefault pMCD=MapMain.Object as IMapControlDefault;
            IFeatureLayer pFeatureLayer = ModDBOperator.GetMapFrameLayer("zone", pMCD, "示意图") as IFeatureLayer;
            ModHistory.SetMapLyrsDefinitionOfHPoint(DateTime.Now.ToString("yyyy-MM-dd"), ucHistoryMap1.AxMapCtrlHis);
            ucHistoryMap1.AxMapCtrlHis.Refresh();
            IObjectCopy pOC = new ObjectCopyClass();
            ucHistoryMap2.AxMapCtrlHis.Map = pOC.Copy(ucHistoryMap1.AxMapCtrlHis.Map) as IMap;
            ucHistoryMap2.AxMapCtrlHis.Map.Name = "地图2";
            ModHistory.SetMapLyrsDefinitionOfHPoint(DateTime.Now.ToString("yyyy-MM-dd"), ucHistoryMap2.AxMapCtrlHis);
            ucHistoryMap2.AxMapCtrlHis.Refresh();
            MapMain = ucHistoryMap1.AxMapCtrlHis;
            m_pMainXucHis = ucHistoryMap1;
            setAxMapControl();
            splitContainer1.Refresh();
        }
        //需要在show之前赋值
        public IMap HistoryMap
        {
            set
            {
                ucHistoryMap1.AxMapCtrlHis.Map = value;

            }
        }

        private void setAxMapControl()
        {
            axTOCControl1.SetBuddyControl(MapMain);
            this.ucHistoryPoint1.AxMapCtrlHis = MapMain;
            //this.ucHistorySegment1.AxMapCtrlHis = MapMain;
            m_pMainXucHis.devPanelControl1.BorderStyle = BorderStyle.Fixed3D;
            //m_pMainXucHis.devPanelControl1.BackColor = Color.Blue;
            //m_pMainXucHis.devPanelControl
            if (!ucHistoryMap1.Equals(m_pMainXucHis))
            {
                ucHistoryMap1.devPanelControl1.BorderStyle = BorderStyle.None;
            }
            if (!ucHistoryMap2.Equals(m_pMainXucHis))
            {
                ucHistoryMap2.devPanelControl1.BorderStyle = BorderStyle.None;
            }
            Application.DoEvents();
            //MapMain.BorderStyle = esriControlsBorderStyle.esriBorder;
            //MapMain.Appearance = esriControlsAppearance.esri3D;

        }
        private void ucHistoryMap1_AxMapCtrlHis_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            try
            {
                ucHistoryMap2.AxMapCtrlHis.Extent = e.newEnvelope as IEnvelope;
            }
            catch
            { }
        }
        private void ucHistoryMap2_AxMapCtrlHis_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            try
            {
                ucHistoryMap1.AxMapCtrlHis.Extent = e.newEnvelope as IEnvelope;
            }
            catch
            { }
        }
        private void ucHistoryMap1_Load(object sender, EventArgs e)
        {

        }

        private void tsbDefault_Click(object sender, EventArgs e)
        {
            ITool pTool = new ControlsSelectToolClass();
            ICommand pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            ucHistoryMap1.AxMapCtrlHis.CurrentTool = pTool;
            pTool = new ControlsSelectToolClass();
            pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap2.AxMapCtrlHis.Object);
            ucHistoryMap2.AxMapCtrlHis.CurrentTool = pTool;
        }

        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            ITool pTool = new ControlsMapZoomInToolClass();
            ICommand pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            ucHistoryMap1.AxMapCtrlHis.CurrentTool = pTool;
            pTool = new ControlsMapZoomInToolClass();
            pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap2.AxMapCtrlHis.Object);
            ucHistoryMap2.AxMapCtrlHis.CurrentTool = pTool;
        }

        private void tsbZoomOut_Click(object sender, EventArgs e)
        {
            ITool pTool = new ControlsMapZoomOutToolClass();
            ICommand pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            ucHistoryMap1.AxMapCtrlHis.CurrentTool = pTool;
            pTool = new ControlsMapZoomOutToolClass();
            pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap2.AxMapCtrlHis.Object);
            ucHistoryMap2.AxMapCtrlHis.CurrentTool = pTool;
        }

        private void tsbPan_Click(object sender, EventArgs e)
        {
            ITool pTool = new ControlsMapPanToolClass();
            ICommand pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            ucHistoryMap1.AxMapCtrlHis.CurrentTool = pTool;
            pTool = new ControlsMapPanToolClass();
            pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap2.AxMapCtrlHis.Object);
            ucHistoryMap2.AxMapCtrlHis.CurrentTool = pTool;
        }

        private void tsbLastView_Click(object sender, EventArgs e)
        {
            ICommand pCmd = new ControlsMapZoomToLastExtentBackCommandClass();
            pCmd.OnCreate(MapMain.Object);
            pCmd.OnClick();
        }

        private void tsbNextView_Click(object sender, EventArgs e)
        {
            ICommand pCmd = new ControlsMapZoomToLastExtentForwardCommandClass();
            pCmd.OnCreate(MapMain.Object);
            pCmd.OnClick();
        }

        private void tsbFullExtent_Click(object sender, EventArgs e)
        {
            ICommand pCmd = new ControlsMapFullExtentCommandClass();
            pCmd.OnCreate(MapMain.Object);
            pCmd.OnClick();
        }

        private void tsbIdentity_Click(object sender, EventArgs e)
        {
            ITool pTool = new GeoUtilities.ControlsMapIdentify(this);
            ICommand pCmd = pTool as ICommand;
            pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            MapMain.CurrentTool = pTool;

            //ITool pTool = new ControlsMapIdentifyToolClass();
            //ICommand pCmd = pTool as ICommand;
            //pCmd.OnCreate(ucHistoryMap1.AxMapCtrlHis.Object);
            //ucHistoryMap1.AxMapCtrlHis.CurrentTool = pTool;

            //pTool = new ControlsMapIdentifyToolClass();
            //pCmd = pTool as ICommand;
            //pCmd.OnCreate(ucHistoryMap2.AxMapCtrlHis.Object);
            //ucHistoryMap2.AxMapCtrlHis.CurrentTool = pTool;

        }

        private void ucHistoryMap1_Enter(object sender, EventArgs e)
        {
            MapMain = ucHistoryMap1.AxMapCtrlHis;
            m_pMainXucHis = ucHistoryMap1;
            setAxMapControl();
        }

        private void ucHistoryMap2_Enter(object sender, EventArgs e)
        {
            MapMain = ucHistoryMap2.AxMapCtrlHis;
            m_pMainXucHis = ucHistoryMap2;
            setAxMapControl();
        }

        private void xTabHis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xTabHis.SelectedIndex == 0 || xTabHis.SelectedIndex==1)
            {
                (MapMain.Map as IGraphicsContainer).DeleteAllElements();
                MapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
        }

        private void FrmHistoryMapView_Load(object sender, EventArgs e)
        {
            Rectangle structRec = Screen.PrimaryScreen.WorkingArea;
            //structRec.Height=structRec.Height*3/5;
            this.Bounds = structRec;//全客户区显示
            splitContainer1.SplitterDistance = 260;
            splitContainer2.SplitterDistance = splitContainer2.Location.X + splitContainer2.Width / 2 - 2;

            this.Refresh();
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            //frmHisExport fmHE = new frmHisExport(MapMain.Map, MapMain.Extent);
            //fmHE.ShowDialog();
        }

        private void toolAddLayer_Click(object sender, EventArgs e)
        {
            //FrmAddLayer pFrm = new FrmAddLayer(_ProjectTree);
            //DialogResult pRes= pFrm.ShowDialog();
            //if (pRes == DialogResult.OK)
            //{
            //    ucHistoryMap1.AxMapCtrlHis.Map.ClearLayers();
            //    ucHistoryMap2.AxMapCtrlHis.Map.ClearLayers();
            //    IFeatureClass pOldClass = pFrm.OldFeatureClass;
            //    IFeatureClass pNewClass = pFrm.NewFeatureClass;
            //    if (pOldClass == null || pNewClass == null)
            //    {
            //        return;
            //    }
            //    IFeatureLayer pOldLayer = new FeatureLayerClass();
            //    pOldLayer.FeatureClass = pOldClass;
            //    pOldLayer.Name = pOldClass.AliasName;
            //    ucHistoryMap1.AxMapCtrlHis.Map.AddLayer(pOldLayer as ILayer);
            //    IFeatureLayer pNewLayer = new FeatureLayerClass();
            //    pNewLayer.FeatureClass = pNewClass;
            //    pNewLayer.Name = pNewClass.AliasName;
            //    ucHistoryMap2.AxMapCtrlHis.Map.AddLayer(pNewLayer as ILayer);
            //    _OldFeatureClass = pOldClass;
            //    _NewFeatureClass = pNewClass;
            //    IGeoDataset pGeodataset = pOldClass as IGeoDataset;
            //    ucHistoryMap1.AxMapCtrlHis.Extent = pGeodataset.Extent;
            //    ucHistoryMap2.AxMapCtrlHis.Extent = pGeodataset.Extent;
            //    ucHistoryMap1.AxMapCtrlHis.OnExtentUpdated += new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(ucHistoryMap1_AxMapCtrlHis_OnExtentUpdated);
            //    ucHistoryMap2.AxMapCtrlHis.OnExtentUpdated += new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(ucHistoryMap2_AxMapCtrlHis_OnExtentUpdated);
            //    MapMain = ucHistoryMap1.AxMapCtrlHis;                
                
            //    m_pMainXucHis = ucHistoryMap1;
            //    setAxMapControl();
            //    splitContainer1.Refresh();
            //}
        }

        private void toolAnalysisChange_Click(object sender, EventArgs e)
        {
            if (_OldFeatureClass == null || _NewFeatureClass == null)
            {
                MessageBox.Show("请先添加新旧数据！","提示",MessageBoxButtons.OKCancel ,MessageBoxIcon.Error );
                return;
            }//ygc 20130316 新增错误保护!

            IEnvelope pEnv = ucHistoryMap1.AxMapCtrlHis.Extent;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            _Progress = vProgress;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("创建临时结果文件");

            bool bRes=CreateMdbDatabase();
            if (!bRes)
            {
                vProgress.Close();
                vProgress = null;
                return;
            }
            bool bRes2=DoIntersect(_OldFeatureClass, _NewFeatureClass, pEnv as IGeometry);
            if (!bRes2)
            {
                vProgress.Close();
                vProgress = null;
                return;
            }
            DoAttriAnalysis();
            if (_Progress != null)
            {
                _Progress.Close();
                _Progress = null;
            }
            DeleteMdb();
        }
        private void DeleteMdb()
        {
            try
            {
                File.Delete(_MdbPath + "\\" + _MdbName);
            }
            catch
            { }
        }
        private bool CreateMdbDatabase()
        {
            IWorkspace pWks = ModDBOperator.CreatePDBWorkSpace(_MdbPath, _MdbName);
            if (pWks != null)
            {
                return true;
            }
            return false;
        }
        //进行叠置
        private bool DoIntersect(IFeatureClass pOldFeaCls, IFeatureClass pNewFeaCls, IGeometry pGeo)
        {
            _Progress.SetProgress("新旧图层叠置...");
            Application.DoEvents();
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            gp.AddOutputsToMap = false;
            IGpValueTableObject vtobject = new GpValueTableObjectClass();
            vtobject.SetColumns(1);

            IFeatureLayer pFeaturelayer = new FeatureLayerClass();
            pFeaturelayer.FeatureClass = pOldFeaCls;
            pFeaturelayer.Name = pOldFeaCls.AliasName;

            if (pGeo != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeo;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureSelection pFeaSelection = pFeaturelayer as IFeatureSelection;   //仅对选中的重点巡查线进行缓冲分析
                pFeaSelection.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
            }
            if (_Progress != null)
            {
                if (_Progress.UserAskCancel)
                {
                    return false;
                }
            }
            object pObj1 = pFeaturelayer;
            vtobject.AddRow(ref pObj1);

            IFeatureLayer pNewFeaturelayer = new FeatureLayerClass();
            pNewFeaturelayer.FeatureClass = pNewFeaCls;
            pNewFeaturelayer.Name = pNewFeaCls.AliasName;

            if (pGeo != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeo;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureSelection pFeaSelection = pNewFeaturelayer as IFeatureSelection;   //仅对选中的重点巡查线进行缓冲分析
                pFeaSelection.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
            }
            if (_Progress != null)
            {
                if (_Progress.UserAskCancel)
                {
                    return false;
                }
            }
            object pObj2 = pNewFeaturelayer;
            vtobject.AddRow(ref pObj2);

            ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            intersect.in_features = vtobject;
            intersect.join_attributes = "ALL";
            intersect.output_type = "INPUT";

            intersect.out_feature_class = _MdbPath+"\\"+_MdbName+"\\"+_IntersectName;// _ResultPath + _TmpMdbName + "\\" + _IntersectName;
            if (_Progress  != null)
            {
                if (_Progress.UserAskCancel)
                {
                    return false;
                }
            }
            if (!RunTool(gp, intersect))
            {
                gp = null;
                return false;
            }

            intersect.in_features = null;
            vtobject.RemoveRow(1);
            vtobject.RemoveRow(0);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(vtobject);
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(intersect);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(gp);
            }
            catch (Exception err)
            { }

            vtobject = null;
            intersect = null;
            gp = null;

            return true;
        }
        private void DoAttriAnalysis()
        {
            _Progress.SetProgress("分析叠置结果...");
            Application.DoEvents();
            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            ISpatialReference pSpaRef = pSpaFac.CreateESRISpatialReferenceFromPRJFile(_PrjFileName);

            IWorkspaceFactory pWSFact = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkspace = null;
            try
            {
                pWorkspace = pWSFact.OpenFromFile(_MdbPath + "\\" + _MdbName, 0);
            }
            catch
            { }
            if (pWorkspace == null)
            {
                return;
            }
            pSpaFac = null;
            pWSFact = null;
            IFeatureClass pInterFeaClass = (pWorkspace as IFeatureWorkspace).OpenFeatureClass(_IntersectName);

            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = "jsmjtmp";
            pFieldEdit.AliasName_2 = "计算面积";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            if (_Progress != null)
            {
                if (_Progress.UserAskCancel)
                {
                    return;
                }
            }
            pInterFeaClass.AddField(pField);
            pField = null;
            int indexJSMJ = -1;
            indexJSMJ = pInterFeaClass.Fields.FindField("jsmjtmp");
            _Progress.SetProgress("计算叠置结果的面积...");
            Application.DoEvents();
            IQueryFilter pFilter = new QueryFilterClass();
            pFilter.WhereClause = "dl<>dl_1 or lz<>lz_1";
            try
            {
                IFeatureCursor pCursor = pInterFeaClass.Search(pFilter, false);
                IFeature pFea = pCursor.NextFeature();
                while (pFea != null)
                {
                    if (_Progress != null)
                    {
                        if (_Progress.UserAskCancel)
                        {
                            return;
                        }
                    }

                    IClone pClone = (IClone)pFea.Shape;
                    IGeometry pGeo = pClone.Clone() as IGeometry;
                    pGeo.Project(pSpaRef);
                    IArea pArea = pGeo as IArea;
                    if (pArea != null)
                    {
                        pFea.set_Value(indexJSMJ, pArea.Area);
                        pFea.Store();
                    }
                    pFea = pCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }
            catch
            { }
            _Progress.SetProgress("对叠置结果进行分析...");
            Application.DoEvents();
            if (_Progress != null)
            {
                if (_Progress.UserAskCancel)
                {
                    return;
                }
            }
            //从叠置结果生成报表
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _MdbPath + "\\" + _MdbName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();

            OleDbCommand pCommand = oledbconn.CreateCommand();
            ModTableFun.DropTable(oledbconn, "tmpBGJL");
            pCommand.CommandText = "select shi,xian,xiang,cun,xbh,xbh_1,dl,dl_1,lz,lz_1,jsmjtmp into tmpBGJL from "+_IntersectName +" where dl<>dl_1 or lz<>lz_1";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "delete from tmpBGJL where jsmjtmp<0.01";
            pCommand.ExecuteNonQuery();
            OleDbDataReader pReader = ModTableFun.GetReader(oledbconn, "select shi,xian,xiang,cun,xbh,dl_1,lz_1,dl,lz,round(jsmjtmp,2) from tmpBGJL");
            FrmAnalysisResult pFrm = new FrmAnalysisResult();
            DevComponents.DotNetBar.Controls.DataGridViewX pGrid = pFrm.ResultGrid;
            int intIndex = pGrid.Rows.Count;
            _Progress.SetProgress("准备展示分析结果...");
            Application.DoEvents();
            if (pReader == null)
            {
                return;
            }
            while (pReader.Read())
            {
                if (_Progress != null)
                {
                    if (_Progress.UserAskCancel)
                    {
                        oledbconn.Close();
                        return;
                    }
                }
                try
                {

                    pGrid.Rows.Add(pReader.GetValue(0).ToString(), pReader.GetValue(1).ToString(), pReader.GetValue(2).ToString(), pReader.GetValue(3).ToString(), pReader.GetValue(4).ToString(), pReader.GetValue(5).ToString(), pReader.GetValue(6).ToString(), pReader.GetValue(7).ToString(), pReader.GetValue(8).ToString(), pReader.GetValue(9).ToString());

                    intIndex++;
                }
                catch
                { }
            }
            pReader.Close();
            oledbconn.Close();
            oledbconn = null;
            _Progress.Close();
            _Progress = null;
            pFrm.Show();

        }
        //判断gp运行
        private bool RunTool(Geoprocessor geoprocessor, IGPProcess process)
        {
            // Set the overwrite output option to true
            geoprocessor.OverwriteOutput = true;

            try
            {
                geoprocessor.Execute(process, null);
                return true;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }
    }
}
