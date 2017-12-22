using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Fan.Common.Gis;
using Fan.Common.Error;
using Fan.Common.Authorize;
using ESRI.ArcGIS.esriSystem;
using Fan.Common;

namespace GeoUserManager
{
    public partial class UCRole : UserControl
    {
        //右键菜单集合
        private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;
        enumWSType _curWsType = enumWSType.SDE;
        public string UCtag = "Role";
        public UCRole(string strName, string strCaption)
        {
            InitializeComponent();

            this.Dock = System.Windows.Forms.DockStyle.Fill;

            this.Name = strName;
            this.Tag = strCaption;
            this.roleTree.ImageList = IconContainer;

            ModData.v_AppPrivileges.MainUserControl = this;
            ModData.v_AppPrivileges.RoleTree = this.roleTree;
            ModData.v_AppPrivileges.UserTree = this.userTree;
            ModData.v_AppPrivileges.PrivilegeTree = this.priTree;
            
            InitListTree();
            //初始化框架插件控件界面
            InitialFrmDefControl();
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 5;
            this.splitContainer2.SplitterDistance = this.splitContainer2.Width / 5;
            this.splitContainer1.Visible = true;
            this.splitContainer2.Visible = false;
        }
        public void ChangeUC(string strTag)
        {
            Exception eError = null;
            if (strTag == "User")
            {
                ModuleOperator.DisplayUserTree("", userTree, ref ModData.gisDb, out eError);
                this.splitContainer2.Visible = true;               
                this.splitContainer1.Visible = false;
                UCtag = strTag;
            }
            else
            {
                //ModuleOperator.DisplayRoleTree("", roleTree, ref ModData.gisDb, out eError);
                RefreshDataList();
                this.splitContainer1.Visible = true;
                this.splitContainer2.Visible = false;
                UCtag = strTag;
            }
 
        }
        //初始化框架插件控件界面
        private void InitialFrmDefControl()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Fan.Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModData.v_AppPrivileges.ControlContainer, xPath, ModData.v_AppPrivileges);

            _dicContextMenu = ModData.v_AppPrivileges.DicContextMenu;


            //加载所有权限
            XmlDocument doc = new XmlDocument();
            doc.Load(ModData.m_SysXmlPath);

            if (doc != null)
            {
                //将xml权限文档显示在权限树上
                ModuleOperator.DisplayInLstView(doc, priTree);
            }
            //将图层目录从工作库拷贝到本地目录
            ModuleOperator.CopyLayerTreeXmlFromDataBase(Fan.Plugin.ModuleCommon.TmpWorkSpace, ModData.m_DataXmlPath);
            XmlDocument datadoc = new XmlDocument();
            datadoc.Load(ModData.m_DataXmlPath);
            if (datadoc != null)
            {
                ModuleOperator.DisplayInDataLstView(datadoc, DataTree);
            }

