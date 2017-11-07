using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace GeoUtilities
{
    public partial class frmConflicts : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.Node m_SelNode;
        public DevComponents.AdvTree.Node SelNode
        {
            get
            {
                return m_SelNode;
            }
        }

        public DevComponents.AdvTree.AdvTree TreeControl
        {
            get
            {
                return advTree;
            }
        }

        public DevComponents.DotNetBar.ContextMenuBar TreeContextMenuBar
        {
            get
            {
                return contextMenuBar;
            }
        }

        public DevComponents.DotNetBar.Controls.DataGridViewX UserDataGrid
        {
            get
            {
                return dataGridViewX;
            }
        }

        public frmConflicts()
        {
            InitializeComponent();
 
            InitialDataGrid();
        }

        //初始化grid 显示和数据
        private void InitialDataGrid()
        {
            dataGridViewX.Columns.Clear();
            dataGridViewX.ReadOnly = true;
            dataGridViewX.AutoGenerateColumns = false;
            AddColToGrid(dataGridViewX, "字段名称", "FieldName", true, dataGridViewX.Width/4-5, "TextBox");
            AddColToGrid(dataGridViewX, "最初版本值", "CommonVersion", true, dataGridViewX.Width / 4 - 5, "TextBox");
            AddColToGrid(dataGridViewX, "前一版本值", "PreVersion", true, dataGridViewX.Width / 4 - 5, "TextBox");
            AddColToGrid(dataGridViewX, "当前版本值", "CurrentVersion", true, dataGridViewX.Width / 4 - 5, "TextBox");
            AddColToGrid(dataGridViewX, "是否相同", "IsSame", false, 5, "TextBox");
        }
        private void AddColToGrid(DataGridView axiGrid, string HeaderText, string Key, bool Visible, int Width, string CellType)
        {
            DataGridViewColumn vColumn;
            switch (CellType)
            {
                case "TextBox":
                    vColumn = new DataGridViewTextBoxColumn();
                    break;
                case "Button":
                    vColumn = new DataGridViewButtonColumn();
                    break;
                case "CheckBox":
                    vColumn = new DataGridViewCheckBoxColumn();
                    break;
                case "ComboBox":
                    vColumn = new DataGridViewComboBoxColumn();
                    break;
                case "Image":
                    vColumn = new DataGridViewImageColumn();
                    break;
                case "Link":
                    vColumn = new DataGridViewLinkColumn();
                    break;
                default:
                    return;
            }

            vColumn.Visible = Visible;
            vColumn.Width = Width;
            vColumn.HeaderText = HeaderText;
            vColumn.Name = Key;
            vColumn.DataPropertyName = Key; //"C" + (axiGrid.Columns.Count + 1).ToString();
            axiGrid.Columns.Add(vColumn);
        }

        /// <summary>
        /// 填充冲突要素树图显示节点信息
        /// </summary>
        /// <param name="dicValues">key为要素类数组(第一个为当前版本,第二个为前一版本,第三个为最初版本),value为要素类下冲突要素信息</param>
        public void FillFeatureNodes(Dictionary<IFeatureClass[],Dictionary<IFeature[],DataTable>> dicValues)
        {
            if (dicValues == null) return;
            //清空内容
            advTree.Nodes.Clear();
            dataGridViewX.DataSource = null;
            ClearAll();

            foreach (KeyValuePair<IFeatureClass[], Dictionary<IFeature[], DataTable>> keyValue in dicValues)
            {
                //将数据要素类反映到树图上
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text = keyValue.Key[2].AliasName;
                node.Tag = keyValue.Key;
                advTree.Nodes.Add(node);
                                
                //将要素类中冲突要素OID反映到树图上
                if (keyValue.Value == null) continue;
                foreach (KeyValuePair<IFeature[], DataTable> keyFeat in keyValue.Value)
                {
                    DevComponents.AdvTree.Node featNode = new DevComponents.AdvTree.Node();
                    featNode.Text = keyFeat.Key[2].OID.ToString();
                    featNode.Tag = keyFeat.Key;
                    featNode.DataKey = keyFeat.Value;
                    featNode.ContextMenu= contextMenuBar.Items[0] as DevComponents.DotNetBar.ButtonItem;
                    node.Nodes.Add(featNode);
                }
            }
        }

        private void advTree_NodeMouseUp(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                if (advTree.SelectedNode == null) return;
                if (advTree.SelectedNode.Tag == null || advTree.SelectedNode.DataKey == null) return;
                if (m_SelNode == advTree.SelectedNode) return;

                for (int i = 0; i < contextMenuBar.Items[0].SubItems["btnShow"].SubItems.Count; i++)
                {
                    DevComponents.DotNetBar.ButtonItem aItem = contextMenuBar.Items[0].SubItems["btnShow"].SubItems[i] as DevComponents.DotNetBar.ButtonItem;
                    aItem.Checked = false;
                }

                m_SelNode = advTree.SelectedNode;

                //加载冲突要素属性
                DataTable datatable = advTree.SelectedNode.DataKey as DataTable;
                if (datatable == null) return;
                dataGridViewX.DataSource = datatable;
                //特殊显示发生变化的字段
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
                dataGridViewCellStyle.BackColor = System.Drawing.Color.IndianRed;
                dataGridViewCellStyle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                foreach (DataGridViewRow aRow in dataGridViewX.Rows)
                {
                    if (Convert.ToBoolean(aRow.Cells["IsSame"].Value) == false)
                    {
                        aRow.DefaultCellStyle = dataGridViewCellStyle;
                    }
                }

                //加载对应图层
                IFeatureClass[] pFeatCls = advTree.SelectedNode.Parent.Tag as IFeatureClass[];
                if (pFeatCls == null) return;
                //当前版本
                IFeatureLayer pFeatLay = new FeatureLayerClass();
                if (pFeatCls[0].FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatLay = new FDOGraphicsLayerClass();
                }
                pFeatLay.FeatureClass = pFeatCls[0];
                axMapControlCurrent.Map.AddLayer(pFeatLay as ILayer);
                IFeature[] pFeat = advTree.SelectedNode.Tag as IFeature[];

                IGeoDataset pGeoDt=pFeatCls[0] as IGeoDataset;
                ISpatialReference pSpatialRef = null;
                if (pGeoDt != null)
                {
                    pSpatialRef = pGeoDt.SpatialReference;
                }

                if (pFeat[0] != null)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(axMapControlCurrent.Object as IMapControlDefault, pFeat[0], pSpatialRef);
                }
                //前一版本
                pFeatLay = new FeatureLayerClass();
                if (pFeatCls[1].FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatLay = new FDOGraphicsLayerClass();
                }
                pFeatLay.FeatureClass = pFeatCls[1];
                axMapControlPre.Map.AddLayer(pFeatLay as ILayer);
                if (pFeat[1] != null)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(axMapControlPre.Object as IMapControlDefault, pFeat[1], pSpatialRef);
                }
                //最初版本
                pFeatLay = new FeatureLayerClass();
                if (pFeatCls[2].FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatLay = new FDOGraphicsLayerClass();
                }
                pFeatLay.FeatureClass = pFeatCls[2];
                axMapControlCommon.Map.AddLayer(pFeatLay as ILayer);
                if (pFeat[2] != null)
                {
                    SysCommon.Gis.ModGisPub.ZoomToFeature(axMapControlCommon.Object as IMapControlDefault, pFeat[2], pSpatialRef);
                }         
            }
            catch(Exception err)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "出错:" + err.Message);
                return;
            }

            flashfeature();
        }

        private void expandablePanel_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            flashfeature();
        }

        private void advTree_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            flashfeature();
        }

        private void flashfeature()
        {
            try
            {
                if (advTree.SelectedNode == null) return;
                if (advTree.SelectedNode.Tag ==null) return;
                IFeature[] pFeat = advTree.SelectedNode.Tag as IFeature[];
                if (pFeat == null) return;
                if (expandablePanel.Expanded == true)
                {
                    axMapControlCurrent.ActiveView.Refresh();
                    axMapControlPre.ActiveView.Refresh();
                    axMapControlCommon.ActiveView.Refresh();
                    Application.DoEvents();
                    if (pFeat[0] != null)
                    {
                        axMapControlCurrent.FlashShape(pFeat[0].Shape);
                        Thread.Sleep(50);
                    }

                    if (pFeat[1] != null)
                    {
                        axMapControlPre.FlashShape(pFeat[1].Shape);
                        Thread.Sleep(50);
                    }

                    if (pFeat[2] != null)
                    {
                        axMapControlCommon.FlashShape(pFeat[2].Shape);
                    }
                }
            }
            catch (Exception err)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "出错:" + err.Message);
            }
        }

        //清除对比查看图形
        public void ClearAll()
        {
            axMapControlCurrent.Map.ClearLayers();
            axMapControlCurrent.ActiveView.Refresh();
            axMapControlPre.Map.ClearLayers();
            axMapControlPre.ActiveView.Refresh();
            axMapControlCommon.Map.ClearLayers();
            axMapControlCommon.ActiveView.Refresh();
        }
    }
}