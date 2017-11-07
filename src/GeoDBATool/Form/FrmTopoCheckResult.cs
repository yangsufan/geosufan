using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

//ygc 2012-12-27 逻辑检测错误展现

namespace GeoDBATool
{
    public partial class FrmTopoCheckResult : DevComponents.DotNetBar.Office2007Form
    {
        private System.Data.DataTable _ErrDataTable = null;
        public System.Data.DataTable ErrDataTable
        {
            set { _ErrDataTable = value; }
        }
        private IMapControlDefault _MapControl = null;
        public FrmTopoCheckResult(IMapControlDefault pMapControl)
        {
            _MapControl = pMapControl;
            InitializeComponent();
        }
        public DataGridViewX ErrorDataGrid
        {
            get 
            {
                return DataGridErrs; 
            }
        }

        private void DataGridErrs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iOID1Index = DataGridErrs.Columns["要素OID1"].Index;
            int iOID2Index = DataGridErrs.Columns["要素OID2"].Index;
            if (e.ColumnIndex == iOID1Index)
            {
                ShowFalshFeature(e.ColumnIndex, e.RowIndex, "数据图层1");
            }
            else if (e.ColumnIndex == iOID2Index)
            {
                ShowFalshFeature(e.ColumnIndex, e.RowIndex, "数据图层2");
            }
            else
            {
                ShowAllFeature(e.ColumnIndex, e.RowIndex);
            }
        }

