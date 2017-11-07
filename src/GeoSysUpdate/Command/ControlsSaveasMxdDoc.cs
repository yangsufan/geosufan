using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using SysCommon.Error;
namespace GeoSysUpdate
{
    public class ControlsSaveasMxdDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsSaveasMxdDoc()
        {
            base._Name = "GeoSysUpdate.ControlsSaveasMxdDoc";
            base._Caption = "另存为地图文档";
            base._Tooltip = "另存为地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "另存为地图文档";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
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
        {
            //保存路径

            SaveFileDialog Sfd = new SaveFileDialog();

            Sfd.Title = "另存为地图文档";

            Sfd.Filter = "(*.mxd)|*.mxd";

            if (Sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ModMxd._MxdPath = Sfd.FileName;

            IMxdContents pMxdC;

            pMxdC = _AppHk.MapControl.Map as IMxdContents;

            IMapDocument pMapDocument = new MapDocumentClass();
            //创建地图文档
            pMapDocument.New(ModMxd._MxdPath);

            //保存信息
            IActiveView pActiveView = _AppHk.MapControl.Map as IActiveView;

            pMapDocument.ReplaceContents(pMxdC);
            try//yjl20110817 防止保存时，其他程序也在打开这个文档而导致共享冲突从而使系统报错
            {
                pMapDocument.Save(true, true);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("保存地图文档到" + ModMxd._MxdPath);
                }
                ErrorHandle.ShowFrmErrorHandle("提示", "保存成功！");
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "保存失败！请关掉其他正在打开该文档的程序，重新试一次。");
            }
            pMapDocument.Close();//yjl20110817

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
