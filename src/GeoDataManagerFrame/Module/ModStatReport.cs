using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.Data.OleDb;
using System.Xml;
using System.IO;

using System.Windows.Forms;
using GeoDataCenterFunLib;
using SysCommon.Gis;
using System.Data;
namespace GeoDataManagerFrame
{
    class ModStatReport
    {
        public static string _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";//added by chulili 20110802 褚丽丽添加变量,展示图层树路径,专门用来查找道路,河流,地名地物类
        public static string _Statistic_Year = "";
        public static string _Statistic_XZQ = "";
        public static string _Statistic_AreaUnit = "";
        public static string _Statistic_Fractionnum = "2";
        public static bool _Statistic_TDLY = false;
        public static string _ResultPath_LandUse = Application.StartupPath + @"\..\OutputResults\统计成果\LandUseStatistic";
        public static string _ResultPath_Imp = Application.StartupPath + @"\..\OutputResults\统计成果\ImportStatistic";
        public static bool _Statistic_ZTGH = true;
        public static string _LogFilePath = Application.StartupPath + "\\..\\Log\\GeoDataManagerFrame.txt";
        public static void WriteStaticLog(string strLog)
        {           
            //判断文件是否存在  不存在就创建
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);
            
            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            
            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog +"     "+ strTime + "\r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread); 
             sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
        public static void OpenExcelFile(string filepath)
        {
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            object MissingValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks._Open(filepath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
            xApp.Visible = true; 
            
        }
        public static void CopyExcelSheet(string FromFilePath,string FromSheetName,string ToFilePath,string ToSheetName)
        {
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            object MissingValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks._Open(FromFilePath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);

            // 选择源工作表.
            Microsoft.Office.Interop.Excel.Worksheet fromSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets.get_Item(FromSheetName);
            // 选择
            fromSheet.Select(MissingValue);
            // 复制.
            fromSheet.UsedRange.Copy(MissingValue);
            string strAddress=fromSheet.UsedRange.get_Address(MissingValue, MissingValue, Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1, MissingValue, MissingValue);
            
            string strCell1 = strAddress.Substring(0, strAddress.IndexOf(":"));
            string strCell2 = strAddress.Substring(strAddress.IndexOf(":") + 1);
            
            Microsoft.Office.Interop.Excel.Workbook xToBook = xApp.Workbooks._Open(ToFilePath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);

            // 选择目的工作表.
            Microsoft.Office.Interop.Excel.Worksheet toSheet = (Microsoft.Office.Interop.Excel.Worksheet)xToBook.Sheets.get_Item(ToSheetName);
            // 选择.
            toSheet.Activate();
            toSheet.Select(MissingValue);

            // 粘贴格式.
            //toSheet.get_Range(strCell1, strCell2).PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteFormats, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, MissingValue, MissingValue);
            // 粘贴数据.
            toSheet.get_Range(strCell1, strCell2).PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteAll, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, MissingValue, MissingValue);
            xToBook.Save();
            xToBook.Saved = true;
           
            xApp.Workbooks.Close();
            xApp.Quit();
            xApp = null;
            

        }
        
