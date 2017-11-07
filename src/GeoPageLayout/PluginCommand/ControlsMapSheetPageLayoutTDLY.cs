using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110915
    /// 说明：元数据右键菜单森林资源现状分幅图
    /// </summary>
    public class ControlsMapSheetPageLayoutTDLY : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.AppGidUpdate m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsMapSheetPageLayoutTDLY()
        {
            base._Name = "GeoSysUpdate.ControlsMapSheetPageLayoutTDLY";
            base._Caption = "森林资源分幅图";
            base._Tooltip = "森林资源分幅图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "森林资源分幅图";
        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook.MapControl == null || m_Hook.TOCControl == null) return false;
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {//XZQLocation
            if (m_Hook == null) return;
            if (m_Hook.MainUserControl == null) return;
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("标准分幅制图 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
                }
                return;
            }
            ISpatialReference pSpatialRefrence = m_Hook.ArcGisMapControl.SpatialReference;
            if (!(pSpatialRefrence is IProjectedCoordinateSystem))
            {
                MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("森林资源现状标准分幅制图 提示‘请设置地图的投影坐标！’", m_Hook.tipRichBox);
                }
                return;
            }
            try
            {
                IFeatureClass xzqFC = ModGetData.GetXZQFC("//LayerConfig/County");
                if (xzqFC == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区图层！");
                    return;
                }
                string xzqdmFD = ModGetData.GetXZQFd("//LayerConfig/County");
                if (xzqdmFD == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区配置文件的行政区代码字段名称！");
                    return;
                }
                string mapSheet = m_Hook.MetadataTree.SelectedNode.Text;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("森林资源现状标准分幅制图", m_Hook.tipRichBox);
                }
                m_Hook.ArcGisMapControl.CurrentTool = null;

                FrmSheetMapUserSet_ZT fmSMUS = new
                     FrmSheetMapUserSet_ZT(m_Hook.ArcGisMapControl, m_frmhook.MainForm, mapSheet, xzqFC, xzqdmFD);
                fmSMUS.Show(m_frmhook.MainForm);
                fmSMUS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.AppGidUpdate;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
