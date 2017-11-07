using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.06.01
    /// 说明：标志物地名地址查询窗体
    /// </summary>
    public partial class FrmAddressQuery : DevComponents.DotNetBar.Office2007Form
    {
        private IMap pMap = null;
        private IFeatureLayer pFLayer = null;
        public FrmAddressQuery(IMap inMap)
        {
            InitializeComponent();
            pMap = inMap;
            pFLayer = getDMDZLayer(inMap, "标志物地名");
            setColumnOfLstVR(pFLayer);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        //得到地名地址查询图层
        private IFeatureLayer getDMDZLayer(IMap inMap,string strLayerName)
        {
            IFeatureLayer rstFeatureLayer = null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j).Name != strLayerName)
                            continue;
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        rstFeatureLayer = pFLayer;
                        if (rstFeatureLayer != null)
                            break;

                    }
                }
                else//不是grouplayer
                {
                    if (pLayer.Name != strLayerName)
                        continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    rstFeatureLayer = pFLayer;
                    if (rstFeatureLayer != null)
                        break;
                }
            }
            return rstFeatureLayer;
        }
        //从地名地址图层读取字段设置datagridviewR的字段
        private void setColumnOfLstVR(IFeatureLayer inFLayer)
        {
            IFeatureClass pFClass=inFLayer.FeatureClass;
            IFields pFields=pFClass.Fields;
           //DataGridViewColumnCollection pDGVCC =
           //     new DataGridViewColumnCollection(dataGridViewR);
           // pDGVCC.Clear();
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry
                    || pFields.get_Field(i).Name == "DM")
                    continue;
                IField pField = pFields.get_Field(i);
                DataGridViewTextBoxColumn pDGVC = new DataGridViewTextBoxColumn();
                if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeOID)
                {
                    
                    pDGVC.Name = "OBJECTID";
                    pDGVC.Tag = i;
                    pDGVC.ValueType = typeof(string);
                    pDGVC.HeaderText = pField.AliasName;
                    dataGridViewR.Columns.Add(pDGVC);
                    continue;

                }
              
                pDGVC.Name = pField.Name;
                pDGVC.Tag = i;
                pDGVC.ValueType = typeof(string);
                pDGVC.HeaderText = pField.AliasName;
                dataGridViewR.Columns.Add(pDGVC);
                
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            dataGridViewR.Rows.Clear();
            if (txtDM.Text =="")
            {
                MessageBox.Show("请输入查询信息！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
            IQueryFilter pQFilter = new QueryFilterClass();
            string strWhere = "DM LIKE '%" + txtDM.Text.Trim() + "%'";
            pQFilter.WhereClause = strWhere;
            IFeatureClass pFClass = pFLayer.FeatureClass;
            IFields pFields = pFClass.Fields;
            IFeatureCursor pFCursor = pFClass.Search(pQFilter, false);
             IFeature pFeature = pFCursor.NextFeature();
             int orderNO=0;
             //DataGridViewRow pDGVR; 
            while (pFeature != null)
            {
                //pDGVR = new DataGridViewRow();
                dataGridViewR.Rows.Add();
                //pLVI.Tag = pFeature.OID;
                for (int i = 0,j=0; i < pFields.FieldCount; i++,j++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry
                        || pFields.get_Field(i).Name == "DM")
                    {
                        j--;
                        continue;
                    }

                    dataGridViewR.Rows[orderNO].Cells[j].Value = pFeature.get_Value(i);

                }
                
                pFeature = pFCursor.NextFeature();
                orderNO++;
            }
            dataGridViewR.Refresh();
            
        }

     

        private void dataGridViewR_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewR.SelectedRows.Count == 0)
                return;
            int oid = Convert.ToInt32(dataGridViewR.SelectedRows[0].Cells["OBJECTID"].Value);
            IFeature pFeature = pFLayer.FeatureClass.GetFeature(oid);
            IPoint pPoint = pFeature.ShapeCopy as IPoint;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(pPoint.X - 250, pPoint.Y - 250, pPoint.X + 250, pPoint.Y + 250);
            (pMap as IActiveView).Extent = pEnv;
            (pMap as IActiveView).Refresh();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewR.RowCount == 0)
                return;
            exportExcelApp("标志物地名查询", dataGridViewR);
        }
        //利用流对象导出exel
        private void exportExel(DataGridView inDGV)
        {
            SaveFileDialog pSFD = new SaveFileDialog();
            pSFD.Filter = "Exel文件(*.xls)|*.xls";
            pSFD.FilterIndex = 0;
            pSFD.Title = "导出到Exel文件";
            pSFD.RestoreDirectory = true;
            //pSFD.CreatePrompt = true;
            pSFD.ShowDialog();
            Stream pStream;
            pStream = pSFD.OpenFile();
            StreamWriter pSW=new StreamWriter(pStream,Encoding.GetEncoding(-0));
            string str = "";
            try
            {
                //写字段名
                for (int i = 0; i < inDGV.ColumnCount; i++)
                {
                    if (i > 0)
                        str += "\t";
                    str += inDGV.Columns[i].HeaderText;
                }
                pSW.WriteLine(str);
                //写内容
                for (int j = 0; j < inDGV.RowCount; j++)
                {
                    string str2 = "";
                    for (int k = 0; k < inDGV.ColumnCount; k++)
                    {
                        if (k > 0)
                            str2 += "\t";
                        str2 += inDGV.Rows[j].Cells[k].Value.ToString();

                    }
                    pSW.WriteLine(str2);
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            finally
            {
                pSW.Close();
                pStream.Close();
 
            }
 
        }
        //利用exel应用程序
        private void exportExcelApp(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            //bool fileSaved = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件(*.xls)|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            System.Windows.Forms.Application.DoEvents();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            //写入标题
            for (int i = 0; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = myDGV.Columns[i].HeaderText;
            }
            //写入数值
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                for (int i = 0; i < myDGV.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = myDGV.Rows[r].Cells[i].Value;
                }

            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            //if (Microsoft.Office.Interop.cmbxType.Text != "Notification")
            //{
            //    Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);
            //    rg.NumberFormat = "00000000";
            //}

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    //fileSaved = true;
                }
                catch (Exception ex)
                {
                    //fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                }

            }
            //else
            //{
            //    fileSaved = false;
            //}
            xlApp.Quit();
            GC.Collect();//强行销毁 
            // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL
            MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }


       

     
    }
}
