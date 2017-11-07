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
namespace GeoPageLayout
{
    public enum SheetType
    {
        foundationTerrain,//基础地形图
        urbanCadastre//城镇地籍图

    }
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.28
    /// 说明：标准图幅地图输出设置窗体
    /// </summary>
    public partial class FrmSheetMapUserSet : DevComponents.DotNetBar.Office2007Form
    {
        public string MapNo="";
        public int Scale = 50000;
        AxMapControl pAxMapControl;
        Form pMainForm;
        Plugin.Application.IApplicationRef hook;
        private ESRI.ArcGIS.Geometry.IPoint pPt = null;//大比例尺出图用于确定真实坐标
        private SheetType sheetType = SheetType.foundationTerrain;
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
        public FrmSheetMapUserSet(AxMapControl inAxMapControl,Form inForm,Plugin.Application.IApplicationRef inHook,SheetType inST)
        {
            InitializeComponent();
            //isOK = false;
            //cBoxScale.SelectedIndex = 0;
            //curMapName = incurMapName;
            pAxMapControl = inAxMapControl;
            hook = inHook;
            pMainForm = inForm;
            sheetType = inST;
            pAxMapControl.OnMouseDown += new 
                IMapControlEvents2_Ax_OnMouseDownEventHandler(pAxMapControl_OnMouseDown);//订阅事件
        }

        private void txtResolution_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled=true;
            if(txtMapNo.Text=="")
                btnOK.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMapNo.Text == "" || cBoxScale.Text == "")
                return;
            this.Hide();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("选择比例尺为:" + cBoxScale.Text + ",图幅号为:" + txtMapNo.Text);
            }
            MapNo = txtMapNo.Text;
            Scale = Convert.ToInt32(cBoxScale.Text.Split(':')[1]);
            string scaleDM = ModGetData.GetDMofScale("1:" + Scale.ToString());
            if (scaleDM == "")
                return;
            IMap pMap = null;
           
            int type = 0;
            bool isSpecial = ModGetData.IsMapSpecial();

               
            if (sheetType == SheetType.foundationTerrain)
            {
                type = 0;
                if (isSpecial)//如果找特定专题
                {
                    pMap = new MapClass();
                    pMap.SpatialReference = pAxMapControl.Map.SpatialReference;
                    ModGetData.AddMapOfNoneXZQ(pMap, "DLG" + scaleDM, pAxMapControl.Map);
                    if (pMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        goto CloseFrm;
                    }
                    ModuleMap.LayersComposeEx(pMap);//图层排序
                }
                else
                {

                    IObjectCopy pOC = new ObjectCopyClass();
                    pMap = pOC.Copy(pAxMapControl.Map) as IMap;//复制地图
                }
                GeoPageLayout pPageLayout = new
               GeoPageLayout(pMap, MapNo, Scale, pPt, type);
                pPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pPageLayout.typePageLayout = 3;
                pPageLayout.MapOut();

                pPageLayout = null;
            }
            else if (sheetType == SheetType.urbanCadastre)
            {
                type = 2;
                if (isSpecial)//如果找特定专题
                {
                    pMap = new MapClass();
                    pMap.SpatialReference = pAxMapControl.Map.SpatialReference;
                    ModGetData.AddMapOfNoneXZQ(pMap, "CZDJ", pAxMapControl.Map);//寻找专题
                    ModGetData.AddMapOfNoneXZQ(pMap, "DLG" + scaleDM, pAxMapControl.Map);
                    if (pMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        goto CloseFrm;
                    }
                    ModuleMap.LayersComposeEx(pMap);//图层排序
                }
                else
                {
                    IObjectCopy pOC = new ObjectCopyClass();
                    pMap = pOC.Copy(pAxMapControl.Map) as IMap;//复制地图
                }
                ModuleMap.LayersComposeEx(pMap);//图层排序
                GeoPageLayout pPageLayout = new
                 GeoPageLayout(pMap, MapNo, Scale, pPt, type);
                pPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pPageLayout.typePageLayout = 3;
                pPageLayout.MapOut();

                pPageLayout = null;
            }
            
            //BzffOutMap cmdBZTF = new BzffOutMap(MapNo, Scale,pPt);//
            //cmdBZTF.OnCreate(hook);
            //cmdBZTF.OnClick();
            CloseFrm:
               this.Close();
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

                initcBoxScale();
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
