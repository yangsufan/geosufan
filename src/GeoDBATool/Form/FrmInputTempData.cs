using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
//临时库入库窗体 ygc 2013-01-21
namespace GeoDBATool
{
    public partial class FrmInputTempData : DevComponents.DotNetBar.Office2007Form
    {
        public FrmInputTempData()
        {
            InitializeComponent();
        }
        private DataTable m_InputDt;   //入库列表
        List<string> m_Listfilename = new List<string>();//批量入库取数据名
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmInputTempData_Load(object sender, EventArgs e)
        {
            //初始化工程下拉框
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(ModData.v_projectDetalXML))
            { 
                MessageBox.Show ("无法获取工程配置文件","提示",MessageBoxButtons.OK,MessageBoxIcon.Error );
                this.Close();
                return ;
            }
            xml.Load(ModData.v_projectDetalXML);
            XmlNodeList xmlNodeProList = xml.SelectNodes("//工程");
            for (int i = 0; i < xmlNodeProList.Count; i++)
            {
                cbSelectTempData.Items.Add(xmlNodeProList[i].Attributes["名称"].Value .ToString ());
            }
            if (cbSelectTempData.Items.Count > 0)
            {
                cbSelectTempData.SelectedIndex = 0;
            }
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            if (cbSelectTempData.SelectedItem == null)
            {
                MessageBox.Show("请选择临时库工程！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            if(m_InputDt ==null)
            {
            m_InputDt = InitializeInputTable("入库信息表");
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            OpenFileDialog openFile = new OpenFileDialog();
            string file="";
            #region 根据类型不同，打开相应的文件
            if (rbGDBFile.Checked == true)
            {
                if (fbd.ShowDialog() != DialogResult.OK) return;
                if (!fbd.SelectedPath.EndsWith("gdb"))
                {
                    MessageBox.Show("请选择GDB数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                 file = fbd.SelectedPath;
                for (int j = 0; j < m_Listfilename.Count; j++)
                {
                    if (m_Listfilename[j].Trim().CompareTo(file) == 0)
                    {
                        MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            else if (rbMDBFile.Checked == true)
            {
                openFile.Title = "打开MDB文件";
                openFile.Filter = "*.mdb|*.mdb";
                if (openFile.ShowDialog() != DialogResult.OK) return;
                file = openFile.FileName;
                if (m_Listfilename.Contains(file))
                {
                    MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
            }
            else if (rbShapeFile.Checked == true)
            {
                openFile.Title = "打开Shape文件";
                openFile .Filter ="*.shp|*.shp";
             
                if(openFile .ShowDialog ()!=DialogResult .OK )
                {
                    return;
                }
                file = openFile.FileName;
                if (m_Listfilename.Contains(file))
                {
                    MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
             
            }
            #endregion
            #region 打开文件，并将文件中的图层在表格中显示
            string[] str = file.Split('.');
            if (str[1].ToLower() == "shp")
            {
                m_Listfilename.Add(file);
                DataRow newrow = m_InputDt.NewRow();
                newrow["SourceLayerName"] = System.IO.Path.GetFullPath(file); 
                string strdescri = GetDescrib(System.IO.Path.GetFileNameWithoutExtension(file).ToUpper());
                if (strdescri.Trim() != "")
                {
                    newrow["LayerMarker"] = strdescri;
                }
                else
                {
                    newrow["LayerMarker"] = " 无";
                }
                newrow["InputState"] = "等待入库";
                newrow["ProName"] = cbSelectTempData.SelectedItem.ToString();
                m_InputDt.Rows.Add(newrow);
            }
            else if (str[1].ToLower() == "mdb")
            {
                m_Listfilename.Add(file);
                IWorkspaceFactory wf = new AccessWorkspaceFactory();
                IFeatureWorkspace pFeatureWorkspaceMDB = wf.OpenFromFile(@file, 0) as IFeatureWorkspace;
                IWorkspace pWorkspaceMDB = pFeatureWorkspaceMDB as IWorkspace;
                List<string> list = Getfeatureclass(pWorkspaceMDB);
                for (int i = 0; i < list.Count; i++)
                {
                    DataRow newrow = m_InputDt.NewRow();
                    newrow["SourceLayerName"] = file + "--" + list[i];
                    string strdescri = GetDescrib(list[i].ToUpper());
                    if (strdescri.Trim() != "")
                    {
                        newrow["LayerMarker"] = strdescri;
                    }
                    else
                    {
                        newrow["LayerMarker"] = "无";
                    }
                    newrow["InputState"] = "等待入库";
                    newrow["ProName"] = cbSelectTempData.SelectedItem.ToString();
                    m_InputDt.Rows.Add(newrow);
                }
            }
            else if (str[1].ToLower() == "gdb")
            {
                m_Listfilename.Add(file);
                ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new FileGDBWorkspaceFactoryClass();
                IFeatureWorkspace pFeatureWorkspaceGDB = wf.OpenFromFile(@file, 0) as IFeatureWorkspace;
                IWorkspace pWorkspaceGDB = pFeatureWorkspaceGDB as IWorkspace;
                List<string> list = Getfeatureclass(pWorkspaceGDB);
                for (int i = 0; i < list.Count; i++)
                {
                    DataRow newrow = m_InputDt.NewRow();
                    newrow["SourceLayerName"] = file + "--" + list[i];
                    string strdescri = GetDescrib(list[i].ToUpper());
                    if (strdescri.Trim() != "")
                    {
                        newrow["LayerMarker"] = strdescri;
                    }
                    else
                    {
                        newrow["LayerMarker"] = "无";
                    }
                    newrow["InputState"] = "等待入库";
                    newrow["ProName"] = cbSelectTempData.SelectedItem.ToString();
                    m_InputDt.Rows.Add(newrow);
                }
            }
            #endregion
            dgvInputList.DataSource = m_InputDt;
            //初始化列表中的下拉框
            string proName = cbSelectTempData.SelectedItem.ToString();
           InitializeComBox(proName);
        }
        //初始化入库列表
        private DataTable InitializeInputTable(string TableName)
        {
            DataTable newTale = new DataTable(TableName);
            DataColumn column = new DataColumn();
            column.ColumnName = "SourceLayerName";
            column.DataType = Type.GetType("System.String");
            column.Caption = "源图层名称";
            newTale.Columns.Add(column);

            DataColumn column1 = new DataColumn();
            column1.ColumnName = "LayerMarker";
            column1.DataType = Type.GetType("System.String");
            column1.Caption = "图层描述";
            newTale.Columns.Add(column1);

            DataColumn column2 = new DataColumn();
            column2.ColumnName = "InputState";
            column2.DataType = Type.GetType("System.String");
            column2.Caption = "入库状态";
            newTale.Columns.Add(column2);

            DataColumn column3 = new DataColumn();
            column3.ColumnName = "InputLog";
            column3.DataType = Type.GetType("System.String");
            column3.Caption = "详细日志";
            newTale.Columns.Add(column3);

            DataColumn column4 = new DataColumn();
            column4.ColumnName = "ProName";
            column4.DataType = Type.GetType("System.String");
            column4.Caption = "工程名称 ";
            newTale.Columns.Add(column4);

            return newTale;
        }
        //获取图层描述信息
        public string GetDescrib(string str)
        {
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception err = null;
            Dictionary<string, object> dic = sysTable.GetRow("标准图层代码表", "CODE='" + str + "'", out err);
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
        //通过工程名称获取临时库图层名称
        private List<string> GetLayerNameByProjectName(string ProjectName, XmlDocument xmlDoc)
        {
            List<string> newlist = new List<string>();
            if (xmlDoc == null) return newlist;
            XmlNode ProNode = xmlDoc.SelectSingleNode("//工程[@名称='"+ProjectName+"']");
            XmlNode layerNode = ProNode.SelectSingleNode("//工程[@名称='" + ProjectName + "']//临时库//数据集//图层名");
            string LayerName = layerNode.Attributes["名称"].Value.ToString();
            string[] FcArr = LayerName.Split(',');
            for (int i = 0; i < FcArr.Length; i++)
            {
                newlist.Add(FcArr[i]);
            } 
            return newlist;
        }
        //初始化表格中的下拉框
        private void InitializeComBox(string ProName)
        {
            //初始化工程下拉框
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(ModData.v_projectDetalXML))
            {
                MessageBox.Show("无法获取工程配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            xml.Load(ModData.v_projectDetalXML);
            List<string> listLayer = GetLayerNameByProjectName(ProName,xml);
            try
            {
                for (int i = 0; i < dgvInputList.Rows.Count; i++)
                {
                    //((DataGridViewComboBoxCell)dgvInputList.Rows[i].Cells["TargetLayerName"]).Items.Clear();
                    if (((DataGridViewComboBoxCell)dgvInputList.Rows[i].Cells["TargetLayerName"]).Items.Count == 0)
                    {
                        for (int j = 0; j < listLayer.Count; j++)
                        {
                            ((DataGridViewComboBoxCell)dgvInputList.Rows[i].Cells["TargetLayerName"]).Items.Add(listLayer[j]);
                        }
                    }
                    ((DataGridViewComboBoxCell)dgvInputList.Rows[i].Cells["TargetLayerName"]).Value = listLayer[0];
                }
                
            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.ToString() );
            }
        }
        //当切换工程时，刷新列表 (没有实现)
        private void cbSelectTempData_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string proName = cbSelectTempData.SelectedItem.ToString();
            //InitializeComBox(proName);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInputList.CurrentRow == null)
            {
                MessageBox.Show("请选择要删除的数据！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            string sourceName = dgvInputList.CurrentRow.Cells["SourceLayerName"].Value.ToString();
            dgvInputList.Rows.Remove(dgvInputList.CurrentRow);
            DataRow[] selectRow=m_InputDt .Select ("SourceLayerName='"+sourceName+"'");
            if(selectRow .Length >0)
            {
                m_InputDt.Rows.Remove(selectRow[0]);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           // dgvInputList.Rows.Clear();
            m_Listfilename.Clear();
            m_InputDt.Rows.Clear();
        }

        //将数据导入到临时库中
        private void btnDoInput_Click(object sender, EventArgs e)
        {
            if (dgvInputList.Rows.Count == 0)
            {
                MessageBox.Show("请先添加要入库的文件！","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < dgvInputList.Rows.Count; i++)
            {
                #region 获取源图层
                //获取源文件名称和图层名称
                string SourceFile = dgvInputList.Rows[i].Cells["SourceLayerName"].Value.ToString();
                IFeatureClass pSourceFeatureClass = null;
                if (SourceFile.Contains("--"))
                {
                    string filePath = SourceFile.Substring(0, SourceFile.IndexOf("--"));
                    string layerName = SourceFile.Substring(SourceFile.IndexOf("--") + 2, SourceFile.Length - SourceFile.IndexOf("--") - 2);
                    pSourceFeatureClass = GetFeatureClassByName(filePath, layerName);
                }
                else
                {
                    string layerName1 = SourceFile.Substring(SourceFile.LastIndexOf("\\") + 1, SourceFile.Length - 5 - SourceFile.LastIndexOf("\\"));
                    pSourceFeatureClass = GetFeatureClassByName(SourceFile, layerName1);
                }
                //通过工程名称和表格目标层，获取目标图层
                if (!File.Exists(ModData.v_projectDetalXML))
                {
                    MessageBox.Show("无法获取工程配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                #region 获取目标图层
                string ProName = dgvInputList.Rows[i].Cells["ProName"].Value.ToString();
                string targetLayerName = dgvInputList.Rows[i].Cells["TargetLayerName"].FormattedValue.ToString();
               
                XmlDocument xmlPro = new XmlDocument();
                xmlPro.Load(ModData.v_projectDetalXML);
                XmlElement xmlConElement = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//连接信息") as XmlElement;
                IWorkspace pTargetWorkspace = ModDBOperator.GetDBInfoByXMLNode(xmlConElement, "") as IWorkspace;
                XmlNode xmlDataset = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//数据集");
                string datasetName = xmlDataset.Attributes["名称"].Value.ToString();
                IFeatureClass pTargetFeatureClass = GetFeatureClass(pTargetWorkspace, targetLayerName, datasetName);
                #endregion

                dgvInputList.Rows[i].Cells["InputState"].Value = "正在入库";
                Exception error = null;

                List<string> listXianCode = GetUniqueValues(pSourceFeatureClass, "xian", "");
                List<string> listExitCode = null;
                //循环判断临时库中是否存在该县的数据
                if (listXianCode == null || listXianCode.Count == 0)
                {
                    CopySourceFeatureClass(pSourceFeatureClass, pTargetFeatureClass, null, "", out error);
                }
                else
                {
                    string xianName=CheckXianData(pTargetFeatureClass, listXianCode, "xian",out listExitCode);
                    if (xianName != "")
                    {
                        if (MessageBox.Show("当前临时库已存在：" + xianName + "的数据，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            //清空已有县数据
                            for (int t = 0; t < listExitCode.Count; t++)
                            {
                                string tableName = pTargetFeatureClass.AliasName;
                                tableName =tableName .Substring (tableName .IndexOf (".")+1,tableName .Length - tableName .IndexOf (".")-1);
                                clsInputTempDate.DeleteFeatureClass(pTargetFeatureClass, "xian='" + listExitCode[t] + "'", tableName, out error);
                                CopySourceFeatureClass(pSourceFeatureClass, pTargetFeatureClass, null, "", out error);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        CopySourceFeatureClass(pSourceFeatureClass, pTargetFeatureClass, null, "", out error);
                    }
                }
                 if (error != null)
                 {
                     dgvInputList.Rows[i].Cells["InputState"].Value = "入库失败";
                     dgvInputList.Rows[i].Cells["InputLog"].Value = error.Message.ToString();
                 }
                 else
                 {
                     dgvInputList.Rows[i].Cells["InputState"].Value = "入库成功";
                 }
                
            }
            MessageBox.Show("完成入库操作！","提示",MessageBoxButtons .OK,MessageBoxIcon.Information);
        } 
        private void dgvInputList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
        }
        /// <summary>
        /// 通过图层名称和路径获取IFeatureClass
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private IFeatureClass GetFeatureClassByName(string filePath, string LayerName)
        {
            if (filePath == "" || LayerName == "") return null;
            IWorkspace pWorkspace = GetWorkspace(filePath);
            IFeatureClass pFeatureClass = GetFeatureClass(pWorkspace, LayerName,"");
            return pFeatureClass;
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
                        MessageBox.Show(ex.ToString(), "错误");
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
                    catch (Exception ex)
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
        //通过IWorkspace获取图层
        public static IFeatureClass GetFeatureClass(IWorkspace pWorkspace,string LayerName,string DatasetName)
        {
            if (pWorkspace == null)
            {
                return null;
            }
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                if (pDataset.Name == LayerName)
                {
                    return pDataset as IFeatureClass;
                }
                pDataset = pEnumDataset.Next();
            }
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pDataset = pEnumDataset.Next();
          //  IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            while (pDataset != null)
            {
                if (DatasetName == "")
                {
                    IFeatureClassContainer pFeatureClassContainer = pDataset as IFeatureClassContainer;
                    IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                    IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                    while (pFeatureClass != null)
                    {
                        if (pFeatureClass.AliasName.Contains(LayerName))
                        {
                            return pFeatureClass;
                        }
                        pFeatureClass = pEnumFeatureClass.Next();
                    }
                }
                else
                {
                    if (pDataset.Name == DatasetName)
                    {
                        IFeatureClassContainer pFeatureClassContainer = pDataset as IFeatureClassContainer;
                        IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                        IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                        while (pFeatureClass != null)
                        {
                            if (pFeatureClass.AliasName.Contains(LayerName))
                            {
                                return pFeatureClass;
                            }
                            pFeatureClass = pEnumFeatureClass.Next();
                        }
                    }
                }
                pDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            return null;
        }
        //将源图层数据复制到目标图层数据
        private bool CopySourceFeatureClass(IFeatureClass SourceFeatureClass, IFeatureClass TargetFeatureClass, IGeometry pGeometry,string strCondition,out Exception ErrorMsg)
        {
            bool flag = false;
            if (SourceFeatureClass == null || TargetFeatureClass == null)
            {
                ErrorMsg = null;
                return flag;
            }
            IFeatureCursor pFeatureCursor = null;
            int FeatureCount = 0;
            if (pGeometry != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeometry;
                switch (pGeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                }
                if (strCondition == "") { }
                else
                {
                    pSpatialFilter.WhereClause = strCondition;
                }
                FeatureCount = SourceFeatureClass.FeatureCount(pSpatialFilter);
                pFeatureCursor = SourceFeatureClass.Search(pSpatialFilter, false);
            }
            else
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = strCondition;
                FeatureCount = SourceFeatureClass.FeatureCount(pQueryFilter);
                pFeatureCursor = SourceFeatureClass.Search(pQueryFilter,false);
            }
            notcutExport(pFeatureCursor, TargetFeatureClass, FeatureCount,out ErrorMsg);
            return flag;
        }
        //不剪裁输出
        private void notcutExport(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount,out Exception error)
        {
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            error = null;
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
                    try
                    {
                        if ((iIndex > -1) && (pFeatureBuffer.Fields.get_Field(iIndex).Editable == true))
                        {
                            pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                        }
                    }
                    catch
                    { }
                }
                // 写入入库日期和入库人员
                int DateIndex = pFeatureBuffer.Fields.FindField("PutInDate");
                int UserIndex = pFeatureBuffer.Fields.FindField("USERNAME");
                int CheckIndex = pFeatureBuffer.Fields.FindField("CheckState");
                try
                {
                    if (DateIndex > -1 && UserIndex > -1&&CheckIndex>-1)
                    {
                        pFeatureBuffer.set_Value(DateIndex, DateTime.Today.ToString("yyyy - MM - dd"));
                        pFeatureBuffer.set_Value(CheckIndex, "未审核");
                        if (Plugin.LogTable.user != "")
                        {
                            pFeatureBuffer.set_Value(UserIndex, Plugin.LogTable.user);
                        }
                    }
                }
                catch {}

                try
                {
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureCursor.InsertFeature(pFeatureBuffer);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }

        /// <summary>
        /// 将GDB的数据入到SDE中 xisheng 20110919
        /// </summary>
        private void ImportGDBToSDE(IWorkspace pTargetworkspace,IWorkspace pSourceWorkspace,string LayerName,out int featurecount )
        {
            bool m_success = false;//初始化
            try
            {
                //string filepath = file.Substring(0, file.LastIndexOf("---"));
                //string filename = file.Substring(file.LastIndexOf("-") + 1);

                ////打开mdb文件所在的工作空间
                //ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wf = new FileGDBWorkspaceFactory();
                //IFeatureWorkspace pFeatureWorkspaceGDB = wf.OpenFromFile(@filepath, 0) as IFeatureWorkspace;
                //IWorkspace pWorkspaceGDB = pFeatureWorkspaceGDB as IWorkspace;
                // 创建源工作空间名称     
                IDataset sourceWorkspaceDataset = (IDataset)pSourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;

                //创建源数据集名称        
                //IFeatureClassName sourceFeatureClassName = serverContext.CreateObject("esriGeoDatabase.FeatureClassName") as IFeatureClassName;
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = LayerName.Substring (0,LayerName.IndexOf ("_"));

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
                tmpfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(LayerName);
                //if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureDataset, textBox.Text))
                //{
                //    tmpfeaturedataset = pFeatureWorkspace.OpenFeatureDataset(textBox.Text);
                //    if (text_prj.Text != "")
                //    {
                //        IGeoDatasetSchemaEdit pgeodataset = tmpfeaturedataset as IGeoDatasetSchemaEdit;
                //        if (pgeodataset.CanAlterSpatialReference)
                //            pgeodataset.AlterSpatialReference(GetSpatialReferenceformFile(text_prj.Text));
                //    }
                //}
                //else
                //{
                //    tmpfeaturedataset = CreateFeatureDataset(pFeatureWorkspace, textBox.Text, text_prj.Text);

                //}
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, LayerName))
                {

                    IFeatureClass tmpfeatureclass;
                    tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(LayerName);
                    IDataset tempset = tmpfeatureclass as IDataset;
                    tempset.Delete();
                }

                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = LayerName;
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
                fieldChecker.InputWorkspace = pSourceWorkspace;
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
                featurecount = sourceFeatureClass.FeatureCount(null);
                m_success = true;
            }
            catch (Exception ee) { m_success = false;  featurecount = 0; }

        }

        //获取指定字段唯一值
        private List<string> GetUniqueValues(IFeatureClass pFeatureClass,string FieldName,string condition) 
        {
            List<string> newlist = new List<string>();
            if (pFeatureClass == null) return newlist;
            if (FieldName == "") return newlist;
            try
            {
                IFeatureCursor pCursor = null;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                pCursor = pFeatureClass.Search(pQueryFilter, false);

                System.Collections.IEnumerator enumerator;
                IDataStatistics DS = new DataStatisticsClass();
                DS.Field = FieldName;//设置唯一值字段
                DS.Cursor = pCursor as ICursor;//数据来源
                enumerator = DS.UniqueValues;//得到唯一值
                enumerator.Reset();//从新指向第一个值
                while (enumerator.MoveNext())//遍历唯一值
                {
                    string strTemp = enumerator.Current.ToString();
                    newlist.Add(strTemp);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            } 
            catch(Exception ex)
            {
                return newlist;
            }
            return newlist;
        }
        //检查临时库中是否存在已有县数据
        //存在该县级数据返回false
        private string CheckXianData(IFeatureClass pFeatureClass, List<string> listCode, string FieldName,out List<string > listXianCode)
        {
            string xianName = "";
            listXianCode = new List<string>();
            if (listCode == null || listCode.Count == 0)
            {
                return xianName;
            }
            try
            {
                for (int i = 0; i < listCode.Count; i++)
                {
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = FieldName + "='" + listCode[i] + "'";
                    int count = pFeatureClass.FeatureCount(pFilter);
                    if (count > 0)
                    {
                        listXianCode.Add(listCode[i]);
                        if (xianName == "")
                        {
                            xianName = SysCommon.ModXZQ.GetXzqName(Plugin.ModuleCommon.TmpWorkSpace, listCode[i]);
                        }
                        else
                        {
                            xianName = xianName + "," + SysCommon.ModXZQ.GetXzqName(Plugin.ModuleCommon.TmpWorkSpace, listCode[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return xianName;
            }
            return xianName;
        }
    }
}
