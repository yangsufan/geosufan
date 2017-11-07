using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;


namespace GeoDataChecker
{
    /// <summary>
    /// 控制点注记一致性检查
    /// </summary>
    public class GeoControlPointAnnoCheck : IDataCheckRealize
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

            //分类代码字段名
            string codeName = TopologyCheckClass.GetCodeName(pSysTable, out eError);
            if (eError != null)
            {
                //eError=new Exception(eError.Message);
                return;
            }

            //控制点注记检查，参数ID为29
            DataTable mTable = TopologyCheckClass.GetParaValueTable(pSysTable, 29, out eError);
            if (eError != null)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                return;
            }
            if (mTable.Rows.Count == 0)
            {
                //eError = new Exception("未进行等高线注记一致性检查参数配置！");
                return;
            }
            //控制点注记检查搜索半径,参数ID为32
            string paraValue3 = TopologyCheckClass.GetParaValue(pSysTable, 32, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            double serchRadiu3 = Convert.ToDouble(paraValue3);
            //控制点注记检查精度控制，参数ID为35
            paraValue3 = TopologyCheckClass.GetParaValue(pSysTable, 35, out eError);
            if (eError != null)
            {
                eError = new Exception("读取数据减配置表失败。" + eError.Message);
                return;
            }
            long precision3 = Convert.ToInt64(paraValue3);

            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<IDataset> LstFeaClass = sysGisDataSet.GetAllFeatureClass();
            if (LstFeaClass.Count == 0) return;

            //执行控制点注记检查
            PointAnnoCheck(Hook, LstFeaClass, mTable, codeName, serchRadiu3, precision3, out eError);
            if (eError != null)
            {
                eError = new Exception("控制点注记检查失败。" + eError.Message);
                return;
            }

            # region 获取所有数据集
            //SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            //List<string> featureDatasetNames = sysGisDataSet.GetAllFeatureDatasetNames();
            //if (featureDatasetNames.Count == 0) return;
            //foreach (string name in featureDatasetNames)
            //{
            //    IFeatureDataset featureDataset = sysGisDataSet.GetFeatureDataset(name, out eError);
            //    if (eError != null) continue;


            //    //控制点注记检查，参数ID为29
            //    DataTable mTable = TopologyCheckClass.GetParaValueTable(featureDataset, pSysTable, 29, out eError);
            //    if (eError != null)
            //    {
            //        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
            //        return;
            //    }
            //    if (mTable.Rows.Count == 0)
            //    {
            //        continue;
            //    }
            //    //控制点注记检查搜索半径,参数ID为32
            //    string paraValue1 = TopologyCheckClass.GetParaValue(pSysTable, 32, out eError);
            //    if (eError != null)
            //    {
            //        eError=new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    double serchRadiu1 = Convert.ToDouble(paraValue1);
            //    //控制点注记检查精度控制，参数ID为35
            //    paraValue1 = TopologyCheckClass.GetParaValue(pSysTable, 35, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("读取数据减配置表失败。" + eError.Message);
            //        return;
            //    }
            //    long precision1 = Convert.ToInt64(paraValue1);
            //    //执行控制点注记检查
            //    PointAnnoCheck(Hook, featureDataset, mTable, codeName, serchRadiu1, precision1, out eError);
            //    if (eError != null)
            //    {
            //        eError = new Exception("控制点注记检查失败。" + eError.Message);
            //        return;
            //    }
            //}
            #endregion
        }


        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "控制点注记检查";
            e.ErrInfo.ErrID = enumErrorType.控制点注记一致性检查.GetHashCode();
            DataErrTreat(sender, e);
        }
        /// <summary>
        /// 控制点高程注记检查、高程点高程注记检查、等高线高程注记检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="radiu">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="eError"></param>
        private void PointAnnoCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeaDataset, DataTable pTable, string codeName, double radiu, long precision, out Exception eError)
        {
            eError = null;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string FieldName = pTable.Rows[i]["字段项"].ToString().Trim();  //字段名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //检查项
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }
                if ((FieldName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                string oriCodeValue = "";
                string desCodeValue = "";

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();         //源要素类名称
                string desFeaClsName = feaNameArr[1].Trim();         //目标要素类名称
                string[] fieldNameArr = FieldName.Split(new char[] { ';' });
                string oriFieldName = fieldNameArr[0].Trim();        //源高程字段名
                string desFielsName = fieldNameArr[1].Trim();        //目标高程字段名

                if (codeValue != "" && codeValue.Contains(";"))
                {
                    string[] codeValueArr = codeValue.Split(new char[] { ';' });
                    oriCodeValue = codeValueArr[0].Trim();        //源要素类分类代码限制条件
                    desCodeValue = codeValueArr[1].Trim();        //目标要素类分类代码限制条件
                }
                ElevAccordanceCheck(hook, pFeaDataset, codeName, oriFeaClsName, oriCodeValue, oriFieldName, desFeaClsName, desCodeValue, desFielsName, radiu, precision, out eError);
                if (eError != null) return;
            }
        }

       
        /// <summary>
        /// 控制点高程注记检查、高程点高程注记检查、等高线高程注记检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="LstFeaClass"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="radiu">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="eError"></param>
        private void PointAnnoCheck(IArcgisDataCheckHook hook, List<IDataset> LstFeaClass, DataTable pTable, string codeName, double radiu, long precision, out Exception eError)
        {
            eError = null;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string FieldName = pTable.Rows[i]["字段项"].ToString().Trim();  //字段名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //检查项
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }
                if ((FieldName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                string oriCodeValue = "";
                string desCodeValue = "";

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();         //源要素类名称
                string desFeaClsName = feaNameArr[1].Trim();         //目标要素类名称
                string[] fieldNameArr = FieldName.Split(new char[] { ';' });
                string oriFieldName = fieldNameArr[0].Trim();        //源高程字段名
                string desFielsName = fieldNameArr[1].Trim();        //目标高程字段名

                if (codeValue != "" && codeValue.Contains(";"))
                {
                    string[] codeValueArr = codeValue.Split(new char[] { ';' });
                    oriCodeValue = codeValueArr[0].Trim();        //源要素类分类代码限制条件
                    desCodeValue = codeValueArr[1].Trim();        //目标要素类分类代码限制条件
                }

                ElevAccordanceCheck(hook,LstFeaClass, codeName, oriFeaClsName, oriCodeValue, oriFieldName, desFeaClsName, desCodeValue, desFielsName, radiu, precision, out eError);
                if (eError != null) return;
            }
        }


        /// <summary>
        /// 注记高程值一致性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="pFeatureDataset"></param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="oriFeaClsName">源要素类名称</param>
        /// <param name="oriCodeValue">源要素类分类代码值</param>
        /// <param name="oriElevFieldName">源高程值字段名</param>
        /// <param name="desFeaClsName">目标要素类名称</param>
        /// <param name="desCodeValue">目标要素类分类代码值</param>
        /// <param name="labelFieldName">目标要素类高程值</param>
        /// <param name="radius">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="outError"></param>
        public void ElevAccordanceCheck(IArcgisDataCheckHook hook, IFeatureDataset pFeatureDataset, string codeName, string oriFeaClsName, string oriCodeValue, string oriElevFieldName, string desFeaClsName, string desCodeValue, string labelFieldName, double radius, long precision, out Exception outError)
        {
            outError = null;
            try
            {
                //源要素类
                IFeatureClass pOriFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                //源要素类高程字段索引值
                int oriEleIndex = pOriFeaCls.Fields.FindField(oriElevFieldName);
                if (oriEleIndex == -1)
                {
                    outError = new Exception("要素类" + oriFeaClsName + "字段" + oriElevFieldName + "不存在！");
                    return;
                }

                //目标要素类
                IFeatureClass pDesFeaCls = (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                //目标要素类高程字段索引值
                int desElevIndex = pDesFeaCls.Fields.FindField(labelFieldName);
                if (desElevIndex == -1)
                {
                    outError = new Exception("要素类" + desFeaClsName + "字段" + labelFieldName + "不存在！");
                    return;
                }

                //查找源要素类中符合分类代码限制条件的要素
                string whereStr = "";
                if (oriCodeValue != "")
                {
                    whereStr = codeName + " ='" + oriCodeValue + "'";
                }
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;
                IFeatureCursor pCursor = pOriFeaCls.Search(pFilter, false);
                if (pCursor == null) return;
                IFeature pOriFea = pCursor.NextFeature();

                //设置进度条
                ProgressChangeEvent eInfo = new ProgressChangeEvent();
                eInfo.Max = pOriFeaCls.FeatureCount(null);
                int pValue = 0;

                //遍历源要素，进行比较
                while (pOriFea != null)
                {
                    #region 进行检查
                    string oriElevValue = pOriFea.get_Value(oriEleIndex).ToString();
                    //根据原高程值求出精度允许的高程值用于比较
                    if (oriElevValue.Contains("."))
                    {
                        //原高程值包含小数点
                        int oriDotIndex = oriElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            oriElevValue = oriElevValue.Substring(0, oriDotIndex);
                        }
                        else if (oriElevValue.Substring(oriDotIndex + 1).Length > precision && precision > 0)
                        {
                            //原高程值的小数点位数大于精度控制
                            int oriLen = oriDotIndex + 1 + Convert.ToInt32(precision);
                            oriElevValue = oriElevValue.Substring(0, oriLen);
                        }
                    }

                    IFeature desFeature = GetNearestFeature(pDesFeaCls, codeName, desCodeValue, pOriFea, radius, out outError);
                    if (outError != null) return;
                    if (desFeature == null)
                    {
                        pOriFea = pCursor.NextFeature();
                        //进度条加1
                        pValue++;
                        eInfo.Value = pValue;
                        GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);

                        continue;
                    }
                    string desElevValue = desFeature.get_Value(desElevIndex).ToString();
                    if (desElevValue.Contains("."))
                    {
                        //目标高程值包含小数点
                        int desDotIndex = desElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            desElevValue = desElevValue.Substring(0, desDotIndex);
                        }
                        else if (desElevValue.Substring(desDotIndex + 1).Length > precision && precision > 0)
                        {
                            //目标高程值的小数点位数大于精度
                            int desLen = desDotIndex + 1 + Convert.ToInt32(precision);
                            desElevValue = desElevValue.Substring(0, desLen);
                        }
                    }

                    //根据精度进行比较，在容许的范围内不相同不算错误
                    if (Convert.ToDouble(oriElevValue) != Convert.ToDouble(desElevValue))
                    {
                        //说明点，或线与相应注记的高程值不一致。将错误结果显示出来
                        double pMapx = 0.0;
                        double pMapy = 0.0;
                        IPoint pPoint = new PointClass();
                        if (pOriFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                        {
                            pPoint = TopologyCheckClass.GetPointofFeature(pOriFea);
                        }
                        else
                        {
                            //点要素类
                            pPoint = pOriFea.Shape as IPoint;
                        }
                        pMapx = pPoint.X;
                        pMapy = pPoint.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("要素属性检查");//功能组名称
                        ErrorLst.Add("点线注记高程值一致性检查");//功能名称
                        ErrorLst.Add((pFeatureDataset as IDataset).Workspace.PathName);  //数据文件名
                        ErrorLst.Add(0);//错误id;
                        ErrorLst.Add("图层" + oriFeaClsName + "与图层" + desFeaClsName + "高程值不一致");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(oriFeaClsName);
                        ErrorLst.Add(pOriFea.OID);
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desFeature.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);

                    }
                    pOriFea = pCursor.NextFeature();
                    #endregion

                    //进度条加1
                    pValue++;
                    eInfo.Value = pValue;
                    GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }


        /// <summary>
        /// 注记高程值一致性检查
        /// </summary>
        /// <param name="hook"></param>
        /// <param name="feaClsLst"></param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="oriFeaClsName">源要素类名称</param>
        /// <param name="oriCodeValue">源要素类分类代码值</param>
        /// <param name="oriElevFieldName">源高程值字段名</param>
        /// <param name="desFeaClsName">目标要素类名称</param>
        /// <param name="desCodeValue">目标要素类分类代码值</param>
        /// <param name="labelFieldName">目标要素类高程值</param>
        /// <param name="radius">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="outError"></param>
        public void ElevAccordanceCheck(IArcgisDataCheckHook hook, List<IDataset> feaClsLst, string codeName, string oriFeaClsName, string oriCodeValue, string oriElevFieldName, string desFeaClsName, string desCodeValue, string labelFieldName, double radius, long precision, out Exception outError)
        {
            outError = null;
            try
            {
                //源要素类
                IFeatureClass pOriFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(oriFeaClsName);
                foreach (IDataset  pDT in feaClsLst)
                {
                    string tempName = pDT.Name;
                    if(tempName.Contains("."))
                    {
                        tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                    }
                    if (tempName == oriFeaClsName)
                    {
                        pOriFeaCls = pDT as IFeatureClass;
                        break;
                    }
                }
                if (pOriFeaCls == null) return;
                //源要素类高程字段索引值
                int oriEleIndex = pOriFeaCls.Fields.FindField(oriElevFieldName);
                if (oriEleIndex == -1)
                {
                    outError = new Exception("要素类" + oriFeaClsName + "字段" + oriElevFieldName + "不存在！");
                    return;
                }

                //目标要素类
                IFeatureClass pDesFeaCls = null;// (pFeatureDataset.Workspace as IFeatureWorkspace).OpenFeatureClass(desFeaClsName);
                foreach (IDataset pDT in feaClsLst)
                {
                    string tempName = pDT.Name;
                    if (tempName.Contains("."))
                    {
                        tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                    }
                    if (tempName == desFeaClsName)
                    {
                        pDesFeaCls = pDT as IFeatureClass;
                        break;
                    }
                }
                if (pDesFeaCls == null) return;
                //目标要素类高程字段索引值
                int desElevIndex = pDesFeaCls.Fields.FindField(labelFieldName);
                if (desElevIndex == -1)
                {
                    outError = new Exception("要素类" + desFeaClsName + "字段" + labelFieldName + "不存在！");
                    return;
                }

                //查找源要素类中符合分类代码限制条件的要素
                string whereStr = "";
                if (oriCodeValue != "")
                {
                    whereStr = codeName + " ='" + oriCodeValue + "'";
                }
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereStr;
                IFeatureCursor pCursor = pOriFeaCls.Search(pFilter, false);
                if (pCursor == null) return;
                IFeature pOriFea = pCursor.NextFeature();

                //遍历源要素，进行比较
                while (pOriFea != null)
                {
                    #region 进行检查
                    string oriElevValue = pOriFea.get_Value(oriEleIndex).ToString();
                    //根据原高程值求出精度允许的高程值用于比较
                    if (oriElevValue.Contains("."))
                    {
                        //原高程值包含小数点
                        int oriDotIndex = oriElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            oriElevValue = oriElevValue.Substring(0, oriDotIndex);
                        }
                        else if (oriElevValue.Substring(oriDotIndex + 1).Length > precision && precision > 0)
                        {
                            //原高程值的小数点位数大于精度控制
                            int oriLen = oriDotIndex + 1 + Convert.ToInt32(precision);
                            oriElevValue = oriElevValue.Substring(0, oriLen);
                        }
                    }

                    IFeature desFeature = GetNearestFeature(pDesFeaCls, codeName, desCodeValue, pOriFea, radius, out outError);
                    if (outError != null) return;
                    if (desFeature == null)
                    {
                        pOriFea = pCursor.NextFeature();
                        continue;
                    }
                    string desElevValue = desFeature.get_Value(desElevIndex).ToString();
                    if (desElevValue.Contains("."))
                    {
                        //目标高程值包含小数点
                        int desDotIndex = desElevValue.IndexOf('.');
                        if (precision == 0)
                        {
                            desElevValue = desElevValue.Substring(0, desDotIndex);
                        }
                        else if (desElevValue.Substring(desDotIndex + 1).Length > precision && precision > 0)
                        {
                            //目标高程值的小数点位数大于精度
                            int desLen = desDotIndex + 1 + Convert.ToInt32(precision);
                            desElevValue = desElevValue.Substring(0, desLen);
                        }
                    }

                    //根据精度进行比较，在容许的范围内不相同不算错误
                    if (Convert.ToDouble(oriElevValue) != Convert.ToDouble(desElevValue))
                    {
                        //说明点，或线与相应注记的高程值不一致。将错误结果显示出来
                        double pMapx = 0.0;
                        double pMapy = 0.0;
                        IPoint pPoint = new PointClass();
                        if (pOriFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                        {
                            pPoint = TopologyCheckClass.GetPointofFeature(pOriFea);
                        }
                        else
                        {
                            //点要素类
                            pPoint = pOriFea.Shape as IPoint;
                        }
                        pMapx = pPoint.X;
                        pMapy = pPoint.Y;

                        List<object> ErrorLst = new List<object>();
                        ErrorLst.Add("要素属性检查");//功能组名称
                        ErrorLst.Add("点线注记高程值一致性检查");//功能名称
                        ErrorLst.Add("");  //数据文件名
                        ErrorLst.Add(0);//错误id;
                        ErrorLst.Add("图层" + oriFeaClsName + "与图层" + desFeaClsName + "高程值不一致");//错误描述
                        ErrorLst.Add(pMapx);    //...
                        ErrorLst.Add(pMapy);    //...
                        ErrorLst.Add(oriFeaClsName);
                        ErrorLst.Add(pOriFea.OID);
                        ErrorLst.Add(desFeaClsName);
                        ErrorLst.Add(desFeature.OID);
                        ErrorLst.Add(false);
                        ErrorLst.Add(System.DateTime.Now.ToString());

                        //传递错误日志
                        IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                        DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                        DataErrTreat(hook.DataCheckParaSet as object, dataErrTreatEvent);

                    }
                    pOriFea = pCursor.NextFeature();
                    #endregion
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
        }



        /// <summary>
        /// 在要素类上查找距离最近的要素
        /// </summary>
        /// <param name="desFeaCls">目标要素类</param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="codeValue">分类代码值</param>
        /// <param name="oriFeature">源要素</param>
        /// <param name="radiu">缓冲半径</param>
        /// <param name="outError"></param>
        /// <returns></returns>
        private IFeature GetNearestFeature(IFeatureClass desFeaCls, string codeName, string codeValue, IFeature oriFeature, double radiu, out Exception outError)
        {
            outError = null;
            IFeature returnFeature = null;
            double pDistance = -1;
            try
            {
                //根据源要素的缓冲半径得到缓冲范围
                ITopologicalOperator pTopoOper = oriFeature.Shape as ITopologicalOperator;
                IGeometry pGeo = pTopoOper.Buffer(radiu);

                //查找目标要素类中给定分类代码的缓冲范围内的要素
                string str = "";
                if (codeValue != "")
                {
                    str = codeName + " = '" + codeValue + "'";
                }
                ISpatialFilter pFilter = new SpatialFilterClass();
                pFilter.WhereClause = str;
                pFilter.GeometryField = "SHAPE";
                pFilter.Geometry = pGeo;
                pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//.esriSpatialRelWithin;
                IFeatureCursor pCursor = desFeaCls.Search(pFilter, false);
                if (pCursor == null) return null;
                IFeature pFeature = pCursor.NextFeature();

                //遍历要素，查找距离最近的要素
                while (pFeature != null)
                {
                    IProximityOperator pProxiOper = oriFeature.Shape as IProximityOperator;
                    double tempDis = pProxiOper.ReturnDistance(pFeature.Shape);
                    if (pDistance == -1 || pDistance > tempDis)
                    {
                        pDistance = tempDis;
                        returnFeature = pFeature;
                    }
                    pFeature = pCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                outError = ex;
            }
            return returnFeature;
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
