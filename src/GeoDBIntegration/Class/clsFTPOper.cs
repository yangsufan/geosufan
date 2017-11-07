using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace GeoDBIntegration
{
    /*
     * guozheng 2010-10-11 
     * 该类实现文件库的相关操作，包括：从数据库集成管理子系统获取工程信息保存到xml配置文件
     */
    class clsFTPOper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public clsFTPOper()
        {
        }

        public bool SaveProjectXML(string sDBid, string sDBName, out Exception ex)
        {
            ex = null;
            long lDBid = -1;////数据库ID
            XmlElement ele = null;/////连接信息
            if (string.IsNullOrEmpty(sDBName)) { ex = new Exception("数据库名称为空"); return false; }
            ///获取数据库的ID////////
            try
            {
                lDBid = Convert.ToInt64(sDBid);
            }
            catch
            {
                return false;
            }
            ///获取数据库的相关信息////
            DevComponents.AdvTree.Node ProjectNode = GetProjectNode(enumInterDBType.成果文件数据库.ToString(), sDBName, out ex);
            if (ex != null) return false;
            if (ProjectNode == null)
            {
                ex = new Exception("找不到名称为：" + sDBName + " 的数据库节点");
                return false;
            }
            try
            {
                ele = ProjectNode.Tag as XmlElement;
            }
            catch
            {
                ex = new Exception("节点获取连接信息失败");
                return false;
            }
            if (ele == null) { ex = new Exception("节点获取连接信息失败"); return false; }
            /////////记录到xml文件中/////
            string sServer = string.Empty;/////////////FTP服务IP
            string sUser = string.Empty;///////////////FTP用户
            string sPassWord = string.Empty;///////////FTP密码
            string sMetaDBConnectInfo = string.Empty;//Oracle元信息库连接字符串
            #region 获取必要信息
            try
            {
                string[] getinfo = ele.GetAttribute("数据库连接信息").Split('|');
                sServer = getinfo[0];
                sUser = getinfo[3];
                sPassWord = getinfo[4];
                sMetaDBConnectInfo = getinfo[6];
            }
            catch
            {
                ex = new Exception("连接信息中获取字段失败");
                return false;
            }
            #endregion
            if (File.Exists(ModuleData.v_FTPCoonectionInfoXML))
            {
                try
                {
                    File.Delete(ModuleData.v_FTPCoonectionInfoXML);
                }
                catch (Exception Error)
                {
                    ex = new Exception("原工程文件：" + ModuleData.v_FTPCoonectionInfoXML + "删除失败，\n原因：" + Error.Message);
                    return false;
                }
            }
            XmlDocument Doc = new XmlDocument();
            #region 建立工程xml
            XmlNode RootNode = Doc.CreateNode(XmlNodeType.Element, "DBCatalog", null);/////根节点
            XmlNode DataBaseNode = Doc.CreateNode(XmlNodeType.Element, "DataBase", null);/////DataBase节点
            XmlNode ChildProjectNode = Doc.CreateNode(XmlNodeType.Element, "连接信息", null);//////连接信息节点
            XmlAttribute addAttr = null;
            ////DataBaseNode节点///////////////////////
            addAttr = Doc.CreateAttribute("NodeName");
            addAttr.Value = "文件连接";
            DataBaseNode.Attributes.SetNamedItem(addAttr);
            ////
            ////
            addAttr = Doc.CreateAttribute("NodeText");
            addAttr.Value = sServer;
            DataBaseNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = Doc.CreateAttribute("NodeType");
            addAttr.Value = "DATABASE";
            DataBaseNode.Attributes.SetNamedItem(addAttr);
            ////连接信息节点////////////////////////////
            addAttr = Doc.CreateAttribute("服务器");
            addAttr.Value = sServer;
            ChildProjectNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = Doc.CreateAttribute("用户");
            addAttr.Value = sUser;
            ChildProjectNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = Doc.CreateAttribute("密码");
            addAttr.Value = sPassWord;
            ChildProjectNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = Doc.CreateAttribute("MetaDBConn");
            addAttr.Value = sMetaDBConnectInfo;
            ChildProjectNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = Doc.CreateAttribute("数据库ID");
            addAttr.Value = sDBid;
            ChildProjectNode.Attributes.SetNamedItem(addAttr);
            ////
            DataBaseNode.AppendChild(ChildProjectNode);
            RootNode.AppendChild(DataBaseNode);
            Doc.AppendChild(RootNode);
            try
            {
                Doc.Save(ModuleData.v_FTPCoonectionInfoXML);
            }
            catch (Exception Error)
            {
                ex = new Exception("工程信息xml保存失败，\n原因：" + Error.Message);
                return false;
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 根据数据库类型名称和数据库名称，在工程树图中获取节点
        /// </summary>
        /// <param name="sDBtype">数据库类型</param>
        /// <param name="sDBName">数据库名称</param>
        /// <param name="ex">输出：错误信息</param>
        /// <returns>AdvTree.Node</returns>
        public static DevComponents.AdvTree.Node GetProjectNode(string sDBtype, string sDBName, out Exception ex)
        {
            ex = null;
            DevComponents.AdvTree.Node DBTypeNode = null;
            DevComponents.AdvTree.Node DBProjectNode = null;
            DevComponents.AdvTree.NodeCollection rootnodes = ModuleData.v_AppDBIntegra.ProjectTree.Nodes;
            //////////获取数据库类型的节点////////
            if (rootnodes == null)
            {
                ex = new Exception("工程树获取节点失败");
                return null;
            }
            for (int i = 0; i < rootnodes.Count; i++)
            {
                DevComponents.AdvTree.Node getnode = rootnodes[i];
                if (getnode.Text == "数据库管理工具")
                {
                    DevComponents.AdvTree.NodeCollection DesNodes = getnode.Nodes;
                    if (DesNodes == null) { ex = new Exception("工程树获取节点失败"); return null; }
                    for (int j = 0; j < DesNodes.Count; j++)
                    {
                        if (DesNodes[j].Text == sDBtype)
                        {
                            DBTypeNode = DesNodes[j];
                            break;
                        }
                    }

                }
            }
            //////////获取数据库工程的节点///////
            if (DBTypeNode == null) { ex = new Exception("获取数据库类型节点失败"); return null; }
            for (int i = 0; i < DBTypeNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node getnode = DBTypeNode.Nodes[i];
                if (getnode.Text == sDBName)
                {
                    DBProjectNode = getnode;
                    break;
                }
            }
            if (DBProjectNode == null)
            {
                ex = new Exception("找不到名称为：" + sDBName + " 的数据库节点");
                return null;
            }
            else
            {
                return DBProjectNode;
            }
        }
    }
}
