using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// 数据库工程树图、XML管理
    /// </summary>
    public static class ProjectXml
    {
        /// <summary>
        /// 根据XML内容添加数据库工程树图节点　　　陈亚飞　修改20101013
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="projectTree"></param>
        /// <returns></returns>
        public static void AddTreeNodeByXML(XmlDocument xmlDoc,DevComponents.AdvTree.AdvTree projectTree)
        {
            XmlElement rootElem = xmlDoc.DocumentElement;
            //cyf 20110612 modify
            //string curProID = rootElem.GetAttribute("当前库体编号");
            //end
            //if (curProID != "")
            //{
                //若存在工程，则遍历工程节点，获得属性信息并挂在树节点上
                foreach (XmlNode aXmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                   
                    XmlElement aElement = aXmlNode as XmlElement;
                    //cyf 20110612 modify
                    //工程项目ID
                    //string proID = aElement.GetAttribute("编号");
                    //if (proID != curProID) continue;
                    //end

                    DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
                    newNode.Name = aElement.GetAttribute("编号");
                    newNode.Text = aElement.GetAttribute("名称");
                    newNode.Tag = aElement;
                    newNode.Image = projectTree.ImageList.Images["数据库"];
                    newNode.DataKeyString = "project";
                    newNode.Expanded = true;
                    projectTree.Nodes.Add(newNode);

                    XmlNode contextNode = aXmlNode.FirstChild;
                    if (contextNode == null) continue;

                    //遍历数据库子节点
                    foreach (XmlNode subNode in contextNode.ChildNodes)
                    {
                        XmlElement subElement = subNode as XmlElement;
                        string sVisible = subElement.GetAttribute("是否显示");
                        if (sVisible == bool.FalseString.ToLower()) continue;

                        DevComponents.AdvTree.Node newNodeTemp = new DevComponents.AdvTree.Node();
                        newNodeTemp.Name = subElement.GetAttribute("名称");
                        newNodeTemp.Text = subElement.GetAttribute("名称");
                        newNodeTemp.Tag = subElement;
                        newNodeTemp.DataKeyString = subElement.Name;
                        if (newNodeTemp.Name == "现势库" || newNodeTemp.Name == "历史库" || newNodeTemp.Name == "工作库")
                        {
                            newNodeTemp.Image = projectTree.ImageList.Images["数据库子节点"];
                        }
                        else
                        {
                            newNodeTemp.Image = projectTree.ImageList.Images["栅格数据库"];
                        }
                        newNode.Nodes.Add(newNodeTemp);
                    }
                    //cyf 20110613 delete ;
                    //break;
                    //end
                }
           //}
        }

        /// <summary>
        /// 添加数据库工程树图节点，同时写入XML
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="projectTree"></param>
        /// <param name="projectName"></param>
        /// <param name="scale"></param>
        /// <param name="xmlTemple"></param>
        public static void AddTreeNode(XmlDocument xmlDoc, DevComponents.AdvTree.AdvTree projectTree, string projectName, string scale, string xmlTemple)
        {
            XmlElement proElement = xmlDoc.CreateElement("工程");
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(xmlTemple);
            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", projectName);
            (DBXmlNodeNew as XmlElement).SetAttribute("比例尺", scale);
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);

            XmlElement aElement = DBXmlNodeNew as XmlElement;
            DevComponents.AdvTree.Node newNode = new DevComponents.AdvTree.Node();
            newNode.Name = aElement.GetAttribute("名称");
            newNode.Text = aElement.GetAttribute("名称");
            newNode.Tag = aElement;
            newNode.Image = projectTree.ImageList.Images["数据库"];
            newNode.DataKeyString = "project";
            newNode.Expanded = true;
            projectTree.Nodes.Add(newNode);

            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                XmlElement subElement = subNode as XmlElement;
                string sVisible = subElement.GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                DevComponents.AdvTree.Node newNodeTemp = new DevComponents.AdvTree.Node();
                newNodeTemp.Name = subElement.GetAttribute("名称");
                newNodeTemp.Text = subElement.GetAttribute("名称");
                newNodeTemp.Tag = subElement;
                newNodeTemp.DataKeyString = subElement.Name;
                newNodeTemp.Image = projectTree.ImageList.Images["数据库子节点"];
                newNode.Nodes.Add(newNodeTemp);
            }

            projectTree.SelectedNode = newNode;
        }

        /// <summary>
        /// 删除数据库工程树图节点，同时写入XML
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="selNode"></param>
        public static void DelTreeNode(XmlDocument xmlDoc, DevComponents.AdvTree.Node selNode)
        {
            XmlNode aNode = xmlDoc.DocumentElement.SelectSingleNode(".//工程[@名称='" + selNode.Text + "']");
            if(aNode==null) return;
            xmlDoc.DocumentElement.RemoveChild(aNode);
            string savepath = xmlDoc.BaseURI;
            savepath = savepath.Substring(xmlDoc.BaseURI.IndexOf("///") + 3);
            xmlDoc.Save(savepath);
            selNode.Remove();
        }
    }

    

    //=================================================================================
    //陈亚飞添加
    /// <summary>
    /// 现势库
    /// </summary>
    public class UserDataBaseInfo
    {
        //是否显示
        private bool pIsVisible;
        public bool IsVisible
        {
            get
            {
                return pIsVisible;
            }
            set
            {
                pIsVisible = value;
            }
        }

        //库体名称
        private string pName;
        public string Name
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }

        #region 连接信息
        //数据库类型
        private string pDBType;
        public string DBType
        {
            get 
            {
                return pDBType;
            }
            set
            {
                pDBType=value;
            }
        }

        //服务器
        private string pServer;
        public string Server
        {
            get
            {
                return pServer;
            }
            set
            {
                pServer = value;
            }
        }

        //实例名
        private string pInstance;
        public string Instance
        {
            get
            {
                return pInstance;
            }
            set
            {
                pInstance = value;
            }
        }

        //数据库
        private string pDataBase;
        public string DataBase
        {
            get
            {
                return pDataBase;
            }
            set
            {
                pDataBase = value;
            }
        }

        //用户名
        private string pUser;
        public string User
        {
            get
            {
                return pUser;
            }
            set
            {
                pUser = value;
            }
        }

        //密码
        private string pPassword;
        public string Password
        {
            get
            {
                return pPassword;
            }
            set
            {
                pPassword = value;
            }
        }

        //版本
        private string pVersion;
        public string Version
        {
            get
            {
                return pVersion;
            }
            set
            {
                pVersion = value;
            }
        }

        //数据集名称
        private string pDataset;
        public string DataSet
        {
            get
            {
                return pDataset;
            }
            set
            {
                pDataset = value;
            }
        }

        #endregion

    }
    /// <summary>
    /// 历史库
    /// </summary>
    public class HistoryDataBaseInfo
    {
        //是否显示
        private bool pIsVisible;
        public bool IsVisible
        {
            get
            {
                return pIsVisible;
            }
            set
            {
                pIsVisible = value;
            }
        }

        //库体名称
        private string pName;
        public string Name
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }

        #region 连接信息
        //数据库类型
        private string pDBType;
        public string DBType
        {
            get
            {
                return pDBType;
            }
            set
            {
                pDBType = value;
            }
        }

        //服务器
        private string pServer;
        public string Server
        {
            get
            {
                return pServer;
            }
            set
            {
                pServer = value;
            }
        }

        //实例名
        private string pInstance;
        public string Instance
        {
            get
            {
                return pInstance;
            }
            set
            {
                pInstance = value;
            }
        }

        //数据库
        private string pDataBase;
        public string DataBase
        {
            get
            {
                return pDataBase;
            }
            set
            {
                pDataBase = value;
            }
        }

        //用户名
        private string pUser;
        public string User
        {
            get
            {
                return pUser;
            }
            set
            {
                pUser = value;
            }
        }

        //密码
        private string pPassword;
        public string Password
        {
            get
            {
                return pPassword;
            }
            set
            {
                pPassword = value;
            }
        }

        //版本
        private string pVersion;
        public string Version
        {
            get
            {
                return pVersion;
            }
            set
            {
                pVersion = value;
            }
        }

        //数据集名称
        private string pDataset;
        public string DataSet
        {
            get
            {
                return pDataset;
            }
            set
            {
                pDataset = value;
            }
        }

        #endregion
    }
    /// <summary>
    /// FID记录表
    /// </summary>
    public class FIDDataBaseInfo
    {
        //是否显示
        private bool pIsVisible;
        public bool IsVisible
        {
            get
            {
                return pIsVisible;
            }
            set
            {
                pIsVisible = value;
            }
        }

        //库体名称
        private string pName;
        public string Name
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }

        #region 连接信息
        //数据库类型
        private string pDBType;
        public string DBType
        {
            get
            {
                return pDBType;
            }
            set
            {
                pDBType = value;
            }
        }

        //服务器
        private string pServer;
        public string Server
        {
            get
            {
                return pServer;
            }
            set
            {
                pServer = value;
            }
        }

        //实例名
        private string pInstance;
        public string Instance
        {
            get
            {
                return pInstance;
            }
            set
            {
                pInstance = value;
            }
        }

        //数据库
        private string pDataBase;
        public string DataBase
        {
            get
            {
                return pDataBase;
            }
            set
            {
                pDataBase = value;
            }
        }

        //用户名
        private string pUser;
        public string User
        {
            get
            {
                return pUser;
            }
            set
            {
                pUser = value;
            }
        }

        //密码
        private string pPassword;
        public string Password
        {
            get
            {
                return pPassword;
            }
            set
            {
                pPassword = value;
            }
        }

        //版本
        private string pVersion;
        public string Version
        {
            get
            {
                return pVersion;
            }
            set
            {
                pVersion = value;
            }
        }

        //数据集名称
        private string pDataset;
        public string DataSet
        {
            get
            {
                return pDataset;
            }
            set
            {
                pDataset = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 图幅数据更新本地库
    /// </summary>
    public class LocalDataBaseInfo
    {
        //是否显示
        private bool pIsVisible;
        public bool IsVisible
        {
            get
            {
                return pIsVisible;
            }
            set
            {
                pIsVisible = value;
            }
        }

        //库体名称
        private string pName;
        public string Name
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }

        #region 连接信息
        //数据库类型
        private string pDBType;
        public string DBType
        {
            get
            {
                return pDBType;
            }
            set
            {
                pDBType = value;
            }
        }

        //服务器
        private string pServer;
        public string Server
        {
            get
            {
                return pServer;
            }
            set
            {
                pServer = value;
            }
        }

        //实例名
        private string pInstance;
        public string Instance
        {
            get
            {
                return pInstance;
            }
            set
            {
                pInstance = value;
            }
        }

        //数据库
        private string pDataBase;
        public string DataBase
        {
            get
            {
                return pDataBase;
            }
            set
            {
                pDataBase = value;
            }
        }

        //用户名
        private string pUser;
        public string User
        {
            get
            {
                return pUser;
            }
            set
            {
                pUser = value;
            }
        }

        //密码
        private string pPassword;
        public string Password
        {
            get
            {
                return pPassword;
            }
            set
            {
                pPassword = value;
            }
        }

        //版本
        private string pVersion;
        public string Version
        {
            get
            {
                return pVersion;
            }
            set
            {
                pVersion = value;
            }
        }

        //数据集名称
        private string pDataset;
        public string DataSet
        {
            get
            {
                return pDataset;
            }
            set
            {
                pDataset = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 更新库 ：现势库、历史库、工作库、配置库
    /// </summary>
    public class UpdateDataBaseInfo
    {
        //是否显示
        private bool pIsVisible;
        public bool IsVisible
        {
            get
            {
                return pIsVisible;
            }
            set
            {
                pIsVisible = value;
            }
        }

        //库体名称
        private string pName;
        public string Name
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }

        #region 连接信息
        //数据库类型
        private string pDBType;
        public string DBType
        {
            get
            {
                return pDBType;
            }
            set
            {
                pDBType = value;
            }
        }

        //服务器
        private string pServer;
        public string Server
        {
            get
            {
                return pServer;
            }
            set
            {
                pServer = value;
            }
        }

        //实例名
        private string pInstance;
        public string Instance
        {
            get
            {
                return pInstance;
            }
            set
            {
                pInstance = value;
            }
        }

        //数据库
        private string pDataBase;
        public string DataBase
        {
            get
            {
                return pDataBase;
            }
            set
            {
                pDataBase = value;
            }
        }

        //用户名
        private string pUser;
        public string User
        {
            get
            {
                return pUser;
            }
            set
            {
                pUser = value;
            }
        }

        //密码
        private string pPassword;
        public string Password
        {
            get
            {
                return pPassword;
            }
            set
            {
                pPassword = value;
            }
        }

        //版本
        private string pVersion;
        public string Version
        {
            get
            {
                return pVersion;
            }
            set
            {
                pVersion = value;
            }
        }

        #endregion
    }
}
