using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GeoDataCenterFunLib
{
    //数据单元树
    public partial class frmDataUnitTree : DevComponents.DotNetBar.Office2007Form
    {
        public frmDataUnitTree()
        {
            InitializeComponent();
            InitDataUnitTree();
        }
        //private TreeNode m_node;//全局变量选择当前节点
        public int flag;
        public void InitDataUnitTree()
        {//从 数据单元表 中获取信息
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strDispLevel = dIndex.GetXmlElementValue("UnitTree", "tIsDisp");//是否从市级开始创建数据单元树
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select * from " + "数据单元表";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();

                DataUnitTree.Nodes.Clear();
                TreeNode tparent;
                tparent = new TreeNode();
                tparent.Text = "所有行政区";
                tparent.Tag = 0;

                TreeNode tRoot;
                tRoot = new TreeNode();
                tRoot = tparent;
                TreeNode tNewNode;
                TreeNode tNewNodeClild;
                TreeNode tNewLeafNode;
                DataUnitTree.Nodes.Add(tparent);
                DataUnitTree.ExpandAll();
                while (aReader.Read())
                {
                    //如果是行政区级别是1就是根节点
                    //此处默认都已经排序合理，针对排序的维护在数据字典维护界面中实现
                    if (aReader["数据单元级别"].ToString().Equals("1")) //省级节点
                    {
                        //if (strDispLevel.Equals("0"))
                        //{
                            tNewNode = new TreeNode();
                            tNewNode.Text = aReader["行政名称"].ToString();
                            tNewNode.Name = aReader["行政代码"].ToString();
                            tparent.Nodes.Add(tNewNode);
                            tparent.Expand();
                            tRoot = tNewNode;
                            tNewNode.Tag = 1;
                            tNewNode.ImageIndex = 17;
                            tNewNode.SelectedImageIndex = 17;
                        //}

                    }
                    else if (aReader["数据单元级别"].ToString().Equals("2")) //市级节点
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tparent.Expand();
                        tRoot = tNewNodeClild;
                        tNewNodeClild.Tag = 2;
                        tNewNodeClild.ImageIndex = 17;
                        tNewNodeClild.SelectedImageIndex = 17;
                    }
                    else if (aReader["数据单元级别"].ToString().Equals("3"))//县级节点
                    {
                        tNewLeafNode = new TreeNode();
                        tNewLeafNode.Text = aReader["行政名称"].ToString();
                        tNewLeafNode.Name = aReader["行政代码"].ToString();
                        tRoot.Nodes.Add(tNewLeafNode);
                        tNewLeafNode.Tag = 3;
                        tNewLeafNode.ImageIndex = 17;
                        tNewLeafNode.SelectedImageIndex = 17;
                    }
                    else
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tparent.Expand();
                        tNewNodeClild.Tag = 1;
                    }
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

        private void DataUnitTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node!= null)
            {
                if (flag == 0)
                {

                    if (Convert.ToInt32(e.Node.Tag) != 0)
                    frmAnalyseInLibMap.Node = e.Node;
                }
                else if (flag == 1)
                    frmDataReduction.Node = e.Node;
                else if (flag == 2)
                    frmDocRedution.Node = e.Node;
                else
                {
                    if (Convert.ToInt32(e.Node.Tag) != 3)
                    {
                        MessageBox.Show("请选择一个县级节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        frmDocUpload.Node = e.Node;
                }
            }

            this.Close();
        }
        //确定
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (DataUnitTree.SelectedNode != null)
            {
                if (flag == 0)
                {
                    if (Convert.ToInt32(DataUnitTree.SelectedNode.Tag) != 0)
                    frmAnalyseInLibMap.Node = DataUnitTree.SelectedNode;
                }
                else if (flag == 1)
                    frmDataReduction.Node = DataUnitTree.SelectedNode;
                else if (flag == 2)
                    frmDocRedution.Node = DataUnitTree.SelectedNode;
                else
                {
                    if (Convert.ToInt32(DataUnitTree.SelectedNode.Tag) != 3)
                    {
                        MessageBox.Show("请选择一个县级节点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                        frmDocUpload.Node = DataUnitTree.SelectedNode;
                }
               
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}