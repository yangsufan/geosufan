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

namespace GeoDBATool
{
    public partial class FrmSQLQuery : DevComponents.DotNetBar.Office2007Form
    {
        private IFeatureClass m_CurrentFeaCls;
        private string m_WhereClause = "";
        public string WhereClause
        {
            get
            {
                return m_WhereClause;
            }
            set
            {
                m_WhereClause = value;
            }
        }

        public FrmSQLQuery(IFeatureClass mFeaCls, List<IField> fieldList)
        {
            m_CurrentFeaCls = mFeaCls;
            if (m_CurrentFeaCls == null) return;
            InitializeComponent();

            IntialCom(fieldList);
        }

        /// <summary>
        /// 初始化字段名称列表
        /// </summary>
        /// <param name="fieldList"></param>
        private void IntialCom(List<IField> fieldList)
        {
            this.richTextExpression.Text = "";
            this.listViewField.Scrollable = true;
            this.listViewValue.Scrollable = true;


            //初始化字段列表
            ColumnHeader newColumnHeader1 = new ColumnHeader();
            newColumnHeader1.Width = listViewField.Width - 5;
            newColumnHeader1.Text = "字段名";
            this.listViewField.Columns.Add(newColumnHeader1);

            //初始化值列表
            ColumnHeader newColumnHeader2 = new ColumnHeader();
            newColumnHeader2.Width = listViewValue.Width - 5;
            newColumnHeader2.Text = "字段值";
            this.listViewValue.Columns.Add(newColumnHeader2);

            //清空列表
            listViewField.Items.Clear();
            listViewValue.Items.Clear();

            //初始化字段列表
            for (int i = 0; i < fieldList.Count; i++)
            {
                if (fieldList[i].Type == esriFieldType.esriFieldTypeBlob) continue;
                if (fieldList[i].Type == esriFieldType.esriFieldTypeGeometry) continue;
                if (fieldList[i].Type == esriFieldType.esriFieldTypeRaster) continue;
                if (fieldList[i].Type == esriFieldType.esriFieldTypeOID) continue;
                listViewField.Items.Add(fieldList[i].Name);
            }

        }

