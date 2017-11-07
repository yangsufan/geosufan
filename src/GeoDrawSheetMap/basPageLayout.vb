Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Controls
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.GeoDatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports System.Math
Public Module basPageLayout
    Public m_bScalePageLayout As Boolean               '用与记录是否出于比例尺制图过程中
    '用于有平面坐标到图纸坐标转换的参数
    Public g_pMapEnvelope As IEnvelope      '制图框中的平面坐标框
    Public g_pPaperEnvelope As IEnvelope    '制图框中的图纸坐标框
    Public g_pMapFrame As IMapFrame         '制图框

    Public g_pRecElement As IElement         '参考矩形框
    Public g_MapFrameEle As IElement         '数据框
    Public g_ScaleType As String
    Public g_lngScale As Long
    Public g80To54 As Boolean
    Public g_OffX As Double
    Public g_OffY As Double
    Public g_Rubber As Boolean  '画笔
    Public g_SubjectTool As Boolean
    Public g_strSheetNo As String
    Public g_pProjectSpatial As ISpatialReference
    Public g_Geometry As IGeometry
    Public g_bAddLayer As Boolean
    Public g_Rotation As Double   '地图当前的旋转角度　
    Public g_CurrentLayer As ILayer  '当前图层
    Public g_BufferGeometry As IGeometry

    'Public g_pFrmBufferSet As frmBufferSet_Dev  'liujiang 20080311
    Public g_CustomPagepath As String '普通模板制图模板文件
    'Public g_bInStandardPageLayout As Boolean '标准制图状态


    '临时
    Public g_PagelayoutControl As IPageLayoutControl

    ''' <summary>
    ''' 多幅打印参数信息
    ''' </summary>
    ''' <remarks>kxj,2008.04.15</remarks>
    Public Structure MapInfo
        Public Keys As String
        Public Infos As Object
        Public pGeometry As IPolygon

    End Structure
    Public Function CreateElement(ByVal pTextElem As ITextElement, ByVal pPoint As IPoint, ByVal pTextSymbol As ITextSymbol) As ITextElement
        CreateElement = Nothing
        Try
            CreateElement = Nothing
            Dim pGraphicsCon As IGraphicsContainer
            Dim pElem As IElement
            pTextElem.Symbol = pTextSymbol
            pGraphicsCon = g_PagelayoutControl.ActiveView
            pElem = pTextElem
            pElem.Geometry = pPoint
            pGraphicsCon.AddElement(pTextElem, 0)
            CreateElement = pElem
        Catch ex As Exception

        End Try


    End Function

    '图层的排序
    Public Function SortIndex(ByVal pSortLayer As ILayer, ByVal pMap As IBasicMap) As Long
        Dim pLayer As ILayer
        Dim LayerType As String, tmpLayerType As String
        Dim i As Long, Index As Long
        Dim bExist As Boolean       '这个变量标明欲排序的图层是否已经存在于pMap中,且一般来说，位于第0层
        Dim OldIndex As Long       '这个变量标明欲排序的图层当前在pMap中的位置
        pLayer = pSortLayer
        LayerType = GetLayerType(pLayer)
        Index = pMap.LayerCount
        '//注意，当注记层为一个多的
        Dim pCompLayer As ICompositeLayer
        Select Case LayerType
            Case "Point"
                '点层
                For i = 0 To pMap.LayerCount - 1
                    pLayer = pMap.Layer(i)
                    If TypeOf pLayer Is ICompositeLayer And (Not TypeOf pLayer Is IAnnotationLayer) Then
                        pCompLayer = pLayer
                        If pCompLayer.count > 0 Then
                            pLayer = pCompLayer.Layer(0)
                        End If
                    End If
                    If pLayer.Name <> pSortLayer.Name Then
                        tmpLayerType = GetLayerType(pLayer)
                        If tmpLayerType <> "Anno" Then
                            Index = i
                            Exit For
                        End If
                    Else
                        OldIndex = i
                        bExist = True
                    End If
                    Index = i + 1
                Next i

            Case "Anno"
                '直接放入第一层，不操作
                Index = 0
            Case "Polyline"
                '线层
                For i = 0 To pMap.LayerCount - 1

                    pLayer = pMap.Layer(i)
                    If TypeOf pLayer Is ICompositeLayer And (Not TypeOf pLayer Is IAnnotationLayer) Then
                        pCompLayer = pLayer
                        If pCompLayer.count > 0 Then
                            pLayer = pCompLayer.Layer(0)
                        End If
                    End If
                    If pLayer.Name <> pSortLayer.Name Then
                        tmpLayerType = GetLayerType(pLayer)
                        If tmpLayerType <> "Anno" And tmpLayerType <> "Point" Then
                            Index = i
                            Exit For
                        End If
                    Else
                        OldIndex = i
                        bExist = True
                    End If
                    Index = i + 1
                Next i

            Case "Polygon"
                '面层

                For i = 0 To pMap.LayerCount - 1
                    pLayer = pMap.Layer(i)
                    If TypeOf pLayer Is ICompositeLayer And (Not TypeOf pLayer Is IAnnotationLayer) Then
                        pCompLayer = pLayer
                        If pCompLayer.count > 0 Then
                            pLayer = pCompLayer.Layer(0)
                        End If
                    End If
                    If pLayer.Name <> pSortLayer.Name Then
                        tmpLayerType = GetLayerType(pLayer)
                        If tmpLayerType <> "Anno" And tmpLayerType <> "Point" And tmpLayerType <> "Polyline" Then
                            Index = i
                            Exit For
                        End If
                    Else
                        OldIndex = i
                        bExist = True
                    End If
                    Index = i + 1
                Next i

            Case "Raster"
                '影像
                For i = 0 To pMap.LayerCount - 1
                    pLayer = pMap.Layer(i)
                    If pLayer.Name <> pSortLayer.Name Then
                        tmpLayerType = GetLayerType(pLayer)
                        If tmpLayerType <> "Anno" And tmpLayerType <> "Point" _
                            And tmpLayerType <> "Polyline" And tmpLayerType <> "Polygon" Then
                            Index = i
                            Exit For
                        End If
                    Else
                        OldIndex = i
                        bExist = True
                    End If
                    Index = i + 1
                Next i

            Case Else
        End Select

        If bExist = True Then
            If OldIndex < Index Then              '老位置在新位置的前面，那么就应该去掉老位置所占的那个空
                Index = Index - 1
            End If
        End If
        If Index < 0 Then Index = 0
        SortIndex = Index
    End Function

    Private Function GetLayerType(ByVal pLayer As ILayer) As String
        GetLayerType = ""
        Dim pFeatureClass As IFeatureClass
        Dim pFeatureLayer As IFeatureLayer
        Dim pCompLayer As ICompositeLayer


        If TypeOf pLayer Is IAnnotationLayer Or TypeOf pLayer Is IDimensionLayer Or TypeOf pLayer Is ITopologyLayer Then
            GetLayerType = "Anno"
            Exit Function
        End If
        If TypeOf pLayer Is ICompositeLayer Then
            pCompLayer = pLayer
            If pCompLayer.Count > 0 Then
                pLayer = pCompLayer.Layer(0)
            End If
        End If
        If TypeOf pLayer Is IFeatureLayer Then
            pFeatureLayer = pLayer
            pFeatureClass = pFeatureLayer.FeatureClass

            If TypeOf pFeatureLayer Is IGdbRasterCatalogLayer Then
                GetLayerType = "Raster"
            Else
                If pFeatureClass.ShapeType = esriGeometryType.esriGeometryPoint Then

                    GetLayerType = "Point"
                ElseIf pFeatureClass.ShapeType = esriGeometryType.esriGeometryPolyline Then
                    GetLayerType = "Polyline"
                ElseIf pFeatureClass.ShapeType = esriGeometryType.esriGeometryPolygon Then
                    GetLayerType = "Polygon"
                End If
            End If
        End If
    End Function

    Public Function AddLine(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal width As Double, ByVal pSym As ISymbol) As IElement
        AddLine = Nothing
        Try
            AddLine = Nothing
            Dim pPoint As IPoint
            Dim pPnt As IPoint
            pPoint = New Point
            pPnt = New Point
            pPoint.PutCoords(x1, y1)
            pPnt.PutCoords(x2, y2)
            Dim pPntCol As IPointCollection
            Dim pLine As IPolyline
            pLine = New Polyline
            pPntCol = pLine
            pPntCol.AddPoint(pPoint)
            pPntCol.AddPoint(pPnt)

            Dim pGraCont As IGraphicsContainer
            pGraCont = g_PagelayoutControl.ActiveView

            Dim pElem As IElement
            '    Dim pRGB As IRgbColor
            '    Set pRGB = New RgbColor
            '    pRGB.RGB = RGB(0, 0, 0)
            'Takes an ILine and IActiveView and creates a Line element in the ActiveView's BasicGraphicsLayer
            Dim pElemLine As ILineElement
            'QI for the ILineElement interface so that the Symbol property can be set
            pElem = New LineElement
            pElemLine = pElem
            'Use the IElement interface to set the LineElement's Geometry
            pElem.Geometry = pLine

            '    Dim pLnSym As ISimpleLineSymbol
            '    Set pLnSym = pSym
            '    pLnSym.Style = esriSLSSolid
            '    pLnSym.Color = pRGB
            '    pLnSym.width = width * (72 / 25.4)
            '    pElemLine.symbol = pLnSym
            pElemLine.Symbol = pSym

            AddLine = pElemLine
            'Add the element at Z order zero
            pGraCont.AddElement(pElemLine, 0)
            '    pElem.Locked = True
            g_PagelayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, pElemLine)
        Catch ex As Exception

        End Try


    End Function

    Public Function AddText(ByVal x As Double, ByVal y As Double, ByVal strText As String) As IElement
        AddText = Nothing
        'Try
        '    Dim pTextElem As ITextElement
        '    pTextElem = New TextElement

        '    pTextElem.Text = Trim(strText)
        '    Dim pPoint As IPoint
        '    pPoint = New Point
        '    pPoint.PutCoords(x, y)

        '    '设置符号
        '    Dim pTextSym As ITextSymbol
        '    pTextSym = New TextSymbol
        '    pTextSym.Font = MakeStdFont("黑体")
        '    pTextSym.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
        '    '    pTextSym.VerticalAlignment = esriTVABottom
        '    pTextSym.Size = 10

        '    AddText = CreateElement(pTextElem, pPoint, pTextSym)

        '    pTextElem = Nothing
        'Catch ex As Exception

        'End Try


    End Function

    Public Function AddMarker(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal pSym As ISymbol) As IElement
        AddMarker = Nothing
        Try
            Dim pElement As IElement
            pElement = New MarkerElement
            Dim pMarkerElement As IMarkerElement
            pMarkerElement = pElement
            pMarkerElement.Symbol = pSym
            Dim pEnv As IEnvelope
            pEnv = New Envelope
            pEnv.XMin = x1
            pEnv.YMin = y1
            pEnv.XMax = x2
            pEnv.YMax = y2
            Dim pPoint As IPoint
            pPoint = New Point
            pPoint.PutCoords((x1 + x2) / 2, (y1 + y2) / 2)
            Dim pArea As IArea
            pArea = pEnv
            pElement.Geometry = pPoint 'pArea.Centroid

            AddMarker = pElement

            g_PagelayoutControl.AddElement(pElement)
            g_PagelayoutControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            pElement = Nothing
            pMarkerElement = Nothing
        Catch ex As Exception

        End Try


    End Function

    Public Function AddFill(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal pSym As ISymbol) As IElement
        AddFill = Nothing
        Try
            Dim pEnv As IEnvelope
            pEnv = New Envelope
            pEnv.XMin = x1
            pEnv.YMin = y1
            pEnv.XMax = x2
            pEnv.YMax = y2

            ' Takes an IEnvelope and IActiveView and creates an Envelope element in the ActiveView's BasicGraphicsLayer
            Dim pElemFillShp As IFillShapeElement
            Dim pElem As IElement
            Dim pGraCont As IGraphicsContainer
            '  Dim pSFSym As ISimpleFillSymbol
            '  Dim pRGB As IRgbColor

            ' Use the IElement interface to set the EnvelopeElement's Geometry
            pElem = New RectangleElement
            pElem.Geometry = pEnv

            ' QI for the IFillShapeElement interface so that the Symbol property can be set
            pElemFillShp = pElem

            ' Create a new RGBColor
            '  Set pRGB = New RgbColor
            '  With pRGB
            '    .Red = 255
            '    .Green = 100
            '    .Blue = 0
            '  End With

            ' Create a new SimpleFillSymbol and set its Color and Style
            '  Set pSFSym = New SimpleFillSymbol
            '  pSFSym.Color = pRGB
            '  pSFSym.Style = esriSFSSolid
            pElemFillShp.Symbol = pSym

            ' QI for the IGraphicsContainer interface from the IActiveView, allows access to the BasicGraphicsLayer
            pGraCont = g_PagelayoutControl.ActiveView

            AddFill = pElemFillShp

            'Add the element at Z order zero
            pGraCont.AddElement(pElemFillShp, 0)
            g_PagelayoutControl.Refresh(esriViewDrawPhase.esriViewGraphics, pElemFillShp)
        Catch ex As Exception

        End Try



    End Function

    Public Function AddAnno(ByVal x As Double, ByVal y As Double, ByVal strText As String, ByVal pSym As ISymbol) As IElement
        AddAnno = Nothing
        Try
            Dim pTextElem As ITextElement
            pTextElem = New TextElement
            pTextElem.Text = Trim(strText)

            Dim pPos As IPoint
            pPos = New Point
            pPos.PutCoords(x - 0.5 / 2, y - 0.5 / 2)
            AddAnno = CreateElement(pTextElem, pPos, pSym)

            pTextElem = Nothing
        Catch ex As Exception

        End Try

    End Function
    Public Function GetMapFrameSize(ByVal pGraphicsContainer As IGraphicsContainer) As IEnvelope
        Dim pElememt As IElement
        pGraphicsContainer.Reset()
        pElememt = pGraphicsContainer.Next
        While Not pElememt Is Nothing
            If TypeOf pElememt Is IMapFrame And Not pElememt Is Nothing Then
                Dim pMapFrame As IMapFrame
                pMapFrame = pElememt
                '得到MapFrame的矩形框
                Dim pGeometry As IGeometry
                pGeometry = pElememt.Geometry
                GetMapFrameSize = pGeometry.Envelope
                Exit Function
            End If
            pElememt = pGraphicsContainer.Next
        End While
        GetMapFrameSize = Nothing
    End Function

    Public Function InitLayerLayout() As Boolean
        If g_PagelayoutControl.ActiveView.FocusMap.LayerCount = 0 Then
            MsgBox("当前地图中没有加载图层，请加载图层后继续制图操作.", vbOKOnly + vbInformation, "提示")
            InitLayerLayout = True
        Else
            InitLayerLayout = False
        End If
    End Function

    Public Sub StartLayout()
        'm_bScalePageLayout = True
        'Dim frm As New frmLoadFigure
        'frm.Show()
        'If frm.m_OK = False Then Exit Sub
        'frm = Nothing
    End Sub
    Public Sub LayoutByCoord(ByVal pActive As IActiveView)
        'Dim frm As New frmDefineCrd
        'frm.Init(pActive)
        'frm = Nothing
    End Sub

    Public Sub LayoutByEnvelope()
        If Not g_PagelayoutControl.CurrentTool Is Nothing Then
            g_PagelayoutControl.CurrentTool = Nothing
            g_PagelayoutControl.MousePointer = esriControlsMousePointer.esriPointerDefault
        End If
        g_Rubber = True
    End Sub

    Public Sub LayoutByExtent(ByVal pEnvelope As IEnvelope)
        'Dim pActive As IActiveView
        'pActive = g_Map
        'pActive.Extent = pEnvelope
        'pActive.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

    End Sub
    Public Sub LayoutByGeometry(ByVal pGeometry As IGeometry, ByVal pSpatialReference As ISpatialReference, Optional ByVal pClipGeometry As IGeometry = Nothing)
        'g_SubjectTool = True
        If TypeOf pSpatialReference Is IGeographicCoordinateSystem Then
            g_ScaleType = "经纬度坐标"
        End If
        'Dim pFrmSetting As New frmMain
        'pFrmSetting.InitBufferForm(pSpatialReference, g_EnumLayer, pGeometry, pClipGeometry)

        'pFrmSetting.ShowDialog()
        'pFrmSetting = Nothing
    End Sub


    Public Sub LayoutByDef(ByVal pLayer As ILayer, ByVal whereclause As String, ByVal pGeometry As IGeometry, ByVal pSpatialReference As ISpatialReference)
        'g_SubjectTool = True
        'If TypeOf pSpatialReference Is IGeographicCoordinateSystem Then
        '    g_ScaleType = "经纬度坐标"
        'End If
        'Dim pFrmSetting As New frmMain
        'pFrmSetting.InitBufferForm(pSpatialReference, g_EnumLayer, pGeometry)
        'Dim pFeatureLayer As IFeatureLayer = Nothing
        'Dim pFeatureLayerDefintion As IFeatureLayerDefinition = Nothing
        'If Not pLayer Is Nothing Then
        '    pFeatureLayer = pLayer
        '    pFeatureLayerDefintion = pFeatureLayer
        '    pFeatureLayerDefintion.DefinitionExpression = whereclause
        '    If Not g_Activeview Is Nothing Then
        '        g_Activeview.Refresh()
        '    End If
        'End If
        'pFrmSetting.ShowDialog()
        'pFrmSetting = Nothing
    End Sub


    '根据比例尺获得一个图幅的经纬度偏差(注意对于比例尺的偏差是指平面偏差）
    Public Sub GetMapXYoff(ByVal lngMapScale As Long, _
    ByVal dblXoff As Double, _
    ByVal dblYoff As Double)

        Select Case lngMapScale
            Case 500
                dblXoff = 250
                dblYoff = 250
            Case 1000
                dblXoff = 500
                dblYoff = 500
            Case 2000
                dblXoff = 1000
                dblYoff = 1000
                '以上为比例尺数据
                '以下为小比例尺数据 单位均为度
            Case 5000
                dblXoff = 112.5
                dblYoff = 75
            Case 10000
                dblXoff = 225
                dblYoff = 150
            Case 25000
                dblXoff = 7.5 * 60.0#
                dblYoff = 5 * 60.0#
            Case 50000
                dblXoff = 15 * 60.0#
                dblYoff = 10 * 60.0#
            Case 250000
                dblXoff = 1.5 * 3600.0#
                dblYoff = 1 * 3600.0#
        End Select

    End Sub

    '有经纬度的坐标点返回平面下的坐标点（这里返回的只是值　坐标点上并没有坐标参考信息）
    Public Function GetxyVal(ByVal pGeoPoint As IPoint, ByVal pPrj As IProjectedCoordinateSystem) As IPoint
        GetxyVal = Nothing
        If (pPrj Is Nothing) Or (pGeoPoint Is Nothing) Then Exit Function
        Dim pTempPoint As IPoint
        Dim pWKSPoint As WKSPoint

        pWKSPoint.X = pGeoPoint.X
        pWKSPoint.Y = pGeoPoint.Y
        pPrj.Forward(1, pWKSPoint)

        pTempPoint = New Point
        If g80To54 = True Then
            pTempPoint.X = pWKSPoint.X - g_OffX
            pTempPoint.Y = pWKSPoint.Y - g_OffY
        Else
            pTempPoint.X = pWKSPoint.X
            pTempPoint.Y = pWKSPoint.Y
        End If

        GetxyVal = pTempPoint
        Return GetxyVal
    End Function


    Public Function GetCoordinateFromNewCode(ByVal strNewMapCode As String, ByRef x As Double, ByRef y As Double) As Boolean
        On Error GoTo ERRHANDLER

        '根据新图号算西南角点坐标
        Dim txt As String
        Dim b As String       '纬度编号
        Dim l As Integer      '经度序号
        Dim r As Integer      '行数
        Dim c As Integer      '列数
        Dim th As String       '图幅比例尺编号
        Dim YInt As Integer      '纬度序号

        Dim XPlus As Double         '经度差
        Dim YPlus As Double         '纬度差

        GetCoordinateFromNewCode = True

        txt = UCase(Trim(strNewMapCode))
        If txt <> "" Then
            If Len(txt) = 3 Then '*****百万新图号到老图号的转换
                '求纬度编号
                b = Left(txt, 1)
                '求经度序号
                l = Val(Mid(txt, 2, 2))
                If b >= "A" And b < "O" And l > 42 And l < 54 Then
                    '求纬度序号
                    YInt = Asc(b) - 64
                    '求纬度
                    y = (YInt - 1) * 4
                    '求经度
                    x = (l - 31) * 6
                Else
                    GetCoordinateFromNewCode = False
                End If
            ElseIf Len(txt) = 10 Then
                b = Left(txt, 1)
                l = Val(Mid(txt, 2, 2))
                th = Mid(txt, 4, 1)
                r = Val(Mid(txt, 5, 3))
                c = Val(Mid(txt, 8, 3))
                If b > "A" And b < "O" And l > 42 And l < 54 Then
                    YInt = Asc(b) - 64
                    Select Case th
                        Case "B" '*****50万2行2列新图号到老图号的转换
                            '经度差
                            XPlus = 3 * 3600
                            '纬度差
                            YPlus = 2 * 3600
                            If r > 0 And r < 3 And c > 0 And c < 3 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "C" '*****25万4行4列新图号到老图号的转换
                            '经度差
                            XPlus = 3 / 2 * 3600
                            '纬度差
                            YPlus = 1 * 3600
                            If r > 0 And r < 5 And c > 0 And c < 5 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "D" '*****10万12行12列新图号到老图号的转换
                            '经度差
                            XPlus = 1 / 2 * 3600
                            '纬度差
                            YPlus = 1 / 3 * 3600
                            If r > 0 And r < 13 And c > 0 And c < 13 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "E" '*****5万24行24列新图号到老图号的转换
                            '经度差
                            XPlus = 1 / 4 * 3600
                            '纬度差
                            YPlus = 1 / 6 * 3600
                            If r > 0 And r < 25 And c > 0 And c < 25 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "F"    '*****2万5比例尺48行48列新图号到老图号的转换
                            '经度差
                            XPlus = 1 / 8 * 3600
                            '纬度差
                            YPlus = 1 / 12 * 3600
                            If r > 0 And r < 97 And c > 0 And c < 97 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "G" '*****1万96行96列新图号到老图号的转换
                            '经度差
                            XPlus = 1 / 16 * 3600
                            '纬度差
                            YPlus = 1 / 24 * 3600
                            If r > 0 And r < 97 And c > 0 And c < 97 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case "H" '*****5千共192行192列新图号到老图号的转换
                            '经度差
                            XPlus = 1 / 32 * 3600
                            '纬度差
                            YPlus = 1 / 48 * 3600
                            If r > 0 And r < 193 And c > 0 And c < 193 Then
                                '求经度
                                x = ((l - 31) * 6 + (c - 1) * (XPlus / 3600)) * 3600
                                '求纬度
                                y = ((YInt - 1) * 4 + (4 / (YPlus / 3600) - r) * (YPlus / 3600)) * 3600
                            Else
                                GetCoordinateFromNewCode = False
                            End If
                        Case Else
                            GetCoordinateFromNewCode = False
                    End Select
                Else
                    GetCoordinateFromNewCode = False
                End If
            Else
                GetCoordinateFromNewCode = False
            End If
        Else
            GetCoordinateFromNewCode = False
        End If
        Exit Function

