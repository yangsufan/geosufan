using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Data.OracleClient;
using SysCommon.Gis;
using SysCommon;

namespace GeoStatistics
{
    public partial class FrmCustomizeStatistic :DevComponents.DotNetBar.Office2007Form
    {
        private IFeatureClass m_pCurFeaCls = null;
        private Dictionary<string, int> _DicFieds = new Dictionary<string, int>();
        private string _LayerTreePath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\查询图层树_CusStatis.xml"; //图层目录文件路径

        private string _LinBanXmlPath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\林斑_CusStatis.xml"; //图层目录文件路径
        private string _StatisticConfigPath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\CustomizeStatistic.xml";
        private XmlDocument _StatisticConfigXmldoc = null;
        private List<string> _ListNoFactors = new List<string>();
        private int _YfactorCnt = 3;
        private int _ContentCnt = 0;
        private OracleConnection _OracleConn = null;
        private string _ResTableName = "";//"TmpCustomizeStatistic";
        private bool _InStatistic = false;
        private Dictionary<string, int> _DicXfactorItems = new Dictionary<string, int>();
        public FrmCustomizeStatistic()
        {
            InitializeComponent();
            InitStatisticConfig();
        }
        private void InitStatisticConfig()
        {
            if (_StatisticConfigXmldoc == null)
            {
                _StatisticConfigXmldoc = new XmlDocument();
            }
            if (!File.Exists(_StatisticConfigPath))
            {
                return;
            }
            try
            {
                _StatisticConfigXmldoc.Load(_StatisticConfigPath);
                string strSearch = "//StatisticConfig/Region";
                XmlNode pNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList pNodeList = pNode.SelectNodes(".//RegionItem");
                for (int i = 0; i < pNodeList.Count; i++)
                {
                    XmlNode pTmpNode = pNodeList[i];
                    string strItemText = pTmpNode.Attributes["ItemText"].Value;
                    cmbRegion.Items.Add(strItemText);
                }
                strSearch = "//StatisticConfig/StatisticContent";
                XmlNode pContentNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                XmlNodeList pContentList = pContentNode.SelectNodes(".//ContentItem");
                for (int i = 0; i < pContentList.Count; i++)
                {
                    XmlNode pContentItemNode = pContentList[i];
                    string strItemText = pContentItemNode.Attributes["ItemText"].Value;
                    XmlNodeList pContentItemList = pContentItemNode.ChildNodes;

                    string strUnitText = "";
                    try
                    {
                        strUnitText = pContentItemList[0].Attributes["ItemText"].Value;
                    }
                    catch (System.Exception ex)
                    {
                    	
                    }
                    ListContents.Rows.Add(false, strItemText, strUnitText);
                }
                
                strSearch = "//StatisticConfig/DecimalDigits";
                XmlNode pDecimalNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                XmlNodeList pDecimalList = pDecimalNode.SelectNodes(".//DigitsItem");
                for (int i = 0; i < pDecimalList.Count; i++)
                {
                    XmlNode pDecimalItemNode = pDecimalList[i];
                    string strItemText = pDecimalItemNode.Attributes["ItemText"].Value;
                    cmbDigits.Items.Add(strItemText);
                }

            }
            catch
            { }
        }
        private void txtBoxLayer_Click(object sender, EventArgs e)
        {
            this.advTreeLayers.Width = this.txtBoxLayer.Width;
            this.advTreeLayers.Visible = true;
            this.advTreeLayers.Focus();
            //SysCommon.SelectLayerByTree frm = new SysCommon.SelectLayerByTree(Plugin.ModuleCommon.TmpWorkSpace, Plugin.ModuleCommon.ListUserdataPriID);
            //frm._LayerTreePath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\临时图层树.xml";
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    m_pCurFeaCls = null;
            //    if (frm.m_NodeKey.Trim() != "")
            //    {
            //        m_pCurFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath, frm.m_NodeKey);

            //    }
            //    if (frm.m_DataSourceKey != "")
            //    {
            //        _OracleConn = GetOracleConn(Plugin.ModuleCommon.TmpWorkSpace, frm.m_DataSourceKey);
            //    }
            //    if (m_pCurFeaCls != null)
            //    {
            //        txtBoxLayer.Text = frm.m_NodeText;
            //        InitListFields();                    
            //    }
            //}
            //System.IO.File.Delete(frm._LayerTreePath);
            //frm = null;
        }
        private void InitListFields()
        {
            if (m_pCurFeaCls != null)
            {
                if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
                {
                    SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
                }

                _DicFieds.Clear();
                IFields pFields = m_pCurFeaCls.Fields;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    IField pField = pFields.get_Field(i);
                    switch (pField.Type)
                    {
                        case esriFieldType.esriFieldTypeString:
                            break;
                        case esriFieldType.esriFieldTypeInteger:
                            break;
                        case esriFieldType.esriFieldTypeSmallInteger:
                            break;
                        default:
                            continue;
                            break;
                    }
                    if (pField.Name.ToUpper().StartsWith("SHAPE"))
                    {
                        continue;
                    }
                    string strChineseName = SysCommon.ModField.GetChineseNameOfField(pField.Name);
                    if (!_ListNoFactors.Contains(strChineseName))
                    {
                        cmbXfactor.Items.Add(strChineseName);
                        cmbYfactor1.Items.Add(strChineseName);
                        cmbYfactor2.Items.Add(strChineseName);
                        cmbYfactor3.Items.Add(strChineseName);
                    }                    
                    _DicFieds.Add(strChineseName, i);

                }
            }
        }

        private void fieldlistBox_DoubleClick(object sender, EventArgs e)
        {

        }

        private void fieldlistBox_Click(object sender, EventArgs e)
        {

        }

