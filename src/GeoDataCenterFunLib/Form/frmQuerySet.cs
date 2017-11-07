using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataCenterFunLib
{
    //查询类型
    public enum queryType
    {
        Cross = 1,
        Within = 2,
        Contain = 3,
        Intersect=4
    }

    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：相交、落入和包含查询设置窗体
    /// </summary>
    public partial class frmQuerySet : DevComponents.DotNetBar.Office2007Form
    {
       
        //全局公开变量
        public queryType QueryType;
        public List<ILayer> lstQueryedLayer;//被查询的图层集合
        public IGeometry GeometryBag;//搜索图形
        //全局内部变量
        private IMap pMap;
        private List<ILayer> lstSelectLayer;
        
        
        //构造函数
        public frmQuerySet(IMap inMap,queryType inQType)
        {
            InitializeComponent();
            rdSelect.Checked = true;
            QueryType = inQType;
            pMap = inMap;
            initForm();
            initControls();
            //m_list = list;
            //m_listview = listview;
        }
        //初始化窗体
        private void initForm()
        {
            switch (QueryType)
            {
                case queryType.Cross :
                    this.Text = "穿越查询设置";
                    break;
                case queryType.Contain:
                    this.Text = "包含查询设置";
                    break;
                case queryType.Intersect:
                    this.Text = "相交查询设置";
                    break;
                case queryType.Within:
                    this.Text = "落入查询设置";
                    break;
            } 
        }
        //初始化控件
        private void initControls()  
        {
            lstLayer.Items.Clear();
            lstQueryedLayer = new List<ILayer>();
            lstSelectLayer = new List<ILayer>();
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    IFeatureLayer pFL = pMap.get_Layer(i) as IFeatureLayer;
                    if (pFL.FeatureClass.FeatureType==esriFeatureType.esriFTSimple && pFL.Valid && pFL.Visible)//若有效、可见和可选
                    {
                        if (pFL.Selectable)
                        {
                            switch (QueryType)
                            {
                                case queryType.Cross :
                                    lstSelectLayer.Add(pFL);//加入查询图层集合
                                    cboxSearchLayer.Items.Add(pFL.Name);
                                    break;
                                case queryType.Contain:
                                    if (pFL.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                    {
                                        lstSelectLayer.Add(pFL);//加入查询图层集合
                                        cboxSearchLayer.Items.Add(pFL.Name);
                                    }
                                    break;
                                case queryType.Intersect:
                                    lstSelectLayer.Add(pFL);//加入查询图层集合
                                    cboxSearchLayer.Items.Add(pFL.Name);
                                    break;
                                case queryType.Within:
                                    lstSelectLayer.Add(pFL);//加入查询图层集合
                                    cboxSearchLayer.Items.Add(pFL.Name);
                                    break;
                            }
                            
                        }//若可选
                        switch (QueryType)
                        {
                            case queryType.Cross :
                                lstQueryedLayer.Add(pFL);//加入被查询图层集合
                                break;
                            case queryType.Contain:
                                lstQueryedLayer.Add(pFL);//加入被查询图层集合
                                break;
                            case queryType.Intersect:
                                lstQueryedLayer.Add(pFL);//加入被查询图层集合
                                break;
                            case queryType.Within:
                                if (pFL.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    lstQueryedLayer.Add(pFL);//加入被查询图层集合
                                }
                                break;
                        }
                        
                    }//若有效可见

                }//若是要素层
                else if (pMap.get_Layer(i) is IGroupLayer)
                {
                    ICompositeLayer pCL = pMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < pCL.Count; j++)
                    {
                        if (pCL.get_Layer(j) is IFeatureLayer)
                        {
                            IFeatureLayer pFL1 = pCL.get_Layer(j) as IFeatureLayer;
                            if (pFL1.FeatureClass.FeatureType == esriFeatureType.esriFTSimple && pFL1.Valid && pFL1.Visible)//若有效、可见和可选
                            {
                                if (pFL1.Selectable)
                                {
                                    switch (QueryType)
                                    {
                                        case queryType.Cross :
                                            lstSelectLayer.Add(pFL1);//加入查询图层集合
                                            cboxSearchLayer.Items.Add(pFL1.Name);
                                            break;
                                        case queryType.Contain:
                                            if (pFL1.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                            {
                                                lstSelectLayer.Add(pFL1);//加入查询图层集合
                                                cboxSearchLayer.Items.Add(pFL1.Name);
                                            }
                                            break;
                                        case queryType.Intersect:
                                            lstSelectLayer.Add(pFL1);//加入查询图层集合
                                            cboxSearchLayer.Items.Add(pFL1.Name);
                                            break;
                                        case queryType.Within:
                                            lstSelectLayer.Add(pFL1);//加入查询图层集合
                                            cboxSearchLayer.Items.Add(pFL1.Name);
                                            break;
                                    }

                                }//若可选
                                switch (QueryType)
                                {
                                    case queryType.Cross :
                                        lstQueryedLayer.Add(pFL1);//加入被查询图层集合
                                        break;
                                    case queryType.Contain:
                                        lstQueryedLayer.Add(pFL1);//加入被查询图层集合
                                        break;
                                    case queryType.Intersect:
                                        lstQueryedLayer.Add(pFL1);//加入被查询图层集合
                                        break;
                                    case queryType.Within:
                                        if (pFL1.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                        {
                                            lstQueryedLayer.Add(pFL1);//加入被查询图层集合
                                        }
                                        break;
                                }

                            }//若有效可见
 
                        }
                    }
                }
            }//for循环
            if (cboxSearchLayer.Items.Count > 0)
                cboxSearchLayer.SelectedIndex = 0;
            //初始化被查询层listview，使不包括查询层
            lstLayer.Items.Clear();
            foreach (ILayer pLayer in lstQueryedLayer)
            {
                if (pLayer.Name != cboxSearchLayer.Text)
                    lstLayer.Items.Add(pLayer.Name).Tag = pLayer;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstLayer.Items)
            {
                if (lvi.Checked == true)
                {
                    lvi.Checked = false;
                }
                else
                {
                    lvi.Checked = true;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //生成被查询的图层集以传入查询结果窗体
            lstQueryedLayer = new List<ILayer>();
            foreach (ListViewItem lvi in lstLayer.Items)
            {
                if (lvi.Checked == true)
                {
                    lstQueryedLayer.Add(lvi.Tag as ILayer);
                }
            }
            //生成选择的查询图层的搜索图形
            IFeatureLayer pFL=null;
            foreach (ILayer pLayer in lstSelectLayer)
            {
                if (pLayer.Name == cboxSearchLayer.Text)
                    pFL = pLayer as IFeatureLayer;
            }
            IGeometryBag gmBag = new GeometryBagClass();
            gmBag.SpatialReference=pMap.SpatialReference;//定义空间参考，否则加入的图形将失去参考
            IGeometryCollection gmCollection = gmBag as IGeometryCollection;
            if (rdSelect.Checked)//如果单选框 选择的要素是选中状态
            {
                ISelectionSet pSelSet = (pFL as IFeatureSelection).SelectionSet;
                ICursor pCursor;
                pSelSet.Search(null, false, out pCursor);
                IFeature pFeature = (pCursor as IFeatureCursor).NextFeature();
                object obj = Type.Missing;
                while (pFeature != null)
                {
                    gmCollection.AddGeometry(pFeature.ShapeCopy, ref obj, ref obj);
                    pFeature = (pCursor as IFeatureCursor).NextFeature();
                }
            }
            else//如果单选框 全部要素是选择状态
            {
                IFeatureCursor pFeaCursor = pFL.FeatureClass.Search(null, false);
                IFeature pFea = pFeaCursor.NextFeature();
                object obj = Type.Missing;
                while (pFea != null)
                {
                    gmCollection.AddGeometry(pFea.ShapeCopy, ref obj, ref obj);
                    pFea = pFeaCursor.NextFeature();
                }
            }
            ISpatialIndex pSI = gmBag as ISpatialIndex;//重建索引
            pSI.AllowIndexing = true;
            pSI.Invalidate();
            GeometryBag = gmBag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstLayer.Items)
            {
                lvi.Checked = true;
            }
        }

        private void cboxSearchLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxSearchLayer.Text== "")
                return;
            //初始化listView，使不包括查询层
            lstLayer.Items.Clear();
            foreach (ILayer pLayer in lstQueryedLayer)
            {
                if (pLayer.Name != cboxSearchLayer.Text)
                    lstLayer.Items.Add(pLayer.Name).Tag = pLayer;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}