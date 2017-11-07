using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace GeoHistory.Control
{
    public partial class UCHistoryPoint : UserControl
    {
        private string queryDate = "";
        public AxMapControl AxMapCtrlHis = null;

        public UCHistoryPoint()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            queryDate = dTimePost.Value.ToString("yyyy-MM-dd HH:mm:ss");
            setMapLyrsDefinitionOfHPoint(queryDate);
        }
        //定义地图图层的表达式,历史日期点调用
        private void setMapLyrsDefinitionOfHPoint(string in_sQueryDate)
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
                    //setGpLyrsDefinitionOfHPoint(sDefinition, pLyr);
                    ModHistory.setGpLyrsDefinitionOfHPoint(sDefinition, pLyr);
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
        //private void setGpLyrsDefinitionOfHPoint(string in_sDefinition, ILayer in_Layer)
        //{

        //    //ICompositeLayer
        //    IGroupLayer pTmpGroupLyr = in_Layer as IGroupLayer;
        //    IFeatureLayer pTmpFeaLayer = in_Layer as IFeatureLayer;
        //    if (pTmpGroupLyr != null)
        //    {
        //        ICompositeLayer pComLayer = pTmpGroupLyr as ICompositeLayer;
        //        for (int i = 0; i < pComLayer.Count; i++)
        //        {
        //            ILayer pLyr = pComLayer.get_Layer(i);
        //            if (pLyr.Valid && pLyr.Visible)
        //            {
        //                if (pLyr is IFeatureLayer)
        //                {
        //                    ILayerFields pLyrFds = pLyr as ILayerFields;
        //                    int idxValidDate = pLyrFds.FindField("QSRQ"),
        //                        idxInvalidDate = pLyrFds.FindField("ZZRQ");
        //                    if (idxInvalidDate != -1 && idxValidDate != -1)
        //                    {
        //                        IFeatureLayerDefinition2 pPFLD2 = pLyr as IFeatureLayerDefinition2;
        //                        pPFLD2.DefinitionExpression = in_sDefinition;
        //                    }
        //                }
        //            }
        //        }
        //        return;
        //    }
        //    if (pTmpFeaLayer != null)
        //    {
        //        ILayerFields pLyrFds = pTmpFeaLayer as ILayerFields;
        //        int idxValidDate = pLyrFds.FindField("QSRQ"),
        //            idxInvalidDate = pLyrFds.FindField("ZZRQ");
        //        if (idxInvalidDate != -1 && idxValidDate != -1)
        //        {
        //            IFeatureLayerDefinition2 pPFLD2 = in_Layer as IFeatureLayerDefinition2;
        //            pPFLD2.DefinitionExpression = in_sDefinition;
        //        }
        //    }
        //}
    }
}