ERRHANDLER:
        GetCoordinateFromNewCode = False
    End Function



    '根据经纬度的两点(经纬度坐标点）　以及插入点的数量返回一个插点后的图幅框　上下边线（平面坐标系下的线,但不带坐标信息）
    Public Function GetTopOrBotLine(ByVal pPointFrom As IPoint, _
                                    ByVal pPointTo As IPoint, _
                                    ByVal pPrj As IProjectedCoordinateSystem, _
                                    ByVal intPointCount As Integer) As IGeometry
        Dim dblXMin As Double  'x最小坐标
        Dim dblXMax As Double  'Ｘ最大坐标
        Dim xOff As Double     '插入的间隔
        Dim i As Integer
        Dim pPointCol As IPointCollection
        Dim pTempPoint As IPoint
        Dim pPoint As IPoint
        GetTopOrBotLine = Nothing
        dblXMin = pPointFrom.X
        dblXMax = pPointTo.X
        xOff = (dblXMax - dblXMin) / (intPointCount + 1)

        pPointCol = New Polyline

        For i = 0 To intPointCount
            pTempPoint = New Point
            pTempPoint.X = (pPointFrom.X + i * xOff)
            pTempPoint.Y = pPointFrom.Y

            pPoint = New Point
            pPoint = GetxyVal(pTempPoint, pPrj)

            '将其转到图纸坐标中 并返回一条平面线
            pPointCol.AddPoint(pPoint)
        Next i

        '把最后一点插入
        pPoint = New Point
        pPoint = GetxyVal(pPointTo, pPrj)

        pPointCol.AddPoint(pPoint)

        If pPointCol.PointCount > 1 Then
            GetTopOrBotLine = pPointCol
        End If
        Return GetTopOrBotLine
    End Function
    Public Sub GetPageLayoutSize(ByVal Filepath As String, ByRef dblPageWidth As Double, ByRef dblPageHeight As Double, ByRef dblDataFrameWidth As Double, ByRef dblDataFrameHeight As Double)
        Try
            Dim pMapDoc As IMapDocument
            Dim pElement As IElement
            Dim pGraphicsContainer As IGraphicsContainer
            pMapDoc = New MapDocument

            If pMapDoc.IsMapDocument(Filepath) Then
                pMapDoc.Open(Filepath)
            Else
                MsgBox(Filepath & "的制图模板不存在!", MsgBoxStyle.Information, "提示")
                Exit Sub
            End If
            pMapDoc.PageLayout.Page.QuerySize(dblPageWidth, dblPageHeight)
            '添加制图模板中的Element
            pGraphicsContainer = pMapDoc.PageLayout
            pGraphicsContainer.Reset()
            pElement = pGraphicsContainer.Next
            While Not pElement Is Nothing
                If TypeOf pElement Is IMapFrame Then
                    dblDataFrameWidth = pElement.Geometry.Envelope.Width
                    dblDataFrameHeight = pElement.Geometry.Envelope.Height
                End If
                pElement = pGraphicsContainer.Next
            End While

        Catch ex As Exception

        End Try
    End Sub
    Public Sub SetPageLayoutSize(ByVal vPagelayout As IPageLayoutControl, ByVal dblPageWidth As Double, ByVal dblPageHeight As Double, ByVal dblDataFrameWidth As Double, ByVal dblDataFrameHeight As Double)
        Dim pGrfCtn As IGraphicsContainer
        Dim pElement As IElement
        Dim pMapFrame As IMapFrame
        Dim pBorderSym As ISymbolBorder
        Dim pLineSym As ILineSymbol
        Dim pClone As IClone
        Dim pGeom As IEnvelope
        Dim pColor As IColor
        Dim pCntPt As IPoint
        Dim xzoomRadio As Double
        Dim yzoomRadio As Double
        Dim dblCenterX As Double
        Dim dblCenterY As Double

        dblCenterX = dblPageWidth / 2
        dblCenterY = dblPageHeight / 2

        pGrfCtn = vPagelayout.PageLayout
        pGrfCtn.Reset()
        pElement = pGrfCtn.Next
        On Error Resume Next
        While Not pElement Is Nothing
            If (TypeOf pElement Is IMapFrame) Then
                pMapFrame = pElement
                pBorderSym = pMapFrame.Border
                If Not pBorderSym Is Nothing Then
                    pClone = pBorderSym.LineSymbol
                    pLineSym = pClone.Clone
                    pLineSym.Width = 0.6          '设置外框线粗和制图标准一致
                    pColor = New RgbColor
                    pColor.RGB = RGB(0, 0, 0)
                    pLineSym.Color = pColor
                    pBorderSym.LineSymbol = pLineSym
                    pMapFrame.Border = pBorderSym
                Else
                    pBorderSym = New SymbolBorder
                    pLineSym = New SimpleLineSymbol
                    pColor = New RgbColor
                    pColor.RGB = RGB(0, 0, 0)
                    pLineSym.Color = pColor
                    pLineSym.Width = 0.6
                    pBorderSym.LineSymbol = pLineSym
                    pMapFrame.Border = pBorderSym
                End If

                pGeom = pElement.Geometry.Envelope
                pCntPt = New Point
                pCntPt.PutCoords(dblCenterX, dblCenterY)
                pGeom.CenterAt(pCntPt)
                xzoomRadio = (dblDataFrameWidth - pGeom.Width) / 2 'dblDataFrameWidth / pGeom.Width
                yzoomRadio = (dblDataFrameHeight - pGeom.Height) / 2 'dblDataFrameHeight / pGeom.Height
                pGeom.Expand(xzoomRadio, yzoomRadio, False)
                pElement.Geometry = pGeom
            End If
            pElement = pGrfCtn.Next
        End While
        vPagelayout.Page.PutCustomSize(dblPageWidth, dblPageHeight)
        'vPagelayout.Page.Units = esriUnits.esriCentimeters
    End Sub

    '由MAP上的GEOMETRY转到图纸坐标上来
    Public Function GetPaperPoint(ByVal pMapPoint1 As IPoint, ByVal pMapPoint2 As IPoint, _
        ByVal pPaperPoint1 As IPoint, ByVal pPaperPoint2 As IPoint, _
        ByVal pMapPoint As IPoint) As IPoint
        Dim dblX As Double
        Dim dblY As Double
        GetPaperPoint = Nothing
        dblX = (pPaperPoint1.X - pPaperPoint2.X) * (pMapPoint.X - pMapPoint1.X) / (pMapPoint1.X - pMapPoint2.X) + pPaperPoint1.X
        dblY = (pPaperPoint1.Y - pPaperPoint2.Y) * (pMapPoint.Y - pMapPoint1.Y) / (pMapPoint1.Y - pMapPoint2.Y) + pPaperPoint1.Y

        Dim pPoint As IPoint
        pPoint = New Point

        pPoint.PutCoords(dblX, dblY)
        If Not pPoint Is Nothing Then
            GetPaperPoint = pPoint
        End If
        Return GetPaperPoint
    End Function



    Public Function GetGeometryByNewCode(ByVal vNewCode As String, _
                                      ByVal vScale As Long, _
                                      ByVal vSpatialReference As ISpatialReference, _
                                      Optional ByVal vType As Integer = 0) As Polygon
        '根据图号计算几何图形
        'vNewCode: 5000~100000为国家标准图号的新图号
        'vScale:500、1000、2000、5000、10000、25000、50000、100000
        'vSpatialReference:ProjectedCoordinateSystem空间参考
        'vType:针对500、1000、2000图幅的两种形式0: 50cm*50cm、1: 50cm*40cm,
        On Error GoTo MyErr

        GetGeometryByNewCode = Nothing

        Dim pProjectReference As IProjectedCoordinateSystem

        If TypeOf vSpatialReference Is IProjectedCoordinateSystem Then
            pProjectReference = vSpatialReference
        Else
            Exit Function
        End If

        Dim pt1 As IPoint = Nothing, pt2 As IPoint = Nothing, Pt3 As IPoint = Nothing, Pt4 As IPoint = Nothing
        Dim p1 As WKSPoint, p2 As WKSPoint, P3 As WKSPoint, P4 As WKSPoint

        Dim x As Double
        Dim y As Double

        Select Case vScale

            Case "500"
                'Dim pI, pII, pIII As Double
                'x = GetStrByNumber(vNewCode, "-", 1)
                'y = GetStrByNumber(vNewCode, "-", 2)
                'pI = GetStrByNumber(vNewCode, "-", 3)
                'pII = GetStrByNumber(vNewCode, "-", 4)
                'pIII = GetStrByNumber(vNewCode, "-", 5)

                'If vType = 0 Then
                '    Pt4 = New Point : Pt4.X = CDbl(x) * 1000 : Pt4.Y = CDbl(y) * 1000
                '    Pt3 = New Point : Pt3.X = (CDbl(x) + 0.25) * 1000 : Pt3.Y = CDbl(y) * 1000
                '    pt2 = New Point : pt2.X = (CDbl(x) + 0.25) * 1000 : pt2.Y = (CDbl(y) + 0.25) * 1000
                '    pt1 = New Point : pt1.X = CDbl(x) * 1000 : pt1.Y = (CDbl(y) + 0.25) * 1000
                'ElseIf vType = 1 Then

                'End If

            Case "1000"

                If CountDelimiters(vNewCode, ".") <> 2 Or CountDelimiters(vNewCode, "-") <> 1 Then GoTo MyErr
                x = GetStrByNumber(vNewCode, "-", 2)
                y = GetStrByNumber(vNewCode, "-", 1)

                If vType = 0 Then
                    Pt4 = New Point : Pt4.X = CDbl(x) * 1000 : Pt4.Y = CDbl(y) * 1000
                    Pt3 = New Point : Pt3.X = (CDbl(x) + 0.5) * 1000 : Pt3.Y = CDbl(y) * 1000
                    pt2 = New Point : pt2.X = (CDbl(x) + 0.5) * 1000 : pt2.Y = (CDbl(y) + 0.5) * 1000
                    pt1 = New Point : pt1.X = CDbl(x) * 1000 : pt1.Y = (CDbl(y) + 0.5) * 1000
                ElseIf vType = 1 Then
                    Pt4 = New Point : Pt4.X = CDbl(x) * 1000 : Pt4.Y = CDbl(y) * 1000
                    Pt3 = New Point : Pt3.X = (CDbl(x) + 0.5) * 1000 : Pt3.Y = CDbl(y) * 1000
                    pt2 = New Point : pt2.X = (CDbl(x) + 0.5) * 1000 : pt2.Y = (CDbl(y) + 0.4) * 1000
                    pt1 = New Point : pt1.X = CDbl(x) * 1000 : pt1.Y = (CDbl(y) + 0.4) * 1000
                End If

            Case "2000"

                If CountDelimiters(vNewCode, ".") <> 2 Or CountDelimiters(vNewCode, "-") <> 1 Then GoTo MyErr
                x = GetStrByNumber(vNewCode, "-", 2)
                y = GetStrByNumber(vNewCode, "-", 1)

                If vType = 0 Then
                    Pt4 = New Point : Pt4.X = CDbl(x) * 1000 : Pt4.Y = CDbl(y) * 1000
                    Pt3 = New Point : Pt3.X = (CDbl(x) + 1) * 1000 : Pt3.Y = CDbl(y) * 1000
                    pt2 = New Point : pt2.X = (CDbl(x) + 1) * 1000 : pt2.Y = (CDbl(y) + 1) * 1000
                    pt1 = New Point : pt1.X = CDbl(x) * 1000 : pt1.Y = (CDbl(y) + 1) * 1000
                ElseIf vType = 1 Then
                    Pt4 = New Point : Pt4.X = CDbl(x) * 1000 : Pt4.Y = CDbl(y) * 1000
                    Pt3 = New Point : Pt3.X = (CDbl(x) + 1) * 1000 : Pt3.Y = CDbl(y) * 1000
                    pt2 = New Point : pt2.X = (CDbl(x) + 1) * 1000 : pt2.Y = (CDbl(y) + 0.8) * 1000
                    pt1 = New Point : pt1.X = CDbl(x) * 1000 : pt1.Y = (CDbl(y) + 0.8) * 1000
                End If

            Case "5000"

                If GetCoordinateFromNewCode(vNewCode, x, y) = True Then
                    P4.X = x / 3600 : P4.Y = y / 3600 : pProjectReference.Forward(1, P4)
                    Pt4 = New Point : Pt4.X = P4.X : Pt4.Y = P4.Y

                    P3.X = (x + 1 / 32 * 3600) / 3600 : P3.Y = y / 3600 : pProjectReference.Forward(1, P3)
                    Pt3 = New Point : Pt3.X = P3.X : Pt3.Y = P3.Y

                    p2.X = (x + 1 / 32 * 3600) / 3600 : p2.Y = (y + 1 / 48 * 3600) / 3600 : pProjectReference.Forward(1, p2)
                    pt2 = New Point : pt2.X = p2.X : pt2.Y = p2.Y

                    p1.X = x / 3600 : p1.Y = (y + 1 / 48 * 3600) / 3600 : pProjectReference.Forward(1, p1)
                    pt1 = New Point : pt1.X = p1.X : pt1.Y = p1.Y
                Else
                    Exit Function
                End If

            Case "10000"

                If GetCoordinateFromNewCode(vNewCode, x, y) = True Then
                    P4.X = x / 3600 : P4.Y = y / 3600 : pProjectReference.Forward(1, P4)
                    Pt4 = New Point : Pt4.X = P4.X : Pt4.Y = P4.Y

                    P3.X = (x + 1 / 16 * 3600) / 3600 : P3.Y = y / 3600 : pProjectReference.Forward(1, P3)
                    Pt3 = New Point : Pt3.X = P3.X : Pt3.Y = P3.Y

                    p2.X = (x + 1 / 16 * 3600) / 3600 : p2.Y = (y + 1 / 24 * 3600) / 3600 : pProjectReference.Forward(1, p2)
                    pt2 = New Point : pt2.X = p2.X : pt2.Y = p2.Y

                    p1.X = x / 3600 : p1.Y = (y + 1 / 24 * 3600) / 3600 : pProjectReference.Forward(1, p1)
                    pt1 = New Point : pt1.X = p1.X : pt1.Y = p1.Y
                Else
                    Exit Function
                End If

            Case "25000"

                If GetCoordinateFromNewCode(vNewCode, x, y) = True Then
                    P4.X = x / 3600 : P4.Y = y / 3600 : pProjectReference.Forward(1, P4)
                    Pt4 = New Point : Pt4.X = P4.X : Pt4.Y = P4.Y

                    P3.X = (x + 1 / 8 * 3600) / 3600 : P3.Y = y / 3600 : pProjectReference.Forward(1, P3)
                    Pt3 = New Point : Pt3.X = P3.X : Pt3.Y = P3.Y

                    p2.X = (x + 1 / 8 * 3600) / 3600 : p2.Y = (y + 1 / 12 * 3600) / 3600 : pProjectReference.Forward(1, p2)
                    pt2 = New Point : pt2.X = p2.X : pt2.Y = p2.Y

                    p1.X = x / 3600 : p1.Y = (y + 1 / 12 * 3600) / 3600 : pProjectReference.Forward(1, p1)
                    pt1 = New Point : pt1.X = p1.X : pt1.Y = p1.Y
                Else
                    Exit Function
                End If

            Case "50000"

                If GetCoordinateFromNewCode(vNewCode, x, y) = True Then
                    P4.X = x / 3600 : P4.Y = y / 3600 : pProjectReference.Forward(1, P4)
                    Pt4 = New Point : Pt4.X = P4.X : Pt4.Y = P4.Y

                    P3.X = (x + 1 / 4 * 3600) / 3600 : P3.Y = y / 3600 : pProjectReference.Forward(1, P3)
                    Pt3 = New Point : Pt3.X = P3.X : Pt3.Y = P3.Y

                    p2.X = (x + 1 / 4 * 3600) / 3600 : p2.Y = (y + 1 / 6 * 3600) / 3600 : pProjectReference.Forward(1, p2)
                    pt2 = New Point : pt2.X = p2.X : pt2.Y = p2.Y

                    p1.X = x / 3600 : p1.Y = (y + 1 / 6 * 3600) / 3600 : pProjectReference.Forward(1, p1)
                    pt1 = New Point : pt1.X = p1.X : pt1.Y = p1.Y
                Else
                    Exit Function
                End If

            Case "100000"

                If GetCoordinateFromNewCode(vNewCode, x, y) = True Then
                    P4.X = x / 3600 : P4.Y = y / 3600 : pProjectReference.Forward(1, P4)
                    Pt4 = New Point : Pt4.X = P4.X : Pt4.Y = P4.Y

                    P3.X = (x + 1 / 2 * 3600) / 3600 : P3.Y = y / 3600 : pProjectReference.Forward(1, P3)
                    Pt3 = New Point : Pt3.X = P3.X : Pt3.Y = P3.Y

                    p2.X = (x + 1 / 2 * 3600) / 3600 : p2.Y = (y + 1 / 3 * 3600) / 3600 : pProjectReference.Forward(1, p2)
                    pt2 = New Point : pt2.X = p2.X : pt2.Y = p2.Y

                    p1.X = x / 3600 : p1.Y = (y + 1 / 3 * 3600) / 3600 : pProjectReference.Forward(1, p1)
                    pt1 = New Point : pt1.X = p1.X : pt1.Y = p1.Y
                Else
                    Exit Function
                End If

            Case Else
                Exit Function
        End Select

        Dim pPolygon As IPolygon
        Dim pPointCollection As IPointCollection
        pPointCollection = New Polygon

        pPointCollection.AddPoint(Pt4)
        pPointCollection.AddPoint(pt1)
        pPointCollection.AddPoint(pt2)
        pPointCollection.AddPoint(Pt3)
        pPointCollection.AddPoint(Pt4)
        pPolygon = pPointCollection

        pPolygon.Project(vSpatialReference)

        GetGeometryByNewCode = pPolygon
        Return GetGeometryByNewCode
