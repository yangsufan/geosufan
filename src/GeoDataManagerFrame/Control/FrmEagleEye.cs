using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using System.IO;
using System.Xml;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图鹰眼窗体
    /// </summary>
    public partial class FrmEagleEye : DevComponents.DotNetBar.Office2007Form
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
        AxMapControl pAxMapCtrl;//主窗体控件
        Plugin.Interface.CommandRefBase pCommand;
        private string _Mapindexpath = Application.StartupPath + "\\..\\Template\\MapIndex.mxd";
        ///ZQ 20110927   modify   构造函数添加系统库
        public FrmEagleEye(AxMapControl inAxMapCtrl, Plugin.Interface.CommandRefBase inCommand, IWorkspace pWorkspace)
        {
            InitializeComponent();
            CopyEagleEyeMap(pWorkspace, _Mapindexpath);
            pAxMapCtrl = inAxMapCtrl;
            pCommand = inCommand;
            pAxMapCtrl.OnMapReplaced += new IMapControlEvents2_Ax_OnMapReplacedEventHandler(pAxMapCtrl_OnMapReplaced);
            pAxMapCtrl.OnExtentUpdated += new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(pAxMapCtrl_OnExtentUpdated);
            InitEagleMap();
            
            
        }
        //从数据库向本地拷贝
        private void  CopyEagleEyeMap(IWorkspace pWorkspace, string strPath)
        {
            Exception eError = null;
            //读取数据库表内容
            object tempObj = GetFieldValue("SYSSETTING", "SETTINGVALUE2", "SETTINGNAME='鹰眼图'", pWorkspace, out eError);
            IMemoryBlobStreamVariant pMemoryBlobStreamVariant =tempObj as  IMemoryBlobStreamVariant;
            IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
            if (pMemoryBlobStream != null)
            {
                pMemoryBlobStream.SaveToFile(strPath);
            }
        }
        public object GetFieldValue(string tablename, string keyfieldname, string condition,IWorkspace pWorkspace, out Exception eError)
        {
            eError = null;
            //得到FeatrueWS
            try
            {
                Dictionary<string, object> dicValue = new Dictionary<string, object>();
                ITable pTable = OpenTable_(tablename, pWorkspace, out eError);
                if (eError != null) return null;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condition;
                ICursor pCursor = pTable.Search(pQueryFilter, true);
                IRow pRow = pCursor.NextRow();
                if (pRow == null)
                {
                    Marshal.ReleaseComObject(pCursor);
                    return null;
                }

                int indexF = pRow.Fields.FindField(keyfieldname);
                if (indexF == -1)
                {
                    Marshal.ReleaseComObject(pCursor);
                    return null;
                }

                object val = pRow.get_Value(indexF);
                Marshal.ReleaseComObject(pCursor);
                return val;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }
        public ITable OpenTable_(string tablename, IWorkspace pWorkspace, out Exception eError)
        {
            eError = null;

            try
            {
                //得到FeatrueWS
                IFeatureWorkspace pFeaWS = (IFeatureWorkspace)pWorkspace;
                return pFeaWS.OpenTable(tablename);
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return null;
            }
        }
        private ITable OpenTable(string tablename, out Exception eError)
        {
            throw new NotImplementedException();
        }

        public void InitEagleMap()
        {
            //      string strLayer = getLayerName();
            //    if (strLayer != "")
            //        goto Next;

            //setEagleEye:
            //    FrmEagleEyeSet pFEES = new FrmEagleEyeSet(pAxMapCtrl.Map);
            //    if (pFEES.ShowDialog() != DialogResult.OK)
            //    {
            //        this.Close();
            //    }
            //   Next://如果设置了鹰眼地图，则判断其是否存在于当前地图控件中
            //    strLayer = getLayerName();
            //    ILayer pLayer2 = isExistLayer(pAxMapCtrl.Map, strLayer);
            if (!File.Exists(_Mapindexpath))
            {
                MessageBox.Show("鹰眼图不存在，请检查配置!", "询问", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            axMapControl1.LoadMxFile(_Mapindexpath, "", "");
            //axMapControl1.Map = new MapClass();

            //if (pLayer2 != null)
            //{
            //    axMapControl1.AddLayer(pLayer2);
            //}
            //else//如果没有存在设置的底图，则重新设置底图
            //{
            //    goto setEagleEye;
            //    //for (int i = 0; i < pAxMapCtrl.LayerCount; i++)
            //    //{
            //    //    ILayer pLayer = pAxMapCtrl.get_Layer(pAxMapCtrl.LayerCount - i - 1);
            //    //    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            //    //    if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            //    //        axMapControl1.AddLayer(pLayer);

            //    //}
            //}

            //axMapControl1.Extent = pAxMapCtrl.get_Layer(0).AreaOfInterest;//设置成底图范围
            axMapControl1.Refresh();
            drawAxMapC1Extent();
           
        }
        //从xml文件读取layer
        private string getLayerName()
        {
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EagleEye.xml";
            if (!File.Exists(cPath))
            {
                return "";
            }
            XmlDocument cXmlDoc = new XmlDocument();
            string strLayer = "";
            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EagleEyeInfo");
                strLayer=xnl.Item(0).Attributes["LayerName"].Value;
            }
            return strLayer;
        }
        //判断layer是否在当前地图
        private ILayer isExistLayer(IMap inMap,string strLayerName)
        {
            ILayer res = null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if (pCLayer.get_Layer(j).Name == strLayerName)
                        {
                            res=pCLayer.get_Layer(j);
                            
                            break;
                        }

                    }
                }
                else//不是grouplayer
                {
                    if (pLayer.Name == strLayerName)
                    {
                        res = pLayer;

                        break;
                    }
                }
            }
            return res;
        }

        private void pAxMapCtrl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {

            drawAxMapC1Extent1(e);
            

        }
        //以AxMapC1Extent生成一个mapElement在Ax2显示
        private void drawAxMapC1Extent1(IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            // 得到新范围 
            
            IEnvelope pEnv = (IEnvelope)e.newEnvelope;
			axMapControl1.Map.SpatialReference = pAxMapCtrl.SpatialReference;//20110802 xisheng 给小地图空间参考赋成大地图空间参考

            IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;

            IActiveView pAv = pGra as IActiveView;

            // 在绘制前，清除 axMapControl 中的任何图形元素 

            pGra.DeleteAllElements();

            IRectangleElement pRectangleEle = new RectangleElementClass();

            IElement pEle = pRectangleEle as IElement;

            pEle.Geometry = pEnv;

			//获取比例尺来判断十字丝是否出现 20110801 xisheng start changed
            string strscale = pAxMapCtrl.Map.MapScale.ToString();
			if (strscale.Contains("."))//判断是否包含点20110802
            {
                strscale = strscale.Substring(0, strscale.LastIndexOf("."));
            }//end
            
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
            if (Convert.ToInt32(strscale) <5000)//比例尺大于50万
            //中心点 xisheng 20110731 十字丝
            {
                ESRI.ArcGIS.Geometry.Point p = new ESRI.ArcGIS.Geometry.Point();
                p.X = (pEnv.XMax + pEnv.XMin) / 2;
                p.Y = (pEnv.YMax + pEnv.YMin) / 2;
                int x = Convert.ToInt32(p.X); 
                int y=Convert.ToInt32(p.Y);
                 axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p, out x, out y);
               
                p1.Location = new System.Drawing.Point(x-11,y-1);
                p2.Location = new System.Drawing.Point(x-1,y-11);
                p3.Location = new System.Drawing.Point(x + 1, y - 1);
                p4.Location = new System.Drawing.Point(x - 1, y + 1);
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;
            }
            //20110801 end
            // 设置鹰眼图中的红线框 

            IRgbColor pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 255;

            // 产生一个线符号对象 

            ILineSymbol pOutline = new SimpleLineSymbolClass();

            pOutline.Width = 2;

            pOutline.Color = pColor;

            // 设置颜色属性 

            pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 0;

            // 设置填充符号的属性 

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();

            pFillSymbol.Color = pColor;

            pFillSymbol.Outline = pOutline;

            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;

            pFillShapeEle.Symbol = pFillSymbol;

            pGra.AddElement((IElement)pFillShapeEle, 0);

            // 刷新 

            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
 
        }
        //以AxMapC1Extent生成一个mapElement在Ax2显示
        private void drawAxMapC1Extent()
        {
            // 得到新范围 
            this.Refresh();
            IEnvelope pEnv = (IEnvelope)pAxMapCtrl.Extent;
			axMapControl1.Map.SpatialReference = pAxMapCtrl.SpatialReference;//20110802 xisheng 给小地图空间参考赋成大地图空间参考
            IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;

            IActiveView pAv = pGra as IActiveView;

            // 在绘制前，清除 axMapControl 中的任何图形元素 

            pGra.DeleteAllElements();

            IRectangleElement pRectangleEle = new RectangleElementClass();

            IElement pEle = pRectangleEle as IElement;

            pEle.Geometry = pEnv;

           //获取比例尺来判断十字丝是否出现 20110801 xisheng start changed
            string strscale = pAxMapCtrl.Map.MapScale.ToString();
			if (strscale.Contains("."))//判断是否包含点20110802
            {
                strscale = strscale.Substring(0, strscale.LastIndexOf("."));
            }//end
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
            if (Convert.ToInt32(strscale) <5000)//比例尺大于50万
            //中心点 xisheng 20110731 十字丝
            {
                ESRI.ArcGIS.Geometry.Point p = new ESRI.ArcGIS.Geometry.Point();
                p.X = (pEnv.XMax + pEnv.XMin) / 2;
                p.Y = (pEnv.YMax + pEnv.YMin) / 2;
                int x = Convert.ToInt32(p.X); 
                int y=Convert.ToInt32(p.Y);
                 axMapControl1.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(p, out x, out y);//将地理坐标转为屏幕坐标
               
                p1.Location = new System.Drawing.Point(x-11,y-1);
                p2.Location = new System.Drawing.Point(x-1,y-11);
                p3.Location = new System.Drawing.Point(x + 1, y - 1);
                p4.Location = new System.Drawing.Point(x - 1, y + 1);
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;
            }
            //20110801 xisheng end 
            // 设置鹰眼图中的红线框 

            IRgbColor pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 255;

            // 产生一个线符号对象 

            ILineSymbol pOutline = new SimpleLineSymbolClass();

            pOutline.Width = 2;

            pOutline.Color = pColor;

            // 设置颜色属性 

            pColor = new RgbColorClass();

            pColor.Red = 255;

            pColor.Green = 0;

            pColor.Blue = 0;

            pColor.Transparency = 0;

            // 设置填充符号的属性 

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();

            pFillSymbol.Color = pColor;

            pFillSymbol.Outline = pOutline;

            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;

            pFillShapeEle.Symbol = pFillSymbol;

            pGra.AddElement((IElement)pFillShapeEle, 0);

            // 刷新 

            pAv.Refresh();

        }


        private void pAxMapCtrl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            axMapControl1.Map = new MapClass();
            for (int i = 0; i < pAxMapCtrl.LayerCount; i++)
            {
                ILayer pLayer = pAxMapCtrl.get_Layer(pAxMapCtrl.LayerCount - i - 1);
                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    axMapControl1.AddLayer(pLayer);

            }

            axMapControl1.Extent = pAxMapCtrl.FullExtent;
            axMapControl1.Refresh();
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (this.axMapControl1.Map.LayerCount != 0) 

            { 

                // 按下鼠标左键移动矩形框 

                if (e.button == 1) 

                { 

                    IPoint pPoint = new PointClass(); 

                    pPoint.PutCoords(e.mapX, e.mapY);
                    IEnvelope pEnv = GetEnvelope(pPoint);
                    //IEnvelope pEnvelope = pAxMapCtrl.Extent; 

                    //pEnvelope.CenterAt(pPoint);
                    pAxMapCtrl.Extent = pEnv;
                    //pAxMapCtrl.CenterAt(pPoint);// = pEnvelope;

                    pAxMapCtrl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null); 

                } 

                // 按下鼠标右键绘制矩形框 

                else if (e.button == 2) 

                {
                    double fAxMapCtrlRatio = pAxMapCtrl.Extent.Width / pAxMapCtrl.Extent.Height;
                    IEnvelope pEnvelop = this.axMapControl1.TrackRectangle();//若画出的形状和主控件比例不一样则转化
                    double fEnvRatio = pEnvelop.Width / pEnvelop.Height;
                    double midX = pEnvelop.XMin+(pEnvelop.XMax - pEnvelop.XMin) / 2;
                    double midY = pEnvelop.YMin+(pEnvelop.YMax - pEnvelop.YMin) / 2;
                    IEnvelope realEnv = new EnvelopeClass();
                    if (fEnvRatio > fAxMapCtrlRatio)
                    {
                        realEnv.PutCoords(midX - pEnvelop.Height * fAxMapCtrlRatio / 2, midY - pEnvelop.Height / 2,
                            midX + pEnvelop.Height * fAxMapCtrlRatio / 2, midY + pEnvelop.Height / 2);
                    }
                    else
                    {
                        realEnv.PutCoords(midX - pEnvelop.Width / 2, midY - pEnvelop.Width / fAxMapCtrlRatio / 2,
                            midX + pEnvelop.Width / 2, midY + pEnvelop.Width / fAxMapCtrlRatio / 2);
 
                    }
                    //realEnv.Project(pEnvelop.SpatialReference);
                    pAxMapCtrl.Extent = realEnv;

                    pAxMapCtrl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null); 

                } 

            } 



          }
        private IEnvelope GetEnvelope(IPoint pPoint)
        {
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.Geometry = pPoint;
            IEnvelope pEnv = null;
            pFilter.SpatialRel =esriSpatialRelEnum.esriSpatialRelIntersects;
            for (int i = 0; i < this.axMapControl1.LayerCount; i++)
            {
                ILayer pLayer = this.axMapControl1.get_Layer(i);
                IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                if (pFeaLayer != null)
                {
                    IFeatureClass pFeaClass = pFeaLayer.FeatureClass;
                    if (pFeaClass != null)
                    {                        
                        if (pFeaClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            IFeatureCursor pCursor = pFeaClass.Search(pFilter,false);
                            if (pCursor != null)
                            {
                                IFeature pFea = pCursor.NextFeature();
                                if (pFea != null)
                                {
                                    pEnv = new EnvelopeClass();
                                    IEnvelope pTmpenv = pFea.ShapeCopy.Envelope;
                                    pEnv.XMax = pTmpenv.XMax;
                                    pEnv.YMax = pTmpenv.YMax;
                                    pEnv.XMin = pTmpenv.XMin;
                                    pEnv.YMin = pTmpenv.YMin;
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                                    pCursor = null;
                                    break;
                                }
                                
                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                            pCursor = null;
                        }
                    }
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFilter);
            pFilter = null;
            return pEnv;

        }
        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            //// 如果不是左键按下就直接返回 

            //if (e.button != 1) return; 

            //IPoint pPoint = new PointClass(); 

            //pPoint.PutCoords(e.mapX, e.mapY);

            //pAxMapCtrl.CenterAt(pPoint);

            //pAxMapCtrl.ActiveView.Refresh();//.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null); 



        }

        private void FrmEagleEye_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    FormCollection pFC = Application.OpenForms;
            //    //Form[] fm = new Form[pFC.Count];
            //    //int i = 0;

            //    foreach (Form pFm in pFC)
            //    {

            //        if (pFm.Name == "frmMain" && pFm.Text != "数据展示管理")//判断系统是否是数据展示管理
            //        {
            //            if (this.CloseEnabled)
            //                this.Close();
            //        }
            //        //fm[i] = pFm;
            //        //i++;
            //    }
            //}
            //catch
            //{
 
            //}
            //Form[] f = fm;
        }

        private void FrmEagleEye_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("关闭鹰眼图");
            }
        }
      }
}
