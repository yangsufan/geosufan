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

        public static void DoEightStatistics(IFeatureClass pFeatureClass,string strMjFieldName,SysCommon.CProgress pProgress)
        {
            pProgress.SetProgress(1, "进行林地质量等级统计");
            bool bRes = false;
            //林地质量等级
            bRes = DoLDZL_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDZL_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(2, "进行林地利用方向规划面积统计");
            //林地利用方向规划面积
            bRes=DoLDLYFXGHMJ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDLYFXGHMJ_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(3, "进行林地结构现状统计");
            //林地结构现状
            bRes=DoLDJGXZ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDJGXZ_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(4, "进行林地及森林面积规划统计");
            //林地及森林面积规划
            bRes=DoLDSLMJGH_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDSLMJGH_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(5, "进行林地现状统计");
            //林地现状
            bRes=DoLDXZ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDXZ_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(6, "进行国家级公益林地分保护等级现状统计");
            //国家级公益林地分保护等级现状
            bRes=DoGJGYHDJ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCGJGYHDJ_Statistic(pFeatureClass, strMjFieldName);//林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(7, "进行林地保护等级统计");
            //林地保护等级
            bRes=DoLDBHDJ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCLDBHDJ_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10

            pProgress.SetProgress(8, "进行国家级公益林地规划面积统计");
            //国家级公益林地规划面积
            bRes=DoGJGYLGHMJ_Statistic(pFeatureClass, strMjFieldName);
            bRes=DoLCGJGYLGHMJ_Statistic(pFeatureClass, strMjFieldName); //林场八类统计表生成 ygc 2012-10-10
            pProgress.SetProgress(9,"统计完毕");
        }
        //删除临时表;
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
                string townsSQL = "create table EightTable_LDZL as select  substr(" + tableName + ".xiang,1,8) as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                                  "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 一级," +
                                  "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 二级," +
                                  "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 三级," +
                                  "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 四级," +
                                  "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 五级 " +
                                  "  from " + tableName +
                                  "  group by substr(xiang,1,8)";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_LDZL modify 统计单位 nvarchar2(20)");
                //查询县级统计数据
                //ygc 2012-8-21
                string stringSQL = "insert into EightTable_LDZL select " + tableName + ".xian as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                                    "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 一级," +
                                     "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 二级," +
                                     "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 三级," +
                                     "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 四级," +
                                     "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 五级 " +
                                      "  from " + tableName +
                                      "  group by xian";
                pWorkspace.ExecuteSQL(stringSQL);
                //查询乡镇级统计数据
                
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_LDZL select  substr(" + tableName + ".shi,1,4) as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                  "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 一级," +
                  "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 二级," +
                  "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 三级," +
                  "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 四级," +
                  "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 五级 " +
                  "  from " + tableName +
                  "  group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
                //查询省级数据 ygc 2012-10-24
                string SHENGSQL = "insert into EightTable_LDZL select  substr(" + tableName + ".sheng,1,2) as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                  "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 一级," +
                  "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 二级," +
                  "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 三级," +
                  "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 四级," +
                  "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 五级 " +
                  "  from " + tableName +
                  "  group by substr(sheng,1,2)";
                pWorkspace.ExecuteSQL(SHENGSQL);
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
                    string stringSQL = "create table EightTable_LDZL as select " + tableName + ".lc as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                                       "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 一级," +
                                        "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 二级," +
                                        "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 三级," +
                                        "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 四级," +
                                        "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 五级 " +
                                         "  from " + tableName +
                                         "  where lc <>' '" +
                                         "  group by lc";
                    pWorkspace.ExecuteSQL(stringSQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDZL modify 统计单位 nvarchar2(20)");
                }
                else
                {
                    //查询林场统计数据
                    string townsSQL = "insert into EightTable_LDZL select  " + tableName + ".lc as 统计单位,sum(case when Zl_DJ between '1' and '5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 合计 ," +
                                      "sum(case when " + tableName + ".Zl_DJ='1' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 一级," +
                                      "sum(case when " + tableName + ".Zl_DJ='2' then round(" + tableName + "." + strMjFieldName + ",2) else 0  end) as 二级," +
                                      "sum(case when " + tableName + ".Zl_DJ='3' then round(" + tableName + "." + strMjFieldName + ",2)  else 0 end) as 三级," +
                                      "sum(case when " + tableName + ".Zl_DJ='4' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 四级," +
                                      "sum(case when " + tableName + ".Zl_DJ='5' then round(" + tableName + "." + strMjFieldName + ",2) else 0 end) as 五级 " +
                                      "  from " + tableName +
                                      "  where lc <> ' '" +
                                      "  group by lc";
                    pWorkspace.ExecuteSQL(townsSQL);
                }
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
                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "create table EightTable_LDLYFXGHMJ as select substr(" +
                                 tableName + ".xiang,1,8) as 统计单位," +
                                 "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                 " from " + tableName +
                                 " group by substr(xiang,1,8)";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_LDLYFXGHMJ modify 统计单位 nvarchar2(20)");
                //查询县级统计数据SQL语句
                //ygc 2012-8-22
                string CitySQL = "insert  into EightTable_LDLYFXGHMJ  select " +
                                 tableName + ".xian as 统计单位," +
                                 "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                 " from " + tableName +
                                 " group by xian";
                pWorkspace.ExecuteSQL(CitySQL);
                
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert  into EightTable_LDLYFXGHMJ  select substr(" +
                 tableName + ".shi,1,4) as 统计单位," +
                 "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                 " from " + tableName +
                 " group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
                //查询省级数据  ygc 2012-10-24
                string SHENGSQL = "insert  into EightTable_LDLYFXGHMJ  select substr(" +
                        tableName + ".sheng,1,2) as 统计单位," +
                        "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                        "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                        "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                        "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                        "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                        "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                        "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                        "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                        "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                        "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                        "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                        "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                        "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                        "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                        "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                        "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                        "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                        "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                        "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                        "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                        "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                        "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                        "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                        "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                        "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                        " from " + tableName +
                        " group by substr(sheng,1,2)";
                pWorkspace.ExecuteSQL(SHENGSQL);
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
                                     "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                     "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                     "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                     " from " + tableName +
                                     " where lc <>' '" +
                                     " group by lc";
                    pWorkspace.ExecuteSQL(CitySQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDLYFXGHMJ modify 统计单位 nvarchar2(20)");
                }
                else
                {
                    //查询乡镇级统计数据
                    //ygc 2012-8-22
                    string townsSQL = "insert  into EightTable_LDLYFXGHMJ  select " +
                                     tableName + ".lc as 统计单位," +
                                     "sum (case when " + tableName + ".sen_lin_lb in('11','12','21','22') and " + tableName + ".dl <= '114'then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                     "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                     "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                     " from " + tableName +
                                     "  where lc <> ' '" +
                                     " group by lc";
                    pWorkspace.ExecuteSQL(townsSQL);
                }
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
                pWorkspace.ExecuteSQL("create table tempTable as select sheng,shi,xian,xiang,LD_QS,DL,QI_YUAN,SUM(" + StatisticsFieldName + ") AS mj from " + tableName + " group by sheng,shi,xian,xiang,LD_QS,DL,QI_YUAN");
                //更新临时表中的权属信息
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='国有' where LD_QS in('10','15')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='集体' where LD_QS in('21','22','23','16')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='其他' where LD_QS in('20')");

                //经用户讨论，颜东海
                //在临时表中为绕过不合理规则，进行折中处理
                //不合理规则，非林地本无起源，但无起源便无法将统计结果归属对应的行，表中一组数据有天然和人工两行，现将其归为天然行。
                //林地中150至180原因与上雷同
                pWorkspace.ExecuteSQL("update temptable set qi_yuan='19' where dl between '161' and '180'");
                pWorkspace.ExecuteSQL("update temptable set qi_yuan='29' where dl = '150'");
                pWorkspace.ExecuteSQL("update temptable set qi_yuan='19' where qi_yuan=' '");
                //ygc 2013-3-12

                //统计乡镇级数据
                string townsSQL = "create table EightTable_LDXZ as select " +
                                 "substr(xiang,1,8) as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                                 "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                                 "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                                 "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                                 "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                                 "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                                 "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                                 "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                                 "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                                 "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                                 "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                                 "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                                 "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                                 "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                                 "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                                 "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                                 "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                                 "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                                 "  from tempTable" +
                                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                                 "  group by substr(xiang,1,8),rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_LDXZ modify 统计单位 nvarchar2(20)");
                //统计县级数据
                string CitySQL = "insert into EightTable_LDXZ  select " +
                                 "xian as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                                 "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                                 "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                                 "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                                 "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                                 "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                                 "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                                 "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                                 "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                                 "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                                 "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                                 "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                                 "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                                 "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                                 "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                                 "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                                 "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                                 "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                                 "  from tempTable" +
                                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                                 "  group by xian,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(CitySQL);
                
                //获得市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_LDXZ  select " +
                 "substr(shi,1,4) as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                 "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                 "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                 "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                 "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                 "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                 "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                 "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                 "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                 "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                 "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                 "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                 "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                 "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                 "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                 "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                 "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                 "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                 "  from tempTable" +
                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                 "  group by substr(shi,1,4),rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(SHISQL);
                //查询省级数据 ygc 2012-10-24
                string SHENGSQL = "insert into EightTable_LDXZ  select " +
                 "substr(sheng,1,2) as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                 "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                 "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                 "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                 "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                 "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                 "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                 "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                 "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                 "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                 "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                 "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                 "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                 "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                 "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                 "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                 "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                 "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                 "  from tempTable" +
                 "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' '" +
                 "  group by substr(sheng,1,2),rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                 "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                pWorkspace.ExecuteSQL(SHENGSQL);
                //更新森林覆盖率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 森林覆盖率=round((有林地小计+特灌)/土地总面积 * 100,2) where 权属 is null");
                //更新林木绿化率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 林木绿化率=round((有林地小计+灌木林地小计)/土地总面积 * 100,2) where 权属 is null ");
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
                pWorkspace.ExecuteSQL("create table tempTable as select LC,LD_QS,DL,QI_YUAN,SUM(" + StatisticsFieldName + ") AS mj from " + tableName + " group by LC,LD_QS,DL,QI_YUAN");
                //更新临时表中的权属信息
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='国有' where LD_QS in('10','15')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='集体' where LD_QS in('21','22','23','16')");
                pWorkspace.ExecuteSQL("update tempTable set LD_QS='其他' where LD_QS in('20')");
                if (!ExistTable(pWorkspace, "EightTable_LDXZ"))
                {
                    //统计县级数据
                    string CitySQL = "create table EightTable_LDXZ as select " +
                                     "lc as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                     "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                                     "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                                     "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                                     "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                                     "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                                     "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                                     "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                                     "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                                     "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                                     "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                                     "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                                     "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                                     "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                                     "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                                     "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                                     "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                                     "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                                     "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                                     "  from tempTable" +
                                     "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     "  group by lc,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                     "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                    pWorkspace.ExecuteSQL(CitySQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDXZ modify 统计单位 nvarchar2(20)");
                }
                else
                {
                    //统计乡镇级数据
                    string townsSQL = "insert into EightTable_LDXZ  select " +
                                     "lc as 统计单位,LD_QS as 权属,substr(QI_YUAN,1,1) as 起源," +
                                     "sum(case when DL <>' ' then round(mj,2) else 0 end) as 土地总面积," +
                                     "sum(case when DL<=180 then round(mj,2) else 0 end) as 林地合计," +
                                     "sum(case when DL between '111' and '114' then round(mj,2) else 0 end) as 有林地小计," +
                                     "sum(case when DL='111' OR DL='112' OR DL='114' then round(mj,2) else 0 end) as 乔木林," +
                                     "sum(case when DL='113' then round(mj,2) else 0 end) as 竹林," +
                                     "sum(case when DL='114' then round(mj,2) else 0 end) as 红树林," +
                                     "sum(case when DL='120' then round(mj,2) else 0 end) as 疏林地," +
                                     "sum(case when DL between '131' and '133' then round(mj,2) else 0 end) as 灌木林地小计," +
                                     "sum(case when DL='131' then round(mj,2) else 0 end) as 特灌," +
                                     "sum(case when DL='133' or DL='132' then round(mj,2) else 0 end ) as 其他灌木林," +
                                     "sum(case when DL='141' or DL='142' then round(mj,2) else 0 end) as 未成林造林地," +
                                     "sum(case when DL='150' then round(mj,2) else 0 end) as 苗圃地," +
                                     "sum(case when DL='161' or DL='162' or DL='163' then round(mj,2) else 0 end) as 无立木林地," +
                                     "sum(case when DL='171' or DL='172' or DL='173' or DL='174' then round(mj,2) else 0 end) as 宜林地," +
                                     "sum(case when DL='180' then  round(mj,2) else 0 end ) as 林辅地," +
                                     "sum(case when dl like '2%' then round(mj,2) else 0 end) as 非林地," +
                                     "sum (case when dl=' ' then round(mj,2) else 0 end) as 森林覆盖率," +
                                     "sum(case when dl=' ' then round(mj,2) else 0 end) as 林木绿化率" +
                                     "  from tempTable" +
                                     "  where LD_QS is not null and LD_QS<>' ' and substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     "  group by lc,rollup(LD_QS),rollup(substr(QI_YUAN,1,1))" +
                                     "  order by LD_QS desc,substr(QI_YUAN,1,1) desc";
                    pWorkspace.ExecuteSQL(townsSQL);
                }
                //更新森林覆盖率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 森林覆盖率=round((有林地小计+特灌)/土地总面积 * 100,2) where 权属 is null");
                //更新林木绿化率
                pWorkspace.ExecuteSQL("update EightTable_LDXZ set 林木绿化率=round((有林地小计+灌木林地小计)/土地总面积 * 100,2) where 权属 is null ");
                pWorkspace.ExecuteSQL("alter table EightTable_LDXZ modify 起源 nvarchar2(10)");
                UpdateStatistictable(pWorkspace, "EightTable_LDXZ", "起源", "天然", "1");
                UpdateStatistictable(pWorkspace, "EightTable_LDXZ", "起源", "人工", "2");
            
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

                //查询乡镇及统计数据SQL ygc 2012-8-23
                string townsSQL = "create table EightTable_LDSLMJGH as select  " +
                                 "substr(xiang,1,8) as 统计单位," +
                                 "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                                 "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                                 "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                                 "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                                 "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                                 "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                                 "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                                 "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                                 "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                                 "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                                 "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                                 "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                                 "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                                 "  from " + tableName +
                                 "  group by substr(xiang,1,8)";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_LDSLMJGH modify 统计单位 nvarchar2(20)");
                //查询县级数据SQL语句 ygc 2012-8-23
                string CitySQL = "insert into EightTable_LDSLMJGH  select " +
                                 "xian as 统计单位," +
                                 "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                                 "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                                 "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                                 "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                                 "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                                 "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                                 "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                                 "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                                 "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                                 "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                                 "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                                 "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                                 "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                                 "  from " + tableName +
                                 "  group by xian";
                pWorkspace.ExecuteSQL(CitySQL);
                
                //获得市级数据 ygc 2012-10-22
                string SHISQL = " insert into EightTable_LDSLMJGH  select " +
                 "substr(shi,1,4) as 统计单位," +
                 "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                 "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                 "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                 "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                 "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                 "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                 "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                 "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                 "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                 "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                 "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                 "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                 "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                 "  from " + tableName +
                 "  group by substr(shi,1,4)";
                pWorkspace.ExecuteSQL(SHISQL);
                //查询省级数据 ygc 2012-10-24
                string SHENGSQL = " insert into EightTable_LDSLMJGH  select " +
                 "substr(sheng,1,2) as 统计单位," +
                 "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                 "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                 "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                 "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                 "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                 "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                 "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                 "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                 "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                 "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                 "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                 "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                 "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                 "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                 "  from " + tableName +
                 "  group by substr(sheng,1,2)";
                pWorkspace.ExecuteSQL(SHENGSQL);
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
                                     "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                                     "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                                     "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                                     "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                                     "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                                     "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                                     "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                                     "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                                     "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                                     "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                                     "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                                     "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                                     "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                                     "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                                     "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                                     "  from " + tableName +
                                     "  where lc <>' '" +
                                     "  group by lc";
                    pWorkspace.ExecuteSQL(CitySQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDSLMJGH modify 统计单位 nvarchar2(20)");
                }
                else
                {
                    //查询乡镇及统计数据SQL ygc 2012-8-23
                    string townsSQL = " insert into EightTable_LDSLMJGH  select " +
                                     "lc as 统计单位," +
                                     "sum(case when DL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 现状林地," +
                                     "sum(case when (DL LIKE '11%' OR DL='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 现状森林," +
                                     "sum(case when substr(dl,1,2) between '21'and '25' and GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充合计," +
                                     "sum(case when DL LIKE '24%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充未利用地," +
                                     "sum(case when DL LIKE '25%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充建设用地," +
                                     "sum(case when DL LIKE '22%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充牧草地," +
                                     "sum(case when DL LIKE '21%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充耕地," +
                                     "sum(case when DL LIKE '23%' AND GHDL LIKE '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 补充其他," +
                                     "sum(case when substr(dl,1,2) between '21'and '25'and (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加合计," +
                                     "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加林地," +
                                     "sum(case when DL LIKE '24%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加未利用地," +
                                     "sum(case when DL LIKE '25%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加建设用地," +
                                     "sum(case when DL LIKE '22%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加草地," +
                                     "sum(case when DL LIKE '21%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加耕地," +
                                     "sum(case when DL LIKE '23%' AND (GHDL LIKE '11%' OR GHDL ='131') then round(" + StatisticsFieldName + ",2) else 0 end) as 森林增加其他" +
                                     "  from " + tableName +
                                     "  where lc<> ' '" +
                                     "  group by lc";
                    pWorkspace.ExecuteSQL(townsSQL);
                }
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
                //查询乡镇级统计数据
                //ygc 2012-8-22
                string townsSQL = "create table EightTable_LDJGXZ as select substr(" +
                                 tableName + ".xiang,1,8) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                 " from " + tableName +
                                 " where  substr(QI_YUAN,1,1)<>' '" +
                                 " group by substr(xiang,1,8),rollup(substr(QI_YUAN,1,1)) " +
                                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_LDJGXZ modify 统计单位 nvarchar2(20)");

                //查询县级统计数据SQL语句
                //ygc 2012-8-22
                string CitySQL = "insert  into EightTable_LDJGXZ  select " +
                                 tableName + ".xian as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                 " from " + tableName +
                                 " where substr(QI_YUAN,1,1)<>' '" +
                                 " group by xian,rollup(substr(QI_YUAN,1,1)) " +
                                 " order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(CitySQL);
                
                //查询市级数据 ygc 2012-10-22
                string SHISQL = "insert  into EightTable_LDJGXZ  select  substr(" +
                 tableName + ".shi,1,4) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                 " from " + tableName +
                 " where  substr(QI_YUAN,1,1)<>' '" +
                 " group by substr(shi,1,4),rollup(substr(QI_YUAN,1,1)) " +
                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(SHISQL);
                //查询省级数据 ygc 2012-10-24 
                string SHENGSQL = "insert  into EightTable_LDJGXZ  select  substr(" +
                 tableName + ".sheng,1,2) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                 "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                 "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                 "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                 "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                 "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                 "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                 " from " + tableName +
                 " where  substr(QI_YUAN,1,1)<>' '" +
                 " group by substr(sheng,1,2),rollup(substr(QI_YUAN,1,1)) " +
                 " order by substr(QI_YUAN,1,1)";
                pWorkspace.ExecuteSQL(SHENGSQL);
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
                                     "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                     "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                     "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                     " from " + tableName +
                                     " where substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     " group by lc,rollup(substr(QI_YUAN,1,1)) " +
                                     " order by substr(QI_YUAN,1,1) ";
                    pWorkspace.ExecuteSQL(CitySQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDJGXZ modify 统计单位 nvarchar2(20)");

                }
                else
                {
                    //查询乡镇级统计数据
                    //ygc 2012-8-22
                    string townsSQL = "insert  into EightTable_LDJGXZ  select " +
                                     tableName + ".lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '11' and '12'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 公益林合计," +
                                     "sum (case when " + tableName + ".sen_lin_lb ='11' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 重点公益林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益小计," +
                                     "sum (case when " + tableName + ".sen_lin_lb='11' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='11'and (((substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') and " + tableName + ".dl between '113' and '114')) and (substr(" + tableName + ".lz,1,2)<>'11' or substr(" + tableName + ".lz,1,2)<>'12') then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as  重点公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='12' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)   end ) as 一般公益合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112' and (substr(" + tableName + ".lz,1,2)='11'or substr(" + tableName + ".lz,1,2)='12') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='11' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益防护林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and " + tableName + ".dl<='112' and substr(" + tableName + ".lz,1,2)='12' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般公益特用林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='12'and ((substr(" + tableName + ".lz,1,2)<>'11' and substr(" + tableName + ".lz,1,2)<>'12') or (((substr(" + tableName + ".lz,1,2)='11' or substr(" + tableName + ".lz,1,2)='12')) and " + tableName + ".dl between '113' and '114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般公益其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb between '21' and '22'and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='21' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 重点商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='21'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 重点其他," +
                                     "sum(case when " + tableName + ".sen_lin_lb ='22' and " + tableName + ".dl<='114' then round(" + tableName + "." + StatisticsFieldName + ",2)  else 0 end) as 一般商品林合计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='114' and (substr(" + tableName + ".lz,1,2) between '23' and '25') then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 一般商品林小计," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='23' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般用材林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='25'and " + tableName + ".dl='114' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般经济林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22' and " + tableName + ".dl<='112'and substr(" + tableName + ".lz,1,2)='24' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般薪碳林," +
                                     "sum(case when " + tableName + ".sen_lin_lb='22'and ((substr(" + tableName + ".lz,1,2)<>'23' and substr(" + tableName + ".lz,1,2)<>'24' and substr(" + tableName + ".lz,1,2)<>'25') or (substr(" + tableName + ".lz,1,2)='23' and " + tableName + ".dl>'112') or (substr(" + tableName + ".lz,1,2)='25' and " + tableName + ".dl<>'114')) then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 一般其他" +
                                     " from " + tableName +
                                     " where  substr(QI_YUAN,1,1)<>' ' and lc<>' '" +
                                     " group by lc,rollup(substr(QI_YUAN,1,1)) " +
                                     " order by substr(QI_YUAN,1,1)";
                    pWorkspace.ExecuteSQL(townsSQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_LDJGXZ modify 起源 nvarchar2(10)");
                    UpdateStatistictable(pWorkspace, "EightTable_LDJGXZ", "起源", "天然", "1");
                    UpdateStatistictable(pWorkspace, "EightTable_LDJGXZ", "起源", "人工", "2");
                }
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
                //利用SQL语句查询统计乡镇数据 ygc 2012-8-23
                string townsSQL = "create table EightTable_GJGYHDJ as select " +
                                 "substr(xiang,1,8) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                                 "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                                 "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                                 "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                                 "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                                 "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                                 "sum(case when  BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                                 "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                                 "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                                 "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                                 "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                                 "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                                 "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                                 "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                                 "  from " + tableName +
                                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                 "  group by substr(xiang,1,8),rollup(substr(QI_YUAN,1,1))" +
                                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(townsSQL);
                pWorkspace.ExecuteSQL("alter table EightTable_GJGYHDJ modify 统计单位 nvarchar2(20)");
                //通过SQL语句获得县级统计数据 ygc 2012-8-23
                string CitySQL = "insert into EightTable_GJGYHDJ select " +
                                 "xian as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                 "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                                 "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                                 "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                                 "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                                 "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                                 "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                                 "sum(case when BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                                 "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                                 "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                                 "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                                 "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                                 "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                                 "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                                 "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                                 "  from " + tableName +
                                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                                 "  group by xian,rollup(substr(QI_YUAN,1,1))" +
                                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(CitySQL);
                
                //获取市级数据 ygc 2012-10-22
                string SHISQL = "insert into EightTable_GJGYHDJ select " +
                 "substr(shi,1,4) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                 "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                 "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                 "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                 "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                 "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                 "sum(case when  BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                 "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                 "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                 "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                 "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                 "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                 "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                 "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                 "  from " + tableName +
                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                 "  group by substr(shi,1,4),rollup(substr(QI_YUAN,1,1))" +
                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(SHISQL); 
                //查询省级数据 ygc 2012-10-24
                string SHENGSQL = "insert into EightTable_GJGYHDJ select " +
                 "substr(sheng,1,2) as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                 "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                 "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                 "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                 "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                 "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                 "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                 "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                 "sum(case when  BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                 "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                 "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                 "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                 "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                 "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                 "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                 "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                 "  from " + tableName +
                 "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1'" +
                 "  group by substr(sheng,1,2),rollup(substr(QI_YUAN,1,1))" +
                 "  order by substr(QI_YUAN,1,1) ";
                pWorkspace.ExecuteSQL(SHENGSQL); 

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
                    //利用SQL语句查询统计乡镇数据 ygc 2012-8-23
                    string townsSQL = "create table EightTable_GJGYHDJ as select " +
                                     "lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                     "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                     "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                                     "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                                     "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                                     "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                                     "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                                     "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                                     "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                                     "sum(case when  BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                                     "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                                     "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                                     "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                                     "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                                     "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                                     "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                                     "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                                     "  from " + tableName +
                                     "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1' and lc<>' '" +
                                     "  group by lc,rollup(substr(QI_YUAN,1,1))" +
                                     "  order by substr(QI_YUAN,1,1) ";
                    pWorkspace.ExecuteSQL(townsSQL);
                    pWorkspace.ExecuteSQL("alter table EightTable_GJGYHDJ modify 统计单位 nvarchar2(20)");

                }
                else 
                {
                    //通过SQL语句获得县级统计数据 ygc 2012-8-23
                    string CitySQL = "insert into EightTable_GJGYHDJ select " +
                                     "lc as 统计单位,substr(QI_YUAN,1,1) as 起源," +
                                     "sum(case when BHDJ between '1' and '3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计," +
                                     "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 防护林合计," +
                                     "sum(case when BHDJ between '1' and '3' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 特用林合计," +
                                     "sum(case when BHDJ between '1' and '3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 其他合计," +
                                     "sum(case when BHDJ ='1' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级小计," +
                                     "sum(case when BHDJ='1' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 一级防护," +
                                     "sum(case when BHDJ='1' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 一级特用," +
                                     "sum(case when BHDJ='1' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 一级其他," +
                                     "sum(case when BHDJ ='2' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级小计," +
                                     "sum(case when BHDJ='2' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 二级防护," +
                                     "sum(case when BHDJ='2' and substr(lz,1,2)='12' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二级特用," +
                                     "sum(case when BHDJ='2' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as 二级其他," +
                                     "sum(case when BHDJ ='3' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级小计," +
                                     "sum(case when BHDJ='3' and substr(lz,1,2)='11' and dl<='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 三级防护," +
                                     "sum(case when BHDJ='3' and substr(lz,1,2)='12' and dl<='112'then round(" + StatisticsFieldName + ",2) else 0 end) as 三级特用," +
                                     "sum(case when BHDJ='3' and ((substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12') or ((substr(lz,1,2)='11' or substr(lz,1,2)='12') and dl>'112')) then round(" + StatisticsFieldName + ",2) else 0 end) as  三级其他" +
                                     "  from " + tableName +
                                     "  where substr(QI_YUAN,1,1)<>' ' and QI_YUAN is not null and sq='1' and lc<>' '" +
                                     "  group by lc,rollup(substr(QI_YUAN,1,1))" +
                                     "  order by substr(QI_YUAN,1,1) ";
                    pWorkspace.ExecuteSQL(CitySQL);
                }

                
                //更新合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 合计= 防护林合计+特用林合计+其他合计");
                //更新一级合计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 一级小计=一级防护+一级特用+一级其他 ");
                //更新二级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 二级小计=二级防护+二级特用+二级其他");
                //更新三级小计
                pWorkspace.ExecuteSQL("update EightTable_GJGYHDJ set 三级小计=三级防护+三级特用+三级其他");

                pWorkspace.ExecuteSQL("alter table EightTable_GJGYHDJ modify 起源 nvarchar2(10)");
                UpdateStatistictable(pWorkspace, "EightTable_GJGYHDJ", "起源", "天然", "1");
                UpdateStatistictable(pWorkspace, "EightTable_GJGYHDJ", "起源", "人工", "2");
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
               //查询乡级统计数据SQL语句
               //ygc 2012-8-22
               string townsSQL = "create table EightTable_LDBHDJTable as select substr(" +
                                 tableName + ".xiang,1,8) as 统计单位," +
                                "sum(case when (BCLD<>'2' or bcld is null)and bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                                "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                                "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                                "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                                "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                                "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                                " from " + tableName +
                                " group by substr(xiang,1,8)";
               pWorkspace.ExecuteSQL(townsSQL);
               pWorkspace.ExecuteSQL("alter table EightTable_LDBHDJTable modify 统计单位 nvarchar2(20)");
               //查询县级统计数据SQL语句
               //ygc 2012-8-22
               string CitySQL = "insert into EightTable_LDBHDJTable select " +
                                 tableName + ".xian as 统计单位," +
                                "sum(case when (BCLD<>'2' or bcld is null)and  bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                                "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                                "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                                "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                                "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                                "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                                "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                                " from " + tableName +
                                " group by xian";
               pWorkspace.ExecuteSQL(CitySQL);
               
               //查询市级数据 ygc 2012-10-22
               string SHISQL = "insert into EightTable_LDBHDJTable select  substr(" +
                  tableName + ".shi,1,4) as 统计单位," +
                 "sum(case when (BCLD<>'2' or bcld is null)and bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                 "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                 "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                 "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                 "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                 "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                 " from " + tableName +
                 " group by substr(shi,1,4)";
               pWorkspace.ExecuteSQL(SHISQL);
               //查询省级数据 ygc 2012-10-24
               string SHENGSQL = "insert into EightTable_LDBHDJTable select  substr(" +
                  tableName + ".sheng,1,2) as 统计单位," +
                 "sum(case when (BCLD<>'2' or bcld is null)and bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                 "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                 "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                 "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                 "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                 "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                 "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                 " from " + tableName +
                 " group by substr(sheng,1,2)";
               pWorkspace.ExecuteSQL(SHENGSQL);
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
                                    "sum(case when (BCLD<>'2' or bcld is null)and  bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                                    "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                                    "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                                    "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                                    "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                                    "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                                    " from " + tableName +
                                    "  where lc <>' '" +
                                    " group by lc";
                   pWorkspace.ExecuteSQL(CitySQL);
                   pWorkspace.ExecuteSQL("alter table EightTable_LDBHDJTable modify 统计单位 nvarchar2(20)");
               }
               else
               {

                   //查询乡级统计数据SQL语句
                   //ygc 2012-8-22
                   string townsSQL = "insert into EightTable_LDBHDJTable select  " +
                                     tableName + ".lc as 统计单位," +
                                    "sum(case when (BCLD<>'2' or bcld is null)and bh_dj between '1'and '4' then round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end ) as 现状合计," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状1级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='2'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状2级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='3'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状3级," +
                                    "sum(case when  (BCLD<>'2' or bcld is null) AND BH_DJ='4'then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 现状4级," +
                                    "sum(case when  LB_GHBHDJ between '1'and '4'then round(" + tableName + "." + StatisticsFieldName + ",2) end) as 规划合计," +
                                    "sum(case when  LB_GHBHDJ='1' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划1级," +
                                    "sum(case when  LB_GHBHDJ='2' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划2级," +
                                    "sum(case when  LB_GHBHDJ='3' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划3级," +
                                    "sum(case when  LB_GHBHDJ='4' then  round(" + tableName + "." + StatisticsFieldName + ",2) else 0 end) as 规划4级" +
                                    " from " + tableName +
                                    " where lc<>' '" +
                                    " group by lc";
                   pWorkspace.ExecuteSQL(townsSQL);
               }
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
               pWorkspace.ExecuteSQL("create table temp_table_1 as select xiang,xian,shi,sheng,lz,ghlz,SQ,GHSQ,BHDJ,sum(" + StatisticsFieldName + ") as mj from  " + tableName + " group by xiang,xian,shi,sheng,lz,ghlz,SQ,GHSQ,BHDJ");
               pWorkspace.ExecuteSQL("create table temp_table_2 as select * from  temp_table_1");
               pWorkspace.ExecuteSQL("alter table temp_table_1 modify lz nvarchar2(20)");
               pWorkspace.ExecuteSQL("alter table temp_table_2 modify lz nvarchar2(20)");
               //更新临时表 ygc 2012-8-27 
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='2防护林' where substr(lz,1,2)='11'");
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='1特用林' where substr(lz,1,2)='12'");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='4其他林地' where substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12' and substr(lz,1,2)<>' '");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='3公益林' where (substr(lz,1,2)='11' or substr(lz,1,2)='12')");

               //用SQL语句获取乡级行政区统计数据
               string townsSQL = " create table EightTable_GJGYLGHMJ as (select " +
                                "substr(temp_table_1.xiang,1,8) as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                " from temp_table_1" +
                                " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
                                " group by substr(xiang,1,8),lz" +
                                " union all" +
                                " select " +
                                " substr(xiang,1,8) as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                " from temp_table_2" +
                                " where  ( lz<>' 'and ghlz<>' ')" +
                                " group by substr(xiang,1,8),rollup(lz))";
               pWorkspace.ExecuteSQL(townsSQL);
               pWorkspace.ExecuteSQL("alter table EightTable_GJGYLGHMJ modify 统计单位 nvarchar2(20)");
               //通过SQL获得县级统计数据 ygc 2012-9-27 
               string CitySQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                                "temp_table_1.xian as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                " from temp_table_1" +
                                " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
                                " group by xian,lz" +
                                " union all" +
                                " select " +
                                " xian as 统计单位,lz as 林种," +
                                "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                " from temp_table_2" +
                                " where  ( lz<>' 'and ghlz<>' ')" +
                                " group by xian,rollup(lz))";
               pWorkspace.ExecuteSQL(CitySQL);
               
               //获取市级数据a
               string SHISQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                 "substr(temp_table_1.shi,1,4) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                 " from temp_table_1" +
                 " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
                 " group by substr(shi,1,4),lz" +
                 " union all" +
                 " select " +
                 " substr(shi,1,4) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                 " from temp_table_2" +
                 " where  ( lz<>' 'and ghlz<>' ')" +
                 " group by substr(shi,1,4),rollup(lz))";
               pWorkspace.ExecuteSQL(SHISQL);
               //查询省级数据 ygc 2012-10-24
               string SHENGSQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                 "substr(temp_table_1.sheng,1,2) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                 " from temp_table_1" +
                 " where  ( lz<>' 'and ghlz<>' ') and (lz='1特用林' or lz='2防护林')" +
                 " group by substr(sheng,1,2),lz" +
                 " union all" +
                 " select " +
                 " substr(sheng,1,2) as 统计单位,lz as 林种," +
                 "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                 "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                 "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                 "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                 "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                 "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                 "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                 "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                 " from temp_table_2" +
                 " where  ( lz<>' 'and ghlz<>' ')" +
                 " group by substr(sheng,1,2),rollup(lz))";
               pWorkspace.ExecuteSQL(SHENGSQL);
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
               pWorkspace.ExecuteSQL("create table temp_table_1 as select lc,SQ,GHSQ,BHDJ,lz,ghlz,sum(" + StatisticsFieldName + ") as mj from  " + tableName + " group by lc,SQ,GHSQ,BHDJ,lz,ghlz");
               pWorkspace.ExecuteSQL("create table temp_table_2 as select * from  temp_table_1");
               pWorkspace.ExecuteSQL("alter table temp_table_1 modify lz nvarchar2(20)");
               pWorkspace.ExecuteSQL("alter table temp_table_2 modify lz nvarchar2(20)");
               //更新临时表 ygc 2012-8-27 
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='2防护林' where substr(lz,1,2)='11'");
               pWorkspace.ExecuteSQL("update temp_table_1 set lz='1特用林' where substr(lz,1,2)='12'");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='4其他林地' where substr(lz,1,2)<>'11' and substr(lz,1,2)<>'12' and substr(lz,1,2)<>' '");
               pWorkspace.ExecuteSQL("update temp_table_2 set lz='3公益林' where (substr(lz,1,2)='11' or substr(lz,1,2)='12')");
               if (!ExistTable(pWorkspace, "EightTable_GJGYLGHMJ"))
               {
                   string CitySQL = " create table EightTable_GJGYLGHMJ as (select " +
                                   "temp_table_1.lc as 统计单位,lz as 林种," +
                                   "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                   "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                   "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                   "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                   "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                   "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                   "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                   "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                   " from temp_table_1" +
                                   " where  ( lz<>' 'or ghlz<>' ') and (lz='1特用林' or lz='2防护林') and lc<>' '" +
                                   " group by lc,lz" +
                                   " union all" +
                                   " select " +
                                   " lc as 统计单位,lz as 林种," +
                                   "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                   "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                   "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                   "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                   "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                   "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                   "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                   "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                   " from temp_table_2" +
                                   " where  ( lz<>' 'or ghlz<>' ') and lc<>' '" +
                                   " group by lc,rollup(lz))";
                   pWorkspace.ExecuteSQL(CitySQL);
                   pWorkspace.ExecuteSQL("alter table EightTable_GJGYLGHMJ modify 统计单位 nvarchar2(20)");
               }
               else
               {
                   //用SQL语句获取乡级行政区统计数据
                   string townsSQL = " insert  into EightTable_GJGYLGHMJ  (select " +
                                    "temp_table_1.lc as 统计单位,lz as 林种," +
                                    "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                    "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                    "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                    "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                    "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                    "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                    "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                    "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                    " from temp_table_1" +
                                    " where  ( lz<>' 'or ghlz<>' ') and (lz='1特用林' or lz='2防护林') and lc<>' '" +
                                    " group by lc,lz" +
                                    " union all" +
                                    " select " +
                                    " lc as 统计单位,lz as 林种," +
                                    "sum(case when SQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 现状合计," +
                                    "sum(case when SQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 现状一级," +
                                    "sum(case when SQ='1' AND BHDJ='2' then round(mj,2) else 0 end) as 现状二级," +
                                    "sum(case when SQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 现状三级," +
                                    "sum(case when GHSQ='1' AND BHDJ between '1' and '3' then round(mj,2) else 0 end) as 规划合计," +
                                    "sum(case when GHSQ='1' AND BHDJ='1' then round(mj,2) else 0 end) as 规划一级," +
                                    "sum(case when GHSQ='1' AND BHDJ='2'then round(mj,2) else 0 end) as 规划二级," +
                                    "sum(case when GHSQ='1' AND BHDJ='3' then round(mj,2) else 0 end) as 规划三级" +
                                    " from temp_table_2" +
                                    " where  ( lz<>' 'or ghlz<>' ') and lc<>' '" +
                                    " group by lc,rollup(lz))";
                   pWorkspace.ExecuteSQL(townsSQL);
               }
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
        //进行二类统计 ygc 2012-12-6
        public  static void DoEcostatistic(IFeatureClass iFeatureclass, string strMjFieldName, string strXJFielldName,string strZSFieldName,SysCommon.CProgress pProgress)
        {
            pProgress.SetProgress(1, "进行用材林面积蓄积按龄级统计");
            bool bRes = false;
            bRes = XZQYCLMJXJ_Statistic(iFeatureclass, strXJFielldName, strMjFieldName);
            bRes = LCYCLMJXJ_Statistic(iFeatureclass, strXJFielldName, strMjFieldName);

            pProgress.SetProgress(2, "生态公益林（地）统计");
            bRes = XZQSTGYL_Statistic(iFeatureclass, strMjFieldName);
            bRes = LCSTGYL_Statistic(iFeatureclass, strMjFieldName);

            pProgress.SetProgress(3, "乔木林面积蓄积按龄组统计");
           bRes= XZQQMLMJ_Statistic(iFeatureclass, strXJFielldName, strMjFieldName);
           //bRes = LCQMLMJ_Statistic(iFeatureclass, strXJFielldName, strMjFieldName);

           pProgress.SetProgress(4, "林种统计");
           bRes = XZQLZ_Statistic(iFeatureclass, strXJFielldName, strMjFieldName);

           pProgress.SetProgress(5, "经济林统计");
           bRes = XZQJJL_Statistic(iFeatureclass, strZSFieldName, strMjFieldName);
           //bRes = LCJJL_Statistic(iFeatureclass, strZSFieldName, strMjFieldName);

           pProgress.SetProgress(6, "灌木林统计");
           bRes = XZQGML_Statistic(iFeatureclass, strMjFieldName);
          // bRes = LCCML_Statistic(iFeatureclass, strMjFieldName);

           pProgress.SetProgress(7, "各类土地面积统计");
           bRes = XZQGLTDMJ_Statistic(iFeatureclass, strMjFieldName);

           pProgress.SetProgress(8, "各类森林、林木面积蓄积统计");
           bRes = XZQGLSLMJ_Statistic(iFeatureclass, strXJFielldName, strZSFieldName, strZSFieldName);

        }
        #region 用材林面积蓄积按龄级统计
        //用材林面积蓄积按龄级统计 ygc 2012-12-11
        private static bool XZQYCLMJXJ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName, string StatisticsFieldName)
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
            DropTable(pWorkspace, "EcosTable_YCLMJXJ");
             //通过SQL语句获得统计结果 ygc 2012-12-11
             try
             {
                 string xianSQL = "create table EcosTable_YCLMJXJ as select xian as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                    "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                    "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                    "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                    "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                    "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                    "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                    "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                    "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                    "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                    "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                    "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                    "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                    "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                    "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                    "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                    "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                    "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级面积," +
                                    "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                    "  from " + tableName +
                                    "  where lmqs<> ' ' and lz between '231' and '233'" +
                                    "  group by xian,rollup(lmqs),rollup(lz)";
                 pWorkspace.ExecuteSQL(xianSQL);
                 string xiangSQL = "insert into EcosTable_YCLMJXJ ( select substr(xiang,1,8) as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                    "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                    "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                    "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                    "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                    "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                    "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                    "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                    "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                    "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                    "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                    "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                    "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                    "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                    "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                    "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                    "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                    "sum(case when llz='8' then round(" + StatisticsFieldName + ",2) else 0 end) as 八龄级面积," +
                                    "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                    "  from " + tableName +
                                    "  where lmqs<> ' ' and lz between '231' and '233'" +
                                    "  group by substr(xiang,1,8),rollup(lmqs),rollup(lz))";
                 pWorkspace.ExecuteSQL(xiangSQL);
                 string shiSQL = "insert into EcosTable_YCLMJXJ ( select substr(shi,1,4) as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                    "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                    "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                    "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                    "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                    "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                    "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                    "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                    "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                    "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                    "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                    "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                    "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                    "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                    "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                    "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                    "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                    "sum(case when llz='8' then round(" + StatisticsFieldName + ",2) else 0 end) as 八龄级面积," +
                                    "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                    "  from " + tableName +
                                    "  where lmqs<> ' ' and lz between '231' and '233'" +
                                    "  group by substr(shi,1,4),rollup(lmqs),rollup(lz))";
                 pWorkspace.ExecuteSQL(shiSQL);
                 string shengSQL = "insert into EcosTable_YCLMJXJ ( select substr(sheng,1,2) as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                    "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                    "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                    "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                    "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                    "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                    "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                    "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                    "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                    "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                    "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                    "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                    "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                    "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                    "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                    "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                    "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                    "sum(case when llz='8' then round(" + StatisticsFieldName + ",2) else 0 end) as 八龄级面积," +
                                    "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                    "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                    "  from " + tableName +
                                    "  where lmqs<> ' ' and lz between '231' and '233'" +
                                    "  group by substr(sheng,1,2),rollup(lmqs),rollup(lz))";
                 pWorkspace.ExecuteSQL(shengSQL);

             }
             catch (Exception ex)
             {
                 return false;
             }
             return true;
        }
        private static bool LCYCLMJXJ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName, string StatisticsFieldName)
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
                if (!ExistTable(pWorkspace, "EcosTable_YCLMJXJ"))
                {
                    string lcSQL = "create table EcosTable_YCLMJXJ as select lc as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                        "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                        "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                        "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                        "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                        "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                        "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                        "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                        "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                        "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                        "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                        "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                        "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                        "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                        "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                        "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                        "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                        "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级面积," +
                                        "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                        "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                        "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                        "  from " + tableName +
                                        "  where lmqs<> ' ' and lz between '231' and '233' and lc<>' '" +
                                        "  group by lc,rollup(lmqs),rollup(lz)";
                    pWorkspace.ExecuteSQL(lcSQL);
                }
                else
                {
                    string lcSQL1 = "insert into EcosTable_YCLMJXJ ( select lc as 统计单位,lmqs as 林木使用权,lz as 亚林种," +
                                        "sum(case when llz between '1' and '8' then round(" + StatisticsFieldName + ",2) else 0 end) as 合计面积," +
                                        "sum(case when llz between '1' and '8' then round(xjl,2) else 0 end) as 合计蓄积," +
                                        "sum(case when llz='1' then round(" + StatisticsFieldName + ",2) else 0 end) as 一龄级面积," +
                                        "sum(case when llz='1' then round(" + strXJFielldName + ",2) else 0 end) as 一龄级蓄积," +
                                        "sum(case when llz='2' then round(" + StatisticsFieldName + ",2) else 0 end ) as 二龄级面积," +
                                        "sum(case when llz='2' then round(" + strXJFielldName + ",2) else 0 end) as 二龄级蓄积," +
                                        "sum(case when llz='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 三龄级面积," +
                                        "sum(case when llz='3' then round(" + strXJFielldName + ",2) else 0 end) as 三龄级蓄积," +
                                        "sum(case when llz='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 四龄级面积," +
                                        "sum(case when llz='4' then round(" + strXJFielldName + ",2) else 0 end) as 四龄级蓄积," +
                                        "sum(case when llz='5' then round(" + StatisticsFieldName + ",2) else 0 end) as 五龄级面积," +
                                        "sum(case when llz='5' then round(" + strXJFielldName + ",2) else 0 end) as 五龄级蓄积," +
                                        "sum(case when llz='6' then round(" + StatisticsFieldName + ",2) else 0 end) as 六龄级面积," +
                                        "sum(case when llz='6' then round(" + strXJFielldName + ",2) else 0 end) as 六龄级蓄积," +
                                        "sum(case when llz='7' then round(" + StatisticsFieldName + ",2) else 0 end) as 七龄级面积," +
                                        "sum(case when llz='7' then round(" + strXJFielldName + ",2) else 0 end) as 七龄级蓄积," +
                                        "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级面积," +
                                        "sum(case when llz='8' then round(" + strXJFielldName + ",2) else 0 end) as 八龄级蓄积," +
                                        "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + StatisticsFieldName + ",2) else 0 end) as 八以上龄级面积," +
                                        "sum(case when llz not in ('1','2','3','4','5','6','7','8') then round(" + strXJFielldName + ",2) else 0 end) as 八以上龄级蓄积" +
                                        "  from " + tableName +
                                        "  where lmqs<> ' ' and lz between '231' and '233' and lc<>' '" +
                                        "  group by lc,rollup(lmqs),rollup(lz))";
                    pWorkspace.ExecuteSQL(lcSQL1);
                    pWorkspace.ExecuteSQL("alter table EcosTable_YCLMJXJ modify 亚林种 nvarchar2(10)");
                    pWorkspace.ExecuteSQL("alter table EcosTable_YCLMJXJ modify 林木使用权 nvarchar2(10)");
                    Dictionary<string, string> dicLZ = dicGetFieldValue(pWorkspace, "林种字典");
                    UpadateStatistictable(pWorkspace, dicLZ, "EcosTable_YCLMJXJ", "亚林种");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "国有", "1");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "集体", "2");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "个人", "3");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "其它", "9");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "其它", "B");
                    UpdateStatistictable(pWorkspace, "EcosTable_YCLMJXJ", "林木使用权", "所有", "0");


                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 生态公益林（地）统计
        //生态公益林（地）统计 ygc 2012-12-11
        private static bool XZQSTGYL_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
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
            DropTable(pWorkspace, "EcosTable_STGYL");
            try
            {
                string xianSQL = "create table EcosTable_STGYL as select xian as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                     "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                      "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' '" +
                                    "  group by xian ,rollup(gclb),rollup(sq),rollup(bhdj)";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_STGYL ( select substr(xiang,1,8) as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                    "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                    "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' '" +
                                    "  group by substr(xiang,1,8) ,rollup(gclb),rollup(sq),rollup(bhdj))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_STGYL ( select substr(shi,1,4) as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                    "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                    "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' '" +
                                    "  group by substr(shi,1,4) ,rollup(gclb),rollup(sq),rollup(bhdj))";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_STGYL ( select substr(sheng,1,2) as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                    "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                    "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' '" +
                                    "  group by substr(sheng,1,2) ,rollup(gclb),rollup(sq),rollup(bhdj))";
                pWorkspace.ExecuteSQL(shengSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        private static bool LCSTGYL_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
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
                if (!ExistTable(pWorkspace, "EcosTable_YCLMJXJ"))
                {
                    string LCSQL = "create table EcosTable_STGYL as select lc as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                    "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                    "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' ' and lc<>' '" +
                                    "  group by lc ,rollup(gclb),rollup(sq),rollup(bhdj)";
                    pWorkspace.ExecuteSQL(LCSQL);
                }
                else
                {
                    string LC = "insert into EcosTable_STGYL ( select lc as 统计单位,gclb as 工程类别,sq as 事权等级,bhdj as 保护等级," +
                                    "sum(case when dl like'1%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                    "sum(case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 有林地小计," +
                                    "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                    "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林," +
                                    "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as  混交林," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end ) as 经济林," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as 疏林地," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林," +
                                    "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                    "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                    "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                    "sum(case when dl='150' then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                    "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                    "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end ) as 采伐迹地," +
                                    "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end ) as 火烧迹地," +
                                    "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它无立木林地," +
                                    "sum(case when dl between '171' and '172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                    "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                    "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                    "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                    "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕," +
                                    "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地" +
                                    "  from " + tableName +
                                    "  where gclb<>' 'and sq<>' ' and bhdj<>' ' and lc<>' '" +
                                    "  group by lc ,rollup(gclb),rollup(sq),rollup(bhdj))";
                    pWorkspace.ExecuteSQL(LC);
                    pWorkspace.ExecuteSQL("alter table EcosTable_STGYL modify 工程类别 nvarchar2(20)");
                    pWorkspace.ExecuteSQL("alter table EcosTable_STGYL modify 事权等级 nvarchar2(10)");
                    pWorkspace.ExecuteSQL("alter table EcosTable_STGYL modify 保护等级 nvarchar2(10)");
                    Dictionary<string, string> dicGC = dicGetFieldValue(pWorkspace, "工程类别字典");
                    UpadateStatistictable(pWorkspace, dicGC, "EcosTable_STGYL", "工程类别");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "事权等级", "国家公益林", "1");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "事权等级", "地方公益林", "2");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "事权等级", "其他", "B");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "事权等级", "所有", "0");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "保护等级", "特殊", "1");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "保护等级", "重点", "2");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "保护等级", "一般", "3");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "保护等级", "所有", "0");
                    UpdateStatistictable(pWorkspace, "EcosTable_STGYL", "保护等级", "其他", "B");
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 乔木林面积蓄积按龄组统计
        private static bool XZQQMLMJ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName, string StatisticsFieldName)
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
            DropTable(pWorkspace, "EcosTable_QMLMJ");
            DropTable(pWorkspace, "Temp_QML");
            try
            {
                //创建临时表
                pWorkspace.ExecuteSQL("create table Temp_QML as select xian,xiang,sheng,shi,qi_yuan,llz,dl,zysz,lc,"+StatisticsFieldName+","+strXJFielldName+"  from "+tableName);
                //更新临时表
                pWorkspace.ExecuteSQL("update Temp_QML set qi_yuan='天然' where qi_yuan like'1%'");
                pWorkspace.ExecuteSQL("update Temp_QML set qi_yuan='人工' where qi_yuan like '2%'");
                //统计数据
                string xianSQL = "create table EcosTable_QMLMJ as select xian as 统计单位," +
                                   "qi_yuan  as 起源,zysz as 树种,dl as 乔木林,"+
                                   "sum( case when LLZ between '1' and '5' then round("+StatisticsFieldName+",2) else 0 end) as 面积小计,"+
                                   "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
                                   "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                   "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                   "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
                                   "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                   "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                   "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                   "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                   "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
                                   "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
                                   "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
                                   "  from Temp_QML" +
                                   "  where dl between '111' and '112' and zysz in('120','130','150') and qi_yuan<> ' '" +
                                   "  group by xian ,rollup(zysz),rollup(qi_yuan）,rollup(dl) ";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_QMLMJ ( select substr(xiang,1,8) as 统计单位," +
                                   " qi_yuan  as 起源,zysz as 树种,dl as 乔木林," +
                                   "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
                                   "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
                                   "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                   "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                   "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
                                   "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                   "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                   "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                   "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                   "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
                                   "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
                                   "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
                                   "  from Temp_QML"  +
                                   "  where dl between '111' and '112' and zysz in('120','130','150')  and qi_yuan<> ' '" +
                                   "  group by substr(xiang,1,8) ,rollup(zysz),rollup(qi_yuan）,rollup(dl))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_QMLMJ ( select substr(shi,1,4) as 统计单位," +
                                   " qi_yuan  as 起源,zysz as 树种,dl as 乔木林," +
                                   "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
                                   "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
                                   "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                   "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                   "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
                                   "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                   "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                   "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                   "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                   "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
                                   "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
                                   "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
                                   "  from Temp_QML" +
                                   "  where dl between '111' and '112' and zysz in('120','130','150') and qi_yuan<> ' '" +
                                   "  group by substr(shi,1,4) ,rollup(zysz),rollup(qi_yuan）,rollup(dl)) ";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_QMLMJ ( select substr(sheng,1,2) as 统计单位," +
                                   " qi_yuan as 起源,zysz as 树种,dl as 乔木林," +
                                   "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
                                   "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
                                   "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                   "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                   "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
                                   "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                   "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                   "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                   "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                   "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
                                   "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
                                   "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
                                   "  from Temp_QML"  +
                                   "  where dl between '111' and '112' and zysz in('120','130','150') and qi_yuan<> ' '" +
                                   "  group by substr(sheng,1,2) ,rollup(zysz),rollup(qi_yuan）,rollup(dl)) ";
                pWorkspace.ExecuteSQL(shengSQL);
                string LC = "insert into  EcosTable_QMLMJ ( select lc as 统计单位," +
                               " qi_yuan  as 起源,zysz as 树种,dl as 乔木林," +
                               "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
                               "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
                               "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                               "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                               "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
                               "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                               "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                               "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                               "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                               "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
                               "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
                               "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
                               "  from Temp_QML" +
                               "  where dl between '111' and '112' and zysz in('120','130','150') and lc<>' ' and qi_yuan<> ' '" +
                               "  group by lc ,rollup(zysz),rollup(qi_yuan）,rollup(dl))";
                pWorkspace.ExecuteSQL(LC);
                Dictionary<string, string> dicSZ = dicGetFieldValue(pWorkspace, "树种字典");
                UpadateStatistictable(pWorkspace, dicSZ, "EcosTable_QMLMJ", "树种");
                Dictionary<string, string> dicLZ = dicGetFieldValue(pWorkspace, "林种字典");
                UpadateStatistictable(pWorkspace, dicLZ, "EcosTable_QMLMJ", "乔木林");
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        //private static bool LCQMLMJ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName, string StatisticsFieldName)
        //{
        //    if (pFeatureClass == null)
        //    {
        //        return false;
        //    }
        //    IWorkspace pWorkspace = null;
        //    string tableName = "";
        //    try
        //    {
        //        pWorkspace = pFeatureClass.FeatureDataset.Workspace;
        //        tableName = (pFeatureClass as IDataset).Name;
        //    }
        //    catch (Exception ex)
        //    { }
        //    try
        //    {
        //        if (!ExistTable(pWorkspace, "EcosTable_QMLMJ"))
        //        {
        //          string LCSQL= "create table EcosTable_QMLMJ as select lc as 统计单位," +
        //                           "case when qi_yuan like '1%' then '天然' else '人工' end as 起源,zysz as 树种,dl as 乔木林," +
        //                           "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
        //                           "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
        //                           "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
        //                           "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
        //                           "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
        //                           "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
        //                           "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
        //                           "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
        //                           "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
        //                           "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
        //                           "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
        //                           "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
        //                           "  from " + tableName +
        //                           "  where dl between '111' and '112' and zysz in('120','130','150') and lc<>' 'and qi_yuan<> ' '" +
        //                           "  group by lc ,rollup(zysz),rollup(qi_yuan）,rollup(dl)";
        //          pWorkspace.ExecuteSQL(LCSQL);

        //        }
        //        else
        //        {
        //            string LC = "insert into  EcosTable_QMLMJ ( select lc as 统计单位," +
        //                           "case when qi_yuan like '1%' then '天然' else '人工' end as 起源,zysz as 树种,dl as 乔木林," +
        //                           "sum( case when LLZ between '1' and '5' then round(" + StatisticsFieldName + ",2) else 0 end) as 面积小计," +
        //                           "sum( case when LLZ between '1' and '5' then round(" + strXJFielldName + ",2) else 0 end) as  蓄积小计," +
        //                           "sum( case when LLZ='1'then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
        //                           "sum(case when LLZ='1' then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
        //                           "sum(case when LLZ='2' then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林面积," +
        //                           "sum(case when LLZ='2' then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
        //                           "sum(case when LLZ='3' then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
        //                           "sum(case when LLZ='3' then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
        //                           "sum(case when LLZ='4' then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
        //                           "sum(case when LLZ ='4' then round(" + strXJFielldName + ",2) else 0 end ) as 成熟林蓄积," +
        //                           "sum(case when LLZ='5' then round(" + StatisticsFieldName + ",2) else 0 end ) as 过熟林面积," +
        //                           "sum(case when LLZ='5' then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积" +
        //                           "  from " + tableName +
        //                           "  where dl between '111' and '112' and zysz in('120','130','150') and lc<>' ' and qi_yuan<> ' '" +
        //                           "  group by lc ,rollup(zysz),rollup(qi_yuan）,rollup(dl))";
        //            pWorkspace.ExecuteSQL(LC);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
        #region 林种统计
        private static bool XZQLZ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName, string StatisticsFieldName)
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
            DropTable(pWorkspace, "tempTable_LZ");
            DropTable(pWorkspace, "EcosTable_LZ");
            try
            {
                //创建临时表
                pWorkspace.ExecuteSQL("create table tempTable_LZ as select lc,xian,sheng,shi,xiang,case when lz<>' ' then lz end  as 林种,case when lz<>' ' then lz end as 亚林种,LLZ,DL," + StatisticsFieldName + "," + strXJFielldName + ",JJLZS  from  " + tableName + "  where lz<>' '");
                //修改临时表
                pWorkspace.ExecuteSQL("alter table tempTable_LZ modify 林种 nvarchar2(10)");
                pWorkspace.ExecuteSQL("alter table tempTable_LZ modify 亚林种 nvarchar2(10)");
                pWorkspace.ExecuteSQL("update tempTable_LZ set 林种='防护林' where 林种 between '111' and '117'");
                pWorkspace.ExecuteSQL("update tempTable_LZ set 林种='特种用途林' where 林种 between '121' and '127'");
                pWorkspace.ExecuteSQL("update tempTable_LZ set 林种='用材林' where 林种 between '231' and '233'");
                pWorkspace.ExecuteSQL("update tempTable_LZ set 林种='薪炭林' where 林种='240'");
                pWorkspace.ExecuteSQL("update tempTable_LZ set 林种='经济林' where 林种 between '251' and '255'");
                //统计数据
                string xianSQL = "create table EcosTable_LZ as select xian as 统计单位,林种,亚林种," +
                                    "sum(case when (llz between 1 and 5) and (dl='111'or dl='120') then round(" + strXJFielldName + ",2) else 0 end) as 活立木总蓄积量," +
                                    "sum (case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林sum面积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 成熟林蓄积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 过熟林面积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end ) as  竹林面积," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as  疏林面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end ) as 疏林蓄积," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林面积小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林面积," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林面积" +
                                    "  from tempTable_LZ" +
                                    "  where xjl<>0"+
                                    "  group by xian,rollup(林种),rollup(亚林种）";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_LZ( select substr(xiang,1,8) as 统计单位,林种,亚林种," +
                                    "sum(case when (llz between 1 and 5) and (dl='111'or dl='120') then round(" + strXJFielldName + ",2) else 0 end) as 活立木总蓄积量," +
                                    "sum (case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林sum面积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 成熟林蓄积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 过熟林面积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end ) as  竹林面积," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as  疏林面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end ) as 疏林蓄积," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林面积小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林面积," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林面积" +
                                    "  from tempTable_LZ" +
                                    "  where xjl<>0" +
                                    "  group by substr(xiang,1,8),rollup(林种),rollup(亚林种）)";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_LZ ( select substr(shi,1,4) as 统计单位,林种,亚林种," +
                                    "sum(case when (llz between 1 and 5) and (dl='111'or dl='120') then round(" + strXJFielldName + ",2) else 0 end) as 活立木总蓄积量," +
                                    "sum (case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林sum面积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 成熟林蓄积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 过熟林面积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end ) as  竹林面积," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as  疏林面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end ) as 疏林蓄积," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林面积小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林面积," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林面积" +
                                    "  from tempTable_LZ" +
                                    "  where xjl<>0" +
                                    "  group by substr(shi,1,4),rollup(林种),rollup(亚林种）)";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_LZ ( select substr(sheng,1,2) as 统计单位,林种,亚林种," +
                                    "sum(case when (llz between 1 and 5) and (dl='111'or dl='120') then round(" + strXJFielldName + ",2) else 0 end) as 活立木总蓄积量," +
                                    "sum (case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林sum面积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 成熟林蓄积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 过熟林面积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end ) as  竹林面积," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as  疏林面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end ) as 疏林蓄积," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林面积小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林面积," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林面积" +
                                    "  from tempTable_LZ" +
                                    "  where xjl<>0" +
                                    "  group by substr(sheng,1,2),rollup(林种),rollup(亚林种）)";
                pWorkspace.ExecuteSQL(shengSQL);
                string LC = "insert into EcosTable_LZ ( select lc as 统计单位,林种,亚林种," +
                                    "sum(case when (llz between 1 and 5) and (dl='111'or dl='120') then round(" + strXJFielldName + ",2) else 0 end) as 活立木总蓄积量," +
                                    "sum (case when dl between '111' and '114' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林面积小计," +
                                    "sum(case when llz between 1 and 5 and(dl='111' or dl='112') then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 幼龄林面积," +
                                    "sum(case when LLZ=1 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 幼龄林蓄积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 中龄林sum面积," +
                                    "sum(case when LLZ=2 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 中龄林蓄积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 近熟林面积," +
                                    "sum(case when LLZ=3 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 近熟林蓄积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 成熟林面积," +
                                    "sum(case when LLZ=4 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 成熟林蓄积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + StatisticsFieldName + ",2) else 0 end) as 过熟林面积," +
                                    "sum(case when LLZ=5 and (DL='111' or DL='112') then round(" + strXJFielldName + ",2) else 0 end) as 过熟林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end ) as  竹林面积," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end ) as  疏林面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end ) as 疏林蓄积," +
                                    "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木林面积小计," +
                                    "sum(case when dl='131' then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林面积," +
                                    "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 其他灌木林面积" +
                                    "  from tempTable_LZ" +
                                    "  where xjl<>0 and lc<>' '" +
                                    "  group by lc,rollup(林种),rollup(亚林种）)";
                pWorkspace.ExecuteSQL(LC);
                Dictionary<string, string> dicLZ = dicGetFieldValue(pWorkspace, "林种字典");
                UpadateStatistictable(pWorkspace, dicLZ, "EcosTable_LZ", "亚林种");

            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 经济林统计
        //经济林统计
        private static bool XZQJJL_Statistic(IFeatureClass pFeatureClass, string ZSFieldName, string StatisticsFieldName)   
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
            DropTable(pWorkspace, "TempTable_JJL");
            DropTable(pWorkspace, "EcosTable_JJL");
            try
            {
                //创建临时表
                pWorkspace.ExecuteSQL("create table TempTable_JJL as select xian,sheng,shi,xiang,dl,lc,zysz,ll,lmqs,qi_yuan," + StatisticsFieldName + "," + ZSFieldName + " from " + tableName);
                // 更新临时表
                pWorkspace.ExecuteSQL("update TempTable_JJL set qi_yuan='人工' where qi_yuan like'1%'");
                pWorkspace.ExecuteSQL("update TempTable_JJL set qi_yuan='天然' where qi_yuan like'2%'");

                string xianSQL = "create table EcosTable_JJL as select xian as 统计单位,lmqs as 林木使用权," +
                                  " qi_yuan as 起源,zysz as 树种,"+
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
                                  "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
                                  "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
                                  "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
                                  "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
                                  "  from TempTable_JJL"  +
                                  "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704'"+
                                  "  group by xian ,rollup(lmqs),rollup(qi_yuan),rollup(zysz)";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  EcosTable_JJL ( select substr(xiang,1,8) as 统计单位,lmqs as 林木使用权," +
                                  " qi_yuan  as 起源,zysz as 树种," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
                                  "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
                                  "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
                                  "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
                                  "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
                                  "  from TempTable_JJL" +
                                  "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704'" +
                                  "  group by substr(xiang,1,8) ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into  EcosTable_JJL ( select substr(shi,1,4) as 统计单位,lmqs as 林木使用权," +
                                  " qi_yuan  as 起源,zysz as 树种," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
                                  "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
                                  "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
                                  "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
                                  "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
                                  "  from TempTable_JJL"  +
                                  "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704'" +
                                  "  group by substr(shi,1,4) ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into  EcosTable_JJL ( select substr(sheng,1,2) as 统计单位,lmqs as 林木使用权," +
                                  " qi_yuan  as 起源,zysz as 树种," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
                                  "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
                                  "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
                                  "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
                                  "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
                                  "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
                                  "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
                                  "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
                                  "  from TempTable_JJL"  +
                                  "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704'" +
                                  "  group by substr(sheng,1,2) ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(shengSQL);
                string LC = "insert into  EcosTable_JJL ( select lc as 统计单位,lmqs as 林木使用权," +
                                 " qi_yuan  as 起源,zysz as 树种," +
                                 "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
                                 "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
                                 "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
                                 "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
                                 "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
                                 "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
                                 "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木盛产期面积," +
                                 "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
                                 "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
                                 "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
                                 "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
                                 "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
                                 "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
                                 "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
                                 "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
                                 "  from TempTable_JJL"  +
                                 "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704' and lc<>' '" +
                                 "  group by lc ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(LC);
                DropTable(pWorkspace, "TempTable_JJL");
                pWorkspace.ExecuteSQL("alter table EcosTable_JJL modify 林木使用权 nvarchar2(10)");
                pWorkspace.ExecuteSQL("alter table EcosTable_JJL modify 树种 nvarchar2(10)");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权 ='国有' where 林木使用权='1'");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权='集体' where 林木使用权='2'");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权='个人' where 林木使用权='3'");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权='其它' where 林木使用权='9'");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权='所有' where 林木使用权='0'");
                pWorkspace.ExecuteSQL("update EcosTable_JJL set 林木使用权='其它' where 林木使用权='B'");
                Dictionary<string, string> dicSZ = dicGetFieldValue(pWorkspace, "树种字典");
                UpadateStatistictable(pWorkspace, dicSZ, "EcosTable_JJL", "树种");
            }
            catch (System.Exception ex)
            {
                DropTable(pWorkspace, "TempTable_JJL");
                return false;
            }
            return true;
        }
        //private static bool LCJJL_Statistic(IFeatureClass pFeatureClass, string ZSFieldName, string StatisticsFieldName)
        //{
        //    if (pFeatureClass == null)
        //    {
        //        return false;
        //    }
        //    IWorkspace pWorkspace = null;
        //    string tableName = "";
        //    try
        //    {
        //        pWorkspace = pFeatureClass.FeatureDataset.Workspace;
        //        tableName = (pFeatureClass as IDataset).Name;
        //    }
        //    catch (Exception ex)
        //    { }
        //    try
        //    {
        //        if (!ExistTable(pWorkspace, "EcosTable_JJL"))
        //        {
        //            string LCSQL = "create table EcosTable_JJL as select lc as 统计单位,lmqs as 林木使用权," +
        //                              "case when qi_yuan like '1%' then '人工' else  '天然' end as 起源,zysz as 树种," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
        //                              "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
        //                              "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
        //                              "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) end) as 乔木盛产期面积," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
        //                              "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
        //                              "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
        //                              "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
        //                              "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
        //                              "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
        //                              "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
        //                              "  from " + tableName +
        //                              "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704' and lc<>' '" +
        //                              "  group by lc ,rollup(lmqs),rollup(qi_yuan),rollup(zysz)";
        //            pWorkspace.ExecuteSQL(LCSQL);
        //        }
        //        else
        //        {
        //            string LC = "insert into  EcosTable_JJL ( select lc as 统计单位,lmqs as 林木使用权," +
        //                          "case when qi_yuan like '1%' then '人工' else  '天然' end as 起源,zysz as 树种," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll>0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木面积合计," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll>0 then " + ZSFieldName + " else 0 end) as 乔木株数合计," +
        //                          "sum(case when dl='114' and zysz like '7%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木产前期面积," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll <=3 then " + ZSFieldName + " else 0 end ) as 乔木产前期株数," +
        //                          "sum(case when dl='114' and zysz like '7%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木初产期面积," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll between 3 and 6 then " + ZSFieldName + " else 0 end ) as 乔木初产期株数," +
        //                          "sum(case when dl='114' and zysz like '7%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) end) as 乔木盛产期面积," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll between 6 and 15 then " + ZSFieldName + " else 0 end) as 乔木盛产期株数," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll >= 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木衰产期面积," +
        //                          "sum(case when dl='114' and zysz like'7%' and ll>=15 then " + ZSFieldName + " else 0 end) as 乔木衰产期株数," +
        //                          "sum(case when dl='114' and zysz like'9%' and ll >=0 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木合计," +
        //                          "sum(case when dl='114' and zysz like'9%' and ll<=3 then round(" + StatisticsFieldName + ",2) else 0 end) 灌木产前期面积," +
        //                          "sum(case when dl='114' and zysz like'9%' and ll between 3 and 6 then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木初产期面积," +
        //                          "sum(case when dl='114' and zysz like'9%' and ll between 6 and 15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木盛产期面积," +
        //                          "sum(case when dl='114' and zysz like'9%' and ll>=15 then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木衰产期面积" +
        //                          "  from " + tableName +
        //                          "  where lmqs<>' ' and qi_yuan <>' ' and zysz between '702' and '704' and lc<>' '" +
        //                          "  group by lc ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
        //            pWorkspace.ExecuteSQL(LC);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
        #region 灌木林统计
        private static bool XZQGML_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
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
            DropTable(pWorkspace, "temp_GM");
            DropTable(pWorkspace, "EcosTable_GML");
            try
            {
                //创建临时表
                pWorkspace.ExecuteSQL("create table temp_GM as select xian,xiang,shi,sheng," + StatisticsFieldName + ",qi_yuan,dl,gmgd,zysz,lmqs,lc from " + tableName);
                //更新临时表
                pWorkspace.ExecuteSQL("update temp_GM set qi_yuan='天然' where qi_yuan like'1%'");
                pWorkspace.ExecuteSQL("update temp_GM set qi_yuan='人工' where qi_yuan like'2%'");
                string xianSQL = "create table EcosTable_GML as select xian as 统计单位,lmqs as 使用权," +
                                  " qi_yuan as 起源,zysz as 优势树种," +
                                  "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                  "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
                                  "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
                                  "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
                                  "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
                                  "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
                                  "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
                                  "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
                                  "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
                                  "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
                                  "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
                                  "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
                                  "  from temp_GM" +
                                  "  where qi_yuan <>' ' and zysz in('120','150','130','170')" +
                                  "  group  by xian ,rollup(lmqs),rollup(qi_yuan),rollup(zysz)";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_GML ( select substr(xiang,1,8) as 统计单位,lmqs as 使用权," +
                                  " qi_yuan as 起源,zysz as 优势树种," +
                                  "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                  "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
                                  "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
                                  "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
                                  "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
                                  "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
                                  "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
                                  "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
                                  "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
                                  "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
                                  "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
                                  "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
                                  "  from temp_GM" +
                                  "  where qi_yuan <>' ' and zysz in('120','150','130','170')" +
                                  "  group  by substr(xiang,1,8) ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_GML ( select substr(shi,1,4) as 统计单位,lmqs as 使用权," +
                                  "qi_yuan as 起源,zysz as 优势树种," +
                                  "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                  "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
                                  "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
                                  "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
                                  "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
                                  "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
                                  "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
                                  "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
                                  "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
                                  "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
                                  "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
                                  "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
                                  "  from temp_GM" +
                                  "  where qi_yuan <>' ' and zysz in('120','150','130','170')" +
                                  "  group  by substr(shi,1,4),rollup(lmqs) ,rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_GML ( select substr(sheng,1,2) as 统计单位,lmqs as 使用权," +
                                  " qi_yuan as 起源,zysz as 优势树种," +
                                  "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                                  "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
                                  "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
                                  "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
                                  "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
                                  "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
                                  "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
                                  "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
                                  "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
                                  "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
                                  "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
                                  "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
                                  "  from temp_GM" +
                                  "  where qi_yuan <>' ' and zysz in('120','150','130','170')" +
                                  "  group  by substr(sheng,1,2) ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(shengSQL);
                string LC = "insert into EcosTable_GML (select lc as 统计单位,lmqs as 使用权" +
                              ",qi_yuan  as 起源,zysz as 优势树种," +
                              "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
                              "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
                              "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
                              "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
                              "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
                              "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
                              "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
                              "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
                              "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
                              "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
                              "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
                              "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
                              "  from temp_GM" +
                              "  where qi_yuan <>' ' and zysz in('120','150','130','170') and lc<>' '" +
                              "  group  by lc ,rollup(lmqs),rollup(qi_yuan),rollup(zysz))";
                pWorkspace.ExecuteSQL(LC);
                DropTable(pWorkspace, "temp_GM");
                pWorkspace.ExecuteSQL("alter table EcosTable_GML modify 使用权 nvarchar2(10)");
                pWorkspace.ExecuteSQL("alter table EcosTable_GML modify 优势树种 nvarchar2(10)");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权 ='国有' where 使用权='1'");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权='集体' where 使用权='2'");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权='个人' where 使用权='3'");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权='其它' where 使用权='9'");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权='所有' where 使用权='0'");
                pWorkspace.ExecuteSQL("update EcosTable_GML set 使用权='其它' where 使用权='B'");
                Dictionary <string ,string > dicSZ=dicGetFieldValue(pWorkspace ,"树种字典");
                UpadateStatistictable(pWorkspace, dicSZ, "EcosTable_GML", "优势树种");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //private static bool LCCML_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
        //{
        //    if (pFeatureClass == null)
        //    {
        //        return false;
        //    }
        //    IWorkspace pWorkspace = null;
        //    string tableName = "";
        //    try
        //    {
        //        pWorkspace = pFeatureClass.FeatureDataset.Workspace;
        //        tableName = (pFeatureClass as IDataset).Name;
        //    }
        //    catch (Exception ex)
        //    { }
        //    try
        //    {
        //        if (!ExistTable(pWorkspace, "EcosTable_GML"))
        //        {
        //            string LCSQL = "create table EcosTable_GML as select lc as 统计单位," +
        //                              "case when qi_yuan like'1%' then ' 天然' else '人工' end as 起源,zysz as 优势树种," +
        //                              "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
        //                              "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
        //                              "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
        //                              "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
        //                              "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
        //                              "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
        //                              "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
        //                              "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
        //                              "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
        //                              "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
        //                              "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
        //                              "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
        //                              "  from " + tableName +
        //                              "  where qi_yuan <>' ' and zysz in('120','150','130','170') and lc<>' '" +
        //                              "  group  by lc ,rollup(qi_yuan),rollup(zysz)";
        //            pWorkspace.ExecuteSQL(LCSQL);
        //        }
        //        else
        //        {
        //            string LC = "insert into EcosTable_GML (select lc as 统计单位," +
        //                              "case when qi_yuan like'1%' then ' 天然' else '人工' end as 起源,zysz as 优势树种," +
        //                              "sum(case when dl between '131' and '132' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end ) as 合计," +
        //                              "sum(case when dl between '131' and '132' and  gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end)  疏合计," +
        //                              "sum(case when dl between '131' and '132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 中合计," +
        //                              "sum(case when dl between '131' and '132' and gmgd >70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 密合计," +
        //                              "sum(case when dl='131' and gmgd>=30 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林小计," +
        //                              "sum(case when dl='131' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林疏," +
        //                              "sum(case when dl='131'and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林中," +
        //                              "sum(case when dl='131' and gmgd>70 then round(" + StatisticsFieldName + ",2) else 0 end) as 国家特别规定灌木林密," +
        //                              "sum(case when dl='132' and gmgd >= 30 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林小计," +
        //                              "sum(case when dl='132' and gmgd between 30 and 50 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林疏," +
        //                              "sum(case when dl='132' and gmgd between 50 and 70 then round(" + StatisticsFieldName + ",2) else 0 end) as 其它灌木林中," +
        //                              "sum(case when dl='132' and gmgd >= 70 then round(" + StatisticsFieldName + ",2) else 0 end ) as 其它灌木林密" +
        //                              "  from " + tableName +
        //                              "  where qi_yuan <>' ' and zysz in('120','150','130','170') and lc<>' '" +
        //                              "  group  by lc ,rollup(qi_yuan),rollup(zysz))";
        //            pWorkspace.ExecuteSQL(LC);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
        #region 各类土地面积统计
        private static bool XZQGLTDMJ_Statistic(IFeatureClass pFeatureClass, string StatisticsFieldName)
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
            DropTable(pWorkspace, "tempTable_GLTDMJ");
            DropTable(pWorkspace, "EcosTable_GLTDMJ");
            try
            {
                //创建临时表
                pWorkspace.ExecuteSQL("create table tempTable_GLTDMJ as select xian,xiang,shi,sheng,ld_qs,lz,dl,lc,sen_lin_lb,"+StatisticsFieldName+" from " + tableName + "");
                pWorkspace.ExecuteSQL("alter table tempTable_GLTDMJ  modify ld_qs NVARCHAR2(10)");
                pWorkspace.ExecuteSQL("update tempTable_GLTDMJ set ld_qs='国有林地' where ld_qs like'1%'");
                pWorkspace.ExecuteSQL("update tempTable_GLTDMJ set ld_qs='集体林地' where ld_qs like'2%' and ld_qs<>'20'");
                pWorkspace.ExecuteSQL("update tempTable_GLTDMJ set ld_qs='其他林地' where ld_qs='20'");
                pWorkspace.ExecuteSQL("update tempTable_GLTDMJ set  sen_lin_lb='生态公益林' where sen_lin_lb like '1%'");
                pWorkspace.ExecuteSQL("update tempTable_GLTDMJ set  sen_lin_lb='商品林' where sen_lin_lb like '2%'");
                //统计SQL语句
                string xianSQL = "create table EcosTable_GLTDMJ as select "+
                                "xian as 统计单位,"+
                                "ld_qs as 土地使用权,sen_lin_lb as 森林类别," +
                                "sum(case when lz<>' 'then round(" + StatisticsFieldName + ",2) else 0 end) as 总面积," +
                                "sum(case when dl like '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 林地合计," +
                                "sum(case when dl between '111' and '113' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地小计," +
                                "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 混交林," +
                                "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林," +
                                "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as 疏林地," +
                                "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木林地小计," +
                                "sum(case when dl='131' then round(" + StatisticsFieldName + ",2)  else 0 end) as 国家特别规定灌木林," +
                                "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木经济林," +
                                "sum(case when dl='133' then round(" + StatisticsFieldName + ",2)  else 0 end) as 其它灌木林," +
                                "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150'then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl between '171' and'174' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end ) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl like '2%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 非林地," +
                                "sum(" + StatisticsFieldName + ") as 森林覆盖率,sum(" + StatisticsFieldName + ") as 林木绿化率" +
                                "  from tempTable_GLTDMJ" +
                                "  where dl<>' ' and sen_lin_lb<>' '" +
                                "  group by xian,rollup(ld_qs),rollup(sen_lin_lb)";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_GLTDMJ ( select " +
                                "substr(xiang,1,8) as 统计单位," +
                                "ld_qs as 土地使用权,sen_lin_lb as 森林类别," +
                                "sum(case when lz<>' 'then round(" + StatisticsFieldName + ",2) else 0 end) as 总面积," +
                                "sum(case when dl like '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 林地合计," +
                                "sum(case when dl between '111' and '113' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地小计," +
                                "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 混交林," +
                                "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林," +
                                "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as 疏林地," +
                                "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木林地小计," +
                                "sum(case when dl='131' then round(" + StatisticsFieldName + ",2)  else 0 end) as 国家特别规定灌木林," +
                                "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木经济林," +
                                "sum(case when dl='133' then round(" + StatisticsFieldName + ",2)  else 0 end) as 其它灌木林," +
                                "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150'then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl between '171' and'174' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end ) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl like '2%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 非林地," +
                                "sum(" + StatisticsFieldName + ") as 森林覆盖率,sum(" + StatisticsFieldName + ") as 林木绿化率" +
                                "  from tempTable_GLTDMJ" +
                                "  where dl<>' 'and sen_lin_lb<>' '" +
                                "  group by substr(xiang,1,8),rollup(ld_qs),rollup(sen_lin_lb))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_GLTDMJ ( select " +
                                "substr(shi,1,4) as 统计单位," +
                                "ld_qs as 土地使用权,sen_lin_lb as 森林类别," +
                                "sum(case when lz<>' 'then round(" + StatisticsFieldName + ",2) else 0 end) as 总面积," +
                                "sum(case when dl like '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 林地合计," +
                                "sum(case when dl between '111' and '113' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地小计," +
                                "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 混交林," +
                                "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林," +
                                "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as 疏林地," +
                                "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木林地小计," +
                                "sum(case when dl='131' then round(" + StatisticsFieldName + ",2)  else 0 end) as 国家特别规定灌木林," +
                                "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木经济林," +
                                "sum(case when dl='133' then round(" + StatisticsFieldName + ",2)  else 0 end) as 其它灌木林," +
                                "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150'then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl between '171' and'174' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end ) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl like '2%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 非林地," +
                                "sum(" + StatisticsFieldName + ") as 森林覆盖率,sum(" + StatisticsFieldName + ") as 林木绿化率" +
                                "  from tempTable_GLTDMJ" +
                                "  where dl<>' 'and sen_lin_lb<>' '" +
                                "  group by substr(shi,1,4),rollup(ld_qs),rollup(sen_lin_lb))";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_GLTDMJ ( select " +
                                "substr(sheng,1,2) as 统计单位," +
                                "ld_qs as 土地使用权,sen_lin_lb as 森林类别," +
                                "sum(case when lz<>' 'then round(" + StatisticsFieldName + ",2) else 0 end) as 总面积," +
                                "sum(case when dl like '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 林地合计," +
                                "sum(case when dl between '111' and '113' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地小计," +
                                "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 混交林," +
                                "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林," +
                                "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as 疏林地," +
                                "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木林地小计," +
                                "sum(case when dl='131' then round(" + StatisticsFieldName + ",2)  else 0 end) as 国家特别规定灌木林," +
                                "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木经济林," +
                                "sum(case when dl='133' then round(" + StatisticsFieldName + ",2)  else 0 end) as 其它灌木林," +
                                "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150'then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl between '171' and'174' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end ) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl like '2%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 非林地," +
                                "sum(" + StatisticsFieldName + ") as 森林覆盖率,sum(" + StatisticsFieldName + ") as 林木绿化率" +
                                "  from tempTable_GLTDMJ" +
                                "  where dl<>' 'and sen_lin_lb<>' '" +
                                "  group by substr(sheng,1,2),rollup(ld_qs),rollup(sen_lin_lb))";
                pWorkspace.ExecuteSQL(shengSQL);
                string LCSQL = "insert into EcosTable_GLTDMJ ( select " +
                                "lc as 统计单位," +
                                "ld_qs as 土地使用权,sen_lin_lb as 森林类别," +
                                "sum(case when lz<>' 'then round(" + StatisticsFieldName + ",2) else 0 end) as 总面积," +
                                "sum(case when dl like '1%' then round(" + StatisticsFieldName + ",2) else 0 end) as 林地合计," +
                                "sum(case when dl between '111' and '113' then round(" + StatisticsFieldName + ",2) else 0 end) as 有林地小计," +
                                "sum(case when dl between '111' and '112' then round(" + StatisticsFieldName + ",2) else 0 end) as 乔木林小计," +
                                "sum(case when dl='111' then round(" + StatisticsFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end ) as 混交林," +
                                "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林," +
                                "sum(case when dl='113' then round(" + StatisticsFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as 疏林地," +
                                "sum(case when dl between '131' and '132' then round(" + StatisticsFieldName + ",2) else 0 end ) as 灌木林地小计," +
                                "sum(case when dl='131' then round(" + StatisticsFieldName + ",2)  else 0 end) as 国家特别规定灌木林," +
                                "sum(case when dl='132' then round(" + StatisticsFieldName + ",2) else 0 end) as 灌木经济林," +
                                "sum(case when dl='133' then round(" + StatisticsFieldName + ",2)  else 0 end) as 其它灌木林," +
                                "sum(case when dl between '141' and '142' then round(" + StatisticsFieldName + ",2) else 0 end) as 未成林造林地小计," +
                                "sum(case when dl='141' then round(" + StatisticsFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + StatisticsFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150'then round(" + StatisticsFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl between '161' and '163' then round(" + StatisticsFieldName + ",2) else 0 end) as 无立木林地小计," +
                                "sum(case when dl='161' then round(" + StatisticsFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + StatisticsFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl between '171' and'174' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林地小计," +
                                "sum(case when dl='171' then round(" + StatisticsFieldName + ",2) else 0 end ) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + StatisticsFieldName + ",2) else 0 end) as 宜林沙荒地," +
                                "sum(case when dl='173' then round(" + StatisticsFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + StatisticsFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + StatisticsFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl like '2%' then round(" + StatisticsFieldName + ",2) else 0 end ) as 非林地," +
                                "sum(" + StatisticsFieldName + ") as 森林覆盖率,sum(" + StatisticsFieldName + ") as 林木绿化率" +
                                "  from tempTable_GLTDMJ" +
                                "  where dl<>' 'and sen_lin_lb<>' ' and lc<>' '" +
                                "  group by lc,rollup(ld_qs),rollup(sen_lin_lb))";
                pWorkspace.ExecuteSQL(LCSQL);
                DropTable(pWorkspace, "tempTable_GLTDMJ");
                //计算森林覆盖率和林业覆盖率
                pWorkspace.ExecuteSQL("update EcosTable_GLTDMJ set 森林覆盖率=0 ");
                pWorkspace.ExecuteSQL("update EcosTable_GLTDMJ set 林木绿化率=0 ");
                pWorkspace.ExecuteSQL("update EcosTable_GLTDMJ set 森林覆盖率=round(((乔木林小计+国家特别规定灌木林)/总面积)*100 ,2) where 土地使用权 is null and 森林类别 is null");
                pWorkspace.ExecuteSQL("update EcosTable_GLTDMJ set 林木绿化率=round(((乔木林小计+灌木林地小计)/总面积)*100,2) where 土地使用权 is null and 森林类别 is null");

              //  UpdateTotal(pWorkspace, "土地使用权", "合计", "EcosTable_GLTDMJ");
               // UpdateTotal(pWorkspace, "森林类别", "小计", "EcosTable_GLTDMJ");
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region 各类森林、林木面积蓄积统计
        private static bool XZQGLSLMJ_Statistic(IFeatureClass pFeatureClass, string strXJFielldName,string ZSFieldName, string StatisticsFieldName)
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
            DropTable(pWorkspace, "EcosTable_GLSLMJ");
            try
            {
                string xianSQL = "create table EcosTable_GLSLMJ as select  xian as 统计单位,lmqs as 林木使用权," +
                                    "sum (case when (dl= '142'or dl='141'or dl='120'or dl='114' or dl='112'or dl='111') then round(" + strXJFielldName + ",2) else 0 end ) as 活立木总蓄积量," +
                                    "sum( case when dl between '111' and '114' then  round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积合计," +
                                    "sum(case when dl between '111' and '112' then  round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木林面积小计," +
                                    "sum(case when dl between '111' and '112' then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when dl ='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林面积," +
                                    "sum( case when dl='111' then round(" + strXJFielldName + ",2) else 0 end) as 纯林蓄积," +
                                    "sum( case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 混交林面积," +
                                    "sum(case when dl='112' then round(" + strXJFielldName + ",2) else 0 end) as 混交林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='114' then round (" + strXJFielldName + ",2) else 0 end) as  经济林蓄积," +
                                    "sum(case when dl='113' then  round(" + StatisticsFieldName + ",2) else 0 end) as 竹林蓄积," +
                                    "sum(case when dl='113' then " + ZSFieldName + " else 0 end)/10000 as 竹林株数," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as  疏林地面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end) as 疏林地蓄积," +
                                    "sum(case when dl='141' then  " + ZSFieldName + "  else 0 end)/10000 as 四旁树株数," +
                                    "sum(case when dl='141' then round(" + strXJFielldName + ",2) else 0 end) as 四旁树蓄积," +
                                    "sum(case when dl='142' then  " + ZSFieldName + "  else 0 end)/10000 as 散生木株数," +
                                    "sum(case when dl='142' then round(" + strXJFielldName + ",2) else 0 end) as 散生木蓄积" +
                                    " from " + tableName +
                                    " where lmqs<>' '" +
                                    " group by xian,rollup(lmqs)";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into EcosTable_GLSLMJ ( select  substr(xiang,1,8) as 统计单位,lmqs as 林木使用权," +
                                    "sum (case when (dl= '142'or dl='141'or dl='120'or dl='114' or dl='112'or dl='111') then round(" + strXJFielldName + ",2) else 0 end ) as 活立木总蓄积量," +
                                    "sum( case when dl between '111' and '114' then  round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积合计," +
                                    "sum(case when dl between '111' and '112' then  round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木林面积小计," +
                                    "sum(case when dl between '111' and '112' then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when dl ='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林面积," +
                                    "sum( case when dl='111' then round(" + strXJFielldName + ",2) else 0 end) as 纯林蓄积," +
                                    "sum( case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 混交林面积," +
                                    "sum(case when dl='112' then round(" + strXJFielldName + ",2) else 0 end) as 混交林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='114' then round (" + strXJFielldName + ",2) else 0 end) as  经济林蓄积," +
                                    "sum(case when dl='113' then  round(" + StatisticsFieldName + ",2) else 0 end) as 竹林蓄积," +
                                    "sum(case when dl='113' then " + ZSFieldName + " else 0 end)/10000 as 竹林株数," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as  疏林地面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end) as 疏林地蓄积," +
                                    "sum(case when dl='141' then  " + ZSFieldName + "  else 0 end)/10000 as 四旁树株数," +
                                    "sum(case when dl='141' then round(" + strXJFielldName + ",2) else 0 end) as 四旁树蓄积," +
                                    "sum(case when dl='142' then  " + ZSFieldName + "  else 0 end)/10000 as 散生木株数," +
                                    "sum(case when dl='142' then round(" + strXJFielldName + ",2) else 0 end) as 散生木蓄积" +
                                    " from " + tableName +
                                    " where lmqs<>' '" +
                                    " group by substr(xiang,1,8),rollup(lmqs))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string shiSQL = "insert into EcosTable_GLSLMJ ( select  substr(shi,1,4) as 统计单位,lmqs as 林木使用权," +
                                    "sum (case when (dl= '142'or dl='141'or dl='120'or dl='114' or dl='112'or dl='111') then round(" + strXJFielldName + ",2) else 0 end ) as 活立木总蓄积量," +
                                    "sum( case when dl between '111' and '114' then  round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积合计," +
                                    "sum(case when dl between '111' and '112' then  round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木林面积小计," +
                                    "sum(case when dl between '111' and '112' then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when dl ='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林面积," +
                                    "sum( case when dl='111' then round(" + strXJFielldName + ",2) else 0 end) as 纯林蓄积," +
                                    "sum( case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 混交林面积," +
                                    "sum(case when dl='112' then round(" + strXJFielldName + ",2) else 0 end) as 混交林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='114' then round (" + strXJFielldName + ",2) else 0 end) as  经济林蓄积," +
                                    "sum(case when dl='113' then  round(" + StatisticsFieldName + ",2) else 0 end) as 竹林蓄积," +
                                    "sum(case when dl='113' then " + ZSFieldName + " else 0 end)/10000 as 竹林株数," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as  疏林地面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end) as 疏林地蓄积," +
                                    "sum(case when dl='141' then  " + ZSFieldName + "  else 0 end)/10000 as 四旁树株数," +
                                    "sum(case when dl='141' then round(" + strXJFielldName + ",2) else 0 end) as 四旁树蓄积," +
                                    "sum(case when dl='142' then  " + ZSFieldName + "  else 0 end)/10000 as 散生木株数," +
                                    "sum(case when dl='142' then round(" + strXJFielldName + ",2) else 0 end) as 散生木蓄积" +
                                    " from " + tableName +
                                    " where lmqs<>' '" +
                                    " group by substr(shi,1,4),rollup(lmqs))";
                pWorkspace.ExecuteSQL(shiSQL);
                string shengSQL = "insert into EcosTable_GLSLMJ ( select  substr(sheng,1,2) as 统计单位,lmqs as 林木使用权," +
                                    "sum (case when (dl= '142'or dl='141'or dl='120'or dl='114' or dl='112'or dl='111') then round(" + strXJFielldName + ",2) else 0 end ) as 活立木总蓄积量," +
                                    "sum( case when dl between '111' and '114' then  round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积合计," +
                                    "sum(case when dl between '111' and '112' then  round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木林面积小计," +
                                    "sum(case when dl between '111' and '112' then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when dl ='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林面积," +
                                    "sum( case when dl='111' then round(" + strXJFielldName + ",2) else 0 end) as 纯林蓄积," +
                                    "sum( case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 混交林面积," +
                                    "sum(case when dl='112' then round(" + strXJFielldName + ",2) else 0 end) as 混交林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='114' then round (" + strXJFielldName + ",2) else 0 end) as  经济林蓄积," +
                                    "sum(case when dl='113' then  round(" + StatisticsFieldName + ",2) else 0 end) as 竹林蓄积," +
                                    "sum(case when dl='113' then " + ZSFieldName + " else 0 end)/10000 as 竹林株数," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as  疏林地面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end) as 疏林地蓄积," +
                                    "sum(case when dl='141' then  " + ZSFieldName + "  else 0 end)/10000 as 四旁树株数," +
                                    "sum(case when dl='141' then round(" + strXJFielldName + ",2) else 0 end) as 四旁树蓄积," +
                                    "sum(case when dl='142' then  " + ZSFieldName + "  else 0 end)/10000 as 散生木株数," +
                                    "sum(case when dl='142' then round(" + strXJFielldName + ",2) else 0 end) as 散生木蓄积" +
                                    " from " + tableName +
                                    " where lmqs<>' '" +
                                    " group by substr(sheng,1,2),rollup(lmqs))";
                pWorkspace.ExecuteSQL(shengSQL);
                string lcSQL = "insert into EcosTable_GLSLMJ ( select  lc as 统计单位,lmqs as 林木使用权," +
                                    "sum (case when (dl= '142'or dl='141'or dl='120'or dl='114' or dl='112'or dl='111') then round(" + strXJFielldName + ",2) else 0 end ) as 活立木总蓄积量," +
                                    "sum( case when dl between '111' and '114' then  round(" + StatisticsFieldName + ",2) else 0 end) as 有林地面积合计," +
                                    "sum(case when dl between '111' and '112' then  round(" + StatisticsFieldName + ",2) else 0 end ) as 乔木林面积小计," +
                                    "sum(case when dl between '111' and '112' then round(" + strXJFielldName + ",2) else 0 end) as 乔木林蓄积小计," +
                                    "sum(case when dl ='111' then round(" + StatisticsFieldName + ",2) else 0 end ) as 纯林面积," +
                                    "sum( case when dl='111' then round(" + strXJFielldName + ",2) else 0 end) as 纯林蓄积," +
                                    "sum( case when dl='112' then round(" + StatisticsFieldName + ",2) else 0 end) as 混交林面积," +
                                    "sum(case when dl='112' then round(" + strXJFielldName + ",2) else 0 end) as 混交林蓄积," +
                                    "sum(case when dl='114' then round(" + StatisticsFieldName + ",2) else 0 end) as 经济林面积," +
                                    "sum(case when dl='114' then round (" + strXJFielldName + ",2) else 0 end) as  经济林蓄积," +
                                    "sum(case when dl='113' then  round(" + StatisticsFieldName + ",2) else 0 end) as 竹林蓄积," +
                                    "sum(case when dl='113' then " + ZSFieldName + " else 0 end)/10000 as 竹林株数," +
                                    "sum(case when dl='120' then round(" + StatisticsFieldName + ",2) else 0 end) as  疏林地面积," +
                                    "sum(case when dl='120' then round(" + strXJFielldName + ",2) else 0 end) as 疏林地蓄积," +
                                    "sum(case when dl='141' then  " + ZSFieldName + "  else 0 end)/10000 as 四旁树株数," +
                                    "sum(case when dl='141' then round(" + strXJFielldName + ",2) else 0 end) as 四旁树蓄积," +
                                    "sum(case when dl='142' then  " + ZSFieldName + "  else 0 end)/10000 as 散生木株数," +
                                    "sum(case when dl='142' then round(" + strXJFielldName + ",2) else 0 end) as 散生木蓄积" +
                                    " from " + tableName +
                                    " where lmqs<>' '" +
                                    " group by lc,rollup(lmqs))";
                pWorkspace.ExecuteSQL(lcSQL);
                //更新表
                pWorkspace.ExecuteSQL("alter table EcosTable_GLSLMJ modify 林木使用权 nvarchar2(10)");
                pWorkspace.ExecuteSQL("update EcosTable_GLSLMJ set 林木使用权 ='国有' where 林木使用权='1'");
                pWorkspace.ExecuteSQL("update EcosTable_GLSLMJ set 林木使用权='集体' where 林木使用权='2'");
                pWorkspace.ExecuteSQL("update EcosTable_GLSLMJ set 林木使用权='个人' where 林木使用权='3'");
                pWorkspace.ExecuteSQL("update EcosTable_GLSLMJ set 林木使用权='其它' where 林木使用权='9'");
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion
        //数据更新
        private static bool UpdateStatistictable(IWorkspace pWorkspace, string tableName, string FieldName, string newValue,string oldValue)
        {
            if (pWorkspace == null)
                return false;
            if (tableName == "") return false;
            try
            {
                string strSQL = "update " + tableName + " set " + FieldName + "='" + newValue + "' where  " + FieldName + "='" + oldValue + "'";
                pWorkspace.ExecuteSQL(strSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //将字段汉化
        private static bool UpadateStatistictable(IWorkspace pWorkspace, Dictionary<string, string> dicField, string tableName, string FieldName)
        {
            bool flag = false;
            if (pWorkspace == null)
                return false;
            if (tableName == "") return false;
            if (dicField == null || dicField.Count == 0) return false;
            foreach (string key in dicField.Keys)
            {
                flag = UpdateStatistictable(pWorkspace, tableName, FieldName, dicField[key], key);
            }
            return flag;
        }
        //汉化合计
        private static bool UpdateTotal(IWorkspace pWorkspace, string FieldName, string newvalue,string tableName)
        {
            if (pWorkspace == null || tableName == "" || FieldName == "")
            {
                return false;
            }
            try
            {
                pWorkspace.ExecuteSQL("update "+tableName +" set "+FieldName +"='"+newvalue+"' where "+FieldName +" is null");
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
        private static Dictionary<string, string> dicGetFieldValue(IWorkspace pWorkspace,string taleName)
        {
            if (pWorkspace == null) return null;
            Dictionary<string, string> newdic = new Dictionary<string, string>();
            try
            {
                ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable(taleName);
                ICursor pCursor = pTable.Search(null, false);
                IRow pRow = pCursor.NextRow();
                int codeIndex=0;
                int NameIndex=0;
                if(pRow !=null)
                {
                    codeIndex=pRow .Fields .FindField ("Code");
                    NameIndex =pRow .Fields .FindField ("Name");
                }
                while (pRow != null)
                {
                    newdic.Add(pRow .get_Value (codeIndex).ToString (),pRow .get_Value (NameIndex).ToString ());
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                
            }
            catch(Exception ex)
            {
                return null;
            }
            return newdic;
        }
        //进行各类信息统计 ygc 2013-01-16
        public static void DoReportStatistic(IFeatureClass pFeatureClass, string strMjFieldName,string strxjlFieldName,SysCommon.CProgress pProgress)
        {
                //保护信息统计
            pProgress.SetProgress(1, "进行保护信息统计");
            bool bRes = false;
            bRes = BHXX_Statistic(pFeatureClass, strMjFieldName);
            //进行地类信息统计
            pProgress.SetProgress(2, "进行地类息统计");
            bRes = DLXX_Statistic(pFeatureClass, strMjFieldName);
            //工程类别信息
            pProgress.SetProgress(3, "进行工程类别信息统计");
            bRes = GCLBXX_Statistic(pFeatureClass, strMjFieldName);
            //基本信息统计
            pProgress.SetProgress(4, "进行基本信息统计");
            bRes = JBXX_Statistic(pFeatureClass, strMjFieldName,strxjlFieldName);
            //林地结构信息统计
            pProgress.SetProgress(5, "进行林地结构信息统计");
            bRes = LDJGXX_Statistic(pFeatureClass, strMjFieldName);
            //林种信息统计
            pProgress.SetProgress(6, "进行林种信息统计");
            bRes = LZXX_Statistic(pFeatureClass, strMjFieldName);
            //起源信息统计
            pProgress.SetProgress(7, "进行起源信息统计");
            bRes = QYXX_Statistic(pFeatureClass, strMjFieldName);
            //权属信息统计
            pProgress.SetProgress(8, "进行权属信息统计");
            bRes = QSXX_Statistic(pFeatureClass, strMjFieldName);
            //灾害等级
            pProgress.SetProgress(9, "进行灾害等级信息统计");
            bRes = ZHDJ_Statistic(pFeatureClass, strMjFieldName);
            //灾害类型
            pProgress.SetProgress(10, "进行灾害类型信息统计");
            bRes = ZHLX_Statistic(pFeatureClass, strMjFieldName);
            //质量等级信息统计
            pProgress.SetProgress(11, "进行质量等级信息统计");
            bRes = ZLDJ_Statistic(pFeatureClass, strMjFieldName);
            //主要树种信息统计
            pProgress.SetProgress(12, "进行主要树种信息统计");
            bRes = ZYSZ_Statistic(pFeatureClass, strMjFieldName);
        }
        //保护信息统计 ygc 2013-01-16
        private static bool BHXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_BHXX");
            //通过SQL语句进行统计
            try
            {
                string cunSQL = "create table DATA_BHXX as select substr(cun,1,10) as 统计单位,"+
                                    "sum(case when bhdj <>' ' then round("+strMjFieldName+",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from  "+tableName +
                                    " group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into DATA_BHXX (select substr(sheng,1,2) as 统计单位," +
                                    "sum(case when bhdj <>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from "+tableName  +
                                    " group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into DATA_BHXX (select substr(shi,1,4) as 统计单位," +
                                    "sum(case when bhdj <>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from  " + tableName +
                                    " group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into DATA_BHXX (select substr(xian,1,6) as 统计单位," +
                                    "sum(case when bhdj <>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from " + tableName +
                                    " group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into DATA_BHXX (select substr(xiang,1,8) as 统计单位," +
                                    "sum(case when bhdj <>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from " + tableName +
                                    " group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string LCSQL = "insert into DATA_BHXX (select lc as 统计单位," +
                                    "sum(case when bhdj <>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when bhdj='1' then round(" + strMjFieldName + ",2) else 0 end) as 特殊," +
                                    "sum(case when bhdj='2' then round(" + strMjFieldName + ",2) else 0 end) as 重点," +
                                    "sum(case when bhdj='3' then round(" + strMjFieldName + ",2) else 0 end) as 一般  " +
                                    " from " + tableName +
                                    " group by lc)";
                pWorkspace.ExecuteSQL(LCSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //地类信息统计 ygc 2013-01-16
        private static bool DLXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_DLXX");
            try
            {
                string cunSQL = "create table DATA_DLXX as select substr(cun,1,10) as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地  " +
                                "  from " + tableName +
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into DATA_DLXX (select substr(sheng,1,2) as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into DATA_DLXX (select substr(shi,1,4) as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into DATA_DLXX (select substr(xian,1,6) as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into DATA_DLXX (select substr(xiang,1,8) as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into DATA_DLXX (select lc as 统计单位," +
                                "sum( case when dl<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when dl ='111' then round(" + strMjFieldName + ",2) else 0 end) as 纯林," +
                                "sum(case when dl='112' then round(" + strMjFieldName + ",2) else 0 end) as 混交林," +
                                "sum(case when dl ='113' then round(" + strMjFieldName + ",2) else 0 end) as 竹林," +
                                "sum(case when dl ='114' then round(" + strMjFieldName + ",2) else 0 end) as 经济林," +
                                "sum( case when dl='120' then round(" + strMjFieldName + ",2) else 0 end) as 疏林地," +
                                "sum( case when dl='131' then round(" + strMjFieldName + ",2) else 0 end) as 国家特别规定的灌木林地," +
                                "sum( case when dl='132' then round(" + strMjFieldName + ",2) else 0 end) as 其他灌木林地," +
                                "sum(case when dl='141' then round (" + strMjFieldName + ",2) else 0 end) as 人工造林未成林地," +
                                "sum(case when dl='142' then round(" + strMjFieldName + ",2) else 0 end) as 封育未成林地," +
                                "sum(case when dl='150' then round(" + strMjFieldName + ",2) else 0 end) as 苗圃地," +
                                "sum(case when dl='161' then round(" + strMjFieldName + ",2) else 0 end) as 采伐迹地," +
                                "sum(case when dl='162' then round(" + strMjFieldName + ",2) else 0 end) as 火烧迹地," +
                                "sum(case when dl='163' then round(" + strMjFieldName + ",2) else 0 end) as 其它无立木林地," +
                                "sum(case when dl='171' then round(" + strMjFieldName + ",2) else 0 end) as 宜林荒山荒地," +
                                "sum(case when dl='172' then round(" + strMjFieldName + ",2) else 0 end) as  宜林沙荒地," +
                                "sum(case when dl='173' then round(" + strMjFieldName + ",2) else 0 end) as 其它宜林地," +
                                "sum(case when dl='174' then round(" + strMjFieldName + ",2) else 0 end) as 退耕地," +
                                "sum(case when dl='180' then round(" + strMjFieldName + ",2) else 0 end) as 辅助生产林地," +
                                "sum(case when dl='211' then round(" + strMjFieldName + ",2) else 0 end) as 耕地," +
                                "sum(case when dl='212' then round(" + strMjFieldName + ",2) else 0 end) as 其它农用林地," +
                                "sum(case when dl='220' then round(" + strMjFieldName + ",2) else 0 end) as 牧草地," +
                                "sum(case when dl='221' then round(" + strMjFieldName + ",2) else 0 end) as 天然草地," +
                                "sum(case when dl='222' then round(" + strMjFieldName + ",2) else 0 end) as 改良草地," +
                                "sum(case when dl='223' then round(" + strMjFieldName + ",2) else 0 end) as 人工草地," +
                                "sum(case when dl='231' then round(" + strMjFieldName + ",2) else 0 end) as 河流," +
                                "sum(case when dl='232' then round(" + strMjFieldName + ",2) else 0 end) as 湖泊," +
                                "sum(case when dl='233' then round(" + strMjFieldName + ",2) else 0 end) as 水库," +
                                "sum(case when dl='234' then round(" + strMjFieldName + ",2) else 0 end) as 池塘," +
                                "sum(case when dl='241' then round(" + strMjFieldName + ",2) else 0 end) as 荒草地," +
                                "sum(case when dl='242' then round(" + strMjFieldName + ",2) else 0 end) as 盐碱地," +
                                "sum(case when dl='243' then round(" + strMjFieldName + ",2) else 0 end) as 沙地," +
                                "sum(case when dl='244' then round(" + strMjFieldName + ",2) else 0 end) as 裸土地," +
                                "sum( case when dl='245' then round(" + strMjFieldName + ",2) else 0 end) as 裸岩石砾地," +
                                "sum(case when dl='246' then round(" + strMjFieldName + ",2) else 0 end) as 滩涂," +
                                "sum(case when dl='247' then round(" + strMjFieldName + ",2) else 0 end) as 其它未利用地," +
                                "sum(case when dl='251' then round(" + strMjFieldName + ",2) else 0 end) as 工矿建设用地," +
                                "sum(case when dl='252' then round(" + strMjFieldName + ",2) else 0 end) as 城乡居民点建设用地," +
                                "sum(case when dl='253' then round(" + strMjFieldName + ",2) else 0 end) as 交通建设用地," +
                                "sum( case when dl='254' then round(" + strMjFieldName + ",2) else 0 end) as  其它用地" +
                                "  from " + tableName +
                                "  group by lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //工程类别信息统计 ygc 2013-01-17
        private static bool GCLBXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_GCLB");
            try
            {
                string cunSQL = "create table DATA_GCLB as select substr(cun,1,10) as 统计单位," +
                                    "sum(case when gclb<>' ' then round("+strMjFieldName+",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from "+tableName +
                                    "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into DATA_GCLB ( select substr(sheng,1,2) as 统计单位," +
                                    "sum(case when gclb<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from " + tableName +
                                    "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into DATA_GCLB ( select substr(shi,1,4) as 统计单位," +
                                    "sum(case when gclb<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from " + tableName +
                                    "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into DATA_GCLB ( select substr(xian,1,6) as 统计单位," +
                                    "sum(case when gclb<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from " + tableName +
                                    "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into DATA_GCLB ( select substr(xiang,1,8) as 统计单位," +
                                    "sum(case when gclb<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from " + tableName +
                                    "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into DATA_GCLB ( select lc as 统计单位," +
                                    "sum(case when gclb<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                    "sum(case when gclb='12' then round(" + strMjFieldName + ",2) else 0 end) as 天然林保护工程," +
                                    "sum(case when gclb='21' then round(" + strMjFieldName + ",2) else 0 end) as 三北防护林工程," +
                                    "sum(case when gclb='26' then round(" + strMjFieldName + ",2) else 0 end) as 太行山绿化工程," +
                                    "sum(case when gclb='27' then round(" + strMjFieldName + ",2) else 0 end) as 平原绿化工程," +
                                    "sum(case when gclb='30'  then round(" + strMjFieldName + ",2) else 0 end) as 退耕还林工程," +
                                    "sum(case when gclb='40' then round(" + strMjFieldName + ",2) else 0 end) as 京津风沙源治理工程," +
                                    "sum(case when gclb='51' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物国家级保护区," +
                                    "sum(case when gclb='52' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物地方级保护区," +
                                    "sum(case when gclb='53' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物市方级保护区," +
                                    "sum(case when gclb='54' then round(" + strMjFieldName + ",2) else 0 end) as 野生动植物县方级保护区," +
                                    "sum(case when gclb='60' then round(" + strMjFieldName + ",2) else 0 end) as 速生丰产林基地建设工程," +
                                    "sum(case when gclb='61' then round(" + strMjFieldName + ",2) else 0 end) as 省级通道绿化工程," +
                                    "sum(case when gclb='62' then round(" + strMjFieldName + ",2) else 0 end) as 省级平原绿化工程," +
                                    "sum(case when gclb='63' then round(" + strMjFieldName + ",2) else 0 end) as 省级交通沿线荒山绿化工程," +
                                    "sum(case when gclb='64' then round(" + strMjFieldName + ",2) else 0 end) as 省级园林村镇绿化工程," +
                                    "sum(case when gclb='65' then round(" + strMjFieldName + ",2) else 0 end) as 省级环城绿化工程," +
                                    "sum(case when gclb='66' then round(" + strMjFieldName + ",2) else 0 end) as 省级城市绿化工程," +
                                    "sum(case when gclb='70' then round(" + strMjFieldName + ",2) else 0 end) as 全国湿地保护工程," +
                                    "sum(case when gclb='71' then round(" + strMjFieldName + ",2) else 0 end) as 雁门关生态畜牧经济区建设工程," +
                                    "sum(case when gclb='80' then round(" + strMjFieldName + ",2) else 0 end) as 重点公益林经营工程," +
                                    "sum(case when gclb='81' then round(" + strMjFieldName + ",2) else 0 end) as 首都水资源可持续利用工程," +
                                    "sum(case when gclb='82' then round(" + strMjFieldName + ",2) else 0 end) as 汾河库区上游河道治理工程," +
                                    "sum(case when gclb='83' then round(" + strMjFieldName + ",2) else 0 end) as 小型水利和水保建设工程," +
                                    "sum(case when gclb='84' then round(" + strMjFieldName + ",2) else 0 end) as 省级应急水源工程," +
                                    "sum(case when gclb='85' then round(" + strMjFieldName + ",2) else 0 end) as 省级农田水利灌溉工程," +
                                    "sum(case when gclb='86' then round(" + strMjFieldName + ",2) else 0 end) as 省级水土保持淤地坝工程," +
                                    "sum(case when gclb='87' then round(" + strMjFieldName + ",2) else 0 end) as 省级农村饮水安全工程," +
                                    "sum(case when gclb='88' then round(" + strMjFieldName + ",2) else 0 end) as 省级城乡节水工程," +
                                    "sum(case when gclb='89' then round(" + strMjFieldName + ",2) else 0 end) as 省级水源保护工程," +
                                    "sum(case when gclb='90' then round(" + strMjFieldName + ",2) else 0 end) as 其它林业工程," +
                                    "sum(case when gclb='91' then round(" + strMjFieldName + ",2) else 0 end) as 其它农业工程," +
                                    "sum(case when gclb='92' then round(" + strMjFieldName + ",2) else 0 end) as 其它水利工程  " +
                                    "  from " + tableName +
                                    "  group by lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //基本信息统计 ygc 2013-01-17
        private static bool JBXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName,string strXJLFieldName)
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
            DropTable(pWorkspace, "DATA_JBXX");
            try
            {
                string cunSQL = "create table DATA_JBXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when dl<> ' ' then round("+strMjFieldName+",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round("+strXJLFieldName+",2) else 0 end ) as 蓄积量  " +
                                "  from "+tableName +
                                "  group by substr(cun,1,10) ";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_JBXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when dl<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round(" + strXJLFieldName + ",2) else 0 end ) as 蓄积量  " +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2)) ";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_JBXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when dl<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round(" + strXJLFieldName + ",2) else 0 end ) as 蓄积量  " +
                                "  from " + tableName +
                                "  group by substr(shi,1,4)) ";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_JBXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when dl<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round(" + strXJLFieldName + ",2) else 0 end ) as 蓄积量  " +
                                "  from " + tableName +
                                "  group by substr(xian,1,6)) ";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_JBXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when dl<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round(" + strXJLFieldName + ",2) else 0 end ) as 蓄积量  " +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8)) ";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_JBXX ( select lc as 统计单位," +
                                "sum(case when dl<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 国土面积," +
                                "sum(case when dl like'1%' then round(" + strMjFieldName + ",2) else 0 end ) 林地面积," +
                                "sum(case when dl like'2%' then round(" + strMjFieldName + ",2) else 0 end) 非林地面积," +
                                "sum(case when dl in ('111','112','131' )then round(" + strMjFieldName + ") else 0 end ) as 森林覆盖率," +
                                "sum(case when dl in ('111','112','131','132','133' )then round(" + strMjFieldName + ") else 0 end ) as 林木覆盖率," +
                                "sum(case when dl <>' ' then round(" + strXJLFieldName + ",2) else 0 end ) as 蓄积量  " +
                                "  from " + tableName +
                                "  group by lc) ";
                pWorkspace.ExecuteSQL(lcSQL);
                //更新覆盖率
                pWorkspace.ExecuteSQL("update data_jbxx set 森林覆盖率=round((森林覆盖率/国土面积)*100,2)");
                pWorkspace.ExecuteSQL("update data_jbxx set 林木覆盖率=round((林木覆盖率/国土面积)*100,2)");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //林地结构信息统计 ygc 2013-01-17
        private static bool LDJGXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_LDJG");
            try
            {
                string cunSQL="create table DATA_LDJG as select substr(cun,1,10) as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by substr(cun,1,10)";
                pWorkspace .ExecuteSQL (cunSQL);
                string shengSQL="insert into DATA_LDJG (select substr(sheng,1,2) as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by substr(sheng,1,2))";
                pWorkspace .ExecuteSQL (shengSQL );
                string shiSQL="insert into DATA_LDJG (select substr(shi,1,4) as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by substr(shi,1,4))";
                pWorkspace .ExecuteSQL (shiSQL );
                string xianSQL="insert into DATA_LDJG (select substr(xian,1,6) as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by substr(xian,1,6))";
                pWorkspace .ExecuteSQL (xianSQL  );
                string xiangSQL="insert into DATA_LDJG (select substr(xiang,1,8) as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by substr(xiang,1,8))";
                pWorkspace .ExecuteSQL (xiangSQL );
                string lcSQL="insert into DATA_LDJG (select lc as 统计单位,"+
                                "sum(case when sen_lin_lb <>' ' then  round("+strMjFieldName+",2) else 0 end) as 合计,"+
                                "sum(case when sen_lin_lb like'1%' then round("+strMjFieldName+",2) else 0 end) as  公益林小计,"+
                                "sum(case when sen_lin_lb='11' then round("+strMjFieldName+",2) else 0 end) as 重点公益林,"+
                                "sum(case when sen_lin_lb='12' then round("+strMjFieldName+",2) else 0 end) as  一般公益林,"+
                                "sum(case when sen_lin_lb like'2%' then round("+strMjFieldName+",2) else 0 end) as 商品林小计,"+
                                "sum(case when sen_lin_lb ='21' then round("+strMjFieldName+",2) else 0 end) as 重点商品林 ，"+
                                "sum(case when sen_lin_lb='22' then round("+strMjFieldName+",2) else 0 end) as 一般商品林"+
                                "  from "+tableName+
                                "  group by lc)";
                pWorkspace.ExecuteSQL (lcSQL );
            }
            catch (Exception ex)
            {
                return false ;
            }
            return true;
        }
        //林种信息统计 ygc 2013-01-17
        private static bool LZXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_LZXX");
            try
            {
                string cunSQL = "create table DATA_LZXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when lz<>' ' then round("+strMjFieldName+",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from "+tableName+
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert  into DATA_LZXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when lz<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert  into DATA_LZXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when lz<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert  into DATA_LZXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when lz<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert  into DATA_LZXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when lz<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert  into DATA_LZXX ( select lc as 统计单位," +
                                "sum(case when lz<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when lz between '111' and '117' then round(" + strMjFieldName + ",2) else 0 end) as 防护林小计," +
                                "sum(case when lz='111' then round(" + strMjFieldName + ",2) else 0 end) as 水源涵养林," +
                                "sum(case when lz='112' then round(" + strMjFieldName + ",2) else 0 end) as 水土保持林," +
                                "sum(case when lz='113' then round(" + strMjFieldName + ",2) else 0 end) as 防风固沙林," +
                                "sum(case when lz='114' then round(" + strMjFieldName + ",2) else 0 end) as 农田牧场防护林," +
                                "sum(case when lz='115' then round(" + strMjFieldName + ",2) else 0 end) as 护岸林," +
                                "sum(case when lz='116' then round(" + strMjFieldName + ",2) else 0 end) as 护路林," +
                                "sum(case when lz='117' then round(" + strMjFieldName + ",2) else 0 end) as 其它防护林," +
                                "sum(case when lz between '121' and '127' then round(" + strMjFieldName + ",2) else 0 end ) as 特种用途林小计," +
                                "sum(case when lz='121' then round(" + strMjFieldName + ",2) else 0 end) as 国防林," +
                                "sum(case when lz='122' then round(" + strMjFieldName + ",2) else 0 end) as 实验林," +
                                "sum(case when lz='123' then round(" + strMjFieldName + ",2) else 0 end) as 母树林," +
                                "sum(case when lz='124' then round(" + strMjFieldName + ",2) else 0 end) as 环境保护林," +
                                "sum(case when lz='125' then round(" + strMjFieldName + ",2) else 0 end) as 风景林," +
                                "sum(case when lz='126' then round(" + strMjFieldName + ",2) else 0 end) as 名胜古迹和革命纪念林," +
                                "sum(case when lz='127' then round(" + strMjFieldName + ",2) else 0 end) as  自然保护林," +
                                "sum(case when lz between '231' and '233' then round(" + strMjFieldName + ",2) else 0 end ) as 用材林小计," +
                                "sum(case when lz='231' then round(" + strMjFieldName + ",2) else 0 end) as 一般用材林," +
                                "sum(case when lz='232' then round(" + strMjFieldName + ",2) else 0 end) as  速生丰产用材林," +
                                "sum(case when lz='233' then round(" + strMjFieldName + ",2) else 0 end) as 短轮伐期工业原材料用材林," +
                                "sum(case when lz='240' then round(" + strMjFieldName + ",2) else 0 end) as 薪炭林," +
                                "sum(case when lz between '251' and '255' then round(" + strMjFieldName + ",2) else 0 end) as 经济林小计," +
                                "sum(case when lz='251' then round(" + strMjFieldName + ",2) else 0 end) as 果树林," +
                                "sum(case when lz='252' then round(" + strMjFieldName + ",2) else 0 end) as 食用原料林," +
                                "sum(case when lz='253' then round(" + strMjFieldName + ",2) else 0 end) as 林化工业原料林," +
                                "sum(case when lz='254' then round(" + strMjFieldName + ",2) else 0 end) as 药用材林," +
                                "sum(case when lz='255' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济林" +
                                "  from " + tableName +
                                "  group by lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //起源信息统计 ygc 2013-01-17
        private static bool QYXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_QYXX");
            try
            {
                string cunSQL = "create table DATA_QYXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round("+strMjFieldName+",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from "+tableName +
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_QYXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_QYXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_QYXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_QYXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_QYXX ( select lc as 统计单位," +
                                "sum(case when qi_yuan <>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when qi_yuan like'1%' then round(" + strMjFieldName + ",2) else 0 end) as  天然小计," +
                                "sum(case when qi_yuan ='11' then round(" + strMjFieldName + ",2) else 0 end) as 纯天然," +
                                "sum(case when qi_yuan='12' then round(" + strMjFieldName + ",2) else 0 end) as 人工促进," +
                                "sum(case when qi_yuan='13' then round(" + strMjFieldName + ",2) else 0 end) as 萌生," +
                                "sum(case when qi_yuan like'2%' then round(" + strMjFieldName + ",2) else 0 end) as 人工小计," +
                                "sum(case when qi_yuan ='21' then round(" + strMjFieldName + ",2) else 0 end) as 植苗," +
                                "sum(case when qi_yuan='22' then round(" + strMjFieldName + ",2) else 0 end) as  直播," +
                                "sum(case when qi_yuan='23' then round(" + strMjFieldName + ",2) else 0 end) as  飞播," +
                                "sum(case when qi_yuan='24' then round(" + strMjFieldName + ",2) else 0 end) as 人工萌生" +
                                "  from " + tableName +
                                "  group by lc）";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //权属信息统计 ygc 2013-01-17
        private static bool QSXX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_QSXX");
            try
            {
                string cunSQL = "create table DATA_QSXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when ld_qs='10' then  round("+strMjFieldName+",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from "+tableName +
                                "  group by substr(cun ,1,10) ";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_QSXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when ld_qs='10' then  round(" + strMjFieldName + ",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from " + tableName +
                                "  group by substr(sheng ,1,2)) ";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_QSXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when ld_qs='10' then  round(" + strMjFieldName + ",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from " + tableName +
                                "  group by substr(shi ,1,4)) ";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_QSXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when ld_qs='10' then  round(" + strMjFieldName + ",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from " + tableName +
                                "  group by substr(xian ,1,6)) ";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_QSXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when ld_qs='10' then  round(" + strMjFieldName + ",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from " + tableName +
                                "  group by substr(xiang ,1,8)) ";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_QSXX ( select lc as 统计单位," +
                                "sum(case when ld_qs='10' then  round(" + strMjFieldName + ",2) else 0 end) as 国有," +
                                "sum(case when ld_qs in ('21','22','23') then round(" + strMjFieldName + ",2) else 0 end ) as 集体小计," +
                                "sum(case when ld_qs='21' then round(" + strMjFieldName + ",2) else 0 end) as 农户家庭承包经营," +
                                "sum(case when ld_qs='22' then round(" + strMjFieldName + ",2) else 0 end) as 联户合作经营," +
                                "sum(case when ld_qs='23' then round(" + strMjFieldName + ",2) else 0 end) as 集体经济组织经营," +
                                "sum(case when ld_qs='20' then round(" + strMjFieldName + ",2) else 0 end) as  其它" +
                                "  from " + tableName +
                                "  group by lc) ";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        //灾害等级信息统计 ygc 2013-01-17
        private static bool ZHDJ_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_ZHDJ");
            try
            {
                string cunSQL = "create table DATA_ZHDJ as select substr(cun,1,10) as 统计单位," +
                                "sum(case when disaster_c<>' ' then round("+strMjFieldName+",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from "+tableName +
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_ZHDJ ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when disaster_c<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_ZHDJ ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when disaster_c<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_ZHDJ ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when disaster_c<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_ZHDJ ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when disaster_c<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_ZHDJ ( select lc as 统计单位," +
                                "sum(case when disaster_c<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when disaster_c='0'  then round(" + strMjFieldName + ",2) else 0 end) as 无," +
                                "sum(case when disaster_c='1' then round(" + strMjFieldName + ",2) else 0 end) as 轻," +
                                "sum(case when disaster_c='2' then round(" + strMjFieldName + ",2) else 0 end) as 中," +
                                "sum(case when disaster_c='3' then round(" + strMjFieldName + ",2) else 0 end) as 重" +
                                "  from " + tableName +
                                "  group by lc)";
                pWorkspace .ExecuteSQL(lcSQL );

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //灾害类型信息统计 ygc 2013-01-17
        private static bool ZHLX_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_ZHLX");
            try
            {
                string cunSQL = "create table DATA_ZHLX as select substr(cun,1,10)  as 统计单位," +
                                "sum(case when dispe<>' ' then round("+strMjFieldName+",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from "+tableName +
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_ZHLX  (select substr(sheng,1,2)  as 统计单位," +
                                "sum(case when dispe<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_ZHLX  (select substr(shi,1,4)  as 统计单位," +
                                "sum(case when dispe<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_ZHLX  (select substr(xian,1,6)  as 统计单位," +
                                "sum(case when dispe<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_ZHLX  (select substr(xiang,1,8)  as 统计单位," +
                                "sum(case when dispe<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_ZHLX  (select lc  as 统计单位," +
                                "sum(case when dispe<>' ' then round(" + strMjFieldName + ",2) else 0 end ) as 合计," +
                                "sum(case when dispe between '11' and '12' then round(" + strMjFieldName + ",2) else 0 end ) as 病虫害小计," +
                                "sum(case when dispe='11' then round(" + strMjFieldName + ",2) else 0 end) as 病害," +
                                "sum(case when dispe='12' then round(" + strMjFieldName + ",2) else 0 end) as 虫害," +
                                "sum(case when dispe='20' then round(" + strMjFieldName + ",2) else 0 end) as 火灾," +
                                "sum(case when dispe between '31' and '34' then round(" + strMjFieldName + ",2) else 0 end) as 气候灾害小计," +
                                "sum(case when dispe ='31' then round(" + strMjFieldName + ",2) else 0 end) as 风折," +
                                "sum(case when dispe='32' then round(" + strMjFieldName + ",2) else 0 end) as 雪压," +
                                "sum(case when dispe='33' then round(" + strMjFieldName + ",2) else 0 end) as 滑坡," +
                                "sum(case when dispe='34' then round(" + strMjFieldName + ",3) else 0 end) as 干旱 ," +
                                "sum(case when dispe='40' then round(" + strMjFieldName + ",2) else 0 end) as 其它灾害," +
                                "sum(case when dispe='00' then round(" + strMjFieldName + ",2) else 0 end) as 无灾害" +
                                "  from " + tableName +
                                "  group by lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //质量等级信息统计 ygc 2013-01-17
        private static bool ZLDJ_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_ZLDJXX");
            try
            {
                string cunSQL = "create table DATA_ZLDJXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round("+strMjFieldName+",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from "+tableName +
                                "  group by substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_ZLDJXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from " + tableName +
                                "  group by substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_ZLDJXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from " + tableName +
                                "  group by substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_ZLDJXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from " + tableName +
                                "  group by substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_ZLDJXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from " + tableName +
                                "  group by substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_ZLDJXX ( select lc as 统计单位," +
                                "sum(case when zl_dj<> ' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zl_dj='1' then round(" + strMjFieldName + ",2) else 0 end) as  一级," +
                                "sum(case when zl_dj='2' then round(" + strMjFieldName + ",2) else 0 end) as  二级," +
                                "sum(case when zl_dj='3' then round(" + strMjFieldName + ",2) else 0 end) as 三级," +
                                "sum(case when zl_dj='4' then round(" + strMjFieldName + ",2) else 0 end) as 四级，" +
                                "sum(case when zl_dj='5' then round(" + strMjFieldName + ",2) else 0 end) as 五级" +
                                "  from " + tableName +
                                "  group by lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //主要树种信息 ygc 2013-01-17
        private static bool ZYSZ_Statistic(IFeatureClass pFeatureClass, string strMjFieldName)
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
            DropTable(pWorkspace, "DATA_SZXX");
            try
            {
                string cunSQL = "create table DATA_SZXX as select substr(cun,1,10) as 统计单位," +
                                "sum(case when zysz<>' ' then round("+strMjFieldName+",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from "+tableName +
                                "  group by  substr(cun,1,10)";
                pWorkspace.ExecuteSQL(cunSQL);
                string shengSQL = "insert into  DATA_SZXX ( select substr(sheng,1,2) as 统计单位," +
                                "sum(case when zysz<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from " + tableName +
                                "  group by  substr(sheng,1,2))";
                pWorkspace.ExecuteSQL(shengSQL);
                string shiSQL = "insert into  DATA_SZXX ( select substr(shi,1,4) as 统计单位," +
                                "sum(case when zysz<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from " + tableName +
                                "  group by  substr(shi,1,4))";
                pWorkspace.ExecuteSQL(shiSQL);
                string xianSQL = "insert into  DATA_SZXX ( select substr(xian,1,6) as 统计单位," +
                                "sum(case when zysz<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from " + tableName +
                                "  group by  substr(xian,1,6))";
                pWorkspace.ExecuteSQL(xianSQL);
                string xiangSQL = "insert into  DATA_SZXX ( select substr(xiang,1,8) as 统计单位," +
                                "sum(case when zysz<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from " + tableName +
                                "  group by  substr(xiang,1,8))";
                pWorkspace.ExecuteSQL(xiangSQL);
                string lcSQL = "insert into  DATA_SZXX ( select lc as 统计单位," +
                                "sum(case when zysz<>' ' then round(" + strMjFieldName + ",2) else 0 end) as 合计," +
                                "sum(case when zysz='905' then round(" + strMjFieldName + ",2) else 0 end) as 丁香," +
                                "sum(case when zysz='906' then round(" + strMjFieldName + ",2) else 0 end) as 女贞," +
                                "sum(case when zysz='907' then round(" + strMjFieldName + ",2) else 0 end) as 酸枣," +
                                "sum(case when zysz='908' then round(" + strMjFieldName + ",2) else 0 end) as 无患子," +
                                "sum(case when zysz='909' then round(" + strMjFieldName + ",2) else 0 end) as 灌木仁用杏," +
                                "sum(case when zysz='910' then round(" + strMjFieldName + ",2) else 0 end) as 沙棘," +
                                "sum(case when zysz='911' then round(" + strMjFieldName + ",2) else 0 end) as 山桃," +
                                "sum(case when zysz='912' then round(" + strMjFieldName + ",2) else 0 end) as 山杏," +
                                "sum(case when zysz='913' then round(" + strMjFieldName + ",2) else 0 end) as 绣线菊," +
                                "sum(case when zysz='914' then round(" + strMjFieldName + ",2) else 0 end) as 荀子木," +
                                "sum(case when zysz='915' then round(" + strMjFieldName + ",2) else 0 end) as 柠条," +
                                "sum(case when zysz='916' then round(" + strMjFieldName + ",2) else 0 end) as 黄刺玫," +
                                "sum(case when zysz='917' then round(" + strMjFieldName + ",2) else 0 end) as 山皂荚," +
                                "sum(case when zysz='918' then round(" + strMjFieldName + ",2) else 0 end) as 紫穗槐," +
                                "sum(case when zysz='919' then round(" + strMjFieldName + ",2) else 0 end) as 连翘," +
                                "sum(case when zysz='920' then round(" + strMjFieldName + ",2) else 0 end) as 胡枝子," +
                                "sum(case when zysz='921' then round(" + strMjFieldName + ",2) else 0 end) as 鼠李," +
                                "sum(case when zysz='922' then round(" + strMjFieldName + ",2) else 0 end) as 杞柳," +
                                "sum(case when zysz='923' then round(" + strMjFieldName + ",2) else 0 end) as 榛子," +
                                "sum(case when zysz='924' then round(" + strMjFieldName + ",2) else 0 end) as 虎榛子," +
                                "sum(case when zysz='925' then round(" + strMjFieldName + ",2) else 0 end) as 锦鸡儿," +
                                "sum(case when zysz='926' then round(" + strMjFieldName + ",2) else 0 end) as 忍冬," +
                                "sum(case when zysz='927' then round(" + strMjFieldName + ",2) else 0 end) as 黄栌," +
                                "sum(case when zysz='928' then round(" + strMjFieldName + ",2) else 0 end) as 蚂蚱腿子," +
                                "sum(case when zysz='929' then round(" + strMjFieldName + ",2) else 0 end) as 鬼见愁," +
                                "sum(case when zysz='930' then round(" + strMjFieldName + ",2) else 0 end) as 荆条," +
                                "sum(case when zysz='931' then round(" + strMjFieldName + ",2) else 0 end) as 卫矛," +
                                "sum(case when zysz='932' then round(" + strMjFieldName + ",2) else 0 end) as 小檗," +
                                "sum(case when zysz='933' then round(" + strMjFieldName + ",2) else 0 end) as 地椒," +
                                "sum(case when zysz='934' then round(" + strMjFieldName + ",2) else 0 end) as 柽柳," +
                                "sum(case when zysz='935' then round(" + strMjFieldName + ",2) else 0 end) as 河朔荛花," +
                                "sum(case when zysz='936' then round(" + strMjFieldName + ",2) else 0 end) as 悬钩子," +
                                "sum(case when zysz='937' then round(" + strMjFieldName + ",2) else 0 end) as 六道木," +
                                "sum(case when zysz='938' then round(" + strMjFieldName + ",2) else 0 end) as 照山白," +
                                "sum(case when zysz='939' then round(" + strMjFieldName + ",2) else 0 end) as 铁线莲," +
                                "sum(case when zysz='940' then round(" + strMjFieldName + ",2) else 0 end) as 狼牙刺," +
                                "sum(case when zysz='999' then round(" + strMjFieldName + ",2) else 0 end) as 其它灌木树种," +
                                "sum(case when zysz='702' then round(" + strMjFieldName + ",2) else 0 end) as 苹果," +
                                "sum(case when zysz='703' then round(" + strMjFieldName + ",2) else 0 end) as 梨," +
                                "sum(case when zysz='704' then round(" + strMjFieldName + ",2) else 0 end) as 桃," +
                                "sum(case when zysz='705' then round(" + strMjFieldName + ",2) else 0 end) as 李," +
                                "sum(case when zysz='706' then round(" + strMjFieldName + ",2) else 0 end) as 杏," +
                                "sum(case when zysz='707' then round(" + strMjFieldName + ",2) else 0 end) as 枣," +
                                "sum(case when zysz='708' then round(" + strMjFieldName + ",2) else 0 end) as 红果," +
                                "sum(case when zysz='709' then round(" + strMjFieldName + ",2) else 0 end) as 柿子," +
                                "sum(case when zysz='710' then round(" + strMjFieldName + ",2) else 0 end) as 核桃," +
                                "sum(case when zysz='711' then round(" + strMjFieldName + ",2) else 0 end) as 板栗," +
                                "sum(case when zysz='720' then round(" + strMjFieldName + ",2) else 0 end) as 葡萄," +
                                "sum(case when zysz='722' then round(" + strMjFieldName + ",2) else 0 end) as 经济仁用杏," +
                                "sum(case when zysz='749' then round(" + strMjFieldName + ",2) else 0 end) as 其它经济树种," +
                                "sum(case when zysz='758' then round(" + strMjFieldName + ",2) else 0 end) as 花椒," +
                                "sum(case when zysz='799' then round(" + strMjFieldName + ",2) else 0 end) as 黑椋子," +
                                "sum(case when zysz='851' then round(" + strMjFieldName + ",2) else 0 end) as 桑树," +
                                "sum(case when zysz='120' then round(" + strMjFieldName + ",2) else 0 end) as 云杉," +
                                "sum(case when zysz='150' then round(" + strMjFieldName + ",2) else 0 end) as 落叶松," +
                                "sum(case when zysz='170' then round(" + strMjFieldName + ",2) else 0 end) as 樟子松," +
                                "sum(case when zysz='200' then round(" + strMjFieldName + ",2) else 0 end) as 油松," +
                                "sum(case when zysz='210' then round(" + strMjFieldName + ",2) else 0 end) as 华山松," +
                                "sum(case when zysz='350' then round(" + strMjFieldName + ",2) else 0 end) as 侧柏," +
                                "sum(case when zysz='411' then round(" + strMjFieldName + ",2) else 0 end) as 栓皮栎," +
                                "sum(case when zysz='412' then round(" + strMjFieldName + ",2) else 0 end) as 辽东栎," +
                                "sum(case when zysz='413' then round(" + strMjFieldName + ",2) else 0 end) as 槲栎," +
                                "sum(case when zysz='414' then round(" + strMjFieldName + ",2) else 0 end) as 鹅耳栎," +
                                "sum(case when zysz='420' then round(" + strMjFieldName + ",2) else 0 end) as 红桦," +
                                "sum(case when zysz='421' then round(" + strMjFieldName + ",2) else 0 end) as 白桦," +
                                "sum(case when zysz='490' then round(" + strMjFieldName + ",2) else 0 end) as 硬阔类," +
                                "sum(case when zysz='491' then round(" + strMjFieldName + ",2) else 0 end) as 刺槐," +
                                "sum(case when zysz='492' then round(" + strMjFieldName + ",2) else 0 end) as 国槐," +
                                "sum(case when zysz='493' then round(" + strMjFieldName + ",2) else 0 end) as 橿子木," +
                                "sum(case when zysz='494' then round(" + strMjFieldName + ",2) else 0 end) as 白榆," +
                                "sum(case when zysz='530' then round(" + strMjFieldName + ",2) else 0 end) as 杨树," +
                                "sum(case when zysz='531' then round(" + strMjFieldName + ",2) else 0 end) as 杨树矮林," +
                                "sum(case when zysz='535' then round(" + strMjFieldName + ",2) else 0 end) as 柳树," +
                                "sum(case when zysz='540' then round(" + strMjFieldName + ",2) else 0 end) as 泡桐," +
                                "sum(case when zysz='545' then round(" + strMjFieldName + ",2) else 0 end) as 其它速生杂木," +
                                "sum(case when zysz='590' then round(" + strMjFieldName + ",2) else 0 end) as 软阔类," +
                                "sum(case when zysz='591' then round(" + strMjFieldName + ",2) else 0 end) as 山杨," +
                                "sum(case when zysz='670' then round(" + strMjFieldName + ",2) else 0 end) as 杂竹林," +
                                "sum(case when zysz='130' then round(" + strMjFieldName + ",2) else 0 end) as 臭冷杉," +
                                "sum(case when zysz='360' then round(" + strMjFieldName + ",2) else 0 end) as 桧柏," +
                                "sum(case when zysz='380' then round(" + strMjFieldName + ",2) else 0 end) as 红豆杉," +
                                "sum(case when zysz='753' then round(" + strMjFieldName + ",2) else 0 end) as 文冠果," +
                                "sum(case when zysz='271' then round(" + strMjFieldName + ",2) else 0 end) as 白皮松" +
                                "  from " + tableName +
                                "  group by  lc)";
                pWorkspace.ExecuteSQL(lcSQL);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
    }
}
