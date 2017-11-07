using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace GeoDataEdit
{
    public class DrawVertex
    {
        private IPointCollection m_pPointCol;
        private ISimpleMarkerSymbol m_MarkerSym;
        private ISimpleMarkerSymbol m_VertexSym;
        private ISimpleMarkerSymbol m_EndPointSym;
        private ISimpleLineSymbol m_LineSym;
        private ISimpleLineSymbol m_TracklineSym;
        private ISimpleLineSymbol m_Osym;
        private ISimpleFillSymbol m_FillSym;

        private IMap m_pMap;
        private IActiveView m_pAV;

        private ClsEditorMain clsEditorMain;

        public DrawVertex ( IMap pMap,ClsEditorMain _clsEditorMain )
        {
            clsEditorMain = _clsEditorMain;
            m_pMap = pMap;
            m_pAV = m_pMap as IActiveView;
        }

        

        public void SymbolInit ()
        {
            IRgbColor pFcolor = new RgbColorClass ();
            IRgbColor pOcolor = new RgbColorClass ();
            IRgbColor pTrackcolor = new RgbColorClass ();
            IRgbColor pVcolor = new RgbColorClass ();

            pFcolor.Red = 255;
            pFcolor.Green = 0;
            pFcolor.Blue = 0;

            pOcolor.Red = 0;
            pOcolor.Green = 0;
            pOcolor.Blue = 255;

            pTrackcolor.Red = 0;
            pTrackcolor.Green = 255;
            pTrackcolor.Blue = 255;

            pVcolor.Red = 0;
            pVcolor.Green = 255;
            pVcolor.Blue = 0;

            m_MarkerSym = new SimpleMarkerSymbolClass ();
            m_MarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
            m_MarkerSym.Color = pFcolor;
            m_MarkerSym.Outline = true;
            m_MarkerSym.OutlineColor = pOcolor;
            m_MarkerSym.OutlineSize = ScaleChange(1,m_pMap);

            m_VertexSym = new SimpleMarkerSymbolClass();
            m_VertexSym.Style = esriSimpleMarkerStyle.esriSMSSquare;
            m_VertexSym.Color = pVcolor;
            m_VertexSym.Size = ScaleChange(4,m_pMap);

            m_EndPointSym = new SimpleMarkerSymbolClass();
            m_EndPointSym.Style = esriSimpleMarkerStyle.esriSMSSquare;
            m_EndPointSym.Color = pFcolor;
            m_EndPointSym.Size = ScaleChange(4,m_pMap);

            m_LineSym = new SimpleLineSymbolClass();
            m_LineSym.Style = esriSimpleLineStyle.esriSLSSolid;
            m_LineSym.Color = pFcolor;
            m_LineSym.Width = ScaleChange(1,m_pMap);

            m_TracklineSym = new SimpleLineSymbolClass();
            m_TracklineSym.Color = pTrackcolor;
            m_TracklineSym.Width = ScaleChange(1,m_pMap);

            m_Osym = new SimpleLineSymbolClass();
            m_Osym.Color = pOcolor;
            m_Osym.Width = ScaleChange(1,m_pMap);

            m_FillSym = new SimpleFillSymbolClass();
            m_FillSym.Color = pFcolor;
            m_FillSym.Style = esriSimpleFillStyle.esriSFSVertical;
            m_FillSym.Outline = m_Osym;
            

        }


        public double ScaleChange(double vVal,IMap pMap)
        {
            if(pMap.MapScale == 0||pMap.ReferenceScale == 0)
                return vVal;
            else
                return vVal*pMap.MapScale/pMap.ReferenceScale;
        }


        private void DisplayGraphic ( IGeometry pGeometry , IColor pColor , ISymbol pSymbol )
        {
            ISimpleMarkerSymbol pMarkSym = new SimpleMarkerSymbolClass ();
            ISimpleLineSymbol pLineSym = new SimpleLineSymbolClass ();
            ISimpleFillSymbol pFillSym = new SimpleFillSymbolClass ();
            if ( pColor != null )
            {
                pMarkSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pMarkSym.Color = pColor;

                pLineSym.Color = pColor;

                pFillSym.Color = pColor;
                pFillSym.Style = esriSimpleFillStyle.esriSFSSolid;
            }

            m_pAV.ScreenDisplay.StartDrawing ( m_pAV.ScreenDisplay.hDC , -1 );
            switch ( pGeometry.GeometryType )
            {
                case esriGeometryType.esriGeometryPoint:
                    if ( pColor != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pMarkSym as ISymbol );
                    else if ( pSymbol != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pSymbol );
                    else
                        m_pAV.ScreenDisplay.SetSymbol (m_MarkerSym as ISymbol );
                    m_pAV.ScreenDisplay.DrawPoint ( pGeometry );
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    if ( pColor != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pLineSym as ISymbol );
                    else if ( pSymbol != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pSymbol );
                    else
                        m_pAV.ScreenDisplay.SetSymbol (m_LineSym as ISymbol);

                    m_pAV.ScreenDisplay.DrawPolyline ( pGeometry );
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    if ( pColor != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pFillSym as ISymbol );
                    else if ( pSymbol != null )
                        m_pAV.ScreenDisplay.SetSymbol ( pSymbol );
                    else
                        m_pAV.ScreenDisplay.SetSymbol ( m_FillSym as ISymbol);
                    m_pAV.ScreenDisplay.DrawPolygon ( pGeometry );
                    break;
            }
            m_pAV.ScreenDisplay.FinishDrawing ();
        }



        public void DrawAllVertex ()
        {
            if ( m_pMap.SelectionCount == 1 )
            {
                IPolyline pPolyline;
                IPolygon pPolygon;
                IFeature pFeature;
                IEnumFeature pEnumFeature = m_pMap.FeatureSelection as IEnumFeature;
                if ( pEnumFeature == null ) return;
                pEnumFeature.Reset ();
                pFeature = pEnumFeature.Next ();
                switch ( pFeature.Shape.GeometryType )
                {
                    case esriGeometryType.esriGeometryPolyline:
                        m_pPointCol = new PolylineClass ();
                        pPolyline = pFeature.Shape as IPolyline;
                        m_pPointCol.AddPointCollection ( pPolyline as IPointCollection );
                        ShowAllVertex ( m_pPointCol );
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        m_pPointCol = new PolygonClass ();
                        pPolygon = pFeature.Shape as IPolygon;
                        
                        m_pPointCol.AddPointCollection ( pPolygon as IPointCollection );
                        ShowAllVertex ( m_pPointCol );
                        break;
                    default: break;

                }

            }
        }

        public void StoreFeature ( IFeature pFeature )
        {
            IWorkspaceEdit pWorkspaceEdit = clsEditorMain.EditWorkspace as IWorkspaceEdit;
            if ( clsEditorMain.EditFeatureLayer == null ) return;
            pWorkspaceEdit.StartEditOperation ();
            pFeature.Store ();
            pWorkspaceEdit.StopEditOperation ();
            m_pAV.PartialRefresh ( esriViewDrawPhase.esriViewGeography , pFeature , null );
            //RefreshModifyFeature ( pFeature );
        }


        public void ShowAllVertex ( IPointCollection pPointColl )
        {
            SymbolInit ();
            IPoint pPoint = new PointClass ();
            for ( int i = 0 ; i < pPointColl.PointCount ; i++ )
            {
                pPoint = pPointColl.get_Point ( i );
                if ( i == 0 || i == pPointColl.PointCount-1)
                    DisplayGraphic ( pPoint , null , m_EndPointSym as ISymbol );
                else
                    DisplayGraphic ( pPoint , null , m_VertexSym as ISymbol );
            }

        }
    }
}
