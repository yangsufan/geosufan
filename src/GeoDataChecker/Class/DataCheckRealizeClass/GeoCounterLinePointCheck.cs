using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    /// <summary>
    /// 等高悬点线矛盾一致性检查
    /// </summary>
    public class GeoCounterLinePointCheck : IDataCheckRealize
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

            //高程点图层,参数ID为19(还需要改进）
            string pointFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 19, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            //等高线图层,参数ID为20
            string lineFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 20, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            //高程点高程字段名,参数ID为22
            string pointFieldsname = TopologyCheckClass.GetParaValue(pSysTable, 22, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            //等高线高程字段名,参数ID为23
            string lineFieldname = TopologyCheckClass.GetParaValue(pSysTable, 23, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            //等高线间距值,参数ID为21
            string intervalValue = TopologyCheckClass.GetParaValue(pSysTable, 21, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            //高程点搜索半径,参数ID为38
            string radiu = TopologyCheckClass.GetParaValue(pSysTable, 38, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }

            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<IDataset> LstFeaClass = sysGisDataSet.GetAllFeatureClass();
            if (LstFeaClass.Count == 0) return;

            //执行等高线点线矛盾检查
            PointLineElevCheck(Hook,LstFeaClass, lineFeaclsname, lineFieldname, pointFeaclsname, pointFieldsname, Convert.ToDouble(intervalValue), out eError);
            if (eError != null)
            {
                eError = new Exception("等高线点线矛盾检查失败！" + eError.Message);
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

            //    //高程点图层,参数ID为19(还需要改进）
            //    string pointFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 19, out eError);
            //    if (eError != null)
            //    {
            //        eError=new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //等高线图层,参数ID为20
            //    string lineFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 20, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //高程点高程字段名,参数ID为22
            //    string pointFieldsname = TopologyCheckClass.GetParaValue(pSysTable, 22, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //等高线高程字段名,参数ID为23
            //    string lineFieldname = TopologyCheckClass.GetParaValue(pSysTable, 23, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //等高线间距值,参数ID为21
            //    string intervalValue = TopologyCheckClass.GetParaValue(pSysTable, 21, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //高程点搜索半径,参数ID为38
            //    string radiu = TopologyCheckClass.GetParaValue(pSysTable, 38, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    //执行等高线点线矛盾检查
            //    PointLineElevCheck(Hook,featureDataset, lineFeaclsname, lineFieldname, pointFeaclsname, pointFieldsname, Convert.ToDouble(intervalValue), out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("等高线点线矛盾检查失败！" + eError.Message);
            //        return;
            //    }
            //}

            #endregion
        }


        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "高程点注记检查";
            e.ErrInfo.ErrID = enumErrorType.高程点注记一致性检查.GetHashCode();
            DataErrTreat(sender, e);
        }

        /// <summary>
        /// 等高线点线矛盾一致性检查
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="lineFeaClsName">等高线要素类名称</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="pointFeaClsName">高程点要素类名称</param>
        /// <param name="pointFieldName">高程点字段名</param>
        /// <param name="radiu">高程点搜索半径</param>
        /// <param name="intervalValue">等高线间距值</param>
        /// <param name="eError"></param>
        private void PointLineElevCheck(IArcgisDataCheckHook hook,IFeatureDataset pFeatureDataset, string lineFeaClsName, string lineFieldName, string pointFeaClsName, string pointFieldName, double intervalValue, out Exception eError)
        {
            eError = null;
            try
            {
                //等高线要素类
                IFeatureClass lineFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(lineFeaClsName);
                //等高线高程字段索引值
                int lineIndex = lineFeaCls.Fields.FindField(lineFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("等高线图层的高程字段不存在,字段名为：" + lineFieldName);
                    return;
                }
                //高程点要素类
                IFeatureClass pointFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(pointFeaClsName);
                int pointIndex = pointFeaCls.Fields.FindField(pointFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("高程点图层的高程字段不存在,字段名为：" + pointFieldName);
                    return;
                }

                //遍历高程点要素
                IFeatureCursor pCusor = pointFeaCls.Search(null, false);
                if (pCusor == null) return;
                IFeature pointFeature = pCusor.NextFeature();

                //设置进度条
                ProgressChangeEvent eInfo = new ProgressChangeEvent();
                eInfo.Max = pointFeaCls.FeatureCount(null);
                int pValue = 0;

                while (pointFeature != null)
                {
                    //高程点要素的高程值
                    double pointElevValue = Convert.ToDouble(pointFeature.get_Value(pointIndex).ToString());
                    //查找高程点相邻的两条高程线要素

                    //与高程点最近的等高线要素以及最短距离
                    Dictionary<double, IFeature> nearestFeaDic = GetShortestDis(lineFeaCls, pointFeature, out eError);
                    if (eError != null || nearestFeaDic == null)
                    {
                        eError = new Exception("在搜索范围内的未找到要素!");
                        return;
                    }
                    double pShortestDis = -1;
                    IFeature nearestFea = null;
                    foreach (KeyValuePair<double, IFeature> item in nearestFeaDic)
                    {
                        pShortestDis = item.Key;
                        nearestFea = item.Value;
                        break;
                    }
                    if (eError != null || pShortestDis == -1) return;
                    //获得等高线上离高程点最近的点
                    IPoint nearestPoint = new PointClass();//等高线上的最近点
                    IProximityOperator mProxiOpe = nearestFea.Shape as IProximityOperator;
                    if (mProxiOpe == null) return;
                    nearestPoint = mProxiOpe.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                    //将高程点和等高线上的点连成线段并进行两端延长

                    PointLineAccordanceCheck2(hook,pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineFieldName, lineIndex, pointIndex, nearestFea, pShortestDis, intervalValue, out eError);
                    //PointLineAccordanceCheck(pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineIndex, pointIndex, pShortestDis, nearestFea, intervalValue, out eError);
                    if (eError != null) return;
                    pointFeature = pCusor.NextFeature();

                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }


        /// <summary>
        /// 等高线点线矛盾一致性检查
        /// </summary>
        /// <param name="feaClsLst"></param>
        /// <param name="lineFeaClsName">等高线要素类名称</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="pointFeaClsName">高程点要素类名称</param>
        /// <param name="pointFieldName">高程点字段名</param>
        /// <param name="radiu">高程点搜索半径</param>
        /// <param name="intervalValue">等高线间距值</param>
        /// <param name="eError"></param>
        public void PointLineElevCheck(IArcgisDataCheckHook hook,List<IDataset> feaClsLst, string lineFeaClsName, string lineFieldName, string pointFeaClsName, string pointFieldName, double intervalValue, out Exception eError)
        {
            eError = null;
            try
            {
                //等高线要素类
                IFeatureClass lineFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(lineFeaClsName);
                foreach (IDataset pDT in feaClsLst)
                {
                    string tempName = pDT.Name;
                    if(tempName.Contains("."))
                    {
                        tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                    }
                    if (tempName == lineFeaClsName)
                    {
                        lineFeaCls = pDT as IFeatureClass;
                        break;
                    }
                }
                if (lineFeaCls == null) return;
                //等高线高程字段索引值
                int lineIndex = lineFeaCls.Fields.FindField(lineFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("等高线图层的高程字段不存在,字段名为：" + lineFieldName);
                    return;
                }
                //高程点要素类
                IFeatureClass pointFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(pointFeaClsName);
                foreach (IDataset pDT in feaClsLst)
                {
                    string tempName = pDT.Name;
                    if (tempName.Contains("."))
                    {
                        tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                    }
                    if (tempName == pointFeaClsName)
                    {
                        pointFeaCls = pDT as IFeatureClass;
                        break;
                    }
                }
                if (pointFeaCls == null) return;
                int pointIndex = pointFeaCls.Fields.FindField(pointFieldName);
                if (lineIndex == -1)
                {
                    eError = new Exception("高程点图层的高程字段不存在,字段名为：" + pointFieldName);
                    return;
                }

                //遍历高程点要素
                IFeatureCursor pCusor = pointFeaCls.Search(null, false);
                if (pCusor == null) return;
                IFeature pointFeature = pCusor.NextFeature();

                while (pointFeature != null)
                {
                    //高程点要素的高程值
                    double pointElevValue = Convert.ToDouble(pointFeature.get_Value(pointIndex).ToString());
                    //查找高程点相邻的两条高程线要素

                    //与高程点最近的等高线要素以及最短距离
                    Dictionary<double, IFeature> nearestFeaDic = GetShortestDis(lineFeaCls, pointFeature, out eError);
                    if (eError != null || nearestFeaDic == null)
                    {
                        eError = new Exception("在搜索范围内的未找到要素!");
                        return;
                    }
                    double pShortestDis = -1;
                    IFeature nearestFea = null;
                    foreach (KeyValuePair<double, IFeature> item in nearestFeaDic)
                    {
                        pShortestDis = item.Key;
                        nearestFea = item.Value;
                        break;
                    }
                    if (eError != null || pShortestDis == -1) return;
                    //获得等高线上离高程点最近的点
                    IPoint nearestPoint = new PointClass();//等高线上的最近点
                    IProximityOperator mProxiOpe = nearestFea.Shape as IProximityOperator;
                    if (mProxiOpe == null) return;
                    nearestPoint = mProxiOpe.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                    //将高程点和等高线上的点连成线段并进行两端延长

                    PointLineAccordanceCheck2(hook,null, lineFeaCls, pointFeaCls, pointFeature, lineFieldName, lineIndex, pointIndex, nearestFea, pShortestDis, intervalValue, out eError);
                    //PointLineAccordanceCheck(pFeatureDataset, lineFeaCls, pointFeaCls, pointFeature, lineIndex, pointIndex, pShortestDis, nearestFea, intervalValue, out eError);
                    if (eError != null) return;
                    pointFeature = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }

        /// <summary>
        /// 等高线高程点点线矛盾检查,还需要完善
        /// </summary>
        /// <param name="pFeaDataset"></param>
        /// <param name="desFeaCls">等高线要素类</param>
        /// <param name="oriFeaCls">高程点要素类</param>
        /// <param name="pointFeature">高程点要素</param>
        /// <param name="lineFieldName">等高线高程字段名</param>
        /// <param name="lineIndex">等高线高程字段索引</param>
        /// <param name="pointIndex">高程点高程字段民</param>
        /// <param name="nearestFea">离高程点要素最近的等高线要素</param>
        /// <param name="intervalElev">等高线间距值</param>
        /// <param name="eError"></param>
        private void PointLineAccordanceCheck2(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, IFeatureClass desFeaCls, IFeatureClass oriFeaCls, IFeature pointFeature, string lineFieldName, int lineIndex, int pointIndex, IFeature nearestFea, double pShortestDis, double intervalElev, out Exception eError)
        {
            eError = null;

            try
            {
                double pValue = 0.0;                                       //点要素的高程值
                double fValue = 0.0;                                       //最近线要素高程值
                double lValue = 0.0;                                        //最近线要素相邻的第一条要素
                double sValue = 0.0;                                       //最近线要素相邻的第二条要素
                int eErrorID = enumErrorType.等高线点线矛盾检查.GetHashCode(); //错误ID
                string eErrorDes = "等高线与高程点高程值点线矛盾！";       //错误描述

                //点要素的高程值
                string pValueStr = pointFeature.get_Value(pointIndex).ToString().Trim();
                if (pValueStr == "")
                {
                    eError = new Exception("高程点的高程值为空，OID为：" + pointFeature.OID);
                    return;
                }
                pValue = Convert.ToDouble(pValueStr);
                //最近线要素高程值
                string fValueStr = nearestFea.get_Value(lineIndex).ToString().Trim();
                if (fValueStr == "")
                {
                    eError = new Exception("线要素的高程值为空。OID为：" + nearestFea.OID);
                    return;
                }
                fValue = Convert.ToDouble(fValueStr);

                if (pShortestDis == 0)
                {
                    //说明点在线上
                    if (pValue != fValue)
                    {
                        //点线高程值矛盾
                        GetErrorList(hook,pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                        if (eError != null) return;
                    }
                }
                else
                {
                    if (fValue < intervalElev)
                    {
                        #region 说明是最小高程,说明该要素是最里面或最外面的要素
                        //最近线要素相邻的第一条要素的高程值
                        lValue = fValue + intervalElev;
                        string whereStr = lineFieldName + "=" + lValue;
                        List<IFeature> lstFefa = GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null || lstFefa == null || lstFefa.Count == 0)
                        {
                            return;
                        }
                        //bool isIntersact = false;

                        //for (int k = 0; k < lstFefa.Count; k++)
                        //{
                        if (lstFefa.Count == 1)
                        {
                            IFeature secondFea = lstFefa[0];
                            //返回secondFea上与高程点最近的点
                            IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                            IPoint p = new PointClass();
                            p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            //    if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            //    {
                            //        isIntersact = true;
                            //        secondFea = lstFefa[k];
                            //        break;
                            //    }
                            //}

                            #region 最近线要素相邻的第一条要素上点与高程点连线与最近线要素的拓扑关系包含两种情况：相交、不相交。
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //若两点连线与nearestFea相交，说明高程点在线要素的最里面或者最外面
                                //由于fValue<lValue
                                if (pValue > fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            else
                            {
                                //若两点连线与nearestFea不相交，高程点在两条线要素之间
                                if (pValue < fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > lValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                    else if (fValue >= intervalElev)
                    {
                        #region 分为两种情况：或者是最里面或最外面的线要素、或者是中间线要素
                        //与nearestFea相邻的两线要素的高程值
                        lValue = fValue + intervalElev;
                        sValue = fValue - intervalElev;
                        //与最近要素要素的两个要素
                        IFeature secondFea = null;
                        IFeature thirdFea = null;

                        string whereStr = lineFieldName + "=" + lValue;

                        List<IFeature> lstFea2 = GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null || lstFea2 == null)
                        {
                            return;
                        }
                        whereStr = lineFieldName + "=" + sValue;
                        List<IFeature> lstFea3 = GetFeatureByStr(desFeaCls, whereStr, out eError);
                        if (eError != null || lstFea3 == null)
                        {
                            return;
                        }

                        if (lstFea2.Count == 0 && lstFea3.Count == 0)
                        {
                            //只有一条等高线
                            eError = new Exception("只有一条等高线，不能进行检查！");
                            return;
                        }
                        else if (lstFea2.Count == 1 && lstFea3.Count == 1)
                        {
                            secondFea = lstFea2[0];
                            thirdFea = lstFea3[0];
                            #region nearestFea不是最里面或最外面的要素
                            IPoint p = new PointClass();
                            IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                            p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //相交。sValue<fValue<lValue ,pValue应位于sValue与fValue之间
                                if (pValue < sValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, thirdFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            else
                            {
                                //不相交。sValue<fValue<lValue ,pValue应位于fValue与lValue之间
                                if (pValue < fValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                                if (pValue > lValue)
                                {
                                    //点线高程值矛盾
                                    GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                    if (eError != null) return;
                                }
                            }
                            #endregion
                        }
                        else if (lstFea2.Count == 1 || lstFea3.Count == 1)
                        {
                            if (lstFea2.Count == 1 && lstFea3.Count == 0)
                            {
                                secondFea = lstFea2[0];
                            }
                            else if (lstFea3.Count == 1 && lstFea2.Count == 0)
                            {
                                thirdFea = lstFea3[0];
                            }
                            if (secondFea == null && thirdFea == null) return;

                            #region  nearestFea是最里面或最外面的要素
                            IPoint p = new PointClass();
                            if (secondFea != null)
                            {
                                IProximityOperator pProxiOper = secondFea.Shape as IProximityOperator;
                                p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            }
                            if (thirdFea != null)
                            {
                                IProximityOperator pProxiOper = thirdFea.Shape as IProximityOperator;
                                p = pProxiOper.ReturnNearestPoint(pointFeature.Shape as IPoint, esriSegmentExtension.esriNoExtension);
                            }
                            if (IsIntersect(pointFeature.Shape as IPoint, p, nearestFea))
                            {
                                //相交。高程点在等高线的最外面或最里面
                                if (secondFea != null)
                                {
                                    //由于fValue<lValue
                                    if (pValue > fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                                if (thirdFea != null)
                                {
                                    //由于fValue>sValue
                                    if (pValue < fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                            }
                            else
                            {
                                //不相交。高程点在等高线的最靠边的两条等高线之间
                                if (secondFea != null)
                                {
                                    //由于fValue<lValue
                                    if (pValue < fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                    if (pValue > lValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, secondFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                                if (thirdFea != null)
                                {
                                    //由于fValue>sValue
                                    if (pValue > fValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, nearestFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                    if (pValue < sValue)
                                    {
                                        //点线高程值矛盾
                                        GetErrorList(hook, pFeaDataset, oriFeaCls, pointFeature, desFeaCls, thirdFea, eErrorID, eErrorDes, out eError);
                                        if (eError != null) return;
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }


        /// <summary>
        /// 获得错误列表，等高线点线矛盾
        /// </summary>
        /// <param name="pFeatureDataset"></param>
        /// <param name="pFeatureCls"></param>
        /// <param name="feaClsname"></param>
        /// <param name="pFeature"></param>
        /// <param name="eErrorID"></param>
        /// <param name="eError"></param>
        private void GetErrorList(IArcgisDataCheckHook hook, IFeatureDataset pFeatureDataset, IFeatureClass pOriFeatureCls, IFeature pOriFeature, IFeatureClass pDesFeatureCls, IFeature pDesFeature, int eErrorID, string eErrorDes, out Exception eError)
        {
            eError = null;
            try
            {
                double pMapx = 0.0;
                double pMapy = 0.0;
                IPoint pPoint = null;
                //高程值不在给定的高程范围内,将错误结果保存下来
                if (pOriFeatureCls.ShapeType != esriGeometryType.esriGeometryPoint)
                {
                    pPoint = TopologyCheckClass.GetPointofFeature(pOriFeature);
                }
                else
                {
                    pPoint = pOriFeature.Shape as IPoint;
                }
                pMapx = pPoint.X;
                pMapy = pPoint.Y;

                List<object> ErrorLst = new List<object>();
                ErrorLst.Add("要素属性检查");//功能组名称
                ErrorLst.Add("等高线点线矛盾检查");//功能名称
                if (pFeatureDataset == null)
                {
                    ErrorLst.Add("");  //数据文件名
                }
                else
                {
                    ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                }

                ErrorLst.Add(eErrorID);//错误id;
                ErrorLst.Add(eErrorDes);//错误描述
                ErrorLst.Add(pMapx);    //...
                ErrorLst.Add(pMapy);    //...
                ErrorLst.Add((pOriFeatureCls as IDataset).Name);
                ErrorLst.Add(pOriFeature.OID);
                ErrorLst.Add((pDesFeatureCls as IDataset).Name);
                ErrorLst.Add(pDesFeature.OID);
                ErrorLst.Add(false);
                ErrorLst.Add(System.DateTime.Now.ToString());

                //传递错误日志
                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);
            }
            catch (Exception ex)
            {
                eError = ex;
            }
        }



        /// <summary>
        /// 点要素与目标要素的最短距离
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="pointFeature">点要素</param>
        /// <param name="radiu">点要素搜索半径</param>
        /// <param name="outError"></param>
        /// <returns></returns>
        private Dictionary<double, IFeature> GetShortestDis(IFeatureClass desFeaCls, IFeature pointFeature, out Exception outError)
        {
            outError = null;
            Dictionary<double, IFeature> ShortestFeature = new Dictionary<double, IFeature>();
            double pShortestDistance = -1;
            IFeature pFeature = null;
            try
            {
                //List<IFeature> tempFeaLst = new List<IFeature>();
                //tempFeaLst = GetFeatureByDis(desFeaCls, pointFeature,esriSpatialRelEnum.esriSpatialRelIntersects, radiu);
                IFeatureCursor pCusor = desFeaCls.Search(null, false);
                if (pCusor == null) return null;
                IFeature mFea = pCusor.NextFeature();
                //遍历要素，查找距离最近的要素,
                while (mFea != null)
                {
                    IProximityOperator pProxiOper = pointFeature.Shape as IProximityOperator;
                    double tempDis = pProxiOper.ReturnDistance(mFea.Shape);
                    if (pShortestDistance == -1 || pShortestDistance > tempDis)
                    {
                        pShortestDistance = tempDis;
                        pFeature = mFea;
                    }
                    mFea = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);

                if (pShortestDistance == -1 || pFeature == null)
                {
                    outError = new Exception("未找到任何要素!");
                    return null;
                }
                ShortestFeature.Add(pShortestDistance, pFeature);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
            return ShortestFeature;
        }

        /// <summary>
        /// 两点之间的连线是否与线相交
        /// </summary>
        /// <param name="mFPoint"></param>
        /// <param name="mLPoint"></param>
        /// <param name="lineFeature"></param>
        /// <returns>true:相交;false:不相交</returns>
        private bool IsIntersect(IPoint mFPoint, IPoint mLPoint, IFeature lineFeature)
        {
            //声明线要素
            //IPolyline mPLine = new PolylineClass();
            //mPLine.FromPoint = mFPoint;
            //mPLine.ToPoint = mLPoint;
            //声明点集
            //IPointCollection mPointCol = new PolylineClass();
            //object obj = System.Reflection.Missing.Value;
            //mPointCol.AddPoint(mFPoint, ref obj, ref obj);
            //mPointCol.AddPoint(mLPoint, ref obj, ref obj);
            //mPLine = mPointCol as IPolyline;

            ILine pLine = new LineClass();
            pLine.PutCoords(mFPoint, mLPoint);
            ISegmentCollection pSegCol = new PolylineClass();
            object obj = Type.Missing;
            pSegCol.AddSegment(pLine as ISegment, ref obj, ref obj);
            IRelationalOperator pRelOpera = pSegCol as IRelationalOperator;
            if (pRelOpera.Disjoint(lineFeature.Shape))
            {
                //不相交
                return false;
            }
            else
            {
                //相交
                return true;
            }
        }

        /// <summary>
        /// 根据指定的条件获得要素
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="whereStr"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private List<IFeature> GetFeatureByStr(IFeatureClass desFeaCls, string whereStr, out Exception eError)
        {
            eError = null;
            List<IFeature> LstFeature = new List<IFeature>();
            try
            {
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;

                IFeatureCursor pCusor = desFeaCls.Search(pFilter, false);
                if (pCusor == null) return null;
                IFeature pFea = pCusor.NextFeature();
                while (pFea != null)
                {
                    LstFeature.Add(pFea);
                    pFea = pCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCusor);
                return LstFeature;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 根据要素的缓冲距离查找要素
        /// </summary>
        /// <param name="desFeaCls"></param>
        /// <param name="pointFeature"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        private List<IFeature> GetFeatureByDis(IFeatureClass desFeaCls, IFeature pointFeature, esriSpatialRelEnum spatialRelEnum, double dis)
        {
            List<IFeature> LstFeature = new List<IFeature>();
            //根据源要素的缓冲半径得到缓冲范围
            ITopologicalOperator pTopoOper = pointFeature.Shape as ITopologicalOperator;
            IGeometry pGeo = pTopoOper.Buffer(dis);

            //查找缓冲范围内的要素
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.GeometryField = "SHAPE";
            pFilter.Geometry = pGeo;
            pFilter.SpatialRel = spatialRelEnum;// esriSpatialRelEnum.esriSpatialRelIntersects;//.esriSpatialRelWithin;
            IFeatureCursor pCursor = desFeaCls.Search(pFilter, false);
            if (pCursor == null) return null;
            IFeature pFeature = pCursor.NextFeature();
            while (pFeature != null)
            {
                LstFeature.Add(pFeature);
                pFeature = pCursor.NextFeature();
            }

            //释放cursor
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            return LstFeature;
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
