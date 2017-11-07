using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace GeoHistory
{
    public class ControlsAddHistoryDataBase : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        public ControlsAddHistoryDataBase()
        {
            base._Name = "GeoHistory.ControlsAddHistoryDataBase";
            base._Caption = "加载历史库数据";
            base._Tooltip = "加载历史库数据";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载服务器上历史库数据";
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                bool bres = true;
                for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
                {
                    ILayer mLayer = _AppHk.MapControl.get_Layer(i);
                    if (mLayer is IGroupLayer)
                    {
                        if (mLayer.Name == "历史库数据")
                        {
                            bres = false;
                            break;
                        }
                    }
                }
                return bres;
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
            //当前所有范围面，用来设置数据显示范围
            IFeatureLayer pRangeFeatLay = null;
            IFeatureLayer pFeatureLayer = null;
            IFeatureClass pFeatureClass = null;
            Exception exError = null;

            //如果历史库数据group图层已经存在 就不让再加了
            for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
            {
                ILayer mLayer = _AppHk.MapControl.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    if (mLayer.Name == "历史库数据")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "历史库数据图层已经存在。");
                        return;
                    }
                }
            }

            Plugin.Application.IAppFormRef pArrForm = _AppHk as Plugin.Application.IAppFormRef;
            pArrForm.MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //获取更新库体数据集
            if (_AppHk.DBXmlDocument == null)
            {
                pArrForm.MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }
            XmlNode DBNode = _AppHk.DBXmlDocument.SelectSingleNode(".//项目工程");
            if (DBNode == null)
            {
                return;
            }
            XmlElement DBElement = DBNode as XmlElement;
            XmlElement objNode = DBNode.SelectSingleNode(".//历史数据连接") as XmlElement;//yjl20120510 需改为选择历史数据连接

            SysCommon.Gis.SysGisDataSet pObjSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            pObjSysGisDataSet.SetWorkspace(objNode.GetAttribute("服务器"), objNode.GetAttribute("服务名"), objNode.GetAttribute("数据库"), objNode.GetAttribute("用户"), objNode.GetAttribute("密码"), objNode.GetAttribute("版本"), out exError);
            if (exError != null)
            {
                pArrForm.MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "历史库数据连接失败,请确认");
                return;
            }
            //获取数据显示范围
            IGeometry pGeometry = null;
            pRangeFeatLay = ModDBOperator.GetMapFrameLayer("zone", _AppHk.MapControl, "示意图") as IFeatureLayer;
            if (pRangeFeatLay == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无法获取数据显示范围！");
                return;
            }
            //判断当前是否有示提交的范围面
            IFeatureCursor pFeatureCursor = pRangeFeatLay.Search(null, false);
            if (pFeatureCursor != null)
            {
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    if (pGeometry == null)
                    {
                        pGeometry = pFeature.Shape;
                    }
                    else
                    {
                        pGeometry = (pGeometry as ITopologicalOperator).Union(pFeature.Shape);
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
            }
            if (pGeometry == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无法获取数据显示范围！");
                return;
            }
            //获取所有更新数据的FeatureClass
            IFeatureDataset pFeaDataset = pObjSysGisDataSet.GetFeatureDataset("h_njtdt", out exError);// 这个地方要素集的名称是写死的 暂时没有办法获得工作库的信息 需要修改 陈新伟 20091211
            if (pFeaDataset == null)
            {
                pArrForm.MainForm.Cursor = System.Windows.Forms.Cursors.Default;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无法获得指定的现势库数据【H_NJTDT】。");//yjl20120503 
                return;
            }

            //符号化类
            //SymbolLyr symLyr = new SymbolLyr();   //@@@

            List<IDataset> listFC = pObjSysGisDataSet.GetFeatureClass(pFeaDataset);
            IGroupLayer pLayer = new GroupLayerClass();
            pLayer.Name = "历史库数据";

            foreach (IDataset pDataset in listFC)
            {
                pFeatureClass = pDataset as IFeatureClass;
                if (pFeatureClass == null) continue;
                pFeatureLayer = new FeatureLayerClass();
                if (pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatureLayer = new FDOGraphicsLayerClass();
                }

                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pDataset.Name;
                pFeatureLayer.ScaleSymbols = false;
                //pFeatureLayer = ModDBOperator.GetSelectionLayer(pTempFeatureLayer, pGeometry);
                //if (pFeatureLayer == null) return;

                //符号化图层
                //symLyr.SymbolFeatrueLayer(pFeatureLayer);  //@@@

                //收缩图例
                if (pFeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    ModDBOperator.ExpandLegend(pFeatureLayer as ILayer, false);
                }
                pLayer.Add(pFeatureLayer as ILayer);
            }
            _AppHk.MapControl.Map.AddLayer(pLayer);
            pObjSysGisDataSet.CloseWorkspace(true);

            //对图层进行排序
            SysCommon.Gis.ModGisPub.LayersCompose(pLayer);

            //将图幅结合表置于底层
            //ModDBOperator.MoveMapFrameLayer(_AppHk.MapControl);
            _AppHk.TOCControl.Update();
            _AppHk.MapControl.Map.ClipGeometry = pGeometry;
            _AppHk.MapControl.ActiveView.Refresh();

            pArrForm.MainForm.Cursor = System.Windows.Forms.Cursors.Default;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
        }
    }
}
