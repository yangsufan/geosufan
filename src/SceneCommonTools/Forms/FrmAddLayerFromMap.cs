using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;

namespace SceneCommonTools
{
    public partial class FrmAddLayerFromMap : DevComponents.DotNetBar.Office2007Form
    {

        private IMap m_pMap = null;
        private IScene m_pScene = null;
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public FrmAddLayerFromMap()
        {
            InitializeComponent();
        }

        private void FrmAddLayerFromMap_Load(object sender, EventArgs e)
        {
            InitLst();
        }
        public IMap CurMap
        {
            set { m_pMap = value; }
        }
        public IScene CurScene
        {
            set { m_pScene = value; }
        }
        //初始化二维图层列表
        private void InitLst()
        {
            this.LayerTree.Nodes.Clear();

            if (m_pMap == null) return;

            for (int i = 0; i < m_pMap.LayerCount; i++)
            {
                ILayer pLyr = m_pMap.get_Layer(i) as ILayer;
                //创建树节点
                DevComponents.AdvTree.Node rootnode = new DevComponents.AdvTree.Node();
                rootnode.Text = pLyr.Name;
                rootnode.Name = pLyr.Name;
                rootnode.Image=this.ImageList.Images["Layer"];
                rootnode.Tag = pLyr;//直接用tag存储图层
                //创建选择框节点
                DevComponents.AdvTree.Cell rootcell = new DevComponents.AdvTree.Cell();
                rootcell.CheckBoxVisible = true;
                rootcell.Checked = false;
                rootnode.Cells.Add(rootcell);
                rootnode.Expanded = true;
                LayerTree.Nodes.Add(rootnode);
                //如果节点是图层组，则展开，添加子节点
                if (m_pMap.get_Layer(i) is IGroupLayer)
                {
                    rootnode.Image = this.ImageList.Images["DataDIR"];
                    ICompositeLayer comlayer=pLyr as ICompositeLayer;
                    //遍历图层组中的图层
                    for (int j = 0; j < comlayer.Count; j++)
                    {
                        ILayer pTmpLayer = comlayer.get_Layer(j);
                        DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                        aNode.Text = pTmpLayer.Name ;
                        aNode.Name = pTmpLayer.Name ;
                        aNode.Image=this.ImageList.Images["Layer"];
                        aNode.Tag = pTmpLayer;//直接用tag存储图层
                        DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
                        cell.CheckBoxVisible = true;
                        cell.Checked = false;
                        aNode.Cells.Add(cell);
                        aNode.Expanded = true;
                        rootnode.Nodes.Add(aNode);
                    }
                }
            }
        }
        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.LayerTree.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pnode=this.LayerTree.Nodes[i];
                SelectAll(pnode);
            }
        }
        //全选函数，递归
        private void SelectAll(DevComponents.AdvTree.Node pnode)
        {
            pnode.Cells[1].Checked = true;
            if (pnode.Nodes.Count > 0)
            {
                for (int i = 0; i < pnode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pTmpnode=pnode.Nodes[i];
                    SelectAll(pTmpnode );
                }
 
            }
        }
        private void btnNoSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.LayerTree.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pnode = this.LayerTree.Nodes[i];
                unSelectAll(pnode);
            }
        }
        //全不选函数，递归
        private void unSelectAll(DevComponents.AdvTree.Node pnode)
        {
            pnode.Cells[1].Checked = false ;
            if (pnode.Nodes.Count > 0)
            {
                for (int i = 0; i < pnode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pTmpnode = pnode.Nodes[i];
                    unSelectAll(pTmpnode);
                }

            }
        }
        //点确定，将选中图层添加到三维视图，仅支持两个层次的图层树
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_pScene == null) return;

            for (int i = 0; i < this.LayerTree.Nodes.Count ; i++)
            {
                DevComponents.AdvTree.Node pnode = LayerTree.Nodes[i];
                ILayer player = pnode.Tag as ILayer;
                if (pnode.Nodes.Count>0)
                {   //若该节点存在子节点（该节点是图层组），则遍历子节点
                    for (int j = 0; j < pnode.Nodes.Count ; j++)
                    {
                        DevComponents.AdvTree.Node pTmpnode = pnode.Nodes[j];
                        ILayer pTmplayer = pTmpnode.Tag as ILayer;
                        if (pTmpnode.Cells[1].Checked && !ContainLyr(pTmplayer, m_pScene))
                        {
                            m_pScene.AddLayer(pTmplayer, true);
                        }

                    }
                }
                else//若不存在子节点，则该节点是图层
                {
                    if (pnode.Cells[1].Checked && !ContainLyr(player, m_pScene))
                    {
                        m_pScene.AddLayer(player, true);
                    }
                }
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("三维视图加载二维图层");//xisheng 日志记录07.08
            }
            this.DialogResult = DialogResult.OK;
        }
        //判断三维视图中是否已有该图层
        private bool ContainLyr(ILayer pLyr, IScene pScene)
        {
            for (int i = 0; i < pScene.LayerCount; i++)
            {
                if (pScene.get_Layer(i).Equals(pLyr)) return true;
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void LayerTree_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            if (e.Action ==DevComponents.AdvTree.eTreeAction.Mouse)
            {
                DevComponents.AdvTree.Node vNode = e.Cell.Parent;
                if (vNode.Nodes.Count == 0)
                {
                    return;
                }
                if (e.Cell.Checked )
                {
                    SelectAll(vNode);
                }
                else
                {
                    unSelectAll(vNode);
                }
            }
        }
    }
}
