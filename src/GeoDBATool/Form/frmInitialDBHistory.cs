using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public partial class frmInitialDBHistory : DevComponents.DotNetBar.Office2007Form
    {
        private XmlElement m_DbProjectElement;

        public frmInitialDBHistory()
        {
            InitializeComponent();
            InitializeForm();
        }

        public frmInitialDBHistory(XmlElement dbProjectElement)
        {
            InitializeComponent();
            m_DbProjectElement = dbProjectElement;
            InitializeForm();
        }

        private void InitializeForm()
        {
            object[] TagDBType = new object[] { "SDE", "GDB", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 1;
            comboBoxTypeHistory.Items.AddRange(TagDBType);
            comboBoxTypeHistory.SelectedIndex = 1;

            if (m_DbProjectElement != null)
            {
                try
                {
                    XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                    FillOrgDB(aElement);
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
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                    return;
                }
            }
        }

        private void FillOrgDB(XmlElement aElement)
        {
            comBoxType.Text = aElement.GetAttribute("类型");
            txtServer.Text = aElement.GetAttribute("服务器");
            txtInstance.Text = aElement.GetAttribute("服务名");
            txtDB.Text = aElement.GetAttribute("数据库");
            txtVersion.Text = aElement.GetAttribute("版本");
            txtUser.Text = aElement.GetAttribute("用户");
            txtPassword.Text = aElement.GetAttribute("密码");

            string strName = (aElement.FirstChild as XmlElement).GetAttribute("名称");
            if (strName != string.Empty)
            {
                ListViewItem aItem = listViewEx.Items.Add(strName);
                aItem.Tag = "FD";
                aItem.Checked = true;
            }
        }

        private void FillObjDB(XmlElement aElement)
        {
            comboBoxTypeHistory.Text = aElement.GetAttribute("类型");
            txtServerHistory.Text = aElement.GetAttribute("服务器");
            txtInstanceHistory.Text = aElement.GetAttribute("服务名");
            txtDBHistory.Text = aElement.GetAttribute("数据库");
            txtVersionHistory.Text = aElement.GetAttribute("版本");
            txtUserHistory.Text = aElement.GetAttribute("用户");
            txtPasswordHistory.Text = aElement.GetAttribute("密码");
        }

        private void SaveObjDB(XmlElement aElement, string strName)
        {
            aElement.SetAttribute("类型", comboBoxTypeHistory.Text);
            aElement.SetAttribute("服务器", txtServerHistory.Text);
            aElement.SetAttribute("服务名", txtInstanceHistory.Text);
            aElement.SetAttribute("数据库", txtDBHistory.Text);
            aElement.SetAttribute("版本", txtVersionHistory.Text);
            aElement.SetAttribute("用户", txtUserHistory.Text);
            aElement.SetAttribute("密码", txtPasswordHistory.Text);
            (aElement.FirstChild as XmlElement).SetAttribute("名称", strName);
            aElement.OwnerDocument.Save(ModData.v_projectXML);
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxType.Text != "SDE")
            {
                btnDB.Visible = true;
                txtDB.Size = new Size(txtServer.Size.Width - btnDB.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnDB.Visible = false;
                txtDB.Size = new Size(txtServer.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtVersion.Enabled = true;

            }
        }

        private void comboBoxTypeHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTypeHistory.Text != "SDE")
            {
                btnDBHistory.Visible = true;
                txtDBHistory.Size = new Size(txtServerHistory.Size.Width - btnDBHistory.Size.Width, txtDBHistory.Size.Height);
                txtServerHistory.Enabled = false;
                txtInstanceHistory.Enabled = false;
                txtUserHistory.Enabled = false;
                txtPasswordHistory.Enabled = false;
                txtVersionHistory.Enabled = false;
            }
            else
            {
                btnDBHistory.Visible = false;
                txtDBHistory.Size = new Size(txtServerHistory.Size.Width, txtDBHistory.Size.Height);
                txtServerHistory.Enabled = true;
                txtInstanceHistory.Enabled = true;
                txtUserHistory.Enabled = true;
                txtPasswordHistory.Enabled = true;
                txtVersionHistory.Enabled = true;

            }
        }

        private void btnLink_Click(object sender, EventArgs e)
        {
            listViewEx.Items.Clear();

            Exception err = null;
            SysCommon.Gis.SysGisDataSet sysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            switch (comBoxType.Text)
            {
                case "SDE":
                    sysGisDataSet.SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out err);
                    break;
                case "GDB":
                    sysGisDataSet.SetWorkspace(txtDB.Text, SysCommon.enumWSType.GDB, out err);
                    break;
                case "PDB":
                    sysGisDataSet.SetWorkspace(txtDB.Text, SysCommon.enumWSType.PDB, out err);
                    break;
                default:
                    break;
            }

            if (err != null || sysGisDataSet.WorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未设置用户数据库连接或连接失败,请检查!");
                return;
            }

            List<string> lstNames = sysGisDataSet.GetAllFeatureDatasetNames();
            foreach (string name in lstNames)
            {
                ListViewItem aItem = listViewEx.Items.Add(name);
                aItem.Tag = "FD";
                aItem.Checked = true;
            }
            lstNames = sysGisDataSet.GetFeatureClassNames();
            foreach (string name in lstNames)
            {
                ListViewItem aItem = listViewEx.Items.Add(name);
                aItem.Tag = "FC";
                aItem.Checked = true;
            }

            sysGisDataSet.Dispose();
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = !aItem.Checked;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            listViewEx.Items.Clear();
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            txtDB.Text = pFolderBrowser.SelectedPath;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件!");
                        }
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDB.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void btnDBHistory_Click(object sender, EventArgs e)
        {
            switch (comboBoxTypeHistory.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        DirectoryInfo dir = new DirectoryInfo(pFolderBrowser.SelectedPath);
                        string name = dir.Name;
                        if (dir.Parent == null)
                        {
                            name = dir.Name.Substring(0, dir.Name.Length - 2);
                        }
                        txtDBHistory.Text = dir.FullName + "\\" + name + ".gdb";
                    }
                    break;

                case "PDB":
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.OverwritePrompt = false;
                    saveFile.Title = "保存为ESRI个人数据库";
                    saveFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDBHistory.Text = saveFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (listViewEx.CheckedItems.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未选择对象,无法生成历史!");
                return;
            }

            Exception err = null;
            //获取现势库连接信息

            SysCommon.Gis.SysGisDataSet sourceSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
            switch (comBoxType.Text)
            {
                case "SDE":
                    sourceSysGisDataSet.SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out err);
                    break;
                case "GDB":
                    sourceSysGisDataSet.SetWorkspace(txtDB.Text, SysCommon.enumWSType.GDB, out err);
                    break;
                case "PDB":
                    sourceSysGisDataSet.SetWorkspace(txtDB.Text, SysCommon.enumWSType.PDB, out err);
                    break;
                default:
                    break;
            }

            if (err != null || sourceSysGisDataSet.WorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未设置用户数据库连接或连接失败,请检查!");
                return;
            }

            //获取历史库连接信息

            IPropertySet pPropSet = new PropertySetClass();
            IWorkspace pTagetWorkspace = null;
            try
            {
                switch (comboBoxTypeHistory.Text)
                {
                    case "PDB":
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(txtDBHistory.Text))
                        {
                            FileInfo filePDB = new FileInfo(txtDBHistory.Text);
                            pAccessFact.Create(filePDB.DirectoryName, filePDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", txtDBHistory.Text);
                        pTagetWorkspace = pAccessFact.Open(pPropSet, 0);

                        break;
                    case "GDB":
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        DirectoryInfo dirGDB = new DirectoryInfo(txtDBHistory.Text);
                        pFileGDBFact.Create(dirGDB.Parent.FullName, dirGDB.Name.Substring(0, dirGDB.Name.Length - 4), null, 0);
                        pPropSet.SetProperty("DATABASE", txtDBHistory.Text);
                        pTagetWorkspace = pFileGDBFact.Open(pPropSet, 0);

                        break;
                    case "SDE":
                        IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                        pPropSet.SetProperty("SERVER", txtServerHistory.Text);
                        pPropSet.SetProperty("INSTANCE", txtInstanceHistory.Text);
                        pPropSet.SetProperty("DATABASE", txtDBHistory.Text);
                        pPropSet.SetProperty("USER", txtUserHistory.Text);
                        pPropSet.SetProperty("PASSWORD", txtPasswordHistory.Text);
                        pPropSet.SetProperty("VERSION", txtVersionHistory.Text);
                        pTagetWorkspace = pSdeFact.Open(pPropSet, 0);

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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未设置历史库连接或连接失败,请检查!");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.AppStarting;

            List<string> lstData = new List<string>();
            try
            {
                //根据用户数据库结构创建历史库
                foreach (ListViewItem aItem in listViewEx.CheckedItems)
                {
                    if (aItem.Tag.ToString() == "FD")
                    {
                        IFeatureDataset tagetFeatureDataset = null;
                        IFeatureDataset pFeatureDataset = sourceSysGisDataSet.GetFeatureDataset(aItem.Text, out err);
                        if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureDataset, aItem.Text + "_GOH"))
                        {
                            tagetFeatureDataset = (pTagetWorkspace as IFeatureWorkspace).CreateFeatureDataset(aItem.Text + "_GOH", (pFeatureDataset as IGeoDataset).SpatialReference);
                        }
                        else
                        {
                            tagetFeatureDataset = (pTagetWorkspace as IFeatureWorkspace).OpenFeatureDataset(aItem.Text + "_GOH");
                        }

                        IEnumDataset pEnumDs = pFeatureDataset.Subsets;
                        pEnumDs.Reset();
                        IDataset pDs = pEnumDs.Next();
                        while (pDs != null)
                        {
                            IFeatureClass pFeatureClass = pDs as IFeatureClass;
                            if (pFeatureClass != null)
                            {
                                if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, pDs.Name + "_GOH"))
                                {
                                    CreateFeatCls(tagetFeatureDataset, pFeatureClass, pDs.Name + "_GOH", out err);
                                }
                                lstData.Add(pDs.Name);
                            }
                            pDs = pEnumDs.Next();
                        }
                    }
                    else if (aItem.Tag.ToString() == "FC")
                    {
                        IFeatureClass pFeatureClass = sourceSysGisDataSet.GetFeatureClass(aItem.Text, out err);
                        if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, aItem.Text + "_GOH"))
                        {
                            CreateFeatCls(pTagetWorkspace as IFeatureWorkspace, pFeatureClass, aItem.Text + "_GOH", out err);
                        }
                        lstData.Add(aItem.Text);
                    }
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
                this.Cursor = System.Windows.Forms.Cursors.Default;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建历史库结构失败!");
                return;
            }

            //遍历现势库数据FC进行数据移植
            Dictionary<string, string> dicFieldsPair = new Dictionary<string, string>();
            dicFieldsPair.Add("OBJECTID", "SourceOID");
            Dictionary<string, object> dicValue = new Dictionary<string, object>();
            dicValue.Add("FromDate", DateTime.Now.ToString("u"));
            dicValue.Add("ToDate", DateTime.MaxValue.ToString("u"));//.ToString("YYYY-MM-DD HH:MI:SS"));
            dicValue.Add("State", 0);
            (pTagetWorkspace as IWorkspaceEdit).StartEditing(false);
            bool res = true;
            progressBarXLay.Maximum = lstData.Count;
            progressBarXLay.Minimum = 0;
            progressBarXLay.Value = 0;
            foreach (string aFeatClsName in lstData)
            {
                labelXMemo.Text = "正在进行图层" + aFeatClsName + "...";
                Application.DoEvents();
                int cnt = 0;
                int allcnt = 0;
                IFeatureCursor featureCursor = null;
                IFeatureClass tagetFeatCls = null;
                try
                {
                    featureCursor = sourceSysGisDataSet.GetFeatureCursor(aFeatClsName, "", null, "", out err, out cnt, out allcnt);
                    tagetFeatCls = (pTagetWorkspace as IFeatureWorkspace).OpenFeatureClass(aFeatClsName + "_GOH");
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
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    return;
                }

                progressBarXFeat.Maximum = cnt;
                progressBarXFeat.Minimum = 0;
                progressBarXFeat.Value = 0;
                ModDBOperator.NewFeatures(tagetFeatCls, featureCursor, dicFieldsPair, dicValue, true, false, progressBarXFeat, out err);
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
                if (m_DbProjectElement != null)
                {
                    try
                    {
                        XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
                        SaveObjDB(aElement, listViewEx.Items[0].Text + "_GOH");
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
                        return;
                    }
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成历史库成功!");
                this.Close();
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成历史库失败!");
            }
        }

        private bool CreateFeatCls(IFeatureWorkspace featureWorkspace, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {
            try
            {
                err = null;
                //取源图层字段,并添加字段FromDate(生效日期),ToDate(失效日期),SourceOID(现势库对应数据OID),State(更新变化状态)
                IFields pFields = (sourceFeatCls.Fields as IClone).Clone() as IFields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "FromDate";
                fieldEdit.AliasName_2 = "生效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "ToDate";
                fieldEdit.AliasName_2 = "失效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "SourceOID";
                fieldEdit.AliasName_2 = "现势库对应数据OID";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "State";
                fieldEdit.AliasName_2 = "更新变化状态";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);


                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "VERSION";
                fieldEdit.AliasName_2 = "更新版本号";
                fieldEdit.DefaultValue_2 = 0;
                fieldEdit.IsNullable_2 = false;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);


                if (sourceFeatCls.FeatureType == esriFeatureType.esriFTSimple)
                {
                    featureWorkspace.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                else if (sourceFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    IObjectClassDescription pObjectClassDesc = new AnnotationFeatureClassDescriptionClass();
                    UID pCLSID = pObjectClassDesc.InstanceCLSID;
                    UID pExtCLSID = pObjectClassDesc.ClassExtensionCLSID;
                    IFeatureWorkspaceAnno pFeatWorkspaceAnno = (IFeatureWorkspaceAnno)featureWorkspace;
                    IAnnoClass pAnnoCls = (IAnnoClass)sourceFeatCls.Extension;
                    IGraphicsLayerScale pRefScale = new GraphicsLayerScaleClass();

                    // 设置参考比例尺的相关参数

                    pRefScale.ReferenceScale = pAnnoCls.ReferenceScale;
                    pRefScale.Units = pAnnoCls.ReferenceScaleUnits;

                    pFeatWorkspaceAnno.CreateAnnotationClass(strTagetName, pFieldsEdit,
                                                                        pCLSID, pExtCLSID, sourceFeatCls.ShapeFieldName,
                                                                        "", null, null, pAnnoCls.AnnoProperties,
                                                                        pRefScale, pAnnoCls.SymbolCollection, false);


                }
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
                return false;
            }
        }

        private bool CreateFeatCls(IFeatureDataset featureDataset, IFeatureClass sourceFeatCls, string strTagetName, out Exception err)
        {
            try
            {
                err = null;
                IFeatureWorkspace featureWorkspace = featureDataset.Workspace as IFeatureWorkspace;

                //取源图层字段,并添加字段FromDate(生效日期),ToDate(失效日期),SourceOID(现势库对应数据OID),State(更新变化状态)
                IFields pFields = (sourceFeatCls.Fields as IClone).Clone() as IFields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField newfield = new FieldClass();                //字段对象
                IFieldEdit fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "FromDate";
                fieldEdit.AliasName_2 = "生效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "ToDate";
                fieldEdit.AliasName_2 = "失效日期";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "SourceOID";
                fieldEdit.AliasName_2 = "现势库对应数据OID";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "State";
                fieldEdit.AliasName_2 = "更新变化状态";
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);

                newfield = new FieldClass();                //字段对象
                fieldEdit = newfield as IFieldEdit;     //字段编辑对象
                fieldEdit.Name_2 = "VERSION";
                fieldEdit.AliasName_2 = "更新版本号";
                fieldEdit.DefaultValue_2 = 0;
                fieldEdit.IsNullable_2 = false;
                //字段类型要装化为枚举类型
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                pFieldsEdit.AddField(newfield);


                if (sourceFeatCls.FeatureType == esriFeatureType.esriFTSimple)
                {
                    featureDataset.CreateFeatureClass(strTagetName, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                else if (sourceFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    IObjectClassDescription pObjectClassDesc = new AnnotationFeatureClassDescriptionClass();
                    UID pCLSID = pObjectClassDesc.InstanceCLSID;
                    UID pExtCLSID = pObjectClassDesc.ClassExtensionCLSID;
                    IFeatureWorkspaceAnno pFeatWorkspaceAnno = (IFeatureWorkspaceAnno)featureWorkspace;
                    IAnnoClass pAnnoCls = (IAnnoClass)sourceFeatCls.Extension;
                    IGraphicsLayerScale pRefScale = new GraphicsLayerScaleClass();

                    // 设置参考比例尺的相关参数

                    pRefScale.ReferenceScale = pAnnoCls.ReferenceScale;
                    pRefScale.Units = pAnnoCls.ReferenceScaleUnits;

                    pFeatWorkspaceAnno.CreateAnnotationClass(strTagetName, pFieldsEdit,
                                                                        pCLSID, pExtCLSID, sourceFeatCls.ShapeFieldName,
                                                                        "", featureDataset, null, pAnnoCls.AnnoProperties,
                                                                        pRefScale, pAnnoCls.SymbolCollection, false);


                }
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
                return false;
            }
        }
    }
}