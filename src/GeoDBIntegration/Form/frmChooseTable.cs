using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.OracleClient;


namespace GeoDBIntegration
{
    public partial class frmChooseTable : DevComponents.DotNetBar.Office2007Form
    {

        private Dictionary<string, string> m_TableDic;////////记录需要建立的表的名称和SQL语句
        private Dictionary<string, string> m_CreatedTable;////记录已创建的表名和SQL语句
        public Dictionary<string, string> CreatedTable
        {
            get { return this.m_CreatedTable; }
        }
        private string m_Server;
        private string m_User;
        private string m_Password;
        public frmChooseTable(Dictionary<string ,string >TableDic,string sOracleServer,string sUser,string sPassWord)
        {
            InitializeComponent();
            if (TableDic == null) return;
            this.m_TableDic = TableDic;
            this.m_Server = sOracleServer;
            this.m_User = sUser;
            this.m_Password = sPassWord;
            m_CreatedTable = new Dictionary<string, string>();
            /////////将配置方案获取到的的表显示在表格中
            this.list_Table.Items.Clear();
            foreach (KeyValuePair<string, string> item in TableDic)
            {
                string sTableName = item.Key;
                string sTableSQL = item.Value;
                this.list_Table.Items.Add(sTableName, false);               
            }
                        
        }   
        private void btnOk_Click(object sender, EventArgs e)
        {
              ///////执行创建库体
            OracleConnectionStringBuilder Connectstrbuilder = new OracleConnectionStringBuilder();
            Connectstrbuilder.DataSource = this.m_Server;
            Connectstrbuilder.UserID = this.m_User;
            Connectstrbuilder.Password= this.m_Password;
            Connectstrbuilder.Unicode = true;
            Connectstrbuilder.PersistSecurityInfo = true;
            ////////获取表名和建库的SQL语句进行建库
            OracleConnection Con = new OracleConnection(Connectstrbuilder.ConnectionString);
            OracleCommand Com = null;
            OracleTransaction Tra = null;
            FrmProcessBar ProcBar = new FrmProcessBar(this.list_Table.Items.Count);
            ProcBar.SetFrmProcessBarText("正在创建表");
            Application.DoEvents();
            //////开始创建///////
            try
            {
                Con.Open();////////////////////打开连接
                Tra=Con.BeginTransaction();////事务对象
            }
            catch(Exception eError)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "打开数据库失败！\n原因：" + eError.Message);
               // throw new Exception("打开数据库失败！\n原因：" + eError.Message);
                ProcBar.Close();
                return;
            }
            
            for (int i = 0; i < this.list_Table.Items.Count; i++)
            {
                ProcBar.SetFrmProcessBarValue((long)i);
                Application.DoEvents();
                if (this.list_Table.GetItemChecked(i))
                {
                    string sTableName = this.list_Table.Items[i].ToString().Trim();
                    string sSQL = string.Empty;
                    if (this.m_TableDic.ContainsKey(sTableName))
                    {
                        /////获取建表用的SQL语句
                        bool bGet = this.m_TableDic.TryGetValue(sTableName, out sSQL);
                        if (bGet)
                        {
                            ProcBar.SetFrmProcessBarText("正在创建表：" + sTableName);
                            Application.DoEvents();
                            try
                            {
                                Com = new OracleCommand(sSQL, Con);
                                Com.Transaction = Tra;
                                Com.ExecuteNonQuery();
                                if (!m_CreatedTable.ContainsKey(sTableName))
                                    m_CreatedTable.Add(sTableName, sSQL);
                            }
                            catch(Exception eError)
                            {
                                Tra.Rollback();
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建表：" + sTableName + "失败！\n原因：" + eError.Message);
                               // throw new Exception("创建表：" + sTableName + "失败！\n原因：" + eError.Message);
                                ProcBar.Close();                               
                                if (Con.State == ConnectionState.Open) Con.Close();
                                return;
                            }
                        }
                    }
                }
            }
            ///////创建完成，关闭连接，提交事务
            try
            {
                Tra.Commit();
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "库体创建完成" );
                ProcBar.Close();
            }
            catch (Exception eError)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "库体创建失败！\n原因：" + eError.Message);
                //throw new Exception("库体创建失败！\n原因：" + eError.Message);
                ProcBar.Close();
                return;
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
           
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        /// <summary>
        /// 选择全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.list_Table.Items.Count; i++)
            {
                this.list_Table.SetItemChecked(i, true);
            }
        }
        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectRev_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.list_Table.Items.Count; i++)
            {
                if (this.list_Table.GetItemChecked(i))
                    this.list_Table.SetItemChecked(i, false);
                else
                    this.list_Table.SetItemChecked(i, true);
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.list_Table.Items.Count; i++)
            {
                this.list_Table.SetItemChecked(i, false);
            }
        }


    }
}