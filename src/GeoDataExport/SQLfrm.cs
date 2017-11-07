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
namespace GeoDataExport
{
    public partial class SQLfrm :DevComponents.DotNetBar.Office2007Form
    {
        public SQLfrm(DataGridViewRow datagridviewrow)
        {
            InitializeComponent();
           // pmapControl = pcMapcontrol;
          // pmap = pcMapcontrol.Map;
           // datagridview=pdatagridview;
            //pFeatureLayer = pfeaturelayer;
            pDatagridviewRow = datagridviewrow;
        }
        //public IMapControl2 pmapControl;
       // public IMap pmap;
       // public DataGridView datagridview;
        //public IFeatureLayer pFeatureLayer;
        public DataGridViewRow pDatagridviewRow;
        /// <summary>
        /// ZQ  20111118 add  记录上一次双击选中的字段名称
        /// </summary>
        private string m_strFiledName = "";
        //ygc 20130326 记录选择字段名称
        private string m_FieldName;
        private void SQLfrm_Load(object sender, EventArgs e)
        {
            
            //ILayer pLayer;
          //  IFeatureLayer pFeatureLayer;
          //  pFeatureLayer = new FeatureLayerClass();
           // frmExport frmmain = new frmExport(pmapControl);
            //int index = frmmain.cellclickindex;
            //string strlayer = frmmain.dataGridLayers.Rows[1].Cells[2].Value.ToString();
           // IFeatureLayer  = (IMap)datagridview.Tag;
           /* for (int i = 0; i < datagridview.RowCount-1; i++)
            {
                pLayer = (ILayer)datagridview.Rows[i].Tag;
                if(pLayer.Name=="dd")
                {
                    pFeatureLayer = (IFeatureLayer)pLayer;
                }
            }*/
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pDatagridviewRow.Tag;
            for (int j = 0; j < pFeatureLayer.FeatureClass.Fields.FieldCount; j++)
            {
                ///Zq  20111027  modify 
                fieldlistBox.Items.Add(pFeatureLayer.FeatureClass.Fields.get_Field(j).Name
                    + "【" +SysCommon.ModField.GetChineseNameOfField( pFeatureLayer.FeatureClass.Fields.get_Field(j).Name )+ "】");
            }
            if (fieldlistBox.SelectedItem == null)
            {
                guvbuttonX.Enabled = false;
            }

            if (pDatagridviewRow.Cells["colWhere"].Value == null) return;
            this.sqlrichTextBox.Text = pDatagridviewRow.Cells["colWhere"].Value.ToString();
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
           /* ILayer pLayer;
            IFeatureLayer pFeatureLayer;
            pFeatureLayer = new FeatureLayerClass();
            for (int i = 0; i < datagridview.RowCount - 1; i++)
            {
                pLayer = (ILayer)datagridview.Rows[i].Tag;
                if (pLayer.Name == "dd")
                {
                    pFeatureLayer = (IFeatureLayer)pLayer;
                }
            }*/
            //IFeatureLayer pFeatureLayer = (IFeatureLayer)pDatagridviewRow.Tag;
            //string strfieldName = fieldlistBox.SelectedItem.ToString();
            //strfieldName = strfieldName.Substring(0, strfieldName.IndexOf('【'));

            //bool blnStr = false;
            //if (pFeatureLayer.FeatureClass.Fields.get_Field(j).Type == esriFieldType.esriFieldTypeString)
            //{
            //}

            //int j = pFeatureLayer.FeatureClass.Fields.FindField(strfieldName);
            //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
            //IFeature pFeature = pFeatureCursor.NextFeature();
            //while (pFeature != null)
            //{
            //    if (blnStr)
            //    {
            //        fieldvaluelistBox.Items.Add(pFeature.get_Value(j).ToString());
            //    }
            //    else
            //    {
            //        fieldvaluelistBox.Items.Add(pFeature.get_Value(j).ToString());
            //    }
                
            //    pFeature= pFeatureCursor.NextFeature();
            //}

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pDatagridviewRow.Tag;
            //int Conunt = pFeatureLayer.FeatureClass.FeatureCount(null);
            //MessageBox.Show(Conunt.ToString(),"提示！");
            
            string strfieldName = fieldlistBox.SelectedItem.ToString();
            strfieldName = strfieldName.Substring(0, strfieldName.IndexOf('【'));
            m_FieldName = strfieldName;
            bool blnStr = false;
            if (pFeatureLayer.FeatureClass.Fields.get_Field(pFeatureLayer.FeatureClass.Fields.FindField(strfieldName)).Type == esriFieldType.esriFieldTypeString)
            {
                blnStr = true;
            }

            if (pFeatureLayer == null || this.fieldlistBox.SelectedItems.Count == 0) return;
            if (pFeatureLayer.FeatureClass == null) return;

            string sFieldName = strfieldName;        //获取选中项的字符串

            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;         //得到要素集合
            if (pFeatureClass == null) return;

            this.fieldvaluelistBox.Items.Clear();

            try
            {
                List<string> listFieldValue = SysCommon.ModXZQ.GetListChineseName(Plugin .ModuleCommon .TmpWorkSpace , strfieldName);
                if (listFieldValue != null || listFieldValue.Count != 0)
                {
                    for (int t = 0; t < listFieldValue.Count; t++)
                    {
                        if (blnStr)
                        {
                            fieldvaluelistBox.Items.Add("'" + listFieldValue[t] + "'");
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
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "";
                    IFeatureCursor pCursor = pFeatureClass.Search(pQueryFilter, false);
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
                string fieldCode = SysCommon.ModXZQ.GetCode(Plugin .ModuleCommon .TmpWorkSpace, m_FieldName, tempCode);
                if (isExit)
                {
                    sqlrichTextBox.Text = sqlrichTextBox.Text + "'" + fieldCode + "'";
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
             if (sqlrichTextBox.Text == "")
             {
                 pDatagridviewRow.Cells["colWhere"].Value = "";
                 this.Close();
             }
             else
             {
                 if (!m_blnOk)
                 {
                     IFeatureLayer pFeatureLayer = (IFeatureLayer)pDatagridviewRow.Tag;
                     IQueryFilter pQueryFilter;
                     pQueryFilter = new QueryFilterClass();
                     pQueryFilter.WhereClause = sqlrichTextBox.Text;
                     try
                     {
                         IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                         System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                         pFeatureCursor = null;
                     }
                     catch
                     {
                         MessageBox.Show("SQL语句不符规则，请重新输入.");
                         return;
                     }
                 }

                 pDatagridviewRow.Cells["colWhere"].Value = sqlrichTextBox.Text;
                 this.Close();
             }
        }

        private void verifybuttonX_Click(object sender, EventArgs e)
        {

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pDatagridviewRow.Tag;
            IQueryFilter pQueryFilter;
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = sqlrichTextBox.Text;
            try
            {
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pFeatureCursor = null;
                MessageBox.Show("验证通过");
                m_blnOk = true;
            }
            catch
            {
                MessageBox.Show("SQL语句不符规则，请重新输入.");
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
