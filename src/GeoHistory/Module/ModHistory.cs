using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GeoHistory
{
    public static class ModHistory
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        /// <summary>
        /// //设置前台窗体
        /// </summary>
        public static extern bool SetForegroundWindow(IntPtr hWnd); //WINAPI 设置当前活动窗体的句柄
        //public static void ZoomToCaseArea(IFeatureClass pFeatureClass, IMap pMap, string CaseID)
        //{
        //    try
        //    {
        //        //查找案件对应的案件Feature
        //        IQueryFilter pQueryFilt = new QueryFilterClass();
        //        if (CaseID != "")
        //        {
        //            IGeometry pGeometry = null;
        //            pQueryFilt.WhereClause = "PRJID='" + CaseID + "'";

        //            if (pFeatureClass == null) return;
        //            IFeatureCursor pQueryCursor = pFeatureClass.Search(pQueryFilt, false);
        //            IFeature pFeature = pQueryCursor.NextFeature();

        //            while (pFeature != null)
        //            {
        //                if (pGeometry == null)
        //                {
        //                    pGeometry = pFeature.Shape;
        //                }
        //                else
        //                {
        //                    pGeometry = (pGeometry as ITopologicalOperator).Union(pFeature.Shape);
        //                }
        //                pFeature = pQueryCursor.NextFeature();

        //            }
        //            IActiveView pActiveView = (IActiveView)pMap;
        //            ZoomToFeature(pGeometry, pActiveView);
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("案件编号" + CaseID + "范围为空", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
        //private static void ZoomToFeature(IGeometry pGeometry, IActiveView pActiveView)
        //{

        //    if (pGeometry != null)
        //    {
        //        IEnvelope pEnvelope = pGeometry.Envelope;
        //        if (pEnvelope.Width == 0 || pEnvelope.Height == 0)
        //        {
        //            IPoint pPnt = new PointClass();
        //            pPnt.PutCoords(pEnvelope.XMax, pEnvelope.YMax);
        //            ITopologicalOperator pTopo = (ITopologicalOperator)pPnt;
        //            if (pTopo != null)
        //            {
        //                pGeometry = pTopo.Buffer(10);
        //                pEnvelope = pGeometry.Envelope;
        //            }
        //        }

        //        pEnvelope.Expand(1.5, 1.5, true);
        //        pActiveView.Extent = pEnvelope;
        //        pActiveView.Refresh();
        //    }
        //}
        ////定义地图图层的表达式,历史日期点调用
        public static void SetMapLyrsDefinitionOfHPoint(string in_sQueryDate, AxMapControl AxMapCtrlHis)
        {
            //ModData.v_SysDataSet.WorkSpace.ExecuteSQL("alter session set nls_date_format = 'yyyy-mm-dd'");
            string sDefinitionDate = "QSRQ<=TO_DATE('" + in_sQueryDate + "','YYYY-MM-DD HH24:MI:SS') AND ZZRQ>TO_DATE('" + in_sQueryDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinitionDateEq = "QSRQ=ZZRQ AND QSRQ=TO_DATE('" + in_sQueryDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinition = "(" + sDefinitionDate + ") OR (" + sDefinitionDateEq + ")";
            IMap pMap = AxMapCtrlHis.Map;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLyr = pMap.get_Layer(i);

                if (pLyr.Valid && pLyr.Visible)
                {

                    setGpLyrsDefinitionOfHPoint(sDefinition, pLyr);

                }

            }
            (pMap as IActiveView).Refresh();
            Label lblDate = AxMapCtrlHis.Tag as Label;
            if (lblDate == null) return;
            lblDate.Text = "日期点：" + in_sQueryDate;
            lblDate.ForeColor = Color.Red;
            lblDate.Invalidate();
            Application.DoEvents();
        }
        ////定义地图图层的表达式,针对组图层二级分组
        public static void setGpLyrsDefinitionOfHPoint(string in_sDefinition, ILayer in_Layer)
        {
            //ICompositeLayer
            IGroupLayer pTmpGroupLyr = in_Layer as IGroupLayer;
            IFeatureLayer pTmpFeaLayer = in_Layer as IFeatureLayer;
            if (pTmpGroupLyr != null)
            {
                ICompositeLayer pComLayer = pTmpGroupLyr as ICompositeLayer;
                for (int i = 0; i < pComLayer.Count ; i++)
                {
                    ILayer pLyr = pComLayer.get_Layer(i);
                    if (pLyr.Valid && pLyr.Visible)
                    {
                        if (pLyr is IFeatureLayer)
                        {
                            ILayerFields pLyrFds = pLyr as ILayerFields;
                            int idxValidDate = pLyrFds.FindField("QSRQ"),
                                idxInvalidDate = pLyrFds.FindField("ZZRQ");
                            if (idxInvalidDate != -1 && idxValidDate != -1)
                            {
                                IFeatureLayerDefinition2 pPFLD2 = pLyr as IFeatureLayerDefinition2;
                                pPFLD2.DefinitionExpression = in_sDefinition;
                            }
                        }
                    }
                }
                return;
            }              
            if (pTmpFeaLayer != null)
            {
                ILayerFields pLyrFds = pTmpFeaLayer as ILayerFields;
                int idxValidDate = pLyrFds.FindField("QSRQ"),
                    idxInvalidDate = pLyrFds.FindField("ZZRQ");
                if (idxInvalidDate != -1 && idxValidDate != -1)
                {
                    IFeatureLayerDefinition2 pPFLD2 = in_Layer as IFeatureLayerDefinition2;
                    pPFLD2.DefinitionExpression = in_sDefinition;
                }
            }
        }
       
    }
}
