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

namespace GeoDataCenterFunLib
{
    public partial class frmRasterDataReduction : DevComponents.DotNetBar.Office2007Form
    {
        public frmRasterDataReduction()
        {
            InitializeComponent();
        }

        private void frmRasterDataReduction_Load(object sender, EventArgs e)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在加载数据");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
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
            vProgress.Close();
            this.Activate();
        }

        /// <summary>
        /// 得到数据源地址
        /// </summary>
        /// <param name="str">数据源名称</param>
        /// <returns></returns>
        private string GetSourcePath(string str)
        {
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 数据库 from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                return strname;
            }
            catch { return ""; }
        }

        //加载列表
        private void LoadDataGridView()
        {
            if (comboBoxSource.Text.Trim() != ""&&comboBoxCatalog.Text!="")
            {
                dataGridView.Rows.Clear();
                //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                //IWorkspace pWorkspace = (IWorkspace)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
                if (pWorkspace == null)
                {
                    MessageBox.Show("数据源空间不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog) as IEnumDataset;
                IDataset dataset = enumDataset.Next();
                while (dataset!= null)
                {
                    if (dataset.Name.Trim() == comboBoxCatalog.Text.Trim())
                    {
                        ITable pTable = dataset as ITable;
                        //IRasterCatalog irastercatalog = (IRasterCatalog)dataset;
                       
                        ICursor pCursor = pTable.Search(null, false);
                        IRow pRow = pCursor.NextRow();
                        while (pRow != null)
                        {
                            dataGridView.Rows.Add(new object[] { true, pRow.get_Value(pRow.Fields.FindField("Name")) });
                            pRow = pCursor.NextRow();
                            
                        }
                        return;
                    }
                    dataset = enumDataset.Next();
                }
            }
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
        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {

            comboBoxCatalog.Items.Clear();
            comboBoxCatalog.Text = "";
            dataGridView.Rows.Clear();
            if (comboBoxSource.Text.Trim() != "")
            { 
               //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
               // IWorkspace pWorkspace = (IWorkspace)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
                IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
                if (pWorkspace == null)
                {
                    MessageBox.Show("数据源空间不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog) as IEnumDataset;
                IDataset dataset = enumDataset.Next();
                while (dataset != null)
                {
                    comboBoxCatalog.Items.Add(dataset.Name);
                    dataset = enumDataset.Next();
                }
                if (comboBoxCatalog.Items.Count > 0)
                {
                    comboBoxCatalog.SelectedIndex = 0;
                }
            }
            
        }
        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                this.dataGridView.Rows[i].Cells[0].Value = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if ((bool)dataGridView.Rows[i].Cells[0].EditedFormattedValue == false)
                {
                    this.dataGridView.Rows[i].Cells[0].Value = true;
                    //dataGridView.Rows[i].Selected = true;
                }
                else
                {
                    this.dataGridView.Rows[i].Cells[0].Value = false;
                    //dataGridView.Rows[i].Selected = false;
                }
            }
        }

        //退出
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
              bool flag = false;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue == true)
                {
                    flag = true;
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
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if ((bool)row.Cells[0].EditedFormattedValue == true)
                    {
                        string cellvalue = row.Cells[1].Value.ToString().Trim();
                        IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
                        if (pWorkspace == null)
                        {
                            MessageBox.Show("数据源空间不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        IWorkspace2 pWorkspace2 = (IWorkspace2)pWorkspace;
                       // if (pWorkspace2.get_NameExists(esriDatasetType.esriDTRasterDataset, cellvalue))
                        {
                            IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog) as IEnumDataset;
                            IDataset dataset = enumDataset.Next();
                            while (dataset != null)
                            {
                                if (dataset.Name.Trim() == comboBoxCatalog.Text.Trim())
                                {
                                    ITable pTable = dataset as ITable;
                                    //IRasterCatalog irastercatalog = (IRasterCatalog)dataset;

                                    ICursor pCursor = pTable.Search(null, false);
                                    IRow pRow = pCursor.NextRow();
                                    while (pRow != null)
                                    {
                                        if (pRow.get_Value(pRow.Fields.FindField("Name")).ToString() == cellvalue)
                                        {
                                            this.Cursor = Cursors.WaitCursor;
                                            pRow.Delete();//如果相等则删除
                                            this.Cursor = Cursors.Default;
                                        }
                                        pRow = pCursor.NextRow();
                                    }
                                }
                                dataset = enumDataset.Next();
                            }
                        }
                    }
                }
                dataGridView.Rows.Clear();
                LoadDataGridView();//重新加载数据
                    MessageBox.Show("删除数据成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void comboBoxCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataGridView();
        }
    }
}