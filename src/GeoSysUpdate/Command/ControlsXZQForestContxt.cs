using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoSysUpdate
{
    public class ControlsXZQForestContxt : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQForestContxt()
        {
            base._Name = "GeoSysUpdate.ControlsXZQForestContxt";
            base._Caption = "查询小班";
            base._Tooltip = "查询小班";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查询小班";
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
            IMap pMap = null;
            try
            {
                pMap=_hook.MapControl.Map;
            }
            catch
            { }
            UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            
            DevComponents.AdvTree.AdvTree pTree = pUserControl.XZQTree;
            DevComponents.AdvTree.Node pNode = pTree.SelectedNode;
            IGeometry pGeometry = pUserControl.getExtentByXZQ(pNode);
            QueryForestByAttri(pNode, pMap);
            //QueryForestByGeometry(pGeometry,pMap);
        }
        private void QueryForestByAttri(DevComponents.AdvTree.Node pXZQnode,IMap pMap)
        {
            if (pXZQnode == null)
            {
                return;
            }
            //加进度条 xisheng 2011.06.28
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("开始查询");
            try
            {
                string strTag = pXZQnode.Tag.ToString();
                string strCode = pXZQnode.Name;
                if (strTag.ToUpper() == "TOWN")
                {
                    DevComponents.AdvTree.Node pParentNode = pXZQnode.Parent;
                    string strTmpCode = pParentNode.Name;
                    strCode = strTmpCode + strCode.Substring(1, 2);
                }
                string strNodeKey = SysCommon.ModSysSetting.GetLinBanLayerNodeKey(Plugin.ModuleCommon.TmpWorkSpace);
                ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(null, pMap, strNodeKey, null, true);
                SysCommon.BottomQueryBar pQueryBar = _hook.QueryBar;
                if (pQueryBar.m_WorkSpace == null)
                {
                    pQueryBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
                }
                if (pQueryBar.ListDataNodeKeys == null)
                {
                    pQueryBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
                }
                esriSelectionResultEnum pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;
                //构造查询过滤器
                IQueryFilter pQueryFilter = new QueryFilterClass();
                //ygc 20130326 根据山西数据格式修改条件合成方式
                if (strTag.ToUpper() == "TOWN")
                {
                    pQueryFilter.WhereClause = "xiang ='" + strCode + "00'";
                }
                else if (strTag.ToUpper() == "County")
                {
                    pQueryFilter.WhereClause = "XIANG = '" + strCode + "'";
                }
                else if (strTag.ToUpper() == "City")
                {
                    pQueryFilter.WhereClause = "shi = '" + strCode + "00'";
                }
                else
                {

                    pQueryFilter.WhereClause = "XIANG like '" + strCode + "%'";
                }

                pQueryBar.m_pMapControl = _hook.MapControl;
                vProgress.SetProgress("获取查询结果...");
                pQueryBar.EmergeQueryData(pLayer as IFeatureLayer, pQueryFilter,vProgress);
                vProgress.Close();
                try
                {
                    DevComponents.DotNetBar.Bar pBar = pQueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                    if (pBar != null)
                    {
                        pBar.AutoHide = false;
                        //pBar.SelectedDockTab = 1;
                        int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                        pBar.SelectedDockTab = tmpindex;
                    }
                }
                catch
                { }
            }
            catch
            {
                vProgress.Close();
            }
            vProgress = null;

        }
        private void QueryForestByGeometry(IGeometry pGeometry, IMap pMap)
        {
            //加进度条 xisheng 2011.06.28
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("开始查询");
            try
            {
                string strNodeKey = SysCommon.ModSysSetting.GetLinBanLayerNodeKey(Plugin.ModuleCommon.TmpWorkSpace);
                ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(null, pMap, strNodeKey, null, true);
                SysCommon.BottomQueryBar pQueryBar = _hook.QueryBar;
                if (pQueryBar.m_WorkSpace == null)
                {
                    pQueryBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
                }
                if (pQueryBar.ListDataNodeKeys == null)
                {
                    pQueryBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
                }
                pQueryBar.m_pMapControl = _hook.MapControl;
                //构造查询过滤器
                ISpatialFilter pQueryFilter = new SpatialFilterClass();
                pQueryFilter.Geometry = pGeometry;
                pQueryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                esriSelectionResultEnum pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;

                pQueryBar.m_pMapControl = _hook.MapControl;
                vProgress.SetProgress("获取查询结果...");
                pQueryBar.EmergeQueryData(pLayer as IFeatureLayer, pQueryFilter, vProgress);
                vProgress.Close();
                //pQueryBar.EmergeQueryData(pMap, pGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
                try
                {
                    DevComponents.DotNetBar.Bar pBar = pQueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                    if (pBar != null)
                    {
                        pBar.AutoHide = false;
                        //pBar.SelectedDockTab = 1;
                        int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                        pBar.SelectedDockTab = tmpindex;
                    }
                }
                catch
                { }
            }
            catch
            {
                vProgress.Close();
            }
            vProgress = null;

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
