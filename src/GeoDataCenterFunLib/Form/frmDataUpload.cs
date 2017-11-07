using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using SysCommon;
using SysCommon.Gis;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;

//*********************************************************************************
//** 文件名：frmDataUpload.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-07
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************

namespace GeoDataCenterFunLib
{
    public partial class frmDataUpload : DevComponents.DotNetBar.Office2007Form
    {
        public frmDataUpload()
        {
            InitializeComponent();
            
        }

        private void frmDataUpload_Load(object sender, EventArgs e)
        {
           
            string strExp = "select 数据源名称 from 物理数据源表";
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int ii = 0; ii < list.Count; ii++)
            {
                comboBoxSource.Items.Add(list[ii]);//加载数据源列表框
            }
            if (list.Count > 0)
            {
                comboBoxSource.SelectedIndex = 0;//默认选择第一个
            }
            checkBoxNew.Checked = true;//覆盖源文件的checkbox默认为真
            checkboxLegal.Checked = true;//检查文件名合法性
        }

        GetDataTreeInitIndex m_dIndex = new GetDataTreeInitIndex();//取得路径的类
        frmDataReduction fdr=new frmDataReduction();//删除时调用数据整理中删除表的方法
        //SysGisDataSet ds = new SysGisDataSet();
        OpenFileDialog OpenFile;
        int i = 0;
        bool m_success=false;
        bool m_newfile;
        string[] array = new string[6];//分析数据数组
        string m_strErr;//错误信息提醒

