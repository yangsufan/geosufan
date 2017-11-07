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

namespace GeoDataEdit
{
    public class MovePoint : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;
        private ITool _tool = null;
        private ICommand _cmd = null;
       

        public MovePoint()
        {
            base._Name = "GeoDataEdit.MovePoint";
            base._Caption = "移动点";
            base._Tooltip = "移动点";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "移动点";
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
            IFeatureLayer tmpFeatureLayer=layerCurSeleted();
            if (tmpFeatureLayer == null||tmpFeatureLayer.FeatureClass.ShapeType!=esriGeometryType.esriGeometryPoint)
            {
                MessageBox.Show("请在地图目录设置当前选择图层为点图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            //_cmd.OnClick();

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
            _cmd = new ControlsMapMovePoint(pAppForm.MainForm);
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

    public class ControlsMapMovePoint : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private frmAddPoint frmAddPt = null;
        private IFeatureLayer pFeatureLayer = null;
        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;

        //类的方法
        public ControlsMapMovePoint(Form mainFrm)
        {
           
            base.m_category = "GeoCommon";
            base.m_caption = "MapAddPoint";
            base.m_message = "添加点";
            base.m_toolTip = "添加点";
            base.m_name = base.m_category + "_" + base.m_caption;
            //try
            //{
            //    base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.MapIdentify.cur");
            //}
            //catch
            //{

            //}

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


            //if (frmAddPt != null)
            //    frmAddPt.Close();
            //IFeatureLayer curLayer = layerCurSeleted();
            //IFeatureSelection curLayerSn = curLayer as IFeatureSelection;
            //ISelectionSet pSS = curLayerSn.SelectionSet;
            //if (pSS.Count != 1) return;
            //IEnumFeature pEnumFeature = m_MapControl.Map.FeatureSelection as IEnumFeature;
            //pEnumFeature.Reset();
            //IFeature tmpFeature = pEnumFeature.Next();
            //frmAddPt = new frmAddPoint(tmpFeature, m_MapControl.Map as IActiveView, curLayer);
            //frmAddPt.Show();
            //Application.DoEvents();

            

            
        }

   
        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
            


         
            //ESRI.ArcGIS.Geometry.IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            //IMarkerElement pMarkerEle = new MarkerElementClass();
            //pMarkerEle.Symbol = curLayerSn.SelectionSymbol as IMarkerSymbol;
            //m_MapControl.ActiveView.GraphicsContainer.AddElement(pMarkerEle as IElement,0);
            ////m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pMarkerEle, null);
            //m_MapControl.ActiveView.Refresh();

          
          

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
            
            IFeatureLayer curLayer = layerCurSeleted();
            IFeatureSelection curLayerSn = curLayer as IFeatureSelection;
            ISelectionSet pSS = curLayerSn.SelectionSet;
            if (pSS.Count!=1) return;
            IEnumFeature pEnumFeature = m_MapControl.Map.FeatureSelection as IEnumFeature;
            pEnumFeature.Reset();
            IFeature tmpFeature = pEnumFeature.Next();
            IPoint pp = new PointClass();
            pp = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            tmpFeature.Shape = pp;
            tmpFeature.Store();
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
