using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;


namespace GeoDataCenterFunLib
{
    public partial class frmAddNewFile : DevComponents.DotNetBar.Office2007Form
    {
        public frmAddNewFile()
        {
            InitializeComponent();
        }

        public string strName;
        public string strDescri;
        public string strPath;
        public string strFeauture;

        //选择路径
        private void btnServer_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP数据|*.shp|MDB数据|*.mdb";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                textBoxPath.Text = dlg.FileName;
                if (dlg.FileName.Substring(dlg.FileName.LastIndexOf(".") + 1).ToLower() == "mdb")
                {
                    comboBoxFeature.Enabled = true;
                    IWorkspaceFactory wf = new AccessWorkspaceFactory();
                    IFeatureWorkspace pFeatureWorkspaceMDB = wf.OpenFromFile(@dlg.FileName, 0) as IFeatureWorkspace;
                    IWorkspace pWorkspaceMDB = pFeatureWorkspaceMDB as IWorkspace;
                    List<string> list = Getfeatureclass(pWorkspaceMDB);
                    for (int ii = 0; ii < list.Count; ii++)
                    {
                        comboBoxFeature.Items.Add(list[ii]);
                    }
                }
                Cursor = Cursors.Default;
            }
            if (comboBoxFeature.Items.Count > 0)
            {
                comboBoxFeature.SelectedIndex = 0;
            }
        }

        //获得mdb中要素类
        public List<string> Getfeatureclass(IWorkspace pWorkspaceMDB)
        {
            List<string> list = new List<string>();
            IEnumDataset enumDataset = pWorkspaceMDB.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
            IDataset dataset = enumDataset.Next();
            while (dataset != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    //IFeatureClass pFeatureClass = dataset as IFeatureClass;
                    //IDataset pDataset = pFeatureClass as IDataset;;
                    list.Add(dataset.Name);
                    dataset = enumDataset.Next();
                }
            }
            enumDataset = pWorkspaceMDB.get_Datasets(esriDatasetType.esriDTFeatureDataset) as IEnumDataset;
            dataset = enumDataset.Next();
            while (dataset != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    IEnumDataset pEnumDataset = dataset.Subsets;
                    IDataset dataset2 = pEnumDataset.Next();
                    while (dataset2 != null)
                    {
                        list.Add(dataset2.Name);
                        dataset2 = pEnumDataset.Next();
                    }

                }
                dataset = enumDataset.Next();
            }
            return list;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            textBoxDescri.Text = textBoxName.Text;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            strName = textBoxName.Text;
            strDescri = textBoxDescri.Text;
            strPath = textBoxPath.Text;
            strFeauture = comboBoxFeature.Text;
            if (strName == "")
            {
                MessageBox.Show("图层名不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (strDescri == "")
            {
                MessageBox.Show("图层描述不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (strPath == "")
            {
                MessageBox.Show("请选择图层路径!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Hide();
            this.Dispose(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }


    }

}