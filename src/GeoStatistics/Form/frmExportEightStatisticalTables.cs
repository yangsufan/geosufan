using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Collections;

namespace GeoStatistics
{
    public partial class frmExportEightStatisticalTables : DevComponents.DotNetBar.Office2007Form
    {
       public frmExportEightStatisticalTables()
        {
            InitializeComponent();
        }
       public  IFeatureClass m_pFeatrueClass
        {
            get;
            set;
        }
       public string m_Caption
       {
           get;
           set;
       }
       private string m_Province = "";
       private string m_County="";
       private string m_City = "";
       private string m_ExportName = "";
       private string filePath = Application.StartupPath+"\\..\\Template";  //changed by chulili 20120828 修改模板路径
       IList<string> listProvince;
       IList<string> listCity;
       IList<string> listCounty;
       List<string> m_listUnit = new List<string>();
       int m_count = 0;
       private void frmExportEightStatisticalTables_Load(object sender, EventArgs e)
       {
           this.Text = m_Caption;
           InitializecbProvince(m_pFeatrueClass);
       }

        //初始化省级行政区列表
        //ygc 2012-8-21
       private void InitializecbProvince(IFeatureClass pFeatureClass)
       {
           listProvince = new List<string>();

           IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
           IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
           if (pFW == null) return;
           if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return ;
           ITable pTable = pFW.OpenTable("行政区字典表");

           int ndx = pTable.FindField("NAME"),
           cdx = pTable.FindField("CODE");

           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = "XZJB='" + 1 + "'";

           ICursor pCursor = pTable.Search(pQueryFilter, false);
           if (pCursor == null)  return;

           IRow pRow = pCursor.NextRow();

           listProvince.Clear();
           while (pRow != null)
           {
               cbProvince.Items.Add(pRow.get_Value(ndx).ToString ());
               listProvince.Add(pRow.get_Value(cdx).ToString ());
               pRow = pCursor.NextRow();
           }
           if (listProvince.Count <= 0)
           {
               MessageBox.Show("无省级行政区数据！","错误",MessageBoxButtons .OK ,MessageBoxIcon.Error );
               return;
           }
           cbProvince.SelectedIndex = 0;
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
       }
       //获取指定字段的唯一值
        //ygc 2012-8-21
       private string GetChineseName(string code)
       {
           string ChineseName = "";
           IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
           IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
           if (pFW == null) return code;
           if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表"))return null;
           ITable pTable = pFW.OpenTable("行政区字典表");

           int ndx = pTable.FindField("NAME");

           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = "code='" + code + "'";

           ICursor pCursor = pTable.Search(pQueryFilter, false);
           if (pCursor == null) return code;

           IRow pRow = pCursor.NextRow();
           if (pRow == null) return code;
           while (pRow != null)
           {
               ChineseName = pRow.get_Value(ndx).ToString();
               pRow = pCursor.NextRow();
           }
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
           return ChineseName;
       }
        //根据选择的市获取该市级行政区中县级行政区唯一值 ygc 2012-8-27
       private IList<string> GetUniqueXian(IFeatureClass pFeatureClass, string shi)
       {
           IList<string> newListCode = new List<string>(); 
           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = "shi='"+shi+"'";
           ICursor pCursor = pFeatureClass.Search(pQueryFilter, false) as ICursor;
           IDataStatistics dataStatistics = new DataStatisticsClass();
           dataStatistics.Field = "xian";
           dataStatistics.Cursor = pCursor;
           System.Collections.IEnumerator enumerator = dataStatistics.UniqueValues;
           enumerator.Reset();
           while (enumerator.MoveNext())
           {
               newListCode.Add(enumerator.Current.ToString());
           }
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
           return newListCode;
       }
       private void btnExportData_Click(object sender, EventArgs e)
       {
           if (m_County == "")
           {
               m_County = m_City;
               m_ExportName = cbCity.Text.ToString();
           }
           if (m_City == "")
           {
               m_County = m_Province;
               m_ExportName = cbProvince.Text.ToString();
           }
           if (m_Caption == "" || m_Caption == null) return;
           if (m_ExportName == "")
           {
               m_ExportName = cbCounty.Text.ToString();
           }
           DataTable DtData=null ;
           switch (m_Caption)
           {
               case "林地质量等级":
                   DtData = GetStatisticalTable("EIGHTTABLE_LDZL", m_County, m_pFeatrueClass);
                   ExportDataTableToEXCEL(DtData, "林地质量等级");
                   break;
               case "林地利用方向规划面积":
                   DtData = GetStatisticalTable("EIGHTTABLE_LDLYFXGHMJ", m_County, m_pFeatrueClass);
                    ExportDataTableToEXCEL(DtData, "林地利用方向规划面积");
                   break;
               case "林地现状":
                   DtData =GetStatisticalTable("EIGHTTABLE_LDXZ", m_County, m_pFeatrueClass);
                   ExportLDXZTable(DtData, "林地现状");
                   break;
               case "林地及森林面积规划":
                   DtData=GetStatisticalTable("EIGHTTABLE_LDSLMJGH", m_County, m_pFeatrueClass);
                   ExportDataTableToEXCEL(DtData, "林地及森林面积规划");
                   break;
               case "林地结构现状":
                   DtData = GetStatisticalTable("EIGHTTABLE_LDJGXZ", m_County, m_pFeatrueClass);
                   ExportGJGYHDJ(GetStatisticalTable("EIGHTTABLE_LDJGXZ", m_County, m_pFeatrueClass), "林地结构现状");
                   break;
               case "国家级公益林地分保护等级现状":
                   DtData =GetStatisticalTable("EIGHTTABLE_GJGYHDJ", m_County, m_pFeatrueClass);
                   ExportGJGYHDJ(DtData, "国家级公益林地分保护等级现状");
                   break;
               case "林地保护等级":
                   DtData = GetStatisticalTable("EIGHTTABLE_LDBHDJTABLE", m_County, m_pFeatrueClass);
                   ExportDataTableToEXCEL(DtData, "林地保护等级");
                   break;
               case "国家级公益林地规划面积":
                   DtData = GetStatisticalTable("EIGHTTABLE_GJGYLGHMJ", m_County, m_pFeatrueClass);
                   ExportGJGYLGHMJ(DtData, "国家级公益林地规划面积");
                   break;
               case "各类土地面积统计":
                   DtData = GetStatisticalTable("ECOSTABLE_GLTDMJ", m_County, m_pFeatrueClass);
                   ExportGLTDMJ(DtData, "各类土地面积统计");
                   break;
               case "各类森林、林木面积蓄积统计":
                   DtData = GetStatisticalTable("ECOSTABLE_GLSLMJ", m_County, m_pFeatrueClass);
                   ExportGLTDMJ(DtData, "各类森林、林木面积蓄积统计");
                   break;
               case "林种统计":
                   DtData = GetStatisticalTable("ECOSTABLE_LZ", m_County, m_pFeatrueClass);
                   ExportGLTDMJ(DtData, "林种统计");
                   break;
               case "乔木林面积蓄积按龄组统计":
                   DtData = GetStatisticalTable("ECOSTABLE_QMLMJ", m_County, m_pFeatrueClass);
                   ExportQML(DtData, "乔木林面积蓄积按龄组统计");
                   break;
               case "生态公益林（地）统计":
                   DtData = GetStatisticalTable("ECOSTABLE_STGYL", m_County, m_pFeatrueClass);
                   ExportSTGYL(DtData, "生态公益林（地）统计");
                   break;
               case "用材林面积蓄积按龄级统计":
                   DtData = GetStatisticalTable("ECOSTABLE_YCLMJXJ", m_County, m_pFeatrueClass);
                   ExportQML(DtData, "用材林面积蓄积按龄级统计");
                   break;
               case "经济林统计":
                   DtData = GetStatisticalTable("ECOSTABLE_JJL", m_County, m_pFeatrueClass);
                   ExportSTGYL(DtData, "经济林统计");
                   break;
               case "灌木林统计":
                   DtData = GetStatisticalTable("ECOSTABLE_GML", m_County, m_pFeatrueClass);
                   ExportQML(DtData, "灌木林统计");
                   break;
               case "保护信息":
                   DtData = GetStatisticalTable("DATA_BHXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "保护信息");
                   break;
               case "地类信息":
                   DtData = GetStatisticalTable("DATA_DLXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "地类信息");
                   break ;
               case "工程类别信息":
                   DtData = GetStatisticalTable("DATA_GCLB", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "工程类别");
                   break;
               case "基本信息":
                   DtData = GetStatisticalTable("DATA_JBXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "基本信息");
                   break;
               case "林地结构信息":
                   DtData = GetStatisticalTable("DATA_LDJG", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "林地结构信息");
                   break;
               case "林种信息":
                   DtData = GetStatisticalTable("DATA_LZXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "林种信息");
                   break;
               case "起源信息":
                   DtData = GetStatisticalTable("DATA_QYXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "起源信息");
                   break;
               case "权属信息":
                   DtData = GetStatisticalTable("DATA_QSXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "权属信息");
                   break;
               case "灾害等级":
                   DtData = GetStatisticalTable("DATA_ZHDJ", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "灾害等级信息");
                   break;
               case "灾害类型":
                   DtData = GetStatisticalTable("DATA_ZHLX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "灾害类型");
                   break;
               case "质量等级信息":
                   DtData = GetStatisticalTable("DATA_ZLDJXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "质量等级信息");
                   break;
               case "主要树种":
                   DtData = GetStatisticalTable("DATA_SZXX", m_County, m_pFeatrueClass);
                   ExportXXTable(DtData, "树种信息");
                   break;
           }
       }

       #region 利用SQL语句获得八类统计表数据
       //删除临时表
        //ygc 2012-8-21
       private void DropTable(IWorkspace pWks, string strTableName)
       {
           try
           {
               pWks.ExecuteSQL("drop table " + strTableName);

           }
           catch
           { }
       }
        //导出完成后关闭EXCEl进程
        //ygc 2012-8-21
       public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
       {
           //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

           int k = 0;
           GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
           System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
           p.Kill();     //关闭进程k
       }
       [DllImport("User32.dll", CharSet = CharSet.Auto)]
       public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        //通过SQL语句获得林地质量等级统计表
        //ygc 2012-8-21
       private DataTable GetLDZLDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempLDZLTable");
           try
           {
               //查询县级统计数据
               //ygc 2012-8-21
               string stringSQL = "create table tempLDZLTable as select " + tableName + ".xian as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + StatisticsFieldName + " end) as 合计 ," +
                                   "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + StatisticsFieldName + " end) as Ⅰ级," +
                                    "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + StatisticsFieldName + " end) as Ⅱ级," +
                                    "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + StatisticsFieldName + " end) as Ⅲ级," +
                                    "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + StatisticsFieldName + " end) as Ⅳ级," +
                                    "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + StatisticsFieldName + " end) as Ⅴ级 " +
                                     "  from " + tableName +
                                     "  where xian='" + country + "'" +
                                     "  group by xian";
               pWorkspace.ExecuteSQL(stringSQL);
               //查询乡镇级统计数据
               string townsSQL = "insert into tempLDZLTable select  " + tableName + ".xiang as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + StatisticsFieldName + " end) as 合计 ," +
                                 "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + StatisticsFieldName + "  end) as Ⅰ级," +
                                 "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + StatisticsFieldName + "  end) as Ⅱ级," +
                                 "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + StatisticsFieldName + "  end) as Ⅲ级," +
                                 "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + StatisticsFieldName + "  end) as Ⅳ级," +
                                 "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + StatisticsFieldName + "  end) as Ⅴ级 " +
                                 "  from " + tableName +
                                 "  where xian='" + country + "'" +
                                 "  group by xiang";
               pWorkspace.ExecuteSQL(townsSQL);
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDZLTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;
               DropTable(pWorkspace, "tempLDZLTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
        //通过SQL语句获得林地保护等级统计表
        //ygc 2012-8-22
       private DataTable GetLDBHDJDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempLDBHDJTable");
           try
           {
               //查询县级统计数据SQL语句
               //ygc 2012-8-22
               string CitySQL = "create table tempLDBHDJTable as select  " +
                                 tableName + ".xian as 统计单位," +
                                "sum(case when (BCLD<>'2' or bcld is null)and  bh_dj between '1'and '4' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 现状合计," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状1级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状2级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状3级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状4级," +
                                "sum(case when  LB_GHBHDJ between '1'and '4'then " + tableName + "." + StatisticsFieldName + " end) as 规划合计," +
                                "sum(case when  LB_GHBHDJ='1' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划1级," +
                                "sum(case when  LB_GHBHDJ='2' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划2级," +
                                "sum(case when  LB_GHBHDJ='3' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划3级," +
                                "sum(case when  LB_GHBHDJ='4' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划4级" +
                                " from " + tableName +
                                " where xian ='" + country + "'" +
                                " group by xian";
               pWorkspace.ExecuteSQL(CitySQL);
               //查询乡级统计数据SQL语句
               //ygc 2012-8-22
               string townsSQL = "insert into tempLDBHDJTable select  " +
                                 tableName + ".xiang as 统计单位," +
                                "sum(case when (BCLD<>'2' or bcld is null)and bh_dj between '1'and '4' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 现状合计," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状1级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状2级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状3级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 现状4级," +
                                "sum(case when  LB_GHBHDJ between '1'and '4'then " + tableName + "." + StatisticsFieldName + " end) as 规划合计," +
                                "sum(case when  LB_GHBHDJ='1' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划1级," +
                                "sum(case when  LB_GHBHDJ='2' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划2级," +
                                "sum(case when  LB_GHBHDJ='3' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划3级," +
                                "sum(case when  LB_GHBHDJ='4' then  " + tableName + "." + StatisticsFieldName + " else 0 end) as 规划4级" +
                                " from " + tableName +
                                " where xian ='" + country + "'" +
                                " group by xiang";
               pWorkspace.ExecuteSQL(townsSQL);
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDBHDJTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;
               DropTable(pWorkspace, "tempLDBHDJTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
       //通过SQL语句获得林地利用方向规划面积统计表
        //ygc 2012-8-22
       private DataTable GetLDLYFXGHMJDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempLDLYFXGHMJTable");
           try
           {
               #region 执行统计
               //查询县级统计数据SQL语句
               //ygc 2012-8-22
               string CitySQL = "create table tempLDLYFXGHMJTable as select " +
                                tableName + ".xian as 统计单位," +
                                "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then " + tableName + "." + StatisticsFieldName + " else 0 end) as 合计," +
                                "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 公益林合计," +
                                "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 重点公益林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益小计," +
                                "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then " + tableName + "." + StatisticsFieldName + "  else 0 end) as  重点公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "   end ) as 一般公益合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 一般商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般其他" +
                                " from " + tableName +
                                " where xian='" + country + "'" +
                                " group by xian";
               pWorkspace.ExecuteSQL(CitySQL);
               //查询乡镇级统计数据
               //ygc 2012-8-22
               string townsSQL = "insert  into tempLDLYFXGHMJTable  select " +
                                tableName + ".xiang as 统计单位," +
                                "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then " + tableName + "." + StatisticsFieldName + " else 0 end) as 合计," +
                                "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 公益林合计," +
                                "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 重点公益林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益小计," +
                                "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then " + tableName + "." + StatisticsFieldName + "  else 0 end) as  重点公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "   end ) as 一般公益合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 一般商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般其他" +
                                " from " + tableName +
                                " where xian='" + country + "'" +
                                " group by xiang";
               pWorkspace.ExecuteSQL(townsSQL);
               #endregion
               #region 更新统计
               //更新公益林小计
               //ygc 2012-8-22
               string UpdateXJ = "update tempLDLYFXGHMJTable set 重点公益小计=重点公益防护林+ 重点公益特用林";
               pWorkspace.ExecuteSQL(UpdateXJ);
               //更新重点公益林合计 ygc 2012-8-22
               string UpdateZDFYHJ = "update tempLDLYFXGHMJTable set 重点公益林合计=重点公益小计+重点公益其他";
               pWorkspace.ExecuteSQL(UpdateZDFYHJ);
               //更新一般公益小计 ygc 2012-8-22
               string UpdateYBGYXJ = "update tempLDLYFXGHMJTable set 一般公益小计=一般公益防护林+一般公益特用林";
               pWorkspace.ExecuteSQL(UpdateYBGYXJ);
               //更新一般公益合计 ygc 2012-8-22
               string UpdateYBGYHJ = "update tempLDLYFXGHMJTable set 一般公益合计=一般公益小计+一般公益其他";
               pWorkspace.ExecuteSQL(UpdateYBGYHJ);
               //更新公益林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 公益林合计=一般公益合计 + 重点公益林合计");
               //更新重点商品林小计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
               //更新重点商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 重点商品林合计=重点商品林小计+重点其他");
               //更新一般商品林小计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
               //更新一般商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 一般商品林合计=一般商品林小计+一般其他");
               //更新商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 商品林合计 =一般商品林合计+重点商品林合计");
               //更新总计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDLYFXGHMJTable set 合计=商品林合计+公益林合计");
               #endregion
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDLYFXGHMJTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;
               DropTable(pWorkspace, "tempLDLYFXGHMJTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
       //通过SQL语句获得林地结构现状统计表 ygc 2012-8-23
       private DataTable GetLDJGXZDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           m_count = 0;
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempLDJGXZTable");
           try
           {
               #region 执行统计
               //查询县级统计数据SQL语句
               //ygc 2012-8-22
               string CitySQL = "create table tempLDJGXZTable as select " +
                                tableName + ".xian as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 公益林合计," +
                                "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 重点公益林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益小计," +
                                "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then " + tableName + "." + StatisticsFieldName + "  else 0 end) as  重点公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "   end ) as 一般公益合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 一般商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般其他" +
                                " from " + tableName +
                                " where xian='" + country + "' and substr(QI_YUAN,1,1)<>' '" +
                                " group by xian,rollup(substr(QI_YUAN,1,1)) " +
                                " order by substr(QI_YUAN,1,1) ";
               pWorkspace.ExecuteSQL(CitySQL);
               //查询乡镇级统计数据
               //ygc 2012-8-22
               string townsSQL = "insert  into tempLDJGXZTable  select " +
                                tableName + ".xiang as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 公益林合计," +
                                "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 重点公益林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益小计," +
                                "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then " + tableName + "." + StatisticsFieldName + "  else 0 end) as  重点公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "   end ) as 一般公益合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益防护林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般公益特用林," +
                                "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般公益其他," +
                                "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 重点商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 重点其他," +
                                "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then " + tableName + "." + StatisticsFieldName + "  else 0 end) as 一般商品林合计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then " + tableName + "." + StatisticsFieldName + " else 0 end ) as 一般商品林小计," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般用材林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般经济林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般薪碳林," +
                                "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then " + tableName + "." + StatisticsFieldName + " else 0 end) as 一般其他" +
                                " from " + tableName +
                                " where xian='" + country + "'and substr(QI_YUAN,1,1)<>' '" +
                                " group by xiang,rollup(substr(QI_YUAN,1,1)) " +
                                " order by substr(QI_YUAN,1,1)";
               pWorkspace.ExecuteSQL(townsSQL);
               #endregion
               #region 更新统计
               //更新公益林小计
               //ygc 2012-8-22
               string UpdateXJ = "update tempLDJGXZTable set 重点公益小计=重点公益防护林+ 重点公益特用林";
               pWorkspace.ExecuteSQL(UpdateXJ);
               //更新重点公益林合计 ygc 2012-8-22
               string UpdateZDFYHJ = "update tempLDJGXZTable set 重点公益林合计=重点公益小计+重点公益其他";
               pWorkspace.ExecuteSQL(UpdateZDFYHJ);
               //更新一般公益小计 ygc 2012-8-22
               string UpdateYBGYXJ = "update tempLDJGXZTable set 一般公益小计=一般公益防护林+一般公益特用林";
               pWorkspace.ExecuteSQL(UpdateYBGYXJ);
               //更新一般公益合计 ygc 2012-8-22
               string UpdateYBGYHJ = "update tempLDJGXZTable set 一般公益合计=一般公益小计+一般公益其他";
               pWorkspace.ExecuteSQL(UpdateYBGYHJ);
               //更新公益林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 公益林合计=一般公益合计 + 重点公益林合计");
               //更新重点商品林小计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
               //更新重点商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 重点商品林合计=重点商品林小计+重点其他");
               //更新一般商品林小计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
               //更新一般商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 一般商品林合计=一般商品林小计+一般其他");
               //更新商品林合计 ygc 2012-8-22
               pWorkspace.ExecuteSQL("update tempLDJGXZTable set 商品林合计 =一般商品林合计+重点商品林合计");
               #endregion
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDJGXZTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;
               m_count = GetCountUnique("统计单位", "tempLDJGXZTable", pFeatureClass);
               DropTable(pWorkspace, "tempLDJGXZTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }

           return StatisticsDT;
       }
       //通过SQL语句获得国家级公益林地分保护等级现状统计表 ygc 2012-8-23
       private DataTable GetGJGYHDJDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           m_count = 0;
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempGJGYHDJTable");
           try
           {
               //通过SQL语句获得县级统计数据 ygc 2012-8-23
               string CitySQL = "create table tempGJGYHDJTable as select " +
                                "xian as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when BHDJ between '1' and '3' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 合计," +
                                "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 防护林合计," +
                                "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 特用林合计," +
                                "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 其他合计," +
                                "sum(case when BHDJ ='1' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 一级小计," +
                                "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end ) as 一级防护," +
                                "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 一级特用," +
                                "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 一级其他," +
                                "sum(case when BHDJ ='2' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 二级小计," +
                                "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 二级防护," +
                                "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end ) as 二级特用," +
                                "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 二级其他," +
                                "sum(case when BHDJ ='3' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 三级小计," +
                                "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 三级防护," +
                                "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then " + StatisticsFieldName + " else 0 end) as 三级特用," +
                                "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as  三级其他" +
                                "  from " + tableName +
                                "  where xian='" + country + "' and substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                "  group by xian,rollup(substr(QI_YUAN,1,1))" +
                                "  order by substr(QI_YUAN,1,1) ";
               pWorkspace.ExecuteSQL(CitySQL);
               //利用SQL语句查询统计乡镇数据 ygc 2012-8-23
               string townsSQL = "insert into tempGJGYHDJTable select " +
                                "xiang as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when BHDJ between '1' and '3' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 合计," +
                                "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 防护林合计," +
                                "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 特用林合计," +
                                "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 其他合计," +
                                "sum(case when BHDJ ='1' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 一级小计," +
                                "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end ) as 一级防护," +
                                "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 一级特用," +
                                "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 一级其他," +
                                "sum(case when  BHDJ ='2' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 二级小计," +
                                "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 二级防护," +
                                "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then " + StatisticsFieldName + " else 0 end ) as 二级特用," +
                                "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as 二级其他," +
                                "sum(case when BHDJ ='3' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 三级小计," +
                                "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then " + StatisticsFieldName + " else 0 end) as 三级防护," +
                                "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then " + StatisticsFieldName + " else 0 end) as 三级特用," +
                                "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then " + StatisticsFieldName + " else 0 end) as  三级其他" +
                                "  from " + tableName +
                                "  where xian='" + country + "' and substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                "  group by xiang,rollup(substr(QI_YUAN,1,1))" +
                                "  order by substr(QI_YUAN,1,1) ";
               pWorkspace.ExecuteSQL(townsSQL);
               //更新合计
               pWorkspace.ExecuteSQL("update tempGJGYHDJTable set 合计= 防护林合计+特用林合计+其他合计");
               //更新一级合计
               pWorkspace.ExecuteSQL("update tempGJGYHDJTable set 一级小计=一级防护+一级特用+一级其他 ");
               //更新二级小计
               pWorkspace.ExecuteSQL("update tempGJGYHDJTable set 二级小计=二级防护+二级特用+二级其他");
               //更新三级小计
               pWorkspace.ExecuteSQL("update tempGJGYHDJTable set 三级小计=三级防护+三级特用+三级其他");

               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempGJGYHDJTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;
               m_count = GetCountUnique("统计单位", "tempGJGYHDJTable", pFeatureClass);
               DropTable(pWorkspace, "tempGJGYHDJTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
       //通过SQL语句获得林地及森林面积规划统数据
        //ygc 2012-8-23
       private DataTable GetLDSLMJGHDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }

           DropTable(pWorkspace, "tempLDSLMJGHTable");
           try
           {
               //查询县级数据SQL语句 ygc 2012-8-23
               string CitySQL = "create table tempLDSLMJGHTable as select " +
                                "xian as 统计单位," +
                                "sum(case when DL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 现状林地," +
                                "sum(case when (DL LIKE '11%' OR DL='131') then " + StatisticsFieldName + " else 0 end) as 现状森林," +
                                "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充合计," +
                                "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充未利用地," +
                                "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充建设用地," +
                                "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充牧草地," +
                                "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充耕地," +
                                "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充其他," +
                                "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加合计," +
                                "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加林地," +
                                "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加未利用地," +
                                "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加建设用地," +
                                "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加草地," +
                                "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加耕地," +
                                "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加其他" +
                                "  from " + tableName +
                                "  where xian ='" + country + "'" +
                                "  group by xian";
               pWorkspace.ExecuteSQL(CitySQL);
               //查询乡镇及统计数据SQL ygc 2012-8-23
               string townsSQL = " insert into tempLDSLMJGHTable  select " +
                                "xiang as 统计单位," +
                                "sum(case when DL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 现状林地," +
                                "sum(case when (DL LIKE '11%' OR DL='131') then " + StatisticsFieldName + " else 0 end) as 现状森林," +
                                "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充合计," +
                                "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充未利用地," +
                                "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充建设用地," +
                                "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充牧草地," +
                                "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充耕地," +
                                "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then " + StatisticsFieldName + " else 0 end) as 补充其他," +
                                "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加合计," +
                                "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加林地," +
                                "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加未利用地," +
                                "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加建设用地," +
                                "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加草地," +
                                "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加耕地," +
                                "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then " + StatisticsFieldName + " else 0 end) as 森林增加其他" +
                                "  from " + tableName +
                                "  where xian ='" + country + "'" +
                                "  group by xiang";
               pWorkspace.ExecuteSQL(townsSQL);
               //更新森林增加合计 ygc 2012-8-23
               pWorkspace.ExecuteSQL("update tempLDSLMJGHTable set 森林增加合计= 森林增加林地+森林增加未利用地+森林增加建设用地+森林增加草地+森林增加耕地+森林增加其他");
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDSLMJGHTable");
               StatisticsDT = ITableToDataTable(pTable);
               DropTable(pWorkspace, "tempLDSLMJGHTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
 
       }
       //通过SQL语句获得林地现状统计数据 ygc 2012-8-24
       private DataTable GetLDXZDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }

               DropTable(pWorkspace, "tempTable");
               DropTable(pWorkspace, "tempLDXZTable");
               //创建临时表
               try
               {
               pWorkspace.ExecuteSQL("create table tempTable as select * from " + tableName);
               //更新临时表中的权属信息
               pWorkspace.ExecuteSQL("update tempTable set LD_QS='国有' where LD_QS in('10','15')");
               pWorkspace.ExecuteSQL("update tempTable set LD_QS='集体' where LD_QS in('21','22','23','16')");
               pWorkspace.ExecuteSQL("update tempTable set LD_QS='其他' where LD_QS in('20')");
               //统计县级数据
               string CitySQL = "create table tempLDXZTable as select " +
                                "xian as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when DL <>' ' then " + StatisticsFieldName + " else 0 end) as 土地总面积," +
                                "sum(case when DL<=180 then " + StatisticsFieldName + " else 0 end) as 林地合计," +
                                "sum(case when DL between '111' and '114' then " + StatisticsFieldName + " else 0 end) as 有林地小计," +
                                "sum(case when DL='111' OR DL='112' OR DL='114' then " + StatisticsFieldName + " else 0 end) as 乔木林," +
                                "sum(case when DL='113' then " + StatisticsFieldName + " else 0 end) as 竹林," +
                                "sum(case when DL='114' then " + StatisticsFieldName + " else 0 end) as 红树林," +
                                "sum(case when DL='120' then " + StatisticsFieldName + " else 0 end) as 疏林地," +
                                "sum(case when DL between '131' and '133' then " + StatisticsFieldName + " else 0 end) as 灌木林地小计," +
                                "sum(case when DL='131' then " + StatisticsFieldName + " else 0 end) as 特灌," +
                                "sum(case when DL='133' or DL='132' then " + StatisticsFieldName + " else 0 end ) as 其他灌木林," +
                                "sum(case when DL='141' or DL='142' then " + StatisticsFieldName + " else 0 end) as 未成林造林地," +
                                "sum(case when DL='150' then " + StatisticsFieldName + " else 0 end) as 苗圃地," +
                                "sum(case when DL='161' or DL='162' or DL='163' then " + StatisticsFieldName + " else 0 end) as 无立木林地," +
                                "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then " + StatisticsFieldName + " else 0 end) as 宜林地," +
                                "sum(case when DL='180' then  " + StatisticsFieldName + " else 0 end ) as 林辅地," +
                                "sum(case when dl like '2%' then " + StatisticsFieldName + " else 0 end) as 非林地," +
                                "sum (case when dl=' ' then " + StatisticsFieldName + " else 0 end) as 森林覆盖率," +
                                "sum(case when dl=' ' then " + StatisticsFieldName + " else 0 end) as 林木绿化率" +
                                "  from tempTable" +
                                "  where LD_QS is not null and LD_QS<>' ' and xian='" + country + "' and substr(QI_YUAN,1,1)<>' '" +
                                "  group by xian,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
               pWorkspace.ExecuteSQL(CitySQL);

               //统计乡镇级数据
               string townsSQL = "insert into tempLDXZTable  select " +
                                "xiang as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                "sum(case when DL <>' ' then " + StatisticsFieldName + " else 0 end) as 土地总面积," +
                                "sum(case when DL<=180 then " + StatisticsFieldName + " else 0 end) as 林地合计," +
                                "sum(case when DL between '111' and '114' then " + StatisticsFieldName + " else 0 end) as 有林地小计," +
                                "sum(case when DL='111' OR DL='112' OR DL='114' then " + StatisticsFieldName + " else 0 end) as 乔木林," +
                                "sum(case when DL='113' then " + StatisticsFieldName + " else 0 end) as 竹林," +
                                "sum(case when DL='114' then " + StatisticsFieldName + " else 0 end) as 红树林," +
                                "sum(case when DL='120' then " + StatisticsFieldName + " else 0 end) as 疏林地," +
                                "sum(case when DL between '131' and '133' then " + StatisticsFieldName + " else 0 end) as 灌木林地小计," +
                                "sum(case when DL='131' then " + StatisticsFieldName + " else 0 end) as 特灌," +
                                "sum(case when DL='133' or DL='132' then " + StatisticsFieldName + " else 0 end ) as 其他灌木林," +
                                "sum(case when DL='141' or DL='142' then " + StatisticsFieldName + " else 0 end) as 未成林造林地," +
                                "sum(case when DL='150' then " + StatisticsFieldName + " else 0 end) as 苗圃地," +
                                "sum(case when DL='161' or DL='162' or DL='163' then " + StatisticsFieldName + " else 0 end) as 无立木林地," +
                                "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then " + StatisticsFieldName + " else 0 end) as 宜林地," +
                                "sum(case when DL='180' then  " + StatisticsFieldName + " else 0 end ) as 林辅地," +
                                "sum(case when dl like '2%' then " + StatisticsFieldName + " else 0 end) as 非林地," +
                                "sum (case when dl=' ' then " + StatisticsFieldName + " else 0 end) as 森林覆盖率," +
                                "sum(case when dl=' ' then " + StatisticsFieldName + " else 0 end) as 林木绿化率" +
                                "  from tempTable" +
                                "  where LD_QS is not null and LD_QS<>' ' and xian='" + country + "' and substr(QI_YUAN,1,1)<>' '" +
                                "  group by xiang,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
               pWorkspace.ExecuteSQL(townsSQL);
               //更新森林覆盖率
               pWorkspace.ExecuteSQL("update tempLDXZTable set 森林覆盖率=(有林地小计+特灌)/土地总面积 * 100 where 权属 is null");
               //更新林木绿化率
               pWorkspace.ExecuteSQL("update tempLDXZTable set 林木绿化率=(有林地小计+灌木林地小计)/土地总面积 * 100 where 权属 is null ");
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempLDXZTable");
               StatisticsDT = ITableToDataTable(pTable);
               m_count = GetCountUnique("统计单位", "tempLDXZTable", pFeatureClass);
               DropTable(pWorkspace, "tempLDXZTable");
               DropTable(pWorkspace, "tempTable");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
       //通过SQL 语句获得国家级公益林地规划面积统计表 ygc 2012-8-27
       private DataTable GetGJGYLGHMJDataTable(IFeatureClass pFeatureClass, string country, string StatisticsFieldName)
       {
           DataTable StatisticsDT = new DataTable();
           if (pFeatureClass == null)
           {
               return null;
           }
           if (country == "")
           {
               MessageBox.Show("请选择要统计的县级行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
           }
           IWorkspace pWorkspace = null;
           m_count=0;
           string tableName = "";
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
               tableName = (pFeatureClass as IDataset).Name;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "TempGJGYLGHMJTable");
           DropTable(pWorkspace ,"temp_table_1");
           DropTable(pWorkspace, "temp_table_2");
           try
           {
               //复制临时表 ygc 2012-8-27
               pWorkspace.ExecuteSQL("create table temp_table_1 as select * from  " + tableName);
               pWorkspace.ExecuteSQL("create table temp_table_2 as select * from  " + tableName);
               //更新临时表 ygc 2012-8-27 
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='防护林' where substr(lz,1,2)='11'");
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='特用林' where substr(lz,1,2)='12'");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='其他林地' where substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12' and substr(lz,1,2)<>' '");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='公益林' where (substr(lz,1,2)='11' or substr(lz,1,2)='12')");
               //通过SQL获得县级统计数据 ygc 2012-9-27 
               string CitySQL = " create table TempGJGYLGHMJTable as (select " +
                                "temp_table_1.xian as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_1" +
                                " where  ( lz<>' 'and ghlz<>' ') and xian='" + country + "' and (lz='特用林' or lz='防护林')" +
                                " group by xian,lz" +
                                " union all" +
                                " select " +
                                " xian as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_2" +
                                " where  ( lz<>' 'and ghlz<>' ') and xian='" + country + "'" +
                                " group by xian,rollup(lz))";
               pWorkspace.ExecuteSQL(CitySQL);
               //用SQL语句获取乡级行政区统计数据
               string townsSQL = " insert  into TempGJGYLGHMJTable  (select " +
                                "temp_table_1.xiang as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_1" +
                                " where  ( lz<>' 'and ghlz<>' ') and xian='" + country + "' and (lz='特用林' or lz='防护林')" +
                                " group by xiang,lz" +
                                " union all" +
                                " select " +
                                " xiang as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_2" +
                                " where  ( lz<>' 'and ghlz<>' ') and xian='" + country + "'" +
                                " group by xiang,rollup(lz))";
               pWorkspace.ExecuteSQL(townsSQL);

               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("TempGJGYLGHMJTable");
               StatisticsDT = ITableToDataTable(pTable);
               m_count = GetCountUnique("统计单位", "TempGJGYLGHMJTable", pFeatureClass);
               DropTable(pWorkspace, "TempGJGYLGHMJTable");
               DropTable(pWorkspace, "temp_table_1");
               DropTable(pWorkspace, "temp_table_2");
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.ToString ());
           }
           return StatisticsDT;
       }
       #endregion
       #region 将统计数据导出成EXCEL文件
       //将ITable转换成DataTable
        //ygc 2012-8-21
       private DataTable ITableToDataTable(ITable pip_Table)
        {
            DataTable lc_TableData = new DataTable("统计结果");
            if (pip_Table == null) return null;
            ICursor lip_Cursor=null;
            try
            {
                // 无数据返回空表
                if (pip_Table.RowCount(null) == 0) return null;
                // 给列赋值
                for (int index = 0; index < pip_Table.Fields.FieldCount; index++)
                {
                    DataColumn lc_DataColum = new DataColumn();
                    lc_DataColum.Caption = pip_Table.Fields.get_Field(index).AliasName;
                    lc_DataColum.DataType = System.Type.GetType("System.String");
                    lc_TableData.Columns.Add(lc_DataColum );
                }
                // 循环拷贝数据
                lip_Cursor = pip_Table.Search(null, false);
                if (lip_Cursor == null)
                {
                    return null;
                }
                IRow lip_Row = lip_Cursor.NextRow();
                lc_TableData.BeginLoadData();
                while (lip_Row != null)
                {
                    DataRow lc_Row = lc_TableData.NewRow();
                    for (int i = 0; i < pip_Table.Fields.FieldCount; i++)
                    { 
                        string values=lip_Row.get_Value(i).ToString ();
                        if(i==0)
                        {
                            lc_Row[i] = GetChineseName(values);
                        }
                        else if (i == 1)
                        {
                            lc_Row[i] = SysCommon.ModField.GetDomainValueOfFieldValue(m_pFeatrueClass, "QI_YUAN", values);
                        }
                        else if (i == 2)
                        {
                            lc_Row[i] = SysCommon.ModField.GetDomainValueOfFieldValue(m_pFeatrueClass, "QI_YUAN", values);
                        }
                        else
                        {
                            lc_Row[i] = values;
                        }
                        
                    }
                    lc_TableData.Rows.Add(lc_Row);
                    lip_Row = lip_Cursor.NextRow();
                }
                lc_TableData.EndLoadData();
                return lc_TableData;
            }
            catch (Exception ex)
            {
                return lc_TableData;
            }
            finally
            {
                if (lip_Cursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lip_Cursor);
                }
            }
        }
        //将DataTable导出成EXCEL
        //ygc 2012-8-21
       private void ExportDataTableToEXCEL(DataTable dt,string fileName)
       {
           //打开模板EXCEL
          Microsoft.Office.Interop.Excel.Application excel =new Microsoft.Office.Interop.Excel.Application();
          string fileNameString = filePath +"\\" +fileName +".xls";
          if (excel == null)
          {
              MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
              return;
          }
          if (dt == null)
          {
              MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
              Kill(excel);
              return;
          }
          Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
           //end
          int rowIndex = 0;
          switch (fileName)
          {
              case "林地质量等级":
                  rowIndex = 5;
                  break;
              case "林地保护等级":
                  rowIndex = 6;
                  break;
              case "林地利用方向规划面积":
                  rowIndex = 9;
                  break;
              case "林地及森林面积规划":
                  rowIndex = 7;
                  break;
              default :
                  rowIndex =9;
                  break ;
          }
           //添加进度条 ygc 2012-10-8
          SysCommon.CProgress vProgress = new SysCommon.CProgress();
          vProgress.ShowDescription = true;
          vProgress.ShowProgressNumber = true;
          vProgress.TopMost = true;
          vProgress.EnableCancel = true;
          vProgress.EnableUserCancel(true);
          vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
          vProgress.ProgresssValue = 0;
          vProgress.Step = 1;
          vProgress.ShowProgress();
          vProgress.SetProgress("正在统计" + fileName+"数据......");
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  int columnIndex = 1;
                  for (int j = 0; j < dt.Columns.Count; j++)
                  {
                      excel.Cells[rowIndex, columnIndex] = dt.Rows[i][j].ToString();
                      columnIndex++;
                      vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                  }
                  rowIndex++;
              }
              vProgress.Close();

              //导出后的数据另存
              this.Hide();      //added by chulili 20120828 本对话框先隐藏，避免选择路径对话框获取不到焦点
              Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
              fd.InitialFileName = m_ExportName + fileName + "统计表";
              int result = fd.Show();
              if (result == 0)
              {
                  //资源回收
                  System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                  System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                  Kill(excel);
                  GC.Collect();
                  return;
              }
              string fileName0 = fd.InitialFileName;
              if (!string.IsNullOrEmpty(fileName0))
              {
                  if (fileName0.IndexOf(".xls") == -1)
                  {
                      fileName0 += ".xls";
                  }
                  //ygc 2012-9-27 修改提示
                  if (File.Exists(fileName0))
                  {
                      DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                      if (rs != DialogResult.OK)
                      {
                          System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                          System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                          Kill(excel);
                          GC.Collect();
                          return;
                      }
                  }//end
                  //保存生成的统计表
                  excel.DisplayAlerts = false;
                  try
                  {
                      book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                  }
                  catch(Exception err)
                  {
                      MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                      System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                      System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                      Kill(excel);
                      GC.Collect();
                      this.Close();
                      return;
                  }
              }
              //资源回收
              System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
              System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
              Kill(excel);
              GC.Collect();
              DialogResult diaResult;
              diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
              if (diaResult == DialogResult.OK)
              {
                  Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                  Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                  excel1.Visible = true;
              }
              this.Close();
       }
       //导出国家级公益林地分保护等级现状统计表 ygc 2012-8-23
       private void ExportGJGYHDJ(DataTable dt, string fileName)
       {
           //打开模板EXCEL
           Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
           string fileNameString = filePath + "\\" + fileName + ".xls";
           if (excel == null)
           {
               MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
               return;
           }
           if (dt == null)
           {
               MessageBox.Show("无统计数据导出！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information );
               Kill(excel);
               return;
           }
           Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
           Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
           int rowIndex = 0;
           int row = 0;
           switch (fileName)
           {
               case "国家级公益林地分保护等级现状":
                   rowIndex = 7;
                   row = 7;
                   break;
               case "林地结构现状":
                   rowIndex = 9;
                   row = 9;
                   break;
               default:
                   rowIndex = 8;
                   row = 8;
                   break ;
           }

           if (m_count == 0) return;
           //写表头
           for (int i = 1; i <=m_count; i++)
           {
               //合并行
               xlsheet.get_Range(xlsheet.Cells[rowIndex, 1], xlsheet.Cells[rowIndex + 2, 1]).Merge(false);
               rowIndex = rowIndex + 3;
           }
           //导出数据
           SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
           vProgress.ShowDescription = true;
           vProgress.ShowProgressNumber = true;
           vProgress.TopMost = true;
           vProgress.EnableCancel = true;
           vProgress.EnableUserCancel(true);
           vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
           vProgress.ProgresssValue = 0;
           vProgress.Step = 1;
           vProgress.ShowProgress();
           vProgress.SetProgress("正在统计" + fileName + "数据......");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               int columnIndex = 1;
               for (int j = 0; j < dt.Columns.Count; j++)
               {
                   if (dt.Rows[i][j].ToString() == "")
                   {
                       excel.Cells[row, columnIndex] = "合计";
                   }
                   else
                   {
                       excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                   }
                   columnIndex++;
                   vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
               }
               row++;
           }
           vProgress.Close();
           this.Hide();
           //导出后的数据另存
           Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
           fd.InitialFileName = m_ExportName + fileName + "统计表";
           int result = fd.Show();
           if (result == 0)
           {
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               return; 
           }
           string fileName0 = fd.InitialFileName;
           if (!string.IsNullOrEmpty(fileName0))
           {
               if (fileName0.IndexOf(".xls") == -1)
               {
                   fileName0 += ".xls";
               }
               //ygc 2012-9-27 修改提示
               if (File.Exists(fileName0))
               {
                   DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                   if (rs != DialogResult.OK)
                   {
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                       Kill(excel);
                       GC.Collect();
                       return;
                   }
               }//end
               //保存生成的统计表
               excel.DisplayAlerts = false;
               //保存生成的统计表
               try
               {
                   book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               }
               catch (Exception err)
               {
                   MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                   Kill(excel);
                   GC.Collect();
                   this.Close();
                   return;
               }
           }

           //资源回收
           System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
           System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
           Kill(excel);
           GC.Collect();
           DialogResult diaResult;
           diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
           if (diaResult == DialogResult.OK)
           {
               Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
               Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
               excel1.Visible = true;
           }
           this.Close();
       }
       //导出国家级公益林地规划面积统计表 ygc 2012-8-23
        private void ExportGJGYLGHMJ(DataTable dt,string fileName)
        {
            //打开模板EXCEL
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            string fileNameString = filePath + "\\" + fileName + ".xls";
            if (excel == null)
            {
                MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                return;
            }
            if (dt == null)
            {
                MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kill(excel);
                return;
            }
            dt = GetDataTable(dt);
            Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
            Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            int rowIndex = 6;
            int row = 6;
            if (m_listUnit.Count == 0) return;
            //写行的表头
            for (int i = 0; i < m_listUnit .Count; i++)
            {
                //合并第一列
                xlsheet.get_Range(xlsheet.Cells[rowIndex, 1], xlsheet.Cells[rowIndex + 4, 1]).Merge(false);
                xlsheet.Cells[rowIndex, 1] = m_listUnit[i];
                //合并第二列
                xlsheet.get_Range(xlsheet.Cells[rowIndex+4, 2], xlsheet.Cells[rowIndex+4 , 3]).Merge(false);
                xlsheet.Cells[rowIndex+4, 2] = "总计";
                xlsheet.get_Range(xlsheet.Cells[rowIndex, 2], xlsheet.Cells[rowIndex + 2, 2]).Merge(false);
                xlsheet.Cells[rowIndex, 2] = "公益林";
                xlsheet.Cells[rowIndex, 3] = "特用林";
                xlsheet.Cells[rowIndex + 1, 3] = "防护林";
                xlsheet.Cells[rowIndex + 2, 3] = "合计";
                xlsheet.get_Range(xlsheet.Cells[rowIndex + 3, 2], xlsheet.Cells[rowIndex + 3, 3]).Merge(false);
                xlsheet.Cells[rowIndex + 3, 2] = "其他林地";
                rowIndex = rowIndex + 5;

            }



            //导出数据
            SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);
            vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            vProgress.SetProgress("正在统计" + fileName + "数据......");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int columnIndex = 4;
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                    columnIndex++;
                    vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                }
                row++;
            }
            vProgress.Close();
            this.Hide();
            //导出后的数据另存
            Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
            fd.InitialFileName = m_ExportName + fileName + "统计表";
            int result = fd.Show();
            if (result == 0)
            {
                //资源回收
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                Kill(excel);
                GC.Collect();
                return; 
            }
            string fileName0 = fd.InitialFileName;
            if (!string.IsNullOrEmpty(fileName0))
            {
                if (fileName0.IndexOf(".xls") == -1)
                {
                    fileName0 += ".xls";
                }
                //ygc 2012-9-27 修改提示
                if (File.Exists(fileName0))
                {
                    DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                        Kill(excel);
                        GC.Collect();
                        return;
                    }
                }//end
                //保存生成的统计表
                excel.DisplayAlerts = false;
                //保存生成的统计表
                try
                {
                    book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (Exception err)
                {
                    MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    Kill(excel);
                    GC.Collect();
                    this.Close();
                    return;
                }
            }
            //资源回收
            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            Kill(excel);
            GC.Collect();
            DialogResult diaResult;
            diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (diaResult == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                excel1.Visible = true;
            }
            this.Close();
        }
        //导出林地现状统计表
       private void ExportLDXZTable(DataTable dt, string fileName)
       {
           //打开模板EXCEL
           Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
           string fileNameString = filePath + "\\" + fileName + ".xls";
           if (excel == null)
           {
               MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
               return;
           }
           if (dt == null)
           {
               MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               Kill(excel);
               return;
           }
           Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
           Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

           //合并单元格
           for (int i = 0; i < dt.Rows.Count - 1; i++)
           {
               Microsoft.Office.Interop.Excel.Range r;
               Microsoft.Office.Interop.Excel.Range r1;
               if (dt.Rows[i][0].ToString().Trim() == dt.Rows[i + 1][0].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r = xlsheet.get_Range(xlsheet.Cells[8 + i, 1], xlsheet.Cells[9 + i, 1]);
                   r.Value2 = excel.Cells[8 + i, 1];
                   r.MergeCells = true;
               }
               if (dt.Rows[i][1].ToString().Trim() == dt.Rows[i + 1][1].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r1 = xlsheet.get_Range(xlsheet.Cells[8 + i, 2], xlsheet.Cells[9 + i, 2]);
                   r1.Value2 = excel.Cells[8 + i, 2];
                   r1.MergeCells = true;
               }
           }
           int row = 8;
           //导出数据
           SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
           vProgress.ShowDescription = true;
           vProgress.ShowProgressNumber = true;
           vProgress.TopMost = true;
           vProgress.EnableCancel = true;
           vProgress.EnableUserCancel(true);
           vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
           vProgress.ProgresssValue = 0;
           vProgress.Step = 1;
           vProgress.ShowProgress();
           vProgress.SetProgress("正在统计" + fileName + "数据......");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               int columnIndex = 1;
               for (int j = 0; j < dt.Columns.Count; j++)
               {
                   if (dt.Rows[i][j].ToString() == "")
                   {
                       excel.Cells[row, columnIndex] = "合计";
                   }
                   else
                   {
                       excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                   }
                   columnIndex++;
                   vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
               }
               row++;
           }
           //导出后的数据另存
           vProgress.Close();
           this.Hide();
           Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
           fd.InitialFileName = m_ExportName + fileName + "统计表";
           int result = fd.Show();
           if (result == 0)
           {
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               return;
           }

           string fileName0 = fd.InitialFileName;
           if (!string.IsNullOrEmpty(fileName0))
           {
               if (fileName0.IndexOf(".xls") == -1)
               {
                   fileName0 += ".xls";
               }
               //ygc 2012-9-27 修改提示
               if (File.Exists(fileName0))
               {
                   DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                   if (rs != DialogResult.OK)
                   {
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                       Kill(excel);
                       GC.Collect();
                       return;
                   }
               }//end
               //保存生成的统计表
               excel.DisplayAlerts = false;
               //保存生成的统计表
               try
               {
                   book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               }
               catch (Exception err)
               {
                   MessageBox.Show("导出数据出错:" + err.Message,"错误提示");
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                   Kill(excel);
                   GC.Collect();
                   this.Close();
                   return;
               }
           }
           //资源回收
           System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
           System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
           Kill(excel);
           GC.Collect();
           DialogResult diaResult;
           diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
           if (diaResult == DialogResult.OK)
           {
               Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
               Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
               excel1.Visible = true;
           }
           this.Close();
       }
       #endregion 
       private int GetCountUnique(string ColumnName,string TableName,IFeatureClass pFeatureClass)
       {
           int count = 0;
           if (pFeatureClass == null) return 0;
           IWorkspace pWorkspace = null;
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
           }
           catch (Exception ex)
           { }
           DropTable(pWorkspace, "tempTable");
           m_listUnit.Clear();
           try
           {
               string SQL = "create table tempTable as select distinct " + ColumnName + " as 汇总 from " + TableName + " order by " + ColumnName;
               pWorkspace.ExecuteSQL(SQL);
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tempTable");
               DataTable dt = ITableToDataTable(pTable);
               if (dt != null)
               {
                   for (int i = 0; i < dt.Rows.Count; i++)
                   {
                       for (int j = 0; j < dt.Columns.Count; j++)
                       {
                           m_listUnit.Add(dt.Rows[i][j].ToString ());
                       }
                   }
                   count = m_listUnit.Count;
               }
           }
           catch (Exception ex)
           {
               count = 0;
           }
           DropTable(pWorkspace, "tempTable");
           return count;
       }
       private void cbProvince_SelectedIndexChanged(object sender, EventArgs e)
       {
           listCity = new List<string>();
           cbCity.Items.Clear();
           cbCity.Text = "";
           listCity.Clear();
           int ProvinceIndex = cbProvince.SelectedIndex;
           m_Province = listProvince[ProvinceIndex];

           IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
           IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
           if (pFW == null) return;
           if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
           ITable pTable = pFW.OpenTable("行政区字典表");

           int ndx = pTable.FindField("NAME"),
           cdx = pTable.FindField("CODE");

           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = "XZJB='" + 2 + "'and substr(code,1,2)= '"+m_Province+"'";

           ICursor pCursor = pTable.Search(pQueryFilter, false);
           if (pCursor == null) return;

           IRow pRow = pCursor.NextRow();

        
           while (pRow != null)
           {
               cbCity.Items.Add(pRow.get_Value(ndx).ToString());
               listCity.Add(pRow.get_Value(cdx).ToString());
               pRow = pCursor.NextRow();
           }
           if (listCity.Count <= 0)
           {
               MessageBox.Show("无市级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
           }
           //cbCity.SelectedIndex = 0; 屏蔽初始化下拉框
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
       }
       private void cbCity_SelectedIndexChanged(object sender, EventArgs e)
       {
           listCounty = new List<string>();
           cbCounty.Items.Clear();
           cbCounty.Text = "";
           //listCounty.Clear();
           int cityIndex = cbCity.SelectedIndex;

               m_City = listCity[cityIndex];


           IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
           IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
           if (pFW == null) return;
           if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
           ITable pTable = pFW.OpenTable("行政区字典表");

           int ndx = pTable.FindField("NAME"),
           cdx = pTable.FindField("CODE");

           IQueryFilter pQueryFilter = new QueryFilterClass();
           pQueryFilter.WhereClause = "XZJB='" + 3 + "' and substr(code,1,4)='" + m_City + "'";

           ICursor pCursor = pTable.Search(pQueryFilter, false);
           if (pCursor == null) return;

           IRow pRow = pCursor.NextRow();


           while (pRow != null)
           {
               cbCounty.Items.Add(pRow.get_Value(ndx).ToString());
               listCounty.Add(pRow.get_Value(cdx).ToString());
               pRow = pCursor.NextRow();
           }
           if (listCounty.Count <= 0)
           {
               MessageBox.Show("无县级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
           }
           //cbCounty.SelectedIndex = 0; 屏蔽初始化县级行政去下拉框 
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
       }
       private void cbCounty_SelectedIndexChanged(object sender, EventArgs e)
       {
           m_County = "";
           int CountyIndex = cbCounty.SelectedIndex;
           m_County = listCounty[CountyIndex];
       }
       private void btnCancle_Click(object sender, EventArgs e)
       {
           this.Close();
           this.Dispose();
       }
        //从数据库中获取统计数据 ygc 2012-10-8
       private DataTable GetStatisticalTable(string TableName, string CountryCode, IFeatureClass pFeatureClass)
       {
           if (pFeatureClass == null) return null;
           if (TableName == "" && CountryCode == "") return null;
           DataTable StatisticsDT = new DataTable();
           IWorkspace pWorkspace = null;
           try
           {
               pWorkspace = pFeatureClass.FeatureDataset.Workspace;
           }
           catch (Exception ex)
           { }
           try
           {
               if (pWorkspace == null) return null;
               DropTable(pWorkspace, "StaTempTable");
               string StrSQL = "";
               if (cbCounty.Text.ToString() != "")
               {
                   StrSQL = "create table StaTempTable as select * from " + TableName + " where substr(统计单位,1,6)=" + CountryCode + " order by 统计单位";
               }
               else if (cbCounty.Text.ToString() == "" && cbCity.Text.ToString() != "")
               {
                   StrSQL = "create table StaTempTable as select * from " + TableName + " where substr(统计单位,1,4)=" + CountryCode + " and (length(统计单位)=4 or length(统计单位)=6) order by 统计单位";
               }
               else if (cbCity.Text.ToString() == "")
               {
                   StrSQL = "create table StaTempTable as select * from " + TableName + " where substr(统计单位,1,2)=" + CountryCode + "  and (length(统计单位)=2 or length(统计单位)=4)  order by 统计单位";
               }
               switch (TableName)
               {
                   case "EIGHTTABLE_GJGYHDJ":
                       StrSQL += ",起源";
                       break;
                   case "EIGHTTABLE_GJGYLGHMJ":
                       StrSQL += ",林种";
                       break;
                   case "EIGHTTABLE_LDJGXZ":
                       StrSQL += ",起源";
                       break;
                   case "EIGHTTABLE_LDXZ":
                       StrSQL += ",权属 ,起源";
                       break;
                   case "ECOSTABLE_GLTDMJ":
                       StrSQL += ",土地使用权,森林类别";
                       break;
                   case "ECOSTABLE_GLSLMJ":
                       StrSQL += ",林木使用权";
                       break;
                   case "ECOSTABLE_LZ":
                       StrSQL += ",林种,亚林种";
                       break;
                   case "ECOSTABLE_QMLMJ":
                       StrSQL += ",起源,树种,乔木林";
                       break ;
                   case "ECOSTABLE_STGYL":
                       StrSQL += ",工程类别,事权等级,保护等级";
                       break;
                   case "ECOSTABLE_YCLMJXJ":
                       StrSQL += ",林木使用权,亚林种";
                       break;
                   case "ECOSTABLE_JJL":
                       StrSQL += ",林木使用权,起源,树种";
                       break;
                   case "ECOSTABLE_GML":
                       StrSQL += ",使用权,起源,优势树种";
                       break;
               }
               pWorkspace.ExecuteSQL(StrSQL);
               ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable("StaTempTable");
               StatisticsDT = ITableToDataTable(pTable);
               if (StatisticsDT == null) return null;

           }
           catch (Exception ex)
           {
               DropTable(pWorkspace, "StaTempTable");
               return null;
           }
           m_count = GetCountUnique("统计单位", "StaTempTable", pFeatureClass);
           DropTable(pWorkspace, "StaTempTable");
           return StatisticsDT;
       }

       private void checkBox1_Click(object sender, EventArgs e)
       {
           if (checkBox1.Checked == false)
           {
               cbCounty.Text = "";
               cbCounty.Enabled = true;
           }
           else
           {
               cbCounty.Text = "";
               cbCounty.Enabled = false ;
           }

       }
       private DataTable GetDataTable(DataTable OdeDt)
       {
           DataTable lc_TableData = new DataTable("结果数据");
           List<int> ListRow = new List<int>();
           if (OdeDt == null) return null;
               // 无数据返回空表
           if (OdeDt.Rows .Count== 0) return null;
               // 给列赋值
              for (int index = 0; index < OdeDt.Columns .Count; index++)
               {
                   DataColumn lc_DataColum = new DataColumn();
                   lc_DataColum.Caption = OdeDt.Columns[index].Caption;
                   lc_DataColum.DataType = OdeDt.Columns[index].DataType;
                   lc_TableData.Columns.Add(lc_DataColum);
               }
              for (int i = 0; i < m_listUnit.Count; i++)
              {
                  //StaticUnit = m_listUnit[i];
                  for (int t = 0; t < 5; t++)
                  {
                      DataRow newRow = lc_TableData.NewRow();
                      switch (t)
                      {
                          case 0:
                              newRow.ItemArray = new object[] { m_listUnit[i], "1特用林" };
                              break;
                          case 1:
                              newRow.ItemArray = new object[] { m_listUnit[i], "2防护林" };
                              break;
                          case 2:
                              newRow.ItemArray = new object[] { m_listUnit[i], "3公益林" };
                              break;
                          case 3:
                              newRow.ItemArray = new object[] { m_listUnit[i], "4其他林地" };
                              break;
                          case 4:
                              newRow.ItemArray = new object[] { m_listUnit[i], ""};
                              break;
                      }
                      lc_TableData.Rows.Add(newRow);
                  }
              }
                  for (int row = 0; row < OdeDt.Rows.Count; row++)
                  {

                      if (OdeDt.Rows[row][1].ToString() == lc_TableData.Rows[row][1].ToString() && OdeDt.Rows[row][0].ToString() == lc_TableData.Rows[row][0].ToString())
                          {
                              lc_TableData.Rows[row].ItemArray = OdeDt.Rows[row].ItemArray;
                          }
                          else
                          {
                              for (int k = 0; k < lc_TableData .Rows .Count; k++)
                              {
                                  if (OdeDt.Rows[row][1].ToString() == lc_TableData.Rows[row + k][1].ToString()&&OdeDt .Rows [row][0].ToString ()==lc_TableData .Rows [row +k][0].ToString ())
                                  {
                                      lc_TableData.Rows[row + k].ItemArray = OdeDt.Rows[row].ItemArray;
                                      break;
                                  }
                              }
                          }
                  
                        }
                  return lc_TableData;
 
       }
       #region 导出生态统计数据
        //导出各类土地面积统计表
        private void ExportGLTDMJ(DataTable dt, string fileName)
       {
           //打开模板EXCEL
           Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
           string fileNameString = filePath + "\\" + fileName + ".xls";
           if (excel == null)
           {
               MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
               return;
           }
           if (dt == null)
           {
               MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               Kill(excel);
               return;
           }
           Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
           Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
           //end
           //合并单元格
           for (int i = 0; i < dt.Rows.Count - 1; i++)
           {
               Microsoft.Office.Interop.Excel.Range r;
               Microsoft.Office.Interop.Excel.Range r1;
               if (dt.Rows[i][0].ToString().Trim() == dt.Rows[i + 1][0].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r = xlsheet.get_Range(xlsheet.Cells[8 + i, 1], xlsheet.Cells[9 + i, 1]);
                   r.Value2 = excel.Cells[8 + i, 1];
                   r.MergeCells = true;
               }
               if (dt.Rows[i][1].ToString().Trim() == dt.Rows[i + 1][1].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r1 = xlsheet.get_Range(xlsheet.Cells[8 + i, 2], xlsheet.Cells[9 + i, 2]);
                   r1.Value2 = excel.Cells[8 + i, 2];
                   r1.MergeCells = true;
               }

           }
            int row = 8;
           //导出数据
           SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
           vProgress.ShowDescription = true;
           vProgress.ShowProgressNumber = true;
           vProgress.TopMost = true;
           vProgress.EnableCancel = true;
           vProgress.EnableUserCancel(true);
           vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
           vProgress.ProgresssValue = 0;
           vProgress.Step = 1;
           vProgress.ShowProgress();
           vProgress.SetProgress("正在统计" + fileName + "数据......");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               int columnIndex = 1;
               for (int j = 0; j < dt.Columns.Count; j++)
               {
                   if (dt.Rows[i][j].ToString() == "")
                   {
                       excel.Cells[row, columnIndex] = "合计";
                   }
                   else
                   {
                       excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                   }
                   columnIndex++;
                   vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
               }
               row++;
           }
           //导出后的数据另存
           vProgress.Close();
           this.Hide();
           Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
           fd.InitialFileName = m_ExportName + fileName + "统计表";
           int result = fd.Show();
           if (result == 0)
           {
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               return;
           }

           string fileName0 = fd.InitialFileName;
           if (!string.IsNullOrEmpty(fileName0))
           {
               if (fileName0.IndexOf(".xls") == -1)
               {
                   fileName0 += ".xls";
               }
               //ygc 2012-9-27 修改提示
               if (File.Exists(fileName0))
               {
                   DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                   if (rs != DialogResult.OK)
                   {
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                       Kill(excel);
                       GC.Collect();
                       return;
                   }
               }//end
               //保存生成的统计表
               excel.DisplayAlerts = false;
               //保存生成的统计表
               try
               {
                   book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               }
               catch (Exception err)
               {
                   MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                   Kill(excel);
                   GC.Collect();
                   this.Close();
                   return;
               }
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               DialogResult diaResult;
               diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
               if (diaResult == DialogResult.OK)
               {
                   Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                   Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                   excel1.Visible = true;
               }
               this.Close();
           }
       }
        private void ExportQML(DataTable dt,string fileName)
       {
           //打开模板EXCEL
           Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
           string fileNameString = filePath + "\\" + fileName + ".xls";
           if (excel == null)
           {
               MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
               return;
           }
           if (dt == null)
           {
               MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               Kill(excel);
               return;
           }
           Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
           Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
           //end
           //合并单元格
           for (int i = 0; i < dt.Rows.Count - 1; i++)
           {
               Microsoft.Office.Interop.Excel.Range r;
               Microsoft.Office.Interop.Excel.Range r1;
               Microsoft.Office.Interop.Excel.Range r2;
               if (dt.Rows[i][0].ToString().Trim() == dt.Rows[i + 1][0].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r = xlsheet.get_Range(xlsheet.Cells[6 + i, 1], xlsheet.Cells[7 + i, 1]);
                   r.Value2 = excel.Cells[6 + i, 1];
                   r.MergeCells = true;
               }
               if (dt.Rows[i][1].ToString().Trim() == dt.Rows[i + 1][1].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r1 = xlsheet.get_Range(xlsheet.Cells[6 + i, 2], xlsheet.Cells[7 + i, 2]);
                   r1.Value2 = excel.Cells[6 + i, 2];
                   r1.MergeCells = true;
               }
               if (dt.Rows[i][2].ToString().Trim() == dt.Rows[i + 1][2].ToString().Trim())
               {
                   //excel.Cells[8 + i, 1] = " ";
                   r2 = xlsheet.get_Range(xlsheet.Cells[6 + i, 3], xlsheet.Cells[7 + i, 3]);
                   r2.Value2 = excel.Cells[6 + i, 3];
                   r2.MergeCells = true;
               }
           }
           int row = 6;
           //导出数据
           SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
           vProgress.ShowDescription = true;
           vProgress.ShowProgressNumber = true;
           vProgress.TopMost = true;
           vProgress.EnableCancel = true;
           vProgress.EnableUserCancel(true);
           vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
           vProgress.ProgresssValue = 0;
           vProgress.Step = 1;
           vProgress.ShowProgress();
           vProgress.SetProgress("正在统计" + fileName + "数据......");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               int columnIndex = 1;
               for (int j = 0; j < dt.Columns.Count; j++)
               {
                   if (dt.Rows[i][j].ToString() == "")
                   {
                       excel.Cells[row, columnIndex] = "合计";
                   }
                   else
                   {
                       excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                   }
                   columnIndex++;
                   vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
               }
               row++;
           }
           //导出后的数据另存
           vProgress.Close();
           this.Hide();
           Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
           fd.InitialFileName = m_ExportName + fileName + "统计表";
           int result = fd.Show();
           if (result == 0)
           {
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               return;
           }

           string fileName0 = fd.InitialFileName;
           if (!string.IsNullOrEmpty(fileName0))
           {
               if (fileName0.IndexOf(".xls") == -1)
               {
                   fileName0 += ".xls";
               }
               //ygc 2012-9-27 修改提示
               if (File.Exists(fileName0))
               {
                   DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                   if (rs != DialogResult.OK)
                   {
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                       System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                       Kill(excel);
                       GC.Collect();
                       return;
                   }
               }//end
               //保存生成的统计表
               excel.DisplayAlerts = false;
               //保存生成的统计表
               try
               {
                   book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
               }
               catch (Exception err)
               {
                   MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                   Kill(excel);
                   GC.Collect();
                   this.Close();
                   return;
               }
               //资源回收
               System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
               Kill(excel);
               GC.Collect();
               DialogResult diaResult;
               diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
               if (diaResult == DialogResult.OK)
               {
                   Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                   Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                   excel1.Visible = true;
               }
               this.Close();
           }  
       }
        private void ExportSTGYL(DataTable dt, string fileName)
        {
            //打开模板EXCEL
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            string fileNameString = filePath + "\\" + fileName + ".xls";
            if (excel == null)
            {
                MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                return;
            }
            if (dt == null)
            {
                MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kill(excel);
                return;
            }
            Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
            Microsoft.Office.Interop.Excel.Worksheet xlsheet = book.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            //end
            //合并单元格
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                Microsoft.Office.Interop.Excel.Range r;
                Microsoft.Office.Interop.Excel.Range r1;
                Microsoft.Office.Interop.Excel.Range r2;
                if (dt.Rows[i][0].ToString().Trim() == dt.Rows[i + 1][0].ToString().Trim())
                {
                    //excel.Cells[8 + i, 1] = " ";
                    r = xlsheet.get_Range(xlsheet.Cells[7 + i, 1], xlsheet.Cells[8 + i, 1]);
                    r.Value2 = excel.Cells[7 + i, 1];
                    r.MergeCells = true;
                }
                if (dt.Rows[i][1].ToString().Trim() == dt.Rows[i + 1][1].ToString().Trim())
                {
                    //excel.Cells[8 + i, 1] = " ";
                    r1 = xlsheet.get_Range(xlsheet.Cells[7 + i, 2], xlsheet.Cells[8 + i, 2]);
                    r1.Value2 = excel.Cells[7+ i, 2];
                    r1.MergeCells = true;
                }
                if (dt.Rows[i][2].ToString().Trim() == dt.Rows[i + 1][2].ToString().Trim())
                {
                    //excel.Cells[8 + i, 1] = " ";
                    r2 = xlsheet.get_Range(xlsheet.Cells[7 + i, 3], xlsheet.Cells[8 + i, 3]);
                    //r2.Value2 = excel.Cells[7 + i, 3];
                    r2.MergeCells = true;
                }
            }
            int row = 7;
            //导出数据
            SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-8
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);
            vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            vProgress.SetProgress("正在统计" + fileName + "数据......");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int columnIndex = 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString() == "")
                    {
                        excel.Cells[row, columnIndex] = "合计";
                    }
                    else
                    {
                        excel.Cells[row, columnIndex] = dt.Rows[i][j].ToString();
                    }
                    columnIndex++;
                    vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                }
                row++;
            }
            //导出后的数据另存
            vProgress.Close();
            this.Hide();
            Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
            fd.InitialFileName = m_ExportName + fileName + "统计表";
            int result = fd.Show();
            if (result == 0)
            {
                //资源回收
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                Kill(excel);
                GC.Collect();
                return;
            }

            string fileName0 = fd.InitialFileName;
            if (!string.IsNullOrEmpty(fileName0))
            {
                if (fileName0.IndexOf(".xls") == -1)
                {
                    fileName0 += ".xls";
                }
                //ygc 2012-9-27 修改提示
                if (File.Exists(fileName0))
                {
                    DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                        Kill(excel);
                        GC.Collect();
                        return;
                    }
                }//end
                //保存生成的统计表
                excel.DisplayAlerts = false;
                //保存生成的统计表
                try
                {
                    book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (Exception err)
                {
                    MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    Kill(excel);
                    GC.Collect();
                    this.Close();
                    return;
                }
                //资源回收
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                Kill(excel);
                GC.Collect();
                DialogResult diaResult;
                diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (diaResult == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                    excel1.Visible = true;
                }
                this.Close();
            }  
        }
       #endregion
        //导出信息统计表
        private void ExportXXTable(DataTable dt, string fileName)
        {
            //打开模板EXCEL
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            string fileNameString = filePath + "\\" + fileName + ".xls";
            if (excel == null)
            {
                MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                return;
            }
            if (dt == null)
            {
                MessageBox.Show("无统计数据导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Kill(excel);
                return;
            }
            Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(fileNameString);
            //end
            int rowIndex = 0;
            switch (fileName)
            {
                case "林种信息":
                    rowIndex =6;
                    break;
                case "灾害类型":
                    rowIndex = 6;
                    break;
                case "林地利用方向规划面积":
                    rowIndex = 9;
                    break;
                case "林地及森林面积规划":
                    rowIndex = 7;
                    break;
                default:
                    rowIndex = 5;
                    break;
            }
            //添加进度条 ygc 2012-10-8
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);
            vProgress.MaxValue = dt.Rows.Count * dt.Columns.Count;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            vProgress.SetProgress("正在统计" + fileName + "数据......");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int columnIndex = 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    excel.Cells[rowIndex, columnIndex] = dt.Rows[i][j].ToString();
                    columnIndex++;
                    vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                }
                rowIndex++;
            }
            vProgress.Close();

            //导出后的数据另存
            this.Hide();      //added by chulili 20120828 本对话框先隐藏，避免选择路径对话框获取不到焦点
            Microsoft.Office.Core.FileDialog fd = book.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
            fd.InitialFileName = m_ExportName + fileName + "统计表";
            int result = fd.Show();
            if (result == 0)
            {
                //资源回收
                System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                Kill(excel);
                GC.Collect();
                return;
            }
            string fileName0 = fd.InitialFileName;
            if (!string.IsNullOrEmpty(fileName0))
            {
                if (fileName0.IndexOf(".xls") == -1)
                {
                    fileName0 += ".xls";
                }
                //ygc 2012-9-27 修改提示
                if (File.Exists(fileName0))
                {
                    DialogResult rs = MessageBox.Show(fileName0.ToString() + "已存在！是否替换该文件?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                        Kill(excel);
                        GC.Collect();
                        return;
                    }
                }//end
                //保存生成的统计表
                excel.DisplayAlerts = false;
                try
                {
                    book.SaveAs(fileName0, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (Exception err)
                {
                    MessageBox.Show("导出数据出错:" + err.Message, "错误提示");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    Kill(excel);
                    GC.Collect();
                    this.Close();
                    return;
                }
            }
            //资源回收
            System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            Kill(excel);
            GC.Collect();
            DialogResult diaResult;
            diaResult = MessageBox.Show("导出数据成功！是否查看？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (diaResult == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook book1 = excel1.Application.Workbooks.Add(fileName0);
                excel1.Visible = true;
            }
            this.Close();
        }
    }
}
