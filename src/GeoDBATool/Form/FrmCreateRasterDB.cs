using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;

namespace GeoDBATool
{
    /// <summary>
    /// 栅格数据建库
    /// </summary>
    public partial class FrmCreateRasterDB : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace pworkSpace = null;
        private Plugin.Application.IAppGISRef m_Hook = null;
        public FrmCreateRasterDB(Plugin.Application.IAppGISRef pHook)
        {
            InitializeComponent();

            m_Hook = pHook;
            cmbRasterType.Items.Clear();
            cmbRasterSpaRef.Items.Clear();
            cmbGeoSpaRef.Items.Clear();
            comBoxType.Items.Clear();
            cmbRasterPixeType.Items.Clear();

            //cmbRasterType.Items.AddRange(new object[] { "DOM", "DEM" });//"DLG",
            cmbRasterType.Items.AddRange(new object[] { "栅格编目","栅格数据集" });
            cmbRasterType.SelectedIndex = 0;
            object[] TagDBType = new object[] { "GDB", "SDE", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
            object[] spaRef = new object[] { "西安高斯117度(3度带)", "西安高斯120度(3度带)", "西安高斯123度(3度带)"};
            cmbRasterSpaRef.Items.AddRange(spaRef);
            //cmbRasterSpaRef.SelectedIndex = 0;
            cmbGeoSpaRef.Items.AddRange(spaRef);
            //cmbGeoSpaRef.SelectedIndex = 0;

            cmbResampleType.Items.Clear();
            cmbCompression.Items.Clear();

            cmbResampleType.Items.AddRange(new object[] { "邻近法", "双线性内插法", "立方卷积法" });
            cmbResampleType.SelectedIndex = 0;

            cmbCompression.Items.AddRange(new object[] { "LZ77", "JPEG", "JPEG2000", "PackBits", "LZW" });
            cmbCompression.SelectedIndex = 0;

            cmbRasterPixeType.Items.AddRange(new object[] { "PT_UCHAR", "PT_UNKNOWN", "PT_U1", "PT_U2", "PT_U4", "PT_CHAR", 
                "PT_USHORT", "PT_SHORT", "PT_ULONG", "PT_LONG","PT_FLOAT","PT_DOUBLE","PT_COMPLEX","PT_DCOMPLEX" });
            cmbRasterPixeType.SelectedIndex = 0;

            tileH.Text = "128";
            tileW.Text = "128";
            txtBand.Text = "1";

        }

        /// <summary>
        /// Get Raster Pixel Type
        /// </summary>
        /// <returns></returns>
        private rstPixelType GetPixelType()
        {
            rstPixelType pPixelType =rstPixelType.PT_UCHAR;
            switch(cmbRasterPixeType.Text.Trim())
            {     
                case "PT_UNKNOWN":
                    pPixelType = rstPixelType.PT_UNKNOWN;
                    break;
                case "PT_U1":
                    pPixelType = rstPixelType.PT_U1;
                    break;
                case "PT_U2":
                    pPixelType = rstPixelType.PT_U2;
                    break;
                case "PT_U4":
                    pPixelType = rstPixelType.PT_U4;
                    break;
                case "PT_UCHAR":
                    pPixelType = rstPixelType.PT_UCHAR;
                    break;
                case "PT_CHAR":
                    pPixelType = rstPixelType.PT_CHAR;
                    break;
                case "PT_USHORT":
                    pPixelType = rstPixelType.PT_USHORT;
                    break;
                case "PT_SHORT":
                    pPixelType = rstPixelType.PT_SHORT;
                    break;
                case "PT_ULONG":
                    pPixelType = rstPixelType.PT_ULONG;
                    break;
                case "PT_LONG":
                    pPixelType = rstPixelType.PT_LONG;
                    break;
                case "PT_FLOAT":
                    pPixelType = rstPixelType.PT_FLOAT;
                    break;
                case "PT_DOUBLE":
                    pPixelType = rstPixelType.PT_DOUBLE;
                    break;
                case "PT_COMPLEX":
                    pPixelType = rstPixelType.PT_COMPLEX;
                    break;
                case "PT_DCOMPLEX":
                    pPixelType = rstPixelType.PT_DCOMPLEX;
                    break;
                default:
                    break;
            }
            return pPixelType;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.OK;
            Exception err = null;
            #region 检查设置是否完备
            //服务器连接设置检查
            if (comBoxType.SelectedIndex == 1)
            {
                if (txtUser.Text.Length == 0 || txtPassWord.Text.Length == 0 )
                {
                    labelXErr.Text = "请完整设置SDE服务器访问参数！";
                    return;
                }
            }
            else
            {
                if (txtDataBase.Text.Length == 0)
                {
                    labelXErr.Text = "请完整设置本地数据库路径！";
                    return;
                }
            }

            //判断数据库是否已经存在
            if (comBoxType.Text.Trim().ToUpper() == "PDB")
            {
                if (File.Exists(txtDataBase.Text.Trim()))
                {
                    MessageBox.Show("数据库'" + txtDataBase.Text.Trim().Substring(txtDataBase.Text.Trim().LastIndexOf('\\') + 1) + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (comBoxType.Text.Trim().ToUpper() == "GDB")
            {
                if (Directory.Exists(txtDataBase.Text.Trim()))
                {
                    MessageBox.Show("数据库'" + txtDataBase.Text.Trim().Substring(txtDataBase.Text.Trim().LastIndexOf('\\') + 1) + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (rbdataset.Checked)//　cmbRasterType.Text.Trim() == "栅格数据集")
            {
                //栅格数据集设置检查
                if (cmbCompression.Text.Trim() == "")
                {
                    MessageBox.Show("请选择压缩类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbResampleType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择重采样类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    if (txtPyramid.Text.Trim() != "")
                    {
                        Convert.ToInt32(txtPyramid.Text.Trim());
                    }
                    
                    Convert.ToInt32(tileH.Text.Trim());
                    Convert.ToInt32(tileW.Text.Trim());
                    Convert.ToInt32(txtBand.Text.Trim());
                }
                catch (System.Exception ex)
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

                    MessageBox.Show("请填写有效的数字!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //labelXErr.Text = "请填写有效的数字！";
                    return;
                }
            }
            
            if (txtRasterName.Text=="")
            {
                labelXErr.Text = "请设置栅格编目名称或栅格数据集名称！";
                return;
            }
            
            #endregion


            #region 设置数据库连接
            //SysCommon.Gis.SysGisDataSet pSysDT = new SysCommon.Gis.SysGisDataSet();
            if (this.comBoxType.SelectedIndex == 2)    //PDB库体
            {
                //pSysDT.SetWorkspace(txtDataBase.Text.Trim(),SysCommon.enumWSType.PDB,out err);
                SetDestinationProp("PDB", txtDataBase.Text, "", "", "", "");
            }
            else if (this.comBoxType.SelectedIndex == 1)    //SDE库体
            {
                //pSysDT.SetWorkspace(txtServer.Text, txtInstance.Text, "", txtUser.Text, txtPassWord.Text, txtVersion.Text, out err);
                SetDestinationProp("SDE", txtServer.Text, txtInstance.Text, txtUser.Text, txtPassWord.Text, txtVersion.Text);
            }
            else if (this.comBoxType.SelectedIndex == 0)   //GDB库体
            {
                //pSysDT.SetWorkspace(txtDataBase.Text.Trim(), SysCommon.enumWSType.GDB, out err);
                SetDestinationProp("GDB", txtDataBase.Text, "", "", "", "");
            }
            if(err!=null)
            {
                MessageBox.Show("连接数据库出错！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //pworkSpace=pSysDT.WorkSpace;
            #endregion

            //创建库体
            //ISpatialReference pGeoSpaRef = GetSpatialRef(cmbGeoSpaRef.Text.Trim());
            //ISpatialReference pRasterSpaRef = GetSpatialRef(cmbRasterSpaRef.Text.Trim());

            //几何空间参考
            ISpatialReference pGeoSpaRef = GetSpatialRef(txtGeoSpati.Text.Trim(),out err);
            if(err!=null)
            {
                return;
            }
            //栅格空间参考
            ISpatialReference pRasterSpaRef = GetSpatialRef2(txtRasterSpati.Text.Trim(), out err);
            if (err != null)
            {
                return;
            }

            rstPixelType pPixelType = GetPixelType();
            //栅格数据工作空间
            IRasterWorkspaceEx pRasterWSEx = pworkSpace as IRasterWorkspaceEx;
            if(pRasterWSEx==null)
            {
                labelXErr.Text = "数据库连接出错！";
                return;
            }

            if (rbcatalog.Checked)
            {
                //首先判断栅格目录是否存在
                try
                {
                    IRasterCatalog tempRasterCatalog = pRasterWSEx.OpenRasterCatalog(txtRasterName.Text.Trim());
                    if(tempRasterCatalog!=null)
                    {
                        MessageBox.Show("栅格数据'" + txtRasterName.Text.Trim() + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (System.Exception ex)
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
                }
                //创建栅格目录
                //*********************************************************
                //guozheng added CreateCatalog Log
                List<string> Pra = new List<string>();
                Pra.Add(txtRasterName.Text.Trim());
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write("创建栅格目录", Pra, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write("创建栅格目录", Pra, DateTime.Now);
                }
                //*********************************************************
                //创建栅格目录
                CreateCatalog(pRasterWSEx, txtRasterName.Text.Trim(), "Raster", "Shape", pRasterSpaRef, pGeoSpaRef, "", true, out err);
            }
            else if (rbdataset.Checked)
            {
                //首先判断栅格目录是否存在
                try
                {
                    IRasterDataset tempRasterDataset = pRasterWSEx.OpenRasterDataset(txtRasterName.Text.Trim());
                    if (tempRasterDataset != null)
                    {
                        MessageBox.Show("栅格数据'" + txtRasterName.Text.Trim() + "'已经存在，请检查！","系统提示",MessageBoxButtons.OK ,MessageBoxIcon.Information );
                        return;
                    }
                }
                catch (System.Exception ex)
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
                }
                //创建栅格数据集
                //*********************************************************
                //guozheng added CreateRasterDataset Log
                List<string> Pra = new List<string>();
                Pra.Add(txtRasterName.Text.Trim());
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write("创建栅格数据集", Pra, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write("创建栅格数据集", Pra, DateTime.Now);
                }
                //*********************************************************
                //创建栅格数据集
                CreateRasterDataset(pRasterWSEx, txtRasterName.Text.Trim(), Convert.ToInt32(txtBand.Text.Trim()), pPixelType, pRasterSpaRef, pGeoSpaRef, null, null, "", out err);
            }  
            else 
            {
                MessageBox.Show("请选择栅格数据库的类型！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (err != null)
            {
                MessageBox.Show(err.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //labelXErr.Text = err.Message;
                return;
            }


            ///将现势库信息写入配置文件（res\\schema）
            ///
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            string pProjectname = pCurNode.Name;

            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            //设置存储类型,栅格目录，栅格数据集
            System.Xml.XmlElement DbTypeEle = ProjectNodeElement.SelectSingleNode(".//栅格数据库") as System.Xml.XmlElement;
            if (rbcatalog.Checked)
            {
                DbTypeEle.SetAttribute("存储类型", "栅格编目");
            }
            else if(rbdataset.Checked)
            {
                DbTypeEle.SetAttribute("存储类型", "栅格数据集");
            }

            System.Xml.XmlElement ProjectConnEle = ProjectNodeElement.SelectSingleNode(".//栅格数据库/连接信息") as System.Xml.XmlElement;

            ///设置数据库连接类型
            if (this.comBoxType.SelectedIndex == 2)
            {
                ProjectConnEle.SetAttribute("类型", "PDB");
                ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);

            }
            else if (this.comBoxType.SelectedIndex == 0)
            {
                ProjectConnEle.SetAttribute("类型", "GDB");
                ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
            }
            else if (this.comBoxType.SelectedIndex == 1)
            {
                ProjectConnEle.SetAttribute("类型", "SDE");
                ProjectConnEle.SetAttribute("服务器", txtServer.Text);
                ProjectConnEle.SetAttribute("服务名", txtInstance.Text);
                ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                ProjectConnEle.SetAttribute("用户", txtUser.Text);
                ProjectConnEle.SetAttribute("密码", txtPassWord.Text);
                ProjectConnEle.SetAttribute("版本", txtVersion.Text);
            }

            ///设置数据集名称
            ///
            System.Xml.XmlElement ProjectUserDSEle = ProjectConnEle.SelectSingleNode(".//库体") as System.Xml.XmlElement;
            ProjectUserDSEle.SetAttribute("名称", txtRasterName.Text.Trim());

            //设置栅格数据参数
            System.Xml.XmlElement rasterParaEle = ProjectNodeElement.SelectSingleNode(".//栅格数据库/参数设置") as System.Xml.XmlElement;
            rasterParaEle.SetAttribute("重采样类型", cmbResampleType.Text.Trim());
            rasterParaEle.SetAttribute("压缩类型",cmbCompression.Text.Trim());
            rasterParaEle.SetAttribute("金字塔", txtPyramid.Text.Trim());
            rasterParaEle.SetAttribute("瓦片高度", tileH.Text.Trim());
            rasterParaEle.SetAttribute("瓦片宽度",tileW.Text.Trim());
            rasterParaEle.SetAttribute("波段", txtBand.Text.Trim());


            m_Hook.DBXmlDocument.Save(ModData.v_projectXML);

            MessageBox.Show("创建成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbRasterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRasterType.Text == "栅格编目")
            {
                //txtBand.Enabled = false;
                groupBox2.Enabled = false;
            }
            else if (cmbRasterType.Text == "栅格数据集")
            {
                //txtBand.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
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
                        txtDataBase.Text = dir.FullName + "\\" + name + ".gdb";
                    }
                    break;

                case "PDB":
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Title = "保存为ESRI个人数据库";
                    saveFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = saveFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }
       
        /// <summary>
        /// 设置连接属性
        /// </summary>
        /// <param name="Type">数据库类型</param>
        /// <param name="IPoPath">数据库访问路径或服务器IP</param>
        /// <param name="Intance">sde服务实例</param>
        /// <param name="User">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Version">sde版本</param>
        /// <returns></returns>
        public bool SetDestinationProp(string Type, string IPoPath, string Intance, string User, string PassWord, string Version)
        {
            IWorkspace TempWorkSpace = null;                                 //工作空间
            IWorkspaceFactory pWorkspaceFactory = null;                      //工作空间工厂

            try
            {
                //初始化工作空间工厂
                if (Type == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (Type == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else if (Type == "SDE")
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();
                }
                ///如果创建的是本地库体，则首先判断库体是否存在
                ///如果库体存在，则先删除原有库体
                if (File.Exists(IPoPath))
                {
                    File.Delete(IPoPath);
                }

                if (Type == "SDE")  //如果是SDE则设置sde工作空间连接信息
                {
                    IPropertySet propertySet = new PropertySetClass();
                    propertySet.SetProperty("SERVER", IPoPath);
                    propertySet.SetProperty("INSTANCE", Intance);
                    //propertySet.SetProperty("DATABASE", ""); 
                    propertySet.SetProperty("USER", User);
                    propertySet.SetProperty("PASSWORD", PassWord);
                    propertySet.SetProperty("VERSION", Version);
                    TempWorkSpace = pWorkspaceFactory.Open(propertySet, 0);
                }
                else  //如果不是sde则创建工作空间
                {
                    FileInfo finfo = new FileInfo(IPoPath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    if (outputDBName.EndsWith(".gdb"))
                    {
                        outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
                    }
                    IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                    ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
                    TempWorkSpace = (IWorkspace)pName.Open();
                }

                //判断获取工作空间是否成功
                if (TempWorkSpace != null)
                {
                    pworkSpace = TempWorkSpace;                //工作空间赋值
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
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
                return false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDataBase.Text = "";
            txtUser.Text = "";
            txtPassWord.Text = "";
            if (comBoxType.Text != "SDE")
            {
                btnServer.Visible = true;
                txtDataBase.Size = new Size(txtServer.Size.Width - btnServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnServer.Visible = false;
                txtDataBase.Size = new Size(txtServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                txtVersion.Enabled = true;

            }
        }


        /// <summary>
        /// 在Geodatabase中创建栅格数据编目
        /// </summary>
        /// <param name="pRasterWSEx">目标Geodatabase工作区</param>
        /// <param name="pCatalogName">栅格编目的名称</param>
        /// <param name="pRasterFielsName">栅格列的名称</param>
        /// <param name="pShapeFieldName">几何要素列名称(Shape)</param>
        /// <param name="pRasterSpatialRef">几何要素列空间参考</param>
        /// <param name="pGeoSpatialRef">栅格列空间参考</param>
        /// <param name="pKeyword"> 栅格编目表的字段</param>
        /// <param name="eError">ArcSDE 适用, 表示configuration keyword</param>
        /// <returns></returns>
        private IRasterCatalog CreateCatalog(IRasterWorkspaceEx pRasterWSEx, string pCatalogName, string pRasterFielsName, string pShapeFieldName, ISpatialReference pRasterSpatialRef, ISpatialReference pGeoSpatialRef, string pKeyword, bool ismanaged, out Exception eError)
        {
            eError = null;
            IRasterCatalog pRasterCat = null;
            try
            {
                #region 创建字段
                IFields pFields = new FieldsClass();
                IFieldsEdit pFeildsEdit = pFields as IFieldsEdit;
                IField pField = null;

                pField = CreateCommonField("Name", esriFieldType.esriFieldTypeString);
                if (pField == null)
                {
                    eError = new Exception("创建'name'字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);

                IField2 pField2 = CreateRasterField(pRasterFielsName, pRasterSpatialRef, ismanaged);
                if (pField == null)
                {
                    eError = new Exception("创建栅格字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField2);
                pField = CreateShapeField(pShapeFieldName, pGeoSpatialRef);
                if (pField == null)
                {
                    eError = new Exception("创建几何字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = CreateOIDField("OBJECTID");
                if (pField == null)
                {
                    eError = new Exception("创建OID字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = null;
                if (pKeyword.Trim() == "")
                {
                    pKeyword = "defaults";
                }
                pFields = pFeildsEdit as IFields;

                //创建用户自定义字段
                #endregion

                pRasterCat = pRasterWSEx.CreateRasterCatalog(pCatalogName, pFields, pShapeFieldName, pRasterFielsName, pKeyword);

                return pRasterCat;

            }
            catch (System.Exception ex)
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

                eError = new Exception("创建栅格编目出错！\n" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 创建栅格字段
        /// </summary>
        /// <param name="pRasterFielsName">栅格字段名</param>
        /// <param name="pSpatialRes">栅格空间参考</param>
        /// <param name="eError"></param>
        /// <returns>返回字段</returns>
        private IField2 CreateRasterField(string pRasterFielsName, ISpatialReference pSpatialRes, bool isManaged)
        {
            IField2 pField = new FieldClass();
            IFieldEdit2 pFieldEdit = pField as IFieldEdit2;
            pFieldEdit.Name_2 = pRasterFielsName;
            pFieldEdit.AliasName_2 = pRasterFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "this is Raster catalog";
            if (pSpatialRes == null)
            {
                //如果空间参考为空，则设置为UnknownCoordinateSystemClass
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            //only for PGDB
            pRasterDef.IsManaged = isManaged;
            pRasterDef.SpatialReference = pSpatialRes;
            pFieldEdit.RasterDef = pRasterDef;
            pField = pFieldEdit as IField2;

            return pField;
        }

        /// <summary>
        /// 创建shape字段
        /// </summary>
        /// <param name="pShapeFielsName">shape字段名</param>
        /// <param name="pSpatialRes">空间参考</param>
        /// <returns>返回字段</returns>
        private IField CreateShapeField(string pShapeFielsName, ISpatialReference pSpatialRes)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pShapeFielsName;
            pFieldEdit.AliasName_2 = pShapeFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDef pGeoDef = new GeometryDefClass();
            pGeoDef = CreateGeoDef(pSpatialRes);
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 创建OID字段
        /// </summary>
        /// <param name="pOIDFieldName">OID字段名</param>
        /// <returns></returns>
        private IField CreateOIDField(string pOIDFieldName)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pOIDFieldName;
            pFieldEdit.AliasName_2 = pOIDFieldName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 创建用户自定义字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private IField CreateCommonField(string fieldName, esriFieldType fieldType)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = fieldName;
            pFieldEdit.AliasName_2 = fieldName;
            pFieldEdit.Type_2 = fieldType;
            pFieldEdit.Length_2 = 50;
            pField = pFieldEdit as IField;
            return pField;
        }

       
        /// <summary>
        /// 创建几何空间参考
        /// </summary>
        /// <param name="LoadPath">空间参考文件</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public ISpatialReference GetSpatialRef(string LoadPath, out Exception eError)
        {
            eError = null;
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    //eError = new Exception("空间参考文件不存在！");
                    return null;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                pSRR.SetDefaultXYResolution();
                pSRT.SetDefaultXYTolerance();
                return pSR;
            }
            catch(Exception ex)
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

                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 创建栅格空间参考
        /// </summary>
        /// <param name="LoadPath">空间参考文件</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public ISpatialReference GetSpatialRef2(string LoadPath, out Exception eError)
        {
            eError = null;
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    //eError = new Exception("空间参考文件不存在！");
                    return null;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                //ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                //ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                //IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                //pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                //pSRR.SetDefaultXYResolution();
                //pSRT.SetDefaultXYTolerance();
                return pSR;
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

                eError = ex;
                return null;
            }
        }



        /// <summary>
        /// 在Geodatabase中创建栅格数据集
        /// </summary>
        /// <param name="pRasterWsEx">目标Geodatabase工作区</param>
        /// <param name="pDsName">栅格数据集名称</param>
        /// <param name="iNBannd">波段数</param>
        /// <param name="iPixeType">像素类型</param>
        /// <param name="pSpaRef">空间参考</param>
        /// <param name="pRasterStoreRef">存储栅格参数定义</param>
        /// <param name="pRasterDef">栅格空间参考定义</param>
        /// <param name="pKeyword"></param>
        /// <param name="eex"></param>
        /// <returns></returns>
        private IRasterDataset CreateRasterDataset(IRasterWorkspaceEx pRasterWsEx, string pDsName, int iNBannd, rstPixelType iPixeType, ISpatialReference pRasterSpaRef,ISpatialReference pGeoSpaRef, IRasterStorageDef pRasterStoreRef, IRasterDef pRasterDef, string pKeyword, out Exception eex)
        {
            eex = null;
            IRasterDataset pRasterDs = null;
            try
            {
                IGeometryDef pGeoDef = null;
                if (pRasterDef == null)
                {
                    pRasterDef = CreateRasterDef(pRasterSpaRef);
                }
                if (pRasterStoreRef == null)
                {
                    pRasterStoreRef = CreaterRasterStoreDef();
                }
                pGeoDef = CreateGeoDef(pGeoSpaRef);     
                if (pKeyword.Trim() == "")
                {
                    pKeyword = "DEFAULTS";
                }
                pRasterDs = pRasterWsEx.CreateRasterDataset(pDsName,iNBannd,iPixeType, pRasterStoreRef, pKeyword, pRasterDef, pGeoDef);
            }
            catch(Exception ex)
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

                eex =new Exception("创建栅格数据集出错！\n"+ex.Message) ;
            }
            return pRasterDs;
        }

        /// <summary>
        /// 设置空间参考定义
        /// </summary>
        /// <param name="pSR"></param>
        /// <returns></returns>
        private IRasterDef CreateRasterDef(ISpatialReference pSR)
        {
            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "rasterDataset";
            if(pSR==null)
            {
                pSR = new UnknownCoordinateSystemClass();
            }
            pRasterDef.SpatialReference = pSR;
            return pRasterDef;
        }
        
        /// <summary>
        /// 设置栅格存储的参数
        /// </summary>
        /// <returns></returns>
        private IRasterStorageDef CreaterRasterStoreDef()
        {
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = GetCompression();
            pRasterStorageDef.PyramidResampleType = GetResampleTpe();
            if (txtPyramid.Text.Trim() != "")
            {
                pRasterStorageDef.PyramidLevel = Convert.ToInt32(txtPyramid.Text.Trim());
            }
            pRasterStorageDef.TileHeight = Convert.ToInt32(tileH.Text.Trim());
            pRasterStorageDef.TileWidth = Convert.ToInt32(tileW.Text.Trim());
            return pRasterStorageDef;
        }

        /// <summary>
        /// 设置几何空间参考定义
        /// </summary>
        /// <param name="pSpatialRes"></param>
        /// <returns></returns>
        private IGeometryDef CreateGeoDef(ISpatialReference pSpatialRes)
        {
            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;

            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoDefEdit.AvgNumPoints_2 = 4;
            pGeoDefEdit.GridCount_2 = 1;
            pGeoDefEdit.set_GridSize(0, 1000);

            if (pSpatialRes == null)
            {
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            pGeoDefEdit.SpatialReference_2 = pSpatialRes;
            pGeoDef = pGeoDefEdit as IGeometryDef;
            return pGeoDef;
        }

        /// <summary>
        /// 获得重采样类型
        /// </summary>
        /// <returns></returns>
        private rstResamplingTypes GetResampleTpe()
        {
            rstResamplingTypes resampleType = rstResamplingTypes.RSP_NearestNeighbor;
            switch (cmbResampleType.Text.Trim())
            {
                case "邻近法":
                    resampleType = rstResamplingTypes.RSP_NearestNeighbor;
                    break;
                case "双线性内插法":
                    resampleType = rstResamplingTypes.RSP_BilinearInterpolation;
                    break;
                case "立方卷积法":
                    resampleType = rstResamplingTypes.RSP_CubicConvolution;
                    break;
            }
            return resampleType;
        }

        /// <summary>
        /// 获得压缩类型
        /// </summary>
        /// <returns></returns>
        private esriRasterCompressionType GetCompression()
        {
            esriRasterCompressionType compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
            switch (cmbCompression.Text.Trim())
            {
                case "LZ77":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                    break;
                case "JPEG":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG;
                    break;
                case "JPEG2000":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG2000;
                    break;
                case "PackBits":
                    compressionType = esriRasterCompressionType.esriRasterCompressionPackBits;
                    break;
                case "LZW":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZW;
                    break;
            }
            return compressionType;
        }


        /// <summary>
        /// 创建栅格数据集，本地
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private IRasterDataset CreateFileRasterDataset(string directoryName, string fileName)
        {
            // This function creates a new img file in the given workspace
            // and then assigns pixel values
            try
            {
                IRasterDataset rasterDataset = null;
                IPoint originPoint = new PointClass();
                originPoint.PutCoords(0, 0);

                // Create the dataset
                IRasterWorkspace2 rasterWorkspace2 = null;
                rasterWorkspace2 = CreateRasterWorkspace(directoryName);

                rasterDataset = rasterWorkspace2.CreateRasterDataset(fileName, "IMAGINE Image", originPoint, 200, 100, 1, 1, 1, rstPixelType.PT_UCHAR, new UnknownCoordinateSystemClass(), true);

                IRawPixels rawPixels = null;
                IPixelBlock3 pixelBlock3 = null;
                IPnt pixelBlockOrigin = null;
                IPnt pixelBlockSize = null;
                IRasterBandCollection rasterBandCollection;
                IRasterProps rasterProps;



                // QI for IRawPixels and IRasterProps
                rasterBandCollection = (IRasterBandCollection)rasterDataset;
                rawPixels = (IRawPixels)rasterBandCollection.Item(0);
                rasterProps = (IRasterProps)rawPixels;



                // Create pixelblock
                pixelBlockOrigin = new DblPntClass();
                pixelBlockOrigin.SetCoords(0, 0);

                pixelBlockSize = new DblPntClass();
                pixelBlockSize.SetCoords(rasterProps.Width, rasterProps.Height);

                pixelBlock3 = (IPixelBlock3)rawPixels.CreatePixelBlock(pixelBlockSize);



                // Read pixelblock
                rawPixels.Read(pixelBlockOrigin, (IPixelBlock)pixelBlock3);

                // Get pixeldata array
                System.Object[,] pixelData;
                pixelData = (System.Object[,])pixelBlock3.get_PixelDataByRef(0);

                // Loop through all the pixels and assign value
                for (int i = 0; i < rasterProps.Width; i++)
                    for (int j = 0; j < rasterProps.Height; j++)
                        pixelData[i, j] = (i * j) % 255;



                // Write the pixeldata back
                System.Object cachePointer;

                cachePointer = rawPixels.AcquireCache();

                rawPixels.Write(pixelBlockOrigin, (IPixelBlock)pixelBlock3);

                rawPixels.ReturnCache(cachePointer);

                // Return raster dataset
                return rasterDataset;
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

                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 在geodatabase中创建栅格工作空间
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private IRasterWorkspace2 CreateRasterWorkspace(string pathName)
        {
            // Create RasterWorkspace
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();

            return workspaceFactory.OpenFromFile(pathName, 0) as IRasterWorkspace2;
        }

        private void btnRuleFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择库体配置文件";
            OpenFile.Filter = "库体配置文件(*.mdb)|*.mdb|库体配置文件(*.gosch)|*.gosch";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textRuleFilePath.Text = OpenFile.FileName;
                btnRuleFile.Tooltip = OpenFile.FileName;
            }
        }

        private void rbdataset_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdataset.Checked)
            {
                groupBox2.Enabled = true;
            }
        }

        private void rbcatalog_CheckedChanged(object sender, EventArgs e)
        {
            if (rbcatalog.Checked)
            {
                groupBox2.Enabled = false;
            }
        }

        private void btnGeoSpati_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtGeoSpati.Text = OpenFile.FileName;
                btnGeoSpati.Tooltip = OpenFile.FileName;
            }
        }

        private void btnRasterSpati_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtRasterSpati.Text = OpenFile.FileName;
                btnRasterSpati.Tooltip = OpenFile.FileName;
            }
        }


        /// <summary>
        /// 获得空间参考
        /// </summary>
        /// <param name="spatialStr">空间参考参数</param>
        /// <returns></returns>
        private ISpatialReference GetSpatialRef(string spatialStr)
        {
            ISpatialReference pSpaRef = null;
            ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
            switch (spatialStr)
            {
                case "西安高斯117度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_117E) as ISpatialReference;
                    break;
                case "西安高斯120度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_120E) as ISpatialReference;
                    break;
                case "西安高斯123度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_123E) as ISpatialReference;
                    break;

            }
            return pSpaRef;
        }

        private void btnAddList_Click(object sender, EventArgs e)
        {
            if (pListViewDT.Items.Count != 0)
            {
                //清空历史数据
                pListViewDT.Items.Clear();
            }

            //添加数据
            IEnumDatasetName pEnumRasterName = null;
            IDatasetName pDT = null;
            ListViewItem pListViewItem = null;
            //添加栅格编目图层

            pEnumRasterName = pworkSpace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            if (pEnumRasterName == null) return;
            pDT = pEnumRasterName.Next();
            while (pDT != null)
            {
                //将查到的结果，添加在列表中
                pListViewItem = new ListViewItem();
                pListViewItem.Name = pDT.Name;
                pListViewItem.Text = pDT.Name;
                pListViewItem.Tag = pDT;
                pListViewDT.Items.Add(pListViewItem);
                pDT = pEnumRasterName.Next();
            }
        }

    }
}