using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using GeoDataCenterFunLib;
using SysCommon;

namespace GeoDataCenterFrame
{
    public partial class frmDataUpload : DevComponents.DotNetBar.Office2007Form
    {
        public frmDataUpload()
        {
            InitializeComponent();
        }
        OpenFileDialog OpenFile;
        int i = 0;

        private void ImportFeatureClassToNewWorkSpace(string filename,string outfilename)
        {

            //try
            //{
                string ImportShapeFileName = filename;
                if (ImportShapeFileName == "") { return; }
                string ImportFileShortName = System.IO.Path.GetFileNameWithoutExtension(ImportShapeFileName);
                string ExportFileShortName = System.IO.Path.GetFileNameWithoutExtension(outfilename);
                string ImportFilePath = System.IO.Path.GetDirectoryName(ImportShapeFileName);
                IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                IWorkspace pWorkspace = Pwf.OpenFromFile(@comboBox1.Text, 0);
                
                //IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
                //IWorkspace pWorkspace = Pwf.OpenFromFile(@"E:\test\x.mdb", 0);

                IWorkspaceName pInWorkspaceName;
                IFeatureDatasetName pOutFeatureDSName;
                IFeatureClassName pInFeatureClassName;
                IDatasetName pInDatasetName;
                IFeatureClassName pOutFeatureClassName;
                IDatasetName pOutDatasetName;
                long iCounter;
                IFields pOutFields, pInFields;
                IFieldChecker pFieldChecker;
                IField pGeoField;
                IGeometryDef pOutGeometryDef;
                IGeometryDefEdit pOutGeometryDefEdit;
                IName pName;
                IFeatureClass pInFeatureClass;
                IFeatureDataConverter pShpToClsConverter;
                IEnumFieldError pEnumFieldError = null;

                //得到一个输入SHP文件的工作空间，
                pInWorkspaceName = new WorkspaceNameClass();
                pInWorkspaceName.PathName = ImportFilePath;
                pInWorkspaceName.WorkspaceFactoryProgID = "esriCore.ShapefileWorkspaceFactory.1";
                //创建一个新的要素类名称，目的是为了以来PNAME接口的OPEN方法打开SHP文件
                pInFeatureClassName = new FeatureClassNameClass();
                pInDatasetName = (IDatasetName)pInFeatureClassName;
                pInDatasetName.Name = ImportFileShortName;
                pInDatasetName.WorkspaceName = pInWorkspaceName;
                //打开一个SHP文件，将要读取它的字段集合
                pName = (IName)pInFeatureClassName;
                pInFeatureClass = (IFeatureClass)pName.Open();
                //通过FIELDCHECKER检查字段的合法性，为输入要素类获得字段集合
                pInFields = pInFeatureClass.Fields;
                pFieldChecker = new FieldChecker();
                pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);
                //通过循环查找几何字段
                pGeoField = null;
                for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
                {
                    if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pGeoField = pOutFields.get_Field((int)iCounter);
                        break;
                    }
                }
                //得到几何字段的几何定义
                pOutGeometryDef = pGeoField.GeometryDef;
                //设置几何字段的空间参考和网格
                pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
                pOutGeometryDefEdit.GridCount_2 = 1;
                pOutGeometryDefEdit.set_GridSize(0, 1500000);

                //创建一个新的要素类名称作为可用的参数
                pOutFeatureClassName = new FeatureClassNameClass();
                pOutDatasetName = (IDatasetName)pOutFeatureClassName;
                pOutDatasetName.Name = ExportFileShortName;
                //创建一个新的数据集名称作为可用的参数
                pOutFeatureDSName = (IFeatureDatasetName)new FeatureDatasetName();
                //如果参数的值是NULL，说明要创建独立要素类
                //创建一个不存在的要素集合pFDN，通过它将IFeatureClassName和工作空间连接起来，而ConvertFeatureClass函数并不使用该变量作为参数，
                IFeatureDatasetName pFDN = new FeatureDatasetNameClass();
                IDatasetName pDN = (IDatasetName)pFDN;
                IDataset pDS = (IDataset)pWorkspace;
                pDN.WorkspaceName = (IWorkspaceName)pDS.FullName;
                pOutFeatureClassName.FeatureDatasetName = (IDatasetName)pFDN;
                //将pOutFeatureDSName设置为Null，将它做为参数给ConvertFeatureClass函数，因为IFeatureClassName本身已经和工作空间关联了，生成的
                //要素类在工作空间的根目录下，即独立要素类
                pOutFeatureDSName = null;


                //开始导入
                pShpToClsConverter = new FeatureDataConverterClass();
                pShpToClsConverter.ConvertFeatureClass(pInFeatureClassName, null, pOutFeatureDSName, pOutFeatureClassName, null, pOutFields, "", 1000, 0);
                MessageBox.Show("导入成功", "系统提示");
            //}
            //catch
            //{
            //    MessageBox.Show("导入失败", "系统提示");
            //}
        }

        private void checkboxPf_CheckedChanged(object sender, EventArgs e)
        {
            textBox.Enabled = true;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.SubItems[1].Text = textBox.Text + item.Text;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFile = new OpenFileDialog();
            OpenFile.Filter = "SHP文件|*.shp";
            //打开SHP文件
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string name = OpenFile.FileName.Substring(OpenFile.FileName.LastIndexOf('\\')+1,OpenFile.FileName.Length-OpenFile.FileName.LastIndexOf('\\')-1);
                  listView.Items.Add(name);
                  if (textBox.Text != "")
                      listView.Items[name].SubItems.Add(textBox.Text + name + "");
                  else
                      listView.Items[i].SubItems.Add(name + "");
                  i++;

            }
            
             
            
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    listView.Items.Remove(item);
                    i--;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            i = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            bool _check=false;
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    _check = true;
                    MessageBox.Show("正在导入" + item.Text);
                    ImportFeatureClassToNewWorkSpace(OpenFile.FileName,item.SubItems[1].Text);
                }
            }
            if (_check ==false)
                MessageBox.Show("请选择要上传的文件！");
        }
    }
}