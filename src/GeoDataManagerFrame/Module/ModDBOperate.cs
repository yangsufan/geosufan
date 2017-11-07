using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataManagerFrame
{
    // *===============================================
    // *类功能： 静态公共函数
    // *开发者： 陈亚飞
    // *时  间： 201105118
    // *==============================================
    public static class ModDBOperate
    {

        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Plugin.ModuleCommon.DicTabs == null || Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Plugin.Interface.IControlRef> keyValue in Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevComponents.DotNetBar.RibbonTabItem, string> keyValue in Plugin.ModuleCommon.DicTabs)
            {
                if (keyValue.Value == pSysName)
                {
                    i = i + 1;
                    keyValue.Key.Visible = true;
                    keyValue.Key.Enabled = true;
                    if (i == 1)
                    {
                        //默认选中第一项
                        keyValue.Key.Checked = true;
                    }
                }
                else
                {
                    keyValue.Key.Visible = false;
                    keyValue.Key.Enabled = false;
                }
            }
        }
        public static IGeometry GetSelectFeatureGeom(IMap pMap)
        {
            try
            {
                object obj = System.Reflection.Missing.Value;
                IGeometryBag pGeometryBag = new GeometryBagClass();
                IGeometryCollection pGeomtryCol = (IGeometryCollection)pGeometryBag;
                ISelection pSelection = pMap.FeatureSelection;
                IEnumFeature pEnumFea = pSelection as IEnumFeature;
                IFeature pFea = pEnumFea.Next();
                while (pFea != null)
                {
                    if (pFea.Shape != null && pFea.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        pGeomtryCol.AddGeometry(pFea.Shape, ref obj, ref obj);
                    }
                    pFea = pEnumFea.Next();
                }
                ITopologicalOperator pTopo = new PolygonClass();
                pTopo.ConstructUnion(pGeomtryCol as IEnumGeometry);
                IGeometry pGeometry = pTopo as IGeometry;
                return pGeometry;
            }
            catch { return null; }
        }
    }
}
