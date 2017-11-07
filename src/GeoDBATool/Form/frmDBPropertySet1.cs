using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDBATool
{
    /// <summary>
    /// 加载SDE数据,陈亚飞添加 修改20101123
    /// </summary>
    public partial class frmDBPropertySet1 :DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_WS = null;
        public IWorkspace OBJWS
        {
            get
            {
                return m_WS;
            }
            set
            {
                m_WS = value;
            }
        }
        string m_DBName = "";
        //IHookHelper m_HookHelp = null;
        Plugin.Application.IAppGISRef m_Hook=null;
        EnumFeatureType m_FeaType;

        public frmDBPropertySet1(Plugin.Application.IAppGISRef pHook, EnumFeatureType pFeaType)
        {
            InitializeComponent();
            m_Hook = pHook;
            if (m_Hook == null) return;
            //if (m_HookHelp == null) return;
            m_FeaType = pFeaType;

            InitialFrm(pFeaType);
            //if (pFeaType == EnumFeatureType.更新要素)
            //{
            //    ModData.m_ComboBox.Items.Clear();
            //    ModData.m_ComboBox.Update();
            //}
        }

        //连接测试
        private void btnTest_Click(object sender, EventArgs e)
        {
            Exception err = null;
            cmbDataset.Items.Clear();
            //连接数据库
            try
            {
                switch (comBoxType.Text)
                {
                    case "GDB":
                        if (txtDB.Text.Trim() == "")
                        {
                            MessageBox.Show("请选择数据库", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        m_WS = SetWorkspace(txtDB.Text, "GDB", out err);
                        break;
                    case "SDE":
                        if (txtServer.Text.Trim() == "" || txtInstance.Text.Trim() == "" || txtUser.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtVersion.Text.Trim() == "")
                        {
                            MessageBox.Show("请填写完整的SDE参数！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        m_WS = SetWorkspace(txtServer.Text, txtInstance.Text, "", txtUser.Text, txtPassword.Text, txtVersion.Text, out err);
                        break;
                    case "PDB":
                        if (txtDB.Text.Trim() == "")
                        {
                            MessageBox.Show("请选择数据库", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        m_WS = SetWorkspace(txtDB.Text, "PDB", out err);
                        break;
                }
                if (err != null)
                {
                    MessageBox.Show("连接数据库失败,原因:" + err.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception eex)
            {
                MessageBox.Show("连接数据库失败,原因:" + eex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           //获得数据集
            Dictionary<string, IFeatureDataset> dicFeaDtInfo = new Dictionary<string, IFeatureDataset>();
            IEnumDataset pEnumDt = m_WS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pEnumDt.Reset();
            IDataset pDt = pEnumDt.Next();
            //遍历数据集，将数据集加载在comboBox中
            while (pDt != null)
            {
                string pFeaDtName = pDt.Name;
                if (m_WS.WorkspaceFactory.WorkspaceType == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    if (!pFeaDtName.ToUpper().Contains(txtUser.Text.Trim().ToUpper()))
                    {
                        pDt = pEnumDt.Next();
                        continue;
                    }
                }
                IFeatureDataset pFeaDt=pDt as IFeatureDataset;
                if (!dicFeaDtInfo.ContainsKey(pFeaDtName))
                {
                    dicFeaDtInfo.Add(pFeaDtName, pFeaDt);
                    cmbDataset.Items.Add(pFeaDtName);
                }
                pDt = pEnumDt.Next();
            }
            if (cmbDataset.Items.Count > 0)
            {
                cmbDataset.SelectedIndex = 0;
                cmbDataset.Tag = dicFeaDtInfo;
            }
            //if (m_FeaType == EnumFeatureType.更新要素)
            //{
            //    if (m_DBName != "")
            //    {
            //        cmbDataset.Text = m_DBName;
            //    }
            //}
            btnOK.Enabled = true;
        }

        //确定
        private void btnOK_Click(object sender, EventArgs e)
        {
            //清空数据
            if (m_FeaType == EnumFeatureType.更新要素)
            {
                m_Hook.MapControl.Map.ClearLayers();
            }
            //加载数据
            AddHistoryData();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //加载数据集下面的要素类到列表框中
        private void btnConn_Click(object sender, EventArgs e)
        {
            string pFeaDtName = "";//数据集名称
            IFeatureDataset pFeaDt = null; //数据集
            Dictionary<string, IFeatureDataset> dicFeaDtInfo = new Dictionary<string, IFeatureDataset>();//要素集信息
            Dictionary<string, IFeatureClass> dicFeaClsInfo = new Dictionary<string, IFeatureClass>();   //要素类信息

            LstAllFeaCls.Items.Clear();
            LstCheckoutFeaCls.Items.Clear();

            //根据数据集名称获得数据集
            pFeaDtName = cmbDataset.Text.Trim();
            if (cmbDataset.Tag == null) return;
            dicFeaDtInfo = cmbDataset.Tag as Dictionary<string, IFeatureDataset>;
            if (!dicFeaDtInfo.ContainsKey(pFeaDtName)) return;
            pFeaDt = dicFeaDtInfo[pFeaDtName];
            if (pFeaDt == null) return;

            IEnumDataset vEnumDt = pFeaDt.Subsets;
            vEnumDt.Reset();
            IDataset vDt = vEnumDt.Next();
            //遍历要素类,并将要素类加载在列表框中
            while (vDt != null)
            {
                IFeatureClass vFeaCls = vDt as IFeatureClass;  //要素类
                string vFeaClsName = vDt.Name;  //要素类名称

                if (!dicFeaClsInfo.ContainsKey(vFeaClsName))
                {
                     LstAllFeaCls.Items.Add(vFeaClsName);
                    dicFeaClsInfo.Add(vFeaClsName, vFeaCls);
                }
                vDt = vEnumDt.Next();
            }
            LstAllFeaCls.Tag = dicFeaClsInfo;
        }

        //添加
        private void btnAddFeaCls_Click(object sender, EventArgs e)
        {
            if (LstAllFeaCls.SelectedItems.Count == 0)
            {
                MessageBox.Show( "请选择要添加的项！", "系统提示", MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            for (int i = 0; i < LstAllFeaCls.SelectedItems.Count; i++)
            {
                if (!LstCheckoutFeaCls.Items.Contains(LstAllFeaCls.SelectedItems[i].ToString()))
                {
                    LstCheckoutFeaCls.Items.Add(LstAllFeaCls.SelectedItems[i].ToString());
                }
            }
        }

        //全部添加
        private void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LstAllFeaCls.Items.Count; i++)
            {
                if (!LstCheckoutFeaCls.Items.Contains(LstAllFeaCls.Items[i].ToString()))
                {
                    LstCheckoutFeaCls.Items.Add(LstAllFeaCls.Items[i].ToString());
                }
            }
        }

        //清除
        private void btnClearFeacls_Click(object sender, EventArgs e)
        {
            LstCheckoutFeaCls.Items.Clear();
        }


        /// <summary>
        /// 根据数据库体类型初始化界面
        /// </summary>
        /// <param name="pFeaType">库体类型：参照库体、目标库体</param>
        private void InitialFrm(EnumFeatureType pFeaType)
        {
            btnOK.Enabled = false;
            object[] TagDBType = new object[] { "SDE", "GDB", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
            if (pFeaType == EnumFeatureType.参照要素)
            {
                //历史数据，sde上的
                comBoxType.Enabled = true;
            }
            else if (pFeaType == EnumFeatureType.更新要素)
            {
                //Exception err = null;
                /////获取工程项目名称
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(ModData.v_projectXML))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "找不到配置文件\n" + ModData.v_projectXML);
                    return;
                }
                xmlDoc.Load(ModData.v_projectXML);
                XmlElement rootElem = xmlDoc.DocumentElement;//根元素
                string proID = "";//项目ID
                string proName = "";//项目名称
                proID = rootElem.GetAttribute("当前库体编号");
                XmlNode proCurNode = null;//工程节点
                proCurNode = rootElem.SelectSingleNode(".//工程[@编号='" + proID + "']");
                if (proCurNode == null) return;
                proName = (proCurNode as XmlElement).GetAttribute("名称");
                XmlNode conNode = null;//连接信息节点元素
                conNode = proCurNode.FirstChild.FirstChild.FirstChild;
                if (conNode == null) return;

                comBoxType.Text = (conNode as XmlElement).GetAttribute("类型");
                txtServer.Text = (conNode as XmlElement).GetAttribute("服务器");
                txtInstance.Text = (conNode as XmlElement).GetAttribute("服务名");
                txtDB.Text = (conNode as XmlElement).GetAttribute("数据库");
                txtUser.Text = (conNode as XmlElement).GetAttribute("用户");
                txtPassword.Text = (conNode as XmlElement).GetAttribute("密码");
                txtVersion.Text = (conNode as XmlElement).GetAttribute("版本");

                comBoxType.Enabled = false;
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtDB.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;

                m_DBName = (conNode.FirstChild as XmlElement).GetAttribute("名称");
            }
        }

        /// <summary>
        /// 加载历史数据
        /// </summary>
        /// <param name="strTemp"></param>
        /// <param name="strType"></param>
        private void AddHistoryData()
        {
            //if (m_FeaType == EnumFeatureType.更新要素)
            //{
            //    //更新要素，则清空comboBox插件
            //    ModData.m_ComboBox.Items.Clear();
            //}
            Application.DoEvents();
            //加载历史数据
            if (m_WS == null)
            {
                MessageBox.Show("数据连接失败!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, IFeatureClass> dicFeaClsInfo = new Dictionary<string, IFeatureClass>();//要素类信息
            Dictionary<string, IFeatureClass> dicSelFeaClsInfo = new Dictionary<string, IFeatureClass>(); //要加载的要素类
            if (LstAllFeaCls.Tag == null) return;
            dicFeaClsInfo = LstAllFeaCls.Tag as Dictionary<string, IFeatureClass>;

            //遍历下拉列表框，将选中的要素类加载到图面上来
            for (int i = 0; i < LstCheckoutFeaCls.Items.Count; i++)
            {
                string pFeaClsName = "";//要素类名称
                IFeatureClass pFeaCls = null; //要素类

                pFeaClsName = LstCheckoutFeaCls.Items[i].ToString();
                if (!dicFeaClsInfo.ContainsKey(pFeaClsName)) return;
                pFeaCls = dicFeaClsInfo[pFeaClsName];

                if (pFeaCls == null) continue;

                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatureLayer = new FDOGraphicsLayerClass();
                }
                pFeatureLayer.FeatureClass = pFeaCls;
                pFeatureLayer.Name = pFeaClsName;

                //更新要素，则将要素类加载在comboBox插件中
                if (!dicSelFeaClsInfo.ContainsKey(pFeaClsName))
                {
                    dicSelFeaClsInfo.Add(pFeaClsName, pFeaCls);
                }

                if (m_FeaType == EnumFeatureType.更新要素)
                {
                    m_Hook.MapControl.Map.AddLayer(pFeatureLayer as ILayer);
                }
            }

            if (m_FeaType == EnumFeatureType.更新要素)
            {
                //对图层进行排序

                SysCommon.Gis.ModGisPub.LayersCompose(m_Hook.MapControl);
            }
            else if (m_FeaType == EnumFeatureType.参照要素)
            {
                //参照要素另外加载在一个框框中
                ModData.UpDataCompareFrm = new frmCompareData(m_Hook, dicSelFeaClsInfo);
                ModData.UpDataCompareFrm.Show();
                
            }
        }
        /// <summary>
        /// 设置SDE工作区
        /// </summary>
        /// <param name="sServer">服务器名</param>
        /// <param name="sService">服务名</param>
        /// <param name="sDatabase">数据库名(SQLServer)</param>
        /// <param name="sUser">用户名</param>
        /// <param name="sPassword">密码</param>
        /// <param name="strVersion">SDE版本</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace SetWorkspace(string sServer, string sService, string sDatabase, string sUser, string sPassword, string strVersion, out Exception eError)
        {
            eError = null;
            IWorkspace pWS = null;
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);

            try
            {
                pWS = pSdeFact.Open(pPropSet, 0);
                pPropSet = null;
                pSdeFact = null;
                return pWS;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 设置PDB、GDB、SHP工作区
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace SetWorkspace(string sFilePath,string dbType, out Exception eError)
        {
            eError = null;
            IWorkspace pWS = null;
            IPropertySet pPropSet = new PropertySetClass();
            try
            {
                switch (dbType)
                {
                    case "PDB":
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWS = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        break;
                    case "GDB":
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWS = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        break;
                    case "SHP":
                        ShapefileWorkspaceFactory pFileSHPFact = new ShapefileWorkspaceFactory();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWS = pFileSHPFact.Open(pPropSet, 0);
                        pFileSHPFact = null;
                        break;
                }
                pPropSet = null;
                return pWS;          
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }


        /// <summary>
        /// 获取数据库下全部的离散FC名称
        /// </summary>
        /// <returns></returns>
        private List<string> GetFeatureClassNames(IWorkspace pWs)
        {
            return GetDatasetNames(pWs, esriDatasetType.esriDTFeatureClass);
        }

        /// <summary>
        /// 获取工作空间下指定类型数据集名称
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="aDatasetTyp">数据集类型</param>
        /// <returns></returns>
        private List<string> GetDatasetNames(IWorkspace pWorkspace, esriDatasetType aDatasetTyp)
        {
            List<string> DatasetNames = new List<string>();
            IEnumDatasetName pEnumDatasetName = pWorkspace.get_DatasetNames(aDatasetTyp);
            IDatasetName pDatasetName = pEnumDatasetName.Next();
            while (pDatasetName != null)
            {
                DatasetNames.Add(pDatasetName.Name);
                pDatasetName = pEnumDatasetName.Next();
            }
            return DatasetNames;
        }

        /// <summary>
        /// 获取要素集FeatureClass
        /// </summary>
        /// <param name="feaclassname">要素集名</param>
        /// <returns></returns>
        private IFeatureClass GetFeatureClass(IWorkspace pWs, string feaclassname, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)pWs;
            //打开FeaClass
            try
            {   //要素集可能不存在，做一次保护
                return pFeaWS.OpenFeatureClass(feaclassname);
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 获取数据库下全部的FD名称
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllFeatureDatasetNames(IWorkspace pWS)
        {
            return GetDatasetNames(pWS, esriDatasetType.esriDTFeatureDataset);
        }

        /// <summary>
        /// 获取要素集FeatureDataset
        /// </summary>
        /// <param name="featuredsname">要素集名</param>
        /// <returns></returns>
        private IFeatureDataset GetFeatureDataset(IWorkspace pWs, string featuredsname, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            IFeatureWorkspace pFeaWS = (IFeatureWorkspace)pWs;
            //打开FeatureDataset
            try
            {   //要素集可能不存在，做一次保护
                return pFeaWS.OpenFeatureDataset(featuredsname);
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxType.Text != "SDE")
            {
                btnDB.Visible = true;
                txtDB.Size = new Size(txtServer.Size.Width - btnDB.Size.Width, txtServer.Size.Height);
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtServer.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnDB.Visible = false;
                txtDB.Size = new Size(txtServer.Size.Width, txtServer.Size.Height);
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtServer.Enabled = true;
                txtVersion.Enabled = false;
            }
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
                            MessageBox.Show("请选择GDB格式文件!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

     
    }
}
