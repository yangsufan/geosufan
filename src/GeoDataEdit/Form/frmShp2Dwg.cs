using System;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
namespace GeoDataEdit
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110812
    /// 说明：dwg<->shp
    /// </summary>
    public partial class frmShp2Dwg : DevComponents.DotNetBar.Office2007Form
    {
        private string strColName = "名称";
        private string m_strTemplate = "";
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public frmShp2Dwg()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitListViewStyle(this.lstLyrFile);

            
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            //转换类型
            try
            {
                //check null
                if (txtSource.Text == "" || txtTarget.Text == "")
                    return;
                DCtype tempType = DCtype.NoXdata;
                switch (1)
                {
                    case 0:
                        tempType = DCtype.NoXdata;
                        break;
                    case 1:
                        tempType = DCtype.HasXdata;
                        break;
                    case 2:
                        tempType = DCtype.FmeXdata;
                        break;
                    default:
                        return;
                }

                if (m_OutType == OutType.gdb2dwg || m_OutType == OutType.mdb2dwg||m_OutType==OutType.shp2dwg)
                {
                    if (lstLyrFile.CheckedItems.Count == 0)
                        return;
                    string strgdbLyrs = GetInputFile(this.txtSource.Text);
                    if (tempType == DCtype.FmeXdata)
                    {
                        strgdbLyrs = this.txtSource.Text;
                    }
                    string strdwgfile = this.txtTarget.Text;

                    if (strgdbLyrs == "" || strdwgfile == "") return;
                    string strtemp = System.IO.Path.GetDirectoryName(strdwgfile);

                    if (strtemp == "")
                    {
                        MessageBox.Show("输出dwg的路径出现错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    strtemp=System.IO.Path.GetFileName(strdwgfile);
                    if (strtemp == "")
                    {
                        return;
                    }

                    if (System.IO.Path.GetExtension(strdwgfile).ToUpper() != ".DWG")
                    {
                        MessageBox.Show("输出dwg的路径后缀名应为dwg。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    this.Enabled = false;
                    label1.Text = "正在转换，请稍后...";
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("将矢量数据转换成dwg数据");
                    }
                    
                    Application.DoEvents();
                    QuickExportToDWG(strgdbLyrs, strdwgfile, tempType, "");
                    label1.Text = "";
                    Application.DoEvents();
                }
                else if(m_OutType==OutType.dwg2shp||m_OutType==OutType.dwg2gdb)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.Enabled = false;
                    label1.Text = "正在转换，请稍后...";
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("将dwg数据转换成矢量数据");
                    }
                    Application.DoEvents();
                    QuickImportTopdb(txtSource.Text, txtTarget.Text);
                    label1.Text = "";
                    Application.DoEvents();
                }

                this.Cursor = Cursors.Default;
                this.Enabled = true;
                MessageBox.Show("转换完成。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("完成转换");
                }
                //this.Close();//转换完成不应该关闭
            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.Enabled = true;
                label1.Text = "";
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("转换出现错误（" + ex.Message + "），停止转换");
                }
                MessageBox.Show("转换过程出现错误。" + Environment.NewLine + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

//        public   bool   IsPositiveNumber(String   path)   
//{ 
//        Regex   objNotPositivePattern   =   new   Regex(@ "^([a-zA-Z]:\\)?[^\/\:\*\?\ " "\ <\> \|\,]+$ "); 
//        return   objNotPositivePattern.IsMatch(path); 
//} 

        private string GetInputFile(string strPath)
        {
            string strTemp = "";
            for (int i = 0; i < this.lstLyrFile.Items.Count; i++)
            {
                if (!this.lstLyrFile.Items[i].Checked) continue;

                if (strTemp == "")
                {
                    strTemp = strPath + "\\" + this.lstLyrFile.Items[i].Text;
                }
                else
                {
                    strTemp =strTemp + ";" + strPath + "\\" + this.lstLyrFile.Items[i].Text;
                }
            }

            return strTemp;
        }

      

        enum DCtype
        {
            NoXdata,
            HasXdata,
            FmeXdata
        }

        private OutType m_OutType = OutType.Typeother;
        enum OutType
        {
            shp2dwg,
            gdb2dwg,
            mdb2dwg,
            dwg2gdb,
            dwg2mdb,
            dwg2shp,
            Typeother
        }

        private void DataChange(DCtype dcType, string strSource, string strTarget)
        {
            if (strSource.Substring(strSource.Length - 3).ToUpper() == strTarget.Substring(strTarget.Length - 3).ToUpper())
            {
                MessageBox.Show("数据格式一致，不需要转换。");
                return;
            }

            //由dwg到gdb数据转换
            if (strSource.Substring(strSource.Length - 3).ToUpper() == "DWG")
            {

            }
            else
            {

            }
        }

        private void DwgToGdb(string strSource, string strTarget)
        {

        }

        //通过workspace获得tool toolbox的名称 tool的名称 
        private ESRI.ArcGIS.Geoprocessing.IGPTool GetGPtoolByName(string strToolboxPath,string strToolBoxName,string strToolName)
        {
            IWorkspaceFactory pToolWksFac = new ToolboxWorkspaceFactoryClass();
            IToolboxWorkspace ptoolWks = pToolWksFac.OpenFromFile(strToolboxPath, 0) as IToolboxWorkspace;

            IGPToolbox pGPToolbox = ptoolWks.OpenToolbox(strToolBoxName);
            if (pGPToolbox == null) return null;

            IGPTool pGPTool = pGPToolbox.OpenTool(strToolName);
            return pGPTool;
        }
        

        //利用quickexport进行数据转换  gdb-->dwg 有扩展属性的转换
        private void QuickExportToDWG(string strSource, string strdwgFile, DCtype type,string strTemplate)
        {
            ITrackCancel pTrackCancel = null;

            //找到tool
            ESRI.ArcGIS.Geoprocessing.GeoProcessor _geoPro = new GeoProcessor();
            pTrackCancel = new TrackCancelClass();
            IVariantArray pVArray = new VarArrayClass();
            IVariantArray parameters = new VarArrayClass();

            //模板文件
            string strT="";
            if (strTemplate != "")
            {
                strT=",_TMPL," + strTemplate;// D:\南京\data\dddd.dwg
            }

            string strtoolName = "";
            if (type==DCtype.HasXdata)
            {
                parameters.Add(strSource);
                parameters.Add("ACAD," + strdwgFile);
                parameters.Add("RUNTIME_MACROS,");
                parameters.Add("_ATTRKIND,extended_entity_data,_REL,Release2000" + strT);
                parameters.Add(",META_MACROS,");
                parameters.Add("Dest_ATTRKIND,extended_entity_data,Dest_REL,Release2000" + strT);
                parameters.Add(",METAFILE,ACAD,COORDSYS,,__FME_DATASET_IS_SOURCE__,false");

                strtoolName="QuickExport_interop";
            }
            else if(type==DCtype.NoXdata)
            {
                parameters.Add(strSource);
                parameters.Add("ACAD," + strdwgFile);
                parameters.Add("RUNTIME_MACROS,");
                parameters.Add("_ATTRKIND,ignore,_REL,Release2000" + strT);
                parameters.Add(",META_MACROS,");
                parameters.Add("Dest_ATTRKIND,ignore,Dest_REL,Release2000" + strT);
                parameters.Add(",METAFILE,ACAD,COORDSYS,,__FME_DATASET_IS_SOURCE__,false");

                strtoolName="QuickExport_interop";
            }
            else if (type == DCtype.FmeXdata)
            {
                string strTblPath = Application.StartupPath + "\\dc.tbx";
                if (strTemplate == "")
                {
                    parameters.Add(strSource);
                    parameters.Add(strdwgFile);
                    _geoPro.AddToolbox(strTblPath);
                    strtoolName = "SpatialETLTool";
                }
                else
                {
                    parameters.Add(strSource);
                    parameters.Add(strdwgFile);
                    parameters.Add(strTemplate);
                    _geoPro.AddToolbox(strTblPath);
                    strtoolName = "SpatialETLTool";
                }
                
            }
            else
            {
                return;
            }
            
            IGPEnvironmentManager pgpEnv = new GPEnvironmentManager();
            IGPMessages pGpMessages; //= _geoPro.Validate(parameters, false, pgpEnv);
            IGPComHelper pGPCOMHelper = new GpDispatch();

            //这里是关键，如果不赋值的话，那么就会报错
            //IGPEnvironmentManager pEnvMgr = pGPCOMHelper.EnvironmentManager;
            //pgpEnv.PersistAll = true;
            //pGpMessages = new GPMessagesClass();

            // Execute the model tool by name.
            _geoPro.Execute(strtoolName, parameters, pTrackCancel);
        }
        //利用quickexport进行数据转换  dwg-->mdb 
        private void QuickImportTopdb(string strSrcDwg, string strdesPdb)
        {
            ITrackCancel pTrackCancel = null;

            //找到tool
            ESRI.ArcGIS.Geoprocessor.Geoprocessor _geoPro = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            //ESRI.ArcGIS.Geoprocessing.IGeoProcessor _geoPro = new ESRI.ArcGIS.Geoprocessing.GeoProcessorClass();
            pTrackCancel = new TrackCancelClass();
            IVariantArray pVArray = new VarArrayClass();
            IVariantArray parameters = new VarArrayClass();
            parameters.Add("ACAD," + strSrcDwg);
            string strtoolName = "";
            //parameters.Add("RUNTIME_MACROS,");
            //parameters.Add("METAFILE,acad,_EXPAND_BLOCKS,yes,ACAD_IN_USE_BLOCK_HEADER_LAYER,yes,"
            //+ "ACAD_IN_RESOLVE_ENTITY_COLOR,yes,_EXPAND_VISIBLE,yes,_READ_AS_2_5D,no,"
            //+ "_BULGES_AS_ARCS,no,_STORE_BULGE_INFO,no,_READ_PAPER_SPACE,no,"
            //+ "ACAD_IN_READ_GROUPS,no,_IGNORE_UCS,no,_ACADPreserveComplexHatches,no,"
            //+ "_MERGE_SCHEMAS,YES,");
            //parameters.Add("META_MACROS,");
            //parameters.Add("Source_EXPAND_BLOCKS,yes,"
            //+ "SourceACAD_IN_USE_BLOCK_HEADER_LAYER,yes,SourceACAD_IN_RESOLVE_ENTITY_COLOR,yes"
            //+ ",Source_EXPAND_VISIBLE,yes,Source_READ_AS_2_5D,no,Source_BULGES_AS_ARCS,no,"
            //+ "Source_STORE_BULGE_INFO,no,Source_READ_PAPER_SPACE,no,SourceACAD_IN_READ_GROUPS,no"
            //+ ",Source_IGNORE_UCS,no,Source_ACADPreserveComplexHatches,no,METAFILE,acad,COORDSYS,,IDLIST,,");
            //parameters.Add("__FME_DATASET_IS_SOURCE__,true");
            //string param = "__FME_DATASET_IS_SOURCE__,true";
            //parameters.Add(param);
            parameters.Add(strdesPdb+"\\..\\tmp.mdb");


            strtoolName = "QuickImport_interop";

            IGPEnvironmentManager pgpEnv = new GPEnvironmentManager();
            IGPMessages pGpMessages; //= _geoPro.Validate(parameters, false, pgpEnv);
            IGPComHelper pGPCOMHelper = new GpDispatch();

            //这里是关键，如果不赋值的话，那么就会报错
            IGPEnvironmentManager pEnvMgr = pGPCOMHelper.EnvironmentManager;
            pgpEnv.PersistAll = true;
            pGpMessages = new GPMessagesClass();
            _geoPro.OverwriteOutput = true;
             //Execute the model tool by name.
            _geoPro.Execute(strtoolName, parameters, pTrackCancel);
            //QuickImport qi = new QuickImport();
            //qi.Input = txtSource.Text;
            //qi.Output = txtTarget.Text;
            //_geoPro.Execute(qi, pTrackCancel);
            ESRI.ArcGIS.ConversionTools.FeatureClassToShapefile FCTS = new ESRI.ArcGIS.ConversionTools.FeatureClassToShapefile();
            FCTS.Output_Folder = strdesPdb;
            IWorkspaceFactory pWF=new AccessWorkspaceFactoryClass();
            IWorkspace pWorkSpace=pWF.OpenFromFile(strdesPdb+"\\..\\tmp.mdb",0);
            IEnumDatasetName pED=pWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            IDatasetName pDataSet=pED.Next();
            while(pDataSet!=null)
            {
                if ((pWorkSpace as IFeatureWorkspace).OpenFeatureClass(pDataSet.Name).FeatureType != esriFeatureType.esriFTSimple)//排除注记等
                {
                    pDataSet = pED.Next();
                    continue;
                }
                //_geoPro = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                //_geoPro.OverwriteOutput = true;
                FCTS.Input_Features =pWorkSpace.PathName+"\\"+pDataSet.Name;
                _geoPro.Execute(FCTS, pTrackCancel);
                pDataSet = pED.Next();
            }

            
            Stream s=new System.IO.FileStream("d:\\qimport.txt",FileMode.Create);
            StreamWriter sw=new StreamWriter(s);
            for (int i = 0; i < _geoPro.MessageCount; i++)
            {
                sw.WriteLine(_geoPro.GetMessage(i));
 
            }
            sw.Close();
            s.Close();
           
        }

        //判断gp运行
        private bool RunTool(Geoprocessor geoprocessor, IGPProcess process)
        {
            // Set the overwrite output option to true
            geoprocessor.OverwriteOutput = true;

            try
            {
                geoprocessor.Execute(process, null);
                return true;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //QuickExportToDWG();
        }

        private void cmdSource_Click(object sender, EventArgs e)
        {
            if (rdoS2D.Checked)
            {
                btnShp2Dwg_Click(sender, e);
 
            }
            else if (rdoM2D.Checked)
            {
                btnMdb2dwg_Click(sender, e);

            }
            else
            {
                btnDwg2Shp_Click(sender, e);
 
            }
        }

        private void btnShp2Dwg_Click(object sender, EventArgs e)
        {
            lstLyrFile.Enabled = true;//源是ESRI格式，则列出图层可用
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.Description = "shp文件夹";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string str = folder.SelectedPath;
                this.txtSource.Text = str;
            }
            else
            {
                return;
            }

            IWorkspace pWks = GetWorkspace(this.txtSource.Text, 3);
            if (pWks == null)
            {
                this.txtSource.Text = "";
                this.lstLyrFile.Items.Clear();
                MessageBox.Show("不是shp的工作空间。");
                return;
            }

            

            m_OutType = OutType.shp2dwg;

            LstAllLyrFile(pWks);
        }

        private void btnGdb2dwg_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.Description = "选择FGDB文件";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string str = folder.SelectedPath;
                this.txtSource.Text = str;
            }
            else
            {
                return;
            }

            IWorkspace pWks = GetWorkspace(this.txtSource.Text, 2);
            if (pWks == null)
            {
                this.txtSource.Text = "";
                this.lstLyrFile.Items.Clear();
                MessageBox.Show("不是FGDB的工作空间。");
                return;
            }

         

            m_OutType = OutType.gdb2dwg;

            LstAllLyrFile(pWks);
        }

        private void btnMdb2dwg_Click(object sender, EventArgs e)
        {
            lstLyrFile.Enabled = true;//源是ESRI格式，则列出图层可用
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            filedialog.Title = "选择mdb文件";
            filedialog.Filter = "*.mdb|*.mdb";
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string str = filedialog.FileName;
                this.txtSource.Text = str;
            }
            else
            {
                return;
            }

            IWorkspace pWks = GetWorkspace(this.txtSource.Text, 1);
            if (pWks == null)
            {
                this.txtSource.Text = "";
                this.lstLyrFile.Items.Clear();
                MessageBox.Show("不是mdb的工作空间。");
                return;
            }

         
            m_OutType = OutType.mdb2dwg;
            LstAllLyrFile(pWks);
        }

        private IWorkspace GetWorkspace(string sFilePath, int wstype)
        {
            IWorkspace pWks = null;

            try
            {
                IPropertySet pPropSet = new PropertySetClass();
                switch (wstype)
                {
                    case 1:
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWks = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        break;
                    case 2:
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWks = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        break;
                    case 3:
                        IWorkspaceFactory shpWF = new ShapefileWorkspaceFactoryClass();
                        pWks = shpWF.OpenFromFile(sFilePath, 0);
                        shpWF = null;
                        break;
                }
                pPropSet = null;
                return pWks;
            }
            catch
            {
                return null;
            }
        }

        private void LstAllLyrFile(IWorkspace pWks)
        {
            this.lstLyrFile.Items.Clear();
            IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
            if (pFeaWks == null) return;

            IEnumDatasetName pEnumFeaCls = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            IDatasetName pFeaClsName = pEnumFeaCls.Next();
            while (pFeaClsName != null)
            {
                ListViewItem lvi = this.lstLyrFile.Items.Add(pFeaClsName.Name);
                if (m_OutType == OutType.shp2dwg)
                    lvi.Text += ".shp";//shp extention is need
                pFeaClsName = pEnumFeaCls.Next();
            }

            IEnumDatasetName pEnumDataNames = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName pDatasetName = pEnumDataNames.Next();
            while (pDatasetName != null)
            {
                IEnumDatasetName pSubNames = pDatasetName.SubsetNames;
                IDatasetName pSubName = pSubNames.Next();
                while (pSubName != null)
                {
                    this.lstLyrFile.Items.Add(pSubName.Name);
                    pSubName = pSubNames.Next();
                }

                pDatasetName = pEnumDataNames.Next();
            }

            for (int i = 0; i < this.lstLyrFile.Items.Count; i++)
            {
                this.lstLyrFile.Items[i].Checked = true;
            }
        }

        private void lstLyrFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in this.lstLyrFile.Items)
                {
                    item.Selected = true;
                }
            }
        }

        private void lstLyrFile_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // 以上述的排序选项设定来加以排序。
            this.lstLyrFile.Sort();
        }

        private void InitListViewStyle(ListView plv)
        {
            // 失焦点时不隐藏所选项
            plv.HideSelection = false;

            // Set the view to show details.
            plv.View = View.Details;
            // Allow the user to edit item text.
            plv.LabelEdit = false;
            // Allow the user to rearrange columns.
            plv.AllowColumnReorder = true;
            // Display check boxes.
            plv.CheckBoxes = true;   //_heluyuan_20071117_modify
            // Select the item and subitems when selection is made.
            plv.FullRowSelect = true;
            // Sort the items in the list in ascending order.
            plv.Sorting = SortOrder.Ascending;

            plv.Columns.Add(strColName, -2, HorizontalAlignment.Center);
            //plv.Columns.Add(strAliasName, -2, HorizontalAlignment.Left);
        }

        private void cmbTarget_Click(object sender, EventArgs e)
        {
            if (this.txtSource.Text == "")
            {
                MessageBox.Show("需要选择源数据");
                return;
            }

            SaveFileDialog savefile = new SaveFileDialog();
            if (m_OutType == OutType.gdb2dwg || m_OutType==OutType.mdb2dwg||m_OutType==OutType.shp2dwg)
            {
                savefile.Filter = "*.dwg|*.dwg";
            }
            else if (m_OutType == OutType.dwg2gdb)
            {
                savefile.Filter = "*.gdb|*.gdb";
                
            }
            else if (m_OutType == OutType.dwg2shp)
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                //folder.ShowNewFolderButton = false;
                folder.Description = "shp文件夹";
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    string str = folder.SelectedPath;
                    this.txtTarget.Text = str;
                }
                return;
            }
            else
            {
                return;
            }

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                string str = savefile.FileName;
                this.txtTarget.Text = str;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
   
        private void btnDwg2Shp_Click(object sender, EventArgs e)
        {
            lstLyrFile.Items.Clear();
            lstLyrFile.Enabled = false;//源是dwg，则列出图层不可用
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            filedialog.Title = "选择dwg文件";
            filedialog.Filter = "*.dwg|*.dwg";
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string str = filedialog.FileName;
                this.txtSource.Text = str;
                m_OutType = OutType.dwg2shp;
            }
            else
            {
                return;
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstLyrFile.Items.Count; i++)
            {
                ListViewItem lvi = lstLyrFile.Items[i]; 
                if (!lvi.Checked)
                    lvi.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstLyrFile.Items.Count; i++)
            {
                ListViewItem lvi = lstLyrFile.Items[i];
                if (lvi.Checked)
                    lvi.Checked = false;
                else
                    lvi.Checked = true;
            }
        }

        private void rdoS2D_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoS2D.Checked)
            {
                txtSource.Text = "";
                txtTarget.Text = "";
                lstLyrFile.Items.Clear();
            }
        }

        private void rdoM2D_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoM2D.Checked)
            {
                txtSource.Text = "";
                txtTarget.Text = "";
                lstLyrFile.Items.Clear();
            }

        }

        private void rdoD2S_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoD2S.Checked)
            {
                txtSource.Text = "";
                txtTarget.Text = "";
                lstLyrFile.Items.Clear();
            }

        }

    }
}