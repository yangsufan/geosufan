using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 简单线检查
    /// </summary>
    public class GeoCheckMustBeSinglePart: IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;
        public GeoCheckMustBeSinglePart()
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
            ModDataCheck.CommonTopologyCheck(Hook, this, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoMultipart);
        }

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "简单线检查";
            e.ErrInfo.ErrID = enumErrorType.简单线检查.GetHashCode();
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
