using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using System.Data.OleDb;
using System.IO;

namespace GeoDBConfigFrame
{
    public partial class AddGroupForm : DevComponents.DotNetBar.Office2007Form
    {
        public string m_mypath;
        public string m_strSubName;
        public AddGroupForm()
        {
            InitializeComponent();

            //初始化fromtreeview窗口
            InitFromTreeView();
            
        }

        //初始化fromtreeview窗口
        public void InitFromTreeView()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            m_mypath = dIndex.GetDbInfo();

            //判断文件是否存在
            if (!File.Exists(m_mypath))
                return;

            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select 代码,描述 from 标准图层信息表 order by 业务小类代码";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();
                TreeNode tNewNode;
                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                while (aReader.Read())
                {

                    tNewNode = new TreeNode();
                    tNewNode.Text = aReader["描述"].ToString();
                    tNewNode.Name = aReader["代码"].ToString();
                    fromtreeView.Nodes.Add(tNewNode);
                    fromtreeView.ExpandAll();
                 }

                 //关闭reader对象     
                 aReader.Close();

                 //关闭连接,这很重要     
                 mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //查找树节点
        private TreeNode FindNode( TreeNode tnParent, string strValue )
        {
            if( tnParent == null ) 
                return null;
            if( tnParent.Text == strValue ) 
                return tnParent;
     
            TreeNode tnRet = null;
            foreach( TreeNode tn in tnParent.Nodes )
            {
                tnRet = FindNode( tn, strValue );
                if( tnRet != null ) 
                    break;
            }
            return tnRet;
        }


        private void btnIn_Click(object sender, EventArgs e)
        {
            if (fromtreeView.SelectedNode != null)
            {
                //判断节点是否存在与fromTreeView中
                bool bFlag = false;
                for (int ii = 0; ii < totreeView.Nodes.Count;ii++ )
                {
                    if (totreeView.Nodes[ii].Name.Equals(fromtreeView.SelectedNode.Name))
                    {
                        bFlag = true;
                        break;
                    }
                }

                if (bFlag == false)
                {
                    TreeNode tSleNode = null;
                    tSleNode = new TreeNode();
                    tSleNode.Text = fromtreeView.SelectedNode.Text;
                    tSleNode.Name = fromtreeView.SelectedNode.Name;
                    totreeView.Nodes.Add(tSleNode);
                }        
                totreeView.Refresh();
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            if (totreeView.SelectedNode != null)
            {
                totreeView.Nodes.Remove(totreeView.SelectedNode);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (textBoxGroupName.Text.Trim().Equals(""))
            {
                MessageBox.Show("请输入组名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            m_strSubName = textBoxGroupName.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Hide();
            this.Dispose(true);
        }

        private void butnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }
    }
}