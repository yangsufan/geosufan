using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using System.Data.OleDb;

namespace GeoDBConfigFrame
{
    public partial class ModifyDataUnitControl : DevComponents.DotNetBar.Office2007Form
    {
        public TreeNode m_NewRootNode;  //toTreeView的根节点

        public ModifyDataUnitControl()
        {
            InitializeComponent();

            InitFromTree();

            InitToTree();
        }


        //初始化建树
        public void InitFromTree()
        {
            //从 数据单元表 中获取信息
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select * from " + "标准数据单元表";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();

                FromTreeView.Nodes.Clear();
                TreeNode tRoot;
                tRoot = new TreeNode();
                tRoot.Text = "数据单元";
                tRoot.Tag = 0;
                tRoot.ImageIndex = 0;
                tRoot.SelectedImageIndex = 0;
                TreeNode tparent;
                tparent = new TreeNode();
                tparent = tRoot;

                TreeNode tCityNode;
                tCityNode = new TreeNode();
                tCityNode = tRoot;

                TreeNode tNewNode;
                TreeNode tNewNodeClild;
                TreeNode tNewLeafNode;

                FromTreeView.Nodes.Add(tRoot);
                FromTreeView.ExpandAll();
                while (aReader.Read())
                {
                    //如果是行政区级别是1就是根节点
                    //此处默认都已经排序合理，针对排序的维护在数据字典维护界面中实现
                    if (aReader["数据单元级别"].ToString().Equals("1")) //省级节点
                    {
                          tNewNode = new TreeNode();
                          tNewNode.Text = aReader["行政名称"].ToString();
                          tNewNode.Name = aReader["行政代码"].ToString();
                          tRoot.Nodes.Add(tNewNode);
                          tparent = tNewNode;
                          tNewNode.Tag = 1;
                          tNewNode.ImageIndex = 1;
                          tNewNode.SelectedImageIndex = 1;

                    }
                    else if (aReader["数据单元级别"].ToString().Equals("2")) //市级节点
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tNewNodeClild.Tag = 2;
                        tCityNode = tNewNodeClild;
                        tNewNodeClild.ImageIndex = 1;
                        tNewNodeClild.SelectedImageIndex = 1;
                    }
                    else if (aReader["数据单元级别"].ToString().Equals("3"))//县级节点
                    {
                        tNewLeafNode = new TreeNode();
                        tNewLeafNode.Text = aReader["行政名称"].ToString();
                        tNewLeafNode.Name = aReader["行政代码"].ToString();
                        tCityNode.Nodes.Add(tNewLeafNode);
                        tNewLeafNode.Tag = 3;
                        tNewLeafNode.ImageIndex = 1;
                        tNewLeafNode.SelectedImageIndex = 1;
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

        public void InitToTree()
        {
            //从 数据单元表 中获取信息
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strDispLevel = dIndex.GetXmlElementValue("UnitTree", "tIsDisp");//是否从市级开始创建数据单元树
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select * from 数据单元表";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();

                ToTreeView.Nodes.Clear();
                TreeNode tparent;
                tparent = new TreeNode();
                tparent.Text = "数据单元";
                tparent.Tag = 0;
                tparent.ImageIndex = 0;
                tparent.SelectedImageIndex = 0;
                TreeNode tRoot;
                tRoot = new TreeNode();
                tRoot = tparent;
                TreeNode tNewNode;
                TreeNode tNewNodeClild;
                TreeNode tNewLeafNode;
                ToTreeView.Nodes.Add(tparent);
                ToTreeView.ExpandAll();
                while (aReader.Read())
                {
                    //如果是行政区级别是1就是根节点
                    //此处默认都已经排序合理，针对排序的维护在数据字典维护界面中实现
                    if (aReader["数据单元级别"].ToString().Equals("1")) //省级节点
                    {
                        if (strDispLevel.Equals("0"))
                        {
                            tNewNode = new TreeNode();
                            tNewNode.Text = aReader["行政名称"].ToString();
                            tNewNode.Name = aReader["行政代码"].ToString();
                            tparent.Nodes.Add(tNewNode);
                            tparent.Expand();
                            tparent = tNewNode;
                            tNewNode.Tag = 1;
                            tNewNode.ImageIndex = 1;
                            tNewNode.SelectedImageIndex = 1;
                        }

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
                        tNewNodeClild.ImageIndex = 1;
                        tNewNodeClild.SelectedImageIndex = 1;
                    }
                    else if (aReader["数据单元级别"].ToString().Equals("3"))//县级节点
                    {
                        tNewLeafNode = new TreeNode();
                        tNewLeafNode.Text = aReader["行政名称"].ToString();
                        tNewLeafNode.Name = aReader["行政代码"].ToString();
                        tRoot.Nodes.Add(tNewLeafNode);
                        tNewLeafNode.Tag = 3;
                        tNewLeafNode.ImageIndex = 1;
                        tNewLeafNode.SelectedImageIndex = 1;
                    }
                    else
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tparent.Expand();
                        tNewNodeClild.Tag = 1;
                        tNewNodeClild.ImageIndex = 1;
                        tNewNodeClild.SelectedImageIndex = 1;
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

        //进入
        private void btnIn_Click(object sender, EventArgs e)
        {
            //从 数据单元表 中获取信息
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strDispLevel = dIndex.GetXmlElementValue("UnitTree", "tIsDisp");//是否从市级开始创建数据单元树
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 行政名称 from 数据单元表 where 数据单元级别='1'";
            GeoDataCenterDbFun db=new GeoDataCenterDbFun();
            string strProvince = db.GetInfoFromMdbByExp(constr, strExp);

            //获取当前点击节点 省、市节点
            //ToTreeView.Nodes.Clear();
            TreeNode tFromSelNode = FromTreeView.SelectedNode;   
            if (tFromSelNode != null)
            {
                //是省级节点 包含整个市的
                if (tFromSelNode.Tag.Equals(1))
                {
                    ToTreeView.Nodes.Clear();
                    //获取该省级节点的所有子节点
                    TreeNode tRootNode;
                    TreeNode tCityNode;
                    TreeNode tCountyNode;
                    tRootNode = new TreeNode();
                    tRootNode.Name = tFromSelNode.Name;
                    tRootNode.Text = tFromSelNode.Text;
                    tRootNode.Tag = tFromSelNode.Tag;
                    tRootNode.ImageIndex = tFromSelNode.ImageIndex;
                    ToTreeView.Nodes.Add(tRootNode);
                    m_NewRootNode = tRootNode;

                    int iNodeCount = tFromSelNode.GetNodeCount(false);
                    tCityNode = new TreeNode();
                    TreeNode tOldCityNode = tFromSelNode.FirstNode;
                    tCityNode.Name = tOldCityNode.Name;
                    tCityNode.Text = tOldCityNode.Text;
                    tCityNode.Tag = tOldCityNode.Tag;
                    tCityNode.ImageIndex = tOldCityNode.ImageIndex;
                    while (iNodeCount >0)
                    {
                        tRootNode.Nodes.Add(tCityNode);
                        int iLeafCount = tOldCityNode.GetNodeCount(false);
                        tCountyNode =  new TreeNode();
                        TreeNode tOldCountyNode = tOldCityNode.FirstNode;
                        tCountyNode.Name = tOldCountyNode.Name;
                        tCountyNode.Text = tOldCountyNode.Text;
                        tCountyNode.Tag = tOldCountyNode.Tag;
                        tCountyNode.ImageIndex = tOldCountyNode.ImageIndex;
                        tCountyNode.ExpandAll();
                        while (iLeafCount > 0)
                        {
                            tCityNode.Nodes.Add(tCountyNode);
                            if (tOldCountyNode != tOldCityNode.LastNode)
                            {
                                tOldCountyNode = tOldCountyNode.NextNode;
                                tCountyNode = new TreeNode();
                                tCountyNode.Name = tOldCountyNode.Name;
                                tCountyNode.Text = tOldCountyNode.Text;
                                tCountyNode.Tag = tOldCountyNode.Tag;
                                tCountyNode.ImageIndex = tOldCountyNode.ImageIndex;
                                tCountyNode.ExpandAll();
                            }
                            
                            iLeafCount--;
                        }

                        if (tOldCityNode != tFromSelNode.LastNode)
                        {
                            tOldCityNode = tOldCityNode.NextNode;
                            tCityNode = new TreeNode();
                            tCityNode.Name = tOldCityNode.Name;
                            tCityNode.Text = tOldCityNode.Text;
                            tCityNode.Tag = tOldCityNode.Tag;
                            tCityNode.ImageIndex = tOldCityNode.ImageIndex;
                            tCityNode.ExpandAll();
                        }
                        iNodeCount--;
                        tRootNode.Expand();
                       
                    }
                   // ToTreeView.Refresh();
                    
                }
                else if (tFromSelNode.Tag.Equals(2)) //是市级节点 要包含省的
                {
                    //获取省级节点 然后该市级节点包含子节点
                    TreeNode tParentNode = tFromSelNode.Parent;
                    TreeNode tRootNode=null;
                    TreeNode tCityNode=null;
                    TreeNode tCountyNode=null;
                    if (ToTreeView.Nodes.Count>=1)
                    {
                        if (strProvince != (tParentNode.Text) && ToTreeView.Nodes[0].Text != tParentNode.Text)//是该省节点
                        {
                            ToTreeView.Nodes.Clear();
                            tRootNode = new TreeNode();
                            //插入省节点
                            tRootNode.Name = tParentNode.Name;
                            tRootNode.Text = tParentNode.Text;
                            tRootNode.Tag = tParentNode.Tag;
                            tRootNode.ImageIndex = tParentNode.ImageIndex;
                            ToTreeView.Nodes.Add(tRootNode);
                        }
                        else
                        {
                            tRootNode = ToTreeView.Nodes[0];
                        }
                        if (ToTreeView.Nodes[0].Nodes.Count > 0)//已经有市节点
                        {
                            foreach (TreeNode node in ToTreeView.Nodes[0].Nodes)
                            {
                                if ((node.Text.Trim() == tFromSelNode.Text.Trim()))//市节点已存在，
                                {
                                    DialogResult result = MessageBox.Show("是否替换该市节点?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (result == DialogResult.No)//不替换
                                    {
                                        return;//不做反应，直接返回
                                    }
                                    else//替换
                                    {
                                        node.Remove();//移除该节点
                                    }
                                }
                            }
                        }

                        tCityNode = new TreeNode();
                        //插入市节点
                        tCityNode.Name = tFromSelNode.Name;
                        tCityNode.Text = tFromSelNode.Text;
                        tCityNode.Tag = tFromSelNode.Tag;
                        tCityNode.ImageIndex = tFromSelNode.ImageIndex;
                        tRootNode.Nodes.Add(tCityNode);
                        tRootNode.Expand();
                    }
                    else
                    {
                        tRootNode = new TreeNode();
                        //插入省节点
                        tRootNode.Name = tParentNode.Name;
                        tRootNode.Text = tParentNode.Text;
                        tRootNode.Tag = tParentNode.Tag;
                        tRootNode.ImageIndex = tParentNode.ImageIndex;
                        ToTreeView.Nodes.Add(tRootNode);;

                        tCityNode = new TreeNode();
                        //插入市节点
                        tCityNode.Name = tFromSelNode.Name;
                        tCityNode.Text = tFromSelNode.Text;
                        tCityNode.Tag = tFromSelNode.Tag;
                        tCityNode.ImageIndex = tFromSelNode.ImageIndex;
                        tRootNode.Nodes.Add(tCityNode);
                        tRootNode.Expand();
                    }
                    m_NewRootNode = tRootNode;

                    //插入县级节点
                    int iNodeCount = tFromSelNode.GetNodeCount(false);
                    tCountyNode = new TreeNode();
                    TreeNode tOldCountyNode = tFromSelNode.FirstNode;
                    if (tOldCountyNode != null)
                    {
                        tCountyNode.Name = tOldCountyNode.Name;
                        tCountyNode.Text = tOldCountyNode.Text;
                        tCountyNode.Tag = tOldCountyNode.Tag;
                        tCountyNode.ImageIndex = tOldCountyNode.ImageIndex;
                    }
                    while (iNodeCount > 0)
                    {
                        tCityNode.Nodes.Add(tCountyNode);
                        tCountyNode = new TreeNode();
                        if (tOldCountyNode != tFromSelNode.LastNode)
                        {
                            tOldCountyNode = tOldCountyNode.NextNode;
                            tCountyNode.Name = tOldCountyNode.Name;
                            tCountyNode.Text = tOldCountyNode.Text;
                            tCountyNode.Tag = tOldCountyNode.Tag;
                            tCountyNode.ImageIndex = tOldCountyNode.ImageIndex;
                        }
                        iNodeCount--;
                    }
                }
                else if (tFromSelNode.Tag.Equals(3))  //县级节点
                {
                    //获取省级节点 然后该市级节点包含子节点
                    TreeNode tParentNode = tFromSelNode.Parent.Parent;
                    TreeNode tRootNode = null;
                    TreeNode tCityNode = null;
                    TreeNode tCountyNode = null;
                    if (ToTreeView.Nodes.Count >= 1)
                    {
                        if (strProvince != (tParentNode.Text) && ToTreeView.Nodes[0].Text != tParentNode.Text)//是该省节点
                        {
                            ToTreeView.Nodes.Clear();
                            tRootNode = new TreeNode();
                            //插入省节点
                            tRootNode.Name = tParentNode.Name;
                            tRootNode.Text = tParentNode.Text;
                            tRootNode.Tag = tParentNode.Tag;
                            tRootNode.ImageIndex = tParentNode.ImageIndex;
                            ToTreeView.Nodes.Add(tRootNode);
                        }
                        else
                        {
                            tRootNode = ToTreeView.Nodes[0];
                        }
                        if (ToTreeView.Nodes[0].Nodes.Count > 0)//已经有市节点
                        {
                            foreach (TreeNode node in ToTreeView.Nodes[0].Nodes)
                            {
                                if ((node.Text.Trim() == tFromSelNode.Parent.Text.Trim()))//市节点已存在，
                                {
                                    tCityNode = node;
                                }
                            }
                           
                        }
                        if (tCityNode == null)//没有该市节点，添加一个市节点
                        {
                            tCityNode = new TreeNode();
                            //插入市节点
                            tCityNode.Name = tFromSelNode.Parent.Name;
                            tCityNode.Text = tFromSelNode.Parent.Text;
                            tCityNode.Tag = tFromSelNode.Parent.Tag;
                            tCityNode.ImageIndex = tFromSelNode.Parent.ImageIndex;
                            tRootNode.Nodes.Add(tCityNode);
                            tRootNode.ExpandAll();
                            tCityNode.ExpandAll();
                        }

                    }
                    else
                    {
                        tRootNode = new TreeNode();
                        //插入省节点
                        tRootNode.Name = tParentNode.Name;
                        tRootNode.Text = tParentNode.Text;
                        tRootNode.Tag = tParentNode.Tag;
                        tRootNode.ImageIndex = tParentNode.ImageIndex;
                        ToTreeView.Nodes.Add(tRootNode);

                        tCityNode = new TreeNode();
                        //插入市节点
                        tCityNode.Name = tFromSelNode.Parent.Name;
                        tCityNode.Text = tFromSelNode.Parent.Text;
                        tCityNode.Tag = tFromSelNode.Parent.Tag;
                        tCityNode.ImageIndex = tFromSelNode.Parent.ImageIndex;
                        tRootNode.Nodes.Add(tCityNode);
                    }
                    
                    m_NewRootNode = tRootNode;
                    tRootNode.Expand();
                    //插入县级节点
                    if (tCityNode.Nodes.Count > 0)
                    {
                        foreach (TreeNode node in tCityNode.Nodes)
                        {
                            if ((node.Text.Trim() == tFromSelNode.Text.Trim()))//县节点已存在，
                            {
                                MessageBox.Show("该节点已存在!", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    tCountyNode = new TreeNode();
                    tCountyNode.Name = tFromSelNode.Name;
                    tCountyNode.Text = tFromSelNode.Text;
                    tCountyNode.Tag = tFromSelNode.Tag;
                    tCountyNode.ImageIndex = tFromSelNode.ImageIndex;
                    tCountyNode.ExpandAll();
                    tCityNode.Nodes.Add(tCountyNode);
                    tCityNode.ExpandAll();
                    tCountyNode.ExpandAll();

                }
            }
        }

        //把当前的数据单元写入 “数据单元表”
        public void WriteDataIntoDataUnitTbl()
        {
            if(ToTreeView.GetNodeCount(true) > 0)  //有记录就更新
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
                string strExp = "";
                strExp = "delete * from " + "数据单元表";
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);

                try
                {
                    mycon.Open();

                    //删除记录    
                    int  iRows = aCommand.ExecuteNonQuery();

                    //插入记录
                    SearchAllTreeNod(mycon,m_NewRootNode);

                    //关闭连接,这很重要     
                    mycon.Close();
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
       
            }
        }

        //递归遍历所有树节点
        public void  SearchAllTreeNod( OleDbConnection mycon,TreeNode   node) 
        {
            if (node == null)
                return;
            string strExp = "";
            strExp = "insert into 数据单元表 (行政代码,行政名称,数据单元级别) values ('" + node.Name + "','" + node.Text + "','" + node.Tag + "')";
          
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                //删除记录    
                int iRows = aCommand.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //id 重新编号
    /*        strExp = "alter table [数据单元表] alter column [ID] counter (1,1)";
            OleDbCommand aCommandbh = new OleDbCommand(strExp, mycon);
            try
            {
                //重新编号    
                 aCommandbh.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
*/
            foreach(TreeNode   findNode   in   node.Nodes) 
            { 
                 //在这里读取n的内容 
                 SearchAllTreeNod(mycon,findNode); 
            } 
        } 


        //清除
        private void btnOut_Click(object sender, EventArgs e)
        {
            //ToTreeView.Nodes.Clear();
            //if (ToTreeView.SelectedNode.Nodes > 0)
            //{
            //    ToTreeView.SelectedNode.Nodes.Clear();
            //}
            try
            {
                ToTreeView.SelectedNode.Remove();
            }
            catch { }

        }

        //确定
        private void btnOk_Click(object sender, EventArgs e)
        {
            //把当前的数据单元写入 “数据单元表”
            WriteDataIntoDataUnitTbl();
            MessageBox.Show("操作完成!","提示");
            this.DialogResult = DialogResult.OK;
            this.Hide();
            this.Dispose(true);
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }

        private void FromTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            labelCode.Text = "";
            if (FromTreeView.SelectedNode != e.Node)
            {
                if (FromTreeView.SelectedNode != null)
                {
                    FromTreeView.SelectedNode.ForeColor = Color.Black;
                }
            }

            FromTreeView.SelectedNode = e.Node;
            FromTreeView.SelectedNode.ForeColor = Color.Red;

            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 行政代码 from  标准数据单元表 where 行政名称='" + FromTreeView.SelectedNode.Text + "' and 数据单元级别=" + FromTreeView.SelectedNode.Tag+ "";
            GeoDataCenterDbFun db=new GeoDataCenterDbFun();
            labelCode.Text = db.GetInfoFromMdbByExp(strCon, strExp);
           
        }

        private void ToTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            labelCode.Text = "";
            if (ToTreeView.SelectedNode != e.Node)
                ToTreeView.SelectedNode = e.Node;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 行政代码 from  标准数据单元表 where 行政名称='" + ToTreeView.SelectedNode.Text + "' and 数据单元级别=" + ToTreeView.SelectedNode.Tag + "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            labelCode.Text = db.GetInfoFromMdbByExp(strCon, strExp);
        }
    }
}