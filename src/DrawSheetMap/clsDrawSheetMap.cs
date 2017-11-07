using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace DrawSheetMap
{
    public class clsDrawSheetMap
    {
        /// <summary>
        /// 记录模板中点与现有点的坐标偏移量X
        /// </summary>
        private IList<double> m_vXoffset;
        /// <summary>
        /// 记录模板中点与现有点的坐标偏移量Y
        /// </summary>
        private IList<double> m_vYoffset;
        /// <summary>
        /// 比例尺
        /// </summary>
        private long vScale;
        /// <summary>
        /// 传进来的图幅形状
        /// </summary>
        public IGeometry pGeometry;
        /// <summary>
        /// 传进来的制图对象
        /// </summary>
        public IPageLayoutControl vPageLayoutControl;
        /// <summary>
        /// 图幅号
        /// </summary>
        public string m_strSheetNo;
        /// <summary>
        /// 要插入的点数
        /// </summary>
        public int m_intPntCount;
        /// <summary>
        /// 采用的投影坐标系统
        /// </summary>
        public ISpatialReference m_pPrjCoor;
        /// <summary>
        /// '专题类型0地形图1二调现状图2规划图
        /// </summary>
        public int type_ZT;
        /// <summary>
        /// 计算图幅四个顶点的位置信息
        /// </summary>
        private clsGetGeoInfo m_clsGetGeoInfo;
        /// <summary>
        /// 四个角点的图纸点信息
        /// </summary>
        private IPointCollection m_pPaperPointCol;
        /// <summary>
        /// 得到尽可能多的图幅信息
        /// </summary>
        private MapInfo m_pMapInfo;
        /// <summary>
        /// 参考矩形框
        /// </summary>
        public IElement g_pRecElement;
        /// <summary>
        /// 数据框
        /// </summary>
        public IElement g_MapFrameEle;
        public void DrawSheetMap()
        {

        }
        public void DrawSheetMap(object pMapInfo)
        {

        }
        public void DrawSheetMap(double pPageWidth, double pPageHeight, double pMapWidth, double pMapHeight)
        {

        }
        public void DrawSheetMapByMultiMap(IGeometry pSourceGeo)
        {

        }
        /// <summary>
        /// 移动要素的位置
        /// </summary>
        /// <param name="pPagelayout"></param>
        /// <param name="vXoffset"></param>
        /// <param name="vYoffset"></param>
        public void MoveElementInPage(IPageLayout pPagelayout, IList<double> vXoffset, IList<double> vYoffset)
        {

        }
        public void MoveElementInPageForTDLY(IPageLayout pPagelayout, IList<double> vXoffset, IList<double> vYoffset)
        {

        }
        private void MoveElementInBigScalePage(IPageLayout pPagelayout, IList<double> vXoffset, IList<double> vYoffset)
        {

        }
        private void OpenTemplateElement(string vType, IGeometry pGeometry, IPageLayoutControl vPagelayout)
        {

        }
        /// <summary>
        /// 对点排序 左上角为第一点(这里只是一个见排序 近世矩形的 且X Y偏差不能太大
        /// </summary>
        /// <param name="OrderPointCol"></param>
        private void OrderPointCol(IPointCollection OrderPointCol)
        {

        }
        /// <summary>
        /// 得到两个图框以便进行坐标有平面到图纸的转换 同时得到参考矩形框 获得的参数均以公用变量的形式传出
        /// 在得到一些参数的同时 又进行了一些必要的处理过程
        /// </summary>
        /// <param name="pPagelayout"></param>
        private void GetMapAndPagerEn(IPageLayout pPagelayout)
        {

        }
        /// <summary>
        /// 画图幅的内外图框  思路是先根据图幅的几何形状画外图框 再根据外图框画内图框（边线）
        /// </summary>
        /// <param name="lngScale"></param>
        /// <param name="pGeometry"></param>
        /// <param name="vPagelayout"></param>
        private void DrawMapBroderIn(long lngScale, IGeometry pGeometry, IPageLayoutControl vPagelayout)
        {

        }
        /// <summary>
        /// 画经纬度格网短线
        /// </summary>
        /// <param name="pFromPoint"></param>
        /// <param name="pToPoint"></param>
        /// <param name="pLine"></param>
        /// <param name="lngMinX"></param>
        /// <param name="lngMinY"></param>
        /// <param name="vType"></param>
        /// <param name="lngScale"></param>
        /// <param name="pMapelement"></param>
        /// <param name="pGrapContainer"></param>
        private void DrawCoordinameGridLine(IPoint pFromPoint, IPoint pToPoint,
           ILine pLine, double lngMinX, double lngMinY, int vType, long lngScale, IMapFrame pMapelement, IGraphicsContainer pGrapContainer)
        {

        }
        /// <summary>
        /// 获得平面坐标
        /// </summary>
        /// <param name="dblMapx"></param>
        /// <param name="dblMapY"></param>
        /// <param name="pProjectReference"></param>
        /// <param name="db1X"></param>
        /// <param name="db1Y"></param>
        public void GetPanleByCoordinate(double dblMapx, double dblMapY, IProjectedCoordinateSystem pProjectReference, double db1X, double db1Y)
        {

        }
        /// <summary>
        /// 写格网的标市
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="strText"></param>
        /// <param name="db1TextSize"></param>
        /// <param name="h_alignment"></param>
        /// <param name="v_alignment"></param>
        /// <returns></returns>
        private ITextElement DrawGridText(IPoint pPoint, string strText, double db1TextSize, esriTextHorizontalAlignment h_alignment, esriTextVerticalAlignment v_alignment)
        {
            return null;
        }
        /// <summary>
        /// 由MAP上的GEOMETRY转到图纸坐标上来
        /// </summary>
        /// <param name="pMapPoint1"></param>
        /// <param name="pMapPoint2"></param>
        /// <param name="pPaperPoint1"></param>
        /// <param name="pPaperPoint2"></param>
        /// <param name="db1X"></param>
        /// <param name="db1Y"></param>
        /// <returns></returns>
        private double GetPaperXY(IPoint pMapPoint1, IPoint pMapPoint2, IPoint pPaperPoint1, IPoint pPaperPoint2, double db1X, double db1Y)
        {
            return 0;
        }
        /// <summary>
        /// 得到要素的交点
        /// </summary>
        /// <param name="pSegment1"></param>
        /// <param name="pSegment2"></param>
        /// <param name="blnEx"></param>
        /// <returns></returns>
        private IPoint GetIntersectionSeg(ISegment pSegment1, ISegment pSegment2, bool blnEx)
        {
            return null;
        }
        /// <summary>
        /// 得到要素的交点
        /// </summary>
        /// <param name="pPolyline1"></param>
        /// <param name="pPolyline2"></param>
        /// <returns></returns>
        private IPoint GetIntersection(IPolyline pPolyline1, IPolyline pPolyline2)
        {
            return null;
        }
        private void CopyToPageLayout(IGeometry pGeometry, IPageLayoutControl vPagelayout, long lngScale)
        {

        }
        private void MovePageLyrOutlbl(IElement pElement, double db1X, double db1Y)
        {

        }
    }
}
