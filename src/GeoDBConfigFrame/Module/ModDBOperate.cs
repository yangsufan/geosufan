using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using System.Data.OleDb;
using System.Windows.Forms;
//using Plugin.Flex;
//using Plugin.Flex;
namespace GeoDBConfigFrame
{
   public static class ModDBOperate
    {
       public static bool boolreturn = true;//监测添加记录返回是否
       public static int oid = 0;//添加记录的ID 
       /// <summary>
        /// 根据条件获得查询值
        /// </summary>
        /// <param name="pWks">工作空间</param>
        /// <param name="strTable1">表名</param>
        /// <param name="pGeo">图形过滤条件</param>
        /// <param name="strWhere">属性过滤条件</param>
        /// <returns></returns>
        public static  DataTable GetQueryTable(IFeatureWorkspace pFeaWks, string strTable1, string strWhere)
        {
            //计数的集合
            IList<string> lstTemp = new List<string>();

            if (pFeaWks == null) return null;
            // shduan 20110731***********************************
            ITable pTable = null;
            try
            {
             pTable = pFeaWks.OpenTable(strTable1);
            }
            catch
            {
            }
            if (pTable == null) return null;
            //end ************************************************

            IQueryFilter pQueryFilter = new SpatialFilterClass();
            pQueryFilter = new QueryFilterClass();
            IQueryFilter pQueryFilter1 = new QueryFilterClass();
            if (!boolreturn)//判断添加记录是取消，ID相同就删除该行以免造成取消也添加了记录 xisheng  20110922
            {
                pQueryFilter1.WhereClause="ID='"+oid+"'";
                pTable.DeleteSearchedRows(pQueryFilter1);
                boolreturn = true;
            }
            if (strWhere != "")
            {
                pQueryFilter.WhereClause = strWhere;
            }

            ICursor pCursor = pTable.Search(pQueryFilter, true);
            if (pCursor == null) return null;

            //先创建个表
            DataTable dt = new DataTable();
            for (int i = 0; i < pCursor.Fields.FieldCount; i++)
            {
                IField pfield = pCursor.Fields.get_Field(i);
                string strFieldName = pfield.Name;
                string strFieldText = pfield.AliasName;
                //字段名称
                strFieldName = strFieldName.Substring(strFieldName.IndexOf('.') + 1);
                if (dt.Columns.Contains(strFieldName)) continue;
                DataColumn dc = new DataColumn(strFieldName);
                dc.Caption = strFieldText;
                dc.ColumnName = strFieldText;//给列名改成别名 xisheng
                dt.Columns.Add(dc);
            }

            dt.BeginLoadData();
            //取值
            try//这个错误处理是有问题的 需要对程序进行改进 chenxinwei
            {
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    //创建一行
                    DataRow dr = dt.NewRow();
                    //获得每一个字段的值
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        //查找表中对应的字段
                        string strFieldName = pRow.Fields.get_Field(i).AliasName;
                        strFieldName = strFieldName.Substring(strFieldName.IndexOf('.') + 1);
                        if (!lstTemp.Contains(strFieldName))
                        lstTemp.Add(strFieldName);

                        object objValue = pRow.get_Value(i);
                        if (objValue == null) continue;

                        //获得值
                        dr[strFieldName] = objValue;
                    }

                    lstTemp.Clear();
                    dt.Rows.Add(dr);

                    pRow = pCursor.NextRow();
                }
            }
            catch
            {
            }

            dt.EndLoadData();

            //释放指针
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            pCursor = null;

