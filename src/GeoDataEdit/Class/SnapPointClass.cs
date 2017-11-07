using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Runtime.InteropServices;

namespace GeoDataEdit
{
    /// <summary>
    /// 捕捉点的类型枚举值
    /// </summary>
    public enum PointType { CenterPoint,PortPoint,MidPoint,VertexPoint,BoundryPoint,IntersectPoint};

    public class SnapPointClass
    {

        #region 变量

        #region 对外接口变量

        #region 是否开启捕捉  IsOpenSnap
        private bool b_IsOpenSnap;
        public bool IsOpenSnap
        {
            get { return b_IsOpenSnap; }
            set { this.b_IsOpenSnap = value; }
        }
        #endregion

        #region 是否捕捉端点 IsSnapPortPoint
        private bool b_IsSnapPortPoint;
        public bool IsSnapPortPoint
        {
            get { return this.b_IsSnapPortPoint; }
            set { this.b_IsSnapPortPoint = value; }
        }
        #endregion

        #region 是否捕捉中点 IsSnapMidPoint
        private bool b_IsSnapMidPoint;
        public bool IsSnapMidPoint
        {
            get { return this.b_IsSnapMidPoint; }
            set { this.b_IsSnapMidPoint = value; }
        }
        /***************************************************************/
        #endregion

        #region 是否捕捉交点 IsSnapIntersectPoint
        private bool b_IsSnapIntersectPoint;
        public bool IsSnapIntersectPoint
        {
            get { return this.b_IsSnapIntersectPoint; }
            set { this.b_IsSnapIntersectPoint = value; }
        }
        #endregion

        #region 是否捕捉结点 IsSnapNodePoint
        private bool b_IsSnapNodePoint;
        public bool IsSnapNodePoint
        {
            get { return this.b_IsSnapNodePoint; }
            set { this.b_IsSnapNodePoint = value; }
        }
        #endregion

        #region 是否捕捉边上点 IsSnapBoundryPoint
        private bool b_IsSnapBoundryPoint;
        public bool IsSnapBoundryPoint
        {
            get { return this.b_IsSnapBoundryPoint; }
            set { this.b_IsSnapBoundryPoint = value; }
        }
        #endregion

        #region 是否捕捉中心点 IsSnapCenterPoint
        private bool b_IsSnapCenterPoint;
        public bool IsSnapCenterPoint
        {
            get { return this.b_IsSnapCenterPoint; }
            set { this.b_IsSnapCenterPoint = value; }
        }
        #endregion

        #region 捕捉半径(地图) SnapMapRadius
        private double d_SnapMapRadius;
        public double SnapMapRadius
        {
            get { return this.d_SnapMapRadius; }
        }
        #endregion

        #region 缓冲半径(地图) CacheMapRadius
        private double d_CacheMapRadius;
        public double CacheMapRadius
        {
            get { return this.d_CacheMapRadius; }
        }
        #endregion

        #region 捕捉半径(屏幕) SnapPixelRadius
        private double d_SnapPixelRadius;
        public double SnapPixelRadius
        {
            get { return d_SnapPixelRadius; }
            set
            {
                this.d_SnapPixelRadius = value;
                if ( m_CurrentMap != null )
                    d_SnapMapRadius = ConvertPixelDistanceToMapDistance ( this.d_SnapPixelRadius );
            }
        }
        #endregion

        #region 缓冲半径(屏幕) CachePixelRadius
        private double d_CachePixelRadius;
        public double CachePixelRadius
        {
            get { return d_CachePixelRadius; }
            set
            {
                this.d_CachePixelRadius = value;
                if ( m_CurrentMap != null )
                    d_CacheMapRadius = ConvertPixelDistanceToMapDistance ( this.d_CachePixelRadius );
            }
        }
        #endregion

        #region 捕捉结果点 SnapResultPoint
        private IPoint m_SnapResultPoint;
        public IPoint SnapResultPoint
        {
            get { return this.m_SnapResultPoint; }
        }
        #endregion

        #region 捕捉参考点 SnapRefrencePoint
        private IPoint m_SnapRefrencePoint;
        private IPoint SnapRefrencePoint
        {
            get { return this.m_SnapRefrencePoint; }
            set { this.m_SnapRefrencePoint = value; }
        }
        #endregion

        #region 当前Map  CurrentMap
        private IMap m_CurrentMap;
        #endregion

