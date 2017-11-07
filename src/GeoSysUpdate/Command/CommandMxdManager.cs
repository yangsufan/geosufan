using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;
using System.Xml;
namespace GeoSysUpdate
{
    public class CommandMxdManager : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandMxdManager()
        {
            base._Name = "GeoSysUpdate.CommandMxdManager";
            base._Caption = "管理显示方案";
            base._Tooltip = "管理显示方案";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "管理显示方案";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentControl == null) return false;
                if (m_Hook.CurrentControl is ISceneControl) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("创建书签");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FrmManagerMxd pFrm = new FrmManagerMxd(Plugin.ModuleCommon.TmpWorkSpace);
            DialogResult pRes = pFrm.ShowDialog();
            if (pRes == DialogResult.OK)
            {
                string strCondition=pFrm.m_Condition;
                IMap pMap = null;
                SysCommon.ModSysSetting.CopySelectedMap(Plugin.ModuleCommon.TmpWorkSpace, strCondition,out pMap);
                string _TmpPath = Application.StartupPath + "\\..\\res\\xml\\自定义图层树.xml";
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, _TmpPath);
                XmlDocument pDoc = new XmlDocument();
                pDoc.Load(_TmpPath);

                GeoLayerTreeLib.LayerManager.UcDataLib pUC = null;
                Plugin.Application.AppGidUpdate pTmpApp = m_Hook as Plugin.Application.AppGidUpdate;
                pUC = pTmpApp.LayerTree as GeoLayerTreeLib.LayerManager.UcDataLib;

                pUC.RefreshDataByMap(pDoc, pMap);
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