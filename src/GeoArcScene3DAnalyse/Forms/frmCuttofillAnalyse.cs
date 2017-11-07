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

//填挖      张琪   20110627
namespace GeoArcScene3DAnalyse
{
    public partial class frmCuttfillAnalyse : DevComponents.DotNetBar.Office2007Form
    {
        private string fortraster_Path; //填挖前栅格数据的路径
        private string fortraster_Name;//填挖前栅格数据名
        private string toraster_Path;//填挖后栅格数据的路径
        private string toraster_Name;//填挖后栅格数据名
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
        public frmCuttfillAnalyse()
        {
            InitializeComponent();
            txtZFactor.Text = "1";
            btnSure.Enabled = false;
            txtSave.Text = "";
            txtCellSize.Text = "0.2";
        
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
            set { m_pCurrentSceneControl = value; }                  //张琪   20110627
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
                        comboBoxFortraster.Items.Add(pLayer.Name);
                        comboBoxToraster.Items.Add(pLayer.Name);
                    }

                    if (comboBoxFortraster.Items.Count > 0)
                    {
                        comboBoxFortraster.SelectedIndex = 0;

                    }
                    if (comboBoxToraster.Items.Count > 0)
                    {
                        comboBoxToraster.SelectedIndex = 0;
                    }
                }
            }
        }
        #region  输入栅格数据
        private void comboBoxFortraster_DropDown(object sender, EventArgs e)
        {
            comboBoxFortraster.Items.Clear();
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IRasterLayer)
                    {
                        comboBoxFortraster.Items.Add(pLayer.Name);
                    }
                    //else if (pLayer is ITinLayer)
                    //{
                    //    comboBoxFortraster.Items.Add(pLayer.Name);
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

        private void comboBoxFortraster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFortraster.Items.Count > 0)
            {
                fortraster_Name = comboBoxFortraster.SelectedItem.ToString();
                ;
                ILayer pLayer = GetLayerByName(ref fortraster_Name);
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
                    fortraster_Path = pSourceWorkspace.PathName;
                    fortraster_Path = fortraster_Path + fortraster_Name;

                }
                else if (pLayer is ITinLayer)
                {
                    ITinLayer pTinLayer = pLayer as ITinLayer;
                    ITin pTin = pTinLayer.Dataset;
                    IDataset pDataset = pTin as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    fortraster_Path = pSourceWorkspace.PathName;
                    fortraster_Path = fortraster_Path + "\\" + fortraster_Name;
                    //txtCellSize.Text = "";
                }
                btnSure.Enabled = true;
            }
            else
            {
                btnSure.Enabled = false;
            }
        }

        private void comboBoxToraster_DropDown(object sender, EventArgs e)
        {
            comboBoxToraster.Items.Clear();
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IRasterLayer)
                    {
                        comboBoxToraster.Items.Add(pLayer.Name);
                    }
                    //else if (pLayer is ITinLayer)
                    //{
                    //    comboBoxFortraster.Items.Add(pLayer.Name);
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

        private void comboBoxToraster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxToraster.Items.Count > 0)
            {
                toraster_Name = comboBoxToraster.SelectedItem.ToString();
                ;
                ILayer pLayer = GetLayerByName(ref fortraster_Name);
                IWorkspace pSourceWorkspace;
                if (pLayer is IRasterLayer)//读取RasterLayer的路径名和像元大小值
                {
                    IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                    IDataset pDataset = pRasterLayer as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    toraster_Path = pSourceWorkspace.PathName;
                    toraster_Path = toraster_Path + toraster_Name;

                }
                else if (pLayer is ITinLayer)
                {
                    ITinLayer pTinLayer = pLayer as ITinLayer;
                    ITin pTin = pTinLayer.Dataset;
                    IDataset pDataset = pTin as IDataset;
                    pSourceWorkspace = pDataset.Workspace;
                    toraster_Path = pSourceWorkspace.PathName;
                    toraster_Path = toraster_Path + "\\" + toraster_Name;
                }
                btnSure.Enabled = true;
            }
            else
            {
                btnSure.Enabled = false;
            }
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
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
                string RasterType = System.IO.Path.GetExtension(txtSave.Text);
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
                    MessageBox.Show("该文件正在被使用，不能被替换", "提示！");
                    if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    txtSave.Text = pSaveFileDialog.FileName;
                    return;

                }
            }
            catch
            {
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
                if (txtSave.Text == "" || comboBoxFortraster.Text == "" || comboBoxToraster.Text == "")
                {
                    MessageBox.Show("参数未设置完全！", "提示");
                    return;
                }

                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在进行填挖方分析");
                double Z = Convert.ToDouble(txtZFactor.Text.ToString());
                string SavName = System.IO.Path.GetDirectoryName(txtSave.Text);

               ClsGPTool  pClsGPTool = new ClsGPTool();
               pClsGPTool.CutFillGP(fortraster_Path,toraster_Path,txtSave.Text,Z);//进行填挖方分析
               vProgress.Close();
               if (MessageBox.Show("填挖方分析成功，是否加载分析结果", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
               {
                   if (this.WriteLog)
                   {
                       Plugin.LogTable.Writelog("填挖方分析，填挖前表面集:" + comboBoxFortraster.Text + ",填挖后表面集:" + comboBoxToraster.Text);
                       Plugin.LogTable.Writelog("输出栅格路径为:" + txtSave.Text);
                   }
                   vProgress.ShowProgress();
                   vProgress.SetProgress("正在进行加载结果数据");
                   IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();                              // 张琪  20110627
                   IRasterWorkspace pRasterWorkspace = pWorkspaceFactory.OpenFromFile(SavName, 0) as IRasterWorkspace;
                   IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(System.IO.Path.GetFileName(txtSave.Text));//包含扩展名的表面数据集
                   IRasterLayer pRasterLayer = new RasterLayerClass();
                   pRasterLayer.CreateFromDataset(pRasterDataset);
                   ILayer pLayer = pRasterLayer as ILayer;
                   m_pCurrentSceneControl.Scene.AddLayer(pLayer, true);
                   m_pCurrentSceneControl.SceneGraph.RefreshViewers();
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
