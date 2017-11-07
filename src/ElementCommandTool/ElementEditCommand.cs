using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace ElementCommandTool
{
    [Guid("2836F8BD-16DD-4693-A38B-493961D7E46E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.ElementEditCommand")]
    public sealed class ElementEditCommand : BaseCommand
    {
        private IHookHelper m_pHookHelper;
        private IActiveView m_pAV;

        public ElementEditCommand()
        {
            base.m_category = "NJGIS";
            base.m_caption = "编辑Element";
            base.m_message = "显示并编辑Element属性";
            base.m_toolTip = "显示并编辑Element属性";
            base.m_name = "NJ_ElementEdit";
            try
            {
                string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = Resource.ElementEditCommand; //new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_pHookHelper == null)
                m_pHookHelper = new HookHelperClass();

            m_pHookHelper.Hook = hook;
        }

        public override bool Enabled
        {
            get
            {
                //获得当前选择对象
                try
                {
                    IGraphicsContainerSelect pGraphicsCon = m_pHookHelper.PageLayout as IGraphicsContainerSelect;
                    IEnumElement pEnumElement = pGraphicsCon.SelectedElements;
                    pEnumElement.Reset();
                    IElement pElement = pEnumElement.Next();
                    if (pElement != null && pEnumElement.Next() == null)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            try
            {
                m_pAV = m_pHookHelper.ActiveView;
                IViewManager pViewManager = (IViewManager)m_pAV;
                IEnumElement pEnumElement = (IEnumElement)pViewManager.ElementSelection;
                pEnumElement.Reset();
                IElement pElement = pEnumElement.Next();
                if (pElement == null) return;
                int i = 0;
                while (pElement != null)
                {
                    i++;
                    pElement = pEnumElement.Next();
                    if (i > 1) return;
                }

                pEnumElement.Reset();
                pElement = pEnumElement.Next();
                if (pElement is IMapSurroundFrame)
                {
                    IMapSurroundFrame pMapSFrame = (IMapSurroundFrame)pElement;
                    if (pMapSFrame.MapSurround is ILegend)
                    {
                        frmAddLegend vFrmLegend = new frmAddLegend(false);
                        vFrmLegend.SetMapSurroundFrame(pMapSFrame);
                        return;
                    }
                }
                frmElementProperty frm = new frmElementProperty(2);
                frm.StartEditElement(m_pAV, ref pElement);
                frm.ShowDialog();
                frm.Dispose();
            }
            catch { }
        }

        public void OnClick(int EditMode)
        {
            m_pAV = m_pHookHelper.ActiveView;
            IViewManager pViewManager = (IViewManager)m_pAV;
            IEnumElement pEnumElement = (IEnumElement)pViewManager.ElementSelection;
            pEnumElement.Reset();
            IElement pElement = pEnumElement.Next();
            if (pElement == null) return;
            int i = 0;
            while (pElement != null)
            {
                i++;
                pElement = pEnumElement.Next();
                if (i > 1) return;
            }

            pEnumElement.Reset();
            pElement = pEnumElement.Next();

            if (pElement is IMapSurroundFrame)
            {
                IMapSurroundFrame pMapSFrame = (IMapSurroundFrame)pElement;
                if (pMapSFrame.MapSurround is ILegend)
                {
                    frmAddLegend vFrmLegend = new frmAddLegend(false);
                    vFrmLegend.SetMapSurroundFrame(pMapSFrame);
                    return;
                }
            }

            IPageLayoutControl pPageLayoutControl = null;
            if (EditMode == 1)
            {
                if (m_pHookHelper.Hook is IToolbarControl)
                {
                    IToolbarControl pToolBarControl = m_pHookHelper.Hook as IToolbarControl;
                    if (pToolBarControl.Buddy is IPageLayoutControl)
                        pPageLayoutControl = pToolBarControl.Buddy as IPageLayoutControl;
                }
                else if (m_pHookHelper.Hook is IPageLayoutControl)
                {
                    pPageLayoutControl = m_pHookHelper.Hook as IPageLayoutControl;
                }

            }
            frmElementProperty frm = new frmElementProperty(EditMode);
            frm.set_PageLayoutControl = pPageLayoutControl;
            frm.StartEditElement(m_pAV, ref pElement);
            frm.ShowDialog();
            frm.Dispose();
        }
        #endregion
    }
}
