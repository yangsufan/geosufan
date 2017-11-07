using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Error;

namespace GeoEdit
{
    public partial class FrmStartEdit : DevComponents.DotNetBar.Office2007Form
    {
        private Dictionary<IWorkspace, List<IFeatureLayer>> pAllEditInfo = new Dictionary<IWorkspace, List<IFeatureLayer>>();
        public Dictionary<IWorkspace, List<IFeatureLayer>> AllEditInfo
        {
            get
            {
                return pAllEditInfo;
            }
            set
            {
                pAllEditInfo = value;
            }
        }
        private IWorkspace pSelectWorkspace;
        public IWorkspace SelectWorkspace
        {
            get
            {
                return pSelectWorkspace;
            }
            set
            {
                pSelectWorkspace = value;
            }
        }
        public FrmStartEdit()
        {
            InitializeComponent();
        }

        private void FrmStartEdit_Load(object sender, EventArgs e)
        {
            //创建listview的标题
            listViewEx2.Items.Clear();
            listViewEx2.Columns.Clear();
            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Source";
            columnHeader1.Width = listViewEx2.Width * 2 / 3;
            ColumnHeader columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "Type";
            columnHeader2.Width = listViewEx2.Width / 3;
            listViewEx2.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });

            //遍历字典，获得Workspace的信息和Ifeaturelayer的信息用于放在界面上
            IWorkspace pWorkSpace = null;
            foreach (KeyValuePair<IWorkspace, List<IFeatureLayer>> item in pAllEditInfo)
            {
                pWorkSpace = item.Key;
                string filePath = pWorkSpace.PathName.ToString();
                ListViewItem newItem = new ListViewItem(new string[] { filePath, "Personal Geodatabase" });
                listViewEx2.Items.Add(newItem);
            }
            if (listViewEx2.Items.Count != 0)
            {
                ListViewItem pItem = listViewEx2.Items[0];
                pItem.Selected = true;
            }
            listViewEx2.FullRowSelect = true;
            listViewEx2.Refresh();
        }


        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;


            int c = listViewEx1.SelectedItems.Count;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void listViewEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEx2.SelectedItems.Count == 0)
            {
                return;
            }
            if (listViewEx2.SelectedItems.Count > 1)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", "请只选择一行");
                return;
            }
            listViewEx1.Items.Clear();
            ListViewItem pListItem = listViewEx2.SelectedItems[0];
            string path = pListItem.SubItems[0].Text;
            string type = pListItem.SubItems[1].Text;
            string lastPath = path.Substring(path.LastIndexOf('.') + 1).ToLower();
            IWorkspace mWorkSpace = null;
            List<IFeatureLayer> mFeatureLayers = new List<IFeatureLayer>();
            if (type == "Personal Geodatabase")
            {
                try
                {
                    if (lastPath == "mdb")
                    {
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        IPropertySet pPropSet = new PropertySetClass();
                        pPropSet.SetProperty("DATABASE", path);
                        mWorkSpace = pAccessFact.Open(pPropSet, 0);
                    }
                    else if (lastPath == "gdb")
                    {
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        IPropertySet pPropSet = new PropertySetClass();
                        pPropSet.SetProperty("DATABASE", path);
                        mWorkSpace = pFileGDBFact.Open(pPropSet, 0);
                    }

                    if (pAllEditInfo.ContainsKey(mWorkSpace))
                    {
                        mFeatureLayers = pAllEditInfo[mWorkSpace];
                        foreach (IFeatureLayer i in mFeatureLayers)
                        {
                            listViewEx1.Items.Add(i.Name.ToString());
                        }
                        listViewEx1.Refresh();
                    }
                    pSelectWorkspace = mWorkSpace;
                }
                catch (System.Exception ex)
                {
                    //******************************************
                    //guozheng added System Exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(ex);
                    //******************************************
                }
            }
        }
    }
}