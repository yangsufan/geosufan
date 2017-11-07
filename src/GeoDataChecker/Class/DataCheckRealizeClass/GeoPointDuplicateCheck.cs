using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using System.Xml;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    public class GeoPointDuplicateCheck : IDataCheckRealize
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;
        private IArcgisDataCheckHook Hook;

        /// <summary>
        /// 构造函数，用来传一个数据集合进来
        /// </summary>
        /// <param name="Dataset">数据集合</param>
        public GeoPointDuplicateCheck(IArcgisDataCheckHook Mhook)
        {
            Hook = Mhook;
        }

        #region IDataCheckRealize 成员

        public void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            e.ErrInfo.FunctionCategoryName = "批量检查";
            e.ErrInfo.FunctionName = "同层重复点检查";
            e.ErrInfo.ErrID = enumErrorType.同层点重复检查.GetHashCode();
            DataErrTreat(sender, e);
        }

        #endregion

        #region IDataCheck 成员

        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook as IArcgisDataCheckHook;
        }

        public void OnDataCheck()
        {
            if (Hook == null) return;

        }

        /// <summary>
        /// 点重复出错检查
        /// </summary>
        /// <param name="featureDataset"></param>
        public void DupointError(IFeatureDataset featureDataset,out Exception ex)
        {
            ex = null;
            try
            {
                if (featureDataset == null) return;
                IEnumDataset GetFeatureClass = featureDataset.Subsets;//得到集合里的类集合
                GetFeatureClass.Reset();
                IDataset TempSet = GetFeatureClass.Next();
                while (TempSet != null)
                {
                    if (TempSet is IFeatureClass)
                    {
                        IFeatureClass FeatureClass = TempSet as IFeatureClass;
                        RePoint(FeatureClass);//重复点检查
                    }
                }
            }
            catch(Exception EX)
            {
                ex = EX;
            }
        }

        /// <summary>
        /// 判断重复点
        /// </summary>
        private void RePoint(IFeatureClass FeatureClass)
        {
            #region point
            try
            {
                #region 读XML内容
                string[] para;
                ArrayList list = new ArrayList();
                list = Foreach();
                if (list != null)
                {
                    int list_count = list.Count;
                    para = new string[list_count];
                    for (int num = 0; num < list_count; num++)
                    {
                        para[num] = list[num].ToString();
                    }
                }
                else
                {
                    para = new string[0];
                }
                #endregion
                list = null;
                Hashtable Foreach_hs = new Hashtable();//用一个哈希来遍历重复
                IFeatureClass pFeatureClass = FeatureClass;//要素类
                if (FeatureClass == null || FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint) return;//如果为空或是非点层就返回

                IDataset set = pFeatureClass as IDataset;
                //开始遍历
                #region 遍历要素类
                IFeatureCursor cour = pFeatureClass.Search(null, false);
                IFeature fu = cour.NextFeature();
                ArrayList FeatureList = new ArrayList();//存要素动态数组
                //将当前要素类里的所有要素存入动态数组
                while (fu != null)
                {
                    FeatureList.Add(fu);
                    fu = cour.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cour);

                //如果为空，就返回
                int FeatureCount = FeatureList.Count;
                if (FeatureCount == 0)
                {
                    return;
                }
                #region 遍历要素类里的重复点
                for (int F = 0; F < FeatureCount; F++)
                {
                    fu = FeatureList[F] as IFeature;
                    int i = F + 1;//方便计算
                    #region 总体上得到一个比较值，然后加入到LIST当中
                    IPoint point = fu.Shape as IPoint;
                    double x = point.X;
                    double y = point.Y;
                    string temp = x.ToString() + "," + y.ToString();
                    #region 从XML里提取要比较字段
                    int C = para.Length;
                    if (C > 0)
                    {
                        for (int n = 0; n < C; n++)
                        {
                            int ID = fu.Fields.FindField(para[n]);//得到OID
                            if (ID >= 0)
                            {
                                string content = fu.get_Value(ID).ToString();//得到对应字段的值
                                temp = temp + "," + content;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    #endregion
                    #endregion
                    string soruce = set.Name + " 源OID：" + fu.OID.ToString();
                    if (Foreach_hs == null)
                    {

                        Foreach_hs.Add(temp, soruce);

                    }
                    else
                    {
                        if (!Foreach_hs.Contains(temp))
                        {
                            Foreach_hs.Add(temp, soruce);
                        }
                        else
                        {
                            //以下是出错的记录
                            string values = Foreach_hs[temp].ToString();//取得源重复的OID和NAME
                            string temps = values + " 重复的OID：" + fu.OID.ToString();//组合信息

                            //System.Data.DataRow Row = Datatable.NewRow();//新创建一个行
                            //Row[0] = temps;
                            //Datatable.Rows.Add(Row);//将行加入到表数据集中
                            //SetCheckState.CheckShowTips(pAppForm, temps);

                        }
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            #endregion

        }
        /// <summary>
        /// 遍历XML
        /// </summary>
        /// <returns></returns>
        private ArrayList Foreach()
        {
            ArrayList list = new ArrayList();
            XmlDataDocument doc = new XmlDataDocument();
            string path = Application.StartupPath + "\\..\\Res\\checker\\point.xml";//通过获取程序的运行路径再返到上层得到我们所需要的XML路径
            doc.Load(path);//加载XML文件
            XmlNode node = doc.DocumentElement;//得到XML的节点
            int count = node.ChildNodes.Count;
            if (count != 0)
            {
                for (int n = 0; n < count; n++)
                {
                    list.Add(node.ChildNodes[n].InnerText);//读取XML节点的内容
                }
                return list;
            }
            else
            {
                list = null;
                return list;
            }
        }
        #endregion

        #region ICheckEvent 成员
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }
        #endregion
    }
}
