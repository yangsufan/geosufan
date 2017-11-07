using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


using ESRI.ArcGIS.Geodatabase;

using ESRI.ArcGIS.DataSourcesFile;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using GeoDataCenterFunLib;




namespace GeoDataEdit
{
    public partial class frmBatchFillField : DevComponents.DotNetBar.Office2007Form
    {
        public frmBatchFillField()
        {
            InitializeComponent();
        }

        private string shpPath = "";
        private IWorkspace pWorkspace = null;
        private IFeatureClass pFeaClass=null;




        private void buttonX1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "SHP数据|*.shp";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFileDialog1.FileName;
 
            }
            pFeaClass = openShp();
            getFields(pFeaClass);
          

        }
        private IFeatureClass openShp()
        {
            if (txtPath.Text == "") 
                return null;
            string wsPath=Path.GetDirectoryName(txtPath.Text);
            string shpName=Path.GetFileName(txtPath.Text);
            string exten=Path.GetExtension(txtPath.Text);
            pWorkspace = new ShapefileWorkspaceFactoryClass().OpenFromFile(wsPath,0);
            IFeatureClass pFeatureClass=(pWorkspace as IFeatureWorkspace).OpenFeatureClass(shpName.Remove(shpName.Length-4));
            return pFeatureClass;
            
        }
        private void getFields(IFeatureClass pFC)
        {
            if(pFC==null)
                return;
            IFields pFields = pFC.Fields;
            for (int i = 0; i < pFields.FieldCount; i++)
            {

                if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeOID || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || pFields.get_Field(i).Editable == false)
                    continue;
                cBoxField.Items.Add(pFields.get_Field(i).Name);
            }

 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cBoxField.Text == "" || txtValue.Text == "")
                return;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在批量赋值,请稍后");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = false;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            Application.DoEvents();
            bool result = false;
            try
            {
                UpdateFieldValues(pFeaClass, cBoxField.Text, txtValue.Text);
                vProgress.Close();
                result = true;
                if (result == true)
                {
                    MessageBox.Show("批量赋值成功完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
               
            }
            catch (Exception ex)
            {
                vProgress.Close();
                
                MessageBox.Show("请按正确格式输入属性值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
          
                
            
            
            Application.DoEvents();
            
        }
        // The following function uses an update cursor to change the value of a particular
        // field for a set of features in a feature class.
        private void UpdateFieldValues(IFeatureClass featureClass,string fieldname,object value)
        {  
            // Create a new query filter. 
            IQueryFilter queryFilter = new QueryFilterClass();  
            queryFilter.WhereClause = ""; 
            queryFilter.SubFields = fieldname;
           // Get an update cursor constrained by the query filter.  
            IFeatureCursor featureCursor = featureClass.Update(queryFilter, false);
              
            IFields fields = featureCursor.Fields;
            int fieldIndex = fields.FindField(fieldname);
            IField pf = fields.get_Field(fieldIndex);

           
            IFeature feature = null;  
            while ((feature = featureCursor.NextFeature()) != null)  
            {
                feature.set_Value(fieldIndex, value);    
                featureCursor.UpdateFeature(feature);    
                
            }
            Marshal.ReleaseComObject(featureCursor);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pFeaClass == null || cBoxField.Text=="")
                return;
            IFeature pFeature = pFeaClass.GetFeature(0);
            int fdindex = pFeaClass.Fields.FindField(cBoxField.Text);
            txtValue.Text = pFeature.get_Value(fdindex).ToString();
            if (pFeaClass.Fields.get_Field(fdindex).Type == esriFieldType.esriFieldTypeDate)
            {
                txtValue.Text = DateTime.Today.ToShortDateString();
            }

        }





    }
}