        #region 操作符的点击事件
        private void btnBigger_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnBigger);
        }

        private void btnSmaller_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnSmaller);
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnEqual);
        }

        private void btnBiggerEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnBiggerEqual);
        }

        private void btnSmallerEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnSmallerEqual);
        }

        private void btnNotEqual_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnNotEqual);
        }

        private void btn1Ultra_Click(object sender, EventArgs e)
        {
            btnOperateClick(btn1Ultra);
        }

        private void btn2Ultra_Click(object sender, EventArgs e)
        {
            btnOperateClick(btn2Ultra);
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnPercent);
        }

        private void btnIs_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnIs);
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnOr);
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnNot);
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnLike);
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
            btnOperateClick(btnAnd);
        }

        private void btnOperateClick(DevComponents.DotNetBar.ButtonX button)
        {
            richTextExpression.Text += button.Text.Trim() + " ";
        }
        #endregion


        //字段列表双击事件，将字段名称加入到richTextExpression中
        private void listViewField_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            ListViewItem currentFieldItem = this.listViewField.GetItemAt(e.Location.X, e.Location.Y);
            if (this.listViewField.SelectedItems.Count > 1)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请只选择一个字段");
            }
            if (currentFieldItem == null) return;
            if (currentFieldItem.Selected == true)
            {
                string sValue = currentFieldItem.Text.Trim();
                this.richTextExpression.Text = this.richTextExpression.Text + sValue + " ";
            }

        }

        //值列表双击事件，将字段的值加入到richTextExpression中
        private void listViewValue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem currentValueItem = this.listViewValue.GetItemAt(e.Location.X, e.Location.Y);
            //如果当前图层为空或者字段值为空，则返回
            if (currentValueItem == null) return;
            IDataset pDataSet = m_CurrentFeaCls as IDataset;
            IWorkspace pWorkSpace = pDataSet.Workspace;
            if (pWorkSpace == null) return;
            string sValue = currentValueItem.Text.Trim();

            string sFieldName = this.listViewField.SelectedItems[0].Text.Trim();
            if (sFieldName == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选中字段名");
            }

            int iFieldIndex = m_CurrentFeaCls.Fields.FindField(sFieldName);
            IField pField = m_CurrentFeaCls.Fields.get_Field(iFieldIndex);

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
                        this.richTextExpression.Text = this.richTextExpression.Text + "date '" + sValue + " 00:00:00'";
                    }
                    else
                    {//PDB
                        string interV = sValue.Substring(5, lastIndex - 5);
                        string firstV = sValue.Substring(0, 4);
                        sValue = interV + "-" + firstV;
                        this.richTextExpression.Text = this.richTextExpression.Text + "#" + sValue + " 00:00:00#";
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

        //显示可能的值
        private void btnDisplayValue_Click(object sender, EventArgs e)
        {
            //不存在当前层 或者 不存在 选中字段时返回
            if (this.listViewField.SelectedItems.Count == 0) return;

            string sFieldName = this.listViewField.SelectedItems[0].Text.Trim();        //获取选中项的字符串

            try
            {
                //注意正确使用FeatureLayer和FeatureClass
                IFeatureCursor pFeatureCursor = m_CurrentFeaCls.Search(null, false);

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
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "可能值的记录已经超过200条，将不再继续显示!");
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
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "获取字段值发生错误，错误原因为" + ex.Message);
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "表达式为空，请输入表达式！");
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


            //字段框存在字段名
            if (this.listViewField.Items.Count > 1)
            {
                //获取当前的IFeatureClass,然后取得Feature的指针

                //注意正确使用FeatureLayer和FeatureClass
                IFeatureCursor pFeatureCursor = m_CurrentFeaCls.Search(null, false);

                //依次取得每一个feature
                IFeature pFeature = pFeatureCursor.NextFeature();
                if (pFeature != null)
                {
                    string sValue;
                    string sFieldName = this.listViewField.Items[0].Text;
                    int iIndex = m_CurrentFeaCls.Fields.FindField(sFieldName);

                    IField pField = m_CurrentFeaCls.Fields.get_Field(iIndex);

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

            //构造查询过滤器
            IQueryFilter pFilter = new QueryFilterClass();

            try
            {
                //赋值查许条件
                pFilter.WhereClause = sExpression;
                //得到查询结果
                //注意正确使用FeatureLayer和FeatureClass
                //IFeatureCursor pFeatCursor = m_pCurrentLayer.Search(pFilter, false);
                IFeatureCursor pFeatCursor = m_CurrentFeaCls.Search(pFilter, false);
                //取得第一个feature
                IFeature pFeat = pFeatCursor.NextFeature();
                Marshal.ReleaseComObject(pFeatCursor);
                if (pFeat != null)
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
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "此表达式搜索不到要素,请检查表达式");
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                if (bShow == true)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("系统提示", "此表达式搜索不到要素,请检查表达式");
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
            string whereClause = this.richTextExpression.Text.Trim();
            if (CheckExpression(whereClause, false) == false) return;

            m_WhereClause = whereClause;
            //获取当前图层的 featureclass
            //IFeatureClass pFeatClass = m_pCurrentLayer.FeatureClass;

            ////构造查询过滤器
            //IQueryFilter pQueryFilter = new QueryFilterClass();
            ////赋值查许条件
            //pQueryFilter.WhereClause = whereClause;

            ////赋值查询方式,由查询方式的combo获得
            //esriSelectionResultEnum pSelectionResult;
            //switch (this.cmbselmode.SelectedItem.ToString())
            //{
            //    case ("创建一个新的选择结果"):
            //        pSelectionResult = esriSelectionResultEnum.esriSelectionResultNew;
            //        break;
            //    case ("添加到当前选择集中"):
            //        pSelectionResult = esriSelectionResultEnum.esriSelectionResultAdd;
            //        break;
            //    case ("从当前选择结果中移除"):
            //        pSelectionResult = esriSelectionResultEnum.esriSelectionResultSubtract;
            //        break;
            //    case ("从当前选择结果中选择"):
            //        pSelectionResult = esriSelectionResultEnum.esriSelectionResultAnd;
            //        break;
            //    default:
            //        return;
            //}

            //if (m_Show)
            //{
            //    //进行查询，并将结果显示出来
            //    GeoUtilities.frmQuery frm = new GeoUtilities.frmQuery(m_MapControlDefault);
            //    frm.FillData(m_pCurrentLayer, pQueryFilter, pSelectionResult);
            //    frm.Show();
            //}
            //else
            //{
            //    IFeatureSelection pFeatureSelection = m_pCurrentLayer as IFeatureSelection;
            //    pFeatureSelection.SelectFeatures(pQueryFilter, pSelectionResult, false);
            //    m_MapControlDefault.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_MapControlDefault.ActiveView.FullExtent);
            //}

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}