        #endregion

        #region 类变量
        private IFeatureCache2 m_FeatureCache;
        public IPoint m_LastSnapPoint;
        private PointType m_PointType;
        private ISymbol m_LastSnapSymbol;
        private double m_IgnoreDistance;
        private IMap m_pNewMap;//用于填充Cache
        private double m_pMapScale;//用于地图比例尺变化时
        #endregion

        #endregion

        /*************************构造函数********************************/
        public SnapPointClass ()
        {
            DefaultSetting ();
            Init ();
        }

        /// <summary>
        /// 用户设置
        /// </summary>        
        public void CustomSetting ( bool _IsSnapPortPoint , bool _IsSnapMidPoint , bool _IsSnapNodePoint ,
            bool _IsSnapIntersectPoint , bool _IsSnapPointPoint , bool _IsSnapBoundryPoint,bool _IsSnapCenterPoint,
            double _SnapPixelRadius , double _CachePixelRadius )
        {
            IsOpenSnap = true;
            IsSnapPortPoint = _IsSnapPortPoint;
            IsSnapMidPoint = _IsSnapMidPoint;
            IsSnapIntersectPoint = _IsSnapIntersectPoint;
            IsSnapBoundryPoint = _IsSnapBoundryPoint;
            IsSnapCenterPoint = _IsSnapCenterPoint;
            IsSnapNodePoint = _IsSnapNodePoint;

            SnapPixelRadius = _SnapPixelRadius;
            CachePixelRadius = _CachePixelRadius;
        }

        /// <summary>
        /// 默认设置
        /// </summary>
        private void DefaultSetting ()
        {
            IsOpenSnap = false;

            IsSnapPortPoint = true;
            IsSnapNodePoint = false;
            IsSnapMidPoint = false;
            IsSnapIntersectPoint = false;
            IsSnapCenterPoint = false;
            IsSnapBoundryPoint = false;
            //b_IsHasSnaped = false;
            //IsReFillCache = true;

            SnapPixelRadius = 5;
            CachePixelRadius = 300;

            m_IgnoreDistance = 0;
        }

        /// <summary>
        /// 对象初始化
        /// </summary>
        private void Init ()
        {
            SnapRefrencePoint = null;
            m_SnapResultPoint = null;


            m_LastSnapPoint = null;
            m_FeatureCache = new FeatureCacheClass ();
            m_LastSnapSymbol = null;
            m_PointType = PointType.VertexPoint;
        }

        /// <summary>
        /// 对外接口，执行捕捉点的操作
        /// 执行该操作之前，先要对CurrentMap变量赋值
        /// 并且CurrentMap中层的LayerDescription值必须为XML格式
        /// 程序将根据XML中的IsSnap属性确定是否对该层进行捕捉操作
        /// </summary>
        public bool SnapExcute ( IPoint _SnapPoint  )
        {           
            //如果捕捉未开启，那么捕捉点即为参考点
            //b_IsHasSnaped = false;
            if ( !IsOpenSnap )
            {
                m_SnapResultPoint = _SnapPoint;                
                return false;
            }

            m_PointType = PointType.VertexPoint;
            /*******临时捕捉设置***********/

            //设置比例尺
            if ( m_pMapScale != m_CurrentMap.MapScale )
            {
                m_pMapScale = m_CurrentMap.MapScale;
                d_SnapMapRadius = ConvertPixelDistanceToMapDistance ( this.d_SnapPixelRadius );
                d_CacheMapRadius = ConvertPixelDistanceToMapDistance ( this.d_CachePixelRadius );
            }

            SnapRefrencePoint = _SnapPoint;//当前鼠标点，即参考点
            m_SnapResultPoint = null;
            if ( !isCanExcute () ) return false;


            //重新填充缓冲区
            if ( m_FeatureCache == null )
                m_FeatureCache = new FeatureCacheClass ();

            if ( !m_FeatureCache.Contains ( _SnapPoint ) )
            {
                if ( !ReFillCache () ) return false;
            }
            

            HitFeature ();

            /*********************画点算法**********************************/
            if ( m_LastSnapPoint != null && m_LastSnapSymbol != null )
            {
                DrawPoint ( m_LastSnapPoint , m_LastSnapSymbol as ISymbol );
            }
            if ( SnapResultPoint != null && SnapResultPoint.IsEmpty == false )
            {
                DrawPoint ( SnapResultPoint , SetSnapSymbol () as ISymbol );
            }           
            m_LastSnapPoint = SnapResultPoint;
            m_LastSnapSymbol = SetSnapSymbol () as ISymbol;

            //如果没有捕捉到点，那么把捕捉参考点返回
            if ( SnapResultPoint == null || SnapResultPoint.IsEmpty )
                m_SnapResultPoint = SnapRefrencePoint;

            return true;

        }

