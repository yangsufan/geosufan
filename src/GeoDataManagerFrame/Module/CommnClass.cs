using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Windows.Forms;
using SysCommon.Gis;
//ZQ 20110922   街坊最大地籍号查询
namespace GeoDataManagerFrame
{
   public class CommnClass
    {

        public static IFeatureClass m_JFFeatureClass;
        public static IFeatureClass m_ZDFeatureClass;
        public static string m_cmbBoxCountry = "";
        public static string m_cmbBoxVillage = "";
        public static string m_cmbBoxNeighbour = "";
        public static string m_JFLike = "%";
        public static string m_ZDLike = "%";
        public static string _DJHFieldName = "DJH";
        public static string _JFDMFieldName = "JFDM";
        public static string _JFMCFieldName = "JFMC";
        public static IWorkspace m_Workspace;

       /// <summary>
       /// 获取宗地图层查询要素的游标
       /// </summary>
       /// <param name="strValue"></param>
       /// <returns></returns>
        private static IFeatureCursor GetZDCursor(string  strValue)
        {
            IFeatureCursor pFeatureCursor = null;
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = _DJHFieldName+" Like '" + strValue + m_ZDLike + "'";
                pFeatureCursor = m_ZDFeatureClass.Search(pQueryFilter, false);
                return pFeatureCursor;
            }
            catch { return pFeatureCursor; }
        }
        private static IFeatureCursor GetLinBanCursor(string strValue)
        {
            IFeatureCursor pFeatureCursor = null;
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = _JFDMFieldName + " Like '" + strValue + m_ZDLike + "'";
                pFeatureCursor = m_ZDFeatureClass.Search(pQueryFilter, false);
                return pFeatureCursor;
            }
            catch { return pFeatureCursor; }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="pComboBoxItem"></param>
       /// <param name="strValue"></param>
       /// <returns></returns>
        private static  bool IsExist(DevComponents.DotNetBar.Controls.ComboBoxEx pComboBoxItem,string strValue)
        {
            bool IsExist = false;
            if (pComboBoxItem.Items.Count > 0)
            {
                for (int i = 0; i < pComboBoxItem.Items.Count;i++ )
                {
                    if (pComboBoxItem.Items[i].ToString() == strValue)
                    {
                        IsExist = true;
                        return IsExist;
                    }
                }
            }
            return IsExist;
        }
       /// <summary>
       /// 判断节点是否存在相同的值
       /// </summary>
       /// <param name="pNode"></param>
       /// <param name="strValue"></param>
       /// <returns></returns>
        private static bool IsNodeExist(System.Windows.Forms.TreeNode pNode,string strValue)
        {
            bool IsExist = false;
            if (pNode.Nodes.Count == 0) { return IsExist; }
            for (int i = 0; i < pNode.Nodes.Count; i++)
            {
                if (pNode.Nodes[i].Text == strValue)
                {
                    IsExist = true;
                    return IsExist;
                }
            }
            return IsExist;
        }
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static string GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            string strLike = "%";
            if (pWorkspace == null)
            {
                return strLike = "%";
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return strLike = "%";
            }
            string strDescrip = pWorkSpaceFac.get_WorkspaceDescription(false);
            switch (strDescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            return strLike;
        }
       /// <summary>
       /// 获取行政区代码中文名
       /// </summary>
       /// <param name="strValue"></param>
       /// <returns></returns>
       private static string  GetDMChineseName(string  strValue)
       {
           string strDMChineseName = strValue;
           SysGisTable mSystable = new SysGisTable(m_Workspace);
           Exception err = null;
           Dictionary<string, object> dic = mSystable.GetRow("行政区字典表", "CODE='" + strValue + "'", out err);
           if (dic != null)
           {
               strDMChineseName = dic["NAME"] + "(" + dic["CODE"] + ")";
           }
           return strDMChineseName;
       }
       private static string GetJFMCChineseName(string strValue, string strJFMC)
       {
           string strDMChineseName = strValue;
           SysGisTable mSystable = new SysGisTable(m_Workspace);
           Exception err = null;
           Dictionary<string, object> dic = mSystable.GetRow("行政区字典表", "CODE='" + strValue.Substring(0,9) + "'", out err);
           if (dic != null)
           {
               strDMChineseName = dic["NAME"] + strJFMC + "(" + strValue + ")";
           }
           return strDMChineseName;
       }
       /// <summary>
       /// 根据地籍号获取街坊名称
       /// </summary>
       /// <param name="strDJH"></param>
       /// <returns></returns>
       private static string GetJFMC(string strDJH)
       {
           string strJFMC = "";
           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = _JFDMFieldName+" Like '" + strDJH.Substring(0, 12) + m_JFLike + "'";
           IFeatureCursor pFeatureCursor = m_JFFeatureClass.Search(pQueryFilter, false);
           int IndexJFMC = m_JFFeatureClass.Fields.FindField(_JFMCFieldName);
           IFeature pFeature = pFeatureCursor.NextFeature();
           while (pFeature != null)
           {
               strJFMC = pFeature.get_Value(IndexJFMC).ToString();
               break;
           }
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
           pFeatureCursor = null;
           return strJFMC;
       }
       /// <summary>
       /// 获取已知名称的节点
       /// </summary>
       /// <param name="node"></param>
       /// <param name="strCJXZQ"></param>
       /// <returns></returns>
       private static TreeNode GetIsExistNode(TreeNode node, string strCJXZQ)
       {
           TreeNode IsExistNode = null;
           if (node.Nodes.Count == 0) { return IsExistNode; }
           for (int i = 0; i < node.Nodes.Count; i++)
           {
               if (node.Nodes[i].Text == strCJXZQ)
               {
                   IsExistNode = node.Nodes[i];
                   return IsExistNode;
               }
           }
           return IsExistNode;
       }
       /// <summary>
        ///  添加村级行政区代码
       /// </summary>
       /// <param name="pComboBoxItem"></param>
       public static void SetcmbBoxCountryVale(DevComponents.DotNetBar.Controls.ComboBoxEx pComboBoxItem)
       {
           try
           {
               //IFeatureCursor pFeatureCursor = m_JFFeatureClass.Search(null, false);
               //int Index = m_JFFeatureClass.Fields.FindField("JFDM");
               //IFeature pFeature = pFeatureCursor.NextFeature();
               //while (pFeature != null)
               //{
               //    string strFieldName = pFeature.get_Value(Index).ToString();
               //    if (strFieldName.Length >= 12)
               //    {
               //        strFieldName = strFieldName.Substring(0, 6);
               //        if (!IsExist(pComboBoxItem, GetDMChineseName(strFieldName)))
               //        {
               //            DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
               //pComboBox.Text = GetDMChineseName(strFieldName);
               //            pComboBox.Tag = strFieldName;
               //            pComboBoxItem.Items.Add(pComboBox);
               //        }
               //    }
               //    pFeature = pFeatureCursor.NextFeature();
               //}
               //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               //pFeatureCursor = null;
               //yjl 20111014 add followed
               IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
               IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
               if (pFW == null)
                   return;
               if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表"))
                   return;
               ITable pXZQDic = pFW.OpenTable("行政区字典表");
               if(pXZQDic==null)
                   return;
               int ndx = pXZQDic.FindField("NAME"),
                  cdx = pXZQDic.FindField("CODE");
               if (ndx == -1 || cdx == -1)
                   return;
               IQueryFilter pQF=new QueryFilterClass();
               pQF.WhereClause="XZJB='3'";
               ICursor pCursor = pXZQDic.Search(pQF, false);
               if (pCursor == null)
                   return;
               IRow pRow = pCursor.NextRow();
              
               while (pRow != null)
               {
                   string name = pRow.get_Value(ndx).ToString(),
                       code = pRow.get_Value(cdx).ToString();
                   DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
                   pComboBox.Text = name + "(" + code + ")";
                   pComboBox.Tag = code;
                   pComboBoxItem.Items.Add(pComboBox);
                   pRow = pCursor.NextRow();
 
               }
               for(int i=0;i<pComboBoxItem.Items.Count-1;i++)
               {
                   DevComponents.DotNetBar.ComboBoxItem itemi = pComboBoxItem.Items[i] as DevComponents.DotNetBar.ComboBoxItem;
                   for (int j = i+1; j < pComboBoxItem.Items.Count; j++)
                   {
                       DevComponents.DotNetBar.ComboBoxItem itemj = pComboBoxItem.Items[j] as DevComponents.DotNetBar.ComboBoxItem;
                       int iti = Convert.ToInt32(itemi.Tag),
                           itj = Convert.ToInt32(itemj.Tag);
                       if (iti > itj)
                       {
                           pComboBoxItem.Items.Remove(itemj);
                           pComboBoxItem.Items.Insert(i, itemj);
                           itemi = pComboBoxItem.Items[i] as DevComponents.DotNetBar.ComboBoxItem;

                       }
 
                   }
                   
               }
               
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
               pCursor = null;
               pW2 = null;
               pFW = null;
               if (pComboBoxItem.Items.Count > 0)
               {
                   pComboBoxItem.SelectedIndex = 0;
               }
           }
           catch { }
       }
       /// <summary>
       /// 添加村级行政区代码
       /// </summary>
       /// <param name="pComboBoxItem"></param>
       public static void SetcmbBoxVillageVale(DevComponents.DotNetBar.Controls.ComboBoxEx pComboBoxItem)
       {
           if (m_cmbBoxCountry == "") { return; }
           try
           {
               //IQueryFilter pQueryFilter = new QueryFilterClass();
               //pQueryFilter.WhereClause = "JFDM Like '" + m_cmbBoxCountry + m_JFLike + "'";
               //IFeatureCursor pFeatureCursor = m_JFFeatureClass.Search(pQueryFilter, false);
               //int Index = m_JFFeatureClass.Fields.FindField("JFDM");
               //IFeature pFeature = pFeatureCursor.NextFeature();

               //while (pFeature != null)
               //{
               //    string strFieldName = pFeature.get_Value(Index).ToString();
               //    if (strFieldName.Length >= 12)
               //    {
               //        strFieldName = strFieldName.Substring(0, 9);
               //        if (!IsExist(pComboBoxItem, GetDMChineseName(strFieldName)))
               //        {
               //            DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
               //            pComboBox.Text = GetDMChineseName(strFieldName);
               //            pComboBox.Tag = strFieldName;
               //            pComboBoxItem.Items.Add(pComboBox);
               //        }
               //    }
               //    pFeature = pFeatureCursor.NextFeature();
               //}
               //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               //pFeatureCursor = null;

               //yjl 20111014 add followed
               IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
               IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
               if (pFW == null)
                   return;
               if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表"))
                   return;
               ITable pXZQDic = pFW.OpenTable("行政区字典表");
               if (pXZQDic == null)
                   return;
               int ndx = pXZQDic.FindField("NAME"),
                  cdx = pXZQDic.FindField("CODE");
               if (ndx == -1 || cdx == -1)
                   return;
               IQueryFilter pQF = new QueryFilterClass();
               pQF.WhereClause = "CODE LIKE '"+m_cmbBoxCountry+"%'";
               ICursor pCursor = pXZQDic.Search(pQF, false);
               if (pCursor == null)
                   return;
               IRow pRow = pCursor.NextRow();
               //pComboBoxItem.Items.Add("");
               while (pRow != null)
               {
                   
                   string name = pRow.get_Value(ndx).ToString(),
                       code = pRow.get_Value(cdx).ToString();
                   if (code.Length <= m_cmbBoxCountry.Length || code.Length>9)
                   {
                       pRow = pCursor.NextRow();
                       continue;
 
                   }
                   DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
                   pComboBox.Text = name + "(" + code + ")";
                   pComboBox.Tag = code;
                   pComboBoxItem.Items.Add(pComboBox);

                   pRow = pCursor.NextRow();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
               pCursor = null;
               pW2 = null;
               pFW = null;
           }
           catch { }

       }
       /// <summary>
       /// 添加街坊代码
       /// </summary>
       /// <param name="pComboBoxItem"></param>
       public static void SetcmbBoxNeighbourVale(DevComponents.DotNetBar.Controls.ComboBoxEx pComboBoxItem)
       {
           if (m_cmbBoxVillage == "") { return; }
           //try
           //{
           //    IQueryFilter pQueryFilter = new QueryFilterClass();
           //    pQueryFilter.WhereClause = _JFDMFieldName+" Like '" + m_cmbBoxVillage + m_JFLike + "'";
           //    IFeatureCursor pFeatureCursor = m_JFFeatureClass.Search(pQueryFilter, false);
           //    int Index = m_JFFeatureClass.Fields.FindField(_JFDMFieldName);
           //    int IndexJFMC = m_JFFeatureClass.Fields.FindField(_JFMCFieldName);
           //    IFeature pFeature = pFeatureCursor.NextFeature();

           //    while (pFeature != null)
           //    {

           //        string strFieldName = pFeature.get_Value(Index).ToString();
           //        string strJFMC = pFeature.get_Value(IndexJFMC).ToString();
           //        if (strFieldName.Length >= 12)
           //        {
           //            strFieldName = strFieldName.Substring(0, 12);
           //            if (!IsExist(pComboBoxItem, GetJFMCChineseName(strFieldName, strJFMC)))
           //            {
           //                DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
           //                pComboBox.Text = GetJFMCChineseName(strFieldName, strJFMC);
           //                pComboBox.Tag = strFieldName;
           //                pComboBoxItem.Items.Add(pComboBox);
           //            }
           //        }
           //        pFeature = pFeatureCursor.NextFeature();
           //    }
           //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
           //    pFeatureCursor = null;
           //}
           //catch { }

               //added by chulili 2012-08-10
           try{
               IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
               IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
               if (pFW == null)
                   return;
               if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表"))
                   return;
               ITable pXZQDic = pFW.OpenTable("行政区字典表");
               if (pXZQDic == null)
                   return;
               int ndx = pXZQDic.FindField("NAME"),
                  cdx = pXZQDic.FindField("CODE");
               if (ndx == -1 || cdx == -1)
                   return;
               IQueryFilter pQF = new QueryFilterClass();
               pQF.WhereClause = "CODE LIKE '"+m_cmbBoxVillage+"%'";
               ICursor pCursor = pXZQDic.Search(pQF, false);
               if (pCursor == null)
                   return;
               IRow pRow = pCursor.NextRow();
               //pComboBoxItem.Items.Add("");
               while (pRow != null)
               {
                   
                   string name = pRow.get_Value(ndx).ToString(),
                       code = pRow.get_Value(cdx).ToString();
                   if (code.Length <= m_cmbBoxVillage.Length || code.Length>12)
                   {
                       pRow = pCursor.NextRow();
                       continue;
 
                   }
                   DevComponents.DotNetBar.ComboBoxItem pComboBox = new DevComponents.DotNetBar.ComboBoxItem();
                   pComboBox.Text = name + "(" + code + ")";
                   pComboBox.Tag = code;
                   pComboBoxItem.Items.Add(pComboBox);

                   pRow = pCursor.NextRow();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
               pCursor = null;
               pW2 = null;
               pFW = null;
           }
           catch { }
       }
       /// <summary>
       /// 设置县级行政区内所有的最大地籍号
       /// </summary>
       /// <param name="vTree"></param>
       public static void SetvTreeCountry(System.Windows.Forms.TreeView vTree)
       {
           if (m_cmbBoxCountry == null) { return; }
           try
           {
               vTree.Nodes.Clear();
               vTree.Nodes.Add(GetDMChineseName(m_cmbBoxCountry), GetDMChineseName(m_cmbBoxCountry), 16, 16);
               vTree.ExpandAll();
               TreeNode tNode1 = vTree.Nodes[0];
               int Index = m_ZDFeatureClass.Fields.FindField(_DJHFieldName);
               IFeatureCursor pFeatureCursor = GetZDCursor(m_cmbBoxCountry);
               Dictionary<string, Int64> pTouchedgroup = new Dictionary<string, Int64>();
               IFeature pFeature = pFeatureCursor.NextFeature();
               while (pFeature != null)
               {
                   string strValue = pFeature.get_Value(Index).ToString();
                   if (strValue.Length >= 12)
                   {
                       pTouchedgroup = GetTouchedGroup(pTouchedgroup, strValue);
                   }
                   pFeature = pFeatureCursor.NextFeature();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               pFeatureCursor = null;
               if (pTouchedgroup.Keys.Count == 0) { return; }
               foreach (string str in pTouchedgroup.Keys)
               {
                   string strCJXZQ = GetDMChineseName(Convert.ToString((pTouchedgroup[str])).Substring(0, 9));
                   string strJFDM = GetJFMCChineseName(Convert.ToString((pTouchedgroup[str])).Substring(0, 12), GetJFMC(Convert.ToString((pTouchedgroup[str]))));
                   string strMaxDJH =Convert.ToString((pTouchedgroup[str]));
                   if (!IsNodeExist(tNode1, strCJXZQ))
                   {
                       tNode1.Nodes.Add(strCJXZQ, strCJXZQ, 16, 16);
                       TreeNode tNode2 = tNode1.Nodes[tNode1.Nodes.Count-1];
                       tNode2.Nodes.Add(strJFDM,strJFDM,16,16);
                       TreeNode  tNode3 =tNode2.Nodes[0];
                       tNode3.Nodes.Add(strMaxDJH,strMaxDJH,16,16);
                   }
                   else
                   {
                       TreeNode vNode2 =GetIsExistNode(tNode1, strCJXZQ);
                       if(vNode2!=null)
                       {
                           vNode2.Nodes.Add(strJFDM,strJFDM,16,16);
                           TreeNode vNode3 = vNode2.Nodes[vNode2.Nodes.Count-1];
                           vNode3.Nodes.Add(strMaxDJH,strMaxDJH,16,16);
                       }
                   }
               }
             
           }
           catch { }
       }
     
       /// <summary>
       /// 设置村级行政区内所有的最大地籍号
       /// </summary>
       /// <param name="vTree"></param>
       public static void SetvTreeVillage(System.Windows.Forms.TreeView vTree)
       {
           if (m_cmbBoxVillage == null) { return; }
           try
           {
               vTree.Nodes.Clear();
               vTree.Nodes.Add(GetDMChineseName(m_cmbBoxCountry), GetDMChineseName(m_cmbBoxCountry), 16, 16);
               vTree.ExpandAll();
               TreeNode tNode1 = vTree.Nodes[0];
               tNode1.Nodes.Add(GetDMChineseName(m_cmbBoxVillage), GetDMChineseName(m_cmbBoxVillage), 16, 16);
               tNode1.ExpandAll();
               TreeNode tNode2 = tNode1.Nodes[0];
               Dictionary<string, Int64> pTouchedgroup = new Dictionary<string, Int64>();
               int Index = m_ZDFeatureClass.Fields.FindField(_DJHFieldName);
               IFeatureCursor pFeatureCursor = GetZDCursor(m_cmbBoxVillage);
               IFeature pFeature = pFeatureCursor.NextFeature();
               while (pFeature != null)
               {
                   string strValue = pFeature.get_Value(Index).ToString();
                   if (strValue.Length >= 12)
                   {
                       pTouchedgroup = GetTouchedGroup(pTouchedgroup, strValue);
                   }
                   pFeature = pFeatureCursor.NextFeature();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               pFeatureCursor = null;
               if (pTouchedgroup.Keys.Count == 0) { return; }
               foreach (string str in pTouchedgroup.Keys)
               {
                   string strJFDM = GetJFMCChineseName(Convert.ToString((pTouchedgroup[str])).Substring(0, 12), GetJFMC(Convert.ToString((pTouchedgroup[str]))));
                   string strMaxDJH = Convert.ToString((pTouchedgroup[str]));
                   tNode2.Nodes.Add(strJFDM, strJFDM, 16, 16);
                   TreeNode tNode3 = tNode2.Nodes[tNode2.Nodes.Count-1];
                   tNode3.Nodes.Add(strMaxDJH, strMaxDJH, 16, 16);
               }
            
           }
           catch { }
       }
       /// <summary>
       /// 设置街坊的最大地籍号查询
       /// </summary>
       /// <param name="vTree"></param>
       public static void SetvTreeNeighbour(System.Windows.Forms.TreeView vTree)
       {
           if (m_cmbBoxNeighbour == "") { return; }
           try
           {
               vTree.Nodes.Clear();
               vTree.Nodes.Add(GetDMChineseName(m_cmbBoxCountry), GetDMChineseName(m_cmbBoxCountry), 16, 16);
               vTree.ExpandAll();
               TreeNode tNode1 = vTree.Nodes[0];
               tNode1.Nodes.Add(GetDMChineseName(m_cmbBoxVillage), GetDMChineseName(m_cmbBoxVillage), 16, 16);
               tNode1.ExpandAll();
               TreeNode tNode2 = tNode1.Nodes[0];
               IFeatureCursor pFeatureCursor = GetZDCursor(m_cmbBoxNeighbour);
               int Index = m_ZDFeatureClass.Fields.FindField(_DJHFieldName);
               Dictionary<string, Int64> pTouchedgroup = new Dictionary<string, Int64>();
               IFeature pFeature = pFeatureCursor.NextFeature();
               while (pFeature != null)
               {
                   string strValue = pFeature.get_Value(Index).ToString();
                   if (strValue.Length >= 12)
                   {
                       pTouchedgroup = GetTouchedGroup(pTouchedgroup, strValue);
                   }
                   pFeature = pFeatureCursor.NextFeature();
               }
               if (pTouchedgroup.Keys.Count == 0) { return; }
               foreach (string str in pTouchedgroup.Keys)
               {
                   string strJFDM = GetJFMCChineseName(Convert.ToString((pTouchedgroup[str])).Substring(0, 12), GetJFMC(Convert.ToString((pTouchedgroup[str]))));
                   string strMaxDJH = Convert.ToString((pTouchedgroup[str]));
                   tNode2.Nodes.Add(strJFDM, strJFDM, 16, 16);
                   TreeNode tNode3 = tNode2.Nodes[tNode2.Nodes.Count - 1];
                   tNode3.Nodes.Add(strMaxDJH, strMaxDJH, 16, 16);
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               pFeatureCursor = null;
           }
           catch { }
       }
       /// <summary>
       /// 设置林地图斑的最大图斑号查询
       /// </summary>
       /// <param name="vTree"></param>
       public static void SetLinBanTreeNeighbour(System.Windows.Forms.TreeView vTree)
       {
           if (m_cmbBoxNeighbour == "") { return; }
           try
           {
               vTree.Nodes.Clear();
               vTree.Nodes.Add(GetDMChineseName(m_cmbBoxCountry), GetDMChineseName(m_cmbBoxCountry), 16, 16);
               vTree.ExpandAll();
               TreeNode tNode1 = vTree.Nodes[0];
               tNode1.Nodes.Add(GetDMChineseName(m_cmbBoxVillage), GetDMChineseName(m_cmbBoxVillage), 16, 16);
               tNode1.ExpandAll();
               TreeNode tNode2 = tNode1.Nodes[0];
               IFeatureCursor pFeatureCursor = GetLinBanCursor(m_cmbBoxNeighbour);
               int Index = m_ZDFeatureClass.Fields.FindField(_DJHFieldName);
               int IndexXzq = m_ZDFeatureClass.Fields.FindField(_JFDMFieldName);
               Dictionary<string, Int64> pTouchedgroup = new Dictionary<string, Int64>();
               IFeature pFeature = pFeatureCursor.NextFeature();
               while (pFeature != null)
               {
                   string strValue = pFeature.get_Value(Index).ToString();
                   string strXzq = pFeature.get_Value(IndexXzq).ToString();
                   //if (strValue.Length >= 12)
                   //{
                   pTouchedgroup = GetLinBanTouchedGroup(pTouchedgroup,strXzq,strValue);
                   //}
                   pFeature = pFeatureCursor.NextFeature();
               }
               if (pTouchedgroup.Keys.Count == 0) { return; }
               foreach (string str in pTouchedgroup.Keys)
               {
                   string strJFDM = GetJFMCChineseName(str,str);
                   string strMaxDJH = Convert.ToString((pTouchedgroup[str]));
                   tNode2.Nodes.Add(strJFDM, strJFDM, 16, 16);
                   TreeNode tNode3 = tNode2.Nodes[tNode2.Nodes.Count - 1];
                   tNode3.Nodes.Add(strMaxDJH, strMaxDJH, 16, 16);
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               pFeatureCursor = null;
           }
           catch { }
       }
       /// <summary>
       /// 最大林斑号重组
       /// </summary>
       /// <param name="pTouchedgroup"></param>
       /// <param name="strValue">村级行政区代码</param>
       /// <returns></returns>
       private static Dictionary<string, Int64> GetLinBanTouchedGroup(Dictionary<string, Int64> pTouchedgroup,string strXZQ, string strValue)
       {
           if (pTouchedgroup.Keys.Count == 0) { pTouchedgroup.Add(strXZQ, Convert.ToInt64(strValue)); return pTouchedgroup; }
           bool IsExist = false;
           foreach (string str in pTouchedgroup.Keys)
           {
               if (str == strXZQ)
               {
                   IsExist = true;
                   break;
               }
           }
           if (IsExist)
           {
               if (Convert.ToInt64(strValue) > pTouchedgroup[strXZQ])
               {
                   pTouchedgroup[strXZQ] = Convert.ToInt64(strValue);
               }
           }
           else
           {
               pTouchedgroup.Add(strXZQ, Convert.ToInt64(strValue));
           }

           return pTouchedgroup;
       }
       /// <summary>
       /// 最大地籍号重组
       /// </summary>
       /// <param name="pTouchedgroup"></param>
       /// <param name="strValue">村级行政区代码</param>
       /// <returns></returns>
       private static Dictionary<string, Int64> GetTouchedGroup(Dictionary<string, Int64> pTouchedgroup, string strValue)
       {
           if (pTouchedgroup.Keys.Count == 0) { pTouchedgroup.Add(strValue.Substring(0, 12), Convert.ToInt64(strValue)); return pTouchedgroup; }
           bool IsExist = false;
           foreach(string str in pTouchedgroup.Keys)
           {
               if (str == strValue.Substring(0, 12))
               {
                   IsExist = true;
                   break;
               }
           }
           if (IsExist)
           {
               if (Convert.ToInt64(strValue) > pTouchedgroup[strValue.Substring(0, 12)])
               {
                   pTouchedgroup[strValue.Substring(0, 12)] = Convert.ToInt64(strValue);
               }
           }
           else
           {
               pTouchedgroup.Add(strValue.Substring(0, 12), Convert.ToInt64(strValue));
           }
          
           return pTouchedgroup;
       }
       /// <summary>
       /// 根据街坊代码查询最大地籍号
       /// </summary>
       /// <param name="strNeighbour"></param>
       /// <returns></returns>
       public static IFeature Query(string strNeighbour)
       {
           IFeature pMaxFeatuer = null;
       
           try
           {
               double MaxDJH = 0;
               int Index = m_ZDFeatureClass.Fields.FindField(_DJHFieldName);
               IFeatureCursor pFeatureCursor = GetLinBanCursor(strNeighbour);
               IFeature pFeature = pFeatureCursor.NextFeature();


               while (pFeature != null)
               {

                   double strFieldName = Convert.ToDouble(pFeature.get_Value(Index).ToString());
                   if (strFieldName > MaxDJH)
                   {
                       MaxDJH = strFieldName;
                       pMaxFeatuer = pFeature;
                   }
                   pFeature = pFeatureCursor.NextFeature();
               }
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
               pFeatureCursor = null;


               return pMaxFeatuer;
           }
           catch
           {
               return pMaxFeatuer;
           }
       }
       
    }
}
