using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDBATool
{
    public class ControlsFieldsCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsFieldsCheck()
        {
            base._Name = "GeoDBATool.ControlsFieldsCheck";
            base._Caption = "属性表结构";
            base._Tooltip = "属性表结构";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "属性表结构";

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
            FrmCheck frmCheckFields = new FrmCheck();
            frmCheckFields.FrmTile = base.Caption;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
            }
            frmCheckFields.Show();
            // 屏蔽之前的代码 ygc 2012-8-30
            //FolderBrowserDialog pdlg = new FolderBrowserDialog();
            //pdlg.Description = "选择SHP文件路径：";
            //DialogResult pRes = pdlg.ShowDialog();
            //if (pRes != DialogResult.OK)
            //{
            //    pdlg = null;
            //    return;
            //}
        //    IWorkspaceFactory pWorkspaceFactory = null;
        //    IWorkspace pWorkspace = null;
        //    ESRI.ArcGIS.esriSystem.IPropertySet pPropertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
        //    //获取初始化系统配置的表格模板
        //    string dataPath = Application.StartupPath + "\\..\\Template\\CheckShpTemplate.mdb";
        //    pPropertySet.SetProperty("DATABASE", dataPath);
        //    pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
        //    try
        //    {
        //        pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
        //    }
        //    catch
        //    { }
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspaceFactory);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pPropertySet);

        //    if (pWorkspace == null)
        //    {
        //        MessageBox.Show("用于匹配检查的配置文件错误!");
        //        return;
        //    }

        //    Dictionary <string, List<string>> pFieldDic =ModDBOperator.GetFieldsListOfWorkSpace (pWorkspace);
        //    string strShpPath = pdlg.SelectedPath;

        //    IWorkspace ShpWorkspace = null;
        //    IWorkspaceFactory pShpWorkSpaceFactory = new ShapefileWorkspaceFactoryClass();
        //    ShpWorkspace = pShpWorkSpaceFactory.OpenFromFile(strShpPath, 0);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pShpWorkSpaceFactory);
        //    IEnumDataset pEnumDataset = ShpWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
        //    pEnumDataset.Reset();
        //    IDataset pDataset = pEnumDataset.Next();
        //    Dictionary<string, List<string>> pMoreDic = new Dictionary<string, List<string>>();
        //    Dictionary<string, List<string>> pLessDic = new Dictionary<string, List<string>>();
        //    while (pDataset != null)
        //    {
        //        if (pFieldDic.ContainsKey(pDataset.Name))
        //        {
        //            List<string> pTmpList = pFieldDic[pDataset.Name];
        //            List<string> pMoreList = null;
        //            IFeatureClass pFeatureClass = pDataset as IFeatureClass;
        //            IFields pFields = pFeatureClass.Fields;
        //            for (int f = 0; f < pFields.FieldCount; f++)
        //            {
        //                IField pField = pFields.get_Field(f);
        //                if (pField.Type != esriFieldType.esriFieldTypeOID && pField.Type != esriFieldType.esriFieldTypeGeometry)
        //                {
        //                    if (pTmpList.Contains(pField.Name))
        //                    {
        //                        pTmpList.Remove(pField.Name);
        //                    }
        //                    else
        //                    {
        //                        if (pMoreList == null)
        //                        {
        //                            pMoreList = new List<string>();
        //                        }
        //                        pMoreList.Add(pField.Name);
        //                    }
        //                }
        //            }
        //            if (pTmpList.Count > 1)
        //            {
        //                pLessDic.Add(pDataset.Name, pTmpList);
        //            }
        //            if (pMoreList != null)
        //            {
        //                pMoreDic.Add(pDataset.Name, pMoreList);
        //            }
                        
        //        }
        //        pDataset = pEnumDataset.Next();
        //    }
        //    string strResult = "";
        //    if (pLessDic.Count >0)
        //    {
        //        strResult = "缺少以下属性列：\n";
        //    }
        //    foreach (string strkey in pLessDic.Keys)
        //    {
        //        strResult = strResult + strkey + "图层：";
        //        List<string> pTmpList0 = pLessDic[strkey];
        //        for (int l = 0; l < pTmpList0.Count; l++)
        //        {
        //            strResult = strResult + pTmpList0[l] + ",";
        //        }
        //        pTmpList0.Clear();
        //        strResult = strResult + "\n";

        //    }
        //    if (pMoreDic.Count > 0)
        //    {
        //        strResult = strResult + "多余以下属性列：\n";
        //    }
        //    foreach (string strkey in pMoreDic.Keys)
        //    {
        //        strResult = strResult + strkey + "图层：";
        //        List<string> pTmpList0 = pMoreDic[strkey];
        //        for (int l = 0; l < pTmpList0.Count; l++)
        //        {
        //            strResult = strResult + pTmpList0[l] + ",";
        //        }
        //        pTmpList0.Clear();
        //        strResult = strResult + "\n";

        //    }
        //    pLessDic.Clear();
        //    pMoreDic.Clear();
        //    if (strResult == "")
        //    {
        //        strResult = "通过检查!";
        //    }
        //    MessageBox.Show(strResult);
            //end ygc
        }
            
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