        /// <summary>
        /// 从传入的Map参数中得到允许捕捉图层的列表
        /// LayerDescription
        /// </summary>
        /// <returns></returns>
        public bool InitMap ( IMap pMap )
        {
            //从LayerDescription读取控制信息来初始化图层列表
            if ( pMap == null ) return false;
            if ( m_pNewMap == null ) m_pNewMap = new MapClass ();
            m_pNewMap.ClearLayers ();
            m_CurrentMap = pMap;
            //XmlDocument xmlDoc = new XmlDocument ();
            //XmlNode pSnapNode;

            for ( int i = 0 ; i < pMap.LayerCount ; i++ )
            {
                if ( pMap.get_Layer ( i ) is IFeatureLayer )
                {
                    try
                    {
                        if ((pMap.get_Layer(i) as IFeatureLayer).FeatureClass == null) continue;
                        if ((pMap.get_Layer(i) as IFeatureLayer).FeatureClass.FeatureType != esriFeatureType.esriFTSimple) continue;
                       
                        //xmlDoc.LoadXml((pMap.get_Layer(i) as ILayerGeneralProperties).LayerDescription);
                        //pSnapNode = xmlDoc.SelectSingleNode("//*[@IsSnap='true']");
                        //if (pSnapNode != null)
                        //{
                            //该层可见并且是IGeoFeatureLayer类型
                        if (pMap.get_Layer(i).Visible && pMap.get_Layer(i) is IGeoFeatureLayer)
                        {
                            IObjectCopy pOC = new ObjectCopyClass();
                            m_pNewMap.AddLayer(pOC.Copy(pMap.get_Layer(i)) as ILayer);
                        }
                        //}
                    }
                    catch
                    { 
                    
                    }

                }
            }

            m_pMapScale = pMap.MapScale;
            d_SnapMapRadius = ConvertPixelDistanceToMapDistance ( d_SnapPixelRadius );
            d_CacheMapRadius = ConvertPixelDistanceToMapDistance ( d_CachePixelRadius );

            return true;
        }

        /// <summary>
        /// 验证当前信息是否可以执行捕捉点的操作
        /// </summary>
        /// <returns></returns>
        private bool isCanExcute ()
        {
            if ( m_CurrentMap == null ) return false;//未设置当前地图
            if ( SnapRefrencePoint == null ) return false;//未设置捕捉参考点
            return true;
        }

        /// <summary>
        /// 重新填充缓冲区
        /// </summary>
        public bool ReFillCache ()
        {
            if ( SnapRefrencePoint == null ) return false;
            if ( CacheMapRadius == 0 ) return false;
            m_FeatureCache = new FeatureCacheClass ();
            m_FeatureCache.Initialize ( SnapRefrencePoint , CacheMapRadius );            
            
            if ( m_pNewMap == null ) return false;
            UID pUID = new UIDClass ();
            pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            if ( m_pNewMap.LayerCount > 0 )
            {
                IEnumLayer pEnumLayer = m_pNewMap.get_Layers ( pUID , true );
                m_FeatureCache.AddLayers ( pEnumLayer , GetPointBufferEnvelope (SnapRefrencePoint, CacheMapRadius ) );
                return true;
            }
            return false;
        }

        /// <summary>
        /// 得到以pPoint点为中心
        /// radius为半径的Envelope
        /// </summary>
        private IEnvelope GetPointBufferEnvelope (IPoint pPoint, double radius )
        {
            IEnvelope pEnvelope = new EnvelopeClass ();
            pEnvelope.XMax = pPoint.X + radius;
            pEnvelope.XMin = pPoint.X - radius;
            pEnvelope.YMin = pPoint.Y - radius;
            pEnvelope.YMax = pPoint.Y + radius;
            return pEnvelope;
        }

