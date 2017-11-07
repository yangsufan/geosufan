using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;

namespace GeoDBConfigFrame
{
    public partial class frmQueryDataRcd : DevComponents.DotNetBar.Office2007Form
    {
        /// <summary>
        /// 存储数据字典的目录树
        /// </summary>
        public  System.Windows.Forms.TreeView m_DataTabIndexTree;
        public  IWorkspace m_Workspace;
        /// <summary>
        /// 记录查询字段名的xml路径
        /// </summary>
        public  string  m_strPath =Application.StartupPath+"\\..\\Res\\Xml\\数据字典检索记录.xml";
        public string m_strLike = "%";
        public frmQueryDataRcd(System.Windows.Forms.TreeView pDataTabIndexTree, IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_DataTabIndexTree = pDataTabIndexTree;
            m_Workspace = pWorkspace;
            
        }

        private void bttQuery_Click(object sender, EventArgs e)
        {
            Query();
        }
        private void Query()
        {

            Plugin.LogTable.Writelog("数据字典检索查询,检索关键字为"+txtKeys.Text);
            if (m_DataTabIndexTree.Nodes.Count == 0) { Plugin.LogTable.Writelog("数据字典检索查询失败"); return; }
            TreeNode vNode = m_DataTabIndexTree.Nodes[0];
            if (vNode.Nodes.Count > 0)
            {
                vNode = GetNode(vNode).Parent;
            }
            dataGridVRe.Rows.Clear();
            List<string> lstField = GetFieldName(m_strPath);
            m_strLike = GetDescriptionOfWorkspace(m_Workspace);
            for (int i = 0; i < vNode.Nodes.Count; i++)
            {
                string strTableName = SysCommon.ModSysSetting.GetTblNameByItemName(vNode.Nodes[i].Text);
                ICursor pCursor = GetCursor(m_Workspace, lstField, txtKeys.Text, strTableName, m_strLike);
                if (pCursor == null) { continue; }
                try
                {
                    IRow pRow = pCursor.NextRow();
                    while (pRow != null)
                    {
                        int pCODEindex = pRow.Fields.FindField(lstField[0]);
                        int pNANMEindex = pRow.Fields.FindField(lstField[1]);
                        ///当NAME的字段不存在时，其值默认为空
                        if (pCODEindex == -1 && pNANMEindex != -1)
                        {
                            dataGridVRe.Rows.Add(vNode.Nodes[i].Text, "", pRow.get_Value(pNANMEindex));
                        }
                        ///当CODE的字段不存在时，其值默认为空
                        else if (pCODEindex != -1 && pNANMEindex == -1)
                        {
                            dataGridVRe.Rows.Add(vNode.Nodes[i].Text, pRow.get_Value(pCODEindex), "");
                        }
                        ///当NAME和CODE的字段不存在时，其值都默认为空
                        else if (pCODEindex == -1 && pNANMEindex == -1)
                        {
                            dataGridVRe.Rows.Add(vNode.Nodes[i].Text, "", "");
                        }
                        else
                        {
                            dataGridVRe.Rows.Add(vNode.Nodes[i].Text, pRow.get_Value(pCODEindex), pRow.get_Value(pNANMEindex));
                        }
                        pRow = pCursor.NextRow();
                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    pCursor = null;
                }
            }
        }
        /// <summary>
        /// 获取最低层树的父节点
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        private TreeNode GetNode(TreeNode pNode)
        {
            TreeNode vNode = new TreeNode();
            if (pNode.Nodes.Count == 0) { vNode = pNode; return vNode; }
            vNode = pNode.Nodes[0];
            if (vNode.Nodes.Count > 0)
            { 
                TreeNode vNode2 = vNode.Nodes[0];
                vNode = GetNode(vNode2);
            }
            return vNode;
        }
        /// <summary>
        /// 获取配置文件中配置的查询字段
        /// </summary>
        /// <param name="strxmlpath">记录字段名的xml 路径</param>
        /// <returns></returns>
        private List<string> GetFieldName(string strxmlpath)
        {
            List<string> lstField = new List<string>();
            if (!File.Exists(strxmlpath))
            {
                MessageBox.Show("检索字段配置文件丢失！","提示！");
                return lstField = null;
            }
            XmlDocument pXml = new XmlDocument();
            pXml.Load(strxmlpath);
            XmlNodeList pNodelist = pXml.SelectNodes("//GisDoc//DataRcd");
            foreach (XmlNode vnode in pNodelist)
            {
                lstField.Add(vnode.Attributes["FileName"].Value);
            }
            return lstField;
        }
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static string GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            string strLike = "%";
            if (pWorkspace == null)
            {
                return strLike = "%";
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return strLike = "%";
            }
            string strDescrip = pWorkSpaceFac.get_WorkspaceDescription(false);
            switch (strDescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            return strLike;
        }
        /// <summary>
        /// 获取查询结果的游标
        /// </summary>
        /// <param name="pWorkspace">查询对象所在的工作空间</param>
        /// <param name="plstField">所要查询的字段名称</param>
        /// <param name="strKey">关键字</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strLike">当前工作空间的通配符</param>
        /// <returns></returns>
        private ICursor GetCursor(IWorkspace pWorkspace, List<string> plstField, string strKey, string strTableName, string strLike)
        {

            ICursor pCursor = null;
            try
            {
                if (plstField.Count == 0) { return pCursor; }
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(strTableName);
                IQueryFilter pQueryFilter = new QueryFilterClass();
                for (int i = 0; i < plstField.Count; i++)
                {
                    ///获取当前字段的索引号
                    int pIndex = pTable.FindField(plstField[i].ToString());
                    if (pIndex == -1) { continue; }
                    ///根据字段名和关键字组合查询条件
                    if (pQueryFilter.WhereClause == "")
                    {
                        pQueryFilter.WhereClause = plstField[i].ToString() + " Like '" + m_strLike + strKey + m_strLike + "'";
                    }
                    else
                    {
                        pQueryFilter.WhereClause = pQueryFilter.WhereClause + " or " + plstField[i].ToString() + " Like '" + m_strLike + strKey + m_strLike + "'";
                    }
                }
                pCursor = pTable.Search(pQueryFilter, false);
                return pCursor;
            }
            catch { return pCursor; }
        }

        private void txtKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Query();
            }
        }
       
    }
}
