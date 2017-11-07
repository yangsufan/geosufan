using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoUtilities
{
    public partial class frmGeocorTrans : DevComponents.DotNetBar.Office2007Form
    {
        private string strColName = "名称";
        private string m_strInPrj = "";
        private string m_strOutPrj = "";
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
        public frmGeocorTrans()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (!CheckPrj()) return;

            string strSourceExtension=System.IO.Path.GetExtension(this.txtSource.Text);
            string strExtension=System.IO.Path.GetExtension(this.txtGdbFile.Text);
            //if ((strExtension == ".gdb" || strExtension==".mdb") && (strSourceExtension!=".gdb" && strSourceExtension!=".mdb"))
            //{
            // MessageBox.Show("输出精度大于输入精度，无法进行转换。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //   return;
           // }

            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor; 
                //首先创建转换
                this.lblTips.Text = "创建转换...";
                //GetTns();     //投影转换不需创建本地环境  deleted by chulili 2013-01-05

                //创建输出空间
                this.lblTips.Text = "创建输出数据库...";

                if (System.IO.Path.GetExtension(this.txtGdbFile.Text) == ".gdb")
                {
                    CreateFileGdb();
                }
                else if (System.IO.Path.GetExtension(this.txtGdbFile.Text)==".mdb")
                {
                    CreatePGdb();
                }
                else
                {
                    System.IO.Directory.CreateDirectory(this.txtGdbFile.Text);
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("将选择的数据进行投影转换");
                    Plugin.LogTable.Writelog("源数据:" + txtSource.Text + ",目标数据:" + txtGdbFile.Text);
                    Plugin.LogTable.Writelog("源空间参考:" + m_strInPrj.Split(',')[0] + ",目标空间参考:" + m_strOutPrj.Split(',')[0]);
                }
                //进行坐标转换
                string strMessage = "";
                ProjectClass(out strMessage);

                this.lblTips.Text = "投影转换完成";
                this.progressBarX1.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("投影转换完成");
                }

                MessageBox.Show("投影转换完成。\r\n" + strMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.lblTips.Text = "转换过程出现错误";
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.progressBarX1.Visible = false;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("转换过程出现错误(" + ex.Message + "),已终止转换");
                }
                MessageBox.Show("转换过程出现错误。"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        //检查是否符合转换的条件 主要是界面上的
        private bool CheckPrj()
        {
            if (this.m_strOutPrj == "" || this.m_strInPrj == "")
            {
                MessageBox.Show("输入或输出的空间参考为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            int intIn = 0;
            for (int i = 0; i < this.lstLyrFile.Items.Count; i++)
            {
                if (this.lstLyrFile.Items[i].Checked) intIn++;
            }
            if (intIn < 1)
            {
                MessageBox.Show("需要选择进行投影转换的图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
          

            // 显示进度条 放在这里了
            this.progressBarX1.Maximum = intIn;
            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Value = 0;
            this.progressBarX1.Step = 1;
            this.progressBarX1.Visible = true;

            return true;
        }

        
        private void frmGeoTrans_Load(object sender, EventArgs e)
        {

            InitListViewStyle(this.lstLyrFile);
        }


        private void btnSource_Click(object sender, EventArgs e)
        {
            if (rdoGDB.Checked)
                btnGDB_Click(sender, e);
            else if (rdoMDB.Checked)
                btnMDB_Click(sender, e);
            else
                btnSHP_Click(sender, e);//yjl20110822 modify
        }

        private void LstAllLyrFile(IWorkspace pWks)
		 {
            try
        {
            IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
            if (pFeaWks == null) return;

            ESRI.ArcGIS.Geometry.ISpatialReference pSpa = null;

            IEnumDatasetName pEnumFeaCls = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            IDatasetName pFeaClsName = pEnumFeaCls.Next();
            while (pFeaClsName != null)
            {
                if (pSpa == null)
                {
                    IName pPrjName = pFeaClsName as IName;
                    IFeatureClass pFeaCls = pPrjName.Open() as IFeatureClass;
                    IGeoDataset pGeodataset = pFeaCls as IGeoDataset;

                    //获得空间参看 源的
                    pSpa = pGeodataset.SpatialReference;
                    m_strInPrj = ExportToESRISpatialReference(pSpa);
                }

                this.lstLyrFile.Items.Add(pFeaClsName.Name);
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
                    if (pSpa == null)
                    {
                        IName pPrjName = pSubName as IName;
                        IFeatureClass pFeaCls = pPrjName.Open() as IFeatureClass;
                        IGeoDataset pGeodataset = pFeaCls as IGeoDataset;

                        //获得空间参看 源的
                        pSpa = pGeodataset.SpatialReference;
                        m_strInPrj = ExportToESRISpatialReference(pSpa);
                    }

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
            catch
            {
            }
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

        private void lstLyrFile_MouseDown(object sender, MouseEventArgs e)
        {

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

        private void btnGdbFile_Click(object sender, EventArgs e)
        {
            OutFile();
        }

        private void OutFile()
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "*.gdb|*.gdb|*.mdb|*.mdb|*.shp|*.shp";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                string str = savefile.FileName;
                if (System.IO.Path.GetExtension(str) == ".shp")
                {
                    this.txtGdbFile.Text = str.Substring(0,str.Length-4);
                    return;
                }

                this.txtGdbFile.Text = str;
            }
        }

        private void btnPrjFile_Click(object sender, EventArgs e)
        {
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            sInstall = sInstall + "\\Coordinate Systems";

            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            filedialog.InitialDirectory = sInstall;
            filedialog.Filter = "*.prj|*.prj";
            filedialog.Title = "选择prj文件";
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string str = filedialog.FileName;
                this.txtPrjFile.Text = str;

                //直接读取坐标系统
				try
                {
                ESRI.ArcGIS.Geometry.ISpatialReferenceFactory pPrjFac = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass();
                ESRI.ArcGIS.Geometry.ISpatialReference pSpa=pPrjFac.CreateESRISpatialReferenceFromPRJFile(str);

                m_strOutPrj = ExportToESRISpatialReference(pSpa);
				 }
                catch
                {
                    
                }
            }
        }

        public string ExportToESRISpatialReference(ISpatialReference spatialReference)
        {
            int bytes = 0;
            string buffer = null;
            ISpatialReference projectedCoordinateSystem = spatialReference;
            IESRISpatialReferenceGEN parameterExport = projectedCoordinateSystem as IESRISpatialReferenceGEN;
            parameterExport.ExportToESRISpatialReference(out buffer,out bytes);

            return buffer;
        }

        //执行转换的gp
        private IVariantArray GetTns()
        {
            IVariantArray pTempArray = new VarArrayClass();
      

            ESRI.ArcGIS.DataManagementTools.CreateCustomGeoTransformation tns = new ESRI.ArcGIS.DataManagementTools.CreateCustomGeoTransformation();
            //tns.custom_geot = strMethod;
            tns.geot_name = "temp";
            tns.in_coor_system = m_strInPrj;
            tns.out_coor_system = m_strOutPrj;

            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, tns))
            {
                return null;
            }

            return null;
        }

    
        private double GetTnsValue(string strName, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string strAtt = dr["tnsName"].ToString();
                if (strAtt.ToUpper() == strName.ToUpper())
                {
                    object obj = dr["tnsValue"];
                    if (obj == null) return 0;

                    double dbltemp = 0;
                    double.TryParse(obj.ToString(), out dbltemp);
                    return dbltemp;
                }
            }

            return 0;
        }


        //创建输出的gdb
        private void CreateFileGdb()
        {
            string strfileName = this.txtGdbFile.Text;
            if (strfileName == "") return;

            string strPath = System.IO.Path.GetDirectoryName(strfileName);
            string strName = System.IO.Path.GetFileName(strfileName);

            ESRI.ArcGIS.DataManagementTools.CreateFileGDB creategdb = new ESRI.ArcGIS.DataManagementTools.CreateFileGDB();
            creategdb.out_folder_path = strPath;
            creategdb.out_name = strName;

            //
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, creategdb))
            {
                return;
            }

        }

        //创建输出的gdb
        private void CreatePGdb()
        {
            string strfileName = this.txtGdbFile.Text;
            if (strfileName == "") return;

            string strPath = System.IO.Path.GetDirectoryName(strfileName);
            string strName = System.IO.Path.GetFileNameWithoutExtension(strfileName);

            ESRI.ArcGIS.DataManagementTools.CreatePersonalGDB creategdb = new ESRI.ArcGIS.DataManagementTools.CreatePersonalGDB();
            creategdb.out_folder_path = strPath;
            creategdb.out_name = strName;

            //
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, creategdb))
            {
                return;
            }

        }

        //做投影变换
        private void ProjectClass(out string strRes)
        {
            strRes = "";

            if (this.lstLyrFile.Items.Count < 0) return ;
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            for (int i = 0; i < this.lstLyrFile.Items.Count; i++)
            {
                if (!this.lstLyrFile.Items[i].Checked) continue;
                string strFeaName = this.lstLyrFile.Items[i].Text;
                this.lblTips.Text = "正在进行投影转换：" + strFeaName;

                ESRI.ArcGIS.DataManagementTools.Project prjtool = new ESRI.ArcGIS.DataManagementTools.Project();
                prjtool.in_dataset = this.txtSource.Text + "\\"+ strFeaName;
                if (rdoSHP.Checked && !prjtool.in_dataset.ToString().EndsWith(".shp"))
                {
                    prjtool.in_dataset += ".shp";//shp文件入口需加后缀名
                }
                prjtool.out_dataset = this.txtGdbFile.Text + "\\" + strFeaName;
                prjtool.out_coor_system = m_strOutPrj;
                //
                

                bool bres=RunTool(gp, prjtool); 

                if (!bres)
                {
                    strRes = strRes +"'"+ strFeaName+"'转换失败\r\n";
                }
                else
                {
                    strRes = strRes +"'"+ strFeaName + "'转换成功\r\n";
                }

                prjtool = null;
                
                this.progressBarX1.PerformStep();
                
            }
            gp = null;
            

        }

        //gp的执行
        private void ExecuteGP(IVariantArray parameters, string strToolName)
        {
            ITrackCancel pTrackCancel = null;

            //找到tool
            ESRI.ArcGIS.Geoprocessing.GeoProcessor _geoPro = new GeoProcessor();
            pTrackCancel = new TrackCancelClass();
            IVariantArray pVArray = new VarArrayClass();

            IGPEnvironmentManager pgpEnv = new GPEnvironmentManager();
            IGPMessages pGpMessages; //= _geoPro.Validate(parameters, false, pgpEnv);
            IGPComHelper pGPCOMHelper = new GpDispatch();

            //这里是关键，如果不赋值的话，那么就会报错
            IGPEnvironmentManager pEnvMgr = pGPCOMHelper.EnvironmentManager;
            pgpEnv.PersistAll = true;
            pGpMessages = new GPMessagesClass();

            // Execute the model tool by name.
            _geoPro.Execute(strToolName, parameters, pTrackCancel);
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

        // Function for returning the tool messages.
        private void ReturnMessages(Geoprocessor gp)
        {
            string ms = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    ms += gp.GetMessage(Count);
                }
            }
        }


        private string ReadRegistry(string p)
        {
            /// <summary> 
            /// 从注册表中取得指定软件的路径 

            /// </summary> 

            /// <param name="sKey"> </param> 

            /// <returns> </returns> 

            //Open the subkey for reading 

            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(p, true);

            if (rk == null) return "";

            // Get the data from a specified item in the key. 

            return (string)rk.GetValue("InstallDir");
        }

        private void btnGDB_Click(object sender, EventArgs e)
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
                MessageBox.Show("不是FGDB的工作空间。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

			this.lstLyrFile.Items.Clear();
            LstAllLyrFile(pWks);
        }

        private void btnMDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.mdb|*.mdb";
            openFile.Title = "选择mdb文件";
            openFile.Multiselect = false;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string str = openFile.FileName;
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
                MessageBox.Show("不是PGDB的工作空间。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
			this.lstLyrFile.Items.Clear();

            LstAllLyrFile(pWks);
        }

        private void btnSHP_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "*.shp|*.shp";
            openFile.Title = "选择shp文件";
            openFile.Multiselect = true ;

            ESRI.ArcGIS.Geometry.ISpatialReference pSpa = null;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string[] strs = openFile.FileNames;
                this.lstLyrFile.Items.Clear();
                for (int i = 0; i < strs.GetLength(0); i++)
                {
                    string str = strs[i];
                    this.lstLyrFile.Items.Add(System.IO.Path.GetFileNameWithoutExtension(str));

                    //获得一个源数据的空间参考
                    //获得空间参看 源的

                    if (pSpa == null)
                    {
                        IWorkspaceFactory pFac = new ShapefileWorkspaceFactory();
                        IWorkspace pWks= pFac.OpenFromFile(System.IO.Path.GetDirectoryName(str), 0);
                        if (pWks == null) continue;
                        IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
                        IFeatureClass pFeaCls = pFeaWks.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(str));
                        if (pFeaCls == null) continue;
                        IGeoDataset pGeoData = pFeaCls as IGeoDataset;

                        pSpa = pGeoData.SpatialReference;
                        if (pSpa == null) continue;
                        m_strInPrj = ExportToESRISpatialReference(pSpa);
                        pFeaWks = null;
                    }
                }

                for (int i = 0; i < this.lstLyrFile.Items.Count; i++)
                {
                    this.lstLyrFile.Items[i].Checked = true;
                }

                this.txtSource.Text = System.IO.Path.GetDirectoryName(strs[0]);
            }
        }

        private void btnTAGGDB_Click(object sender, EventArgs e)
        {

        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstLyrFile.Items)
            {
                lvi.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstLyrFile.Items)
            {
                if (!lvi.Checked)
                    lvi.Checked = true;
                else
                    lvi.Checked = false;
            }
        }

    }
}