        //得到pPointColl中距离pPoint最近的点
        private IPoint GetClosedPoint ( IPointCollection pPointColl , IPoint pPoint )
        {
            if ( pPointColl.PointCount <= 0 ) return null;

            IPoint pTempPoint = pPointColl.get_Point ( 0 );
            for ( int i = 0 ; i < pPointColl.PointCount ; i++ )
            {
                if ( GetDistance ( pTempPoint , pPoint ) > GetDistance ( pPointColl.get_Point ( i ) , pPoint ) )
                    pTempPoint = pPointColl.get_Point ( i );
            }
            if ( GetDistance ( pTempPoint , pPoint ) < SnapMapRadius * SnapMapRadius )
                return pTempPoint;
            else
                return null;
        }

        /// <summary>
        /// 把像素(屏幕)距离转化成为地图上的距离
        /// </summary>
        private double ConvertPixelDistanceToMapDistance ( double PixelDistance )
        {
            tagPOINT tagP = new tagPOINT ();
            WKSPoint wksP = new WKSPoint ();

            tagP.x = ( int )PixelDistance;
            tagP.y = ( int )PixelDistance;
            ( m_CurrentMap as IActiveView ).ScreenDisplay.DisplayTransformation.TransformCoords ( ref wksP , ref tagP , 1 , 6 );
            return wksP.X;
        }

        private double ScaleChange ( double vVal )
        {
            if ( m_CurrentMap.MapScale == 0 || m_CurrentMap.ReferenceScale == 0 )
                return vVal;
            else
                return vVal * m_CurrentMap.MapScale / m_CurrentMap.ReferenceScale;
        }

        /// <summary>
        /// 画出捕捉点
        /// </summary>
        /// <param name="pAV"></param>
        /// <param name="pPoint"></param>
        private void DrawPoint ( IPoint pPoint , ISymbol symbol )
        {
            IActiveView pAV = m_CurrentMap as IActiveView;
            pAV.ScreenDisplay.StartDrawing ( pAV.ScreenDisplay.hDC , -1 );
            pAV.ScreenDisplay.SetSymbol ( symbol );
            pAV.ScreenDisplay.DrawPoint ( pPoint );
            pAV.ScreenDisplay.FinishDrawing ();
        }

        /// <summary>
        /// 得到画点的Symbol
        /// </summary>
        /// <returns></returns>
        private ISimpleMarkerSymbol SetSnapSymbol ()
        {
            IRgbColor pRGB = new RgbColorClass ();
            pRGB.Blue = 0;
            pRGB.Green = 0;
            pRGB.Red = 255;
            pRGB.Transparency = 0;

            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass ();
            ISymbol pSymbol = pMarkerSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            pMarkerSymbol.Color = pRGB;

            switch ( m_PointType )
            {
                case PointType.BoundryPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCross;
                    break;
                case PointType.CenterPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                    break;
                case PointType.IntersectPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSX;
                    break;
                case PointType.MidPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
                    break;
                case PointType.PortPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    break;
                case PointType.VertexPoint:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSX;
                    break;
                default:
                    pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
                    break;
            }
            pRGB.Transparency =255;
            pRGB.Blue = 0;
            pRGB.Green =255;
            pRGB.Red = 0;
            pMarkerSymbol.Outline = true;
            pMarkerSymbol.OutlineColor = pRGB;

            pMarkerSymbol.OutlineSize = ScaleChange (1);
            pMarkerSymbol.Size = ScaleChange ( 6);

            return pMarkerSymbol;
        }

