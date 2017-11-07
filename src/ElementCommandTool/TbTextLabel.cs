using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

public enum geoDimensionStyle
{
    geoLineCallout, geoBalloonCallout
};

namespace ElementCommandTool
{
    [Guid("7A263E26-11DB-4621-9873-62D785B99D3F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.TbTextLabel")]
    public sealed class TbTextLabel : ICommand, ITool
    {
        IHookHelper m_pHookHelper;
        INewLineFeedback m_pLinefeedback;
        IActiveView m_pActiveView;
        IScreenDisplay m_pScreenDisplay;
        IPolyline m_pPolyline;
        int m_iLocateX, m_iLocateY;
        stdole.IFontDisp m_pFont;
        double m_dFontSize;
        System.Drawing.Point m_LocatePt;
        System.Windows.Forms.Cursor m_Cursor;
        geoDimensionStyle m_Style;

        public TbTextLabel()
        {
            m_pHookHelper = new HookHelperClass();
            m_LocatePt = new System.Drawing.Point(0, 0);
           
            m_dFontSize = 16;
            m_Style = geoDimensionStyle.geoLineCallout;
            try
            {
                m_Cursor = new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream("ElementCommandTool.CursorLine.cur"));//设置鼠标

            }
            catch
            {
 
            }
        }
        ~TbTextLabel()
        {
            m_pHookHelper = null;
            m_pLinefeedback = null;
            m_pActiveView = null;
            m_pScreenDisplay = null;
            m_Cursor = null;
        }

        public geoDimensionStyle Style
        {
            set
            {
                m_Style = value;
            }
        }

        public System.Drawing.Point LocatePt
        {
            set
            {
                m_LocatePt = value;
            }
        }

        public stdole.IFontDisp Font
        {
            set
            {
                m_pFont = value;
            }
        }

        public double FontSize
        {
            get
            {
                return m_dFontSize;
            }
            set
            {
                m_dFontSize = value;
            }
        }

        #region "ICommand Implementations"
        public int Bitmap
        {
            get
            {
                return default(int);
            }
        }

        public string Caption
        {
            get
            {
                return "添加标注";
            }
        }

        public string Category
        {
            get
            {
                return "GeoLayoutEdit";
            }
        }

        public bool Checked
        {
            get
            {
                return false;
            }
        }

        public bool Enabled
        {
            get
            {
                return true;
            }
        }

        public int HelpContextID
        {
            get
            {
                return default(int);
            }
        }

        public string HelpFile
        {
            get
            {
                return default(string);
            }
        }

        public string Message
        {
            get
            {
                return "添加插图标注";
            }
        }

        public string Name
        {
            get
            {
                return "添加标注";
            }
        }

        public void OnClick()
        {
            m_pActiveView = m_pHookHelper.ActiveView;
            m_pScreenDisplay = m_pActiveView.ScreenDisplay;
        }

        public void OnCreate(object hook)
        {
            m_pHookHelper.Hook = hook;
        }

        public string Tooltip
        {
            get
            {
                return "添加插图标注";
            }
        }
        #endregion

        #region "ITool Implementations"
        public int Cursor
        {
            get
            {
                if (m_Cursor != null)
                    return m_Cursor.Handle.ToInt32();
                else
                    return 0;
            }
        }

        public bool Deactivate()
        {
            return true;
        }

        public bool OnContextMenu(int x, int y)
        {
            return default(bool);
        }

        public void OnDblClick()
        {

        }

        public void OnKeyDown(int keyCode, int shift)
        {
        }

        public void OnKeyUp(int keyCode, int shift)
        {
        }

        public void OnMouseDown(int button, int shift, int x, int y)
        {
            IPoint pPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
            if (m_pLinefeedback == null)
            {
                m_pLinefeedback = new NewLineFeedbackClass();
                m_pLinefeedback.Display = m_pScreenDisplay;
                m_pLinefeedback.Start(pPoint);
                m_iLocateX = x;
                m_iLocateY = y;
            }
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
            if (m_pLinefeedback != null)
            {
                IPoint pPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                m_pLinefeedback.MoveTo(pPoint);
            }
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if (m_pLinefeedback != null)
            {
                IPoint pPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                m_pLinefeedback.AddPoint(pPoint);
                m_pPolyline = m_pLinefeedback.Stop();
                m_pLinefeedback = null;
                AddCallOutElement(m_pPolyline);
            }
        }

        public void Refresh(int hdc)
        {
        }
        #endregion

        private void AddCallOutElement(IPolyline pPolyline)
        {
            IGraphicsContainer pGraphicCtn = m_pActiveView.GraphicsContainer;
            try
            {
                frmEdit frmCallOut = new frmEdit(true);
                PublicClass.POINTAPI pos = new PublicClass.POINTAPI();
                PublicClass.GetCursorPos(ref pos);
                frmCallOut.Location = new System.Drawing.Point(pos.x, pos.y); // * pScreen.BitsPerPixel
                int scrW = Screen.PrimaryScreen.WorkingArea.Width;//主显示宽度
                int scrH = Screen.PrimaryScreen.WorkingArea.Height;//主显示高度
                frmCallOut.Location = new System.Drawing.Point(pos.x, pos.y); // * pScreen.BitsPerPixel
                if (pos.x + frmCallOut.Width > scrW
                    && pos.y + frmCallOut.Height > scrH)//超出显示器边界宽和高，则迂回
                {
                    frmCallOut.Location = new System.Drawing.Point(scrW - frmCallOut.Width,
                        scrH - frmCallOut.Height);
                }
                if (pos.x + frmCallOut.Width > scrW
                   && pos.y + frmCallOut.Height < scrH)//超出显示器边界宽，则迂回
                {
                    frmCallOut.Location = new System.Drawing.Point(scrW - frmCallOut.Width, pos.y);

                }
                if (pos.x + frmCallOut.Width < scrW
                   && pos.y + frmCallOut.Height > scrH)//超出显示器边界高，则迂回
                {
                    frmCallOut.Location = new System.Drawing.Point(pos.x, scrH - frmCallOut.Height);

                }
                frmCallOut.ShowDialog();
                if (frmCallOut.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;

                ITextElement pTextElement = new TextElementClass();
                pTextElement.ScaleText = true;
                pTextElement.Text = frmCallOut.AnnoText;

                IFormattedTextSymbol pTextSymbol = (IFormattedTextSymbol)frmCallOut.m_pTextSymbol;

                IBalloonCallout pCallout = new BalloonCalloutClass();
                pCallout.Symbol = frmCallOut.m_pFillSymbol;
                pCallout.AnchorPoint = pPolyline.FromPoint;
                pTextSymbol.Background = (ITextBackground)pCallout;

                pTextElement.Symbol = pTextSymbol as ITextSymbol;
                IElement pElement = (IElement)pTextElement;
                pElement.Geometry = pPolyline.ToPoint;

                //刷新显示
                frmCallOut.Dispose();
                IGraphicsContainerSelect pGraphicsSel = pGraphicCtn as IGraphicsContainerSelect;
                pGraphicsSel.UnselectAllElements();
                pGraphicsSel.SelectElement(pElement);
                pGraphicCtn.AddElement(pElement, 0);
                m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("添加文本注记失败：" + ex.Message, "提示");
                return;
            }
        }
    }
}
