using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace ElementCommandTool
{
    [Guid("B377A0B5-0412-4ec3-830F-3985DEFC9752")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.AddLegendCommand")]
    public sealed class AddLegendCommand : BaseCommand
    {
        private IHookHelper m_pHookHelper;

        public AddLegendCommand()
        {
            base.m_category = "NJGIS";
            base.m_caption = "Ìí¼ÓÍ¼Àý";
            base.m_message = "Ìí¼ÓÍ¼Àý";
            base.m_toolTip = "Ìí¼ÓÍ¼Àý";
            base.m_name = "NJ_AddLegend";
          
            //try
            //{
            //    string bitmapResourceName = GetType().Name + ".bmp";
            //    base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            //}
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

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_pHookHelper.PageLayout == null) return;

            try
            {
                IActiveView pView = (IActiveView)m_pHookHelper.PageLayout;
                frmAddLegend frm = new frmAddLegend(true);
                
                IMapSurroundFrame pLegendFrame = frm.GetMapLegend(pView);
                if (pLegendFrame == null) return;
                frm.Dispose();

                pView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ìí¼ÓÍ¼ÀýÊ§°Ü£º" + ex.Message, "ÌáÊ¾");
            }
        }
        #endregion
    }
}
