using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

namespace GeoHistory.Control
{
    public partial class UCHistorySegment : UserControl
    {
        public IGeometry QueryGeometry = null;
        public IFeatureClass PrjEnvelope = null;
        public AxMapControl AxMapCtrlHis = null;
        private IElement pEleEnv = null;//范围面
        private IGraphicsContainer pGra = null;
        private string m_sCurPrjName = "";
        public UCHistorySegment()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            QueryGeometry = AxMapCtrlHis.Extent;
            string sSQL = createSql();
            //RefreshQueryGrid(sSQL);
            pGra = AxMapCtrlHis.Map as IGraphicsContainer;
            if (pEleEnv != null)
            {
                try
                {
                    pGra.DeleteElement(pEleEnv);
                }
                catch { }
            }
            pEleEnv = null;
            //DataGridViewRow row = dataGridViewX1.Rows[e.RowIndex];
            //if (row == null) return;
            //if (row.Tag == null) return;
            //string sPrjID = row.Tag.ToString();

            //m_sCurPrjName = row.Cells["ColPrjName"].Value.ToString();

            string sDataUpDate = this.dTimePost.Value.ToString("yyyy-MM-dd HH:mm:ss");
            
            setMapLyrsDefinitionOfHPeriod( sDataUpDate);

            //ModHistory.ZoomToCaseArea(this.PrjEnvelope, AxMapCtrlHis.Map, sPrjID);

            //pEleEnv = new LineElementClass();
            //IGeometry pGeo = getEnvGeometry(sPrjID);
            //ITopologicalOperator pTO = pGeo as ITopologicalOperator;
            //IGeometry pOutLine = pTO.Boundary;
            //if (pGeo == null)
            //    return;
            //pEleEnv.Geometry = pOutLine;
            //(pEleEnv as ILineElement).Symbol = getSymbol();
            //pGra.AddElement(pEleEnv, 0);
            //AxMapCtrlHis.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        private void RefreshQueryGrid(string sSQL)
        {
            //if (sSQL == "")
            //    return;
            //IFeatureWorkspace pWorkFW = null;//@@@ ModData.v_SysDataSet.WorkSpace as IFeatureWorkspace;
            //ITable pPrjTable = pWorkFW.OpenTable("PROJECT");
            //List<string> lstFields = new List<string>();
            ////lstFields.Add("OBJECTID");
            ////lstFields.Add("NAME");
            ////lstFields.Add("END_TIME");

            //DataTable pPrjDT = ModDBOperator.GetTable(pPrjTable, sSQL, lstFields, GeoHistory.ModDBOperator.enumMetaType.ProjectInfo,true);
            //foreach (DataRow dr in pPrjDT.Rows)
            //{
            //    int idx=dataGridViewX1.Rows.Add(new object[]{dr["NAME"],dr["END_TIME"]});
            //    DataGridViewRow dgvr = dataGridViewX1.Rows[idx];
            //    dgvr.Tag = dr["OBJECTID"];   
            //}
        }
        private void dataGridViewX1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            pGra = AxMapCtrlHis.Map as IGraphicsContainer;
            if (pEleEnv != null)
            {
                try
                {
                    pGra.DeleteElement(pEleEnv);
                }
                catch { }
            }
            pEleEnv = null;
            DataGridViewRow row = dataGridViewX1.Rows[e.RowIndex];
            if (row == null) return;
            if (row.Tag == null) return;
            string sPrjID = row.Tag.ToString();

            m_sCurPrjName = row.Cells["ColPrjName"].Value.ToString();
            string sDataUpDate = Convert.ToDateTime(row.Cells["ColUpdateTime"].Value).ToString("yyyy-MM-dd HH:mm:ss");
            setMapLyrsDefinitionOfHPeriod(sPrjID, sDataUpDate);

            //ModHistory.ZoomToCaseArea(this.PrjEnvelope, AxMapCtrlHis.Map, sPrjID);

