using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;


namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110915
    /// 说明：点击地图森林资源现状分幅图
    /// </summary>
   public class ControlsMapOutTDLYFFT : Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGisUpdateRef m_Hook;
       private Plugin.Application.IAppFormRef m_frmhook;
       public ControlsMapOutTDLYFFT()
        {
            base._Name = "GeoPageLayout.ControlsMapOutTDLYFFT";
            base._Caption = "标准图幅";
            base._Tooltip = "标准图幅";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "标准图幅";
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
            //    log.Writelog("标准图幅");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("出森林资源现状分幅图 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
                }
                return;
            }
            ISpatialReference pSpatialRefrence = m_Hook.ArcGisMapControl.SpatialReference;
            if (!(pSpatialRefrence is IProjectedCoordinateSystem))
            {
                //MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //Plugin.LogTable.Writelog("标准分幅制图 提示‘请设置地图的投影坐标！’", m_Hook.tipRichBox);
                //return;
            }
            try
            {
                IFeatureClass xzqFC = ModGetData.GetXZQFC("//LayerConfig/County");
                if (xzqFC == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到县级行政区图层！");
                    return;
                }
                string xzqdmFD = ModGetData.GetXZQFd("//LayerConfig/County");
                if (xzqdmFD == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到县节点的行政区代码字段名称！");
                    return;
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("森林资源现状分幅图", m_Hook.tipRichBox);
                }
                m_Hook.ArcGisMapControl.CurrentTool = null;

                FrmSheetMapUserSet_ZT fmSMUS = new
                     FrmSheetMapUserSet_ZT(m_Hook.ArcGisMapControl, m_frmhook.MainForm, "", xzqFC, xzqdmFD);
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
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
