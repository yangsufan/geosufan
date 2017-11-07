using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Data;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 代码图层检查。批量检查
    /// </summary>
    public class GeoCodeLayerCheck: IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;

        private IArcgisDataCheckHook Hook;
        public GeoCodeLayerCheck()
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
            IArcgisDataCheckParaSet dataCheckParaSet = Hook.DataCheckParaSet as IArcgisDataCheckParaSet;
            if (dataCheckParaSet == null) return;
            if (dataCheckParaSet.Workspace == null) return;

            //获取所有数据集
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet(dataCheckParaSet.Workspace);
            List<IDataset> lstDT = sysGisDataSet.GetAllFeatureClass();
            string path = dataCheckParaSet.Workspace.PathName;
            ExcuteLayerCheck(lstDT, path);
        }

        /// <summary>
        /// 获得分类代码字段名
        /// </summary>
        /// <param name="outErr"></param>
        /// <returns></returns>
        private string GetClassifyName1(out Exception outErr)
        {
            outErr = null;
            Exception eError = null;

            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            if (eError != null)
            {
                outErr = new Exception("连接库体出错！路径为：" + TopologyCheckClass.GeoDataCheckParaPath);
                pSysTable.CloseDbConnection();
                return "";
            }
            string str = "select * from GeoCheckPara where 参数ID=1";//分类代码信息
            DataTable tempDt = pSysTable.GetSQLTable(str, out eError);
            if (eError != null || tempDt.Rows.Count == 0)
            {
                outErr = new Exception("获取分类代码名称信息出错！");
                pSysTable.CloseDbConnection();
                return "";
            }
            pSysTable.CloseDbConnection();
            string pClassifyName = tempDt.Rows[0]["参数值"].ToString();//分类代码字段名
            return pClassifyName;

        }



        /// <summary>
        /// 代码图层检查---检查主函数
        /// </summary>
        private void ExcuteLayerCheck(List<IDataset> LstDataset, string path)
        {
            Exception eError=null;

            string pClassifyName = GetClassifyName1(out eError);
            if (eError != null || pClassifyName == "")
            {
                return;
            }

            //用来存储MapControl上的图层的分类代码的相关信息
            Dictionary<IFeatureClass, Dictionary<string, List<int>>> DicFea = new Dictionary<IFeatureClass, Dictionary<string, List<int>>>();

            foreach (IDataset pDT in LstDataset)
            {
                IFeatureClass pFeatureClass = pDT as IFeatureClass;
                if (pFeatureClass == null) return;
                
                #region 首先检查Mapcontrol上的要素类是否具有分类代码这个字段
                int index = -1;   //分类代码索引
                index = pFeatureClass.Fields.FindField(pClassifyName);
                if (index == -1)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在分类代码字段！");
                    //return;
                    continue;
                }
                #endregion

                #region 将MapControl上的图层相关信息用字典存储起来
                IFeatureCursor pFeaCursor = pFeatureClass.Search(null, false);
                if (pFeaCursor == null) return;
                IFeature pFeature = pFeaCursor.NextFeature();
                if (pFeature == null) continue;
                while (pFeature != null)
                {
                    string pGISID = pFeature.get_Value(index).ToString().Trim();  //分类代码
                    int pOID = pFeature.OID;                   //OID

                    if (!DicFea.ContainsKey(pFeatureClass))
                    {
                        //用来保存GISID和对应的OID
                        Dictionary<string, List<int>> DicCode = new Dictionary<string, List<int>>();
                        List<int> LstOID = new List<int>();
                        LstOID.Add(pOID);
                        DicCode.Add(pGISID, LstOID);
                        DicFea.Add(pFeatureClass, DicCode);
                    }
                    else
                    {
                        if (!DicFea[pFeatureClass].ContainsKey(pGISID))
                        {
                            List<int> LstOID = new List<int>();
                            LstOID.Add(pOID);
                            DicFea[pFeatureClass].Add(pGISID, LstOID);
                        }
                        else
                        {
                            DicFea[pFeatureClass][pGISID].Add(pOID);
                        }
                    }
                    pFeature = pFeaCursor.NextFeature();
                }

                //释放CURSOR
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);

                #endregion

            }

            //设置进度条
            ProgressChangeEvent eInfo = new ProgressChangeEvent();
            eInfo.Max = DicFea.Count;
            int pValue = 0;

            //int pMax = DicFea.Count;
            //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.IntiProgressBar(GeoDataChecker.intiaProgress), new object[] { pMax });

            #region 进行分类代码检查
            //遍历图层
            foreach (KeyValuePair<IFeatureClass, Dictionary<string, List<int>>> FeaCls in DicFea)
            {
                IFeatureClass pFeaCls = FeaCls.Key;
                IDataset pDT = pFeaCls as IDataset;
                if (pDT == null) return;
                string pFeaClsNameStr = pDT.Name.Trim();                 //要素类名称
                if(pFeaClsNameStr.Contains("."))
                {
                    pFeaClsNameStr = pFeaClsNameStr.Substring(pFeaClsNameStr.IndexOf('.') + 1);
                }

                #region 检查分类代码与图层的对应关系
                //遍历分类代码值，进行检查
                foreach (KeyValuePair<string, List<int>> pGISIDItem in FeaCls.Value)
                {
                    //分类代码的值
                    string ppGISID = pGISIDItem.Key;
                    string sqlStr = "select * from GeoCheckCode where 分类代码 ='" + ppGISID + "'";
                    int pResult = CodeStandardizeCheck(sqlStr);                       //检查该分类代码是否存在于模板库中
                    if (pResult == -1)
                    {
                        return;
                    }
                    if (pResult == 1)
                    {
                        //分类代码存在
                        #region 检查分类代码与图层名的对应关系是否正确

                        //若能够找到该分类代码，则检查分类代码对应的图层名是否正确
                        string sqlStr2 = "select * from GeoCheckCode where 分类代码 ='" + ppGISID + "' and 图层='" + pFeaClsNameStr + "'";
                        int pResult2 = CodeStandardizeCheck(sqlStr2);                 //检查分类代码对应的图层名是否正确
                        if (pResult2 == -1)
                        {
                            return;
                        }
                        if (pResult2 == 1)
                        {
                            //对应关系正确
                            continue;

                        }
                        if (pResult2 == 0)
                        {
                            //对应关系不正确

                            #region 保存错误结果
                            //遍历该分类代码对应的要素OID集合
                            for (int m = 0; m < pGISIDItem.Value.Count; m++)
                            {
                                int pOID = pGISIDItem.Value[m];

                                IFeature pFeature = pFeaCls.GetFeature(pOID);
                                IPoint pPoint = ModCommonFunction.GetPntOfFeature(pFeature);
                                double pMapx = 0;// pPoint.X;
                                double pMapy = 0;// pPoint.Y;
                                if(pPoint!=null)
                                {
                                    pMapx = pPoint.X;
                                    pMapy = pPoint.Y;
                                }

                                //用来保存错误结果
                                List<object> ErrorLst = new List<object>();
                                ErrorLst.Add("批量检查");
                                ErrorLst.Add("代码图层检查");
                                ErrorLst.Add(path);
                                ErrorLst.Add(enumErrorType.分类代码与图层名对应关系不正确.GetHashCode());
                                ErrorLst.Add("分类代码"+ppGISID+"与图层"+pFeaClsNameStr+"不对应！");
                                ErrorLst.Add(pMapx);    //...
                                ErrorLst.Add(pMapy);    //...
                                ErrorLst.Add(pFeaClsNameStr);
                                ErrorLst.Add(pOID);
                                ErrorLst.Add("");
                                ErrorLst.Add(-1);
                                ErrorLst.Add(false);
                                ErrorLst.Add(System.DateTime.Now.ToString());

                                //传递错误日志
                                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    if (pResult == 0)
                    {
                        //分类代码不存在
                        #region 保存错误结果
                        //遍历该分类代码对应的要素OID集合
                        for (int m = 0; m < pGISIDItem.Value.Count; m++)
                        {
                            int pOID = pGISIDItem.Value[m];

                            double pMapx = 0.0;
                            double pMapy = 0.0;

                            //用来保存错误结果
                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("批量检查");
                            ErrorLst.Add("代码图层检查");
                            ErrorLst.Add(path);
                            ErrorLst.Add(enumErrorType.分类代码不存在.GetHashCode());
                            ErrorLst.Add("分类代码"+ppGISID+"不存在！");
                            ErrorLst.Add(pMapx);    //...
                            ErrorLst.Add(pMapy);    //...
                            ErrorLst.Add(pFeaClsNameStr);
                            ErrorLst.Add(pOID);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(Hook.DataCheckParaSet as object, dataErrTreatEvent);
                        }
                        #endregion
                    }
                }
                #endregion

                //进度条加1
                pValue++;
                eInfo.Value = pValue;
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.GEODataCheckerProgressShow(GeoDataChecker.GeoDataChecker_ProgressShow), new object[] { (object)GeoDataChecker._ProgressBarInner, eInfo });
                GeoDataChecker.GeoDataChecker_ProgressShow((object)GeoDataChecker._ProgressBarInner, eInfo);
                //GeoDataChecker._CheckForm.Invoke(new GeoDataChecker.ChangeProgressBar(GeoDataChecker.changeProgress), new object[] {pValue});
            }
            #endregion
        }

        /// <summary>
        /// 代码检查---根据条件在模板里面进行检查
        /// </summary>
        /// <param name="sqlStr">查询条件</param>
        /// <param name="pGISID">分类代码</param>
        /// <returns></returns>
        private int CodeStandardizeCheck(string sqlStr)
        {
            Exception Error = null;
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +TopologyCheckClass.GeoDataCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out Error);
            if (Error != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接库体出错！路径为：" + TopologyCheckClass.GeoDataCheckParaPath);
                return -1;
            }
            //pSysTable.DbConn = (Hook.DataCheckParaSet as IArcgisDataCheckParaSet).DbConnPara;
            //pSysTable.DBType = SysCommon.enumDBType.ACCESS;
            //pSysTable.DBConType = SysCommon.enumDBConType.OLEDB;

            DataTable dt = pSysTable.GetSQLTable(sqlStr, out Error);
            if (Error != null)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "打开表格出错！");
                pSysTable.CloseDbConnection();
                return -1;
            }
            if (dt.Rows.Count == 0)
            {
                //在模板里面没有找到该分类代码
                pSysTable.CloseDbConnection();
                return 0;
            }
            else
            {
                pSysTable.CloseDbConnection();
                return 1;
            }
        }
        #endregion

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
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

