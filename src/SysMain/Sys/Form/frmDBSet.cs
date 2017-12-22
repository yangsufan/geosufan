using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using Fan.Common;
using Fan.Common.Gis;
using Fan.Common.Error;
using ESRI.ArcGIS.Geodatabase;
using Fan.DataBase;

namespace GDBM
{
    public partial class frmDBSet :Fan.Common.BaseForm
    {
        public frmDBSet()
        {
            InitializeComponent();
        }
        private DBConfig m_dbConfig = new DBConfig();
        private DBOperatorType m_DBoType = DBOperatorType.UnKnowOperator;
        private DBType m_dbType = DBType.DEFAULT;
        private Dictionary<string, string> dicDbconfig = new Dictionary<string, string>()
        {
            ["server"] = string.Empty,
            ["serverice"] = string.Empty,
            ["database"] = string.Empty,
            ["user"] = string.Empty,
            ["password"] = string.Empty,
            ["version"] = string.Empty,
            ["serverport"] = string.Empty
        };
        private Dictionary<string, string> dicDbOpType = new Dictionary<string, string>() {
            ["ODBC"] ="ODBC",
            ["EsriOperator"] ="Esri DataBase Operator"
        };
        private Dictionary<string, string> dicODBCDbType = new Dictionary<string, string>()
        {
            ["ODBCMDB"]="Access DataBase",
            ["ODBCSQL"]="SQL Server",
            ["ODBCORACLE"] ="Oracle",
            ["ODBCPOST"]="Postgresql"
        };
        private Dictionary<string, string> dicEsriDbType = new Dictionary<string, string>()
        {
            ["ESRISDEOracle"] = "ArcSDE(For Oracle)",
            ["ESRISDESqlServer"]="ArcSDE(For SQL Server)",
            ["ESRIPDB"]= "Prersonal Geodatabase",
            ["ESRIGDB"] ="File Geodatabase"
        };
        private IDBOperate m_DbOp = null;
        public IDBOperate Dbop
        {
            get { return m_DbOp; }
        }
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            switch (m_DBoType)
            {
                case DBOperatorType.EsriOperator:
                    dicDbconfig["server"] = txtEsriServer.Text;
                    dicDbconfig["serverice"] = txtService.Text;
                    dicDbconfig["database"] = txtEsriDb.Text;
                    dicDbconfig["user"] = txtEsriUser.Text;
                    dicDbconfig["password"] = txtEsriPassword.Text;
                    break;
                case DBOperatorType.ODBC:
                    dicDbconfig["server"] = txtODBCServer.Text;
                    dicDbconfig["database"] = txtODBCDb.Text;
                    dicDbconfig["user"] = txtODBCUser.Text;
                    dicDbconfig["password"] = txtODBCPassword.Text;
                    dicDbconfig["serverport"] = txtODBCPort.Text;
                    break;
            }
            m_dbConfig.SetConfig(m_DBoType, m_dbType, dicDbconfig);
            DBOperatorFactory pFac = new DBOperatorFactory(m_dbConfig);
            m_DbOp = pFac.GetDbOperate();
            if (m_DbOp != null)
            {
                if (!m_DbOp.TestConnect())
                {
                    MessageBox.Show("无法连接数据库，请检查设置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    m_dbConfig.SaveConfig(Fan.Common.ModuleConfig.m_ConnectFileName);
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("无法连接数据库，请检查设置！","提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void cbSelectDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectStr = getKeyFromValue(dicODBCDbType, cbSelectDbType.SelectedItem.ToString());
            Enum.TryParse<DBType>(selectStr, out m_dbType);
            switch (m_dbType)
            {
                case DBType.ODBCMDB:
                    txtODBCDb.Width = 406;
                    btnSelectDB.Visible = true;
                    txtODBCServer.Enabled = false;
                    txtODBCPort.Enabled = false;
                    txtODBCUser.Enabled = false;
                    break;
                case DBType.ODBCORACLE:
                case DBType.ODBCPOST:
                case DBType.ODBCSQL:
                    txtODBCPort.Width = 455;
                    btnSelectDB.Visible = false;
                    txtODBCPort.Enabled = true;
                    txtODBCServer.Enabled = true;
                    txtODBCUser.Enabled = true;
                    break;
                    
            }
        }

        private void cbEsriDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectStr = getKeyFromValue(dicEsriDbType, cbEsriDbType.SelectedItem.ToString());
            Enum.TryParse<DBType>(selectStr, out m_dbType);
            switch (m_dbType)
            {
                case DBType.ESRIGDB:
                case DBType.ESRIPDB:
                    txtEsriDb.Width = 406;
                    btnSelectEsriDb.Visible = true;
                    txtEsriPassword.Enabled = false;
                    txtEsriServer.Enabled = false;
                    txtEsriUser.Enabled = false;
                    txtEsriVersion.Enabled = false;
                    txtService.Enabled = false;
                    break;
                case DBType.ESRISDEOracle:
                case DBType.ESRISDESqlServer:
                    txtEsriDb.Width = 455;
                    btnSelectEsriDb.Visible = false;
                    txtEsriPassword.Enabled = true;
                    txtEsriServer.Enabled = true;
                    txtEsriUser.Enabled = true;
                    txtEsriVersion.Enabled = true;
                    txtService.Enabled = true;
                    break;
            }
        }

        private void cbSelectDbOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selctText = getKeyFromValue(dicDbOpType, cbSelectDbOpType.SelectedItem.ToString());
            Enum.TryParse<DBOperatorType>(selctText, out m_DBoType);
            switch (m_DBoType)
            {
                case DBOperatorType.EsriOperator:
                    gpODBCConnect.Visible = false;
                    gpEsri.Visible = true;
                    gpEsri.Location = new Point(7, 115);
                    cbEsriDbType.Properties.Items.Clear();
                    cbEsriDbType.Properties.Items.AddRange(dicEsriDbType.Values);
                    cbEsriDbType.SelectedIndex = 0;
                    break;
                case DBOperatorType.ODBC:
                    gpEsri.Visible = false;
                    gpODBCConnect.Visible = true;
                    cbSelectDbType.Properties.Items.Clear();
                    cbSelectDbType.Properties.Items.AddRange(dicODBCDbType.Values);
                    cbSelectDbType.SelectedIndex = 0;
                    break;
            }
        }
        private void frmDBSet_Load(object sender, EventArgs e)
        {
            cbSelectDbOpType.Properties.Items.Clear();
            cbSelectDbOpType.Properties.Items.AddRange(dicDbOpType.Values);
            cbSelectDbOpType.SelectedIndex = 0;
        }
        private string getKeyFromValue(Dictionary<string, string> dic,string vaule)
        {
            foreach (string key in dic.Keys)
            {
                if (dic[key] == vaule)
                {
                    return key;
                }
            }
            return string.Empty;
        }

        private void btnSelectDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.mdb|*.mdb";
            openFile.Multiselect = false;
            if (openFile.ShowDialog() != DialogResult.OK) return;
            txtODBCDb.Text = openFile.FileName;
        }

        private void btnSelectEsriDb_Click(object sender, EventArgs e)
        {
            if (m_dbType == DBType.ESRIPDB)
            {
                OpenFileDialog openfile = new OpenFileDialog();
                openfile.Filter = "*.mdb|*.mdb";
                openfile.Multiselect = false;
                if (openfile.ShowDialog() != DialogResult.OK) return;
                txtEsriDb.Text = openfile.FileName;
            }
            else
            {
                FolderBrowserDialog openFolder = new FolderBrowserDialog();
                openFolder.ShowNewFolderButton = false;
                if (openFolder.ShowDialog() != DialogResult.OK) return;
                string selectFolder = openFolder.SelectedPath;
                if (!selectFolder.EndsWith("gdb"))
                {
                    MessageBox.Show("请选择正确的GDB路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtEsriDb.Text = selectFolder;
            }
        }
    }
}