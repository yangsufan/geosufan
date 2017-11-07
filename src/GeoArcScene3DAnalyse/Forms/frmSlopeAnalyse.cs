using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;


using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
//坡度分析      张琪   20110614
namespace GeoArcScene3DAnalyse
{
    public partial class frmSlopeAnalyse : DevComponents.DotNetBar.Office2007Form
    {
        private string filePath;//输入文件路径
        private string fileName;//输入文件名
        private string RasterType;//输出文件的类型
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
        public frmSlopeAnalyse()
        {
            InitializeComponent();
            txtZFactor.Text = "1";
            btnSure.Enabled = false;
            txtSave.Text = "";
            txtCellSize.Text = "0.02";
            txtCellSize.ReadOnly = false;
        }

        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl//获取sceneControl 的值 
        {
            set { m_pCurrentSceneControl = value; }                  //张琪
        }                                                            // 20110614
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
                    if (pLayer is  IRasterLayer )
                    {
                        comboBoxOpen.Items.Add(pLayer.Name);
                        //comboBoxOpen_SelectedIndexChanged();

                    }
                    //else if(pLayer is ITinLayer)
                    //{
                    //    comboBoxOpen.Items.Add(pLayer.Name);
                    //}
                    if (comboBoxOpen.Items.Count > 0)
                    {
                        comboBoxOpen.SelectedIndex = 0;
                        break;
                    }
                }
            }
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

        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }
        private void comboBoxOpen_DropDown(object sender, EventArgs e)//下拉获取栅格图层
        {
            comboBoxOpen.Items.Clear();                               
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)  
            {
                
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                if (pLayer is IRasterLayer)
                {
                    comboBoxOpen.Items.Add(pLayer.Name);
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


        private void comboBoxOpen_SelectedIndexChanged(object sender, EventArgs e)//选择图层并自动设置默认输出像元大小及图层路径
        {
            if(comboBoxOpen.Items.Count>0)
            {
                txtSave.Text = "";
                txtCellSize.Text = "0.02";
             fileName = comboBoxOpen.SelectedItem.ToString();
    
             ILayer pLayer = GetLayerByName(ref fileName);
                //去除不可用的数据    ZQ  20110907
             if (pLayer.Valid)
             {
                 return;
             }
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
                 filePath = pSourceWorkspace.PathName;
                 filePath = filePath + fileName;
                 
             }
             else if(pLayer is ITinLayer)//读取TIN数据的路径信息
             {
         
                 ITinLayer pTinLayer = pLayer as ITinLayer;
                 ITin pTin = pTinLayer.Dataset;
                 IDataset pDataset = pTin as IDataset;
                 pSourceWorkspace = pDataset.Workspace;
                 filePath = pSourceWorkspace.PathName;
                 filePath = filePath + "\\"+fileName;
             }

             btnSure.Enabled = true;

            }
            else
            {
                btnSure.Enabled = false;
            }
            
        }

        private void radioBtnDegree_CheckedChanged(object sender, EventArgs e)//度与百分比二选一
        {
            if (radioBtnDegree.Checked)
            {
                radioBtn.Checked = false;
            }
            else
            {
                radioBtn.Checked = true;
                txtCellSize.ReadOnly = true;
            }

        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)//张琪    20110613
        {

            if (radioBtn.Checked)
            {
                radioBtnDegree.Checked = false;
            }
            else
            {
                radioBtnDegree.Checked = true;
                txtCellSize.ReadOnly = false;
            }
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string OutFilePath,OutfileName;
            SaveFileDialog pSaveFileDialog = new SaveFileDialog();
            pSaveFileDialog.Filter = "TIFF(*.tif)|*.tif|GRID||IMAGINE Image|*.img";//保存GRID格式的数据无需加扩展名(*.grid)(*.img)
            if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            txtSave.Text = pSaveFileDialog.FileName;
            OutFilePath=System.IO.Path.GetDirectoryName(txtSave.Text);
            OutfileName =System.IO.Path.GetFileNameWithoutExtension( txtSave.Text);
            RasterType = System.IO.Path.GetExtension(txtSave.Text);
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
                
                if(comboBoxOpen.Text=="")
                {
                    MessageBox.Show("请选择数据！", "提示！");
                    return;
                }
                if(txtZFactor.Text=="")
                {
                    MessageBox.Show("请设置Z值参数！", "提示！");
                    return;
                }
                if (txtCellSize.Text=="")
                {
                    MessageBox.Show("请设置输出精度参数！", "提示！");
                    return;
                }
                if (txtSave.Text == "")
                {
                    MessageBox.Show("请输入保存文件名！", "提示！");
                    return;
                }
                string Unit;
                if (radioBtnDegree.Checked)
                {
                    Unit = "DEGREE";
                }
                else
                {
                    Unit = "PERCENT";
                }
                //this.Hide();
                //Progress.FormProgress
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在进行坡度分析");
                ILayer pLayer = GetLayerByName(ref fileName);//根据图层名获取图层
                IGeoDataset pGeoDataset = pLayer  as IGeoDataset;
                ISurfaceOp pSurfaceOp;
                IRasterAnalysisEnvironment pRasterAnalysisEnvironment = new RasterSurfaceOp();
                pSurfaceOp = pRasterAnalysisEnvironment as ISurfaceOp;
                object  pObject = Convert.ToDouble(txtCellSize.Text);
                object pZFactor = Convert.ToDouble(txtZFactor.Text);
                pRasterAnalysisEnvironment.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref pObject);//设置输出数据的像元值大小
                IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();
                string OutPath =System.IO.Path.GetDirectoryName(txtSave.Text);//输出文件存储的目录信息
                string TempPath =string.Concat(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "\\Temp\\") ;//临时文件存储位置
                  if(!Directory.Exists(TempPath))
                {
                    Directory.CreateDirectory(TempPath);// 当路径不存在时创建临时文件存储路径
                }
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(TempPath, 0);
                pRasterAnalysisEnvironment.OutWorkspace = pWorkspace;
                IRasterBandCollection pRasterBandCollection;
                string Newfile = System.IO.Path.GetFileName(txtSave.Text);
                if (pGeoDataset is IRasterLayer)
                {
                    IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                    pGeoDataset = (IGeoDataset)pRasterLayer.Raster;
                        
                    if (Unit == "DEGREE")//选择不同单位进行坡度分析
                    {
                        pRasterBandCollection = pSurfaceOp.Slope(pGeoDataset, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopeDegrees, ref pZFactor) as IRasterBandCollection;
                    }
                    else
                    {
                        pRasterBandCollection = pSurfaceOp.Slope(pGeoDataset, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopePercentrise, ref pZFactor) as IRasterBandCollection;
                    }
                    pWorkspace = pWorkspaceFactory.OpenFromFile(OutPath, 0);

                     pRasterBandCollection.SaveAs(Newfile, pWorkspace, RasterType);//保存分析结果到固定路径下
                     vProgress.Close();
                    if (MessageBox.Show("坡度分析成功，是否加载分析结果", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        vProgress.ShowProgress();
                        vProgress.SetProgress("正在进行加载结果数据");
                        IRasterWorkspace pRasterWorkspace = pWorkspaceFactory.OpenFromFile(OutPath, 0) as IRasterWorkspace; // 张琪  20110613
                        IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(System.IO.Path.GetFileName(txtSave.Text));//包含扩展名的表面数据集
                        IRasterLayer pOutRasterLayer = new RasterLayerClass();
                        pOutRasterLayer.CreateFromDataset(pRasterDataset);
                        m_pCurrentSceneControl.Scene.AddLayer(pOutRasterLayer as ILayer, true);
                        vProgress.Close();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pRasterBandCollection);
                }
                else if (pGeoDataset is ITinLayer)
                {
                    ITinLayer pTinLayer = pGeoDataset as ITinLayer;
                    ITinAdvanced pTinAdvanced = pTinLayer.Dataset as ITinAdvanced;
                    Cls3DModulsefun pCls3DModulsefun = new Cls3DModulsefun();
                    IRasterDataset pRasterDataset;
                    esriRasterizationType pesriRasterizationType ;
                       
                    if (Unit == "DEGREE")
                    {
                        pesriRasterizationType = esriRasterizationType.esriDegreeSlopeAsRaster;
                    }
                    else
                    {
                        pesriRasterizationType = esriRasterizationType.esriPercentageSlopeAsRaster;   
                    }
                    rstPixelType prstPixelType = rstPixelType.PT_LONG;
                    //TIN数据转换成Raster数据并进行坡度分析（暂时未成功）
                    vProgress.Close();
                    pRasterDataset = pCls3DModulsefun.TinToRaster2(pTinAdvanced, pesriRasterizationType, OutPath, Newfile, prstPixelType, Convert.ToDouble(txtCellSize.Text), pTinAdvanced.Extent, true, RasterType);
                    if (MessageBox.Show("坡度分析成功，是否加载分析结果", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        IRasterLayer pOutRasterLayer = new RasterLayerClass();
                        pOutRasterLayer.CreateFromDataset(pRasterDataset);
                        m_pCurrentSceneControl.Scene.AddLayer(pOutRasterLayer as ILayer, true);
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("坡度分析,表面集为:" + comboBoxOpen.Text);
                            Plugin.LogTable.Writelog("输出栅格路径为:" + txtSave.Text);
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("目前不支持对当前选择图层的坡度分析功能","提示！");
                }
                m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                this.Close();
                DeleteFolder(TempPath);//清楚临时文件
            }
            catch
            {
                vProgress.Close();
                this.Close();
                MessageBox.Show("很抱歉，操作失败！","提示！");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtZFactor_KeyPress(object sender, KeyPressEventArgs e)//规定Z值参数只能输入0123456789.
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }

        }

        private void txtCellSize_KeyPress(object sender, KeyPressEventArgs e)//规定像元精度参数只能输入0123456789.
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }

        }





      
       
    }
}
