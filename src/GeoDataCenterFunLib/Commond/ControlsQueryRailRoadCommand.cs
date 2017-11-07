using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using SysCommon.Error;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataCenterFunLib
{

    /// <summary>
    /// 作者：yjl
    /// 日期：20110730
    /// 说明：道路查询
    /// </summary>

    public class ControlsQueryRailRoadCommand : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        Plugin.Application.IAppFormRef _pAppForm = null;

        public ControlsQueryRailRoadCommand()
        {
            base._Name = "GeoDataCenterFunLib.ControlsQueryRailRoadCommand";
            base._Caption = "铁路查询";
            base._Tooltip = "铁路查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "铁路查询";
            //base._Image = "";
            //base._Category = "";
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
        public override bool Enabled
        {
            /*  get
              {
                  //不存在图幅结合表图层、数据操作进程正在进行时不可用
                  if (_AppHk.MapControl == null) return false;
                  if (_AppHk.MapControl.Map.LayerCount == 0) return false;
                  return true;
              }*/
            get
            {
                try
                {
                    if (_AppHk.CurrentControl is ISceneControl) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;

            string strFieldName = "";//名称字段
            string strFieldCode = "";//编码字段
            string strRoadName = "铁路查询";
            string strLayerName = "";
            IFeatureClass pRoadFeaClass = null;
            try
            {//获取道路地物类
                ModQuery.GetQueryConfig(strRoadName,out pRoadFeaClass,out strLayerName, out strFieldName, out strFieldCode);
                if (pRoadFeaClass == null)
                {
                    MessageBox.Show("找不到铁路数据,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //检查河流名称字段
                if (pRoadFeaClass.FindField(strFieldName) < 0)
                {
                    MessageBox.Show("找不到铁路名称属性,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pRoadFeaClass = null;
                    return;
                }
                //检查河流编码字段
                if (pRoadFeaClass.FindField(strFieldCode) < 0)
                {
                    MessageBox.Show("找不到铁路编码属性,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pRoadFeaClass = null;
                    return;
                }
                frmQueryRoad fmQD = new frmQueryRoad(_pAppForm.MainForm, _AppHk.MapControl, pRoadFeaClass, strLayerName,strFieldName, strFieldCode, "铁 路 名：", "铁路编码：", "铁路查询");
                fmQD.WriteLog = this.WriteLog;
                fmQD.Show(_pAppForm.MainForm );
                 
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);

            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _pAppForm = hook as Plugin.Application.IAppFormRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
