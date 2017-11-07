using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

//xisheng 20111119 增加选择图层的功能。
namespace SysCommon
{
    public partial class SelectLayerByTree : DevComponents.DotNetBar.Office2007Form

    {
        public string _LayerTreePath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\查询图层树.xml"; //图层目录文件路径
        private XmlDocument _LayerTreeXmldoc = null;
        private Dictionary<string, string> _DicLayerList = null;
        private int m_m=0;//设置分类，是查询还是统计，判断是不是需要判断图层是否加载或者可查询
        private bool m_checkbox = false;//设置是否有checkbox
        private IMap m_Map = null;//设置Imap;
        private IMap m_Mapold = null;
        private List<DevComponents.AdvTree.Node> ListNode=new List<DevComponents.AdvTree.Node>();
        private bool flag = true;
        private bool flag2 = true;
        private bool tbclick = false;//是否点击同步按钮

        public Dictionary<string, ILayer> m_DicLayer = new Dictionary<string, ILayer>(); //xisheng 用字典将图层和NodeKey绑到字典里，方便获取 20111203
        public IWorkspace m_TmpWorkSpace = null;//added by chulili 2011-11-30 从外部获取
        public IMap m_returnMap = new MapClass();
        public string m_NodeKey = "";//设置图层NodeKey属性。、
        public string m_DataSourceKey = "";
        public string m_NodeText = "";//图层名
        private SelectLayer.ClsSelectLayerByTree _ClsSelectLayerByTree = null;
        bool remove = false;//判断是否有移除

       public SelectLayerByTree()
        {
            InitializeComponent();
            InitClsSelectLayerTree(null);
        }

