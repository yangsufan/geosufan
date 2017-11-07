using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using System.Xml;

//ygc 2012-12-26逻辑检查类
namespace GeoDBATool
{
   public class clsLogicCheck
    {
       public static string m_Log
       {
           get;
           set;
       }
       public static string m_LayerName
       {
           get;
           set;
       }
       //通过IWorkspace获取图层
       public static List<IFeatureClass> GetFeatureClass(IWorkspace pWorkspace)
       {
           List<IFeatureClass> newlist = new List<IFeatureClass>();
           if (pWorkspace == null)
           {
               return newlist;
           }
           IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
           pEnumDataset.Reset();
           IDataset pDataset = pEnumDataset.Next();
           while (pDataset != null)
           {
               newlist.Add(pDataset as IFeatureClass);
               pDataset = pEnumDataset.Next();
           }
           pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
           pEnumDataset.Reset();
           IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
           while (pFeaDataset != null)
           {
               IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
               IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
               IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
               while (pFeatureClass != null)
               {
                   newlist.Add(pFeatureClass);
                   pFeatureClass = pEnumFeatureClass.Next();
               }
               pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
           }
           return newlist;
       }
       //通过IWokspace和图层名称获取图层
       public static List<IFeatureClass> GetFeatureClass(IWorkspace pWorkspace,string DatasetName)
       {
           List<IFeatureClass> newlist = new List<IFeatureClass>();
           if (pWorkspace == null||DatasetName =="")
           {
               return newlist;
           }
           IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
           pEnumDataset.Reset();
           IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
           while (pFeaDataset != null)
           {
               if (pFeaDataset.Name == DatasetName)
               {
                   IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
                   IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                   IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                   while (pFeatureClass != null)
                   {
                       newlist.Add(pFeatureClass);
                       pFeatureClass = pEnumFeatureClass.Next();
                   }
               }
               pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
           }
           return newlist;
       }
       //获取业务库中检查配置信息
       public static Dictionary<string, string> GetCheckCondition()
       {
           IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
           IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
           Dictionary <string ,string > newdic=new Dictionary<string,string> ();
           if (pFeatureWorkspace == null) return newdic;
           try
           {
               ITable pTable = pFeatureWorkspace.OpenTable("逻辑检查");
               ICursor pCursor = pTable.Search(null, false);
               IRow pRow = pCursor.NextRow();
               while (pRow != null)
               {
                   int oidIndex = pRow.Fields.FindField("OBJECTID");
                   int conditionIndex = pRow.Fields.FindField("CONDITION");
                   newdic.Add(pRow.get_Value(oidIndex).ToString(), pRow.get_Value(conditionIndex).ToString());
                   pRow = pCursor.NextRow();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
           }
           catch (Exception ex)
           {
               m_Log += "\r\n";
               m_Log += "无法获取业务库中配置信息!";
               m_Log += "\r\n";
           }
           return newdic;
       }
       //检查一个图层中的错误信息
       public static Dictionary<string, int> CheckFeautreClass(IFeatureClass pFeatureClass, Dictionary<string, string> dicCondition, out Exception ex)
       {
           ex = null;
           if (pFeatureClass == null) return null;
           if (dicCondition == null || dicCondition.Count == 0) return null;
           Dictionary<string, int> newdic = new Dictionary<string, int>();
           SysCommon.CProgress vProgress = new SysCommon.CProgress();
           vProgress.ShowDescription = true;
           vProgress.ShowProgressNumber = true;
           vProgress.TopMost = true;
           vProgress.EnableCancel = false;
           vProgress.EnableUserCancel(false);
           vProgress.MaxValue =dicCondition.Count;
           vProgress.ProgresssValue = 0;
           vProgress.Step = 1;
           vProgress.ShowProgress();
           vProgress.SetProgress("正在检查" + pFeatureClass.AliasName + "数据......");
           foreach (string key in dicCondition.Keys)
           {
               int featureCount = CheckSingleCondition(pFeatureClass, dicCondition[key], out ex);
               if (ex != null)
               {
                   newdic.Add(key, -1);
               }
               else
               {
                   newdic.Add(key, featureCount);
               }
               vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
           }
           vProgress.Close();
           return newdic;
       }
       //单一条件检查
       public static int CheckSingleCondition(IFeatureClass pFeatureClass,string Condition, out Exception ex)
       {
           int count = 0;
           ex = null;
           if (pFeatureClass == null) return 0;
           if (Condition == "") return 0;
           IQueryFilter pQuery = new QueryFilterClass();
           pQuery.WhereClause = Condition;
           try
           {
               int featurecout = pFeatureClass.FeatureCount(pQuery);
               count = featurecout;
           }
           catch (Exception e)
           {
               ex = e;
               m_Log += "\r\n";
               m_Log += "检查条件  "+ Condition +" 对图层 "+pFeatureClass .AliasName +" 无效!  ";
               m_Log += "\r\n";
               return 0;
           }
           return count;
       }
       //主检查函数
       public static Dictionary<IFeatureClass ,Dictionary <string ,int>>GetLogicCheck(IWorkspace pWorkspace,out Exception  ex)
       {
           ex = null;
           if (pWorkspace == null) return null;
           Dictionary<IFeatureClass, Dictionary<string, int>> newdic = new Dictionary<IFeatureClass, Dictionary<string, int>>();
           List<IFeatureClass> listfeatureClass = GetFeatureClass(pWorkspace);
           Dictionary<string, string> dicCondition = GetCheckCondition();
           for (int i = 0; i < listfeatureClass.Count; i++)
           {
               if (m_LayerName != ""&&m_LayerName !=null)
               {
                   if (listfeatureClass[i].AliasName == m_LayerName)
                   {
                       Dictionary<string, int> dicCheckResult = CheckFeautreClass(listfeatureClass[i], dicCondition, out ex);
                       newdic.Add(listfeatureClass[i], dicCheckResult);
                   }
               }
               else
               {
                   Dictionary<string, int> dicCheckResult = CheckFeautreClass(listfeatureClass[i], dicCondition, out ex);
                   newdic.Add(listfeatureClass[i], dicCheckResult);
               }
           }
           return newdic;
       }
       //主检查函数，针对临时库检查  ygc 2013-01-24
       public static Dictionary<IFeatureClass, Dictionary<string, int>> GetLogicCheck(out Exception ex)
       {
                //获取工程配置文件
           ex = null;
           Dictionary<IFeatureClass, Dictionary<string, int>> newdic = new Dictionary<IFeatureClass, Dictionary<string, int>>();
           XmlDocument xmlPro = new XmlDocument();
           xmlPro.Load(ModData.v_projectDetalXML);
           XmlNodeList xmlProList = xmlPro.SelectNodes("//工程");
           for (int t = 0; t < xmlProList.Count; t++)
           {
               XmlElement xmlproCon = xmlProList[t].SelectSingleNode("//工程[@名称='" + xmlProList[t].Attributes["名称"].Value .ToString () + "']//内容//临时库//连接信息") as XmlElement;
               IWorkspace pWorkspace = ModDBOperator.GetDBInfoByXMLNode(xmlproCon, "") as IWorkspace;
               string DatasetName = xmlProList[t].SelectSingleNode("//工程[@名称='" + xmlProList[t].Attributes["名称"].Value.ToString() + "']//内容//临时库//数据集").Attributes["名称"].Value.ToString();
               List<IFeatureClass> listfeatureClass = GetFeatureClass(pWorkspace, DatasetName);
               Dictionary<string, string> dicCondition = GetCheckCondition();
               for (int i = 0; i < listfeatureClass.Count; i++)
               {
                       Dictionary<string, int> dicCheckResult = CheckFeautreClass(listfeatureClass[i], dicCondition, out ex);
                       newdic.Add(listfeatureClass[i], dicCheckResult);
               }
           }
           return newdic;
       }
    }
}
