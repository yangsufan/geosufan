using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Word;

//*********************************************************************************
//** 文件名：frmDetailLog.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-11
//** 修改人：yjl
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDataCenterFunLib
{  
    public partial class frmDetailLog : DevComponents.DotNetBar.Office2007Form
    {
        public string m_strLogFilePath;
        public frmDetailLog(string strFilePath)
        {
            InitializeComponent();
            m_strLogFilePath = strFilePath;
        }
        LogFile log;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //log = new LogFile(null, m_strLogFilePath);
            string whereclause="";
            whereclause+=(dateTimePickerStart.Text =="")?"":"logTime > '"+Convert.ToDateTime(dateTimePickerStart.Text).ToString("yyyy-MM-dd HH:mm:ss")+"' AND "; 
            whereclause+=(dateTimePickerEnd.Text =="")?"":"logTime < '"+Convert.ToDateTime(dateTimePickerEnd.Text+" 23:59:59").ToString("yyyy-MM-dd HH:mm:ss")+"' AND "; 
            whereclause+=(cBoxUser.Text =="")?"":"logUser = '"+cBoxUser.Text+"' AND "; 
            whereclause+=(cBoxAccessIP.Text=="")?"":"logIP = '"+cBoxAccessIP.Text+"'";
            if(whereclause.EndsWith(" AND "))
                whereclause=whereclause.Substring(0,whereclause.Length-5);
            listView.Items.Clear();
            List<string[]> list = LogTable.SeachLog(whereclause);
           
            if (list.Count == 0)
            {
                MessageBox.Show("没有符合条件的日志", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            for(int i=0;i<list.Count;i++)
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
            if( MessageBox.Show("您确定要清空列表上所有日志吗？（清空后不可恢复）", "提示", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information)!=DialogResult.OK)
                return;
            LogTable.ClearLog(listView);
            btnSearch_Click(sender,e);
        }

        private void frmDetailLog_Load(object sender, EventArgs e)
        {
            ////log = new LogFile(null, m_strLogFilePath);
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
            List<string> distinctUser = LogTable.SeachLog2("logUser");
            cBoxUser.Items.Clear();
            foreach (string user in distinctUser)
            {
                cBoxUser.Items.Add(user);
            }
            List<string> distinctIP = LogTable.SeachLog2("logIP");
            cBoxAccessIP.Items.Clear();
            foreach (string IP in distinctIP)
            {
                cBoxAccessIP.Items.Add(IP);
            }
           
        }

        //导出
        private void btn_Export_Click(object sender, EventArgs e)
        {
            //log = new LogFile(null, m_strLogFilePath);
            //FolderBrowserDialog dg=new FolderBrowserDialog();
            //if (dg.ShowDialog() == DialogResult.OK)
            //{
            //    log.ExportLog(dg.SelectedPath + "\\log.txt");
            //    Stream pStream=File.op
            //}
            LogTable.ExportLog("日志记录",listView);
        }
        //删除选择日志
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("您确定要删除列表上所选择的日志吗？（删除后不可恢复）", "提示", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Information) != DialogResult.OK)
                return;
            LogTable.DeleteSelectedLog(listView);
            btnSearch_Click(sender,e);
        }

      
    }
}