        //基本农田统计
        public static void DoJBNTStatistic(string DataSourcePath, string DataSourceName,string JBNTtablename,string TemplateFileName,string ResultFilePath,string strAreaUnit,int FractionNum)
        {
            //int FractionNum = 2;
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataSourcePath + "\\" + DataSourceName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();

            ModTableFun.DropTable(oledbconn, "tmpJBNT");
            string sqlstr = "select " + ModFieldConst.g_XZQHMC + ",QSDWMC,sum(JBNTMJ) as MJ, sum(shape_area) as JSMJ into tmpJBNT from " + JBNTtablename + " GROUP BY " + ModFieldConst.g_XZQHMC + " ,QSDWMC";
            //行政区划代码，编号，监测图斑面积，规划图斑编号，规划用地用途代码
            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();
            if (strAreaUnit == "公顷")
            {
                //FractionNum = 4;
                oledbcomm.CommandText = " update tmpJBNT set jsmj=round(jsmj/10000," + FractionNum + "),MJ=round(MJ/10000," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            else if (strAreaUnit == "亩")
            {
                oledbcomm.CommandText = " update tmpJBNT set jsmj=round(jsmj*3/2000," + FractionNum + "),MJ=round(MJ*3/2000," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            else
            {
                oledbcomm.CommandText = " update tmpJBNT set jsmj=round(jsmj," + FractionNum + "),MJ=round(MJ," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            oledbcomm.CommandText = " update tmpJBNT set jsmj=MJ where jsmj>MJ";
            oledbcomm.ExecuteNonQuery();
            OleDbDataReader pReader = null;
            oledbconn.Close();
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            //规划图斑没有图斑编号怎么办？？暂时使用森林用途分区编号
            frm = ModFlexcell.SendDataToFlexcell(connstr, "基本农田", "tmpJBNT", ModFieldConst.g_XZQHMC + ",QSDWMC,mj,jsmj", "", TemplateFileName, 4, 1);
            //弹出报表对话框
            ModFlexcell.m_SpecialRow = 2;
            AxFlexCell.AxGrid pGrid = frm.GetGrid();
            pGrid.Cell(2, 1).Text = "单位：" + strAreaUnit + "("+GetFractionStr(FractionNum)+")";

            pGrid.Cell(2, 1).Alignment = FlexCell.AlignmentConstants.cellRightCenter;
            pGrid.Range(2, 1, 2, 1).FontName = "宋体";
            pGrid.Range(2, 1, 2, 1).FontSize = 9;
            pGrid.ExportToExcel(ResultFilePath);
            pGrid.RemoveChart(pGrid.Rows-1,pGrid.Cols-1);
            frm.ReleaseCell();
            frm = null;
            //frm.SaveFile(m_WorkPath + "\\基本农田.cel");
            //ModStatReport.OpenExcelFile(excelPath);
            //string strConn;
            //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + TemplateFileName + ";Extended Properties='Excel 8.0;HDR=Yes'";
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //OleDbCommand pCommand = conn.CreateCommand();

            ////pCommand.CommandText = "CREATE TABLE 基本农田 (坐落单位 char(255), 权属单位 char(255),基本农田面积 char(15),压占面积 char(15))";
            ////pCommand.ExecuteNonQuery();


            //OleDbDataAdapter da = new OleDbDataAdapter("Select * From [基本农田$]", conn);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "inputTable");

            //da.InsertCommand = new OleDbCommand("INSERT INTO [基本农田$] VALUES(?,?,?,?)", conn);
            //da.InsertCommand.Parameters.Add("@坐落单位", OleDbType.VarChar).SourceColumn = "坐落单位";
            //da.InsertCommand.Parameters.Add("@权属单位", OleDbType.VarChar).SourceColumn = "权属单位";
            //da.InsertCommand.Parameters.Add("@基本农田面积", OleDbType.VarChar).SourceColumn = "基本农田面积";
            //da.InsertCommand.Parameters.Add("@压占面积", OleDbType.VarChar).SourceColumn = "压占面积";

            //oledbcomm.CommandText = "select " + ModFieldConst.g_XZQHMC + ",QSDWMC,JSMJ,MJ from tmpJBNT";
            //pReader = oledbcomm.ExecuteReader();
            //while (pReader.Read())
            //{
            //    //pCommand.CommandText = "insert into [Sheet2$](坐落单位,权属单位,基本农田面积,压占面积) values('"+pReader[0].ToString()+"','"+pReader[1].ToString()+"','"+pReader[2].ToString()+"','"+pReader[3].ToString()+"')";
            //    //pCommand.ExecuteNonQuery();
            //    DataRow dr = ds.Tables[0].NewRow();
            //    dr[0] = pReader[0].ToString();
            //    dr[1] = pReader[1].ToString();
            //    dr[2] = pReader[2].ToString();
            //    dr[3] = pReader[3].ToString();
            //    ds.Tables[0].Rows.Add(dr);
            //}
            //da.Update(ds, "inputTable");

            //oledbconn.Close();
            //conn.Close();
        }
        //用途分区统计
        public static void DoYTFQStatistic(string DataSourcePath, string DataSourceName, string YTFQtablename, string TemplateFileName, string ResultFilePath, string strAreaUnit, int FractionNum)
        {
            //int FractionNum = 2;
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataSourcePath + "\\" + DataSourceName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();

            ModTableFun.DropTable(oledbconn, "tmpYTFQ");
            string sqlstr = "select " + ModFieldConst.g_XZQHMC + ",YTFQMC,sum(shape_area) as jsmj,sum(MJ) as YTFQMJ into tmpYTFQ from " + YTFQtablename + " GROUP BY " + ModFieldConst.g_XZQHMC + ",YTFQMC";

            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();
            if (strAreaUnit == "公顷")
            {
                //FractionNum = 4;
                oledbcomm.CommandText = " update tmpYTFQ set jsmj=round(jsmj/10000," + FractionNum + "),YTFQMJ=round(YTFQMJ/10000," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            else if (strAreaUnit == "亩")
            {
                oledbcomm.CommandText = " update tmpYTFQ set jsmj=round(jsmj*3/2000," + FractionNum + "),YTFQMJ=round(YTFQMJ*3/2000," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            else
            {
                oledbcomm.CommandText = " update tmpYTFQ set jsmj=round(jsmj," + FractionNum + "),YTFQMJ=round(YTFQMJ," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            oledbcomm.CommandText = " update tmpYTFQ set jsmj=YTFQMJ where jsmj>YTFQMJ";
            oledbcomm.ExecuteNonQuery();
            OleDbDataReader pReader = null;

            ////生成报表对话框
            oledbconn.Close();
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            //规划图斑没有图斑编号怎么办？？暂时使用森林用途分区编号
            frm = ModFlexcell.SendDataToFlexcell(connstr, "森林用途分区", "tmpYTFQ", ModFieldConst.g_XZQHMC + ",YTFQMC,YTFQMJ,jsmj", "", TemplateFileName, 4, 1);
            //弹出报表对话框
            ModFlexcell.m_SpecialRow = 2;
            AxFlexCell.AxGrid pGrid = frm.GetGrid();

            pGrid.Cell(2, 1).Text = "单位：" + strAreaUnit + "("+GetFractionStr(FractionNum )+")";

            pGrid.Cell(2, 1).Alignment = FlexCell.AlignmentConstants.cellRightCenter;
            pGrid.Range(2, 1, 2, 1).FontName = "宋体";
            pGrid.Range(2, 1, 2, 1).FontSize = 9;

            pGrid.ExportToExcel(ResultFilePath);
            pGrid.RemoveChart(pGrid.Rows - 1, pGrid.Cols - 1);
            frm.ReleaseCell();
            frm = null;
            //frm.SaveFile(m_WorkPath + "\\监测图斑规划情况表.cel");
            //ModStatReport.OpenExcelFile(excelPath);

            //string strConn;
            //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + TemplateFileName + ";Extended Properties='Excel 8.0;HDR=Yes'";
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //OleDbCommand pCommand = conn.CreateCommand();

            ////pCommand.CommandText = "CREATE TABLE 森林用途分区 (坐落单位 char(255), 森林用途区类型 char(255),森林用途区面积 char(15),压占面积 char(15))";
            ////pCommand.ExecuteNonQuery();


            //OleDbDataAdapter da = new OleDbDataAdapter("Select * From [森林用途分区$]", conn);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "inputTable");

            //da.InsertCommand = new OleDbCommand("INSERT INTO [森林用途分区$] VALUES(?, ?,?,?)", conn);
            //da.InsertCommand.Parameters.Add("@坐落单位", OleDbType.VarChar).SourceColumn = "坐落单位";
            //da.InsertCommand.Parameters.Add("@森林用途区类型", OleDbType.VarChar).SourceColumn = "森林用途区类型";
            //da.InsertCommand.Parameters.Add("@森林用途区面积", OleDbType.VarChar).SourceColumn = "森林用途区面积";
            //da.InsertCommand.Parameters.Add("@压占面积", OleDbType.VarChar).SourceColumn = "压占面积";

            //oledbcomm.CommandText = "select " + ModFieldConst.g_XZQHMC + ",YTFQMC,jsmj,YTFQMJ from tmpYTFQ";
            //pReader = oledbcomm.ExecuteReader();
            //while (pReader.Read())
            //{
            //    //pCommand.CommandText = "insert into [Sheet2$](坐落单位,权属单位,基本农田面积,压占面积) values('"+pReader[0].ToString()+"','"+pReader[1].ToString()+"','"+pReader[2].ToString()+"','"+pReader[3].ToString()+"')";
            //    //pCommand.ExecuteNonQuery();
            //    DataRow dr = ds.Tables[0].NewRow();
            //    dr[0] = pReader[0].ToString();
            //    dr[1] = pReader[1].ToString();
            //    dr[2] = pReader[2].ToString();
            //    dr[3] = pReader[3].ToString();
            //    ds.Tables[0].Rows.Add(dr);
            //}
            //da.Update(ds, "inputTable");

            //oledbconn.Close();
            //conn.Close();

        }
        //建设用地统计
        public static void DoJSYDStatistic(string DataSourcePath, string DataSourceName, string JSYDtablename, string TemplateFileName, string ResultFilePath, string strAreaUnit, int FractionNum)
        {
            //int FractionNum = 2;
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataSourcePath + "\\" + DataSourceName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();

            ModTableFun.DropTable(oledbconn, "tmpJSYD");
            string sqlstr = "select " + ModFieldConst.g_XZQHMC + ",GZFQMC,sum(shape_area) as jsmj,sum(MJ) as GZFQMJ into tmpJSYD from " + JSYDtablename + " group by " + ModFieldConst.g_XZQHMC + ",GZFQMC";

            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();
            if (strAreaUnit == "公顷")
            {
                //FractionNum = 4;
                oledbcomm.CommandText = " update tmpJSYD set jsmj=round(jsmj/10000,"+FractionNum+"),gzfqmj=round(gzfqmj/10000,"+FractionNum+")";
                oledbcomm.ExecuteNonQuery();
            }
            else if (strAreaUnit == "亩")
            {
                oledbcomm.CommandText = " update tmpJSYD set jsmj=round(jsmj*3/2000," + FractionNum + "),gzfqmj=round(gzfqmj*3/2000," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            else
            {
                oledbcomm.CommandText = " update tmpJSYD set jsmj=round(jsmj," + FractionNum + "),gzfqmj=round(gzfqmj," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            oledbcomm.CommandText = " update tmpJSYD set jsmj=gzfqmj where jsmj>gzfqmj";
            oledbcomm.ExecuteNonQuery();
            OleDbDataReader pReader = null;
            ////生成报表对话框
            oledbconn.Close();
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            //规划图斑没有图斑编号怎么办？？暂时使用森林用途分区编号
            frm = ModFlexcell.SendDataToFlexcell(connstr, "建设用地管制区", "tmpJSYD", ModFieldConst.g_XZQHMC + ",GZFQMC,GZFQMJ,jsmj", "", TemplateFileName, 4, 1);
            //弹出报表对话框
            ModFlexcell.m_SpecialRow = 2;
            AxFlexCell.AxGrid pGrid = frm.GetGrid();

            pGrid.Cell(2, 1).Text = "单位：" + strAreaUnit + "("+GetFractionStr(FractionNum )+")";


            pGrid.Cell(2, 1).Alignment = FlexCell.AlignmentConstants.cellRightCenter;
            pGrid.Range(2, 1, 2, 1).FontName = "宋体";
            pGrid.Range(2, 1, 2, 1).FontSize = 9;

            pGrid.ExportToExcel(ResultFilePath);
            pGrid.RemoveChart(pGrid.Rows - 1, pGrid.Cols - 1);
            frm.ReleaseCell();
            frm = null;
            //frm.SaveFile(m_WorkPath + "\\监测图斑规划情况表.cel");
            //ModStatReport.OpenExcelFile(excelPath);

            //string strConn;
            //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + TemplateFileName + ";Extended Properties='Excel 8.0;HDR=Yes'";
            //OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //OleDbCommand pCommand = conn.CreateCommand();

            ////pCommand.CommandText = "CREATE TABLE 建设用地管制区 (坐落单位 char(255), 管制区类型名称 char(255),管制区面积 char(15),压占面积 char(15))";
            ////pCommand.ExecuteNonQuery();


            //OleDbDataAdapter da = new OleDbDataAdapter("Select * From [建设用地管制区$]", conn);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "inputTable");

            //da.InsertCommand = new OleDbCommand("INSERT INTO [建设用地管制区$] VALUES(?, ?,?,?)", conn);
            //da.InsertCommand.Parameters.Add("@坐落单位", OleDbType.VarChar).SourceColumn = "坐落单位";
            //da.InsertCommand.Parameters.Add("@管制区类型名称", OleDbType.VarChar).SourceColumn = "管制区类型名称";
            //da.InsertCommand.Parameters.Add("@管制区面积", OleDbType.VarChar).SourceColumn = "管制区面积";
            //da.InsertCommand.Parameters.Add("@压占面积", OleDbType.VarChar).SourceColumn = "压占面积";

            //oledbcomm.CommandText = "select " + ModFieldConst.g_XZQHMC + ",GZFQMC,jsmj,GZFQMJ from tmpJSYD";
            //pReader = oledbcomm.ExecuteReader();
            //while (pReader.Read())
            //{
            //    //pCommand.CommandText = "insert into [Sheet2$](坐落单位,权属单位,基本农田面积,压占面积) values('"+pReader[0].ToString()+"','"+pReader[1].ToString()+"','"+pReader[2].ToString()+"','"+pReader[3].ToString()+"')";
            //    //pCommand.ExecuteNonQuery();
            //    DataRow dr = ds.Tables[0].NewRow();
            //    dr[0] = pReader[0].ToString();
            //    dr[1] = pReader[1].ToString();
            //    dr[2] = pReader[2].ToString();
            //    dr[3] = pReader[3].ToString();
            //    ds.Tables[0].Rows.Add(dr);
            //}
            //da.Update(ds, "inputTable");

            //oledbconn.Close();
            //conn.Close();

        }
        //获取森林资源现状的图层，并执行森林资源现状统计功能
        public static void GetTDLYLayerKey(string XmlPath, string XZQcode, string strYear, out string DLTBNodeKey, out string XZDWNodeKey, out string LXDWNodeKey)
        {
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(XmlPath);
            //XmlNode pRoot = pXmlDoc.SelectSingleNode("//Root");
            ////XmlNode pDirNode = pRoot.SelectSingleNode("//DIR [@DIRType='TDLY' and @Year='" + strYear + "' and @XZQCode='" + XZQcode + "']");
            //XmlNode pDirNode = pRoot.SelectSingleNode("//DIR [@DIRType='TDLY' and @Year='" + strYear + "']");

            //XmlElement pDirEle = pDirNode as XmlElement;
            DLTBNodeKey = "";
            XZDWNodeKey = "";
            LXDWNodeKey = "";
            //changed by chulili 20110924 直接从图层获取
            //XmlNode pDLTBnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='TDLY' and @Year='" + strYear + "']/DataDIR/DataDIR/Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_DLTB')]");
            XmlNode pDLTBnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_DLTB')]");
            if (pDLTBnode != null)
            {
                DLTBNodeKey = pDLTBnode.Attributes["NodeKey"].Value.ToString();
            }
            //XmlNode pXZDWnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='TDLY' and @Year='" + strYear + "']/DataDIR/DataDIR/Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_XZDW')]");
            XmlNode pXZDWnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_XZDW')]");
            if (pXZDWnode != null)
            {
                XZDWNodeKey = pXZDWnode.Attributes["NodeKey"].Value.ToString();
            }
            //XmlNode pLXDWnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='TDLY' and @Year='" + strYear + "']/DataDIR/DataDIR/Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_LXDW')]");
            XmlNode pLXDWnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'TDLY" + XZQcode + "_" + strYear + "_LXDW')]");
            if (pLXDWnode != null)
            {
                LXDWNodeKey = pLXDWnode.Attributes["NodeKey"].Value.ToString();
            }
            //找到专题节点，获取地类图斑、线状地图、零星地NodeKey
            //if (pDirEle != null)
            //{
            //    XmlNodeList pLayerList = pDirEle.SelectNodes("//Layer");
            //    foreach (XmlNode pLayerNode in pLayerList)
            //    {
            //        XmlElement pLayerEle = pLayerNode as XmlElement;
            //        if (pLayerEle != null)
            //        {
            //            if (pLayerEle.HasAttribute("Code"))
            //            {
            //                string strLayerCode = pLayerEle.GetAttribute("Code");
            //                if (strLayerCode.ToUpper().EndsWith("TDLY" + XZQcode + "_" + strYear + "_DLTB"))
            //                {
            //                    DLTBNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //                else if (strLayerCode.ToUpper().EndsWith("TDLY" + XZQcode + "_" + strYear + "_XZDW"))
            //                {
            //                    XZDWNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //                else if (strLayerCode.ToUpper().EndsWith("TDLY" + XZQcode + "_" + strYear + "_LXDW"))
            //                {
            //                    LXDWNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //            }
            //        }
            //    }
            //}
        }
        //获取森林资源总体规划的图层
        public static void GetZTGHLayerKey(string XmlPath, string XZQcode, out string JBNTNodeKey, out string TDYTNodeKey, out string JSYDNodeKey)
        {
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(XmlPath);
            //XmlNode pRoot = pXmlDoc.SelectSingleNode("//Root");
            //XmlNode pDirNode = pRoot.SelectSingleNode("//DIR [@DIRType='ZTGH']");
            ////XmlNode pDirNode = pRoot.SelectSingleNode("//DIR [@DIRType='ZTGH' and @XZQCode='" + XZQcode + "']");
            //XmlElement pDirEle = pDirNode as XmlElement;
            JBNTNodeKey = "";
            TDYTNodeKey = "";
            JSYDNodeKey = "";
            //changed by chulili 20110924 直接从图层获取
            //XmlNode pJBNTnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='ZTGH']/DataDIR/DataDIR/Layer [contains(@Code,'ZTGH" + XZQcode + "_JBNT')]");
            XmlNode pJBNTnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'ZTGH" + XZQcode + "_JBNT')]");
            if (pJBNTnode != null)
            {
                JBNTNodeKey=pJBNTnode.Attributes["NodeKey"].Value.ToString();
            }
            //XmlNode pTDYTnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='ZTGH']/DataDIR/DataDIR/Layer [contains(@Code,'ZTGH" + XZQcode + "_TDYT')]");
            XmlNode pTDYTnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'ZTGH" + XZQcode + "_TDYT')]");
            if (pTDYTnode != null)
            {
                TDYTNodeKey = pTDYTnode.Attributes["NodeKey"].Value.ToString();
            }
            //XmlNode pJSYDnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='ZTGH']/DataDIR/DataDIR/Layer [contains(@Code,'ZTGH" + XZQcode + "_JSYDGZQ')]");
            XmlNode pJSYDnode = pXmlDoc.SelectSingleNode("//Layer [contains(@Code,'ZTGH" + XZQcode + "_JSYDGZQ')]");
            if (pJSYDnode != null)
            {
                JSYDNodeKey = pJSYDnode.Attributes["NodeKey"].Value.ToString();
            }

            //if (pDirEle != null)
            //{
            //    //找到专题节点，获取地类图斑、线状地图、零星地NodeKey
            //    XmlNodeList pLayerList = pDirEle.SelectNodes("//Layer");
            //    foreach (XmlNode pLayerNode in pLayerList)
            //    {
            //        XmlElement pLayerEle = pLayerNode as XmlElement;
            //        if (pLayerEle != null)
            //        {
            //            if (pLayerEle.HasAttribute("Code"))
            //            {
            //                string strLayerCode = pLayerEle.GetAttribute("Code");
            //                if (strLayerCode.ToUpper().EndsWith("ZTGH" + XZQcode + "_ZTGH_JBNT"))
            //                {
            //                    JBNTNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //                else if (strLayerCode.ToUpper().EndsWith("ZTGH" + XZQcode + "_ZTGH_TDYT"))
            //                {
            //                    TDYTNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //                else if (strLayerCode.ToUpper().EndsWith("ZTGH" + XZQcode + "_ZTGH_JSYDGZQ"))
            //                {
            //                    JSYDNodeKey = pLayerEle.GetAttribute("NodeKey");
            //                }
            //            }
            //        }
            //    }
            //}

        }
        //获取城镇地籍的图层
        public static void GetCZDJLayerKey(string XmlPath, out string ZDNodeKey, out string JFNodeKey)
        {
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(XmlPath);
            ZDNodeKey = "";
            JFNodeKey = "";
            XmlNode pZDnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='CZDJ']/Layer [contains(@Code,'CZDJ_ZD')]");
            if (pZDnode != null)
            {
                ZDNodeKey = pZDnode.Attributes["NodeKey"].Value.ToString();
            }
            XmlNode pJFnode = pXmlDoc.SelectSingleNode("//Root/DIR[@DIRType='CZDJ']/Layer [contains(@Code,'CZDJ_JF')]");
            if (pJFnode != null)
            {
                JFNodeKey = pJFnode.Attributes["NodeKey"].Value.ToString();
            }

        }
       
        public static void DoCustomizeStat(string path,string sLayerName,string TableName,List<IField > listGroupbyFields,Field pSumField,SysCommon.CProgress vProgress)
        {
            if (listGroupbyFields.Count == 0)
                return;
            //打开连接
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            //创建统计临时表
            vProgress.SetProgress("创建临时统计表");
            ModTableFun.DropTable(oledbconn, "tmpstat");
            //对分组字段做预处理
            string sGroupField = "";
            List<string> listGroupFieldname = new List<string>();
            
            for (int i = 0; i < listGroupbyFields.Count; i++)
            {
                IField  pField = listGroupbyFields[i];
                string strFieldName = pField.Name;
                if (strFieldName.Contains("SHAPE."))
                {
                    strFieldName=strFieldName.Replace("SHAPE.", "SHAPE_");
                }
                sGroupField = sGroupField + strFieldName + ",";
                listGroupFieldname.Add(pField.AliasName);

            }
            if (sGroupField.Equals(""))
            {
                return;
            }
            //对汇总字段做处理
            string sSumField = "";
            string sSumFieldAlias = "";
            sSumField = pSumField.Name;
            if (sSumField.Contains("SHAPE."))
            {
                sSumField=sSumField.Replace("SHAPE.", "SHAPE_");
            }
            listGroupFieldname.Add(pSumField.AliasName );
            sGroupField = sGroupField.Substring(0, sGroupField.Length - 1);
            oledbcomm.CommandText = "select " + sGroupField + ",sum(" + sSumField + ") as FieldSum,0.0001 as 占比 into tmpstat from " + TableName + " group by " + sGroupField;
            oledbcomm.ExecuteNonQuery();
            //按照分组字段、汇总字段进行汇总
            vProgress.SetProgress("按照分组字段、汇总字段进行汇总统计");
            OleDbDataReader myReader = ModTableFun.GetReader(oledbconn, "select sum(FieldSum) from tmpstat");
            double dSum = 0;
            if (myReader.Read())
            {   //changed by chulili 20110709排错
                if (myReader.GetValue(0).ToString().Equals(""))
                {
                    dSum = 0;
                }
                else
                {
                    dSum = double.Parse(myReader.GetValue(0).ToString());
                }
            }
            myReader.Close();
            if (dSum != 0)
            {
                oledbcomm.CommandText = "update tmpstat set 占比=round(FieldSum/" + dSum+",4)";
                oledbcomm.ExecuteNonQuery();
            }
            else            
            {
                oledbcomm.CommandText = "update tmpstat set 占比=0";
                oledbcomm.ExecuteNonQuery();
            }
            oledbconn.Close();
            //报表输出
            vProgress.SetProgress("报表输出");
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;

            string Templatepath = Application.StartupPath + "\\..\\Template\\CustomizeStat.cel";
            frm = ModFlexcell.SendDataToFlexcell(connstr, sLayerName + "自定义汇总统计表", "tmpstat", sGroupField + ",FieldSum,占比", "", Templatepath, 3, 1);
            ModFlexcell.m_SpecialRow = 2;
            AxFlexCell.AxGrid pGrid = frm.GetGrid();
            //修改flexcell中各列标题的名字，用属性列的别名
            for (int i = 0; i < listGroupFieldname.Count; i++)
            {
                pGrid.Cell(2,i+1).Text = listGroupFieldname[i];
            }
            pGrid.Cell(2, listGroupFieldname.Count+1).Text = "占比";
            pGrid.Refresh();
            string excelPath = Application.StartupPath + "\\..\\Temp\\" + sLayerName + "自定义汇总统计表.xls";
            pGrid.ExportToExcel(excelPath );
            frm = null;
            //frm.SaveFile(Application.StartupPath + "\\..\\Temp\\"+sLayerName+"自定义汇总统计表.cel");
            //弹出报表对话框
            vProgress.Close();
            OpenExcelFile(excelPath);
        }
        //农村森林资源现状汇总
        public static void LandUseCurSum(IMap pMap, string TopicName,string xmlpath, string LandUseKey,SysCommon.CProgress vProgress)
        {
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc == null)
            {
                vProgress.Close();
                return;
            }
            if (!File.Exists(xmlpath))
            {
                vProgress.Close();
                return;
            }
            xmldoc.Load(xmlpath);
            string strSearch = "//StatType [@Key='"+LandUseKey+"']";
            XmlNode xmlstattype = xmldoc.SelectSingleNode(strSearch);
            if (xmlstattype == null)
            {
                vProgress.Close();
                return;
            }
            XmlNodeList xmllist = xmlstattype.ChildNodes;
            IFeatureLayer pTBLayer=null,pXZDWLayer=null,pLXDLayer=null;
            vProgress.SetProgress("获取图层数据...");
            foreach (XmlNode layernode in xmllist)
            {
                if (layernode.NodeType == XmlNodeType.Element)
                {
                    XmlElement pEle = layernode as XmlElement;
                    string layername = pEle.GetAttribute("layername");
                    string layerkey = pEle.GetAttribute("Key");
                    switch (layerkey)
                    {
                        case "DLTB":
                            pTBLayer = GetLayerByName(pMap,TopicName,layername ) as IFeatureLayer ;
                            break;
                        case "XZDW":
                            pXZDWLayer = GetLayerByName(pMap, TopicName, layername) as IFeatureLayer;
                            break;
                        case "LXDW":
                            pLXDLayer = GetLayerByName(pMap, TopicName, layername) as IFeatureLayer;
                            break;
                        default:
                            break;                            
                    }
                }
            }
            //创建汇总结果数据库
            vProgress.SetProgress("创建汇总结果数据库");
            string workpath = Application.StartupPath + @"\..\OutputResults\汇总成果\森林资源现状汇总";

            int index1 = TopicName.IndexOf("_") + 1;
            int index2 = TopicName.LastIndexOf("年") - 4;
            string XZQname = TopicName.Substring(index1, index2 - index1);
            string sYear = TopicName.Substring(index2, 4);
            //根据行政名称获取行政代码
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();            
            string strExp = "select 行政代码 from 数据单元表 where 行政名称 ='" + XZQname + "' and 数据单元级别='3'";
            string XZQcode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            string workSpaceName = workpath + "\\" +sYear+ "年"+XZQname+"("+XZQcode+").mdb";
            if (File.Exists(workSpaceName))
            {
                File.Delete(workSpaceName);
            }
            //判断结果目录是否存在，不存在则创建
            if (System.IO.Directory.Exists(workpath) == false)
                System.IO.Directory.CreateDirectory(workpath);
            //创建一个新的mdb数据库,并打开工作空间
            IWorkspace pOutWorkSpace = ChangeJudge.CreatePDBWorkSpace(workpath, sYear + "年" + XZQname + "(" + XZQcode + ").mdb");
            string sTBName = "", sXZDWName = "", sLXDName = "";
            vProgress.SetProgress("向结果数据库拷贝数据...");
            if (pTBLayer != null)
            {
                IDataset pTBDataSet = pTBLayer.FeatureClass as IDataset;
                IWorkspace pTBWorkSpace = pTBDataSet.Workspace ;                
                sTBName = pTBDataSet.Name;
                CopyPasteGDBData.CopyPasteGeodatabaseData(pTBWorkSpace, pOutWorkSpace, sTBName, esriDatasetType.esriDTFeatureClass);
            }
            if (pXZDWLayer != null)
            {
                IDataset pXZDWDataSet = pXZDWLayer.FeatureClass as IDataset;
                IWorkspace pXZDWWordspace = pXZDWDataSet.Workspace;                
                sXZDWName = pXZDWDataSet.Name;
                CopyPasteGDBData.CopyPasteGeodatabaseData(pXZDWWordspace, pOutWorkSpace, sXZDWName, esriDatasetType.esriDTFeatureClass);
            }
            if (pLXDLayer != null)
            {
                IDataset pLXDDataSet = pLXDLayer.FeatureClass as IDataset;
                IWorkspace pLXDWordspace = pLXDDataSet.Workspace;
                
                sLXDName = pLXDDataSet.Name;
                CopyPasteGDBData.CopyPasteGeodatabaseData(pLXDWordspace, pOutWorkSpace, sLXDName, esriDatasetType.esriDTFeatureClass);
            }
            if (!sTBName.Equals("")) sTBName = sTBName.Substring(sTBName.IndexOf(".") + 1);
            if (!sXZDWName.Equals("")) sXZDWName = sXZDWName.Substring(sXZDWName.IndexOf(".") + 1);
            if (!sLXDName.Equals("")) sLXDName = sLXDName.Substring(sLXDName.IndexOf(".") + 1);
            DoLandUseStatic(workSpaceName, sTBName, sXZDWName, sLXDName, vProgress);
        }
        public static string GetXZQcode(string path, string sXZQName)
        {
            string XZQcode = "";
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            oledbcomm.CommandText = "select distinct mid(" + ModFieldConst.g_XZQDM + ",1,6) as xzq from " + sXZQName;
            OleDbDataReader pXZQnodeReader = null;
            try
            {
                pXZQnodeReader = oledbcomm.ExecuteReader();
            }
            catch(Exception err)
            { }
            if (pXZQnodeReader != null)
            {
                if (pXZQnodeReader.Read())
                {
                    XZQcode = pXZQnodeReader[0].ToString();
                }
            }
            if (pXZQnodeReader != null)
            {
                pXZQnodeReader.Close();
                pXZQnodeReader = null;
            }
            oledbconn.Close();
            oledbconn = null;
            //测试代码 chulili 20110921
            //if (XZQcode == "")
            //{
            //    XZQcode = "440203";
            //}
            return XZQcode;
        }
        public static void DoLandUseStatic(string path, string sTBName, string sXZDWName, string sLXDName, SysCommon.CProgress vProgress)
        {
            string KZColName = "tbmj";
            string KZFeaClassName = sTBName;
            string substr = "mid";
            string strim = "trim";
            string varchar2 = "text";
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();

            ModTableFun.DropTable(oledbconn, "tmpBaseComputeTable");
            
            oledbcomm.CommandText = "select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_DLBM + ",1,3) as dlbm1," + ModFieldConst.g_TBJMJ + " as MJ into tmpBaseComputeTable from " + sTBName + " where 1<>1";
            oledbcomm.ExecuteNonQuery();
            //'获取临时表数据  ----从地类图斑
            if (vProgress!=null) vProgress.SetProgress("汇总地类图斑数据");
            oledbcomm.CommandText = "insert into tmpBaseComputeTable select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_DLBM + ",1,3) as dlbm1,sum(" + ModFieldConst.g_TBJMJ + ") as MJ from " + sTBName + " group by " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_DLBM + ",1,3)";
            oledbcomm.ExecuteNonQuery();
            //'获取临时表数据  ----从地类图斑--扣除地类
            oledbcomm.CommandText = "insert into tmpBaseComputeTable select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + ",null as " + ModFieldConst.g_GDLX + ",'1' as " + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_KCDLBM + ",1,3) as dlbm1,sum(" + ModFieldConst.g_TKMJ + ") as MJ from " + sTBName + " group by " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_KCDLBM + ",1,3)";
            oledbcomm.ExecuteNonQuery();
            //'获取临时表数据  ----从线状地物
            if (!sXZDWName.Equals(""))
            {
                if (vProgress != null) vProgress.SetProgress("汇总线状地物数据");
                oledbcomm.CommandText = "insert into tmpBaseComputeTable select " + ModFieldConst.g_KCTBZLDM1 + " as " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM1 + " as " + ModFieldConst.g_QSDM + ",a." + ModFieldConst.g_QSXZ + ",null as " + ModFieldConst.g_GDLX + ",null as " + ModFieldConst.g_PDJB + "," + substr + "(a." + ModFieldConst.g_DLBM + ",1,3) as dlbm1,sum(a." + ModFieldConst.g_KCXS + "*a." + ModFieldConst.g_XZMJ + ") as MJ from " + sXZDWName + " a  where a."+ModFieldConst.g_KCTBBH1 +"<>'' and a."+ModFieldConst.g_KCTBDWDM1 +"<>'' group by " + ModFieldConst.g_KCTBZLDM1 + "," + ModFieldConst.g_QSDM1 + ",a." + ModFieldConst.g_QSXZ + "," + substr + "(a." + ModFieldConst.g_DLBM + ",1,3)";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "insert into tmpBaseComputeTable select " + ModFieldConst.g_KCTBZLDM2 + " as " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM2 + " as " + ModFieldConst.g_QSDM + ",a." + ModFieldConst.g_QSXZ + ",null as " + ModFieldConst.g_GDLX + ",null as " + ModFieldConst.g_PDJB + "," + substr + "(a." + ModFieldConst.g_DLBM + ",1,3) as dlbm1,sum((1-a." + ModFieldConst.g_KCXS + ")*a." + ModFieldConst.g_XZMJ + ") as MJ from " + sXZDWName + " a where a."+ModFieldConst.g_KCTBBH2 +"<>'' and a."+ModFieldConst.g_KCTBDWDM2 +"<>'' group by " + ModFieldConst.g_KCTBZLDM2 + "," + ModFieldConst.g_QSDM2 + ",a." + ModFieldConst.g_QSXZ + "," + substr + "(a." + ModFieldConst.g_DLBM + ",1,3)";
                oledbcomm.ExecuteNonQuery();
            }
            //'获取临时表数据  ----从零星地类
            if (!sLXDName.Equals(""))
            {
                if (vProgress != null) vProgress.SetProgress("汇总零星地物数据");
                oledbcomm.CommandText = "insert into tmpBaseComputeTable select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_DLBM + ",1,3) as dlbm1,sum(" + ModFieldConst.g_KZMJ + ") as MJ from " + sLXDName + " group by " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + substr + "(" + ModFieldConst.g_DLBM + ",1,3)";
                oledbcomm.ExecuteNonQuery();
            }
            if (vProgress != null) vProgress.SetProgress("整理基础计算表");
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_ZLDM + ") is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_QSDM + ") is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_QSXZ + ") is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(mj) is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where mj=0";
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_ZLDM + ")=''";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_QSDM + ")=''";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(" + ModFieldConst.g_QSXZ + ")=''";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete from tmpBaseComputeTable where " + strim + "(mj)=''";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmpBaseComputeTable set " + ModFieldConst.g_GDLX + "=' ' where " + ModFieldConst.g_GDLX + " is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmpBaseComputeTable set " + ModFieldConst.g_PDJB + "=' ' where " + ModFieldConst.g_PDJB + " is null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmpBaseComputeTable set " + ModFieldConst.g_GDLX + "=' '," + ModFieldConst.g_PDJB + "=' ' where dlbm1 not like '01%'";
            oledbcomm.ExecuteNonQuery();

            ModTableFun.DropTable(oledbconn, "tmpBaseComputesum");
            oledbcomm.CommandText = "select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + ", dlbm1,sum(mj) as summj into tmpBaseComputesum from tmpBaseComputeTable group by " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + ",dlbm1";
            oledbcomm.ExecuteNonQuery();

            //////////////////////////////////////////
            ModTableFun.DropTable(oledbconn, ModFieldConst.g_BaseCompuTable);

            oledbcomm.CommandText = "select " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_ZLDWMC + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSDWMC + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + ModFieldConst.g_TBJMJ + " as TDZMJ into " + ModFieldConst.g_BaseCompuTable + " from "+sTBName+" where 1<>1";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "insert into " + ModFieldConst.g_BaseCompuTable + " select " + ModFieldConst.g_ZLDM + ",null as " + ModFieldConst.g_ZLDWMC + "," + ModFieldConst.g_QSDM + ",null as " + ModFieldConst.g_QSDWMC + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + ",sum(MJ) as TDZMJ from tmpBaseComputeTable group by " + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB;
            oledbcomm.ExecuteNonQuery();
            string updatestr = ModFieldConst.m_InitFieldsValue;
            string tmpfields = ModFieldConst.m_Fields;
            string fieldsname = ModFieldConst.m_FieldsName_Access;
            oledbcomm.CommandText = "alter table " + ModFieldConst.g_BaseCompuTable + " add column " + fieldsname;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ModFieldConst.g_BaseCompuTable + " set " + updatestr;
            oledbcomm.ExecuteNonQuery();
            //为便于更新  给基础计算表添加id列   其根据id  dlbm与辅助临时表联系

            oledbcomm.CommandText = "alter table " + ModFieldConst.g_BaseCompuTable + " add column id counter(1,1)";
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "alter table tmpBaseComputesum add column ID integer";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmpBaseComputesum a," + ModFieldConst.g_BaseCompuTable + " b  set a.id=b.id where a." + ModFieldConst.g_ZLDM + "=b." + ModFieldConst.g_ZLDM + " and a." + ModFieldConst.g_QSDM + "=b." + ModFieldConst.g_QSDM + " and a." + ModFieldConst.g_QSXZ + "=b." + ModFieldConst.g_QSXZ + " and a." + ModFieldConst.g_GDLX + "=b." + ModFieldConst.g_GDLX + " and a." + ModFieldConst.g_PDJB + "=b." + ModFieldConst.g_PDJB;
            oledbcomm.ExecuteNonQuery();
            ModTableFun.DropTable(oledbconn, "tmpdlbm");

            OleDbDataReader myreader;
            //oledbcomm.CommandText = "select DISTINCT DLBM1 from tmpBaseComputesum";
            myreader = ModTableFun.GetReader(oledbconn, "select DISTINCT DLBM1 from tmpBaseComputesum");
            string tmpDLBM, strID, tmpFieldName;
            while (myreader.Read())
            {
                tmpDLBM = myreader.GetValue(0).ToString();
                tmpFieldName = "FIELD_" + tmpDLBM;
                if (ModTableFun.isFieldExist(oledbconn, ModFieldConst.g_BaseCompuTable, tmpFieldName))
                {
                    oledbcomm.CommandText = "update " + ModFieldConst.g_BaseCompuTable + " a,tmpBaseComputesum b set a." + tmpFieldName + "=b.summj where a.id=b.id and b.dlbm1='" + tmpDLBM + "'";
                    oledbcomm.ExecuteNonQuery();
                }
            }
            myreader.Close();

            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_01 = " + ModFieldConst.m_sum01; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_02 = " + ModFieldConst.m_sum02; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_03 = " + ModFieldConst.m_sum03; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_04 = " + ModFieldConst.m_sum04; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_10 = " + ModFieldConst.m_sum10; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_11 = " + ModFieldConst.m_sum11; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_12 = " + ModFieldConst.m_sum12; oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set Field_20 = " + ModFieldConst.m_sum20; oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseCompuTable + " Set tdzmj = " + ModFieldConst.m_sumsum; oledbcomm.ExecuteNonQuery();
            //比较森林汇总面积与控制面积
            double dtdzmj = 0, dtdzmj_r = 0, dkzzmj = 0, dkzzmj_r = 0;
            myreader = ModTableFun.GetReader(oledbconn, "select sum(tdzmj) from " + ModFieldConst.g_BaseCompuTable); ;
            if (myreader.Read())
            {
                dtdzmj = double.Parse(myreader.GetValue(0).ToString());
            }
            myreader.Close();
            dkzzmj = 0;
            string sqlstr = "select sum(" + KZColName + "),round(sum(" + KZColName + ")/10000,2) from " + KZFeaClassName;
            myreader = ModTableFun.GetReader(oledbconn, sqlstr);
            if (myreader.Read())
            {
                dkzzmj = double.Parse(myreader.GetValue(0).ToString());
                dkzzmj_r = double.Parse(myreader.GetValue(1).ToString());
            }
            myreader.Close();


            /////////////////2
            if (vProgress != null) vProgress.SetProgress("创建基础统计表");
            ModTableFun.DropTable(oledbconn, ModFieldConst.g_BaseStaticTable);
            oledbcomm.CommandText = "select * into " + ModFieldConst.g_BaseStaticTable + " from " + ModFieldConst.g_BaseCompuTable;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set tdzmj=round(tdzmj*0.0001,2),Field_01=round(Field_01*0.0001,2),Field_011=round(Field_011*0.0001,2),Field_012=round(Field_012*0.0001,2),Field_013=round(Field_013*0.0001,2),Field_02=round(Field_02*0.0001,2),Field_021=round(Field_021*0.0001,2),Field_022=round(Field_022*0.0001,2),Field_023=round(Field_023*0.0001,2),Field_03=round(Field_03*0.0001,2),Field_031=round(Field_031*0.0001,2),Field_032=round(Field_032*0.0001,2),Field_033=round(Field_033*0.0001,2),";
            oledbcomm.CommandText = oledbcomm.CommandText + "Field_04=round(Field_04*0.0001,2),Field_041=round(Field_041*0.0001,2),Field_042=round(Field_042*0.0001,2),Field_043=round(Field_043*0.0001,2),Field_20=round(Field_20*0.0001,2),Field_201=round(Field_201*0.0001,2),Field_202=round(Field_202*0.0001,2),Field_203=round(Field_203*0.0001,2),Field_204=round(Field_204*0.0001,2),Field_205=round(Field_205*0.0001,2),";
            oledbcomm.CommandText = oledbcomm.CommandText + "Field_10=round(Field_10*0.0001,2),Field_101=round(Field_101*0.0001,2),Field_102=round(Field_102*0.0001,2),Field_104=round(Field_104*0.0001,2),Field_105=round(Field_105*0.0001,2),Field_106=round(Field_106*0.0001,2),Field_107=round(Field_107*0.0001,2),Field_11=round(Field_11*0.0001,2),Field_111=round(Field_111*0.0001,2),Field_112=round(Field_112*0.0001,2),";
            oledbcomm.CommandText = oledbcomm.CommandText + "Field_113=round(Field_113*0.0001,2),Field_114=round(Field_114*0.0001,2),Field_115=round(Field_115*0.0001,2),Field_116=round(Field_116*0.0001,2),Field_117=round(Field_117*0.0001,2),Field_118=round(Field_118*0.0001,2),Field_119=round(Field_119*0.0001,2),Field_12=round(Field_12*0.0001,2),Field_122=round(Field_122*0.0001,2),";
            oledbcomm.CommandText = oledbcomm.CommandText + "Field_123=round(Field_123*0.0001,2),Field_124=round(Field_124*0.0001,2),Field_125=round(Field_125*0.0001,2),Field_126=round(Field_126*0.0001,2),Field_127=round(Field_127*0.0001,2)";
            oledbcomm.ExecuteNonQuery();
            //按照村进行面积平差
            if (vProgress != null) vProgress.SetProgress("按照村进行面积平差");
            ModTableFun.DropTable(oledbconn, "tmpVillageArea");
            ModTableFun.DropTable(oledbconn, "tmptable");
            ModTableFun.DropTable(oledbconn, "tmpVillageAreamini");

            oledbcomm.CommandText = "select " + ModFieldConst.g_ZLDM + ",sum(TDZMJ) AS QTDZMJ,round(sum(TDZMJ),2) AS HTDZMJ into tmpvillagearea FROM " + ModFieldConst.g_BaseCompuTable + " group by " + ModFieldConst.g_ZLDM;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "alter table tmpvillagearea add column gzs integer";
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "select " + ModFieldConst.g_ZLDM + ",sum(TDZMJ) AS hTDZMJ into tmptable FROM " + ModFieldConst.g_BaseStaticTable + " group by " + ModFieldConst.g_ZLDM;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmpvillagearea a,tmptable b set a.htdzmj=b.htdzmj where a." + ModFieldConst.g_ZLDM + "=b." + ModFieldConst.g_ZLDM;
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "select * into tmpVillageAreamini from tmpvillagearea where htdzmj<=1";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "delete * from tmpvillagearea where htdzmj<=1";
            oledbcomm.ExecuteNonQuery();
            //double  dtdzmj, dtdzmj_r, dkzzmj, dkzzmj_r;
            int VillageCount = 0;
            oledbcomm.CommandText = "";
            sqlstr = "select count(*) from tmpvillagearea";
            myreader = ModTableFun.GetReader(oledbconn, sqlstr);
            if (myreader.Read())
            { VillageCount = int.Parse(myreader.GetValue(0).ToString()); }
            myreader.Close();
            string VillAreaTable;
            VillAreaTable = "tmpvillagearea";
            if (VillageCount == 0)
            {
                sqlstr = "select count(*) from tmpVillageAreamini";
                myreader = ModTableFun.GetReader(oledbconn, sqlstr);
                if (myreader.Read())
                {
                    VillageCount = int.Parse(myreader.GetValue(0).ToString());
                }
                myreader.Close();
                VillAreaTable = "tmpvillageareamini";
            }
            sqlstr = "select sum(tdzmj) from " + ModFieldConst.g_BaseStaticTable;
            myreader = ModTableFun.GetReader(oledbconn, sqlstr);
            if (myreader.Read())
            {
                dtdzmj_r = double.Parse(myreader.GetValue(0).ToString());
            }
            myreader.Close();
            int icount;       //改正数
            icount = Convert.ToInt32((dkzzmj_r - dtdzmj_r) * 100);
            int Shang, Yushu;
            oledbcomm.CommandText = "update " + VillAreaTable + " set gzs=0";
            oledbcomm.ExecuteNonQuery();
            if (icount != 0)
            {

                Shang = icount / VillageCount;
                if (icount > 0)
                { if (Shang * VillageCount > icount)  Shang = Shang - 1; }
                else
                { if (Shang * VillageCount < icount)  Shang = Shang + 1; }

                Yushu = icount % VillageCount;
                //oledbcomm.CommandText = "update " + VillAreaTable + " set gzs=0";
                //oledbcomm.ExecuteNonQuery();
                if (Yushu != 0)
                {
                    ModTableFun.DropTable(oledbconn, "tmpvillagename1");
                    ModTableFun.DropTable(oledbconn, "tmpvillagename");

                    oledbcomm.CommandText = "select top " + Math.Abs(Yushu) + " " + ModFieldConst.g_ZLDM + " into tmpvillagename from " + VillAreaTable + " order by qtdzmj desc";
                    oledbcomm.ExecuteNonQuery();
                    if (icount > 0)
                    {
                        oledbcomm.CommandText = "update " + VillAreaTable + " set htdzmj=htdzmj+0.01,gzs=1 where " + ModFieldConst.g_ZLDM + " in(select " + ModFieldConst.g_ZLDM + " from tmpvillagename)";
                        oledbcomm.ExecuteNonQuery();
                    }
                    else
                    {
                        oledbcomm.CommandText = "update " + VillAreaTable + " set htdzmj=htdzmj-0.01,gzs=-1 where " + ModFieldConst.g_ZLDM + " in(select " + ModFieldConst.g_ZLDM + " from tmpvillagename)";
                        oledbcomm.ExecuteNonQuery();
                    }
                }
                oledbcomm.CommandText = "update " + VillAreaTable + " set htdzmj=htdzmj+" + Shang + "*0.01,gzs=gzs+" + Shang;
                oledbcomm.ExecuteNonQuery();
            }
            //每个村的改正数计算完毕
            //计算基本统计表中每个记录的改正数
            if (vProgress != null) vProgress.SetProgress("基础统计表村内面积平差");
            oledbcomm.CommandText = "alter table " + ModFieldConst.g_BaseStaticTable + " add column gzs_tdzmj integer";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set gzs_tdzmj=0";
            oledbcomm.ExecuteNonQuery();

            sqlstr = "select " + ModFieldConst.g_ZLDM + ",gzs from " + VillAreaTable;
            myreader = ModTableFun.GetReader(oledbconn, sqlstr);

            int gznumber; string tmpZLDM;
            int rowcount = -1;
            OleDbDataReader tmpReader;
            while (myreader.Read())
            {
                gznumber = int.Parse(myreader.GetValue(1).ToString());
                tmpZLDM = myreader.GetValue(0).ToString();
                sqlstr = "SELECT COUNT(*) FROM " + ModFieldConst.g_BaseStaticTable + " WHERE " + ModFieldConst.g_ZLDM + "='" + tmpZLDM + "' and tdzmj>1";
                tmpReader = ModTableFun.GetReader(oledbconn, sqlstr);
                if (tmpReader.Read())
                {
                    rowcount = int.Parse(tmpReader.GetValue(0).ToString());
                }
                tmpReader.Close();
                ModTableFun.DropTable(oledbconn, "tmpchangerow");
                if (rowcount > 0)
                {
                    oledbcomm.CommandText = "select id,tdzmj into tmpchangerow from " + ModFieldConst.g_BaseStaticTable + " where " + ModFieldConst.g_ZLDM + "='" + tmpZLDM + "' and tdzmj>1";
                    oledbcomm.ExecuteNonQuery();
                }
                else
                {
                    oledbcomm.CommandText = "select id,tdzmj into tmpchangerow from " + ModFieldConst.g_BaseStaticTable + " where " + ModFieldConst.g_ZLDM + "='" + tmpZLDM + "'";
                    oledbcomm.ExecuteNonQuery();
                }
                sqlstr = "SELECT COUNT(*) FROM tmpchangerow";
                tmpReader = ModTableFun.GetReader(oledbconn, sqlstr);
                if (tmpReader.Read())
                {
                    rowcount = int.Parse(tmpReader.GetValue(0).ToString());
                }
                tmpReader.Close();
                Shang = gznumber / rowcount;
                if (gznumber > 0)
                {
                    if (Shang * rowcount > gznumber) Shang = Shang - 1;
                }
                else
                {
                    if (Shang * rowcount < gznumber) Shang = Shang + 1;
                }

                Yushu = gznumber % rowcount;
                if (Yushu != 0)
                {
                    ModTableFun.DropTable(oledbconn, "tmpid1");
                    ModTableFun.DropTable(oledbconn, "tmpid");

                    oledbcomm.CommandText = "select top " + Math.Abs(Yushu) + " id into tmpid from tmpchangerow order by tdzmj desc";
                    oledbcomm.ExecuteNonQuery();

                    if (gznumber > 0)
                    {
                        oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set tdzmj=tdzmj+0.01,gzs_tdzmj=1 where id in(select id from tmpid)";
                        oledbcomm.ExecuteNonQuery();
                    }
                    else
                    {
                        if (gznumber < 0)
                        {
                            oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set tdzmj=tdzmj-0.01,gzs_tdzmj=-1 where id in(select id from tmpid)";
                            oledbcomm.ExecuteNonQuery();
                        }
                    }
                    ModTableFun.DropTable(oledbconn, "tmpid");
                    ModTableFun.DropTable(oledbconn, "tmpid1");
                    ModTableFun.DropTable(oledbconn, "tmpidmini");
                }
                oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set tdzmj=tdzmj+" + Shang + "*0.01,gzs_tdzmj=gzs_tdzmj+" + Shang + " where id in(select id from tmpchangerow)";
                oledbcomm.ExecuteNonQuery();
            }
            myreader.Close();
            //基础统计表中森林总面积平差完毕
            //对基础统计表中各个二级地类进行面积平差
            if (vProgress != null) vProgress.SetProgress("基础统计表二级地类面积平差");
            oledbcomm.CommandText = "alter table " + ModFieldConst.g_BaseStaticTable + " add column gzs integer";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_01 = " + ModFieldConst.m_sum01;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_02 = " + ModFieldConst.m_sum02;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_03 = " + ModFieldConst.m_sum03;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_04 = " + ModFieldConst.m_sum04;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_10 = " + ModFieldConst.m_sum10;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_11 = " + ModFieldConst.m_sum11;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_12 = " + ModFieldConst.m_sum12;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_20 = " + ModFieldConst.m_sum20;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ModFieldConst.g_BaseStaticTable + " set gzs=((tdzmj-(Field_01+Field_02+Field_03+Field_04+Field_20+Field_10+Field_11+Field_12))*100)";
            oledbcomm.ExecuteNonQuery();
            AdjustBaseStaticColumn(oledbconn, ModFieldConst.g_BaseStaticTable, ModFieldConst.g_BaseCompuTable);

            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_01 = " + ModFieldConst.m_sum01;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_02 = " + ModFieldConst.m_sum02;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_03 = " + ModFieldConst.m_sum03;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_04 = " + ModFieldConst.m_sum04;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_10 = " + ModFieldConst.m_sum10;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_11 = " + ModFieldConst.m_sum11;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_12 = " + ModFieldConst.m_sum12;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "Update " + ModFieldConst.g_BaseStaticTable + " Set Field_20 = " + ModFieldConst.m_sum20;
            oledbcomm.ExecuteNonQuery();
            oledbconn.Close();
            //if (vProgress != null) vProgress.Close();
            //MessageBox.Show("森林资源现状汇总成功！");

        }
        public static void ComputeXZDWMJ(string WorkSpaceName,string strDLTB,string strXZDW,out string  resDLTB )
        {
            resDLTB = "";
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + WorkSpaceName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            try
            {
                oledbconn.Open();
                OleDbCommand oledbcomm = oledbconn.CreateCommand();
                //处理被割成两半的同一个地类图斑
                ModTableFun.DropTable(oledbconn, "tmpdltb");
                oledbcomm.CommandText = "update " + strDLTB + " set " + ModFieldConst.g_TBMJ + "=round(" + ModFieldConst.g_TBMJ + ",1)";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "select " + ModFieldConst.g_TBBH + "," + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_ZLDWMC + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSDWMC + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + ModFieldConst.g_DLBM + "," + ModFieldConst.g_KCDLBM + "," + ModFieldConst.g_TKXS + ",sum(" + ModFieldConst.g_TBMJ + ") as tbmj1,sum(" + ModFieldConst.g_XZDWMJ + ") as xzmj1,sum(" + ModFieldConst.g_LXDWMJ + ") as lxmj1,sum(" + ModFieldConst.g_TKMJ + ") as tkmj1,sum(" + ModFieldConst.g_TBDLMJ + ") as tbdlmj1 into tmpdltb from " + strDLTB + " group by " + ModFieldConst.g_TBBH + "," + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_ZLDWMC + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSDWMC + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + ModFieldConst.g_DLBM + "," + ModFieldConst.g_KCDLBM + "," + ModFieldConst.g_TKXS;
                oledbcomm.ExecuteNonQuery();
                
                ModTableFun.DropTable(oledbconn, "tmpdltb2");
                oledbcomm.CommandText = "select " + ModFieldConst.g_TBBH + "," + ModFieldConst.g_ZLDM + "," + ModFieldConst.g_ZLDWMC + "," + ModFieldConst.g_QSDM + "," + ModFieldConst.g_QSDWMC + "," + ModFieldConst.g_QSXZ + "," + ModFieldConst.g_GDLX + "," + ModFieldConst.g_PDJB + "," + ModFieldConst.g_DLBM + "," + ModFieldConst.g_KCDLBM + "," + ModFieldConst.g_TKXS + ",tbmj1 as " + ModFieldConst.g_TBMJ + ", xzmj1 as " + ModFieldConst.g_XZDWMJ + ",lxmj1 as " + ModFieldConst.g_LXDWMJ + ",tkmj1 as " + ModFieldConst.g_TKMJ + ",tbdlmj1 as " + ModFieldConst.g_TBDLMJ + " into tmpdltb2 from tmpdltb";
                oledbcomm.ExecuteNonQuery();
                resDLTB = "tmpdltb2";
                ModTableFun.DropTable(oledbconn, "tmpxzdwmj");
                oledbcomm.CommandText = "update " + strXZDW + " set shape_length=round(shape_length,1),"+ModFieldConst.g_KuanDu+"=round("+ModFieldConst.g_KuanDu +",1)";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + strXZDW + " set "+ModFieldConst.g_XZDWMJ +"=shape_length*"+ModFieldConst.g_KuanDu;
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "select " + ModFieldConst.g_KCTBBH1 + " as kctbbh," + ModFieldConst.g_KCTBDWDM1 + " as kctbdwdm,sum(" + ModFieldConst.g_XZDWMJ + "*" + ModFieldConst.g_KCBL + ") as xzmj into tmpxzdwmj from " + strXZDW + " group by " + ModFieldConst.g_KCTBBH1 + "," + ModFieldConst.g_KCTBDWDM1;
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "insert into tmpxzdwmj select " + ModFieldConst.g_KCTBBH2 + " as kctbbh," + ModFieldConst.g_KCTBDWDM2 + " as kctbdwdm,sum(" + ModFieldConst.g_XZDWMJ + "*(1-" + ModFieldConst.g_KCBL + ")) as xzmj from " + strXZDW + " group by " + ModFieldConst.g_KCTBBH2 + "," + ModFieldConst.g_KCTBDWDM2;
                oledbcomm.ExecuteNonQuery();
                ModTableFun.DropTable(oledbconn, "tmpxzdwmj2");
                oledbcomm.CommandText = "select kctbbh,kctbdwdm,sum(xzmj) as xzmj2 into tmpxzdwmj2 from tmpxzdwmj group by kctbbh,kctbdwdm";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " set " + ModFieldConst.g_XZDWMJ + "=0," + ModFieldConst.g_LXDWMJ + "=0," + ModFieldConst.g_TKMJ + "=0";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " a,tmpxzdwmj2 b set a." + ModFieldConst.g_XZDWMJ + "=b.xzmj2 where a." + ModFieldConst.g_TBBH + "=b.kctbbh and a." + ModFieldConst.g_ZLDM + "=b.kctbdwdm";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " set " + ModFieldConst.g_TBDLMJ + "=(" + ModFieldConst.g_TBMJ + "-" + ModFieldConst.g_XZDWMJ + "-" + ModFieldConst.g_LXDWMJ + ")";
                oledbcomm.ExecuteNonQuery();
                //令图斑地类面积小于零的线不再扣除
                oledbcomm.CommandText = "update " + strXZDW + " a," + resDLTB + " b set a." + ModFieldConst.g_KCTBBH1 + "='',a." + ModFieldConst.g_KCTBDWDM1 + "='' where a." + ModFieldConst.g_KCTBBH1 + "=b." + ModFieldConst.g_TBBH + " and a." + ModFieldConst.g_KCTBDWDM1 + "=b." + ModFieldConst.g_ZLDM + " and b." + ModFieldConst.g_TBDLMJ + "<0";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + strXZDW + " a," + resDLTB + " b set a." + ModFieldConst.g_KCTBBH2 + "='',a." + ModFieldConst.g_KCTBDWDM2 + "='' where a." + ModFieldConst.g_KCTBBH2 + "=b." + ModFieldConst.g_TBBH + " and a." + ModFieldConst.g_KCTBDWDM2 + "=b." + ModFieldConst.g_ZLDM + " and b." + ModFieldConst.g_TBDLMJ + "<0";
                oledbcomm.ExecuteNonQuery();
                //重新计算线状地物面积
                ModTableFun.DropTable(oledbconn, "tmpxzdwmj");
                oledbcomm.CommandText = "update " + strXZDW + " set shape_length=round(shape_length,1)," + ModFieldConst.g_KuanDu + "=round(" + ModFieldConst.g_KuanDu + ",1)";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + strXZDW + " set " + ModFieldConst.g_XZDWMJ + "=shape_length*" + ModFieldConst.g_KuanDu;
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "select " + ModFieldConst.g_KCTBBH1 + " as kctbbh," + ModFieldConst.g_KCTBDWDM1 + " as kctbdwdm,sum(" + ModFieldConst.g_XZDWMJ + "*" + ModFieldConst.g_KCBL + ") as xzmj into tmpxzdwmj from " + strXZDW + " group by " + ModFieldConst.g_KCTBBH1 + "," + ModFieldConst.g_KCTBDWDM1;
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "insert into tmpxzdwmj select " + ModFieldConst.g_KCTBBH2 + " as kctbbh," + ModFieldConst.g_KCTBDWDM2 + " as kctbdwdm,sum(" + ModFieldConst.g_XZDWMJ + "*(1-" + ModFieldConst.g_KCBL + ")) as xzmj from " + strXZDW + " group by " + ModFieldConst.g_KCTBBH2 + "," + ModFieldConst.g_KCTBDWDM2;
                oledbcomm.ExecuteNonQuery();
                ModTableFun.DropTable(oledbconn, "tmpxzdwmj2");
                oledbcomm.CommandText = "select kctbbh,kctbdwdm,-1 as tag,sum(xzmj) as xzmj2 into tmpxzdwmj2 from tmpxzdwmj group by kctbbh,kctbdwdm";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " set " + ModFieldConst.g_XZDWMJ + "=0," + ModFieldConst.g_LXDWMJ + "=0," + ModFieldConst.g_TKMJ + "=0";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " a,tmpxzdwmj2 b set a." + ModFieldConst.g_XZDWMJ + "=b.xzmj2 where a." + ModFieldConst.g_TBBH + "=b.kctbbh and a." + ModFieldConst.g_ZLDM + "=b.kctbdwdm";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " a,tmpxzdwmj2 b set b.tag=1 where a." + ModFieldConst.g_TBBH + "=b.kctbbh and a." + ModFieldConst.g_ZLDM + "=b.kctbdwdm";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " set " + ModFieldConst.g_TKMJ + "=" + ModFieldConst.g_TKXS + "*0.01*(" + ModFieldConst.g_TBMJ + "-" + ModFieldConst.g_XZDWMJ + "-" + ModFieldConst.g_LXDWMJ + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + resDLTB + " set " + ModFieldConst.g_TBDLMJ + "=" + ModFieldConst.g_TBMJ + "-" + ModFieldConst.g_XZDWMJ + "-" + ModFieldConst.g_LXDWMJ + "-" + ModFieldConst.g_TKMJ;
                oledbcomm.ExecuteNonQuery();
                //oledbcomm.CommandText = "update " + strDLTB + " set " + ModFieldConst.g_TBMJ + "=0 where " + ModFieldConst.g_TBMJ + "<0";
                //oledbcomm.ExecuteNonQuery();
                //不在地类图斑内的线状地物  处理扣除属性
                oledbcomm.CommandText = "update " + strXZDW + " a,tmpxzdwmj2 b set a." + ModFieldConst.g_KCTBBH1 + "='',a." + ModFieldConst.g_KCTBDWDM1 + "='' where a." + ModFieldConst.g_KCTBBH1 + "=b.kctbbh and a.kctbdwdm1=b.kctbdwdm and b.tag=-1";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + strXZDW + " a,tmpxzdwmj2 b set a." + ModFieldConst.g_KCTBBH2 + "='',a." + ModFieldConst.g_KCTBDWDM2 + "='' where a." + ModFieldConst.g_KCTBBH2 + "=b.kctbbh and a." + ModFieldConst.g_KCTBDWDM2 + "=b.kctbdwdm and b.tag=-1";
                oledbcomm.ExecuteNonQuery();
                oledbconn.Close();
            }
            catch(Exception err)
            {
                if (oledbconn.State==ConnectionState.Open)
                {
                    oledbconn.Close();
                }
            }
            oledbconn = null;

        }
        public static void LandUseCurReport(string DataSourcePath,string DataSourceName, string xzqCode, string AreaUnitName,int FractionNum, int iDLJB,string excelPath,SysCommon.CProgress vProgress)
        {
            //int FractionNum = 2;
            int iXZJB = 3;

            string substr = "mid";
            string strim = "trim";
            string varchar2 = "text";

            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataSourcePath + "\\" + DataSourceName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            if (vProgress!=null) vProgress.SetProgress("查找基础统计表数据");
            if (!ModTableFun.isExist(oledbconn,ModFieldConst.g_BaseStaticTable))
            {
                if (vProgress != null)  vProgress.Close();
                MessageBox.Show("未找到基础统计表数据!", "提示");                
                return;
            }
            string delstr = "*";
            //ModTableFun.DropTable(oledbconn, ModFieldConst.g_XZQBtable);

            //GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //string dbInfopath = dIndex.GetDbInfo();
            //try
            //{
            //    oledbcomm.CommandText = "select * into " + ModFieldConst.g_XZQBtable + " from [" + dbInfopath + "]." + ModFieldConst.g_XZQBtable;
            //    oledbcomm.ExecuteNonQuery();
            //}
            //catch
            //{
            //    if (vProgress != null)  vProgress.Close();
            //    MessageBox.Show("读取行政区字典失败!","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);                
            //    return;
            //}
            if (vProgress != null)  vProgress.SetProgress("读取行政区字典");
            //从业务库拷贝行政区字典 县级至村级
            //CopyTableToAccess g_ywconn, conn, g_XZQBtable, , g_XZBM + "," + g_XZQM + "," + g_XZJB + "," + g_SFZL


            //判断从哪个基础表获取数据  基础计算表或基础统计表
            string BaseTableName;
            switch (AreaUnitName)
            {
                case "平方米":
                    BaseTableName = ModFieldConst.g_BaseCompuTable;
                    break;
                case "公顷":
                    BaseTableName = ModFieldConst.g_BaseStaticTable;
                    //FractionNum = 4;
                    break;
                case "亩":
                    BaseTableName = ModFieldConst.g_BaseCompuTable;
                    break;
                default:
                    BaseTableName = ModFieldConst.g_BaseStaticTable;
                    //FractionNum = 2;
                    break;

            }


            string ClassifyName;
            ClassifyName = "森林资源分类面积汇总表";
            int[] xzbmcount = new int[3];

            xzbmcount[0] = 6;
            xzbmcount[1] = 9;
            xzbmcount[2] = 12;
            ModTableFun.DropTable(oledbconn, ClassifyName);
            if (vProgress != null)  vProgress.SetProgress("创建森林资源现状分类面积汇总表");
            oledbcomm.CommandText = "select " + ModFieldConst.g_XZQM + "," + ModFieldConst.g_XZBM + " as xzbm1 into " + ClassifyName + " from " + ModFieldConst.g_XZQBtable + " where 1<>1";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "alter table " + ClassifyName + " add column id counter(1,1)";
            oledbcomm.ExecuteNonQuery();
            OleDbDataReader myReader;
            string sqlstr = "select " + ModFieldConst.g_XZBM + "," + ModFieldConst.g_XZQM + " from " + ModFieldConst.g_XZQBtable + " where " + ModFieldConst.g_XZBM + " like '" + xzqCode + "%' and " + ModFieldConst.g_XZJB + "='" + (iXZJB + 1) + "' order by " + ModFieldConst.g_XZBM;// and (" + strim + "(" + g_SFZL + ")='0' or " + strim + "(" + g_SFZL + ") is null)
            myReader = ModTableFun.GetReader(oledbconn, sqlstr);
            string tmpxzbm;
            int idcount;
            OleDbDataReader tmpReader;
            //初始化行政区代码
            if (vProgress != null)  vProgress.SetProgress("初始化行政区代码");
            idcount = 1;

            oledbcomm.CommandText = "insert into " + ClassifyName + " select " + ModFieldConst.g_XZQM + ",mid(" + ModFieldConst.g_XZBM + ",1," + xzbmcount[0] + ") as xzbm1 from " + ModFieldConst.g_XZQBtable + " where " + ModFieldConst.g_XZBM + " like '" + xzqCode + "%' and " + ModFieldConst.g_XZJB + "='" + iXZJB + "'";// and (" + strim + "(" + g_SFZL + ")='0' or " + strim + "(" + g_SFZL + ") is null)";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "insert into " + ClassifyName + " values(null,null," + idcount + ")";
            oledbcomm.ExecuteNonQuery();
            idcount = idcount + 2;

            while (myReader.Read())
            {
                tmpxzbm = myReader.GetValue(0).ToString().Substring(0, xzbmcount[1]);

                oledbcomm.CommandText = "insert into " + ClassifyName + " values('" + myReader.GetValue(1).ToString() + "','" + tmpxzbm + "'," + idcount + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "insert into " + ClassifyName + " select " + ModFieldConst.g_XZQM + ",mid(" + ModFieldConst.g_XZBM + ",1," + xzbmcount[2] + ") as xzbm1 from " + ModFieldConst.g_XZQBtable + " where " + ModFieldConst.g_XZBM + " like '" + tmpxzbm + "%' and " + ModFieldConst.g_XZJB + "='" + (iXZJB + 2) + "'";// and (" + strim + "(" + g_SFZL + ")='0' or " + strim + "(" + g_SFZL + ") is null)";
                oledbcomm.ExecuteNonQuery();
                sqlstr = "select count(xzbm1) from " + ClassifyName;
                tmpReader = ModTableFun.GetReader(oledbconn, sqlstr);
                if (tmpReader.Read()) idcount = int.Parse(tmpReader.GetValue(0).ToString()) + 1;
                tmpReader.Close();
                oledbcomm.CommandText = "insert into " + ClassifyName + " values(null,null," + idcount + ")";
                oledbcomm.ExecuteNonQuery();
            }
            myReader.Close();
            ModTableFun.DropTable(oledbconn, "tmptmpxzbm");
            
            string fieldsname = "";         //add fields 时使用
            string updatestr = "";          //update ...set ...=0时使用
            string tmpfields = "";          //选取列名列举在表单中使用
            string sumFields = "";          //sum(fields_*) as fields_*tmp
            string[] FieldsArray = new string[50];
            int ArrayCount;
            ArrayCount = InitFieldsNameArraybyXml(FieldsArray, "农村森林资源现状分类面积汇总表", iDLJB);
            string FieldsType = "";
            FieldsType = "double";
            for (int i = 0; i < ArrayCount; i++)
            {
                fieldsname = fieldsname + FieldsArray[i] + " " + FieldsType + ",";
                updatestr = updatestr + FieldsArray[i] + "=0,";
                tmpfields = tmpfields + FieldsArray[i] + ",";
                if (AreaUnitName == "亩")
                {
                    sumFields = sumFields + "round(sum(" + FieldsArray[i] + ")*3/2000," + FractionNum + ") as " + FieldsArray[i] + "tmp,";
                }
                else
                {
                    sumFields = sumFields + "round(sum(" + FieldsArray[i] + ")," + FractionNum + ") as " + FieldsArray[i] + "tmp,";
                }
            }
            fieldsname = fieldsname.Substring(0, fieldsname.Length - 1);
            updatestr = updatestr.Substring(0, updatestr.Length - 1);
            tmpfields = tmpfields.Substring(0, tmpfields.Length - 1);
            sumFields = sumFields.Substring(0, sumFields.Length - 1);

            oledbcomm.CommandText = "alter table " + ClassifyName + " add column sum0 double," + fieldsname;
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ClassifyName + " set " + updatestr + " where " + strim + "(xzbm1) is not null";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ClassifyName + " set sum0=0 where " + strim + "(xzbm1) is not null";
            oledbcomm.ExecuteNonQuery();
            string setabFields_access = "";

            for (int i = 0; i < ArrayCount; i++)
            {
                setabFields_access = setabFields_access + "a." + FieldsArray[i] + "=b." + FieldsArray[i] + "tmp,";
            }
            setabFields_access = setabFields_access.Substring(0, setabFields_access.Length - 1);

            ModTableFun.DropTable(oledbconn, "tmpClassify0");
            ModTableFun.DropTable(oledbconn, "tmpClassify1");
            ModTableFun.DropTable(oledbconn, "tmpClassify2");


            oledbcomm.CommandText = "select " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[2] + ") as zldm1,round(sum(tdzmj)," + FractionNum + ") as sum0_tmp," + sumFields + " into tmpClassify2 from " + BaseTableName + " group by " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[2] + ")";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ClassifyName + " a,tmpClassify2 b set " + setabFields_access + ",a.sum0=b.sum0_tmp where a.xzbm1=b.zldm1";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "select " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[1] + ") as zldm1,round(sum(tdzmj),"+FractionNum+") as sum0_tmp," + sumFields + " into tmpClassify1 from " + BaseTableName + " group by " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[1] + ")";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ClassifyName + " a,tmpClassify1 b set " + setabFields_access + ",a.sum0=b.sum0_tmp where a.xzbm1=b.zldm1";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "select " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[0] + ") as zldm1,round(sum(tdzmj),"+FractionNum +") as sum0_tmp," + sumFields + " into tmpClassify0 from " + BaseTableName + " group by " + substr + "(" + ModFieldConst.g_ZLDM + ",1," + xzbmcount[0] + ")";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update " + ClassifyName + " a,tmpClassify0 b set " + setabFields_access + ",a.sum0=b.sum0_tmp where a.xzbm1=b.zldm1";
            oledbcomm.ExecuteNonQuery();

            if (iDLJB == 2)
            {
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_01=round(" + ModFieldConst.m_sum01 + ","+FractionNum+")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_02=round(" + ModFieldConst.m_sum02 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_03=round(" + ModFieldConst.m_sum03 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_04=round(" + ModFieldConst.m_sum04 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_10=round(" + ModFieldConst.m_sum10 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_11=round(" + ModFieldConst.m_sum11 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_12=round(" + ModFieldConst.m_sum12 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
                oledbcomm.CommandText = "update " + ClassifyName + " set Field_20=round(" + ModFieldConst.m_sum20 + "," + FractionNum + ")";
                oledbcomm.ExecuteNonQuery();
            }
            oledbcomm.CommandText = "update " + ClassifyName + " set sum0=round(" + ModFieldConst.m_sumsum + "," + FractionNum + ")";
            oledbcomm.ExecuteNonQuery();
            ModTableFun.DropTable(oledbconn, "tmpClassify0");
            ModTableFun.DropTable(oledbconn, "tmpClassify1");
            ModTableFun.DropTable(oledbconn, "tmpClassify2");

            oledbcomm.CommandText = "delete " + delstr + " from " + ClassifyName + " where sum0=0";
            oledbcomm.ExecuteNonQuery();
            //删除面积为空的行
            if (vProgress != null)  vProgress.SetProgress("对统计数据进行处理");
            //DeleteNullCol conn, ClassifyName, "xzbm1", "ID", dbtype
            DeleteNullCol(oledbconn, ClassifyName, "xzbm1", "ID", "access");
            oledbconn.Close();
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;

            if (iDLJB == 1)
            {
                string Templatepath = Application.StartupPath + "\\..\\Template\\森林资源分类面积汇总表.cel";
                //生成报表对话框
                
                frm = ModFlexcell.SendDataToFlexcell(connstr, "农村森林资源现状一级分类面积汇总表", ClassifyName,ModFieldConst.g_XZQM + ",xzbm1,sum0," + tmpfields, "", Templatepath, 5, 1);
                ModFlexcell.m_SpecialRow = 2;
                
                AxFlexCell.AxGrid pGrid = frm.GetGrid();
                pGrid.Cell(2, 1).Text = "单位：" + AreaUnitName + "("+GetFractionStr(FractionNum)+")";
                pGrid.Cell(2, 1).Alignment = FlexCell.AlignmentConstants.cellRightCenter;
                pGrid.Range(2, 1, 2, 1).FontSize = 9;
                //excelPath = DataSourcePath + "\\" + DataSourceName.Substring(0, DataSourceName.IndexOf(".")) + "农村森林资源现状一级分类面积汇总表.xls";
                pGrid.ExportToExcel(excelPath);
                pGrid.RemoveChart(pGrid.Rows - 1, pGrid.Cols - 1);
                //弹出报表
            }
            else
            {
                string Templatepath = Application.StartupPath + "\\..\\Template\\森林资源二级分类面积汇总表.cel";
                //生成报表对话框
                frm = ModFlexcell.SendDataToFlexcell(connstr, "农村森林资源现状二级分类面积汇总表", ClassifyName, ModFieldConst.g_XZQM + ",xzbm1," + tmpfields, "", Templatepath, 5, 1);
                ModFlexcell.m_SpecialRow = 2;
                AxFlexCell.AxGrid pGrid = frm.GetGrid();
                pGrid.Cell(2, 1).Text = "单位：" + AreaUnitName + "("+GetFractionStr(FractionNum)+")";

                pGrid.Cell(2, 1).Alignment = FlexCell.AlignmentConstants.cellRightCenter;
                pGrid.Range(2, 1, 2, 1).FontName = "宋体";
                pGrid.Range(2, 1, 2, 1).FontSize = 9;
                
                //excelPath = DataSourcePath + "\\" + DataSourceName.Substring(0, DataSourceName.IndexOf(".")) + "农村森林资源现状二级分类面积汇总表.xls";
                pGrid.ExportToExcel(excelPath);
                pGrid.RemoveChart(pGrid.Rows - 1, pGrid.Cols - 1);
               
                //弹出报表
            }
            frm.ReleaseCell();
            frm = null;
            //if (vProgress != null)  vProgress.Close();
            //OpenExcelFile(excelPath);

        }
        private static string GetFractionStr(int FractionNum)
        {
            if (FractionNum == 0) return "0";
            string strFraction="";
            for (int i = 0; i < FractionNum; i++)
            {
                strFraction += "0";
            }
            strFraction = "0." + strFraction;
            return strFraction;
        }
        private static ILayer GetLayerByName( IMap pMap ,string GroupName, string LayerName)
        {
            if (pMap == null) return null;
            if (GroupName.Equals("")) return null;
            if (LayerName.Equals("")) return null;

            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == GroupName)//通过比对找到专题对应图层组
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        IFeatureLayer pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer == null)
                            continue;
                        if (pFeatureLayer.FeatureClass == null)
                            continue;
                        if (pFeatureLayer.Name == LayerName)    //在图层组内，通过比对找到图层
                        {
                            return pFeatureLayer as ILayer;
                        }
                    }
                }
            }
            return null;
        }


        public static int InitFieldsNameArraybyXml(string[] FieldArray, string TableName, int iDLJB)
        {
            XmlDocument xmldoc = new XmlDocument();
            string strReportFile = Application.StartupPath + "\\..\\Template\\ReportConfig.xml";
            XmlDocument xmlCurdoc = new XmlDocument();
            string strTableExp = "//ReportTable [@Name='" + TableName + "' and @Grade='" + iDLJB + "']";
            XmlNode xmlTableNode;
            if (File.Exists(strReportFile) == false)
                return -1;
            xmldoc.Load(strReportFile);
            xmlTableNode = xmldoc.SelectSingleNode(strTableExp);
            if (xmlTableNode == null)
                return -1;
            XmlNodeList xmllist = xmlTableNode.ChildNodes;
            XmlElement pEle;
            string FieldName = "";
            int icount = 0;
            foreach (XmlNode fieldnode in xmllist)
            {
                if (fieldnode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                pEle = fieldnode as XmlElement;
                FieldName = "Field_" + pEle.GetAttribute("Name");
                FieldArray[icount] = FieldName;
                icount++;

            }
            return icount;
        }
        public static void DeleteNullCol(OleDbConnection conn, string tablename, string NullColName, string IDCol, string dbtype)
        {
            if (conn == null)
                return;

            OleDbCommand oledbcomm = conn.CreateCommand();
            if (ModTableFun.isFieldExist(conn, tablename, NullColName) == false)
                return;
            if (ModTableFun.isFieldExist(conn, tablename, IDCol) == false)
                return;
            ModTableFun.DropTable(conn, "tmpNullID");

            oledbcomm.CommandText = "select " + IDCol + " into tmpNullID from " + tablename + " where 1<>1";
            oledbcomm.ExecuteNonQuery();
            string sqlstr = "select " + IDCol + "," + NullColName + " from " + tablename + " order by " + IDCol + " asc";
            OleDbDataReader myreader = ModTableFun.GetReader(conn, sqlstr);
            int nullcount = 0;
            while (myreader.Read())
            {
                if (myreader.GetValue(1).ToString().Equals(""))
                {
                    nullcount = nullcount + 1;
                    if (nullcount > 1)
                    {
                        oledbcomm.CommandText = "insert into tmpNullID values(" + myreader.GetValue(0).ToString() + ")";
                        oledbcomm.ExecuteNonQuery();
                    }
                }
                else
                {
                    nullcount = 0;
                }
            }
            myreader.Close();



            oledbcomm.CommandText = "delete * from " + tablename + " where " + IDCol + " in(select " + IDCol + " from tmpnullid)";
            oledbcomm.ExecuteNonQuery();

        }
        //专门负责基础统计表的二级地类调平
        private static void AdjustBaseStaticColumn(OleDbConnection conn,string BaseStaticName,string  BaseComputName)
        {
            string[] FieldsArray=new string[38];
            FieldsArray[0] = "Field_127";
            FieldsArray[1] = "Field_126";
            FieldsArray[2] = "Field_125";
            FieldsArray[3] = "Field_124";
            FieldsArray[4] = "Field_123";
            FieldsArray[5] = "Field_122";
            FieldsArray[6] = "Field_119";
            FieldsArray[7] = "Field_118";
            FieldsArray[8] = "Field_117";
            FieldsArray[9] = "Field_116";
            FieldsArray[10] = "Field_115";
            FieldsArray[11] = "Field_114";
            FieldsArray[12] = "Field_113";
            FieldsArray[13] = "Field_112";
            FieldsArray[14] = "Field_111";
            FieldsArray[15] = "Field_107";
            FieldsArray[16] = "Field_106";
            FieldsArray[17] = "Field_105";
            FieldsArray[18] = "Field_104";
            FieldsArray[19] = "Field_102";
            FieldsArray[20] = "Field_101";
            FieldsArray[21] = "Field_205";
            FieldsArray[22] = "Field_204";
            FieldsArray[23] = "Field_203";
            FieldsArray[24] = "Field_202";
            FieldsArray[25] = "Field_201";
            FieldsArray[26] = "Field_043";
            FieldsArray[27] = "Field_042";
            FieldsArray[28] = "Field_041";
            FieldsArray[29] = "Field_033";
            FieldsArray[30] = "Field_032";
            FieldsArray[31] = "Field_031";
            FieldsArray[32] = "Field_023";
            FieldsArray[33] = "Field_022";
            FieldsArray[34] = "Field_021";
            FieldsArray[35] = "Field_013";
            FieldsArray[36] = "Field_012";
            FieldsArray[37] = "Field_011";
            OleDbCommand myComm=conn.CreateCommand();            
            string sqlstr="select id,gzs from " + BaseStaticName;
            OleDbDataReader myReader=ModTableFun.GetReader(conn,sqlstr);
         
            int Lid;
            int colcount;
            int Lgzs;
            int Shang, Yushu;
            int i;
            while(myReader.Read())
            {
                Lid=int.Parse(myReader.GetValue(0).ToString());
                Lgzs=int.Parse(myReader.GetValue(1).ToString());
                string[] MaxArray=new string[38];
                if(Lgzs!=0)
                {
                    colcount = GetMaxColumn(conn, BaseStaticName, BaseComputName, FieldsArray, MaxArray, Lid);
                    if (colcount > 0)
                    {
                        Shang = Lgzs / colcount;
                        if (Lgzs > 0)
                        {
                            if (Shang * colcount > Lgzs) Shang = Shang - 1;
                        }
                        else
                        {
                            if (Shang * colcount < Lgzs) Shang = Shang + 1;
                        }
                        Yushu = Lgzs % colcount;
                        for(i=0;i<Math.Abs (Yushu );i++)
                        {
                            if (Lgzs > 0)
                            {
                                myComm.CommandText = "update " + BaseStaticName + " set " + MaxArray[i] + "=" + MaxArray[i] + "+" + (Shang + 1) + "*0.01 where id=" + Lid;
                                myComm.ExecuteNonQuery();
                            }
                            else
                            {
                                if(Lgzs < 0)
                                {
                                    myComm.CommandText = "update " + BaseStaticName + " set " + MaxArray[i] + "=" + MaxArray[i] + "+" + (Shang - 1) + "*0.01 where id=" + Lid;
                                    myComm.ExecuteNonQuery();
                                }
                            }
                        }
                        for(i = Math.Abs (Yushu);i<colcount;i++)
                        {
                            myComm.CommandText = "update " + BaseStaticName + " set " + MaxArray[i] + "=" + MaxArray[i] + "+" + Shang + "*0.01 where id=" + Lid;
                            myComm.ExecuteNonQuery();
                        }
                    }
                }
            }
            myReader.Close();
        }
        private static int GetMaxColumn(OleDbConnection conn, string BaseStaticName, string BaseComputName, string[] FieldsArray,string[] MaxArray, int id)
        {
            
            OleDbDataReader myReader,tmpReader;
            string sqlstr="select * from " + BaseStaticName + " where id=" + id;
            myReader=ModTableFun.GetReader(conn,sqlstr);

            double tmpvalue;
            int i=0;
            int j=0;
            myReader.Read();
            for(i=0;i<38;i++)
            {
                tmpvalue = double.Parse(myReader[FieldsArray[i]].ToString());
                if(tmpvalue > 1)
                {
                    MaxArray[j] = FieldsArray[i];
                    j=j+1;
                }
            }
            if(j==0)
            {
                for(i=0;i<38;i++)
                {
                    tmpvalue = double.Parse(myReader[FieldsArray[i]].ToString());
                    if( tmpvalue > 0 )
                    {
                        MaxArray[j] = FieldsArray[i];
                        j=j+1;
                    }
                }
            }
            if(j==0)
            {
                sqlstr="select * from " + BaseComputName + " where id=" + id;
                tmpReader=ModTableFun.GetReader(conn,sqlstr);
                for(i=0;i<38;i++)
                {
                    tmpvalue = double.Parse (tmpReader[FieldsArray[i]].ToString ());
                    if( tmpvalue > 0)
                    {
                        MaxArray[j] = FieldsArray[i];
                        j=j+1;
                        myReader.Close();
                        tmpReader.Close();
                        return j;
                    }
                }
                tmpReader.Close();
            }
            myReader.Close();
            return j;
        }
    }
}
