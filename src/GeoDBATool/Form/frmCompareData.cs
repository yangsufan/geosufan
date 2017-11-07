using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public partial class frmCompareData :DevComponents.DotNetBar.Office2007Form
    {
        //IHookHelper m_HookHelp = null;

        Plugin.Application.IAppGISRef m_Hook=null;
        public frmCompareData(Plugin.Application.IAppGISRef pHook, Dictionary<string, IFeatureClass> dicSelFeaClsInfo)
        {
            InitializeComponent();

            m_Hook = pHook;

            if (dicSelFeaClsInfo == null) return;
            InitialMapControl(dicSelFeaClsInfo);

            ModData.m_orgMap = axMapControl1.Map;

            ModData.m_Mapcontrol = axMapControl1;
        }

        /// <summary>
        /// 初始化mapControl控件
        /// </summary>
        /// <param name="layerList"></param>
        private void InitialFrm(List<IFeatureLayer> layerList)
        {
          
        }

        /// <summary>
        /// 初始化时间列表框和时间trackbar
        /// </summary>
        /// <param name="mapcontrol"></param>
        private void InitialtrsckbarTime(IMapControlDefault mapcontrol)
        {
            cmbTime.Items.Clear();

            ArrayList arrayListFromDate = new ArrayList();
            //计算各历史图层的fromdate字段唯一值
            try
            {
                for (int i = 0; i < mapcontrol.Map.LayerCount; i++)
                {
                    IFeatureLayer featLay = mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    ITable table = featLay.FeatureClass as ITable;
                    IDataStatistics statistics = new DataStatisticsClass();
                    statistics.Cursor = table.Search(null, false);
                    statistics.Field = "FromDate";
                    IEnumerator enumDate = statistics.UniqueValues;
                    enumDate.Reset();
                    while (enumDate.MoveNext())
                    {
                        if (!arrayListFromDate.Contains(enumDate.Current))
                        {
                            arrayListFromDate.Add(enumDate.Current);
                        }
                    }
                }
            }
            catch
            { }

            //组合形成时间段
            if (arrayListFromDate.Count > 0)
            {
                arrayListFromDate.Sort();
                for (int i = 0; i < arrayListFromDate.Count; i++)
                {
                    cmbTime.Items.Add(arrayListFromDate[i]);
                }
                try
                {
                    cmbTime.SelectedIndex = arrayListFromDate.Count - 1;
                    trsckbarTime.Maximum = arrayListFromDate.Count;
                    trsckbarTime.Minimum = 1;
                    trsckbarTime.Value = arrayListFromDate.Count;
                    trsckbarTime.Tag = arrayListFromDate;
                }
                catch
                { }
            }
        }

        //初始化mapcontrol
        private void InitialMapControl(Dictionary<string, IFeatureClass> dicSelFeaClsInfo)
        {
            //遍历要素类信息，将要素类加载在面板上
            foreach (KeyValuePair<string, IFeatureClass> feaclsItem in dicSelFeaClsInfo)
            {
                IFeatureLayer pFeaLayer = new FeatureLayerClass();
                IFeatureClass pFeaCls = feaclsItem.Value;
                pFeaLayer.FeatureClass = pFeaCls;

                axMapControl1.Map.AddLayer(pFeaLayer as ILayer);
            }
            axMapControl1.Extent = axMapControl1.FullExtent;// m_Hook.ArcGisMapControl.Extent;
            axMapControl1.ActiveView.Refresh();

            InitialtrsckbarTime(this.axMapControl1.Object as IMapControlDefault);
        }


        private void ChangeLayersDef()
        {
            for (int i = 0; i < axMapControl1.Map.LayerCount; i++)
            {
                IFeatureLayer featLay = axMapControl1.Map.get_Layer(i) as IFeatureLayer;
                IFeatureLayerDefinition featLayDef = featLay as IFeatureLayerDefinition;
                featLayDef.DefinitionExpression = "FromDate<='" + cmbTime.Items[cmbTime.SelectedIndex].ToString() + "' and ToDate>'" + cmbTime.Items[cmbTime.SelectedIndex].ToString() + "'";
            }

            axMapControl1.ActiveView.Refresh();
        }


        private void axMapControl1_OnExtentUpdated(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {
           
            
        }

        private void axMapControl1_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            //IEnvelope pReferFuulEx = axMapControl1.FullExtent;
            //IEnvelope pReferEnvelop = axMapControl1.Extent;
            //IEnvelope pMainFullEx = m_Hook.ArcGisMapControl.ActiveView.FullExtent;// m_HookHelp.ActiveView.FullExtent;

            //IEnvelope pMainEx = new EnvelopeClass();

            //pMainEx.XMin = pMainFullEx.XMin + (pReferEnvelop.XMin - pReferFuulEx.XMin) / pReferFuulEx.Width * pMainFullEx.Width;
            //pMainEx.XMax = pMainFullEx.XMax - (pReferFuulEx.XMax - pReferEnvelop.XMax) / pReferFuulEx.Width * pMainFullEx.Width;
            //pMainEx.YMin = pMainFullEx.YMin + (pReferEnvelop.YMin - pReferFuulEx.YMin) / pReferFuulEx.Height * pMainFullEx.Height;
            //pMainEx.YMax = pMainFullEx.YMax - (pReferFuulEx.YMax - pReferEnvelop.YMax) / pReferFuulEx.Height * pMainFullEx.Height;

            //m_Hook.ArcGisMapControl.Extent = pMainEx;
            //m_Hook.ArcGisMapControl.ActiveView.Refresh();
        }

        private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTime.Items.Count > 0)
            {
                trsckbarTime.Value = cmbTime.SelectedIndex + 1;
                ChangeLayersDef();
            }
        }

        private void trsckbarTime_ValueChanged(object sender, EventArgs e)
        {
            if (cmbTime.Items.Count > 0)
            {
                cmbTime.SelectedIndex = trsckbarTime.Value - 1;
            }
        }

        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            //m_HookHelp.ActiveView.Refresh();
            //m_Hook.MapControl.ActiveView.Refresh();
        }

        private void frmCompareData_FormClosed(object sender, FormClosedEventArgs e)
        {
            ModData.UpDataCompareFrm = null;
        }
    }
}
