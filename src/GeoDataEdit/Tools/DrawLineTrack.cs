using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;

namespace GeoDataEdit
{
    internal class DrawLineTrack
    {

        private DrawTypeConstant m_nDrawType;                                         //当前所画类型
        //1为普通点，2普通线，3为普通面，4为平行线，5添加节点，6删除节点，7移动节点，8删除要素,9移动要素,
        //10画已知线的平行线,11改变线的方向,12过点作垂线,13比例缩放,14要素转动,15要素属性修改,16合并线,
        //17合并面,18分割线,19分割面,20画注记,21修改注记,22选择
        //23倒圆角，24倒直角，25延伸线，26修剪线，27镜象，28投影,29分解线,30打断线
        public IPointCollection m_pDrawedPoints;     //已经画的点集
        public IGeometryCollection m_pGeometryBag;   //存放路径或者环的包

        private IMap m_pCurMap;                          //当前地图
        private IFeatureLayer m_pCurFeatureLayer;        //当前编辑层
        private esriGeometryType m_nShapeType;        //当前图层几何类型

        private IOperationStack m_pOperationStack;   //操作栈
        private IPoint m_pLastMouseStopedPoint;        //鼠标上次停靠的位置

        private ClsEditorMain clsEditorMain;

        public DrawLineTrack ()
        {
            this.m_pDrawedPoints = null;
            this.m_pGeometryBag = null;
            this.m_pLastMouseStopedPoint = new PointClass ();
        }

        ~DrawLineTrack ()
        {
            if ( this.m_pDrawedPoints != null )
                this.m_pDrawedPoints = null;

            if ( this.m_pGeometryBag != null )
                this.m_pGeometryBag = null;

            this.m_pLastMouseStopedPoint = null;
        }

        public void ReSet ()
        {
            this.m_pDrawedPoints = null;
            this.m_pGeometryBag = null;

            if ( this.m_pLastMouseStopedPoint == null )
                this.m_pLastMouseStopedPoint = new PointClass ();
            else
                this.m_pLastMouseStopedPoint.SetEmpty ();

        }

        public DrawTypeConstant GetCurDrawType ()
        {
            return this.m_nDrawType;
        }

        public Boolean InitTrack ( DrawTypeConstant nDrawType , ref IMap pCurMap , ClsEditorMain _clsEditorMain )
        {

            if ( _clsEditorMain == null ) return false;
            clsEditorMain = _clsEditorMain;

            m_pCurFeatureLayer = clsEditorMain.EditFeatureLayer;
            //if ( m_pCurFeatureLayer == null ) return false;
            //if ( m_pCurFeatureLayer.FeatureClass == null ) return false;

            //m_pOperationStack = clsEditorMain.OperationStack;
            //if ( m_pOperationStack == null ) return false;

            this.m_pCurMap = pCurMap;
            if ( m_pCurMap == null ) return false;
            if ( m_pCurMap.LayerCount < 1 ) return false;


            this.m_nDrawType = nDrawType;
            this.m_nShapeType = esriGeometryType.esriGeometryPolygon;

            return true;
        }

        //检查当前图层是否适合当前的编辑类型
        public bool CheckCanDraw ()
        {
            if ( m_pCurMap == null )
            {
                System.Windows.Forms.MessageBox.Show ( "请先加载地图！" , "系统提示" );
                return false;
            }

            if ( m_pCurMap.LayerCount < 1 )
            {
                System.Windows.Forms.MessageBox.Show ( "请先加载图层！" , "系统提示" );
                return false;
            }

            if ( clsEditorMain.EditWorkspace == null )
            {
                System.Windows.Forms.MessageBox.Show ( "编辑工程不能为空！" , "系统提示" );
                return false;
            }


            if ( !( m_pCurFeatureLayer.FeatureClass as IWorkspaceEdit ).IsBeingEdited () )
            {
                System.Windows.Forms.MessageBox.Show ( "请先启动编辑！" , "系统提示" );
                return false;
            }

            if ( clsEditorMain.EditFeatureLayer == null )
            {
                System.Windows.Forms.MessageBox.Show ( "没有设置当前编辑的层！" , "系统提示" );
                return false;
            }

            if ( clsEditorMain.EditFeatureLayer.Visible == false )
            {
                System.Windows.Forms.MessageBox.Show ( "当前层不可见，不能编辑！" , "系统提示" );
                return false;
            }

            if ( !( clsEditorMain.EditFeatureLayer is IGeoFeatureLayer ) )
            {
                System.Windows.Forms.MessageBox.Show ( "当前层不能进行要素编辑！" , "系统提示" );
                return false;
            }



            IFeatureClass pFeatureClass = m_pCurFeatureLayer.FeatureClass;

            if ( pFeatureClass == null )
            {
                System.Windows.Forms.MessageBox.Show ( "当前层没有关联的要素类！" , "系统提示" );
                return false;
            }

            esriGeometryType nGeometryType = pFeatureClass.ShapeType;

            switch ( m_nDrawType )
            {
                case DrawTypeConstant.CommonPoint:
                    if ( nGeometryType != esriGeometryType.esriGeometryPoint )
                    {
                        System.Windows.Forms.MessageBox.Show ( "当前编辑的层不是点要素层！" , "系统提示" );
                        return false;
                    }
                    break;

                case DrawTypeConstant.CommonLine:
                case DrawTypeConstant.ParallelLine:
                case DrawTypeConstant.AnchorParallelLine:
                case DrawTypeConstant.ChangeLineDirection:
                case DrawTypeConstant.VerticalLineFromPoint:
                case DrawTypeConstant.UnionLine:
                case DrawTypeConstant.SplitLine:
                case DrawTypeConstant.InRoundAngle:
                    if ( nGeometryType != esriGeometryType.esriGeometryPolyline )
                    {
                        System.Windows.Forms.MessageBox.Show ( "当前编辑的层不是线要素层！" , "系统提示" );
                        return false;
                    }
                    break;

                case DrawTypeConstant.CommonPolygon:
                case DrawTypeConstant.UnionPolygon:
                case DrawTypeConstant.SplitPolygon:
                    if ( nGeometryType != esriGeometryType.esriGeometryPolygon )
                    {
                        System.Windows.Forms.MessageBox.Show ( "当前编辑的层不是面要素层！" , "系统提示" );
                        return false;
                    }
                    break;

                case DrawTypeConstant.AddVertex:
                case DrawTypeConstant.DeleteVertex:
                case DrawTypeConstant.ScaleZoom:
                    if ( nGeometryType != esriGeometryType.esriGeometryPolyline && nGeometryType != esriGeometryType.esriGeometryPolygon )
                    {
                        System.Windows.Forms.MessageBox.Show ( "当前编辑的层只能是线要素层和面要素层！" , "系统提示" );
                        return false;
                    }
                    break;

                default:
                    break;
            }
            return true;

        }

        public bool InitTrack ( DrawTypeConstant nDrawType , IMap pCurMap , ClsEditorMain _clsEditorMain )
        {

            if ( _clsEditorMain == null ) return false;
            clsEditorMain = _clsEditorMain;

            m_pOperationStack = clsEditorMain.OperationStack;
            if ( m_pOperationStack == null ) return false;

            this.m_pCurMap = pCurMap;
            if ( m_pCurMap == null ) return false;

            this.m_nDrawType = nDrawType;
            this.m_nShapeType = esriGeometryType.esriGeometryPolyline;

            //this.m_pDrawedPoints = null;

            return true;
        }

        //得到以画点数（不含已完成的路径或环上的点）
        public int GetPointCount ()
        {
            if ( this.m_pDrawedPoints == null )
                return 0;
            else
                return this.m_pDrawedPoints.PointCount;

        }

        //环或路径数
        public int GetPartCount ()
        {
            if ( this.m_pGeometryBag == null )
                return 0;
            else
                return this.m_pGeometryBag.GeometryCount;

        }