        //SHP数据入库到GDB数据库的方法
        private void ImportFeatureClassToNewWorkSpace(string filename,string outfilename)
        {
            //try
            //{   
                m_success = false;//初始化
                string ImportShapeFileName = filename;
                  string ExportFileShortName = outfilename;
                if (ImportShapeFileName == "") { return; }
                string ImportFileShortName = System.IO.Path.GetFileNameWithoutExtension(ImportShapeFileName);
                string ImportFilePath = System.IO.Path.GetDirectoryName(ImportShapeFileName);
                
                //打开存在的工作空间，作为导入的空间
                IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                //IWorkspace pWorkspace = Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0);
               // IWorkspace2 pWorkspace2 =(IWorkspace2)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                IWorkspace pWorkspace=GetWorkspace(comboBoxSource.Text);
                if (pWorkspace == null)
                {
                    m_strErr = "数据源未找到";
                    m_success = false;
                    return;
                }
                string username = GetSourceUser(comboBoxSource.Text);
                if (username.Trim() != "")
                    ExportFileShortName = username + "." + ExportFileShortName;
                IWorkspace2 pWorkspace2 = pWorkspace as IWorkspace2;
                //判断要素是否存在，若存在将删除源文件
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass,ExportFileShortName))
                {
                    if (m_newfile == true)
                    {
                        IFeatureClass tmpfeatureclass;
                        IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                        tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(ExportFileShortName);
                        IDataset set = tmpfeatureclass as IDataset;
                        fdr.DeleteSql(ExportFileShortName);
                        set.CanDelete();
                        set.Delete();
                    }
                    else
                    {
                        //MessageBox.Show("存在相同文件名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_strErr = "存在相同文件名";
                        m_success = false;
                        return;
                    }

                }  
                IWorkspaceName pInWorkspaceName;
                IFeatureDatasetName pOutFeatureDSName;
                IFeatureClassName pInFeatureClassName;
                IDatasetName pInDatasetName;
                IFeatureClassName pOutFeatureClassName;
                IDatasetName pOutDatasetName;
                long iCounter;
                IFields pOutFields, pInFields;
                IFieldChecker pFieldChecker;
                IField pGeoField;
                IGeometryDef pOutGeometryDef;
                IGeometryDefEdit pOutGeometryDefEdit;
                IName pName;
                IFeatureClass pInFeatureClass;
                IFeatureDataConverter pShpToClsConverter;
                IEnumFieldError pEnumFieldError = null;

                //得到一个输入SHP文件的工作空间，
                pInWorkspaceName = new WorkspaceNameClass();
                pInWorkspaceName.PathName = ImportFilePath;
                pInWorkspaceName.WorkspaceFactoryProgID = "esriCore.ShapefileWorkspaceFactory.1";

                //创建一个新的要素类名称，目的是为了以来PNAME接口的OPEN方法打开SHP文件
                pInFeatureClassName = new FeatureClassNameClass();
                pInDatasetName = (IDatasetName)pInFeatureClassName;
                pInDatasetName.Name = ImportFileShortName;
                pInDatasetName.WorkspaceName = pInWorkspaceName;

                //打开一个SHP文件，将要读取它的字段集合
                pName = (IName)pInFeatureClassName;
                pInFeatureClass = (IFeatureClass)pName.Open();

                //通过FIELDCHECKER检查字段的合法性，为输入要素类获得字段集合
                pInFields = pInFeatureClass.Fields;
                pFieldChecker = new FieldChecker();
                pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);

                //通过循环查找几何字段
                pGeoField = null;
                for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
                {
                    if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pGeoField = pOutFields.get_Field((int)iCounter);
                        break;
                    }
                }

                //得到几何字段的几何定义
                pOutGeometryDef = pGeoField.GeometryDef;

                //设置几何字段的空间参考和网格
                pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
                pOutGeometryDefEdit.GridCount_2 = 1;
                pOutGeometryDefEdit.set_GridSize(0, 1500000);

                //创建一个新的要素类名称作为可用的参数
                pOutFeatureClassName = new FeatureClassNameClass();
                pOutDatasetName = (IDatasetName)pOutFeatureClassName;
                pOutDatasetName.Name = ExportFileShortName;

                //创建一个新的数据集名称作为可用的参数
                pOutFeatureDSName = (IFeatureDatasetName)new FeatureDatasetName();

                //如果参数的值是NULL，说明要创建独立要素类
                //创建一个不存在的要素集合pFDN，通过它将IFeatureClassName和工作空间连接起来，而ConvertFeatureClass函数并不使用该变量作为参数，
                IFeatureDatasetName pFDN = new FeatureDatasetNameClass();
                IDatasetName pDN = (IDatasetName)pFDN;
                IDataset pDS = (IDataset)pWorkspace;
                pDN.WorkspaceName = (IWorkspaceName)pDS.FullName;
                pOutFeatureClassName.FeatureDatasetName = (IDatasetName)pFDN;

                //将pOutFeatureDSName设置为Null，将它做为参数给ConvertFeatureClass函数，因为IFeatureClassName本身已经和工作空间关联了，生成的
                //要素类在工作空间的根目录下，即独立要素类
                pOutFeatureDSName = null;

                //开始导入
                if (InsertIntoDatabase(ExportFileShortName))
                {
                    pShpToClsConverter = new FeatureDataConverterClass();
                    pShpToClsConverter.ConvertFeatureClass(pInFeatureClassName, null, pOutFeatureDSName, pOutFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
                    //MessageBox.Show("导入成功", "提示");
                    m_success = true;
                }
                  
            //}
            //catch
            //{
            //    m_success = false;
            //}
        }

       
        //获得mdb中要素类
        public List<string> Getfeatureclass(IWorkspace pWorkspaceMDB)
        {
            List<string> list = new List<string>();
            IEnumDataset enumDataset = pWorkspaceMDB.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
            IDataset dataset = enumDataset.Next();
            while (dataset != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    //IFeatureClass pFeatureClass = dataset as IFeatureClass;
                    //IDataset pDataset = pFeatureClass as IDataset;;
                    list.Add(dataset.Name); 
                    dataset = enumDataset.Next();
                }
            }
            enumDataset = pWorkspaceMDB.get_Datasets(esriDatasetType.esriDTFeatureDataset) as IEnumDataset;
            dataset = enumDataset.Next();
            while (dataset != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    IEnumDataset pEnumDataset = dataset.Subsets;
                    IDataset dataset2 = pEnumDataset.Next();
                    while (dataset2 != null)
                    {
                        list.Add(dataset2.Name);
                        dataset2 = pEnumDataset.Next();
                    }
            
                }
                dataset = enumDataset.Next();
            }
            return list;
        }
        //将mdb中要素类转换复制到GDB数据库中
        private void ImportMDBToGDB(string file,string outfilename)
        {

            m_success = false;//初始化
            try
            {
                string filepath = file.Substring(0, file.LastIndexOf("---"));
                string filename = file.Substring(file.LastIndexOf("-") + 1);

                //打开mdb文件所在的工作空间
                ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new AccessWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspaceMDB = wf.OpenFromFile(@filepath, 0) as IFeatureWorkspace;
                IWorkspace pWorkspaceMDB = pFeatureWorkspaceMDB as IWorkspace;
                // 创建源工作空间名称     
                IDataset sourceWorkspaceDataset = (IDataset)pWorkspaceMDB;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;

                //创建源数据集名称        
                //IFeatureClassName sourceFeatureClassName = serverContext.CreateObject("esriGeoDatabase.FeatureClassName") as IFeatureClassName;
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = filename;

                //打开存在的工作空间，作为导入的空间
                IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                //IWorkspace pWorkspaceGDB = Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0);
                //IWorkspace2 pWorkspace2 = (IWorkspace2)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                IWorkspace pWorkspaceGDB =GetWorkspace(comboBoxSource.Text);
                if (pWorkspaceGDB == null)
                {
                    m_strErr = "数据源未找到";
                    m_success = false;
                    return;
                }
                string username = GetSourceUser(comboBoxSource.Text);
                if (username.Trim() != "")
                    outfilename = username + "." + outfilename;
                IWorkspace2 pWorkspace2 = pWorkspaceGDB as IWorkspace2;
                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceGDB;

                //创建目标工作空间名称    
                IDataset targetWorkspaceDataset = (IDataset)pWorkspaceGDB;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;

                //创建目标数据集名称    
                // IFeatureClassName targetFeatureClassName = serverContext.CreateObject("esriGeoDatabase.FeatureClassName") as IFeatureClassName;
                //判断要素是否存在，若存在将删除源文件
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, outfilename))
                {
                    if (m_newfile == true)
                    {
                        IFeatureClass tmpfeatureclass;
                        tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(outfilename);
                        IDataset set = tmpfeatureclass as IDataset;
                        fdr.DeleteSql(filename);
                        set.CanDelete();
                        set.Delete();
                    }
                    else
                    {
                        m_strErr = "存在相同文件名";
                        m_success = false;
                        return;
                    }
                }
                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = outfilename;

                //打开输入的要素类以得到字段定义      
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();//打开源要素类 

                //验证字段名称，因为你正在不同类型的工作空间之间进行数据转换
                //IFieldChecker fieldChecker = serverContext.CreateObject("esriGeoDatabase.FieldChecker") as IFieldChecker;
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
                IFields targetFeatureClassFields;
                IEnumFieldError enumFieldError;

                //最重要的设置输入和验证工作空间
                fieldChecker.InputWorkspace = pWorkspaceMDB;
                fieldChecker.ValidateWorkspace = pWorkspaceGDB;
                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);

                //遍历所有输出字段找到几何字段
                IField geometryField;
                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
                {
                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        geometryField = targetFeatureClassFields.get_Field(i);
                        //得到几何字段的几何定义                
                        IGeometryDef geometryDef = geometryField.GeometryDef;
                        //赋予几何定义一个空间索引格网数目和格网大小值
                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                        targetFCGeoDefEdit.GridCount_2 = 1;
                        targetFCGeoDefEdit.set_GridSize(0, 0);
                        //允许ArcGIS为数据加载确定一个有效的格网大小
                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                        //转换要素类中所有的要素
                        //IQueryFilter queryFilter = serverContext.CreateObject("esriGeoDatabase.QueryFilter") as IQueryFilter; ;
                        QueryFilter queryFilter = new QueryFilterClass();
                        queryFilter.WhereClause = "";
                        //加载要素类               
                        //IFeatureDataConverter fctofc = serverContext.CreateObject("esriGeoDatabase.FeatureDataConverter") as IFeatureDataConverter;
                        if (InsertIntoDatabase(outfilename))//更新数据库
                        {
                            IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                            IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
                        }
                    }

                }
                m_success = true;
            }
            catch { m_success = false; m_strErr = ""; }
        }

          
        private void checkboxPf_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxPf.Checked == true)
            {
                textBox.Enabled = true;
                btnSelect.Enabled = true;
            }
            else
            {
                textBox.Text = "";
                textBox.Enabled = false;
                btnSelect.Enabled = false;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            string name;string strName="";
            foreach (ListViewItem item in listView.Items)
            {
                string[] str = item.Text.Split('.');
                str[1] = str[1].Substring(0, 3);
                if (str[1].ToLower() == "shp")
                {
                    name = System.IO.Path.GetFileNameWithoutExtension(item.Text);
                    item.SubItems[1].Text = textBox.Text + name.ToUpper();
                }
                else if (str[1].ToLower() == "mdb")
                {
                    name = item.Text.Substring(item.Text.LastIndexOf("-") + 1);
                    if (textBox.Text.Trim() != "")
                        strName = GetForwadName(textBox.Text.ToUpper(), name);
                    else
                        strName = name;
                    item.SubItems[1].Text = strName.ToUpper();
                }
                
                
            }
        }
        /// <summary>
        /// 将数据分析成字符串数组
        /// </summary>
        /// <param name="filename">数据名称</param>
        public void AnalyseDataToArray(string filename)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 字段名称 from 图层命名规则表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {
                switch (arrName[i])
                {
                    case "业务大类代码":
                        array[0] = filename.Substring(0, 2);//业务大类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "年度":
                        array[1] = filename.Substring(0, 4);//年度
                        filename = filename.Remove(0, 4);
                        break;
                    case "业务小类代码":
                        array[2] = filename.Substring(0, 2);//业务小类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "行政代码":
                        array[3] = filename.Substring(0, 6);//行政代码
                        filename = filename.Remove(0, 6);
                        break;
                    case "比例尺":
                        array[4] = filename.Substring(0, 1);//比例尺
                        filename = filename.Remove(0, 1);
                        break;
                }
            }
        }

        public string GetDescrib(string str)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 描述 from 标准图层信息表 where 代码='"+str+"'";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strreturn=db.GetInfoFromMdbByExp(strCon, strExp);
            return strreturn;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(@GetSourcePath(comboBoxSource.Text)))
            //{
            //    MessageBox.Show("数据源路径不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            OpenFile = new OpenFileDialog();
            OpenFile.Filter = "SHP数据|*.shp|MDB数据|*.mdb;";
            OpenFile.Multiselect = true;  
             
            //打开SHP文件
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (string file in OpenFile.FileNames)
                {
                    for (int j = 0; j < i; j++)
                    {
                        string strExist = listView.Items[j].Text.Trim();
                        if (strExist.Contains("---"))
                        {
                            strExist = strExist.Substring(0, strExist.LastIndexOf("---"));
                        }
                        if (strExist.CompareTo(file) == 0)
                        {
                            MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                    }
                    //string name = file.Substring(file.LastIndexOf('\\') + 1, file.Length - file.LastIndexOf('\\') - 1);
                    string[] str = file.Split('.');
                    if (str[1].ToLower() == "shp")
                    {
                        listView.Items.Add(file);
                        string name1 = System.IO.Path.GetFileNameWithoutExtension(file).ToUpper();
                        if (textBox.Text != "")
                        {
                            string strName = GetForwadName(textBox.Text.ToUpper(), name1);
                            listView.Items[i].SubItems.Add(strName);
                        }
                        else
                            listView.Items[i].SubItems.Add(name1);
                        if (name1.Length > 15)
                            name1=name1.Substring(15);
                        string strdescri = GetDescrib(name1.ToUpper());
                        if (strdescri.Trim() != "")
                        {
                            listView.Items[i].SubItems.Add(strdescri);
                        }
                        else
                        {
                            listView.Items[i].SubItems.Add("需要新增");
                        }
                        listView.Items[i].SubItems.Add("等待入库");
                         listView.Items[i].Checked = true;
                        i++;
                    }
                    else if (str[1].ToLower() == "mdb")
                    {
                        ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new AccessWorkspaceFactory();
                        IFeatureWorkspace pFeatureWorkspaceMDB = wf.OpenFromFile(@file, 0) as IFeatureWorkspace;
                        IWorkspace pWorkspaceMDB = pFeatureWorkspaceMDB as IWorkspace;
                        List<string> list = Getfeatureclass(pWorkspaceMDB);
                        for (int ii = 0; ii < list.Count; ii++)
                        {
                            listView.Items.Add(file+"---"+list[ii]);
                            if (textBox.Text != "")
                            {
                                string strName = GetForwadName(textBox.Text.ToUpper(), list[ii].ToUpper());
                                listView.Items[i].SubItems.Add(strName);

                            }
                            else
                                listView.Items[i].SubItems.Add(list[ii].ToUpper());
                            if (list[ii].Length> 15)
                                list[ii]=list[ii].Substring(15);
                            string strdescri = GetDescrib(list[ii].ToUpper());
                            if (strdescri.Trim() != "")
                                listView.Items[i].SubItems.Add(strdescri);
                            else
                                listView.Items[i].SubItems.Add("需要新增");    
                            listView.Items[i].SubItems.Add("等待入库");
                            listView.Items[i].Checked = true;
                            i++;
                        }
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 将前缀按照标准表规则化，并组织
        /// </summary>
        /// <param name="str1">前缀</param>
        /// <param name="str2">图层</param>
        /// <returns></returns>
        public string GetForwadName(string str1, string str2)
        {
            AnalyseDataToArray(str1);
            str1 = "";
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 业务大类代码,业务小类代码 from 标准图层信息表 where 代码='" + str2 + "'";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
            if (dt.Rows.Count > 0)
            {
                array[0] = dt.Rows[0]["业务大类代码"].ToString();
                array[2] = dt.Rows[0]["业务小类代码"].ToString();
            }
            strExp = "select 字段名称 from 图层命名规则表";
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {

                switch (arrName[i])
                {
                    case "业务大类代码":
                        str1 += array[0];
                        break;
                    case "年度":
                        str1 += array[1];
                        break;
                    case "业务小类代码":
                        str1 += array[2];
                        break;
                    case "行政代码":
                        str1 += array[3];
                        break;
                    case "比例尺":
                        str1 += array[4];
                        break;
                }
            }
            str1 += str2;
            return str1;

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    listView.Items.Remove(item);
                    i--;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            i = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            bool _check=false;
            foreach (ListViewItem item in listView.Items)
            {
                m_strErr = "";
                string []str=item.Text.Split('.');
                str[1] = str[1].Substring(0, 3);
                if (item.Checked&&item.SubItems[3].Text=="等待入库")
                { 
                    _check = true;
                    if (checkboxLegal.Checked)
                    {
                        if (item.SubItems[1].Text.Trim()!=""&&!CheckNames(item.SubItems[1].Text))
                        {
                            item.SubItems[3].Text = "命名不规则,入库失败";
                            continue;
                        }
                    }
                   
                    item.SubItems[3].Text = "正在入库";
                    listView.Refresh();
                    if (str[1].ToLower() == "shp")
                        ImportFeatureClassToNewWorkSpace(item.Text, item.SubItems[1].Text);
                    else if (str[1].ToLower() == "mdb")
                        ImportMDBToGDB(item.Text, item.SubItems[1].Text);

                    if (m_success)
                        item.SubItems[3].Text = "入库完成";
                    else
                    {
                        if(m_strErr!="")
                            item.SubItems[3].Text = m_strErr + ",入库失败";
                        else
                        item.SubItems[3].Text = "入库失败";
                    }
                    listView.Refresh();
                }
            }
            if (_check ==false)
                MessageBox.Show("请选择要入库的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("操作已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBoxNew_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNew.Checked == true)
                m_newfile = true;
            else
                m_newfile = false;
        }
        /// <summary>
        /// 检查名称合法性
        /// </summary>
        /// <param name="name">需要检查的名称</param>
        /// <returns></returns>
        public bool CheckNames(string name)
        {
            if (name.Length < 15 && name.Length > 20)
                return false;
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strExp = "select 命名规则 from 图层命名规则表";
                string strRegex = db.GetInfoFromMdbByExp(strCon, strExp);//正则表达式
                strExp = "select 字段名称 from 图层命名规则表";
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                string[] arrRegex = strRegex.Split('(', ')');//分离正则表达式
                string[] arrName = strname.Split('+');//分离字段名称
                Regex regex;
                for (int i = 0; i < arrName.Length; i++)
                {
                    regex = new Regex(arrRegex[2 * i + 1]);
                    switch (arrName[i])
                    {
                        case "业务大类代码":
                            if (!regex.IsMatch(name.Substring(0, 2)))//匹配业务大类代码
                            {
                                return false;
                            }
                            else name = name.Remove(0, 2);
                            break;
                        case "年度":
                            if (!regex.IsMatch(name.Substring(0, 4)))//匹配年度
                            {
                                return false;
                            }
                            else name = name.Remove(0, 4);
                            break;
                        case "业务小类代码":
                            if (!regex.IsMatch(name.Substring(0, 2)))//匹配业务小类代码
                            {
                                return false;
                            }
                            else name = name.Remove(0, 2);
                            break;
                        case "行政代码":
                            if (!regex.IsMatch(name.Substring(0, 6)))//匹配行政代码
                            {
                                return false;
                            }
                            else name = name.Remove(0, 6);
                            break;
                        case "比例尺":
                            if (!regex.IsMatch(name.Substring(0, 1)))//匹配行比例尺
                            {
                                return false;
                            }
                            else name = name.Remove(0, 1);
                            break;
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        private void btnServer_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (!FolderBrowser.SelectedPath.Contains(".gdb"))
                {
                    MessageBox.Show("请选择File Geodatabase数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                comboBoxSource.Text = FolderBrowser.SelectedPath;
            }
        }

        //写入数据编码表和地图入库信息表的方法
        public bool InsertIntoDatabase(string filename)
        {
            bool success;
            try
            {
                if (filename.Contains("."))
                    filename = filename.Substring(filename.LastIndexOf(".") + 1);//针对SDE
                if (filename.Length > 16)
                {
                    GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                    string mypath = dIndex.GetDbInfo();
                    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                     string strExp = "select 字段名称 from 图层命名规则表";
                    GeoDataCenterDbFun db=new GeoDataCenterDbFun();
                    string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                     string[] arrName = strname.Split('+');//分离字段名称
                     for (int i = 0; i < arrName.Length; i++)
                     {
                         switch (arrName[i])
                         {
                             case "业务大类代码":
                                 array[0] = filename.Substring(0, 2);//业务大类代码
                                 filename = filename.Remove(0, 2);
                                 break;
                             case "年度":
                                 array[1] = filename.Substring(0, 4);//年度
                                 filename = filename.Remove(0, 4);
                                 break;
                             case "业务小类代码":
                                 array[2] = filename.Substring(0, 2);//专题
                                 filename = filename.Remove(0, 2);
                                 break;
                             case "行政代码":
                                 array[3]= filename.Substring(0, 6);//行政代码
                                 filename = filename.Remove(0, 6);
                                 break;
                             case "比例尺":
                                 array[4]= filename.Substring(0, 1);//比例尺
                                 filename = filename.Remove(0, 1);
                                 break;
                         }
                     }
                    array[5] = filename;//图层组成
                    string sourcename = comboBoxSource.Text.Trim();
                    strExp = string.Format("select count(*) from 数据编码表 where 业务大类代码='{0}' and 年度='{1}' and 业务小类代码='{2}'and 行政代码='{3}' and 比例尺='{4}' and 图层代码='{5}' and 数据源名称='{6}'",
                    array[0], array[1], array[2], array[3], array[4], array[5],sourcename);
                    GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
                    int count = dDbFun.GetCountFromMdb(strCon, strExp);
                    if (count!= 1)
                    {
                        strExp = string.Format("insert into 数据编码表(业务大类代码,年度,业务小类代码,行政代码,比例尺,图层代码,数据源名称) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                            array[0], array[1], array[2], array[3], array[4], array[5],sourcename);
                        dDbFun.ExcuteSqlFromMdb(strCon, strExp); //更新数据编码表
                        dDbFun.UpdateMdbInfoTable(array[0], array[1], array[2], array[3], array[4]);//更新地图入库信息表
                    }
                    success = true;
                }
                else
                {
                    m_strErr = "命名不规则，写入数据表失败";
                    success = false;
                }
            }
            catch(System.Exception e)
            {
                MessageBox.Show(e.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                success = false;
            }
            return success;
        }
        //得到数据源地址
        private string GetSourceUser(string str)
        {
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 用户 from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                return strname.ToUpper();
            }
            catch { return ""; }
        }
       /// <summary>
        /// 得到数据库空间 Added by xisheng 2011.04.28
       /// </summary>
       /// <param name="str">数据源名称</param>
       /// <returns>工作空间</returns>
        private IWorkspace GetWorkspace(string str)
        {
            try
            {
                IWorkspace pws = null;
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select * from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                string type = dt.Rows[0]["数据源类型"].ToString();
               if (type.Trim() == "GDB")
               {
                   IWorkspaceFactory pWorkspaceFactory;
                   pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                   pws = pWorkspaceFactory.OpenFromFile(dt.Rows[0]["数据库"].ToString(), 0);
               }
               else if (type.Trim() == "SDE")
               {
                   IWorkspaceFactory pWorkspaceFactory;
                   pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                   //PropertySet
                   IPropertySet pPropertySet;
                   pPropertySet = new PropertySetClass();
                   pPropertySet.SetProperty("Server", dt.Rows[0]["服务器"].ToString());
                   pPropertySet.SetProperty("Database", dt.Rows[0]["数据库"].ToString());
                   pPropertySet.SetProperty("Instance","5151");//"port:" + txtService.Text
                   pPropertySet.SetProperty("user", dt.Rows[0]["用户"].ToString());
                   pPropertySet.SetProperty("password", dt.Rows[0]["密码"].ToString());
                   pPropertySet.SetProperty("version", "sde.DEFAULT");
                   pws = pWorkspaceFactory.Open(pPropertySet, 0);
                  
               }
               return pws;
            }
            catch
            {
                return null; 
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            frmNameRule frm = new frmNameRule(textBox);
            frm.ShowDialog();

        }

        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <listView.Items.Count; i++)
            {
                listView.Items[i].Checked = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[i].Checked == false)
                {
                    listView.Items[i].Checked = true;
                    //datagwSource.Rows[i].Selected = true;
                }
                else
                {
                    listView.Items[i].Checked = false;
                    //datagwSource.Rows[i].Selected = false;
                }
            }
        }

        //取消命名不规则
        private void checkboxLegal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxLegal.Checked == false)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.SubItems[3].Text.Trim() == "命名不规则,入库失败")
                    {
                        item.SubItems[3].Text = "等待入库";
                    }
                }

            }

        }

        //新增图层
        private void btnNewLayer_Click(object sender, EventArgs e)
        {
            List<string> Laylist = new List<string>();
            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[2].Text.Trim() != "需要新增")
                    continue;
                string strName=item.SubItems[1].Text;
               if (strName.Length > 15)
                  strName= strName.Substring(15);
              Laylist.Add(strName);
            }
            frmNewLayer frm = new frmNewLayer(Laylist,this.listView);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    string strName = item.SubItems[1].Text;
                    if (strName.Length > 15)
                    {
                        string strforward = strName.Substring(0,15);
                        strName = strName.Substring(15);
                        item.SubItems[1].Text = GetForwadName(strforward, strName);
                    }
                    
                    string strdescri = GetDescrib(strName); 
                    if (strdescri.Trim() != "")
                    {
                        item.SubItems[2].Text = strdescri;
                    }
                    else
                    {
                        item.SubItems[2].Text="需要新增";
                    }
                }
            }
            listView.Refresh();
        }
      
    }
}