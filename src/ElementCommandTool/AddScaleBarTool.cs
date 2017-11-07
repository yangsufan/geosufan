using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;

namespace ElementCommandTool
{
    [Guid("7720065E-BA47-4d5b-92E5-F5DD716D7C93")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.AddScaleBarTool")]
    public sealed class AddScaleBarTool : BaseTool
    {
        private IHookHelper m_HookHelper;
        private INewEnvelopeFeedback m_Feedback;
        private IPoint m_Point;
        private bool m_InUse;

        [DllImport("User32", CharSet = CharSet.Auto)]
        private static extern int SetCapture(int hWnd);
        [DllImport("User32", CharSet = CharSet.Auto)]
        private static extern int GetCapture();
        [DllImport("User32", CharSet = CharSet.Auto)]
        private static extern int ReleaseCapture();

        public AddScaleBarTool()
        {
            m_HookHelper = new HookHelperClass();

            //base.m_cursor = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream(GetType(), "AddNorthArrowTool.cur"));
            //base.m_bitmap = Properties.Resource.scalebar;
            //base.m_bitmap = new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream(GetType(), "Resources\\scalebar.bmp"));
            base.m_caption = "比例尺";
            base.m_category = "NJGIS";
            base.m_message = "添加比例尺";
            base.m_name = "NJ_AddScaleBar";
            base.m_toolTip = "添加比例尺";
            base.m_deactivate = true;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream("ElementCommandTool.CursorRec.cur"));//设置鼠标

            }
            catch
            {

            }
        }

        public override void OnCreate(object hook)
        {
            m_HookHelper.Hook = hook;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            m_Point = m_HookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            SetCapture(m_HookHelper.ActiveView.ScreenDisplay.hWnd);

            m_InUse = true;
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (m_InUse == false) return;

            if (m_Feedback == null)
            {
                m_Feedback = new NewEnvelopeFeedbackClass();
                m_Feedback.Display = m_HookHelper.ActiveView.ScreenDisplay;
                m_Feedback.Start(m_Point);
            }

            m_Feedback.MoveTo(m_HookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y));
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_InUse == false) return;

            if (GetCapture() == m_HookHelper.ActiveView.ScreenDisplay.hWnd)
                ReleaseCapture();

            if (m_Feedback == null)
            {
                m_Feedback = null;
                m_InUse = false;
                return;
            }
            IEnvelope envelope = m_Feedback.Stop();
            if ((envelope.IsEmpty) || (envelope.Width == 0) || (envelope.Height == 0))
            {
                m_Feedback = null;
                m_InUse = false;
                return;
            }

            frmSymbolSelector symbolForm = new frmSymbolSelector();
            IStyleGalleryItem styleGalleryItem = symbolForm.GetItem(esriSymbologyStyleClass.esriStyleClassScaleBars, null);
            symbolForm.Dispose();
            if (symbolForm.DialogResult != DialogResult.OK) return;
            if (styleGalleryItem == null) return;

            IMapFrame mapFrame = (IMapFrame)m_HookHelper.ActiveView.GraphicsContainer.FindFrame(m_HookHelper.ActiveView.FocusMap);
            UID vUid = new UIDClass();
            vUid.Value = "{6589F143-F7F7-11d2-B872-00600802E603}";
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(vUid, (IMapSurround)styleGalleryItem.Item);
            if (mapSurroundFrame.MapSurround is IScaleBar)
            {
                IScaleBar pScalbeBar = (IScaleBar)mapSurroundFrame.MapSurround;
                pScalbeBar.Units = mapFrame.Map.MapUnits;
                switch (pScalbeBar.Units)
                {
                    case esriUnits.esriMeters:
                        pScalbeBar.UnitLabel = "米";
                        break;
                    case esriUnits.esriKilometers:
                        pScalbeBar.UnitLabel = "公里";
                        break;
                    case esriUnits.esriMiles:
                        pScalbeBar.UnitLabel = "英里";
                        break;
                    case esriUnits.esriDecimalDegrees:
                        pScalbeBar.UnitLabel = "度";
                        break;

                }

            }


            IElement element = (IElement)mapSurroundFrame;
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.Name = "比例尺";

            IGraphicsContainerSelect pGraphicsSel = m_HookHelper.ActiveView.GraphicsContainer as IGraphicsContainerSelect;
            pGraphicsSel.UnselectAllElements();
            pGraphicsSel.SelectElement(element);
            m_HookHelper.ActiveView.GraphicsContainer.AddElement(element, 0);
            m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            m_Feedback = null;
            m_InUse = false;
        }
    }
}
