using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoDataManagerFrame
{
    public class CommandAddressQueryQY : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandAddressQueryQY()
        {
            base._Name = "GeoDataManagerFrame.CommandAddressQueryQY";
            base._Caption = "地名地址查询";
            base._Tooltip = "地名地址查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "地名地址查询";
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
            //    log.Writelog("地名地址查询");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption + " 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
                }
                return;
            }
            IFeatureLayer pFLayer=getDMDZLayer(m_Hook.ArcGisMapControl.Map, "区域地名");
            if (pFLayer == null)
            {
                MessageBox.Show("当前没有加载区域地名图层！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption + " 提示‘当前没有加载区域地名图层！’", m_Hook.tipRichBox);
                }
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption, m_Hook.tipRichBox);
            }
            FrmAddressQueryQY fmMBMM = new FrmAddressQueryQY(m_Hook.MapControl.Map);
            //int fmMBMMX = m_frmhook.MainForm.Location.X + m_frmhook.MainForm.Width - fmMBMM.Width;
            //int fmMBMMY = m_frmhook.MainForm.Location.Y + 100;
            //fmMBMM.SetDesktopLocation(fmMBMMX, fmMBMMY);
            fmMBMM.Show(m_frmhook.MainForm);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
        //得到地名地址查询图层
        private IFeatureLayer getDMDZLayer(IMap inMap, string strLayerName)
        {
            IFeatureLayer rstFeatureLayer = null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j).Name != strLayerName)
                            continue;
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        rstFeatureLayer = pFLayer;
                        if (rstFeatureLayer != null)
                            break;

                    }
                }
                else//不是grouplayer
                {
                    if (pLayer.Name != strLayerName)
                        continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    rstFeatureLayer = pFLayer;
                    if (rstFeatureLayer != null)
                        break;
                }
            }
            return rstFeatureLayer;
        }
    }
}