using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace DrawSheetMap
{
    public class basPageLayout
    {
        /// <summary>
        /// 用与记录是否出于比例尺制图过程中
        /// </summary>
        public bool m_bScalePageLayout;
        /// <summary>
        /// 制图框中的平面坐标框
        /// </summary>
        public IEnvelope g_pMapEnvelope;
        /// <summary>
        /// 制图框中的图纸坐标框
        /// </summary>
        public IEnvelope g_pPaperEnvelope;
        /// <summary>
        /// 制图框
        /// </summary>
        public IMapFrame g_pMapFrame;
        /// <summary>
        /// 参考矩形框
        /// </summary>
        public IElement g_pRecElement;
        /// <summary>
        /// 数据框
        /// </summary>
        public IElement g_MapFrameEle;
        public string g_ScaleType;
        public long g_lngScale;
        public bool g80To54;
        public double g_OffX;
        public double g_OffY;
        /// <summary>
        /// 画笔
        /// </summary>
        public bool g_Rubber;
        public bool g_SubjectTool;
        public string g_strSheetNo;
        public ISpatialReference g_pProjectSpatial;
        public IGeometry g_Geometry;
        public bool g_bAddLayer;
        /// <summary>
        /// 地图当前的旋转角度
        /// </summary>
        public double g_Rotation;
        /// <summary>
        /// 当前图层
        /// </summary>
        public ILayer g_CurrentLayer;
        public IGeometry g_BufferGeometry;
        /// <summary>
        /// 普通模板制图模板文件
        /// </summary>
        public string g_CustomPagepath;
        public IPageLayoutControl g_PagelayoutControl;
        public ITextElement CreateElement(ITextElement pTextElem, IPoint pPoint, ITextSymbol textSymbol)
        {
            return null;
        }
        /// <summary>
        /// 图层的排序
        /// </summary>
        /// <param name="pSortLayer"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public long SortIndex(ILayer pSortLayer, IBasicMap pMap)
        {
            return 0;
        }
        private string GetLayerType(ILayer pLayer)
        {
            return string.Empty;
        }
        public IElement addLine(double x1, double y1, double x2, double y2, double width, ISymbol pSym)
        {
            return null;
        }
        public IElement AddText(double x, double y, string strText)
        {
            return null;
        }
        public IElement AddMarker(double x1, double y1, double x2, double y2, ISymbol pSym)
        {
            return null;
        }
        public IElement AddFill(double x1, double y1, double x2, double y2, ISymbol pSym)
        {
            return null;
        }
        public IElement AddAnno(double x, double y, string strText, ISymbol pSym)
        {
            return null;
        }
        public IEnvelope GetMapFrameSize(IGraphicsContainer pGraphicsContainer)
        {
            return null;
        }
        public bool InitLayerLayout()
        {
            return false;
        }
        public IPoint GetxyVal(IPoint pGeoPoint, IProjectedCoordinateSystem pPrj)
        {
            return null;
        }
        public bool GetCoordinateFromNewCode(string strNewMapCode, ref double x, ref double y)
        {
            return false;
        }
        /// <summary>
        /// 根据经纬度的两点(经纬度坐标点）　以及插入点的数量返回一个插点后的图幅框　上下边线（平面坐标系下的线,但不带坐标信息）
        /// </summary>
        /// <param name="pPointFrom"></param>
        /// <param name="pPointTo"></param>
        /// <param name="pPrj"></param>
        /// <param name="intPointCount"></param>
        /// <returns></returns>
        public IGeometry GetTopOrBotLine(IPoint pPointFrom, IPoint pPointTo, IProjectedCoordinateSystem pPrj, int intPointCount)
        {
            return null;
        }
        public void SetPageLayoutSize(IPageLayoutControl vPagelayout, double dblPageWidth, double dblPageHeight, double dblDataFrameWidth, double dblDataFrameHeight)
        {

        }
        /// <summary>
        /// 由MAP上的GEOMETRY转到图纸坐标上来
        /// </summary>
        /// <param name="pMapPoint1"></param>
        /// <param name="pMapPoint2"></param>
        /// <param name="pPaperPoint1"></param>
        /// <param name="pPaperPoint2"></param>
        /// <param name="pMapPoint"></param>
        /// <returns></returns>
        public IPoint GetPaperPoint(IPoint pMapPoint1, IPoint pMapPoint2, IPoint pPaperPoint1, IPoint pPaperPoint2, IPoint pMapPoint)
        {
            return null;
        }
        public Polygon GetGeometryByNewCode(string vNewCode, long vScale, ISpatialReference vSpatialReference, int vType = 0)
        {
            return null;
        }
        public int CountDelimiters(string sFiles, object vSearchChar)
        {
            return 0;
        }
        public string GetStrByNumber(string string1, string Identifiers, long num)
        {
            return string.Empty;
        }
        public IGeometry GetGeoByBigMapSheet(string strSheetNo, long lngMapScale, ISpatialReference pSpatial)
        {
            return null;
        }
        public void CalculateFigureforme(string vFigure, string vScaleType, ref IList<string> neighborTH)
        {

        }
        public void CalculateFigure(string vFigure, string vScaleType, Dictionary<string, string> m_FrameInfo, IPageLayout pPageLayout, IGeometry pSheetGeometry)
        {

        }
        public void FigureChar(double LeftCoord, string vFigure, ITextElement pTxtElement)
        {

        }
        /// <summary>
        /// 模板图四角坐标重新赋值
        /// </summary>
        /// <param name="db1S"></param>
        /// <param name="pTextElement"></param>
        /// <param name="blnEnter"></param>
        public void ReWriteCoor(double db1S, ITextElement pTextElement, bool blnEnter = false)
        {

        }
        public void FigureCharTable(string strSheetNo, ITextElement pTxtElement)
        {

        }
        public void DoEmpty(string vStrElement, long vScale, ITextElement pTxtElement, Dictionary<string, string> m_FrameInfo, double dblX, double dblY, double xOffset, double yOffset)
        {

        }
        /// <summary>
        /// 根据比例尺和图幅号算出来相邻的图幅信息
        /// </summary>
        /// <param name="vStrElement"></param>
        /// <param name="vScale"></param>
        /// <param name="dblX"></param>
        /// <param name="dblY"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="strSheetNo"></param>
        public void DoEmptyforme(string vStrElement, long vScale, double dblX, double dblY, double xOffset, double yOffset, ref string strSheetNo)
        {

        }
        public void GetFrameInfo(long vScalem, Dictionary<string, string> m_FrameInfo, double db1X, double db1Y, double xOffset, double yOffset)
        {

        }
        public bool GetSheetNoFromXY(ref string strSheetNo, long lngScale, double db1X, double db1Y)
        {
            return false;
        }
        public bool GetNewCodeFromCoordinate(ref string strNewMapCode, long x, long y, long vScale)
        {
            return false;
        }
        public ISpatialReference GetSpatialByX(double db1X)
        {
            return null;
        }
    }
    public class MapInfo
    {
        public string Keys;
        public object Infos;
        public IPolygon pGeometry;
    }
}
