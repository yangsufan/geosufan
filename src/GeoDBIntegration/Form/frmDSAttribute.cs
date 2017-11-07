using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Collections;

namespace GeoDBIntegration
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.07.29
    /// 说明：数据源属性窗体
    /// </summary>
    public partial class frmDSAttribute : DevComponents.DotNetBar.Office2007Form
    {
        //public string LabelExpression = "";//设置完毕，传给调用窗体
        //public IList<ILayer> ResLst = null;

        public string[] InList = null;

        public string m_strValue;
        public string HostIP  //ip
        {
            set { txtIP.Text = value; }
        }
        public string DataBase  //数据库
        {
            set { txtDataBase.Text = value; }
        }
        public string m_strIp;  //ip  
        public string m_strDatabase; // 5151
        public string m_strDb;  //数据源 sggt
        public string m_strUser;  //用户名
        public string m_strPassword; //密码
        public string m_strSde;  //sde版本
        /// <summary>
        /// 获取业务库
        /// </summary>
        private IWorkspace m_Workspace = null;
        public frmDSAttribute(string strValue, IWorkspace pWorkspace)
        {
            InitializeComponent();

            m_strValue = strValue;
            string[] info = m_strValue.Split('|');
            this.HostIP = info[0];
            m_strIp = info[0];
            m_strDb = info[1];
            this.DataBase = info[2];
            m_strDatabase = info[2];
            m_strUser = info[3];
            m_strPassword = info[4];
            m_strSde = info[5];
            string[] ds = info[info.Length - 1].Split(',');
            InList = ds;
            m_Workspace = pWorkspace;
           
        }
       


    
       

        private void frmMetaMap_Load(object sender, EventArgs e)
        {
            foreach (string fdName in InList)
            {
                ListViewItem lvi = listViewEx1.Items.Add(fdName);
                //lvi.Tag = fdName;
            }
            listViewEx1.Refresh();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int lviindex = lvi.Index;
                    listViewEx1.Items.Remove(lvi);
                    listViewEx1.Items.Insert(lviindex - 1, lvi);
                }
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnDn_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                if (lvi.Index < listViewEx1.Items.Count-1)
                {
                    int lviindex = lvi.Index;
                    listViewEx1.Items.Remove(lvi);
                    listViewEx1.Items.Insert(lviindex + 1, lvi);
                }
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnTp_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = 0;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnBt_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = listViewEx1.Items.Count - listViewEx1.SelectedItems.Count;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnSA_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                lvi.Checked = true;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnSR_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.Items)
            {
                if(lvi.Checked)
                    lvi.Checked = false;
                else
                    lvi.Checked = true;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //ResLst = new List<ILayer>();
            //foreach (ListViewItem lvi in listViewEx1.Items)
            //{
            //    if(!lvi.Checked)
            //      ResLst.Add(lvi.Tag as ILayer);
            //}


        }

        private void btnCs_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //移除要素集
        private void buttonXDel_Click(object sender, EventArgs e)
        {
            //获取listViewEx1中选中的记录
            string strSelItem = listViewEx1.SelectedItems[0].Text;
           
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                if (lvi.Index >= 0)
                {
                    int lviindex = lvi.Index;
                    listViewEx1.Items.Remove(lvi);
                }
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();

            //删除数据库中对应记录

        }

        //增加要素集
        private void buttonXAdd_Click(object sender, EventArgs e)
        {
            List<string> lstFields = new List<string>();
            List<string> lstNames = new List<string>();
            List<string> InListDb = null;
            //获取数据库连接信息，只显示没有在当前列表中显示的
            GetAllDataSets(m_strIp, m_strDb,m_strDatabase, m_strUser, m_strPassword,m_strSde, "DLG", out InListDb);
            bool bFlag = false;
            if (InListDb == null)
                return;
            foreach (string fdName in InListDb)
            {
                for (int kk = 0; kk < listViewEx1.Items.Count; kk++)
                {
                    bFlag = false;
                    if (fdName.Equals(listViewEx1.Items[kk].Text))
                    {
                        bFlag = true;
                        break;
                    }
                }
                if (bFlag == false)
                {
                    lstFields.Add(fdName);
                    lstNames.Add(fdName);
                }
              
            } 
            
            frmFields frm = new frmFields();
            frm.lstSourceFields = lstFields;
            frm.lstSourceNames = lstNames;

            frm.ShowDialog();
            
            lstFields = frm.lstTagFields;
            for (int ii = 0; ii < lstFields.Count;ii++ )
            {
                bool tag = false;
                for (int j = 0; j < listViewEx1.Items.Count; j++)
                {
                    ListViewItem pTmpItem = listViewEx1.Items[j];
                    if (pTmpItem.Text.Equals(lstFields[ii].Trim()))
                    {
                        tag = true;
                        break;
                    }
                }
                if (!tag)
                {
                    ListViewItem lvi = listViewEx1.Items.Add(lstFields[ii].Trim());
                    ///添加要素就新建一行元数据
                    NewRow(lstFields[ii].Trim());
                }
                
            }
     
            listViewEx1.Refresh();
        }
        private void NewRow(string strFeatureClssName)
        {
            Exception exError = null;
            SysGisTable sysTable = new SysGisTable(m_Workspace);
            try
            {
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                //不存在则添加
                if (!sysTable.ExistData("METADATA_LIB", "数据库名称='" + strFeatureClssName + "'"))
                {
                    dicData.Add("数据库名称", strFeatureClssName);
                    if (sysTable.NewRow("METADATA_LIB", dicData, out exError))
                    {
                        return;
                    }
                }
            }
            catch { }
            finally { sysTable = null; }
        }
        //确定
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            //修改value m_strValue
            string strDbName;
            string strTemp;
            strDbName = listViewEx1.Items[0].Text;
            for (int ii = 1; ii < listViewEx1.Items.Count; ii++)
            {
                strDbName =  strDbName + "," + listViewEx1.Items[ii].Text;
            }

            strTemp = m_strIp + "|" + m_strDb + "|" + m_strDatabase + "|" + m_strUser + "|" + m_strPassword + "|" + m_strSde + "|" + strDbName;
            m_strValue = strTemp;
            this.DialogResult = DialogResult.OK;
        }

        //获取数据集
        public bool GetAllDataSets(string strServer, string strServerName, string strDataBase, string strUser, string strPassWord, string strVersion, string strDataType, out List<string> listDatasets)
        {
            //cyf 20110609 测试数据库连接 包括SDE、PDB、gdb
            SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
            Exception pError = null;
            listDatasets = null;
            pSysDt.SetWorkspace(strServer, strServerName, strDataBase, strUser, strPassWord, strVersion, out pError);

    
            ESRI.ArcGIS.Geodatabase.IWorkspace pWs = pSysDt.WorkSpace;
            if (pWs == null)
                return false;
            //连接成功后，将数据集加载到下拉列表框中
            if (strDataType.Equals("DLG"))
            {
                //框架要素库中数据集名称
                List<string> LstDtName = pSysDt.GetAllFeatureDatasetNames();
                if (LstDtName.Count == 0)
                {
                    //pError = new Exception("该数据库下不存在数据集，请检查！");
                    return false;
                }
                //遍历数据集名称，将数据集加载在下拉列表框中
                if (listDatasets == null)
                {
                    listDatasets = new List<string>();
                }
                foreach (string item in LstDtName)
                {
                    //历史数据集，不添加
                    if (item.ToLower().EndsWith("_GOH"))
                        continue;
                    string GetDataSetName = item;//数据集名称
                    //添加
                    listDatasets.Add(GetDataSetName);

                }

            }
          //  else if (strDataBase.Equals("DOM") || strDataBase.Equals("DEM"))
            else if (strDataType.Equals("DOM") || strDataType.Equals("DEM"))

            {
                IEnumDataset pEnumDataset = null;

                pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterDataset);
                if (pEnumDataset == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }
                pEnumDataset.Reset();
                IDataset pDt = pEnumDataset.Next();
                if (listDatasets == null)
                {
                    listDatasets = new List<string>();
                }
                //遍历栅格数据集
                while (pDt != null)
                {
                    string rasteName = ""; //栅格数据名称
                    rasteName = pDt.Name;
                    //添加
                    listDatasets.Add(rasteName);
                    pDt = pEnumDataset.Next();
                }


                pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                if (pEnumDataset == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }
                pEnumDataset.Reset();
                pDt = pEnumDataset.Next();
                if (pDt == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }

                //遍历栅格编目
                while (pDt != null)
                {
                    string rasteName = ""; //栅格数据名称
                    rasteName = pDt.Name;

                    //added by chulili 20110715过滤掉原本存在的历史库
                    if (!rasteName.ToLower().EndsWith("_GOH"))
                    {
                        //将栅格数据名称添加到数组中
                        listDatasets.Add(rasteName);

                    }
                    pDt = pEnumDataset.Next();
                }

            }
            return true;
        }
        private void FillDataGrid(string strFeatureClassName)
        {
            try
            {
                IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable("METADATA_LIB");
                IQueryFilter pQueryFilter = new QueryFilterClass();
                int pIndex = pTable.FindField("数据库名称");
                IField pField = pTable.Fields.get_Field(pIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeString:
                        pQueryFilter.WhereClause = pField.Name + " = '" + strFeatureClassName + "'";
                        break;
                    default:
                        pQueryFilter.WhereClause = pField.Name + " = " + strFeatureClassName;
                        break;
                }
                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                dataGrid.Rows.Clear();
                ///存储当前元数据数据库名称
                dataGrid.Tag = strFeatureClassName;
                while (pRow != null)
                {

                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        if (pRow.Fields.get_Field(i).Name.ToString() == "OBJECTID") { continue; }
                        dataGrid.Rows.Add(pRow.Fields.get_Field(i).Name, pRow.get_Value(i).ToString());
                    }
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }
            catch { }
        }
        private void listViewEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEx1.SelectedItems.Count == 0) { return; }
            ListViewItem pLsView = listViewEx1.SelectedItems[0];
            string strFeatureClassName = pLsView.Text;
            FillDataGrid(strFeatureClassName);
        }
        /// <summary>
        /// 结束编辑时保存当前编辑值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strFeatureClassName = dataGrid.Tag.ToString();
            try
            {
                IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable("METADATA_LIB");
                IQueryFilter pQueryFilter = new QueryFilterClass();
                int pIndex = pTable.FindField("数据库名称");
                IField pField = pTable.Fields.get_Field(pIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeString:
                        pQueryFilter.WhereClause = pField.Name + " = '" + strFeatureClassName + "'";
                        break;
                    default:
                        pQueryFilter.WhereClause = pField.Name + " = " + strFeatureClassName;
                        break;
                }
                ///获取更新的行
                ICursor pCursor = pTable.Update(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    if (dataGrid.Rows[e.RowIndex].Cells[0].Value != null)
                    {
                        string strFiledName = dataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                        if (strFiledName != "")
                        {
                            string strFiledValue = "";
                            if (dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null) { strFiledValue = dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); }
                            ///更新该行strFiledName 字段值
                            pRow.set_Value(pRow.Fields.FindField(strFiledName), strFiledValue);
                            pRow.Store();
                        }
                    }
                    pRow = pCursor.NextRow();
                }
                pCursor.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }
            catch { }
        }
    }
}
