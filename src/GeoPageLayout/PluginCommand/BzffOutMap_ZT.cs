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
    //yjl20110914 add 森林资源现状专题分幅出图
   public class BzffOutMap_ZT : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        string pMapNo = "";
        int pScale = 50000;
        private IPoint pPoint = null;
        private IMap pMap = null;
        public BzffOutMap_ZT(string inMapNo, int inScale, IPoint inPoint,IMap inMap)
        {
            base._Name = "GeoDataManagerFrame.BzffOutMap_ZT";
            base._Caption = "标准分幅制图";
            base._Tooltip = "标准分幅制图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "标准分幅制图";
            pMapNo = inMapNo;
            pScale = inScale;
            pPoint = inPoint;
            pMap = inMap;
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
            try
            {
                GeoPageLayout pPageLayout = new
                   GeoPageLayout(pMap, pMapNo, pScale, pPoint, 1);
                pPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pPageLayout.typePageLayout = 3;
                pPageLayout.MapOut();

                pPageLayout = null;
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
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