using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;



using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF.BaseClasses;
//ZQ 20111009     Mxd导出JPG
namespace GeoPageLayout
{
    public partial class FrmMxdExportJPG : DevComponents.DotNetBar.Office2007Form
    {
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
        IMapDocument m_MapDocument;
        public FrmMxdExportJPG()
        {
            InitializeComponent();
        }
        #region    数据源设置
        private void bttOpenFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = true;
            sOpenFileD.Title = "选择数据源";
            sOpenFileD.Filter = "(.mxd文件)|*.mxd";
            int m = 1;
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                string[] strFileName = sOpenFileD.FileNames;
                for (int j = 0; j < strFileName.Length; j++)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (strFileName[j].ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                break;
                            }
                            else
                            {
                                m = 0;
                                break;
                            }
                        }

                    }
                    if (m == 1)
                    {
                        checkedMDData.Items.Add(strFileName[j], true);
                    }
                }
            }
    
            if (checkedMDData.Items.Count > 0)
            {
              bttRemove.Enabled = true;
             bttAllRemove.Enabled = true;
            }
        }

        private void bttOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            int m = 1;
            if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] pFilePath = Directory.GetFiles(pFolderBrowserDialog.SelectedPath, "*.mxd", SearchOption.TopDirectoryOnly);
                if (pFilePath.Length == 0)
                {
                    for (int i = 0; i < checkedMDData.Items.Count; i++)
                    {
                        if (pFolderBrowserDialog.SelectedPath.ToString() == checkedMDData.Items[i].ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                break;
                            }
                            else
                            {
                                m = 0;
                                break;
                            }
                        }
                    }
                    if (m == 1)
                    {
                        checkedMDData.Items.Add(pFolderBrowserDialog.SelectedPath, true);
                    }
                }
                else
                {
                    for (int j = 0; j < pFilePath.Length; j++)
                    {
                        for (int i = 0; i < checkedMDData.Items.Count; i++)
                        {
                            if (pFilePath[j].ToString() == checkedMDData.Items[i].ToString())
                            {
                                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    m = 0;
                                    break;
                                }
                            }
                        }
                        if (m == 1)
                        {
                            checkedMDData.Items.Add(pFilePath[j].ToString(), true);
                        }
                    }
                }
            }
                
            if (checkedMDData.Items.Count > 0)
            {
                bttRemove.Enabled = true;
                bttAllRemove.Enabled = true;
            }
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    this.checkedMDData.SetItemChecked(i, true);
                }
            }
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count > 0)
            {
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    if (this.checkedMDData.GetItemChecked(i))
                    {
                        this.checkedMDData.SetItemChecked(i, false);
                    }
                    else
                    {
                        this.checkedMDData.SetItemChecked(i, true);
                    }
                }
            }
        }


        private void bttRemove_Click(object sender, EventArgs e)
        {
            int Index = this.checkedMDData.SelectedIndex;
            if (Index != -1)
            {
                checkedMDData.Items.Remove(checkedMDData.Items[Index].ToString());
            }
            if (checkedMDData.Items.Count == 0)
            {
                bttRemove.Enabled = false;
                bttAllRemove.Enabled = false;
            }
        }

        private void bttAllRemove_Click(object sender, EventArgs e)
        {
            checkedMDData.Items.Clear();
            bttRemove.Enabled = false;
            bttAllRemove.Enabled = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
        #endregion

        private void btnSaveDl_Click(object sender, EventArgs e)
        {  
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            if (pFolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            if (pFolderBrowserDialog.SelectedPath == "")
                return;
            txtPath.Text = pFolderBrowserDialog.SelectedPath;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (checkedMDData.Items.Count == 0) { MessageBox.Show("请指定数据源！","提示！"); return; }
            if (txtPath.Text == "") { MessageBox.Show("请指定输出路径！","提示！"); return; }
            IActiveView pActiveView;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;//设置进度条
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false;
            vProgress.TopMost = true;
            vProgress.MaxValue = checkedMDData.Items.Count;
            //vProgress.EnableUserCancel(true);
            vProgress.ShowProgress();
            try
            {
                //string jpgDir = "";
                for (int i = 0; i < checkedMDData.Items.Count; i++)
                {
                    vProgress.ProgresssValue = i + 1;
                    if (checkedMDData.GetItemChecked(i))
                    {
                        
                        string sFilePath = checkedMDData.Items[i].ToString();
                        //jpgDir = System.IO.Path.GetDirectoryName(sFilePath);
                        string jpgFileName=System.IO.Path.GetFileNameWithoutExtension(sFilePath) + ".jpg";
                        vProgress.SetProgress("正在导出图片：" + jpgFileName);
                        OpenDocument(sFilePath);
                        pActiveView = m_MapDocument.ActiveView;
                        //ExportJPG(pActiveView, Convert.ToInt32(UpDownResolution.Value), txtPath.Text, sFilePath);
                        CommandExportActiveView pCmd = new CommandExportActiveView(Convert.ToInt32(UpDownResolution.Value), 1, txtPath.Text+"\\"+jpgFileName, "JPG", false);
                        pCmd.ExportActiveView = pActiveView;
                        pCmd.OnClick();
                       
                    }
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("输出图片完成！保存在： " + txtPath.Text);
                }
                this.Close();
                this.Dispose();
            }
            catch { MessageBox.Show("图片导出失败！","提示！"); }
            finally
            {
                m_MapDocument.Close();
                m_MapDocument = null;
                vProgress.Close();
            }
        }
        /// <summary>
        /// 打开Mxd文档
        /// </summary>
        /// <param name="sFilePath">文档路径</param>
        private void OpenDocument(string sFilePath)
        {
            if (m_MapDocument != null) m_MapDocument.Close();
            m_MapDocument = new MapDocumentClass();
            m_MapDocument.Open(sFilePath, "");
        }
        /// <summary>
        /// 根据Mxd输出JPG图片
        /// </summary>
        /// <param name="pActiveView"></param>
        /// <param name="Resolution">分辨率</param>
        /// <param name="strFilePath">保存图片路径</param>
        /// <param name="strMxdPath">Mxd文档路径</param>
        private void ExportJPG(IActiveView pActiveView, int Resolution,string strFilePath,string strMxdPath)
        {
          
            long iPrevOutputImageQuality;
            IOutputRasterSettings docOutputRasterSettings;
            IDisplayTransformation docDisplayTransformation;
            tagRECT DisplayBounds;
            IPageLayout docPageLayout;
            IEnvelope docGraphicsExtentEnv;
            IEnvelope docMapExtEnv;
            IUnitConverter pUnitConvertor;
            IEnvelope PixelBoundsEnv;
            IExport pExport = new ExportJPEGClass();
            try
            {
                string strPath =strFilePath + "\\" + System.IO.Path.GetFileNameWithoutExtension(strMxdPath) + ".jpg";
                pExport.ExportFileName = strPath;
                if (System.IO.File.Exists(strPath))
                {
                    System.IO.File.Delete(strPath);
                }
                int iOutputResolution = Resolution;
                pExport.Resolution = iOutputResolution;

                docOutputRasterSettings = pActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                iPrevOutputImageQuality = docOutputRasterSettings.ResampleRatio;
                SetOutputQuality(pActiveView, 1);
                if (pActiveView is IPageLayout)
                {
                    DisplayBounds = pActiveView.ExportFrame;
                    docGraphicsExtentEnv = GetGraphicsExtent(pActiveView);
                }
                else
                {
                    docDisplayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
                    DisplayBounds = docDisplayTransformation.get_DeviceFrame();
                }
                tagRECT exportRECT;
                PixelBoundsEnv = new Envelope() as IEnvelope;
               

                double tempratio = iOutputResolution / Resolution;
                double tempbottom = DisplayBounds.bottom * tempratio;
                double tempright = DisplayBounds.right * tempratio;

                exportRECT.bottom = (int)Math.Truncate(tempbottom);
                exportRECT.left = 0;
                exportRECT.top = 0;
                exportRECT.right = (int)Math.Truncate(tempright);

                PixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
                docMapExtEnv = null;

            

                pExport.PixelBounds = PixelBoundsEnv;
                int hDc = pExport.StartExporting();
                pActiveView.Output(hDc, (int)pExport.Resolution, ref exportRECT, docMapExtEnv, null);
                pExport.FinishExporting();
            }
            catch { }
            finally
            {
                //完成输出后清理临时文件
                pExport.Cleanup();
                docMapExtEnv = null;
                PixelBoundsEnv = null;
                docGraphicsExtentEnv = null;
            }

        }

        private void SetOutputQuality(IActiveView docActiveView, long iResampleRatio)
        {
            IGraphicsContainer oiqGraphicsContainer;
            IElement oiqElement;
            IOutputRasterSettings docOutputRasterSettings;
            IMapFrame docMapFrame;
            IActiveView TmpActiveView;

            if (docActiveView is IMap)
            {
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
            }
            else if (docActiveView is IPageLayout)
            {
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                oiqGraphicsContainer = docActiveView as IGraphicsContainer;
                oiqGraphicsContainer.Reset();

                oiqElement = oiqGraphicsContainer.Next();
                while (oiqElement != null)
                {
                    if (oiqElement is IMapFrame)
                    {
                        docMapFrame = oiqElement as IMapFrame;
                        TmpActiveView = docMapFrame.Map as IActiveView;
                        docOutputRasterSettings = TmpActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                        docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                    }
                    oiqElement = oiqGraphicsContainer.Next();
                }

                docMapFrame = null;
                oiqGraphicsContainer = null;
                TmpActiveView = null;
            }
            docOutputRasterSettings = null;

        }

        private IEnvelope GetGraphicsExtent(IActiveView docActiveView)
        {
            IEnvelope GraphicsBounds;
            IEnvelope GraphicsEnvelope;
            IGraphicsContainer oiqGraphicsContainer;
            IPageLayout docPageLayout;
            IDisplay GraphicsDisplay;
            IElement oiqElement;

            GraphicsBounds = new EnvelopeClass();
            GraphicsEnvelope = new EnvelopeClass();
            docPageLayout = docActiveView as IPageLayout;
            GraphicsDisplay = docActiveView.ScreenDisplay;
            oiqGraphicsContainer = docActiveView as IGraphicsContainer;
            oiqGraphicsContainer.Reset();

            oiqElement = oiqGraphicsContainer.Next();
            while (oiqElement != null)
            {
                oiqElement.QueryBounds(GraphicsDisplay, GraphicsEnvelope);
                GraphicsBounds.Union(GraphicsEnvelope);
                oiqElement = oiqGraphicsContainer.Next();
            }
            return GraphicsBounds;
        }

       
 


    }
}
