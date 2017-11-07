using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF.BaseClasses;
using SysCommon.Error;

namespace GeoDataEdit
{
    public class MoveFeature : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;
        private ITool _tool = null;
        private ICommand _cmd = null;
       

        public MoveFeature()
        {
            base._Name = "GeoDataEdit.MoveFeature";
            base._Caption = "移动图元";
            base._Tooltip = "移动图元";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "移动图元";
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

            _cmd.OnClick();

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

            Plugin.LogTable.Writelog(Caption);


        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _cmd = new ControlsMoveFeature(pAppForm.MainForm);
            //_cmd = new ESRI.ArcGIS.Controls.ControlsMapIdentifyTool();
            _tool = _cmd as ITool;

            _cmd.OnCreate(_AppHk.MapControl);
        }
        private IFeatureLayer layerCurSeleted()
        {

            int iLayerCount = _AppHk.MapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer;
            IFeatureLayer pCurSeLayer = null;
            IMap pMap = _AppHk.MapControl.Map;
            int countSelectable = 0;
            for (int n = 0; n < iLayerCount; n++)
            {
                if (countSelectable >= 2) break;
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer != null && pFeatureLayer.Selectable)
                        {
                            countSelectable++;
                            pCurSeLayer = pFeatureLayer;

                        }

                    }
                }
            }//for
            if (countSelectable != 1)
                return null;
            else
                return pCurSeLayer;




        }




    }

    public class ControlsMoveFeature : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private frmAddPoint frmAddPt = null;
        private IFeatureLayer pFeatureLayer = null;
        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;

        //类的方法
        public ControlsMoveFeature(Form mainFrm)
        {
           
            base.m_category = "GeoCommon";
            base.m_caption = "ControlsMoveFeature";
            base.m_message = "";
            base.m_toolTip = "";
            base.m_name = base.m_category + "_" + base.m_caption;

            try
            {
                
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                System.IO.Stream strm = GetType().Assembly.GetManifestResourceStream("GeoDataEdit.Command.CurMoveFeature.cur");
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
          
            
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {

            IFeatureSelection curLayerSn = getEditLayer.isExistLayer(m_MapControl.Map) as IFeatureSelection;
            if (curLayerSn == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请先设置编辑图层。");
                return;
 
            }
            ISelectionSet pSS = curLayerSn.SelectionSet;
            if (pSS.Count != 1)
            {
                m_MapControl.CurrentTool = null;
                ErrorHandle.ShowFrmErrorHandle("提示", "请在编辑图层上选择要移动的要素。");
                return;
            }
            

            
        }

   
        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
          
            //ESRI.ArcGIS.Geometry.IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            //int iPiexl = 3;
            //double iMapTolerance = ConvertPixelsToMapUnits(m_MapControl.ActiveView, iPiexl);

            //IGeometry pGeometry = null;
            //ITopologicalOperator pTopo = (ITopologicalOperator)pPoint;
            //if (pTopo != null) pGeometry = pTopo.Buffer(iMapTolerance);
            //if (pGeometry == null)
            //    return;
            //ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            //pSpatialFilter.Geometry = pGeometry;
            //pSpatialFilter.GeometryField = "SHAPE";
            //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //IFeatureLayer pFL = getEditLayer.isExistLayer(m_MapControl.Map) as IFeatureLayer;
            //IFeatureSelection pFS = pFL as IFeatureSelection;
            //pFS.Clear();
            //m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            //pFS.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, true);
            //m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
          

        }


        

        private IFeatureLayer layerCurSeleted()
        {
            IMap pMap = ( m_hookHelper.Hook as IMapControl3).Map;
            int iLayerCount = pMap.LayerCount;
            IFeatureLayer pFeatureLayer;
            IFeatureLayer pCurSeLayer = null;
          
            int countSelectable = 0;
            for (int n = 0; n < iLayerCount; n++)
            {
                if (countSelectable >= 2) break;
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer != null && pFeatureLayer.Selectable)
                        {
                            countSelectable++;
                            pCurSeLayer = pFeatureLayer;

                        }

                    }
                }
            }//for
            if (countSelectable != 1)
                return null;
            else
                return pCurSeLayer;




        }





        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {

            //OnClick();

            if (Button != 1) return;
            moveFeature(X, Y);
            
        }
        private void moveFeature(int x,int y)
        {
            IFeatureLayer curLayer = getEditLayer.isExistLayer(m_MapControl.Map) as IFeatureLayer;
            IWorkspaceEdit iWE = (curLayer.FeatureClass as IDataset).Workspace as IWorkspaceEdit;
            if (!iWE.IsBeingEdited())
                iWE.StartEditing(false);

            IFeatureSelection curLayerSn = curLayer as IFeatureSelection;
            ISelectionSet pSS = curLayerSn.SelectionSet;
            if (pSS.Count != 1) return;
            ISelectionSet iSS = (curLayer as IFeatureSelection).SelectionSet;
            ICursor iCursor;
            iSS.Search(null, false, out iCursor);
            IFeatureCursor iFC = iCursor as IFeatureCursor;
            IFeature tmpFeature = iFC.NextFeature();
            IPoint pPnt = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            IGeometry pGeometry = tmpFeature.ShapeCopy;
            if (pGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                pGeometry = pPnt;
            else if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline || pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                double offX = 0, offY = 0;
                offX = pGeometry.Envelope.XMin + pGeometry.Envelope.Width / 2 - pPnt.X;
                offY = pGeometry.Envelope.YMin + pGeometry.Envelope.Height / 2 - pPnt.Y;
                ITransform2D iT2D = pGeometry as ITransform2D;
                iT2D.Move(-offX, -offY);
            }
            iWE.StartEditOperation();
            tmpFeature.Shape = pGeometry;
            tmpFeature.Store();
            iWE.StopEditOperation();
            //iWE.StopEditing(true);
            iWE = null;
            curLayerSn.Clear();
            curLayerSn.Add(tmpFeature);
            m_MapControl.ActiveView.Refresh();
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
