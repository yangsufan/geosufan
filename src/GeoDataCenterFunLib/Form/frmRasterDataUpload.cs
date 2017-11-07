using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using SysCommon;
using SysCommon.Gis;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;

//*********************************************************************************
//** 文件名：frmRasterDataUpload.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-04-11
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************

namespace GeoDataCenterFunLib
{
    public partial class frmRasterDataUpload : DevComponents.DotNetBar.Office2007Form
    {
        public frmRasterDataUpload()
        {
            InitializeComponent();
            
        }

        private void frmRasterDataUpload_Load(object sender, EventArgs e)
        {
           
            string strExp = "select 数据源名称 from 物理数据源表";
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int ii = 0; ii < list.Count; ii++)
            {
                comboBoxSource.Items.Add(list[ii]);//加载数据源列表框
            }
            if (list.Count > 0)
            {
                comboBoxSource.SelectedIndex = 0;//默认选择第一个
            }
        }

        GetDataTreeInitIndex m_dIndex = new GetDataTreeInitIndex();//取得路径的类
        frmDataReduction fdr=new frmDataReduction();//删除时调用数据整理中删除表的方法
        //SysGisDataSet ds = new SysGisDataSet();
        OpenFileDialog OpenFile;
        int i = 0;
        bool m_success=false;
        bool m_newfile;

