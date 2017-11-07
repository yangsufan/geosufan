using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;

namespace GeoDataCenterFunLib
{
    public partial class FrmSQLQuery : DevComponents.DotNetBar.Office2007Form
    {
        private IMapControlDefault m_MapControlDefault;
        private IMap m_pMap;
        private IFeatureLayer m_pCurrentLayer;
        ///添加业务库用于初始化数据字典   20111011  Zq add
        private IWorkspace m_Workspace = null;
        public string _LayerTreePath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\查询图层树.xml"; //图层目录文件路径
        private XmlDocument _LayerTreeXmldoc = null;
        private Dictionary<string, string> _DicLayerList = null;
        public SysCommon.BottomQueryBar _QueryBar
        {
            get;
            set;
        }
        private string LayerID;
        public FrmSQLQuery(IMapControlDefault pMapControl, IWorkspace pWorkspace)
        {
            m_MapControlDefault = pMapControl;
            m_pMap = pMapControl.Map;
            ///初始化数据字典  20111011  Zq add
            m_Workspace = pWorkspace;
            if (SysCommon.ModField._DicFieldName.Count == 0)
            {
                SysCommon.ModField.InitNameDic(m_Workspace, SysCommon.ModField._DicFieldName, "属性对照表");
            }

            InitializeComponent();
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
            //改变图层选择方式 屏蔽之前代码 xisheng 20111119
            //IFeatureLayer pFeaTureLayer;
            //DevComponents.Editors.ComboItem item;
            //for (int iIndex = 0; iIndex < m_pMap.LayerCount; iIndex++)
            //{
            //    ILayer pLayer = m_pMap.get_Layer(iIndex);
            //    if (pLayer is IFeatureLayer)
            //    {
            //        pFeaTureLayer = pLayer as IFeatureLayer;
            //        item = new DevComponents.Editors.ComboItem();
            //        item.Text = pFeaTureLayer.Name;
            //        item.Tag = pFeaTureLayer;
            //        this.cmblayersel.Items.Add(item);//
            //    }
            //    else
            //    {
            //        if (pLayer is ICompositeLayer)
            //        {
            //            ICompositeLayer pCLayer=pLayer as ICompositeLayer;
            //            for (int j = 0; j < pCLayer.Count; j++)
            //            {
            //                ILayer pTemLayer = pCLayer.get_Layer(j);
            //                if (pTemLayer is IFeatureLayer)
            //                {
            //                    pFeaTureLayer = pTemLayer as IFeatureLayer;
            //                    item = new DevComponents.Editors.ComboItem();
            //                    item.Text = pFeaTureLayer.Name;
            //                    item.Tag = pFeaTureLayer;
            //                    this.cmblayersel.Items.Add(item);//
            //                }
            //            }
            //        }
            //    }
            //}
            //if (this.cmblayersel.Items.Count > 0)
            //{
            //    this.cmblayersel.SelectedIndex = 0;
            //    this.cmblayersel.Sorted = true;
            //}
            cmblayersel.Text = "点击选择查询图层";

            //选择模式数据加载
            object[] objArray = new object[5];
            objArray[0] = "不创建选择结果";
            objArray[1] = "创建一个新的选择结果";
            objArray[2] = "添加到当前选择集中";
            objArray[3] = "从当前选择结果中移除";
            objArray[4] = "从当前选择结果中选择";

            this.cmbselmode.Items.AddRange(objArray);//
            this.cmbselmode.SelectedIndex = 0;

            
            //初始化字段列表
            ColumnHeader newColumnHeader1 = new ColumnHeader();
            newColumnHeader1.Text = "Name";
            //newColumnHeader1.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.listViewField.Columns.Add(newColumnHeader1);

            //初始化值列表
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath);
            ColumnHeader newColumnHeader2 = new ColumnHeader();
            newColumnHeader2.Text = "Value";
            //newColumnHeader2.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.listViewValue.Columns.Add(newColumnHeader2);