        public IPoint AddPoint ( double x , double y , bool blOnMap )
        {
            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IPoint pClickPoint;

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            if ( blOnMap == true )
            {
                pClickPoint = new PointClass ();
                pClickPoint.X = x;
                pClickPoint.Y = y;
            }
            else
            {
                IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint ( ( int )x , ( int )y );
                this.clsEditorMain.SnapPoint.SnapExcute ( pPoint );
                pClickPoint = this.clsEditorMain.SnapPoint.SnapResultPoint;
            }


            if ( this.m_nDrawType == DrawTypeConstant.AddVertex ) return pClickPoint;
            if ( this.m_nShapeType == esriGeometryType.esriGeometryPoint ) return pClickPoint;

            int nPointCount;
            if ( this.m_pDrawedPoints == null )
                nPointCount = 0;
            else
                nPointCount = this.m_pDrawedPoints.PointCount;

            if ( nPointCount == 0 )
                this.m_pDrawedPoints = new PolylineClass ();

            this.m_pDrawedPoints.AddPoint ( pClickPoint , ref pBObj , ref pAObj );

            DrawWhileDown ( ref pActiveView );

            return pClickPoint;
        }

        //
        private void DrawWhileDown ( ref IActiveView pActiveView )
        {
            int nPointCount;

            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if ( nPointCount < 0 ) return;

            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref this.m_pCurMap ) as ISymbol;
            ILineSymbol pLineSymbol = pFeedbackSymbol as ILineSymbol;
            ISymbol pPFeedbackSymbol = MakePolygonFeedbackSymbol ( ref this.m_pCurMap , ref  pLineSymbol ) as ISymbol;
            ISymbol pFeatureSymbol = MakeFeatureSymbol ( ref this.m_pCurMap ) as ISymbol;
            ISimpleMarkerSymbol pPTSymbol = MakeMarkerSymbol ( ref this.m_pCurMap , 0 , 0 , 128 , false ) as ISimpleMarkerSymbol;
            ISimpleMarkerSymbol pTSSymbol = MakeMarkerSymbol ( ref this.m_pCurMap , 255 , 0 , 0 , false ) as ISimpleMarkerSymbol;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;

            if ( ( this.m_nDrawType == DrawTypeConstant.CommonLine ) || ( this.m_nDrawType == DrawTypeConstant.CommonPolygon ) )
            {
                pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 /*esriScreenCache.esriNoScreenCache*/);

                IPoint pLastPoint;
                pLastPoint = this.m_pDrawedPoints.get_Point ( nPointCount );

                ISymbol pSymbol = pTSSymbol as ISymbol;
                //画新的尾节点
                pScreenDisplay.SetSymbol ( pSymbol );
                pScreenDisplay.DrawPoint ( pLastPoint );