            //返回
            lstTemp = null;
            return dt;
        }
		public static void ExportTableToExcel(string TableName, string ExcelFileName)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false;
            vProgress.TopMost = true;
            vProgress.MaxValue = 5;
            vProgress.ShowProgress();
            vProgress.SetProgress("导出Excel文件");
          //  FormFlexcell pfrm = new FormFlexcell();
          //  ModFlexcell.SendTableToExcel(pfrm, Plugin.ModuleCommon.TmpWorkSpace, TableName, "", ExcelFileName);
            //vProgress.SetProgress("打开导出的Excel文件");
            //OpenExcelFile(ExcelFileName);
            vProgress.Close();
            MessageBox.Show("导出成功!");

        }
        //打开excel文档
        private static void OpenExcelFile(string filepath)
        {
            //Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            //object MissingValue = Type.Missing;
            //Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks._Open(filepath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            //Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
            //xApp.Visible = true;

        }
       //added by chulili 20110715
       //导入excel文件，内容写入数据字典所选表格中
       //excel文件第一行对应表格的字段名称（别名），第二行开始是记录内容
        public static void ImportExcelToTableEx(string strFilename, string strTablename, bool isCovered)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false;
            vProgress.TopMost = true;
            vProgress.MaxValue = 5;
            vProgress.ShowProgress();
            vProgress.SetProgress("导入Excel表格");
            IFeatureWorkspace pFeatureWks = null;
            ITable pTable = null;
            try
            {
                pFeatureWks = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
                pTable = pFeatureWks.OpenTable(strTablename);
            }
            catch
            {
                vProgress.Close();
                return;
            }
            if (pTable == null)
            {
                vProgress.Close();
                return;
            }
            string strConn;
            vProgress.SetProgress(1, "打开Excel文件");
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strFilename + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            //获取EXCEL文件内工作表名称  added by chulili 20110924
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = "Sheet1$";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    SheetName = dt.Rows[0][2].ToString();//默认取第一张工作表
                }
            }
            dt = null;
            OleDbCommand pCommand = null;
            OleDbDataReader pReader = null;
            try
            {
                vProgress.SetProgress("读取Excel文件表格内容");
                pCommand = new OleDbCommand("SELECT * FROM [" + SheetName + "]", conn);
                pReader = pCommand.ExecuteReader();
            }
            catch
            {
                conn.Close();
                vProgress.Close();
                return;
            }
            if (pReader == null)
            {
                conn.Close();
                vProgress.Close();
                return;
            }
            Dictionary<int, int> pDicFieldname = new Dictionary<int, int>();
            for (int i = 0; i < pReader.FieldCount; i++)
            {
                string strFieldname = pReader.GetName(i);
                int j = pTable.Fields.FindFieldByAliasName(strFieldname);
                pDicFieldname.Add(i, j);
            }
            ICursor pCursor = null;
            try
            {
                if (isCovered)
                {
                    vProgress.SetProgress("清除'" + strTablename + "'中记录");
                    pTable.DeleteSearchedRows(null);
                }
                vProgress.SetProgress("将Excel中表格内容写入'" + strTablename + "'");
                
                pCursor = pTable.Insert(true);
                int Rowcnt = 0;
                while (pReader.Read())
                {
                    IRowBuffer  pRowbuffer = pTable.CreateRowBuffer();
                    for (int i = 0; i < pReader.FieldCount; i++)
                    {
                        try
                        {
                            int index = pDicFieldname[i];
                            if (index < 0)
                            {
                                continue;
                            }
                            //OID列不允许赋值
                            if (pRowbuffer.Fields.get_Field(index).Type == esriFieldType.esriFieldTypeOID)
                            {
                                continue;
                            }
                            object objText = null;
                            if (pReader[i].ToString() != "")
                            {
                                objText = pReader[i].ToString();
                            }
                            if (index >= 0)
                            {
                                if (objText != null)
                                {
                                    pRowbuffer.set_Value(index, objText);
                                }
                                else
                                {
                                    pRowbuffer.set_Value(index, DBNull.Value as object);
                                }                               

                            }
                        }
                        catch (System.Exception ex)
                        {                        	
                        }
                        
                    }
                    pCursor.InsertRow(pRowbuffer);
                    Rowcnt = Rowcnt + 1;                    
                    if (Rowcnt % 1000 == 0)
                    {
                        pCursor.Flush();
                    }                    
                }
                pCursor.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
                pReader.Close();
                conn.Close();
                vProgress.Close();
            }
            catch
            {
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    pCursor = null;
                }                
                pReader.Close();
                conn.Close();
                vProgress.Close();
            }
        }
       //added by chulili 20110715
       //导入excel文件，内容写入数据字典所选表格中
       //excel文件第一行对应表格的字段名称（别名），第二行开始是记录内容
        public static void ImportExcelToTable(string strFilename,string strTablename,bool isCovered)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false ;
            vProgress.TopMost = true;
            vProgress.MaxValue = 5;
            vProgress.ShowProgress();
            vProgress.SetProgress("导入Excel表格");
            IFeatureWorkspace pFeatureWks=null;
            ITable pTable=null;
            try
            {
                pFeatureWks = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
                pTable = pFeatureWks.OpenTable(strTablename);
            }
            catch
            {
                vProgress.Close();
                return;
            }
            if (pTable == null)
            {
                vProgress.Close();
                return;
            }
            string strConn;
            vProgress.SetProgress(1,"打开Excel文件");
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strFilename + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            //获取EXCEL文件内工作表名称  added by chulili 20110924
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = "Sheet1$";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    SheetName = dt.Rows[0][2].ToString();//默认取第一张工作表
                }
            }
            dt = null;
            OleDbCommand pCommand = null;
            OleDbDataReader pReader = null;
            try
            {
                vProgress.SetProgress("读取Excel文件表格内容");
                pCommand = new OleDbCommand("SELECT * FROM ["+SheetName+"]", conn);
                pReader = pCommand.ExecuteReader();
            }
            catch
            {
                conn.Close();
                vProgress.Close();
                return;
            }
            if(pReader==null)
            {
                conn.Close();
                vProgress.Close();
                return;
            }
            //Dictionary<int, int> pDicFieldname = new Dictionary<int, int>();
            //for (int i = 0; i < pReader.FieldCount; i++)
            //{
            //    string strFieldname=pReader.GetName(i);
            //    int j = pTable.Fields.FindFieldByAliasName(strFieldname);
            //    pDicFieldname.Add(i, j);
            //}
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            SysCommon.Gis.SysGisTable pSystable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            try
            {
                if (isCovered)
                {
                    vProgress.SetProgress("清除'"+strTablename+"'中记录");
                    pSystable.DeleteRows(strTablename, "", out eError);
                }
                vProgress.SetProgress("将Excel中表格内容写入'"+strTablename +"'");
                pSystable.StartTransaction(out eError);
                while (pReader.Read())
                {
                    pDic.Clear();
                    for (int i = 0; i < pReader.FieldCount; i++)
                    {
                        pDic.Add(pReader.GetName(i).Trim(), pReader[i].ToString());
                    }
                    pSystable.NewRowByAliasName(strTablename, pDic, out eError); //shduan 20110730暂时屏蔽
                }
                pSystable.EndTransaction(true, out eError);
                pReader.Close();
                conn.Close();
                vProgress.Close();
            }
            catch
            {
                pReader.Close();
                conn.Close();
                vProgress.Close();
            }
 
        }
        public static void ImportTextToTable(string strFileName, string strTablename, bool isCovered)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false;
            vProgress.TopMost = true;
            //vProgress.MaxValue = 5;
            vProgress.ShowProgress();
            vProgress.SetProgress("导入Text文档");
            IFeatureWorkspace pFeatureWks = null;
            ITable pTable = null;
            try
            {
                pFeatureWks = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
                pTable = pFeatureWks.OpenTable(strTablename);
            }
            catch
            {
                vProgress.Close();
                return;
            }
            if (pTable == null)
            {
                vProgress.Close();
                return;
            }
            if (!System.IO.File.Exists(strFileName))
            {
                vProgress.Close();
                return;
            }
            ///记录读取txt中的字段名称
            List<string> pLstFieldName = new List<string>();
            ///记录一行字段存储的字段值与字段名一一对应
            List<string[]> pLstFieldValue = new List<string[]>();
            ///记录读取txt的第几行
            int iNum = 1;
            using (System.IO.StreamReader sr = System.IO.File.OpenText(strFileName))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    string[] strValue = input.Split('\t');
                    if (iNum == 1)
                    {
                        for (int i = 0; i < strValue.Length; i++)
                        {
                            pLstFieldName.Add(strValue[i]);
                        }
                    }
                    else if(strValue.Length!=0)
                    {
                        pLstFieldValue.Add(strValue);
                    }
                    iNum++;
                }
            }
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            SysCommon.Gis.SysGisTable pSystable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            try
            {
                if (isCovered)
                {
                    vProgress.SetProgress("清除'" + strTablename + "'中记录");
                    pSystable.DeleteRows(strTablename, "", out eError);
                }
                vProgress.MaxValue = pLstFieldValue.Count;
                vProgress.SetProgress("将Text文档中内容写入'" + strTablename + "'");
                pSystable.StartTransaction(out eError);
                for (int j = 0; j < pLstFieldValue.Count;j++ )
                {
                    vProgress.Step = j + 1;
                    pDic.Clear();
                    try
                    {
                        for (int n = 0; n < pLstFieldName.Count; n++)
                        {
                            pDic.Add(pLstFieldName[n].ToString(), pLstFieldValue[j][n].ToString());
                        }
                        pSystable.NewRowByAliasName(strTablename, pDic, out eError); //shduan 20110730暂时屏蔽
                    }
                    catch { }
                }
                pSystable.EndTransaction(true, out eError);
                vProgress.Close();
            }
            catch
            {
                MessageBox.Show("导入失败请查看txt文档格式是否正确！","提示！");
                vProgress.Close();
            }
 
        }
        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Plugin.ModuleCommon.DicTabs == null || Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Plugin.Interface.IControlRef> keyValue in Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevComponents.DotNetBar.RibbonTabItem, string> keyValue in Plugin.ModuleCommon.DicTabs)
            {
                if (keyValue.Value == pSysName)
                {
                    i = i + 1;
                    keyValue.Key.Visible = true;
                    keyValue.Key.Enabled = true;
                    if (i == 1)
                    {
                        //默认选中第一项
                        keyValue.Key.Checked = true;
                    }
                }
                else
                {
                    keyValue.Key.Visible = false;
                    keyValue.Key.Enabled = false;
                }
            }
        }

       
    }
}
