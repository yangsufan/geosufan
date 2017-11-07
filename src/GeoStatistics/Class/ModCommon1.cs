using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar.Controls;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoStatistics
{
    public class ModCommon
    {

        public static void DoEightStatistics(IFeatureClass pFeatureClass,string strMjFieldName)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress();       //添加进度条 ygc 2012-10-24
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            try
            {
                vProgress.SetProgress("正在统计林地质量等级数据......");
                //林地质量等级
                DoLDZL_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计林地利用方向规划面积数据......");
                //林地利用方向规划面积
                DoLDLYFXGHMJ_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计林地结构现状数据......");
                //林地结构现状
                DoLDJGXZ_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计林地及森林面积规划数据......");
                //林地及森林面积规划
                DoLDSLMJGH_Statistic(pFeatureClass, strMjFieldName);
                //林地现状
                vProgress.SetProgress("正在统计国家级公益林地分保护等级现状数据......");
                DoLDXZ_Statistic(pFeatureClass, strMjFieldName);
                //国家级公益林地分保护等级现状
                DoGJGYHDJ_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计林地保护等级数据......");
                //林地保护等级
                DoLDBHDJ_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计国家级公益林地规划面积数据......");
                //国家级公益林地规划面积
                DoGJGYLGHMJ_Statistic(pFeatureClass, strMjFieldName);
                vProgress.SetProgress("正在统计林场数据......");
                //林场八类统计表生成 ygc 2012-10-10
                DoLCLDZL_Statistic(pFeatureClass, strMjFieldName);
                DoLCLDLYFXGHMJ_Statistic(pFeatureClass, strMjFieldName);
                DoLCLDJGXZ_Statistic(pFeatureClass, strMjFieldName);
                DoLCLDSLMJGH_Statistic(pFeatureClass, strMjFieldName);
                DoLCLDXZ_Statistic(pFeatureClass, strMjFieldName);
                DoLCGJGYHDJ_Statistic(pFeatureClass, strMjFieldName);
                DoLCLDBHDJ_Statistic(pFeatureClass, strMjFieldName);
                DoLCGJGYLGHMJ_Statistic(pFeatureClass, strMjFieldName);
            }
            catch
            { }
            finally
            {
                vProgress.Close();
            }
 
        }
        //删除临时表
        //ygc 2012-8-21
        private static void DropTable(IWorkspace pWks, string strTableName)
        {
            try
            {
                pWks.ExecuteSQL("drop table " + strTableName);

            }
            catch
            { }
        }
        //林地质量等级
        private static bool DoLDZL_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            DropTable(pWorkspace, "EightTable_LDZL");
            try
            {
                //查询县级统计数据
                //ygc 2012-8-21
                string stringSQL = "create table EightTable_LDZL as select " + tableName + ".xian as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + strMjFieldName + " end) as 合计 ," +
                                    "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + strMjFieldName + " end) as Ⅰ级," +
                                     "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + strMjFieldName + " end) as Ⅱ级," +
                                     "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + strMjFieldName + " end) as Ⅲ级," +
                                     "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + strMjFieldName + " end) as Ⅳ级," +
                                     "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + strMjFieldName + " end) as Ⅴ级 " +
                                      "  from " + tableName +
                                      "  group by xian";
                pWorkspace.ExecuteSQL(stringSQL);
                //查询乡镇级统计数据
                string townsSQL = "insert into EightTable_LDZL select  " + tableName + ".xiang as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + strMjFieldName + " end) as 合计 ," +
                                  "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + strMjFieldName + "  end) as Ⅰ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + strMjFieldName + "  end) as Ⅱ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + strMjFieldName + "  end) as Ⅲ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + strMjFieldName + "  end) as Ⅳ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + strMjFieldName + "  end) as Ⅴ级 " +
                                  "  from " + tableName +
                                  "  group by xiang";
                pWorkspace.ExecuteSQL(townsSQL);
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_LDZL select  substr(" + tableName + ".shi,1,4) as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + strMjFieldName + " end) as 合计 ," +
                  "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + strMjFieldName + "  end) as Ⅰ级," +
                  "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + strMjFieldName + "  end) as Ⅱ级," +
                  "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + strMjFieldName + "  end) as Ⅲ级," +
                  "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + strMjFieldName + "  end) as Ⅳ级," +
                  "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + strMjFieldName + "  end) as Ⅴ级 " +
                  "  from " + tableName +
                  "  group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private static bool DoLCLDZL_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            try
            {
                if (!ExistTable(pWorkspace, "EightTable_LDZL"))
                {
                    string stringSQL = "create table EightTable_LDZL as select " + tableName + ".lc as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + strMjFieldName + " end) as 合计 ," +
                                       "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + strMjFieldName + " end) as Ⅰ级," +
                                        "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + strMjFieldName + " end) as Ⅱ级," +
                                        "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + strMjFieldName + " end) as Ⅲ级," +
                                        "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + strMjFieldName + " end) as Ⅳ级," +
                                        "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + strMjFieldName + " end) as Ⅴ级 " +
                                         "  from " + tableName +
                                         "  where lc <>' '"+
                                         "  group by lc";
                    pWorkspace.ExecuteSQL(stringSQL);
                }
                //查询林场统计数据
                string townsSQL = "insert into EightTable_LDZL select  " + tableName + ".lc as 统计单位,sum(case when Zl_DJ between '1' and '5' then " + tableName + "." + strMjFieldName + " end) as 合计 ," +
                                  "sum(case when " + tableName + ".Zl_DJ='1' then " + tableName + "." + strMjFieldName + "  end) as Ⅰ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='2' then " + tableName + "." + strMjFieldName + "  end) as Ⅱ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='3' then " + tableName + "." + strMjFieldName + "  end) as Ⅲ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='4' then " + tableName + "." + strMjFieldName + "  end) as Ⅳ级," +
                                  "sum(case when " + tableName + ".Zl_DJ='5' then " + tableName + "." + strMjFieldName + "  end) as Ⅴ级 " +
                                  "  from " + tableName +
                                  "  where lc <> ' '"+
                                  "  group by lc";
                pWorkspace.ExecuteSQL(townsSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //林地利用方向规划面积
        private static bool DoLDLYFXGHMJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            DropTable(pWorkspace, "EightTable_LDLYFXGHMJ");
            try
            {
                #region 执行统计
                //查询县级统计数据SQL语句
                //ygc 2012-8-22
                string CitySQL = "create table EightTable_LDLYFXGHMJ as select " +
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
                                 " group by xian";
                pWorkspace.ExecuteSQL(CitySQL);
                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "insert  into EightTable_LDLYFXGHMJ  select " +
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
                                 " group by xiang";
                pWorkspace.ExecuteSQL(townsSQL);
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert  into EightTable_LDLYFXGHMJ  select substr(" +
                 tableName + ".shi,1,4) as 统计单位," +
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
                 " group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
                #endregion
                #region 更新统计
                //更新公益林小计
                //ygc 2012-8-22
                string UpdateXJ = "update EightTable_LDLYFXGHMJ set 重点公益小计=重点公益防护林+ 重点公益特用林";
                pWorkspace.ExecuteSQL(UpdateXJ);
                //更新重点公益林合计 ygc 2012-8-22
                string UpdateZDFYHJ = "update EightTable_LDLYFXGHMJ set 重点公益林合计=重点公益小计+重点公益其他";
                pWorkspace.ExecuteSQL(UpdateZDFYHJ);
                //更新一般公益小计 ygc 2012-8-22
                string UpdateYBGYXJ = "update EightTable_LDLYFXGHMJ set 一般公益小计=一般公益防护林+一般公益特用林";
                pWorkspace.ExecuteSQL(UpdateYBGYXJ);
                //更新一般公益合计 ygc 2012-8-22
                string UpdateYBGYHJ = "update EightTable_LDLYFXGHMJ set 一般公益合计=一般公益小计+一般公益其他";
                pWorkspace.ExecuteSQL(UpdateYBGYHJ);
                //更新公益林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 公益林合计=一般公益合计 + 重点公益林合计");
                //更新重点商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
                //更新重点商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 重点商品林合计=重点商品林小计+重点其他");
                //更新一般商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
                //更新一般商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 一般商品林合计=一般商品林小计+一般其他");
                //更新商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 商品林合计 =一般商品林合计+重点商品林合计");
                //更新总计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 合计=商品林合计+公益林合计");
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private static bool DoLCLDLYFXGHMJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            try
            {
                #region 执行统计
                if (!ExistTable(pWorkspace, "EightTable_LDLYFXGHMJ"))
                {
                    //查询县级统计数据SQL语句
                    //ygc 2012-8-22
                    string CitySQL = "create table EightTable_LDLYFXGHMJ as select " +
                                     tableName + ".lc as 统计单位," +
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
                                     " where lc <>' '"+
                                     " group by lc";
                    pWorkspace.ExecuteSQL(CitySQL);
                }

                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "insert  into EightTable_LDLYFXGHMJ  select " +
                                 tableName + ".lc as 统计单位," +
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
                                 "  where lc <> ' '"+
                                 " group by lc";
                pWorkspace.ExecuteSQL(townsSQL);
                #endregion
                #region 更新统计
                //更新公益林小计
                //ygc 2012-8-22
                string UpdateXJ = "update EightTable_LDLYFXGHMJ set 重点公益小计=重点公益防护林+ 重点公益特用林";
                pWorkspace.ExecuteSQL(UpdateXJ);
                //更新重点公益林合计 ygc 2012-8-22
                string UpdateZDFYHJ = "update EightTable_LDLYFXGHMJ set 重点公益林合计=重点公益小计+重点公益其他";
                pWorkspace.ExecuteSQL(UpdateZDFYHJ);
                //更新一般公益小计 ygc 2012-8-22
                string UpdateYBGYXJ = "update EightTable_LDLYFXGHMJ set 一般公益小计=一般公益防护林+一般公益特用林";
                pWorkspace.ExecuteSQL(UpdateYBGYXJ);
                //更新一般公益合计 ygc 2012-8-22
                string UpdateYBGYHJ = "update EightTable_LDLYFXGHMJ set 一般公益合计=一般公益小计+一般公益其他";
                pWorkspace.ExecuteSQL(UpdateYBGYHJ);
                //更新公益林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 公益林合计=一般公益合计 + 重点公益林合计");
                //更新重点商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
                //更新重点商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 重点商品林合计=重点商品林小计+重点其他");
                //更新一般商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
                //更新一般商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 一般商品林合计=一般商品林小计+一般其他");
                //更新商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 商品林合计 =一般商品林合计+重点商品林合计");
                //更新总计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDLYFXGHMJ set 合计=商品林合计+公益林合计");
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //林地现状
        //通过SQL语句获得林地现状统计数据 ygc 2012-8-24
        private static bool DoLDXZ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            DropTable(pWorkspace, "EightTable_LDXZ");
            //创建临时表
            try
            {
                pWorkspace.ExecuteSQL("create table tempTable as select * from " + tableName);
                //更新临时表中的权属信息
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='国有' where LD_QS in('10','15')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='集体' where LD_QS in('21','22','23','16')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='其他' where LD_QS in('20')");
                //统计县级数据
                string CitySQL = "create table EightTable_LDXZ as select " +
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
                                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                                 "  group by xian,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(CitySQL);
                //统计乡镇级数据
                string townsSQL = "insert into EightTable_LDXZ  select " +
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
                                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                                 "  group by xiang,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(townsSQL);
                //获得市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_LDXZ  select " +
                 "substr(shi,1,4) as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
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
                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                 "  group by substr(shi,1,4),rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(SHISQL);
                //更新森林覆盖率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 森林覆盖率=(有林地小计+特灌)/土地总面积 * 100 where 权属 is null");
                //更新林木绿化率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 林木绿化率=(有林地小计+灌木林地小计)/土地总面积 * 100 where 权属 is null ");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true ;
        }
        private static bool DoLCLDXZ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            //创建临时表
            try
            {
                pWorkspace.ExecuteSQL("create table tempTable as select * from " + tableName);
                //更新临时表中的权属信息
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='国有' where LD_QS in('10','15')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='集体' where LD_QS in('21','22','23','16')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='其他' where LD_QS in('20')");
                if (!ExistTable(pWorkspace, "EightTable_LDXZ"))
                {
                    //统计县级数据
                    string CitySQL = "create table EightTable_LDXZ as select " +
                                     "lc as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
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
                                     "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     "  group by lc,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                     "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                    pWorkspace.ExecuteSQL(CitySQL);
                }
                //统计乡镇级数据
                string townsSQL = "insert into EightTable_LDXZ  select " +
                                 "lc as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
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
                                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                 "  group by lc,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(townsSQL);
                //更新森林覆盖率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 森林覆盖率=(有林地小计+特灌)/土地总面积 * 100 where 权属 is null");
                //更新林木绿化率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 林木绿化率=(有林地小计+灌木林地小计)/土地总面积 * 100 where 权属 is null ");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //林地及森林面积规划
        //通过SQL语句获得林地及森林面积规划统数据
        //ygc 2012-8-23
        private static bool DoLDSLMJGH_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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

            DropTable(pWorkspace, "EightTable_LDSLMJGH");
            try
            {
                //查询县级数据SQL语句 ygc 2012-8-23
                string CitySQL = "create table EightTable_LDSLMJGH as select " +
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
                                 "  group by xian";
                pWorkspace.ExecuteSQL(CitySQL);
                //查询乡镇及统计数据SQL ygc 2012-8-23
                string townsSQL = " insert into EightTable_LDSLMJGH  select " +
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
                                 "  group by xiang";
                pWorkspace.ExecuteSQL(townsSQL);
                //获得市级数据 ygc 2012-10-22
                string SHISQL = " insert into EightTable_LDSLMJGH  select " +
                 "substr(shi,1,4) as 统计单位," +
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
                 "  group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
                //更新森林增加合计 ygc 2012-8-23
                pWorkspace.ExecuteSQL("update EightTable_LDSLMJGH set 森林增加合计= 森林增加林地+森林增加未利用地+森林增加建设用地+森林增加草地+森林增加耕地+森林增加其他");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        private static bool DoLCLDSLMJGH_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            try
            {
                if (!ExistTable(pWorkspace, "EightTable_LDSLMJGH"))
                {
                    //查询县级数据SQL语句 ygc 2012-8-23
                    string CitySQL = "create table EightTable_LDSLMJGH as select " +
                                     "lc as 统计单位," +
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
                                     "  where lc <>' '"+
                                     "  group by lc";
                    pWorkspace.ExecuteSQL(CitySQL);
                }

                //查询乡镇及统计数据SQL ygc 2012-8-23
                string townsSQL = " insert into EightTable_LDSLMJGH  select " +
                                 "lc as 统计单位," +
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
                                 "  where lc<> ' '"+
                                 "  group by lc";
                pWorkspace.ExecuteSQL(townsSQL);
                //更新森林增加合计 ygc 2012-8-23
                pWorkspace.ExecuteSQL("update EightTable_LDSLMJGH set 森林增加合计= 森林增加林地+森林增加未利用地+森林增加建设用地+森林增加草地+森林增加耕地+森林增加其他");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        //林地结构现状
        private static bool DoLDJGXZ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            DropTable(pWorkspace, "EightTable_LDJGXZ");
            try
            {
                #region 执行统计
                //查询县级统计数据SQL语句
                //ygc 2012-8-22
                string CitySQL = "create table EightTable_LDJGXZ as select " +
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
                                 " where substr(QI_YUAN,1,1)<>' '" +
                                 " group by xian,rollup(substr(QI_YUAN,1,1)) " +
                                 " order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(CitySQL);
                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "insert  into EightTable_LDJGXZ  select " +
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
                                 " where  substr(QI_YUAN,1,1)<>' '" +
                                 " group by xiang,rollup(substr(QI_YUAN,1,1)) " +
                                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(townsSQL);
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert  into EightTable_LDJGXZ  select substr(" +
                 tableName + ".shi,1,4) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                 " where  substr(QI_YUAN,1,1)<>' '" +
                 " group by substr(shi,1,4),rollup(substr(QI_YUAN,1,1)) " +
                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(SHISQL);
                #endregion
                #region 更新统计
                //更新公益林小计
                //ygc 2012-8-22
                string UpdateXJ = "update EightTable_LDJGXZ set 重点公益小计=重点公益防护林+ 重点公益特用林";
                pWorkspace.ExecuteSQL(UpdateXJ);
                //更新重点公益林合计 ygc 2012-8-22
                string UpdateZDFYHJ = "update EightTable_LDJGXZ set 重点公益林合计=重点公益小计+重点公益其他";
                pWorkspace.ExecuteSQL(UpdateZDFYHJ);
                //更新一般公益小计 ygc 2012-8-22
                string UpdateYBGYXJ = "update EightTable_LDJGXZ set 一般公益小计=一般公益防护林+一般公益特用林";
                pWorkspace.ExecuteSQL(UpdateYBGYXJ);
                //更新一般公益合计 ygc 2012-8-22
                string UpdateYBGYHJ = "update EightTable_LDJGXZ set 一般公益合计=一般公益小计+一般公益其他";
                pWorkspace.ExecuteSQL(UpdateYBGYHJ);
                //更新公益林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 公益林合计=一般公益合计 + 重点公益林合计");
                //更新重点商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
                //更新重点商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 重点商品林合计=重点商品林小计+重点其他");
                //更新一般商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
                //更新一般商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 一般商品林合计=一般商品林小计+一般其他");
                //更新商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 商品林合计 =一般商品林合计+重点商品林合计");
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        private static bool DoLCLDJGXZ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            try
            {
                #region 执行统计
                if (!ExistTable(pWorkspace, "EightTable_LDJGXZ"))
                {                
                    //查询县级统计数据SQL语句
                    //ygc 2012-8-22
                    string CitySQL = "create table EightTable_LDJGXZ as select " +
                                     tableName + ".lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                                     " where substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     " group by lc,rollup(substr(QI_YUAN,1,1)) " +
                                     " order by substr(QI_YUAN,1,1) ";
                    pWorkspace.ExecuteSQL(CitySQL);
 
                }
                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "insert  into EightTable_LDJGXZ  select " +
                                 tableName + ".lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                                 " where  substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                 " group by lc,rollup(substr(QI_YUAN,1,1)) " +
                                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(townsSQL);
                #endregion
                #region 更新统计
                //更新公益林小计
                //ygc 2012-8-22
                string UpdateXJ = "update EightTable_LDJGXZ set 重点公益小计=重点公益防护林+ 重点公益特用林";
                pWorkspace.ExecuteSQL(UpdateXJ);
                //更新重点公益林合计 ygc 2012-8-22
                string UpdateZDFYHJ = "update EightTable_LDJGXZ set 重点公益林合计=重点公益小计+重点公益其他";
                pWorkspace.ExecuteSQL(UpdateZDFYHJ);
                //更新一般公益小计 ygc 2012-8-22
                string UpdateYBGYXJ = "update EightTable_LDJGXZ set 一般公益小计=一般公益防护林+一般公益特用林";
                pWorkspace.ExecuteSQL(UpdateYBGYXJ);
                //更新一般公益合计 ygc 2012-8-22
                string UpdateYBGYHJ = "update EightTable_LDJGXZ set 一般公益合计=一般公益小计+一般公益其他";
                pWorkspace.ExecuteSQL(UpdateYBGYHJ);
                //更新公益林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 公益林合计=一般公益合计 + 重点公益林合计");
                //更新重点商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 重点商品林小计=重点用材林+重点经济林+重点薪碳林");
                //更新重点商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 重点商品林合计=重点商品林小计+重点其他");
                //更新一般商品林小计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 一般商品林小计=一般用材林+一般经济林+一般薪碳林");
                //更新一般商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 一般商品林合计=一般商品林小计+一般其他");
                //更新商品林合计 ygc 2012-8-22
                pWorkspace.ExecuteSQL("update EightTable_LDJGXZ set 商品林合计 =一般商品林合计+重点商品林合计");
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        //国家级公益林地分保护等级现状
        //通过SQL语句获得国家级公益林地分保护等级现状统计表 ygc 2012-8-23
        private static bool DoGJGYHDJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            DropTable(pWorkspace, "EightTable_GJGYHDJ");
            try
            {
                //通过SQL语句获得县级统计数据 ygc 2012-8-23
                string CitySQL = "create table EightTable_GJGYHDJ as select " +
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
                                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                 "  group by xian,rollup(substr(QI_YUAN,1,1))" +
                                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(CitySQL);
                //利用SQL语句查询统计乡镇数据 ygc 2012-8-23
                string townsSQL = "insert into EightTable_GJGYHDJ select " +
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
                                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                 "  group by xiang,rollup(substr(QI_YUAN,1,1))" +
                                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(townsSQL);
                //获取市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_GJGYHDJ select " +
                 "substr(shi,1,4) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                 "  group by substr(shi,1,4),rollup(substr(QI_YUAN,1,1))" +
                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(SHISQL); 
                //更新合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 合计= 防护林合计+特用林合计+其他合计");
                //更新一级合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 一级小计=一级防护+一级特用+一级其他 ");
                //更新二级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 二级小计=二级防护+二级特用+二级其他");
                //更新三级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 三级小计=三级防护+三级特用+三级其他");

            }
            catch (Exception ex)
            {
                return false;
            }
            return true ;
        }
        private static bool DoLCGJGYHDJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        {
            if (pFeatureClass == null)
            {
                return false;
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
            try
            {
                if (!ExistTable(pWorkspace, "EightTable_GJGYHDJ"))
                {
                    //通过SQL语句获得县级统计数据 ygc 2012-8-23
                    string CitySQL = "create table EightTable_GJGYHDJ as select " +
                                     "lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                                     "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1' and lc<>' '" +
                                     "  group by lc,rollup(substr(QI_YUAN,1,1))" +
                                     "  order by substr(QI_YUAN,1,1) ";
                    pWorkspace.ExecuteSQL(CitySQL);
                }

                //利用SQL语句查询统计乡镇数据 ygc 2012-8-23
                string townsSQL = "insert into EightTable_GJGYHDJ select " +
                                 "lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
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
                                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1' and lc<>' '" +
                                 "  group by lc,rollup(substr(QI_YUAN,1,1))" +
                                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(townsSQL);
                //更新合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 合计= 防护林合计+特用林合计+其他合计");
                //更新一级合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 一级小计=一级防护+一级特用+一级其他 ");
                //更新二级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 二级小计=二级防护+二级特用+二级其他");
                //更新三级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 三级小计=三级防护+三级特用+三级其他");

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

       private static bool DoLDBHDJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
       {
           if (pFeatureClass == null)
           {
               return false;
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
           DropTable(pWorkspace, "EightTable_LDBHDJTable");
           try
           {
               //查询县级统计数据SQL语句
               //ygc 2012-8-22
               string CitySQL = "create table EightTable_LDBHDJTable as select  " +
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
                                " group by xian";
               pWorkspace.ExecuteSQL(CitySQL);
               //查询乡级统计数据SQL语句
               //ygc 2012-8-22
               string townsSQL = "insert into EightTable_LDBHDJTable select  " +
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
                                " group by xiang";
               pWorkspace.ExecuteSQL(townsSQL);
               //查询市级数据 ygc 2012-10-22
               string SHISQL = "insert into EightTable_LDBHDJTable select  substr(" +
                  tableName + ".shi,1,4) as 统计单位," +
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
                 " group by substr(shi,1,4)";
               pWorkspace.ExecuteSQL(SHISQL);
           }
           catch (Exception ex)
           {
               return false;
           }
           return true ;
       }
       private static bool DoLCLDBHDJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
       {
           if (pFeatureClass == null)
           {
               return false;
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
           try
           {
               if (!ExistTable(pWorkspace, "EightTable_LDBHDJTable"))
               {
                   //查询县级统计数据SQL语句
                   //ygc 2012-8-22
                   string CitySQL = "create table EightTable_LDBHDJTable as select  " +
                                     tableName + ".lc as 统计单位," +
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
                                    "  where lc <>' '"+
                                    " group by lc";
                   pWorkspace.ExecuteSQL(CitySQL);
               }

               //查询乡级统计数据SQL语句
               //ygc 2012-8-22
               string townsSQL = "insert into EightTable_LDBHDJTable select  " +
                                 tableName + ".lc as 统计单位," +
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
                                " where lc<>' '" +
                                " group by lc";
               pWorkspace.ExecuteSQL(townsSQL);
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }
       //通过SQL 语句获得国家级公益林地规划面积统计表 ygc 2012-8-27
       private static bool DoGJGYLGHMJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
       {

           if (pFeatureClass == null)
           {
               return false;
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
           DropTable(pWorkspace, "EightTable_GJGYLGHMJ");
           DropTable(pWorkspace, "temp_table_1");
           DropTable(pWorkspace, "temp_table_2");
           try
           {
               //复制临时表 ygc 2012-8-27
               pWorkspace.ExecuteSQL("create table temp_table_1 as select * from  " + tableName);
               pWorkspace.ExecuteSQL("create table temp_table_2 as select * from  " + tableName);
               //更新临时表 ygc 2012-8-27 
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='2防护林' where substr(lz,1,2)='11'");
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='1特用林' where substr(lz,1,2)='12'");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='4其他林地' where substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12' and substr(lz,1,2)<>' '");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='3公益林' where (substr(lz,1,2)='11' or substr(lz,1,2)='12')");
               //通过SQL获得县级统计数据 ygc 2012-9-27 
               string CitySQL = " create table EightTable_GJGYLGHMJ as (select " +
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
                                " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
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
                                " where  ( lz<>' 'and ghlz<>' ')" +
                                " group by xian,rollup(lz))";
               pWorkspace.ExecuteSQL(CitySQL);
               //用SQL语句获取乡级行政区统计数据
               string townsSQL = " insert  into EightTable_GJGYLGHMJ  (select " +
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
                                " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
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
                                " where  ( lz<>' 'and ghlz<>' ')" +
                                " group by xiang,rollup(lz))";
               pWorkspace.ExecuteSQL(townsSQL);
               //获取市级数据a
               string SHISQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                 "substr(temp_table_1.shi,1,4) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                 " from temp_table_1" +
                 " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
                 " group by substr(shi,1,4),lz" +
                 " union all" +
                 " select " +
                 " substr(shi,1,4) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                 " from temp_table_2" +
                 " where  ( lz<>' 'and ghlz<>' ')" +
                 " group by substr(shi,1,4),rollup(lz))";
               pWorkspace.ExecuteSQL(SHISQL);
               DropTable(pWorkspace, "temp_table_1");
               DropTable(pWorkspace, "temp_table_2");
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }
       private static bool DoLCGJGYLGHMJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
       {

           if (pFeatureClass == null)
           {
               return false;
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
           DropTable(pWorkspace, "temp_table_1");
           DropTable(pWorkspace, "temp_table_2");
           try
           {
               //复制临时表 ygc 2012-8-27
               pWorkspace.ExecuteSQL("create table temp_table_1 as select * from  " + tableName);
               pWorkspace.ExecuteSQL("create table temp_table_2 as select * from  " + tableName);
               //更新临时表 ygc 2012-8-27 
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='2防护林' where substr(lz,1,2)='11'");
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='1特用林' where substr(lz,1,2)='12'");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='4其他林地' where substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12' and substr(lz,1,2)<>' '");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='3公益林' where (substr(lz,1,2)='11' or substr(lz,1,2)='12')");
               if (!ExistTable(pWorkspace, "EightTable_GJGYLGHMJ"))
               {
                   string CitySQL = " create table EightTable_GJGYLGHMJ as (select " +
                                   "temp_table_1.lc as 统计单位,lz as 林种," +
                                   "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                   "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                   "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                   "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                   "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                   "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                   "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                   "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                   " from temp_table_1" +
                                   " where  ( lz<>' 'or ghlz<>' ') and (lz='1特用林' or lz='2防护林') and lc<>' '" +
                                   " group by lc,lz" +
                                   " union all" +
                                   " select " +
                                   " lc as 统计单位,lz as 林种," +
                                   "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                   "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                   "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                   "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                   "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                   "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                   "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                   "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                   " from temp_table_2" +
                                   " where  ( lz<>' 'or ghlz<>' ') and lc<>' '" +
                                   " group by lc,rollup(lz))";
                   pWorkspace.ExecuteSQL(CitySQL);
               }

               //用SQL语句获取乡级行政区统计数据
               string townsSQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                                "temp_table_1.lc as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_1" +
                                " where  ( lz<>' 'or ghlz<>' ') and (lz='1特用林' or lz='2防护林') and lc<>' '" +
                                " group by lc,lz" +
                                " union all" +
                                " select " +
                                " lc as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then " + StatisticsFieldName + " else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then " + StatisticsFieldName + " else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then " + StatisticsFieldName + " else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then " + StatisticsFieldName + " else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then " + StatisticsFieldName + " else 0 end) as 规划三级" +
                                " from temp_table_2" +
                                " where  ( lz<>' 'or ghlz<>' ') and lc<>' '" +
                                " group by lc,rollup(lz))";
               pWorkspace.ExecuteSQL(townsSQL);

               DropTable(pWorkspace, "temp_table_1");
               DropTable(pWorkspace, "temp_table_2");
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }
        /// <summary>
        /// 将DataGridView控件中数据导出到Excel
        /// </summary>
        /// <param name="gridView">DataGridView对象</param>
        /// <returns></returns>
        public static bool ExportDataGridview(DataGridViewX gridView,string defaultName)
        {
            Application excel = null;
            Workbook wb = null;
            try
            {
                if (gridView.Rows.Count == 0)
                    return false;
                //建立Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = true;
                wb.Application.ActiveWindow.Caption=defaultName;
                
                //生成字段名称
                for (int i = 0; i < gridView.ColumnCount; i++)
                {
                    excel.Cells[1, i + 1] = gridView.Columns[i].HeaderText;
                }
                //填充数据
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    for (int j = 0; j < gridView.ColumnCount; j++)
                    {
                        if (gridView[j, i].ValueType == typeof(string))
                        {
                            if (gridView[j, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = "'" + gridView[j, i].Value.ToString();
                            }
                        }
                        else
                        {
                            if (gridView[j, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = gridView[j, i].Value.ToString();
                            }
                        }
                    }
                }
                FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                fd.InitialFileName = defaultName;
                int result=fd.Show();
                if (result == 0) return true;
                string fileName=fd.InitialFileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.IndexOf(".xls")==-1)
                    {
                        fileName += ".xls";
                    }
                    wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将DataGridView控件中数据导出到Excel
        /// </summary>
        /// <param name="gridView">DataGridView对象</param>
        /// <returns></returns>
        public static bool ExportDataGridview(DataGridViewX gridView,List<string> lstFields, string defaultName)
        {
            Application excel = null;
            Workbook wb = null;
            try
            {
                if (gridView.Rows.Count == 0)
                    return false;
                //建立Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = true;
                wb.Application.ActiveWindow.Caption = defaultName;

                //生成字段名称
                //for (int i = 0; i < gridView.ColumnCount; i++)
                //{
                //    if(!lstFields.Contains(gridView.Columns[i].HeaderText)) continue;
                //    excel.Cells[1, i + 1] = gridView.Columns[i].HeaderText;
                //}

                for (int i = 0; i < lstFields.Count; i++)
                {
                    //if (!lstFields.Contains(gridView.Columns[i].HeaderText)) continue;
                    excel.Cells[1, i + 1] = gridView.Columns[lstFields[i]].HeaderText;
                }

                //填充数据
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    for (int j = 0; j < lstFields.Count; j++)
                    {
                        //if (!lstFields.Contains(gridView.Columns[j].HeaderText)) continue;
                        int intFieldIndex=gridView.Columns.IndexOf(gridView.Columns[lstFields[j]]);

                        if (gridView[intFieldIndex, i].ValueType == typeof(string))
                        {
                            if (gridView[intFieldIndex, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = "'" + gridView[intFieldIndex, i].Value.ToString();
                            }
                        }
                        else
                        {
                            if (gridView[intFieldIndex, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = gridView[intFieldIndex, i].Value.ToString();
                            }
                        }
                    }
                }
                FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                fd.InitialFileName = defaultName;
                int result = fd.Show();
                if (result == 0) return true;
                string fileName = fd.InitialFileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.IndexOf(".xls") == -1)
                    {
                        fileName += ".xls";
                    }
                    wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //判断表是否存在 ygc 2012-10-10 
        private static bool ExistTable(IWorkspace pWorkspace, string TableName)
        {
            try
            {
                ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable(TableName);
                if (pTable != null) return true;
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
