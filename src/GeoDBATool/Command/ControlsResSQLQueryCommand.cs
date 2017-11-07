using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace GeoUtilities
{
    public class ControlsResSQLQueryCommand : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.AppGIS _AppHk;

        public ControlsResSQLQueryCommand()
        {
            base._Name = "GeoUtilities.ControlsResSQLQueryCommand";
            base._Caption = "SQL查询";
            base._Tooltip = "SQL查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "SQL查询";
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
            get
            {
                //不存在图幅结合表图层、数据操作进程正在进行时不可用
                if (_AppHk == null) return false;
                if (_AppHk.CurrentControl is ISceneControl) return false;  //为了只有效于2维控件
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.Map.LayerCount == 0) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;
            FrmSQLQuery frmSQL = new FrmSQLQuery(_AppHk.MapControl, true);
            frmSQL._QueryBar = _AppHk.QueryBar;
            frmSQL.ShowDialog();//.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as  Plugin.Application.AppGIS ;
            if (_AppHk.MapControl == null) return;
        }
    }
}
