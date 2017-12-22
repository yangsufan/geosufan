using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using Fan.Common.Gis;
using Fan.Common.Error;
using Fan.Common.Authorize;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
namespace GeoUserManager
{
    public static class ModuleOperator
    {
        public static bool IsCheckChanged = true;//用于控制角色权限是否发生改变
        public static string GroupByName = "";//用于用户分组显示
        //从数据库向本地拷贝图层树
        public static bool CopyLayerTreeXmlFromDataBase(IWorkspace pWorkspace, string xmlpath)
        {
            Exception eError = null;
            SysGisTable sysTable = new SysGisTable(pWorkspace);
            //读取数据库表内容
            object objXml = sysTable.GetFieldValue("LAYERTREE_XML", "LAYERTREE", "NAME='LAYERTREE'", out eError);
            if (objXml == null)
            {
                return false;
            }
            //获取xml文档
            XmlDocument pXml = objXml as XmlDocument;
            //保存到本地
            pXml.Save(xmlpath);
            return true;
        }

        /// <summary>
        /// 将查询的用户组数据信息绑定树图
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tree"></param>
        /// <param name="gisDb"></param>
        public static void DisplayRoleTree(string condition, DevComponents.AdvTree.AdvTree tree, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            Role role = null;
            DevComponents.AdvTree.Node node = null;

            try
            {
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("role", condition, out exError);
                Dictionary<string, Role> dic = new Dictionary<string, Role>();
                if (lstDicData != null)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        role = new Role();
                        role.IDStr = lstDicData[i]["ROLEID"].ToString();
                        role.Name = lstDicData[i]["NAME"].ToString();
                        role.PROJECTID = lstDicData[i]["PROJECTID"].ToString();
                        role.TYPEID = lstDicData[i]["TYPEID"].ToString();
                        //role.Privilege = lstDicData[i]["PRIVILEGE"] as XmlDocument;
                        role.Remark = lstDicData[i]["REMARK"].ToString();
                        dic.Add(role.Name, role);
                    }
                    if (dic.Count > 0)
                    {
                        tree.Nodes.Clear();
                        node = new DevComponents.AdvTree.Node();
                        node.Text = "角色";
                        node.Name = node.Text;
                        node.Image = global::GeoUserManager.Properties.Resources.main;
                        tree.Nodes.Add(node);
                        node.Expanded = true;
                        foreach (string key in dic.Keys)
                        {
                            node = new DevComponents.AdvTree.Node();
                            role = dic[key] as Role;
                            //added by chulili 20111026 普通用户看不到超级管理员
                            if (role.Name.Equals("超级管理员") && role.TYPEID == "1")
                            {
                                if (ModData.v_AppPrivileges.ConnUser.Role != "超级管理员" || ModData.v_AppPrivileges.ConnUser.RoleTypeID != 1)
                                    continue;
                            }
                            //end added by chulili d20111026
                            node.Image = global::GeoUserManager.Properties.Resources.rulemanager;
                            node.Text = role.Name;
                            node.Name = role.IDStr;
                            node.Tag = role;
                            tree.Nodes[0].Nodes.Add(node);
                        }
                        //tree.Nodes[0].ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                exError = ex;
            }
        }

