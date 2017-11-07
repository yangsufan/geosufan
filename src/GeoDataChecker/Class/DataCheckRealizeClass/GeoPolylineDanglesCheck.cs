using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{

    //线悬挂点检查
    public class GeoPolylineDanglesCheck : IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;
        public GeoPolylineDanglesCheck()
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
            ModDataCheck.CommonTopologyCheck(Hook, this, esriGeometryType.esriGeometryPolyline,esriTopologyRuleType.esriTRTLineNoDangles);
        }

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "线悬挂点检查";
            e.ErrInfo.ErrID = enumErrorType.线存在悬挂点.GetHashCode();
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
