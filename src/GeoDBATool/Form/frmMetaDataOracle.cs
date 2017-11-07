using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.OracleClient;


using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
//ZQ 20110815     mdb数据入库到Oracle
namespace GeoDBATool
{
    public partial class frmMetaDataOracle : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace m_WS = null;        //系统维护库连接工作空间
        private string m_OracleConnection = ""; 
        public frmMetaDataOracle(IWorkspace pWs)
        {
            InitializeComponent();
            m_WS = pWs;
        }
        private void frmMetaDataOracle_Load(object sender, EventArgs e)
        {

            if (Plugin.Mod.Server == "" || Plugin.Mod.User == "" || Plugin.Mod.Password == "" || Plugin.Mod.Instance == "")
            {
                m_OracleConnection = "";
                return;
            }
            m_OracleConnection = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + Plugin.Mod.Database + ")));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
        }
        #region  公共函数
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private string  GetOracleConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return "";
            }
            if (type < 0)
            {
                return "";
            }
            //end added by chulili 20111109
            string OracleConnin = "";
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 3://sde
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
                default:
                    break;
            }
            if (sDatabase == "" || sUser == "" || sPassword == "")
            {
                OracleConnin = "";
            }
            else
            {
                OracleConnin = "Data Source=" + sDatabase + ";User Id=" + sUser
                          + ";Password=" + sPassword + ";";
            }
            return OracleConnin;
        }
        //用来判断oracle数据库中是否存在strTableName表
        private bool GetBySameName(string strTableName, OracleConnection pOracleConnection)
        {
            bool bIsYN = false;
            DataTable dt = null;
            try
            {
                if (pOracleConnection.State == ConnectionState.Closed)
                {
                    pOracleConnection.Open();
                }

                OracleDataAdapter da = new OracleDataAdapter("select Table_Name from  user_tables", pOracleConnection);

                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == strTableName.ToUpper())
                    {
                        bIsYN = true;
                        break;
                    }
                }
                dt = null;
                da = null;
            }
            catch
            {
                bIsYN = false;
            }

            return bIsYN;
        }
        /// <summary>
        /// 部分Oracle数据库与.net数据类型转换
        /// </summary>
        /// <param name="drow"></param>
        /// <returns></returns>
        private string NetChangOracleType(System.Data.DataRow drow)
        {
            String FieldAttr = String.Empty;
            String coltype = drow["DataType"].ToString();
            if (coltype == "System.Double")
                FieldAttr = " FLOAT";
            else if (coltype == "System.Byte" || coltype == "System.SByte")
                FieldAttr = " BLOB";
            else if (coltype == "System.DateTime")
                FieldAttr = " DATE";
            else if (coltype == "System.Int16" || coltype == "System.Int32" || coltype == "System.Int64" || coltype == "System.Char" || coltype == "System.UInt16"
                || coltype == "System.UInt32" || coltype == "System.UInt64")
                FieldAttr = " INTEGER";
            else if (coltype == "System.Decimal")
            {
                FieldAttr = "  NUMBER";
            }
            else if (coltype == "System.Numeric")
            {
                object testb = drow["NumericScale"];
                if (testb != Convert.DBNull && testb != null && Convert.ToInt32(testb) > 0)
                    FieldAttr = " NUMBER";
                else
                    FieldAttr = " long";
            }
            else if (coltype == "System.Single")
                FieldAttr = " NUMBER";
            else if (coltype == "System.String")
            {
                if (Convert.ToInt32(drow["ColumnSize"]) > 255)
                    FieldAttr = " CLOB";
                else
                    FieldAttr = " NVARCHAR2(2000)";
            }
            else
            {
                FieldAttr = " NVARCHAR2(2000)";
            }
            return FieldAttr;
        }

        #endregion
        #region    元数据入库设置
        private void bttOpenFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = true;
            sOpenFileD.Title = "选择元数据";
            sOpenFileD.Filter = "元数据库2000(*.mdb)|*.mdb|元数据库2007(*accdb)|*.accdb";
            int m = 1;
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (sOpenFileD.FileName.ToString() == checkedMDData.Items[i].ToString())
                    {
                        if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            checkedMDData.Items.Add(sOpenFileD.FileName, true);
                            return;
                        }
                        else
                        {
                            m = 0;
                            break;
                        }
                    }

                }
                if (m == 1)
                {
                    checkedMDData.Items.Add(sOpenFileD.FileName, true);
                }
                //this.clbTable.Items.Add(System.IO.Path.GetFileNameWithoutExtension(sOpenFileD.FileName));

            }
            if (checkedMDData.Items.Count > 0)
            {
              bttRemove.Enabled = true;
             bttAllRemove.Enabled = true;
            }
        }

        private void bttOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            int m = 1;
            if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] pFilePathArr = null;
                pFilePathArr = Directory.GetFiles(pFolderBrowserDialog.SelectedPath, "*.mdb", SearchOption.TopDirectoryOnly);
                for (int j = 0; j < pFilePathArr.Length; j++)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (pFilePathArr[j].ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                                return;
                            }
                            else
                            {
                                m = 0;
                                break;

                            }

                        }
                    }
                    if (m == 1)
                    {
                        checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                    }
                }
                pFilePathArr = Directory.GetFiles(pFolderBrowserDialog.SelectedPath, "*.accdb", SearchOption.TopDirectoryOnly);
                m = 1;
                for (int j = 0; j < pFilePathArr.Length; j++)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (pFilePathArr[j].ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                                return;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    if (m == 1)
                    {
                        checkedMDData.Items.Add(pFilePathArr[j].ToString(), true);
                    }
                }


            }
            if (checkedMDData.Items.Count > 0)
            {
                bttRemove.Enabled = true;
                bttAllRemove.Enabled = true;
            }
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    this.checkedMDData.SetItemChecked(i, true);
                }
            }
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (this.checkedMDData.GetItemChecked(i))
                    {
                        this.checkedMDData.SetItemChecked(i, false);
                    }
                    else
                    {
                        this.checkedMDData.SetItemChecked(i, true);
                    }
                }
            }
        }


        private void bttRemove_Click(object sender, EventArgs e)
        {
            int Index = this.checkedMDData.SelectedIndex;
            if (Index != -1)
            {
                checkedMDData.Items.Remove(checkedMDData.Items[Index].ToString());
            }
            if (checkedMDData.Items.Count == 0)
            {
                bttRemove.Enabled = false;
                bttAllRemove.Enabled = false;
            }
        }

        private void bttAllRemove_Click(object sender, EventArgs e)
        {
            checkedMDData.Items.Clear();
            bttRemove.Enabled = false;
            bttAllRemove.Enabled = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region  元数据入库

        private void btnOK_Click(object sender, EventArgs e)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.EnableCancel = false;//设置进度条
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            try
            {
                if (m_OracleConnection == "")
                {
                    MessageBox.Show("连接信缺失！","提示！");
                    return;
                }
             
                for (int i = 0; i < this.checkedMDData.Items.Count; i++)//遍历用户添加的源数据库
                {
                    if (this.checkedMDData.GetItemChecked(i) == true)//用户勾选的默认为要入库的数据
                    {
                        
                        string strPath = checkedMDData.Items[i].ToString();
                        OleDbConnection m_vConnSor = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + strPath + "'");
                        OleDbCommand vCommand = new OleDbCommand();
                        m_vConnSor.Open();
                        vProgress.ShowProgress();
                        vProgress.SetProgress("正在读取"+strPath+"数据库信息");
                        //获取源数据库中所有表的信息
                        DataTable schemaTable = m_vConnSor.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        for (int k = 0; k < schemaTable.Rows.Count; k++)
                        {
                            //获取表名
                           flog:
                            string strTableName = schemaTable.Rows[k][2].ToString();   
                            OracleConnection pOracleConnection = new OracleConnection(m_OracleConnection);
                            if (pOracleConnection.State == ConnectionState.Closed)
                            {
                                pOracleConnection.Open();
                            }
                            OracleCommand pOracleCommand = pOracleConnection.CreateCommand();
                            //判断oracle数据库中是否存在该表
                            if (GetBySameName(strTableName, pOracleConnection))
                            {
                                if (m_vConnSor.State == ConnectionState.Closed)
                                {
                                    m_vConnSor.Open();
                                }
                                pOracleCommand.CommandText = "select * from " + strTableName;
                                OracleDataAdapter pOracleDataAdapter = new OracleDataAdapter(pOracleCommand.CommandText, pOracleConnection);
                                OracleCommandBuilder pOracleCommandBuilder = new OracleCommandBuilder(pOracleDataAdapter);
                                DataSet pDataSet =new DataSet();
                                pDataSet.Tables.Add(strTableName);
                                pOracleDataAdapter.Fill(pDataSet, strTableName);
                                DataTable pOrDataTable = pDataSet.Tables[strTableName];
                                //获取当前获得的源数据库表的信息
                                vCommand = new OleDbCommand("select * from " + strTableName, m_vConnSor);
                                OleDbDataReader vDataReaderY = vCommand.ExecuteReader();
                                vProgress.SetProgress(strTableName + "表正在入库");
                                while (vDataReaderY.Read())
                                {
                                    //获取源数据库表的架构信息
                                    DataTable pDataTableY = vDataReaderY.GetSchemaTable();
                                    //在oracle数据库新建的表中新建一行来存储读到源数据库表的一条记录
                                    DataRow pDataRow = pOrDataTable.NewRow();
                                    for (int m = 0; m < pDataTableY.Rows.Count; m++)
                                    {
                                        DataRow drowY = pDataTableY.Rows[m];
                                        switch (drowY["DataType"].ToString())
                                         {
                                            case"System.DateTime":
                                                 if (vDataReaderY[drowY["ColumnName"].ToString()].ToString() != "")
                                                 {
                                                     pDataRow[drowY["ColumnName"].ToString()] =Convert.ToDateTime(vDataReaderY[drowY["ColumnName"].ToString()].ToString());
                                                 }
                                                 break;
                                            case "System.Double":
                                            case "System.Single":
                                                 if (vDataReaderY[drowY["ColumnName"].ToString()].ToString() != "")
                                                 {
                                                     pDataRow[drowY["ColumnName"].ToString()] = (Convert.ToDouble(vDataReaderY[drowY["ColumnName"].ToString()]).ToString());
                                                 }
                                                 break;
                                            default:
                                                 pDataRow[drowY["ColumnName"].ToString()] = vDataReaderY[drowY["ColumnName"].ToString()].ToString();
                                                 break;

                                         }
                                    }
                                    //提交新建行存储的信息
                                    pOrDataTable.Rows.Add(pDataRow);
                                    pOracleDataAdapter.Update(pDataSet, strTableName);
                                    pDataSet.AcceptChanges();
                                }
                                vDataReaderY.Close();
                                pOracleDataAdapter = null;
                                pOracleCommandBuilder = null;
                                pDataSet = null;
                                pOrDataTable = null;
                            }
                            else//当oracle中不存在表时，新建表及其结构
                            {
                                vProgress.SetProgress("正在数据库中创建"+strTableName+"表结构");
                                pOracleCommand.CommandText = "create table " + strTableName + "(id number)";
                                if (pOracleConnection.State == ConnectionState.Closed)
                                {
                                    pOracleConnection.Open();
                                }
                                pOracleCommand.ExecuteNonQuery();
                                if (m_vConnSor.State == ConnectionState.Closed)
                                {
                                    m_vConnSor.Open();
                                }
                                vCommand = new OleDbCommand("select * from " + strTableName, m_vConnSor);
                                OleDbDataReader vDataReader = vCommand.ExecuteReader();
                                DataTable pDataTable = null;
                                while (vDataReader.Read())
                                {
                                    pDataTable = vDataReader.GetSchemaTable();
                                    for (int m = 0; m < pDataTable.Rows.Count; m++)
                                    {
                                        DataRow drow = pDataTable.Rows[m];
                                        if (m == 0)
                                        {
                                            pOracleCommand.CommandText = "create table " + strTableName + "(" + drow["ColumnName"].ToString() + " " + NetChangOracleType(drow).ToString() + ")";
                                            if (pOracleConnection.State == ConnectionState.Closed)
                                            {
                                                pOracleConnection.Open();
                                            }
                                            try
                                            {
                                                pOracleCommand.ExecuteNonQuery();
                                            }
                                            catch { MessageBox.Show(strTableName + "表新建失败！", "提示！"); }
                                        }
                                        else
                                        {
                                            pOracleCommand.CommandText = "alter table " + strTableName + " add " + drow["ColumnName"].ToString() + NetChangOracleType(drow).ToString();
                                            try
                                            {
                                                pOracleCommand.ExecuteNonQuery();
                                            }
                                            catch { MessageBox.Show(drow["ColumnName"].ToString() + "列新建失败！", "提示！"); }
                                        }
                                    }
                                    vDataReader.Close();
                                    break;
                                }
                                //当新建表完成时返回到开始进入的状态进行执行
                                goto flog;
                            }
                            if (m_vConnSor.State == ConnectionState.Open)
                            {
                                m_vConnSor.Close();
                            }
                            if (pOracleConnection.State == ConnectionState.Open)
                            {
                                pOracleConnection.Close();
                            }
                        }
                    }
                }
                vProgress.SetProgress("完成入库操作！");
                vProgress.Close();
            }
            catch(Exception ex)
            {
                vProgress.Close();
                MessageBox.Show(ex.ToString ()+"导致入库操作失败！", "提示！");
            }
        }
        #endregion
        
       
 


    }
}
