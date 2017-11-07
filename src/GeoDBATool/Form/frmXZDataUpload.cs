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
using GeoDataCenterFunLib;


namespace GeoDBATool
{
    public partial class frmXZDataUpload : DevComponents.DotNetBar.Office2007Form
    {
        public frmXZDataUpload(IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_TempWorkspace = pWorkspace;
        }
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        //SysGisDataSet ds = new SysGisDataSet();
        OpenFileDialog OpenFile;
        int i = 0;
        bool m_success=false;
        List<string> filename = new List<string>();//批量入库取数据名
        string[] array = new string[6];//分析数据数组
        string m_strErr;//错误信息提醒
        private IWorkspace pTargetworkspace;
        private IWorkspace m_TempWorkspace=null;

     
        /// <summary>
        /// 将GDB的数据入到SDE中 xisheng 20110919
        /// </summary>
        private void ImportGDBToSDE(string file, string outfilename, out int featurecount)
        {
            m_success = false;//初始化
            try
            {
                string filepath = file.Substring(0, file.LastIndexOf("---"));
                string filename = file.Substring(file.LastIndexOf("-") + 1);

                //打开mdb文件所在的工作空间
                ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new FileGDBWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspaceGDB = wf.OpenFromFile(@filepath, 0) as IFeatureWorkspace;
                IWorkspace pWorkspaceGDB = pFeatureWorkspaceGDB as IWorkspace;
                // 创建源工作空间名称     
                IDataset sourceWorkspaceDataset = (IDataset)pFeatureWorkspaceGDB;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;

                //创建源数据集名称        
                //IFeatureClassName sourceFeatureClassName = serverContext.CreateObject("esriGeoDatabase.FeatureClassName") as IFeatureClassName;
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = filename;

                //打开存在的工作空间，作为导入的空间;
                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pTargetworkspace;

                //创建目标工作空间名称    
                IDataset targetWorkspaceDataset = (IDataset)pTargetworkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                IWorkspace2 pWorkspace2 = pTargetworkspace as IWorkspace2;
                IFeatureDataset tmpfeaturedataset;
                //创建目标数据集名称    
                // IFeatureClassName targetFeatureClassName = serverContext.CreateObject("esriGeoDatabase.FeatureClassName") as IFeatureClassName;
                //判断要素是否存在，若存在将删除源文件
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureDataset, textBox.Text))
                {
                    tmpfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(textBox.Text);
                    if (text_prj.Text != "")
                    {
                        IGeoDatasetSchemaEdit pgeodataset = tmpfeaturedataset as IGeoDatasetSchemaEdit;
                        if (pgeodataset.CanAlterSpatialReference)
                            pgeodataset.AlterSpatialReference(GetSpatialReferenceformFile(text_prj.Text));
                    }
                }
                else
                {
                    tmpfeaturedataset = CreateFeatureDataset(pFeatureWorkspace, textBox.Text, text_prj.Text);
                     
                }
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass,outfilename))
                {

                    IFeatureClass tmpfeatureclass;
                    tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(outfilename);
                    IDataset tempset = tmpfeatureclass as IDataset;
                    tempset.Delete();
                }

                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = outfilename;
                //目标数据集
                IFeatureDatasetName outfeaturedatasetname = tmpfeaturedataset.FullName as IFeatureDatasetName;
                

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
                fieldChecker.InputWorkspace = pWorkspaceGDB;
                fieldChecker.ValidateWorkspace = pTargetworkspace;
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

                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, outfeaturedatasetname, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);

                    }
                }
                featurecount=sourceFeatureClass.FeatureCount(null);
                m_success = true;
            }
            catch (Exception ee) { m_success = false; m_strErr = ee.Message; featurecount = 0; }
        
        }
        
        /// <summary>
        ///  创建要素集 20110919 xisheng
        /// </summary>
        /// <param name="feaworkspace">指定工作空间</param>
        /// <param name="datasetname">指定要素集名称</param>
        /// <param name="PrjPath">空间参考</param>
        /// <returns></returns>
        private static IFeatureDataset CreateFeatureDataset(IFeatureWorkspace feaworkspace, string datasetname, string PrjPath)
        {
            try
            {

                string spatialPath = PrjPath;
                ISpatialReferenceFactory pSpaReferenceFac = new SpatialReferenceEnvironmentClass();//空间参考工厂
                ISpatialReference pSpatialReference = null;//用来获得空间参考
                if (File.Exists(spatialPath))
                {
                    pSpatialReference = pSpaReferenceFac.CreateESRISpatialReferenceFromPRJFile(spatialPath);
                }
                if (pSpatialReference == null)
                {
                    pSpatialReference = new UnknownCoordinateSystemClass();
                }

                //设置默认的Resolution
                ISpatialReferenceResolution pSpatiaprefRes = pSpatialReference as ISpatialReferenceResolution;
                pSpatiaprefRes.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon 
                pSpatiaprefRes.SetDefaultXYResolution();
                pSpatiaprefRes.SetDefaultZResolution();
                pSpatiaprefRes.SetDefaultMResolution();
                //设置默认的Tolerence
                ISpatialReferenceTolerance pSpatialrefTole = pSpatiaprefRes as ISpatialReferenceTolerance;
                pSpatialrefTole.SetDefaultXYTolerance();
                pSpatialrefTole.SetDefaultZTolerance();
                pSpatialrefTole.SetDefaultMTolerance();

                //创建数据集

                IFeatureDataset pFeatureDataset = null;//定义数据集用来装载要素类
                pFeatureDataset = feaworkspace.CreateFeatureDataset(datasetname, pSpatialReference);


                return pFeatureDataset;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return null;
            }
        }


        /// <summary>
        ///  获得空间参考 20110919 xisheng
        /// </summary>
        /// <param name="PrjPath">空间参考路径</param>
        /// <returns></returns>
        private static ISpatialReference GetSpatialReferenceformFile(string PrjPath)
        {
            try
            {

                string spatialPath = PrjPath;
                ISpatialReferenceFactory pSpaReferenceFac = new SpatialReferenceEnvironmentClass();//空间参考工厂
                ISpatialReference pSpatialReference = null;//用来获得空间参考
                if (File.Exists(spatialPath))
                {
                    pSpatialReference = pSpaReferenceFac.CreateESRISpatialReferenceFromPRJFile(spatialPath);
                }
                if (pSpatialReference == null)
                {
                    pSpatialReference = new UnknownCoordinateSystemClass();
                }

                //设置默认的Resolution
                ISpatialReferenceResolution pSpatiaprefRes = pSpatialReference as ISpatialReferenceResolution;
                pSpatiaprefRes.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon 
                pSpatiaprefRes.SetDefaultXYResolution();
                pSpatiaprefRes.SetDefaultZResolution();
                pSpatiaprefRes.SetDefaultMResolution();
                //设置默认的Tolerence
                ISpatialReferenceTolerance pSpatialrefTole = pSpatiaprefRes as ISpatialReferenceTolerance;
                pSpatialrefTole.SetDefaultXYTolerance();
                pSpatialrefTole.SetDefaultZTolerance();
                pSpatialrefTole.SetDefaultMTolerance();

                return pSpatialReference;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return null;
            }
        }

        //获得mdb/gdb中要素类
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

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            string name;
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
                    string strName = GetForwadName(textBox.Text.ToUpper(),name);
                    item.SubItems[1].Text = strName.ToUpper();
                }
                else if (str[1].ToLower() == "gdb")
                {
                    name = item.Text.Substring(item.Text.LastIndexOf("-") + 1);
                    string strName = GetForwadName(textBox.Text.ToUpper(), name);
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
            array[3] = filename.Substring(4, filename.LastIndexOf("_") - 4);
            array[1] = filename.Substring(filename.LastIndexOf("_") + 1);
        }

        public string GetDescrib(string str)
        {
            SysGisTable sysTable = new SysGisTable(m_TempWorkspace);
            Exception err = null;
            Dictionary<string,object> dic=sysTable.GetRow("标准图层代码表", "CODE='" + str + "'", out err);
            string strreturn = "";
            if (dic != null)
            {
                if (dic.ContainsKey("NAME"))
                {
                    strreturn = dic["NAME"].ToString();
                }
            }
            sysTable = null;
            return strreturn;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (pTargetworkspace == null)
            {
                MessageBox.Show("请先设置目标数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                if (!fbd.SelectedPath.EndsWith("gdb"))
                {
                    MessageBox.Show("请选择GDB数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string file = fbd.SelectedPath;
                for (int j = 0; j < filename.Count; j++)
                {
                    if (filename[j].Trim().CompareTo(file) == 0)
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
                    filename.Add(file);
                    string name1 = System.IO.Path.GetFileNameWithoutExtension(file).ToUpper();
                    if (textBox.Text != "")
                    {
                        string strName = GetForwadName(textBox.Text.ToUpper(), name1);
                        listView.Items[i].SubItems.Add(strName);
                    }
                    else
                        listView.Items[i].SubItems.Add(name1);
                    if (name1.Length > 15)
                        name1 = name1.Substring(15);
                    string strdescri = GetDescrib(name1.ToUpper());
                    if (strdescri.Trim() != "")
                    {
                        listView.Items[i].SubItems.Add(strdescri);
                    }
                    else
                    {
                        listView.Items[i].SubItems.Add("无");
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
                        listView.Items.Add(file + "---" + list[ii]);
                        filename.Add(file);
                        if (textBox.Text != "")
                        {
                            string strName = GetForwadName(textBox.Text.ToUpper(), list[ii].ToUpper());
                            listView.Items[i].SubItems.Add(strName);

                        }
                        else
                            listView.Items[i].SubItems.Add(list[ii].ToUpper());
                        if (list[ii].Length > 15)
                            list[ii] = list[ii].Substring(15);
                        string strdescri = GetDescrib(list[ii].ToUpper());
                        if (strdescri.Trim() != "")
                            listView.Items[i].SubItems.Add(strdescri);
                        else
                            listView.Items[i].SubItems.Add("无");
                        listView.Items[i].SubItems.Add("等待入库");
                        listView.Items[i].Checked = true;
                        i++;
                    }
                }
                else if (str[1].ToLower() == "gdb")
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new FileGDBWorkspaceFactoryClass();
                    IFeatureWorkspace pFeatureWorkspaceGDB = wf.OpenFromFile(@file, 0) as IFeatureWorkspace;
                    IWorkspace pWorkspaceGDB = pFeatureWorkspaceGDB as IWorkspace;
                    List<string> list = Getfeatureclass(pWorkspaceGDB);
                    for (int ii = 0; ii < list.Count; ii++)
                    {
                        listView.Items.Add(file + "---" + list[ii]);
                        filename.Add(file);
                        if (textBox.Text != "")
                        {
                            string strName = GetForwadName(textBox.Text.ToUpper(), list[ii].ToUpper());
                            listView.Items[i].SubItems.Add(strName);

                        }
                        else
                            listView.Items[i].SubItems.Add(list[ii].ToUpper());
                        string strdescri = GetDescrib(list[ii].ToUpper());
                        if (strdescri.Trim() != "")
                            listView.Items[i].SubItems.Add(strdescri);
                        else
                            listView.Items[i].SubItems.Add("无");
                        listView.Items[i].SubItems.Add("等待入库");
                        listView.Items[i].Checked = true;
                        i++;
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
            str1 =str1+"_"+str2;
            return str1;

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    listView.Items.Remove(item);
                    filename.Remove(item.Text.Substring(0,item.Text.LastIndexOf("---")));
                    i--;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            filename.Clear();
            i = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            bool _check=false;
            //if (text_prj.Text.Trim() == "")
            //{
            //    MessageBox.Show("请选择一个空间参考！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            if (textBox.Text.Trim() == "")
            {
                MessageBox.Show("请设置文件名前缀！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string lastpath="";
            foreach (ListViewItem item in listView.Items)
            {
                m_strErr = "";
                string []str=item.Text.Split('.');
                string[] strfile = item.Text.Split("-".ToCharArray());
                str[1] = str[1].Substring(0, 3);
                int featurecount = 0;
                if(strfile[0]!=lastpath)
                {
                    lastpath=strfile[0];
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("现状数据开始入库,源数据路径为:" + strfile[0]);
                    }
                }
                if (item.Checked&&item.SubItems[3].Text=="等待入库")
                {
                    _check = true;
                    item.SubItems[3].Text = "正在入库";
                    listView.Refresh();
                    //if (str[1].ToLower() == "shp")
                    //    ImportFeatureClassToNewWorkSpace(item.Text, item.SubItems[1].Text);
                   if (str[1].ToLower() == "gdb")
                        ImportGDBToSDE(item.Text, item.SubItems[1].Text,out featurecount);
                   if (m_success)
                   {
                       item.SubItems[3].Text = "入库完成";
                       item.SubItems.Add(featurecount + "个要素已入库");//增加记录入库
                       if (this.WriteLog)
                       {
                           Plugin.LogTable.Writelog("源图层:" + strfile[3] + ",目标图层:" + item.SubItems[1].Text + ",共" + featurecount + "个要素已入库");
                       }
                   }
                   else
                   {
                       if (m_strErr != "")
                       {
                           item.SubItems[3].Text = m_strErr + ",入库失败";
                           if (this.WriteLog)
                           {
                               Plugin.LogTable.Writelog("源图层:" + strfile[3] + "入库失败,详细信息为:" + m_strErr);
                           }
                       }

                       else
                       {
                           item.SubItems[3].Text = "入库失败";
                           if (this.WriteLog)
                           {
                               Plugin.LogTable.Writelog("源图层:" + strfile[3] + "入库失败");
                           }
                       }
                   }
                    listView.Refresh();
                   
                }
            }

            if (_check == false)
                MessageBox.Show("请选择要入库的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("操作已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("入库完成");
                }
            }
        }

        
        private void btnServer_Click(object sender, EventArgs e)
        {
            frmXZDBPropertySet frm = new frmXZDBPropertySet();
            frm.GetPropertySetStr = textSource.Text;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                textSource.Text = frm.GetPropertySetStr;
                pTargetworkspace = frm.m_pworkspace;
            }
        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            frmNameRule frm = new frmNameRule(textBox,m_TempWorkspace);
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

        
        //新增图层
        private void btnNewLayer_Click(object sender, EventArgs e)
        {
            List<string> Laylist = new List<string>();
            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[2].Text.Trim() != "无")
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
                        item.SubItems[2].Text="无";
                    }
                }
            }
            listView.Refresh();
        }

        //选择空间参考
        private void btn_SelectPRJ_Click(object sender, EventArgs e)
        {
            OpenFileDialog flg = new OpenFileDialog();
            flg.Filter = "空间参考文件|*.prj";
            flg.Title = "选择一个空间参考文件";
            if (flg.ShowDialog() == DialogResult.OK)
            {
                text_prj.Text = flg.FileName;
            }

        }
      
    }
}