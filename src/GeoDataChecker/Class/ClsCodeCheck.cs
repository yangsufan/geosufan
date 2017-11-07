using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Data.Common;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    public class ClsCodeCheck : GOGISErrorChecker
    {
        private Plugin.Application.IAppGISRef _AppHk;
        string m_TempletePath = "";                         //数据库标准模板文件路径
        List<IFeatureLayer> m_LstFeaLayer = null;           //源数据所有图层

        private DbConnection _ErrDbCon = null;                       //错误日志表连接
        public DbConnection ErrDbCon
        {
            get
            {
                return _ErrDbCon;
            }
            set
            {
                _ErrDbCon = value;
            }
        }
        private string _ErrTableName = "";                            //错误日志表名
        public string ErrTableName
        {
            get
            {
                return _ErrTableName;
            }
            set
            {
                _ErrTableName = value;
            }
        }

        public event DataErrTreatHandle DataErrTreat;

        private DataTable _ResultTable = new DataTable();

        public ClsCodeCheck(Plugin.Application.IAppGISRef pAppHk, string path, List<IFeatureLayer> LstFeaLayer)
        {
            CreateTable();
            m_TempletePath = path;
            _AppHk = pAppHk;
            m_LstFeaLayer = LstFeaLayer;

            if (_AppHk.DataCheckGrid.RowCount > 0)
            {
                _AppHk.DataCheckGrid.DataSource = null;
            }
            _AppHk.DataCheckGrid.DataSource = _ResultTable;
            _AppHk.DataCheckGrid.SelectionMode=DataGridViewSelectionMode.FullRowSelect;
            DataErrTreat += new DataErrTreatHandle(GeoDataChecker_DataErrTreat);
        }

         /// <summary>
        /// 分类代码检查---检查主函数
        /// </summary>
        public void ExcuteCheck()
        {
            Exception eError = null;

            string pClassifyName = GetClassifyName1(out eError);
            if (eError != null || pClassifyName == "")
            {
                return;
            }

            //用来存储MapControl上的图层的分类代码的相关信息
            Dictionary<string, Dictionary<string, List<long>>> DicFea = new Dictionary<string, Dictionary<string, List<long>>>();
            foreach (IFeatureLayer pFeaLayer in m_LstFeaLayer)
            {
                
                IFeatureClass pFeatureClass = pFeaLayer.FeatureClass;
                IDataset pDT = pFeatureClass as IDataset;
                if (pDT == null) return;
                string pFeaClsName = pDT.Name.Trim();
              

                #region 首先检查Mapcontrol上的要素类是否具有分类代码这个字段
                int index = -1;   //分类代码索引
                index = pFeatureClass.Fields.FindField(pClassifyName);
                if (index == -1)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在分类代码字段！");
                    return;
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

                    if (!DicFea.ContainsKey(pFeaClsName))
                    {
                        //用来保存GISID和对应的OID
                        Dictionary<string, List<long>> DicCode = new Dictionary<string, List<long>>();
                        List<long> LstOID = new List<long>();
                        LstOID.Add(pOID);
                        DicCode.Add(pGISID, LstOID);
                        DicFea.Add(pFeaClsName, DicCode);
                    }
                    else
                    {
                        if (!DicFea[pFeaClsName].ContainsKey(pGISID))
                        {
                            List<long> LstOID = new List<long>();
                            LstOID.Add(pOID);
                            DicFea[pFeaClsName].Add(pGISID, LstOID);
                        }
                        else
                        {
                            DicFea[pFeaClsName][pGISID].Add(pOID);
                        }
                    }
                    pFeature = pFeaCursor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);

                #endregion

            }
            #region 进行分类代码检查
            //ErrorEventArgs ErrEvent = new ErrorEventArgs();

            if (_AppHk.DataTree == null) return;
            _AppHk.DataTree.Nodes.Clear();
            //创建处理树图
            IntialTree(_AppHk.DataTree);
            //设置树节点颜色
            setNodeColor(_AppHk.DataTree);
            _AppHk.DataTree.Tag = false;

            //遍历图层
            foreach (KeyValuePair<string, Dictionary<string, List<long>>> FeaCls in DicFea)
            {
                string pFeaClsNameStr = FeaCls.Key;
                if(pFeaClsNameStr.Contains("."))
                {
                    pFeaClsNameStr = pFeaClsNameStr.Substring(pFeaClsNameStr.IndexOf('.') + 1);
                }
                //创建树图节点(以图层名作为根结点)
                DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                pNode = (DevComponents.AdvTree.Node)CreateAdvTreeNode(_AppHk.DataTree.Nodes, pFeaClsNameStr, pFeaClsNameStr, _AppHk.DataTree.ImageList.Images[6], true);//图层名节点
               
                ////添加树图节点列
                //CreateAdvTreeCell(pNode, "", null);
                ////设置初始进度条
                int tempValue = 0;
                //ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, 0, FeaCls.Value.Count, tempValue);
                
 
                //遍历分类代码值，进行检查
                foreach (KeyValuePair<string, List<long>> pGISIDItem in FeaCls.Value)
                {
                    //分类代码的值
                    string ppGISID = pGISIDItem.Key;
                    string sqlStr = "select * from GeoCheckCode where 分类代码 ='" + ppGISID + "'";
                    //执行检查
                    int pResult = CodeStandardizeCheck(sqlStr);
                    #region 对检查结果进行判断
                    if (pResult == -1)
                    {
                        return;
                    }
                    if (pResult == 1)
                    {
                        //该分类代码正确
                        tempValue += 1;//进度条的值加1
                        continue;
                    }
                    if (pResult == 0)
                    {
                        //该分类代码不正确
                        #region 保存错误结果
                        //字段属性为空，与标准不一致，将结果保存起来

                        //遍历该分类代码对应的要素OID集合
                        long[] OIDs = new long[pGISIDItem.Value.Count];
                        for (int m = 0; m < pGISIDItem.Value.Count; m++)
                        {
                            OIDs[m] = pGISIDItem.Value[m];

                            //ErrEvent.FeatureClassName = pFeaClsNameStr;
                            //ErrEvent.OIDs = OIDs;
                            //ErrEvent.ErrorName = "分类代码不存在";
                            //ErrEvent.ErrDescription = "在模板库中找不到分类代码：" + ppGISID;
                            //ErrEvent.CheckTime = System.DateTime.Now.ToString();

                            //double pMapx = 0.0;
                            //double pMapy = 0.0;
                            //IPoint pPoint = new PointClass();
                            //if (pFeaCls.ShapeType != esriGeometryType.esriGeometryPoint)
                            //{
                            //    pPoint = TopologyCheckClass.GetPointofFeature(pFeature);
                            //}
                            //else
                            //{
                            //    pPoint = pFeature.Shape as IPoint;
                            //}
                            //pMapx = pPoint.X;
                            //pMapy = pPoint.Y;

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("分类代码检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.分类代码不存在.GetHashCode());//错误id;
                            ErrorLst.Add("模板中图层" + pFeaClsNameStr + "中不存在分类代码" + ppGISID);//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);           //...
                            ErrorLst.Add(pFeaClsNameStr);
                            ErrorLst.Add(OIDs[m]);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(_AppHk.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        #endregion
                    }
                    #endregion
                    
                    //引发事件
                     //this.OnErrorFind(_AppHk, ErrEvent);

                    tempValue += 1;//进度条的值加1
                    //引发进度条事件
                    this.OnProgressStep(_AppHk, tempValue, FeaCls.Value.Count);

                    //ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                }
                //改变树图运行状态
                ChangeTreeSelectNode(pNode, "完成"+FeaCls.Value.Count + "个分类代码的检查", false);
          
            }
           
            
            #endregion

            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "检查完成!");
        }


        /// <summary>
        /// 代码图层检查---检查主函数
        /// </summary>
        public void ExcuteLayerCheck()
        {
            Exception eError = null;
            //用来存储MapControl上的图层的分类代码的相关信息
            Dictionary<IFeatureClass, Dictionary<string, List<long>>> DicFea = new Dictionary<IFeatureClass, Dictionary<string, List<long>>>();
            ////用来存储图层名和类型字典对
            //Dictionary<string, string> DicFeaType = new Dictionary<string, string>();


            string pClassifyName = GetClassifyName1(out eError);
            if (eError != null || pClassifyName == "")
            {
                return;
            }

            foreach (IFeatureLayer pFeaLayer in m_LstFeaLayer)
            {
                IFeatureClass pFeatureClass = pFeaLayer.FeatureClass;     
               

                #region 首先检查Mapcontrol上的要素类是否具有分类代码这个字段
                int index = -1;   //分类代码索引
                index = pFeatureClass.Fields.FindField(pClassifyName);
                if (index == -1)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在分类代码字段！");
                    return;
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
                        Dictionary<string, List<long>> DicCode = new Dictionary<string, List<long>>();
                        List<long> LstOID = new List<long>();
                        LstOID.Add(pOID);
                        DicCode.Add(pGISID, LstOID);
                        DicFea.Add(pFeatureClass, DicCode);
                    }
                    else
                    {
                        if (!DicFea[pFeatureClass].ContainsKey(pGISID))
                        {
                            List<long> LstOID = new List<long>();
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

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                #endregion

            }
            #region 进行分类代码检查
            //ErrorEventArgs ErrEvent = new ErrorEventArgs();
            //声明表格用来保存检查结果
            //DataTable pResultTable = CreateTable();

            if (_AppHk.DataTree == null) return;
            _AppHk.DataTree.Nodes.Clear();
            //创建处理树图
            IntialTree(_AppHk.DataTree);
            //设置树节点颜色
            setNodeColor(_AppHk.DataTree);
            _AppHk.DataTree.Tag = false;


            //遍历图层
            foreach (KeyValuePair<IFeatureClass, Dictionary<string, List<long>>> FeaCls in DicFea)
            {
                IFeatureClass pFeaCls = FeaCls.Key;
                IDataset pDT = pFeaCls as IDataset;
                if (pDT == null) return;
                //string pFeaClsType="";                                        //要素类型类型：点、线、面、注记

                string pFeaClsNameStr = pDT.Name.Trim();                 //要素类名称
                if(pFeaClsNameStr.Contains("."))
                {
                    pFeaClsNameStr = pFeaClsNameStr.Substring(pFeaClsNameStr.IndexOf('.') + 1);
                }

                //创建树图节点(以图层名作为根结点)
                DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                pNode = (DevComponents.AdvTree.Node)CreateAdvTreeNode(_AppHk.DataTree.Nodes, pFeaClsNameStr, pFeaClsNameStr, _AppHk.DataTree.ImageList.Images[6], true);//图层名节点
                //显示进度条
                ShowProgressBar(true);
                ////添加树图节点列
                //CreateAdvTreeCell(pNode, "", null);
                //设置初始进度条
                int tempValue = 0;
                #region 检查图层名类型是否正确
                
                #region 对检查结果进行判断

                ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, 0, FeaCls.Value.Count, tempValue);

                #region 检查分类代码与图层的对应关系
                //遍历分类代码值，进行检查
                foreach (KeyValuePair<string, List<long>> pGISIDItem in FeaCls.Value)
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
                            //在模板中能够找到该分类代码对应的图层名,说明分类代码对应的图层名正确
                            tempValue += 1;//进度条的值加1
                            ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                            continue;

                        }
                        if (pResult2 == 0)
                        {
                            //在模板中找不到该分类代码对应的图层名,说明分类代码对应的图层名不正确

                            #region 保存错误结果
                            //遍历该分类代码对应的要素OID集合
                            long[] OIDs = new long[pGISIDItem.Value.Count];
                            for (int m = 0; m < pGISIDItem.Value.Count; m++)
                            {
                                OIDs[m] = pGISIDItem.Value[m];

                                List<object> ErrorLst = new List<object>();
                                ErrorLst.Add("要素属性检查");//功能组名称
                                ErrorLst.Add("代码图层检查");//功能名称
                                ErrorLst.Add("");  //数据文件名
                                ErrorLst.Add(enumErrorType.分类代码与图层名对应关系不正确.GetHashCode());//错误id;
                                ErrorLst.Add("模板中图层" + pFeaClsNameStr + "中与分类代码" + ppGISID+"的对应关系不正确！");//错误描述
                                ErrorLst.Add(0);    //...
                                ErrorLst.Add(0);           //...
                                ErrorLst.Add(pFeaClsNameStr);
                                ErrorLst.Add(OIDs[m]);
                                ErrorLst.Add("");
                                ErrorLst.Add(-1);
                                ErrorLst.Add(false);
                                ErrorLst.Add(System.DateTime.Now.ToString());

                                //传递错误日志
                                IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                                DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                                DataErrTreat(_AppHk.DataCheckGrid as object, dataErrTreatEvent);
                            }
                            //ErrEvent.FeatureClassName = pFeaClsNameStr;
                            //ErrEvent.OIDs = OIDs;
                            //ErrEvent.ErrorName = "分类代码与图层名对应不正确";
                            //ErrEvent.ErrDescription = "在模板中分类代码" + ppGISID + "对应的图层名不是" + pFeaClsNameStr;
                            //ErrEvent.CheckTime = System.DateTime.Now.ToString();
                            #endregion
                        }
                        #endregion
                    }
                    if (pResult == 0)
                    {
                        //分类代码不存在
                        #region 保存错误结果
                        //遍历该分类代码对应的要素OID集合
                        long[] OIDs = new long[pGISIDItem.Value.Count];
                        for (int m = 0; m < pGISIDItem.Value.Count; m++)
                        {
                            OIDs[m] = pGISIDItem.Value[m];

                            List<object> ErrorLst = new List<object>();
                            ErrorLst.Add("要素属性检查");//功能组名称
                            ErrorLst.Add("代码图层检查");//功能名称
                            ErrorLst.Add("");  //数据文件名
                            ErrorLst.Add(enumErrorType.分类代码不存在.GetHashCode());//错误id;
                            ErrorLst.Add("模板中图层" + pFeaClsNameStr + "中不存在分类代码" + ppGISID);//错误描述
                            ErrorLst.Add(0);    //...
                            ErrorLst.Add(0);           //...
                            ErrorLst.Add(pFeaClsNameStr);
                            ErrorLst.Add(OIDs[m]);
                            ErrorLst.Add("");
                            ErrorLst.Add(-1);
                            ErrorLst.Add(false);
                            ErrorLst.Add(System.DateTime.Now.ToString());

                            //传递错误日志
                            IDataErrInfo dataErrInfo = new DataErrInfo(ErrorLst);
                            DataErrTreatEvent dataErrTreatEvent = new DataErrTreatEvent(dataErrInfo);
                            DataErrTreat(_AppHk.DataCheckGrid as object, dataErrTreatEvent);
                        }
                        //ErrEvent.FeatureClassName = pFeaClsNameStr;
                        //ErrEvent.OIDs = OIDs;
                        //ErrEvent.ErrorName = "分类代码不存在";
                        //ErrEvent.ErrDescription = "在模板库中找不到分类代码：" + ppGISID;
                        //ErrEvent.CheckTime = System.DateTime.Now.ToString();
                        #endregion
                    }

                    //引发事件
                    //this.OnErrorFind(_AppHk, ErrEvent);

                    tempValue += 1;//进度条的值加1
                    ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);
                }
                #endregion

                #endregion

                #endregion
                //改变树图运行状态
                ChangeTreeSelectNode(pNode, "完成" + FeaCls.Value.Count + "个代码图层的检查", false);
            }
            #endregion

            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "检查完成!");
            //隐藏进度条
            ShowProgressBar(false);
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
            pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_TempletePath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out Error);
            if (Error != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接库体出错！路径为：" + m_TempletePath);
                return -1;
            }
            DataTable dt = pSysTable.GetSQLTable(sqlStr, out Error);
            if (Error != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "打开表格出错！");
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
            pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +TopologyCheckClass.GeoDataCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
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

        private void CreateTable()
        {
            _ResultTable.Columns.Add("检查功能名", typeof(string));
            _ResultTable.Columns.Add("错误类型", typeof(string));
            _ResultTable.Columns.Add("错误描述", typeof(string));
            _ResultTable.Columns.Add("数据图层1", typeof(string));
            _ResultTable.Columns.Add("要素OID1", typeof(string));
            _ResultTable.Columns.Add("数据图层2", typeof(string));
            _ResultTable.Columns.Add("要素OID2", typeof(string));
            _ResultTable.Columns.Add("检查时间", typeof(string));
            _ResultTable.Columns.Add("定位点X", typeof(string));
            _ResultTable.Columns.Add("定位点Y", typeof(string));
            _ResultTable.Columns.Add("数据文件名", typeof(string));
        }

        //处理检查结果
        public void GeoDataChecker_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            //用户界面上表现出错误信息
            DevComponents.DotNetBar.Controls.DataGridViewX pDataGrid = sender as DevComponents.DotNetBar.Controls.DataGridViewX;
            if (_ResultTable == null) return;
            DataRow newRow = _ResultTable.NewRow();
            newRow["检查功能名"] = e.ErrInfo.FunctionName;
            newRow["错误类型"] = GeoDataChecker.GetErrorTypeString(Enum.Parse(typeof(enumErrorType), e.ErrInfo.ErrID.ToString()).ToString());
            newRow["错误描述"] = e.ErrInfo.ErrDescription;
            newRow["数据图层1"] = e.ErrInfo.OriginClassName;
            newRow["要素OID1"] = e.ErrInfo.OriginFeatOID;
            newRow["数据图层2"] = e.ErrInfo.DestinationClassName;
            newRow["要素OID2"] = e.ErrInfo.DestinationFeatOID;
            newRow["检查时间"] = e.ErrInfo.OperatorTime;
            newRow["定位点X"] = e.ErrInfo.MapX;
            newRow["定位点Y"] = e.ErrInfo.MapY;
            newRow["数据文件名"] = e.ErrInfo.DataFileName;
            _ResultTable.Rows.Add(newRow);

            pDataGrid.Update();

            //将结果输出excle

            InsertRowToExcel(e);
        }

        //将数据检查结果存入Excel中 
        private void InsertRowToExcel(DataErrTreatEvent e)
        {
            if (_ErrDbCon != null && _ErrTableName != "")
            {
                SysCommon.DataBase.SysTable sysTable = new SysCommon.DataBase.SysTable();
                sysTable.DbConn = _ErrDbCon;
                sysTable.DBConType = SysCommon.enumDBConType.OLEDB;
                sysTable.DBType = SysCommon.enumDBType.ACCESS;
                string sql = "insert into " + _ErrTableName +
                    "(数据文件路径,检查功能名,错误类型,错误描述,数据图层1,数据OID1,数据图层2,数据OID2,定位点X,定位点Y,检查时间) values(" +
                    "'" + e.ErrInfo.DataFileName + "','" + e.ErrInfo.FunctionName + "','" + GeoDataChecker.GetErrorTypeString(Enum.Parse(typeof(enumErrorType), e.ErrInfo.ErrID.ToString()).ToString()) + "','" + e.ErrInfo.ErrDescription + "','" + e.ErrInfo.OriginClassName + "','" + e.ErrInfo.OriginFeatOID.ToString() + "','" +
                    e.ErrInfo.DestinationClassName + "','" + e.ErrInfo.DestinationFeatOID.ToString() + "'," + e.ErrInfo.MapX + "," + e.ErrInfo.MapY + ",'" + e.ErrInfo.OperatorTime + "')";

                Exception errEx = null;
                sysTable.UpdateTable(sql, out errEx);
                if (errEx != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "写入Excel文件出错！");
                    return;
                }
            }
        }


        #region 处理树图相关函数
        //创建处理树图
        private void IntialTree(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.AdvTree.ColumnHeader aColumnHeader;
            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "FCName";
            aColumnHeader.Text = "图层名";
            aColumnHeader.Width.Relative = 50;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeRes";
            aColumnHeader.Text = "结果";
            aColumnHeader.Width.Relative = 45;
            aTree.Columns.Add(aColumnHeader);
        }
        //设置选中树节点颜色
        private void setNodeColor(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.DotNetBar.ElementStyle elementStyle = new DevComponents.DotNetBar.ElementStyle();
            elementStyle.BackColor = Color.FromArgb(255, 244, 213);
            elementStyle.BackColor2 = Color.FromArgb(255, 216, 105);
            elementStyle.BackColorGradientAngle = 90;
            elementStyle.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottomWidth = 1;
            elementStyle.BorderColor = Color.DarkGray;
            elementStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderLeftWidth = 1;
            elementStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderRightWidth = 1;
            elementStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderTopWidth = 1;
            elementStyle.BorderWidth = 1;
            elementStyle.CornerDiameter = 4;
            elementStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            aTree.NodeStyleSelected = elementStyle;
            aTree.DragDropEnabled = false;
        }
        //创建树图节点
        private DevComponents.AdvTree.Node CreateAdvTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand)
        {

            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = strText;
            node.Image = pImage;
            if (strName != null)
            {
                node.Name = strName;
            }

            if (bExpand == true)
            {
                node.Expand();
            }
            //添加树图列节点
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell();
            aCell.Images.Image = null;
            node.Cells.Add(aCell);
            nodeCol.Add(node);
            return node;
        }

        //添加树图节点列
        private DevComponents.AdvTree.Cell CreateAdvTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage)
        {
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell(strText);
            aCell.Images.Image = pImage;
            aNode.Cells.Add(aCell);

            return aCell;
        }

        //为数据处理树图节点添加处理结果状态
        private void ChangeTreeSelectNode(DevComponents.AdvTree.Node aNode,string strRes, bool bClear)
        {
            if (aNode == null)
            {
                _AppHk.DataTree.SelectedNode = null;
                _AppHk.DataTree.Refresh();
                return;
            }

            _AppHk.DataTree.SelectedNode = aNode;
            if (bClear)
            {
                _AppHk.DataTree.SelectedNode.Nodes.Clear();
            }
            _AppHk.DataTree.SelectedNode.Cells[1].Text = strRes;
            _AppHk.DataTree.Refresh();
        }
        #endregion

        #region 进度条显示
        //控制进度条显示
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
               (_AppHk as Plugin.Application.IAppFormRef).ProgressBar.Visible = true;
            }
            else
            {
                (_AppHk as Plugin.Application.IAppFormRef).ProgressBar.Visible = false;
            }
        }
        //修改进度条
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }


        //改变状态栏提示内容
        private void ShowStatusTips(string strText)
        {
            (_AppHk as Plugin.Application.IAppFormRef).OperatorTips = strText;
        }
        #endregion
    }
}
