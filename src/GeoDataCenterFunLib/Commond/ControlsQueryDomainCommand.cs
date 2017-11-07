using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Error;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110730
    /// 说明：地名查询
    /// </summary>
    public class ControlsQueryDomainCommand : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        Plugin.Application.IAppFormRef _pAppForm = null;

        public ControlsQueryDomainCommand()
        {
            base._Name = "GeoDataCenterFunLib.ControlsQueryDomainCommand";
            base._Caption = "地名查询";
            base._Tooltip = "地名查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "地名查询";
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
            if (pBar.ListDataNodeKeys == null)
            {
                pBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
            }
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;
      
            string strDMFL = "", strDM = "";

            List<ILayer> pListLayers = null;
            List<IFeatureClass> pListFeatureClasses = null;
            List<string> pListLayerNames = null;
            List<string> pListNodeKeys = null;
            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace,"查询配置", ModQuery.m_QueryPath);    //added by chulili 20111110先从业务库拷贝配置文件
            try
            {//获取地名地物类
                ModQuery.GetPlaceNameQueryConfig(pMap, out pListNodeKeys,out pListLayers, out pListFeatureClasses, out pListLayerNames, out strDM);
                //检查地名字段
                if (pListFeatureClasses == null)
                {
                    MessageBox.Show("找不到地名数据,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (pListFeatureClasses.Count == 0)
                {
                    MessageBox.Show("找不到地名数据,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                for (int i = 0; i < pListFeatureClasses.Count; i++)
                {
                    IFeatureClass pTmpFeaCls = pListFeatureClasses[i];
                    if (pTmpFeaCls.FindField(strDM) < 0)
                    {
                        MessageBox.Show("找不到地名数据名称属性,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                frmQueryDomain fmQD = new frmQueryDomain(Plugin.ModuleCommon.TmpWorkSpace, _pAppForm.MainForm, _AppHk.MapControl,pListNodeKeys, pListLayers,pListFeatureClasses,pListLayerNames,strDM);
                fmQD.WriteLog = this.WriteLog;
                fmQD.QueryBar = pBar;
                fmQD.Show(_pAppForm.MainForm);
                //IQueryFilter pQF = new QueryFilterClass();
                //pQF.WhereClause = fmQD.SqlWhere;
                //frmQuery fmQ = new frmQuery(_AppHk.MapControl);
                ////用查询地物类,查询条件 填充查询窗体
                //if (fmQD._QueryTag.Equals("XZ"))
                //{
                //    fmQ.FillData(pXZFeaClass, pQF,false);
                //}
                //else
                //{
                //    fmQ.FillData(pZRFeaClass, pQF,false);
                //}
                //fmQ.Show((_AppHk as Plugin.Application.IAppFormRef).MainForm);
            }
            catch(Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
 
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _pAppForm = hook as Plugin.Application.IAppFormRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