MyErr:

    End Function

    Public Function CountDelimiters(ByVal sFiles As String, _
                                    ByVal vSearchChar As Object) As Integer

        Dim iCtr As Integer
        Dim iResult As Integer

        For iCtr = 1 To Len(sFiles)

            If Mid(sFiles, iCtr, 1) = vSearchChar Then iResult = iResult + 1
        Next iCtr

        CountDelimiters = iResult

    End Function

    Public Function GetStrByNumber(ByVal string1 As String, _
                                   ByVal Identifiers As String, _
                                   ByVal num As Long) As String
        '获得一个用间隔符分开的字符串中的某个序号的字符串

        GetStrByNumber = ""

        If num <= 0 Then Exit Function
        If string1 = "" Then Exit Function

        Dim Splits() As String
        ReDim Splits(0)
        Splits = Split(string1, Identifiers, -1, vbTextCompare)

        If num - 1 > UBound(Splits) Then
            Erase Splits
            Exit Function
        End If

        GetStrByNumber = Splits(num - 1)
        Erase Splits
        Exit Function
    End Function

    '
    Public Function GetGeoByBigMapSheet(ByVal strSheetNo As String, ByVal lngMapScale As Long, ByVal pSpatial As ISpatialReference) As IGeometry
        Dim pPt1 As IPoint
        Dim pPt2 As IPoint
        Dim pPt3 As IPoint
        Dim pPt4 As IPoint
        Dim pPointCol As IPointCollection
        Dim pPolygon As IPolygon
        GetGeoByBigMapSheet = Nothing
        Dim dblXL As Double  '西南角X坐标值
        Dim dblYL As Double  '西南角Y坐标值
        Dim strTemp() As String
        Dim dblXoff As Double  '图幅长度
        Dim dblYoff As Double  '图幅宽度

        Select Case lngMapScale
            Case 2000
                dblXoff = 1000
                dblYoff = 1000

                strTemp = Split(strSheetNo, "-")
                dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000
                dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000
            Case 1000
                dblXoff = 500
                dblYoff = 500

                strTemp = Split(strSheetNo, "-")
                Select Case strTemp(4)
                    Case "A"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000 + 500
                    Case "B"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000 + 500
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000 + 500
                    Case "C"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000
                    Case "D"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000 + 500
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000
                End Select
            Case 500
                dblXoff = 250
                dblYoff = 250

                strTemp = Split(strSheetNo, "-")

                Select Case strTemp(4)
                    Case "01", "02", "05", "06"
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000 + 750
                    Case "03", "04", "07", "08"
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000 + 500
                    Case "09", "10", "13", "14"
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000 + 250
                    Case "11", "12", "15", "16"
                        dblYL = CDbl(strTemp(0) & strTemp(1)) * 1000
                End Select

                Select Case strTemp(4)
                    Case "01", "03", "09", "11"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000
                    Case "02", "04", "10", "12"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000 + 250
                    Case "05", "07", "13", "15"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000 + 500
                    Case "06", "08", "14", "16"
                        dblXL = CDbl(strTemp(2) & strTemp(3)) * 1000 + 750
                End Select
        End Select

        '注意点的顺序是从西南角坐标开始 按顺时针方向
        pPt1 = New Point : pPt1.X = dblXL : pPt1.Y = dblYL
        pPt2 = New Point : pPt2.X = dblXL : pPt2.Y = dblYL + dblYoff
        pPt3 = New Point : pPt3.X = dblXL + dblXoff : pPt3.Y = dblYL + dblYoff
        pPt4 = New Point : pPt4.X = dblXL + dblXoff : pPt4.Y = dblYL

        pPointCol = New Polygon
        pPointCol.AddPoint(pPt1)
        pPointCol.AddPoint(pPt2)
        pPointCol.AddPoint(pPt3)
        pPointCol.AddPoint(pPt4)
        pPointCol.AddPoint(pPt1)

        If pPointCol.PointCount < 3 Then Exit Function
        pPolygon = pPointCol
        pPolygon.Close()
        pPolygon.Project(pSpatial)

        If Not pPolygon Is Nothing Then
            GetGeoByBigMapSheet = pPolygon
        End If

        Return GetGeoByBigMapSheet
    End Function
    Public Sub CalculateFigureforme(ByVal vFigure As String, ByVal vScaleType As String, ByRef neighborTH() As String)
        'Dim mySql As String
        Dim vVal As Double
        Dim vScaleText As String = ""
        '根据图幅号获得西南坐标
        Dim dblX As Double
        Dim dblY As Double
        Dim xOffset As Double  '经差
        Dim yOffset As Double  '纬差

        Select Case vScaleType

            '    'Case "1:2000比例尺"
            '    '    'mySql = "select * from  元数据_DLG2000"
            '    '    vVal = 0.25
            '    '    vScaleText = "2000"
            '    '    xOffset = 1000
            '    '    yOffset = 1000
            Case "1:500比例尺"
                'mySql = "select * from  元数据_DLG2000"
                vScaleText = "500"
                xOffset = 250
                yOffset = 250
            Case "5000"
                'mySql = "select * from  元数据_DLG5000"
                vVal = 0.5
                vScaleText = "5000"
                xOffset = 112.5
                yOffset = 75
            Case "10000"
                'mySql = "select * from  元数据_DLG10000"
                vVal = 0.5
                vScaleText = "10000"
                xOffset = 225
                yOffset = 150
            Case "25000"
                vVal = 1
                vScaleText = "25000"
                xOffset = 450
                yOffset = 300
            Case "50000"
                'mySql = "select * from  元数据_DLG50000"
                vVal = 1
                vScaleText = "50000"
                xOffset = 900
                yOffset = 600
            Case "100000"
                vVal = 1
                vScaleText = "100000"
                xOffset = 1800
                yOffset = 1200
            Case "250000"
                vVal = 1
                vScaleText = "250000"
                xOffset = 4500
                yOffset = 3000

        End Select
        'If CLng(vScaleText) < 10000 Then
        '    dblX = pSheetEnvelope.xmin
        '    dblY = pSheetEnvelope.ymin
        'Else
        GetCoordinateFromNewCode(vFigure, dblX, dblY)


        '左上角图幅
        DoEmptyforme("G50G004004", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(0))
        '正上方图幅
        DoEmptyforme("G50G004005", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(1))
        '右上角图幅
        DoEmptyforme("G50G004006", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(2))
        '左边图幅
        DoEmptyforme("G50G005004", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(3))

        DoEmptyforme("G50G005006", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(4))
        '左下角图幅
        DoEmptyforme("G50G006004", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(5))
        '正下方图幅
        DoEmptyforme("G50G006005", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(6))
        '右下角图幅
        DoEmptyforme("G50G006006", CLng(vScaleText), dblX, dblY, xOffset, yOffset, neighborTH(7))


    End Sub

    Public Sub CalculateFigure(ByVal vFigure As String, ByVal vScaleType As String, ByVal m_FrameInfo As Collection, _
     ByVal pPageLayout As IPageLayout, ByVal pSheetGeometry As IGeometry)
        'Dim mySql As String
        Dim vVal As Double
        Dim vScaleText As String = ""
        '获得图幅各个角坐标 
        Dim pSheetEnvelope As IEnvelope
        pSheetEnvelope = pSheetGeometry.Envelope


        '根据图幅号获得西南坐标
        Dim dblX As Double
        Dim dblY As Double
        Dim xOffset As Double  '经差
        Dim yOffset As Double  '纬差

        Select Case vScaleType

            '    'Case "1:2000比例尺"
            '    '    'mySql = "select * from  元数据_DLG2000"
            '    '    vVal = 0.25
            '    '    vScaleText = "2000"
            '    '    xOffset = 1000
            '    '    yOffset = 1000
            Case "1:500比例尺"
                'mySql = "select * from  元数据_DLG2000"
                vScaleText = "500"
                xOffset = 250
                yOffset = 250
            Case "5000"
                'mySql = "select * from  元数据_DLG5000"
                vVal = 0.5
                vScaleText = "5000"
                xOffset = 112.5
                yOffset = 75
            Case "10000"
                'mySql = "select * from  元数据_DLG10000"
                vVal = 0.5
                vScaleText = "10000"
                xOffset = 225
                yOffset = 150
            Case "25000"
                vVal = 1
                vScaleText = "25000"
                xOffset = 450
                yOffset = 300
            Case "50000"
                'mySql = "select * from  元数据_DLG50000"
                vVal = 1
                vScaleText = "50000"
                xOffset = 900
                yOffset = 600
            Case "100000"
                vVal = 1
                vScaleText = "100000"
                xOffset = 1800
                yOffset = 1200
        End Select
        'If CLng(vScaleText) < 10000 Then
        '    dblX = pSheetEnvelope.xmin
        '    dblY = pSheetEnvelope.ymin
        'Else
        GetCoordinateFromNewCode(vFigure, dblX, dblY)
        'End If

        Dim myRec As IRecordSet = Nothing
        'myRec = New RecordSet
        'myRec.open(mySql, g_Sys.SysCn, , adLockReadOnly)
        Dim GetFieldValue As String = ""
        Dim FieldValue As String

        Dim pTxtElement As ITextElement
        Dim pGraphicsContainer As IGraphicsContainer
        FieldValue = ""
        pGraphicsContainer = New PageLayout
        pGraphicsContainer = pPageLayout
        Dim pElement As IElement
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next

        Try
            While Not pElement Is Nothing
                If TypeOf pElement Is ITextElement Then
                    pTxtElement = pElement

                    Select Case pTxtElement.Text
                        '左上角图幅
                        Case "G50G004004"
                            FieldValue = "图幅接合表中西北图幅名称"
                            DoEmpty("G50G004004", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '正上方图幅
                        Case "G50G004005"
                            FieldValue = "图幅接合表中北图幅名称"
                            DoEmpty("G50G004005", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '右上角图幅
                        Case "G50G004006"
                            FieldValue = "图幅接合表中东北图幅名称"
                            DoEmpty("G50G004006", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '左边图幅
                        Case "G50G005004"
                            FieldValue = "图幅接合表中西图幅名称"
                            DoEmpty("G50G005004", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                        Case "图号1"
                            'FieldValue = "图号1"
                            FieldValue = "图号"
                        Case "图号2"
                            'FieldValue = "图号2"
                            FieldValue = "图号"
                        Case "图号3"
                            'FieldValue = "图号3"
                            FieldValue = "图号"
                        Case "图号4"
                            'FieldValue = "图号4"
                            FieldValue = "图号"
                            'Case "图号3"
                            '    DoEmpty("图号3", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            'Case "图号4"
                            '    DoEmpty("图号4", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            'Case "图号5"
                            '    DoEmpty("图号5", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            'Case "图号6"
                            '    DoEmpty("图号6", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            'FigureCharTable vFigure, pTxtElement
                            '当前图幅
                        Case "G50G005005"
                            FigureCharTable(vFigure, pTxtElement)
                            FieldValue = "图号"
                        Case "图名1"
                            'FieldValue = "图名1"
                            FieldValue = "图名"
                        Case "图名2"
                            'FigureCharTable vFigure, vScaleType, pTxtElement]
                            'FieldValue = "图名2"
                            FieldValue = "图名"
                        Case "图名3"
                            'FieldValue = "图名3"
                            FieldValue = "图名"
                        Case "图名4"
                            'FieldValue = "图名4"
                            FieldValue = "图名"
                            '当前图幅
                        Case "G50G005005"
                            FigureCharTable(vFigure, pTxtElement)
                            FieldValue = "图号"
                            '右边图幅
                        Case "G50G005006"
                            FieldValue = "图幅接合表中东图幅名称"
                            DoEmpty("G50G005006", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '左下角图幅
                        Case "G50G006004"
                            FieldValue = "图幅接合表中西南图幅名称"
                            DoEmpty("G50G006004", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '正下方图幅
                        Case "G50G006005"
                            FieldValue = "图幅接合表中南图幅名称"
                            DoEmpty("G50G006005", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '右下角图幅
                        Case "G50G006006"
                            FieldValue = "图幅接合表中东南图幅名称"
                            DoEmpty("G50G006006", CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                            '当前比例尺
                        Case "比例尺"
                            FieldValue = "比例尺分母"
                        Case "比例尺2"
                            FieldValue = "比例尺2"
                        Case "图名"
                            FieldValue = "图名"

                        Case "密级"
                            If vScaleType = "50000" Then
                                pTxtElement.Text = "机密 ★ 长期"
                            End If
                            FieldValue = "密级"

                        Case "附注："
                            FieldValue = "附注"
                        Case "坐标系"
                            'FieldValue = "平面坐标系"
                            FieldValue = "坐标系统"
                        Case "测绘机关全称"
                            'FieldValue = "产品生产单位名称"
                            FieldValue = "制图单位"
                            'Case "测绘机关全称"
                            '    FieldValue = "产品生产单位名称"
                            'Case "检查员"
                            '    FieldValue = "检查员"
                            'Case "绘图员"
                            '    FieldValue = "入库人员"
                            'Case "测量员"
                            '    FieldValue = "测绘员"
                        Case "1985国家高程基准，"
                            'FieldValue = "高程基准"
                            FieldValue = "高程"
                        Case "等高距为1米。"
                            'FieldValue = "图幅等高距"
                            FieldValue = "等高距"
                        Case "1995年5月XXX测图。"
                            'FieldValue = "产品生产日期"
                            FieldValue = "测图时间"
                            'FieldValue = "年份"
                        Case "1996年版图式。"
                            FieldValue = "年份"

                            '因为图廓角坐标没有让用户编辑，所以就直接算出来了，和上面数据库取的方法不同
                            '左下角y,x坐标
                        Case "553.75左下"
                            'FieldValue = "西南图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY, pTxtElement, True)
                            End If
                        Case "385.50左下"
                            ' FieldValue = "西南图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX, pTxtElement)
                            End If
                            '左上角y,x坐标
                        Case "554.00左上"
                            'FieldValue = "西北图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY + 600, pTxtElement, True)
                            End If
                        Case "385.50左上"
                            'FieldValue = "西北图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX, pTxtElement)
                            End If
                            '右上角坐标
                        Case "554.00右上"
                            ' FieldValue = "东北图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY + 600, pTxtElement, True)
                            End If
                        Case "385.75右上"
                            'FieldValue = "东北图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX + 900, pTxtElement)
                            End If
                            '右下角坐标
                        Case "553.75右下"
                            ' FieldValue = "东南图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY, pTxtElement, True)
                            End If
                        Case "385.75右下"
                            'FieldValue = "东南图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX + 900, pTxtElement)
                            End If
                        Case "检查员"
                            FieldValue = "检查员"
                        Case "绘图员"
                            FieldValue = "绘图员"
                        Case "测量员"
                            FieldValue = "测量员"
                            '因为图廓角坐标没有让用户编辑，所以就直接算出来了，和上面数据库取的方法不同
                            '左下角y,x坐标
                        Case "553.75"
                            'FieldValue = "西南图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY, pTxtElement, True)
                            End If
                        Case "385.50"
                            ' FieldValue = "西南图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX, pTxtElement)
                            End If
                            '左上角y,x坐标
                        Case "554.00"
                            'FieldValue = "西北图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY + 600, pTxtElement, True)
                            End If
                        Case "385.50"
                            'FieldValue = "西北图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX, pTxtElement)
                            End If
                            '右上角坐标
                        Case "554.00"
                            ' FieldValue = "东北图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY + 600, pTxtElement, True)
                            End If
                        Case "385.75"
                            'FieldValue = "东北图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX + 900, pTxtElement)
                            End If
                            '右下角坐标
                        Case "553.75"
                            ' FieldValue = "东南图廓角点Y坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.YMin, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblY, pTxtElement, True)
                            End If
                        Case "385.75"
                            'FieldValue = "东南图廓角点X坐标"
                            If CLng(vScaleText) < 5000 Then
                                FigureChar(pSheetEnvelope.XMax, vScaleType, pTxtElement)
                            Else
                                Call ReWriteCoor(dblX + 900, pTxtElement)
                            End If
                    End Select

                    'If FieldValue <> "" Then
                    '    If FieldValue = "比例尺分母" Then
                    '        GetFieldValue = "1:" & vScaleText
                    '    ElseIf FieldValue = "比例尺2" Then
                    '        GetFieldValue = "1:" & vScaleText
                    '        'GetFieldValue = ReadMeatTable(mySql, myRec, FieldValue, vFigure)
                    '    End If
                    '    'm_FrameInfo.Add(GetFieldValue, FieldValue)
                    '    If GetFieldValue <> "" Then
                    '        pTxtElement.Text = GetFieldValue
                    '    Else
                    '        'If FieldValue = "图名" Then
                    '        '    pTxtElement.Text = "图幅名"
                    '        If FieldValue = "图号" Then
                    '            pTxtElement.Text = "图幅号"
                    '        Else
                    '            pTxtElement.Text = FieldValue
                    '        End If

                    '        '传个GEOMETRY过去 以便查相邻的图幅号
                    '        'DoEmpty pTxtElement.Text, vScaleType, pTxtElement, LeftY, LeftX, vVal, m_FrameInfo, FieldValue
                    '        '    'DoEmpty(FieldValue, CLng(vScaleText), pTxtElement, m_FrameInfo, dblX, dblY, xOffset, yOffset)
                    '    End If
                    'End If
                    FieldValue = ""
                    GetFieldValue = ""
                    'ElseIf Not TypeOf pElement Is IMapFrame Then

                    '        ElseIf TypeOf pElement Is IGroupElement Then
                    '           pElement.Deactivate
                    '        ElseIf Not TypeOf pElement Is IMapFrame Then
                Else
                    pElement.Locked = True
                End If

                pElement = pGraphicsContainer.Next
            End While
        Catch ex As Exception
            Debug.Write(ex.Message)
        End Try


    End Sub

    'Public Function ReadMeatTable(ByVal mySql As String, ByVal myRec As IRecordSet, ByVal vFieldValue As String, ByVal vFigure As String) As String

    '    '将DlG的三种比例尺元数据表打开

    '    Do While Not myRec.EOF = True
    '        If myRec.!图号 = vFigure Then
    '            If IsNull(myRec.Fields(vFieldValue)) = False Then
    '                ReadMeatTable = myRec.Fields(vFieldValue)
    '            Else
    '                ReadMeatTable = ""
    '            End If
    '            Exit Do
    '        End If
    '        myRec.MoveNext()
    '    Loop
    'End Function

    Public Sub FigureChar(ByVal LeftCoord As Double, ByVal vFigure As String, ByVal pTxtElement As ITextElement)
        If vFigure = "1:10000比例尺" Then
            pTxtElement.Text = LeftCoord
        ElseIf vFigure = "1:500比例尺" Then
            pTxtElement.Text = Format(LeftCoord / 1000, "##.00")
        Else
            pTxtElement.Text = Format(LeftCoord / 1000, "###.0")
        End If
    End Sub
    '模板图四角坐标重新赋值 
    Public Sub ReWriteCoor(ByVal dblS As Double, ByVal pTextElement As ITextElement, Optional ByVal blnEnter As Boolean = False)
        Dim lngD As Long
        Dim lngM As Long
        Dim lngS As Long
        Dim strS As String
        Dim strM As String

        lngD = Fix(dblS / 3600)
        lngM = Fix((dblS - lngD * 3600) / 60)
        lngS = Fix(dblS - lngD * 3600 - lngM * 60)
        If lngM < 10 Then
            strM = "0" & CStr(lngM)
        Else
            strM = CStr(lngM)
        End If
        If lngS < 10 Then
            strS = "0" & CStr(lngS)
        Else
            strS = CStr(lngS)
        End If
        If blnEnter = True Then
            pTextElement.Text = lngD & "°" & vbCrLf & strM & "′" & strS & "″"
        Else
            pTextElement.Text = lngD & "°" & strM & "′" & strS & "″"
        End If
    End Sub

    Public Sub FigureCharTable(ByVal strSheetNo As String, ByVal pTxtElement As ITextElement)
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If
        pTxtElement.Text = strSheetNo
    End Sub

    ''根据比例尺和图幅号算出来相邻的图幅信息
    'Private Sub DoEmpty(ByVal vStrElement As String, ByVal vScale As Long, _
    'ByVal pTxtElement As ITextElement, ByVal m_FrameInfo As Collection, ByVal dblX As Double, ByVal dblY As Double, ByVal xOffset As Double, ByVal yOffset As Double)

    '    Dim strSheetNo As String = ""
    '    '由图幅号获得西南角坐标
    '    Select Case vStrElement

    '        Case "图幅接合表中西北图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + yOffset + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + yOffset + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)
    '        Case "图幅接合表中北图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + yOffset + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)

    '        Case "图幅接合表中东北图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + yOffset + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + yOffset + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)

    '        Case "图幅接合表中西图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)
    '        Case "图号"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)
    '            '
    '        Case "图幅接合表中东图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)

    '        Case "图幅接合表中西南图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY - 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY - 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)

    '        Case "图幅接合表中南图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY - 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)

    '        Case "图幅接合表中东南图幅名称"
    '            If vScale < 5000 Then
    '                GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY - 100)
    '            Else
    '                GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY - 30, vScale)
    '            End If
    '            Call FigureCharTable(strSheetNo, pTxtElement)
    '            m_FrameInfo.Remove(vStrElement)
    '            m_FrameInfo.Add(pTxtElement.Text, vStrElement)
    '        Case "图号1"

    '        Case "图号2"

    '        Case "图号3"
    '            'If vScale >= 50000 Then

    '            '    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
    '            '    Call FigureCharTable(strSheetNo, pTxtElement)
    '            'End If
    '        Case "图号4"
    '            'If vScale >= 50000 Then
    '            '    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
    '            '    Call FigureCharTable(strSheetNo, pTxtElement)
    '            'End If
    '            'Case "图号5"
    '            '    If vScale >= 50000 Then
    '            '        GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
    '            '        Call FigureCharTable(strSheetNo, pTxtElement)
    '            '    End If
    '            'Case "图号6"
    '            '    If vScale >= 50000 Then
    '            '        GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
    '            '        Call FigureCharTable(strSheetNo, pTxtElement)
    '            '    End If
    '    End Select

    'End Sub

    '根据比例尺和图幅号算出来相邻的图幅信息

    '根据比例尺和图幅号算出来相邻的图幅信息
    Private Sub DoEmpty(ByVal vStrElement As String, ByVal vScale As Long, _
    ByVal pTxtElement As ITextElement, ByVal m_FrameInfo As Collection, ByVal dblX As Double, ByVal dblY As Double, ByVal xOffset As Double, ByVal yOffset As Double)

        Dim strSheetNo As String = ""
        '由图幅号获得西南角坐标
        Select Case vStrElement

            Case "G50G004004"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + yOffset + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)
            Case "G50G004005"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)

            Case "G50G004006"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + yOffset + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)

            Case "G50G005004"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)
            Case "图号"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                m_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)
                '
            Case "G50G005006" '"图幅接合表中东图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)

            Case "G50G006004" '"图幅接合表中西南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY - 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)

            Case "G50G006005" '"图幅接合表中南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                'm_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)

            Case "G50G006006" '"图幅接合表中东南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY - 30, vScale)
                End If
                Call FigureCharTable(strSheetNo, pTxtElement)
                ' m_FrameInfo.Remove(vStrElement)
                m_FrameInfo.Add(pTxtElement.Text, vStrElement)
            Case "图号1"

            Case "图号2"

            Case "图号3"
                'If vScale >= 50000 Then

                '    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
                '    Call FigureCharTable(strSheetNo, pTxtElement)
                'End If
            Case "图号4"
                'If vScale >= 50000 Then
                '    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
                '    Call FigureCharTable(strSheetNo, pTxtElement)
                'End If
                'Case "图号5"
                '    If vScale >= 50000 Then
                '        GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
                '        Call FigureCharTable(strSheetNo, pTxtElement)
                '    End If
                'Case "图号6"
                '    If vScale >= 50000 Then
                '        GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
                '        Call FigureCharTable(strSheetNo, pTxtElement)
                '    End If
        End Select

    End Sub


    '根据比例尺和图幅号算出来相邻的图幅信息
    Private Sub DoEmptyforme(ByVal vStrElement As String, ByVal vScale As Long, _
     ByVal dblX As Double, ByVal dblY As Double, ByVal xOffset As Double, ByVal yOffset As Double, ByRef strSheetNo As String)

        'Dim strSheetNo As String = ""
        '由图幅号获得西南角坐标
        Select Case vStrElement

            Case "G50G004004"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + yOffset + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)

            Case "G50G004005"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)


            Case "G50G004006"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + yOffset + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + yOffset + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)


            Case "G50G005004"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)

            Case "图号"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)

                '
            Case "G50G005006" '"图幅接合表中东图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY + 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)


            Case "G50G006004" '"图幅接合表中西南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX - 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY - 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)


            Case "G50G006005" '"图幅接合表中南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
                End If

                'm_FrameInfo.Remove(vStrElement)


            Case "G50G006006" '"图幅接合表中东南图幅名称"
                If vScale < 5000 Then
                    GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset + 100, dblY - 100)
                Else
                    GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY - 30, vScale)
                End If

                ' m_FrameInfo.Remove(vStrElement)

            Case "图号1"

            Case "图号2"

            Case "图号3"
                'If vScale >= 50000 Then

                '    GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
                '  
                'End If
            Case "图号4"
                'If vScale >= 50000 Then
                '    GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
                '  
                'End If
                'Case "图号5"
                '    If vScale >= 50000 Then
                '        GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
                '      
                '    End If
                'Case "图号6"
                '    If vScale >= 50000 Then
                '        GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
                '      
                '    End If
        End Select

    End Sub


    Public Sub GetFrameInfo(ByVal vScale As Long, _
    ByVal m_FrameInfo As Collection, ByVal dblX As Double, ByVal dblY As Double, ByVal xOffset As Double, ByVal yOffset As Double)

        Dim strSheetNo As String = ""
        '由图幅号获得西南角坐标

        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX - xOffset, dblY + yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + yOffset + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中西北图幅名称")

        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX, dblY + yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + yOffset + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中北图幅名称")


        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset, dblY + yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + yOffset + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中东北图幅名称")


        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX - xOffset, dblY)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中西图幅名称")

        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX, dblY)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图号")
        '

        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset, dblY)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY + 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中东图幅名称")


        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX - xOffset, dblY - yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX - 30, dblY - 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中西南图幅名称")


        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX, dblY - yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + 30, dblY - 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中南图幅名称")


        If vScale < 5000 Then
            GetSheetNoFromXY(strSheetNo, vScale, dblX + xOffset, dblY - yOffset)
        Else
            GetNewCodeFromCoordinate(strSheetNo, dblX + xOffset + 30, dblY - 30, vScale)
        End If
        If InStr(strSheetNo, "-") = 0 Then
            strSheetNo = Left(strSheetNo, 3) & " " & Mid(strSheetNo, 4, 1) & " " & Mid(strSheetNo, 5)
        End If

        m_FrameInfo.Add(strSheetNo, "图幅接合表中东南图幅名称")


    End Sub

    Public Function GetSheetNoFromXY(ByRef strSheetNo As String, ByVal lngScale As Long, ByVal dblX As Double, ByVal dblY As Double) As Boolean
        On Error GoTo errH
        Dim bIntX, bIntY As Long '整数部分
        Dim bDecX, bDecY As Double '小数部分
        Dim bResultX As Long
        Dim bResultY As Long
        Dim bNewX As Long
        Dim bNewY As Long
        Dim bIndexX As Long
        Dim bIndexY As Long
        Dim pI, pII, pIII As Long
        bIntX = Fix(dblX / 1000) : bIntY = Fix(dblY / 1000)
        bDecX = dblX / 1000 - bIntX : bDecY = dblY / 1000 - bIntY
        Select Case lngScale
            Case 500
                bNewX = DivRem(bIntX, 2, bResultX)
                bResultX = CLng((bResultX + bDecX) * 1000)
                bNewX = bNewX * 2

                bNewY = DivRem(bIntY, 2, bResultY)
                bResultY = CLng((bResultY + bDecY) * 1000)
                bNewY = bNewY * 2

                bIndexX = bResultX / 250
                bIndexY = bResultY / 250
                If bIndexX < 4 And bIndexY >= 4 Then
                    pI = 1
                ElseIf bIndexX >= 4 And bIndexY >= 4 Then
                    pI = 2
                ElseIf bIndexX < 4 And bIndexY < 4 Then
                    pI = 3
                ElseIf bIndexX >= 4 And bIndexY < 4 Then
                    pI = 4
                End If
                If pI = 1 And bIndexX < 2 And bIndexY >= 6 Then
                    pII = 1
                ElseIf pI = 1 And bIndexX >= 2 And bIndexX < 4 And bIndexY >= 6 Then
                    pII = 2
                ElseIf pI = 1 And bIndexX < 2 And bIndexY >= 4 And bIndexY < 6 Then
                    pII = 3
                ElseIf pI = 1 And bIndexX >= 2 And bIndexX < 4 And bIndexY >= 4 And bIndexY < 6 Then
                    pII = 4
                End If
                If pI = 2 And bIndexX >= 4 And bIndexX < 6 And bIndexY >= 6 Then
                    pII = 1
                ElseIf pI = 2 And bIndexX >= 6 And bIndexY >= 6 Then
                    pII = 2
                ElseIf pI = 2 And bIndexX >= 4 And bIndexX < 6 And bIndexY >= 4 And bIndexY < 6 Then
                    pII = 3
                ElseIf pI = 2 And bIndexX >= 6 And bIndexY >= 4 And bIndexY < 6 Then
                    pII = 4
                End If
                If pI = 3 And bIndexX < 2 And bIndexY >= 2 And bIndexY < 4 Then
                    pII = 1
                ElseIf pI = 3 And bIndexX >= 2 And bIndexX < 4 And bIndexY >= 2 And bIndexY < 4 Then
                    pII = 2
                ElseIf pI = 3 And bIndexX < 2 And bIndexY < 2 Then
                    pII = 3
                ElseIf pI = 3 And bIndexX >= 2 And bIndexX < 4 And bIndexY < 2 Then
                    pII = 4
                End If
                If pI = 4 And bIndexX >= 4 And bIndexX < 6 And bIndexY >= 2 And bIndexY < 4 Then
                    pII = 1
                ElseIf pI = 4 And bIndexX >= 6 And bIndexY >= 2 And bIndexY < 4 Then
                    pII = 2
                ElseIf pI = 4 And bIndexX >= 4 And bIndexX < 6 And bIndexY < 2 Then
                    pII = 3
                ElseIf pI = 4 And bIndexX >= 6 And bIndexY < 2 Then
                    pII = 4
                End If
                If bIndexX = 0 And bIndexY = 1 Then pIII = 1
                If bIndexX = 0 And bIndexY = 3 Then pIII = 1
                If bIndexX = 0 And bIndexY = 5 Then pIII = 1
                If bIndexX = 0 And bIndexY = 7 Then pIII = 1
                If bIndexX = 2 And bIndexY = 1 Then pIII = 1
                If bIndexX = 2 And bIndexY = 3 Then pIII = 1
                If bIndexX = 2 And bIndexY = 5 Then pIII = 1
                If bIndexX = 2 And bIndexY = 7 Then pIII = 1
                If bIndexX = 4 And bIndexY = 1 Then pIII = 1
                If bIndexX = 4 And bIndexY = 3 Then pIII = 1
                If bIndexX = 4 And bIndexY = 5 Then pIII = 1
                If bIndexX = 4 And bIndexY = 7 Then pIII = 1
                If bIndexX = 6 And bIndexY = 1 Then pIII = 1
                If bIndexX = 6 And bIndexY = 3 Then pIII = 1
                If bIndexX = 6 And bIndexY = 5 Then pIII = 1
                If bIndexX = 6 And bIndexY = 7 Then pIII = 1

                If bIndexX = 1 And bIndexY = 1 Then pIII = 2
                If bIndexX = 1 And bIndexY = 3 Then pIII = 2
                If bIndexX = 1 And bIndexY = 5 Then pIII = 2
                If bIndexX = 1 And bIndexY = 7 Then pIII = 2
                If bIndexX = 3 And bIndexY = 1 Then pIII = 2
                If bIndexX = 3 And bIndexY = 3 Then pIII = 2
                If bIndexX = 3 And bIndexY = 5 Then pIII = 2
                If bIndexX = 3 And bIndexY = 7 Then pIII = 2
                If bIndexX = 5 And bIndexY = 1 Then pIII = 2
                If bIndexX = 5 And bIndexY = 3 Then pIII = 2
                If bIndexX = 5 And bIndexY = 5 Then pIII = 2
                If bIndexX = 5 And bIndexY = 7 Then pIII = 2
                If bIndexX = 7 And bIndexY = 1 Then pIII = 2
                If bIndexX = 7 And bIndexY = 3 Then pIII = 2
                If bIndexX = 7 And bIndexY = 5 Then pIII = 2
                If bIndexX = 7 And bIndexY = 7 Then pIII = 2

                If bIndexX = 0 And bIndexY = 0 Then pIII = 3
                If bIndexX = 0 And bIndexY = 2 Then pIII = 3
                If bIndexX = 0 And bIndexY = 4 Then pIII = 3
                If bIndexX = 0 And bIndexY = 6 Then pIII = 3
                If bIndexX = 2 And bIndexY = 0 Then pIII = 3
                If bIndexX = 2 And bIndexY = 2 Then pIII = 3
                If bIndexX = 2 And bIndexY = 4 Then pIII = 3
                If bIndexX = 2 And bIndexY = 6 Then pIII = 3
                If bIndexX = 4 And bIndexY = 0 Then pIII = 3
                If bIndexX = 4 And bIndexY = 2 Then pIII = 3
                If bIndexX = 4 And bIndexY = 4 Then pIII = 3
                If bIndexX = 4 And bIndexY = 6 Then pIII = 3
                If bIndexX = 6 And bIndexY = 0 Then pIII = 3
                If bIndexX = 6 And bIndexY = 2 Then pIII = 3
                If bIndexX = 6 And bIndexY = 4 Then pIII = 3
                If bIndexX = 6 And bIndexY = 6 Then pIII = 3

                If bIndexX = 1 And bIndexY = 0 Then pIII = 4
                If bIndexX = 1 And bIndexY = 2 Then pIII = 4
                If bIndexX = 1 And bIndexY = 4 Then pIII = 4
                If bIndexX = 1 And bIndexY = 6 Then pIII = 4
                If bIndexX = 3 And bIndexY = 0 Then pIII = 4
                If bIndexX = 3 And bIndexY = 2 Then pIII = 4
                If bIndexX = 3 And bIndexY = 4 Then pIII = 4
                If bIndexX = 3 And bIndexY = 6 Then pIII = 4
                If bIndexX = 5 And bIndexY = 0 Then pIII = 4
                If bIndexX = 5 And bIndexY = 2 Then pIII = 4
                If bIndexX = 5 And bIndexY = 4 Then pIII = 4
                If bIndexX = 5 And bIndexY = 6 Then pIII = 4
                If bIndexX = 7 And bIndexY = 0 Then pIII = 4
                If bIndexX = 7 And bIndexY = 2 Then pIII = 4
                If bIndexX = 7 And bIndexY = 4 Then pIII = 4
                If bIndexX = 7 And bIndexY = 6 Then pIII = 4

                'strSheetNo = bNewX & "-" & bNewY & "-" & pI & "-" & pII & "-" & pIII
                strSheetNo = bNewY & "-" & bNewX & "-" & pI & "-" & pII & "-" & pIII    'liujiang 20080425 重庆图号XY坐标是反的
                Dim pCls As New MapIDManager
                strSheetNo = pCls.GetMapNameString(strSheetNo)
            Case 1000
                bNewX = DivRem(bIntX, 2, bResultX)
                bResultX = CLng((bResultX + bDecX) * 1000)
                bNewX = bNewX * 2

                bNewY = DivRem(bIntY, 2, bResultY)
                bResultY = CLng((bResultY + bDecY) * 1000)
                bNewY = bNewY * 2

                bIndexX = bResultX / 500
                bIndexY = bResultY / 500
                If bIndexX < 2 And bIndexY >= 2 Then
                    pI = 1
                ElseIf bIndexX >= 2 And bIndexY >= 2 Then
                    pI = 2
                ElseIf bIndexX < 2 And bIndexY < 2 Then
                    pI = 3
                ElseIf bIndexX >= 2 And bIndexY < 2 Then
                    pI = 4
                End If
                If pI = 1 And bIndexX = 0 And bIndexY = 3 Then pII = 1
                If pI = 2 And bIndexX = 2 And bIndexY = 3 Then pII = 1
                If pI = 3 And bIndexX = 0 And bIndexY = 1 Then pII = 1
                If pI = 4 And bIndexX = 2 And bIndexY = 1 Then pII = 1

                If pI = 1 And bIndexX = 1 And bIndexY = 3 Then pII = 2
                If pI = 2 And bIndexX = 3 And bIndexY = 3 Then pII = 2
                If pI = 3 And bIndexX = 1 And bIndexY = 1 Then pII = 2
                If pI = 4 And bIndexX = 3 And bIndexY = 1 Then pII = 2

                If pI = 1 And bIndexX = 0 And bIndexY = 2 Then pII = 3
                If pI = 2 And bIndexX = 2 And bIndexY = 2 Then pII = 3
                If pI = 3 And bIndexX = 0 And bIndexY = 0 Then pII = 3
                If pI = 4 And bIndexX = 2 And bIndexY = 0 Then pII = 3

                If pI = 1 And bIndexX = 1 And bIndexY = 2 Then pII = 4
                If pI = 2 And bIndexX = 3 And bIndexY = 2 Then pII = 4
                If pI = 3 And bIndexX = 1 And bIndexY = 0 Then pII = 4
                If pI = 4 And bIndexX = 3 And bIndexY = 0 Then pII = 4

                strSheetNo = bNewX & "-" & bNewY & "-" & pI & "-" & pII
                Dim pCls As New MapIDManager
                strSheetNo = pCls.GetMapNameString(strSheetNo)
            Case 2000
                bNewX = DivRem(bIntX, 2, bResultX)
                bResultX = CLng((bResultX + bDecX) * 1000)
                bNewX = bNewX * 2

                bNewY = DivRem(bIntY, 2, bResultY)
                bResultY = CLng((bResultY + bDecY) * 1000)
                bNewY = bNewY * 2

                bIndexX = bResultX / 250
                bIndexY = bResultY / 250
                If bIndexX < 4 And bIndexY >= 4 Then
                    pI = 1
                ElseIf bIndexX >= 4 And bIndexY >= 4 Then
                    pI = 2
                ElseIf bIndexX < 4 And bIndexY < 4 Then
                    pI = 3
                ElseIf bIndexX >= 4 And bIndexY < 4 Then
                    pI = 4
                End If

                strSheetNo = bNewX & "-" & bNewY & "-" & pI
                Dim pCls As New MapIDManager
                strSheetNo = pCls.GetMapNameString(strSheetNo)
            Case Else

        End Select
        GetSheetNoFromXY = True
        Exit Function