            pEleEnv = new LineElementClass();
            IGeometry pGeo = getEnvGeometry(sPrjID);
            ITopologicalOperator pTO = pGeo as ITopologicalOperator;
            IGeometry pOutLine = pTO.Boundary;
            if (pGeo == null)
                return;
            pEleEnv.Geometry = pOutLine;
            (pEleEnv as ILineElement).Symbol = getSymbol();
            pGra.AddElement(pEleEnv, 0);
            AxMapCtrlHis.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        //获得范围面
        private IGeometry getEnvGeometry(string in_sPrjID)
        {
            IGeometry res = null;
            int idxPrjID = this.PrjEnvelope.FindField("PRJID");
            if (idxPrjID != -1)
            {
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "PRJID='" + in_sPrjID + "'";
                IFeatureCursor pCursor = this.PrjEnvelope.Search(pQF, false);
                IFeature pFeature = pCursor.NextFeature();
                while (pFeature != null)
                {
                    if (res == null)
                    {
                        res = pFeature.ShapeCopy;
                    }
                    else
                    {
                        res = (res as ITopologicalOperator).Union(pFeature.Shape);
                    }
                   
                    pFeature = pCursor.NextFeature();
                }

            }
            return res;
        }
        private string createSql()
        {
            //ModData.v_SysDataSet.WorkSpace.ExecuteSQL("alter session set nls_date_format = 'yyyy-mm-dd'");
            //IFeatureWorkspace pWorkFW = null;   //@@@ModData.v_SysDataSet.WorkSpace as IFeatureWorkspace;
            string sSql = "TO_DATE('" + dTimePost.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS') <=END_TIME";
            sSql += " AND END_TIME<=" + "TO_DATE('" + dTimePostEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS')";
            //string sSql2 = " AND OBJECTID IN(";
            //PrjEnvelope = pWorkFW.OpenFeatureClass("zone");     
            //int idxPrjID = PrjEnvelope.FindField("OBJECTID");
            //if (idxPrjID != -1)
            //{
            //    ISpatialFilter pSF = new SpatialFilterClass();
            //    pSF.Geometry = QueryGeometry;
            //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //    IFeatureCursor pCursor = PrjEnvelope.Search(pSF, false);
            //    IFeature pFeature = pCursor.NextFeature();
            //    while (pFeature != null)
            //    {

            //        string val = pFeature.get_Value(idxPrjID).ToString();
            //        if (val != "")
            //        {
            //            sSql2 += "'" + val + "',";
            //        }

            //        pFeature = pCursor.NextFeature();

            //    }

            //}
            //if (sSql2.EndsWith(","))
            //    sSql2 = sSql2.Substring(0, sSql2.Length - 1);
            //sSql2 += ")";
            //if (sSql2 == " AND OBJECTID IN()")
            //    return "";
            return sSql;// +sSql2;
        }

        //定义地图图层的表达式,历史日期段调用
        private void setMapLyrsDefinitionOfHPeriod(string in_sPrjID, string in_sDataUpDate)
        {
            if (in_sDataUpDate == "" || in_sPrjID == "")
                return;
            //ModData.v_SysDataSet.WorkSpace.ExecuteSQL("alter session set nls_date_format = 'yyyy-mm-dd'");
            string sDefinitionPrjID = "项目ID='" + in_sPrjID + "'";
            string sDefinitionDate = "QSRQ<=TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS') AND ZZRQ>TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinitionDateEq = "QSRQ=ZZRQ AND QSRQ=TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinition = "(" + sDefinitionPrjID + ") AND (" + sDefinitionDate + ") OR (" + sDefinitionDateEq + ")";
            IMap pMap = AxMapCtrlHis.Map;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLyr = pMap.get_Layer(i);

                if (pLyr.Valid && pLyr.Visible)
                {

                    //setGpLyrsDefinitionOfHPeriod(sDefinition, pLyr);
                    ModHistory.setGpLyrsDefinitionOfHPoint(sDefinition, pLyr);

                }

            }
            (pMap as IActiveView).Refresh();
            Label lblDate = AxMapCtrlHis.Tag as Label;
            if (lblDate == null) return;
            lblDate.Text = "项目ID：" + in_sPrjID;
           
