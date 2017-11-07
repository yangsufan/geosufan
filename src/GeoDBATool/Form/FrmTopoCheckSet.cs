using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Controls;


//ygc 2012-12-26逻辑检查设置
namespace GeoDBATool
{
    public partial class FrmTopoCheckSet : DevComponents.DotNetBar.Office2007Form
    {
        public FrmTopoCheckSet(IMapControlDefault pMapControl)
        {
            _MapControl = pMapControl;
            InitializeComponent();
        }
        private DataTable _DataTable = null;
        private string _CheckDataPath = "";
        private IWorkspace _WorkSpace = null;
        private IFeatureClass _FeatureClass = null;
        private ClsTopoCheck _ClsTopoCheck = null;
        private IMapControlDefault _MapControl = null;
        //选择检查数据路径
        private void btnScanDataPath_Click(object sender, EventArgs e)
        {
            _CheckDataPath = "";
            if (!rdbMDB.Checked && !rdbGDB.Checked)
            {
                MessageBox.Show("请选择检查数据类型！","提示",MessageBoxButtons .OK,MessageBoxIcon.Information);
                return;
            }
            if (rdbMDB.Checked == true)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Title = "打开拓扑检查数据源";
                openFile.Filter = "*.mdb|*.mdb";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    txtCheckDataPath.Text = "";
                    _CheckDataPath = openFile.FileName;
                    txtCheckDataPath.Text = _CheckDataPath;
                    _WorkSpace = GetWorkspace(_CheckDataPath);
                    InitLayerCombox();
                }
                else
                { return; }
            }
            else if (rdbGDB.Checked == true)
            {
                FolderBrowserDialog ScanGDB = new FolderBrowserDialog();
                ScanGDB.Description = "选择拓扑检查GDB路径";
                if (ScanGDB.ShowDialog() == DialogResult.OK)
                {
                    txtCheckDataPath.Text = "";
                    _CheckDataPath = ScanGDB.SelectedPath;
                    txtCheckDataPath.Text = _CheckDataPath;
                    _WorkSpace = GetWorkspace(_CheckDataPath);
                    InitLayerCombox();
                }
                else
                { return; }
            }
            
        }
        private void InitLayerCombox()
        {
            cmbLayer.Items.Clear();
            if (_WorkSpace!=null)
            {
                IEnumDataset pDatasets = _WorkSpace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset ptmpDataset = pDatasets.Next();
                while (ptmpDataset != null)
                {
                    IEnumDataset pSubsets = ptmpDataset.Subsets;
                    IDataset pSubDataset = pSubsets.Next();
                    while (pSubDataset != null)
                    {
                        IFeatureClass pFeatureClass = pSubDataset as IFeatureClass;
                        if (pFeatureClass != null)
                        {
                            cmbLayer.Items.Add(pSubDataset.Name);
                        }
                        pSubDataset = pSubsets.Next();
                    }
                    ptmpDataset = pDatasets.Next();
                }
            }
        }
        //根据文件路径打开文件 ygc 2012-8-29
        private IWorkspace GetWorkspace(string filePath)
        {
            IWorkspace pWorkspace = null;
            string FileType = filePath.Substring(filePath.Length - 4, 4);
            switch (FileType)
            {
                case ".shp":
                    IWorkspaceFactory pShpWorkSpaceFactory = new ShapefileWorkspaceFactory();
                    try
                    {
                        pWorkspace = pShpWorkSpaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pShpWorkSpaceFactory);
                    }
                    break;
                case ".mdb":
                    IPropertySet pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("DATABASE", filePath);
                    IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspaceFactory);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pPropertySet);
                    }
                    break;
                case ".gdb":
                    IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pGDBWorkSpace.OpenFromFile(filePath, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pGDBWorkSpace);
                    }
                    break;
            }
            return pWorkspace;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (_FeatureClass == null)
            {
                MessageBox.Show("请选择小班图层!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            IFeatureDataset pFeaDataSet = _FeatureClass.FeatureDataset;
            string pFeaClsName = (_FeatureClass as IDataset).Name;
            if (pFeaDataSet == null)
            {
                MessageBox.Show("检查数据必须在要素集中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.progressStep.Visible = true;
            progressStep.Minimum = 0;
            progressStep.Maximum = 7;
            progressStep.Step = 1;
            Exception outError = null;

            ClsTopoCheck pTopoCheck = new ClsTopoCheck();
            _ClsTopoCheck = pTopoCheck;
            //创建拓扑
            progressStep.PerformStep();
            this.lblTips.Text = "创建拓扑";
            Application.DoEvents();
            ITopology pTopo = pTopoCheck.CreateTopo(pFeaDataSet, pFeaDataSet.Name, 0, out outError);
            if (outError != null)
            {
                pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);
                progressStep.Visible = false;
                this.lblTips.Text = "创建拓扑失败";
                return;
            }
            esriTopologyRuleType pRuleNoOverLap = esriTopologyRuleType.esriTRTAreaNoOverlap;

            //将要素类添加到拓扑中并创建拓扑规则
            progressStep.PerformStep();
            pTopoCheck.AddRuleandClasstoTopology(pTopo, pFeaDataSet, pFeaClsName, pRuleNoOverLap, out outError);

            if (outError != null)
            {
                pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);
                progressStep.Visible = false;
                this.lblTips.Text = "添加拓扑规则失败";
                return;
            }

            esriTopologyRuleType pRuleNoGap = esriTopologyRuleType.esriTRTAreaNoGaps;

            //将要素类添加到拓扑中并创建拓扑规则
            progressStep.PerformStep();
            this.lblTips.Text = "创建拓扑";
            pTopoCheck.AddRuleandClasstoTopology(pTopo, pFeaDataSet, pFeaClsName, pRuleNoGap, out outError);
            if (outError != null)
            {
                pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);
                progressStep.Visible = false;
                this.lblTips.Text = "添加拓扑规则失败";
                return;
            }

            //拓扑验证范围
            IEnvelope pEnvelop = null;
            pEnvelop = (pFeaDataSet as IGeoDataset).Extent;

            //文字提示
            //验证拓扑
            progressStep.PerformStep();
            this.lblTips.Text = "验证拓扑";
            pTopoCheck.ValidateTopology(pTopo, pEnvelop, out outError);
            if (outError != null)
            {
                pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);
                progressStep.Visible = false;
                this.lblTips.Text = "验证拓扑失败";
                return;
            }
            //文字提示

            DataTable pDataTable= InitDataTale();
            pTopoCheck.DataTable = pDataTable;
            //获得错误列表
            FrmTopoCheckResult pFrm = new FrmTopoCheckResult(_MapControl);
            DevComponents.DotNetBar.Controls.DataGridViewX pDataGridErrs = pFrm.ErrorDataGrid;
            progressStep.PerformStep();
            this.lblTips.Text = "获取错误列表";
            pTopoCheck.GetAreaNoOverlopErrorList(pFeaDataSet, pTopo, pEnvelop, pDataGridErrs, out outError);
            if (outError != null)
            {
                pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);
                progressStep.Visible = false;
                this.lblTips.Text = "获取错误列表失败";
                return;
            }
            pDataGridErrs.DataSource = pDataTable;
            pFrm.ErrDataTable = pDataTable;

            //移除拓扑
            progressStep.PerformStep();
            this.lblTips.Text = "移除拓扑";
            pTopoCheck.RemoveTopo(pFeaDataSet, pFeaDataSet.Name, out outError);//因为移除完无法编辑暂时没找到好办法先不屏蔽   zhangqi 2012-08-10
            progressStep.Visible = false;
            if (outError != null) return;

            
            this.Close();
            pFrm.Show();

        }
        private DataTable InitDataTale()
        {
            DataTable pDt = new DataTable();

            //添加列
            ///检查功能名、检查时间、检查人屏蔽掉  ZQ 20111020 modify
            pDt.Columns.Add("错误类型", typeof(string));
            pDt.Columns.Add("错误描述", typeof(string));
            pDt.Columns.Add("数据图层1", typeof(string));
            pDt.Columns.Add("要素OID1", typeof(string));
            pDt.Columns.Add("数据图层2", typeof(string));
            pDt.Columns.Add("要素OID2", typeof(string));
            pDt.Columns.Add("检查时间", typeof(string));
            pDt.Columns.Add("检查人", typeof(string));
            pDt.Columns.Add("定位点X", typeof(string));
            pDt.Columns.Add("定位点Y", typeof(string));
            pDt.Columns.Add("数据文件名", typeof(string));
            pDt.Columns.Add("错误几何形状", typeof(string));
            pDt.Columns.Add("错误坐标串", typeof(string));
            return pDt;
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void rdbTempData_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTempData.Checked == true)
            {
                btnScanDataPath.Enabled = false;
            }
            else
            {
                btnScanDataPath.Enabled = true;
            }
        }

        private void FrmTopoCheckSet_Load(object sender, EventArgs e)
        {

        }

        private void txtCheckDataPath_Click(object sender, EventArgs e)
        {
            if (rdbTempData.Checked)
            {
 
            }
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FeatureClass = null;
            if (!rdbTempData.Checked)
            {
                if (_WorkSpace != null)
                {
                    string strName = cmbLayer.Text;
                    IFeatureClass pFeatureClass = (_WorkSpace as IFeatureWorkspace).OpenFeatureClass(strName);
                    if (pFeatureClass != null)
                    {
                        _FeatureClass = pFeatureClass;
                    }
                }
            }
            else
            { }
        }
    }
}
