using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace DrawSheetMap
{
   public class clsGetGeoInfo
    {
        /// <summary>
        ///左上角点坐标(平面）
        /// </summary>
        public IPoint m_pPointLT1;
        /// <summary>
        /// 左下角点坐标(平面）
        /// </summary>
        public IPoint m_pPointLB2;
        /// <summary>
        /// 右下角点坐标(平面）
        /// </summary>
        public IPoint m_pPointRB3;
        /// <summary>
        /// 右上角点坐标(平面）
        /// </summary>
        public IPoint m_pPointRT4;
        /// <summary>
        /// 左上角点坐标(经纬度
        /// </summary>
        public IPoint m_pGeoPointLT1;
        /// <summary>
        /// 左下角点坐标(经纬度）
        /// </summary>
        public IPoint m_pGeoPointLB2;
        /// <summary>
        /// 右下角点坐标(经纬度）
        /// </summary>
        public IPoint m_pGeoPointRB3;
        /// <summary>
        /// 右上角点坐标(经纬度）
        /// </summary>
        public IPoint m_pGeoPointRT4;
        /// <summary>
        /// 得到的返回的图幅图形
        /// </summary>
        public IGeometry m_pSheetMapGeometry;
        /// <summary>
        /// 上边线的几何图形（插入点后 图纸坐标）
        /// </summary>
        public IGeometry m_pTopLineGeometry;
        /// <summary>
        /// 下边线的几何图形（插入点后 图纸坐标）
        /// </summary>
        public IGeometry m_pBottomLineGeometry;
        /// <summary>
        /// 比例尺
        /// </summary>
        public long m_lngMapScale;
        /// <summary>
        /// 图幅号
        /// </summary>
        public string m_strMapNO;
        /// <summary>
        /// 投影信息
        /// </summary>
        public IProjectedCoordinateSystem m_pPrjCoor;
        /// <summary>
        /// 插入点数
        /// </summary>
        public int m_intInsertCount;
        /// <summary>
        /// 该模块用到的模块级变量
        /// </summary>
        private IGeometry m_pTopLine;
        private IGeometry m_pBottomLine;
        /// <summary>
        /// 由图幅号和比例尺　投影计算出所有的图幅形状信息
        /// </summary>
        public void ComputerAllGeoInfo()
        {

        }
        public void ChangeMapToPaper()
        {

        }
        /// <summary>
        /// 将平面坐标线转到图纸坐标中去 主要是针对上下两条边线
        /// </summary>
        /// <param name="pPolyline"></param>
        /// <returns></returns>
        private IPolyline ChangeLineToPaper(IPolyline pPolyline)
        {
            return null;
        }

    }
}
