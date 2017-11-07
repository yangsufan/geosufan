using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace GeoSysUpdate
{
    public class ControlsResultFileLocation : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsResultFileLocation()
        {
            base._Name = "GeoSysUpdate.ControlsResultFileLocation";
            base._Caption = "定位";
            base._Tooltip = "定位";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "定位";
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
            LocateFile(pNode);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        private void LocateFile(DevComponents.AdvTree.Node pNode)
        {
            string filepath = pNode.Name;
            if(File.Exists(filepath))
            {
                string dirpath = filepath.Substring(0, filepath.LastIndexOf("\\"));
                System.Diagnostics.Process.Start("explorer.exe", dirpath);
            }
            else
            {
                if (Directory.Exists(filepath))
                {
                    System.Diagnostics.Process.Start("explorer.exe", filepath);
                }
            }

        }
    }
}