        /// <summary>
        /// 将查询的用户数据信息绑定树图
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tree"></param>
        /// <param name="gisDb"></param>
        public static void DisplayUserTree(string condition, DevComponents.AdvTree.AdvTree tree, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            User user = null;
            DevComponents.AdvTree.Node node = null;
            GroupByName = "科室"; //ygc 20130319 新增分科室显示用户
            try
            {

                SysGisTable sysTable = new SysGisTable(gisDb);
                if (GroupByName != "")
                {
                    IQueryFilter pQF = new QueryFilterClass();
                    pQF.WhereClause = condition;
                    ITable pUserTable = sysTable.OpenTable("user_info", out exError);
                    ICursor pCursor = pUserTable.Search(pQF,false);
                    IDataStatistics pDataStatistics = new DataStatisticsClass();
                    pDataStatistics.Cursor = pCursor;
                    if (GroupByName == "性别")
                        pDataStatistics.Field = "usex";
                    else if (GroupByName == "职称")
                        pDataStatistics.Field = "uposition";
                    else if(GroupByName == "科室")
                        pDataStatistics.Field = "USERDEPARTMENT";
                    System.Collections.IEnumerator pEnumerator = pDataStatistics.UniqueValues;
                    int iCount = 0;
                    while (pEnumerator.MoveNext())
                    {
                        iCount++;
                        object obj = pEnumerator.Current;
                        string vle = obj.ToString();
                        string sCondition = condition;
                        if (sCondition == "")
                            sCondition = string.Format("{0} = '{1}'", pDataStatistics.Field, vle);
                        else
                            sCondition += string.Format("and ({0} = '{1}')", pDataStatistics.Field, vle);
                        #region 分组时刷新过程
                        List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_info", sCondition, out exError);
                        Dictionary<string, User> dic = new Dictionary<string, User>();
                        if (lstDicData != null)
                        {
                            //判断是否存在ENDDATE 属性结构  20111102
                            bool bFlag = false;
                            //判断是否存在字段？？？？
                            if (lstDicData[0].ContainsKey("ENDDATE"))
                            {
                                bFlag = true;
                            }
                            for (int i = 0; i < lstDicData.Count; i++)
                            {
                                user = new User();
                                user.IDStr = lstDicData[i]["USERID"].ToString();
                                user.Name = lstDicData[i]["NAME"].ToString();
                                user.TrueName = lstDicData[i]["TRUTHNAME"].ToString();
                                user.Password = lstDicData[i]["UPWD"].ToString();
                                user.SexInt = int.Parse(lstDicData[i]["USEX"].ToString());
                                user.Position = lstDicData[i]["UPOSITION"].ToString();
                                user.Remark = lstDicData[i]["UREMARK"].ToString();
                                user.UserDepartment = lstDicData[i]["USERDEPARTMENT"].ToString();
                                if (bFlag == true)
                                {
                                    user.UserDate = lstDicData[i]["ENDDATE"].ToString(); //wgf 20111102
                                }

                                if (lstDicData[i]["EXPORTAREA"].ToString() != "")
                                {
                                    user.ExportArea = Convert.ToDouble(lstDicData[i]["EXPORTAREA"]);
                                }
                                else
                                {
                                    user.ExportArea = -1;
                                }
                                dic.Add(user.Name, user);
                            }
                            if (dic.Count > 0)
                            {
                                if (iCount == 1)//第一次循环清除，添加用户节点
                                {
                                    tree.Nodes.Clear();
                                    node = new DevComponents.AdvTree.Node();
                                    node.Image = global::GeoUserManager.Properties.Resources.main;
                                    node.Text = "用户";
                                    tree.Nodes.Add(node);
                                    node.Expanded = true;
                                }
                                //组节点
                                if (GroupByName == "性别")
                                {
                                    if (vle == "0")
                                        vle = "男";
                                    else
                                        vle = "女";
                                }
                                //ygc 20130319 通过科室的id获取科室名称
                                Dictionary<string, object> newdic = new Dictionary<string, object>(); ;
                                if (GroupByName == "科室")
                                {
                                    Exception ex=null ;
                                    newdic = sysTable.GetRow("USER_DEPARTMENT", "DEPARTMENTID='" + vle+"'", out ex);
                                }
                                //end ygc 20130319
                                DevComponents.AdvTree.Node nodeGp = new DevComponents.AdvTree.Node();
                                nodeGp.Image = global::GeoUserManager.Properties.Resources.rulemanager;
                              //  nodeGp.Text =string.Format("{0}:{1}", GroupByName, vle);
                                if (newdic.Count > 0)
                                {
                                    nodeGp.Text = newdic["DEPARTMENTNAME"].ToString();//ygc 20130320 修改显示状态
                                }
                                //ygc 20130320 新增组
                                if(newdic !=null&&newdic.Count !=0)
                                {
                                    nodeGp.TagString = vle;
                                }
                                //end
                                foreach (string key in dic.Keys)
                                {
                                    node = new DevComponents.AdvTree.Node();
                                    user = dic[key] as User;
                                    node.Image = global::GeoUserManager.Properties.Resources.usemanager;
                                    //   node.Text =user.TrueName+"@"+user.Name;
                                    if (bFlag == false)
                                    {
                                        node.Text = user.TrueName + "@" + user.Name;
                                    }
                                    else
                                    {
                                        if (user.UserDate != null)
                                        {
                                            if (user.UserDate.Equals(""))//20111102
                                            {
                                                node.Text = user.TrueName + "@" + user.Name;
                                            }
                                            else
                                            {
                                                //string strYear = user.UserDate.Substring(0, user.UserDate.IndexOf(" "));//wgf 20111102
                                                node.Text = user.TrueName + "@" + user.Name + "[" + user.UserDate + "]";
                                            }
                                        }

                                    }

                                    //end
                                    node.Tag = user;
                                    nodeGp.Nodes.Add(node);
                                }
                                tree.Nodes[0].Nodes.Add(nodeGp);
                                nodeGp.Expanded = true;
                                //tree.Nodes[0].ExpandAll();

                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    #region 未分组时刷新过程
                    List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_info", condition, out exError);
                    Dictionary<string, User> dic = new Dictionary<string, User>();
                    if (lstDicData != null)
                    {
                        //判断是否存在ENDDATE 属性结构  20111102
                        bool bFlag = false;
                        //判断是否存在字段？？？？
                        if (lstDicData[0].ContainsKey("ENDDATE"))
                        {
                            bFlag = true;
                        }
                        for (int i = 0; i < lstDicData.Count; i++)
                        {
                            user = new User();
                            user.IDStr = lstDicData[i]["USERID"].ToString();
                            user.Name = lstDicData[i]["NAME"].ToString();
                            user.TrueName = lstDicData[i]["TRUTHNAME"].ToString();
                            user.Password = lstDicData[i]["UPWD"].ToString();
                            user.SexInt = int.Parse(lstDicData[i]["USEX"].ToString());
                            user.Position = lstDicData[i]["UPOSITION"].ToString();
                            user.Remark = lstDicData[i]["UREMARK"].ToString();
                            if (bFlag == true)
                            {
                                user.UserDate = lstDicData[i]["ENDDATE"].ToString(); //wgf 20111102
                            }

                            if (lstDicData[i]["EXPORTAREA"].ToString() != "")
                            {
                                user.ExportArea = Convert.ToDouble(lstDicData[i]["EXPORTAREA"]);
                            }
                            else
                            {
                                user.ExportArea = -1;
                            }
                            dic.Add(user.Name, user);
                        }
                        if (dic.Count > 0)
                        {
                            tree.Nodes.Clear();
                            node = new DevComponents.AdvTree.Node();
                            node.Image = global::GeoUserManager.Properties.Resources.main;
                            node.Text = "用户";
                            tree.Nodes.Add(node);
                            node.Expanded = true;
                            foreach (string key in dic.Keys)
                            {
                                node = new DevComponents.AdvTree.Node();
                                user = dic[key] as User;
                                node.Image = global::GeoUserManager.Properties.Resources.usemanager;
                                //   node.Text =user.TrueName+"@"+user.Name;
                                if (bFlag == false)
                                {
                                    node.Text = user.TrueName + "@" + user.Name;
                                }
                                else
                                {
                                    if (user.UserDate != null)
                                    {
                                        if (user.UserDate.Equals(""))//20111102
                                        {
                                            node.Text = user.TrueName + "@" + user.Name;
                                        }
                                        else
                                        {
                                            //string strYear = user.UserDate.Substring(0, user.UserDate.IndexOf(" "));//wgf 20111102
                                            node.Text = user.TrueName + "@" + user.Name + "[" + user.UserDate + "]";
                                        }
                                    }

                                }

                                //end
                                node.Tag = user;
                                tree.Nodes[0].Nodes.Add(node);
                            }
                            //tree.Nodes[0].ExpandAll();
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                exError = ex;
            }
        }
        /// <summary>
        /// 将查询的用户数据信息绑定树图，列举选定角色下的用户
        /// </summary>
        public static void DisplayUserLstTree(DevComponents.AdvTree.AdvTree tree, ref SysGisDB gisDb, Role role, out Exception exError)
        {
            exError = null;
            User user = null;
            DevComponents.AdvTree.Node node = null;

            try
            {
                tree.Nodes.Clear();
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicU_R = sysTable.GetRows("user_role", string.Format("roleid='{0}'", role.IDStr), out exError);
                if (lstDicU_R == null || lstDicU_R.Count == 0)
                    return;
                string condition = "userid in(";
                foreach (Dictionary<string, object> dicO in lstDicU_R)
                {
                    condition += "'" + dicO["USERID"].ToString() + "',";
                }
                if (condition.EndsWith(","))
                {
                    condition = condition.Substring(0, condition.Length - 1) + ")";
                }
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_info", condition, out exError);
                Dictionary<string, User> dic = new Dictionary<string, User>();
                if (lstDicData != null)
                {
                    //判断是否存在ENDDATE 属性结构  20111102
                    bool bFlag = false;
                    //判断是否存在字段？？？？
                    if (lstDicData[0].ContainsKey("ENDDATE"))
                    {
                        bFlag = true;
                    }
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        user = new User();
                        user.IDStr = lstDicData[i]["USERID"].ToString();
                        user.Name = lstDicData[i]["NAME"].ToString();
                        user.TrueName = lstDicData[i]["TRUTHNAME"].ToString();
                        user.Password = lstDicData[i]["UPWD"].ToString();
                        user.SexInt = int.Parse(lstDicData[i]["USEX"].ToString());
                        user.Position = lstDicData[i]["UPOSITION"].ToString();
                        user.Remark = lstDicData[i]["UREMARK"].ToString();
                        if (bFlag == true)
                        {
                            user.UserDate = lstDicData[i]["ENDDATE"].ToString(); //wgf 20111102
                        }

                        if (lstDicData[i]["EXPORTAREA"].ToString() != "")
                        {
                            user.ExportArea = Convert.ToDouble(lstDicData[i]["EXPORTAREA"]);
                        }
                        else
                        {
                            user.ExportArea = -1;
                        }
                        dic.Add(user.Name, user);
                    }
                    if (dic.Count > 0)
                    {
                        tree.Nodes.Clear();
                        node = new DevComponents.AdvTree.Node();
                        node.Image = global::GeoUserManager.Properties.Resources.main;
                        node.Text = "用户";
                        tree.Nodes.Add(node);
                        node.Expanded = true;
                        foreach (string key in dic.Keys)
                        {
                            node = new DevComponents.AdvTree.Node();
                            user = dic[key] as User;
                            node.Image = global::GeoUserManager.Properties.Resources.usemanager;
                            //   node.Text =user.TrueName+"@"+user.Name;
                            if (bFlag == false)
                            {
                                node.Text = user.TrueName + "@" + user.Name;
                            }
                            else
                            {
                                if (user.UserDate != null)
                                {
                                    if (user.UserDate.Equals(""))//20111102
                                    {
                                        node.Text = user.TrueName + "@" + user.Name;
                                    }
                                    else
                                    {
                                        //string strYear = user.UserDate.Substring(0, user.UserDate.IndexOf(" "));//wgf 20111102
                                        node.Text = user.TrueName + "@" + user.Name + "[" + user.UserDate + "]";
                                    }
                                }

                            }

                            //end
                            node.Tag = user;
                            tree.Nodes[0].Nodes.Add(node);
                        }
                        //tree.Nodes[0].ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                exError = ex;
            }
        }
        //将数据源显示在角色权限的数据源列表里面
        public static void DisplayInDbsourceLstView(DevComponents.AdvTree.AdvTree tree)
        {
            tree.Nodes.Clear();
            DevComponents.AdvTree.Node rootnode = new DevComponents.AdvTree.Node();
            rootnode.Image = global::GeoUserManager.Properties.Resources.main;
            rootnode.Text = "数据源列表";
            rootnode.Name = "0";
            DevComponents.AdvTree.Cell rootcell = new DevComponents.AdvTree.Cell();
            rootcell.CheckBoxVisible = true;
            rootcell.Checked = false;
            rootcell.Tag = "Visible";
            rootnode.Cells.Add(rootcell);
            rootnode.Tag = rootcell.Checked;
            rootnode.Expanded = true;
            tree.Nodes.Add(rootnode);
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            Exception exError;
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows("DATABASEMD", "", out exError);
            //Dictionary<string, Role> dic = new Dictionary<string, Role>();
            if (lstDicData != null)
            {
                for (int i = 0; i < lstDicData.Count; i++)
                {
                    string dbname = lstDicData[i]["DATABASENAME"].ToString();
                    string dbid = lstDicData[i]["ID"].ToString();

                    DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                    aNode.Image = global::GeoUserManager.Properties.Resources.node;
                    aNode.Text = dbname;
                    aNode.Name = dbid;
                    DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                    cell.CheckBoxVisible = true;
                    cell.Checked = false;
                    cell.Tag = "Visible";
                    aNode.Cells.Add(cell);
                    aNode.Tag = cell.Checked;
                    aNode.Expanded = true;
                    rootnode.Nodes.Add(aNode);
                }
            }

        }
        /// <summary>
        /// 将数据权限文档显示在listView上，区别于功能权限
        /// </summary>
        /// <param name="document"></param>
        /// <param name="view"></param>
        public static void DisplayInDataLstView(XmlDocument document, DevComponents.AdvTree.AdvTree tree)
        {
            if (document.DocumentElement != null)
            {
                tree.Nodes.Clear();
                tree.Tag = document;
                string xPath = "//Root";
                XmlNode rootNode = document.SelectSingleNode(xPath);
                //显示根节点
                XmlElement pElementRoot = rootNode as XmlElement;
                string captionroot = pElementRoot.GetAttribute("NodeText") == null ? "" : pElementRoot.GetAttribute("NodeText");
                string strIDroot = pElementRoot.GetAttribute("NodeKey") == null ? "" : pElementRoot.GetAttribute("NodeKey");

                DevComponents.AdvTree.Node aNoderoot = new DevComponents.AdvTree.Node();

                //aNoderoot.Image = global::GeoUserManager.Properties.Resources.main ;
                aNoderoot.Text = captionroot;
                aNoderoot.Name = strIDroot;
                DevComponents.AdvTree.Cell cellroot = new DevComponents.AdvTree.Cell();
                cellroot.CheckBoxVisible = true;
                cellroot.Checked = false;
                cellroot.Tag = "Visible";
                aNoderoot.Cells.Add(cellroot);
                aNoderoot.Tag = cellroot.Checked;
                aNoderoot.Expanded = true;
                tree.Nodes.Add(aNoderoot);
                SetDataNodeImage(aNoderoot, rootNode);//chulili 20110709添加到树上之后，再设置图标，因为设置图标时用到其所在树的imagelist
                XmlNodeList nodeList = rootNode.ChildNodes;
                if (nodeList == null) return;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name.Equals("ConfigInfo"))
                    {
                        continue;
                    }
                    XmlElement pElement = node as XmlElement;
                    string caption = pElement.GetAttribute("NodeText") == null ? "" : pElement.GetAttribute("NodeText");
                    string strID = pElement.GetAttribute("NodeKey") == null ? "" : pElement.GetAttribute("NodeKey");
                    //string strNodeType = pElement.GetAttribute("FeatureType").ToString();

                    DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();

                    //aNode.Image = global::GeoUserManager.Properties.Resources.node;
                    aNode.Text = caption;
                    aNode.Name = strID;
                    DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                    cell.CheckBoxVisible = true;
                    cell.Checked = false;
                    cell.Tag = "Visible";
                    aNode.Cells.Add(cell);
                    aNode.Tag = cell.Checked;
                    aNode.Expanded = true;
                    aNoderoot.Nodes.Add(aNode);
                    SetDataNodeImage(aNode, node);//chulili 20110709添加到树上之后，再设置图标，因为设置图标时用到其所在树的imagelist
                    if (node.HasChildNodes )//&& !node.Name.Equals("Layer"))
                    {
                        DisPlaySubDataNodeView(node, aNode);
                    }
                }
            }
        }
        //设置图层树节点图标 added by chulili 20110709
        private static void SetDataNodeImage(DevComponents.AdvTree.Node treenode, XmlNode pXmlnode)
        {
            if (treenode == null)
                return;
            if (pXmlnode == null)
                return;
            switch (pXmlnode.Name)
            {
                case "Root":
                    treenode.Image = treenode.TreeControl.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "DIR":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DIR"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = treenode.TreeControl.ImageList.Images["DataDIROpen"];
                    break;
                case "Layer":
                    if (pXmlnode.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = pXmlnode.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = treenode.TreeControl.ImageList.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = treenode.TreeControl.ImageList.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = treenode.TreeControl.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = treenode.TreeControl.ImageList.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = treenode.TreeControl.ImageList.Images["RD"];
                    break;
            }

        }
        /// <summary>
        /// 将数据子结点显示到listView上
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="View"></param>
        private static void DisPlaySubDataNodeView(XmlNode xNode, DevComponents.AdvTree.Node treeNode)
        {
            foreach (XmlNode node in xNode.ChildNodes)
            {
                XmlElement pElement = node as XmlElement;
                if (pElement == null) return;
                if (node.Name == "AboutShow" || node.Name == "AttrLabel" || node.Name == "ShowDefine" || node.Name == "AboutCADShow")
                {
                    continue;
                }
                string caption = pElement.GetAttribute("NodeText") == null ? "" : pElement.GetAttribute("NodeText");
                string strID = pElement.GetAttribute("NodeKey") == null ? "" : pElement.GetAttribute("NodeKey");

                DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();

                //aNode.Image = global::GeoUserManager.Properties.Resources.node;
                aNode.Text = caption;
                aNode.Name = strID;
                DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                cell.CheckBoxVisible = true;
                cell.Checked = false;
                cell.Tag = "Visible";
                aNode.Cells.Add(cell);
                aNode.Tag = cell.Checked;
                treeNode.Nodes.Add(aNode);
                SetDataNodeImage(aNode, node);//chulili 20110709添加到树上之后，再设置图标，因为设置图标时用到其所在树的imagelist
                if (node.HasChildNodes)// && node.Name != "Layer")
                {
                    DisPlaySubDataNodeView(node, aNode);
                }
            }
        }

        /// <summary>
        /// 将用户组添加到可用权限中
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tree"></param>
        /// <param name="gisDb"></param>
        public static void DisplayRoleLstView(string condition, DevComponents.DotNetBar.Controls.ListViewEx view, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            bool result = false;
            Role role = null;

            try
            {
                if (gisDb == null)
                {
                    gisDb = new SysGisDB();
                    result = gisDb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out exError);
                    if (!result) return;
                }
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("role", condition, out exError);
                Dictionary<string, Role> dic = new Dictionary<string, Role>();
                if (lstDicData != null)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        role = new Role();
                        role.IDStr = lstDicData[i]["ROLEID"].ToString();
                        role.Name = lstDicData[i]["NAME"].ToString();
                        role.PROJECTID = lstDicData[i]["PROJECTID"].ToString();
                        role.TYPEID = lstDicData[i]["TYPEID"].ToString();
                        //role.Privilege = lstDicData[i]["PRIVILEGE"] as XmlDocument;
                        role.Remark = lstDicData[i]["REMARK"].ToString();
                        dic.Add(lstDicData[i]["ROLEID"].ToString(), role);
                    }
                    if (dic.Count > 0)
                    {
                        view.Items.Clear();
                        ListViewItem item = null;
                        foreach (string key in dic.Keys)
                        {
                            item = new ListViewItem();
                            role = dic[key] as Role;
                            //added by chulili 20111026 普通用户看不到超级管理员
                            if (role.Name.Equals("超级管理员") && role.TYPEID == "1")
                            {
                                if (ModData.v_AppPrivileges.ConnUser.Role != "超级管理员" || ModData.v_AppPrivileges.ConnUser.RoleTypeID != 1)
                                    continue;
                            }
                            //end added by chulili d20111026
                            item.Text = role.Name;
                            item.Tag = role;
                            view.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exError = ex;
            }
        }

        /// <summary>
        /// 获取用户角色对照表中给定用户的角色ID集
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        public static List<string> GetRoleIds(string userid, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            bool result = false;

            try
            {
                if (gisDb == null)
                {
                    gisDb = new SysGisDB();
                    result = gisDb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out exError);
                    if (!result) return null;
                }
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_role", "USERID='" + userid + "'", out exError);
                List<string> ids = null;
                if (lstDicData != null && lstDicData.Count > 0)
                {
                    ids = new List<string>();
                    foreach (Dictionary<string, object> dic in lstDicData)
                    {
                        foreach (string key in dic.Keys)
                        {
                            if (key.Equals("ROLEID"))
                            {
                                ids.Add(dic[key].ToString());
                            }
                        }
                    }
                }
                return ids;
            }
            catch (Exception ex)
            {
                exError = ex;
                return null;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        public static bool DeleteData(string tableName, string fieldName, string id, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            bool result = false;

            try
            {
                result = gisDb.StartTransaction(out exError);
                if (!result) return false;
                SysGisTable sysTable = new SysGisTable(gisDb);
                result = sysTable.DeleteRows(tableName, fieldName + "='" + id + "'", out exError);
                string condition = "";
                if (tableName.Equals("role"))
                {
                    condition = "ROLEID='" + id + "'";
                }
                else
                {
                    condition = "USERID='" + id + "'";
                }
                if (tableName.Equals("role"))
                {
                    result = sysTable.DeleteRows("role_pri", condition, out exError);
                }
                result = sysTable.DeleteRows("user_role", condition, out exError);
                sysTable.EndTransaction(result, out exError);
                return result;
            }
            catch (Exception ex)
            {
                exError = ex;
                return false;
            }
        }

        /// <summary>
        /// 为角色添加权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="document"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        public static bool AddPrivilege(Role role, XmlDocument document, ref SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            bool result = false;

            try
            {
                result = gisDb.StartTransaction(out exError);
                if (!result) return false; ;
                SysGisTable sysTable = new SysGisTable(gisDb);
                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                byte[] bytes = Encoding.Default.GetBytes(document.OuterXml);
                pBlobStream.ImportFromMemory(ref bytes[0], (uint)bytes.GetLength(0));
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData.Add("privilege", pBlobStream);
                result = sysTable.UpdateRow("role", "roleid='" + role.ID + "'", dicData, out exError);
                gisDb.EndTransaction(result, out exError);
                //将修改后的权限文档保存给相应的树结点
                IMemoryBlobStreamVariant var = pBlobStream as IMemoryBlobStreamVariant;
                object tempObj = null;
                if (var != null)
                {
                    var.ExportToVariant(out tempObj);
                    XmlDocument doc = new XmlDocument();
                    byte[] btyes = (byte[])tempObj;
                    string xml = Encoding.Default.GetString(btyes);
                    doc.LoadXml(xml);
                    role.Privilege = doc;
                }
                return result;
            }
            catch (Exception ex)
            {
                exError = ex;
                return false;
            }
        }

        /// <summary>
        /// 将权限文档显示在listView上
        /// </summary>
        /// <param name="document"></param>
        /// <param name="view"></param>
        public static void DisplayInLstView(XmlDocument document, DevComponents.AdvTree.AdvTree tree)
        {
            if (document.DocumentElement != null)
            {
                tree.Nodes.Clear();
                tree.Tag = document;
                string xPath = ".//System[@ControlType='UserControl']";
                XmlNode rootNode = document.DocumentElement;
                XmlNodeList nodeList = rootNode.SelectNodes(xPath);
                if (nodeList == null) return;
                foreach (XmlNode node in nodeList)
                {
                    XmlElement pElement = node as XmlElement;
                    string caption = pElement.GetAttribute("Caption") == null ? "" : pElement.GetAttribute("Caption");
                    string strID = pElement.GetAttribute("ID") == null ? "" : pElement.GetAttribute("ID");

                    DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                    aNode.Image = global::GeoUserManager.Properties.Resources.main;
                    aNode.Text = caption + " 子系统";
                    aNode.Name = strID;
                    DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                    cell.CheckBoxVisible = true;
                    cell.Checked = false;
                    cell.Tag = "Visible";
                    aNode.Cells.Add(cell);
                    aNode.Tag = cell.Checked;
                    aNode.Expanded = true;
                    tree.Nodes.Add(aNode);
                    if (node.HasChildNodes)
                    {
                        DisPlaySubNodeView(node, aNode);
                    }
                }
            }
        }

        /// <summary>
        /// 将子结点显示到listView上
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="View"></param>
        private static void DisPlaySubNodeView(XmlNode xNode, DevComponents.AdvTree.Node treeNode)
        {
            foreach (XmlNode node in xNode.ChildNodes)
            {
                XmlElement pElement = node as XmlElement;
                if (pElement == null) return;
                string caption = pElement.GetAttribute("Caption") == null ? "" : pElement.GetAttribute("Caption");
                string strID = pElement.GetAttribute("ID") == null ? "" : pElement.GetAttribute("ID");

                DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                aNode.Image = global::GeoUserManager.Properties.Resources.node;
                aNode.Text = caption;
                aNode.Name = strID;
                DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                cell.CheckBoxVisible = true;
                cell.Checked = false;
                cell.Tag = "Visible";
                aNode.Cells.Add(cell);
                aNode.Tag = cell.Checked;
                treeNode.Nodes.Add(aNode);
                if (node.HasChildNodes)
                {
                    DisPlaySubNodeView(node, aNode);
                }
            }
        }

        /// <summary>
        /// 根据选择的角色更新权限
        /// </summary>
        /// <param name="lstPrivilegeID"></param>
        /// <param name="treeNode"></param>
        public static void UpdateRolePrivilege(List<string> lstPrivilegeID, DevComponents.AdvTree.Node treeNode)
        {
            if (lstPrivilegeID.Count != 0)
            {
                for (int i = 0; i < treeNode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node childNode = treeNode.Nodes[i];
                    IsCheckChanged = false;
                    if (lstPrivilegeID.Contains(childNode.Name))
                    {
                        childNode.Cells[1].Checked = true;
                    }
                    else
                    {
                        childNode.Cells[1].Checked = false;
                    }
                    if (childNode.Tag.ToString() == childNode.Cells[1].Checked.ToString())
                    {
                        IsCheckChanged = true;
                    }
                    else
                    {
                        childNode.Tag = childNode.Cells[1].Checked;
                    }
                    if (childNode.HasChildNodes)
                    {
                        UpdateRolePrivilege(lstPrivilegeID, childNode);
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeNode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node childNode = treeNode.Nodes[i];
                    IsCheckChanged = false;
                    childNode.Cells[1].Checked = false;

                    if (childNode.Tag.ToString() == childNode.Cells[1].Checked.ToString())
                    {
                        IsCheckChanged = true;
                    }
                    else
                    {
                        childNode.Tag = childNode.Cells[1].Checked;
                    }
                    if (childNode.HasChildNodes)
                    {
                        UpdateRolePrivilege(lstPrivilegeID, childNode);
                    }
                }
            }
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="ObjNode"></param>
        public static void MovUp(DevComponents.AdvTree.AdvTree tree, DevComponents.AdvTree.Node ObjNode)
        {
            //----节点的向上移动   
            if (ObjNode != null)
            {
                DevComponents.AdvTree.Node newnode = new DevComponents.AdvTree.Node();
                //--------如果选中节点为最顶节点   
                if (ObjNode.Index == 0)
                {
                    //-------------   
                }
                else if (ObjNode.Index != 0)
                {
                    newnode = (DevComponents.AdvTree.Node)ObjNode.Copy();
                    //-------------若选中节点为根节点
                    if (ObjNode.Level == 0)
                    {
                        tree.Nodes.Insert(ObjNode.PrevNode.Index, newnode);
                    }
                    //-------------若选中节点并非根节点   
                    else if (ObjNode.Level != 0)
                    {
                        ObjNode.Parent.Nodes.Insert(ObjNode.PrevNode.Index, newnode);
                    }
                    ObjNode.Remove();
                    ObjNode = newnode;
                }
                tree.SelectedNode = ObjNode;
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="ObjNode"></param>
        public static void MovDown(DevComponents.AdvTree.AdvTree tree, DevComponents.AdvTree.Node ObjNode)
        {
            //----节点的向下移动   
            if (ObjNode != null)
            {
                DevComponents.AdvTree.Node newnode = new DevComponents.AdvTree.Node();
                //-------------如果选中的是根节点   
                if (ObjNode.Level == 0)
                {
                    //---------如果选中节点为最底节点   
                    if (ObjNode.Index == ObjNode.Parent.Nodes.Count - 1)
                    {
                        //---------------   
                    }
                    //---------如果选中的不是最底的节点   
                    else
                    {
                        newnode = (DevComponents.AdvTree.Node)ObjNode.Copy();
                        tree.Nodes.Insert(ObjNode.NextNode.Index + 1, newnode);
                        ObjNode.Remove();
                        ObjNode = newnode;
                    }
                }
                //-------------如果选中节点不是根节点   
                else if (ObjNode.Level != 0)
                {
                    //---------如果选中最底的节点   
                    if (ObjNode.Index == ObjNode.Parent.Nodes.Count - 1)
                    {
                        //-----------   
                    }
                    //---------如果选中的不是最低的节点   
                    else
                    {
                        newnode = (DevComponents.AdvTree.Node)ObjNode.Copy();
                        ObjNode.Parent.Nodes.Insert(ObjNode.NextNode.Index + 1, newnode);
                        ObjNode.Remove();
                        ObjNode = newnode;
                    }
                }
                tree.SelectedNode = ObjNode;
            }
        }

        /// <summary>
        /// 从XML获取数据库连接配置文件
        /// </summary>
        /// <param name="strPath"></param>
        public static bool GetSettingXml(string strPath)
        {
            if (string.IsNullOrEmpty(strPath)) return false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strPath);
                if (doc.DocumentElement != null)
                {
                    XmlElement pElement = doc.DocumentElement;
                    SdeConfig.Server = pElement["server"].InnerText;
                    SdeConfig.Instance = pElement["service"].InnerText;
                    SdeConfig.Database = pElement["database"].InnerText;
                    SdeConfig.User = pElement["user"].InnerText;
                    SdeConfig.Password = pElement["password"].InnerText;
                    SdeConfig.Version = pElement["version"].InnerText;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 拷贝角色，新角色添加复件后缀
        /// </summary>
        public static void CopyGroup(DevComponents.AdvTree.Node ObjNode)
        {
            Role roleSrc = ObjNode.Tag as Role;
            if (roleSrc == null) return;
            Exception exError;
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            ModData.gisDb.StartTransaction(out exError);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            string sNewName = roleSrc.Name + "复件";
            string sNewID = roleSrc.Name + DateTime.Now.ToString("yyyymmddhhss");
            dicData.Add("roleid", sNewID);
            dicData.Add("name", sNewName);
            dicData.Add("remark", roleSrc.Remark);
            dicData.Add("TYPEID", roleSrc.TYPEID);
            dicData.Add("PROJECTID", roleSrc.PROJECTID);
            if (sysTable.ExistData("role", "name='" + sNewName + "'"))
            {
                ErrorHandle.ShowFrmErrorHandle("提示", string.Format("已存在相同的角色名【{0}】！", sNewName));
                return;
            }
            if (!sysTable.NewRow("role", dicData, out exError))
            {
                if (exError != null)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                }
                else
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！");
                }
                return;
            }
            //功能权限
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows("role_pri", "ROLEID='" + roleSrc.IDStr + "'", out exError);
            if (exError != null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                return;
            }
            foreach (Dictionary<string, object> dic in lstDicData)
            {
                dic["ROLEID"] = sNewID;
                sysTable.NewRow("role_pri", dic, out exError);
                if (exError != null)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                    return;
                }
            }
            //数据权限设置
            List<Dictionary<string, object>> lstDicDataData = sysTable.GetRows("role_data", "ROLEID='" + roleSrc.IDStr + "'", out exError);
            if (exError != null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                return;
            }
            foreach (Dictionary<string, object> dic in lstDicDataData)
            {
                dic["ROLEID"] = sNewID;
                sysTable.NewRow("role_data", dic, out exError);
                if (exError != null)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                    return;
                }
            }
            //数据源权限设置
            List<Dictionary<string, object>> lstDicDbsource = sysTable.GetRows("role_dbsource", "ROLEID='" + roleSrc.IDStr + "'", out exError);
            if (exError != null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                return;
            }
            foreach (Dictionary<string, object> dic in lstDicDbsource)
            {
                dic["ROLEID"] = sNewID;
                sysTable.NewRow("role_dbsource", dic, out exError);
                if (exError != null)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                    return;
                }
            }
            ModData.gisDb.EndTransaction(true, out exError);
            if (exError != null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "复制失败！" + exError.Message);
                return;
            }
            DevComponents.AdvTree.Node newNode = ObjNode.Copy();
            ObjNode.Parent.Nodes.Insert(ObjNode.Index, newNode);
        }
    }
    public static class ModDBOperate
    {

        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Fan.Plugin.ModuleCommon.DicTabs == null || Fan.Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Fan.Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Fan.Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Fan.Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Fan.Plugin.Interface.IControlRef> keyValue in Fan.Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Fan.Plugin.Interface.ICommandRef pCmd = keyValue.Value as Fan.Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevComponents.DotNetBar.RibbonTabItem, string> keyValue in Fan.Plugin.ModuleCommon.DicTabs)
            {
                if (keyValue.Value == pSysName)
                {
                    i = i + 1;
                    keyValue.Key.Visible = true;
                    keyValue.Key.Enabled = true;
                    if (i == 1)
                    {
                        //默认选中第一项
                        keyValue.Key.Checked = true;
                    }
                }
                else
                {
                    keyValue.Key.Visible = false;
                    keyValue.Key.Enabled = false;
                }
            }
        }
    }
}
