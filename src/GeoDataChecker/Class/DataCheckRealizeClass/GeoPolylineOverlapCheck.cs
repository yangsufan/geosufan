using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 同层线重叠检查
    /// </summary>
    public class GeoPolylineOverlapCheck : IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;

        #region IDataCheck 成员

        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook as IArcgisDataCheckHook;
        }

        public void OnDataCheck()
        {
            Exception eError = null;
            if (Hook == null) return;
            IArcgisDataCheckParaSet dataCheckParaSet = Hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //实现
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.DbConn = dataCheckParaSet.DbConnPara;
            pSysTable.DBConType = SysCommon.enumDBConType.OLEDB;
            pSysTable.DBType = SysCommon.enumDBType.ACCESS;
           
            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            if (featureDatasetNames.Count == 0) return;


            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = featureDatasetNames.Count;
            int pValue = 0;

            foreach (string name in featureDatasetNames)
            {
                IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out eError);
                if (eError != null) continue;

                //同层线重叠检查
                DataTable mTable = TopologyCheckClass.GetParaValueTable(featureDataset,pSysTable, 14, out eError);
                if (eError != null)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }
                if (mTable.Rows.Count == 0)
                {
                    //return;
                    continue;
                }

                SpecialFeaClsTopoCheck2(Hook, featureDataset, mTable, esriTopologyRuleType.esriTRTLineNoOverlap, out eError);
                if (eError != null)
                {
                    eError = new Exception("同层线重叠检查失败！" + eError.Message);
                    return;
                }

                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
            }
        }

      
        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "同层线重叠检查";
            e.ErrInfo.ErrID = enumErrorType.同层线重叠检查.GetHashCode();
            DataErrTreat(sender, e);
        }
        #endregion

        /// <summary>
        /// 同层线重叠检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="eError"></param>
        public void SpecialFeaClsTopoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType pTopoRule, out Exception eError)
        {
            eError = null;

            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = pTable.Rows.Count;
            int pValue = 0;


            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if (FeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                CommonTopologyCheckClass commonTopologyCheckClass = new CommonTopologyCheckClass();
                commonTopologyCheckClass.DataErrTreat += new DataErrTreatHandle(DataCheckRealize_DataErrTreat);
                commonTopologyCheckClass.OrdinaryTopoCheck(hook, pFeaDataset, FeaClsName, pTopoRule, out eError);
                if (eError != null) return;

                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
            }
        }

        public void SpecialFeaClsTopoCheck2(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType pTopoRule, out Exception eError)
        {
            eError = null;
            List<string> feaclsNameList = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if (FeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (!feaclsNameList.Contains(FeaClsName))
                {
                    feaclsNameList.Add(FeaClsName);
                }
            }
            if (feaclsNameList.Count == 0) return;

            CommonTopologyCheckClass commonTopologyCheckClass = new CommonTopologyCheckClass();
            commonTopologyCheckClass.DataErrTreat += new DataErrTreatHandle(DataCheckRealize_DataErrTreat);
            commonTopologyCheckClass.OrdinaryTopoCheck(hook, pFeaDataset, feaclsNameList, pTopoRule, out eError);
            if (eError != null) return;
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }

    }
}