        private void btnExportImage_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                pSaveFileDialog.Filter = "JPEG|*.jpg|BMP|*.bmp";
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
                chartlet.Width = chartlet.Width * 3;
                chartlet.Height = chartlet.Height * 3;
                chartlet.Refresh();
                Image myImage = new Bitmap(chartlet.Width, chartlet.Height);
                //从一个继承自Image类的对象中创建Graphics对象
                //Graphics g = Graphics.FromImage(myImage);
                //抓屏并拷贝到myimage里
                // g.CopyFromScreen(this.Location,chartlet.Location, new Size(chartlet.Width+20, chartlet.Height+45));
                Bitmap pBitmap = new Bitmap(myImage);
                //Bitmap vBitmap;
                ///将抓屏获得图片在进行截取 由于通过抓屏无法获得最小的统计图
                System.Drawing.Point pPoint = new System.Drawing.Point(0, 0);
                System.Drawing.Rectangle cloneRect = new System.Drawing.Rectangle(pPoint, chartlet.Size);
                //vBitmap = pBitmap.Clone(cloneRect, pBitmap.PixelFormat);
                chartlet.DrawToBitmap(pBitmap, cloneRect);// 修改图片导出方式 ygc 2012-9-4
                //保存为文件
                pBitmap.Save(strPath);
                //myImage.Save(strPath);
                pBitmap.Dispose();
                // g.Dispose();
                myImage.Dispose();
                chartlet.Width = chartlet.Width /3;
                chartlet.Height = chartlet.Height / 3;
                chartlet.Refresh();
                //chartlet.BackgroundImage.Save(pSaveFileDialog.FileName);
                MessageBox.Show("图片导出成功！", "提示！");
            }
            catch
            {
                MessageBox.Show("图片导出失败！", "提示！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmCustomizeStatistic_Load(object sender, EventArgs e)
        {
            ListContents.Controls.Add(cmbUnit);
            _ListNoFactors.Add("省");
            _ListNoFactors.Add("市");
            _ListNoFactors.Add("县");
            _ListNoFactors.Add("村");

            cmbChartType.Items.Add("柱状图");
            cmbChartType.Items.Add("线划图");
            cmbChartType.Items.Add("饼状图");

            cmbDigits.SelectedIndex = 2;
            InitLayersTree();
            //InitDefaultLayer();   //耗时较长，先屏蔽

        }
        private void InitDefaultLayer()
        {
            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace, "最大林斑号", _LinBanXmlPath);
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(_LinBanXmlPath);
            string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'最大林斑号查询'" + "]";
            XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return;
            }
            XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
            string LinBanLayerKey = "";
            string LinBanNodetxt = "";
            if (pNodeList.Count > 0)
            {
                XmlNode pXZnode = pNodeList[0];
                LinBanLayerKey = pXZnode.Attributes["NodeKey"].Value;//林斑图层名
                LinBanNodetxt = pXZnode.Attributes["NodeText"].Value;
            }
            IFeatureClass pFeatureClass = null;

            string DataSourceKey = "";
            if (LinBanLayerKey != "")
            {
                m_pCurFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath, LinBanLayerKey, out DataSourceKey);

            }
            if (DataSourceKey != "")
            {
                _OracleConn = GetOracleConn(Plugin.ModuleCommon.TmpWorkSpace, DataSourceKey);
            }
            if (m_pCurFeaCls != null)
            {
                txtBoxLayer.Text = LinBanNodetxt;
                Plugin.LogTable.WriteLocalLog("InitListFields start");
                InitListFields();
            }
            File.Delete(_LinBanXmlPath);
        }
        private void InitUnitBoxValue()
        {
            DataGridViewRow pRow = ListContents.CurrentRow;
            string strContent = "";
            if (pRow != null)
            {
                strContent = pRow.Cells[1].Value.ToString();
            }
            if (strContent == "")
            {
                return;
            }
            cmbUnit.Items.Clear();
            if (_StatisticConfigXmldoc != null)
            {
                string strSearch = "//StatisticConfig/StatisticContent/ContentItem[@ItemText='"+strContent+"']";
                XmlNode pNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                if (pNode != null)
                {
                    XmlNodeList pNodeList = pNode.ChildNodes;
                    for (int i = 0; i < pNodeList.Count; i++)
                    {
                        XmlNode pUnitNode = pNodeList[i];
                        string strUnitText = pUnitNode.Attributes["ItemText"].Value;
                        cmbUnit.Items.Add(strUnitText);
                        
                    }
                }

            }
        }
        private void ListContents_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridViewCell CurrentCell = ListContents.CurrentCell;
            
            if (CurrentCell != null)
            {
                if (CurrentCell.OwningColumn.Name == "ColumnUnit")
                {
                    Rectangle TmpRect = ListContents.GetCellDisplayRectangle(CurrentCell.ColumnIndex, CurrentCell.RowIndex, true);
                    
                    cmbUnit.Size = TmpRect.Size;
                    cmbUnit.Top = TmpRect.Top;
                    cmbUnit.Left = TmpRect.Left;
                    InitUnitBoxValue(); //根据配置文件初始化统计单位下拉框
                    cmbUnit.Text = CurrentCell.Value.ToString();
                    cmbUnit.Visible = true;

                }
                else
                {
                    cmbUnit.Visible = false;
                }
            }
            else
            {
                cmbUnit.Visible = false;
            }
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListContents.CurrentCell.Value = cmbUnit.Text;
        }

        private void FrmCustomizeStatistic_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_StatisticConfigXmldoc != null)
            { 
                _StatisticConfigXmldoc = null;
            }
            if (_ListNoFactors != null)
            {
                if (_ListNoFactors.Count > 0)
                {
                    _ListNoFactors.Clear();
                }
                _ListNoFactors = null;
            }
            if (_ResTableName != "")
            {
                ModOracle.DropTable(_OracleConn, _ResTableName);
            }
            if (_OracleConn != null)
            {
                if (_OracleConn.State == ConnectionState.Open)
                {
                    _OracleConn.Close();
                }
                _OracleConn = null;
            }
            if (_DicXfactorItems != null)
            {
                _DicXfactorItems.Clear();
                _DicXfactorItems = null;
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog pDlg = new SaveFileDialog();
            pDlg.Filter = "Excel WorkBook (*.xls)|*.xls";
            if (pDlg.ShowDialog() == DialogResult.Cancel)
                return;
            string strFileName = pDlg.FileName;
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = null;
                Microsoft.Office.Interop.Excel.Workbook wb = null;
                //建立Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = false;
                wb.Application.ActiveWindow.Caption = strFileName;
                for (int i = 0; i < rowMergeView1.ColumnCount; i++)
                {
                    string strColName = rowMergeView1.Columns[i].HeaderText;
                    if(i<1+_YfactorCnt)
                    {
                        excel.Cells[1, i+1] = strColName;
                        excel.get_Range(excel.Cells[1, i+1], excel.Cells[2, i + 1]).Merge(false);

                    }
                    else
                    {
                        excel.Cells[2, i+1] = strColName;
                    }
                }
                IEnumerator<string> enm = _DicXfactorItems.Keys.GetEnumerator();
                enm.Reset();
                
                while (enm.MoveNext())
                {
                    string key = enm.Current;
                    int iColID = _DicXfactorItems[key];
                    excel.Cells[1, iColID + 1] = key;
                    excel.get_Range(excel.Cells[1, iColID + 1], excel.Cells[1, iColID + _ContentCnt]).Merge(false);
                }


                for (int i = 0; i < rowMergeView1.RowCount; i++)
                {
                    for (int j = 0; j < rowMergeView1.ColumnCount; j++)
                    {
                        try
                        {
                            string strText = "";
                            object objText = rowMergeView1.Rows[i].Cells[j].Value;
                            if (objText != null)
                            {
                                strText = objText.ToString();
                            }
                            excel.Cells[i + 3, j + 1] = strText;
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                DealFormatOfExcelEx(excel, _YfactorCnt);
                ///
                try
                {
                    wb.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (System.Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    SysCommon.ModExcel.Kill(excel);
                    GC.Collect();
                    return;
                }
                excel.Workbooks.Close(); 
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                SysCommon.ModExcel.Kill(excel);
                GC.Collect();
                try
                {
                    System.Diagnostics.Process.Start(strFileName);
                }
                catch (System.Exception ex)
                {

                }
                //OpenExcelFile(strFileName);
            }
            catch
            {
            }
        }

        private void cmbXfactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strXfactor = cmbXfactor.Text;
            if (_DicFieds.ContainsKey(strXfactor))
            {
                int index = _DicFieds[strXfactor];
                string strFieldName = m_pCurFeaCls.Fields.get_Field(index).Name;
                InitXfactorItems(m_pCurFeaCls, strFieldName);
                groupPanelXfactors.Left = cmbXfactor.Right;
                groupPanelXfactors.Top = cmbXfactor.Top+groupPanelAutoConfig.Top+25;
                groupPanelXfactors.Visible = true;
            }
            
        }
        private void InitXfactorItems(IFeatureClass pFeatureClass, string strFieldName)
        {
            dataGridXfactors.Rows.Clear();
            if (pFeatureClass == null)
            {
                return;
            }
            if (strFieldName == "")
            {
                return;
            }
            if (pFeatureClass.Fields.FindField(strFieldName) < 0)
            {
                return;
            }
            IDataset pDataset=pFeatureClass as IDataset;
            IWorkspace pWorkspace = pDataset.Workspace;
            string strFeatureClassName=pDataset.Name;
            if (_OracleConn == null)
            {
                return;//_OracleConn = GetOracleConn(pWorkspace);
            }
            if (_OracleConn != null)
            {
                OracleDataReader pReader = ModOracle.GetReader(_OracleConn, "select distinct " + strFieldName + " from " + strFeatureClassName);
                if(pReader!=null)
                {
                    while(pReader.Read())
                    {
                        string strFieldValue = pReader.GetValue(0).ToString();
                       // string strName = GetDomainOfValue(m_pCurFeaCls, strFieldName, strFieldValue);//ygc 通过字典获取中文值！
                        //CheckListXfactors.Items.Add(strFieldValue);
                        string strName = ModXZQ.GetChineseName(Plugin.ModuleCommon.TmpWorkSpace, strFieldName, strFieldValue);
                        dataGridXfactors.Rows.Add(false, strName);
                    }
                }
                
            }
        }
        
        private string GetDomainOfValue(IFeatureClass pFeatureClass, string fieldName, string strValue)
        {
            if (pFeatureClass == null)
            {
                return strValue;
            }
            int indexField = pFeatureClass.Fields.FindField(fieldName);
            IField pField = pFeatureClass.Fields.get_Field(indexField);
            IDomain domain = pField.Domain;
            ICodedValueDomain codeDomain = domain as ICodedValueDomain;
            if (codeDomain == null)
            {
                return strValue;
            }
            try
            {
                for (int i = 0; i < codeDomain.CodeCount; i++)
                {
                    string strdomain = codeDomain.get_Value(i).ToString();
                    if (strdomain == strValue)
                    {
                        string strName=codeDomain.get_Name(i).ToString();
                        return strName;
                    }
                }
            }
            catch (System.Exception ex)
            {            	
            }
            return strValue;
            
        }
        private OracleConnection GetOracleConn(IWorkspace pWorkSpace)
        {
            if (pWorkSpace == null)
            {
                return null;
            }
            try
            {
                IPropertySet pPropertyset = pWorkSpace.ConnectionProperties;
                string strServer = pPropertyset.GetProperty("Server").ToString();
                string strDatabase = pPropertyset.GetProperty("Database").ToString();
                string strUser = pPropertyset.GetProperty("User").ToString();
                string strPassword = pPropertyset.GetProperty("Password").ToString();

                OracleConnection Conn = ModOracle.GetOracleConnection(strServer, strDatabase, strUser, strPassword);
                return Conn;
            }
            catch (System.Exception ex)
            {
            	
            }
            return null;

        }
        private OracleConnection GetOracleConn(IWorkspace pTmpWorkSpace,string connectKey)
        {
            if (pTmpWorkSpace == null)
            {
                return null;
            }
            try
            {
                SysGisTable sysTable = new SysGisTable(pTmpWorkSpace);
                Exception eError = null;

                object objConnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + connectKey, out eError);
                string conninfostr = "";
                if (objConnstr != null)
                {
                    conninfostr = objConnstr.ToString();
                }
                eError = null;
                object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "ID=" + connectKey, out eError);
                int type = -1;
                if (objType != null)
                {
                    type = int.Parse(objType.ToString());
                }
                if (type != 3)
                {
                    return null;
                }
                sysTable = null;
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

                sServer = conninfostr.Substring(0, index1);
                sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);

                OracleConnection Conn = ModOracle.GetOracleConnection(sServer, sDatabase, sUser, sPassword);
                return Conn;
            }
            catch (System.Exception ex)
            {

            }
            return null;

        }
        private void btnStatistic_Click(object sender, EventArgs e)
        {
            if (cmbRegion.Text == "")
            {
                MessageBox.Show("请选定统计单位!");
                return;
            }
            if (cmbXfactor.Text == "")
            {
                MessageBox.Show("请选定横轴因子!");
                return;
            }
            if (cmbYfactor1.Text == "")
            {
                MessageBox.Show("请选定纵轴因子!");
                return;
            }
            if (cmbDigits.Text == "")
            {
                MessageBox.Show("请选定小数位数!");
                return;
            }

            if (m_pCurFeaCls == null)
            {
                MessageBox.Show("请选定目标图层!");
                return;
            }
            IDataset pDataset = m_pCurFeaCls as IDataset;
            IWorkspace pWorkSpace = pDataset.Workspace;
            if (_OracleConn == null)
            {
                return;//_OracleConn = GetOracleConn(pWorkSpace);
            }
            if (_OracleConn == null)
            {
                return;
            }

            if (groupPanelXfactors.Visible)
            {
                groupPanelXfactors.Visible = false;
            }
            if (_ResTableName != "")
            {
                ModOracle.DropTable(_OracleConn, _ResTableName);
            }
            DoCustomStatisticEx();
        }
        private void DoCustomStatistic()
        {
            string strFeatureClassName = (m_pCurFeaCls as IDataset).Name;
            string strXfactor = cmbXfactor.Text;
            List<string> ListYfactors = new List<string>();
            List<string> ListYfactorFields = new List<string>();
            List<string> ListXfactorItemNames=new List<string>();
            if (cmbYfactor1.Text != "")
            {
                string strYfactor1Field = GetFieldNameByChiName(cmbYfactor1.Text);
                if (strYfactor1Field != null)
                {
                    ListYfactors.Add(cmbYfactor1.Text);
                    ListYfactorFields.Add(strYfactor1Field);
                }
            }
            
            if (cmbYfactor2.Text != "")
            {
                string strYfactor2Field = GetFieldNameByChiName(cmbYfactor2.Text);
                if (strYfactor2Field != null)
                {
                    ListYfactors.Add(cmbYfactor2.Text);
                    ListYfactorFields.Add(strYfactor2Field);
                }
            }
            if (cmbYfactor3.Text != "")
            {
                string strYfactor3Field = GetFieldNameByChiName(cmbYfactor3.Text);
                if (strYfactor3Field != null)
                {
                    ListYfactors.Add(cmbYfactor3.Text);
                    ListYfactorFields.Add(strYfactor3Field);
                }
            }
            _YfactorCnt = ListYfactorFields.Count;
            string strRegion = cmbRegion.Text;
            string strDigits = cmbDigits.Text;

            string strXfactorField = GetFieldNameByChiName(strXfactor);
            
            List<string> ListContentsName = new List<string>();
            List<string> ListContentsField = new List<string>();  
            List<string> ListUnits = new List<string>();    //单位名称
            List<string> ListUnitValues = new List<string>();   //转换系数 如米转换成千米乘以系数：0.001
            for (int i = 0; i < ListContents.RowCount; i++)
            {
                if (ListContents.Rows[i].Cells[0].Value != null)
                {
                    if (ListContents.Rows[i].Cells[0].Value.ToString() == "True")
                    {
                        string strContent = ListContents.Rows[i].Cells[1].Value.ToString();
                        string strContentField = GetContentField(strContent);
                        string strUnit = ListContents.Rows[i].Cells[2].Value.ToString();
                        string strUnitValue = GetUnitValue(strContent, strUnit);
                        ListContentsName.Add(strContent);
                        ListContentsField.Add(strContentField);
                        ListUnits.Add(strUnit);
                        ListUnitValues.Add(strUnitValue);
                    }

                }
                
            }
            
            for (int i = 0; i < dataGridXfactors.RowCount; i++)
            {
                if (dataGridXfactors.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridXfactors.Rows[i].Cells[0].Value.ToString() == "True")
                    {
                        string strTmp = dataGridXfactors.Rows[i].Cells[1].Value.ToString();
                        ListXfactorItemNames.Add(strTmp);
                    }
                }
                
            }
            //写统计结果表头
            int intColid=0;
            
            dGridViewStatisticRes.ColumnCount = 1+3+ListXfactorItemNames.Count *ListContentsField.Count+1;
            dGridViewStatisticRes.Rows.Add();
            dGridViewStatisticRes.Rows.Add();
            dGridViewStatisticRes.Rows[0].Cells[intColid].Value = "统计单位"; intColid++;
            for (int i = 0; i < ListYfactors.Count; i++)
            {
                dGridViewStatisticRes.Rows[0].Cells[intColid].Value = ListYfactors[i];
                intColid++;
            }
            for (int i = 0; i < ListContentsName.Count; i++)
            {
                //if (i == 0)
                //{
                    dGridViewStatisticRes.Rows[0].Cells[intColid].Value = "合计";
                //}                
                dGridViewStatisticRes.Rows[1].Cells[intColid].Value = ListContentsName[i];
                intColid++;
            }
            
            string strRegionField = GetRegionField(strRegion);

            Dictionary<string, int> DicXfactorItems = new Dictionary<string, int>();
            int ColCnt = 0;
            int indexField = m_pCurFeaCls.Fields.FindField(strXfactorField);
            IField pField = m_pCurFeaCls.Fields.get_Field(indexField);
            IDomain domain = pField.Domain;
            ICodedValueDomain codeDomain = domain as ICodedValueDomain;
            OracleDataReader pReader = null;
            int intRowid = 2;//统计值从第3行开始写，第3行下标为2
            //for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            //{
            ModOracle.DropTable(_OracleConn, "Tmpsum1");
            OracleCommand pCommand = _OracleConn.CreateCommand();
            string strGroup = "";
            strGroup = strGroup + strRegionField + " ";
            strGroup = strGroup + "," + strXfactorField + " ";
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                strGroup = strGroup + "," + ListYfactorFields[i] + " ";
            }
            string strSumField = "";
            for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            {
                strSumField = strSumField + " round(sum(" + ListContentsField[ContentID] + ")*" + ListUnitValues[ContentID] + "," +
                strDigits + ") as tmpmj"+ContentID+",";
            }
            if (strSumField.EndsWith(","))
            {
                strSumField = strSumField.Substring(0, strSumField.Length - 1);
            }
            pCommand.CommandText = "create table Tmpsum1 as select " + strGroup +
                ","+strSumField+" from " + strFeatureClassName + " group by " +
                strGroup;
            pCommand.ExecuteNonQuery();
            //整理一下临时表
            pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "='' where trim(" + strXfactorField + ") is null";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "update Tmpsum1 set " + strRegionField + "='' where trim(" + strRegionField + ") is null";
            pCommand.ExecuteNonQuery();
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "='' where trim(" + ListYfactorFields[i] + ") is null";
                pCommand.ExecuteNonQuery();
            }
            //获取行政区名称
            string strRegionNameField = "regionname";
            DealRegionField(pCommand,"Tmpsum1", strRegionField, strRegion, strRegionNameField);

            //把横轴因子的值转换一下，转成中文
            string strInClause = "";
            pCommand.CommandText = "alter table Tmpsum1 add " + strXfactorField + "name varchar2(100)";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "name ="+ strXfactorField ;
            pCommand.ExecuteNonQuery();
            if (codeDomain != null)
            {
                
                for (int i = 0; i < codeDomain.CodeCount; i++)
                {
                    string strTmpValue = codeDomain.get_Value(i).ToString();
                    string strTmpName = codeDomain.get_Name(i).ToString();
                    if (ListXfactorItemNames.Contains(strTmpName))
                    {
                        
                        pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "name ='" + strTmpName + "' where " + strXfactorField + "='" + strTmpValue + "'";
                        pCommand.ExecuteNonQuery();
                    }
                }
            }
            //把纵轴因子的值转换一下，转成中文
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                int indexFieldY = m_pCurFeaCls.Fields.FindField(ListYfactorFields[i]);
                IField pFieldY = m_pCurFeaCls.Fields.get_Field(indexFieldY);
                IDomain domainY = pFieldY.Domain;
                ICodedValueDomain codeDomainY = domainY as ICodedValueDomain;
                pCommand.CommandText = "alter table Tmpsum1 add " + ListYfactorFields[i] + "name varchar2(100)";
                pCommand.ExecuteNonQuery();
                pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "name ="  + ListYfactorFields[i] ;
                pCommand.ExecuteNonQuery();

                if (codeDomainY != null)
                {
                    for (int j = 0; j < codeDomainY.CodeCount; j++)
                    {
                        string strTmpValueY = codeDomainY.get_Value(j).ToString();
                        string strTmpNameY = codeDomainY.get_Name(j).ToString();
                        
                        pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "name ='" + strTmpNameY + "' where " + ListYfactorFields[i] + "='" + strTmpValueY + "'";
                        pCommand.ExecuteNonQuery();
                    }
                }
            }           

            for (int i = 0; i < ListXfactorItemNames.Count; i++)
            {
                strInClause = strInClause +"'"+ ListXfactorItemNames[i] + "',";
            }
            if (strInClause.EndsWith(","))
            {
                strInClause = strInClause.Substring(0, strInClause.Length - 1);
            }
            string strGroup11 = "";
            strGroup11 = strGroup11 + strRegionNameField + " ";
            strGroup11 = strGroup11 + "," + strXfactorField + "name ";
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                strGroup11 = strGroup11 + "," + ListYfactorFields[i] + "name ";
            }

            string strSumField11 = "";
            for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            {
                strSumField11 = strSumField11 + " sum(tmpmj"+ContentID+") as tmpsum"+ContentID+",";
            }
            if (strSumField11.EndsWith(","))
            {
                strSumField11 = strSumField11.Substring(0, strSumField11.Length - 1);
            }
            _ResTableName = ModOracle.GetNewTableName(_OracleConn, "TmpCustomizeStatistic");
            ModOracle.DropTable(_OracleConn, _ResTableName);
            pCommand.CommandText = "create table "+_ResTableName+" as select " + strGroup11 +
                ","+strSumField11+" from Tmpsum1 where " + strXfactorField + "name in(" + strInClause + ") group by " +
                strGroup11;
            pCommand.ExecuteNonQuery();


            pReader = ModOracle.GetReader(_OracleConn, "select distinct " + strXfactorField + "name from " + _ResTableName + "");
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    string strTmpXfactor = pReader.GetValue(0).ToString();
                    DicXfactorItems.Add(strTmpXfactor, intColid);
                    for (int i = 0; i < ListContentsName.Count; i++)
                    {
                        //if (i == 0)
                        //{
                        dGridViewStatisticRes.Rows[0].Cells[intColid].Value = strTmpXfactor;
                        //}
                        dGridViewStatisticRes.Rows[1].Cells[intColid].Value = ListContentsName[i];
                        intColid++;
                    }

                }
                pReader.Close();
            }
            ColCnt = intColid;

            string strGroup1 = "";
            strGroup1 = strGroup1 + strRegionNameField + " ";
            for (int i = 0; i < ListYfactors.Count; i++)
            {
                strGroup1 = strGroup1 + "," + ListYfactorFields[i] + "name ";
            }
            ModOracle.DropTable(_OracleConn, "TmpGroup1");
            pCommand.CommandText = "create table TmpGroup1 as select distinct " + strGroup1 + " from " + _ResTableName;
            pCommand.ExecuteNonQuery();
            
            pReader = ModOracle.GetReader(_OracleConn, "select " + strGroup1 + " from TmpGroup1 order by " + strGroup1);
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    dGridViewStatisticRes.Rows.Add();
                    string strTmpWhere = "";
                    string strRegionValue = pReader.GetValue(0).ToString();
                    strTmpWhere = strRegionNameField + "='" + strRegionValue + "' ";
                    dGridViewStatisticRes.Rows[intRowid].Cells[0].Value = strRegionValue;
                    intColid = 1;
                    for (int i = 0; i < ListYfactorFields.Count; i++)
                    {
                        string strTmpValue = pReader.GetValue(i + 1).ToString();
                        strTmpWhere =strTmpWhere+ "and " + ListYfactorFields[i] + "name ='" + strTmpValue + "' ";
                        dGridViewStatisticRes.Rows[intRowid].Cells[intColid].Value = strTmpValue;
                        intColid++;
                    }
                    for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
                    {
                        OracleDataReader pTmpReader = ModOracle.GetReader(_OracleConn, "select " + strXfactorField + "name,tmpsum" + ContentID + " from " + _ResTableName + " where " + strTmpWhere);
                        if (pTmpReader != null)
                        {
                            while (pTmpReader.Read())
                            {
                                string strXfactorValue = pTmpReader.GetValue(0).ToString();
                                string strArea = pTmpReader.GetValue(1).ToString();
                                //根据 横轴因子小类 查找统计值写在哪一列
                                int intColXfactor = DicXfactorItems[strXfactorValue];
                                intColid = intColXfactor + ContentID;
                                //写统计值
                                dGridViewStatisticRes.Rows[intRowid].Cells[intColid].Value = strArea;

                            }
                            pTmpReader.Close();
                        }
                        pTmpReader = ModOracle.GetReader(_OracleConn, "select sum(tmpsum" + ContentID + ") from " + _ResTableName + " where " + strTmpWhere);
                        if (pTmpReader != null)
                        {
                            while (pTmpReader.Read())
                            {

                                string strArea = pTmpReader.GetValue(0).ToString();
                                intColid = 1 + ListYfactorFields.Count + ContentID;
                                //写合计值
                                dGridViewStatisticRes.Rows[intRowid].Cells[intColid].Value = strArea;

                            }
                            pTmpReader.Close();
                        }
                    }
                    
                    intRowid++;
                }
                pReader.Close();
            }
            //}
            //整理一下统计结果的格式
            //DealFormatOfDataGrid(ListYfactorFields.Count);
            cmbY.Items.Clear();
            for (int i = 0; i < ListYfactors.Count; i++)
            {
                cmbY.Items.Add(ListYfactors[i]);
            }
            if (cmbY.Items.Count > 0)
            {
                cmbY.SelectedIndex = 0;
            }
            cmbContent.Items.Clear();
            for (int i = 0; i < ListContentsName.Count; i++)
            {
                cmbContent.Items.Add(ListContentsName[i]);
            }
            if (cmbContent.Items.Count > 0)
            {
                cmbContent.SelectedIndex = 0;
            }
            _InStatistic = true;
            if (cmbChartType.Items.Count > 0)
            {
                cmbChartType.SelectedIndex = 0;
            }

            RefreshChart();
            _InStatistic = false;
        }
        private void WriteHeader(List<string> ListYfactors, List<string> ListXfactorItemNames, List<string> ListContentsName)
        {
            int colcnt = 1;
            DataGridViewTextBoxColumn pColumn = new DataGridViewTextBoxColumn();
            pColumn.Name = "Column" + colcnt.ToString(); colcnt++;
            pColumn.HeaderText = "统计单位";
            rowMergeView1.Columns.Add(pColumn);
            //Y因子
            for (int i = 0; i < ListYfactors.Count; i++) 
            {
                DataGridViewTextBoxColumn pColumnY = new DataGridViewTextBoxColumn();
                pColumnY.Name = "Column" + colcnt.ToString(); colcnt++;
                pColumnY.HeaderText = ListYfactors[i];
                rowMergeView1.Columns.Add(pColumnY);
            }
            //合计
            for (int j = 0; j < ListContentsName.Count; j++)
            {
                DataGridViewTextBoxColumn pColumnY = new DataGridViewTextBoxColumn();
                pColumnY.Name = "Column" + colcnt.ToString(); colcnt++;
                pColumnY.HeaderText = ListContentsName[j];
                rowMergeView1.Columns.Add(pColumnY);                
            }
            //X因子元素
            for (int i = 0; i < ListXfactorItemNames.Count; i++)
            {
                for (int j = 0; j < ListContentsName.Count; j++)
                {
                    DataGridViewTextBoxColumn pColumnY = new DataGridViewTextBoxColumn();
                    pColumnY.Name = "Column" + colcnt.ToString(); colcnt++;
                    pColumnY.HeaderText = ListContentsName[j];
                    rowMergeView1.Columns.Add(pColumnY);
                }
            }
            //ygc 20130402  改变表格表头样式，使其不能排序
            for (int t = 0; t < rowMergeView1.Columns.Count; t++)
            {
                rowMergeView1.Columns[t].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        //用新的控件重写代码
        private void DoCustomStatisticEx()
        {
            rowMergeView1.Visible = false;
            string strFeatureClassName = (m_pCurFeaCls as IDataset).Name;
            string strXfactor = cmbXfactor.Text;
            string strXFieldName = GetFieldNameByChiName(strXfactor);//ygc 20130403
            List<string> ListXfactorItemNames = new List<string>();
            List<string> ListXfactorChinese = new List<string>();//ygc 20130403 添加横轴翻译
           
            for (int i = 0; i < dataGridXfactors.RowCount; i++)
            {
                if (dataGridXfactors.Rows[i].Cells[0].Value != null)
                {
                    if (dataGridXfactors.Rows[i].Cells[0].Value.ToString() == "True")
                    {
                        string strTmp = dataGridXfactors.Rows[i].Cells[1].Value.ToString();
                        ListXfactorItemNames.Add(strTmp);
                        //将代码转换成中文
                        ListXfactorChinese.Add(ModXZQ .GetChineseName (Plugin .ModuleCommon .TmpWorkSpace ,strXFieldName,strTmp));
                    }
                }

            }
            if (ListXfactorItemNames.Count == 0)
            {
                MessageBox.Show("请选定横轴因子的统计项!");
                ListXfactorItemNames = null;
                return;
            }
            List<string> ListContentsName = new List<string>();
            List<string> ListContentsField = new List<string>();
            List<string> ListUnits = new List<string>();    //单位名称
            List<string> ListUnitValues = new List<string>();   //转换系数 如米转换成千米乘以系数：0.001
            for (int i = 0; i < ListContents.RowCount; i++)
            {
                if (ListContents.Rows[i].Cells[0].Value != null)
                {
                    if (ListContents.Rows[i].Cells[0].Value.ToString() == "True")
                    {
                        string strContent = ListContents.Rows[i].Cells[1].Value.ToString();
                        string strContentField = GetContentField(strContent);
                        string strUnit = ListContents.Rows[i].Cells[2].Value.ToString();
                        string strUnitValue = GetUnitValue(strContent, strUnit);
                        ListContentsName.Add(strContent);
                        ListContentsField.Add(strContentField);
                        ListUnits.Add(strUnit);
                        ListUnitValues.Add(strUnitValue);
                    }

                }

            }
            if (ListContentsName.Count == 0)
            {
                MessageBox.Show("请选定统计内容!");
                ListXfactorItemNames.Clear();
                ListContentsName = null;
                ListContentsField = null;
                ListUnits = null;
                ListUnitValues = null;
                return;
            }
            _ContentCnt = ListContentsName.Count;
            List<string> ListYfactors = new List<string>();
            List<string> ListYfactorFields = new List<string>();
            if (cmbYfactor1.Text != "")
            {
                string strYfactor1Field = GetFieldNameByChiName(cmbYfactor1.Text);
                if (strYfactor1Field != null)
                {
                    ListYfactors.Add(cmbYfactor1.Text);
                    ListYfactorFields.Add(strYfactor1Field);
                }
            }

            if (cmbYfactor2.Text != "")
            {
                string strYfactor2Field = GetFieldNameByChiName(cmbYfactor2.Text);
                if (strYfactor2Field != null)
                {
                    ListYfactors.Add(cmbYfactor2.Text);
                    ListYfactorFields.Add(strYfactor2Field);
                }
            }
            if (cmbYfactor3.Text != "")
            {
                string strYfactor3Field = GetFieldNameByChiName(cmbYfactor3.Text);
                if (strYfactor3Field != null)
                {
                    ListYfactors.Add(cmbYfactor3.Text);
                    ListYfactorFields.Add(strYfactor3Field);
                }
            }
            _YfactorCnt = ListYfactorFields.Count;
            string strRegion = cmbRegion.Text;
            string strDigits = cmbDigits.Text;

            string strXfactorField = GetFieldNameByChiName(strXfactor);

            


            //写统计结果表头
            int intColid = 0;
            //WriteHeader(ListYfactors,ListXfactorItemNames, ListContentsName);
            //ygc 20130403 修改写统计结果表头
            WriteHeader(ListYfactors, ListXfactorChinese, ListContentsName);

            //rowMergeView1.ColumnCount = 1 + 3 + ListXfactorItemNames.Count * ListContentsField.Count + 1;
            rowMergeView1.Rows.Add();
            rowMergeView1.Rows.Add();

            string strRegionField = GetRegionField(strRegion);

            
            int ColCnt = 0;
            //int indexField = m_pCurFeaCls.Fields.FindField(strXfactorField);
            //IField pField = m_pCurFeaCls.Fields.get_Field(indexField);
            //IDomain domain = pField.Domain;
            //ICodedValueDomain codeDomain = domain as ICodedValueDomain;
            OracleDataReader pReader = null;
            int intRowid = 0;//统计值从第3行开始写，第3行下标为2
            //for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            //{
            ModOracle.DropTable(_OracleConn, "Tmpsum1");
            OracleCommand pCommand = _OracleConn.CreateCommand();
            string strGroup = "";
            strGroup = strGroup + strRegionField + " ";
            strGroup = strGroup + "," + strXfactorField + " ";
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                strGroup = strGroup + "," + ListYfactorFields[i] + " ";
            }
            string strSumField = "";
            for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            {
                strSumField = strSumField + " round(sum(" + ListContentsField[ContentID] + ")*" + ListUnitValues[ContentID] + "," +
                strDigits + ") as tmpmj" + ContentID + ",";
            }
            if (strSumField.EndsWith(","))
            {
                strSumField = strSumField.Substring(0, strSumField.Length - 1);
            }
            pCommand.CommandText = "create table Tmpsum1 as select " + strGroup +
                "," + strSumField + " from " + strFeatureClassName + " group by " +
                strGroup;
            pCommand.ExecuteNonQuery();
            //整理一下临时表
            pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "='' where trim(" + strXfactorField + ") is null";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "update Tmpsum1 set " + strRegionField + "='' where trim(" + strRegionField + ") is null";
            pCommand.ExecuteNonQuery();
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "='' where trim(" + ListYfactorFields[i] + ") is null";
                pCommand.ExecuteNonQuery();
            }
            //获取行政区名称
            string strRegionNameField = "regionname";
            DealRegionField(pCommand, "Tmpsum1", strRegionField, strRegion, strRegionNameField);

            //把横轴因子的值转换一下，转成中文
            string strInClause = "";
            pCommand.CommandText = "alter table Tmpsum1 add " + strXfactorField + "name varchar2(100)";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "name =" + strXfactorField;
            pCommand.ExecuteNonQuery();
            //if (codeDomain != null)
            //{

            //    for (int i = 0; i < codeDomain.CodeCount; i++)
            //    {
            //        string strTmpValue = codeDomain.get_Value(i).ToString();
            //        string strTmpName = codeDomain.get_Name(i).ToString();
            //        if (ListXfactorItemNames.Contains(strTmpName))
            //        {

            //            pCommand.CommandText = "update Tmpsum1 set " + strXfactorField + "name ='" + strTmpName + "' where " + strXfactorField + "='" + strTmpValue + "'";
            //            pCommand.ExecuteNonQuery();
            //        }
            //    }
            //}
            //ygc 20130407 把对临时表进行翻译
            TranslateTempTable(_OracleConn, "Tmpsum1", strXfactorField , strXfactorField + "name");
            //把纵轴因子的值转换一下，转成中文
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                //int indexFieldY = m_pCurFeaCls.Fields.FindField(ListYfactorFields[i]);
                //IField pFieldY = m_pCurFeaCls.Fields.get_Field(indexFieldY);
                //IDomain domainY = pFieldY.Domain;
                //ICodedValueDomain codeDomainY = domainY as ICodedValueDomain;
                pCommand.CommandText = "alter table Tmpsum1 add " + ListYfactorFields[i] + "name varchar2(100)";
                pCommand.ExecuteNonQuery();
                pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "name =" + ListYfactorFields[i];
                pCommand.ExecuteNonQuery();

                //if (codeDomainY != null)
                //{
                //    for (int j = 0; j < codeDomainY.CodeCount; j++)
                //    {
                //        string strTmpValueY = codeDomainY.get_Value(j).ToString();
                //        string strTmpNameY = codeDomainY.get_Name(j).ToString();

                //        pCommand.CommandText = "update Tmpsum1 set " + ListYfactorFields[i] + "name ='" + strTmpNameY + "' where " + ListYfactorFields[i] + "='" + strTmpValueY + "'";
                //        pCommand.ExecuteNonQuery();
                //    }
                //}
                //ygc 20130407 对临时表进行翻译
                TranslateTempTable(_OracleConn, "Tmpsum1", ListYfactorFields[i], ListYfactorFields[i] + "name");
            }

            for (int i = 0; i < ListXfactorItemNames.Count; i++)
            {
                strInClause = strInClause + "'" + ListXfactorItemNames[i] + "',";
            }
            if (strInClause.EndsWith(","))
            {
                strInClause = strInClause.Substring(0, strInClause.Length - 1);
            }
            string strGroup11 = "";
            strGroup11 = strGroup11 + strRegionNameField + " ";
            strGroup11 = strGroup11 + "," + strXfactorField + "name ";
            for (int i = 0; i < ListYfactorFields.Count; i++)
            {
                strGroup11 = strGroup11 + "," + ListYfactorFields[i] + "name ";
            }

            string strSumField11 = "";
            for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
            {
                strSumField11 = strSumField11 + " sum(tmpmj" + ContentID + ") as tmpsum" + ContentID + ",";
            }
            if (strSumField11.EndsWith(","))
            {
                strSumField11 = strSumField11.Substring(0, strSumField11.Length - 1);
            }
            _ResTableName = ModOracle.GetNewTableName(_OracleConn, "TmpCustomizeStatistic");
            ModOracle.DropTable(_OracleConn, _ResTableName);
            pCommand.CommandText = "create table " + _ResTableName + " as select " + strGroup11 +
                "," + strSumField11 + " from Tmpsum1 where " + strXfactorField + "name in(" + strInClause + ") group by " +
                strGroup11;
            pCommand.ExecuteNonQuery();
            
            intColid = 1 + ListYfactors.Count;
            _DicXfactorItems.Clear();
            _DicXfactorItems.Add("合计", intColid);
            this.rowMergeView1.AddSpanHeader(intColid, ListContentsName.Count, "合计");
            intColid = intColid + ListContentsName.Count;
            pReader = ModOracle.GetReader(_OracleConn, "select distinct " + strXfactorField + "name from " + _ResTableName + "");
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    string strTmpXfactor = pReader.GetValue(0).ToString();
                    _DicXfactorItems.Add(strTmpXfactor, intColid);
                    this.rowMergeView1.AddSpanHeader(intColid, ListContentsName.Count, strTmpXfactor);
                    intColid = intColid + ListContentsName.Count;
                }
                pReader.Close();
            }
            if (intColid < this.rowMergeView1.Columns.Count)
            {
                for (int i = rowMergeView1.Columns.Count-1; i >intColid-1; i--)
                {
                    rowMergeView1.Columns.RemoveAt(i);
                }
            }
            //ColCnt = intColid;

            string strGroup1 = "";
            strGroup1 = strGroup1 + strRegionNameField + " ";
            for (int i = 0; i < ListYfactors.Count; i++)
            {
                strGroup1 = strGroup1 + "," + ListYfactorFields[i] + "name ";
            }
            ModOracle.DropTable(_OracleConn, "TmpGroup1");
            pCommand.CommandText = "create table TmpGroup1 as select distinct " + strGroup1 + " from " + _ResTableName;
            pCommand.ExecuteNonQuery();

            pReader = ModOracle.GetReader(_OracleConn, "select " + strGroup1 + " from TmpGroup1 order by " + strGroup1);
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    rowMergeView1.Rows.Add();
                    string strTmpWhere = "";
                    string strRegionValue = pReader.GetValue(0).ToString();
                    strTmpWhere = strRegionNameField + "='" + strRegionValue + "' ";
                    rowMergeView1.Rows[intRowid].Cells[0].Value = strRegionValue;
                    intColid = 1;
                    for (int i = 0; i < ListYfactorFields.Count; i++)
                    {
                        string strTmpValue = pReader.GetValue(i + 1).ToString();
                        if (strTmpValue == "")
                        {
                            strTmpWhere = strTmpWhere + "and trim(" + ListYfactorFields[i] + "name) is null ";
                            rowMergeView1.Rows[intRowid].Cells[intColid].Value = "小计";
                        }
                        else
                        {
                            strTmpWhere = strTmpWhere + "and " + ListYfactorFields[i] + "name ='" + strTmpValue + "' ";
                            rowMergeView1.Rows[intRowid].Cells[intColid].Value = strTmpValue;
                        }
                      
                        intColid++;
                    }
                    for (int ContentID = 0; ContentID < ListContentsField.Count; ContentID++)
                    {
                        OracleDataReader pTmpReader = ModOracle.GetReader(_OracleConn, "select " + strXfactorField + "name,tmpsum" + ContentID + " from " + _ResTableName + " where " + strTmpWhere);
                        if (pTmpReader != null)
                        {
                            while (pTmpReader.Read())
                            {
                                string strXfactorValue = pTmpReader.GetValue(0).ToString();
                                string strArea = pTmpReader.GetValue(1).ToString();
                                //根据 横轴因子小类 查找统计值写在哪一列
                                int intColXfactor = _DicXfactorItems[strXfactorValue];
                                intColid = intColXfactor + ContentID;
                                //写统计值
                                rowMergeView1.Rows[intRowid].Cells[intColid].Value = strArea;

                            }
                            pTmpReader.Close();
                        }
                        pTmpReader = ModOracle.GetReader(_OracleConn, "select sum(tmpsum" + ContentID + ") from " + _ResTableName + " where " + strTmpWhere);
                        if (pTmpReader != null)
                        {
                            while (pTmpReader.Read())
                            {

                                string strArea = pTmpReader.GetValue(0).ToString();
                                intColid = 1 + ListYfactorFields.Count + ContentID;
                                //写合计值
                                rowMergeView1.Rows[intRowid].Cells[intColid].Value = strArea;

                            }
                            pTmpReader.Close();
                        }
                    }

                    intRowid++;
                }
                pReader.Close();
            }
            //}
            //整理一下统计结果的格式
            //DealFormatOfDataGrid(ListYfactorFields.Count);
            cmbY.Items.Clear();
            for (int i = 0; i < ListYfactors.Count; i++)
            {
                cmbY.Items.Add(ListYfactors[i]);
            }
            if (cmbY.Items.Count > 0)
            {
                cmbY.SelectedIndex = 0;
            }
            cmbContent.Items.Clear();
            for (int i = 0; i < ListContentsName.Count; i++)
            {
                cmbContent.Items.Add(ListContentsName[i]);
            }
            if (cmbContent.Items.Count > 0)
            {
                cmbContent.SelectedIndex = 0;
            }
            _InStatistic = true;
            if (cmbChartType.Items.Count > 0)
            {
                cmbChartType.SelectedIndex = 0;
            }
            //rowMergeView1.Rows.Add();
            rowMergeView1.MergeColumnNames.Add("Column1");
            rowMergeView1.MergeColumnNames.Add("Column2");
            rowMergeView1.Visible = true;
            RefreshChart();
            _InStatistic = false;
        }
        private string GetRegionField(string strRegion)
        {
            if (_StatisticConfigXmldoc != null)
            {
                try
                {
                    string strSearch = "//StatisticConfig/Region/RegionItem[@ItemText='" + strRegion + "']";
                    XmlNode pNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                    if (pNode != null)
                    {
                        string strRes = pNode.Attributes["ItemField"].Value;
                        return strRes;
                    }
                }
                catch (System.Exception ex)
                {                	
                }
                
            }
            return "";
        }
        private string GetContentField(string strContent)
        {
            if (_StatisticConfigXmldoc != null)
            {
                try
                {
                    string strSearch = "//StatisticConfig/StatisticContent/ContentItem[@ItemText='" + strContent + "']";
                    XmlNode pNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);
                    if (pNode != null)
                    {
                        string strRes = pNode.Attributes["ItemField"].Value;
                        return strRes;
                    }
                }
                catch (System.Exception ex)
                {
                }

            }
            return "";
        }
        private string GetUnitValue(string strContent,string strUnitName)
        {
            if (_StatisticConfigXmldoc != null)
            {
                try
                {
                    string strSearch = "//StatisticConfig/StatisticContent/ContentItem[@ItemText='" + strContent + "']";
                    XmlNode pNode = _StatisticConfigXmldoc.SelectSingleNode(strSearch);

                    if (pNode != null)
                    {
                        string strSearch2 = ".//UnitItem[@ItemText='"+strUnitName+"']";
                        XmlNode pUnitNode = pNode.SelectSingleNode(strSearch2);
                        if (pUnitNode != null)
                        {
                            string strRes = pUnitNode.Attributes["ItemValue"].Value;
                            return strRes;
                        }
                        
                    }
                }
                catch (System.Exception ex)
                {
                }

            }
            return "";
        }
        private string GetFieldNameByChiName(string strChineseName)
        {
            try
            {
                if (_DicFieds.ContainsKey(strChineseName))
                {
                    int index = _DicFieds[strChineseName];
                    string strFieldName = m_pCurFeaCls.Fields.get_Field(index).Name;
                    return strFieldName;
                }
            }
            catch (System.Exception ex)
            {            	
            }
            return "";            
        }

        private void chkboxXfactorItems_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxXfactorItems.Checked)
            {
                for (int i = 0; i < dataGridXfactors.RowCount; i++)
                {
                    dataGridXfactors.Rows[i].Cells[0].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridXfactors.RowCount; i++)
                {
                    dataGridXfactors.Rows[i].Cells[0].Value = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupPanelXfactors.Visible = false;
        }
        private void DealRegionField(OracleCommand pCommand,string strTable, string strRegionField, string strType, string RegionNameField)
        {
            IWorkspace pWorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            IPropertySet pPropset= pWorkSpace.ConnectionProperties;
            string strUser = pPropset.GetProperty("User").ToString();
            ModOracle.DropTable(_OracleConn, "tmpdicxzq");
            switch (strType)
            {
                case "县":
                    pCommand.CommandText = "update Tmpsum1 set " + strRegionField + "=substr(" + strRegionField + ",0,6)";
                    pCommand.ExecuteNonQuery();
                    pCommand.CommandText = "create table tmpdicxzq as select distinct code,name from "+strUser+".行政区字典表 where xzjb='3'";
                    pCommand.ExecuteNonQuery();
                    break;
                case "乡":
                    pCommand.CommandText = "update Tmpsum1 set " + strRegionField + "=substr(" + strRegionField + ",0,8)";
                    pCommand.ExecuteNonQuery();
                    pCommand.CommandText = "create table tmpdicxzq as select distinct code,name from " + strUser + ".行政区字典表 where xzjb='4'";
                    pCommand.ExecuteNonQuery();
                    break;
                case "村":
                    pCommand.CommandText = "update Tmpsum1 set " + strRegionField + "=substr(" + strRegionField + ",0,10)"; 
                    pCommand.ExecuteNonQuery();
                    pCommand.CommandText = "create table tmpdicxzq as select distinct code,name from " + strUser + ".行政区字典表 where xzjb='5'";
                    pCommand.ExecuteNonQuery();
                    break;
            }
            pCommand.CommandText = "delete from tmpdicxzq where code in(select code from tmpdicxzq group by code having count(*)>1)";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "alter table tmpdicxzq add primary key(code)";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "alter table "+strTable+" add "+RegionNameField+" varchar2(100)";
            pCommand.ExecuteNonQuery();
            pCommand.CommandText = "update(select a." + RegionNameField + " as aname,b.name from " + strTable + " a,tmpdicxzq b where a." + strRegionField + "=b.code )set aname=name";
            pCommand.ExecuteNonQuery();
            ModOracle.DropTable(_OracleConn, "tmpdicxzq");

        }
        private void DealFormatOfDataGrid(int iYfactorCnt)
        {
            for (int i = dGridViewStatisticRes.ColumnCount-1; i >0; i--)
            {
                string strCur = "";
                object objCur=dGridViewStatisticRes.Rows[0].Cells[i].Value;
                if (objCur != null)
                {
                    strCur = objCur.ToString();
                }
                string strPre = "";
                object objPre= dGridViewStatisticRes.Rows[0].Cells[i - 1].Value;
                if (objPre != null)
                {
                    strPre = objPre.ToString();
                }
                if (strCur == strPre)
                {
                    dGridViewStatisticRes.Rows[0].Cells[i].Value = "";
                }
            }
            for (int i = 0; i < 2; i++)     //i < 1 + iYfactorCnt;=> i<2
            {
                for (int j = dGridViewStatisticRes.RowCount - 2; j > 2; j--)
                {
                    string strCur = "";
                    object objCur=dGridViewStatisticRes.Rows[j].Cells[i].Value;
                    string strPre = "";
                    object objPre=dGridViewStatisticRes.Rows[j - 1].Cells[i].Value;
                    if (objCur != null)
                    {
                        strCur = objCur.ToString();
                    }
                    if (objPre != null)
                    {
                        strPre = objPre.ToString();
                    }
                    if (strCur == strPre)
                    {
                        dGridViewStatisticRes.Rows[j].Cells[i].Value = "";
                    }
                }
            }
        }
        private void DealFormatOfExcel(Microsoft.Office.Interop.Excel.Application excel,int iYfactorCnt)
        {
            for (int i = dGridViewStatisticRes.ColumnCount - 1; i > 0; i--)
            {
                string strCur = "";
                object objCur = dGridViewStatisticRes.Rows[0].Cells[i].Value;
                if (objCur != null)
                {
                    strCur = objCur.ToString();
                }
                if (strCur == "")
                {
                    try
                    {
                        excel.get_Range(excel.Cells[1, i], excel.Cells[1, i + 1]).Merge(false);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            for (int i = 0; i < 2; i++)//i < 1 + iYfactorCnt => i < 2  第2个纵轴因子不能随意合并
            {
                for (int j = dGridViewStatisticRes.RowCount - 1; j > 2; j--)
                {
                    string strCur = "";
                    try
                    {
                        object objCur = dGridViewStatisticRes.Rows[j].Cells[i].Value;
                        if (objCur != null)
                        {
                            strCur = objCur.ToString();
                        }
                    }
                    catch (System.Exception ex)
                    {                    	
                    }
                    
                    if (strCur == "")
                    {
                        try
                        {
                            excel.get_Range(excel.Cells[j, i + 1], excel.Cells[j + 1, i + 1]).Merge(false);
                        }
                        catch (System.Exception ex)
                        {                        	
                        }                        
                    }
                }
            }
            //统计单位标题，单元格合并
            excel.get_Range(excel.Cells[1, 1], excel.Cells[2, 1]).Merge(false);
            //纵轴因子标题，单元格合并
            string strCur0 = "";
            object objCur0 = dGridViewStatisticRes.Rows[1].Cells[1].Value;
            if (objCur0 != null)
            {
                strCur0 = objCur0.ToString();
            }
            if (strCur0 == "")
            {
                excel.get_Range(excel.Cells[1, 2], excel.Cells[2, 2]).Merge(false);
            }
            strCur0 = ""; objCur0 = null;
            objCur0 = dGridViewStatisticRes.Rows[1].Cells[2].Value;
            if (objCur0 != null)
            {
                strCur0 = objCur0.ToString();
            }
            if (strCur0 == "")
            {
                excel.get_Range(excel.Cells[1, 3], excel.Cells[2, 3]).Merge(false);
            }
            strCur0 = ""; objCur0 = null;
            objCur0 = dGridViewStatisticRes.Rows[1].Cells[3].Value;
            if (objCur0 != null)
            {
                strCur0 = objCur0.ToString();
            }
            if (strCur0 == "")
            {
                excel.get_Range(excel.Cells[1, 4], excel.Cells[2, 4]).Merge(false);
            }
            excel.get_Range(excel.Cells[1, 1], excel.Cells[2, dGridViewStatisticRes.ColumnCount]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[dGridViewStatisticRes.RowCount, 1+_YfactorCnt]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }
        private void DealFormatOfExcelEx(Microsoft.Office.Interop.Excel.Application excel, int iYfactorCnt)
        {
            //for (int i = dGridViewStatisticRes.ColumnCount - 1; i > 0; i--)
            //{
            //    string strCur = "";
            //    object objCur=dGridViewStatisticRes.Rows[0].Cells[i].Value;
            //    if (objCur != null)
            //    {
            //        strCur = objCur.ToString();
            //    }
            //    string strPre = "";
            //    object objPre= dGridViewStatisticRes.Rows[0].Cells[i - 1].Value;
            //    if (objPre != null)
            //    {
            //        strPre = objPre.ToString();
            //    }
            //    if (strCur == strPre)
            //    {
            //        try
            //        {
            //            excel.Cells[1, i + 1] = "";
            //            excel.get_Range(excel.Cells[1, i], excel.Cells[1, i + 1]).Merge(false);
            //        }
            //        catch (System.Exception ex)
            //        {
            //        }
            //    }
            //}
            for (int i = 0; i < 2; i++)//i < 1 + iYfactorCnt => i < 2  第2个纵轴因子不能随意合并
            {
                for (int j = rowMergeView1.RowCount - 1; j > 0; j--)
                {
                    string strCur = "";
                    object objCur = rowMergeView1.Rows[j].Cells[i].Value;
                    string strPre = "";
                    object objPre = rowMergeView1.Rows[j - 1].Cells[i].Value;
                    if (objCur != null)
                    {
                        strCur = objCur.ToString();
                    }
                    if (objPre != null)
                    {
                        strPre = objPre.ToString();
                    }
                    if (strCur == strPre && strCur!="")　
                    {
                        try
                        {
                            excel.Cells[j + 3, i + 1] = "";
                            excel.get_Range(excel.Cells[j+2, i + 1], excel.Cells[j + 3, i + 1]).Merge(false);
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
            }
            //统计单位标题，单元格合并
            //excel.get_Range(excel.Cells[1, 1], excel.Cells[2, 1]).Merge(false);
            //纵轴因子标题，单元格合并
            //string strCur0 = "";
            //object objCur0 = dGridViewStatisticRes.Rows[1].Cells[1].Value;
            //if (objCur0 != null)
            //{
            //    strCur0 = objCur0.ToString();
            //}
            //if (strCur0 == "")
            //{
            //    excel.get_Range(excel.Cells[1, 2], excel.Cells[2, 2]).Merge(false);
            //}
            //strCur0 = ""; objCur0 = null;
            //objCur0 = dGridViewStatisticRes.Rows[1].Cells[2].Value;
            //if (objCur0 != null)
            //{
            //    strCur0 = objCur0.ToString();
            //}
            //if (strCur0 == "")
            //{
            //    excel.get_Range(excel.Cells[1, 3], excel.Cells[2, 3]).Merge(false);
            //}
            //strCur0 = ""; objCur0 = null;
            //objCur0 = dGridViewStatisticRes.Rows[1].Cells[3].Value;
            //if (objCur0 != null)
            //{
            //    strCur0 = objCur0.ToString();
            //}
            //if (strCur0 == "")
            //{
            //    excel.get_Range(excel.Cells[1, 4], excel.Cells[2, 4]).Merge(false);
            //}
            excel.get_Range(excel.Cells[1, 1], excel.Cells[2, rowMergeView1.ColumnCount]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            excel.get_Range(excel.Cells[1, 1], excel.Cells[rowMergeView1.RowCount, 1 + _YfactorCnt]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }

        private void btnShowXitems_Click(object sender, EventArgs e)
        {
            groupPanelXfactors.Left = cmbXfactor.Right;
            groupPanelXfactors.Top = cmbXfactor.Top + groupPanelAutoConfig.Top+10;
            groupPanelXfactors.Visible = true;
        }
        private void RefreshChart()
        {
            string strY = cmbY.Text;
            string strContent = cmbContent.Text;
            if (strY == "")
            {
                MessageBox.Show("请选择纵轴因子!");
                return;
            }
            if (strContent == "")
            {
                MessageBox.Show("请选择内容因子!");
                return;
            }
            int indexY = cmbY.SelectedIndex;
            int indexContent = cmbContent.SelectedIndex;
            string strFieldNameY = "";
            if (_DicFieds.ContainsKey(strY))
            {
                int index = _DicFieds[strY];
                strFieldNameY = m_pCurFeaCls.Fields.get_Field(index).Name;
            }
            string strContentFieldname = GetContentField(strContent);
            string strX = cmbXfactor.Text;
            string strFieldNameX = "";
            if (_DicFieds.ContainsKey(strX))
            {
                int index = _DicFieds[strX];
                strFieldNameX = m_pCurFeaCls.Fields.get_Field(index).Name;
            }
            ArrayList Xtitle = new ArrayList();
            OracleDataReader pReader = ModOracle.GetReader(_OracleConn, "select distinct " + strFieldNameX + "name from " + _ResTableName);
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    string strTmpX = pReader.GetValue(0).ToString();
                    Xtitle.Add(strTmpX);
                }
                pReader.Close();
            }

            ArrayList ColorGuider = new ArrayList();
            pReader = ModOracle.GetReader(_OracleConn, "select distinct " + strFieldNameY + "name from " + _ResTableName);
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    string strTmpY = pReader.GetValue(0).ToString();
                    ColorGuider.Add(strTmpY);
                }
                pReader.Close();
            }
            ArrayList[] Datalist = new ArrayList[ColorGuider.Count];
            for (int k = 0; k < ColorGuider.Count; k++)
            {
                Datalist[k] = new ArrayList();
                for (int i = 0; i < Xtitle.Count; i++)
                {
                    Datalist[k].Add(0);
                }
            }
            double dMaxValue = 0;
            pReader = ModOracle.GetReader(_OracleConn, "select " + strFieldNameX + "name," + strFieldNameY + "name,sum(tmpsum" + indexContent + ") as summj from " + _ResTableName + " group by " + strFieldNameX + "name," + strFieldNameY+"name");
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    string strXvalue = pReader.GetValue(0).ToString();
                    string strYvalue = pReader.GetValue(1).ToString();
                    string strsumValue = pReader.GetValue(2).ToString();
                    int ix = -1; int iy = -1;
                    if (strsumValue == "0")
                    {
                        continue;
                    }
                    for (int ii = 0; ii < Xtitle.Count; ii++)
                    {
                        if (strXvalue == Xtitle[ii].ToString())
                        {
                            iy = ii;
                            break;
                        }
                    }
                    for (int kk = 0; kk < ColorGuider.Count; kk++)
                    {
                        if (strYvalue == ColorGuider[kk].ToString())
                        {
                            ix = kk;
                            break;
                        }
                    }
                    if (ix >= 0 && iy >= 0)
                    {
                        Datalist[ix][iy] = double.Parse(strsumValue);
                        if (dMaxValue < double.Parse(strsumValue))
                        {
                            dMaxValue = double.Parse(strsumValue);
                        }
                    }
                }
                pReader.Close();
            }
            string strValueFormat = GetValueFormat(dMaxValue);

            this.chartlet.ChartTitle.Text = "统计图";
            this.chartlet.XLabels.UnitText = strX;
            this.chartlet.YLabels.UnitText = strContent;
            switch (cmbChartType.Text)
            {
                case "柱状图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_3D_Aurora_FlatCrystal_NoGlow_NoBorder;
                    break;
                case "线划图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Line_3D_Aurora_FlatCrystalNone_NoGlow_NoBorder;
                    break;
                case "饼状图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_2D_Aurora_FlatCrystal_NoGlow_NoBorder;
                    break;
            }
            //this.chartlet.YLabels.ValueFormat = strValueFormat;  //不起作用
            //ygc 当纵轴和数据为0时会报错
            //添加判断条件 20130402 ygc
            if (Xtitle.Count != 0)
            {
                this.chartlet.InitializeData(Datalist, Xtitle, ColorGuider);
            }
            this.chartlet.Refresh();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshChart();
        }
        private int GetStep(double dMaxValue)
        {
            double dStep = dMaxValue / 4;
            
            int d0Cnt = 0;
            while (dStep >= 10)
            {
                dStep = dStep / 10;
                d0Cnt++;
            }
            int iStep = (int)(Math.Floor(dStep));
            int resStep = iStep;
            for (int i = 0; i < d0Cnt; i++)
            {
                resStep = resStep * 10;
            }
            return resStep;
        }
        private string GetValueFormat(double dMaxValue)
        {
            double dStep = dMaxValue / 4;

            int d0Cnt = 0;
            while (dStep >= 10)
            {
                dStep = dStep / 10;
                d0Cnt++;
            }
            string strRes = "";
            for (int i = 0; i < d0Cnt; i++)
            {
                strRes = strRes +'0';
            }
            return strRes;
        }

        private void cmbChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_InStatistic)
            {
                return;
            }
            switch (cmbChartType.Text)
            {
                case "柱状图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_3D_Aurora_FlatCrystal_NoGlow_NoBorder;
                    break;
                case "线划图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Line_3D_Aurora_FlatCrystalNone_NoGlow_NoBorder;
                    break;
                case "饼状图":
                    this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_2D_Aurora_FlatCrystal_NoGlow_NoBorder;
                    break;
            }
            this.chartlet.Refresh();
           
        }

        private void advTreeLayers_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            string LayerID = "";
            if (this.advTreeLayers.SelectedNode == null)
                return;
            if (advTreeLayers.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
            {
                return;
            }

            LayerID = GetNodeKey(advTreeLayers.SelectedNode);
            string DataSourceKey = GetDataSourceKey(advTreeLayers.SelectedNode);

            if (string.IsNullOrEmpty(LayerID))
                return;
            this.advTreeLayers.Visible = false;
            m_pCurFeaCls = null;
            if (LayerID != "")
            {
                m_pCurFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath, LayerID);

            }
            if (DataSourceKey != "")
            {
                _OracleConn = GetOracleConn(Plugin.ModuleCommon.TmpWorkSpace, DataSourceKey);
            }
            if (m_pCurFeaCls != null)
            {
                txtBoxLayer.Text = advTreeLayers.SelectedNode.Text;
                InitListFields();
            }
        }
        //通过NODE 得到NODYKEY
        private string GetNodeKey(DevComponents.AdvTree.Node Node)
        {
            // labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.DataKey;
            XmlElement xmlelement = xmlnode as XmlElement;
            string strDataType = "";
            if (xmlelement.HasAttribute("DataType"))
            {
                strDataType = xmlnode.Attributes["DataType"].Value;
            }
            if (strDataType == "RD" || strDataType == "RC")//是影像数据 返回
            {
                // labelErr.Text = "请选择矢量数据进行操作!";
                return "";
            }
            if (xmlelement.HasAttribute("IsQuery"))
            {
                if (xmlelement["IsQuery"].Value == "False")
                {
                    // labelErr.Text = "该图层不可查询!";
                    return "";
                }
            }
            if (xmlelement.HasAttribute("NodeKey"))
            {
                return xmlelement.GetAttribute("NodeKey");

            }
            return "";

        }
        //通过NODE 得到NODYKEY
        private string GetDataSourceKey(DevComponents.AdvTree.Node Node)
        {
            // labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.DataKey;
            XmlElement xmlelement = xmlnode as XmlElement;
            string strDataType = "";
            if (xmlelement.HasAttribute("DataType"))
            {
                strDataType = xmlnode.Attributes["DataType"].Value;
            }
            if (strDataType == "RD" || strDataType == "RC")//是影像数据 返回
            {
                // labelErr.Text = "请选择矢量数据进行操作!";
                return "";
            }
            if (xmlelement.HasAttribute("IsQuery"))
            {
                if (xmlelement["IsQuery"].Value == "False")
                {
                    // labelErr.Text = "该图层不可查询!";
                    return "";
                }
            }
            if (xmlelement.HasAttribute("ConnectKey"))
            {
                return xmlelement.GetAttribute("ConnectKey");
            }
            return "";

        }
        private void advTreeLayers_Leave(object sender, EventArgs e)
        {
            HideLayerTree();
        }
        public void InitLayersTree()
        {
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath);
            if (File.Exists(_LayerTreePath))
            {

                XmlDocument LayerTreeXmldoc = new XmlDocument();

                LayerTreeXmldoc.Load(_LayerTreePath);
                advTreeLayers.Nodes.Clear();

                //获取Xml的根节点并作为根节点加到UltraTree上
                XmlNode xmlnodeRoot = LayerTreeXmldoc.DocumentElement;
                XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

                xmlelementRoot.SetAttribute("NodeKey", "Root");
                string sNodeText = xmlelementRoot.GetAttribute("NodeText");

                //创建并设定树的根节点
                DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
                treenodeRoot.Name = "Root";
                treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                this.advTreeLayers.Nodes.Add(treenodeRoot);

                treenodeRoot.Image = this.ImageList.Images["Root"];
                InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);
                LayerTreeXmldoc = null;
            }
        }
        //根据配置文件显示图层树
        private void InitLayerTreeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode)
        {

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
                XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
                if (xmlElementChild == null)
                {
                    continue;
                }
                else if (xmlElementChild.Name == "ConfigInfo")
                {
                    continue;
                }
                //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
                if (Plugin.ModuleCommon.ListUserdataPriID != null)
                {
                    if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(sNodeKey))
                    {
                        continue;
                    }
                }
                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;

                treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;


                treenode.Nodes.Add(treenodeChild);

                //递归
                if (xmlElementChild.Name != "Layer")
                {
                    InitLayerTreeByXmlNode(treenodeChild, xmlElementChild as XmlNode);
                }

                InitializeNodeImage(treenodeChild);
            }

        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        private void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image = this.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = this.ImageList.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = this.ImageList.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = this.ImageList.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = this.ImageList.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = this.ImageList.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = this.ImageList.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = this.ImageList.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = this.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = this.ImageList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = this.ImageList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = this.ImageList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = this.ImageList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = this.ImageList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = this.ImageList.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = this.ImageList.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = this.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = this.ImageList.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = this.ImageList.Images["RD"];
                    break;
                case "SubType":
                    treenode.Image = this.ImageList.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }
        private void HideLayerTree()
        {
            if (this.advTreeLayers.Visible)
            {
                this.advTreeLayers.Visible = false;
            }
        }
        #region 按钮事件
        private void FrmCustomizeStatistic_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void groupPanelAutoConfig_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void chartlet_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label12_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            HideLayerTree();
        }
        #endregion
        //ygc 20130407通过配置文件对临时表进行翻译
        private void TranslateTempTable(OracleConnection connect,string tableName, string FiledName,string FchinaName)
        {
            string dicTableName =ModXZQ.GetDicTableName(FiledName);
            if (dicTableName == "") return;
            OracleCommand pCommand = connect.CreateCommand();
            pCommand.CommandText = "update " + tableName + " set " + tableName + "." + FchinaName + "= (select " + dicTableName + ".name from " + dicTableName + " where " + dicTableName + ".code=" + tableName + "." + FchinaName + ")";
            pCommand.ExecuteNonQuery();
        }
    }
}
