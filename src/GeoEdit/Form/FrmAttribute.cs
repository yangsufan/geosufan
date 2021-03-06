using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
namespace GeoEdit
{
    /// <summary>
    /// 属性的设置
    /// </summary>
    public partial class FrmAttribute : DevComponents.DotNetBar.Office2007Form
    {

        static string text = "";//用来记录修改之前的值
        private Plugin.Application.IAppGISRef myHook;//传一个MAP的容器进来，用来得到MAP
        static string layer_name = "";//定义一个静态变量，用来接收当前LAYER的名称
        static string OID = "";//记录当前层下面OID的值
        public Hashtable hs_table_tree = new Hashtable();//首先用来接收TREE将用到的值
        public Hashtable hs_table_attribute = new Hashtable();//用来显示对应的属性值

        /// <summary>
        /// 利用构造函数传值
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="tb_show"></param>
        public FrmAttribute(Hashtable tb, Hashtable tb_show, Plugin.Application.IAppGISRef Hook)
        {
            myHook = Hook;
            hs_table_tree = tb;
            hs_table_attribute = tb_show;
            InitializeComponent();

        }

        /// <summary>
        /// 把MAP上选择的要素显示在控件上
        /// </summary>
        public void DataMain()
        {
            Hashtable table = hs_table_tree;
            if (table == null) return;
            treeview(table);
            treeview_Name.Nodes[0].Nodes[0].CheckState = CheckState.Checked;
        }
        /// <summary>
        /// 首次进来窗体，有一个初始的SHOW,默认为第一个节点加上第一个子节点的TEXT值作为一个KEY来得到里面的属性
        /// </summary>
        private void Initialize_show()
        {
            DataTable table = new DataTable();
            table.Columns.Add("字段名", typeof(string));
            table.Columns.Add("值", typeof(string));

            Hashtable hs_tb = hs_table_attribute;

            string texts = layer_name + OID;//全拼成一个KEY
            char[] sp = { ',' };
            char[] sp1 = { ' ' };
            int count = hs_tb.Count;//键对值的数目
            string[] temp = hs_tb[texts].ToString().Substring(0, hs_tb[texts].ToString().Length - 1).Split(sp);//首先对字串处理，因为最尾会有一个，号。因先处理它，再进行分割我们要的数据
            for (int n = 0; n < temp.Length; n++)
            {
                string[] value = temp[n].Split(sp1);//再次的进行分割，得到最终我们要的数据
                DataRow row = table.NewRow();//创建一个行
                row[0] = value[0];
                row[1] = value[1];
                table.Rows.Add(row);

            }

            DT_VIEW_Attriubte.DataSource = table;//将数据邦定到控件显示
            DT_VIEW_Attriubte.Columns[0].Width = 80;//确定列的宽
            DT_VIEW_Attriubte.Columns[1].Width = 180;
            int Count = DT_VIEW_Attriubte.Rows.Count - 1;//最后一行
            DT_VIEW_Attriubte.Rows[Count].ReadOnly = true;
            if (DT_VIEW_Attriubte.Rows.Count > 0)
            {
                DT_VIEW_Attriubte.Columns[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;//禁用第一列不能排序
                DT_VIEW_Attriubte.Columns[1].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;//第二列不能排序
                for (int c = 0; c < Count; c++)
                {
                    DT_VIEW_Attriubte.Rows[c].Height = 18;
                    string value = DT_VIEW_Attriubte.Rows[c].Cells[0].Value.ToString().ToLower();
                    if (value == "objectid" || value == "shape" || value == "shape_length" || value == "shape_area" || value == "element")//确定控件上不能更改的属性
                    {
                        DT_VIEW_Attriubte.Rows[c].ReadOnly = true;
                    }
                    else
                    {
                        DT_VIEW_Attriubte.Rows[c].Cells[0].ReadOnly = true;//所有的字段名是不能修改的
                    }
                }
            }
        }
        /// <summary>
        /// 将数据邦定到树上，以显示节点
        /// </summary>
        /// <param name="tb"></param>
        private void treeview(Hashtable tb)
        {
            if (treeview_Name.Nodes.Count > 0)
            {
                treeview_Name.Nodes.Clear();
            }
            bool state = false;//要邦定一个初始的SHOW，所以给个状态控件显示属性
            foreach (DictionaryEntry de in tb)
            {

                char[] sp = { ' ' };
                DevComponents.AdvTree.Node nodes = new DevComponents.AdvTree.Node();
                nodes.Text = de.Key.ToString();
                string[] temp = de.Value.ToString().Split(sp);
                for (int n = 0; n < temp.Length; n++)
                {

                    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                    node.Text = temp[n];
                    if (!state)
                    {

                        layer_name = de.Key.ToString();//第一个图层名
                        OID = temp[n];//第一个OID
                        Initialize_show();//调用方法，显示属性初始，默认为第一个
                        state = true;
                    }
                    nodes.Nodes.Add(node);
                    node.Parent.Expand();//展开
                }
                treeview_Name.Nodes.Add(nodes);//将节点加到树中

            }
        }
        /// <summary>
        /// 树的属性
        /// </summary>
        public DevComponents.AdvTree.AdvTree TreeName
        {
            get { return treeview_Name; }
            set { treeview_Name = value; }
        }
        public void FrmAttribute_Load(object sender, EventArgs e)
        {
            DataMain();//调用主函数

        }
        /// <summary>
        /// 重新读取HASHTABLE里的值,重新邦定显示的值
        /// </summary>
        private void Re_readValue()
        {
            DataTable table = new DataTable();
            table.Columns.Add("字段名", typeof(string));
            table.Columns.Add("值", typeof(string));

            Hashtable hs_tb = hs_table_attribute;
            string temps = layer_name + OID;
            char[] sp = { ',' };
            char[] sp1 = { ' ' };
            int count = hs_tb.Count;//键对值的数目
            string[] temp = hs_tb[temps].ToString().Substring(0, hs_tb[temps].ToString().Length - 1).Split(sp);//首先对字串处理，因为最尾会有一个，号。因先处理它，再进行分割我们要的数据
            for (int n = 0; n < temp.Length; n++)
            {
                string[] value = temp[n].Split(sp1);//再次的进行分割，得到最终我们要的数据
                DataRow row = table.NewRow();//创建一个行
                row[0] = value[0];
                row[1] = value[1];
                table.Rows.Add(row);

            }
            DT_VIEW_Attriubte.DataSource = table;//将数据邦定到控件显示
            DT_VIEW_Attriubte.Columns[0].Width = 80;//确定列的宽
            DT_VIEW_Attriubte.Columns[1].Width = 180;
            int Count = DT_VIEW_Attriubte.Rows.Count - 1;//最后一行
            DT_VIEW_Attriubte.Rows[Count].ReadOnly = true;
            DataGridShow(DT_VIEW_Attriubte);//调用控制属性的方法
        }
        /// <summary>
        /// 当单元格的值更改时，进行编辑修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DT_VIEW_Attriubte_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int count = myHook.MapControl.Map.LayerCount;
            for (int n = 0; n < count; n++)
            {
                ILayer layer = myHook.MapControl.Map.get_Layer(n);
                if (layer == null) continue;
                //首先判别工程打开的数据是不是以组形式开展的
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "示意图")
                    {
                        continue;
                    }
                    #region 遍历组下面的普通层
                    IWorkspace space = MoData.v_CurWorkspaceEdit as IWorkspace;//得到相应的操作空间
                    ICompositeLayer pComLayer = layer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        if (mLayer == null) return;
                        IFeatureLayer layer_space = mLayer as IFeatureLayer;//得到要素层
                        IDataset dataset_space = layer_space.FeatureClass as IDataset;//中转成一个要素集合

                        if (!space.Equals(dataset_space.Workspace))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不在同一个操作空间下，不能编辑！");
                            return;//如果操作的空间不一样就进行下一个
                        }
                        //////////对SDE图层名称进行处理
                        string getname = string.Empty;
                        if (layer_space.DataSourceType == "SDE Feature Class")
                            getname = layer_name.Substring(layer_name.IndexOf('.') + 1);
                        else getname = layer_name;

