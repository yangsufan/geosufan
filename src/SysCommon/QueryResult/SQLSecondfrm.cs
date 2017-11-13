using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

namespace SysCommon
{
    public partial class SQLSecondfrm :BaseForm
    {
        private IFeatureClass m_pFeaCls = null;
        private string m_strSQL = "";
        /// <summary>
        /// ZQ  20111116 add 空间过滤条件
        /// </summary>
        private IGeometry m_Geometry = null;
        /// <summary>
        /// ZQ  20111118 add  记录上一次双击选中的字段名称
        /// </summary>
        private string m_strFiledName = "";
        public IWorkspace m_pWorkspace = null;
        public IGeometry pGeometry
        {
            set { m_Geometry = value; }
        }
       //end
        public string SQL
        {
            get { return m_strSQL; }
            set { m_strSQL = value; }
        }

        public SQLSecondfrm(IFeatureClass  pFeaCls)
        {
            InitializeComponent();
            m_pFeaCls = pFeaCls;
        }
        //ygc 20130326 记录选择字段名称
        private string m_FieldName;
        private void SQLfrm_Load(object sender, EventArgs e)
         {
            if (m_pFeaCls == null) return;

            for (int j = 0; j < m_pFeaCls.Fields.FieldCount; j++)
            {
                //ZQ 2011 modify  添加中文对照
                fieldlistBox.Items.Add(m_pFeaCls.Fields.get_Field(j).Name + "【" + SysCommon.ModField.GetChineseNameOfField(m_pFeaCls.Fields.get_Field(j).AliasName) + "】");
            }
            if (fieldlistBox.SelectedItem == null)
            {
                guvbuttonX.Enabled = false;
            }

            this.sqlrichTextBox.Text = m_strSQL;
        }

        private void dybuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + dybuttonX.Text + " ";
        }

