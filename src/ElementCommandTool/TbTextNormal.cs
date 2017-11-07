using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace ElementCommandTool
{
    [Guid("158411F5-1AF5-4dc9-9C25-95397DDE1A32")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.TbTextNormal")]
    public sealed class TbTextNormal : ICommand, ITool
    {
        IHookHelper m_Hook;
        IActiveView m_pAV;
        IScreenDisplay m_pScrd;
        System.Drawing.Point m_LocatePt;
        System.Windows.Forms.Cursor m_Cursor;

        private ESRI.ArcGIS.Geometry.IPoint m_pPoint;

        public TbTextNormal()
        {
            m_Hook = new HookHelperClass();
            try
            {
                 m_Cursor= new System.Windows.Forms.Cursor(GetType().Assembly.GetManifestResourceStream("ElementCommandTool.Cursor.cur"));//设置鼠标

            }
            catch
            {

            }
        }

        ~TbTextNormal()
        {
            m_Hook = null;
            m_pAV = null;

        }

        public System.Drawing.Point LocatePt
        {
            set
            {
                m_LocatePt = value;
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
                return "添加注记";
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
                return default(bool);
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
                return "添加注记";
            }
        }

        public string Name
        {
            get
            {
                return "添加注记";
            }
        }

        public void OnClick()
        {
            m_pAV = m_Hook.ActiveView;
            m_pScrd = m_pAV.ScreenDisplay;
        }

        public void OnCreate(object hook)
        {
            m_Hook.Hook = hook;
        }

        public string Tooltip
        {
            get
            {

                return "添加注记";
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
            //frmEditControl frm = new frmEditControl();
            //frm.Location = new System.Drawing.Point(x + m_LocatePt.X, y + m_LocatePt.Y); // * pScreen.BitsPerPixel
            //frm.ShowDialog();
            //string strText = frm.txtText.Text;
        }

        private void pBox_Enter(object sender, EventArgs e)
        {
            //pBox.Dispose();
            //m_pBox.Hide();
        }

        public void OnMouseMove(int button, int shift, int x, int y)
        {
        }

        public void OnMouseUp(int button, int shift, int x, int y)
        {
            if (button != 1) return;
            IPoint pPoint = m_pScrd.DisplayTransformation.ToMapPoint(x, y);
            m_pPoint = pPoint;
            AddNormalTextElement(m_pPoint);
        }

        private void AddNormalTextElement(IPoint pPoint)
        {
            IGraphicsContainer pGraphicCtn = m_pAV.GraphicsContainer;
            try
            {
                frmEdit frmCallOut = new frmEdit(false);
                PublicClass.POINTAPI pos = new PublicClass.POINTAPI();
                PublicClass.GetCursorPos(ref pos);
                int scrW = Screen.PrimaryScreen.Bounds.Width;//主显示宽度
                int scrH = Screen.PrimaryScreen.Bounds.Height;//主显示高度
                frmCallOut.Location = new System.Drawing.Point(pos.x, pos.y); // * pScreen.BitsPerPixel
                if (pos.x + frmCallOut.Width > scrW
                    && pos.y + frmCallOut.Height > scrH)//超出显示器边界宽和高，则迂回
                {
                    frmCallOut.Location = new System.Drawing.Point(scrW - frmCallOut.Width,
                        scrH - frmCallOut.Height);
                }
                if (pos.x + frmCallOut.Width > scrW
                   && pos.y + frmCallOut.Height <scrH)//超出显示器边界宽，则迂回
                {
                    frmCallOut.Location = new System.Drawing.Point(scrW - frmCallOut.Width,pos.y);
                       
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

                pTextElement.Symbol = pTextSymbol as ITextSymbol;
                IElement pElement = (IElement)pTextElement;
                pElement.Geometry = pPoint;

                //刷新显示
                frmCallOut.Dispose();
                IGraphicsContainerSelect pGraphicsSel = pGraphicCtn as IGraphicsContainerSelect;
                pGraphicsSel.UnselectAllElements();
                pGraphicsSel.SelectElement(pElement);
                pGraphicCtn.AddElement(pElement, 0);
                m_pAV.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("添加文本注记失败：" + ex.Message, "提示");
                return;
            }
        }

        public void Refresh(int hdc)
        {

        }
        #endregion
    }
}
