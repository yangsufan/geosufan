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
using ESRI.ArcGIS.Display;


//通视分析      张琪    20110618
namespace GeoArcScene3DAnalyse
{
    public partial class frm3DLineOfSight : DevComponents.DotNetBar.Office2007Form
    {
        private double m_obsOffset;//观察者高度
        private double m_tarOffset;//被观察点高度
        public ILayer m_Layer;//当前选中的图层
        public ISurface m_Surface;//当前获取的Surface
        public Tool3DLineOfSight pTool3DLineOfSight = null;
        public frm3DLineOfSight()
        {
            InitializeComponent();
        }
        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl//获取sceneControl 的值 
        {
            set { m_pCurrentSceneControl = value; }                  //张琪
        }
        public void Init(double obsOffset, double tarOffset, Tool3DLineOfSight theform)
        {
            pTool3DLineOfSight = theform;
              m_obsOffset = obsOffset;//观察点基于地表的高度
              m_tarOffset = tarOffset;//被观察点基于地表的高度
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
                    if (pLayer is ITinLayer)
                    {
                        comboBoxOpen.Items.Add(pLayer.Name);
                        //comboBoxOpen_SelectedIndexChanged();

                    }
                    //else if(pLayer is IRasterLayer)
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
        private void frm3DLineOfSight_Load(object sender, EventArgs e)

        {
            this.Icon = null;
            txtObsOffset.Text =Math.Round(m_obsOffset, 2).ToString();
            txtTarOffset.Text = Math.Round(m_tarOffset, 2).ToString();
            checkBoxCurv.Checked = false;

        }
        private void comboBoxOpen_DropDown(object sender, EventArgs e)
        {
            comboBoxOpen.Items.Clear();                               
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is ITinLayer)
                    {
                        comboBoxOpen.Items.Add(pLayer.Name);
                    }
                    //else if (pLayer is IRasterLayer)
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
        /// <summary>
        /// 获取分析图层及Surfer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOpen_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxOpen.Items.Count > 0)
            {
               string  LayerName = comboBoxOpen.SelectedItem.ToString();
                m_Layer  = GetLayerByName(ref LayerName);
                if (m_Layer is IRasterLayer)//读取Raster数据的ISurface
                {
                    IRasterLayer pRasterLayer = m_Layer as IRasterLayer;
                    IRasterSurface pRasterSurface = new RasterSurfaceClass();
                    pRasterSurface.PutRaster(pRasterLayer.Raster,0);
                    m_Surface = pRasterSurface as ISurface;

                }
                else if (m_Layer is ITinLayer)//读取TIN数据的ISurface
                {
                    ITinLayer pTinLayer = m_Layer as ITinLayer;
                    m_Surface = pTinLayer.Dataset as ISurface;
                }
                if (m_Surface == null)
                {
                    MessageBox.Show("请选择需要进行分析的DEM数据", "提示！");
                    return;
                }
                pTool3DLineOfSight.m_Layer =m_Layer;//将当前选中的图层传给通视分析工具
                pTool3DLineOfSight.m_SurFace = m_Surface;//将当前获取的Surface传给通视分析工具
                //下面是根据获取的Surface来自动判断是否应用曲率和折射校正
                bool bCurvEnabled;
                bCurvEnabled = false;
                ISurface pSurface = m_Surface;
                IGeoDataset pGeoDataset;
                if (pSurface is ITin)
                {
                    pGeoDataset = pSurface as IGeoDataset;
                }
                else
                {
                    IRasterSurface pRasterSurf = pSurface as IRasterSurface;
                    pGeoDataset = pRasterSurf.RasterBand.RasterDataset as IGeoDataset;
                }
                ISpatialReference pSpatialReference = pGeoDataset.SpatialReference;
                ILinearUnit pLinearUnit = pSpatialReference.ZCoordinateUnit;
                if (pLinearUnit != null)
                {
                    if (pSpatialReference is IProjectedCoordinateSystem)//当投影ProjectedCoordinateSystem时使用应用曲率和折射校正
                    {
                        bCurvEnabled = true;
                    }
                }
              checkBoxCurv.Checked = bCurvEnabled;
          
            }
        }
        /// <summary>
        /// 限制用户只能输入"0123456789."
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCellSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void frm3DLineOfSight_FormClosing(object sender, FormClosingEventArgs e)
        {
            //当窗口关闭时清除绘制的要素
            IGraphicsContainer3D pGCon3D = m_pCurrentSceneControl.Scene.BasicGraphicsLayer as IGraphicsContainer3D;
            pGCon3D.DeleteAllElements();
            m_pCurrentSceneControl.SceneGraph.RefreshViewers();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pGCon3D);
            m_pCurrentSceneControl.CurrentTool = null;//释放使用工具
            this.Dispose();
        }

      

    
     
    }
}