        private bool HitFeature ()
        {
            int segmentIndex = 0;
            double hitDist = 0;
            int part = 0;
            bool rightSide = false;

            IPointCollection pPointColl_Intersect = new PolygonClass ();

            IProximityOperator pProximityOp = SnapRefrencePoint as IProximityOperator;

            //距离捕捉参考点最近的点（除交点外）
            IPoint pClosedPoint = new PointClass ();

            //获得捕捉参考点缓冲矩形
            IEnvelope pEnvelope = GetPointBufferEnvelope ( SnapRefrencePoint,SnapMapRadius );

            //判断pClosedPoint是否已经被赋了值
            bool isSet = false;

            //用于计算交点的列表
            List<IFeature> list_AllFeatures = new List<IFeature> ();

            for ( int i = 0 ; i < m_FeatureCache.Count ; i++ )
            {
                IFeature pFeature = m_FeatureCache.get_Feature ( i );

                //判断IFeature的类型
                if ( pFeature.Shape.GeometryType != esriGeometryType.esriGeometryPoint &&
                    pFeature.Shape.GeometryType != esriGeometryType.esriGeometryPolyline &&
                    pFeature.Shape.GeometryType != esriGeometryType.esriGeometryPolygon )
                    continue;

                //判断IFeature是否与捕捉点的缓冲区相交
                if ( pFeature.Shape.Envelope.XMax < pEnvelope.XMin || pFeature.Shape.Envelope.XMin > pEnvelope.XMax ||
                    pFeature.Shape.Envelope.YMax < pEnvelope.YMin || pFeature.Shape.Envelope.YMin > pEnvelope.YMax )
                    continue;

                //判断IFeature和捕捉参考点的距离,此处是否应该大于捕捉半径，而不是捕捉半径的平方
                if ( pProximityOp.ReturnDistance ( pFeature.Shape ) > SnapMapRadius /** SnapMapRadius */) continue;

                IHitTest pHitTest = m_FeatureCache.get_Feature ( i ).Shape as IHitTest;

                IPoint[] pResPoint = new IPoint[5];
                for ( int j = 0 ; j < 5 ; j++ )
                {
                    pResPoint[j] = new PointClass ();
                }

                #region HitTest捕捉

                //捕捉端点
                if ( IsSnapPortPoint )
                    pHitTest.HitTest ( SnapRefrencePoint , SnapMapRadius , esriGeometryHitPartType.esriGeometryPartEndpoint , pResPoint[0] , ref hitDist , ref part , ref segmentIndex , ref rightSide );
                else
                    pResPoint[0] = null;

                //捕捉中点
                if ( IsSnapMidPoint )
                    pHitTest.HitTest ( SnapRefrencePoint , SnapMapRadius , esriGeometryHitPartType.esriGeometryPartMidpoint , pResPoint[1] , ref hitDist , ref part , ref  segmentIndex , ref  rightSide );
                else
                    pResPoint[1] = null;

                //捕捉节点
                if ( IsSnapNodePoint )
                    pHitTest.HitTest ( SnapRefrencePoint , SnapMapRadius , esriGeometryHitPartType.esriGeometryPartVertex , pResPoint[2] , ref  hitDist , ref part , ref segmentIndex , ref rightSide );
                else
                    pResPoint[2] = null;

                //捕捉边上点
                if ( IsSnapBoundryPoint )
                    pHitTest.HitTest ( SnapRefrencePoint , SnapMapRadius , esriGeometryHitPartType.esriGeometryPartBoundary , pResPoint[3] , ref  hitDist , ref part , ref segmentIndex , ref rightSide );
                else
                    pResPoint[3] = null;

                //捕捉中心点
                if ( IsSnapCenterPoint )
                    pHitTest.HitTest ( SnapRefrencePoint , SnapMapRadius , esriGeometryHitPartType.esriGeometryPartCentroid , pResPoint[4] , ref  hitDist , ref part , ref segmentIndex , ref rightSide );
                else
                    pResPoint[4] = null;

                #endregion

                //捕捉交点
                if ( IsSnapIntersectPoint )
                {
                    list_AllFeatures.Add ( pFeature );
                }

                #region 捕捉端点
                if ( pResPoint[0] != null && pResPoint[0].IsEmpty == false )
                {
                    if ( isSet )
                    {
                        if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) > GetDistance ( SnapRefrencePoint , pResPoint[0] ) )
                        {
                            pClosedPoint = pResPoint[0];

                            //在允许误差范围内
                            //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                            //{
                            //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                            //    {
                            //        m_SnapResultPoint = pClosedPoint;
                            //        m_PointType = PointType.PortPoint;
                            //        return true;
                            //    }
                            //}
                            m_PointType = PointType.PortPoint;
                        }
                    }
                    else
                    {
                        pClosedPoint = pResPoint[0];

                        //在允许误差范围内
                        //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                        //{
                        //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                        //    {
                        //        m_SnapResultPoint = pClosedPoint;
                        //        m_PointType = PointType.PortPoint;
                        //        return true;
                        //    }
                        //}
                        isSet = true;
                        m_PointType = PointType.PortPoint;
                    }
                }
                #endregion

