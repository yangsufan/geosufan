using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using GeoDataCenterFunLib;
using System.Xml;
using ESRI.ArcGIS.Controls;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Error;
using ESRI.ArcGIS.Geometry;
using SysCommon;
namespace GeoDataManagerFrame
{
    public class CommandFunctionZoneStatistic : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public CommandFunctionZoneStatistic()
        {
            base._Name = "GeoDataManagerFrame.CommandFunctionZoneStatistic";
            base._Caption = "功能分区统计";
            base._Tooltip = "功能分区统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "功能分区统计";
        }
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook.CurrentControl is ISceneControl) return false;
                    if (m_Hook.MapControl.LayerCount == 0)
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
            if (m_Hook == null)
                return;
            if (m_Hook.MapControl == null)
                return;
            if (m_Hook.MapControl.Map == null)
                return;
            IMap pMap = m_Hook.MapControl.Map;
            if (pMap.LayerCount == 0)
                return;
            string strZLDJ = "";
            IFeatureClass pZLDJFeaClass = null;
            string ZLDJLayerName = "";
            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace, "统计配置", ModQuery.m_StatisticsPath);    //added by chulili 20111110先从业务库拷贝配置文件
            try
            {//获取地名地物类
                GeoDataCenterFunLib.ModQuery.GetPlaceNameStatisticsConfig(out pZLDJFeaClass, out ZLDJLayerName, out strZLDJ, "主导功能区分布");
                if (pZLDJFeaClass == null)
                {
                    MessageBox.Show("林地图斑数据,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (pZLDJFeaClass != null)
                {
                    if (pZLDJFeaClass.FindField(strZLDJ) < 0)
                    {
                        MessageBox.Show("找不到林地图斑数据主导功能区属性,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        pZLDJFeaClass = null;
                        return;
                    }
                }
                frmSetStatistic pfrmSetStatistic = new frmSetStatistic();
                if (pfrmSetStatistic.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel pExportToExcel = new ExportToExcel();
                    IFeatureCursor pFeatureCursor = null;
                    if (pfrmSetStatistic.bIsCheck)
                    {
                        if (pMap.SelectionCount == 0)
                        {
                            MessageBox.Show("请在当前地图上选择林地图斑数!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        //IFeatureLayer pFeatureLayer = GetLayerByName(ZLDJLayerName, pMap) as IFeatureLayer;
                        //if (pFeatureLayer == null)
                        //{
                        //    MessageBox.Show("找不到林地图斑数据,请先加载该数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        //IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;
                        //ISelectionSet pFeatSet = pFeatureSelection.SelectionSet;

                        //pFeatSet.Search(null, true, out pCursor);
                        //pFeatureCursor = pCursor as IFeatureCursor;
                        IGeometry pGeo = ModDBOperate.GetSelectFeatureGeom(pMap);
                        ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                        pSpatialFilter.GeometryField = "SHAPE";
                        pSpatialFilter.Geometry = pGeo;
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pFeatureCursor = pZLDJFeaClass.Search(pSpatialFilter, false);

                    }
                    else
                    {
                        pFeatureCursor = pZLDJFeaClass.Search(null, false);
                    }
                    pExportToExcel.Export(pZLDJFeaClass, pFeatureCursor, "主导功能区分布", "主导功能区", strZLDJ);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("专题图统计-主导功能区统计");//ygc 2012-9-14 写日志
                }

            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }



        }
        private ILayer GetLayerByName(string sName, IMap pmap)
        {
            ILayer pLayer;
            for (int i = 0; i < pmap.LayerCount; i++)
            {
                pLayer = pmap.get_Layer(i);
                if (pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        pLayer = pCLayer.get_Layer(j);
                        if (pLayer is IFeatureLayer)
                        {
                            if (pLayer.Name == sName) return pLayer;
                        }
                    }
                }
                else if (pLayer is IFeatureLayer)
                {
                    if (pLayer.Name == sName) return pLayer;
                }
            }
            return null;
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
