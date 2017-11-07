using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoUtilities
{
    public partial class FrmSQLQuery : DevComponents.DotNetBar.Office2007Form
    {
        private IMapControlDefault m_MapControlDefault;
        private IMap m_pMap;
        private IFeatureLayer m_pCurrentLayer;
        private bool m_Show;
        public SysCommon.BottomQueryBar _QueryBar
        {
            get;
            set;
        }

        public FrmSQLQuery(IMapControlDefault pMapControl,bool bShow)
        {
            m_MapControlDefault = pMapControl;
            m_pMap = pMapControl.Map;
            m_Show = bShow;
            InitializeComponent();
            cmbselmode.Enabled = !m_Show;
        }
        private void btnOperateClick(DevComponents.DotNetBar.ButtonX button)
        {
            richTextExpression.Text += button.Text.Trim()+" ";
        }
        #region 操作符的点击事件
        private void btnBigger_Click(object sender, EventArgs e)
        {
             btnOperateClick(btnBigger);
        }

        private void btnSmaller_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnSmaller );
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnEqual );
        }

        private void btnBiggerEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnBiggerEqual );
        }

        private void btnSmallerEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnSmallerEqual );
        }

        private void btnNotEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnNotEqual );
        }

        private void btn1Ultra_Click(object sender, EventArgs e)
        {
            btnOperateClick(btn1Ultra );
        }

        private void btn2Ultra_Click(object sender, EventArgs e)
        {
            btnOperateClick(btn2Ultra );
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnPercent );
        }

        private void btnIs_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnIs );
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnOr );
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnNot );
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnLike );
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnAnd );
        }
        #endregion

        private void FrmSQLQuery_Load(object sender, EventArgs e)
        {
            this.richTextExpression.Text = "";
            //图层选择数据加载
            IFeatureLayer pFeaTureLayer;
            for (int iIndex = 0; iIndex < m_pMap.LayerCount; iIndex++)
            {
                ILayer pLayer = m_pMap.get_Layer(iIndex);
                if (pLayer is IGroupLayer)
                {
                    if (pLayer.Name == "示意图") continue;
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for(int i=0;i<pComLayer.Count;i++)
                    {
                        ILayer mLayer =pComLayer.get_Layer(i);
                        if (mLayer is IFeatureLayer)
                        {
                            pFeaTureLayer = mLayer as IFeatureLayer;
                            this.cmblayersel.Items.Add(pFeaTureLayer.Name);//
                        }
                    }
                }
                else if (pLayer is IFeatureLayer)
                {
                    pFeaTureLayer = m_pMap.get_Layer(iIndex) as IFeatureLayer;
                    this.cmblayersel.Items.Add(pFeaTureLayer.Name);//
                }
            }
            if (this.cmblayersel.Items.Count > 0)
            {
                this.cmblayersel.SelectedIndex = 0;
            }
            this.listViewField.Scrollable = true;
            this.listViewValue.Scrollable = true;

            //选择模式数据加载
            object[] objArray = new object[4];
            objArray[0] = "创建一个新的选择结果";
            objArray[1] = "添加到当前选择集中";
            objArray[2] = "从当前选择结果中移除";
            objArray[3] = "从当前选择结果中选择";

            this.cmbselmode.Items.AddRange(objArray);//
            this.cmbselmode.SelectedIndex = 0;

            
            //初始化字段列表
            ColumnHeader newColumnHeader1 = new ColumnHeader();
            newColumnHeader1.Width = listViewField.Width - 5;
            newColumnHeader1.Text = "字段名";
            //newColumnHeader1.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.listViewField.Columns.Add(newColumnHeader1);

            //初始化值列表
            ColumnHeader newColumnHeader2 = new ColumnHeader();
            newColumnHeader2.Width = listViewValue.Width - 5;
            newColumnHeader2.Text = "字段值";
            //newColumnHeader2.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.listViewValue.Columns.Add(newColumnHeader2);

            //范围
            this.comboExtent.SelectedIndex = 0;
        }

        private void cmblayersel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Index = this.cmblayersel.SelectedIndex;                        //获取选中项的索引

            for (int i = 0; i < m_pMap.LayerCount; i++)
            {
                ILayer pLayer = m_pMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    if (pLayer.Name == "示意图") continue;
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        if (mLayer is IFeatureLayer&&mLayer.Name==this.cmblayersel.Items[Index].ToString())
                        {
                            m_pCurrentLayer = mLayer as IFeatureLayer;
                        }
                    }
                }
                else if (pLayer is IFeatureLayer && pLayer.Name == this.cmblayersel.Items[Index].ToString())
                {
                    m_pCurrentLayer = pLayer as IFeatureLayer; 
                }
            }
                //注意在初始化时保持了两个索引的一致

            this.listViewField.Items.Clear();                             //因为字段值会随图层而改变，清空
            this.listViewValue.Items.Clear();

            if (m_pCurrentLayer == null)                                     //不存在选中图层 则提示
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示","所选图层存在错误！");
                return;
            }

            IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass; //获取要素类的集合

            //循环得到每一个字段并根据字段类型进行对应的操作
            //此处以后还要更改增加更多判断和对应操作
            for (int iIndex = 0; iIndex < pFeatureClass.Fields.FieldCount; iIndex++)
            {
                IField pField = pFeatureClass.Fields.get_Field(iIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeDouble:
                    case esriFieldType.esriFieldTypeString:
                    case esriFieldType.esriFieldTypeDate:
                    case esriFieldType.esriFieldTypeOID:
                        ListViewItem newItem = new ListViewItem(new string[] { pField.Name });
                        this.listViewField.Items.Add(newItem);
                        break;
                    default:
                        break;
                }
            }
           
            
        }
        //字段列表双击事件，将字段名称加入到richTextExpression中
        private void listViewField_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
           ListViewItem currentFieldItem= this.listViewField.GetItemAt(e.Location.X, e.Location.Y);
             if (this.listViewField.SelectedItems.Count > 1)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请只选择一个字段");
            }
            if (currentFieldItem == null) return;
            if (currentFieldItem.Selected == true)
            {
                string sValue = currentFieldItem.Text.Trim();
                this.richTextExpression.Text = this.richTextExpression.Text + sValue +" " ;
            }
           
        }

        //值列表双击事件，将字段的值加入到richTextExpression中
        private void listViewValue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem currentValueItem = this.listViewValue.GetItemAt(e.Location.X, e.Location.Y);
            //如果当前图层为空或者字段值为空，则返回
            if (m_pCurrentLayer == null || currentValueItem == null) return;
            IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass;
            IDataset pDataSet = pFeatureClass as IDataset;
            IWorkspace pWorkSpace = pDataSet.Workspace;
            if (pWorkSpace == null) return;
            string sValue = currentValueItem.Text.Trim();

            string sFieldName = this.listViewField.SelectedItems[0].Text.Trim();
            if (sFieldName == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选中字段名");
            }

            int iFieldIndex = m_pCurrentLayer.FeatureClass.Fields.FindField(sFieldName);
            IField pField = m_pCurrentLayer.FeatureClass.Fields.get_Field(iFieldIndex);

            if (pField.VarType > 1 && pField.VarType < 6)
            {
                this.richTextExpression.Text = this.richTextExpression.Text + sValue + " ";
            }
            else if (pField.VarType == 7)
            {//date //"2009-5-18 0:00:00"
                int lastIndex = sValue.IndexOf(" ");//空格的索引值
                if (pWorkSpace.Type == esriWorkspaceType.esriFileSystemWorkspace)
                {//文件型
                    sValue = sValue.Substring(0, lastIndex);
                    this.richTextExpression.Text = this.richTextExpression.Text + "date '" + sValue + "'";
                }
                else if (pWorkSpace.Type == esriWorkspaceType.esriLocalDatabaseWorkspace)
                { //GDB,PDB 
                    if (pWorkSpace.WorkspaceFactory.GetClassID().Value.ToString() == "{71FE75F0-EA0C-4406-873E-B7D53748AE7E}")
                    {//GDB
                        sValue = sValue.Substring(0, lastIndex);
                        this.richTextExpression.Text = this.richTextExpression.Text +"date '" + sValue + " 00:00:00'";
                    }
                    else
                    {//PDB
                        string interV = sValue.Substring(5,lastIndex-5);
                        string firstV = sValue.Substring(0, 4);
                        sValue = interV + "-" + firstV;
                        this.richTextExpression.Text = this.richTextExpression.Text +"#"+ sValue + " 00:00:00#";
                    }
                }
                else if (pWorkSpace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {//SDE
                    sValue = sValue.Substring(0, lastIndex);
                    this.richTextExpression.Text = this.richTextExpression.Text + "TO_DATE('" + sValue + " 00:00:00','YYYY-MM-DD HH24:MI:SS')";
                }

            }
            else if (pField.VarType == 8)
            {//string
                this.richTextExpression.Text = this.richTextExpression.Text + "'" + sValue + "'";
            }
        }

        //图层选择 不能编辑
        private void cmblayersel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        //选择方式 不能编辑
        private void cmbselmode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //显示可能的值
        private void btnDisplayValue_Click(object sender, EventArgs e)
        {
            //不存在当前层 或者 不存在 选中字段时返回
            if (m_pCurrentLayer == null || this.listViewField.SelectedItems.Count  == 0) return;

            string sFieldName = this.listViewField.SelectedItems[0].Text.Trim();        //获取选中项的字符串
            //string sFieldName = listBox_Field.SelectedValue.ToString();         //获取选中项的字符串

            IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass;         //得到要素集合
            if (pFeatureClass == null) return;

            try
            {
                //注意正确使用FeatureLayer和FeatureClass
                //IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);  //无条件查询得到全部的Feature
                IFeatureCursor pFeatureCursor = m_pCurrentLayer.Search(null, false);

                List<string> listValue = new List<string>();                        //创建唯一要素 值的集合 
                IFeature pFeature = pFeatureCursor.NextFeature();                   //取得要素实体

                this.listViewValue.Items.Clear();

                while (pFeature != null)
                {
                    int iFieldIndex = pFeature.Fields.FindField(sFieldName);            //得到字段值的索引
                    object objValue = pFeature.get_Value(iFieldIndex);

                    if (objValue != null)
                    {
                        string sValue = objValue.ToString();
                        if (!listValue.Contains(sValue))
                        {
                            if (listValue.Count > 200)                           //是否超过两百条记录
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示","可能值的记录已经超过200条，将不再继续显示!");
                                break;
                            }

                            listValue.Add(sValue);
                            ListViewItem newItem = new ListViewItem(new string[] { sValue });
                            this.listViewValue.Items.Add(newItem);
                        }
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }

                Marshal.ReleaseComObject(pFeatureCursor);
                listViewValue.Sort();
                this.listViewValue.Update();                                         //更新
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示","获取字段值发生错误，错误原因为" + ex.Message);
            }
        }

        //清除表达式
        private void btnClearExpression_Click(object sender, EventArgs e)
        {
            this.richTextExpression.ResetText();
        }
        //验证表达式
        private void btnValidateExpression_Click(object sender, EventArgs e)
        {
            if (this.richTextExpression.Text.Trim() == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示","表达式为空，请输入表达式！");
                return;
            }
            string whereClause = this.richTextExpression.Text.Trim();

            CheckExpression(whereClause, true);
        }

        //加载结果集
        private void btnLoadResults_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog();

            dlgOpenFile.FilterIndex = 2;
            dlgOpenFile.Title = "请选择需要加载的结果集";
            dlgOpenFile.Filter = "All Files (*.*)|*.*|Text Files(*.exp)|*.exp";

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                richTextExpression.LoadFile(dlgOpenFile.FileName);
            }
        }
        //保存结果集
        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgSaveFile = new SaveFileDialog();

            dlgSaveFile.FilterIndex = 2;
            dlgSaveFile.Title = "请指定保存结果集的路径";
            dlgSaveFile.Filter = "All Files (*.*)|*.*|Text Files(*.exp)|*.exp";

            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                richTextExpression.SaveFile(dlgSaveFile.FileName);
            }
        }
        //示例
        private void btnSample_Click(object sender, EventArgs e)
        {
            //清空当前的表达式框
            this.richTextExpression.ResetText();

            if (m_pCurrentLayer == null) return;

            //字段框存在字段名
            if (this.listViewField.Items.Count > 1)
            {
                //获取当前的IFeatureClass,然后取得Feature的指针
                IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass;
                //注意正确使用FeatureLayer和FeatureClass
                //IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
                IFeatureCursor pFeatureCursor = m_pCurrentLayer.Search(null, false);

                //依次取得每一个feature
                IFeature pFeature = pFeatureCursor.NextFeature();
                if(pFeature != null)
                {
                    string sValue;
                    string sFieldName = this.listViewField.Items[0].Text;
                    int iIndex = pFeatureClass.Fields.FindField(sFieldName);

                    IField pField = pFeatureClass.Fields.get_Field(iIndex);

                    //几何类型和bolb类型的值 赋为"shape"
                    if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeBlob)
                    {
                        sValue = "shape";
                    }
                    else
                    {
                        //一直循环到取得正确的值
                        sValue = pFeature.get_Value(iIndex).ToString();
                        this.richTextExpression.Text = sFieldName + " = " + "'" + sValue + "'";

                        if (CheckExpression(this.richTextExpression.Text.Trim(), false) == true) return;

                    }
                }

                Marshal.ReleaseComObject(pFeatureCursor);
            }
        }
        /// 校验表达式是否合法
        private bool CheckExpression(string sExpression, bool bShow)
        {
            if (m_pCurrentLayer == null) return false;

            //获取当前图层的 featureclass
            IFeatureClass pFeatCls = m_pCurrentLayer.FeatureClass;
            //构造查询过滤器
            IQueryFilter pFilter = null;
            if (m_pGeometryFilter == null)
            {
                pFilter = new QueryFilterClass();
            }
            else
            {
                pFilter = new SpatialFilterClass();
                ISpatialFilter pSpatialFilter = pFilter as ISpatialFilter;
                pSpatialFilter.Geometry = m_pGeometryFilter;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            }

            try
            {
                //赋值查许条件
                pFilter.WhereClause = sExpression;
                //得到查询结果
                int intCount = pFeatCls.FeatureCount(pFilter);
                if (intCount>0)
                {
                    if (bShow == true)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "表达式正确!");
                    }
                    return true;
                }
                else
                {
                    if (bShow == true)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "表达式正确，但搜索不到要素,请检查表达式");
                    }
                    return false;
                }
            }
            catch
            {
                if (bShow == true)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示","表达式书写有误,请检查表达式");
                }
                return false;
            }
        }
       //退出键
        private void FrmSQLQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose(true);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //没有当前图层直接退出
            if (m_pCurrentLayer == null) return;

            string whereClause = this.richTextExpression.Text.Trim();
            if (CheckExpression(whereClause, false) == false) return;

            //获取当前图层的 featureclass
            IFeatureClass pFeatClass = m_pCurrentLayer.FeatureClass;

            //构造查询过滤器
            IQueryFilter pQueryFilter = null;
            if (m_pGeometryFilter == null)
            {
                pQueryFilter = new QueryFilterClass();
            }
            else
            {
                pQueryFilter = new SpatialFilterClass();
                ISpatialFilter pSpatialFilter = pQueryFilter as ISpatialFilter;
                pSpatialFilter.Geometry = m_pGeometryFilter;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            }
            

            //赋值查许条件
            pQueryFilter.WhereClause = whereClause;

            //赋值查询方式,由查询方式的combo获得
            esriSelectionResultEnum pSelectionResult;
            switch (this.cmbselmode.SelectedItem.ToString())
            {
                case ("创建一个新的选择结果"):
                    pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;
                    break;
                case ("添加到当前选择集中"):
                    pSelectionResult = esriSelectionResultEnum.esriSelectionResultAdd;
                    break;
                case ("从当前选择结果中移除"):
                    pSelectionResult = esriSelectionResultEnum.esriSelectionResultSubtract;
                    break;
                case ("从当前选择结果中选择"):
                    pSelectionResult = esriSelectionResultEnum.esriSelectionResultAnd;
                    break;
                default:
                    return;
            }

            if (m_Show)
            {
                //进行查询，并将结果显示出来
               // frmQuery frm = new frmQuery(m_MapControlDefault);
                //frm.FillData(m_pCurrentLayer, pQueryFilter, pSelectionResult);
                //frm.TopMost = true;//防止查询结果最小化     张琪   20110709
                //frm.Show();
                _QueryBar.m_pMapControl = m_MapControlDefault;
                _QueryBar.EmergeQueryData(m_MapControlDefault.Map, m_pCurrentLayer, pQueryFilter, pSelectionResult);
                try
                {
                    DevComponents.DotNetBar.Bar pBar = _QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                    if (pBar != null)
                    {
                        pBar.AutoHide = false;
                        //pBar.SelectedDockTab = 1;
                        int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                        pBar.SelectedDockTab = tmpindex;
                    }
                }
                catch
                { }
            }
            else
            {
                IFeatureSelection pFeatureSelection = m_pCurrentLayer as IFeatureSelection;
                pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
                m_MapControlDefault.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_MapControlDefault.ActiveView.FullExtent);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 空间关系 范围控制
        /// </summary>
        private IGeometry m_pGeometryFilter = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboExtent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pMap == null) return;

            switch (this.comboExtent.SelectedItem.ToString())
            {
                case "全图范围":
                    m_pGeometryFilter = null;
                    break;
                case "当前视图范围":
                    IActiveView pAv=m_pMap as IActiveView;
                    m_pGeometryFilter = pAv.Extent;
                    break;
                case "选择要素范围":
                    m_pGeometryFilter = ConstructUnion(m_pMap);
                    if (m_pGeometryFilter == null)
                    {
                        MessageBox.Show("当前视图无选择要素。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    break;
            }
        }

        /// <summary>
        /// 获得当前视图上的选择要素
        /// </summary>
        /// <param name="pMap"></param>
        /// <returns></returns>
        private IGeometry ConstructUnion(IMap pMap)
        {
            ISelection pSelection = pMap.FeatureSelection;

            //
            IEnumFeature pEnumFeature = pSelection as IEnumFeature;
            IFeature pFeature = pEnumFeature.Next();

            //考虑到有点 线 面的情况 所以只选第一个要素
            if (pFeature == null) return null;

            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator2 pTopo2 = pGeometry as ITopologicalOperator2;
            pTopo2.IsKnownSimple_2 = false;
            pTopo2.Simplify();

            return pTopo2 as IGeometry;
        }

    }
}