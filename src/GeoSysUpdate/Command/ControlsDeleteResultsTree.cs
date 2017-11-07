using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GeoSysUpdate
{
   public  class ControlsDeleteResultsTree: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsDeleteResultsTree()
        {
            base._Name = "GeoSysUpdate.ControlsDeleteResultsTree";
            base._Caption = "删除目录";
            base._Tooltip = "删除目录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除目录";
            //base._Image = "";
            //base._Category = "";
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                TreeNode selectNode = _hook.ResultsTree.SelectedNode;
                if (selectNode == null) return false;
                if ((selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() == "Root") return false;
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
            if (_hook.ResultsTree== null) return;
            TreeNode selectNode = _hook.ResultsTree.SelectedNode;
            if (selectNode != null)
            {
                if (MessageBox.Show("确定要删除该级目录？此操作将导致该目录无法恢复", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    ModStringpro.DeleteFolder((selectNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim());
                    _hook.ResultsTree.Nodes.Remove(selectNode);
                }
            }
            
        }
  
       
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            if (_AppHk.MapControl == null) return;
        }
    }
}