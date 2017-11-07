using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    class ControlsOpenResults: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsOpenResults()
        {
            base._Name = "GeoSysUpdate.ControlsOpenResults";
            base._Caption = "浏览成果";
            base._Tooltip = "浏览成果";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "浏览成果";
            //base._Image = "";
            //base._Category = "";
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_hook.ResultsTree == null) return false;
                TreeNode selectNode = _hook.ResultsTree.SelectedNode;
                if (selectNode == null) return false;
                if ((selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() == "File")
                {
                    return true;
                }
                return false;
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
                string strPath = (selectNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim();
                if (File.Exists(strPath))
                {
                    //当打开的成果为mxd时，打开方式不同
                    if (Path.GetExtension(strPath) != ".mxd")
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(strPath);
                        }
                        catch
                        {
                            MessageBox.Show("请安装该类型成果文件相关的软件", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        IMapDocument pMapDoc = new MapDocumentClass();
                        pMapDoc.Open(strPath,"");
                        GeoPageLayout.FrmPageLayout pFrmPageLayout = new GeoPageLayout.FrmPageLayout(pMapDoc.PageLayout);
                        pFrmPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                        pFrmPageLayout.ShowDialog();

                    }
                }
                else
                {
                    MessageBox.Show(Path.GetFileName(strPath)+"成果文件已被删除", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information); 
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