using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System.IO;

namespace GeoSysUpdate.Control
{
    public delegate void myEventHandler(object sender, TreeNodeMouseClickEventArgs e);//委托事件
    public partial class UCResultsTree : UserControl
    {
        //获取当前地图控件
        private IMapControlDefault m_MapControlDefault = null;
        //获取当前业务库
        private IWorkspace m_Workspace = null;

        public event myEventHandler NodeMouseDownCilck;//定义节点单击事件
        public UCResultsTree()
        {
            InitializeComponent();

        }

        public void InitializeResultsTree()
        {
            if (m_Workspace == null) { return; }

            tVResults.ImageList = imageList_Small;
            //edit xisheng 20120612 改变获取参数的方法 start
            ICursor cursor = SysCommon.Gis.ModGisPub.GetQueryCursor(m_Workspace, "SYSSETTING", "settingname ='制图文件目录'", new string[] { "settingvalue", });
            int ResultsIndex = cursor.FindField("settingvalue");
            if (ResultsIndex == -1)
            { return; }
            IRow row = cursor.NextRow();
            //不存在则插入一条记录
            if (row == null)
            { return; }
            //string strResultsName = row.get_Value(ResultsIndex).ToString();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);

            string strResultsPath = row.get_Value(ResultsIndex).ToString();
            //string strResultsPath = SysCommon.Gis.ModGisPub.re(m_Workspace, "A00GIS_图形模块运行参数表", "参数ID ='10521'","参数" );
            
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "制图文件目录";
            //edit xisheng 20120612 改变获取参数的方法 end
            Dictionary<string, string> pDicTag = new Dictionary<string, string>();
            pDicTag.Add("Path", strResultsPath);
            pDicTag.Add("Type", "Root");
            rootNode.Tag = pDicTag;
            rootNode.ImageKey = "Root";
            rootNode.SelectedImageKey = "Root";
            tVResults.Nodes.Add(rootNode);
            rootNode.Expand();
            //刷新当前节点
            RefreshResultsTree(rootNode);
        }
        //获取当前地图控件
        public IMapControlDefault MapControlDefault
        {
            set
            {
                m_MapControlDefault = value;
            }
        }
        //提供访问成果目录树的属性
        public TreeView ResultsTree
        {
            get
            {
                return tVResults;
            }
        }
        public IWorkspace pWorkspace
        {
            set
            {
                m_Workspace = value;
            }
        }

        //private void UCResultsTree_Load(object sender, EventArgs e)
        //{
        //    //初始化成果目录树
        //    InitializeResultsTree();
        //}
        private void RefreshResultsTree(TreeNode rootNode)
        {
            if (rootNode == null) { return; }
            if (!Directory.Exists((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                return;
            }
            foreach (string d in Directory.GetFileSystemEntries((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                if (File.Exists(d))
                {
                    if (!IsSameTreeNode(rootNode, Path.GetFileName(d), d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        TreeNode ResultsNode = new TreeNode(Path.GetFileName(d));  //创建文件类型新节点
                        pDicTag.Add("Path", d);
                        pDicTag.Add("Type", "File");
                        ResultsNode.Tag = pDicTag;
                        switch (Path.GetExtension(d).ToLower())
                        {
                            case ".mxd":
                                ResultsNode.ImageKey = "ArcMap";
                                ResultsNode.SelectedImageKey = "ArcMap";
                                break;
                            case ".doc":
                            case ".docx":
                                ResultsNode.ImageKey = "word";
                                ResultsNode.SelectedImageKey = "word";
                                break;
                            case ".xls":
                            case ".xlsx":
                                ResultsNode.ImageKey = "excel";
                                ResultsNode.SelectedImageKey = "excel";
                                break;
                            case ".jpg":
                            case ".bmp":
                            case ".png":
                            case ".jpeg":
                            case ".gif":
                                ResultsNode.ImageKey = "images";
                                ResultsNode.SelectedImageKey = "images";
                                break;
                            case ".txt":
                                ResultsNode.ImageKey = "text";
                                ResultsNode.SelectedImageKey = "text";
                                break;
                            case ".pdf":
                                ResultsNode.ImageKey = "pdf";
                                ResultsNode.SelectedImageKey = "pdf";
                                break;
                            default:
                                ResultsNode.ImageKey = "warning";
                                ResultsNode.SelectedImageKey = "warning";
                                break;
                        }
                        rootNode.Nodes.Add(ResultsNode);
                    }
                }
                else if (Directory.Exists(d)&&d.ToLower().EndsWith("gdb"))
                {
                    TreeNode GdbNode = new TreeNode(d.Substring(d.LastIndexOf("\\")+1));  //创建文件类型新节点
                    pDicTag.Add("Path", d);
                    pDicTag.Add("Type", "GDB");
                    GdbNode.Tag = pDicTag;
                    GdbNode.ImageKey = "PDB";
                    GdbNode.SelectedImageKey = "PDB";
                    rootNode.Nodes.Add(GdbNode);
                }
                else
                {
                    if (!IsSameTreeNode(rootNode, Path.GetFileName(d), d))
                    {
                        TreeNode fileNode = new TreeNode(Path.GetFileName(d));  //创建文件夹类型新节点
                        pDicTag.Add("Path", d);
                        pDicTag.Add("Type", "Folder");
                        fileNode.Tag = pDicTag;
                        fileNode.ImageKey = "关闭";
                        fileNode.SelectedImageKey = "关闭";
                        rootNode.Nodes.Add(fileNode);
                        fileNode.Expand();
                        DirectoryInfo d1 = new DirectoryInfo(d);
                        if (d1.GetFileSystemInfos().Length != 0)
                        {
                            RefreshResultsTree(fileNode);
                            //DeleteFolder(d1.FullName);////递归创建节点
                        }
                    }

                }
            }
        }
        //判断当前节点下子节点是否存在相同的节点
        private bool IsSameTreeNode(TreeNode rootNode, string strName, string strPath)
        {
            bool bIsSame = false;
            if (rootNode == null) { return bIsSame = true; }
            if (rootNode.Nodes.Count == 0) { return bIsSame; }
            for (int i = 0; i < rootNode.Nodes.Count; i++)
            {
                if (rootNode.Nodes[i].Text == strName)
                {
                    return bIsSame = true;
                }
                if ((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim() == strPath)
                {
                    return bIsSame = true;
                }
            }
            return bIsSame;
        }

        private void tVResults_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeMouseDownCilck(sender, e);
        }



    }
}