        /// <summary>
        /// 得到数据源地址
        /// </summary>
        /// <param name="str">数据源名称</param>
        /// <returns></returns>
        private string GetSourcePath(string str)
        {
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 数据库 from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                return strname;
            }
            catch { return ""; }
        }
        //栅格数据入库到GDB数据库的方法
        private void ImportRasterToNewWorkSpace(string file, string outfilename)
        {
            try
            {
                string ExportFileShortName = outfilename;
                if (file == "") { return; }
                int Index = file.LastIndexOf("\\");
                string ImportFileName = file.Substring(Index + 1);
                string ImportFilePath = System.IO.Path.GetDirectoryName(file);
                //打开存在的GDB工作空间
                //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
                //IWorkspace pWorkspace = Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0);
                IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
                IWorkspace2 pWorkspace2 = (IWorkspace2)pWorkspace;
                
                //判断要素是否存在，若存在将删除源文件
                if (pWorkspace2.get_NameExists(esriDatasetType.esriDTRasterDataset, ImportFileName))
                {
                    if (m_newfile == true)
                    {
                        IRasterWorkspaceEx pRWs = pWorkspace as IRasterWorkspaceEx;
                        IDataset pDataset = pRWs.OpenRasterDataset(ImportFileName) as IDataset;
                        pDataset.CanDelete();
                        pDataset.Delete();
                        pDataset = null;
                    }
                    else
                    {
                        MessageBox.Show("存在相同文件名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_success = false;
                        return;
                    }
                }
                //IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();
                //IWorkspace pWs = pWorkspaceFactory.OpenFromFile(ImportFilePath, 0);
                //IRasterDataset pRasterDs = null;
                //IRasterWorkspace pRasterWs;
                //IRasterWorkspaceEx pRasterEx=pWs as IRasterWorkspaceEx;
                //pRasterWs = pWs as IRasterWorkspace;
                //pRasterDs = pRasterWs.OpenRasterDataset(ImportFileName);
                //ISaveAs2 saveAs2 = (ISaveAs2)pRasterDs;
                ITrackCancel pTrackCancel = new TrackCancelClass();
                IRasterCatalogLoader pRasterload = new RasterCatalogLoaderClass();
                pRasterload.Workspace = pWorkspace;
                pRasterload.EnableBuildStatistics = true;
                pRasterload.StorageDef = new RasterStorageDefClass();
                pRasterload.StorageDef.CompressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                pRasterload.StorageDef.PyramidLevel = 9;
                pRasterload.StorageDef.CompressionQuality = 50;
                pRasterload.StorageDef.TileHeight = 128;
                pRasterload.StorageDef.TileWidth = 128;
                pRasterload.Projected = true;
                
                //加载栅格数据到catalog方法
                pRasterload.Load(comboBoxCatalog.Text.Trim(), ImportFilePath, pTrackCancel);
                
                //单独导入栅格数据
                #region
                //IRasterStorageDef rasterStorageDef = new RasterStorageDefClass();
                //IRasterStorageDef2 rasterStorageDef2 = (IRasterStorageDef2)rasterStorageDef;
                ////栅格压缩数据格式设定
                //string[] str = file.Split('.');
                //switch (str[1].ToLower())
                //{
                //    case "jpg":
                //        rasterStorageDef2.CompressionType = esriRasterCompressionType.esriRasterCompressionJPEG2000;
                //        break;
                //    case "sid":case "img":
                //        rasterStorageDef2.CompressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                //        break;
                //}
               
                //rasterStorageDef2.CompressionQuality = 50;
                //rasterStorageDef2.Tiled = true;
                //rasterStorageDef2.TileHeight = 128;
                //rasterStorageDef2.TileWidth = 128;
                //saveAs2.SaveAsRasterDataset(outfilename, pWorkspace, "gdb", rasterStorageDef2);
                #endregion

                m_success = true;
            }
            catch(Exception ex)
            {
                m_success = false;
            }
        }      
          
     


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxCatalog.Items.Count==0)
            {
                MessageBox.Show("数据源中没有栅格目录，请先配置栅格目录!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpenFile = new OpenFileDialog();
            OpenFile.Filter = "栅格数据|*.jpg;*.bmp;*.tif;*.sid;*.img;";
            OpenFile.Multiselect = true;  
             
            //打开SHP文件
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {

                foreach (string file in OpenFile.FileNames)
                {
                    for (int j = 0; j < i; j++)
                    {
                        string strExist = listView.Items[j].Text.Trim();
                        if (strExist.CompareTo(file) == 0)
                        {
                            MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                    }
                  
                    {
                        listView.Items.Add(file);
                        listView.Items[i].SubItems.Add("等待入库");
                        listView.Items[i].Checked = true;
                        i++;
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.Checked)
                {
                    listView.Items.Remove(item);
                    i--;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            i = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
          
            bool _check=false;
            foreach (ListViewItem item in listView.Items)
            {   
                
                string []str=item.Text.Split('.');
                str[1] = str[1].Substring(0, 3);
                if (item.Checked&&item.SubItems[1].Text=="等待入库")
                { 
                    _check = true;
                    item.SubItems[1].Text = "正在入库";
                    listView.Refresh();
                    ImportRasterToNewWorkSpace(item.Text,item.Text);
                    if(m_success)
                    item.SubItems[1].Text = "入库完成";
                    else
                    item.SubItems[1].Text = "入库失败";
                    listView.Refresh();
                }
            }
            if (_check ==false)
                MessageBox.Show("请选择要入库的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("操作已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].Checked = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[i].Checked == false)
                {
                    listView.Items[i].Checked = true;
                    //datagwSource.Rows[i].Selected = true;
                }
                else
                {
                    listView.Items[i].Checked = false;
                    //datagwSource.Rows[i].Selected = false;
                }
            }
        }

        //显示栅格目录
        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCatalog.Items.Clear();
            comboBoxCatalog.Text = "";

          
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在加载栅格目录");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();

            //IWorkspaceFactory Pwf = new FileGDBWorkspaceFactoryClass();
            //IWorkspace pWorkspace = (IWorkspace)(Pwf.OpenFromFile(GetSourcePath(comboBoxSource.Text), 0));
            IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
            if (pWorkspace == null)
            {
                MessageBox.Show("数据源空间不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                vProgress.Close();
                this.Activate();
                return;
            }
            //获取所有的raster catalog目录
            comboBoxCatalog.Items.Clear();
            IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog) as IEnumDataset;
            IDataset dataset = enumDataset.Next();
            while (dataset != null)
            {

                string catalog = dataset.Name;
                if (!comboBoxCatalog.Items.Contains(catalog))
                {
                    comboBoxCatalog.Items.Add(catalog);
                }
                dataset = enumDataset.Next();
            }
            if (comboBoxCatalog.Items.Count != 0)
                comboBoxCatalog.SelectedIndex = 0;
            vProgress.Close();
            this.Activate();
        }
        /// <summary>
        /// 得到数据库空间 Added by xisheng 2011.04.28
        /// </summary>
        /// <param name="str">数据源名称</param>
        /// <returns>工作空间</returns>
        private IWorkspace GetWorkspace(string str)
        {
            try
            {
                IWorkspace pws = null;
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select * from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                string type = dt.Rows[0]["数据源类型"].ToString();
                if (type.Trim() == "GDB")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pws = pWorkspaceFactory.OpenFromFile(dt.Rows[0]["数据库"].ToString(), 0);
                }
                else if (type.Trim() == "SDE")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    //PropertySet
                    IPropertySet pPropertySet;
                    pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("Server", dt.Rows[0]["服务器"].ToString());
                    pPropertySet.SetProperty("Database", dt.Rows[0]["数据库"].ToString());
                    pPropertySet.SetProperty("Instance", "5151");//"port:" + txtService.Text
                    pPropertySet.SetProperty("user", dt.Rows[0]["用户"].ToString());
                    pPropertySet.SetProperty("password", dt.Rows[0]["密码"].ToString());
                    pPropertySet.SetProperty("version", "sde.DEFAULT");
                    pws = pWorkspaceFactory.Open(pPropertySet, 0);

                }
                return pws;
            }
            catch
            {
                return null;
            }
        }

      
    }
}