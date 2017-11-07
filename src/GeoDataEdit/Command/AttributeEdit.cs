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
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;



namespace GeoDataEdit
{
    public class AttributeEdit : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;


        public AttributeEdit()
        {
            base._Name = "GeoDataEdit.AttributeEdit";
            base._Caption = "属性编辑";
            base._Tooltip = "属性编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "属性编辑";
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
            if (getEditLayer.isExistLayer(_AppHk.MapControl.Map) == null) 
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
            _cmd = new ControlsMapAttrEdit(pAppForm.MainForm);
            //_cmd = new ESRI.ArcGIS.Controls.ControlsMapIdentifyTool();
            _tool = _cmd as ITool;

            _cmd.OnCreate(_AppHk.MapControl);
        }
        private IFeatureLayer layerCurSeleted()
        {

            int iLayerCount = _AppHk.MapControl.Map.LayerCount;
            IFeatureLayer pFeatureLayer;
            IFeatureLayer pCurSeLayer=null;
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
                            pCurSeLayer=pFeatureLayer;
 
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

    public class ControlsMapAttrEdit : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private Form m_mainFrm;
        private frmAttributeEdit m_frmAttributeEdit;
        private enumAttributeEditMode m_enumAttributeEditMode;

        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;

        //类的方法
        public ControlsMapAttrEdit(Form mainFrm)
        {
            m_mainFrm = mainFrm;
            base.m_category = "GeoCommon";
            base.m_caption = "MapAttrEdit";
            base.m_message = "属性编辑";
            base.m_toolTip = "属性编辑";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                System.IO.Stream strm = GetType().Assembly.GetManifestResourceStream("GeoDataEdit.Command.AttributeEdit.cur");
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
            m_enumAttributeEditMode = enumAttributeEditMode.CurEdit; //changed by chulili 2011-04-15 默认当前编辑图层
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_frmAttributeEdit == null)
            {
                m_frmAttributeEdit = new frmAttributeEdit(m_MapControl, m_enumAttributeEditMode);
                m_frmAttributeEdit.Owner = m_mainFrm;
                m_frmAttributeEdit.FormClosed += new FormClosedEventHandler(frmAttributeEdit_FormClosed);
            }

            m_frmAttributeEdit.Show();
            m_frmAttributeEdit.FillData(m_MapControl.ActiveView.FocusMap, null);
        }

        private void frmAttributeEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumAttributeEditMode = m_frmAttributeEdit.AttributeEditMode;
            m_frmAttributeEdit = null;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;

            ESRI.ArcGIS.Geometry.IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            if (m_pNewEnvelope == null)
            {
                pPoint.SpatialReference = m_MapControl.Map.SpatialReference;
                m_pNewEnvelope = new NewEnvelopeFeedbackClass();

                m_pNewEnvelope.Display = m_MapControl.ActiveView.ScreenDisplay;

                m_pNewEnvelope.Start(pPoint);
            }

            m_pPoint = pPoint;
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_pNewEnvelope == null) return;

            IEnvelope pEnvelope = m_pNewEnvelope.Stop();
            int iPiexl = 3;
            double iMapTolerance = ConvertPixelsToMapUnits(m_MapControl.ActiveView, iPiexl);

            IGeometry pGeometry = null;
            if (pEnvelope == null || pEnvelope.IsEmpty)
            {
                ITopologicalOperator pTopo = (ITopologicalOperator)m_pPoint;
                if (pTopo != null) pGeometry = pTopo.Buffer(iMapTolerance);
            }
            else
            {
                pEnvelope.SpatialReference = m_MapControl.ActiveView.FocusMap.SpatialReference;
                pGeometry = pEnvelope;
            }
            m_pNewEnvelope = null;

            if (m_frmAttributeEdit == null)
            {
                m_frmAttributeEdit = new frmAttributeEdit(m_MapControl, m_enumAttributeEditMode);
                m_frmAttributeEdit.Owner = m_mainFrm;
                m_frmAttributeEdit.FormClosed += new FormClosedEventHandler(frmAttributeEdit_FormClosed);
            }

            m_frmAttributeEdit.Show();
            m_frmAttributeEdit.FillData(m_MapControl.ActiveView.FocusMap, pGeometry);
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
            IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            if (m_pNewEnvelope != null)
            {
                m_pNewEnvelope.MoveTo(pPoint);
            }
        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }

}