       public SelectLayerByTree(int m, IWorkspace pTmpWorkSpace, List<string> ListDataNodeKeys)
        {
            InitializeComponent();
            m_m = m;
            m_TmpWorkSpace = pTmpWorkSpace;
            InitClsSelectLayerTree(ListDataNodeKeys);
        }
        public SelectLayerByTree(IWorkspace pTmpWorkSpace, List<string> ListDataNodeKeys)
        {
            InitializeComponent();
            m_TmpWorkSpace = pTmpWorkSpace;
            InitClsSelectLayerTree(ListDataNodeKeys);
        }
        public SelectLayerByTree(IWorkspace pTmpWorkSpace, List<string> ListDataNodeKeys,List<string> ListLayerKeys)
        {
            InitializeComponent();
            m_TmpWorkSpace = pTmpWorkSpace;
            InitClsSelectLayerTree(ListDataNodeKeys, ListLayerKeys);
        }
        public SelectLayerByTree(int m, bool checkbox, IMap pmap, IWorkspace pTmpWorkSpace, List<string> ListDataNodeKeys)
        {
            InitializeComponent();
            m_m = m;
            m_checkbox = checkbox;
            m_Mapold=m_Map = pmap;
            m_TmpWorkSpace = pTmpWorkSpace;
            this.btn_TbCatalog.Visible = true;
            InitClsSelectLayerTree(ListDataNodeKeys);
            
        }
        private void InitClsSelectLayerTree(List<string > ListDataNodeKeys)
        {
            if (_ClsSelectLayerByTree == null)
            {
                _ClsSelectLayerByTree=new SelectLayer.ClsSelectLayerByTree();
            }
            _ClsSelectLayerByTree.Imagelist = this.ImageList;
            _ClsSelectLayerByTree.CheckBox = m_checkbox;
            _ClsSelectLayerByTree.ListDataNodeKeys = ListDataNodeKeys;

        }
        private void InitClsSelectLayerTree(List<string> ListDataNodeKeys,List<string> ListLayerKeys)
        {
            if (_ClsSelectLayerByTree == null)
            {
                _ClsSelectLayerByTree = new SelectLayer.ClsSelectLayerByTree();
            }
            _ClsSelectLayerByTree.Imagelist = this.ImageList;
            _ClsSelectLayerByTree.CheckBox = m_checkbox;
            _ClsSelectLayerByTree.ListDataNodeKeys = ListDataNodeKeys;
            _ClsSelectLayerByTree.ListLayerKeys = ListLayerKeys;

        }
        private void SelectLayerByTree_Load(object sender, EventArgs e)
        {
            if (m_TmpWorkSpace != null)
            {
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(m_TmpWorkSpace, _LayerTreePath);
            }
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(m_TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (_DicLayerList == null)
            {
                _DicLayerList = new Dictionary<string, string>();
            }

            _ClsSelectLayerByTree.CreatRootNode(m_Map, _LayerTreePath ,this.advTreeLayerList,tbclick);//创建根节点
            _ClsSelectLayerByTree.RemoveChild( advTreeLayerList.Nodes[0],ref remove );

        }








       


        //点击叶子节点 选中该图层
        private void advTreeLayerList_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {

        }
        //双击选择叶子节点 
        private void advTreeLayerList_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (m_checkbox)
                return;
            m_NodeKey = "";
            if (e.Node == null)
                return;
            if (e.Node.Tag.ToString() != "Layer")//不是叶子节点 返回
            {
                return;
            }

            _ClsSelectLayerByTree.GetNodeKey(e.Node,labelErr,m_m,ref m_NodeKey,ref m_NodeText );
            if (string.IsNullOrEmpty(m_NodeKey))
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
       }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            if (!m_checkbox)//不支持多选的
            {
                m_NodeKey = "";
                if (advTreeLayerList.SelectedNode == null)
                    return;
                if (advTreeLayerList.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
                {
                    return;
                }

                _ClsSelectLayerByTree.GetNodeKey(advTreeLayerList.SelectedNode, labelErr, m_m, ref m_NodeKey, ref m_NodeText, ref m_DataSourceKey);
                if (string.IsNullOrEmpty(m_NodeKey))
                    return;
            }
            else//支持多选的 
            {
                
                if (advTreeLayerList.Nodes.Count > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    ListNode.Clear();//清空节点
                    _ClsSelectLayerByTree.GetCheckedNodetoMap(advTreeLayerList.Nodes[0],ListNode );
                    try
                    {
                        for (int i = 0; i < ListNode.Count; i++)
                        {
                            // IFeatureLayer pfeaturelayer = new FeatureLayerClass();
                            // pfeaturelayer.Name = ListNode[i].Text;
                            //pfeaturelayer.FeatureClass=SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace,_LayerTreePath,GetNodeKey1(ListNode[i]));

                            ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(m_TmpWorkSpace, m_Mapold,_ClsSelectLayerByTree.GetNodeKey1(ListNode[i],labelErr,m_m ), _LayerTreeXmldoc, false);
                            if (pLayer == null) continue;
                            //m_returnMap.AddLayer(pLayer);
                            m_DicLayer.Add(_ClsSelectLayerByTree.GetNodeKey1(ListNode[i],labelErr,m_m ), pLayer);
                        }
                    }
                    catch { }
                    this.Cursor = Cursors.Default;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; ;
            this.Close();
        }

        //同步目录所有节点
        private void btn_TbCatalog_Click(object sender, EventArgs e)
        {
            tbclick = true;
            _ClsSelectLayerByTree.CreatRootNode(m_Map, _LayerTreePath,this.advTreeLayerList,tbclick );
        }
        private void advTreeLayerList_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            DevComponents.AdvTree.AdvTree tree = sender as DevComponents.AdvTree.AdvTree;
            DevComponents.AdvTree.Node node = tree.SelectedNode;

            if (node == null) return;
           //如果是叶子节点
            if (flag2 && flag)
            {
                if (flag && node.Nodes.Count==0)
                {
                    //flag2 = false;
                    //node.Parent.Checked = true;
                    //flag2 = true;
                    _ClsSelectLayerByTree.ChangeParentCheck(node,ref flag2 );
                }
                else if (flag2)//不是叶子节点 
                {

                    //foreach (DevComponents.AdvTree.Node item in node.Nodes)
                    //{
                    //    flag = false;
                    //    item.Checked = node.Checked;
                    //}
                    //flag = true;
                    _ClsSelectLayerByTree.ChangeParentCheck(node,ref flag2 );
                    _ClsSelectLayerByTree.ChangeChildCheck(node,ref flag ); 
                }
            }
            //if (flag2 && flag)
            //{
            //    ChangeParentCheck(node);

            //    //如果是叶子节点
            //    if (node.Tag.ToString() == "Layer" && flag)
            //    {

            //    }
            //    else
            //    {

            //        ChangeChildCheck(node);
            //    }
            //}
        }

    }
}