                        if (mLayer.Name == getname)
                        {
                            IFeatureLayer F_layer = mLayer as IFeatureLayer;//将图层转成要素层

                            try
                            {
                                MoData.v_CurWorkspaceEdit.StartEditOperation();//编辑操作开始
                                string temp = DT_VIEW_Attriubte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();//得到修改后的值
                                int oid = Convert.ToInt32(OID);//得到要更新的OID
                                int index = F_layer.FeatureClass.FindField(DT_VIEW_Attriubte.Rows[e.RowIndex].Cells[0].Value.ToString());//得到所有的索引要素行
                                F_layer.FeatureClass.GetFeature(oid).set_Value(index, temp);//更新值
                                F_layer.FeatureClass.GetFeature(oid).Store();//更新到PDB里，存储
                                MoData.v_CurWorkspaceEdit.StopEditOperation();//结束编辑操作
                                UpdateHashTable(temp, e.ColumnIndex, e.RowIndex);
                            }
                            catch (Exception eError)
                            {
                                //******************************************
                                //guozheng added System Exception log
                                if (SysCommon.Log.Module.SysLog == null)
                                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                SysCommon.Log.Module.SysLog.Write(eError);
                                //******************************************
                                Re_readValue();
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "您输入的内容类型不正确，请重新输入！");
                            }
                            //break;
                            return;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 如果不是组，直接遍历
                    if (layer.Name == layer_name)
                    {
                        IFeatureLayer F_layer = layer as IFeatureLayer;//将图层转成要素层空间
                        IDataset det = F_layer.FeatureClass as IDataset;//转成数据集，用来得到编辑空间
                        try
                        {
                            MoData.v_CurWorkspaceEdit.StartEditOperation();//编辑操作开始
                            string temp = DT_VIEW_Attriubte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();//得到修改后的值
                            int oid = Convert.ToInt32(OID);//得到要更新的OID
                            F_layer.FeatureClass.GetFeature(oid).set_Value(e.RowIndex, temp);//更新值
                            F_layer.FeatureClass.GetFeature(oid).Store();//更新到PDB里，存储
                            MoData.v_CurWorkspaceEdit.StopEditOperation();//结束编辑操作
                            UpdateHashTable(temp, e.ColumnIndex, e.RowIndex);
                        }
                        catch (Exception eError)
                        {
                            //******************************************
                            //guozheng added System Exception log
                            if (SysCommon.Log.Module.SysLog == null)
                                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            SysCommon.Log.Module.SysLog.Write(eError);
                            //******************************************
                            Re_readValue();
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "您输入的内容类型不正确，请重新输入！");
                        }
                        break;
                    }
                    #endregion
                }
            }
        }
        /// <summary>
        /// 在关闭窗体时，将状态还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAttribute_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗体关闭时，将两个状态还原
            AttributeShow_state.state_value = false;
            AttributeShow_state.show_state = false;
        }
        /// <summary>
        /// 单击单元格时，得到值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DT_VIEW_Attriubte_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //在单击时，先把值存起来，以便后面类型不对修改进回退回去，使用TRY的原因是因为有可能单击的地方不对
            try
            {
                text = DT_VIEW_Attriubte.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();//得到修改前的值
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 当更新了PDB里的值，那么显示的时候HASHTABLE里的值也要修改，不然就会不同步显示
        /// </summary>
        private void UpdateHashTable(string Content, int index, int row)
        {

            string Value = "";//接收更新后的hashtable值
            string temp = layer_name + OID;//得到HASHTABLE的KEY
            char[] sp = { ',' };
            char[] sp1 = { ' ' };
            int count = hs_table_attribute.Count;//键对值的数目
            string[] temps = hs_table_attribute[temp].ToString().Substring(0, hs_table_attribute[temp].ToString().Length - 1).Split(sp);//首先对字串处理，因为最尾会有一个，号。因先处理它，再进行分割我们要的数据
            for (int n = 0; n < temps.Length; n++)
            {
                if (n == row)
                {
                    string[] value = temps[n].Split(sp1);//再次的进行分割，得到最终我们要的数据
                    Value += value[0] + " " + Content + ",";
                }
                else
                {
                    Value += temps[n] + ",";
                }
            }
            hs_table_attribute[temp] = Value;
        }
        /// <summary>
        /// 单击节点时，显示对应的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeview_Name_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("字段名", typeof(string));
            table.Columns.Add("值", typeof(string));

            Hashtable hs_tb = hs_table_attribute;
            if (e.Node.Parent == null)
            {
                return;
            }
            else
            {

                layer_name = e.Node.Parent.Text;//层名
                OID = e.Node.Text;//OID

                string texts = e.Node.Parent.Text + e.Node.Text;//节点的名称
                char[] sp = { ',' };
                char[] sp1 = { ' ' };
                int count = hs_tb.Count;//键对值的数目
                string[] temp = hs_tb[texts].ToString().Substring(0, hs_tb[texts].ToString().Length - 1).Split(sp);//首先对字串处理，因为最尾会有一个，号。因先处理它，再进行分割我们要的数据
                for (int n = 0; n < temp.Length; n++)
                {
                    string[] value = temp[n].Split(sp1);//再次的进行分割，得到最终我们要的数据
                    DataRow row = table.NewRow();//创建一个行
                    row[0] = value[0];
                    row[1] = value[1];
                    table.Rows.Add(row);

                }
                DT_VIEW_Attriubte.DataSource = table;//将数据邦定到控件显示
                DT_VIEW_Attriubte.Columns[0].Width = 80;//确定列的宽
                DT_VIEW_Attriubte.Columns[1].Width = 180;
                int Count = DT_VIEW_Attriubte.Rows.Count - 1;//最后一行
                DT_VIEW_Attriubte.Rows[Count].ReadOnly = true;
                DataGridShow(DT_VIEW_Attriubte);//调用控制属性显示和控制它可编辑的方法
            }
        }
        /// <summary>
        /// 控制显示并控制可编辑属性
        /// </summary>
        private void DataGridShow(DevComponents.DotNetBar.Controls.DataGridViewX DT_VIEW_Attriubte)
        {
            if (DT_VIEW_Attriubte.Rows.Count > 0)
            {
                ArrayList list = MoData.GetOnlyReadAtt[layer_name] as ArrayList;//不可修改属性的集合
                DT_VIEW_Attriubte.Columns[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;//禁用第一列不能排序
                DT_VIEW_Attriubte.Columns[1].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;//第二列不能排序
                for (int c = 0; c < DT_VIEW_Attriubte.Rows.Count; c++)
                {
                    DT_VIEW_Attriubte.Rows[c].Height = 18;
                    string value = DT_VIEW_Attriubte.Rows[c].Cells[0].Value.ToString();
                    //判别属性的名字是否在列表当中
                    if (list.Contains(value))
                    {
                        DT_VIEW_Attriubte.Rows[c].ReadOnly = true;
                    }
                    else
                    {
                        DT_VIEW_Attriubte.Rows[c].Cells[0].ReadOnly = true;//所有的字段名是不能修改的
                    }
                }
            }
        }
        private void treeview_Name_MouseDoubleClick(object sender, MouseEventArgs e)
        {


        }
        /// <summary>
        /// 双击节点，以定位到图层上的要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeview_Name_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            AttributeShow_state.doubleclick = true;//确定这个操作是双击定位，不影响要素变更
            if (e.Node.Parent == null)
            {
                return;
            }
            else
            {
                layer_name = e.Node.Parent.Text;//层名
                OID = e.Node.Text;//OID

                int Count = myHook.MapControl.Map.LayerCount;//层的总数
                for (int n = 0; n < Count; n++)
                {
                    ILayer layer = myHook.MapControl.get_Layer(n);
                    if (layer is IGroupLayer)
                    {
                        if (layer.Name == "示意图")
                        {
                            continue;
                        }
                        #region 遍历组下面的普通层
                        ICompositeLayer pComLayer = layer as ICompositeLayer;
                        for (int j = 0; j < pComLayer.Count; j++)
                        {
                            ILayer mLayer = pComLayer.get_Layer(j);
                            if (mLayer.Name == layer_name)
                            {
                                IFeatureLayer f_layer = mLayer as IFeatureLayer;//将层转成要素层
                                int oid = Convert.ToInt32(OID);//把OID转成整形
                                IFeature feature = f_layer.FeatureClass.GetFeature(oid);//得到对应的要素
                                IGeoDataset pGeoDt = f_layer.FeatureClass as IGeoDataset;
                                ISpatialReference pSpatialRef = null;
                                if (pGeoDt != null)
                                {
                                    pSpatialRef = pGeoDt.SpatialReference;
                                }

                                //myHook.MapControl.Map.ClearSelection();//清除已选择的要素
                                myHook.MapControl.Map.SelectFeature(mLayer, feature);
                                SysCommon.Gis.ModGisPub.ZoomToFeature(myHook.MapControl, feature, pSpatialRef);//定位到相应的层,中心放大
                                myHook.MapControl.ActiveView.Refresh();
                                Application.DoEvents();
                                myHook.MapControl.FlashShape(feature.Shape, 2, 500, null);
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {

                        if (layer.Name == layer_name)
                        {
                            IFeatureLayer f_layer = layer as IFeatureLayer;//将层转成要素层
                            int oid = Convert.ToInt32(OID);//把OID转成整形
                            IFeature feature = f_layer.FeatureClass.GetFeature(oid);//得到对应的要素
                            myHook.MapControl.Map.SelectFeature(layer, feature);

                            IGeoDataset pGeoDt = f_layer.FeatureClass as IGeoDataset;
                            ISpatialReference pSpatialRef = null;
                            if (pGeoDt != null)
                            {
                                pSpatialRef = pGeoDt.SpatialReference;
                            }

                            SysCommon.Gis.ModGisPub.ZoomToFeature(myHook.MapControl, feature, pSpatialRef);//定位到相应的层,中心放大
                            myHook.MapControl.ActiveView.Refresh();
                            Application.DoEvents();
                            myHook.MapControl.FlashShape(feature.Shape, 2, 500, null);
                            break;
                        }
                    }
                }
            }
        }

        private void DT_VIEW_Attriubte_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
    }
}