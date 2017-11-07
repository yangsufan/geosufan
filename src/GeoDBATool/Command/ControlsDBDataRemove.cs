using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace GeoDBATool
{
    /// <summary>
    /// 卸载数据
    /// </summary>
    public class ControlsDBDataRemove : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        ILayer v_Layer = null;

        public ControlsDBDataRemove()
        {
            base._Name = "GeoDBATool.ControlsDBDataRemove";
            base._Caption = "卸载";
            base._Tooltip = "卸载数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "卸载数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110625
                //if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "现势库" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "历史库" || m_Hook.ProjectTree.SelectedNode.DataKeyString=="栅格数据库")) return false;
                if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RC" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RD")) return false;
                //end
                if (m_Hook.MapControl == null) return false;
                DevComponents.AdvTree.Node ProjectNode = m_Hook.ProjectTree.SelectedNode;
                while (ProjectNode.Parent != null)
                {
                    ProjectNode = ProjectNode.Parent;
                }
                //cyf 20110625 modify 
                ILayer player = null;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC")
                {
                    player = ModDBOperator.GetLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.Text );
                }
                else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RD" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RC")
                {
                    player = ModDBOperator.GetGroupLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.Text + "_" + ProjectNode.Text);
                }
                if (player == null)
                {
                    return false;
                }
                v_Layer = player;
                //end
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            /*针对历史库卸载修改，之前没有针对历史库卸载 xisheng 20110907 */
            DevComponents.AdvTree.Node ProjectNode = m_Hook.ProjectTree.SelectedNode;
            while (ProjectNode.Parent != null)
            {
                ProjectNode = ProjectNode.Parent;
            }

            DevComponents.AdvTree.Node DBNode = m_Hook.ProjectTree.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode.Text == "现势库" || DBNode.Text == "临时库")
            {
                //cyf 20110625 modify 
                //ILayer player = null;
                //player= ModDBOperator.GetGroupLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.Text + "_" + ProjectNode.Text);
                //end
                if (v_Layer != null)
                {
                    m_Hook.MapControl.Map.DeleteLayer(v_Layer);
                    m_Hook.TOCControl.Update();
                }
            }
            else
            {
                UserControlDBATool userControlDBATool = ModData.v_AppGIS.MainUserControl as UserControlDBATool;
                if (userControlDBATool == null) return;
                if (ControlsDBHistoryManage.m_controlHistoryBar != null)    //cyf 20110713 modify
                {
                    userControlDBATool.ToolControl.Controls.Remove(ControlsDBHistoryManage.m_controlHistoryBar.BarHistoryManage);
                }
                ControlsDBHistoryManage.m_controlHistoryBar = null;
                // m_Hook.MapControl.ClearLayers();
                //ClearHisLayer();

                m_Hook.MapControl.ClearLayers();
                m_Hook.TOCControl.Update();

                userControlDBATool._frmBarManager.BarHistoryDataCompare.Items.Clear();
                DevComponents.DotNetBar.DockContainerItem dockItemHistoryData = new DevComponents.DotNetBar.DockContainerItem("dockItemHistoryData0", "历史数据对比浏览");
                DevComponents.DotNetBar.PanelDockContainer PanelTipHistoryData = new DevComponents.DotNetBar.PanelDockContainer();
                frmArcgisMapControl newFrmArcgisMapControl = new frmArcgisMapControl();
                newFrmArcgisMapControl.ArcGisMapControl.Dock = DockStyle.Fill;
                PanelTipHistoryData.Controls.Add(newFrmArcgisMapControl.ArcGisMapControl);
                dockItemHistoryData.Control = PanelTipHistoryData;
                userControlDBATool._frmBarManager.BarHistoryDataCompare.Items.Add(dockItemHistoryData);
                userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.Controls.Remove(userControlDBATool._frmBarManager.BarHistoryDataCompare);
                userControlDBATool._frmBarManager.BarHistoryDataCompare.Visible = false;
                ControlsDBHistoryManage.m_bChecked = false;
            }
          /*end*************************************************************************/
        }

        /// <summary>
        /// 清除历史数据库数据图层
        /// </summary>
        private void ClearHisLayer()
        {
            if (m_Hook.MapControl.LayerCount <= 0) return;
            for (int i = 0; i < m_Hook.MapControl.LayerCount; i++)
            {
                ILayer getLayer = m_Hook.MapControl.get_Layer(i);
                IGroupLayer getGrouLayer = getLayer as IGroupLayer;
                if (getGrouLayer != null)
                {
                    continue;
                }
                else
                {
                    m_Hook.MapControl.DeleteLayer(i);
                    i = i - 1;
                }
            }

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
