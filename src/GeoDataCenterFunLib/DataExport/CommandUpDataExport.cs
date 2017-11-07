using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    public class CommandUpDataExport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;

        public CommandUpDataExport()
        {
            base._Name = "GeoDataCenterFunLib.CommandUpDataExport";
            base._Caption = "上交数据转换输出";
            base._Tooltip = "上交数据转换输出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "上交数据转换输出";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.CurrentControl is ESRI.ArcGIS.Controls.ISceneControl) return false;
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
        {
            if (_AppHk == null) return;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Caption);//xisheng 日志记录
            }
            ESRI.ArcGIS.Carto.IMap pMap = _AppHk.MapControl.ActiveView.FocusMap;
            //IArea pArea = pGon as IArea;
            //double area = pArea.Area;
            //GetArea(ref area, pMap);
            //double dArea = SysCommon.ModSysSetting.GetExportAreaOfUser(Plugin.ModuleCommon.TmpWorkSpace, m_pAppForm.ConnUser);

            //if (dArea >= 0 && area > dArea)
            //{
            //    MessageBox.Show("超过提取最大面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            ///ZQ  20111027 add  判断数据字典是否初始化
            if (SysCommon.ModField._DicFieldName.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            GeoDataExport.frmExportEx frm = null;
            frm = new GeoDataExport.frmExportEx(pMap, null);
            frm.WriteLog = this.WriteLog;//ygc 2012-9-11 新增是否写日志
            frm.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;
            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
    }
}
