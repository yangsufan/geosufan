using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public partial class frmStractHistorySet : DevComponents.DotNetBar.Office2007Form
    {
        private IMap Map;

        private bool m_Sel;                                //是否过滤选择要素

        public frmStractHistorySet(IMap map, ArrayList arrayListFromDate, string currentDate, bool bSel)
        {
            InitializeComponent();

            for (int i = 0; i < arrayListFromDate.Count; i++)
            {
                ListViewItem aItem = listViewEx.Items.Add(arrayListFromDate[i].ToString());
                aItem.ToolTipText = arrayListFromDate[i].ToString();
                if (arrayListFromDate[i].ToString() == currentDate)
                {
                    aItem.Checked = true;
                }
            }

            Map = map;
            m_Sel = bSel;

            object[] TagDBType = new object[] { "ESRI文件数据库(*.gdb)", "ESRI个人数据库(*.mdb)" };//cyf 20110628 "GDB", "PDB"  "ArcSDE(For Oracle)",
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                this.txtSavePath.Text = FolderBrowser.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtProjectName.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", "请填写输出文件名称 ！");
                return;
            }

            if (!Directory.Exists(txtSavePath.Text.Trim()))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", "请选择保存路径 ！");
                return;
            }

            if (listViewEx.CheckedItems.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", "请选择数据版本信息 ！");
                return;
            }

            string WorkSpacePath = txtSavePath.Text.Trim() + "\\" + txtProjectName.Text.Trim();

            IPropertySet pPropSet = new PropertySetClass();
            IWorkspace pTagetWorkspace = null;
            try
            {
                switch (comBoxType.Tag.ToString().Trim())
                {
                    case "PDB":
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(WorkSpacePath + ".mdb"))
                        {
                            pAccessFact.Create(txtSavePath.Text, txtProjectName.Text, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", WorkSpacePath + ".mdb");
                        pTagetWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        pPropSet = null;
                        break;
                    case "GDB":
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        if (!Directory.Exists(WorkSpacePath + ".gdb"))
                        {
                            pFileGDBFact.Create(txtSavePath.Text, txtProjectName.Text, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", WorkSpacePath + ".gdb");
                        pTagetWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        pPropSet = null;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception er)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                //********************************************************************
            }

            if (pTagetWorkspace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未设置输出库连接或连接失败,请检查!");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;

            Exception err = null;
            bool res = true;
            List<IFeatureLayer> lstFeatLay = new List<IFeatureLayer>();
            //根据历史图层创建输出数据图层结构
            for (int i = 0; i < Map.LayerCount; i++)
            {
                //cyf 20110706 add
                ILayer mLayer = Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComLayer.get_Layer(j) as IFeatureLayer;
                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        lstFeatLay.Add(featLay);
                        if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, featLay.Name))//(featLay.FeatureClass as IDataset).Name
                        {
                            if (!CreateFeatCls(pTagetWorkspace as IFeatureWorkspace, featLay.FeatureClass, featLay.Name, out err))
                            {
                                res = false;
                                featLay = null;
                                pComLayer = null;
                                mLayer = null;
                                break;
                            }
                        }
                    }
                }//end
                else
                {
                    IFeatureLayer featLay = Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    lstFeatLay.Add(featLay);
                    if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, featLay.Name))//(featLay.FeatureClass as IDataset).Name
                    {
                        if (!CreateFeatCls(pTagetWorkspace as IFeatureWorkspace, featLay.FeatureClass, featLay.Name, out err))
                        {
                            res = false;
                            featLay = null;
                            mLayer = null;
                            break;
                        }
                    }
                }
            }

            if (res == false)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建输出库结构失败!");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pTagetWorkspace);
                pTagetWorkspace = null;
                lstFeatLay.Clear();
                lstFeatLay = null;
                try
                {
                    Directory.Delete(txtSavePath.Text + "\\" + txtProjectName.Text + ".gdb", true);
                }
                catch (Exception err2)
                { }
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (ListViewItem aItem in listViewEx.CheckedItems)
            {
                if (sb.Length != 0)
                {
                    sb.Append("or");
                }
                else
                {
                    sb.Append("(");
                }

                sb.Append("(FromDate<='" + aItem.Text + "' and ToDate>'" + aItem.Text + "')");
            }

            sb.Append(")");

            //遍历数据FC进行数据移植
            (pTagetWorkspace as IWorkspaceEdit).StartEditing(false);
            progressBarXLay.Maximum = lstFeatLay.Count;
            progressBarXLay.Minimum = 0;
            progressBarXLay.Value = 0;
            foreach (IFeatureLayer aFeatLay in lstFeatLay)
            {
                labelXMemo.Text = aFeatLay.FeatureClass.AliasName + "...";
                Application.DoEvents();

                StringBuilder newSB = new StringBuilder();
                newSB.Append(sb.ToString());

                if (m_Sel)
                {
                    int fdIndex = aFeatLay.FeatureClass.Fields.FindField("SourceOID");
                    if (fdIndex == -1) continue;
                    IFeatureLayerDefinition featLayDefTemp = aFeatLay as IFeatureLayerDefinition;
                    IEnumIDs pEnumIDs = featLayDefTemp.DefinitionSelectionSet.IDs;
                    int ID = pEnumIDs.Next();
                    StringBuilder sbTemp = new StringBuilder();
                    while (ID != -1)
                    {
                        IFeature pFeat = aFeatLay.FeatureClass.GetFeature(ID);
                        if (sbTemp.Length != 0)
                        {
                            sbTemp.Append(",");
                        }
                        sbTemp.Append(pFeat.get_Value(fdIndex).ToString());
                        ID = pEnumIDs.Next();
                    }
                    newSB.Append(" and SourceOID in (" + sbTemp.ToString() + ")");
                }

                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = newSB.ToString();
                IFeatureCursor featureCursor = aFeatLay.FeatureClass.Search(queryFilter, false);
                progressBarXFeat.Maximum = aFeatLay.FeatureClass.FeatureCount(queryFilter);
                progressBarXFeat.Minimum = 0;
                progressBarXFeat.Value = 0;
                try
                {
                    //cyf 20110706 modify:去掉用户名
                    string pFeaLayerName = aFeatLay.Name.Trim();
                    if (pFeaLayerName.Contains("."))
                    {
                        pFeaLayerName = pFeaLayerName.Substring(pFeaLayerName.IndexOf('.') + 1);
                    }
                    //end
                    IFeatureClass tagetFeatCls = (pTagetWorkspace as IFeatureWorkspace).OpenFeatureClass(pFeaLayerName);//(aFeatLay.FeatureClass as IDataset).Name
                    ModDBOperator.NewFeatures(tagetFeatCls, featureCursor, null, null, true, false, progressBarXFeat, out err);
                }
                catch (Exception ex)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    //********************************************************************
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "入库失败!");
                    return;
                }

                Marshal.ReleaseComObject(featureCursor);
                progressBarXLay.Value++;

                labelXMemo.Text = "";
                Application.DoEvents();
                if (err != null)
                {
                    res = false;
                    break;
                }
            }

            (pTagetWorkspace as IWorkspaceEdit).StopEditing(res);

            this.Cursor = System.Windows.Forms.Cursors.Default;
            if (res)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提取成功!");
                this.Close();
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提取失败!");
            }
        }

        private bool CreateFeatCls(IFeatureWorkspace featureWorkspace, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {


            IFields pFields = null;
            IObjectClassDescription pObjectClassDesc = null;
            UID pCLSID = null;
            UID pExtCLSID = null;
            IFeatureWorkspaceAnno pFeatWorkspaceAnno = null;
            IAnnoClass pAnnoCls = null;
            IGraphicsLayerScale pRefScale = null;
            try
            {
                err = null;
                //cyf 20110706 modify:提取数据时，创建本地数据库必须去掉用户名
                if (strTagetName.Contains("."))
                {
                    strTagetName = strTagetName.Substring(strTagetName.IndexOf('.') + 1);
                }
                //end
                //取源图层字段,并添加字段FromDate(生效日期),ToDate(失效日期),SourceOID(现势库对应数据OID),State(更新变化状态)
                pFields = (sourceFeatCls.Fields as IClone).Clone() as IFields;
                if (sourceFeatCls.FeatureType == esriFeatureType.esriFTSimple)
                {
                    featureWorkspace.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                else if (sourceFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pObjectClassDesc = new AnnotationFeatureClassDescriptionClass();
                    pCLSID = pObjectClassDesc.InstanceCLSID;
                    pExtCLSID = pObjectClassDesc.ClassExtensionCLSID;
                    pFeatWorkspaceAnno = (IFeatureWorkspaceAnno)featureWorkspace;
                    pAnnoCls = (IAnnoClass)sourceFeatCls.Extension;
                    pRefScale = new GraphicsLayerScaleClass();

                    // 设置参考比例尺的相关参数

                    pRefScale.ReferenceScale = pAnnoCls.ReferenceScale;
                    pRefScale.Units = pAnnoCls.ReferenceScaleUnits;
                    pFeatWorkspaceAnno.CreateAnnotationClass(strTagetName, pFields,
                                                                        pCLSID, pExtCLSID, sourceFeatCls.ShapeFieldName,
                                                                        "", null, null, pAnnoCls.AnnoProperties,
                                                                        pRefScale, pAnnoCls.SymbolCollection, false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pObjectClassDesc);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pRefScale);
                    pObjectClassDesc = null;
                    pFeatWorkspaceAnno = null;
                    pAnnoCls = null;
                    pRefScale = null;
                    pCLSID = null;
                    pExtCLSID = null;
                }
                pFields = null;
                return true;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                err = e;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObjectClassDesc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRefScale);
                pObjectClassDesc = null;
                pFeatWorkspaceAnno = null;
                pAnnoCls = null;
                pRefScale = null;
                pCLSID = null;
                pExtCLSID = null;
                pFields = null;
                return false;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 
            if (comBoxType.Text == "ESRI个人数据库(*.mdb)")
            {
                comBoxType.Tag = "PDB";
            }
            else if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
            {
                comBoxType.Tag = "GDB";
            }
            else if (comBoxType.Text == "ArcSDE(For Oracle)")
            {
                comBoxType.Tag = "SDE";
            }
            //end
        }
    }
}