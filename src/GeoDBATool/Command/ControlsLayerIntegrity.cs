using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDBATool
{
    public class ControlsLayerIntegrity : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsLayerIntegrity()
        {
            base._Name = "GeoDBATool.ControlsLayerIntegrity";
            base._Caption = "图层完整性";
            base._Tooltip = "图层完整性";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "图层完整性";

        }

        public override bool Enabled
        {
            get
            {
              
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            FrmCheck newfrm = new FrmCheck();
            newfrm.FrmTile = base.Caption;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
            }
            newfrm.Show();
            // 屏蔽之前的检测代码 ygc 2012-8-30
            //FolderBrowserDialog pdlg = new FolderBrowserDialog();
            //pdlg.Description = "选择SHP文件路径：";
            //DialogResult pRes= pdlg.ShowDialog();
            //if (pRes != DialogResult.OK)
            //{
            //    pdlg = null;
            //    return;
            //}
            //IWorkspaceFactory pWorkspaceFactory = null;
            //IWorkspace pWorkspace = null;
            //ESRI.ArcGIS.esriSystem.IPropertySet pPropertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            ////获取初始化系统配置的表格模板
            //string dataPath = Application.StartupPath + "\\..\\Template\\CheckShpTemplate.mdb";
            //pPropertySet.SetProperty("DATABASE", dataPath);
            //pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            //try
            //{
            //    pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
            //}
            //catch
            //{ }
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspaceFactory);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pPropertySet);

            //if (pWorkspace == null)
            //{
            //    MessageBox.Show("用于匹配检查的配置文件错误!");
            //    return;
            //}

            //List<string> pList =ModDBOperator.GetFeatureClassListOfWorkspace(pWorkspace);
            //string strShpPath = pdlg.SelectedPath;

            //IWorkspace ShpWorkspace = null;
            //IWorkspaceFactory pShpWorkSpaceFactory = new ShapefileWorkspaceFactoryClass();
            //ShpWorkspace = pShpWorkSpaceFactory.OpenFromFile(strShpPath, 0);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pShpWorkSpaceFactory);
            //IEnumDataset pEnumDataset = ShpWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            //pEnumDataset.Reset();
            //IDataset pDataset = pEnumDataset.Next();
            //List<string> pMoreList = new List<string>();
            //while (pDataset != null)
            //{
            //    if (pList.Contains(pDataset.Name))
            //    {
            //        pList.Remove(pDataset.Name);
            //    }
            //    else
            //    {
            //        pMoreList.Add(pDataset.Name);
            //    }
            //    pDataset = pEnumDataset.Next();
            //}
            //string strResult = "";
            //if (pList.Count > 0)
            //{
            //    strResult = "缺少以下图层：\n";
            //}
            //for (int i = 0; i < pList.Count; i++)
            //{
            //    strResult = strResult + pList[i] + "\n";
            //}
            //if (pMoreList.Count > 0)
            //{
            //    strResult =strResult+"多余以下图层：\n";
            //}
            //for (int i = 0; i < pMoreList.Count; i++)
            //{
            //    strResult = strResult + pMoreList[i] + "\n";
            //}
            //pList.Clear();
            //pMoreList.Clear();
            //pList = null;
            //pMoreList = null;
            //if (strResult == "")
            //{
            //    strResult = "通过检查!";
            //}
            //MessageBox.Show(strResult);
            //end ygc
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
