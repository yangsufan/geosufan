using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace GeoDataManagerFrame
{
   public class CommandDJAttributeQuery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        private IFeatureClass _ZDFeatureClass = null;
        public CommandDJAttributeQuery()
        {
            base._Name = "GeoDataManagerFrame.CommandDJAttributeQuery";
            base._Caption = "宗地查询";
            base._Tooltip = "宗地查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "宗地查询";
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                
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
           if (_AppHk.MapControl.LayerCount == 0) { MessageBox.Show("未加载数据！", "提示！"); return; }

           string ZDNodeKey;
           string JFNodeKey;
           string playerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树0.xml";
           SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath);
           GeoDataManagerFrame.ModStatReport.GetCZDJLayerKey(playerTreePath, out ZDNodeKey, out JFNodeKey);
           _ZDFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath, ZDNodeKey);
           if (_ZDFeatureClass == null) { MessageBox.Show("未添加宗地数据！", "提示！"); return; }
           frmDJAttributeQuery pfrmDJAttributeQuery = new frmDJAttributeQuery(_ZDFeatureClass,Plugin.ModuleCommon.TmpWorkSpace,Plugin.Mod.User,_AppHk.ArcGisMapControl);
           pfrmDJAttributeQuery.WriteLog = this.WriteLog; //ygc 2012-9-12 是否写日志
           pfrmDJAttributeQuery.ShowDialog();
           System.IO.File.Delete(playerTreePath);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }

    }
}
