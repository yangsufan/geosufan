using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Plugin;
using DevComponents.DotNetBar.Controls;

namespace GeoSysSetting.SubControl
{
    public partial class UCLogManager : UserControl
    {
        public UCLogManager()
        {
            InitializeComponent();
            
        }
       
        private void UCLogManager_Load(object sender, EventArgs e)
        {

            ////log = new LogFile(null, m_strLogFilePath);
            //this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 5;
            listView.Items.Clear();
            dateTimePickerStart.Value = DateTime.Today;
            dateTimePickerEnd.Value = DateTime.Today;
            btnSearch_Click(sender, e);
            //List<string> list = log.SeachLog();
            //for (int i = 0; i < list.Count; i++)
            //{
            //    string[] array = list[i].Split('/');
            //    listView.Items.Add(array[0]);
            //    listView.Items[i].SubItems.Add(array[1]);
            //}
            List<string> distinctIP = LogTable.SeachLog2("logIP");
            cBoxAccessIP.Items.Clear();
            cBoxAccessIP.Items.Add("所有IP");
            foreach (string IP in distinctIP)
            {
                cBoxAccessIP.Items.Add(IP);
            }
            if (cBoxAccessIP.Items.Count > 0)
            {
                cBoxAccessIP.SelectedIndex = 0;
            }
            //btnSearch_Click(sender, e);

            List<string> listOper = LogTable.SeachLog2("logEVENT");
            comBoxOperation.Items.Clear();
            comBoxOperation.Items.Clear();
            comBoxOperation.Items.Add("所有操作");
            foreach (string Oper in listOper)
            {
                comBoxOperation.Items.Add(Oper);
            }
            if (comBoxOperation.Items.Count > 0)
            {
                comBoxOperation.SelectedIndex = 0;
            }
            btnSearch_Click(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //log = new LogFile(null, m_strLogFilePath);
            List<string> listUsers=new List<string> ();
            string groupUser = "";
            if(CBGroupUser .Checked ==true &&ComBoxGroupUsers .Text !="")
            {
               listUsers =LogTable.GetUsersByGroupUsers (ComBoxGroupUsers .Text );
               if (listUsers.Count > 0)
               {
                   for (int i = 0; i < listUsers.Count; i++)
                   {
                       groupUser += "'" + listUsers[i] + "',";
                   }
                   groupUser = groupUser.Substring(0, groupUser.Length - 1);
               }
               else
               {
                   MessageBox.Show("该用户组不存在用户！","错误",MessageBoxButtons .OK);
                   return;
               }
            }
            string whereclause = "";
            whereclause += (dateTimePickerStart.Text == "") ? "" : "logTime > '" + Convert.ToDateTime(dateTimePickerStart.Text).ToString("yyyy-MM-dd HH:mm:ss") + "' AND ";
            whereclause += (dateTimePickerEnd.Text == "") ? "" : "logTime < '" + Convert.ToDateTime(dateTimePickerEnd.Text + " 23:59:59").ToString("yyyy-MM-dd HH:mm:ss") + "' AND ";
            whereclause += (CBAllUsers .Checked ==true) ? "" : "";
            whereclause += (CBSingleUser.Checked == true) ? "logUser ='" + cBoxUser.Text + "' AND " : "";
            whereclause += (CBGroupUser.Checked == true) ? "logUser in (" + groupUser + ") AND " : "";
            whereclause += (cBoxAccessIP.Text == "" || cBoxAccessIP.Text == "所有IP") ? "" : "logIP = '" + cBoxAccessIP.Text + "' AND ";
            whereclause += (comBoxOperation.Text == "" || comBoxOperation.Text == "所有操作") ? "" : "logEVENT = '" + comBoxOperation.Text + "' AND ";
            if (whereclause.EndsWith(" AND "))
                whereclause = whereclause.Substring(0, whereclause.Length - 5);
            listView.Items.Clear();
            whereclause = whereclause + " order by logTime desc ";//让日志按照时间顺序 升序排列
            List<string[]> list = LogTable.SeachLog(whereclause);

            if (list.Count == 0)
            {
                MessageBox.Show("没有符合条件的日志", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] strRow = list[i];
                listView.Items.Add(strRow[0]);
                listView.Items[i].SubItems.Add(strRow[1]);
                listView.Items[i].SubItems.Add(strRow[2]);
                listView.Items[i].SubItems.Add(strRow[3]);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //log = new LogFile(null, m_strLogFilePath);
            //log.ClearLog(this.listView);
            if (listView.Items.Count == 0)
                return;
            if (MessageBox.Show("您确定要清空所有日志吗？（清空后不可恢复）", "提示", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information) != DialogResult.OK)
                return;
            LogTable.ClearLog(listView);
            btnSearch_Click(sender, e);
        }
        private void btn_Export_Click(object sender, EventArgs e)
        {
            //log = new LogFile(null, m_strLogFilePath);
            //FolderBrowserDialog dg=new FolderBrowserDialog();
            //if (dg.ShowDialog() == DialogResult.OK)
            //{
            //    log.ExportLog(dg.SelectedPath + "\\log.txt");
            //    Stream pStream=File.op
            //}
            LogTable.ExportLog("日志记录", listView);
        }
        public void RefreshLog()
        {
            //log = new LogFile(null, m_strLogFilePath);
            string whereclause = "";
            whereclause += (dateTimePickerStart.Text == "") ? "" : "logTime > '" + Convert.ToDateTime(dateTimePickerStart.Text).ToString("yyyy-MM-dd HH:mm:ss") + "' AND ";
            whereclause += (dateTimePickerEnd.Text == "") ? "" : "logTime < '" + Convert.ToDateTime(dateTimePickerEnd.Text + " 23:59:59").ToString("yyyy-MM-dd HH:mm:ss") + "' AND ";
            whereclause += (cBoxUser.Text == "所有用户" || cBoxUser.Text == "") ? "" : "logUser = '" + cBoxUser.Text + "' AND ";
            whereclause += (cBoxAccessIP.Text == "" || cBoxAccessIP.Text == "所有IP") ? "" : "logIP = '" + cBoxAccessIP.Text + "'";
            if (whereclause.EndsWith(" AND "))
                whereclause = whereclause.Substring(0, whereclause.Length - 5);
            if(whereclause!="") whereclause = whereclause + " order by logTime desc ";//让日志按照时间顺序 升序排列
            listView.Items.Clear();
            List<string[]> list = LogTable.SeachLog(whereclause);

            if (list.Count == 0)
            {
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] strRow = list[i];
                listView.Items.Add(strRow[0]);
                listView.Items[i].SubItems.Add(strRow[1]);
                listView.Items[i].SubItems.Add(strRow[2]);
                listView.Items[i].SubItems.Add(strRow[3]);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("您确定要删除列表上所选择的日志吗？（删除后不可恢复）", "提示", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Information) != DialogResult.OK)
                return;
            LogTable.DeleteSelectedLog(listView);
            btnSearch_Click(sender, e);
        }
        private void UCLogManager_Resize(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 5;
            listView.Columns[0].Width = listView.Width / 4;
            listView.Columns[1].Width = listView.Width / 4;
            listView.Columns[2].Width = listView.Width / 4;
            listView.Columns[3].Width = listView.Width / 4;
        }
        //针对事件模糊查询
        private void btn_Select_Click(object sender, EventArgs e)
        {
            string whereclause = "";
            whereclause += (txt_select.Text == "") ? "" : "logEVENT like '%" + txt_select.Text + "%' AND ";
            if (whereclause.EndsWith(" AND "))
                whereclause = whereclause.Substring(0, whereclause.Length - 5);
            if (whereclause != "")  whereclause = whereclause + " order by logTime desc ";//让日志按照时间顺序 升序排列
            listView.Items.Clear();
            List<string[]> list = LogTable.SeachLog(whereclause);
            if (list.Count == 0)
            {
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] strRow = list[i];
                listView.Items.Add(strRow[0]);
                listView.Items[i].SubItems.Add(strRow[1]);
                listView.Items[i].SubItems.Add(strRow[2]);
                listView.Items[i].SubItems.Add(strRow[3]);
            }
        }
        private void CBAllUsers_Click(object sender, EventArgs e)
        {

                CBGroupUser.Checked = false;
                CBSingleUser.Checked = false;
                ComBoxGroupUsers.Enabled = false;
                cBoxUser.Enabled = false;
                ComBoxGroupUsers.Text = "";
                cBoxUser.Text = "";
      
        }
        private void CBGroupUser_Click(object sender, EventArgs e)
        {
            CBSingleUser.Checked = false;
            CBAllUsers.Checked = false;
            ComBoxGroupUsers.Enabled = true;
            cBoxUser.Enabled = false;
            cBoxUser.Text = "";
            RefreshComBoxGroupUsers();
        }
        private void CBSingleUser_Click(object sender, EventArgs e)
        {
            CBAllUsers.Checked = false;
            CBGroupUser.Checked = false;
            ComBoxGroupUsers.Enabled = false;
            cBoxUser.Enabled = true;
            ComBoxGroupUsers.Text = "";
            RefreshComBoxSingleUser();
        }
        //刷新用户组下拉框 ygc 2012-9-3
        private void RefreshComBoxGroupUsers()
        {
            List<string> DistinctGroupUsers = LogTable.GetGroupUser();
            ComBoxGroupUsers.Items.Clear();
            foreach (string groupUser in DistinctGroupUsers)
            {
                ComBoxGroupUsers.Items.Add(groupUser);
            }
            if (ComBoxGroupUsers.Items.Count > 0)
            {
                ComBoxGroupUsers.SelectedIndex = 0;
            }
        }
        //刷新单用户下拉框 ygc 2012-9-3
        private void RefreshComBoxSingleUser()
        {
            List<string> distinctUser = LogTable.SeachLog2("logUser");
            cBoxUser.Items.Clear();
            foreach (string user in distinctUser)
            {
                cBoxUser.Items.Add(user);
            }
            if (cBoxUser.Items.Count > 0)
            {
                cBoxUser.SelectedIndex = 0;
            }
        }
    }
}
