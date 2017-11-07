using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Threading;
using SysCommon.Gis;

namespace GeoUtilities
{
    public partial class frmAppendFea : DevComponents.DotNetBar.Office2007Form
    {
       
        public frmAppendFea()
        {
            InitializeComponent();
        }

        private string m_strUser = "";
        public string User
        {
            set { m_strUser = value; }
        }

        private DateTime m_dt = DateTime.Now;


        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (m_pProThread != null) return;
            if (this.dataBaseSets.AppendFeaTB.Rows.Count<1)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "需要选择备份的数据集或要素类。");
                return;
            }

             try
            {
                //this.Cursor = Cursors.WaitCursor;
                this.pbCopyRows.Visible = true;
                this.lblTips.Text = "正在连接空间数据库...";
                this.lblTips.Refresh();

                IFeatureWorkspace pWksSource = this.ucDataConnectSource.GetWks() as IFeatureWorkspace;
                if (pWksSource == null)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                IFeatureWorkspace pWksTag = this.ucDataconnectTag.GetWks() as IFeatureWorkspace;

                if (pWksTag == null)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                //时间
                DateTime dt = DateTime.Now;
                m_dt = dt;
                string strUser = m_strUser;

                string strLogPath = Application.StartupPath + "\\..\\Log\\AppendFeaLog" + dt.ToString("yyyyMMddHHmmss") + ".txt";
                if (System.IO.File.Exists(strLogPath))
                {
                    System.IO.File.Delete(strLogPath);
                }

                string strErr = "";
                CopyFeaData cpyFea = new CopyFeaData(strLogPath, this.dataBaseSets.AppendFeaTB, strUser, pWksSource, pWksTag, dt, strErr);
                
                cpyFea.OnLblChange += new CopyFeaData.InitLblChangHandler(cpyFea_OnLblChange);
                cpyFea.OnInitProBarChangHandler += new CopyFeaData.InitProBarChangHandler(cpyFea_OnInitProBarChangHandler);
                cpyFea.OnGoStepChangHandler += new CopyFeaData.GoStepChangHandler(cpyFea_OnGoStepChangHandler);
                //m_pProThread = new Thread(new ThreadStart(cpyFea.CopyDatasetOrCls));//yjl20110815
                //m_pProThread.Start();
                cpyFea.CopyDatasetOrCls();
                pbCopyRows.Visible = false;
                //return;
            }
            catch (Exception ex)
            {
                if (m_pProThread != null)
                {
                    if (m_pProThread.IsAlive)
                    {
                        m_pProThread.Abort();
                    }
                }
                this.Cursor = Cursors.Default;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据备份失败！" + Environment.NewLine + ex.Message);
            }

            m_pProThread = null;
        }

        void cpyFea_OnOverCallBack()
        {

            this.lblTips.Text = "执行完成";
            this.pbCopyRows.Visible = false;
            this.Cursor = Cursors.Default;
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据备份完成。");
        }

        void cpyFea_OnLblChange(string strTips)
        {
            if (this.lblTips.InvokeRequired)
            {
                this.lblTips.BeginInvoke(new CopyFeaData.InitLblChangHandler(cpyFea_OnLblChange), new object[] { strTips });
                return;
            }

            lblTips.Text = strTips;
            lblTips.Refresh();
        }

        void cpyFea_OnGoStepChangHandler()
        {
            if (this.pbCopyRows.InvokeRequired)
            {
                this.pbCopyRows.BeginInvoke(new CopyFeaData.GoStepChangHandler(this.cpyFea_OnGoStepChangHandler));
                return;
            }

            pbCopyRows.PerformStep();
        }

        void cpyFea_OnInitProBarChangHandler(int intMax)
        {
            if (this.pbCopyRows.InvokeRequired)
            {
                this.pbCopyRows.BeginInvoke(new CopyFeaData.InitProBarChangHandler(cpyFea_OnInitProBarChangHandler), new object[] { intMax });
                return;
            }
            this.pbCopyRows.Minimum = 0;
            pbCopyRows.Maximum = intMax;
            pbCopyRows.Value = 0;
            pbCopyRows.Step = 1;
        }

        //
        Thread m_pProThread = null;

        private void cmdLog_Click(object sender, EventArgs e)
        {
            if (m_pProThread != null)
            {
                if (m_pProThread.IsAlive)
                {
                    return;
                }
            }

            string strLogPath = Application.StartupPath + "\\..\\Log\\AppendFeaLog" + m_dt.ToString("yyyyMMddHHmmss") + ".txt";
            if (System.IO.File.Exists(strLogPath))
            {
                System.Diagnostics.Process.Start(strLogPath);
            }
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAppendFea_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_pProThread != null)
            {
                if (m_pProThread.IsAlive)
                {
                    if (MessageBox.Show("正在执行，是否中断？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        m_pProThread.Abort();
                        m_pProThread = null;
                    }
                }
            }
        }

        private void btnGetNames_Click(object sender, EventArgs e)
        {
            //
            this.dataBaseSets.AppendFeaTB.Clear();//yjl20110815
            this.Cursor = Cursors.WaitCursor;
            IFeatureWorkspace pWksSource = this.ucDataConnectSource.GetWks() as IFeatureWorkspace;
            if (pWksSource == null)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            //获得名称
            IWorkspace pWks = pWksSource as IWorkspace;
            IEnumDatasetName pEnumName=pWks.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName pName=pEnumName.Next();
            while(pName!=null)
            {
                DataRow dr=this.dataBaseSets.AppendFeaTB.NewRow();
                dr["colChecked"]=false;
                dr["colDatasetName"]=pName.Name;
                dr["colAliasName"]=pName.Name;
                dr["colDataType"]="数据集";

                this.dataBaseSets.AppendFeaTB.Rows.Add(dr);

                pName=pEnumName.Next();
            }

            //要素类
            pEnumName = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            pName = pEnumName.Next();
            while (pName != null)
            {
                DataRow dr = this.dataBaseSets.AppendFeaTB.NewRow();
                dr["colChecked"] = false;
                dr["colDatasetName"] = pName.Name;

                //要素类
                IFeatureWorkspace pFeawks = pWks as IFeatureWorkspace;
                IFeatureClass pFeacls = pFeawks.OpenFeatureClass(pName.Name);
                dr["colAliasName"] = pFeacls.AliasName;
                dr["colDataType"] = "要素类";

                this.dataBaseSets.AppendFeaTB.Rows.Add(dr);

                pName = pEnumName.Next();
            }

            this.Cursor = Cursors.Default;
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataBaseSets.AppendFeaTB.Rows.Count; i++)
            {
                this.dataBaseSets.AppendFeaTB.Rows[i]["colChecked"] = true;
            }
        }

        private void btnNoSel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataBaseSets.AppendFeaTB.Rows.Count; i++)
            {
                bool blnCheck = (bool)this.dataBaseSets.AppendFeaTB.Rows[i]["colChecked"];
                this.dataBaseSets.AppendFeaTB.Rows[i]["colChecked"] = !blnCheck;
            }
        }

        private void ucDataConnectSource_Load(object sender, EventArgs e)
        {

        }
    }


    /// <summary>
    /// 数据拷贝类
    /// </summary>
    public class CopyFeaData
    {
        string strLogPath="";
        string strUser="";
        IFeatureWorkspace pWksTag=null;
        DateTime dt = DateTime.Now;

        //理由同上
        private IEnumFieldError pEnumFieldError = null;//字段检查错误集，为了给出错的字段赋值yjl20110804 add
        private IFields pFixedField = null;//字段检查类修正后的字段集，根据错误集寻找修正后的字段名
        // 定义委托   
        public delegate void InitProBarChangHandler(int intMax);
        public delegate void InitLblChangHandler(string strTips);
        public delegate void GoStepChangHandler();

        public delegate void OverCallBack();//线程结束回调委托

        //为委托指定调用方法   
        public event InitProBarChangHandler OnInitProBarChangHandler;
        public event GoStepChangHandler OnGoStepChangHandler;
        public event InitLblChangHandler OnLblChange;

        public CopyFeaData(string strLogPath,DataTable datatable,string strUser, IFeatureWorkspace pSourceWks, IFeatureWorkspace pWksTag, DateTime dt, string strErr)
        {
            this.strLogPath = strLogPath;
            this.strUser = strUser;
            this.pWksTag = pWksTag;
            this.dt = dt;
            this.m_pTagWks = pWksTag;
            this.m_pSourceWks = pSourceWks;
            m_dataTable = datatable;
        }

        public void BeginCopy(IFeatureDataset pSourceDataset,System.IO.StreamWriter fw)
        {
            try
            {
                IEnumDataset pEnumSubSourceDataset = pSourceDataset.Subsets;
                IDataset pSubDataset = pEnumSubSourceDataset.Next();
                while (pSubDataset != null)
                {
                    IFeatureClass pSourceFeaCls = pSubDataset as IFeatureClass;
                    if (pSourceFeaCls == null)
                    {
                        pSubDataset = pEnumSubSourceDataset.Next();
                        continue;
                    }

                    string[] strNames = pSubDataset.Name.Split('.');
                    string strSubName = strNames[strNames.GetLength(0) - 1];

                    IFeatureClass pTagFeaCls = pWksTag.OpenFeatureClass(strSubName);
                    if (pTagFeaCls == null)
                    {
                        fw.WriteLine(strSubName + "     无法获得目标数据源");
                        pSubDataset = pEnumSubSourceDataset.Next();
                        continue;
                    }

                    int intSucCount = 0;
                    string strError = "";

                    notcutExport(pSourceFeaCls, pTagFeaCls, dt, strUser, ref intSucCount, ref strError);

                    string strInfo = "";
                    if (strError == "")
                    {
                        strInfo = strSubName + "     成功复制数据" + intSucCount + "条";
                    }
                    else
                    {
                        strInfo = strSubName + "     成功复制数据" + intSucCount + "条" + "  出现错误信息:" + strError;
                    }

                    fw.WriteLine(strInfo);

                    pSubDataset = pEnumSubSourceDataset.Next();
                }
            }
            catch (Exception ex)
            {
                fw.WriteLine("数据出现异常："  + ex.Message);
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "DLG数据追加复制失败！" + Environment.NewLine + ex.Message);
                //MessageBox.Show("数据追加复制失败。" + Environment.NewLine + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //OnOverCallBack();
        }

        private DataTable m_dataTable = null;
        private IFeatureWorkspace m_pSourceWks = null;
        private IFeatureWorkspace m_pTagWks = null;

        public void CopyDatasetOrCls() 
        {
            if (m_dataTable == null) return;

            System.IO.StreamWriter fw = null;
            fw = System.IO.File.CreateText(strLogPath);
            fw.WriteLine("*******数据备份日志 " + System.DateTime.Now.ToString() + "*********");
            fw.WriteLine();
            fw.WriteLine("开始时间:" + DateTime.Now.ToString());
            string strErr = "";

            //添加警告信息
            if (strErr != "")
            {
                fw.WriteLine("警告信息:");
                fw.WriteLine(strErr);
                fw.WriteLine();
            }

            for (int i = 0; i < this.m_dataTable.Rows.Count; i++)
            {
                bool blnCheck = (bool)m_dataTable.Rows[i]["colChecked"];
                if (!blnCheck) continue;

                string strDataName = m_dataTable.Rows[i]["colDatasetName"].ToString();
                string strDataType = m_dataTable.Rows[i]["colDataType"].ToString();

                int intSusCount = 0;
                //if (strDataName.Contains("."))
                //{
                //    int idex = strDataName.LastIndexOf('.');
                //    strDataName = strDataName.Substring(idex+1);
                //}
                if (strDataType == "数据集")
                {
                    IDataset pDatasetSource = m_pSourceWks.OpenFeatureDataset(strDataName) as IDataset;
                    string desName = strDataName;
                    if (desName.Contains("."))
                    {
                        int idex = desName.LastIndexOf('.');
                        desName = desName.Substring(idex + 1);
                    }
                    IDataset pTagDataset = CreateDatasetByS(pDatasetSource, m_pTagWks as IWorkspace, desName, ref strErr);
                    if (pTagDataset == null)
                    {
                        fw.WriteLine("无法创建目标数据集: " + strDataName);
                    }
                    if (strErr != "")
                    {
                        fw.WriteLine("创建目标数据集:" + strDataName + " 出现警告信息 " + strErr);
                    }

                    IFeatureDataset pDataset = m_pSourceWks.OpenFeatureDataset(strDataName);


                    //复制数据集
                    BeginCopy(pDataset,fw);
                }
                else if (strDataType == "要素类")
                {
                    IFeatureClass pFeaClsOrg=m_pSourceWks.OpenFeatureClass(strDataName);
                    IFeatureClass pFeaCls = CreateFeatureClass(strDataName, pFeaClsOrg, null, m_pTagWks);
                    if (pFeaCls == null)
                    {
                        fw.WriteLine("无法创建目标要素类: " + strDataName);
                    }
                    if (strErr != "")
                    {
                        fw.WriteLine("创建目标要素类集:" + strDataName + " 出现警告信息 " + strErr);
                    }

                    //拷贝要素
                    notcutExport(pFeaClsOrg, pFeaCls, System.DateTime.Now, "", ref intSusCount, ref strErr);
                    string strInfo = "";
                    if (strErr == "")
                    {
                        strInfo = strDataName + "     成功复制数据" + intSusCount + "条";
                    }
                    else
                    {
                        strInfo = strDataName + "     成功复制数据" + intSusCount + "条" + "  出现错误信息:" + strErr;
                    }

                    fw.WriteLine(strInfo);
                }
            }

            fw.WriteLine();
            fw.WriteLine("结束时间:" + DateTime.Now.ToString());
            fw.Close();
            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "");
            OnLblChange("");//yjl20110815
            
            MessageBox.Show("数据备份完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        //不剪裁输出
        private void notcutExport(IFeatureClass pFromFeaCls, IFeatureClass pToFeatureClass, DateTime dt, string strUser, ref int intSucCount, ref string strError)
        {

            try
            {
                strError = "";
                intSucCount = 0;
                int intCount = pFromFeaCls.FeatureCount(null);
                IFeatureCursor pCursor = pFromFeaCls.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();
                IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                OnInitProBarChangHandler(intCount);
                OnLblChange("正在复制：" + pFromFeaCls.AliasName);

                int iCount = 0;
                while (pFeature != null)
                {
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        string sFieldName = pFeature.Fields.get_Field(i).Name;

                        int iIndex = pFeatureBuffer.Fields.FindField(sFieldName);

                        if ((iIndex > -1) && (pFeatureBuffer.Fields.get_Field(iIndex).Editable == true))
                        {
                            pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                        }
                    }
                    if (pEnumFieldError != null)//yjl20110804 add
                    {
                        pEnumFieldError.Reset();
                        IFieldError pFieldError = pEnumFieldError.Next();
                        while (pFieldError != null)
                        {
                            int srcIx = pFieldError.FieldIndex;//错误字段的源索引
                            int desIx = pFeatureBuffer.Fields.FindField(pFixedField.get_Field(srcIx).Name);//错误字段的新索引
                            if (desIx == -1)
                            {
                                pFieldError = pEnumFieldError.Next();
                                continue;
                            }
                            IField pFld = pFeatureBuffer.Fields.get_Field(desIx);
                            if ((desIx > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                            {
                                pFeatureBuffer.set_Value(desIx, pFeature.get_Value(srcIx));
                            }
                            pFieldError = pEnumFieldError.Next();
                        }
                    }
                    ////插入日期和入库人
                    //int intDateIndex = pFeatureBuffer.Fields.FindField("ImportTime");
                    //if (intDateIndex > -1) pFeatureBuffer.set_Value(intDateIndex, dt);
                    //int intUserIndex = pFeatureBuffer.Fields.FindField("ImportUser");
                    //if (intUserIndex > -1) pFeatureBuffer.set_Value(intUserIndex, strUser);

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    if (!InsertFea(ref pFeatureCursor, ref pFeatureBuffer))
                    {
                        strError = strError + Environment.NewLine + pFeature.OID + "  无法复制";

                        pFeatureCursor.Flush();
                        iCount = 0;
                        OnGoStepChangHandler();
                        pFeature = pCursor.NextFeature();
                        continue;
                    }

                    iCount++;
                    if (iCount == 2000)
                    {
                        pFeatureCursor.Flush();
                        iCount = 0;

                        Application.DoEvents();
                    }

                    //pbCopyRows.PerformStep();
                    OnGoStepChangHandler();
                    intSucCount++;

                    pFeature = pCursor.NextFeature();
                }
                if (iCount > 0) pFeatureCursor.Flush();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pFeatureCursor = null;

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
                pFeatureBuffer = null;
            }
            catch (Exception ex)
            {
                strError = strError + Environment.NewLine + ex.Message;
            }
        }

        private bool InsertFea(ref IFeatureCursor pFeaCursor, ref IFeatureBuffer pFeaBuf)
        {
            try
            {
                pFeaCursor.InsertFeature(pFeaBuf);
                return true;
            }
            catch
            {
                return false;
            }
        }


        //初始化数据库 创建数据集体
        private IDataset CreateDatasetByS(IDataset pDataset, IWorkspace pWks, string strTagDatasetName, ref string strError)
        {
            IGeoDataset pGeoDataS = pDataset as IGeoDataset;
            IWorkspace2 pWks2 = pWks as IWorkspace2;
            IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
            IDataset pTagDataset = null;

            if (!pWks2.get_NameExists(esriDatasetType.esriDTFeatureDataset, strTagDatasetName))
            {
                pTagDataset = pFeaWks.CreateFeatureDataset(strTagDatasetName, pGeoDataS.SpatialReference);
            }
            else
            {
                pTagDataset = pFeaWks.OpenFeatureDataset(strTagDatasetName);
            }
            if (pTagDataset == null) return null;

            IEnumDataset pEnumSubFeaCls = pDataset.Subsets;
            IDataset pSubDataset = pEnumSubFeaCls.Next();

            while (pSubDataset != null)
            {
                IFeatureClass pSubFeaCls = pSubDataset as IFeatureClass;
                if (pSubFeaCls == null)
                {
                    pSubDataset = pEnumSubFeaCls.Next();
                    continue;
                }

                string[] strSubNames = pSubDataset.Name.Split('.');
                string strSubName = strSubNames[strSubNames.GetLength(0) - 1];
                if (pWks2.get_NameExists(esriDatasetType.esriDTFeatureClass, strSubName))
                {
                    IFeatureClass pTagCls = pFeaWks.OpenFeatureClass(strSubName);
                    if (pTagCls.FeatureDataset != null)
                    {
                        string[] strTagFeaClsNames = pTagCls.FeatureDataset.Name.Split('.');
                        string strTagFeaClsName = strTagFeaClsNames[strTagFeaClsNames.GetLength(0) - 1];
                        if (strTagFeaClsName.ToUpper() == strTagDatasetName.ToUpper())
                        {
                            pSubDataset = pEnumSubFeaCls.Next();
                            continue;//已经存在 继续
                        }
                        else
                        {
                            strError = strError + Environment.NewLine + strSubName + "  不在指定的数据集下";
                        }
                    }
                    else
                    {
                        strError = strError + Environment.NewLine + strSubName;
                    }
                }
                else
                {
                    //创建要素
                    IFeatureDataset pTagFeaDataset = pTagDataset as IFeatureDataset;
                    IFeatureClass pTagsubCls = CreateFeatureClass(strSubName, pSubFeaCls, pTagFeaDataset, pTagFeaDataset.Workspace as IFeatureWorkspace);
                    if (pTagsubCls == null)
                    {
                        strError = strError + Environment.NewLine + strSubName + "：要素类创建失败";
                        pSubDataset = pEnumSubFeaCls.Next();//yjl20110815
                        continue;
                    }

                    //如果是注记
                    if (pTagsubCls.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        CopyAnnoPropertyToFC(pSubFeaCls, pTagsubCls);
                    }
                }

                pSubDataset = pEnumSubFeaCls.Next();
            }

            return pTagDataset;
        }


        //创建featureclass
        private IFeatureClass CreateFeatureClass(string name, IFeatureClass pFeaCls, IFeatureDataset pFeaDataset,IFeatureWorkspace pWks)
        {
            UID uidCLSID = null;
            UID uidCLSEXT = null;


            try
            {
                IObjectClassDescription pObjCls = null;
                if (uidCLSID == null)
                {
                    //esriGeometryType GeometryType;
                    uidCLSID = new UIDClass();
                    switch (pFeaCls.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            //GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            //GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            //GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            //GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            //GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pFeaCls.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            pObjCls = new AnnotationFeatureClassDescriptionClass();
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }
                //IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFieldChecker fdCheker = new FieldCheckerClass();//yjl20110804 add
                pEnumFieldError = null;
                pFixedField = null;
                fdCheker.ValidateWorkspace = pWks as IWorkspace;
                fdCheker.Validate(pFeaCls.Fields, out pEnumFieldError, out pFixedField);

                //string strShapeFieldName = pfeaturelayer.FeatureClass.ShapeFieldName;//geometry字段名
                //string[] strShapeNames = strShapeFieldName.Split('.');
                //strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];

                IFields pFields = new FieldsClass();
                if (pObjCls != null)
                {
                    IFeatureClassDescription pClsDes = pObjCls as IFeatureClassDescription;
                    pFields = pObjCls.RequiredFields;
                }

                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                for (int i = 0; i < pFeaCls.Fields.FieldCount; i++)
                {
                    IField pf = pFeaCls.Fields.get_Field(i);
                    string strFieldName = pf.Name;
                    string[] strFieldNames = strFieldName.Split('.');

                    bool blnfind = false;
                    for (int j = 0; j < pFields.FieldCount; j++)
                    {
                        IField pf2 = pFields.get_Field(j);
                        string[] strfields2 = pf2.Name.Split('.');
                        if (strfields2[strfields2.GetLength(0) - 1].ToUpper() == strFieldNames[strFieldNames.GetLength(0) - 1].ToUpper())
                        {
                            blnfind = true;
                            break;
                        }
                    }

                    if (blnfind) continue;

                    if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        if (pFeaCls.ShapeFieldName == pf.Name) continue;
                    }

                    if (pFeaCls.LengthField != null)
                    {
                        if (pFeaCls.LengthField.Name == pf.Name) continue;
                    }
                    if (pFeaCls.AreaField != null)
                    {
                        if (pFeaCls.AreaField.Name == pf.Name) continue;
                    }

                    IClone pClone = pFeaCls.Fields.get_Field(i) as IClone;
                    IField pTempField = pClone.Clone() as IField;
                    IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;

                    if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;

                    pTempFieldEdit.Name_2 = strFieldNames[strFieldNames.GetLength(0) - 1];
                    pFieldsEdit.AddField(pTempField);
                }

                string strShapeFieldName = pFeaCls.ShapeFieldName;
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];

                //修改geometrydef
                IField pFieldShape = pFeaCls.Fields.get_Field(pFeaCls.Fields.FindField(pFeaCls.ShapeFieldName));

                if (pFieldShape != null)
                {
                    IFieldEdit pFieldShapeEdit = pFields.get_Field(pFields.FindField(strShapeFieldName)) as IFieldEdit;
                    pFieldShapeEdit.GeometryDef_2 = pFieldShape.GeometryDef;
                }

                IGeometryDef pGeoDef = pFieldShape.GeometryDef;
                double dblIndex = pGeoDef.get_GridSize(0);

                //添加两个字段一个时间 一个名称
                if (pFields.FindField("ImportTime") < 0)
                {
                    IField pNewField1 = new FieldClass();
                    IFieldEdit pNewEdit1 = pNewField1 as IFieldEdit;
                    pNewEdit1.Name_2 = "ImportTime";
                    pNewEdit1.AliasName_2 = "入库时间";
                    pNewEdit1.Type_2 = esriFieldType.esriFieldTypeDate;
                    pFieldsEdit.AddField(pNewField1);
                }

                if (pFields.FindField("ImportUser") < 0)
                {
                    IField pNewField2 = new FieldClass();
                    IFieldEdit pNewEdit2 = pNewField2 as IFieldEdit;
                    pNewEdit2.Name_2 = "ImportUser";
                    pNewEdit2.AliasName_2 = "入库人";
                    pNewEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                    pFieldsEdit.AddField(pNewField2);
                }

                IFeatureClass targetFeatureclass = null;


                if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    IAnnoClass pAnno = pFeaCls.Extension as IAnnoClass;

                    IFeatureWorkspaceAnno pWksAnno = pWks as IFeatureWorkspaceAnno;
                    IGraphicsLayerScale pGl = new GraphicsLayerScaleClass();
                    pGl.ReferenceScale = pAnno.ReferenceScale;
                    pGl.Units = pAnno.ReferenceScaleUnits;
                    targetFeatureclass = pWksAnno.CreateAnnotationClass(name, pFields, pFeaCls.CLSID, pFeaCls.EXTCLSID, strShapeFieldName, "", pFeaDataset, null, pAnno.AnnoProperties, pGl, pAnno.SymbolCollection, false);
                }
                else
                {
                    if (pFeaDataset != null)
                    {
                        targetFeatureclass = pFeaDataset.CreateFeatureClass(name, pFixedField, uidCLSID, uidCLSEXT, pFeaCls.FeatureType, strShapeFieldName, "");
                    }
                    else
                    {
                        targetFeatureclass = pWks.CreateFeatureClass(name, pFixedField, uidCLSID, uidCLSEXT, pFeaCls.FeatureType, strShapeFieldName, "");
                    }
                   
                }

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.2的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }

        //注记复制
        private void CopyAnnoPropertyToFC(IFeatureClass pSourceFeatureClass, IFeatureClass pToFeatureClass)
        {
            IAnnoClass pSourceAnnoClass = (IAnnoClass)pSourceFeatureClass.Extension;
            IAnnoClass pTargerAnnoClass = (IAnnoClass)pToFeatureClass.Extension;

            IAnnotateLayerPropertiesCollection pSourceAnnoProperCollection = pSourceAnnoClass.AnnoProperties;
            IClone pAnnoCollection = (IClone)pSourceAnnoProperCollection;

            ISymbolCollection pSourceSymbolCollection = pSourceAnnoClass.SymbolCollection;
            IClone pAnnoSymbol = (IClone)pSourceSymbolCollection;

            IAnnoClassAdmin2 pAnnoClassAdmin = (IAnnoClassAdmin2)pTargerAnnoClass;

            pAnnoClassAdmin.ReferenceScale = pSourceAnnoClass.ReferenceScale;
            pAnnoClassAdmin.ReferenceScaleUnits = pSourceAnnoClass.ReferenceScaleUnits;

            pAnnoClassAdmin.AnnoProperties = (IAnnotateLayerPropertiesCollection)pAnnoCollection;
            pAnnoClassAdmin.SymbolCollection = (ISymbolCollection)pAnnoSymbol;

            pAnnoClassAdmin.UpdateProperties();
            pAnnoClassAdmin.UpdateOnShapeChange = true;

        }

    }

}