using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace GeoSysUpdate
{
    public class ControlsResultFileRefresh : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsResultFileRefresh()
        {
            base._Name = "GeoSysUpdate.ControlsResultFileRefresh";
            base._Caption = "刷新";
            base._Tooltip = "刷新";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "刷新";
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
            string DirPath = _hook.ResultFileTree.Nodes[0].Name;
            InitResultFileList(_hook.ResultFileTree, DirPath);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        private void InitResultFileList(DevComponents.AdvTree.AdvTree pTree, string DirPath)
        {
            pTree.Nodes.Clear();
            DevComponents.AdvTree.Node tparent;
            tparent = new DevComponents.AdvTree.Node();
            tparent.Text = "成果列表";
            tparent.Tag = 0;
            tparent.ImageIndex = 13;
            tparent.Name = DirPath;
            //tparent.SelectedImageIndex = 13;
            pTree.Nodes.Add(tparent);
            pTree.ExpandAll();
            AddLeafItemFromFile(tparent, DirPath);
        }
        private  void AddLeafItemFromFile(DevComponents.AdvTree.Node pNode, string DirPath)
        {
            if (Directory.Exists(DirPath))
            {
                DevComponents.AdvTree.Node tChildNode, tChildChildNode;
                // string strTblName = "";
                DirectoryInfo Dinfo = new DirectoryInfo(DirPath);
                foreach (DirectoryInfo eachinfo in Dinfo.GetDirectories())
                {
                    tChildNode = new DevComponents.AdvTree.Node();
                    tChildNode.Text = eachinfo.Name;
                    tChildNode.Name = eachinfo.FullName;
                    tChildNode.ImageIndex = 11;
                    //tChildNode.SelectedImageIndex = 12;
                    pNode.Nodes.Add(tChildNode);
                    AddLeafItemFromFile(tChildNode, eachinfo.FullName);
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.xls"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 15;
                        //tChildChildNode.SelectedImageIndex = 15;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mdb"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 18;
                        //tChildChildNode.SelectedImageIndex = 18;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                    foreach (FileInfo Finfo in eachinfo.GetFiles("*.mxd"))
                    {
                        tChildChildNode = new DevComponents.AdvTree.Node();
                        tChildChildNode.Text = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                        tChildChildNode.Name = Finfo.FullName;
                        tChildChildNode.ImageIndex = 17;
                        //tChildChildNode.SelectedImageIndex = 17;
                        tChildNode.Nodes.Add(tChildChildNode);
                    }
                }
            }

        }
    }
}
