using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110915
    /// 说明：单个森林资源总体规划辖区图
    /// </summary>
    public class ControlsXZQOutMapZTGHXQT : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQOutMapZTGHXQT()
        {
            base._Name = "GeoPageLayout.ControlsXZQOutMapZTGHXQT";
            base._Caption = "森林资源总体规划辖区图";
            base._Tooltip = "森林资源总体规划辖区图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "森林资源总体规划辖区图";
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
            DevComponents.AdvTree.AdvTree xzqTree = _hook.XZQTree;
            IGeometry xzqGeo = ModGetData.getExtentByXZQ(xzqTree.SelectedNode);
            if (xzqGeo == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区范围！");
                return;
            }
            try
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("出" + this._Caption);
                }
                IMap pMap = null;
                bool isSpecial = ModGetData.IsMapSpecial();
                if (isSpecial)
                {
                    pMap = new MapClass();
                    ModGetData.AddMapOfByXZQ(pMap, "ZTGH", "", _hook.ArcGisMapControl.Map, xzqTree.SelectedNode.Text);
                    if (pMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        return;
                    }
                    ModuleMap.LayersComposeEx(pMap);//图层排序
                }
                else
                {
                    IObjectCopy pOC = new ObjectCopyClass();
                    pMap = pOC.Copy(_hook.ArcGisMapControl.Map) as IMap;//复制地图
                }
                if (pMap.LayerCount == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                    return;

                }
                string xzqdmFD = "";
                //构造晕线和渲染图层
                IFeatureClass xzqFC = ModGetData.getFCByXZQ(xzqTree.SelectedNode, ref xzqdmFD);
                if (xzqFC != null && xzqdmFD != null)
                {
                    ILayer hachureLyr = GeoPageLayoutFn.createHachureLyr(xzqFC, xzqdmFD, xzqTree.SelectedNode.Name);
                    if (hachureLyr != null)
                    {
                        IMapLayers pMapLayers = pMap as IMapLayers;
                        IGroupLayer pGroupLayer = pMap.get_Layer(0) as IGroupLayer;
                        if (pGroupLayer != null)
                        {
                            pMapLayers.InsertLayerInGroup(pGroupLayer, hachureLyr, false, 0);
                        }

                    }
                }



                GeoPageLayout pGL = new GeoPageLayout();
                pGL.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pGL.pageLayoutZTGHTXQT(pMap, xzqGeo, xzqTree.SelectedNode.Text);
                
            }
            catch(Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
        #region 自定义方法
       
        #endregion
    }
}
