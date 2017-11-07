using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF.BaseClasses;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using SysCommon.Error;

namespace GeoDataEdit
{
    public class SplitFeature : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;
        private ITool _tool = null;
        private ICommand _cmd = null;

        public SplitFeature()
        {
            base._Name = "GeoDataEdit.SplitFeature";
            base._Caption = "切割图元";
            base._Tooltip = "切割图元";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "切割图元";
            //base._Image = "";
            //base._Category = "";
        }
        public override bool Checked
        {
            get
            {
                if (_AppHk.CurrentTool != this.Name) return false;
                return true;
            }
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.Map.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
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
            if (_tool == null || _cmd == null || _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IFeatureLayer tmpFeatureLayer = getEditLayer.isExistLayer(_AppHk.MapControl.Map) as IFeatureLayer;
            if (tmpFeatureLayer == null)
            {
                MessageBox.Show("请设置编辑图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!(tmpFeatureLayer.FeatureClass.ShapeType==esriGeometryType.esriGeometryPolygon||tmpFeatureLayer.FeatureClass.ShapeType==esriGeometryType.esriGeometryPolyline))
            {
                MessageBox.Show("请设置编辑图层为线图层或面图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _tool = _cmd as ITool;
            if (_tool == null) return;

            if (_AppHk.CurrentControl is IMapControl2)
            {
                _AppHk.MapControl.CurrentTool = _tool;
            }
            else
            {
                _AppHk.PageLayoutControl.CurrentTool = _tool;
            }

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _cmd = new ControlsMapCutFeature(pAppForm.MainForm);
            //_cmd = new ESRI.ArcGIS.Controls.ControlsMapIdentifyTool();
            _tool = _cmd as ITool;

            _cmd.OnCreate(_AppHk.MapControl);
        }
       




    }

    public class ControlsMapCutFeature : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private frmAddPoint frmAddPt = null;
        private IFeatureLayer pFeatureLayer = null;
        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;

        //类的方法
        public ControlsMapCutFeature(Form mainFrm)
        {

            base.m_category = "GeoCommon";
            base.m_caption = "添加图元";
            base.m_message = "添加图元";
            base.m_toolTip = "添加图元";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                System.IO.Stream strm = GetType().Assembly.GetManifestResourceStream("GeoDataEdit.Command.CurAddFeature.cur");
                base.m_cursor = new System.Windows.Forms.Cursor(strm);//(GetType(), GetType().Name + ".cur");
                strm.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;

            //m_enumAttributeEditMode = enumAttributeEditMode.Top;
            //m_enumAttributeEditMode = enumAttributeEditMode.CurEdit; //changed by chulili 2011-04-15 默认当前编辑图层
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {





        }


        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;
            ESRI.ArcGIS.Geometry.IGeometry pGeometry = null;
            pGeometry = pMapCtl.TrackLine();//控件画线
            if (pGeometry == null)
                return;
            //简化拓扑
            ITopologicalOperator2 topoOperator = pGeometry as ITopologicalOperator2;
            topoOperator.IsKnownSimple_2 = false;
            topoOperator.Simplify();
            splitFeature(pGeometry); 
        }
        //分割要素
        private bool splitFeature(IGeometry pGeometry)
        {
            
                IFeatureLayer curLayer = getEditLayer.isExistLayer(m_MapControl.Map) as IFeatureLayer;
                if (curLayer == null)
                    return false;
                IFeatureSelection curLayerSn = curLayer as IFeatureSelection;
                bool hasCut = false;
               
                //空间查询,找出被切割要素
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = pGeometry;
                if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                    spatialFilter.SpatialRelDescription = "TTTFFTTTT";
                }
                else if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                }
                IFeatureClass featureClass = curLayer.FeatureClass;
                IWorkspaceEdit iWE = (featureClass as IDataset).Workspace as IWorkspaceEdit;
                if (!iWE.IsBeingEdited())
                    iWE.StartEditing(false);
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                try
                {
                IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
                IFeature origFeature = featureCursor.NextFeature();
                if (origFeature != null)
                {
                    //检查第一个要素是否Z敏感则将切割线设成Z敏感
                    IZAware zAware = origFeature.Shape as IZAware;
                    if (zAware.ZAware)
                    {
                        zAware = pGeometry as IZAware;
                        zAware.ZAware = true;
                    }

                    ArrayList comErrors = new ArrayList();

                    //开始操作
                    iWE.StartEditOperation();

                    //循环空间查询的要素，执行切割
                    while (origFeature != null)
                    {
                        try
                        {
                           
                            //切割要素，IFeatureEdit.Split此方法可自动处理属性
                            IFeatureEdit featureEdit = origFeature as IFeatureEdit;
                            ISet newFeaturesSet=null;
                            //保存切割生成的新要素的集合  
                            //若源要素是线，则切割图形是交点
                            if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                ITopologicalOperator iTo = pGeometry as ITopologicalOperator;
                                IGeometry pTmpG = iTo.Intersect(origFeature.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension);
                                IPointCollection pTmpPc = pTmpG as IPointCollection;
                                for (int i = pTmpPc.PointCount - 1; i >= 0; i--)
                                {
                                //newFeaturesSet = featureEdit.Split(pTmpG);
                                newFeaturesSet = featureEdit.Split(pTmpPc.get_Point(pTmpPc.PointCount - 1));

                                    //新要素已经生成
                                    if (newFeaturesSet != null)
                                    {
                                        newFeaturesSet.Reset();
                                        IFeature pSplitFeature = null;
                                        pSplitFeature = newFeaturesSet.Next() as IFeature;
                                        //IFeatureEdit tmpFE = pSplitFeature as IFeatureEdit;
                                        while (pSplitFeature != null)
                                        {
                                            //闪烁新要素
                                            m_MapControl.FlashShape(pSplitFeature.ShapeCopy, 1, 300, Type.Missing);
                                            pSplitFeature = newFeaturesSet.Next() as IFeature;
                                        }
                                        hasCut = true;
                                    }
                                  
                                }

                            }
                            //如果是面，切割图形是线
                            else
                            {
                                newFeaturesSet = featureEdit.Split(pGeometry);

                                //新要素已经生成
                                if (newFeaturesSet != null)
                                {
                                    newFeaturesSet.Reset();
                                    IFeature pSplitFeature = null;
                                    pSplitFeature = newFeaturesSet.Next() as IFeature;
                                    while (pSplitFeature != null)
                                    {
                                        //闪烁新要素
                                        m_MapControl.FlashShape(pSplitFeature.ShapeCopy, 1, 300, Type.Missing);
                                        pSplitFeature = newFeaturesSet.Next() as IFeature;
                                    }
                                    hasCut = true;
                                }
                            }
                        }
                        catch (COMException comExc)
                        {
                            comErrors.Add(String.Format("OID: {0}, 错误: {1} , {2}", origFeature.OID.ToString(), comExc.ErrorCode, comExc.Message));
                        }
                        finally
                        {
                            //当前的失败，则继续下一个
                            origFeature = featureCursor.NextFeature();
                        }
                    }
                    //如果有切割动作，则刷新视图并保存
                    if (hasCut)
                    {
                        //清除地图选择集
                        m_MapControl.Map.ClearSelection();

                        //刷新视图

                        m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, m_MapControl.ActiveView.Extent);

                        //完成编辑操作
                        iWE.StopEditOperation();
                    }
                    else
                    {
                        iWE.AbortEditOperation();
                    }

                    //报告错误
                    if (comErrors.Count > 0)
                    {
                        StringBuilder stringBuilder = new StringBuilder("以下要素不能被切割: \n", 200);
                        foreach (string comError in comErrors)
                        {
                            stringBuilder.AppendLine(comError);
                        }

                        MessageBox.Show(stringBuilder.ToString(), "切割错误");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "切割失败！\r\n" + ex.Message);
                iWE.AbortEditOperation();
                
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;

            }
                return hasCut;

        }


        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {

        }

        private double ConvertPixelsToMapUnits(IActiveView pActiveView, int pixelUnits)
        {
            tagRECT deviceRECT = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            int pixelExtent = deviceRECT.right - deviceRECT.left;
            double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {

        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }

}
