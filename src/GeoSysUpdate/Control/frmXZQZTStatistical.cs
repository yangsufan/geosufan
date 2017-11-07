using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using SysCommon;
using GeoPageLayout;
using SysCommon.Error;
using ESRI.ArcGIS.DataSourcesGDB;
using Microsoft.Office.Interop.Excel;
using FanG;
using System.Drawing.Imaging;

namespace GeoSysUpdate
{
    public partial class frmXZQZTStatistical : DevComponents.DotNetBar.Office2007Form
    {
        public readonly static string m_StatisticsPath = System.Windows.Forms.Application.StartupPath + "\\..\\Template\\StatisticsConfig.Xml";
        static string LayerXMLpath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
        private static string XZQXMLPath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\XZQ.xml";
        string StaticUnit = "";
        string WhereCorse = "";
        private IWorkspace m_pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
        Chartlet m_ChartLet = null;
        System.Data.DataTable m_DataTable = null;
        IFeatureClass m_CurrentFeatureClass = null;
       
        public frmXZQZTStatistical(DevComponents.AdvTree.Node SelectNode)
        {
            InitializeComponent();
            Initialifrm(SelectNode);
        }
        public frmXZQZTStatistical(string XZQCode)
        {
            InitializeComponent();
            Initialifrm(XZQCode);
        }
        private void InitializeComType()
        {
            string[] pType = new string[] { "林地质量等级统计", "林地保护等级统计", "林业用地分布统计", "主导功能区分布统计", "林地规划统计", "林地结构统计", "林地利用分布统计", "林场用地分布统计" };
            cmboxType.Items.AddRange(pType);
            List<string> listCust = GetFiledValues("CUSTOMSTATISTICS", "STATISTICSNAME");
            if (listCust.Count > 0)
            {
                for (int i = 0; i < listCust.Count; i++)
                {
                    cmboxType.Items.Add(listCust[i]);
                }
            }

        }
        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="SelectNode">选中的行政区节点</param>
        private void Initialifrm(DevComponents.AdvTree.Node SelectNode)
        {
            try
            {

                InitializeComType();
                try
                {
                    cmboxType.SelectedIndex = 0;
                }
                catch { }
                DevComponents.DotNetBar.ComboBoxItem pCmboxItem = new DevComponents.DotNetBar.ComboBoxItem();
                pCmboxItem.Text = SelectNode.Text;
                pCmboxItem.Tag = SelectNode;
                cmboxExtent.Items.Add(pCmboxItem);
                for (int i = 0; i < SelectNode.Nodes.Count; i++)
                {
                    try
                    {
                        pCmboxItem = new DevComponents.DotNetBar.ComboBoxItem();
                        pCmboxItem.Text = SelectNode.Nodes[i].Text;
                        pCmboxItem.Tag = SelectNode.Nodes[i];
                        cmboxExtent.Items.Add(pCmboxItem);
                    }
                    catch { }
                }
                cmboxExtent.SelectedIndex = 0;
                btnExportImage.Enabled = false;
            }
            catch { }
        }
        private void Initialifrm(string XZQCode)
        {
            try
            {

                InitializeComType();
                try
                {
                    cmboxType.SelectedIndex = 0;
                }
                catch { }
                DevComponents.DotNetBar.ComboBoxItem pCmboxItem = new DevComponents.DotNetBar.ComboBoxItem();
                pCmboxItem.Text = ModXZQ.GetXzqName(Plugin.ModuleCommon.TmpWorkSpace, XZQCode);
                pCmboxItem.Tag = XZQCode;

                //for (int i = 0; i < SelectNode.Nodes.Count; i++)
                //{
                //    try
                //    {
                //        pCmboxItem = new DevComponents.DotNetBar.ComboBoxItem();
                //        pCmboxItem.Text = SelectNode.Nodes[i].Text;
                //        pCmboxItem.Tag = SelectNode.Nodes[i];
                //        cmboxExtent.Items.Add(pCmboxItem);
                //    }
                //    catch { }
                //}

                cmboxExtent.Items.Add(pCmboxItem);
                List<string> m_listXiang = new List<string>();

                IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
                IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
                if (pFW == null) return;
                if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
                ITable pTable = pFW.OpenTable("行政区字典表");

                int ndx = pTable.FindField("NAME"),
                cdx = pTable.FindField("CODE");

                IQueryFilter pQueryFilter = new QueryFilterClass();
                string Condition = "";
                switch (XZQCode.Length)
                {
                    case 2:
                        Condition = null;
                        break;
                    case 4:
                        Condition = "XZJB='" + 3 + "' and substr(code,1,4)='" + XZQCode + "'";
                        break;
                    case 6:
                        Condition = "XZJB='" + 4 + "' and substr(code,1,6)='" + XZQCode + "'";
                        break;
                    case 8:
                        Condition = "XZJB='" + 5 + "' and substr(code,1,8)='" + XZQCode + "'";
                        break;
                    default:
                        Condition = null;
                        break;

                }
                pQueryFilter.WhereClause = Condition;

                ICursor pCursor = pTable.Search(pQueryFilter, false);
                if (pCursor == null) return;

                IRow pRow = pCursor.NextRow();

                while (pRow != null)
                {
                    pCmboxItem = new DevComponents.DotNetBar.ComboBoxItem();
                   // cmboxExtent.Items.Add(pRow.get_Value(ndx).ToString());
                    //m_listXiang.Add(pRow.get_Value(cdx).ToString());
                    pCmboxItem.Text = pRow.get_Value(ndx).ToString();
                    pCmboxItem.Tag = pRow.get_Value(cdx).ToString();
                    cmboxExtent.Items.Add(pCmboxItem);
                    pRow = pCursor.NextRow();
                }
                if (m_listXiang.Count >= 0)
                {
                    cmboxExtent.SelectedIndex = 0;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                btnExportImage.Enabled = false;
            }
            catch { }
        }
        #region 界面事件
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnExportStatistical_Click(object sender, EventArgs e)
        {
            if (dataGViewTable.RowCount == 0)
            {
                return;
            }
            if (dataGViewTable.Columns["CumArea"].HeaderText != "面积(平方米)")
            {
                ExportExcelData();
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = null;
                Workbook wb = null;
                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                    if (excel == null)
                    {
                        MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                        return;
                    }
                    wb = excel.Application.Workbooks.Add(true);
                    excel.Visible = true;

                    excel.DefaultFilePath = "";
                    excel.DisplayAlerts = true;
                    excel.WindowState = XlWindowState.xlNormal;
                    excel.SheetsInNewWorkbook = 1;
                    switch (cmboxType.SelectedItem.ToString())
                    {
                        case "林地利用统计":
                            //表头
                            excel.Cells[1, 1] = "序号";
                            excel.Cells[1, 2] = "林地类型";
                            excel.Cells[1, 3] = dataGViewTable.Columns["CumTYType"].Name.ToString();
                            excel.Cells[1, 4] = "面积(平方米)";
                            excel.Cells[1, 5] = "占地百分比(%)";
                            for (int i = 0; i < dataGViewTable.RowCount - 1; i++)
                            {
                                //RowIndex++;
                                excel.Cells[i + 1, 1] = dataGViewTable.Rows[i].Cells["CumNumber"].Value.ToString();
                                excel.Cells[i + 1, 2] = dataGViewTable.Rows[i].Cells["CumType"].Value.ToString();
                                excel.Cells[i + 1, 3] = dataGViewTable.Rows[i].Cells["CumTYType"].Value.ToString();
                                excel.Cells[i + 1, 4] = dataGViewTable.Rows[i].Cells["CumArea"].Value.ToString();
                                excel.Cells[i + 1, 5] = dataGViewTable.Rows[i].Cells["CumPercentage"].Value.ToString();
                            }
                            break;
                        default:
                            //表头
                            excel.Cells[1, 1] = "序号";
                            excel.Cells[1, 2] = dataGViewTable.Columns["CumType"].Name.ToString();
                            excel.Cells[1, 3] = "面积(平方米)";
                            excel.Cells[1, 4] = "占地百分比(%)";
                            for (int i = 0; i < dataGViewTable.RowCount - 1; i++)
                            {
                                //RowIndex++;
                                excel.Cells[i + 1, 1] = dataGViewTable.Rows[i].Cells["CumNumber"].Value.ToString();
                                excel.Cells[i + 1, 2] = dataGViewTable.Rows[i].Cells["CumType"].Value.ToString();
                                excel.Cells[i + 1, 3] = dataGViewTable.Rows[i].Cells["CumArea"].Value.ToString();
                                excel.Cells[i + 1, 4] = dataGViewTable.Rows[i].Cells["CumPercentage"].Value.ToString();
                            }
                            break;
                    }
                    ///弹出对话保存生成统计表的路径
                    Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                    fd.InitialFileName = cmboxType.Text.ToString() + "表";
                    int result = fd.Show();
                    if (result == 0) return;
                    string fileName = fd.InitialFileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.IndexOf(".xls") == -1)
                        {
                            fileName += ".xls";
                        }
                        ///保存生成的统计表
                        wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    GC.Collect();
                }
                catch { }
            }

        }
        private void cmboxExtent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGViewTable.RowCount != 0)
            {
                dataGViewTable.Rows.Clear();
            }
        }
        private void btnStatistical_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ComboBoxItem pCmbox = cmboxExtent.SelectedItem as DevComponents.DotNetBar.ComboBoxItem;
            DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
            if (pCmbox == null) return; 
            newNode = pCmbox.Tag as DevComponents.AdvTree.Node;
            StaticUnit = ModGetData.GetUnitName(newNode);
            WhereCorse = newNode.Name;
            if (newNode == null) { return; }
            string strZLDJ = "";
            IFeatureClass pZLDJFeaClass = null;
            string ZLDJLayerName = "";
            CopyConfigXml();    //added by chulili 20111110先从业务库拷贝配置文件
            //IGeometry pGeo = ModGetData.getExtentByXZQ(newNode);
            string strWhere = ModGetData.getWhereByXZQ(newNode);
            string strTJType = "";
            try
            {
                //根据选择不同的专题统计进行获取配置要素及统计
                switch (cmboxType.SelectedItem.ToString())
                {
                    case "林地质量等级统计":
                        strTJType = "林地质量等级分布";
                        break;
                    case "林地保护等级统计":
                        strTJType = "林地保护等级分布";
                        break;
                    case "林业用地分布统计":
                        strTJType = "林业用地分布";
                        break;
                    case "主导功能区分布统计":
                        strTJType = "主导功能区分布";
                        break;
                    case "林地规划统计":
                        strTJType = "林地规划统计";
                        break;
                    case "林地结构统计":
                        strTJType = "林地结构统计";
                        break;
                    case "林地利用分布统计":
                        strTJType = "林地利用分布统计";
                        break;
                    case "林场用地分布统计":
                        strTJType = "林场用地分布统计";
                        break;
                    default :
                        DoCustomStatic();
                        return;
                        
                }
                //获取统计地类
                GetPlaceNameStatisticsConfig(out pZLDJFeaClass, out ZLDJLayerName, out strZLDJ, strTJType);
                if (pZLDJFeaClass == null)
                {
                    MessageBox.Show("无林地图斑数据,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (pZLDJFeaClass != null)
                {
                    if (pZLDJFeaClass.FindField(strZLDJ) < 0)
                    {
                        MessageBox.Show("找不到林地图斑数据地类属性,请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        pZLDJFeaClass = null;
                        return;
                    }
                }
                ExportToExcel pExportToExcel = new ExportToExcel();
                IFeatureCursor pFeatureCursor = null; ;

                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.WhereClause = strWhere;
                //pSpatialFilter.GeometryField = "SHAPE";
                //pSpatialFilter.Geometry = pGeo;
                //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pFeatureCursor = pZLDJFeaClass.Search(pSpatialFilter, false);

                //根据选择不同的专题统计进行获取配置要素及统计
                switch (cmboxType.SelectedItem.ToString())
                {
                    case "林地质量等级统计":
                        pExportToExcel.XZQExport(pZLDJFeaClass, pFeatureCursor, strTJType, "林地质量等级", strZLDJ, dataGViewTable);
                        break;
                    case "林地保护等级统计":
                        pExportToExcel.XZQExport(pZLDJFeaClass, pFeatureCursor, strTJType, "林地保护等级", strZLDJ, dataGViewTable);
                        break;
                    case "林业用地分布统计":
                        pExportToExcel.XZQExportLDFB(pZLDJFeaClass, pFeatureCursor, strTJType, "地类名称", strZLDJ, dataGViewTable);
                        break;
                    case "主导功能区分布统计":
                        pExportToExcel.XZQExport(pZLDJFeaClass, pFeatureCursor, strTJType, "主导功能区", strZLDJ, dataGViewTable);
                        break;
                    case "林地规划统计":
                        pExportToExcel.XZQExportLD(pZLDJFeaClass, pFeatureCursor, strTJType, "林地规划", strZLDJ, dataGViewTable);
                        break;
                    case "林地结构统计":
                        pExportToExcel.XZQExport(pZLDJFeaClass, pFeatureCursor, strTJType, "林地结构类型", strZLDJ, dataGViewTable);
                        break;
                    case "林地利用分布统计":
                        pExportToExcel.XZQExportLD(pZLDJFeaClass, pFeatureCursor, strTJType, "林地利用分布", strZLDJ, dataGViewTable);
                        break;
                    case "林场用地分布统计":
                        pExportToExcel.XZQExportLD(pZLDJFeaClass, pFeatureCursor, strTJType, "林场用地分布", strZLDJ, dataGViewTable);
                        break;

                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);


            }
            catch (Exception ex)
            {
              //  ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
            //为柱状图饼状图设置标题 ygc 2012-8-28
            //ChartPie.ChartTitle.Text = cmboxExtent.Text.ToString() + cmboxType.Text.ToString() + "图";
            ChartColumnar.ChartTitle.Text = cmboxExtent.Text.ToString() + cmboxType.Text.ToString() + "图";
            //设置X轴显示标题 ygc 2012-8-28
           // ChartPie.XLabels.UnitText = dataGViewTable.Columns["CumType"].HeaderText;
            ChartColumnar.XLabels.UnitText = dataGViewTable.Columns["CumType"].HeaderText;
            //设置Y轴显示标题 ygc 2012-8-28 
           // ChartPie.YLabels.UnitText = dataGViewTable.Columns["CumArea"].HeaderText;
            ChartColumnar.YLabels.UnitText = dataGViewTable.Columns["CumArea"].HeaderText;

            //ChartPie.BindChartData(GetChartTable(dataGViewTable));
            ChartColumnar.BindChartData(GetChartTable(dataGViewTable));
        }
        //获得统计图的数据源 ygc 2012-8-28
        private System.Data.DataTable GetChartTable(DataGridView dv)
        {
            System.Data.DataTable newTable = new System.Data.DataTable("统计图");
            //初始化Table 列
            DataColumn c = new DataColumn(dv.Columns["CumType"].Name.ToString());
            //c.ColumnName = dv.Columns["CumType"].Name;
            c.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(c);
            DataColumn b = new DataColumn(dv.Columns["CumArea"].Name.ToString());
            b.DataType = System.Type.GetType("System.String");
            b.ColumnName = dv.Columns["CumArea"].Name;
            newTable.Columns.Add(b);
            //导入数据
            for (int i = 0; i < dataGViewTable.Rows.Count; i++)
            {
                DataRow rows =newTable .NewRow();
                if (dv.Rows[i].Cells["CumArea"].Value == null) return null;
                if (dv.Rows[i].Cells["CumType"].Value.ToString() != "" && dv.Rows[i].Cells["CumArea"].Value.ToString() != "")
                {
                    if (dv.Rows[i].Cells["CumType"].Value.ToString() == "林地" || dv.Rows[i].Cells["CumType"].Value.ToString() == "非林地")
                    {
                        rows["CumType"] = dv.Rows[i].Cells["CumTYType"].Value.ToString();
                    }
                    else if (dv.Rows[i].Cells["CumType"].Value.ToString() == "林地总面积")
                    {
                        break;
                    }
                    else
                    {
                        rows["CumType"] = dv.Rows[i].Cells["CumType"].Value.ToString();
                    }
                    rows["CumArea"] = dv.Rows[i].Cells["CumArea"].Value.ToString();
                }
                newTable.Rows.Add(rows);
            }

                return newTable;
        }
        /// <summary>
        /// 根据选择不同的统计类型进行初始化统计结果页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmboxType_SelectedIndexChanged(object sender, EventArgs e)
        {
           // m_DataTable .Rows.Clear ();
          // dataGViewTable.DataSource = null;
            //dataGViewTable.Rows.Clear();
            dataGViewTable.Refresh();
            if (m_DataTable != null)
            {
                InitialDataGridview();
            }
            switch (cmboxType.SelectedItem.ToString())
            {
                case "林地质量等级统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林地质量等级";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林地保护等级统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林地保护等级";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林业用地分布统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林业用地类型";
                    dataGViewTable.Columns["CumTYType"].Visible = true;
                    break;
                case "主导功能区分布统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "主导功能区类型";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林地规划统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林地规划类型";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林地结构统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林地结构类型";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林地利用分布统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林地利用分布类型";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                case "林场用地分布统计":
                    dataGViewTable.Columns["CumType"].HeaderText = "林场用地分布";
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
                default :
                  
                    dataGViewTable.Columns["CumTYType"].Visible = false;
                    break;
            }
        }
        #endregion
        #region 公共函数
        private void CopyConfigXml()
        {
            try
            {
                SysGisTable mSystable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
                Exception err = null;
                Dictionary<string, object> pDic = mSystable.GetRow("SYSSETTING", "SETTINGNAME='统计配置'", out err);
                if (pDic != null)
                {
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                        {
                            object tempObj = pDic["SETTINGVALUE2"];
                            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = tempObj as IMemoryBlobStreamVariant;
                            IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
                            if (pMemoryBlobStream != null)
                            {
                                pMemoryBlobStream.SaveToFile(m_StatisticsPath);
                            }
                        }
                    }
                }
            }
            catch (Exception err2)
            { }
        }
        /// <summary>
        /// 根据配置文件获取图层和字段名
        /// </summary>
        /// <param name="pNameClass">要素</param>
        /// <param name="strLayerName">名称</param>
        /// <param name="strFieldname">字段名</param>
        /// <param name="strTypeName">统计类型</param>
        public void GetPlaceNameStatisticsConfig(out IFeatureClass pNameClass, out string strLayerName, out string strFieldname, string strTypeName)
        {
            strLayerName = "";
            pNameClass = null;
            strFieldname = "";//yjl0730 return前 out参数必须赋值
            if (!File.Exists(m_StatisticsPath))
            {
                return;
            }
            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(m_StatisticsPath);
                string strSearch = "//StatisticsConfig/StatisticsItem[@ItemText= '" + strTypeName + "']";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
                string strNodeKey = "";
                if (pNodeList.Count > 0)
                {
                    XmlNode pXZnode = pNodeList[0];
                    strNodeKey = pXZnode.Attributes["NodeKey"].Value;//行政地名图层名
                }
                XmlNode pFieldNode = pNode.SelectSingleNode(".//FieldItem");
                string strField = "";
                if (pFieldNode != null)
                {
                    strField = pFieldNode.Attributes["FieldName"].Value;
                }
                //string strFieldFL = pNode.Attributes["FieldDMFL"].Value;
                IFeatureClass pFeaClass = GetFeatureClassByNodeKey(strNodeKey);
                strLayerName = (pFeaClass as IDataset).Name;
                pNameClass = pFeaClass;
                strFieldname = strField;
            }
            catch { }
        }
        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public IFeatureClass GetFeatureClassByNodeKey(string strNodeKey)
        {
            if (strNodeKey.Equals(""))
            {
                return null;
            }
            //目录树路径变量:_layerTreePath
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(LayerXMLpath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(LayerXMLpath);
            string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return null;
            }
            //获取图层名,数据源id
            string strFeaClassName = "";
            string strDBSourceID = "";
            try
            {
                strFeaClassName = pNode.Attributes["Code"].Value;
                strDBSourceID = pNode.Attributes["ConnectKey"].Value;
            }
            catch
            { }
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            object objConnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDBSourceID, out eError);
            string conninfostr = "";
            if (objConnstr != null)
            {
                conninfostr = objConnstr.ToString();
            }
            object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "ID=" + strDBSourceID, out eError);
            int type = -1;
            if (objType != null)
            {
                type = int.Parse(objType.ToString());
            }
            //根据数据源连接信息,获取数据源连接
            IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null)
            {
                return null;
            }
            //打开地物类
            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeaClass = pFeaWorkSpace.OpenFeatureClass(strFeaClassName);
            return pFeaClass;

        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
            //end added by chulili 20111109
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        private void btnExportImage_Click(object sender, EventArgs e)
        {
            if (m_ChartLet == null)
            {
                return;
            }
            ExportImage(m_ChartLet);
        }
        //导出统计图  ygc 2012-8-29
        private void ExportImage(Chartlet chart)
        {
            try
            {
                SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                pSaveFileDialog.Filter = "JPEG|*.jpg";
                if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                string strPath = pSaveFileDialog.FileName;
                if (System.IO.File.Exists(strPath))
                {
                    try
                    {
                        System.IO.File.Delete(strPath);
                    }
                    catch { MessageBox.Show("图片导出失败！", "提示！"); return; }

                }
                Image myImage = new Bitmap(chart.Width, chart.Height);
                Bitmap pBitmap = new Bitmap(myImage);
                //获取导出范围
                System.Drawing.Rectangle cloneRect = new System.Drawing.Rectangle(chart.Location,chart.Size);
                ImageFormat imageFormat = chart.OutputFormat;
                chart.DrawToBitmap(pBitmap, cloneRect);
                //保存为文件
                pBitmap.Save(strPath,imageFormat);
                pBitmap.Dispose();
                //g.Dispose();
                myImage.Dispose();
                MessageBox.Show("图片导出成功！", "提示！");
            }
            catch(Exception ex)
            {
                MessageBox.Show("图片导出失败！"+ex.ToString (), "提示！");
            }
           
        }

        private void tbiGrid_Click(object sender, EventArgs e)
        {
            btnExportImage.Enabled = false;
            m_ChartLet = null;
        }

        private void PieChart_Click(object sender, EventArgs e)
        {
            btnExportImage.Enabled = true;
            //m_ChartLet = ChartPie;
        }

        private void tbiColumnar_Click(object sender, EventArgs e)
        {
            btnExportImage.Enabled = true;
            m_ChartLet = ChartColumnar;
        }

        private void btnCustomStatistics_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ComboBoxItem pCmbox = cmboxExtent.SelectedItem as DevComponents.DotNetBar.ComboBoxItem;
            DevComponents.AdvTree.Node newNode=new DevComponents.AdvTree.Node ();
            newNode=pCmbox.Tag as DevComponents.AdvTree.Node;
            StaticUnit = ModGetData.GetUnitName(newNode);
            WhereCorse = newNode.Name;
            if (StaticUnit == "")
            {
                return;
            }
            FrmCustomStatistical frmCustom = new FrmCustomStatistical();
            frmCustom.m_StaticsUnit = StaticUnit;
            frmCustom.ShowDialog();
            if (frmCustom.result != DialogResult.OK) return;
            cmboxType.Items.Clear();
            cmboxType.Text  = frmCustom.m_SolutionName;
            m_CurrentFeatureClass = frmCustom.m_pFeatureClass;
            InitializeComType();

            dataGViewTable.Columns["CumArea"].HeaderText = SysCommon.ModField.GetChineseNameOfField(frmCustom.m_StatisticsField);
            dataGViewTable.Columns["CumType"].HeaderText = SysCommon.ModField.GetChineseNameOfField(frmCustom .m_ClassField);
            dataGViewTable.Columns["CumPercentage"].Visible = false;
            dataGViewTable.Columns["CumTYType"].Visible = false;

            ITable pTable = GetCustomStatisticsTable(frmCustom.m_pFeatureClass, frmCustom.m_StatisticsField, frmCustom.m_ClassField);
            m_DataTable = ITableToDataTable(pTable);
            dataGViewTable.DataSource = m_DataTable;
            //为柱状图饼状图设置标题 ygc 2012-8-28
            ChartColumnar.ChartTitle.Text = cmboxExtent.Text.ToString() + cmboxType.Text.ToString() + "图";
            //设置X轴显示标题 ygc 2012-8-28
            ChartColumnar.XLabels.UnitText = dataGViewTable.Columns["CumType"].HeaderText;
            //设置Y轴显示标题 ygc 2012-8-28 
            ChartColumnar.YLabels.UnitText = dataGViewTable.Columns["CumArea"].HeaderText;

            ChartColumnar.BindChartData(GetChartTable(dataGViewTable));
        }
        
        //通过执行SQL语句获得自定义统计表 ygc 2012-9-5
        private ITable GetCustomStatisticsTable(IFeatureClass pFeatureClass, string StatisticsField, string ClassField)
        {
            string Code = GetXianCode(cmboxExtent.Text);
            if (Code == "")
            {
                Code = GetXiangCode(cmboxExtent.Text);
            }
            IWorkspace pWorkspace = pFeatureClass.FeatureDataset.Workspace;
            ITable pTable = null;
            string tableName = (pFeatureClass as IDataset).Name;
            DropTable(pWorkspace, "TempStatisticsTable");
            //统计数据SQL语句
            string SQLString = "create table TempStatisticsTable as select " + ClassField + " as CumType, sum(" + StatisticsField + ") as CumArea from  " + tableName + "  where substr(xian,1,6)='" + Code + "'and " + StaticUnit + "='" + WhereCorse + "' group by  " + ClassField;
            pWorkspace.ExecuteSQL(SQLString);
            pTable = (pWorkspace as IFeatureWorkspace).OpenTable("TempStatisticsTable");
            return pTable;
        }
        //通过SQL语句删除临时表 ygc 2012-9-5
        private void DropTable(IWorkspace pWks, string TableName)
        {
            try
            {
                pWks.ExecuteSQL("drop table " + TableName);

            }
            catch
            { }
        }
        //将ITable转换成DataTable
        //ygc 2012-8-21
        private System.Data.DataTable ITableToDataTable(ITable pip_Table)
        {
            System.Data.DataTable lc_TableData = new System.Data.DataTable("统计结果");
            if (pip_Table == null) return null;
            ICursor lip_Cursor = null;
            try
            {
                // 无数据返回空表
                if (pip_Table.RowCount(null) == 0) return null;
                // 给列赋值
                DataColumn DataColum = new DataColumn();
                DataColum.ColumnName = "CumNumber";
                DataColum.DataType = System.Type.GetType("System.String");
                lc_TableData.Columns.Add(DataColum);

                DataColumn DataColum1= new DataColumn();
                DataColum1.ColumnName = "CumType";
                DataColum1.DataType = System.Type.GetType("System.String");
                lc_TableData.Columns.Add(DataColum1);

                DataColumn DataColum2 = new DataColumn();
                DataColum2.ColumnName = "CumArea";
                DataColum2.DataType = System.Type.GetType("System.String");
                lc_TableData.Columns.Add(DataColum2);
                // 循环拷贝数据
                lip_Cursor = pip_Table.Search(null, false);
                pip_Table.Search(null, false);
                if (lip_Cursor == null)
                {
                    return null;
                }
                IRow lip_Row = lip_Cursor.NextRow();
                lc_TableData.BeginLoadData();
                int count = 0;
                while (lip_Row != null)
                {
                    DataRow lc_Row = lc_TableData.NewRow();
                    lc_Row["CumNumber"] = count.ToString();
                    count++;

                    int fieldIndex = pip_Table.Fields.FindField("CumType");
                    lc_Row ["CumType"]=lip_Row.get_Value (fieldIndex).ToString ();

                    int t = pip_Table.Fields.FindField("CumArea");
                    lc_Row["CumArea"] = lip_Row.get_Value(t).ToString();

                    lc_TableData.Rows.Add(lc_Row);
                    lip_Row = lip_Cursor.NextRow();
                }
                lc_TableData.EndLoadData();
                return lc_TableData;
            }
            catch (Exception ex)
            {
                
                return null;
            }
            finally
            {
                if (lip_Cursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lip_Cursor);
                }
            }
        }
        //通过配置文件获得行政区编码 ygc 2012-9-5
        private string GetXianCode( string Xian)
        {
            string code = "";
            XmlDocument pXml = new XmlDocument();
            pXml.Load(XZQXMLPath);
            XmlNodeList XZQTreeList = pXml.SelectNodes("GisDoc/XZQTree//Province");
             for (int i = 0; i < XZQTreeList.Count;i++ )
            {
                XmlNode xn = XZQTreeList[i];
                if (xn.Attributes["ItemName"].Value != null)
                {
                    if (xn.Attributes["ItemName"].Value.ToString() == Xian)
                    {
                        code = xn.Attributes["XzqCode"].Value.ToString();
                    }
                }
            }
            return code;
        }
        private string GetXiangCode( string Xiang)
        {
            string code = "";
            XmlDocument pXml = new XmlDocument();
            pXml.Load(XZQXMLPath);
            XmlNodeList XZQTreeList = pXml.SelectNodes("GisDoc/XZQTree//City");
            for (int i = 0; i < XZQTreeList.Count; i++)
            {
                if (XZQTreeList[i].Attributes["ItemName"].Value != null)
                {
                    if (XZQTreeList[i].Attributes["ItemName"].Value.ToString() == Xiang)
                    {
                        code = XZQTreeList[i].Attributes["XzqCode"].Value.ToString();
                    }
                }
            }
            return code;
        }
        //导出自定制统计数据为EXCEL
        private void ExportExcelData()
        {
            if (dataGViewTable.RowCount == 0)
            {
                return;
            }
            Microsoft.Office.Interop.Excel.Application excel = null;
            Workbook wb = null;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                if (excel == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                    return;
                }
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = true;

                excel.DefaultFilePath = "";
                excel.DisplayAlerts = true;
                excel.WindowState = XlWindowState.xlNormal;
                excel.SheetsInNewWorkbook = 1;
                        //表头
                        excel.Cells[1, 1] = "序号";
                        excel.Cells[1, 2] = dataGViewTable.Columns["CumType"].Name.ToString();
                        excel.Cells[1, 3] = dataGViewTable.Columns["CumArea"].Name.ToString();;
                        for (int i = 0; i < dataGViewTable.RowCount - 1; i++)
                        {
                            //RowIndex++;
                            excel.Cells[i + 1, 1] = dataGViewTable.Rows[i].Cells["CumNumber"].Value.ToString();
                            excel.Cells[i + 1, 2] = dataGViewTable.Rows[i].Cells["CumType"].Value.ToString();
                            excel.Cells[i + 1, 3] = dataGViewTable.Rows[i].Cells["CumArea"].Value.ToString();
                        }
                ///弹出对话保存生成统计表的路径
                Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                fd.InitialFileName = cmboxType.Text.ToString() + "表";
                int result = fd.Show();
                if (result == 0) return;
                string fileName = fd.InitialFileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.IndexOf(".xls") == -1)
                    {
                        fileName += ".xls";
                    }
                    ///保存生成的统计表
                    wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                GC.Collect();
            }
            catch { }
        }
        private void btnChartPie_Click(object sender, EventArgs e)
        {
            btnChartColumnar.Enabled = true;
            btnChartPie.Enabled = false;
            ChartColumnar.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_2D_Aurora_FlatCrystal_NoGlow_NoBorder;
            ChartColumnar.Refresh();
        }
        private void btnChartColumnar_Click(object sender, EventArgs e)
        {
            btnChartPie.Enabled = true;
            btnChartColumnar.Enabled = false;
            ChartColumnar.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_3D_Aurora_NoCrystal_NoGlow_NoBorder;
            ChartColumnar.Refresh();
        }
        private List<string> GetFiledValues(string tableName, string Key)
        {
            List<string> newList = new List<string>();
            if (m_pWorkspace == null) return newList;
            DropTable("tempTable");
            m_pWorkspace.ExecuteSQL("create table tempTable as select " + Key + " from " + tableName);
            IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
            ITable pTable = pWs.OpenTable("tempTable");
            ICursor pCursor = pTable.Search(null, false);
            try
            {
                if (pCursor != null)
                {
                    IRow row = pCursor.NextRow();
                    while (row != null)
                    {
                        newList.Add(row.get_Value(0).ToString());
                        row = pCursor.NextRow();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                DropTable("tempTable");
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
            }
            return newList;
        }
        private string GetFiledValues(string tableName, string Key, string condtion)
        {
            string newList = "";
            if (m_pWorkspace == null) return newList;
            IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
            ITable pTable = pWs.OpenTable(tableName);
            int intdex = pTable.Fields.FindField(Key);
            IQueryFilter pFilter = new QueryFilterClass();
            pFilter.WhereClause = condtion;
            ICursor pCursor = pTable.Search(pFilter, false);
            try
            {
                if (pCursor != null)
                {
                    IRow row = pCursor.NextRow();
                    if (row != null)
                    {
                        newList = row.get_Value(intdex).ToString();
                        row = pCursor.NextRow();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if(pCursor!=null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor=null;
            }
            pFilter = null;

            return newList;
        }
        private void DropTable(string tableName)
        {
            try
            {
                m_pWorkspace.ExecuteSQL("drop table " + tableName);
            }
            catch
            { }
        }
        private void DoCustomStatic()
        {
            string CustomName=cmboxType .Text .ToString ();
            string condtion="statisticsname='" + CustomName + "'";
            string ClassfiyField = GetFiledValues("customstatistics", "classifyfield", condtion);
            string NodeKey = GetFiledValues("customstatistics", "layerid", condtion);
            IFeatureClass pFeatureClass = GetFeatureClassByNodeKey(NodeKey);
            string StaticField = GetFiledValues("customstatistics", "statisticsfield", condtion);
           

            DevComponents.DotNetBar.ComboBoxItem pCmbox = cmboxExtent.SelectedItem as DevComponents.DotNetBar.ComboBoxItem;
            DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
            newNode = pCmbox.Tag as DevComponents.AdvTree.Node;
            StaticUnit = ModGetData.GetUnitName(newNode);
            WhereCorse = newNode.Name;
            if (StaticUnit == "")
            {
                return;
            }
            dataGViewTable.Columns["CumArea"].HeaderText = SysCommon.ModField.GetChineseNameOfField(StaticField);
            dataGViewTable.Columns["CumType"].HeaderText = SysCommon.ModField.GetChineseNameOfField(ClassfiyField);
            dataGViewTable.Columns["CumPercentage"].Visible = false;
            dataGViewTable.Columns["CumTYType"].Visible = false;

            ITable pTable = GetCustomStatisticsTable(pFeatureClass, StaticField, ClassfiyField);
            m_DataTable = ITableToDataTable(pTable);
            dataGViewTable.DataSource = m_DataTable;

            //为柱状图饼状图设置标题 ygc 2012-8-28
            ChartColumnar.ChartTitle.Text = cmboxExtent.Text.ToString() + cmboxType.Text.ToString() + "图";
            //设置X轴显示标题 ygc 2012-8-28
            ChartColumnar.XLabels.UnitText = dataGViewTable.Columns["CumType"].HeaderText;
            //设置Y轴显示标题 ygc 2012-8-28 
            ChartColumnar.YLabels.UnitText = dataGViewTable.Columns["CumArea"].HeaderText;
            ChartColumnar.BindChartData(GetChartTable(dataGViewTable));

        }
        private void btnDeleteSolution_Click(object sender, EventArgs e)
        {
            ManageSolution newfrm = new ManageSolution();
            newfrm.ShowDialog();
            InitializeComType();
        }
        //初始化Datagridview
        private void InitialDataGridview()
        {
            dataGViewTable.DataSource = null;
            dataGViewTable.Columns.Clear();
            DataGridViewTextBoxColumn newGridViewColumn1 = new DataGridViewTextBoxColumn();
            newGridViewColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            newGridViewColumn1.DataPropertyName = "CumNumber";
            newGridViewColumn1.HeaderText = "序号";
            newGridViewColumn1.Name = "CumNumber";
            newGridViewColumn1.ReadOnly = true;
            newGridViewColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            dataGViewTable.Columns.Add(newGridViewColumn1);

            DataGridViewTextBoxColumn newGridViewColumn2 = new DataGridViewTextBoxColumn();
            newGridViewColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            newGridViewColumn2.DataPropertyName = "CumType";
            newGridViewColumn2.HeaderText = "统计类型";
            newGridViewColumn2.Name = "CumType";
            newGridViewColumn2.ReadOnly = true;
            newGridViewColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            dataGViewTable.Columns.Add(newGridViewColumn2);
            DataGridViewTextBoxColumn newGridViewColumn3 = new DataGridViewTextBoxColumn();
            newGridViewColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            newGridViewColumn3.HeaderText = "地类名称";
            newGridViewColumn3.Name = "CumTYType";
            newGridViewColumn3.ReadOnly = true;
            newGridViewColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            dataGViewTable.Columns.Add(newGridViewColumn3);
            DataGridViewTextBoxColumn newGridViewColumn4 = new DataGridViewTextBoxColumn();
            newGridViewColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            newGridViewColumn4.DataPropertyName = "CumArea";
            newGridViewColumn4.HeaderText = "面积（平方米）";
            newGridViewColumn4.Name = "CumArea";
            newGridViewColumn4.ReadOnly = true;
            newGridViewColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            dataGViewTable.Columns.Add(newGridViewColumn4);
            DataGridViewTextBoxColumn newGridViewColumn5 = new DataGridViewTextBoxColumn();
            newGridViewColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            newGridViewColumn5.DataPropertyName = "Percent";
            newGridViewColumn5.HeaderText = "占百地分比(%)";
            newGridViewColumn5.Name = "CumPercentage";
            newGridViewColumn5.ReadOnly = true;
            newGridViewColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            dataGViewTable.Columns.Add(newGridViewColumn5);
            dataGViewTable.Refresh();
        }
    
    }
}
