using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public class ControlsDBHistoryManage : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
        public static bool m_bChecked;//卸载时调用 xisheng20110907

        public static ControlHistoryBar m_controlHistoryBar = null;//卸载时调用  xisheng20110907
        private DevComponents.DotNetBar.DockContainerItem m_dockItemHistoryData = null;
        private  DevComponents.DotNetBar.PanelDockContainer m_PanelTipHistoryData = null;
        private frmArcgisMapControl m_newFrmArcgisMapControl = null;
        public ControlHistoryBar MyControlHistoryBar
        {
            get
            {
                return m_controlHistoryBar;
            }
        }

        public ControlsDBHistoryManage()
        {
            base._Name = "GeoDBATool.ControlsDBHistoryManage";
            base._Caption = "历史数据管理";
            base._Tooltip = "历史数据管理";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "历史数据管理";

        }

        public bool IsHistory { get; set; }
        public override bool Enabled
        {
            get
            {
                return false;
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                //cyf 20110713  add
                if (m_Hook.MapControl.LayerCount == 0)
                {
                    m_bChecked = false;
                    UserControlDBATool userControlDBATool = ModData.v_AppGIS.MainUserControl as UserControlDBATool;
                    if (m_controlHistoryBar != null)
                    {
                        userControlDBATool.ToolControl.Controls.Remove(m_controlHistoryBar.BarHistoryManage);
                    }
                    m_controlHistoryBar = null;

                    userControlDBATool._frmBarManager.BarHistoryDataCompare.Items.Clear();
                    if (m_dockItemHistoryData == null)
                    {
                        m_dockItemHistoryData = new DevComponents.DotNetBar.DockContainerItem("dockItemHistoryData0", "历史数据对比浏览");
                        m_PanelTipHistoryData = new DevComponents.DotNetBar.PanelDockContainer();
                        m_newFrmArcgisMapControl = new frmArcgisMapControl();

                        m_newFrmArcgisMapControl.ArcGisMapControl.Dock = DockStyle.Fill;
                        m_PanelTipHistoryData.Controls.Add(m_newFrmArcgisMapControl.ArcGisMapControl);
                        m_dockItemHistoryData.Control = m_PanelTipHistoryData;
                        m_dockItemHistoryData.Width = 750;
                        m_dockItemHistoryData.Control.Width = 600;
                    }
                    userControlDBATool._frmBarManager.BarHistoryDataCompare.Items.Add(m_dockItemHistoryData);
                    userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.Controls.Remove(userControlDBATool._frmBarManager.BarHistoryDataCompare);
                    userControlDBATool._frmBarManager.BarHistoryDataCompare.Visible = false;
                }
                else
                {
                    for (int i = 0; i < m_Hook.MapControl.LayerCount; i++)
                    {
                        ILayer mLyr = m_Hook.MapControl.get_Layer(i);
                        if (mLyr is IGroupLayer)
                        {
                            for (int j = 0; j < (mLyr as ICompositeLayer).Count; j++)
                            {
                                ILayer pLayer = (mLyr as ICompositeLayer).get_Layer(j);
                                if (mLyr.Name.EndsWith("_GOH"))
                                {
                                    m_bChecked = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (mLyr.Name.EndsWith("_GOH"))
                            {
                                m_bChecked = true;
                            }
                        }
                        if (m_bChecked)
                        {
                            break;
                        }
                    }
                }
                //end
                return true;
            }
        }

        public override bool Checked
        {
            get
            {
                return m_bChecked;;
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
		//added by chulili 20110720初始化历史工具
        //现在加载历史数据的概念有所改变（以前每次都是加载所有历史，现在可以分数据集分图层加载历史）
        //(以前加载完了不能再次加载，现在可以多次加载)
        public void InitHistoryBar()
        {
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("历史数据管理", null, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("历史数据管理", null, DateTime.Now);
            }
            try
            {
                UserControlDBATool userControlDBATool = ModData.v_AppGIS.MainUserControl as UserControlDBATool;
                if (userControlDBATool == null) return;

                if (!m_bChecked)
                {
                    //cyf 20110705 modify
                    if (m_controlHistoryBar == null)
                    {
                        m_controlHistoryBar = new ControlHistoryBar(m_Hook.ArcGisMapControl, m_Hook.TOCControl, m_Hook.ProjectTree, userControlDBATool._frmBarManager.BarHistoryDataCompare, userControlDBATool._frmBarManager.MainDotNetBarManager);
                        //end
                        userControlDBATool.ToolControl.Controls.Add(m_controlHistoryBar.BarHistoryManage);
                        userControlDBATool.ToolControl.Controls.SetChildIndex(m_controlHistoryBar.BarHistoryManage, 0);

                        userControlDBATool.ToolControl.Controls.Remove(m_controlHistoryBar.BarHistoryManage);
                        userControlDBATool.ToolControl.Controls.Remove(ModData.v_AppGIS.ArcGisMapControl);
                        userControlDBATool.ToolControl.Controls.Remove(userControlDBATool.MapToolBar);
                        userControlDBATool.ToolControl.Controls.Add(ModData.v_AppGIS.ArcGisMapControl);
                        userControlDBATool.ToolControl.Controls.Add(userControlDBATool.MapToolBar);
                        userControlDBATool.ToolControl.Controls.Add(m_controlHistoryBar.BarHistoryManage);

                        userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.Controls.Add(userControlDBATool._frmBarManager.BarHistoryDataCompare);
                        userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(userControlDBATool._frmBarManager.BarHistoryDataCompare, 664, 126)))}, DevComponents.DotNetBar.eOrientation.Vertical);
                        userControlDBATool._frmBarManager.BarHistoryDataCompare.Size = new System.Drawing.Size(362, 228);
                        userControlDBATool._frmBarManager.BarHistoryDataCompare.Visible = false;
                    }
                    m_bChecked = !m_bChecked;
                }
                
            }
            catch (Exception err)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(err, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(err, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "原因:" + err.Message);
            }
        }
        //end added by chulili

        public override void OnClick()
        {
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("历史数据管理", null, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("历史数据管理", null, DateTime.Now);
            }
            try
            {
                UserControlDBATool userControlDBATool = ModData.v_AppGIS.MainUserControl as UserControlDBATool;
                if (userControlDBATool == null) return;

                if (!m_bChecked)
                {
                    //cyf 20110705 modify
                    m_controlHistoryBar = new ControlHistoryBar(m_Hook.ArcGisMapControl, m_Hook.TOCControl,m_Hook.ProjectTree, userControlDBATool._frmBarManager.BarHistoryDataCompare, userControlDBATool._frmBarManager.MainDotNetBarManager);
                   //end
                    userControlDBATool.ToolControl.Controls.Add(m_controlHistoryBar.BarHistoryManage);
                    userControlDBATool.ToolControl.Controls.SetChildIndex(m_controlHistoryBar.BarHistoryManage, 0);

                    userControlDBATool.ToolControl.Controls.Remove(m_controlHistoryBar.BarHistoryManage);
                    userControlDBATool.ToolControl.Controls.Remove(ModData.v_AppGIS.ArcGisMapControl);
                    userControlDBATool.ToolControl.Controls.Remove(userControlDBATool.MapToolBar);
                    userControlDBATool.ToolControl.Controls.Add(ModData.v_AppGIS.ArcGisMapControl);
                    userControlDBATool.ToolControl.Controls.Add(userControlDBATool.MapToolBar);
                    userControlDBATool.ToolControl.Controls.Add(m_controlHistoryBar.BarHistoryManage);

                    userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.Controls.Add(userControlDBATool._frmBarManager.BarHistoryDataCompare);
                    userControlDBATool._frmBarManager.MainDotNetBarManager.RightDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(userControlDBATool._frmBarManager.BarHistoryDataCompare, 664, 126)))}, DevComponents.DotNetBar.eOrientation.Vertical);
                    userControlDBATool._frmBarManager.BarHistoryDataCompare.Size = new System.Drawing.Size(362, 228);
                    userControlDBATool._frmBarManager.BarHistoryDataCompare.Visible = false;
                    m_bChecked = !m_bChecked;
                }
                else
                {
                    if (!IsHistory)
                    {
                        if (m_controlHistoryBar != null)    //cyf 20110713 modify
                        {
                            userControlDBATool.ToolControl.Controls.Remove(m_controlHistoryBar.BarHistoryManage);
                        }
                        m_controlHistoryBar = null;
                        // m_Hook.MapControl.ClearLayers();
                        ClearHisLayer();
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
                        m_bChecked = !m_bChecked;
                    }
                //end
                }

                
            }
            catch(Exception err)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(err, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(err, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "原因:" + err.Message);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
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
    }
}
