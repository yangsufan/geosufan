using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110914
    /// 说明：标准图幅地图输出设置窗体-专题图
    /// </summary>
    public partial class FrmSheetMapUserSet_ZT : DevComponents.DotNetBar.Office2007Form
    {
        public string MapNo="";
        public int Scale = 50000;
        AxMapControl pAxMapControl;
        Form pMainForm;
        //Plugin.Application.IApplicationRef hook;
        private ESRI.ArcGIS.Geometry.IPoint pPt = null;//大比例尺出图用于确定真实坐标
        private List<DevComponents.AdvTree.Node> dataNode;
        private IFeatureClass xzq_xianFC = null;
        private string xzq_xian_field = null;
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
        public FrmSheetMapUserSet_ZT(AxMapControl inAxMapControl, Form inForm,string tfh, 
            IFeatureClass inXZQ_XIAN,string field)
        {
            InitializeComponent();
            //isOK = false;
            //cBoxScale.SelectedIndex = 0;
            //curMapName = incurMapName;
            pAxMapControl = inAxMapControl;
            xzq_xianFC = inXZQ_XIAN;
            xzq_xian_field = field;
            //hook = inHook;
            pMainForm = inForm;
            pAxMapControl.OnMouseDown += new 
                IMapControlEvents2_Ax_OnMouseDownEventHandler(pAxMapControl_OnMouseDown);//订阅事件
            txtMapNo.Text = tfh;
            List<string> tdlyZTs = ModGetData.GetZT("TDLY");
            foreach (string tdlyzt in tdlyZTs)
            {
                cBoxZT.Items.Add(tdlyzt);
            }
            if (cBoxZT.Items.Count > 0)
                cBoxZT.SelectedIndex = 0;

        }
        //根据图幅号获取比例尺，小比例尺
        private string cScale(string tfh)
        {
            string  chr = tfh.Substring(3, 1);
            string res = "";
            switch (chr)
            {
                case "B": res= "500000";break;
                case "C": res= "250000";break;
                case "D": res= "100000";break;
                case "E": res= "50000";break;
                case "F": res= "25000";break;
                case "G": res= "10000";break;
                case "H": res= "5000";break;

            }
            return res;
 
        }
        private void txtResolution_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled=true;
            if(txtMapNo.Text=="")
                btnOK.Enabled = false;
            try
            {
                string tfh = txtMapNo.Text;
                if (tfh != "" && tfh.Length == 10)
                {
                    int idx = cBoxScale.FindStringExact("1:" + cScale(tfh));
                    if (idx != -1)
                        cBoxScale.SelectedIndex = idx;
                }

            }
            catch { }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMapNo.Text == "" || cBoxScale.Text == "")
                return;
            this.Hide();
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("选择比例尺为:" + cBoxScale.Text + ",图幅号为:" + txtMapNo.Text);
                }
                MapNo = txtMapNo.Text;
                Scale = Convert.ToInt32(cBoxScale.Text.Split(':')[1]);
              
                IMap pMap = null;
                bool isSpecial = ModGetData.IsMapSpecial();
                if (isSpecial)
                {
                    pMap = new MapClass();
                    List<string> lstName = getXZQMC();
                    if (lstName == null || lstName.Count == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "行政区配置无效或图幅号非本地范围。");
                        return;
                    }
                    foreach (string xzq in lstName)
                    {
                        string resXzq = xzq;
                       
                        ModGetData.AddMapOfByXZQ(pMap, "TDLY", cBoxZT.Text, pAxMapControl.Map, resXzq);
                    }
                    if (pMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        return;
                    }
                    ModuleMap.LayersComposeEx(pMap);//图层排序
                }
                else
                {
                    IObjectCopy pOC = new ObjectCopyClass();
                    pMap = pOC.Copy(pAxMapControl.Map) as IMap;//复制地图
                }
                //BzffOutMap_ZT cmdBZTF = new BzffOutMap_ZT(MapNo, Scale,pPt,pMap);//
                GeoPageLayout pPageLayout = new
                 GeoPageLayout(pMap, MapNo, Scale, pPt, 1);
                pPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pPageLayout.typePageLayout = 3;
                pPageLayout.MapOut();

                pPageLayout = null;
            }
            catch
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入正确的图幅号，如G49G077090.");
            }
            finally
            {
                this.Close();
            }
        }
        private void initMap123(IMapLayers pMapLayer, DevComponents.AdvTree.Node advN)
        {
            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        ILayer addLayer =ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                        pMapLayer.InsertLayer(addLayer, false, pMapLayer.LayerCount);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return;
            }
            else if(advN.HasChildNodes)
            {
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    initMap(pMapLayer, avN); 
                }
            }

        }


        private void initMap(IMapLayers pMapLayer, DevComponents.AdvTree.Node advN)
        {
            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        ILayer addLayer = ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                        pMapLayer.InsertLayer(addLayer, false, pMapLayer.LayerCount);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return;
            }
            else if (advN.HasChildNodes)
            {
                List<string> lstName = getXZQMC();
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    if (lstName.Contains(avN.Text.Split('_')[0]))
                    {
                        ILayer pLyr=getLayer(avN);
                        if(pLyr!=null)
                            pMapLayer.InsertLayer(pLyr, false, pMapLayer.LayerCount); 
                    }
                }
            }

        }
        private ILayer getLayer(DevComponents.AdvTree.Node advN)//支持两层结构递归调用
        {

            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                ILayer addLayer=null;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        addLayer = ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return addLayer;
            }
            else if (advN.HasChildNodes)
            {
                IGroupLayer pGpLayer = new GroupLayerClass();
                pGpLayer.Name = advN.Text;
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    ILayer pLyr = getLayer(avN);
                    if (pLyr != null)
                        pGpLayer.Add(pLyr);
                }
                return pGpLayer as ILayer;
            }
            return null;
        }
        private List<string> getXZQMC()
        {
            List<string> res=new List<string>();
            GeoDrawSheetMap.clsGetGeoInfo getGeoinfo = new GeoDrawSheetMap.clsGetGeoInfo();
            getGeoinfo.m_lngMapScale = (long)Convert.ToInt32(cBoxScale.Text.Split(':')[1]);
            getGeoinfo.m_strMapNO = txtMapNo.Text.Replace(" ","");
            getGeoinfo.m_intInsertCount = 3;
            getGeoinfo.m_pPrjCoor = pAxMapControl.Map.SpatialReference as IProjectedCoordinateSystem;
            getGeoinfo.ComputerAllGeoInfo();
            IGeometry tfGeometry = getGeoinfo.m_pSheetMapGeometry;
            //GeoPageLayoutFn.drawPolygonElement(tfGeometry, pAxMapControl.ActiveView.GraphicsContainer);//测试用
            IRelationalOperator pRO = tfGeometry as IRelationalOperator;
            IFeatureCursor pFCursor = xzq_xianFC.Search(null, false);
            IFeature pFeature = pFCursor.NextFeature();
            int idx=pFeature.Fields.FindField(xzq_xian_field);
            while (pFeature != null)
            {
                IGeometry pGm = pFeature.ShapeCopy;
                if (pRO.Relation(pGm,"RELATE(G1,G2,'T********')"))//(pRO.Within(pGm) || pRO.Overlaps(pGm)怀疑此方法有问题 || pRO.Contains(pGm))
                {
                    //GeoPageLayoutFn.drawPolygonElement(pGm, pAxMapControl.ActiveView.GraphicsContainer);//测试用
                    res.Add(getXZQMC(pFeature.get_Value(idx).ToString()));
                }
                pFeature = pFCursor.NextFeature();
            }
            //pAxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);//测试用
            return res;
 
        }
        private string getXZQMC(string xzqCode)
        {
            string schemaPath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
            if (!File.Exists(schemaPath))
            {
                return null;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(schemaPath);
                XmlNode xn = cXmlDoc.SelectSingleNode("//County[@XzqCode='"+xzqCode+"']");
                if (xn != null)
                {
                    string name = xn.Attributes["ItemName"].Value;
                    cXmlDoc = null;
                    return name;
                }
                    
            }
            return null;

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消标准分幅出图");
            }
            this.Close();
        }

        
        private void pAxMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            pPt = new ESRI.ArcGIS.Geometry.PointClass();
            pPt.PutCoords(e.mapX, e.mapY);
            Scale = Convert.ToInt32(cBoxScale.Text.Split(':')[1]);
            if (Scale > 2000)
            {
                WKSPoint pPoint = new WKSPoint();
                pPoint.X = e.mapX;
                pPoint.Y = e.mapY;
                if (pAxMapControl.MapUnits == esriUnits.esriMeters)
                {
                    (pAxMapControl.SpatialReference as IProjectedCoordinateSystem).Inverse(1, ref pPoint);//求点投影的经纬度坐标

                }
                GeoDrawSheetMap.basPageLayout.GetNewCodeFromCoordinate(ref MapNo, (long)(pPoint.X * 3600), (long)(pPoint.Y * 3600),
                    Convert.ToInt32(cBoxScale.Text.Split(':')[1]));
                MapNo=MapNo.Insert(3, " ");//H50 G 054071
                MapNo=MapNo.Insert(5, " ");
                txtMapNo.Text = MapNo;
                this.Show(pMainForm);
            }
            else
            {
                txtMapNo.Text = getMapNOforBigScale(Scale, pPt);
                this.Show(pMainForm);  
            }
        }

        private string getMapNOforBigScale(int inScale,ESRI.ArcGIS.Geometry.IPoint inPoint)
        {
            double x = inPoint.X;
            double y = inPoint.Y;
            if (inScale == 500)
            {
                int dwx = (int)x % 1000;
                int dwy = (int)y % 1000;
                string strDwx = "";
                if (dwx >= 0 && dwx < 250)
                    strDwx = ".00";
                else if (dwx >= 250 && dwx < 500)
                    strDwx = ".25";
                else if (dwx >= 500 && dwx < 750)
                    strDwx = ".50";
                else if (dwx >= 750)
                    strDwx = ".75";
                int midx = (int)x % 100000 / 1000;
                string strTfhX = midx.ToString("0#") + strDwx;//
                //以上是求X的图幅号
                //以下是求Y的
                string strDwy = "";
                if (dwy >= 0 && dwy < 250)
                    strDwy = ".00";
                else if (dwy >= 250 && dwy < 500)
                    strDwy = ".25";
                else if (dwy >= 500 && dwy < 750)
                    strDwy = ".50";
                else if (dwy >= 750)
                    strDwy = ".75";
                int midy = (int)y % 100000 / 1000;
                string strTfhY = midy.ToString("0#") + strDwy;
                return strTfhY + "-" + strTfhX;//返回的图幅号,图号XY和真实是相反的
            }
            else if (inScale == 1000)
            {
                int dwx = (int)x % 1000;
                int dwy = (int)y % 1000;
                string strDwx = "";
                if (dwx >= 0 && dwx < 500)
                    strDwx = ".00";
                else if (dwx >= 500)
                    strDwx = ".50";
                int midx = (int)x % 100000 / 1000;
                string strTfhX = midx.ToString("0#") + strDwx;
                //以上是求X的图幅号
                //以下是求Y的
                string strDwy = "";
                if (dwy >= 0 && dwy < 500)
                    strDwy = ".00";
                else if (dwy >= 500)
                    strDwy = ".50";
                int midy = (int)y % 100000 / 1000;
                string strTfhY = midy.ToString("0#") + strDwy;
                return strTfhY + "-" + strTfhX;//返回的图幅号,图号XY和真实是相反的
            }
            else if (inScale == 2000)
            {
                int dwx = (int)x % 1000;
                int dwy = (int)y % 1000;
                string strDwx = ".00";
                int midx = (int)x % 100000 / 1000;
                string strTfhX = midx.ToString("0#") + strDwx;
                //以上是求X的图幅号
                //以下是求Y的
                string strDwy = ".00";
                int midy = (int)y % 100000 / 1000;
                string strTfhY = midy.ToString("0#") + strDwy;
                return strTfhY + "-" + strTfhX;//返回的图幅号,图号XY和真实是相反的
            }
            return "";
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pPt == null)
                return;
            Scale = (cBoxScale.Text.Contains(":"))?Convert.ToInt32(cBoxScale.Text.Split(':')[1]):0;
            if (Scale > 2000)
            {
                WKSPoint pPoint = new WKSPoint();
                pPoint.X = pPt.X;
                pPoint.Y = pPt.Y;
                if (pAxMapControl.Map.SpatialReference is IProjectedCoordinateSystem)
                {
                    (pAxMapControl.SpatialReference as IProjectedCoordinateSystem).Inverse(1, ref pPoint);//求点投影的经纬度坐标
                }
                GeoDrawSheetMap.basPageLayout.GetNewCodeFromCoordinate(ref MapNo, (long)(pPoint.X * 3600), (long)(pPoint.Y * 3600),
                    Convert.ToInt32(cBoxScale.Text.Split(':')[1]));
                MapNo = MapNo.Insert(3, " ");//H50 G 054071
                MapNo = MapNo.Insert(5, " ");
                txtMapNo.Text = MapNo;
              
            }
            if(Scale>400&&Scale<=2000)
            {
                txtMapNo.Text = getMapNOforBigScale(Scale, pPt);
              
            }
        }

        private void FrmSheetMapUserSet_Load(object sender, EventArgs e)
        {
            try
            {
                cBoxScale.Items.Add("1:10000");
                if (cBoxScale.Items.Count > 0)
                    cBoxScale.SelectedIndex = 0;
                //initcBoxScale();
                try
                {
                    string tfh = txtMapNo.Text;
                    if (tfh != "" && tfh.Length == 10)
                    {
                        int idx = cBoxScale.FindStringExact("1:" + cScale(tfh));
                        if (idx != -1)
                            cBoxScale.SelectedIndex = idx;
                        else
                        {
                            int ix = cBoxScale.Items.Add("1:" + cScale(tfh));
                            cBoxScale.SelectedIndex = ix;
                        }
                    }

                }
                catch { }
                pAxMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//设置地图控件的鼠标指针
            }
            catch
            {
 
            }
        }
        //初始化比例尺列表控件
        private void initcBoxScale()
        {
            cBoxScale.Items.Clear();
          string schemaPath = Application.StartupPath + "\\..\\Res\\Xml\\TFHQuery.xml";
            if (!File.Exists(schemaPath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(schemaPath);

                XmlNode xn1 = cXmlDoc.FirstChild;
                XmlNode xn2 = xn1.NextSibling;
                foreach (XmlNode xn in xn2.ChildNodes)
                {
                    string xnattr = xn.Attributes["ItemName"].Value;
                    cBoxScale.Items.Add(xnattr);
                }
                cXmlDoc = null;
                if (cBoxScale.Items.Count > 0)
                    cBoxScale.SelectedIndex = 0;
                else
                    label1.Text = "请检查配置文件\\Res\\Xml\\TFHQuery.xml";
            }
        }
        private void FrmSheetMapUserSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                pAxMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            catch
            {

            }
        }

    }
}
