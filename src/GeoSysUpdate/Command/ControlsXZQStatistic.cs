using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using GeoDataCenterFunLib;
using System.Windows.Forms;
using SysCommon;

namespace GeoSysUpdate
{
    public class ControlsXZQStatistic : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQStatistic()
        {
            base._Name = "GeoSysUpdate.ControlsXZQStatistic";
            base._Caption = "行政区专题统计";
            base._Tooltip = "行政区专题统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区专题统计";
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
        {
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;
            
            //UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            //if (pUserControl != null)
            //{
            //    //切换到标准图幅tab页
            //    pUserControl.TurnToXZQTab();
            //}
            ////更新图库树
            FrmGetXZQLocation newfrm = new FrmGetXZQLocation();
            newfrm.m_DefaultMap = _hook.MapControl;
            newfrm.m_IsClose = true;
            if (newfrm.ShowDialog() != DialogResult.OK) return;
            if (newfrm.m_XZQCode == null && newfrm.m_XZQCode == "") return;
            newfrm.drawgeometryXOR(newfrm .m_pGeometry);

            DevComponents.AdvTree.Node vRootNode = new DevComponents.AdvTree.Node();
            //vRootNode.Text = ModXZQ.GetXzqName(Plugin.ModuleCommon.TmpWorkSpace, newfrm.m_XZQCode);
            //vRootNode.Name = newfrm.m_XZQCode;
            //vRootNode.Tag = "xiang";
            //_hook.XZQTree.SelectedNode = vRootNode;
            //if (_hook.XZQTree.SelectedNode.Parent != null)
            //{
            //    MessageBox.Show("", "");
            //}
            //for (int i = 0; i < _hook.XZQTree.Nodes.Count; i++)
            //{
            //    if (_hook.XZQTree.Nodes[i].Name == newfrm.m_XZQCode)
            //    {
            //        _hook.XZQTree.SelectedIndex = i;
            //        break;
            //    }
            //}
            vRootNode = GetNode(_hook.XZQTree.Nodes[0], newfrm.m_XZQCode);
            _hook.XZQTree.SelectedNode = vRootNode;
            frmXZQZTStatistical pfrmXZQZTStatistical = new frmXZQZTStatistical(_hook.XZQTree.SelectedNode);
            pfrmXZQZTStatistical.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        private DevComponents.AdvTree.Node GetNode(DevComponents.AdvTree.Node rootNode, string NodeName)
        {
            
            DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
            if (rootNode == null) return newNode;
            if (rootNode.HasChildNodes)
            {
                for (int i = 0; i < rootNode.Nodes.Count; i++)
                {
                    if (rootNode.Nodes[i].Name == NodeName)
                    {
                        newNode = rootNode.Nodes[i];
                        return newNode;
                    }
                    newNode = GetNode(rootNode.Nodes[i], NodeName);
                    if (newNode.Name == NodeName) return newNode;
                }
            }
            else
            {
                if (rootNode.Name.ToString() == NodeName)
                {
                    newNode = rootNode;
                }
            }
            return newNode;
        }
    }
}
