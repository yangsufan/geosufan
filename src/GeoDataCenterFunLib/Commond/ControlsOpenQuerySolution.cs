using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
//打开查询显示方案 ygc 2012-10-15 
namespace GeoDataCenterFunLib
{
    public class ControlsOpenQuerySolution : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        FrmSQLQuery frmSQL = null;
        public ControlsOpenQuerySolution()
        {
            base._Name = "GeoDataCenterFunLib.ControlsOpenQuerySolution";
            base._Caption = "打开查询方案";
            base._Tooltip = "打开查询方案";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "打开查询方案";
            //base._Image = "";
            //base._Category = "";
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
        public override bool Enabled
        {
            /*  get
              {
                  //不存在图幅结合表图层、数据操作进程正在进行时不可用
                  if (_AppHk.MapControl == null) return false;
                  if (_AppHk.MapControl.Map.LayerCount == 0) return false;
                  return true;
              }*/
            get
            {
                try
                {
                    if (_AppHk.CurrentControl is ISceneControl) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override void OnClick()
        {
            Plugin.Application.IAppGisUpdateRef phook = _AppHk as Plugin.Application.IAppGisUpdateRef;
            SysCommon.BottomQueryBar pBar = phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;
            if (frmSQL != null)
            {
                frmSQL.Close();
                frmSQL = null;
            }
            if (WriteLog)
            {
                Plugin.LogTable.Writelog("打开查询方案"); //ygc 2012-9-14 写日志
            }
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);

            vProgress.ShowDescription = false;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            //vProgress.ShowProgress();
            vProgress.SetProgress("开始查询");
            FrmOpenSQLCondition newfrm = new FrmOpenSQLCondition(Plugin.ModuleCommon.TmpWorkSpace);
            newfrm.m_TableName = "SQLSOLUTION";
            newfrm.m_showAllLayer = true;
            if (newfrm.ShowDialog() != DialogResult.OK) return;
            vProgress.ShowProgress();
            string strSQL = newfrm.m_Condition;
            string layerName = newfrm.m_LayerName;
            if (strSQL == "")
            {
                MessageBox.Show("未选择查看的查询方案！","提示",MessageBoxButtons .OKCancel ,MessageBoxIcon.Stop);
                return;
            }
            IFeatureLayer pFeatureLayer = GetLayerByName(layerName, pMap);
            if (pFeatureLayer == null)
            {
                MessageBox.Show("当前地图无该查询方案图层！","提示");
                return;
            }
            //构造查询过滤器
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strSQL;
            esriSelectionResultEnum pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;
            vProgress.SetProgress("正在查询符合条件的结果");
            pBar.m_pMapControl = _AppHk.MapControl;
            pBar.EmergeQueryData(_AppHk.MapControl.Map, pFeatureLayer, pQueryFilter, pSelectionResult, vProgress);
            vProgress.Close();

            try
            {
                DevComponents.DotNetBar.Bar pBar0 = pBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                if (pBar0 != null)
                {
                    pBar0.AutoHide = false;
                    //pBar0.SelectedDockTab = 1;
                    int tmpindex = pBar0.Items.IndexOf("dockItemDataCheck");
                    pBar0.SelectedDockTab = tmpindex;
                }
            }
            catch
            { }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
        private IFeatureLayer GetLayerByName(string layerName, IMap Pmap)
        {
            IFeatureLayer pFeatureLayer = null;
            if (Pmap.LayerCount == 0) return null;
            for (int i = 0; i < Pmap.LayerCount; i++)
            {
                ILayer tempLayer = Pmap.get_Layer(i);
                if (tempLayer.Name == layerName)
                {
                    pFeatureLayer = tempLayer as IFeatureLayer;
                    break;
                }
            }
            return pFeatureLayer;
        }
    }
}
