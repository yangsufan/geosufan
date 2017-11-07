using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FileDBTool
{
    public partial class frmConSet : DevComponents.DotNetBar.Office2007Form
    {
        private string _IP;
        private string _ID;
        private string _Password;
        private string _metaConn;
        public string MetaConn
        {
            get 
            {
                return _metaConn;
            }
            set 
            {
                _metaConn=value;
            }
        }

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public frmConSet()
        {
            InitializeComponent();
            _IP = "";
            _ID = "";
            _Password = "";
            this.Password_tex.PasswordChar = '*';
        }

        public frmConSet(string IP, string ID, string Password, string metaConn)
        {
            InitializeComponent();
            this._IP = IP;
            this._ID = ID;
            this._Password = Password;
            this._metaConn = metaConn;
            this.Password_tex.PasswordChar = '*';
        }

        private void frmConSet_Load(object sender, EventArgs e)
        {
            if ("" != this._IP && null != this._IP)
                this.IP_tex.Text = this.IP;
            else
                this.IP_tex.Text = "127.0.0.1";

            this.ID_tex.Text = this.ID;
            this.Password_tex.Text = this.Password;
            txtDB.Text = _metaConn;
        }

        private void Ok_btn_Click(object sender, EventArgs e)
        {
            Exception eError=null;
            this._ID = this.ID_tex.Text;
            this._IP = this.IP_tex.Text;
            this._Password = this.Password_tex.Text;
            if (this.IP_tex.Text == "" || this.ID_tex.Text == "" || this.Password_tex.Text==""||txtDB.Text=="")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接信息填写不完整！");
                return;
            }
            ModDBOperator.ConnectDB(txtDB.Text.Trim(), this.IP_tex.Text, this.ID_tex.Text, this.Password_tex.Text, out eError);
            if(eError!=null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
            }
           
            this.Close();
        }

        private void Cannel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDBSel_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择PDB数据";
            OpenFile.Filter = "PDB数据(*.mdb)|*.mdb";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtDB.Text = OpenFile.FileName;
            }
        }
    }
}