                #region 捕捉中点
                if ( pResPoint[1] != null && pResPoint[1].IsEmpty == false )
                {
                    if ( isSet )
                    {
                        if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) > GetDistance ( SnapRefrencePoint , pResPoint[1] ) )
                        {
                            pClosedPoint = pResPoint[1];

                            //在允许误差范围内                           
                            //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                            //{
                            //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                            //    {
                            //        m_SnapResultPoint = pClosedPoint;
                            //        m_PointType = PointType.MidPoint;
                            //        return true;
                            //    }
                            //}
                            m_PointType = PointType.MidPoint;
                        }
                    }
                    else
                    {
                        pClosedPoint = pResPoint[1];

                        //在允许误差范围内
                        //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                        //{
                        //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                        //    {
                        //        m_SnapResultPoint = pClosedPoint;
                        //        m_PointType = PointType.MidPoint;
                        //        return true;
                        //    }
                        //}
                        isSet = true;
                        m_PointType = PointType.MidPoint;
                    }
                }
                #endregion

                #region 捕捉节点
                if ( pResPoint[2] != null && pResPoint[2].IsEmpty == false )
                {
                    if ( isSet )
                    {
                        if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) > GetDistance ( SnapRefrencePoint , pResPoint[2] ) )                        
                        {
                            pClosedPoint = pResPoint[2];
                            //在允许误差范围内
                            //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                            //{
                            //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                            //    {
                            //        m_SnapResultPoint = pClosedPoint;
                            //        m_PointType = PointType.VertexPoint;
                            //        return true;
                            //    }
                            //}
                            m_PointType = PointType.VertexPoint;
                        }
                    }
                    else
                    {
                        pClosedPoint = pResPoint[2];
                        //在允许误差范围内
                        //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                        //{
                        //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                        //    {
                        //        m_SnapResultPoint = pClosedPoint;
                        //        m_PointType = PointType.VertexPoint;
                        //        return true;
                        //    }
                        //}
                        isSet = true;
                        m_PointType = PointType.VertexPoint;
                    }
                }
                #endregion

                #region 捕捉边上点
                if ( pResPoint[3] != null && pResPoint[3].IsEmpty == false )
                {
                    if ( isSet )
                    {
                        if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) > GetDistance ( SnapRefrencePoint , pResPoint[3] ) )
                        {
                            pClosedPoint = pResPoint[3];
                            //在允许误差范围内
                            //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                            //{
                            //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                            //    {
                            //        m_SnapResultPoint = pClosedPoint;
                            //        m_PointType = PointType.BoundryPoint;
                            //        return true;
                            //    }
                            //}
                            m_PointType = PointType.BoundryPoint;
                        }
                    }
                    else
                    {
                        pClosedPoint = pResPoint[3];
                        //在允许误差范围内
                        //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                        //{
                        //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                        //    {
                        //        m_SnapResultPoint = pClosedPoint;
                        //        m_PointType = PointType.BoundryPoint;
                        //        return true;
                        //    }
                        //}
                        isSet = true;
                        m_PointType = PointType.BoundryPoint;
                    }
                }
                #endregion

                #region 捕捉中心点
                if ( pResPoint[4] != null && pResPoint[4].IsEmpty == false )
                {
                    if ( isSet )
                    {
                        if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) > GetDistance ( SnapRefrencePoint , pResPoint[4] ) )
                        {
                            pClosedPoint = pResPoint[4];
                            //在允许误差范围内
                            //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                            //{
                            //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                            //    {
                            //        m_SnapResultPoint = pClosedPoint;
                            //        m_PointType = PointType.CenterPoint;
                            //        return true;
                            //    }
                            //}
                            m_PointType = PointType.CenterPoint;
                        }
                    }
                    else
                    {
                        pClosedPoint = pResPoint[4];
                        //在允许误差范围内
                        //if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                        //{
                        //    if ( GetDistance ( pClosedPoint , SnapRefrencePoint ) < m_IgnoreDistance )
                        //    {
                        //        m_SnapResultPoint = pClosedPoint;
                        //        m_PointType = PointType.CenterPoint;
                        //        return true;
                        //    }
                        //}
                        isSet = true;
                        m_PointType = PointType.CenterPoint;
                    }
                }
                #endregion

            }

            //计算交点集合
            if ( IsSnapIntersectPoint )
            {
                while ( list_AllFeatures.Count > 0 )
                {
                    pPointColl_Intersect.AddPointCollection ( GetAllIntersect ( list_AllFeatures[0] , list_AllFeatures ) );
                    list_AllFeatures.RemoveAt ( 0 );
                }
            }

            //得到最近的交点
            IPoint pIntersectPoint = GetClosedPoint ( pPointColl_Intersect , SnapRefrencePoint );

            //比较最近的交点和其他类型点中最近点到参考点的距离
            if ( pIntersectPoint != null && pIntersectPoint.IsEmpty == false )
            {
                if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
                {
                    if ( GetDistance ( pIntersectPoint , SnapRefrencePoint ) < GetDistance ( pClosedPoint , SnapRefrencePoint ) )
                    {
                        m_SnapResultPoint = pIntersectPoint;
                        //b_IsHasSnaped = true;
                        m_PointType = PointType.IntersectPoint;
                    }
                    else
                    {
                        m_SnapResultPoint = pClosedPoint;
                        //b_IsHasSnaped = true;
                    }
                }
                else
                {
                    m_SnapResultPoint = null;
                    //b_IsHasSnaped = false;
                }
            }
            else if ( pClosedPoint != null && pClosedPoint.IsEmpty == false )
            {
                m_SnapResultPoint = pClosedPoint;
                //b_IsHasSnaped = true;
            }
            else
            {
                m_SnapResultPoint = null;
                //b_IsHasSnaped = false;
            }

            return true;
        }

        //返回两点间距离的平方 
        private double GetDistance ( IPoint point1 , IPoint point2 )
        {
            return ( ( point1.X - point2.X ) * ( point1.X - point2.X ) + 
                ( point1.Y - point2.Y ) * ( point1.Y - point2.Y ) );
        }

        #region 获得交点的算法 GetIntersectPointCollection
        /// <summary>
        /// 得到Feature与 FeatureList所有的交点
        /// </summary>
        /// <param name="vNewFeature"></param>
        /// <param name="vFeatureCol"></param>
        /// <returns></returns>
        private IPointCollection GetAllIntersect ( IFeature OneOfFeature , List<IFeature> list_AllFeatures )
        {
            IPolyline tempLine = new PolylineClass ();
            IPointCollection pPointColl = tempLine as IPointCollection;
            pPointColl.AddPointCollection ( OneOfFeature.Shape as IPointCollection );

            IPointCollection vItersectCol = new MultipointClass ();


            IMultipoint vIntersectPnt = new MultipointClass ();

            IFeature vFeature;
            for ( int i = 0 ; i < list_AllFeatures.Count ; i++ )
            {
                vFeature = list_AllFeatures[i];
                if ( vFeature != OneOfFeature )
                {
                    vIntersectPnt = GetIntersection ( vFeature.Shape , tempLine as IPolyline ) as IMultipoint;
                    if ( vIntersectPnt != null )
                        vItersectCol.AddPointCollection ( vIntersectPnt as IPointCollection );
                }
            }

            return vItersectCol;

        }

        /// <summary>
        /// 得到Geometry和Polyline的交点
            /// </summary>
        /// <param name="vIntersect"></param>
        /// <param name="vOther"></param>
        /// <returns></returns>
        private IGeometry GetIntersection ( IGeometry vIntersect , IPolyline vOther )
        {
            //判断这两个是否有交点
            IEnvelope pEnvIntersect = vIntersect.Envelope;
            IEnvelope pEnvOther = vOther.Envelope;

            if ( pEnvIntersect.XMax < pEnvOther.XMin || pEnvIntersect.XMin > pEnvOther.XMax ||
                pEnvIntersect.YMax < pEnvOther.YMin || pEnvIntersect.YMin > pEnvOther.YMax )
                return null;

            if ( vIntersect.SpatialReference != null && !vIntersect.SpatialReference.Equals ( vOther.SpatialReference ) )
            {
                vOther.Project ( vIntersect.SpatialReference );
            }

            ITopologicalOperator vTopoOp = vIntersect as ITopologicalOperator;
            vTopoOp.Simplify ();
            IGeometry vGeomResult = vTopoOp.Intersect ( vOther , esriGeometryDimension.esriGeometry0Dimension );
            if ( vGeomResult == null ) return null;

            if ( vGeomResult is IPointCollection )
            {
                if ( !( ( vGeomResult as IPointCollection ).PointCount >= 1 ) )
                    return null;
            }

            if ( !( vGeomResult.GeometryType == esriGeometryType.esriGeometryMultipoint ) )
            {
                return null;
            }

            return vGeomResult;
        }

        #endregion

    }
}
