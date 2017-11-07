using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

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

//*********************************************************************************
//** 文件名：frmDataReduction.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-10
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDataCenterFunLib
{
    public partial class frmDataReduction : DevComponents.DotNetBar.Office2007Form
    {
        public frmDataReduction()
        {
            InitializeComponent();
        }
        IWorkspace pWorkspace;
        IWorkspace2 pWorkspace2;
        string m_startstr;//列表中编辑前数据
        string m_endstr;//编辑后数据
        string[] array = new string[6];//分析数据成数组形式
        bool m_first;//是否第一次加载列表框
        int [] m_state={0,0,0,0,0};//4个选项列表框选择状态
        public static TreeNode Node;//公共静态变量获得从数据单元树的节点

        private void frmDataReduction_Load(object sender, EventArgs e)
        {
            m_first = true;
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在加载数据");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            LoadGridView(vProgress);
            vProgress.Close();
            this.Activate();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            //listBoxDetail.Items.Clear();
            datagwSource.Rows.Clear();
        }
        //修改
        private void datagwSource_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1||e.ColumnIndex!=1)
                return;
            //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
            //pWorkspace = (IWorkspace)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
            pWorkspace = GetWorkspace(comboBoxSource.Text);
            if (pWorkspace != null)
            {
                pWorkspace2 = (IWorkspace2)pWorkspace;
                m_endstr = datagwSource.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, m_endstr))
                {
                    MessageBox.Show("命名名称已存在,请修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    datagwSource.Rows[e.RowIndex].Cells[1].Value = m_startstr;
                    return;
                }
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, m_startstr))
                {
                    IFeatureClass tmpfeatureclass;
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(m_startstr);
                    IDataset set = tmpfeatureclass as IDataset;
                    set.Rename(m_endstr);
                    EditSql(m_startstr, m_endstr);

                    //更改代码 实时更新图层描述
                    GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                    string mypath = dIndex.GetDbInfo();
                    string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                    string player = m_endstr.Substring(15);//图层组成
                    string strExp = "select 描述 from 标准图层信息表 where 代码='" + player + "'";
                    GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                    string playername = db.GetInfoFromMdbByExp(strCon, strExp);
                    if (playername != "")
                        datagwSource.Rows[e.RowIndex].Cells[2].Value = playername;

                    //listBoxDetail.Items.Add("将" + m_startstr);
                    //listBoxDetail.Items.Add("改为" + m_endstr);
                    //listBoxDetail.Items.Add(" ");
                    MessageBox.Show("修改数据成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void datagwSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                
                if(e.ColumnIndex==1)
                m_startstr = datagwSource.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch { }
        }

        private void 删除选中行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = false;
            foreach (DataGridViewRow row in datagwSource.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue == true)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                MessageBox.Show("没有选中行，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;             
            }
            DialogResult result=MessageBox.Show("是否确定删除!","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result==DialogResult.Yes)
            {
                foreach (DataGridViewRow row in datagwSource.Rows)
                {
                    if ((bool)row.Cells[0].EditedFormattedValue == true)
                    {
                        //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                        //pWorkspace = (IWorkspace)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                        pWorkspace = GetWorkspace(comboBoxSource.Text);
                        if (pWorkspace != null)
                        {
                            pWorkspace2 = (IWorkspace2)pWorkspace;
                            if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, row.Cells[1].Value.ToString().Trim()))
                            {
                                IFeatureClass tmpfeatureclass;
                                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                                tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(row.Cells[1].Value.ToString().Trim());
                                IDataset set = tmpfeatureclass as IDataset;
                                set.CanDelete();
                                set.Delete();
                                
                                //listBoxDetail.Items.Add("删除了" + row.Cells[1].Value + "数据");
                                //listBoxDetail.Items.Add(" ");
                                //listBoxDetail.Refresh();
                            }
                        }
                        DeleteSql(row.Cells[1].Value.ToString());
                      
                    }
                }
                datagwSource.Rows.Clear();
                ChangeGridView();//重新加载数据
                    MessageBox.Show("删除数据成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void btn_Detail_Click(object sender, EventArgs e)
        {
            //if (this.panelDetail.Visible)
            //{
            //    this.panelDetail.Visible = false;
            //    btn_Detail.Text = "详细信息";
            //    btn_Clear.Visible = false;
            //    this.Width = 384;
            //}
            //else
            //{
            //    this.panelDetail.Visible = true;
            //    btn_Detail.Text = "收起";
            //    btn_Clear.Visible = true;
            //    this.Width = 580;
            //}
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagwSource.SelectedRows.Count == 1)
            {
                this.datagwSource.CurrentCell = this.datagwSource.SelectedRows[0].Cells[1];//获取当前单元格
                this.datagwSource.BeginEdit(true);//将单元格设为编辑状态
            }
            else
            {
                MessageBox.Show("请选择一行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        /// <summary>
        /// 将数据分析成字符串数组
        /// </summary>
        /// <param name="filename">数据名称</param>
        public void AnalyseDataToArray(string filename)
        {
            if (filename.Contains("."))//针对SDE 用户名.图层名格式的
                filename = filename.Substring(filename.LastIndexOf(".")+1);
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
            array[5] = filename;//图层组成
        }

        //删除数据库中数据
        public void DeleteSql(string data)
        {
            try
            {
                AnalyseDataToArray(data);
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = string.Format("delete from 数据编码表 where 业务大类代码='{0}' and 年度='{1}' and 业务小类代码='{2}'and 行政代码='{3}' and 比例尺='{4}' and 图层代码='{5}' and 数据源名称='{6}'",
                    array[0], array[1], array[2], array[3], array[4], array[5],comboBoxSource.Text.Trim());
                GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
                dDbFun.ExcuteSqlFromMdb(strCon, strExp);
               
                //从数据编码表更新入库信息表
                dDbFun.UpdateMdbInfoTable(array[0], array[1], array[2], array[3], array[4]);
                
                //从代码更新入库信息表
                   #region
                //strExp = "select 图层组成 from 地图入库信息表 where 业务大类代码='" + array[0] + "' And 行政代码 ='" + array[3] + "' And 年度='" +
                //    array[1] + "'And  比例尺='" + array[4] + "'And 业务小类代码='" + array[2] + "'";
                //string layers = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
                //if (!layers.Contains('/'.ToString()))
                //{
                //    if (layers.Trim() != array[5])
                //        return;
                //    else
                //        layers = "";
                //}
                //else
                //{
                //    string[] layer = layers.Split('/');
                //    for (int i = 0; i < layer.Length; i++)
                //    {
                //        if (layer[i].Trim() == array[5])
                //        {
                //            if (i == 0)
                //            {
                //                layers = layers.Substring(array[5].Length + 1);
                //            }
                //            else
                //                layers = layers.Replace('/' + layer[i], "");
                //        }
                //    }
                //}
                //strExp = "update 地图入库信息表 set 图层组成='" + layers + "' where 业务大类代码='" + array[0] + "' And 行政代码 ='" + array[3] + "' And 年度='" +
                //    array[1] + "'And  比例尺='" + array[4] + "'And 业务小类代码='" + array[2] + "'";
                //dDbFun.ExcuteSqlFromMdb(strCon, strExp);
                #endregion
            }
            catch(System.Exception e)
            {
              //  MessageBox.Show(e.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //修改数据库中数据
        public void EditSql(string data1, string data2)
        {
            try
            {
                AnalyseDataToArray(data1);
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = string.Format("select ID from 数据编码表 where 业务大类代码='{0}' and 年度='{1}' and 业务小类代码='{2}'and 行政代码='{3}' and 比例尺='{4}' and 图层代码='{5}' and 数据源名称='6'",
                    array[0], array[1], array[2], array[3], array[4], array[5],comboBoxSource.Text.Trim());
                GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
                int id1 = dDbFun.GetIDFromMdb(strCon, strExp);
                AnalyseDataToArray(data2);
                if (id1 != 0)
                    strExp = string.Format("update 数据编码表 set 业务大类代码='{0}',年度='{1}',业务小类代码='{2}',行政代码='{3}',比例尺='{4}',图层代码='{5}' where ID={6}",
                        array[0], array[1], array[2], array[3], array[4], array[5], id1);
                dDbFun.ExcuteSqlFromMdb(strCon, strExp);//更新数据编码表
                dDbFun.UpdateMdbInfoTable(array[0], array[1], array[2], array[3], array[4]);//更新地图入库信息表
              
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            this.修改ToolStripMenuItem_Click(sender, e);
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            this.删除选中行ToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// 将一个要素类从一个工作空间转移到另外一个工作空间
        /// 注意目标工作空间不能有改要素类，必须先清除  
        /// </summary>
        /// <param name="sourceWorkspace">源工作空间</param>
        /// <param name="targetWorkspace">目标工作空间</param>
        /// <param name="nameOfSourceFeatureClass">源要素类名</param>
        /// <param name="nameOfTargetFeatureClass">目标要素类名</param>
        public void IFeatureDataConverter_ConvertFeatureClass(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string nameOfSourceFeatureClass, string nameOfTargetFeatureClass)
        {
            //create source workspace name   
            IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
            IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
            //create source dataset name   
            IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
            IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
            sourceDatasetName.WorkspaceName = sourceWorkspaceName;
            sourceDatasetName.Name = nameOfSourceFeatureClass;
            //create target workspace name   
            IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
            IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
            //create target dataset name   
            IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
            IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
            targetDatasetName.WorkspaceName = targetWorkspaceName;
            targetDatasetName.Name = nameOfTargetFeatureClass;
            //Open input Featureclass to get field definitions.   
            ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
            IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
            //Validate the field names because you are converting between different workspace types.   
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IFields targetFeatureClassFields;
            IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
            IEnumFieldError enumFieldError;
            // Most importantly set the input and validate workspaces!     
            fieldChecker.InputWorkspace = sourceWorkspace;
            fieldChecker.ValidateWorkspace = targetWorkspace;
            fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);
            // Loop through the output fields to find the geomerty field   
            IField geometryField;
            for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
            {
                if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = targetFeatureClassFields.get_Field(i);
                    // Get the geometry field's geometry defenition            
                    IGeometryDef geometryDef = geometryField.GeometryDef;
                    //Give the geometry definition a spatial index grid count and grid size        
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0);
                    //Allow ArcGIS to determine a valid grid size for the data loaded      
                    targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                    // we want to convert all of the features   
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "";
                    // Load the feature class     
                    IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                    IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
                    break;
                }
            }
        }
        //下载为MDB文件
        private void btn_ExportNDB_Click(object sender, EventArgs e)
        {
            pWorkspace = GetWorkspace(comboBoxSource.Text);
            if (pWorkspace==null)
            {
                MessageBox.Show("数据源空间不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool flag = false;
            //获取模板路径
            string sourcefilename = Application.StartupPath + "\\..\\Template\\DATATEMPLATE.mdb";
            foreach (DataGridViewRow row in datagwSource.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue == true)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                MessageBox.Show("没有选中行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在下载数据，请稍后");
            try
            {
                if (File.Exists(sourcefilename))//原模板存在
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "MDB数据|*.mdb";
                    dlg.OverwritePrompt =false;
                    dlg.Title = "保存到MDB";


                    DialogResult result = MessageBox.Show("下载是否去掉前缀？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {

                        //初始化进度条
                        
                        vProgress.EnableCancel = false;
                        vProgress.ShowDescription = false;
                        vProgress.FakeProgress = true;
                        vProgress.TopMost = true;
                        vProgress.ShowProgress();
                        Application.DoEvents();
                        //IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                        //pWorkspace = (IWorkspace)(pWorkspaceFactory.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                        //如果存在mdb,替换文件，则复制模板到指定路径
                        //如果存在mdb，不替换，则追加到这个文件
                        File.Copy(sourcefilename, dlg.FileName, true);
                        string cellvalue = "";
                        IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
                        IWorkspace pws = (IWorkspace)(Pwf.OpenFromFile(dlg.FileName, 0));
                        IWorkspace2 pws2 = (IWorkspace2)pws;
                        foreach (DataGridViewRow row in datagwSource.Rows)
                        {
                            if ((bool)row.Cells[0].EditedFormattedValue == true)
                            {
                                
                                cellvalue = row.Cells[1].Value.ToString().Trim();
                                if (cellvalue.Contains("."))
                                {
                                    cellvalue = cellvalue.Substring(cellvalue.LastIndexOf(".") + 1);
                                }
                                if (result == DialogResult.Yes) cellvalue = cellvalue.Substring(15);//去掉前缀
                                pws2 = (IWorkspace2)pws;
                                if (pws2.get_NameExists(esriDatasetType.esriDTFeatureClass, cellvalue))
                                {
                                    IFeatureClass tmpfeatureclass;
                                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pws;
                                    tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(cellvalue);
                                    IDataset set = tmpfeatureclass as IDataset;
                                    set.CanDelete();
                                    set.Delete();
                                    flag = true;
                                }
                                IFeatureDataConverter_ConvertFeatureClass(pWorkspace, pws, row.Cells[1].Value.ToString().Trim(), cellvalue);
                            }
                        }
                        vProgress.Close();
                        MessageBox.Show("下载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Activate();
                    }
                }
            }
            catch (Exception ex)
            {
                vProgress.Close();
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Activate();
            }

        }

        //加载列表框
        private void LoadCombox()
        {
           GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
           string mypath = dIndex.GetDbInfo();
           string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
           string str_Exp = "select 年度 from 数据编码表";
           GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
           List<string> list = dbfun.GetDataReaderFromMdb(strCon, str_Exp);
           comboBoxYear.Items.Add("所有年度");
           for (int i = 0; i < list.Count; i++)
           {
               if (!comboBoxYear.Items.Contains(list[i]))
                   comboBoxYear.Items.Add(list[i]);

           }
           if (comboBoxYear.Items.Count > 0)
               comboBoxYear.SelectedIndex = 0;
           //str_Exp = "select 行政名称,行政代码 from 数据单元表";
           //DataTable dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
           //comboBoxArea.Items.Add("所有行政区");
           //for (int i = 0; i < dt.Rows.Count; i++)
           //{
           //    comboBoxArea.Items.Add(dt.Rows[i]["行政名称"]+"("+dt.Rows[i]["行政代码"]+")");
           //}
           //if (comboBoxArea.Items.Count > 0)
           //    comboBoxArea.SelectedIndex = 0;
           str_Exp = "select 描述,代码 from 比例尺代码表";
           DataTable dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
           comboBoxScale.Items.Add("所有比例尺");
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               comboBoxScale.Items.Add(dt.Rows[i]["描述"]+"("+dt.Rows[i]["代码"]+")");
           }
           if (comboBoxScale.Items.Count > 0)
               comboBoxScale.SelectedIndex = 0;
           str_Exp = "select 描述,代码 from 业务大类代码表";
           dt= dbfun.GetDataTableFromMdb(strCon, str_Exp);
           comboBoxBig.Items.Add("所有大类业务");
           for (int i = 0; i <dt.Rows.Count; i++)
           {
               comboBoxBig.Items.Add(dt.Rows[i]["描述"]+"("+dt.Rows[i]["代码"]+")");
           }
           if (comboBoxBig.Items.Count > 0)
               comboBoxBig.SelectedIndex = 0;
           //str_Exp = "select 描述,业务小类代码 from 业务小类信息表";
           //dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
           comboBoxSub.Items.Add("所有小类业务");
           //for (int i = 0; i < dt.Rows.Count; i++)
           //{
           //    comboBoxSub.Items.Add(dt.Rows[i]["描述"] + "(" + dt.Rows[i]["业务小类代码"] + ")");
           //}
           if (comboBoxSub.Items.Count > 0)
               comboBoxSub.SelectedIndex = 0;

        }
        private void comboBoxYear_Click(object sender, EventArgs e)
        {
            if (m_first)
            {
                LoadCombox();
                m_first = false;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       void LoadGridView()
       {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string strExp = "select 数据源名称 from 物理数据源表";
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            comboBoxSource.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxSource.Items.Add(list[i]);//加载数据源列表框
            }
            if (list.Count > 0)
            {
                comboBoxSource.SelectedIndex = 0;//默认选择第一个
            }
            string player = "";
                string sourename =comboBoxSource.Text.Trim();
                strExp = "select * from 数据编码表 where 数据源名称='" + sourename + "'";
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strExp = "select 字段名称 from 图层命名规则表";
                    string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                    string[] arrName = strname.Split('+');//分离字段名称
                    string layername = "";
                    for (int j = 0; j < arrName.Length; j++)
                    {
                        switch (arrName[j])
                        {
                            case "业务大类代码":
                                layername += dt.Rows[i]["业务大类代码"].ToString();//业务大类代码
                                break;
                            case "年度":
                                layername += dt.Rows[i]["年度"].ToString();//年度
                                break;
                            case "业务小类代码":
                                layername += dt.Rows[i]["业务小类代码"].ToString();//业务小类代码
                                break;
                            case "行政代码":
                                layername += dt.Rows[i]["行政代码"].ToString();//行政代码
                                break;
                            case "比例尺":
                                layername += dt.Rows[i]["比例尺"].ToString();//比例尺
                                break;
                        }
                    }
                    layername += dt.Rows[i]["图层代码"].ToString();
                    strExp = "select 描述 from 标准图层信息表 where 代码='" + dt.Rows[i]["图层代码"].ToString() + "'";
                    string playername = db.GetInfoFromMdbByExp(strCon, strExp);
                    if (playername != "")
                        player = playername;
                    string username = GetSourceUser(comboBoxSource.Text).Trim().ToUpper(); ;
                    if (username != "")
                        layername = username + "." + layername;
                    datagwSource.Rows.Add(new object[] { true, layername, player });
                }
                //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                //pWorkspace = (IWorkspace)(Pwf.OpenFromFile(comboBoxSource.Text, 0));
                //IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
                //IDataset dataset = enumDataset.Next();
                ////遍历mdb的每一个独立要素类
                //while (dataset != null)
                //{

                //    IFeatureClass pFeatureClass = dataset as IFeatureClass;
                //    player = pFeatureClass.AliasName.Substring(15);
                //strExp = "select 描述 from 标准图层信息表 where 代码='" + player + "'";
                //string playername = db.GetInfoFromMdbByExp(strCon, strExp);
                //if (playername != "")
                //    player = playername;
                //datagwSource.Rows.Add(new object[] { true, pFeatureClass.AliasName, player });
                //dataset = enumDataset.Next();
                //}
            
       }

        //显示全部数据
        private void LoadGridView(CProgress vprocess)//重载加载数据函数
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string strExp = "select 数据源名称 from 物理数据源表";
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxSource.Items.Add(list[i]);//加载数据源列表框
            }
            if (list.Count > 0)
            {
                comboBoxSource.SelectedIndex = 0;//默认选择第一个
            }
            string player = "";
                string sourename=comboBoxSource.Text.Trim();
                strExp = "select * from 数据编码表 where 数据源名称='" + sourename + "'";
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strExp = "select 字段名称 from 图层命名规则表";
                    string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                    string[] arrName = strname.Split('+');//分离字段名称
                    string layername="";
                    player = "";
                    for (int j = 0; j < arrName.Length; j++)
                    {
                        switch (arrName[j])
                        {
                            case "业务大类代码":
                                layername += dt.Rows[i]["业务大类代码"].ToString();//业务大类代码
                                break;
                            case "年度":
                                layername += dt.Rows[i]["年度"].ToString();//年度
                                break;
                            case "业务小类代码":
                                layername += dt.Rows[i]["业务小类代码"].ToString();//业务小类代码
                                break;
                            case "行政代码":
                                layername += dt.Rows[i]["行政代码"].ToString();//行政代码
                                break;
                            case "比例尺":
                                layername += dt.Rows[i]["比例尺"].ToString();//比例尺
                                break;
                        }
                    }
                    layername += dt.Rows[i]["图层代码"].ToString();
                    strExp = "select 描述 from 标准图层信息表 where 代码='" + dt.Rows[i]["图层代码"].ToString() + "'";
                    string playername = db.GetInfoFromMdbByExp(strCon, strExp);
                    if (playername != "")
                    {
                        player = playername;
                    }
                    else
                    {
                        player = dt.Rows[i]["图层代码"].ToString();
                    }
                    string username=GetSourceUser(comboBoxSource.Text).Trim().ToUpper();;
                    if (username != "")
                        layername = username +"."+layername;
                    datagwSource.Rows.Add(new object[] { true, layername, player });
                }
                    //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                    //pWorkspace = (IWorkspace)(Pwf.OpenFromFile(comboBoxSource.Text, 0));
                    //IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
                    //IDataset dataset = enumDataset.Next();
                    ////遍历mdb的每一个独立要素类
                    //while (dataset != null)
                    //{

                    //    IFeatureClass pFeatureClass = dataset as IFeatureClass;
                    //    player = pFeatureClass.AliasName.Substring(15);
                    //strExp = "select 描述 from 标准图层信息表 where 代码='" + player + "'";
                    //string playername = db.GetInfoFromMdbByExp(strCon, strExp);
                    //if (playername != "")
                    //    player = playername;
                    //datagwSource.Rows.Add(new object[] { true, pFeatureClass.AliasName, player });
                    //dataset = enumDataset.Next();
                //}
            
           
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
                    pPropertySet.SetProperty("Instance", "5151");//"port:" + txtService.Text
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

       //得到数据库用户
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
                return strname;
            }
            catch { return ""; }
        }
 

        //年度选择状态
        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!m_first)
            {
                m_state[0] = comboBoxYear.SelectedIndex;
                ChangeGridView();
            }

        }
        //行政区的值发生变换
        private void comboBoxArea_TextChanged(object sender, EventArgs e)
        {
                ChangeGridView();
            
        }

        //比例尺选择状态
        private void comboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_first)
            {
                m_state[2] = comboBoxScale.SelectedIndex;
                ChangeGridView();
            }
        }

        //业务大类选择状态
        private void comboBoxBig_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!m_first)
            {
                m_state[3] = comboBoxBig.SelectedIndex;
                ChangeComboxSub();
                
                ChangeGridView();
            }
        }
        //业务小类选择状态
        private void comboBoxSub_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!m_first)
            {
                m_state[4] = comboBoxSub.SelectedIndex;

                ChangeGridView();
            }
        }
        //数据源选择
        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_first)
            {
                ChangeGridView();
            }
        }

        private void ChangeComboxSub()
        {
            comboBoxSub.Items.Clear();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string strExp = "";
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            comboBoxSub.Items.Add("所有小类业务");
            if (m_state[3] != 0)
            {
                string strall = comboBoxBig.Items[m_state[3]].ToString();
                string[] BigClass = strall.Split('(', ')');
                strExp = "select 描述,业务小类代码 from 业务小类代码表 where 业务大类代码='" + BigClass[1] + "'";
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxSub.Items.Add(dt.Rows[i]["描述"] + "(" + dt.Rows[i]["业务小类代码"] + ")");
                }
            }
            if (comboBoxSub.Items.Count > 0)
                comboBoxSub.SelectedIndex = 0;

        }


        /// <summary>
        /// 根据列表框选择状态动态显示GirdView
        /// </summary>
        private void ChangeGridView()
        {
            this.Cursor = Cursors.WaitCursor;
            string strall = "";
            datagwSource.Rows.Clear();
            bool state = true;
            string player = "";
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            //path = GetSourcePath(comboBoxSource.Text);
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string str_Exp = "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            //if (Directory.Exists(@path))
            //{
                //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
            pWorkspace = GetWorkspace(comboBoxSource.Text);
            if (pWorkspace == null)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("数据源空间不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
                IDataset dataset = enumDataset.Next();
                //遍历mdb的每一个独立要素类
                while (dataset != null)
                {
                    state = true;
                    //IFeatureClass pFeatureClass = dataset as IFeatureClass;
                    //player = pFeatureClass.AliasName;
                    player = dataset.Name;

                    if(pWorkspace.Type==esriWorkspaceType.esriRemoteDatabaseWorkspace)//如果SDE去掉用户名前缀
                    { 
                        Int32 userlenth=pWorkspace.ConnectionProperties.GetProperty("USER").ToString().Length;
                        player = player.Substring(userlenth + 1);
                    }

                    AnalyseDataToArray(player);//解析数据
                    //MessageBox.Show(array[1] + "," + comboBoxYear.Items[m_state[0]].ToString());
                    if (m_state[0] != 0 && array[1] != (comboBoxYear.Items[m_state[0]].ToString()))
                    {
                        state = false; dataset = enumDataset.Next(); continue;
                    }

                    if (Node != null && Convert.ToInt32(Node.Tag) != 0 && comboBoxArea.Text.Trim() != "所有行政区")
                    {
                        int tag = Convert.ToInt32(Node.Tag);
                        strall = comboBoxArea.Text.ToString();
                        string[] area = strall.Split('(', ')');
                        switch (tag)
                        {
                            case 1:
                                if (array[3].Substring(0, 3) != area[1].Substring(0, 3))//省级节点取前三位
                                {
                                    state = false;
                                    dataset = enumDataset.Next();
                                    continue;
                                }
                                break;
                            case 2:
                                if (array[3].Substring(0, 4) != area[1].Substring(0, 4))//市级节点取前四位
                                {
                                    state = false;
                                    dataset = enumDataset.Next();
                                    continue;
                                }
                                break;
                            case 3:
                                if (array[3] != area[1])
                                {
                                    state = false;
                                    dataset = enumDataset.Next();
                                    continue;
                                }
                                break;
                        }

                        //if (!array[3].Contains(area[1]))
                        //{
                        //    state = false; dataset = enumDataset.Next(); continue;
                        //}
                    }
                    if (m_state[2] != 0)
                    {
                        strall = comboBoxScale.Items[m_state[2]].ToString();
                        string[] scale = strall.Split('(', ')');
                        //str_Exp = "select 代码 from 比例尺代码表 where 描述='" + comboBoxScale.Items[m_state[2]].ToString() + "'";
                        //string scale = db.GetInfoFromMdbByExp(strCon, str_Exp);
                        if (array[4] != scale[1])
                        {
                            state = false; dataset = enumDataset.Next(); continue;
                        }
                    }
                    if (m_state[3] != 0)//业务大类
                    {
                        strall = comboBoxBig.Items[m_state[3]].ToString();
                        string[] type = strall.Split('(', ')');
                        //str_Exp = "select 专题类型 from 标准专题信息表 where 描述='" + comboBoxType.Items[m_state[3]].ToString() + "'";
                        //string type= db.GetInfoFromMdbByExp(strCon, str_Exp);
                        if (array[0] != type[1])
                        {
                            state = false; dataset = enumDataset.Next(); continue;
                        }

                    }
                    if (m_state[4] != 0)//业务小类
                    {
                        strall = comboBoxSub.Items[m_state[4]].ToString();
                        string[] type = strall.Split('(', ')');
                        //str_Exp = "select 专题类型 from 标准专题信息表 where 描述='" + comboBoxType.Items[m_state[3]].ToString() + "'";
                        //string type= db.GetInfoFromMdbByExp(strCon, str_Exp);
                        if (array[2] != type[1])
                        {
                            state = false; dataset = enumDataset.Next(); continue;
                        }

                    }
                    str_Exp = "select 描述 from 标准图层信息表 where 代码='" + array[5] + "'";
                    string playername = db.GetInfoFromMdbByExp(strCon, str_Exp);
                    if (playername != "")
                        array[5] = playername;
                    if (state)
                    {
                        datagwSource.Rows.Add(new object[] { true,player, array[5] });
                    }
                    dataset = enumDataset.Next();

                } this.Cursor = Cursors.Default;
            //}
            //else
            //{
            //    this.Cursor = Cursors.Default;
            //    MessageBox.Show("数据源路径不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagwSource.Rows.Count; i++)
            {
                this.datagwSource.Rows[i].Cells[0].Value = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagwSource.Rows.Count; i++)
            {
                if ((bool)datagwSource.Rows[i].Cells[0].EditedFormattedValue == false)
                {
                    this.datagwSource.Rows[i].Cells[0].Value = true;
                    //datagwSource.Rows[i].Selected = true;
                }
                else
                {
                    this.datagwSource.Rows[i].Cells[0].Value = false;
                    //datagwSource.Rows[i].Selected = false;
                }
            }
        }
        //加载行政区
        private void comboBoxArea_Click(object sender, EventArgs e)
        {
            GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            frmDataUnitTree frm = new frmDataUnitTree();//初始化数据单元树窗体
            frm.Location = new Point(this.Location.X +42, this.Location.Y + 140);
            frm.flag = 1;
            frm.ShowDialog();
            if (Node != null)//传回的Node不是NULL
            {
                if (Convert.ToInt32(Node.Tag) != 0)
                {

                    string strExp = "select 行政代码 from 数据单元表 where 行政名称='" + Node.Text + "'  and 数据单元级别='" + Node.Tag + "'";
                    string code = dbfun.GetInfoFromMdbByExp(strCon, strExp);
                    comboBoxArea.Text = Node.Text + "(" + code + ")";//为数据单元box显示数据
                }
                else
                {
                    comboBoxArea.Text = Node.Text; //为数据单元box显示数据
                }
            }

        }

       
    }
}