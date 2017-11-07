using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
///////ZQ  20111007   行政区数据提取
namespace GeoDataCenterFunLib 
{
    public class ControlsXZQDataExport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        DevComponents.AdvTree.AdvTree m_xzqTree = null;
        public ControlsXZQDataExport()
        {
            base._Name = "GeoDataCenterFunLib.ControlsXZQDataExport";
            base._Caption = "行政区地图数据提取";
            base._Tooltip = "行政区地图数据提取";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区地图数据提取";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.CurrentControl is ESRI.ArcGIS.Controls.ISceneControl) return false;
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                //try
                //{
                //    if (m_xzqTree.SelectedNode.Tag.ToString() != "County" && m_xzqTree.SelectedNode.Tag.ToString() != "Town" && m_xzqTree.SelectedNode.Tag.ToString() != "Village") return false;
                //}
                //catch {}
                //else { return false; }
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {//XZQLocation
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace,ModGetData.LayerXMLpath);
            IGeometry xzqGeo = ModGetData.getExtentByXZQ(m_xzqTree.SelectedNode);
            if (xzqGeo == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区范围！");
                return;
            }
            ///ZQ  20111027 add  判断数据字典是否初始化
            if (SysCommon.ModField._DicFieldName.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            GeoDataExport.frmExport pfrmExport = new GeoDataExport.frmExport(_hook.MapControl.Map, xzqGeo);
            pfrmExport.WriteLog = this.WriteLog;//ygc 2012-9-11 新增是否写日志
            pfrmExport.XZQCode = m_xzqTree.SelectedNode.Name;//yjl20111012 过滤行政区代码
            pfrmExport.ShowDialog();
          
         }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            m_xzqTree = _hook.XZQTree;
        }

    }
}