        private void ShowFalshFeature(int iColumnIndex, int iRowIndex, string strLayerCoum)
        {
            if (iRowIndex == -1)
            {
                return;
            }
            int iOID = Convert.ToInt32(DataGridErrs.Rows[iRowIndex].Cells[iColumnIndex].Value);
            string strLayer = DataGridErrs.Rows[iRowIndex].Cells[strLayerCoum].Value.ToString();
            ILayer pLayer = ModDBOperator.GetLayer(_MapControl, strLayer);
            double d_mapx = -1;
            double d_mapy = -1;
            string errFeaGepStr = string.Empty;
            try
            {
                d_mapx = Convert.ToDouble(DataGridErrs.CurrentRow.Cells["定位点X"].FormattedValue.ToString());
                d_mapy = Convert.ToDouble(DataGridErrs.CurrentRow.Cells["定位点Y"].FormattedValue.ToString());
            }
            catch
            {
                d_mapx = 0; d_mapy = 0;
            }
            try
            {
                errFeaGepStr = DataGridErrs.CurrentRow.Cells["错误几何形状"].FormattedValue.ToString();
            }
            catch
            {
                errFeaGepStr = string.Empty;
            }

            if (iOID != -1)
            {
                if (pLayer != null && pLayer is IFeatureLayer)
                {
                    IFeatureClass pFeatureClass = (pLayer as IFeatureLayer).FeatureClass;
                    if (pFeatureClass == null) { return; }
                    try
                    {
                        IFeature pFeature = pFeatureClass.GetFeature(iOID);
                        //ModOperator.FlashFeature(pFeature, m_hookHelper.ActiveView);
                        //IHookActions pHookActions = m_hookHelper as IHookActions;
                        //pHookActions.DoAction(pFeature.Shape, esriHookActions.esriHookActionsPan);
                        //Application.DoEvents();
                        //pHookActions.DoAction(pFeature.Shape, esriHookActions.esriHookActionsFlash);
                        _MapControl.Map.ClearSelection();
                        _MapControl.Map.SelectFeature(pLayer, pFeature);
                        ShowErrorGeo(d_mapx, d_mapy, errFeaGepStr);
                        _MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, _MapControl.ActiveView.Extent);
                        //Application.DoEvents();
                        //pHookActions.DoAction(pFeature.Shape, esriHookActions.esriHookActionsCallout);

                    }
                    catch { return; }

                }
            }
        }
        /// <summary>
        /// 选中并定位到选中行中的所有要素
        /// </summary>
        /// <param name="iColumnIndex"></param>
        /// <param name="iRowIndex"></param>
        private void ShowAllFeature(int iColumnIndex, int iRowIndex)
        {
            if (iRowIndex == -1)
            {
                return;
            }
            int iOID1 = Convert.ToInt32(DataGridErrs.Rows[iRowIndex].Cells["要素OID1"].Value);
            int iOID2 = Convert.ToInt32(DataGridErrs.Rows[iRowIndex].Cells["要素OID2"].Value);
            string strSLayer = DataGridErrs.Rows[iRowIndex].Cells["数据图层1"].Value.ToString();
            string strTLayer = DataGridErrs.Rows[iRowIndex].Cells["数据图层2"].Value.ToString();
            double d_mapx = -1;
            double d_mapy = -1;
            string errFeaGepStr = string.Empty;
            try
            {
                d_mapx = Convert.ToDouble(DataGridErrs.CurrentRow.Cells["定位点X"].FormattedValue.ToString());
                d_mapy = Convert.ToDouble(DataGridErrs.CurrentRow.Cells["定位点Y"].FormattedValue.ToString());
            }
            catch
            {
                d_mapx = 0; d_mapy = 0;
            }
            try
            {
                errFeaGepStr = DataGridErrs.CurrentRow.Cells["错误几何形状"].FormattedValue.ToString();
            }
            catch
            {
                errFeaGepStr = string.Empty;
            }
            

            ILayer pSLayer = ModDBOperator.GetLayer(_MapControl as ESRI.ArcGIS.Controls.IMapControlDefault, strSLayer);
            ILayer pTLayer = ModDBOperator.GetLayer(_MapControl as ESRI.ArcGIS.Controls.IMapControlDefault, strTLayer);
            IFeature pSFeature = null;
            IFeature pTFeature = null;
            IList<IFeature> vFeaList = new List<IFeature>();
            if (iOID1 != -1)
            {
                if (pSLayer != null && pSLayer is IFeatureLayer)
                {
                    IFeatureClass pSFeatureClass = (pSLayer as IFeatureLayer).FeatureClass;
                    if (pSFeatureClass == null) { return; }
                    try
                    {
                        pSFeature = pSFeatureClass.GetFeature(iOID1);
                    }
                    catch { return; }

                }
            }
            if (iOID2 != -1)
            {
                if (pTLayer != null && pTLayer is IFeatureLayer)
                {
                    IFeatureClass pTFeatureClass = (pTLayer as IFeatureLayer).FeatureClass;
                    if (pTFeatureClass == null) { return; }
                    try
                    {
                        pTFeature = pTFeatureClass.GetFeature(iOID2);
                    }
                    catch { }
                }
            }
            IGeometry pGeometry = null;
            _MapControl.Map.ClearSelection();
            _MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, _MapControl.ActiveView.Extent);
            if (pTFeature == null)
            {
                if (pSFeature != null)
                {
                    _MapControl.Map.SelectFeature(pSLayer, pSFeature);
                    pGeometry = pSFeature.ShapeCopy;
                }
            }
            else
            {

                if (pSFeature.ShapeCopy.GeometryType == esriGeometryType.esriGeometryPoint && pTFeature.ShapeCopy.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    object _missing = Type.Missing;

                    IGeometryCollection geometryCollection = new PolylineClass();
                    IPointCollection pointCollection = new PathClass();
                    pointCollection.AddPoint(pSFeature.ShapeCopy as IPoint, ref _missing, ref _missing);
                    pointCollection.AddPoint(pTFeature.ShapeCopy as IPoint, ref _missing, ref _missing);
                    geometryCollection.AddGeometry(pointCollection as IGeometry, ref _missing, ref _missing);
                    MakeZAware(geometryCollection as IGeometry);
                    pGeometry = geometryCollection as IGeometry;
                    pGeometry.Project(pSFeature.Shape.SpatialReference);
                }
                else
                {
                    vFeaList.Add(pSFeature); vFeaList.Add(pTFeature);
                    pGeometry = GetLyrUnionPlygon(vFeaList);
                }
                _MapControl.Map.SelectFeature(pTLayer, pTFeature);
            }
            _MapControl.Map.SelectFeature(pSLayer, pSFeature);
            ModDBOperator.ZoomToFeature(_MapControl.Map, pGeometry);
            ShowErrorGeo(d_mapx, d_mapy, errFeaGepStr);
            _MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, _MapControl.ActiveView.Extent);
            //ShowErrState();
        }
        /// <summary>
        /// 获得指定图层的合并范围 为本次加的一个函数
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IGeometry GetLyrUnionPlygon(IList<IFeature> vFeaList)
        {

            if (vFeaList.Count < 1) return null;
            //构造
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            for (int i = 0; i < vFeaList.Count; i++)
            {
                if (vFeaList[i].Shape != null && !vFeaList[i].Shape.IsEmpty) pGeometryCol.AddGeometry(vFeaList[i].ShapeCopy, ref obj, ref obj);
            }
            //构造合并
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeometryCol as IEnumGeometry);
            IGeometry pGeometry = pTopo as IGeometry;
            return pGeometry;
        }
        private void MakeZAware(IGeometry geometry)
        {
            IZAware zAware = geometry as IZAware;
            zAware.ZAware = true;

        }
        private void ExportErr()
        {
            string saveFileName = "";
            //bool fileSaved = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件(*.xls)|*.xls";
            saveDialog.FileName = "检查结果";
            saveDialog.ShowDialog();
            System.Windows.Forms.Application.DoEvents();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

            this.progressStep.Visible = true;
            progressStep.Minimum = 0;
            progressStep.Maximum = 5;
            progressStep.Step = 1;
            progressStep.PerformStep();
            this.lblTips.Text = "创建Excel对象";
            Application.DoEvents();
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                progressStep.Visible = false;
                this.lblTips.Text = "";
                return;
            }

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            //写入标题
            progressStep.PerformStep();
            this.lblTips.Text = "写入标题";
            Application.DoEvents();
            for (int i = 0; i < _ErrDataTable.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = _ErrDataTable.Columns[i].ColumnName;//注意exel起始索引1
            }
            //写入数值
            progressStep.PerformStep();
            this.lblTips.Text = "写入数值...";
            Application.DoEvents();
            for (int r = 0; r < _ErrDataTable.Rows.Count; r++)
            {
                //worksheet.Cells[r + 2, 1] = inListView.Items[r].Text;
                for (int i = 0; i < _ErrDataTable.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = _ErrDataTable.Rows[r][i];
                }

            }
            progressStep.PerformStep();
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            //if (Microsoft.Office.Interop.cmbxType.Text != "Notification")
            //{
            //    Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);
            //    rg.NumberFormat = "00000000";
            //}
            progressStep.PerformStep();
            this.lblTips.Text = "保存文件";
            Application.DoEvents();
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    //fileSaved = true;
                }
                catch (Exception ex)
                {
                    //fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    progressStep.Visible = false;
                    this.lblTips.Text = "";
                    return;
                }

            }
            //else
            //{
            //    fileSaved = false;
            //}
            xlApp.Quit();
            progressStep.Visible=false;
            this.lblTips.Text = "";
            Application.DoEvents();
            GC.Collect();//强行销毁 
            // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL
            MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }
        /// <summary>
        /// 展图
        /// </summary>
        private void ShowErrorGeo(double in_X, double in_Y, string in_sErrGepStr)
        {
            double pMapx = in_X;  //定位点X
            double pMapY = in_Y;  //定位点Y
            string errFeaGepStr = in_sErrGepStr;  //错误形状信息
            long scale = 0;  //符号比例尺
            //进行错误展图
            IGraphicsContainer pGra =_MapControl.Map  as IGraphicsContainer;
            pGra.DeleteAllElements();
            if (errFeaGepStr == "")
            {
                //若错误几何形状为空，说明是点
                if (pMapx == 0 || pMapY == 0) return;

                //展点
                MakePointSymbol(pGra, pMapx, pMapY, scale);
            }
            else
            {
                //展线

                //将错误字符串解析成几何形状
                object pGeo = new PolylineClass();  //初始化一个线类型的对象
                IGeometry errGeo = null;  //错误形状
                byte[] xmlByte = null;
                try
                {
                    xmlByte = Convert.FromBase64String(errFeaGepStr);  //转化为字节型
                }
                catch
                { }
                if (xmlByte == null)
                {
                    return;
                }
                //进行解析
                if (XmlDeSerializer(xmlByte, pGeo) == true)
                {
                    //解析成功
                    errGeo = pGeo as IGeometry;
                }
                else
                {
                    pGeo = new LineClass();
                    //进行解析
                    if (XmlDeSerializer(xmlByte, pGeo) == true)
                    {
                        errGeo = pGeo as IGeometry;
                    }
                    else
                    {
                        //解析不成功
                        pGeo = new PolygonClass();  //用面对象来初始化对象
                        //进行解析
                        if (XmlDeSerializer(xmlByte, pGeo) == true)
                        {
                            errGeo = pGeo as IGeometry;
                        }
                    }
                }
                if (errGeo == null)
                {
                    MessageBox.Show("解析几何形状出错！", "提示");
                    return;
                }
                //执行展线
                IPolyline pLine = errGeo as IPolyline;  //线错误形状
                IPolygon newpolygon = errGeo as IPolygon;  //面错误形状
                if (pLine == null)
                {
                    //若错误形状线伪空，说明错误形状是面
                    if (newpolygon != null)
                    {
                        //获得面的边界线形状
                        IPointCollection PoCol = new Polyline();    //线的点集合
                        PoCol.AddPointCollection(newpolygon as IPointCollection);
                        pLine = PoCol as IPolyline;
                    }
                }

                //展线形状
                if (pLine != null)
                {
                    MakeLineSymbol(pGra, pLine);
                }
            }
        }
        /// <summary>
        /// 将错误点展现在图面上
        /// </summary>
        /// <param name="pGra">符号展现容器</param>
        /// <param name="x">点x坐标</param>
        /// <param name="y">点y坐标</param>
        /// <param name="conScale">底图比例尺</param>
        private void MakePointSymbol(IGraphicsContainer pGra, double x, double y, long conScale)
        {
            //IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            //画点
            ESRI.ArcGIS.Geometry.IPoint pPint = new ESRI.ArcGIS.Geometry.PointClass();
            pPint.PutCoords(x, y);
            //设置点的衍射
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 0;
            pColor.Green = 0;
            pColor.Red = 255;

            //设置显示比例尺和参考比例尺
            double vMapFrameScale = double.Parse(conScale.ToString());
            double vMapRefrenceScale = double.Parse(conScale.ToString());
            // m_Hook.MapControl.Map.ReferenceScale = double.Parse(conScale.ToString());
            _MapControl.Map.ReferenceScale = double.Parse(conScale.ToString());

            //设置符号显示大小，颜色、几何等
            IMarkerElement pMakEle = new MarkerElementClass();
            pEle = pMakEle as IElement;
            ISimpleMarkerSymbol pMakSym = new SimpleMarkerSymbolClass();
            if (vMapRefrenceScale != 0 && vMapFrameScale != 0)
            {
                double size = (vMapFrameScale / 40) * vMapFrameScale / vMapRefrenceScale; ;// (vMapFrameScale / 30) * vMapFrameScale / vMapRefrenceScale;
                pMakSym.Size = size;// *10;
            }
            //IDisplayName pDisName = pMakSym as IDisplayName;
            //pDisName.NameString = pointName;
            pMakSym.Color = pColor;
            pMakEle.Symbol = pMakSym;
            pMakSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pEle.Geometry = pPint as ESRI.ArcGIS.Geometry.IGeometry;

            //添加元素
            pGra.AddElement(pEle, 0);
            //pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //ESRI.ArcGIS.Geometry.IPoint pPint2 = new ESRI.ArcGIS.Geometry.PointClass();
            //pPint2.PutCoords(y - 50, x + 50);

            //ITextElement pTextElement = new TextElementClass();
            //ITextSymbol pTextSymbol = new TextSymbolClass();

            //stdole.StdFont pStdFont = new stdole.StdFontClass();
            //stdole.IFontDisp pFont = (stdole.IFontDisp)pStdFont;
            //pFont.Name = "宋体";
            //if (vMapRefrenceScale != 0 && vMapFrameScale != 0)
            //{
            //    //double size = (vMapFrameScale / 30) * vMapFrameScale / vMapRefrenceScale;
            //    double size = (vMapFrameScale / vMapRefrenceScale) * 16;
            //    pFont.Size = (decimal)size;
            //}
            //pTextSymbol.Font = pFont;
            //pTextSymbol.Color = pColor;

            //pTextElement.Symbol = pTextSymbol;
            //pTextElement.ScaleText = true;
            //pTextElement.Text = "";//pointName;
            //pEle = pTextElement as IElement;
            //pEle.Geometry = pPint2 as ESRI.ArcGIS.Geometry.IGeometry;

            //pGra.AddElement(pEle, 0);
            //pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 展线
        /// </summary>
        /// <param name="pGra">符号展现容器</param>
        /// <param name="line">线形状</param>
        private void MakeLineSymbol(IGraphicsContainer pGra, IPolyline line)
        {
            //IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            //定义颜色
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 0;
            pColor.Green = 0;
            pColor.Red = 255;

            ILineElement pMakEle = new LineElementClass();
            //设置线的宽度，颜色，符号，几何形状等属性
            pEle = pMakEle as IElement;
            ILineSymbol pMakSym = new SimpleLineSymbolClass();
            pMakSym.Width = 1;
            pMakSym.Color = pColor;
            pMakEle.Symbol = pMakSym;
            //IPolyline newline = new PolylineClass();
            //newline.ToPoint = line.ToPoint;
            //newline.FromPoint = line.FromPoint;
            pEle.Geometry = line as ESRI.ArcGIS.Geometry.IGeometry;

            //添加线元素
            pGra.AddElement(pEle, 0);
            //pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 展线
        /// </summary>
        /// <param name="pGra">符号展现容器</param>
        /// <param name="line">线形状</param>
        private void MakePolygonSymbol(IGraphicsContainer pGra, IPolygon polygon)
        {
            //IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            //定义颜色
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 0;
            pColor.Green = 0;
            pColor.Red = 255;

            IPolygonElement pMakEle = new PolygonElementClass();
            //设置线的宽度，颜色，符号，几何形状等属性
            pEle = pMakEle as IElement;
            ISymbol pSymbol = null;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            /// end
            pLineSymbol.Color = pColor;
            pLineSymbol.Width = 1.5;
            pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pFillSymbol.Outline = pLineSymbol;

            pFillSymbol.Color = pColor;
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            (pEle as IFillShapeElement).Symbol = pFillSymbol;
            //IPolyline newline = new PolylineClass();
            //newline.ToPoint = line.ToPoint;
            //newline.FromPoint = line.FromPoint;
            pEle.Geometry = polygon as ESRI.ArcGIS.Geometry.IGeometry;

            //添加线元素
            pGra.AddElement(pEle, 0);
            //pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {

            try
            {
                //判断字符串是否为空
                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch (Exception eError)
            {
                //*********************************************
                //guozheng 2010-12-24 平安夜  added 系统异常日志
                //if (ModData.SysLog == null) ModData.SysLog = new clsWriteSystemFunctionLog();
                //ModData.SysLog.Write(eError);
                //**********************************************
                return false;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportErr();
        }
    }
}
