using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
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
    [Guid("A93FA8C8-B14C-441c-91D6-47A33876000A")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.AddScaleTextTool")]
    public sealed class AddScaleTextTool : BaseTool
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

        public AddScaleTextTool()
        {
            m_HookHelper = new HookHelperClass();

            //base.m_cursor = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream(GetType(), "AddNorthArrowTool.cur"));
            //base.m_bitmap = Resource.ScaleText;
            //base.m_bitmap = new System.Drawing.Bitmap(GetType().Assembly.GetManifestResourceStream(GetType(), "Resources\\ScaleText.bmp"));
            base.m_caption = "比例尺说明";
            base.m_category = "NJGIS";
            base.m_message = "添加比例尺说明";
            base.m_name = "NJ_AddScaleText";
            base.m_toolTip = "添加比例尺说明";
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

            //Stop capturing mouse events
            if (GetCapture() == m_HookHelper.ActiveView.ScreenDisplay.hWnd)
                ReleaseCapture();

            //If an envelope has not been tracked or its height/width is 0
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
            IStyleGalleryItem styleGalleryItem = symbolForm.GetItem(esriSymbologyStyleClass.esriStyleClassScaleTexts, null);
            //Release the form
            symbolForm.Dispose();
            if (symbolForm.DialogResult != DialogResult.OK) return;
            if (styleGalleryItem == null) return;

            //Get the map frame of the focus map
            IMapFrame mapFrame = (IMapFrame)m_HookHelper.ActiveView.GraphicsContainer.FindFrame(m_HookHelper.ActiveView.FocusMap);

            //Create a map surround frame
            UID vUid = new UIDClass();
            vUid.Value = "{7A3F91DC-B9E3-11d1-8756-0000F8751720}";
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(vUid, (IMapSurround)styleGalleryItem.Item);

            IElement element = (IElement)mapSurroundFrame;
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.Name = "比例尺";
            IGraphicsContainerSelect pGraphicsSel = m_HookHelper.ActiveView.GraphicsContainer as IGraphicsContainerSelect;
            pGraphicsSel.UnselectAllElements();
            pGraphicsSel.SelectElement(element);
            m_HookHelper.ActiveView.GraphicsContainer.AddElement(element, 0);
            //Refresh
            m_HookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            m_Feedback = null;
            m_InUse = false;
        }
    }
}
