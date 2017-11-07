using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    //同层面重叠检查
    public class GeoPolygonOverlapCheck : IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;
        public GeoPolygonOverlapCheck()
        {
        }

        #region IDataCheck 成员

        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook as IArcgisDataCheckHook;
        }

        public void OnDataCheck()
        {
            if (Hook == null) return;
            ModDataCheck.CommonTopologyCheck(Hook, this, esriGeometryType.esriGeometryPolygon, esriTopologyRuleType.esriTRTAreaNoOverlap);
        }

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "同层面重叠检查";
            e.ErrInfo.ErrID = enumErrorType.同层面重叠检查.GetHashCode();
            DataErrTreat(sender, e);
        }
        #endregion

        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }
    }
}
