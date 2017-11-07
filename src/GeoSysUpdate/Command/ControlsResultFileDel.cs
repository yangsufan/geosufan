using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace GeoSysUpdate
{
    public class ControlsResultFileDel : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsResultFileDel()
        {
            base._Name = "GeoSysUpdate.ControlsResultFileDel";
            base._Caption = "删除";
            base._Tooltip = "删除";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null) return false;
                if (_hook.ResultFileTree == null) return false;
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
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;
            if (_hook.ResultFileTree == null) return;
            DevComponents.AdvTree.Node pNode = _hook.ResultFileTree.SelectedNode;
            DeleteFile(pNode);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        private void DeleteFile(DevComponents.AdvTree.Node pNode)
        {
            try
            {
                string filepath = pNode.Name;
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                    pNode.Remove();
                }
                else
                {
                    if (Directory.Exists(filepath))
                    {
                        Directory.Delete(filepath, true);
                        pNode.Remove();
                    }
                }

            }
            catch
            {
            }

        }
    }
}