errH:
        GetSheetNoFromXY = False
        strSheetNo = ""
    End Function

    Public Function GetNewCodeFromCoordinate(ByRef strNewMapCode As String, ByVal x As Long, ByVal y As Long, ByVal vScale As Long) As Boolean
        On Error GoTo ERRHANDLER
        '根据坐标（经纬度）和比例尺求新图幅号
        Dim b As String       '纬度编号
        Dim l As Integer      '经度序号
        Dim r As Integer      '行数
        Dim c As Integer      '列数
        Dim th As String       '图幅比例尺编号

        Dim YInt As Integer      '纬度序号

        Dim XPlus As Double         '经度差
        Dim YPlus As Double         '纬度差

        GetNewCodeFromCoordinate = True

        strNewMapCode = ""

        '求纬度序号
        YInt = Int(y / 4 / 3600) + 1
        '求纬度编号
        b = Chr(YInt + 64)
        '求经度序号
        l = Int(x / 6 / 3600) + 31

        '图幅比例尺编号，经度差，纬度差
        Select Case vScale
            Case 500000
                th = "B"
                XPlus = 3 * 3600
                YPlus = 2 * 3600
            Case 250000
                th = "C"
                XPlus = 3 / 2 * 3600
                YPlus = 1 * 3600
            Case 100000
                th = "D"
                XPlus = 1 / 2 * 3600
                YPlus = 1 / 3 * 3600
            Case 50000
                th = "E"
                XPlus = 1 / 4 * 3600
                YPlus = 1 / 6 * 3600
            Case 25000
                th = "F"
                XPlus = 1 / 8 * 3600
                YPlus = 1 / 12 * 3600
            Case 10000
                th = "G"
                XPlus = 1 / 16 * 3600
                YPlus = 1 / 24 * 3600
            Case 5000
                th = "H"
                XPlus = 1 / 32 * 3600
                YPlus = 1 / 48 * 3600
            Case Else
                GetNewCodeFromCoordinate = False
                Exit Function
        End Select

        '行数
        r = 4 / (YPlus / 3600) - Int(((y / 3600) - Int((y / 3600) / 4) * 4) / (YPlus / 3600))
        '列数
        c = Int(((x / 3600) - Int((x / 3600) / 6) * 6) / (XPlus / 3600)) + 1

        '新图号=纬度编号+经度序号+图幅比例尺编号+行数+列数
        strNewMapCode = b & l & th & Format(r, "00#") & Format(c, "00#")

        Exit Function

ERRHANDLER:
        GetNewCodeFromCoordinate = False
    End Function
    '根据经度获得投影文件  传入参数是经度的秒单位 返回的是西安80 3度带的投影 
    Public Function GetSpatialByX(ByVal dblX As Double) As ISpatialReference
        Dim pProjectSpatial As IProjectedCoordinateSystem = Nothing
        Dim pSpatialRef As ISpatialReferenceFactory2 = Nothing
        GetSpatialByX = Nothing
        On Error GoTo ErrH
        pSpatialRef = New SpatialReferenceEnvironment

        '先根据图幅号进行投影
        Select Case dblX
            Case Is > 437400
                pProjectSpatial = pSpatialRef.CreateProjectedCoordinateSystem(esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_117E)
            Case Is > 426960
                pProjectSpatial = pSpatialRef.CreateProjectedCoordinateSystem(esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_120E)
            Case Is > 415800
                pProjectSpatial = pSpatialRef.CreateProjectedCoordinateSystem(esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_123E)
        End Select

        If Not pProjectSpatial Is Nothing Then
            GetSpatialByX = pProjectSpatial
        End If
        Return GetSpatialByX
ErrH:

    End Function





End Module
