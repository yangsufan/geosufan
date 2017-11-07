using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;

namespace GeoStatistics
{
    public class CommandImportAreaStatistics : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private frmAreaStatistics frm = null;
        private IElement mElement = null;
        private IMapControlDefault m_MapControl = null;
        private IActiveView m_ActiveView = null;
        private IMap m_Map = null;
        private IGeometry m_Geometry = null;
        private IEnvelope m_Envelope = null; 
        public CommandImportAreaStatistics()
        {
            base._Name = "GeoStatistics.CommandImportAreaStatistics";
            base._Caption = "导入范围统计";
            base._Tooltip = "导入范围统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "导入范围统计";
        }


        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (_AppHk != null)
                {
                    if (_AppHk.MapControl != null)
                    {
                        if (_AppHk.MapControl.Map.LayerCount > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl.Map == null)
                return;
             m_MapControl = _AppHk.MapControl;
             m_Map = m_MapControl.Map;
             m_ActiveView = m_MapControl.ActiveView;
             if (m_Map.LayerCount == 0)
                return;
             try
             {
                 if (this.WriteLog)
                 {
                     Plugin.LogTable.Writelog("执行导入范围统计");
                 }
                 /// 判断数据字典是否初始化
                 if (SysCommon.ModField._DicFieldName.Count == 0)
                 {
                     SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
                 }
                 ///存储导入shp文件的所有面要素
                 List<IFeature> pLst = new List<IFeature>();

                 OpenFileDialog openFileDialog1 = new OpenFileDialog();
                 openFileDialog1.Filter = "shape文件(*.shp)|*.shp";
                 //openFileDialog1.InitialDirectory = @"E:\test\文档和数据\Data";
                 openFileDialog1.Multiselect = false;
                 DialogResult pDialogResult = openFileDialog1.ShowDialog();
                 if (pDialogResult != DialogResult.OK)
                     return;

                 IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();  //  1
                 string pPath = openFileDialog1.FileName;
                 string pFolder = System.IO.Path.GetDirectoryName(pPath);//文件夹
                 string pFileName = System.IO.Path.GetFileName(pPath);//文件名
                 IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);  // 2
                 IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                 IFeatureClass pFC = null;
                 try
                 {
                     pFC = pFeatureWorkspace.OpenFeatureClass(pFileName);  //3
                 }
                 catch
                 {
                     MessageBox.Show("请导入有效范围数据！", "提示！");
                     if (this.WriteLog)
                     {
                         Plugin.LogTable.Writelog("导入范围数据无效");
                     }
                     return;
                 }
                 if (pFC == null)
                 {
                     MessageBox.Show("请导入有效范围数据！", "提示！");
                     if (this.WriteLog)
                     {
                         Plugin.LogTable.Writelog("导入范围数据无效");
                     }
                     return;
                 }
                 IFeatureClass pPolygonFeaCls = GetPolygoneFeatureClass(pFC);
                 if (pPolygonFeaCls == null)
                 {
                     MessageBox.Show("请导入有效范围数据！", "提示！");
                     if (this.WriteLog)
                     {
                         Plugin.LogTable.Writelog("导入范围数据无效");
                     }
                     return;
                 }
                 //获得整个图层的游标
                 IFeatureCursor pFeatureCursor = GetFeatureCursor(pPolygonFeaCls, null, null, esriSpatialRelEnum.esriSpatialRelUndefined);
                 IFeature pFeature = pFeatureCursor.NextFeature();
                 while (pFeature != null)
                 {
                     pLst.Add(pFeature);
                     pFeature = pFeatureCursor.NextFeature();
                 }
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                 pFeatureCursor = null;
                 m_Geometry = GetLyrUnionPlygon(pLst);

                 if (m_Geometry == null) return;
                 //DrawGeometry(m_Geometry);
                 //m_Envelope = GetLyrUnionEnvelope(pLst);
                 m_Envelope = m_Geometry.Envelope;
                 m_Envelope.Expand(2.0, 2.0, true);
                 m_MapControl.Extent = m_Envelope;
                 //(m_MapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, mElement, m_Envelope);
                 m_MapControl.ActiveView.ScreenDisplay.UpdateWindow();
                 m_MapControl.FlashShape(m_Geometry as IGeometry, 3, 200, null);
                 frm = new frmAreaStatistics();
                 frm.CurGeometry = m_Geometry;
                 frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                 frm.CurMap = m_MapControl;
                 frm.InitFrm();
                 frm.SetSliderValue(true);
                 frm.ShowDialog();
                 if (this.WriteLog)
                 {
                     Plugin.LogTable.Writelog("导入范围统计操作完成");
                 }
             }
             catch(Exception err)
             {
                 if (this.WriteLog)
                 {
                     Plugin.LogTable.Writelog("导入范围统计操作异常");
                 }
             }
        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {

            //IGraphicsContainer pMapGraphics = (IGraphicsContainer)m_Map;
            //m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, mElement, m_Envelope);
            //if (mElement != null) pMapGraphics.DeleteElement(mElement);
            //m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, mElement, m_Envelope);
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;

            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
        /// <summary>
        /// 根据属性条件和空间条件获得查询结果游标
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <param name="pWhereClause"></param>
        /// <param name="pGeometry"></param>
        /// <param name="pesriSpatialRelEnum"></param>
        /// <returns></returns>
        private IFeatureCursor GetFeatureCursor(IFeatureClass pFeatureClass, string pWhereClause, IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            IFeatureCursor pFeatureCursor = null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            try
            {
                if (pGeometry != null)
                {
                    pSpatialFilter.Geometry = pGeometry;
                    pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
                    pSpatialFilter.SpatialRel = pesriSpatialRelEnum;
                }
                pSpatialFilter.WhereClause = pWhereClause;
                pFeatureCursor = pFeatureClass.Search(pSpatialFilter, false);

                return pFeatureCursor;
            }
            catch
            {
                return pFeatureCursor;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }

        }
        /// <summary>
        /// 获得指定图层的合并范围 为本次加的一个函数
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IEnvelope  GetLyrUnionEnvelope(IList<IFeature> vFeaList)
        {
            if (vFeaList.Count < 1) return null;
            //构造
            IEnvelope pEnv = null;
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            for (int i = 0; i < vFeaList.Count; i++)
            {
                if (vFeaList[i].Shape != null && !vFeaList[i].Shape.IsEmpty)
                {
                    if (pEnv == null)
                    {
                        pEnv = new EnvelopeClass();
                        pEnv.XMin = vFeaList[i].ShapeCopy.Envelope.XMin;
                        pEnv.XMax = vFeaList[i].ShapeCopy.Envelope.XMax;
                        pEnv.YMin = vFeaList[i].ShapeCopy.Envelope.YMin;
                        pEnv.YMax = vFeaList[i].ShapeCopy.Envelope.YMax;
                    }
                    else
                    {
                        if (pEnv.XMin > vFeaList[i].ShapeCopy.Envelope.XMin)
                        {
                            pEnv.XMin = vFeaList[i].ShapeCopy.Envelope.XMin;
                        }
                        if (pEnv.XMax < vFeaList[i].ShapeCopy.Envelope.XMax)
                        {
                            pEnv.XMax = vFeaList[i].ShapeCopy.Envelope.XMax;
                        }
                        if (pEnv.YMin > vFeaList[i].ShapeCopy.Envelope.YMin)
                        {
                            pEnv.YMin = vFeaList[i].ShapeCopy.Envelope.YMin;
                        }
                        if (pEnv.YMax < vFeaList[i].ShapeCopy.Envelope.YMax)
                        {
                            pEnv.YMax = vFeaList[i].ShapeCopy.Envelope.YMax;
                        }
 
                    }
                }
            }
            //构造合并
            return pEnv;
        }
        /// <summary>
        /// 获得指定图层的合并范围 为本次加的一个函数
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IGeometry GetLyrUnionPlygon(IList<IFeature> vFeaList)
        {

            if (vFeaList.Count < 1) return null;
            //构造
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            for (int i = 0; i < vFeaList.Count; i++)
            {
                if (vFeaList[i].Shape != null && !vFeaList[i].Shape.IsEmpty) pGeometryCol.AddGeometry(vFeaList[i].ShapeCopy, ref obj, ref obj);
            }
            //构造合并
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeometryCol as IEnumGeometry);
            IGeometry pGeometry = pTopo as IGeometry;
            return pGeometry;
        }

        public void DrawGeometry(IGeometry m_pGeometry)
        {

            //画element
            //先删除
            if (m_Map == null) return;
            IGraphicsContainer pMapGraphics = (IGraphicsContainer)m_Map;
            mElement = SysCommon.Gis.ModGisPub.DoDrawGeometry(_AppHk.MapControl, m_pGeometry, 255, 170, 0, true);
            m_ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
        }

        //cast the polyline object to the polygon xisheng 20110926 
        private IPolygon GetPolygonFormLine(IPolyline pPolyline)
        {
            ISegmentCollection pRing;
            IGeometryCollection pPolygon = new PolygonClass();
            IGeometryCollection pPolylineC = pPolyline as IGeometryCollection;
            object o = Type.Missing;
            for (int i = 0; i < pPolylineC.GeometryCount; i++)
            {
                pRing = new RingClass();
                pRing.AddSegmentCollection(pPolylineC.get_Geometry(i) as ISegmentCollection);
                pPolygon.AddGeometry(pRing as IGeometry, ref o, ref o);
            }
            IPolygon polygon = pPolygon as IPolygon;
            return polygon;
        }
        private IFeatureClass GetPolygoneFeatureClass(IFeatureClass pFeaCls)
        {
            //判断文件类型
            if (pFeaCls.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                return pFeaCls;
            }
            else if (pFeaCls.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
            {
                IDataset pDataset = pFeaCls as IDataset;
                IFeatureWorkspace pWorkSpace = pDataset.Workspace as IFeatureWorkspace;
                ////获取Geometry字段
                //String shapeFieldName = pFeaCls.ShapeFieldName;
                //IFields fields = pFeaCls.Fields;
                //int geometryIndex = fields.FindField(shapeFieldName);
                //IField pGeometryfield = fields.get_Field(geometryIndex);
                ////获取IGeometryDef


                //获取空间参数
                IGeoDataset pGeodataset = pFeaCls as IGeoDataset;
                ISpatialReference pSpatialRef = pGeodataset.SpatialReference;

                //构造属性
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;
                //构造Geometry属性列
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = (IFieldEdit)pField;

                pFieldEdit.Name_2 = "SHAPE";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit pGeoDefEdit = (IGeometryDefEdit)geometryDef;
                pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                pGeoDefEdit.SpatialReference_2 = pSpatialRef;
                pFieldEdit.GeometryDef_2 = geometryDef;
                pFieldsEdit.AddField(pField);

                IFeatureClass pPolygoneFeatureClass = pWorkSpace.CreateFeatureClass("TmpPolygone" + System.DateTime.Now.ToString("YYYYMMDDHHmmss"), pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                //开始编辑
                IWorkspaceEdit pTargetFWorkspaceEdit = (pPolygoneFeatureClass as IDataset).Workspace as IWorkspaceEdit;
                if (!pTargetFWorkspaceEdit.IsBeingEdited())
                {
                    pTargetFWorkspaceEdit.StartEditing(true);
                    pTargetFWorkspaceEdit.StartEditOperation();
                }


                IFeatureCursor pCursor = pFeaCls.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();
                while (pFeature != null)
                {

                    IPolyline pPolyline = pFeature.Shape as IPolyline;
                    if (pPolyline != null)
                    {
                        if (pPolyline.IsClosed == true)
                        {
                            IPolygon pGon = GetPolygonFormLine(pPolyline);
                            if (pGon != null)
                            {
                                IFeature pNewFea = pPolygoneFeatureClass.CreateFeature();
                                pNewFea.Shape = pGon as IGeometry;
                                pNewFea.Store();
                            }

                        }
                    }
                    pFeature = pCursor.NextFeature();
                }
                pTargetFWorkspaceEdit.StopEditOperation();
                pTargetFWorkspaceEdit.StopEditing(true);
                return pPolygoneFeatureClass;
            }
            else//既不是线文件也不是面文件，直接退出
            {
                return null;
            }
        }
    }
}
