using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 点位于线端点检查
    /// </summary>
    public class GeoPointCoveredByLineEndPointCheck : IDataCheckRealize
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

            //实现代码
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


                //点位于线端点检查，参数ID为12
                DataTable mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 12, out eError);
                if (eError != null)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }

                if (mTable.Rows.Count == 0)
                {
                    continue;
                }
                AreaTopoCheck2(Hook, featureDataset, mTable, esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint, out eError);
                if (eError != null)
                {
                    eError=new Exception("点位于线端点检查失败！" + eError.Message);
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
            e.ErrInfo.FunctionName = "点位于线端点检查";
            e.ErrInfo.ErrID = enumErrorType.点位于线端点检查.GetHashCode();
            DataErrTreat(sender, e);
        }

        /// <summary>
        /// 面拓扑检查,面含面检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="topoRule"></param>
        /// <param name="eError"></param>
        public void AreaTopoCheck2(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType topoRule, out Exception eError)
        {
            eError = null;

            List<string> FeaClsNameDic = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                if (!FeaClsNameDic.Contains(oriFeaClsName + ";" + desFeaClsName))
                {
                    FeaClsNameDic.Add(oriFeaClsName + ";" + desFeaClsName);
                }
            }
            CommonTopologyCheckClass commonTopologyCheckClass = new CommonTopologyCheckClass();
            commonTopologyCheckClass.DataErrTreat += new DataErrTreatHandle(DataCheckRealize_DataErrTreat);
            commonTopologyCheckClass.OrdinaryDicTopoCheck(hook, pFeaDataset, FeaClsNameDic, topoRule, out eError);
            if (eError != null) return;
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