            { lblDate.Text += "，项目名称：" +m_sCurPrjName; }
            lblDate.ForeColor = Color.Red;
            lblDate.Invalidate();
            Application.DoEvents();
        }
        //定义地图图层的表达式,历史日期段调用---不含项目信息（褚丽丽修改 2012-05-23）
        private void setMapLyrsDefinitionOfHPeriod(string in_sDataUpDate)
        {
            if (in_sDataUpDate == "")
                return;
            //ModData.v_SysDataSet.WorkSpace.ExecuteSQL("alter session set nls_date_format = 'yyyy-mm-dd'");
            string sDefinitionDate = "QSRQ<=TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS') AND ZZRQ>TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinitionDateEq = "QSRQ=ZZRQ AND QSRQ=TO_DATE('" + in_sDataUpDate + "','YYYY-MM-DD HH24:MI:SS')";
            string sDefinition = "(" + sDefinitionDate + ") OR (" + sDefinitionDateEq + ")";
            IMap pMap = AxMapCtrlHis.Map;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLyr = pMap.get_Layer(i);

                if (pLyr.Valid && pLyr.Visible)
                {

                    //setGpLyrsDefinitionOfHPeriod(sDefinition, pLyr);
                    ModHistory.setGpLyrsDefinitionOfHPoint(sDefinition, pLyr);

                }

            }
            (pMap as IActiveView).Refresh();
            Label lblDate = AxMapCtrlHis.Tag as Label;
            if (lblDate == null) return;
            //lblDate.Text = "项目ID：" + in_sPrjID;

            //{ lblDate.Text += "，项目名称：" + m_sCurPrjName; }
            lblDate.ForeColor = Color.Red;
            lblDate.Invalidate();
            Application.DoEvents();
        }
        ////定义地图图层的表达式,针对组图层二级分组
        //private void setGpLyrsDefinitionOfHPeriod(string in_sDefinition, ILayer in_pLyr)
        //{
        //    //ICompositeLayer
        //    IGroupLayer pTmpGroupLayer = in_pLyr as IGroupLayer;
        //    IFeatureLayer pTmpFeaLayer = in_pLyr as IFeatureLayer;
        //    if (pTmpGroupLayer != null)
        //    {
        //        ICompositeLayer pComLayer = pTmpGroupLayer as ICompositeLayer;
        //        for (int i = 0; i < pComLayer.Count; i++)
        //        {
        //            ILayer pLyr = pComLayer.get_Layer(i);
        //            if (pLyr.Valid && pLyr.Visible)
        //            {
        //                if (pLyr is IFeatureLayer)
        //                {
        //                    ILayerFields pLyrFds = pLyr as ILayerFields;
        //                    int idxValidDate = pLyrFds.FindField("QSRQ");
        //                    int idxInvalidDate = pLyrFds.FindField("ZZRQ");
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
        //    if (pTmpFeaLayer!=null)
        //    {
        //        ILayerFields pLyrFds = pTmpFeaLayer as ILayerFields;
        //            int idxValidDate = pLyrFds.FindField("QSRQ");
        //            int idxInvalidDate = pLyrFds.FindField("ZZRQ");
        //        if (idxInvalidDate != -1 && idxValidDate != -1)
        //        {
        //            IFeatureLayerDefinition2 pPFLD2 = pTmpFeaLayer as IFeatureLayerDefinition2;
        //            pPFLD2.DefinitionExpression = in_sDefinition;
        //        }

        //    }
        //}
        private ILineSymbol getSymbol()
        {
            //ILineSymbol pFillSymbol = new SimpleLineSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            //颜色对象
            IRgbColor pRGBColor = new RgbColorClass();
            //pRGBColor.UseWindowsDithering = false;
            //pRGBColor.Red = 255;
            //pRGBColor.Green = 0;
            //pRGBColor.Blue = 0;

            ////填充符号以及画笔
            //ISymbol pTempSymbol = pFillSymbol as ISymbol;
            ////pTempSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            //pFillSymbol.Color = pRGBColor;
            //pFillSymbol.Color.Transparency = 0;
            ////边缘线颜色以及画笔
            //ISymbol pLSymbol = pLineSymbol as ISymbol;
            //pLSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            pRGBColor.Red = 0;//99,91,208
            pRGBColor.Green = 255;
            pRGBColor.Blue = 0;
            pLineSymbol.Color = (IColor)pRGBColor;

            pLineSymbol.Width = 0.1;
            pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            //pFillSymbol.Outline = pLineSymbol;

            return pLineSymbol;
        }
       
        private void dataGridViewX1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }
    }
}
