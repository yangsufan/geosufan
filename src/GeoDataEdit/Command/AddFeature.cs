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

namespace GeoDataEdit
{
    public class AddFeature : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;
        private ITool _tool = null;
        private ICommand _cmd = null;

        public AddFeature()
        {
            base._Name = "GeoDataEdit.AddFeature";
            base._Caption = "添加图元";
            base._Tooltip = "添加图元";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加图元";
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
            Plugin.LogTable.Writelog(Caption);
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _cmd = new ControlsMapAddFeature(pAppForm.MainForm);
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

    public class ControlsMapAddFeature : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private frmAddPoint frmAddPt = null;
        private IFeatureLayer pFeatureLayer = null;
        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;

        //类的方法
        public ControlsMapAddFeature(Form mainFrm)
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
            IFeatureLayer curLayer = getEditLayer.isExistLayer(m_MapControl.Map) as IFeatureLayer;
            if (curLayer == null)
                return;
            IFeatureSelection curLayerSn = curLayer as IFeatureSelection;

            IMapControl2 pMapCtl = m_hookHelper.Hook as IMapControl2;
            ESRI.ArcGIS.Geometry.IGeometry pGeometry=null;
            if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                pGeometry = pMapCtl.TrackPolygon();
                ITopologicalOperator iTO = pGeometry as ITopologicalOperator;
                iTO.Simplify();
            }
            else if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                pGeometry = pMapCtl.TrackLine();
            else if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                pGeometry = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            }
            if (pGeometry == null) return;
            



            IFeature pFeatureCreated = IFeatureClass_Create(curLayer.FeatureClass, pGeometry);
            m_MapControl.ActiveView.Refresh();
            if (pFeatureCreated == null) return;
            if (curLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
           {
            frmAddPt = new frmAddPoint(pFeatureCreated, m_MapControl.Map as IActiveView,curLayer);
            frmAddPt.Show();
           }
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            curLayerSn.Clear();
            pMapCtl.Map.ClearSelection();
            curLayerSn.Add(pFeatureCreated);//选择刚创建的point
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            //m_MapControl.Map.FeatureSelection.Clear();
            //m_MapControl.Map.FeatureSelection =curLayerSn as ISelection;

            //g = 3 + 7;
            //m_MapControl.ActiveView.Refresh();

        }

        //IFeatureClass CreateFeature Example
        private IFeature IFeatureClass_Create(IFeatureClass featureClass, IGeometry pGeometry)
        {
           
            IWorkspaceEdit iWE = (featureClass as IDataset).Workspace as IWorkspaceEdit;
            if (!iWE.IsBeingEdited())
                iWE.StartEditing(false);
            iWE.StartEditOperation();
            IFeature feature = featureClass.CreateFeature();
            //Apply the constructed shape to the new features shape    
            feature.Shape = pGeometry;
            //如果不是shp 则初始化子类型
            if ((featureClass as IDataset).Workspace.WorkspaceFactory.WorkspaceType != esriWorkspaceType.esriFileSystemWorkspace)
            {
                ISubtypes subtypes = (ISubtypes)featureClass;
                IRowSubtypes rowSubtypes = (IRowSubtypes)feature;
                if (subtypes.HasSubtype)// does the feature class have subtypes?       
                {
                    rowSubtypes.SubtypeCode = 1; //in this example 1 represents the Primary Pipeline subtype       
                }
                // initalize any default values that the feature has       
                rowSubtypes.InitDefaultValues();
                //Commit the default values in the feature to the database 
            }
            feature.Store();
            iWE.StopEditOperation();
            //iWE.StopEditing(true);
            iWE = null;
            return feature;

        }

        private IFeatureLayer layerCurSeleted()
        {
            IMap pMap = (m_hookHelper.Hook as IMapControl3).Map;
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