                if ( nPointCount >= 1 )
                {
                    IPoint pLLastPoint;
                    pLLastPoint = this.m_pDrawedPoints.get_Point ( nPointCount - 1 );
                    //上次的尾节点变为普通节点
                    pSymbol = pPTSymbol as ISymbol;
                    pScreenDisplay.SetSymbol ( pSymbol );
                    pScreenDisplay.DrawPoint ( pLLastPoint );

                    IPolyline pPolyline;
                    pPolyline = MakePolylineFromTowPoint ( pLLastPoint , pLastPoint );
                    //添加新的要素线
                    pSymbol = pFeatureSymbol as ISymbol;
                    pScreenDisplay.SetSymbol ( pSymbol );
                    pScreenDisplay.DrawPolyline ( pPolyline );

                    if ( m_pLastMouseStopedPoint.IsEmpty == false )
                    {
                        pPolyline = MakePolylineFromTowPoint ( pLLastPoint , m_pLastMouseStopedPoint );
                        //擦除上次的反馈线
                        pSymbol = pFeedbackSymbol as ISymbol;
                        pScreenDisplay.SetSymbol ( pSymbol );
                        pScreenDisplay.DrawPolyline ( pPolyline );
                        //画新的反馈线
                        pPolyline = MakePolylineFromTowPoint ( pLastPoint , m_pLastMouseStopedPoint );
                        pScreenDisplay.DrawPolyline ( pPolyline );
                    }
                }
                pScreenDisplay.FinishDrawing ();
            }

        }

        //
        public string GetLastPointXYStr ()
        {
            int nPointCount;
            double fX = 0;
            double fY;
            string sX;
            string sY;

            if ( this.m_pDrawedPoints != null )
            {
                nPointCount = this.m_pDrawedPoints.PointCount;

                if ( nPointCount > 0 )
                {
                    try
                    {
                        IPointCollection pPointColl = this.m_pDrawedPoints as IPointCollection;
                        IGeometry pGeometry = m_pDrawedPoints as IGeometry;
                        fX = pPointColl.get_Point ( nPointCount - 1 ).X;
                    }
                    catch ( Exception ex )
                    { }

                    sX = fX.ToString ( ".###" );

                    fY = this.m_pDrawedPoints.get_Point ( nPointCount - 1 ).Y;
                    sY = fY.ToString ( ".###" );

                    return sX + "," + sY;

                }
            }

            if ( this.m_pGeometryBag != null )
            {
                int nGeometryCount;
                nGeometryCount = this.m_pGeometryBag.GeometryCount;

                if ( nGeometryCount > 0 )
                {
                    IPointCollection pPointCollection = this.m_pGeometryBag.get_Geometry ( nGeometryCount - 1 ) as IPointCollection;

                    nPointCount = pPointCollection.PointCount;

                    if ( this.m_nDrawType == DrawTypeConstant.CommonPolygon )
                        nPointCount = nPointCount - 2;
                    else
                        nPointCount = nPointCount - 1;

                    fX = pPointCollection.get_Point ( nPointCount ).X;
                    sX = fX.ToString ( ".###" );

                    fY = pPointCollection.get_Point ( nPointCount ).Y;
                    sY = fY.ToString ( ".###" );

                    return sX + "," + sY;
                }
            }
            return "";
        }

        //得到已画形状
        public IGeometry GetDrawGeometry ( double fLen )
        {

            int nPointCount;
            nPointCount = GetPointCount ();

            int nPart;
            nPart = GetPartCount ();

            if ( ( nPointCount == 0 ) && ( nPart == 0 ) ) return null;

            nPointCount = nPointCount - 1;

            IPointCollection pPointCollection;

            IPoint pPoint;

            int i;

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            if ( this.m_nDrawType == DrawTypeConstant.CommonLine )
            {
                if ( nPart < 1 )
                    this.m_pGeometryBag = new PolylineClass ();


                if ( nPointCount >= 1 )
                {
                    IPath pPath = new PathClass ();

                    pPointCollection = pPath as IPointCollection;

                    for ( i = 0 ; i <= nPointCount ; i++ )
                    {
                        pPoint = this.m_pDrawedPoints.get_Point ( i );
                        pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );
                    }

                    this.m_pGeometryBag.AddGeometry ( pPath , ref pBObj , ref pAObj );
                    pPath = null;
                }
                return this.m_pGeometryBag as IGeometry;
            }

            if ( this.m_nDrawType == DrawTypeConstant.CommonPolygon )
            {
                if ( nPart < 1 )
                    this.m_pGeometryBag = new PolygonClass ();

                if ( nPointCount >= 2 )
                {
                    IRing pRing = new RingClass ();

                    pPointCollection = pRing as IPointCollection;

                    for ( i = 0 ; i <= nPointCount ; i++ )
                    {
                        pPoint = this.m_pDrawedPoints.get_Point ( i );
                        pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );
                    }

                    pPoint = this.m_pDrawedPoints.get_Point ( 0 );
                    pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );

                    this.m_pGeometryBag.AddGeometry ( pRing , ref pBObj , ref pAObj );
                    pRing = null;
                }
                return this.m_pGeometryBag as IGeometry;
            }

            if ( this.m_pGeometryBag != null )
                this.m_pGeometryBag = null;


            if ( ( this.m_nDrawType == DrawTypeConstant.ParallelLine ) && ( nPointCount >= 1 ) )
            {
                IPolyline pPolyline = GetParallelPolyline2 ( this.m_pDrawedPoints , null , fLen , this.m_pCurMap );

                this.m_pGeometryBag = new PolylineClass ();

                IGeometryCollection pGeometryCollection = pPolyline as IGeometryCollection;

                if ( pGeometryCollection == null )
                    nPart = -1;
                else
                    nPart = pGeometryCollection.GeometryCount - 1;

                IGeometry pGeometry;

                for ( i = 0 ; i <= nPart ; i++ )
                {

                    pGeometry = pGeometryCollection.get_Geometry ( i );
                    this.m_pGeometryBag.AddGeometry ( pGeometry , ref pBObj , ref pAObj );
                }

                pGeometryCollection = this.m_pDrawedPoints as IGeometryCollection;

                if ( pGeometryCollection == null )
                    nPart = -1;
                else
                    nPart = pGeometryCollection.GeometryCount - 1;

                for ( i = 0 ; i <= nPart ; i++ )
                {
                    pGeometry = pGeometryCollection.get_Geometry ( i );
                    this.m_pGeometryBag.AddGeometry ( pGeometry , ref pBObj , ref pAObj );
                }

                return m_pGeometryBag as IGeometry;

            }

            this.m_pDrawedPoints = null;

            return null;
        }

        //
        public IPolyline MakePolylineFromTowPoint ( IPoint sTartPoint , IPoint pEndPoint )
        {
            ILine pLine;
            pLine = new LineClass ();
            pLine.FromPoint = sTartPoint;
            pLine.ToPoint = pEndPoint;

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            IPolyline pPolyline = new PolylineClass ();

            ISegmentCollection pSegmentCollection = pPolyline as ISegmentCollection;
            pSegmentCollection.AddSegment ( pLine as ISegment , ref  pBObj , ref pAObj );

            pLine = null;

            return pPolyline;
        }


        public bool MakeClose ()
        {
            if ( this.m_pDrawedPoints == null ) return false;

            int nPointCount = this.m_pDrawedPoints.PointCount - 1;
            if ( nPointCount < 0 ) return false;

            IPoint pPoint;
            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            pPoint = this.m_pDrawedPoints.get_Point ( 0 );
            this.m_pDrawedPoints.AddPoint ( pPoint , ref pBObj , ref pAObj );
            return true;
        }

        //设置do或undo
        public void SetDo ( bool blDo )
        {
            clsEditorMain.BDo = blDo;
            //this.m_pSnapPointClass.blDo = blDo;
        }
        //判断do或undo
        public bool GetDo ()
        {
            //return this.m_pSnapPointClass.blDo;
            return clsEditorMain.BDo;
        }

        //得到操作栈
        public IOperationStack GetOperationStack ()
        {
            return this.m_pOperationStack;
        }

        //放弃
        public void GiveUpDraw ()
        {
            if ( this.m_pDrawedPoints != null )
                this.m_pDrawedPoints = null;

            this.m_pLastMouseStopedPoint.SetEmpty ();

            if ( this.m_pGeometryBag != null )
                this.m_pGeometryBag = null;

            ESRI.ArcGIS.Carto.IActiveView pActiveView;
            pActiveView = this.m_pCurMap as ESRI.ArcGIS.Carto.IActiveView;
            pActiveView.Refresh ();
        }

        //长度更改（两点以上时）
        public bool LengthChange ( double fNewLength )
        {
            int nPointCount = this.m_pDrawedPoints.PointCount - 1;

            IPoint pfPoint = this.m_pDrawedPoints.get_Point ( nPointCount - 1 );

            IPoint pOldTPoint = this.m_pDrawedPoints.get_Point ( nPointCount );

            ICurve pCurve = new LineClass () as ICurve;
            pCurve.FromPoint = pfPoint;
            pCurve.ToPoint = pOldTPoint;

            IPoint pNewTPoint = new PointClass ();

            IConstructPoint pConstructPoint = pNewTPoint as IConstructPoint;
            pConstructPoint.ConstructAlong ( pCurve , esriSegmentExtension.esriExtendTangentAtTo , fNewLength , false );

            this.m_pDrawedPoints.RemovePoints ( nPointCount , 1 );

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            this.m_pDrawedPoints.AddPoint ( pNewTPoint , ref pBObj , ref pAObj );

            ( m_pCurMap as IActiveView ).Refresh ();

            pCurve = null;
            pNewTPoint = null;
            return true;
        }

        //新路径或环开始
        public bool NewPartBegin ()
        {
            if ( this.m_pDrawedPoints == null ) return false;

            int nPointCount = this.m_pDrawedPoints.PointCount - 1;
            if ( nPointCount < 0 ) return false;

            IPoint pPoint;
            object pBObj = Type.Missing;
            object pAObj = Type.Missing;
            IPointCollection pPointCollection;

            //多路径
            if ( m_nDrawType == DrawTypeConstant.CommonLine )
            {
                if ( this.m_pGeometryBag == null )
                    this.m_pGeometryBag = new PolylineClass ();


                IPath pPath = new PathClass ();

                pPointCollection = pPath as IPointCollection;


                for ( int i = 0 ; i <= nPointCount ; i++ )
                {
                    pPoint = this.m_pDrawedPoints.get_Point ( i );
                    pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );
                }

                this.m_pGeometryBag.AddGeometry ( pPath , ref pBObj , ref pAObj );
                pPath = null;
            }
            //多环
            if ( this.m_nDrawType == DrawTypeConstant.CommonPolygon )
            {
                if ( this.m_pGeometryBag == null )
                    this.m_pGeometryBag = new PolygonClass ();

                IRing pRing = new RingClass ();

                pPointCollection = pRing as IPointCollection;

                for ( int i = 0 ; i <= nPointCount ; i++ )
                {
                    pPoint = this.m_pDrawedPoints.get_Point ( i );
                    pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );
                }
                pPoint = this.m_pDrawedPoints.get_Point ( 0 );
                pPointCollection.AddPoint ( pPoint , ref pBObj , ref pAObj );

                this.m_pGeometryBag.AddGeometry ( pRing , ref pBObj , ref pAObj );
                pRing = null;
            }

            this.m_pDrawedPoints = null;
            this.m_pLastMouseStopedPoint.SetEmpty ();

            ( m_pCurMap as IActiveView ).Refresh ();

            return true;
        }


        //点回退
        public bool GoBackaPoint ()
        {
            int nPointCount;
            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;


            bool blCanBack = false;

            int nGeometryCount;
            IPoint pPoint;

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            IPointCollection pPoints;
            int nCount;


            if ( nPointCount >= 0 )
            {
                this.m_pDrawedPoints.RemovePoints ( nPointCount , 1 );
                blCanBack = true;

                if ( nPointCount == 1 )
                    this.m_pLastMouseStopedPoint.SetEmpty ();  //将上次停靠位置清空

            }
            else
            {
                if ( this.m_nDrawType == DrawTypeConstant.CommonLine )
                {
                    if ( this.m_pGeometryBag == null )
                        nGeometryCount = -1;
                    else
                        nGeometryCount = this.m_pGeometryBag.GeometryCount - 1;

                    if ( nGeometryCount >= 0 )
                    {
                        if ( m_pDrawedPoints != null )
                            m_pDrawedPoints = null;

                        m_pDrawedPoints = new PolylineClass ();

                        IPath pPath = this.m_pGeometryBag.get_Geometry ( nGeometryCount ) as IPath;

                        pPoints = pPath as IPointCollection;

                        nCount = pPoints.PointCount - 1;

                        nCount = nCount - 1;
                        for ( int i = 0 ; i <= nCount ; i++ )
                        {
                            pPoint = pPoints.get_Point ( i );
                            this.m_pDrawedPoints.AddPoint ( pPoint , ref pBObj , ref pAObj );
                        }

                        this.m_pGeometryBag.RemoveGeometries ( nGeometryCount , 1 );
                        blCanBack = true;
                    }
                }

                if ( this.m_nDrawType == DrawTypeConstant.CommonPolygon )
                {
                    if ( this.m_pGeometryBag == null )
                        nGeometryCount = -1;
                    else
                        nGeometryCount = this.m_pGeometryBag.GeometryCount - 1;

                    if ( nGeometryCount >= 0 )
                    {
                        if ( this.m_pDrawedPoints != null )
                            this.m_pDrawedPoints = null;

                        this.m_pDrawedPoints = new PolylineClass ();

                        IRing pRing = this.m_pGeometryBag.get_Geometry ( nGeometryCount ) as IRing;

                        pPoints = pRing as IPointCollection;

                        nCount = pPoints.PointCount - 1;
                        nCount = nCount - 2;

                        for ( int i = 0 ; i <= nCount ; i++ )
                        {
                            pPoint = pPoints.get_Point ( i );
                            this.m_pDrawedPoints.AddPoint ( pPoint , ref pBObj , ref pAObj );
                        }

                        this.m_pGeometryBag.RemoveGeometries ( nGeometryCount , 1 );
                        blCanBack = true;
                    }
                }
            }

            //使先前捕捉无效

            ( m_pCurMap as IActiveView ).Refresh ();

            return blCanBack;
        }


        //重绘
        public bool ReDraw ( bool blDrawTrack )
        {
            IActiveView pActiveView = this.m_pCurMap as IActiveView;


            this.m_pLastMouseStopedPoint.SetEmpty (); ;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 /*esriScreenCache.esriNoScreenCache*/);

            IPolyline pPolyline;

            if ( ( this.m_nDrawType == DrawTypeConstant.CommonLine ) || ( this.m_nDrawType == DrawTypeConstant.CommonPolygon ) )
            {
                //画已完成的路径或环
                if ( this.m_pGeometryBag != null )
                {
                    int nGeometryCount;
                    nGeometryCount = this.m_pGeometryBag.GeometryCount - 1;
                    if ( nGeometryCount >= 0 )
                    {
                        if ( this.m_nShapeType == esriGeometryType.esriGeometryPolygon )
                        {
                            for ( int i = 0 ; i <= nGeometryCount ; i++ )
                            {
                                IRing pRing = this.m_pGeometryBag.get_Geometry ( i ) as IRing;

                                pPolyline = MakePolyLine ( pRing );
                                DrawPoints ( ref pScreenDisplay , ref pPolyline , ref this.m_pCurMap );
                            }
                        }
                        else
                        {
                            pPolyline = this.m_pGeometryBag as IPolyline;
                            DrawPoints ( ref pScreenDisplay , ref pPolyline , ref this.m_pCurMap );
                        }
                    }
                }
                //画未完成的路径或环
                if ( m_pDrawedPoints != null )
                {
                    int nPointCount;
                    nPointCount = m_pDrawedPoints.PointCount - 1;

                    if ( nPointCount >= 0 )
                    {
                        pPolyline = m_pDrawedPoints as IPolyline;
                        DrawPoints ( ref pScreenDisplay , ref pPolyline , ref this.m_pCurMap );

                        IPoint pPoint = this.m_pDrawedPoints.get_Point ( nPointCount );

                        if ( ( blDrawTrack == true ) && ( this.m_pLastMouseStopedPoint.IsEmpty == false ) )
                            DrawFeedback ( ref pScreenDisplay , pPoint , m_pLastMouseStopedPoint , ref this.m_pCurMap );
                        else
                        {
                            if ( this.m_pLastMouseStopedPoint.IsEmpty == false )
                                this.m_pLastMouseStopedPoint.SetEmpty ();
                        }
                    }
                }
            }

            pScreenDisplay.FinishDrawing ();
            return true;
        }

        //由环生成多边线
        private IPolyline MakePolyLine ( IRing pRing )
        {
            IPolyline pPolyline = new PolylineClass ();

            IPointCollection pPointCollection = pPolyline as IPointCollection;

            IPointCollection pPoints = pRing as IPointCollection;

            int nCount = pPoints.PointCount - 1;

            object obj = Type.Missing;

            for ( int i = 0 ; i <= nCount ; i++ )
                pPointCollection.AddPoint ( pPoints.get_Point ( i ) , ref obj , ref obj );

            return pPolyline;
        }

        //画节点
        private void DrawPoints ( ref IScreenDisplay pScreenDisplay , ref IPolyline pPolyline , ref IMap pCurMap )
        {

            ISymbol pFeatureSymbol = MakeFeatureSymbol ( ref pCurMap ) as ISymbol;
            ISimpleMarkerSymbol pPTSymbol = MakeMarkerSymbol ( ref pCurMap , 0 , 0 , 128 , false ) as ISimpleMarkerSymbol;
            ISimpleMarkerSymbol pTSSymbol = MakeMarkerSymbol ( ref pCurMap , 255 , 0 , 0 , false ) as ISimpleMarkerSymbol;

            ISymbol pSymbol = pFeatureSymbol;

            IPointCollection pPointCollection = pPolyline as IPointCollection;

            pScreenDisplay.SetSymbol ( pSymbol );
            pScreenDisplay.DrawPolyline ( pPointCollection as IPolyline );

            int nPointCount;
            if ( pPointCollection == null )
                nPointCount = -1;
            else
                nPointCount = pPointCollection.PointCount - 1;

            if ( nPointCount < 0 ) return;

            for ( int i = 0 ; i <= nPointCount ; i++ )
            {
                IPoint pPoint = pPointCollection.get_Point ( i );

                if ( i == 0 )
                {
                    pScreenDisplay.SetSymbol ( pPTSymbol as ESRI.ArcGIS.Display.ISymbol );
                    pScreenDisplay.DrawPoint ( pPoint );
                }
                else if ( i == nPointCount )
                {
                    pScreenDisplay.SetSymbol ( pTSSymbol as ESRI.ArcGIS.Display.ISymbol );
                    pScreenDisplay.DrawPoint ( pPoint );
                }
                else
                    pScreenDisplay.DrawPoint ( pPoint );

            }
        }

        //具体画反馈线
        private void DrawFeedback ( ref IScreenDisplay pScreenDisplay , IPoint sTartPoint , IPoint pEndPoint , ref IMap pCurMap )
        {
            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref pCurMap ) as ISymbol;

            IPolyline pPolyline = MakePolylineFromTowPoint ( sTartPoint , pEndPoint );

            ISimpleLineSymbol pSimpleLineSymbol = pFeedbackSymbol as ISimpleLineSymbol;

            pScreenDisplay.SetSymbol ( pSimpleLineSymbol as ISymbol );
            pScreenDisplay.DrawPolyline ( pPolyline );

            pPolyline = null;
        }

        //具体画反馈线
        private void DrawFeedbackLine ( ref IScreenDisplay pScreenDisplay , IPolyline pPolyline , ref IMap pCurMap )
        {
            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref pCurMap ) as ISymbol;

            ISimpleLineSymbol pSimpleLineSymbol = pFeedbackSymbol as ISimpleLineSymbol;

            pScreenDisplay.SetSymbol ( pSimpleLineSymbol as ISymbol );
            pScreenDisplay.DrawPolyline ( pPolyline );
        }

        public IPoint MouseMove ( int x , int y )
        {
            IActiveView pActiveView = m_pCurMap as IActiveView;

            IPoint pMouseStopedPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint ( x , y );

            clsEditorMain.SnapPoint.SnapExcute ( pMouseStopedPoint );

            pMouseStopedPoint = clsEditorMain.SnapPoint.SnapResultPoint;

            DrawWhileMove ( ref pActiveView , pMouseStopedPoint , ref this.m_pCurMap );

            return pMouseStopedPoint;
        }

        //更新反馈线
        private bool DrawWhileMove ( ref IActiveView pActiveView , IPoint pMouseStopedPoint , ref IMap pCurMap )
        {
            int nPointCount;
            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if ( nPointCount < 0 ) return false;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;

            if ( ( this.m_nDrawType == DrawTypeConstant.CommonLine ) || ( this.m_nDrawType == DrawTypeConstant.CommonPolygon ) )
            {
                IPoint pPoint = this.m_pDrawedPoints.get_Point ( nPointCount );

                pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 /*esriScreenCache.esriNoScreenCache*/);

                if ( this.m_pLastMouseStopedPoint.IsEmpty == false )
                    DrawFeedback ( ref pScreenDisplay , pPoint , m_pLastMouseStopedPoint , ref pCurMap );


                DrawFeedback ( ref pScreenDisplay , pPoint , pMouseStopedPoint , ref pCurMap );
                this.m_pLastMouseStopedPoint.PutCoords ( pMouseStopedPoint.X , pMouseStopedPoint.Y );


                pScreenDisplay.FinishDrawing ();
                return true;
            }

            return true;

        }



        //生成反馈线的符号
        public static ISimpleLineSymbol MakeFeedbackSymbol ( ref IMap pMap )
        {
            IRgbColor pRgbColor = ModForEdit.CreatRgbColor ( 255 , 0 , 0 , false );
            pRgbColor.Transparency = 255;

            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass ();
            pSimpleLineSymbol.Color = pRgbColor;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;

            ISymbol pSymbol = pSimpleLineSymbol as ISymbol;
            //采用异或方式绘制，擦除以前画的符号
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            //下面的二行代码是不是需要修改,随着放大的不同
            pSimpleLineSymbol.Width = ModForEdit.ScaleChange ( ref pMap , 1 );
            return pSimpleLineSymbol;

        }

        //
        public static ISymbol MakePolygonFeedbackSymbol ( ref IMap pMap , ref ILineSymbol pLineSymbol )
        {
            IRgbColor pFillColor = ModForEdit.CreatRgbColor ( 0 , 0 , 0 , true );

            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass ();

            pFillSymbol.Color = pFillColor;
            pFillSymbol.Outline = pLineSymbol;

            return pFillSymbol as ISymbol;

        }

        //生成要素线的符号
        private static ISimpleLineSymbol MakeFeatureSymbol ( ref IMap pMap )
        {
            IRgbColor pRgbColor = ModForEdit.CreatRgbColor ( 0 , 128 , 0 , false );

            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass ();
            pLineSymbol.Color = pRgbColor;
            pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pLineSymbol.Width = ModForEdit.ScaleChange ( ref pMap , 1 );

            return pLineSymbol;
        }

        //节点符号
        private static ISymbol MakeMarkerSymbol ( ref IMap pMap , int nR , int nG , int nB , bool blTrans )
        {
            IRgbColor pRgbColor = ModForEdit.CreatRgbColor ( nR , nG , nB , blTrans );

            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass ();

            pMarkerSymbol.Size = ModForEdit.ScaleChange ( ref pMap , 4 );
            pMarkerSymbol.Color = pRgbColor;
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;

            return pMarkerSymbol as ISymbol;
        }

        //节点符号
        private static ISymbol MakeCanRasteMarkerSymbol ( ref IMap pMap , int nR , int nG , int nB , bool blTrans )
        {
            IRgbColor pRgbColor = ModForEdit.CreatRgbColor ( nR , nG , nB , blTrans );

            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass ();

            pMarkerSymbol.Size = ModForEdit.ScaleChange ( ref pMap , 4 );
            pMarkerSymbol.Color = pRgbColor;
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;

            ISymbol pSymbol = pMarkerSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;

            return pSymbol;
        }
        //
        public bool DrawMoveGeometrys ( ref IGeometryCollection pGeometrys , ref IPoint pfPoint , ref IPoint ptPoint , ref IPoint pLastPoint )
        {
            if ( pGeometrys == null || pfPoint == null || ptPoint == null ) return false;


            int nCount = pGeometrys.GeometryCount;
            if ( nCount < 1 ) return false;
            nCount = nCount - 1;

            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );

            double dx = ptPoint.X - pfPoint.X;
            double dy = ptPoint.Y - pfPoint.Y;
            double fLastX = 0;
            double fLastY = 0;

            if ( pLastPoint != null && !pLastPoint.IsEmpty )
            {
                fLastX = pLastPoint.X - pfPoint.X;
                fLastY = pLastPoint.Y - pfPoint.Y;
            }

            IPoint pPoint = null;
            IPolyline pPolyline = null;
            IPolygon pPolygon = null;
            ITransform2D pTransform2D = null;

            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref this.m_pCurMap ) as ISymbol;
            ILineSymbol pLineSymbol = pFeedbackSymbol as ILineSymbol;

            ISymbol pPFeedbackSymbol = MakePolygonFeedbackSymbol ( ref this.m_pCurMap , ref  pLineSymbol ) as ISymbol;

            ISymbol pPTSymbol = MakeCanRasteMarkerSymbol ( ref this.m_pCurMap , 255 , 0 , 0 , false );

            for ( int i = 0 ; i <= nCount ; i++ )
            {
                IGeometry pGeometry = pGeometrys.get_Geometry ( i );

                if ( !pGeometry.IsEmpty && pGeometry.GeometryType == esriGeometryType.esriGeometryPoint )
                {
                    pScreenDisplay.SetSymbol ( pPTSymbol );
                    pTransform2D = pGeometry as ITransform2D;

                    if ( !( fLastX == 0 && fLastY == 0 ) )
                    {
                        pTransform2D.Move ( fLastX , fLastY );

                        pPoint = pGeometry as IPoint;
                        pScreenDisplay.DrawPoint ( pPoint );

                        pTransform2D.Move ( -fLastX , -fLastY );
                    }

                    if ( !( dx == 0 && dy == 0 ) )
                    {
                        pTransform2D.Move ( dx , dy );

                        pPoint = pGeometry as IPoint;
                        pScreenDisplay.DrawPoint ( pPoint );

                        pTransform2D.Move ( -dx , -dy );
                    }
                }
                else if ( !pGeometry.IsEmpty && pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline )
                {
                    pScreenDisplay.SetSymbol ( pFeedbackSymbol );
                    pTransform2D = pGeometry as ITransform2D;

                    if ( !( fLastX == 0 && fLastY == 0 ) )
                    {
                        pTransform2D.Move ( fLastX , fLastY );

                        pPolyline = pGeometry as IPolyline;
                        pScreenDisplay.DrawPolyline ( pPolyline );

                        pTransform2D.Move ( -fLastX , -fLastY );
                    }

                    if ( !( dx == 0 && dy == 0 ) )
                    {
                        pTransform2D.Move ( dx , dy );

                        pPolyline = pGeometry as IPolyline;
                        pScreenDisplay.DrawPolyline ( pPolyline );

                        pTransform2D.Move ( -dx , -dy );
                    }
                }
                else if ( !pGeometry.IsEmpty && pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon )
                {
                    pScreenDisplay.SetSymbol ( pPFeedbackSymbol );
                    pTransform2D = pGeometry as ITransform2D;

                    if ( !( fLastX == 0 && fLastY == 0 ) )
                    {
                        pTransform2D.Move ( fLastX , fLastY );

                        pPolygon = pGeometry as IPolygon;
                        pScreenDisplay.DrawPolygon ( pPolygon );

                        pTransform2D.Move ( -fLastX , -fLastY );
                    }

                    if ( !( dx == 0 && dy == 0 ) )
                    {
                        pTransform2D.Move ( dx , dy );

                        pPolygon = pGeometry as IPolygon;
                        pScreenDisplay.DrawPolygon ( pPolygon );

                        pTransform2D.Move ( -dx , -dy );
                    }
                }

            }

            pScreenDisplay.SetSymbol ( pFeedbackSymbol );

            if ( pLastPoint != null && !pLastPoint.IsEmpty )
            {
                pPolyline = MakePolylineFromTowPoint ( pfPoint , pLastPoint );
                pScreenDisplay.DrawPolyline ( pPolyline );
            }

            pPolyline = MakePolylineFromTowPoint ( pfPoint , ptPoint );
            pScreenDisplay.DrawPolyline ( pPolyline );

            pScreenDisplay.FinishDrawing ();
            return true;
        }

        public bool DrawRotateGeometrys ( ref  IGeometryCollection pGeometrys , ref IPoint pFromPoint , double fLastAngle ,
            double fAngle , ref IPoint ptPoint , ref IPoint pLastPoint )
        {

            if ( pGeometrys == null || pFromPoint == null || pFromPoint.IsEmpty ) return false;

            int nCount = pGeometrys.GeometryCount;
            if ( nCount < 1 ) return false;
            nCount = nCount - 1;

            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );

            IPoint pPoint = null;
            IPolyline pPolyline = null;
            IPolygon pPolygon = null;
            ITransform2D pTransform2D = null;

            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref this.m_pCurMap ) as ISymbol;
            ILineSymbol pLineSymbol = pFeedbackSymbol as ILineSymbol;

            ISymbol pPFeedbackSymbol = MakePolygonFeedbackSymbol ( ref this.m_pCurMap , ref  pLineSymbol ) as ISymbol;

            ISymbol pPTSymbol = MakeCanRasteMarkerSymbol ( ref this.m_pCurMap , 255 , 0 , 0 , false );

            for ( int i = 0 ; i <= nCount ; i++ )
            {
                IGeometry pGeometry = pGeometrys.get_Geometry ( i );

                if ( pGeometry.IsEmpty == false && pGeometry.GeometryType == esriGeometryType.esriGeometryPoint )
                {
                    pScreenDisplay.SetSymbol ( pPTSymbol );

                    pTransform2D = pGeometry as ITransform2D;

                    if ( fLastAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fLastAngle );

                        pPoint = pGeometry as IPoint;
                        pScreenDisplay.DrawPoint ( pPoint );

                        pTransform2D.Rotate ( pFromPoint , -fLastAngle );
                    }

                    if ( fAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fAngle );

                        pPoint = pGeometry as IPoint;
                        pScreenDisplay.DrawPoint ( pPoint );

                        pTransform2D.Rotate ( pFromPoint , -fAngle );
                    }
                }
                else if ( pGeometry.IsEmpty == false && pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline )
                {
                    pScreenDisplay.SetSymbol ( pFeedbackSymbol );
                    pTransform2D = pGeometry as ITransform2D;

                    if ( fLastAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fLastAngle );

                        pPolyline = pGeometry as IPolyline;
                        pScreenDisplay.DrawPolyline ( pPolyline );

                        pTransform2D.Rotate ( pFromPoint , -fLastAngle );
                    }

                    if ( fAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fAngle );

                        pPolyline = pGeometry as IPolyline;
                        pScreenDisplay.DrawPolyline ( pPolyline );

                        pTransform2D.Rotate ( pFromPoint , -fAngle );
                    }
                }
                else if ( pGeometry.IsEmpty == false && pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon )
                {
                    pScreenDisplay.SetSymbol ( pPFeedbackSymbol );
                    pTransform2D = pGeometry as ITransform2D;

                    if ( fLastAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fLastAngle );

                        pPolygon = pGeometry as IPolygon;
                        pScreenDisplay.DrawPolygon ( pPolygon );

                        pTransform2D.Rotate ( pFromPoint , -fLastAngle );
                    }

                    if ( fAngle != 0 )
                    {
                        pTransform2D.Rotate ( pFromPoint , fAngle );

                        pPolygon = pGeometry as IPolygon;
                        pScreenDisplay.DrawPolygon ( pPolygon );

                        pTransform2D.Rotate ( pFromPoint , -fAngle );
                    }

                }
            }

            pScreenDisplay.SetSymbol ( pFeedbackSymbol );

            if ( pLastPoint != null && !pLastPoint.IsEmpty )
            {
                pPolyline = MakePolylineFromTowPoint ( pFromPoint , pLastPoint );
                pScreenDisplay.DrawPolyline ( pPolyline );
            }

            pPolyline = MakePolylineFromTowPoint ( pFromPoint , ptPoint );
            pScreenDisplay.DrawPolyline ( pPolyline );

            pScreenDisplay.FinishDrawing ();

            return true;
        }


        //
        public bool DrawMirror ( ref IMap pMap , ref IPoint pFromPoint , ref IPoint pPoint , ref IPoint pLastPoint )
        {
            if ( pMap == null || pFromPoint == null || pPoint == null ) return false;

            IEnumFeature pEnumFeature = pMap.FeatureSelection as IEnumFeature;
            if ( pEnumFeature == null ) return false;
            pEnumFeature.Reset ();

            IFeature pFeature = pEnumFeature.Next ();
            if ( pFeature == null ) return false;

            IActiveView pActiveView = pMap as IActiveView;


            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );

            IPoint pTempPoint = null;
            IPolyline pPolyline = null;
            IPolygon pPolygon = null;

            ISymbol pFeedbackSymbol = MakeFeedbackSymbol ( ref pMap ) as ISymbol;
            ILineSymbol pLineSymbol = pFeedbackSymbol as ILineSymbol;

            ISymbol pPFeedbackSymbol = MakePolygonFeedbackSymbol ( ref pMap , ref  pLineSymbol ) as ISymbol;

            ISymbol pPTSymbol = MakeCanRasteMarkerSymbol ( ref pMap , 255 , 0 , 0 , false );

            if ( !pLastPoint.IsEmpty )
                DrawFeedback ( ref pScreenDisplay , pFromPoint , pLastPoint , ref pMap );
            DrawFeedback ( ref pScreenDisplay , pFromPoint , pPoint , ref pMap );

            while ( pFeature != null )
            {
                IGeometry pGeometry = pFeature.ShapeCopy;
                if ( !( pGeometry == null || pGeometry.IsEmpty ) )
                {
                    ESRI.ArcGIS.esriSystem.IClone pClone = pGeometry as ESRI.ArcGIS.esriSystem.IClone;
                    IGeometry pTempGeometry = pClone.Clone () as IGeometry;

                    if ( !pLastPoint.IsEmpty )
                    {
                        IGeometry pLastMirrorGeo = GetMirrorGeometry ( pGeometry , pFromPoint , pLastPoint );
                        if ( pLastMirrorGeo != null )
                        {
                            if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon )
                            {
                                pScreenDisplay.SetSymbol ( pPFeedbackSymbol );

                                pPolygon = pLastMirrorGeo as IPolygon;
                                pScreenDisplay.DrawPolygon ( pPolygon );
                            }
                            else if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline )
                            {
                                pScreenDisplay.SetSymbol ( pFeedbackSymbol );

                                pPolyline = pLastMirrorGeo as IPolyline;
                                pScreenDisplay.DrawPolyline ( pPolyline );
                            }
                            else if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPoint )
                            {
                                pScreenDisplay.SetSymbol ( pPTSymbol );

                                pTempPoint = pLastMirrorGeo as IPoint;
                                pScreenDisplay.DrawPoint ( pTempPoint );
                            }
                        }
                    }

                    IGeometry pMirrorGeo = GetMirrorGeometry ( pTempGeometry , pFromPoint , pPoint );
                    if ( pMirrorGeo != null )
                    {
                        if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPolygon )
                        {
                            pScreenDisplay.SetSymbol ( pPFeedbackSymbol );

                            pPolygon = pMirrorGeo as IPolygon;
                            pScreenDisplay.DrawPolygon ( pPolygon );
                        }
                        else if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline )
                        {
                            pScreenDisplay.SetSymbol ( pFeedbackSymbol );

                            pPolyline = pMirrorGeo as IPolyline;
                            pScreenDisplay.DrawPolyline ( pPolyline );
                        }
                        else if ( pGeometry.GeometryType == esriGeometryType.esriGeometryPoint )
                        {
                            pScreenDisplay.SetSymbol ( pPTSymbol );

                            pTempPoint = pMirrorGeo as IPoint;
                            pScreenDisplay.DrawPoint ( pTempPoint );
                        }
                    }
                }
                pFeature = pEnumFeature.Next ();
            }
            pScreenDisplay.FinishDrawing ();

            return true;
        }

        public IGeometry GetMirrorGeometry ( IGeometry pGeometry , IPoint pFromPoint , IPoint pToPoint )
        {
            if ( pGeometry == null || pFromPoint == null || pToPoint == null || pFromPoint.IsEmpty || pToPoint.IsEmpty )
                return null;

            if ( Math.Abs ( pFromPoint.X - pToPoint.X ) < 0.01 && Math.Abs ( pFromPoint.Y - pToPoint.Y ) < 0.01 ) return null;

            ILine pLine = new LineClass ();
            pLine.FromPoint = pFromPoint;
            pLine.ToPoint = pToPoint;

            IAffineTransformation2D pATransform2D = new AffineTransformation2DClass ();
            pATransform2D.DefineReflection ( pLine );

            ITransform2D pTransform2D = pGeometry as ITransform2D;
            pTransform2D.Transform ( esriTransformDirection.esriTransformForward , pATransform2D );

            pLine = null;

            return pTransform2D as IGeometry;
        }

        /*
        public void DrawParrelline(double fLen, IPoint pMousePoint)
        {
            Boolean blDrawTrack = true;

            int nPointCount = 0;
            if (this.m_pDrawedPoints == null)
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if (nPointCount < 0) return;
            if (pMousePoint == null && nPointCount < 1) return;

            ESRI.ArcGIS.Carto.IActiveView pActiveView = this.m_pCurMap as ESRI.ArcGIS.Carto.IActiveView;

            ESRI.ArcGIS.Display.IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);// esriNoScreenCache

            IPolyline pPolyline = null;

            IPolyline pTempPolyline = null;

            if (pMousePoint == null)
            {
                //鼠标点击(至少有两个点)
                pPolyline = GetParallelPolyline2(this.m_pDrawedPoints, null, fLen, this.m_pCurMap);

                pTempPolyline = this.m_pDrawedPoints as IPolyline;
                DrawPoints(ref pScreenDisplay, ref  pTempPolyline, ref this.m_pCurMap);
                DrawPoints(ref pScreenDisplay, ref  pPolyline, ref this.m_pCurMap);
            }
            else
            {
                //鼠标移动（至少有一个点）
                IPoint pPoint = this.m_pDrawedPoints.get_Point(nPointCount);
                DrawFeedback(ref pScreenDisplay, pPoint, pMousePoint, ref this.m_pCurMap);

                if (nPointCount > 0)
                {
                    pTempPolyline = this.m_pDrawedPoints as IPolyline;
                    DrawPoints(ref pScreenDisplay, ref pTempPolyline, ref this.m_pCurMap);

                }

                pPolyline = GetParallelPolyline2(this.m_pDrawedPoints, pMousePoint, fLen, this.m_pCurMap);
                if (!pPolyline.IsEmpty)
                {
                    IPointCollection pPointCollection = pPolyline as IPointCollection;

                    int nCount = pPointCollection.PointCount;

                    IPoint pPoint1 = pPointCollection.get_Point(nCount - 2);

                    IPoint pPoint2 = pPointCollection.get_Point(nCount - 1);

                    DrawFeedback(ref pScreenDisplay, pPoint1, pPoint2, ref this.m_pCurMap);

                    if (nCount > 2)
                    {
                        pPointCollection.RemovePoints(nCount - 1, 1);
                        pPolyline = pPointCollection as IPolyline;
                        DrawPoints(ref pScreenDisplay, ref pPolyline, ref this.m_pCurMap);
                    }
                }
            }
            pScreenDisplay.FinishDrawing();
        }*/

        //给定线的点集以及当前鼠标坐标点，返回给定距离的平行线
        private IPolyline GetParallelPolyline2 ( IPointCollection pPointCol , IPoint pCurMonsePoint , double dDistance , IMap pRelMap )
        {
            if ( pPointCol == null || pRelMap == null || pPointCol.PointCount < 1 || dDistance == 0 ) return null;

            IPointCollection pPoints = null;
            IPolyline pPolyline = null;

            object pBObj = Type.Missing;
            object pAObj = Type.Missing;

            if ( pPointCol.PointCount == 1 )
            {
                if ( pCurMonsePoint != null )
                {
                    IPoint pFromPoint = null;
                    IPoint pToPoint = null;

                    pPoints = new PolylineClass () as IPointCollection;
                    pFromPoint = GetPdicularPoint ( pPointCol.get_Point ( 0 ) , pCurMonsePoint , dDistance , pRelMap );

                    if ( pFromPoint != null )
                    {
                        pPoints.AddPoint ( pFromPoint , ref pBObj , ref pAObj );
                        pToPoint = GetCrossPoint ( pPointCol.get_Point ( 0 ) , pFromPoint , pCurMonsePoint );
                        if ( pToPoint != null )
                            pPoints.AddPoint ( pToPoint , ref pBObj , ref pAObj );

                    }

                    if ( pPoints.PointCount > 1 )
                    {
                        pPolyline = pPoints as IPolyline;
                        return pPolyline;
                    }
                }
            }

            pPoints = new PolylineClass () as IPointCollection;

            pPoints.AddPointCollection ( pPointCol );

            if ( pCurMonsePoint != null )
                pPoints.AddPoint ( pCurMonsePoint , ref pBObj , ref pAObj );

            pPolyline = pPoints as IPolyline;

            IConstructCurve pCtructCurcve = new PolylineClass () as IConstructCurve;

            IPolycurve pPolycurve = pPolyline as IPolycurve;

            pCtructCurcve.ConstructOffset ( pPolycurve , dDistance , ref pBObj , ref pAObj );

            return pCtructCurcve as IPolyline;
        }

        //给定一平行四边形三个点，求另一个点(三点必须在同一个坐标系下)
        private IPoint GetCrossPoint ( IPoint pOriginPoint , IPoint pFirstPoint , IPoint pSecondPoint )
        {
            if ( pOriginPoint == null || pFirstPoint == null || pSecondPoint == null ) return null;

            IPoint pPoint = new PointClass ();
            pPoint.X = pFirstPoint.X + pSecondPoint.X - pOriginPoint.X;
            pPoint.Y = pFirstPoint.Y + pSecondPoint.Y - pOriginPoint.Y;
            return pPoint;
        }

        //给定一个线上的第一个端点，作此线右垂线上给定距离(地图坐标距离)的另一个坐标点
        private IPoint GetPdicularPoint ( IPoint pFirstPointOnLine , IPoint pSecondPointOnLine , double dDistance , IMap pRelMap )
        {

            double dNewDistance = 0;
            IPoint pFirstPoint = null;
            IPoint pSecondPoint = null;

            if ( pFirstPointOnLine == null || pSecondPointOnLine == null || pRelMap == null ) return null;

            dNewDistance = MapLengthToDeviceLenth ( dDistance , pRelMap );
            if ( dNewDistance == 0 ) return null;

            pFirstPoint = MapPointToDevicePoint ( pFirstPointOnLine , pRelMap );
            pSecondPoint = MapPointToDevicePoint ( pSecondPointOnLine , pRelMap );

            if ( CompareTwoPoints ( pFirstPoint , pSecondPoint ) ) return null;

            IPoint pPoint = new PointClass ();

            if ( pFirstPoint.X == pSecondPoint.X )
            {
                if ( pFirstPoint.Y < pSecondPoint.Y )
                {
                    pPoint.X = pFirstPoint.X - dNewDistance;
                    pPoint.Y = pFirstPoint.Y;
                }
                else
                {
                    pPoint.X = pFirstPoint.X + dNewDistance;
                    pPoint.Y = pFirstPoint.Y;
                }
                return DevicePointToMapPoint ( pPoint , pRelMap );
            }

            if ( pFirstPoint.Y == pSecondPoint.Y )
            {
                if ( pFirstPoint.X < pSecondPoint.X )
                {
                    pPoint.X = pFirstPoint.X;
                    pPoint.Y = pFirstPoint.Y + dNewDistance;
                }
                else
                {
                    pPoint.X = pFirstPoint.X;
                    pPoint.Y = pFirstPoint.Y - dNewDistance;
                }
                return DevicePointToMapPoint ( pPoint , pRelMap );
            }

            double dKey = ( pSecondPoint.Y - pFirstPoint.Y ) / ( pSecondPoint.X - pFirstPoint.X );
            double dAngle = Math.Atan ( dKey ) + 1.571;  //1.571代表90度的弧度大小
            pPoint.X = dNewDistance * Math.Cos ( dAngle ) + pFirstPoint.X;
            pPoint.Y = dNewDistance * Math.Sin ( dAngle ) + pFirstPoint.Y;

            return DevicePointToMapPoint ( pPoint , pRelMap );
        }

        //比较两个点的是否为同一个点
        //todo:该算法有问题
        private bool CompareTwoPoints ( IPoint pPointOne , IPoint pPointTwo )
        {
            if ( pPointOne == null || pPointTwo == null ) return false;

            if ( pPointOne.X == pPointTwo.X && pPointOne.Y == pPointTwo.Y )
                return true;
            else
                return false;

        }

        //地图距离转化为设备距离
        private double MapLengthToDeviceLenth ( double dMapLenth , IMap pRelMap )
        {
            if ( pRelMap == null ) return 0;

            IActiveView pActView = pRelMap as IActiveView;

            return pActView.ScreenDisplay.DisplayTransformation.ToPoints ( dMapLenth );
        }

        //设备坐标点转化为地图坐标点
        private IPoint DevicePointToMapPoint ( IPoint pDevicePoint , IMap pRelMap )
        {
            if ( pDevicePoint == null || pRelMap == null ) return null;

            IActiveView pActView = pRelMap as IActiveView;
            IPoint pPoint = pActView.ScreenDisplay.DisplayTransformation.ToMapPoint ( ( int )pDevicePoint.X , ( int )pDevicePoint.Y );

            return pPoint;
        }

        //地图坐标点转化为设备坐标点
        private IPoint MapPointToDevicePoint ( IPoint pMapPoint , IMap pRelMap )
        {
            if ( pMapPoint == null || pRelMap == null ) return null;

            IActiveView pActView = pRelMap as IActiveView;
            IPoint pPoint = new PointClass ();

            int dx = 0;
            int dy = 0;
            pActView.ScreenDisplay.DisplayTransformation.FromMapPoint ( pMapPoint , out  dx , out  dy );

            pPoint.X = dx;
            pPoint.Y = dy;

            return pPoint;
        }

        public void DrawParalMove ( double fLen , ref IPoint pMousePoint , ref IPoint pLastMousePoint , ref IPolyline pLastPolyline )
        {
            int nPointCount = 0;
            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if ( nPointCount < 0 ) return; //鼠标移动（至少有一个点）
            if ( pMousePoint == null || pMousePoint.IsEmpty ) return;

            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );// esriNoScreenCache

            IPolyline pPolyline = null;

            IPolyline pTempPolyline = null;

            if ( !( pLastPolyline == null || pLastPolyline.IsEmpty ) )
            {
                pTempPolyline = GetPartPolyLine ( true , pLastPolyline , nPointCount );
                if ( pTempPolyline != null )
                    DrawFeedbackLine ( ref pScreenDisplay , pTempPolyline , ref this.m_pCurMap );//擦除上次的平行线
            }

            IPoint pPoint = this.m_pDrawedPoints.get_Point ( nPointCount );

            if ( pLastMousePoint.IsEmpty == false )
                DrawFeedback ( ref pScreenDisplay , pPoint , pLastMousePoint , ref this.m_pCurMap ); //擦除上次的反馈线

            DrawFeedback ( ref pScreenDisplay , pPoint , pMousePoint , ref this.m_pCurMap );         //绘本次反馈线

            pPolyline = GetParallelPolyline2 ( this.m_pDrawedPoints , pMousePoint , fLen , this.m_pCurMap );
            if ( !pPolyline.IsEmpty )
            {
                pTempPolyline = GetPartPolyLine ( true , pPolyline , nPointCount );
                if ( pTempPolyline != null )
                    DrawFeedbackLine ( ref pScreenDisplay , pTempPolyline , ref this.m_pCurMap );
                pLastPolyline = pPolyline;       //绘本次的平行线

            }
            pScreenDisplay.FinishDrawing ();
        }

        public void DrawParalDown ( double fLen , ref IPolyline pLastPolyline )
        {
            int nPointCount = 0;
            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if ( nPointCount < 1 ) return; //鼠标移动（至少有二个点）

            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );// esriNoScreenCache

            IPolyline pPolyline = null;

            IPolyline pTempPolyline = null;

            if ( !( pLastPolyline == null || pLastPolyline.IsEmpty ) )
            {
                pTempPolyline = GetPartPolyLine ( true , pLastPolyline , nPointCount );
                if ( pTempPolyline != null )
                    DrawFeedbackLine ( ref pScreenDisplay , pTempPolyline , ref this.m_pCurMap );
            }

            pPolyline = GetParallelPolyline2 ( this.m_pDrawedPoints , null , fLen , this.m_pCurMap );
            if ( !pPolyline.IsEmpty )
            {
                pTempPolyline = GetPartPolyLine ( true , pPolyline , nPointCount );
                if ( pTempPolyline != null )
                    DrawFeedbackLine ( ref pScreenDisplay , pTempPolyline , ref this.m_pCurMap );
                pLastPolyline = pPolyline;

                pTempPolyline = GetPartPolyLine ( false , pLastPolyline , nPointCount );
                if ( pTempPolyline != null && nPointCount > 1 )
                {
                    pTempPolyline = GetLastSegment ( pTempPolyline );
                    DrawPoints ( ref pScreenDisplay , ref pTempPolyline , ref this.m_pCurMap );
                }
            }

            if ( nPointCount > 0 )
            {
                pTempPolyline = MakePolylineFromTowPoint ( this.m_pDrawedPoints.get_Point ( nPointCount - 1 ) , this.m_pDrawedPoints.get_Point ( nPointCount ) );
                DrawPoints ( ref pScreenDisplay , ref pTempPolyline , ref this.m_pCurMap );          //绘已经确定的线
            }

            pScreenDisplay.FinishDrawing ();
        }


        private IPolyline GetPartPolyLine ( bool blEnd , IPolyline pPolyline , int nPointCount )
        {
            if ( pPolyline == null || pPolyline.IsEmpty ) return null;
            ISegmentCollection pSegmentCollection = pPolyline as ISegmentCollection;

            int nCount = pSegmentCollection.SegmentCount;
            if ( nCount < 1 ) return null;

            IPolyline pNewPolyline = new PolylineClass ();
            ISegmentCollection pSegments = pNewPolyline as ISegmentCollection;


            object pBobj = Type.Missing;
            object pAobj = Type.Missing;

            ISegment pSegment = null;

            if ( blEnd )
            {
                if ( nCount - 2 > -1 )
                {
                    pSegment = pSegmentCollection.get_Segment ( nCount - 2 );
                    pSegments.AddSegment ( pSegment , ref pBobj , ref pAobj );
                }

                pSegment = pSegmentCollection.get_Segment ( nCount - 1 );
                pSegments.AddSegment ( pSegment , ref pBobj , ref pAobj );
            }
            else
            {
                if ( nCount < 2 ) return null;

                int i = 0;
                for ( i = 0 ; i < nPointCount - 1 ; i++ )
                {
                    pSegment = pSegmentCollection.get_Segment ( i );
                    pSegments.AddSegment ( pSegment , ref pBobj , ref pAobj );
                }
            }
            return pNewPolyline;
        }


        private IPolyline GetLastSegment ( IPolyline pPolyline )
        {
            if ( pPolyline == null || pPolyline.IsEmpty ) return null;
            ISegmentCollection pSegmentCollection = pPolyline as ISegmentCollection;

            int nCount = pSegmentCollection.SegmentCount;
            if ( nCount < 1 ) return null;

            IPolyline pNewPolyline = new PolylineClass ();
            ISegmentCollection pSegments = pNewPolyline as ISegmentCollection;


            object pBobj = Type.Missing;
            object pAobj = Type.Missing;

            ISegment pSegment = null;
            pSegment = pSegmentCollection.get_Segment ( nCount - 1 );
            pSegments.AddSegment ( pSegment , ref pBobj , ref pAobj );
            return pNewPolyline;
        }


        public void ReDrawParal ( double fLen )
        {
            int nPointCount = 0;
            if ( this.m_pDrawedPoints == null )
                nPointCount = -1;
            else
                nPointCount = this.m_pDrawedPoints.PointCount - 1;

            if ( nPointCount < 1 ) return; //鼠标移动（至少有二个点）


            IActiveView pActiveView = this.m_pCurMap as IActiveView;

            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing ( pScreenDisplay.hDC , -1 );// esriNoScreenCache

            IPolyline pPolyline = this.m_pDrawedPoints as IPolyline;
            DrawPoints ( ref pScreenDisplay , ref pPolyline , ref this.m_pCurMap );

            IPolyline pTempPolyline = null;

            pPolyline = GetParallelPolyline2 ( this.m_pDrawedPoints , null , fLen , this.m_pCurMap );
            if ( !pPolyline.IsEmpty )
            {
                pTempPolyline = GetPartPolyLine ( false , pPolyline , nPointCount );
                if ( pTempPolyline != null && nPointCount > 1 )
                    DrawPoints ( ref pScreenDisplay , ref pTempPolyline , ref this.m_pCurMap );
            }

            pScreenDisplay.FinishDrawing ();
        }

    }
}
