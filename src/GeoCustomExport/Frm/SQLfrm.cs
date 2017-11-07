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

using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SpatialAnalyst;
namespace GeoCustomExport
{
    public partial class SQLfrm :DevComponents.DotNetBar.Office2007Form
    {
        public SQLfrm(IFeatureClass pFeatureClass,string txtsql)
        {
            InitializeComponent();
        
            m_FeatureClass = pFeatureClass;
            sqlrichTextBox.Text = txtsql;
        }
      
        public IFeatureClass m_FeatureClass;
        /// <summary>
        /// ZQ  20111118 add  记录上一次双击选中的字段名称
        /// </summary>
        private string m_strFiledName = "";
        public string SqlTxt//得到SQL语句的文本
        {
            get { return sqlrichTextBox.Text; }
        }
        private void SQLfrm_Load(object sender, EventArgs e)
        {
            for (int j = 0; j < m_FeatureClass.Fields.FieldCount; j++)
            {
                ///Zq  20111027  modify 
                fieldlistBox.Items.Add(m_FeatureClass.Fields.get_Field(j).Name + "【" + m_FeatureClass.Fields.get_Field(j).AliasName + "】");
            }
            if (fieldlistBox.SelectedItem == null)
            {
                guvbuttonX.Enabled = false;
            }

        }

        private void dybuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + dybuttonX.Text; ;
        }

        private void jkhbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + jkhbuttonX.Text; ;
        }

        private void likebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + likebuttonX.Text;
        }

        private void ltbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + ltbuttonX.Text;
        }

        private void bandebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + bandebuttonX.Text;
        }

        private void andbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + andbuttonX.Text;
        }

        private void stbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + stbuttonX.Text;
        }

        private void sandebuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + sandebuttonX.Text;
        }

        private void orbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + orbuttonX.Text;
        }

        private void whbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + whbuttonX.Text;
        }

        private void cbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + cbuttonX.Text;
        }

        private void ykhbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + ykhbuttonX.Text;
        }

        private void notbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + notbuttonX.Text;
        }

        private void isbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Text = sqlrichTextBox.Text + " " + isbuttonX.Text;
        }

        private void clearbuttonX_Click(object sender, EventArgs e)
        {
            sqlrichTextBox.Clear();
        }

        private void guvbuttonX_Click(object sender, EventArgs e)
        {
            
            fieldvaluelistBox.Items.Clear();

            //int Conunt = pFeatureLayer.FeatureClass.FeatureCount(null);
            //MessageBox.Show(Conunt.ToString(),"提示！");
            
            string strfieldName = fieldlistBox.SelectedItem.ToString();
            m_strFiledName = strfieldName;
            strfieldName = strfieldName.Substring(0, strfieldName.IndexOf('【'));

            bool blnStr = false;
            if (m_FeatureClass.Fields.get_Field(m_FeatureClass.Fields.FindField(strfieldName)).Type == esriFieldType.esriFieldTypeString)
            {
                blnStr = true;
            }

            if (m_FeatureClass == null || this.fieldlistBox.SelectedItems.Count == 0) return;

            string sFieldName = strfieldName;        //获取选中项的字符串
            //得到要素集合

            this.fieldvaluelistBox.Items.Clear();

            try
            {
                ///ZQ 2011 1203 改成IQueryDef获唯一值加快效率
                IDataset pDataset = m_FeatureClass as IDataset;
                IFeatureWorkspace pFeatureWorkspace = pDataset.Workspace as IFeatureWorkspace;
                IQueryDef pQueryDef = pFeatureWorkspace.CreateQueryDef();
                pQueryDef.Tables =pDataset.Name.ToString();
                pQueryDef.SubFields = "distinct("+sFieldName+")";
                pQueryDef.WhereClause = "";
                //IQueryFilter pQueryFilter = new QueryFilterClass();
                //pQueryFilter.WhereClause = "";
                //IFeatureCursor pCursor = pFeatureClass.Search(pQueryFilter, false);
                //System.Collections.IEnumerator enumerator;
                //IDataStatistics DS = new DataStatisticsClass();
                //DS.Field = sFieldName;//设置唯一值字段
                //DS.Cursor = pCursor as ICursor;//数据来源
                //enumerator = DS.UniqueValues;//得到唯一值
                //enumerator.Reset();//从新指向第一个值
                ICursor pCursor = pQueryDef.Evaluate();
                IRow pRow = pCursor.NextRow();
                while (pRow!=null)//遍历唯一值enumerator.Current.ToString();
                {
                    string strTemp = pRow.get_Value(0).ToString();
                    if (blnStr)
                    {
                        fieldvaluelistBox.Items.Add("'" + strTemp + "'");
                    }
                    else
                    {
                        fieldvaluelistBox.Items.Add(strTemp);
                    }
                    pRow = pCursor.NextRow();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;

                //lstValue.Sorted=true;
                fieldvaluelistBox.Update();
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
            sqlrichTextBox.Text = sqlrichTextBox.Text + fieldvaluelistBox.SelectedItem.ToString();
        }

        private void closebuttonX_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private bool m_blnOk = false;

        private void okbuttonX_Click(object sender, EventArgs e)
        {
             if (sqlrichTextBox.Text == "")
             {
                 this.DialogResult = DialogResult.Cancel;
                 this.Close();
             }
             else
             {
                 if (!m_blnOk)
                 {
                     IWorkspace pWorkspace = null;
                     string strFcname = "";
                     //added by chulili 20111212 改变判断方法（判断查询条件语法是否正确）
                     if (m_FeatureClass != null)
                     {
                         IDataset pDataset = m_FeatureClass as IDataset;
                         if (pDataset != null)
                         {
                             pWorkspace = pDataset.Workspace;
                             strFcname = pDataset.Name;
                         }

                     }
                     try
                     {
                         if (pWorkspace != null)
                         {
                             pWorkspace.ExecuteSQL("select count(*) from " + strFcname + " where " + sqlrichTextBox.Text); //deleted by chulili 20111220删除where中的1=0,因为它会让判断不准确
                         }
                         pWorkspace = null;
                         m_blnOk = true;
                         this.DialogResult = DialogResult.OK;

                     }
                     catch
                     {
                         MessageBox.Show("SQL语句不符规则，请重新输入.");
                         pWorkspace = null;
                         return;
                     }
                 }
                 else
                 {
                     this.DialogResult = DialogResult.OK;
                 }

                
                 this.Close();
             }

        }

        private void verifybuttonX_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass = null;
            IWorkspace pWorkspace = null;
            string strFcname="";
            //added by chulili 20111212 改变判断方法（判断查询条件语法是否正确）
         
                if (m_FeatureClass != null)
                {
                    pFeatureClass =m_FeatureClass;
                    IDataset pDataset = pFeatureClass as IDataset;
                    if (pDataset != null)
                    {
                        pWorkspace = pDataset.Workspace;
                        strFcname=pDataset.Name;
                    }
                }
                     //IQueryFilter pQueryFilter;
            //pQueryFilter = new QueryFilterClass();
            //pQueryFilter.WhereClause = sqlrichTextBox.Text + "and " + sqlrichTextBox.Text;
            try
            {
                if (pWorkspace != null)
                {
                    pWorkspace.ExecuteSQL("select count(*) from " + strFcname + " where " + sqlrichTextBox.Text); //deleted by chulili 20111220删除where中的1=0,因为它会让判断不准确
                }
                //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                //pFeatureCursor = null;
                MessageBox.Show("验证通过");
                m_blnOk = true;
            }
            catch(Exception err)
            {
                MessageBox.Show("SQL语句不符规则，请重新输入.");
                pWorkspace = null;
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