        private void jkhbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + jkhbuttonX.Text + " ";
        }

        private void likebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + likebuttonX.Text + " ";
        }

        private void ltbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + ltbuttonX.Text + " ";
        }

        private void bandebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + bandebuttonX.Text + " ";
        }

        private void andbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + andbuttonX.Text + " ";
        }

        private void stbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + stbuttonX.Text + " ";
        }

        private void sandebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + sandebuttonX.Text + " ";
        }

        private void orbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + orbuttonX.Text + " ";
        }

        private void whbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + whbuttonX.Text + " ";
        }

        private void cbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + cbuttonX.Text + " ";
        }

        private void ykhbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + ykhbuttonX.Text + " ";
        }

        private void notbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + notbuttonX.Text + " ";
        }

        private void isbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + isbuttonX.Text + " ";
        }

        private void clearbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Clear();
        }

        private void guvbuttonX_Click(object sender, EventArgs e)
        {
            
            fieldvaluelistBox.Items.Clear();
            m_FieldName = "";
            string strfieldName = fieldlistBox.SelectedItem.ToString();
            strfieldName = strfieldName.Substring(0, strfieldName.IndexOf('【'));
            m_FieldName = strfieldName;
            bool blnStr = false;
            if (m_pFeaCls.Fields.get_Field(m_pFeaCls.Fields.FindField(strfieldName)).Type == esriFieldType.esriFieldTypeString)
            {
                blnStr = true;
            }

            if (m_pFeaCls == null || this.fieldlistBox.SelectedItems.Count == 0) return;

            string sFieldName = strfieldName;        //获取选中项的字符串

            IFeatureClass pFeatureClass = m_pFeaCls;         //得到要素集合
            if (pFeatureClass == null) return;

            this.fieldvaluelistBox.Items.Clear();

            try
            {
                IFeatureCursor pCursor = null;
                if (m_Geometry == null)
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "";
                    pCursor = pFeatureClass.Search(pQueryFilter, false);
                }
                else
                {
                    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    //pSpatialFilter.GeometryField = "SHAPE";
                    pSpatialFilter.Geometry = m_Geometry;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pSpatialFilter.WhereClause = "";
                    pCursor = pFeatureClass.Search(pSpatialFilter, false);
                }
                List<string> listFieldValue = ModXZQ.GetListChineseName(m_pWorkspace, strfieldName);
                if (listFieldValue != null || listFieldValue.Count != 0)
                {
                    for (int t = 0; t < listFieldValue.Count; t++)
                    {
                        if (blnStr)
                        {
                            fieldvaluelistBox.Items.Add("'" + listFieldValue [t] + "'");
                        }
                        else
                        {
                            fieldvaluelistBox.Items.Add(listFieldValue[t]);
                        }
                    }
                    fieldvaluelistBox.Update();
                }
                else
                {
                    System.Collections.IEnumerator enumerator;
                    IDataStatistics DS = new DataStatisticsClass();
                    DS.Field = sFieldName;//设置唯一值字段
                    DS.Cursor = pCursor as ICursor;//数据来源
                    enumerator = DS.UniqueValues;//得到唯一值
                    enumerator.Reset();//从新指向第一个值
                    while (enumerator.MoveNext())//遍历唯一值
                    {
                        string strTemp = enumerator.Current.ToString();
                        if (blnStr)
                        {
                            fieldvaluelistBox.Items.Add("'" + strTemp + "'");
                        }
                        else
                        {
                            fieldvaluelistBox.Items.Add(strTemp);
                        }

                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    pCursor = null;

                    //lstValue.Sorted=true;
                    fieldvaluelistBox.Update();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取字段值发生错误，错误原因为" + ex.Message, "系统提示");
            }
        }

        private void fieldlistBox_DoubleClick(object sender, EventArgs e)
        {
            ///ZQ 201118 modify
            guvbuttonX.Enabled = true;
            string strfieldName = fieldlistBox.SelectedItem.ToString();
            if (m_strFiledName != strfieldName)
            {
                fieldvaluelistBox.Items.Clear();
                m_strFiledName = strfieldName;

            }
            strfieldName = strfieldName.Substring(0, strfieldName.IndexOf('【'));
            sqlrichTextBox.Text = sqlrichTextBox.Text + strfieldName;
            ///end
        }

        private void fieldvaluelistBox_DoubleClick(object sender, EventArgs e)
        {
            if (m_FieldName != null && m_FieldName != "")
            {
                bool isExit = false;
                string tempCode = fieldvaluelistBox.SelectedItem.ToString();
                if (tempCode.Contains("'"))
                {
                    isExit = true;
                    tempCode = tempCode.Substring(1, tempCode.LastIndexOf("'") - 1);
                }
                string fieldCode = ModXZQ.GetCode(m_pWorkspace, m_FieldName, tempCode);
                if (isExit)
                {
                    sqlrichTextBox.Text = sqlrichTextBox.Text +"'" +fieldCode+"'";
                }
                else
                {
                    sqlrichTextBox.Text = sqlrichTextBox.Text + fieldCode;
                }
            }
            else
            {
                sqlrichTextBox.Text = sqlrichTextBox.Text + fieldvaluelistBox.SelectedItem.ToString() + " ";
            }
        }

        private void closebuttonX_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private bool m_blnOk = false;

        private void okbuttonX_Click(object sender, EventArgs e)
        {
             if (sqlrichTextBox.Text.Trim() == "")
             {
                 m_strSQL = "";
                 DialogResult = DialogResult.OK;
             }
             else
             {
                 if (!m_blnOk)
                 {
                     IQueryFilter pQueryFilter;
                     pQueryFilter = new QueryFilterClass();
                     pQueryFilter.WhereClause = sqlrichTextBox.Text;

                     try
                     {
                         int intFeaCount = m_pFeaCls.FeatureCount(pQueryFilter);

                     }
                     catch
                     {
                         MessageBox.Show("SQL语句不符规则，请重新输入.");
                         return;
                     }
                 }

                 m_strSQL = sqlrichTextBox.Text;
                 DialogResult = DialogResult.OK;
                 this.Close();
             }
        }

        private void verifybuttonX_Click(object sender, EventArgs e)
        {
            if (m_pFeaCls == null) return;

            IQueryFilter pQueryFilter;
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = sqlrichTextBox.Text;
            try
            {
                int intFeatureCount = m_pFeaCls.FeatureCount(pQueryFilter);
                MessageBox.Show("验证通过","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                m_blnOk = true;
            }
            catch
            {
                MessageBox.Show("SQL语句不符规则，请重新输入.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fieldlistBox_Click(object sender, EventArgs e)
        {
            guvbuttonX.Enabled = true;
        }

        private void savebuttonX_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save SQL";
            dlg.Filter = "(*.txt)|*.txt";
            dlg.ShowDialog();
            string filename = @"" + dlg.FileName + "";
            string strnull = "";
            if (filename != strnull)
            {
                System.IO.File.WriteAllText(filename, sqlrichTextBox.Text, Encoding.Default);
            }
        }

        private void loadbuttonX_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open SQL";
            dlg.Filter = "(*.txt)|*.txt";
            dlg.ShowDialog();
            string filename=@""+dlg.FileName+"";
            string strnull = "";
            if (filename != strnull)
            {
                sqlrichTextBox.Text = System.IO.File.ReadAllText(filename, Encoding.Default);
            }
        }

        private void sqlrichTextBox_TextChanged(object sender, EventArgs e)
        {
            m_blnOk = false;
        }


    }
}