            //ygc 初始化图层树
             if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (_DicLayerList == null)
            {
                _DicLayerList = new Dictionary<string, string>();
            }
            //初始化图层树列表
            if (File.Exists(_LayerTreePath))
            {
                if (_LayerTreeXmldoc == null)
                {
                    _LayerTreeXmldoc = new XmlDocument();
                }
                _LayerTreeXmldoc.Load(_LayerTreePath);
                advTreeLayers.Nodes.Clear();

                //获取Xml的根节点并作为根节点加到UltraTree上
                XmlNode xmlnodeRoot = _LayerTreeXmldoc.DocumentElement;
                XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

                xmlelementRoot.SetAttribute("NodeKey", "Root");
                string sNodeText = xmlelementRoot.GetAttribute("NodeText");

                //创建并设定树的根节点
                DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
                treenodeRoot.Name = "Root";
                treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                this.advTreeLayers.Nodes.Add(treenodeRoot);

                treenodeRoot.Image = this.ImageList.Images["Root"];
                InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);
            }
        }
        
        //改变图层选择方式 屏蔽之前代码 xisheng 20111119
        private void cmblayersel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cmblayersel.SelectedItem == null) return;
            //DevComponents.Editors.ComboItem item = this.cmblayersel.SelectedItem as DevComponents.Editors.ComboItem;                        //获取选中项
            //if (item == null) return;

            //m_pCurrentLayer = item.Tag as IFeatureLayer;      //注意在初始化时保持了两个索引的一致

            //this.listViewField.Items.Clear();                             //因为字段值会随图层而改变，清空
            //this.listViewValue.Items.Clear();

            //if (m_pCurrentLayer == null)                                     //不存在选中图层 则提示
            //{
            //    MessageBox.Show("所选图层存在错误！", "提示");
            //    return;
            //}

            //IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass; //获取要素类的集合

            ////循环得到每一个字段并根据字段类型进行对应的操作
            ////此处以后还要更改增加更多判断和对应操作
            //for (int iIndex = 0; iIndex < pFeatureClass.Fields.FieldCount; iIndex++)
            //{
            //    IField pField = pFeatureClass.Fields.get_Field(iIndex);
            //    switch (pField.Type)
            //    {
            //        case esriFieldType.esriFieldTypeSmallInteger:
            //        case esriFieldType.esriFieldTypeInteger:
            //        case esriFieldType.esriFieldTypeSingle:
            //        case esriFieldType.esriFieldTypeDouble:
            //        case esriFieldType.esriFieldTypeString:
            //            ListViewItem newItem = new ListViewItem(new string[] { pField.AliasName + "【" + SysCommon.ModField.GetChineseNameOfField(pField.AliasName)+"】" });
            //            newItem.Tag =pField.Name;
            //            //////处理字段名过长显示问题
            //            newItem.ToolTipText = SysCommon.ModField.GetChineseNameOfField(pField.AliasName);
            //            this.listViewField.Items.Add(newItem);
            //            break;
            //        default:
            //            break;
            //    }
            //}
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
                string sValue = currentFieldItem.Tag.ToString();
                this.richTextExpression.Text = this.richTextExpression.Text + sValue +" " ;
            }           
        }

        //值列表双击事件，将字段的值加入到richTextExpression中
        private void listViewValue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem currentValueItem = this.listViewValue.GetItemAt(e.Location.X, e.Location.Y);

            if (m_pCurrentLayer == null || currentValueItem == null) return;
            string sValue = currentValueItem.Text.Trim();
            if (sValue.Contains("[") && sValue.Contains("]"))
            {
                sValue = currentValueItem.Tag.ToString();
            }
            string sFieldName = this.listViewField.SelectedItems[0].Tag.ToString();
            if (sFieldName == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选中字段名");
            }

            int iFieldIndex = m_pCurrentLayer.FeatureClass.Fields.FindField(sFieldName);
            IField pField = m_pCurrentLayer.FeatureClass.Fields.get_Field(iFieldIndex);

            //
            string tempValue = SysCommon.ModXZQ.GetCode(Plugin.ModuleCommon.TmpWorkSpace, sFieldName, sValue);
            if (pField.VarType > 1 && pField.VarType < 6)
            {
                this.richTextExpression.Text = this.richTextExpression.Text + tempValue + " ";
            }
            else
            {
                this.richTextExpression.Text = this.richTextExpression.Text + "'" + tempValue + "'";
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
        private bool DisplayUniqueValue(string strFieldName)
        {
            this.listViewValue.Items.Clear();
            if (!File.Exists(SysCommon.ModField._MatchFieldValuepath))
                return false;
            //读取配置文件
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(SysCommon.ModField._MatchFieldValuepath);
            string strSearch = "//MatchFieldConfig/Field[@FieldName='"+strFieldName +"']";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
                return false;
            string strTableName = pNode.Attributes["TableName"].Value.ToString();
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace );
            Exception exError = null;
            List<Dictionary<string, object>> lstDicData = sysTable.GetRows(strTableName, "", out exError);
            if (lstDicData == null)
                return false;
            try
            {
                if (lstDicData.Count > 0)
                {
                    for (int i = 0; i < lstDicData.Count; i++)
                    {
                        string strName = "";
                        string strAliasName = "";
                        if (lstDicData[i]["CODE"] != null)
                            strName = lstDicData[i]["CODE"].ToString();
                        if (lstDicData[i]["NAME"] != null)
                            strAliasName = lstDicData[i]["NAME"].ToString();
                        //将属性名及别名添加到字典中
                        if ((!strName.Equals("")) && (!strAliasName.Equals("")))
                        {
                            ListViewItem newItem = new ListViewItem(new string[] { strName+"["+strAliasName +"]" });
                            newItem.Tag = strName;
                            this.listViewValue.Items.Add(newItem);
                        }
                    }
                    return true;
                }
            }
            catch
            { }
            return false;
            
        }
        //显示可能的值
        private void btnDisplayValue_Click(object sender, EventArgs e)
        {
            //不存在当前层 或者 不存在 选中字段时返回
            if (m_pCurrentLayer == null || this.listViewField.SelectedItems.Count  == 0) return;

            string sFieldName = this.listViewField.SelectedItems[0].Tag.ToString();         //获取选中项的字符串
            bool bRes=DisplayUniqueValue(sFieldName);
            if (bRes)
            {
                return;
            }
            //string sFieldName = listBox_Field.SelectedValue.ToString();                   //获取选中项的字符串

            IFeatureClass pFeatureClass = m_pCurrentLayer.FeatureClass;                     //得到要素集合
            if (pFeatureClass == null) return;

            try
            {

                //注意正确使用FeatureLayer和FeatureClass
                //IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);  //无条件查询得到全部的Feature
                IFeatureCursor pFeatureCursor = m_pCurrentLayer.Search(null, false);
                //ygc 20130326 添加获取字典代替获取唯一值
                List<string> listValue = new List<string>();                        //创建唯一要素 值的集合 
                listValue = SysCommon.ModXZQ.GetListChineseName(Plugin.ModuleCommon.TmpWorkSpace, sFieldName);
                if (listValue.Count != 0)
                {
                    for (int t = 0; t < listValue.Count; t++)
                    {
                        ListViewItem newItem = new ListViewItem(new string[] { listValue[t] });
                        this.listViewValue.Items.Add(newItem);
                    }
                }
                else
                {
                    IFeature pFeature = pFeatureCursor.NextFeature();                   //取得要素实体

                    this.listViewValue.Items.Clear();

                    while (pFeature != null)
                    {
                        int iFieldIndex = pFeature.Fields.FindField(sFieldName);            //得到字段值的索引
                        object objValue = pFeature.get_Value(iFieldIndex);

                        if (objValue != null)
                        {
                            string sValue = objValue.ToString();
                            if (!string.IsNullOrEmpty(sValue))
                            {
                                if (!listValue.Contains(sValue))
                                {
                                    if (listValue.Count > 200)                           //是否超过两百条记录
                                    {
                                        MessageBox.Show("可能值的记录已经超过200条，将不再继续显示!", "提示");
                                        break;
                                    }

                                    listValue.Add(sValue);
                                    ListViewItem newItem = new ListViewItem(new string[] { sValue });
                                    this.listViewValue.Items.Add(newItem);
                                }
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                }
                listViewValue.Sort();
                this.listViewValue.Update();                                         //更新
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取字段值发生错误，错误原因为" + ex.Message, "提示");
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
                MessageBox.Show("表达式为空，请输入表达式！", "提示");
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

                for (int i = 0; i < this.listViewField.Items.Count; i++)
                {
                    IFeatureCursor pFeatureCursor = m_pCurrentLayer.Search(null, false);
                    //依次取得每一个feature
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    while (pFeature != null)
                    {
                        string sValue;
                        string sFieldName = this.listViewField.Items[i].Tag.ToString();
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
                            if (!string.IsNullOrEmpty(sValue))
                            {
                                this.richTextExpression.Text = sFieldName + " = " + "'" + sValue + "'";
                                if (CheckExpression(this.richTextExpression.Text.Trim(), false) == true) return;
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                }
            }
        }
        /// 校验表达式是否合法
        private bool CheckExpression(string sExpression, bool bShow)
        {
            if (m_pCurrentLayer == null) return false;

            //获取当前图层的 featureclass
            IFeatureClass pFeatCls = m_pCurrentLayer.FeatureClass;
            //构造查询过滤器
            IQueryFilter pFilter = new QueryFilterClass();

            try
            {
                //赋值查许条件
                pFilter.WhereClause = sExpression;
                //得到查询结果
                //注意正确使用FeatureLayer和FeatureClass
                //IFeatureCursor pFeatCursor = pFeatCls.Search(pFilter, false);
                IFeatureCursor pFeatCursor = m_pCurrentLayer.Search(pFilter, false);
                //取得第一个feature
                IFeature pFeat = pFeatCursor.NextFeature();
                if (pFeat != null)
                {
                    if (bShow == true)
                    {
                        MessageBox.Show("表达式正确!", "提示");
                    }
                    return true;
                }
                else
                {
                    if (bShow == true)
                    {
                        MessageBox.Show("此表达式搜索不到要素,请检查表达式", "提示");
                    }
                    return false;
                }
            }
            catch
            {
                if (bShow == true)
                {
                    MessageBox.Show("此表达式搜索不到要素,请检查表达式", "提示");
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
           //加进度条 xisheng 2011.06.28
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("开始查询");

            bool bRes = false;  //是否查询到地物
            this.DialogResult = DialogResult.OK;
           
            //没有当前图层直接退出
            if (m_pCurrentLayer == null)
            {
                vProgress.Close();// 张琪  20110705  添加
                return;
            }
            try
            {
                string whereClause = this.richTextExpression.Text.Trim();
                if (CheckExpression(whereClause, false) == false)   //changed by chulili 20120818 不弹提示框
                {
                    vProgress.Close();// 张琪  20110705  添加
                    MessageBox.Show("此表达式搜索不到要素,请检查表达式", "提示");
                    return;

                }
                //获取当前图层的 featureclass
                vProgress.SetProgress("获取当前图层");
                IFeatureClass pFeatClass = m_pCurrentLayer.FeatureClass;

                //构造查询过滤器
                IQueryFilter pQueryFilter = new QueryFilterClass();
                //赋值查许条件
                vProgress.SetProgress("构造查询过滤器并赋值查询条件");
                pQueryFilter.WhereClause = whereClause;

                //赋值查询方式,由查询方式的combo获得
                esriSelectionResultEnum pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;
                bool bSelection = true;
                switch (this.cmbselmode.SelectedItem.ToString())
                {
                    case ("不创建选择结果"):
                        bSelection = false;
                        break;
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
                        vProgress.Close();// 张琪  20110705  添加
                        return;
                }


                //进行查询，并将结果显示出来
                vProgress.SetProgress("正在查询符合条件的结果");
                //frmQuery frm = new frmQuery(m_MapControlDefault);
                //frm.FillData(m_pCurrentLayer, pQueryFilter, pSelectionResult);
                _QueryBar.m_pMapControl = m_MapControlDefault;
                if (!bSelection)
                {
                    _QueryBar.EmergeQueryData(m_pCurrentLayer, pQueryFilter, vProgress);
                }
                else
                {
                    _QueryBar.EmergeQueryData(m_MapControlDefault.Map, m_pCurrentLayer, pQueryFilter, pSelectionResult, vProgress);
                }
                try
                {
                    DevComponents.DotNetBar.Bar pBar = _QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                    if (pBar != null)
                    {
                        pBar.AutoHide = false;
                        int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                        pBar.SelectedDockTab = tmpindex;
                    }
                }
                catch
                { }
                bRes = true;
            }
            //frm.Show();
            catch
            { }
            finally
            {
                vProgress.Close();
                if (bRes)
                {
                    this.Hide();
                    this.Dispose(true);
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose(true);
        }
        //增加Click事件弹出目录选择图层 xisheng 20111119
        private void cmblayersel_Click(object sender, EventArgs e)
        {            
            //this.Width = this.advTreeLayerList.Left + this.advTreeLayerList.Width + 5;
            //this.advTreeLayers.Location = new Point(this.cmblayersel.Location.X+this.groupPanel1.Location.X , this.cmblayersel.Location.Y+this.groupPanel1.Location.Y);
            this.advTreeLayers.Width = this.cmblayersel.Width;
            this.advTreeLayers.Visible = true;
            this.advTreeLayers.Focus();

        }
        //增加选择图层使图层名改变时 xs 20111119
        private void cmblayersel_TextChanged(object sender, EventArgs e)
        {
            if (m_pCurrentLayer == null||m_pCurrentLayer.FeatureClass==null)
                return;
            this.listViewField.Items.Clear();                             //因为字段值会随图层而改变，清空
            this.listViewValue.Items.Clear();

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
                        ListViewItem newItem = new ListViewItem(new string[] { pField.AliasName + "【" + SysCommon.ModField.GetChineseNameOfField(pField.AliasName) + "】" });
                        newItem.Tag = pField.Name;
                        //////处理字段名过长显示问题
                        newItem.ToolTipText = SysCommon.ModField.GetChineseNameOfField(pField.AliasName);
                        this.listViewField.Items.Add(newItem);
                        break;
                    default:
                        break;
                }
            }
        }
        private void btnSaveCondition_Click(object sender, EventArgs e)
        {
            FrmSaveSQLSolution frmSave = new FrmSaveSQLSolution(m_Workspace);
            frmSave.m_TableName = "SQLSOLUTION";
            frmSave.m_Condition = richTextExpression.Text;
            if (LayerID == "" || LayerID == null)
            {
                MessageBox.Show("请选择要查询的图层！","提示");
                return;
            }
            if (richTextExpression.Text == "")
            {
                MessageBox.Show("请输入查询条件！","提示");
                return;
            }
            frmSave.m_LayerID = LayerID;
            frmSave.m_layerName = m_pCurrentLayer.Name;
            frmSave.ShowDialog();
        }
        private void btnOpenConditon_Click(object sender, EventArgs e)
        {
            if (LayerID == "" || LayerID == null)
            {
                MessageBox.Show("请选择查询的图层数据！","提示");
                return;
            }
            if (m_Workspace == null) return;
            FrmOpenSQLCondition frmOpen = new FrmOpenSQLCondition(m_Workspace);
            frmOpen.m_TableName = "SQLSOLUTION";
            frmOpen.m_LayerId = LayerID;
            if (frmOpen.ShowDialog () == DialogResult.OK)
            {
                richTextExpression.Text = "";
                richTextExpression.Text = frmOpen.m_Condition;
            }
        }

        //根据配置文件显示图层树
        private void InitLayerTreeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode)
        {

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
                XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
                if (xmlElementChild == null)
                {
                    continue;
                }
                else if (xmlElementChild.Name == "ConfigInfo")
                {
                    continue;
                }
                //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
                if (!Plugin.ModuleCommon.ListUserdataPriID.Contains(sNodeKey))
                {
                    continue;
                }
                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;

                treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;


                treenode.Nodes.Add(treenodeChild);

                //递归
                if (xmlElementChild.Name != "Layer")
                {
                    InitLayerTreeByXmlNode(treenodeChild, xmlElementChild as XmlNode);
                }

                InitializeNodeImage(treenodeChild);
            }

        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        private void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image = this.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = this.ImageList.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = this.ImageList.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = this.ImageList.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = this.ImageList.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = this.ImageList.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = this.ImageList.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = this.ImageList.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = this.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = this.ImageList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = this.ImageList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = this.ImageList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = this.ImageList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = this.ImageList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = this.ImageList.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = this.ImageList.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = this.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = this.ImageList.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = this.ImageList.Images["RD"];
                    break;
                case "SubType":
                    treenode.Image = this.ImageList.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }

        private void advTreeLayerList_Click(object sender, EventArgs e)
        {

        }
        //通过NODE 得到NODYKEY
        private string GetNodeKey(DevComponents.AdvTree.Node Node)
        {
           // labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.DataKey;
            XmlElement xmlelement = xmlnode as XmlElement;
            string strDataType = "";
            if (xmlelement.HasAttribute("DataType"))
            {
                strDataType = xmlnode.Attributes["DataType"].Value;
            }
            if (strDataType == "RD" || strDataType == "RC")//是影像数据 返回
            {
               // labelErr.Text = "请选择矢量数据进行操作!";
                return "";
            }
                if (xmlelement.HasAttribute("IsQuery"))
                {
                    if (xmlelement["IsQuery"].Value == "False")
                    {
                       // labelErr.Text = "该图层不可查询!";
                        return "";
                    }
                }
            if (xmlelement.HasAttribute("NodeKey"))
            {
                return xmlelement.GetAttribute("NodeKey");
              
            }
            return "";

        }

        private void btnLayerOK_Click(object sender, EventArgs e)
        {
            DealSelectNode();
        }

        private void btnLayerCancel_Click(object sender, EventArgs e)
        {
            this.Width = this.advTreeLayerList.Left+2;
        }
        private void DealSelectNode()
        {
            LayerID = "";
            if (advTreeLayerList.SelectedNode == null)
                return;
            if (advTreeLayerList.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
            {
                return;
            }

            GetNodeKey(advTreeLayerList.SelectedNode);
            if (string.IsNullOrEmpty(LayerID))
                return;
            m_pCurrentLayer = new FeatureLayerClass();
            IFeatureClass pClass=SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath, LayerID);
            if (pClass != null)
            {
                m_pCurrentLayer.FeatureClass = pClass;
                cmblayersel.Text = advTreeLayerList.SelectedNode.Text;
                m_pCurrentLayer.Name = advTreeLayerList.SelectedNode.Text;// xisheng 20111122 自定义查询无名称BUG修改

            }
            this.Width = this.advTreeLayerList.Left + 7;
        }
        private void DealSelectNodeEX()
        {
            LayerID = "";
            if (this.advTreeLayers.SelectedNode == null)
                return;
            if (advTreeLayers.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
            {
                return;
            }

            LayerID=GetNodeKey(advTreeLayers.SelectedNode);
            if (string.IsNullOrEmpty(LayerID))
                return;

            this.advTreeLayers.Visible = false;

            m_pCurrentLayer = new FeatureLayerClass();
            IFeatureClass pClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, _LayerTreePath, LayerID);
            if (pClass != null)
            {
                m_pCurrentLayer.FeatureClass = pClass;
                cmblayersel.Text = advTreeLayers.SelectedNode.Text;
                m_pCurrentLayer.Name = advTreeLayers.SelectedNode.Text;// xisheng 20111122 自定义查询无名称BUG修改

            }
        }
        private void advTreeLayerList_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DealSelectNode();
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            groupPanelFewer.Visible = false;
        }

        private void btnEqual2_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnEqual);
        }

        private void advTreeLayers_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DealSelectNodeEX();
        }
        #region  选择图层的树图失去焦点
        private void FrmSQLQuery_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void advTreeLayers_Leave(object sender, EventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }
        private void groupPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void groupPanel2_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void groupPanelFewer_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void groupPanel4_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void groupPanel5_MouseClick(object sender, MouseEventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }      
        private void labelX1_Click(object sender, EventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void labelX2_Click(object sender, EventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {
            this.advTreeLayers.Visible = false;
        }
        #endregion
    }
}