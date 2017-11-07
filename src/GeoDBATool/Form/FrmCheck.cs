using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public partial class FrmCheck : DevComponents.DotNetBar.Office2007Form
    {
        public FrmCheck()
        {
            InitializeComponent();
        }
        public string FrmTile
        { get; set; }
        string TempShapeFilePath = Application.StartupPath + "\\..\\Template\\TemplateShapeFile.shp";
        string TempMDBPath = Application.StartupPath + "\\..\\Template\\TemplateMDBFile.mdb";
        string TempGDBPath = Application.StartupPath + "\\..\\Template\\TemplateGDBFile.gdb";
        private static string _LogFilePath = Application.StartupPath + "\\..\\Log\\CheckResult.txt";
        string TempFilePath = "";
        string TestFilePath = "";
        private void CheckShapeFile_Click(object sender, EventArgs e)
        {
            CheckGDB.Checked = false;
            CheckMDB.Checked = false;
            txtTemplate.Text = "";
            txtTemplate.Text = TempShapeFilePath;
            txtTest.Text = "";
        }

        private void CheckMDB_Click(object sender, EventArgs e)
        {
            CheckGDB.Checked = false;
            CheckShapeFile.Checked = false;
            txtTemplate.Text = "";
            txtTemplate.Text = TempMDBPath;
            txtTest.Text = "";
        }

        private void CheckGDB_Click(object sender, EventArgs e)
        {
            CheckMDB.Checked = false;
            CheckShapeFile.Checked = false;
            txtTemplate.Text = "";
            txtTemplate.Text = TempGDBPath;
            txtTest.Text = "";
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void FrmCheck_Load(object sender, EventArgs e)
        {
            txtTemplate.Text = TempShapeFilePath;
            this.Text = FrmTile;
        }

        private void btnScanTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            FolderBrowserDialog ScanGDB = new FolderBrowserDialog();
            openFile.Title = "打开模板文件";
            TempFilePath = "";
            if (CheckShapeFile.Checked == true)
            {
                openFile.Filter = "(*.shp)|*.shp";
            }
            else if (CheckGDB.Checked == true)
            {
                ScanGDB.Description = "选择GDB路径";
            }
            else if (CheckMDB.Checked == true)
            {
                openFile.Filter = "(*.mdb)|*.mdb";
            }
            else
            {
                openFile.Filter = "*.*|*.*";
            }
            if (CheckGDB.Checked != true)
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    txtTemplate.Text = "";
                    TempFilePath = openFile.FileName;
                    txtTemplate.Text = TempFilePath;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (ScanGDB.ShowDialog() == DialogResult.OK)
                {
                    txtTemplate.Text = "";
                    TempFilePath = ScanGDB.SelectedPath;
                    txtTemplate.Text = TempFilePath;
                }
            }
        }
        private void btnScanTestData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            FolderBrowserDialog ScanGDB = new FolderBrowserDialog();
            openFile.Title = "打开目标文件";
            TestFilePath = "";
            if (CheckShapeFile.Checked == true)
            {
                openFile.Filter = "(*.shp)|*.shp";
            }
            else if (CheckGDB.Checked == true)
            {
                ScanGDB.Description = "选择GDB路径";
            }
            else if (CheckMDB.Checked == true)
            {
                openFile.Filter = "(*.mdb)|*.mdb";
            }
            else
            {
                openFile.Filter = "*.*|*.*";
            }
            if (CheckGDB.Checked != true)
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    txtTest.Text = "";
                    TestFilePath = openFile.FileName;
                    txtTest.Text = TestFilePath;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if(ScanGDB.ShowDialog() ==DialogResult .OK)
                txtTest.Text = "";
                TempFilePath = ScanGDB.SelectedPath;
                txtTest.Text = TempFilePath;
            }
        }

        //根据文件路径打开文件 ygc 2012-8-29
        private IWorkspace GetWorkspace(string filePath)
        {
            IWorkspace pWorkspace = null;
            string FileType = filePath.Substring(filePath.Length - 4, 4);
            switch (FileType)
            {
                case ".shp":
                    IWorkspaceFactory pShpWorkSpaceFactory = new ShapefileWorkspaceFactory();
                  try
                  {
                      pWorkspace = pShpWorkSpaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
                  }
                  catch (Exception ex)
                  {
                      MessageBox .Show (ex.ToString (),"错误");
                      return null;
                  }
                  finally
                  {
                      System.Runtime.InteropServices.Marshal.ReleaseComObject(pShpWorkSpaceFactory);
                  }
                    break;
                case ".mdb":
                    IPropertySet pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("DATABASE", filePath);
                    IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                      return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspaceFactory);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pPropertySet);
                    }
                    break;
                case ".gdb":
                    IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pGDBWorkSpace.OpenFromFile(filePath, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pGDBWorkSpace);
                    }
                    break;
            }
            return pWorkspace;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtTemplate.Text == "")
            {
                MessageBox.Show("请选择模板文件","提示");
                return;
            }
            if (txtTest.Text == "")
            {
                MessageBox.Show("请选择检测数据","提示");
                return;
            }
            IWorkspace pTemplateWork = GetWorkspace(txtTemplate.Text);
            IWorkspace pTestWorkspace = GetWorkspace(txtTest.Text);
            if (pTemplateWork == null || pTestWorkspace == null)
            {
                return;
            }
            switch (FrmTile)
            {
                case "图层完整性":
                    CheckLayer(pTemplateWork, pTestWorkspace);
                    break;
                case "属性表结构":
                    CheckFields(pTemplateWork, pTestWorkspace);
                    break;
            }
            
        }
        //检测图层完整性 ygc 2012-8-30
        private void CheckLayer(IWorkspace TemplateWorkspace, IWorkspace TestWorkspace)
        {
            bool flag = true;
            List<string> pTempLayerList = ModDBOperator.GetFeatureClassListOfWorkspace(TemplateWorkspace);
            IEnumDataset pEnumDataset = TestWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            Dictionary<string, esriGeometryType> pTypeDic = GetFeatureClassType(TemplateWorkspace);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            List<string> pMoreList = new List<string>();
            IList<string> pErrorType = new List<string>();
            string strResult = "";
            while (pDataset != null)
            {

                if (pTempLayerList.Contains(pDataset.Name))
                {
                    IFeatureClass pTestFeatureClass = pDataset as IFeatureClass;
                    if (pTypeDic[pDataset.Name] != pTestFeatureClass.ShapeType)
                    {
                        pErrorType.Add(pDataset.Name);
                        flag = false;
                    }
                    pTempLayerList.Remove(pDataset.Name);
                    
                }
                else
                {
                    pMoreList.Add(pDataset.Name);
                }
                pDataset = pEnumDataset.Next();
            }

            pEnumDataset = TestWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumDataset.Reset();
            IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;

            while (pFeaDataset != null)
            {
                IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
                IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                while (pFeatureClass != null)
                {
                    IDataset pTmpdt = pFeatureClass as IDataset;
                    if (pTempLayerList.Contains(pTmpdt.Name))
                    {
                        if (pTypeDic[pTmpdt.Name] != pFeatureClass.ShapeType)
                        {
                            pErrorType.Add(pTmpdt.Name);
                            flag = false;
                        }
                        pTempLayerList.Remove(pTmpdt.Name);
                    }
                    else
                    {
                        pMoreList.Add(pTmpdt.Name);
                    }
                    pFeatureClass = pEnumFeatureClass.Next();
                }
                pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            #region  写错误信息
            if (pErrorType.Count > 0)
            {
                strResult += "以下图层数据类型不同：\r\n";
            }
            for (int i = 0; i < pErrorType.Count; i++)
            {
                strResult = strResult + "    " + pErrorType[i] + "\r\n";
            }
                if (pTempLayerList.Count > 0)
                {
                    strResult += "缺少以下图层:\r\n";
                }
            for (int i = 0; i < pTempLayerList.Count; i++)
            {
                strResult = strResult + "    "+pTempLayerList[i] + "\r\n";
            }
            if (pMoreList.Count > 0)
            {
                strResult = strResult + "多余以下图层:\r\n    ";
            }
            for (int i = 0; i < pMoreList.Count; i++)
            {
                strResult = strResult +"    "+ pMoreList[i] + "\r\n    ";
            }
            if (strResult == "")
            {
                MessageBox.Show("检查通过", "提示");
                return;
            }
            else
            {
               DialogResult result= MessageBox.Show("检查未通过，是否查看日志文件？","提示",MessageBoxButtons.OKCancel);
               WriteLog(strResult);
               if (result == DialogResult.OK)
               {
                   System.Diagnostics.Process.Start("notepad.exe", _LogFilePath);
               }
            }
            #endregion
        }
        //检测图层属性完整性 ygc 2012-8-30
        private void CheckFields(IWorkspace TemplateWorkspace, IWorkspace TestWorkspace)
        {
            bool flag = true;
            Dictionary<string, List<string>> pFieldDic = ModDBOperator.GetFieldsListOfWorkSpace(TemplateWorkspace);
            IEnumDataset pEnumDataset = TestWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            IEnumDataset pEnumTempDataset = TemplateWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            pEnumTempDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            IDataset pTempDataset=pEnumTempDataset .Next ();
            Dictionary<string, List<string>> pMoreDic = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> pLessDic = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> pDifDic = new Dictionary<string, List<string>>();
            string strResult = "";
            while (pTempDataset != null)
            {
                while (pDataset != null)
                {
                    if (pFieldDic.ContainsKey(pDataset.Name))
                    {
                        List<string> pTmpList = pFieldDic[pDataset.Name];
                        List<string> pMoreList = null;
                        List<string> pDifList = new List<string>();
                        IFeatureClass pFeatureClass = pDataset as IFeatureClass;
                        IFeatureClass pTempFeatureClass = pTempDataset as IFeatureClass;
                        IFields pFields = pFeatureClass.Fields;
                        IFields pTempFields = pTempFeatureClass.Fields;
                        for (int i = 0; i < pFields.FieldCount; i++)
                        {
                            string temp = CheckField(pFields.get_Field(i), pTempFields, pFeatureClass.AliasName);
                            if (temp != null && temp != "")
                            {
                                flag = false;
                                pDifList.Add(temp);
                            }
                        }
                        if (pDifList.Count > 0)
                        {
                            pDifDic.Add(pFeatureClass.AliasName, pDifList);
                        }
                        for (int f = 0; f < pFields.FieldCount; f++)
                        {
                            IField pField = pFields.get_Field(f);
                            if (pField.Type != esriFieldType.esriFieldTypeOID && pField.Type != esriFieldType.esriFieldTypeGeometry)
                            {
                                if (pTmpList.Contains(pField.Name))
                                {
                                    pTmpList.Remove(pField.Name);
                                }
                                else
                                {
                                    if (pMoreList == null)
                                    {
                                        pMoreList = new List<string>();
                                    }
                                    pMoreList.Add(pField.Name);
                                }
                            }
                        }
                        if (pTmpList.Count > 1)
                        {
                            pLessDic.Add(pDataset.Name, pTmpList);
                        }
                        if (pMoreList != null)
                        {
                            pMoreDic.Add(pDataset.Name, pMoreList);
                        }

                    }
                    pDataset = pEnumDataset.Next();
                }
                pTempDataset = pEnumTempDataset.Next();
            }

            pEnumDataset = TestWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumTempDataset = TemplateWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumDataset.Reset();
            pEnumTempDataset.Reset();
            IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            IFeatureDataset pTempFeaDataset = pEnumTempDataset.Next() as IFeatureDataset;

            while (pTempFeaDataset != null)
            {
                while (pFeaDataset != null)
                {
                    IFeatureClassContainer pFeatureClassContainer = pFeaDataset as IFeatureClassContainer;
                    IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                    IFeatureClass pEnFeatureClass = pEnumFeatureClass.Next();

                    IFeatureClassContainer pTemplateFeatureClassContainer = pTempFeaDataset as IFeatureClassContainer;
                    IEnumFeatureClass pTemplateFeatureClass = pTemplateFeatureClassContainer.Classes;
                    IFeatureClass pTempEnFeatureClass = pTemplateFeatureClass.Next();
                    while (pTempEnFeatureClass != null)
                    {
                        IDataset pTemplateDateset = pTempEnFeatureClass as IDataset;
                        IFeatureClass pTempFeatureClass = pTemplateDateset as IFeatureClass;
                        IFields pTemplateFields = pTempFeatureClass.Fields;
                        while (pEnFeatureClass != null)
                        {
                            IDataset pTmpDataset = pEnFeatureClass as IDataset;

                            if (pFieldDic.ContainsKey(pTmpDataset.Name))
                            {
                                List<string> pTmpList = pFieldDic[pTmpDataset.Name];
                                List<string> pMoreList = null;
                                List<string> pDifList = new List<string>() ;
                                IFeatureClass pFeatureClass = pTmpDataset as IFeatureClass;
                                IFields pFields = pFeatureClass.Fields;
                                for (int i = 0; i < pFields.FieldCount; i++)
                                {
                                    string temp = CheckField(pFields.get_Field(i), pTemplateFields, pFeatureClass.AliasName);
                                    if (temp !=null&&temp!="")
                                    {
                                        flag = false;
                                        pDifList.Add(temp);
                                    }
                                    
                                }
                                if (pDifList.Count > 0)
                                {
                                    pDifDic.Add(pFeatureClass.AliasName, pDifList);
                                }
                                    for (int f = 0; f < pFields.FieldCount; f++)
                                    {
                                        IField pField = pFields.get_Field(f);
                                        if (pField.Type != esriFieldType.esriFieldTypeOID && pField.Type != esriFieldType.esriFieldTypeGeometry)
                                        {
                                            if (pTmpList.Contains(pField.Name))
                                            {
                                                pTmpList.Remove(pField.Name);
                                            }
                                            else
                                            {
                                                if (pMoreList == null)
                                                {
                                                    pMoreList = new List<string>();
                                                }
                                                pMoreList.Add(pField.Name);
                                            }
                                        }
                                    }
                                if (pTmpList.Count > 1)
                                {
                                    pLessDic.Add(pTmpDataset.Name, pTmpList);
                                }
                                if (pMoreList != null)
                                {
                                    pMoreDic.Add(pTmpDataset.Name, pMoreList);
                                }

                            }
                            pEnFeatureClass = pEnumFeatureClass.Next();
                        }
                        pTempEnFeatureClass = pTemplateFeatureClass.Next();
                    }
                    pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
                }
                pTempFeaDataset = pEnumTempDataset.Next() as IFeatureDataset;
            }
            #region 写错误日志
            if (pDifDic.Count > 0)
            {
                strResult += "以下属性列不同：\r\n";
            }
            foreach (string strkey in pDifDic.Keys)
            {
                strResult += strkey + "图层：\r\n";
                List<string> pTempList = pDifDic[strkey];
                for (int i = 0; i < pTempList.Count; i++)
                {
                    strResult +=pTempList[i];
                }
                strResult += "\r\n";
                pTempList.Clear();
            }
            if (pLessDic.Count > 0)
            {
                strResult += "缺少以下属性列：\r\n";
                flag = false;
            }
            foreach (string strkey in pLessDic.Keys)
            {
                strResult = strResult + strkey + "图层：\r\n";
                List<string> pTmpList0 = pLessDic[strkey];
                for (int l = 0; l < pTmpList0.Count; l++)
                {
                    strResult = strResult + "    "+pTmpList0[l] + ",\r\n";
                }
                strResult += "\r\n";
                pTmpList0.Clear();

            }
            if (pMoreDic.Count > 0)
            {
                strResult = strResult + "多余以下属性列：\r\n";
                flag = false;
            }
            foreach (string strkey in pMoreDic.Keys)
            {
                strResult = strResult + strkey + "图层：\r\n";
                List<string> pTmpList0 = pMoreDic[strkey];
                for (int l = 0; l < pTmpList0.Count; l++)
                {
                    strResult = strResult +"    "+ pTmpList0[l] + ",\r\n";
                }
                strResult += "\r\n";
                pTmpList0.Clear();

            }
            pLessDic.Clear();
            pMoreDic.Clear();
            if (flag)
            {
                MessageBox.Show("通过检查", "提示");
            }
            else
            {
                DialogResult result = MessageBox.Show("检查未通过，是否查看日志文件？", "提示", MessageBoxButtons.OKCancel);
                WriteLog(strResult);
                if (result == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start("notepad.exe", _LogFilePath);
                }
            }
            #endregion
        }
        public static void WriteLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            System.IO.FileStream fs = new System.IO.FileStream(_LogFilePath, FileMode.Create, FileAccess.Write);
            fs.Close(); 

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string strread = strLog + "     " + strTime + "      \r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }

        private string CheckField(IField ptField, IFields fields,string FeatureClassName)
        {
            string ErrorMsg ="";
            int i = fields.FindField(ptField.Name);
            if (i < 0)
            {                //传递错误日志（字段缺失）
                 ErrorMsg += "    字段"+ptField .Name+"不存在   \r\n";
                 Application.DoEvents();
                 return null;
            }
            IField pField = fields.get_Field(i);

            #region 字段类型检查
            ///检查字段类型是否一致
            //string pfieldType = pAttr.Type.ToString(); 
            esriFieldType pfieldType = ptField.Type;
            switch (pfieldType)
            {
                case esriFieldType.esriFieldTypeString:
                    if (pField.Type != esriFieldType.esriFieldTypeString)
                    {
                        ErrorMsg +=  "    " + ptField.Name + "应为字符串型,\r\n";
                        Application.DoEvents();
                    }
                    break;
                case esriFieldType.esriFieldTypeInteger:
                    if (pField.Type != esriFieldType.esriFieldTypeInteger)
                    {
                        ErrorMsg +=  "    " + ptField.Name + "应为长整型,\r\n";
                        Application.DoEvents();
                    }
                    break;
                //case "GO_VALUETYPE_BOOL":
                //    break;
                case esriFieldType.esriFieldTypeDate:
                    if (pField.Type != esriFieldType.esriFieldTypeDate)
                    {
                         ErrorMsg +=  "    " + ptField.Name + "应为日期型,\r\n";
                        Application.DoEvents();
                    }
                    break;
                case esriFieldType.esriFieldTypeSingle:
                    if (pField.Type != esriFieldType.esriFieldTypeSingle)
                    {  
                         ErrorMsg += "    " + ptField.Name + "应为单精度型,\r\n";
                        Application.DoEvents();
                    }
                    break;
                case esriFieldType.esriFieldTypeDouble:
                    if (pField.Type != esriFieldType.esriFieldTypeDouble)
                    {
                        //传递错误日志:字段应为双精度类型
                         ErrorMsg +=  "    " + ptField.Name + "应为双精度型,\r\n";
                        Application.DoEvents();
                    }
                    break;

                default:
                    break;
            }
            #endregion

            #region 字段长度检查

            if (pfieldType == esriFieldType.esriFieldTypeString)
            {
                if (pField.Length != ptField.Length)
                {
                     ErrorMsg += "    " + ptField.Name + "的长度应为：" + pField.Length+",\r\n";
                    Application.DoEvents();
                }
            }
            #endregion
            #region 字段可否为空检查

            if (pField.IsNullable != ptField.IsNullable)
            {
                //传递错误日志

                 ErrorMsg += "    " + ptField.Name + "可以为空,\r\n" ;
                Application.DoEvents();
            }
            #endregion
            #region 字段是否必须存在检查

            //if (pField.Required != bool.Parse(pAttrDes.Necessary.ToString()))
            //{
            //    //传递错误日志：
            //    ProcErrorList(this.m_DBDataset, pFeatureClassName, null, "", -1, this.m_DataGrideView, EnumCheckType.数据标准化, enumErrorType.属性字段类型不一致, "目标要素类的 " + pField.Name + " 字段是否必填的属性应为：" + pAttrDes.Necessary.ToString());
            //    ModData.m_ErrCount++;
            //    //错误提示
            //    m_ErrCount++;
            //    m_LabelRes.Text = "数据标准化：" + m_ErrCount + "个";
            //    Application.DoEvents();
            //}
            #endregion

            #region 字段是否可编辑检查
            #endregion

            #region 字段值域是否可变检查
            #endregion
                return ErrorMsg;
        }
        private Dictionary<string, esriGeometryType> GetFeatureClassType(IWorkspace pWorkspace)
        {
            Dictionary<string, esriGeometryType> dicFeatureClassType = new Dictionary<string, esriGeometryType>();
            if (pWorkspace == null)
            {
                return null;
            }
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                IFeatureClass pFeatureClass = pDataset as IFeatureClass;
                dicFeatureClassType.Add(pDataset.Name, pFeatureClass .ShapeType);
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
                    IDataset pTmpDataset = pFeatureClass as IDataset;
                    dicFeatureClassType.Add(pTmpDataset.Name, pFeatureClass.ShapeType);
                    pFeatureClass = pEnumFeatureClass.Next();
                }
                pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            return dicFeatureClassType;
        }
    }
}
