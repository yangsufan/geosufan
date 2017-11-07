using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;


using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoPageLayout
{
  
   public class BzffOutMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        string pMapNo = "";
        int pScale = 50000;
        private IPoint pPoint = null;
        private SheetType sheetType = SheetType.foundationTerrain; 
        public BzffOutMap(string inMapNo,int inScale,IPoint inPoint,SheetType inST)
        {
            base._Name = "GeoDataManagerFrame.BzffOutMap";
            base._Caption = "标准分幅制图";
            base._Tooltip = "标准分幅制图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "标准分幅制图";
            pMapNo = inMapNo;
            pScale = inScale;
            pPoint = inPoint;
            sheetType = inST;
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("标准分幅制图");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string scaleDM=ModGetData.GetDMofScale("1:" + pScale.ToString());
            if (scaleDM == "")
                return;
            IMap pMap = new MapClass();
            pMap.SpatialReference = m_Hook.ArcGisMapControl.Map.SpatialReference;
            if(sheetType==SheetType.foundationTerrain)
                 ModGetData.AddMapOfNoneXZQ(pMap,"DLG"+scaleDM,m_Hook.ArcGisMapControl.Map);
            else if(sheetType==SheetType.urbanCadastre)
                 ModGetData.AddMapOfNoneXZQ(pMap, "CZDJ", m_Hook.ArcGisMapControl.Map);
            GeoPageLayout pPageLayout = new
               GeoPageLayout(pMap, pMapNo, pScale, pPoint, 0);
            pPageLayout.typePageLayout = 3;
            pPageLayout.MapOut();

            pPageLayout = null;
        }
        
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}