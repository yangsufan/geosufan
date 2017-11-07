using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
/////ZQ 20110、1008  add  
namespace GeoPageLayout
{
    public partial class FrmBlockOutMapFFT : DevComponents.DotNetBar.Office2007Form
    {
        /// <summary>
        /// 存储符合要求的图形范围要素及地块要素
        /// </summary>
        //public Dictionary<IGeometry, List<IFeature>> m_QueryResult = null;
        public Dictionary<string, List<int>> m_QueryResult = null;
        public string OutputPath = "";
        public FrmBlockOutMapFFT()
        {
            InitializeComponent();
        }
        private IFeatureClass _extentFC=null;
        public IFeatureClass ExtentFC
        {
            get { return _extentFC;}
        }
        private void bttImport_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = false;
            sOpenFileD.Title = "选择地块范围数据";
            sOpenFileD.Filter = "(shp文件夹)|*.shp";
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
              txtPath.Text = sOpenFileD.FileName;
            }
               
        }

        private void bttOK_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "") { MessageBox.Show("请导入块图范围数据！","提示！"); return; }
            if (txtOutPath.Text == "") { MessageBox.Show("请选择输出的文件夹！", "提示！"); return; }
            if(!System.IO.File.Exists(txtPath.Text)){MessageBox.Show("导入的块图范围数据不存在！","提示！");return;}
            OutputPath = txtOutPath.Text;
            IFeatureClass pFeatureClass = GetFeatureClass(txtPath.Text);
            _extentFC=pFeatureClass;
            if (pFeatureClass == null) { MessageBox.Show("未找到指定路径下的要素！","提示！"); return; }
            IGeometry pGeometry;
            //存储所有已经输出过的要素
            List<int> pLstFeature = new List<int>();
            m_QueryResult = new Dictionary<string, List<int>>();
            //获得整个图层的游标
            IFeatureCursor pFeatureCursor = GetFeatureCursor(pFeatureClass, null, null, esriSpatialRelEnum.esriSpatialRelUndefined);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while(pFeature!=null)
            {
                if (!pLstFeature.Contains(pFeature.OID))
                {
                   
                    double x = 470,
                        y = 250;
                    ///需要调整矩形框范围大小
                    ///
                    string mapNo = "";
                    pGeometry = SetGeometry(pFeature,ref mapNo);
                    /////////

                    //存储pGeometry包含的要素却未输出过的
                    //List<IFeature> vLstFeature = new List<IFeature>();
                    List<int> vLstFeature = new List<int>();
                    //获取pGeometry包含要素的游标
                    IFeatureCursor vFeatureCursor = GetFeatureCursor(pFeatureClass, null, pGeometry, esriSpatialRelEnum.esriSpatialRelContains);
                    IFeature vFeature = vFeatureCursor.NextFeature();
                    while (vFeature != null)
                    {
                        if (!vLstFeature.Contains(vFeature.OID))
                        {
                            vLstFeature.Add(vFeature.OID);
                            pLstFeature.Add(vFeature.OID);
                        }
                        vFeature = vFeatureCursor.NextFeature();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(vFeatureCursor);
                    if (vLstFeature.Count != 0)
                    {
                        //添加批量出图要素集合
                        if (!m_QueryResult.ContainsKey(mapNo))
                            m_QueryResult.Add(mapNo, vLstFeature);
                    }
                }
            
                pFeature = pFeatureCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            this.DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// 通过指定的路径获得FeatureClass
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns></returns>
        private IFeatureClass GetFeatureClass(string strPath)
        {
            IFeatureClass pFeatureClass = null;
            IWorkspaceFactory  pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace =null;
            try
            {
                pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(strPath), 0) as IFeatureWorkspace;
            }
            catch
            { return pFeatureClass = null; }
            try
            {
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(strPath));
            }
            catch { return pFeatureClass = null; }
            return pFeatureClass;
        }
        /// <summary>
        /// 根据属性条件和空间条件获得查询结果游标
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <param name="pWhereClause"></param>
        /// <param name="pGeometry"></param>
        /// <param name="pesriSpatialRelEnum"></param>
        /// <returns></returns>
        private IFeatureCursor GetFeatureCursor(IFeatureClass pFeatureClass, string pWhereClause, IGeometry pGeometry, esriSpatialRelEnum pesriSpatialRelEnum)
        {
            IFeatureCursor pFeatureCursor = null;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            try
            {
                if (pGeometry != null)
                {
                    pSpatialFilter.Geometry = pGeometry;
                    pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;
                    pSpatialFilter.SpatialRel = pesriSpatialRelEnum;
                }
                pSpatialFilter.WhereClause = pWhereClause;
                pFeatureCursor = pFeatureClass.Search(pSpatialFilter, false);

                return pFeatureCursor;
            }
            catch
            {
                return pFeatureCursor;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSpatialFilter);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
         
        }
        /// <summary>
        /// 设置矩形框范围
        /// </summary>
        /// <param name="pFeature"></param>
        /// <param name="XLength">X轴方向长度二分之一</param>
        /// <param name="YLength">Y轴方向长度二分之一</param>
        /// <returns></returns>
        private IGeometry SetGeometry(IFeature pFeature,ref string mapNO)
        {
            IGeometry res = null;
            //pGeometry = new EnvelopeClass();
            IEnvelope penvelope = new EnvelopeClass();
            ISpatialReference pSpatialRefrence = pFeature.ShapeCopy.SpatialReference;
            string mapNo = "";
            if (pSpatialRefrence is IProjectedCoordinateSystem)
            {
                IGeographicCoordinateSystem pGCS = (pSpatialRefrence as IProjectedCoordinateSystem).GeographicCoordinateSystem;
                IArea pArea = pFeature.ShapeCopy as IArea;
                IPoint pCPoint = pArea.Centroid;
                double difX = 1 * 3600 / 16,
                         difY = 1 * 3600 / 24;
           
            
                IProjectedCoordinateSystem pPCS = pSpatialRefrence as IProjectedCoordinateSystem;
                WKSPoint pPointMin = new WKSPoint();
                pPointMin.X = pCPoint.X;
                pPointMin.Y = pCPoint.Y;
                pPCS.Inverse(1, ref pPointMin);
                double minX = Math.Floor(pPointMin.X * 3600 / difX) * difX;
                double minY = Math.Floor(pPointMin.Y * 3600 / difY) * difY;
                
                long lScale = 10000;
                GeoDrawSheetMap.basPageLayout.GetNewCodeFromCoordinate(ref mapNo, (long)(pPointMin.X * 3600), (long)(pPointMin.Y * 3600), lScale);
                //GeoDrawSheetMap.basPageLayout.GetNewCodeFromCoordinate(ref mapNo, (long)(minX), (long)(minY+3), lScale);
                penvelope.PutCoords(minX / 3600, minY / 3600, (minX + difX) / 3600, (minY + difY) / 3600);
                penvelope.SpatialReference = pGCS;
                penvelope.Project(pSpatialRefrence);
               
                res= penvelope;
            }

            mapNO = mapNo;
            return res;

        }
        /// <summary>
        /// 判断要素是否被输出过
        /// </summary>
        /// <param name="vLstFeature"></param>
        /// <param name="vFeature"></param>
        /// <returns></returns>
        private bool IsExist(List<IFeature> vLstFeature,IFeature vFeature)
        {
            bool IsExist =false;
            if (vLstFeature.Count == 0) { return IsExist = false; }
            for (int i = 0; i < vLstFeature.Count;i++ )
            {
                if(vLstFeature[i].OID==vFeature.OID)
                {
                    IsExist = true;
                    return IsExist;
                }
            }
            return IsExist;
        }

        private void btnOutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFBD = new FolderBrowserDialog();
            if(pFBD.ShowDialog(this)!=DialogResult.OK)
                return;
            txtOutPath.Text = pFBD.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmBlockOutMap_Load(object sender, EventArgs e)
        {
            
        }
        private void getWHfromID(string inPaper,ref int W,ref int H)
        {
            switch (inPaper)
            {
                case "A0":
                    W = 841;
                    H = 1189;
                    break;
                case "A1":
                    W = 594;
                    H = 841;
                    break;
                case "A2":
                    W = 420;
                    H = 594;
                    break;
                case "A3":
                    W = 297;
                    H = 420;
                    break;
                case "A4":
                    W = 210;
                    H = 297;
                    break;
                case "A5":
                    W = 148;
                    H = 210;
                    break;

            }
 
        }
    }
}
