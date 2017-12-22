using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Fan.Common;
using System.Collections;
using Fan.DataBase.Module;

namespace GDBM
{
    public partial class frmLogin :Fan.Common.BaseForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                MessageBox.Show("请输入用户名!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxXPassword.Text.Trim()))
            {
                MessageBox.Show("请填写密码!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Mod.m_LoginUser = new User(txtUser.Text,textBoxXPassword.Text,Mod.m_SysDbOperate);
            string checkStr = Mod.m_LoginUser.CheckLogin();
            if (!string.IsNullOrEmpty(checkStr))
            {
                MessageBox.Show(checkStr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void frmLogin_Activated(object sender, EventArgs e)
        {
            if (this.txtUser.Text != "")
            {
                this.textBoxXPassword.Focus();
            }
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 0x01;
            const int HTCAPTION = 0x02;
            const int WM_NCLBUTTONDBLCLK = 0xA3;
            switch (m.Msg)
            {
                case 0x4e:
                case 0xd:
                case 0xe:
                case 0x14:
                    base.WndProc(ref m);
                    break;
                case WM_NCHITTEST://鼠标点任意位置后可以拖动窗体
                    this.DefWndProc(ref m);
                    if (m.Result.ToInt32() == HTCLIENT)
                    {
                        m.Result = new IntPtr(HTCAPTION);
                        return;
                    }
                    break;
                case WM_NCLBUTTONDBLCLK://禁止双击最大化
                    Console.WriteLine(this.WindowState);
                    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}