using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
using System.IO;
//视域分析      张琪   20110611
namespace GeoArcScene3DAnalyse
{
    public partial class frmViewshedAnalyse : DevComponents.DotNetBar.Office2007Form
    {
        private string filePath_raster; //输入栅格数据的路径
        private string fileName_raster;//输入栅格数据的名
        private string filePath_features;//输入点、线数据路径
        private string fileName_features;//输入点、线数据名
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
        public frmViewshedAnalyse()
        {
            InitializeComponent();
            txtZFactor.Text = "1";
            btnSure.Enabled = false;

            btnSure.ForeColor = Color.Black;
            txtSave.Text = "";
            txtCellSize.Text = "0.2";
            checkBox1.Checked = true;
         
        }
        /// <summary>
        /// 通过图层名获取图层
        /// </summary>
        /// <param name="sLayerName"></param>
        /// <returns></returns>
        private ILayer GetLayerByName(ref string sLayerName)
        {
            int i;
            int iCount;
            iCount = m_pCurrentSceneControl.Scene.LayerCount;
            for (i = 0; i < iCount; i++)
            {
                ILayer iLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (iLayer.Name == sLayerName)
                {
                    return iLayer;
                }
            }

            return null;
        }
      
        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl//获取sceneControl 的值 
        {
            set { m_pCurrentSceneControl = value; }                  //张琪   20110611

        }
        //初始化 comboBoxOpen选项
        public void initialization()
        {
            if (m_pCurrentSceneControl.Scene.LayerCount == 0)
            {
                return;
            }
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                ILayer pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IRasterLayer)
                    {
                        comboBoxOpenraster.Items.Add(pLayer.Name);
                    }
                    if (pLayer is IFeatureLayer)
                    {
                        IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                        IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                        try
                        {
                            string ShapeType = pFeatureClass.ShapeType.ToString();//处理异常数据

                            if (ShapeType == "esriGeometryPoint" || ShapeType == "esriGeometryPolyline")
                            {
                                comboBoxOpenfeatures.Items.Add(pLayer.Name);
                            }

                        }
                        catch
                        {

                            continue;
                        }
                    }
                    if (comboBoxOpenraster.Items.Count > 0)
                    {
                        comboBoxOpenraster.SelectedIndex = 0;
                        
                    }
                    if(comboBoxOpenfeatures.Items.Count >0)
                    {
                        comboBoxOpenfeatures.SelectedIndex = 0;
                    }
                }
            }
        }

        #region  输入栅格数据操作
        private void comboBoxOpenraster_DropDown(object sender, EventArgs e)
        {
            comboBoxOpenraster.Items.Clear();
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IRasterLayer)
                    {
                        comboBoxOpenraster.Items.Add(pLayer.Name);
                    }
                    //else if (pLayer is ITinLayer)
                    //{
                    //    comboBoxOpen.Items.Add(pLayer.Name);
                    //}
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
           

            }
            
        }

        private void comboBoxOpenraster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOpenraster.Items.Count > 0)
            {
                fileName_raster = comboBoxOpenraster.SelectedItem.ToString();
            
                ILayer pLayer = GetLayerByName(ref fileName_raster);
                IWorkspace pSourceWorkspace;
                if (pLayer is IRasterLayer)//读取RasterLayer的路径名和像元大小值
                {
                    IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                    IRaster pRaster = pRasterLayer.Raster;
                    IRasterAnalysisProps pRasterAnalysisProps = (IRasterAnalysisProps)pRaster;
                    double pixH = pRasterAnalysisProps.PixelHeight;
                    txtCellSize.Text = Convert.ToString(pixH);
                    IDataset pDataset = pRasterLayer as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    filePath_raster = pSourceWorkspace.PathName;
                    filePath_raster = filePath_raster + fileName_raster;

                }
                else if (pLayer is ITinLayer)
                {
                    ITinLayer pTinLayer = pLayer as ITinLayer;
                    ITin pTin = pTinLayer.Dataset;
                    IDataset pDataset = pTin as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    filePath_raster = pSourceWorkspace.PathName;
                    filePath_raster = filePath_raster + "\\" + fileName_raster;
                }
              
                    btnSure.Enabled = true;
              
            }
            else
            {
                btnSure.Enabled = false;
               
                btnSure.ForeColor = Color.Black;
            }
        }

        private void btnOpenraster_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.Filter = "TIFF(*.tif)|*.tif|GRID(*.Grid)|*.grid|IMAGINE(*.img)|*.img";
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {

                comboBoxOpenraster.Text = pOpenFileDialog.FileName;
                if (comboBoxOpenraster.Text != "")//获取打开的栅格数据的路径信息及像元值
                {
                    IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();
                    filePath_raster = comboBoxOpenraster.Text;
                    fileName_raster = System.IO.Path.GetFileName(comboBoxOpenraster.Text);
                    IRasterWorkspace pRasterWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath_raster), 0) as IRasterWorkspace;
                    IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(fileName_raster);
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    IRaster pRaster = pRasterLayer.Raster;
                    IRasterAnalysisProps pRasterAnalysisProps = (IRasterAnalysisProps)pRaster;
                    txtCellSize.Text = Convert.ToString(pRasterAnalysisProps.PixelHeight);
                }
            }
        }
        #endregion

     
        #region  输入点、线数据操作  
        private void comboBoxOpenfeatures_DropDown(object sender, EventArgs e)
        {
            
                comboBoxOpenfeatures.Items.Clear();
                ILayer pLayer;
                for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)  // 张琪   20110611
                {
                    pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                    if (pLayer.Valid)
                    {

                        if (pLayer is IFeatureLayer)
                        {
                            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                            try
                            {
                                string ShapeType = pFeatureClass.ShapeType.ToString();//处理异常数据

                                if (ShapeType == "esriGeometryPoint" || ShapeType == "esriGeometryPolyline")
                                {
                                    comboBoxOpenfeatures.Items.Add(pLayer.Name);
                                }

                            }
                            catch
                            {

                                continue;
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            
           
        }

        private void comboBoxOpenfeatures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOpenfeatures.Items.Count > 0)
            {
                fileName_features = comboBoxOpenfeatures.SelectedItem.ToString();
                ILayer pLayer = GetLayerByName(ref fileName_features);
                IWorkspace pSourceWorkspace;
                if (pLayer is IFeatureLayer)//读取FeatureLayer的路径信息
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    filePath_features = pSourceWorkspace.PathName;
                    filePath_features = filePath_features +"\\"+fileName_features+".shp";
                }

                btnSure.Enabled = true;

            }
            else
            {
                btnSure.Enabled = false;
                btnSure.ForeColor = Color.Black;
            }
        }
    
        #endregion
        private void btnSave_Click(object sender, EventArgs e)
        {
            string OutFilePath, OutfileName;
            SaveFileDialog pSaveFileDialog = new SaveFileDialog();
            pSaveFileDialog.Filter = "TIFF(*.tif)|*.tif|GRID||IMAGINE Image|*.img";//保存GRID格式的数据无需加扩展名(*.grid)(*.img)
            if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            txtSave.Text = pSaveFileDialog.FileName;
            OutFilePath = System.IO.Path.GetDirectoryName(txtSave.Text);
            OutfileName = System.IO.Path.GetFileNameWithoutExtension(txtSave.Text);
             string  RasterType = System.IO.Path.GetExtension(txtSave.Text);
            try
            {
                switch (RasterType)//获取保存文件扩展名，当存在与保存文件同名时删除已存在的文件
                {
                    case ".tif":
                        RasterType = "TIFF";
                        File.Delete(OutFilePath + "\\" + OutfileName + ".tif");
                        File.Delete(OutFilePath + "\\" + OutfileName + ".aux");
                        File.Delete(OutFilePath + "\\" + OutfileName + ".hdr");
                        File.Delete(OutFilePath + "\\" + OutfileName + ".rrd");
                        File.Delete(OutFilePath + "\\" + OutfileName + "");
                        break;
                    case ".img":
                        RasterType = "IMAGINE Image";
                        File.Delete(OutFilePath + "\\" + OutfileName + ".img");
                        File.Delete(OutFilePath + "\\" + OutfileName + ".tfw");
                        break;
                    case "":
                        RasterType = "GRID";
                        File.Delete(OutFilePath + "\\" + OutfileName);
                        File.Delete(OutFilePath + "\\" + OutfileName + ".aux");
                        break;
                }
            }
            catch
            {
                MessageBox.Show("该文件正在被使用，无法被替换", "提示！");
                if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                txtSave.Text = pSaveFileDialog.FileName;
                return;


            }
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
                if (txtSave.Text == ""|| comboBoxOpenfeatures.Text==""||comboBoxOpenraster.Text=="")
                {
                    MessageBox.Show("参数未设置完全！", "提示");
                    return;
                }
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在进行通视分析");
                string  Unit;
                if (checkBox1.Checked)
                {
                    Unit = "CURVED_EARTH";
                }
                else
                {
                    Unit = "FLAT_EARTH";
                }
                double Z = Convert.ToDouble(txtZFactor.Text.ToString());
                string SavName = System.IO.Path.GetDirectoryName(txtSave.Text);
                ClsGPTool  pClsGPTool = new ClsGPTool();
                pClsGPTool.ViewshedAnalyseGP(filePath_features, filePath_raster, txtSave.Text, Unit, Z);//进行通视分析
                vProgress.Close();
              if (MessageBox.Show("通视分析成功，是否加载分析结果", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
               {
                IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();                              // 张琪  20110611
                IRasterWorkspace pRasterWorkspace = pWorkspaceFactory.OpenFromFile(SavName, 0) as IRasterWorkspace;
                IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(System.IO.Path.GetFileName(txtSave.Text));//包含扩展名的表面数据集
                IRasterLayer pRasterLayer = new RasterLayerClass();
                pRasterLayer.CreateFromDataset(pRasterDataset);
                ILayer pLayer = pRasterLayer as ILayer;
                m_pCurrentSceneControl.Scene.AddLayer(pLayer, true);
                m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("通视分析,表面集为" + comboBoxOpenraster.Text + "，观测点为" + comboBoxOpenfeatures.Text);
                    Plugin.LogTable.Writelog("输出栅格路径为:" + txtSave.Text);
                }
                vProgress.Close();
               }
            
                this.Close();
            }
            catch
            {
                vProgress.Close();
                this.Close();
                MessageBox.Show("很抱歉，操作失败！", "提示！");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtZFactor_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void txtCellSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

      
    }
}
