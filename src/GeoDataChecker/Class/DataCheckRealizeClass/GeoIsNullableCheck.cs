using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    /// <summary>
    /// 空值检查
    /// </summary>
    public class GeoIsNullableCheck : IDataCheckRealize
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
            string path = dataCheckParaSet.Workspace.PathName;

            //实现代码
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.DbConn = dataCheckParaSet.DbConnPara;
            pSysTable.DBConType = SysCommon.enumDBConType.OLEDB;
            pSysTable.DBType = SysCommon.enumDBType.ACCESS;

            DataTable mTable = TopologyCheckClass.GetParaValueTable(pSysTable, 2, out eError);
            if (eError != null)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                return;
            }
            if (mTable.Rows.Count == 0)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未进行空值检查配置！");
                return;
            }

             SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<IDataset> LstFeaClass = sysGisDataSet.GetAllFeatureClass();
            if (LstFeaClass.Count == 0) return;

            //执行空值检查
            IsNullCheck(Hook, LstFeaClass, mTable,path, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "空值检查失败。" + eError.Message);
                return;
            }

            #region 
            ////获取所有数据集
            //SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            //List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            //if (featureDatasetNames.Count == 0) return;
            //foreach (string name in featureDatasetNames)
            //{
            //    IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out eError);
            //    if (eError != null) continue;


            //    //空值检查参数ID为2
            //    DataTable mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 2, out eError);
            //    if (eError != null)
            //    {
            //        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
            //        return;
            //    }
            //    if (mTable.Rows.Count == 0)
            //    {
            //        continue;
            //    }
            //    //执行空值检查
            //    IsNullCheck(Hook, featureDataset, mTable, out eError);
            //    if (eError != null)
            //    {
            //        eError=new Exception("空值检查失败。" + eError.Message);
            //        return;
            //    }
            //}
            #endregion
        }


        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "空值检查检查";
            e.ErrInfo.ErrID = enumErrorType.空值检查.GetHashCode();
            DataErrTreat(sender, e);
        }
       
        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="eError"></param>
        private void IsNullCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, DataTable pTable, out Exception eError)
        {
            eError = null;

            //用来保存图层名和合要检查的字段名
            Dictionary<string, List<string>> feaClsInfodic = new Dictionary<string, List<string>>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();   //图层名
                string fieldName = pTable.Rows[i]["字段项"].ToString().Trim();   //要进行检查的字段名
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (fieldName == "")
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    List<string> fieldList = new List<string>();
                    fieldList.Add(fieldName);
                    feaClsInfodic.Add(pFeaClsName, fieldList);
                }
                else
                {
                    if (!feaClsInfodic[pFeaClsName].Contains(fieldName))
                    {
                        feaClsInfodic[pFeaClsName].Add(fieldName);
                    }
                }
            }

            //CommonTopologyCheckClass commonTopologyCheckClass = new CommonTopologyCheckClass();
            //commonTopologyCheckClass.DataErrTreat += new DataErrTreatHandle(DataCheckRealize_DataErrTreat);

            IsNullableCheck(hook,pFeaDataset, feaClsInfodic, out eError);
        }


        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaClsLst"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="eError"></param>
        private void IsNullCheck(IArcgisDataCheckHook hook, List<IDataset> pFeaClsLst, DataTable pTable,string path, out Exception eError)
        {
            eError = null;

            //用来保存图层名和合要检查的字段名
            Dictionary<string, List<string>> feaClsInfodic = new Dictionary<string, List<string>>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();   //图层名
                string fieldName = pTable.Rows[i]["字段项"].ToString().Trim();   //要进行检查的字段名
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (fieldName == "")
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    List<string> fieldList = new List<string>();
                    fieldList.Add(fieldName);
                    feaClsInfodic.Add(pFeaClsName, fieldList);
                }
                else
                {
                    if (!feaClsInfodic[pFeaClsName].Contains(fieldName))
                    {
                        feaClsInfodic[pFeaClsName].Add(fieldName);
                    }
                }
            }
            IsNullableCheck(hook,pFeaClsLst, feaClsInfodic,path, out eError);
        }

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pFeatureClassLst"></param>
        /// <param name="FeaClsNameDic"></param>
        /// <param name="outError"></param>
        public void IsNullableCheck(IArcgisDataCheckHook hook, List<IDataset> pFeatureClassLst, Dictionary<string, List<string>> FeaClsNameDic,string path, out Exception outError)
        {
            outError = null;
            try
            {
                foreach (KeyValuePair<string, List<string>> item in FeaClsNameDic)
                {
                    IFeatureClass pFeaCls = null;
                    foreach (IDataset pFeaClsItem in pFeatureClassLst)
                    {
                        string tempName = pFeaClsItem.Name;
                        if (tempName.Contains("."))
                        {
                            tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                        }
                        if (tempName == item.Key)
                        {
                            pFeaCls = pFeaClsItem as IFeatureClass;
                            break;
                        }
                    }
                    if (pFeaCls == null) continue;
                    //IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(item.Key.Trim());

                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        string fieldName = item.Value[i].Trim();
                        int index = pFeaCls.Fields.FindField(fieldName);
                        if (index == -1)
                        {
                            //outError = new Exception("找不到字段名为" + fieldName + "的字段");
                            //return;
                            continue;
                        }
                        if (pFeaCls.Fields.get_Field(index).IsNullable == true)
                        {
                            //字段属性为空，与标准不一致，将结果保存起来
                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("空值检查");//功能名称
                            ErrorLst.Add(path);  //数据文件名
                            ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                            ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "属性值不允许为空");//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(item.Key);
                            ErrorLst.Add(-1);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                        }
                        else
                        {
                            IFeatureCursor pFeaCusor = pFeaCls.Search(null, false);
                            if (pFeaCusor == null) return;
                            IFeature pFeature = pFeaCusor.NextFeature();
                            if (pFeature == null) continue;
                            while (pFeature != null)
                            {
                                object fieldValue = pFeature.get_Value(index);
                                if (fieldValue == null || fieldValue.ToString() == "")
                                {
                                    //字段值不能为空，将检查结果保存起来
                                    double pMapx = 0.0;
                                    double pMapy = 0.0;
                                    IPoint pPoint = new PointClass();
                                    if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                                    {
                                        pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                                    }
                                    else
                                    {
                                        pPoint = pFeature.Shape as IPoint;
                                    }
                                    pMapx = pPoint.X;
                                    pMapy = pPoint.Y;
                                    List<object> ErrorLst = new List<object>();
                                    ErrorLst.Add("要素属性检查");//功能组名称
                                    ErrorLst.Add("空值检查");//功能名称
                                    ErrorLst.Add(path);  //数据文件名
                                    ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                                    ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "的值不能为空");//错误描述
                                    ErrorLst.Add(pMapx);    //...
                                    ErrorLst.Add(pMapy);    //...
                                    ErrorLst.Add(item.Key);
                                    ErrorLst.Add(pFeature.OID);
                                    ErrorLst.Add("");
                                    ErrorLst.Add(-1);
                                    ErrorLst.Add(false);
                                    ErrorLst.Add(System.DateTime.Now.ToString());

                                    //传递错误日志
                                    IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                    DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                                }
                                pFeature = pFeaCusor.NextFeature();
                            }

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="FeaClsNameDic">图层名和属性非空的字段名对</param>
        /// <param name="outError"></param>
        public void IsNullableCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeatureDataset, Dictionary<string, List<string>> FeaClsNameDic, out Exception outError)
        {
            outError = null;
            try
            {
                //设置进度条
                ProgressChangeEvent eInfo = new ProgressChangeEvent();
                eInfo.Max = FeaClsNameDic.Count;
                int pValue = 0;

                foreach (KeyValuePair<string, List<string>> item in FeaClsNameDic)
                {
                    IFeatureClass pFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(item.Key.Trim());
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        string fieldName = item.Value[i].Trim();
                        int index = pFeaCls.Fields.FindField(fieldName);
                        if (index == -1)
                        {
                            outError = new Exception("找不到字段名为" + fieldName + "的字段");
                            return;
                        }
                        if (pFeaCls.Fields.get_Field(index).IsNullable == true)
                        {
                            //字段属性为空，与标准不一致，将结果保存起来
                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("空值检查");//功能名称
                            ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                            ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                            ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "属性值不允许为空");//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(item.Key);
                            ErrorLst.Add(-1);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                        }
                        else
                        {
                            IFeatureCursor pFeaCusor = pFeaCls.Search(null, false);
                            if (pFeaCusor == null) return;
                            IFeature pFeature = pFeaCusor.NextFeature();
                            if (pFeature == null) continue;
                            while (pFeature != null)
                            {
                                object fieldValue = pFeature.get_Value(index);
                                if (fieldValue == null || fieldValue.ToString() == "")
                                {
                                    //字段值不能为空，将检查结果保存起来
                                    double pMapx = 0.0;
                                    double pMapy = 0.0;
                                    IPoint pPoint = new PointClass();
                                    if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                                    {
                                        pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                                    }
                                    else
                                    {
                                        pPoint = pFeature.Shape as IPoint;
                                    }
                                    pMapx = pPoint.X;
                                    pMapy = pPoint.Y;
                                    List<object> ErrorLst = new List<object>();
                                    ErrorLst.Add("要素属性检查");//功能组名称
                                    ErrorLst.Add("空值检查");//功能名称
                                    ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                                    ErrorLst.Add(enumErrorType.空值检查.GetHashCode());//错误id;
                                    ErrorLst.Add("图层" + item.Key + "的字段" + fieldName + "的值不能为空");//错误描述
                                    ErrorLst.Add(pMapx);    //...
                                    ErrorLst.Add(pMapy);    //...
                                    ErrorLst.Add(item.Key);
                                    ErrorLst.Add(pFeature.OID);
                                    ErrorLst.Add("");
                                    ErrorLst.Add(-1);
                                    ErrorLst.Add(false);
                                    ErrorLst.Add(System.DateTime.Now.ToString());

                                    //传递错误日志
                                    IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                    DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                    DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
                                }
                                pFeature = pFeaCusor.NextFeature();
                            }

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
                        }

                    }


                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
                }
            }
            catch (Exception ex)
            {
                outError = ex;
            }
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
