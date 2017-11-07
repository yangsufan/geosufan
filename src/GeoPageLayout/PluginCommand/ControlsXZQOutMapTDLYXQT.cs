using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110915
    /// 说明：单个森林资源现状辖区图
    /// </summary>
    public class ControlsXZQOutMapTDLYXQT : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsXZQOutMapTDLYXQT()
        {
            base._Name = "GeoPageLayout.ControlsXZQOutMapTDLYXQT";
            base._Caption = "森林资源现状辖区图";
            base._Tooltip = "森林资源现状辖区图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "森林资源现状辖区图";
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
                FrmSheetMapUserSet_ZTbat batZT = new FrmSheetMapUserSet_ZTbat(_AppHk.ArcGisMapControl, pAppFormRef.MainForm, xzqTree.SelectedNode.Text);
                batZT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                if (!(batZT.ShowDialog(pAppFormRef.MainForm) == DialogResult.OK))
                    return;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("出" + this._Caption);
                }
                IMap pMap = batZT.Map;
                if (pMap.LayerCount == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                    return;

                }
                string xzqdmFD ="";
                //构造晕线和渲染图层
                IFeatureClass xzqFC = ModGetData.getFCByXZQ(xzqTree.SelectedNode, ref xzqdmFD);
                if (xzqFC != null && xzqdmFD != null)
                {
                    ILayer hachureLyr = GeoPageLayoutFn.createHachureLyr(xzqFC, xzqdmFD, xzqTree.SelectedNode.Name);
                    if (hachureLyr != null)
                    {
                        IMapLayers pMapLayers=pMap as IMapLayers;
                        IGroupLayer pGroupLayer =pMap.get_Layer(0) as IGroupLayer;
                        if (pGroupLayer != null)
                        {
                            pMapLayers.InsertLayerInGroup(pGroupLayer, hachureLyr, false, 0);
                        }

                    }
                }
                GeoPageLayout pGL = new GeoPageLayout(pMap, xzqGeo, batZT.GetScale, xzqTree.SelectedNode.Text, 1);
                pGL.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pGL.typePageLayout = 5;
                pGL.MapOut();
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