            ModuleOperator.DisplayInDbsourceLstView(this.dbSourceTree);
        }
        private void InitListTree()
        {
            Exception eError;

            bool result = false;
            //生成数据库操作对象
            if (ModData.gisDb == null)
            {
                switch (SdeConfig.dbType)
                {
                    case "ORACLE":
                    case "SQLSERVER":
                        _curWsType = enumWSType.SDE;
                        break;
                    case "ACCESS":
                        _curWsType = enumWSType.PDB;
                        break;
                    case "FILE":
                        _curWsType = enumWSType.GDB;
                        break;
                }
                ModData.gisDb = new SysGisDB();
                switch (_curWsType)
                {
                    case enumWSType.SDE:
                        result = ModData.gisDb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out eError);
                        break;
                    case enumWSType.PDB:
                    case enumWSType.GDB:
                        result = ModData.gisDb.SetWorkspace(SdeConfig.Server, _curWsType, out eError);
                        break;
                    default:
                        break;
                }
                if (!result) return;
                ModuleOperator.DisplayRoleTree("", roleTree, ref ModData.gisDb, out eError);
                if (eError != null)
                {
                    ErrorHandle.ShowInform("提示", eError.Message);
                    return;
                }
                //roleManagerPanel.Visible = true;
                //ModData.v_AppPrivileges.CurrentPanel = roleManagerPanel;
            }
        }

        private void roleTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            roleTree.SelectedNode = e.Node;
        }

        private void roleTree_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || _dicContextMenu == null)
                return;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.X, e.Y);
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuTree"))
            {
                if (_dicContextMenu["ContextMenuTree"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(roleTree.PointToScreen(pPoint));
                    }
                }
            }
        }

        private void roleTree_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            Exception eError;
            if (roleTree.SelectedNode == null)
            {
                return;
            }
            DevComponents.AdvTree.Node selectNode = roleTree.SelectedNode;
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            bool result = false;
            //生成数据库操作对象
            if (ModData.gisDb == null)
            {
                switch (SdeConfig.dbType)
                {
                    case "ORACLE":
                    case "SQLSERVER":
                        _curWsType = enumWSType.SDE;
                        break;
                    case "ACCESS":
                        _curWsType = enumWSType.PDB;
                        break;
                    case "FILE":
                        _curWsType = enumWSType.GDB;
                        break;
                }
                ModData.gisDb = new SysGisDB();
                switch (_curWsType)
                {
                    case enumWSType.SDE:
                        result = ModData.gisDb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out eError);
                        break;
                    case enumWSType.PDB:
                    case enumWSType.GDB:
                        result = ModData.gisDb.SetWorkspace(SdeConfig.Server, _curWsType, out eError);
                        break;
                    default:
                        break;
                }
                if (!result) return;
            }
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows("role_pri", "ROLEID='" + selectNode.Name + "'", out eError);
            List<string> lstPrivilege = new List<string>();
            if (lstDicData != null)
            {
                if (lstDicData.Count != 0)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        lstPrivilege.Add(lstDicData[i]["PRIVILEGE_ID"].ToString());
                    }
                    for (int i = 0; i < priTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = priTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        if (lstPrivilege.Contains(treeNode.Name))
                        {
                            treeNode.Cells[1].Checked = true;
                        }
                        else
                        {
                            treeNode.Cells[1].Checked = false;
                        }
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstPrivilege, treeNode);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < priTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = priTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        treeNode.Cells[1].Checked = false;
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstPrivilege, treeNode);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < priTree.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node treeNode = priTree.Nodes[i];
                    ModuleOperator.IsCheckChanged = false;
                    treeNode.Cells[1].Checked = false;
                    if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                    {
                        ModuleOperator.IsCheckChanged = true;
                    }
                    else
                    {
                        treeNode.Tag = treeNode.Cells[1].Checked;
                    }
                    if (treeNode.HasChildNodes)
                    {
                        ModuleOperator.UpdateRolePrivilege(lstPrivilege, treeNode);
                    }
                }
            }
            //数据权限设置
            List<Dictionary<string, object>> lstDicDataData = sysTable.GetRows("role_data", "ROLEID='" + selectNode.Name + "'", out eError);
            List<string> lstdataPrivilege = new List<string>();
            if (lstDicDataData != null)
            {
                if (lstDicDataData.Count != 0)
                {
                    for (int i = 0; i < lstDicDataData.Count; i++)
                    {
                        lstdataPrivilege.Add(lstDicDataData[i]["DATAPRI_ID"].ToString());
                    }
                    for (int i = 0; i < DataTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = DataTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        if (lstdataPrivilege.Contains(treeNode.Name))
                        {
                            treeNode.Cells[1].Checked = true;
                        }
                        else
                        {
                            treeNode.Cells[1].Checked = false;
                        }
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstdataPrivilege, treeNode);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < DataTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = DataTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        treeNode.Cells[1].Checked = false;
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstdataPrivilege, treeNode);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < DataTree.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node treeNode = DataTree.Nodes[i];
                    ModuleOperator.IsCheckChanged = false;
                    treeNode.Cells[1].Checked = false;
                    if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                    {
                        ModuleOperator.IsCheckChanged = true;
                    }
                    else
                    {
                        treeNode.Tag = treeNode.Cells[1].Checked;
                    }
                    if (treeNode.HasChildNodes)
                    {
                        ModuleOperator.UpdateRolePrivilege(lstdataPrivilege, treeNode);
                    }
                }
            }
            //数据源权限设置
            List<Dictionary<string, object>> lstDicDbsource = sysTable.GetRows("role_dbsource", "ROLEID='" + selectNode.Name + "'", out eError);
            List<string> lstdbsource = new List<string>();
            if (lstDicDbsource != null)
            {
                if (lstDicDbsource.Count != 0)
                {
                    for (int i = 0; i < lstDicDbsource.Count; i++)
                    {
                        lstdbsource.Add(lstDicDbsource[i]["DBSOURCE_ID"].ToString());
                    }
                    for (int i = 0; i < dbSourceTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = dbSourceTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        if (lstdbsource.Contains(treeNode.Name))
                        {
                            treeNode.Cells[1].Checked = true;
                        }
                        else
                        {
                            treeNode.Cells[1].Checked = false;
                        }
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstdbsource, treeNode);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dbSourceTree.Nodes.Count; i++)
                    {
                        DevComponents.AdvTree.Node treeNode = dbSourceTree.Nodes[i];
                        ModuleOperator.IsCheckChanged = false;
                        treeNode.Cells[1].Checked = false;
                        if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                        {
                            ModuleOperator.IsCheckChanged = true;
                        }
                        else
                        {
                            treeNode.Tag = treeNode.Cells[1].Checked;
                        }
                        if (treeNode.HasChildNodes)
                        {
                            ModuleOperator.UpdateRolePrivilege(lstdbsource, treeNode);
                        }
                    }
                }
            }
            if (roleTree.SelectedNode.Level != 0)
            {
                Role role=roleTree.SelectedNode.Tag as Role;
                if(role==null)return;
                ModuleOperator.DisplayUserLstTree(userLstTree, ref ModData.gisDb, role, out eError);
            }
        }

        private void MenudataSelectAll_Click(object sender, EventArgs e)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (roleTree.SelectedNode.Name == "角色")
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            DevComponents.AdvTree.Node aNode = DataTree.SelectedNode;

            SelectAlldataNode(sysTable, aNode);
        }

        private void MenuDataunSelectall_Click(object sender, EventArgs e)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (roleTree.SelectedNode.Name == "角色")
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            DevComponents.AdvTree.Node aNode = DataTree.SelectedNode;

            unSelectAlldataNode(sysTable, aNode);
        }
        public void RefreshDataList()
        {
            //将图层目录从工作库拷贝到本地目录
            ModuleOperator.CopyLayerTreeXmlFromDataBase(Fan.Plugin.ModuleCommon.TmpWorkSpace, ModData.m_DataXmlPath);
            XmlDocument datadoc = new XmlDocument();
            datadoc.Load(ModData.m_DataXmlPath);
            if (datadoc != null)
            {
                ModuleOperator.DisplayInDataLstView(datadoc, DataTree);
            }
            if (roleTree.SelectedNode == null)
                return;
            DevComponents.AdvTree.Node selectNode = roleTree.SelectedNode;

            //数据权限设置
            Exception eError = null;
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            List<Dictionary<string, object>> lstDicDataData = sysTable.GetRows("role_data", "ROLEID='" + selectNode.Name + "'", out eError);
            List<string> lstdataPrivilege = new List<string>();
            if (lstDicDataData.Count != 0)
            {
                for (int i = 0; i < lstDicDataData.Count; i++)
                {
                    lstdataPrivilege.Add(lstDicDataData[i]["DATAPRI_ID"].ToString());
                }
                for (int i = 0; i < DataTree.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node treeNode = DataTree.Nodes[i];
                    ModuleOperator.IsCheckChanged = false;
                    if (lstdataPrivilege.Contains(treeNode.Name))
                    {
                        treeNode.Cells[1].Checked = true;
                    }
                    else
                    {
                        treeNode.Cells[1].Checked = false;
                    }
                    if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                    {
                        ModuleOperator.IsCheckChanged = true;
                    }
                    else
                    {
                        treeNode.Tag = treeNode.Cells[1].Checked;
                    }
                    if (treeNode.HasChildNodes)
                    {
                        ModuleOperator.UpdateRolePrivilege(lstdataPrivilege, treeNode);
                    }
                }
            }
            else
            {
                for (int i = 0; i < DataTree.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node treeNode = DataTree.Nodes[i];
                    ModuleOperator.IsCheckChanged = false;
                    treeNode.Cells[1].Checked = false;
                    if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
                    {
                        ModuleOperator.IsCheckChanged = true;
                    }
                    else
                    {
                        treeNode.Tag = treeNode.Cells[1].Checked;
                    }
                    if (treeNode.HasChildNodes)
                    {
                        ModuleOperator.UpdateRolePrivilege(lstdataPrivilege, treeNode);
                    }
                }
            }

        }
        //刷新图层目录列表
        private void MenuDataRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataList();
        }

        private void MenuSelectAll_Click(object sender, EventArgs e)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (roleTree.SelectedNode.Name == "角色")
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            DevComponents.AdvTree.Node aNode = priTree.SelectedNode;

            SelectAllpriNode(sysTable, aNode);
        }

        private void MenuUnselectall_Click(object sender, EventArgs e)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (roleTree.SelectedNode.Name == "角色")
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            DevComponents.AdvTree.Node aNode = priTree.SelectedNode;

            unSelectAllpriNode(sysTable, aNode);
        }
        #region 数据源列表相关功能注释掉
        //private void MenuDbsourceSelAll_Click(object sender, EventArgs e)
        //{
        //    if (roleTree.SelectedNode == null)
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    if (roleTree.SelectedNode.Name == "角色")
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    SysGisTable sysTable = new SysGisTable(ModData.gisDb);
        //    DevComponents.AdvTree.Node aNode = dbSourceTree.SelectedNode;

        //    SelectAllpriNode(sysTable, aNode);
        //}

        //private void MenuDbsourceunSelAll_Click(object sender, EventArgs e)
        //{
        //    if (roleTree.SelectedNode == null)
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    if (roleTree.SelectedNode.Name == "角色")
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    SysGisTable sysTable = new SysGisTable(ModData.gisDb);
        //    DevComponents.AdvTree.Node aNode = dbSourceTree.SelectedNode;

        //    unSelectAllpriNode(sysTable, aNode);
        //}
        //public void RefreshDBsourcelist()
        //{
        //    ModuleOperator.DisplayInDbsourceLstView(this.dbSourceTree);
        //    if (roleTree.SelectedNode == null)
        //        return;
        //    //如果有选中的角色节点，将它的数据源列表节点选中状态显示到刷新后的列表中
        //    DevComponents.AdvTree.Node selectNode = roleTree.SelectedNode;
        //    //数据源权限设置
        //    Exception eError = null;
        //    SysGisTable sysTable = new SysGisTable(ModData.gisDb);
        //    //从数据库中获取最新的选中ID列表
        //    List<Dictionary<string, object>> lstDicDbsource = sysTable.GetRows("role_dbsource", "ROLEID='" + selectNode.Name.ToLower() + "'", out eError);
        //    List<string> lstdbsource = new List<string>();
        //    if (lstDicDbsource != null)
        //    {
        //        if (lstDicDbsource.Count != 0)
        //        {
        //            for (int i = 0; i < lstDicDbsource.Count; i++)
        //            {
        //                lstdbsource.Add(lstDicDbsource[i]["DBSOURCE_ID"].ToString());
        //            }
        //            for (int i = 0; i < dbSourceTree.Nodes.Count; i++)
        //            {
        //                DevComponents.AdvTree.Node treeNode = dbSourceTree.Nodes[i];
        //                ModuleOperator.IsCheckChanged = false;
        //                if (lstdbsource.Contains(treeNode.Name))
        //                {
        //                    treeNode.Cells[1].Checked = true;
        //                }
        //                else
        //                {
        //                    treeNode.Cells[1].Checked = false;
        //                }
        //                if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
        //                {
        //                    ModuleOperator.IsCheckChanged = true;
        //                }
        //                else
        //                {
        //                    treeNode.Tag = treeNode.Cells[1].Checked;
        //                }
        //                if (treeNode.HasChildNodes)
        //                {
        //                    ModuleOperator.UpdateRolePrivilege(lstdbsource, treeNode);
        //                }
        //            }
        //        }
        //        else//若没有任何选中的，则全部设置为非选中状态
        //        {
        //            for (int i = 0; i < dbSourceTree.Nodes.Count; i++)
        //            {
        //                DevComponents.AdvTree.Node treeNode = dbSourceTree.Nodes[i];
        //                ModuleOperator.IsCheckChanged = false;
        //                treeNode.Cells[1].Checked = false;
        //                if (treeNode.Tag.ToString() == treeNode.Cells[1].Checked.ToString())
        //                {
        //                    ModuleOperator.IsCheckChanged = true;
        //                }
        //                else
        //                {
        //                    treeNode.Tag = treeNode.Cells[1].Checked;
        //                }
        //                if (treeNode.HasChildNodes)
        //                {
        //                    ModuleOperator.UpdateRolePrivilege(lstdbsource, treeNode);
        //                }
        //            }
        //        }
        //    }
 
        //}
        ////刷新数据源列表
        //private void MenuDbsourceRefresh_Click(object sender, EventArgs e)
        //{
        //    RefreshDBsourcelist();
        //}
        //功能权限全选功能（选中当前节点及所有下层节点）
        #endregion
        private void SelectAllpriNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (aNode != null)
            {
                Exception eError;
                Dictionary<string, object> dicvalues = new Dictionary<string, object>();
                if (aNode.Cells[1].Checked == false)
                {
                    //cyf 20110613  :add：添加对树节点的选中状态的保护，不然会死机
                    if (roleTree.SelectedNode == null)
                    {
                        Fan.Common.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个角色节点。");
                        return;
                    }
                    //end
                    dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
                    dicvalues.Add("PRIVILEGE_ID", aNode.Name);
                    sysTable.NewRow("role_pri", dicvalues, out eError);
                }
                aNode.Cells[1].Checked = true;
                if (aNode.Nodes.Count > 0)
                {
                    DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                    foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                    {
                        //eachnode.Cells[1].Checked = true;
                        SelectAllpriNode(sysTable, eachnode);
                    }
                }
            }

        }
        //功能权限全不选功能（不选当前节点及所有下层节点）
        private void unSelectAllpriNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (aNode != null && roleTree.SelectedNode != null)
            {
                Exception eError;
                Dictionary<string, object> dicvalues = new Dictionary<string, object>();
                if (aNode.Cells[1].Checked == true)
                {
                    sysTable.DeleteRows("role_pri", "ROLEID='" + roleTree.SelectedNode.Name + "' and PRIVILEGE_ID='" + aNode.Name + "'", out eError);

                }
                aNode.Cells[1].Checked = false;
                if (aNode.Nodes.Count > 0)
                {
                    DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                    foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                    {
                        //eachnode.Cells[1].Checked = true;
                        unSelectAllpriNode(sysTable, eachnode);
                    }
                }
            }

        }
        //数据权限节点全选功能（当前节点及所有下层节点）
        private void SelectAlldataNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (aNode == null)
                return;
            Exception eError;
            Dictionary<string, object> dicvalues = new Dictionary<string, object>();
            if (aNode.Cells[1].Checked == false)
            {
                dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
                dicvalues.Add("DATAPRI_ID", aNode.Name);
                sysTable.NewRow("role_Data", dicvalues, out eError);
            }
            aNode.Cells[1].Checked = true;
            if (aNode.Nodes.Count > 0)
            {
                DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                {
                    //eachnode.Cells[1].Checked = true;
                    SelectAlldataNode(sysTable, eachnode);
                }
            }
        }
        //数据权限节点全不选功能（当前节点及所有下层节点）
        private void unSelectAlldataNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            if (aNode == null)
                return;
            Exception eError;
            Dictionary<string, object> dicvalues = new Dictionary<string, object>();
            if (aNode.Cells[1].Checked == true)
            {
                sysTable.DeleteRows("role_Data", "ROLEID='" + roleTree.SelectedNode.Name + "' and DATAPRI_ID='" + aNode.Name + "'", out eError);

            }
            aNode.Cells[1].Checked = false;
            if (aNode.Nodes.Count > 0)
            {
                DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                {
                    //eachnode.Cells[1].Checked = true;
                    unSelectAlldataNode(sysTable, eachnode);
                }
            }
        }
        #region 数据源列表相关功能注释掉
        ////数据权限节点全选功能（当前节点及所有下层节点）
        //private void SelectAllDbsourceNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        //{
        //    if (roleTree.SelectedNode == null)
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    if (aNode == null)
        //        return;
        //    Exception eError;
        //    Dictionary<string, object> dicvalues = new Dictionary<string, object>();
        //    if (aNode.Cells[1].Checked == false)
        //    {
        //        dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
        //        dicvalues.Add("DBSOURCE_ID", aNode.Name);
        //        if (!sysTable.ExistData("role_dbsource", "ROLEID='" + roleTree.SelectedNode.Name + "' and DBSOURCE_ID=" + aNode.Name))
        //        {
        //            sysTable.NewRow("ROLE_DBSOURCE", dicvalues, out eError);
        //        }
        //    }
        //    aNode.Cells[1].Checked = true;
        //    if (aNode.Nodes.Count > 0)
        //    {
        //        DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
        //        foreach (DevComponents.AdvTree.Node eachnode in pCollection)
        //        {
        //            //eachnode.Cells[1].Checked = true;
        //            SelectAlldataNode(sysTable, eachnode);
        //        }
        //    }
        //}
        ////数据权限节点全不选功能（当前节点及所有下层节点）
        //private void unSelectAllDbsourceNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        //{
        //    if (roleTree.SelectedNode == null)
        //    {
        //        ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        return;
        //    }
        //    if (aNode == null)
        //        return;
        //    Exception eError;
        //    Dictionary<string, object> dicvalues = new Dictionary<string, object>();
        //    if (aNode.Cells[1].Checked == true)
        //    {   //若本来已选择，从该角色的权限表格中删除该条记录
        //        sysTable.DeleteRows("role_DBSOURCE", "ROLEID='" + roleTree.SelectedNode.Name + "' and DBSOURCE_ID=" + aNode.Name, out eError);

        //    }
        //    aNode.Cells[1].Checked = false;
        //    if (aNode.Nodes.Count > 0)
        //    {
        //        DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
        //        foreach (DevComponents.AdvTree.Node eachnode in pCollection)
        //        {
        //            //eachnode.Cells[1].Checked = true;
        //            //递归执行全不选函数
        //            unSelectAlldataNode(sysTable, eachnode);
        //        }
        //    }
        //}
        #endregion
        private void DataTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DataTree.SelectedNode = e.Node;
        }

        /// <summary>
        /// 设置数据权限树文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataTree_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            Exception eError;
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            if (ModuleOperator.IsCheckChanged)
            {
                if (roleTree.SelectedNode != null && roleTree.SelectedNode.Text != "角色")
                {
                    DevComponents.AdvTree.Cell cell = e.Cell;
                    if (cell == null) return;
                    DevComponents.AdvTree.Node aNode = e.Cell.Parent;
                    if (aNode != null)
                    {
                        aNode.Tag = aNode.Cells[1].Checked;
                        if (aNode.Cells[1].Checked.ToString().ToUpper() == "TRUE")
                        {
                            //Dictionary<string, object> dicvalues = new Dictionary<string, object>();
                            //dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
                            //dicvalues.Add("DATAPRI_ID", aNode.Name);
                            //sysTable.NewRow("role_Data", dicvalues, out eError);
                            CheckeddataNode(sysTable, aNode);
                        }
                        else if (aNode.Cells[1].Checked.ToString().ToUpper() == "FALSE")
                        {
                            //sysTable.DeleteRows("role_Data", "ROLEID='" + roleTree.SelectedNode.Name + "' and DATAPRI_ID='" + aNode.Name + "'", out eError);
                            unCheckeddataNode(sysTable, aNode);
                        }
                        else
                        { return; }
                    }
                }
                else
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                }
            }
            else
            {
                ModuleOperator.IsCheckChanged = true;
            }
        }

        private void priTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            priTree.SelectedNode = e.Node;
        }

        /// <summary>
        /// 设置权限树文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void priTree_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            Exception eError;
            SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            if (ModuleOperator.IsCheckChanged)
            {
                if (roleTree.SelectedNode != null && roleTree.SelectedNode.Text != "角色")
                {
                    DevComponents.AdvTree.Cell cell = e.Cell;
                    if (cell == null) return;
                    DevComponents.AdvTree.Node aNode = e.Cell.Parent;
                    if (aNode != null)
                    {
                        aNode.Tag = aNode.Cells[1].Checked;
                        if (aNode.Cells[1].Checked.ToString().ToUpper() == "TRUE")
                        {
                            //Dictionary<string, object> dicvalues = new Dictionary<string, object>();
                            //dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
                            //dicvalues.Add("PRIVILEGE_ID", aNode.Name);
                            //sysTable.NewRow("role_pri", dicvalues, out eError);
                            CheckedNode(sysTable, aNode);
                        }
                        else if (aNode.Cells[1].Checked.ToString().ToUpper() == "FALSE")
                        {
                            //sysTable.DeleteRows("role_pri", "ROLEID='" + roleTree.SelectedNode.Name + "' and PRIVILEGE_ID='" + aNode.Name + "'", out eError);
                            unCheckedNode(sysTable, aNode);
                        }
                        else
                        { return; }
                    }
                }
                else
                {
                    ErrorHandle.ShowInform("提示", "请选择角色！");
                }
            }
            else
            {
                ModuleOperator.IsCheckChanged = true;
            }
        }
        #region 数据源列表相关功能注释掉
        //private void dbSourceTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        //{
        //    dbSourceTree.SelectedNode = e.Node;
        //}

        //private void dbSourceTree_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        //{
        //    Exception eError;
        //    SysGisTable sysTable = new SysGisTable(ModData.gisDb);
        //    if (ModuleOperator.IsCheckChanged)
        //    {
        //        if (roleTree.SelectedNode != null && roleTree.SelectedNode.Text != "角色")
        //        {
        //            DevComponents.AdvTree.Cell cell = e.Cell;
        //            if (cell == null) return;
        //            DevComponents.AdvTree.Node aNode = e.Cell.Parent;
        //            if (aNode != null)
        //            {
        //                aNode.Tag = aNode.Cells[1].Checked;
        //                if (aNode.Cells[1].Checked.ToString().ToUpper() == "TRUE")
        //                {
        //                    //Dictionary<string, object> dicvalues = new Dictionary<string, object>();
        //                    //dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
        //                    //dicvalues.Add("DATAPRI_ID", aNode.Name);
        //                    //sysTable.NewRow("role_Data", dicvalues, out eError);
        //                    CheckeddbsourceNode(sysTable, aNode);
        //                }
        //                else if (aNode.Cells[1].Checked.ToString().ToUpper() == "FALSE")
        //                {
        //                    //sysTable.DeleteRows("role_Data", "ROLEID='" + roleTree.SelectedNode.Name + "' and DATAPRI_ID='" + aNode.Name + "'", out eError);
        //                    unCheckeddbsourceNode(sysTable, aNode);
        //                }
        //                else
        //                { return; }
        //            }
        //        }
        //        else
        //        {
        //            ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
        //        }
        //    }
        //    else
        //    {
        //        ModuleOperator.IsCheckChanged = true;
        //    }

        //}
        #endregion
        private void btnPrivilegesAdd_Click(object sender, EventArgs e)
        {
            if (this.lstAllPrivilege.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lstAllPrivilege.SelectedItems[0];
                if (item != null)
                {
                    foreach (ListViewItem pItem in this.lstUserPrivilege.Items)
                    {
                        if (pItem.Text.Equals(item.Text))
                        {
                            return;
                        }
                    }
                    this.lstUserPrivilege.Items.Add(item.Clone() as ListViewItem);
                }
            }
        }

        private void btnPrivilegesRemove_Click(object sender, EventArgs e)
        {
            if (this.lstUserPrivilege.SelectedItems.Count > 0)
            {
                ListViewItem item = this.lstUserPrivilege.SelectedItems[0];
                if (item != null)
                {
                    this.lstUserPrivilege.Items.Remove(item);
                }
            }
        }

        /// <summary>
        /// 设置用户的所属用户组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (userTree.SelectedNode == null)
            {
                MessageBox.Show("请选择用户！", "提示！");
                return;
            }
            if (userTree.SelectedNode.Tag==null) //修改：当未用户组时，点击确定的情况    张琪   20110706
            {
                MessageBox.Show("请选择用户组！","提示！");
                return;
            }
            User user = userTree.SelectedNode.Tag as User;
            if (user != null)
            {
                Exception exError;
                Dictionary<string, object> dicValues;
                try
                {
                    if (ModData.gisDb == null)
                    {
                        ModData.gisDb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out exError);
                    }
                    ModData.gisDb.StartTransaction(out exError);
                    SysGisTable sysTable = new SysGisTable(ModData.gisDb);
                    if (lstUserPrivilege.Items.Count > 0)
                    {
                        sysTable.DeleteRows("user_role", "USERID='" + user.IDStr + "'", out exError);
                        bool result = false;
                        foreach (ListViewItem item in lstUserPrivilege.Items)
                        {
                            dicValues = new Dictionary<string, object>();
                            Role role = item.Tag as Role;
                            if (role != null)
                            {
                                dicValues.Add("userid", user.IDStr);
                                dicValues.Add("roleid", role.IDStr);
                                result = sysTable.NewRow("user_role", dicValues, out exError);
                            }
                        }
                        ModData.gisDb.EndTransaction(result, out exError);
                        if (result)
                        {
                            ErrorHandle.ShowInform("提示", "角色设置成功！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    exError = ex;
                    ModData.gisDb.EndTransaction(false, out exError);
                    return;
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            userTree_NodeClick(null, null);
        }
        //选中某个权限节点，遍历父节点，全部选中
        private void CheckedNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            Dictionary<string, object> dicvalues = new Dictionary<string, object>();
            dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
            dicvalues.Add("PRIVILEGE_ID", aNode.Name);
            sysTable.NewRow("role_pri", dicvalues, out eError);
            if (aNode.Parent != null)
            {
                DevComponents.AdvTree.Node pNodeparent = aNode.Parent;
                if (pNodeparent.Cells[1].Checked == false)
                {
                    pNodeparent.Cells[1].Checked = true;
                    CheckedNode(sysTable, pNodeparent);
                }
            }
        }
        //反选某个权限节点，遍历子节点，全部反选
        private void unCheckedNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            sysTable.DeleteRows("role_pri", "ROLEID='" + roleTree.SelectedNode.Name + "' and PRIVILEGE_ID='" + aNode.Name + "'", out eError);
            if (aNode.Nodes.Count > 0)
            {
                DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                {
                    eachnode.Cells[1].Checked = false;
                    unCheckedNode(sysTable, eachnode);
                }
            }
        }
        //选中某个数据权限节点，遍历父节点，全部选中
        private void CheckeddataNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            Dictionary<string, object> dicvalues = new Dictionary<string, object>();
            dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
            dicvalues.Add("DATAPRI_ID", aNode.Name);
            sysTable.NewRow("role_Data", dicvalues, out eError);
            if (aNode.Parent != null)
            {
                DevComponents.AdvTree.Node pNodeparent = aNode.Parent;
                if (pNodeparent.Cells[1].Checked == false)
                {
                    pNodeparent.Cells[1].Checked = true;
                    CheckeddataNode(sysTable, pNodeparent);
                }
            }
        }
        //选中某个数据源权限节点，遍历父节点，全部选中
        private void CheckeddbsourceNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            Dictionary<string, object> dicvalues = new Dictionary<string, object>();
            dicvalues.Add("ROLEID", roleTree.SelectedNode.Name);
            dicvalues.Add("DBSOURCE_ID", aNode.Name);
            if (!sysTable.ExistData("role_dbsource", "ROLEID='" + roleTree.SelectedNode.Name + "' and DBSOURCE_ID=" + aNode.Name))
            {
                sysTable.NewRow("role_dbsource", dicvalues, out eError);
            }
            if (aNode.Parent != null)
            {
                DevComponents.AdvTree.Node pNodeparent = aNode.Parent;
                if (pNodeparent.Cells[1].Checked == false)
                {
                    pNodeparent.Cells[1].Checked = true;
                    CheckeddbsourceNode(sysTable, pNodeparent);
                }
            }
        }
        //反选某个数据权限节点，遍历子节点，全部反选
        private void unCheckeddataNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            sysTable.DeleteRows("role_Data", "ROLEID='" + roleTree.SelectedNode.Name + "' and DATAPRI_ID='" + aNode.Name + "'", out eError);
            if (aNode.Nodes.Count > 0)
            {
                DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                {
                    eachnode.Cells[1].Checked = false;
                    unCheckeddataNode(sysTable, eachnode);
                }
            }
        }
        //反选某个数据源权限节点，遍历子节点，全部反选
        private void unCheckeddbsourceNode(SysGisTable sysTable, DevComponents.AdvTree.Node aNode)
        {
            if (roleTree.SelectedNode == null)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "请选择角色！");
                return;
            }
            Exception eError;
            sysTable.DeleteRows("role_Dbsource", "ROLEID='" + roleTree.SelectedNode.Name + "' and DBSOURCE_ID=" + aNode.Name, out eError);
            if (aNode.Nodes.Count > 0)
            {
                DevComponents.AdvTree.NodeCollection pCollection = aNode.Nodes as DevComponents.AdvTree.NodeCollection;
                foreach (DevComponents.AdvTree.Node eachnode in pCollection)
                {
                    eachnode.Cells[1].Checked = false;
                    unCheckeddbsourceNode(sysTable, eachnode);
                }
            }
        }

        private void lstAllPrivilege_DoubleClick(object sender, EventArgs e)
        {
            btnPrivilegesAdd_Click(null, null);
        }

        private void lstUserPrivilege_DoubleClick(object sender, EventArgs e)
        {
            btnPrivilegesRemove_Click(null, null);
        }

        private void userTree_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            Exception eError;
            if (userTree.SelectedNode == null)  //added by chulili 20111102添加判断
            {
                return;     
            }
            User user = userTree.SelectedNode.Tag as User;
            if (user != null)
            {
                //清空权限树
                lstUserPrivilege.Items.Clear();
                //绑定可用权限
                ModuleOperator.DisplayRoleLstView("", lstAllPrivilege, ref ModData.gisDb, out eError);
                if (eError != null)
                {
                    ErrorHandle.ShowInform("提示", eError.Message);
                    return;
                }
                List<string> ids = ModuleOperator.GetRoleIds(user.IDStr, ref ModData.gisDb, out eError);
                string strSql = "";
                if (ids != null && ids.Count > 0)
                {
                    foreach (string id in ids)
                    {
                        if (string.IsNullOrEmpty(strSql))
                        {
                            strSql = "'" + id.ToString() + "'";
                        }
                        else
                        {
                            strSql += ",'" + id.ToString() + "'";
                        }
                    }
                    //strSql = strSql.Remove(strSql.Length - 1);
                    ModuleOperator.DisplayRoleLstView("ROLEID IN (" + strSql + ")", lstUserPrivilege, ref ModData.gisDb, out eError);
                }
                else
                {
                    if (eError != null)
                    {
                        ErrorHandle.ShowInform("提示", eError.Message);
                        return;
                    }
                }
            }
        }

        private void userTree_NodeMouseDown(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || _dicContextMenu == null)
                return;
            System.Drawing.Point pPoint = new System.Drawing.Point(e.X, e.Y);
            DevComponents.DotNetBar.ButtonItem item = null;
            if (_dicContextMenu.ContainsKey("ContextMenuTree"))
            {
                if (_dicContextMenu["ContextMenuTree"].Items.Count > 0)
                {
                    item = _dicContextMenu["ContextMenuTree"].Items[0] as DevComponents.DotNetBar.ButtonItem;
                    if (item != null)
                    {
                        item.Popup(userTree.PointToScreen(pPoint));
                    }
                }
            }
        }

        private void panelEx1_Resize(object sender, EventArgs e)
        {
            int dwidth = this.panelEx1.Width;
            int dx = this.groupPanel1.Location.X;
            int dy = this.groupPanel1.Location.Y;
            this.groupPanel1.Width=dwidth/3;
            this.groupPanel2.Width = dwidth/3;
            this.groupPanel1.Location = new Point(dwidth/12,dy);
            this.groupPanel2.Location=new Point(dwidth*7/12,dy);
            int btnX = dwidth * 7 / 12 + this.groupPanel2.Width - this.btnCancel.Width;
            this.btnCancel.Location = new Point(btnX, this.btnCancel.Location.Y);
            this.btnOk.Location = new Point(btnX - this.btnOk.Width - 6, this.btnOk.Location.Y);

        }

    }
}
