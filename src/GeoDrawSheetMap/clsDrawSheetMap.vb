Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Controls
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.GeoDatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem


Public Class clsDrawSheetMap
    Private m_vXoffset As Collection                       '记录模板中点与现有点的坐标偏移量X
    Private m_vYoffset As Collection                       '记录模板中点与现有点的坐标偏移量Y
    Public vScale As Long                                 '比例尺
    Public pGeometry As IGeometry                         '传进来的图幅形状
    Public vPageLayoutControl As IPageLayoutControl       '传进来的制图对象
    Public m_strSheetNo As String                         '图幅号
    Public m_intPntCount As Integer                       '要插入的点数
    Public m_pPrjCoor As ISpatialReference      '采用的投影坐标系统
    Public type_ZT As Integer                  '专题类型0地形图1二调现状图2规划图20110914 yjl add

    Private m_clsGetGeoInfo As clsGetGeoInfo              '计算图幅四个顶点的位置信息
    Private m_pPaperPointCol As IPointCollection           '四个角点的图纸点信息

    Private m_pMapInfo As MapInfo()                         '得到尽可能多的图幅信息


    '临时
    Public g_pRecElement As IElement         '参考矩形框
    Public g_MapFrameEle As IElement         '数据框


    '主要包括两部分  分为加载模板并向模板中添加要素和移动要素两部分

    'Public Sub DrawSheetMap()
    '    Try
    '        Dim vType As String

    '        ''On Error GoTo ErrH
    '        vType = CStr(vScale)

    '        '首先改变下MAP的空间参考
    '        vPageLayoutControl.ActiveView.FocusMap.SpatialReference = m_pPrjCoor

    '        '计算四个点的位置信息
    '        'm_clsGetGeoInfo = New clsGetGeoInfo
    '        'm_clsGetGeoInfo.m_lngMapScale = vScale
    '        'm_clsGetGeoInfo.m_strMapNO = m_strSheetNo
    '        'm_clsGetGeoInfo.m_intInsertCount = m_intPntCount
    '        'Dim tempspataialreference As IProjectedCoordinateSystem = New ProjectedCoordinateSystem()
    '        'Try
    '        '    tempspataialreference = m_pPrjCoor
    '        'Catch ex As Exception
    '        '    Dim factroy As ISpatialReferenceFactory = New SpatialReferenceEnvironment()
    '        '    tempspataialreference = factroy.CreateProjectedCoordinateSystem(102018)

    '        'End Try
    '        'm_clsGetGeoInfo.m_pPrjCoor = tempspataialreference
    '        'm_clsGetGeoInfo.ComputerAllGeoInfo()

    '        pGeometry = GISServices.Sheet.LargeScaleMapNO.GetPolygonFromLargeMapNO(m_strSheetNo, m_pPrjCoor)

    '        '改变地图的尺寸大小
    '        If vScale < 5000 Then
    '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 50, 50)
    '            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
    '            m_vXoffset = New Collection
    '            m_vYoffset = New Collection
    '            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.X - g_pRecElement.Geometry.Envelope.LowerLeft.X)
    '            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.X - g_pRecElement.Geometry.Envelope.LowerRight.X)
    '            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.X - g_pRecElement.Geometry.Envelope.UpperRight.X)
    '            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.X - g_pRecElement.Geometry.Envelope.UpperLeft.X)
    '            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.Y - g_pRecElement.Geometry.Envelope.LowerLeft.Y)
    '            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.Y - g_pRecElement.Geometry.Envelope.LowerRight.Y)
    '            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.Y - g_pRecElement.Geometry.Envelope.UpperRight.Y)
    '            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.Y - g_pRecElement.Geometry.Envelope.UpperLeft.Y)
    '            Call MoveElementInBigScalePage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
    '        Else
    '            Select Case vType
    '                Case "5000"
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
    '                Case "10000"
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
    '                Case "25000"
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
    '                Case "50000"
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
    '                Case "250000"
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
    '                Case Else
    '                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 80, 80)
    '            End Select
    '            '添加模板 并画公里格网 格网标示
    '            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
    '            '移动制图元素的位置
    '            Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
    '        End If


    '        'Exit Sub
    '        'ErrH:
    '    Catch ex As Exception

    '    End Try


    'End Sub
    Public Sub DrawSheetMap()
        Dim vType As String

        'On Error GoTo ErrH
        vType = CStr(vScale)

        '首先改变下MAP的空间参考
        vPageLayoutControl.ActiveView.FocusMap.SpatialReference = m_pPrjCoor

        '改变地图的尺寸大小
        '计算四个点的位置信息
        m_clsGetGeoInfo = New clsGetGeoInfo
        m_clsGetGeoInfo.m_lngMapScale = vScale
        m_clsGetGeoInfo.m_strMapNO = m_strSheetNo
        m_clsGetGeoInfo.m_intInsertCount = m_intPntCount
        m_clsGetGeoInfo.m_pPrjCoor = m_pPrjCoor
        m_clsGetGeoInfo.ComputerAllGeoInfo()

        pGeometry = m_clsGetGeoInfo.m_pSheetMapGeometry

        If vScale < 5000 Then
          
            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 50, 50)
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            m_vXoffset = New Collection
            m_vYoffset = New Collection
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.X - g_pRecElement.Geometry.Envelope.LowerLeft.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.X - g_pRecElement.Geometry.Envelope.LowerRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.X - g_pRecElement.Geometry.Envelope.UpperRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.X - g_pRecElement.Geometry.Envelope.UpperLeft.X)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.Y - g_pRecElement.Geometry.Envelope.LowerLeft.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.Y - g_pRecElement.Geometry.Envelope.LowerRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.Y - g_pRecElement.Geometry.Envelope.UpperRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.Y - g_pRecElement.Geometry.Envelope.UpperLeft.Y)
            Call MoveElementInBigScalePage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        Else
            Select Case vType
                Case "5000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
                Case "10000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
                Case "25000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
                Case "50000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
                Case "250000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
                Case Else
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 80, 80)
            End Select
            '添加模板 并画公里格网 格网标示
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            '移动制图元素的位置
            If type_ZT = 0 Then '地形图
                Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
            ElseIf type_ZT = 1 Then '森林资源现状分幅
                Call MoveElementInPageForTDLY(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
            End If
            Dim noc As New Collection

            CalculateFigure(Me.m_strSheetNo, Me.vScale, noc, Me.vPageLayoutControl.PageLayout, pGeometry)
        End If



        ''改变地图的尺寸大小
        'If vScale < 5000 Then
        '    Call SetPageLayoutSize(vPageLayoutControl, 60, 60, 50, 50)
        'Else
        '    Select Case vType
        '        Case "5000"
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
        '        Case "10000"
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
        '        Case "25000"
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
        '        Case "50000"
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
        '        Case "250000"
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
        '        Case Else
        '            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 80, 80)
        '    End Select

        'End If

        ''添加模板 并画公里格网 格网标示
        'Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
        ''移动制图元素的位置
        'Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        ''Exit Sub
        ''ErrH:

    End Sub

    Public Sub DrawSheetMap(ByVal pMapInfo As Object)
        Dim vType As String

        '2008.04.17 t添加......
        m_pMapInfo = pMapInfo

        'On Error GoTo ErrH
        vType = CStr(vScale)

        '首先改变下MAP的空间参考
        vPageLayoutControl.ActiveView.FocusMap.SpatialReference = m_pPrjCoor

        '计算四个点的位置信息
        m_clsGetGeoInfo = New clsGetGeoInfo
        m_clsGetGeoInfo.m_lngMapScale = vScale
        m_clsGetGeoInfo.m_strMapNO = m_strSheetNo
        m_clsGetGeoInfo.m_intInsertCount = m_intPntCount
        m_clsGetGeoInfo.m_pPrjCoor = m_pPrjCoor
        m_clsGetGeoInfo.ComputerAllGeoInfo()

        pGeometry = m_clsGetGeoInfo.m_pSheetMapGeometry

        'For i As Integer = 0 To m_pMapInfo.GetLength(0) - 1
        '    If m_pMapInfo(i).Keys = m_strSheetNo Then
        '        pGeometry = m_pMapInfo(i).pGeometry
        '        Exit For
        '    End If
        'Next
        'pGeometry.Envelope.XMax

        'm_clsGetGeoInfo.m_pSheetMapGeometry

        '改变地图的尺寸大小
        If vScale < 5000 Then
            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 50, 50)
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            m_vXoffset = New Collection
            m_vYoffset = New Collection
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.X - g_pRecElement.Geometry.Envelope.LowerLeft.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.X - g_pRecElement.Geometry.Envelope.LowerRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.X - g_pRecElement.Geometry.Envelope.UpperRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.X - g_pRecElement.Geometry.Envelope.UpperLeft.X)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.Y - g_pRecElement.Geometry.Envelope.LowerLeft.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.Y - g_pRecElement.Geometry.Envelope.LowerRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.Y - g_pRecElement.Geometry.Envelope.UpperRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.Y - g_pRecElement.Geometry.Envelope.UpperLeft.Y)
            Call MoveElementInBigScalePage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        Else
            Select Case vType
                Case "5000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
                Case "10000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
                Case "25000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
                Case "50000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
                Case "250000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
                Case Else
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 80, 80)
            End Select
            '添加模板 并画公里格网 格网标示
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            '移动制图元素的位置
            Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        End If


        'Exit Sub
        'ErrH:

    End Sub



    '主要包括两部分  分为加载模板并向模板中添加要素和移动要素两部分
    Public Sub DrawSheetMap(ByVal pPageWidth As Double, ByVal pPageHeight As Double, ByVal pMapWidth As Double, ByVal pMapHeight As Double)
        Dim vType As String

        'On Error GoTo ErrH
        vType = CStr(vScale)

        '首先改变下MAP的空间参考
        vPageLayoutControl.ActiveView.FocusMap.SpatialReference = m_pPrjCoor

        '计算四个点的位置信息
        m_clsGetGeoInfo = New clsGetGeoInfo
        m_clsGetGeoInfo.m_lngMapScale = vScale
        m_clsGetGeoInfo.m_strMapNO = m_strSheetNo
        m_clsGetGeoInfo.m_intInsertCount = m_intPntCount
        m_clsGetGeoInfo.m_pPrjCoor = m_pPrjCoor
        m_clsGetGeoInfo.ComputerAllGeoInfo()

        pGeometry = m_clsGetGeoInfo.m_pSheetMapGeometry

        '改变地图的尺寸大小
        If vScale < 5000 Then
            Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pMapWidth, pMapHeight)
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            m_vXoffset = New Collection
            m_vYoffset = New Collection
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.X - g_pRecElement.Geometry.Envelope.LowerLeft.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.X - g_pRecElement.Geometry.Envelope.LowerRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.X - g_pRecElement.Geometry.Envelope.UpperRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.X - g_pRecElement.Geometry.Envelope.UpperLeft.X)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.Y - g_pRecElement.Geometry.Envelope.LowerLeft.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.Y - g_pRecElement.Geometry.Envelope.LowerRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.Y - g_pRecElement.Geometry.Envelope.UpperRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.Y - g_pRecElement.Geometry.Envelope.UpperLeft.Y)
            Call MoveElementInBigScalePage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        Else
            Select Case vType
                Case "5000"
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
                Case "10000"
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
                Case "25000"
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
                Case "50000"
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
                Case "250000"
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
                Case Else
                    Call SetPageLayoutSize(vPageLayoutControl, pPageWidth, pPageHeight, pMapWidth, pMapHeight)
            End Select
            '添加模板 并画公里格网 格网标示
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            '移动制图元素的位置
            Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        End If


        'Exit Sub
        'ErrH:

    End Sub




    Public Sub DrawSheetMapByMultiMap(ByRef pSourceGeo As IGeometry)
        Dim vType As String

        'On Error GoTo ErrH
        vType = CStr(vScale)

        '首先改变下MAP的空间参考
        'vPageLayoutControl.ActiveView.FocusMap.SpatialReference = m_pPrjCoor

        '计算四个点的位置信息
        m_clsGetGeoInfo = New clsGetGeoInfo
        m_clsGetGeoInfo.m_lngMapScale = vScale
        m_clsGetGeoInfo.m_strMapNO = m_strSheetNo
        m_clsGetGeoInfo.m_intInsertCount = m_intPntCount
        m_clsGetGeoInfo.m_pPrjCoor = m_pPrjCoor
        m_clsGetGeoInfo.ComputerAllGeoInfo()

        pGeometry = m_clsGetGeoInfo.m_pSheetMapGeometry

        '2008.04.18
        pSourceGeo = pGeometry

        '改变地图的尺寸大小
        If vScale < 5000 Then
            Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 50, 50)
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            m_vXoffset = New Collection
            m_vYoffset = New Collection
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.X - g_pRecElement.Geometry.Envelope.LowerLeft.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.X - g_pRecElement.Geometry.Envelope.LowerRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.X - g_pRecElement.Geometry.Envelope.UpperRight.X)
            m_vXoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.X - g_pRecElement.Geometry.Envelope.UpperLeft.X)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerLeft.Y - g_pRecElement.Geometry.Envelope.LowerLeft.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.LowerRight.Y - g_pRecElement.Geometry.Envelope.LowerRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperRight.Y - g_pRecElement.Geometry.Envelope.UpperRight.Y)
            m_vYoffset.Add(g_MapFrameEle.Geometry.Envelope.UpperLeft.Y - g_pRecElement.Geometry.Envelope.UpperLeft.Y)
            Call MoveElementInBigScalePage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        Else
            Select Case vType
                Case "5000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 50, pGeometry.Envelope.Height / 50)
                Case "10000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 100, pGeometry.Envelope.Height / 100)
                Case "25000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 250, pGeometry.Envelope.Height / 250)
                Case "50000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 500, pGeometry.Envelope.Height / 500)
                Case "250000"
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, pGeometry.Envelope.Width / 2500, pGeometry.Envelope.Height / 2500)
                Case Else
                    Call SetPageLayoutSize(vPageLayoutControl, 80, 80, 80, 80)
            End Select
            '添加模板 并画公里格网 格网标示
            Call OpenTemplateElement(vType, pGeometry, vPageLayoutControl)
            '移动制图元素的位置

            Call MoveElementInPage(vPageLayoutControl.PageLayout, m_vXoffset, m_vYoffset)
        End If


        'Exit Sub
        'ErrH:

    End Sub


    '移动要素的位置
    Private Sub MoveElementInPage(ByVal pPagelayout As IPageLayout, ByVal vXoffset As Collection, ByVal vYoffset As Collection)
        Dim pTxtElement As ITextElement
        Dim pGraphicsContainer As IGraphicsContainer
        pGraphicsContainer = New PageLayout
        pGraphicsContainer = pPagelayout
        Dim pElement As IElement
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next

        While Not pElement Is Nothing
            If TypeOf pElement Is ITextElement Then
                pTxtElement = pElement
                Select Case pTxtElement.Text

                    Case "中华人民共和国基本比例尺地形图"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左上角图幅
                    Case "G50G004004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '正上方图幅
                    Case "G50G004005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '右上角图幅
                    Case "G50G004006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '左边图幅
                    Case "G50G005004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "图号1"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "图号2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "图号3"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "图号4"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        'Case "图号3"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(4) + vXoffset(1)) / 2, (vYoffset(4) + vYoffset(1)) / 2)
                        '    End If
                        'Case "图号4"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        '    End If
                        'Case "图号5"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(2) + vXoffset(3)) / 2, (vYoffset(2) + vYoffset(3)) / 2)
                        '    End If
                        'Case "图号6"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If
                        '当前图幅
                    Case "G50G005005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                    Case "图名1"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "图名2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "图名3"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "图名4"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右边图幅
                    Case "G50G005006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '左下角图幅
                    Case "G50G006004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '正下方图幅
                    Case "G50G006005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '右下角图幅
                    Case "G50G006006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If

                        '当前比例尺
                    Case "比例尺"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        End If
                    Case "比例尺2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "图名"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                        'Case "地名"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If

                    Case "密级"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "附注："
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "坐标系"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "测绘机关全称"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                        'Case "检查员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "绘图员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "测量员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                    Case "1985国家高程基准，"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "等高距为1米。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "1995年5月XXX测图。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "1996年版图式。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If

                        '因为图廓角坐标没有让用户编辑，所以就直接算出来了，和上面数据库取的方法不同
                    Case "553.75左下"
                        'FieldValue = "西南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "385.50左下"
                        ' FieldValue = "西南图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                        '左上角y,x坐标
                    Case "554.00左上"
                        'FieldValue = "西北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "385.50左上"
                        'FieldValue = "西北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右上角坐标
                    Case "554.00右上"
                        ' FieldValue = "东北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "385.75右上"
                        'FieldValue = "东北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '右下角坐标
                    Case "553.75右下"
                        ' FieldValue = "东南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "385.75右下"
                        'FieldValue = "东南图廓角点X坐标"
                        'FigureChar pSheetEnvelope.xmax, vScaleType, pTxtElement
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case Else
                        Dim pElePro As IElementProperties
                        pElePro = pElement
                        If pElePro.Name.Contains("左下角标注") Then
                            If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                                Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                            End If
                        End If
                        If pElePro.Name.Contains("右下角标注") Then
                            If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                                Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                            End If
                        End If

                End Select
            ElseIf TypeOf pElement Is IGroupElement Then
                Dim pGroupElement As IGroupElement
                Dim pElePro As IElementProperties
                pElePro = pElement
                pGroupElement = pElement
                If pElePro.Name = "接图表" Or pGroupElement.ElementCount = 2 Then '现在规定接合表的ELEMENT个数 还没有找到其他的区别方法
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                    End If
                Else
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            ElseIf TypeOf pElement Is IMapSurroundFrame Then
                Dim pMapSurroundFrame As IMapSurroundFrame
                pMapSurroundFrame = pElement
                If TypeOf pMapSurroundFrame.Object Is IScaleBar Then
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            End If
            pElement = pGraphicsContainer.Next
        End While
    End Sub
    '移动要素的位置yjl 20110916 add for 森林资源现状分幅
    Private Sub MoveElementInPageForTDLY(ByVal pPagelayout As IPageLayout, ByVal vXoffset As Collection, ByVal vYoffset As Collection)
        Dim pTxtElement As ITextElement
        Dim pGraphicsContainer As IGraphicsContainer
        pGraphicsContainer = New PageLayout
        pGraphicsContainer = pPagelayout
        Dim pElement As IElement
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next

        While Not pElement Is Nothing
            If TypeOf pElement Is ITextElement Then
                pTxtElement = pElement
                Select Case pTxtElement.Text

                    Case "中华人民共和国基本比例尺地形图"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左上角图幅
                    Case "G50G004004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '正上方图幅
                    Case "G50G004005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右上角图幅
                    Case "G50G004006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左边图幅
                    Case "G50G005004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "图号1"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "图号2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "图号3"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "图号4"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        'Case "图号3"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(4) + vXoffset(1)) / 2, (vYoffset(4) + vYoffset(1)) / 2)
                        '    End If
                        'Case "图号4"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        '    End If
                        'Case "图号5"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(2) + vXoffset(3)) / 2, (vYoffset(2) + vYoffset(3)) / 2)
                        '    End If
                        'Case "图号6"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If
                        '当前图幅
                    Case "G50G005005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                    Case "图名1"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "图名2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "图名3"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "图名4"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右边图幅
                    Case "G50G005006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左下角图幅
                    Case "G50G006004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '正下方图幅
                    Case "G50G006005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右下角图幅
                    Case "G50G006006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If

                        '当前比例尺
                    Case "比例尺"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        End If
                    Case "比例尺2"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "图名"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                        'Case "地名"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If

                    Case "密级"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "附注："
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "坐标系"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "测绘机关全称"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                        'Case "检查员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "绘图员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "测量员"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                    Case "1985国家高程基准，"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "等高距为1米。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "1995年5月XXX测图。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "1996年版图式。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If

                        '因为图廓角坐标没有让用户编辑，所以就直接算出来了，和上面数据库取的方法不同
                    Case "553.75左下"
                        'FieldValue = "西南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "385.50左下"
                        ' FieldValue = "西南图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                        '左上角y,x坐标
                    Case "554.00左上"
                        'FieldValue = "西北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "385.50左上"
                        'FieldValue = "西北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右上角坐标
                    Case "554.00右上"
                        ' FieldValue = "东北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "385.75右上"
                        'FieldValue = "东北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '右下角坐标
                    Case "553.75右下"
                        ' FieldValue = "东南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "385.75右下"
                        'FieldValue = "东南图廓角点X坐标"
                        'FigureChar pSheetEnvelope.xmax, vScaleType, pTxtElement
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                End Select
            ElseIf TypeOf pElement Is IGroupElement Then
                Dim pGroupElement As IGroupElement
                Dim pElePro As IElementProperties
                pElePro = pElement
                pGroupElement = pElement
                If pElePro.Name = "接图表" Or pGroupElement.ElementCount = 2 Then '现在规定接合表的ELEMENT个数 还没有找到其他的区别方法
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                    End If
                Else
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            ElseIf TypeOf pElement Is IMapSurroundFrame Then
                Dim pMapSurroundFrame As IMapSurroundFrame
                pMapSurroundFrame = pElement
                If TypeOf pMapSurroundFrame.Object Is IScaleBar Then
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            End If
            pElement = pGraphicsContainer.Next
        End While
    End Sub
    Private Sub MoveElementInBigScalePage(ByVal pPagelayout As IPageLayout, ByVal vXoffset As Collection, ByVal vYoffset As Collection)
        Dim pTxtElement As ITextElement
        Dim pGraphicsContainer As IGraphicsContainer
        pGraphicsContainer = New PageLayout
        pGraphicsContainer = pPagelayout
        Dim pElement As IElement
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next

        While Not pElement Is Nothing
            If TypeOf pElement Is ITextElement Then
                pTxtElement = pElement
                Select Case pTxtElement.Text

                    Case "中华人民共和国比例尺地形图"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左上角图幅
                    Case "G50G004004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '正上方图幅
                    Case "G50G004005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右上角图幅
                    Case "G50G004006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左边图幅
                    Case "G50G005004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        'Case "图号1"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        '    End If
                        'Case "图号2"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "图号3"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        '    End If
                        'Case "图号4"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        '    End If
                        'Case "图号3"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(4) + vXoffset(1)) / 2, (vYoffset(4) + vYoffset(1)) / 2)
                        '    End If
                        'Case "图号4"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        '    End If
                        'Case "图号5"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(2) + vXoffset(3)) / 2, (vYoffset(2) + vYoffset(3)) / 2)
                        '    End If
                        'Case "图号6"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If
                        '当前图幅
                    Case "G50G005005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                        'Case "图名1"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        '    End If
                        'Case "图名2"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        '    End If
                        'Case "图名3"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        '    End If
                        'Case "图名4"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        '    End If
                        '右边图幅
                    Case "G50G005006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '左下角图幅
                    Case "G50G006004"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '正下方图幅
                    Case "G50G006005"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右下角图幅
                    Case "G50G006006"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If

                        '当前比例尺
                    Case "比例尺"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                        End If
                        'Case "比例尺2"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        '    End If
                    Case "图名"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        End If
                        'Case "地名"
                        '    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        '        Call MovePageLyrOutlbl(pElement, (vXoffset(3) + vXoffset(4)) / 2, (vYoffset(3) + vYoffset(4)) / 2)
                        '    End If

                    Case "密级"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "附注："
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "坐标系"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "测绘机关全称"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "检查员"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "绘图员"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "测量员"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "1985国家高程基准，"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "等高距为1米。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "1995年5月XXX测图。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "1996年版图式。"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If

                        '因为图廓角坐标没有让用户编辑，所以就直接算出来了，和上面数据库取的方法不同
                    Case "553.75左下"
                        'FieldValue = "西南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                    Case "385.50左下"
                        ' FieldValue = "西南图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(1), vYoffset(1))
                        End If
                        '左上角y,x坐标
                    Case "554.00左上"
                        'FieldValue = "西北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                    Case "385.50左上"
                        'FieldValue = "西北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                        End If
                        '右上角坐标
                    Case "554.00右上"
                        ' FieldValue = "东北图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                    Case "385.75右上"
                        'FieldValue = "东北图廓角点X坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(3), vYoffset(3))
                        End If
                        '右下角坐标
                    Case "553.75右下"
                        ' FieldValue = "东南图廓角点Y坐标"
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case "385.75右下"
                        'FieldValue = "东南图廓角点X坐标"
                        'FigureChar pSheetEnvelope.xmax, vScaleType, pTxtElement
                        If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                            Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                        End If
                    Case Else
                        Dim pElePro As IElementProperties
                        pElePro = pElement
                        If pElePro.Name.Contains("左下角标注1") Then
                            If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                                Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2) + 1.190625)
                            End If
                        End If
                        If pElePro.Name.Contains("左下角标注2") Then
                            If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                                Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                            End If
                        End If
                        If pElePro.Name.Contains("右下角标注") Then
                            If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                                Call MovePageLyrOutlbl(pElement, vXoffset(2), vYoffset(2))
                            End If
                        End If
                End Select
            ElseIf TypeOf pElement Is IGroupElement Then
                Dim pGroupElement As IGroupElement
                pGroupElement = pElement
                If pGroupElement.ElementCount = 2 Then '现在规定接合表的ELEMENT个数 还没有找到其他的区别方法
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, vXoffset(4), vYoffset(4))
                    End If
                Else
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            ElseIf TypeOf pElement Is IMapSurroundFrame Then
                Dim pMapSurroundFrame As IMapSurroundFrame
                pMapSurroundFrame = pElement
                If TypeOf pMapSurroundFrame.Object Is IScaleBar Then
                    If (Not vXoffset Is Nothing) And (Not vYoffset Is Nothing) Then
                        Call MovePageLyrOutlbl(pElement, (vXoffset(1) + vXoffset(2)) / 2, (vYoffset(1) + vYoffset(2)) / 2)
                    End If
                End If
            End If
            pElement = pGraphicsContainer.Next
        End While
    End Sub

    Private Sub OpenTemplateElement(ByVal vType As String, ByVal pGeometry As IGeometry, ByVal vPagelayout As IPageLayoutControl)
        Dim pMapDoc As IMapDocument
        Dim pElement As IElement
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pLayOutGrapContainer As IGraphicsContainer

        pMapDoc = New MapDocument
        If type_ZT = 0 Then
            Select Case vType
                Case "500", "1000", "2000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\500Template.mxd")
                Case "5000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\5000Template.mxd")
                Case "10000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\10000Template.mxd")
                Case "25000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\10000Template.mxd")
                Case "50000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\50000Template.mxd")
                Case "250000"
                    pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\250000Template.mxd")
            End Select
        ElseIf type_ZT = 1 Then '20110914 yjl add for tdly
            
            pMapDoc.Open(System.Windows.Forms.Application.StartupPath & "\..\Template\TDLY10000Template.mxd")
         
        End If

        '首先删除原有Element
        pLayOutGrapContainer = vPagelayout.PageLayout
        pLayOutGrapContainer.Reset()
        pElement = pLayOutGrapContainer.Next
        While Not pElement Is Nothing
            If TypeOf pElement Is IMapFrame Then
                g_MapFrameEle = pElement
            End If
            If Not (TypeOf pElement Is IMapFrame) Then  'And Not (TypeOf pElement Is IFrameElement)
                pLayOutGrapContainer.DeleteElement(pElement)
                pLayOutGrapContainer.Reset()  '//删除了之后要reset，不然删除不完
            End If
            pElement = pLayOutGrapContainer.Next
        End While

        '添加制图模板中的Element
        pGraphicsContainer = pMapDoc.PageLayout
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next
        While Not pElement Is Nothing
            'If CLng(vType) < 5000 Then
            '    If Not (TypeOf pElement Is ITextElement) Then   'And Not (TypeOf pElement Is IFrameElement)
            '        pLayOutGrapContainer.AddElement(pElement, 0)
            '    End If
            '    pElement = pGraphicsContainer.Next
            'Else
            If Not (TypeOf pElement Is IMapFrame) And Not (TypeOf pElement Is ITextElement) Then   'And Not (TypeOf pElement Is IFrameElement)
                pLayOutGrapContainer.AddElement(pElement, 0)
                If TypeOf pElement Is IRectangleElement Then
                    '获得制图的矩形参看框
                    g_pRecElement = pElement
                End If
            End If
            pElement = pGraphicsContainer.Next
            'End If

        End While


        'copy数据

        CopyToPageLayout(pGeometry, vPagelayout, CLng(vType))
        If CLng(vType) >= 5000 Then
            '根据不同的比例尺向制图窗口中添加一个内图框
            DrawMapBroderIn(CLng(vType), pGeometry, vPagelayout)
            '画公里格网
            ' DrawKmGrid
        End If
        pGraphicsContainer.Reset()
        pElement = pGraphicsContainer.Next
        While Not pElement Is Nothing
            If (TypeOf pElement Is ITextElement) Then   'And Not (TypeOf pElement Is IFrameElement)
                pLayOutGrapContainer.AddElement(pElement, 0)

            End If
            pElement = pGraphicsContainer.Next
        End While
    End Sub

    '对点排序 左上角为第一点(这里只是一个见排序 近世矩形的 且X Y偏差不能太大
    Private Sub OrderPointCol(ByVal pPointCol As IPointCollection)
        '现在就确定为四个点
        Dim pMinPointTemp As IPoint = Nothing
        Dim pMaxPointTemp As IPoint = Nothing
        Dim pLeftPointTemp As IPoint = Nothing
        Dim pRightPointTemp As IPoint = Nothing

        Dim dblA As Double
        Dim IntMin As Integer
        Dim IntMax As Integer
        Dim i As Integer

        '获得左下角点 右上角点
        For i = 0 To 3
            If dblA < pPointCol.Point(i).x + pPointCol.Point(i).y Then
                pMaxPointTemp = pPointCol.Point(i)
                IntMax = i
                dblA = pPointCol.Point(i).x + pPointCol.Point(i).y
            End If
        Next i
        dblA = 0
        For i = 0 To 3
            If pPointCol.Point(i).x + pPointCol.Point(i).y < dblA Or dblA = 0 Then
                pMinPointTemp = pPointCol.Point(i)
                IntMin = i
                dblA = pPointCol.Point(i).x + pPointCol.Point(i).y
            End If
        Next i

        For i = 0 To 3
            If i <> IntMin And i <> IntMax Then
                If Not pLeftPointTemp Is Nothing Then
                    If pLeftPointTemp.x > pPointCol.Point(i).x Then
                        pRightPointTemp = pLeftPointTemp
                        pLeftPointTemp = pPointCol.Point(i)
                        Exit For
                    Else
                        pRightPointTemp = pPointCol.Point(i)
                    End If
                Else
                    pLeftPointTemp = pPointCol.Point(i)
                End If
            End If
        Next i

        pPointCol = Nothing
        pPointCol = New Polygon
        pPointCol.AddPoint(pLeftPointTemp)
        pPointCol.AddPoint(pMinPointTemp)
        pPointCol.AddPoint(pRightPointTemp)
        pPointCol.AddPoint(pMaxPointTemp)
        pPointCol.AddPoint(pLeftPointTemp)

        pLeftPointTemp = Nothing
        pMinPointTemp = Nothing
        pRightPointTemp = Nothing
        pMaxPointTemp = Nothing

    End Sub

    '得到两个图框以便进行坐标有平面到图纸的转换 同时得到参考矩形框 获得的参数均以公用变量的形式传出
    '在得到一些参数的同时 又进行了一些必要的处理过程
    Private Sub GetMapAndPagerEn(ByVal pPagelayout As IPageLayout)
        Dim pGrapContainer As IGraphicsContainer
        Dim pMapelement As IMapFrame
        Dim pElement As IElement
        Dim pBorderSym As ISymbolBorder
        Dim pLineSym As ISimpleLineSymbol
        Dim pAv As IActiveView

        pGrapContainer = pPagelayout
        pGrapContainer.Reset()
        pElement = pGrapContainer.Next

        Do While Not pElement Is Nothing

            '获得制图框
            If TypeOf pElement Is IMapFrame Then
                pMapelement = pElement
                pAv = pMapelement.Map

                '获得公用变量信息
                g_pMapFrame = pElement
                g_pMapEnvelope = pAv.Extent
                g_pPaperEnvelope = pElement.Geometry.Envelope

                '设置制图数据框的边线宽度为0
                On Error Resume Next
                If Not pMapelement.Border Is Nothing Then
                    pBorderSym = pMapelement.Border
                    pLineSym = pBorderSym.LineSymbol
                    pLineSym.Width = 0

                    pBorderSym.LineSymbol = pLineSym
                    pMapelement.Border = pBorderSym
                End If

            ElseIf TypeOf pElement Is IRectangleElement Then
                '获得制图的矩形参看框
                g_pRecElement = pElement
            End If
            pElement = pGrapContainer.Next
        Loop

    End Sub

    '画图幅的内外图框  思路是先根据图幅的几何形状画外图框 再根据外图框画内图框（边线）
    Private Sub DrawMapBroderIn(ByVal lngScale As Long, ByVal pGeometry As IGeometry, ByVal vPagelayout As IPageLayoutControl)
        Dim pFillElement As IFillShapeElement
        Dim pGrapContainer As IGraphicsContainer
        Dim pFillSymbol As IFillSymbol
        Dim pColor As IColor
        Dim pOutline As ILineSymbol
        Dim pLineColor As IColor
        Dim pElement As IElement
        Dim pPointCol As IPointCollection
        Dim pPolygon As IPolygon
        Dim i As Integer
        '''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim pInLinePolygon As IPolygon
        pInLinePolygon = New Polygon
        Dim pInLinePointCol As IPointCollection
        pInLinePointCol = New Polygon
        Dim pMap As IMap
        Dim pMapActive As IActiveView
        pMap = vPagelayout.ActiveView.FocusMap
        pMapActive = pMap

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''

        pFillElement = New PolygonElement
        pFillSymbol = New MarkerFillSymbol
        pColor = New RgbColor
        pLineColor = New RgbColor
        pOutline = New SimpleLineSymbol

        '外图框的边线样式
        pColor.NullColor = True
        pLineColor.RGB = RGB(0, 0, 0)
        pFillSymbol.Color = pColor
        pOutline.Color = pLineColor
        pOutline.Width = 1.5
        pFillSymbol.Outline = pOutline

        '获得制图框信息
        Call GetMapAndPagerEn(vPagelayout.PageLayout)
        '获得插点后的上下边线信息
        m_clsGetGeoInfo.ChangeMapToPaper()

        pGrapContainer = vPagelayout.PageLayout

        '将四点坐标转换为图纸坐标 并放到集合中 点的顺序依次为左上角 左下角 右下角 右上角
        pPointCol = New Polygon
        pPointCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, m_clsGetGeoInfo.m_pPointLT1))
        pPointCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, m_clsGetGeoInfo.m_pPointLB2))
        pPointCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, m_clsGetGeoInfo.m_pPointRB3))
        pPointCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, m_clsGetGeoInfo.m_pPointRT4))
        pPointCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, m_clsGetGeoInfo.m_pPointLT1))

        '放到模块级的变量中
        m_pPaperPointCol = pPointCol
        pPolygon = pPointCol

        '画线
        Dim pSegmentCol As ISegmentCollection
        Dim pCurve As ICurve2
        Dim pPolyline As IPolyline
        Dim pFromPoint As IPoint
        Dim pToPoint As IPoint
        Dim pLineElement As ILineElement
        pSegmentCol = pPolygon

        '外图框的四条线
        Dim pConstructCurve As IConstructCurve
        Dim pColOutLine As Collection
        '外边框的四条内线
        Dim pConstructOutin As IConstructCurve
        Dim pColOutInLine As Collection

        pColOutLine = New Collection
        pColOutInLine = New Collection

        For i = 0 To pSegmentCol.SegmentCount - 1
            pCurve = Nothing
            pFromPoint = Nothing
            pToPoint = Nothing
            pElement = Nothing

            pCurve = pSegmentCol.Segment(i)
            pPolyline = New Polyline
            pPolyline.FromPoint = pCurve.FromPoint
            pPolyline.ToPoint = pCurve.ToPoint

            '下面是构造平行线  为画外图框做准备 构造两条平行线 一个是外边框的外部 一个是内部
            Dim pConstructPolyline As IPolyline
            pConstructCurve = New Polyline
            Dim pConstructLine As IPolyline
            pConstructLine = New Polyline
            pConstructLine = pPolyline
            '构造外图框的外边线
            pConstructCurve.ConstructOffset(pConstructLine, 0.75, esriConstructOffsetEnum.esriConstructOffsetMitered + esriConstructOffsetEnum.esriConstructOffsetSimple)
            '构造外图框的内边线
            pConstructOutin = New Polyline
            pConstructOutin.ConstructOffset(pConstructLine, 0.6, esriConstructOffsetEnum.esriConstructOffsetMitered + esriConstructOffsetEnum.esriConstructOffsetSimple)

            pConstructPolyline = pConstructCurve
            pColOutLine.Add(pConstructPolyline)
            pConstructPolyline = pConstructOutin
            pColOutInLine.Add(pConstructPolyline)
        Next i

        pElement = New PolygonElement
        pFillElement.Symbol = pFillSymbol
        pElement = pFillElement
        pElement.Geometry = pPolygon
        'pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        '同时再画个图框
        Dim pOutPointCol As IPointCollection
        Dim pOutPoint As IPoint
        pOutPointCol = New Polygon

        '确定是图框 有四个交点
        pOutPoint = GetIntersection(pColOutLine.Item(1), pColOutLine.Item(4))
        pOutPointCol.AddPoint(pOutPoint)
        pOutPoint = GetIntersection(pColOutLine.Item(1), pColOutLine.Item(2))
        pOutPointCol.AddPoint(pOutPoint)
        pOutPoint = GetIntersection(pColOutLine.Item(2), pColOutLine.Item(3))
        pOutPointCol.AddPoint(pOutPoint)
        pOutPoint = GetIntersection(pColOutLine.Item(3), pColOutLine.Item(4))
        pOutPointCol.AddPoint(pOutPoint)
        pOutPoint = GetIntersection(pColOutLine.Item(1), pColOutLine.Item(4))
        pOutPointCol.AddPoint(pOutPoint)

        pPolygon = pOutPointCol
        '画外图框外边线
        pOutline.Width = 1.5
        pFillSymbol.Outline = pOutline
        pFillElement.Symbol = pFillSymbol
        pElement = pFillElement
        pElement.Geometry = pPolygon
        pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        Dim pPolygonOutIn As IPolygon = Nothing
        pOutPointCol = Nothing
        pOutPointCol = New Polygon

        '如果是比例尺大于等于10000的则不需要外边框的内线
        If lngScale > 10000 Then
            '画外图框的内边线
            pOutPoint = GetIntersection(pColOutInLine.Item(1), pColOutInLine.Item(4))
            pOutPointCol.AddPoint(pOutPoint)
            pOutPoint = GetIntersection(pColOutInLine.Item(1), pColOutInLine.Item(2))
            pOutPointCol.AddPoint(pOutPoint)
            pOutPoint = GetIntersection(pColOutInLine.Item(2), pColOutInLine.Item(3))
            pOutPointCol.AddPoint(pOutPoint)
            pOutPoint = GetIntersection(pColOutInLine.Item(3), pColOutInLine.Item(4))
            pOutPointCol.AddPoint(pOutPoint)
            pOutPoint = GetIntersection(pColOutInLine.Item(1), pColOutInLine.Item(4))
            pOutPointCol.AddPoint(pOutPoint)

            pPolygonOutIn = pOutPointCol
            '画外图框的内边线
            pOutline.Width = 0.1
            pFillElement = New PolygonElement
            pFillSymbol.Outline = pOutline
            pFillElement.Symbol = pFillSymbol
            pElement = pFillElement
            pElement.Geometry = pPolygonOutIn
            pElement.Locked = True
            pGrapContainer.AddElement(pElement, 0)

        End If

        '下面在画内图框
        Dim pSegmentOut As ISegmentCollection
        pSegmentOut = pPolygon

        '先画经纬度格网的小短线
        'Dim lngMinX As Double
        'Dim lngMinY As Double

        '    Dim lngCur As Long
        'If lngScale > 10000 Then
        '    '获得西南角坐标
        '    GetCoordinateFromNewCode(m_strSheetNo, lngMinX, lngMinY)

        '    DrawCoordinameGridLine(pOutPointCol.Point(0), pOutPointCol.Point(1), pSegmentOut.Segment(0), lngMinX, lngMinY, 1, lngScale, g_pMapFrame, pGrapContainer)
        '    DrawCoordinameGridLine(pOutPointCol.Point(1), pOutPointCol.Point(2), pSegmentOut.Segment(1), lngMinX, lngMinY, 2, lngScale, g_pMapFrame, pGrapContainer)
        '    DrawCoordinameGridLine(pOutPointCol.Point(2), pOutPointCol.Point(3), pSegmentOut.Segment(2), lngMinX, lngMinY, 3, lngScale, g_pMapFrame, pGrapContainer)
        '    DrawCoordinameGridLine(pOutPointCol.Point(3), pOutPointCol.Point(4), pSegmentOut.Segment(3), lngMinX, lngMinY, 4, lngScale, g_pMapFrame, pGrapContainer)
        'End If

        Dim pTempSegmentCol As ISegmentCollection
        '内图框样式
        pOutline.Width = 0.1
        '第一条
        pCurve = pSegmentCol.Segment(0)
        pFromPoint = GetIntersectionSeg(pSegmentCol.Segment(0), pSegmentOut.Segment(1), True)
        pToPoint = GetIntersectionSeg(pSegmentCol.Segment(0), pSegmentOut.Segment(3), True)
        pPolyline = New Polyline
        pPolyline.FromPoint = pFromPoint
        pPolyline.ToPoint = pToPoint

        pLineElement = New LineElement
        pLineElement.Symbol = pOutline
        pElement = pLineElement
        pElement.Geometry = pPolyline
        pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        '第二条
        '在此处进行插点操作  ***************************88
        pCurve = pSegmentCol.Segment(1)
        pPolyline = New Polyline
        pPolyline = m_clsGetGeoInfo.m_pTopLineGeometry
        pTempSegmentCol = pPolyline

        pFromPoint = GetIntersectionSeg(pTempSegmentCol.Segment(0), pSegmentOut.Segment(0), True)
        pToPoint = GetIntersectionSeg(pTempSegmentCol.Segment(pTempSegmentCol.SegmentCount - 1), pSegmentOut.Segment(2), True)

        pPolyline.FromPoint = pFromPoint
        pPolyline.ToPoint = pToPoint

        pLineElement = New LineElement
        pLineElement.Symbol = pOutline
        pElement = pLineElement
        pElement.Geometry = pPolyline
        pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        '第三条
        pCurve = pSegmentCol.Segment(2)
        pFromPoint = GetIntersectionSeg(pSegmentCol.Segment(2), pSegmentOut.Segment(1), True)
        pToPoint = GetIntersectionSeg(pSegmentCol.Segment(2), pSegmentOut.Segment(3), True)
        pPolyline = New Polyline
        pPolyline.FromPoint = pFromPoint
        pPolyline.ToPoint = pToPoint

        pLineElement = New LineElement
        pLineElement.Symbol = pOutline
        pElement = pLineElement
        pElement.Geometry = pPolyline
        pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        '第四条
        pCurve = pSegmentCol.Segment(3)
        pPolyline = New Polyline
        pPolyline = m_clsGetGeoInfo.m_pBottomLineGeometry
        pTempSegmentCol = pPolyline

        pFromPoint = GetIntersectionSeg(pTempSegmentCol.Segment(0), pSegmentOut.Segment(0), True)
        pToPoint = GetIntersectionSeg(pTempSegmentCol.Segment(pTempSegmentCol.SegmentCount - 1), pSegmentOut.Segment(2), True)

        pPolyline.FromPoint = pFromPoint
        pPolyline.ToPoint = pToPoint

        pLineElement = New LineElement
        pLineElement.Symbol = pOutline
        pElement = pLineElement
        pElement.Geometry = pPolyline
        pElement.Locked = True
        pGrapContainer.AddElement(pElement, 0)

        Dim lngPaperIndex As Double
        Dim lngMinIndex As Long
        Dim lngMaxIndex As Long
        'Dim pGridLine As Polyline
        Dim pGridSegment As ISegment

        '获得四个外图框点与原来外图框点的偏差
        m_vXoffset = New Collection
        m_vYoffset = New Collection
        pElement = g_pRecElement

        m_vXoffset.Add(pSegmentOut.Segment(1).FromPoint.X - pElement.Geometry.Envelope.LowerLeft.X)
        m_vXoffset.Add(pSegmentOut.Segment(2).FromPoint.X - pElement.Geometry.Envelope.LowerRight.X)
        m_vXoffset.Add(pSegmentOut.Segment(3).FromPoint.X - pElement.Geometry.Envelope.UpperRight.X)
        m_vXoffset.Add(pSegmentOut.Segment(0).FromPoint.X - pElement.Geometry.Envelope.UpperLeft.X)

        m_vYoffset.Add(pSegmentOut.Segment(1).FromPoint.Y - pElement.Geometry.Envelope.LowerLeft.Y)
        m_vYoffset.Add(pSegmentOut.Segment(2).FromPoint.Y - pElement.Geometry.Envelope.LowerRight.Y)
        m_vYoffset.Add(pSegmentOut.Segment(3).FromPoint.Y - pElement.Geometry.Envelope.UpperRight.Y)
        m_vYoffset.Add(pSegmentOut.Segment(0).FromPoint.Y - pElement.Geometry.Envelope.UpperLeft.Y)

        '画公里格网
        Dim IntGridLen As Integer
        IntGridLen = 1

        If lngScale > 50000 Then
            IntGridLen = 2
        End If

        lngMinIndex = Fix(pGeometry.Envelope.XMin / 1000)
        lngMaxIndex = Fix(pGeometry.Envelope.XMax / 1000)
        Dim strTemp As String

        '画公里格网
        '如果是比例尺大于10000则公里格网直接与外边线的外图框相交
        If lngScale > 10000 Then
            pSegmentOut = pPolygonOutIn
        Else
            pSegmentOut = pPolygon
        End If

        strTemp = "0"

        For i = lngMinIndex + 1 To lngMaxIndex Step IntGridLen

            If i > lngMaxIndex Then Exit For
            lngPaperIndex = GetPaperXY(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, CDbl(i * 1000.0#), 0.0#)
            pGridSegment = New Line
            pFromPoint = New Point
            pToPoint = New Point
            pFromPoint.PutCoords(lngPaperIndex, 10)
            pToPoint.PutCoords(lngPaperIndex, 15)
            pGridSegment.FromPoint = pFromPoint
            pGridSegment.ToPoint = pToPoint



            '获得与两条外图框的交点
            pFromPoint = GetIntersectionSeg(pGridSegment, pSegmentOut.Segment(3), False) '上
            pToPoint = GetIntersectionSeg(pGridSegment, pSegmentOut.Segment(1), False) '下

            If (Not pFromPoint Is Nothing) And (Not pToPoint Is Nothing) Then

                '判断进行避让 只有公里格网在图幅框内才进行绘制
                If (m_pPaperPointCol.Point(3).X - pFromPoint.X) * (pFromPoint.X - m_pPaperPointCol.Point(0).X) > 0 And _
                    (m_pPaperPointCol.Point(2).X - pToPoint.X) * (pToPoint.X - m_pPaperPointCol.Point(1).X) > 0 Then

                    pPolyline = New Polyline
                    pPolyline.FromPoint = pFromPoint
                    pPolyline.ToPoint = pToPoint
                    pLineElement = New LineElement
                    pLineElement.Symbol = pOutline
                    pElement = pLineElement
                    pElement.Geometry = pPolyline
                    pElement.Locked = True
                    pGrapContainer.AddElement(pElement, 0)

                    '插入带小表示       水平方向
                    '如果说标号离边框太近 则不进行绘制 标准是离图幅框一个厘米才进行绘制
                    If (pFromPoint.X - m_pPaperPointCol.Point(0).X) > 0.5 And (m_pPaperPointCol.Point(3).X - pFromPoint.X) > 0.5 Then
                        If CLng(strTemp) > CLng(Right(CStr(i), 2)) Or i = lngMinIndex + 1 Or i = lngMaxIndex Then
                            pFromPoint.X = pFromPoint.X '+ 0.23
                            pFromPoint.Y = (pFromPoint.Y + m_pPaperPointCol.Point(3).Y) / 2 '- 0.34


                            pElement = DrawGridText(pFromPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)

                            pToPoint.X = pToPoint.X '+ 0.25
                            pToPoint.Y = (pToPoint.Y + m_pPaperPointCol.Point(2).Y) / 2
                            pElement = DrawGridText(pToPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)

                            pFromPoint.X = pFromPoint.X ' - 0.35
                            pFromPoint.Y = pFromPoint.Y + 0.05
                            pElement = DrawGridText(pFromPoint, Left(CStr(i), Len(CStr(i)) - 2), 8, esriTextHorizontalAlignment.esriTHARight, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)

                            pToPoint.X = pToPoint.X '- 0.35
                            pToPoint.Y = pToPoint.Y + 0.05
                            pElement = DrawGridText(pToPoint, Left(CStr(i), Len(CStr(i)) - 2), 8, esriTextHorizontalAlignment.esriTHARight, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)
                        Else
                            '写格网的标识
                            pFromPoint.X = pFromPoint.X '+ 0.23
                            pFromPoint.Y = (pFromPoint.Y + m_pPaperPointCol.Point(3).Y) / 2 '- 0.34
                            pElement = DrawGridText(pFromPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)

                            pToPoint.X = pToPoint.X '+ 0.25
                            pToPoint.Y = (pToPoint.Y + m_pPaperPointCol.Point(2).Y) / 2 '+ 0.02
                            pElement = DrawGridText(pToPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVACenter)
                            'pElement.Locked = True
                            pGrapContainer.AddElement(pElement, 0)
                        End If
                    End If
                    '*********上面为画公里格网部分
                End If
            End If

            strTemp = Right(CStr(i), 2)
        Next i

        lngMinIndex = Fix(pGeometry.Envelope.YMin / 1000)
        lngMaxIndex = Fix(pGeometry.Envelope.YMax / 1000)
        strTemp = "0"

        Try
            For i = lngMinIndex + 1 To lngMaxIndex Step IntGridLen

                If i > lngMaxIndex Then Exit For
                lngPaperIndex = GetPaperXY(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, 0, i * 1000.0#)
                pGridSegment = New Line
                pFromPoint = New Point
                pToPoint = New Point
                pFromPoint.PutCoords(10, lngPaperIndex)
                pToPoint.PutCoords(15, lngPaperIndex)
                pGridSegment.FromPoint = pFromPoint
                pGridSegment.ToPoint = pToPoint

                '获得与两条外图框的交点
                pFromPoint = GetIntersectionSeg(pGridSegment, pSegmentOut.Segment(2), False) '右
                pToPoint = GetIntersectionSeg(pGridSegment, pSegmentOut.Segment(0), False) '左

                If (Not pFromPoint Is Nothing) And (Not pToPoint Is Nothing) Then
                    If (m_pPaperPointCol.Point(0).Y - pToPoint.Y) * (pToPoint.Y - m_pPaperPointCol.Point(1).Y) > 0 And _
                        (m_pPaperPointCol.Point(3).Y - pFromPoint.Y) * (pFromPoint.Y - m_pPaperPointCol.Point(2).Y) > 0 Then

                        pPolyline = New Polyline
                        pPolyline.FromPoint = pFromPoint
                        pPolyline.ToPoint = pToPoint
                        pLineElement = New LineElement
                        pLineElement.Symbol = pOutline
                        pElement = pLineElement
                        pElement.Geometry = pPolyline
                        pElement.Locked = True
                        pGrapContainer.AddElement(pElement, 0)
                        '垂直方向
                        '如果说标号离边框太近 则不进行绘制 标准是离图幅框一个厘米才进行绘制 这里只是判断了一个点是否距离边框大于0.5个厘米
                        If (pFromPoint.Y - m_pPaperPointCol.Point(2).Y) > 0.5 And (m_pPaperPointCol.Point(3).Y - pFromPoint.Y) > 0.5 Then
                            If CLng(strTemp) > CLng(Right(CStr(i), 2)) Or i = lngMinIndex + 1 Or i = lngMaxIndex Then

                                pFromPoint.X = (m_pPaperPointCol.Point(2).X + pFromPoint.X) / 2 - 0.05 '- 0.18
                                pFromPoint.Y = pFromPoint.Y
                                pElement = DrawGridText(pFromPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)

                                pToPoint.X = (m_pPaperPointCol.Point(1).X + pToPoint.X) / 2  '+ 0.42
                                pToPoint.Y = pToPoint.Y
                                pElement = DrawGridText(pToPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)

                                'pFromPoint.X = pFromPoint.X ' - 0.3
                                pFromPoint.Y = pFromPoint.Y + 0.1

                                pElement = DrawGridText(pFromPoint, Left(CStr(i), Len(CStr(i)) - 2), 8, esriTextHorizontalAlignment.esriTHARight, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)

                                'pToPoint.X = pToPoint.X '- 0.3
                                pToPoint.Y = pToPoint.Y + 0.1
                                pElement = DrawGridText(pToPoint, Left(CStr(i), Len(CStr(i)) - 2), 8, esriTextHorizontalAlignment.esriTHARight, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)
                            Else
                                '写格网的标识
                                'pFromPoint.X = pFromPoint.X '- 0.25
                                'pFromPoint.Y = pFromPoint.Y
                                pElement = DrawGridText(pFromPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHARight, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)

                                'pToPoint.X = pToPoint.X '+ 0.24
                                'pToPoint.Y = pToPoint.Y
                                pElement = DrawGridText(pToPoint, Right(CStr(i), 2), 12, esriTextHorizontalAlignment.esriTHALeft, esriTextVerticalAlignment.esriTVABaseline)
                                'pElement.Locked = True
                                pGrapContainer.AddElement(pElement, 0)
                            End If
                        End If

                    End If
                    '****上面为画公里格网部分
                    strTemp = Right(CStr(i), 2)
                End If

            Next i
        Catch ex As Exception
            Debug.Write(ex.Message)
        End Try

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        pInLinePointCol.AddPoint(m_clsGetGeoInfo.m_pPointLT1)
        pInLinePointCol.AddPoint(m_clsGetGeoInfo.m_pPointLB2)
        pInLinePointCol.AddPoint(m_clsGetGeoInfo.m_pPointRB3)
        pInLinePointCol.AddPoint(m_clsGetGeoInfo.m_pPointRT4)
        pInLinePointCol.AddPoint(m_clsGetGeoInfo.m_pPointLT1)

        pInLinePolygon = pInLinePointCol

        Dim pInLineGeo As IGeometry
        pInLinePolygon.SpatialReference = pMap.SpatialReference

        pInLineGeo = pInLinePolygon
        Dim pTopo As ITopologicalOperator4
        pTopo = pInLineGeo
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()
        pMap.ClipGeometry = pInLineGeo
        pInLinePointCol = Nothing
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        pFillElement = Nothing
        pFillSymbol = Nothing
        pColor = Nothing
        pOutline = Nothing

    End Sub




    '画经纬度格网短线
    Private Sub DrawCoordinameGridLine(ByVal pFromPoint As IPoint, ByVal pToPoint As IPoint, _
        ByVal pLine As ILine, ByVal lngMinX As Double, ByVal lngMinY As Double, _
        ByVal vType As Integer, ByVal lngScale As Long, ByVal pMapelement As IMapFrame, ByVal pGrapContainer As IGraphicsContainer)

        Dim lngFZ As Double
        Dim pBaseLine As ILine
        pBaseLine = New Line
        pBaseLine.FromPoint = pFromPoint
        pBaseLine.ToPoint = pToPoint

        '判断下世横向还是列向
        Dim dblWidth As Double
        Dim dblHeight As Double
        Dim blnWidth As Boolean
        dblWidth = pFromPoint.x - pToPoint.x
        dblHeight = pFromPoint.y - pToPoint.y
        If Math.Abs(dblWidth) > Math.Abs(dblHeight) Then
            blnWidth = True
        End If

        Dim IntLineCount As Integer
        Dim i As Integer
        Dim lngMin As Long
        Dim pNormalLine As ILine
        Select Case lngScale
            Case 50000
                If blnWidth Then
                    IntLineCount = 15
                    lngMin = lngMinX
                Else
                    IntLineCount = 10
                    lngMin = lngMinY
                End If
            Case 100000

            Case 250000
                If blnWidth Then
                    IntLineCount = 90
                    lngMin = lngMinX
                Else
                    IntLineCount = 60
                    lngMin = lngMinY
                End If
            Case 500000

            Case 1000000

            Case Else
                Exit Sub
        End Select

        '获得参考的一个坐标值
        Select Case vType
            Case 1
                lngFZ = lngMinX
            Case 2
                lngFZ = lngMinY
            Case 3
                lngFZ = lngMinX + IntLineCount * 60
            Case 4
                lngFZ = lngMinY + IntLineCount * 60
        End Select

        Dim dblMapx As Double
        Dim dblMapY As Double
        Dim lngMapX As Long
        Dim lngMapY As Long
        'Dim DblPaperX As Double
        'Dim DblPagerY As Double
        Dim pPoint1 As IPoint         '真实的镜纬格网在图形边线上的落点
        Dim pPointNormal As IPoint    '垂点
        Dim pConstructPoint As IPoint '构造点
        Dim pShortLine As IPolyline        '格网小短线
        'Dim pNormalLine As esriGeometry.ILine      '参考垂线
        'Dim pConstructPoint As IConstructPoint
        Dim pProximityOperator As IProximityOperator
        Dim pElement As IElement
        Dim pLineElement As ILineElement
        Dim pSimpleSymbol As ISimpleLineSymbol
        Dim pColor As IColor

        pColor = New RgbColor
        pColor.RGB = RGB(0, 0, 0)
        pSimpleSymbol = New SimpleLineSymbol
        pSimpleSymbol.Color = pColor
        pSimpleSymbol.Width = 0.1

        '获得坐标转换的相关参数
        Dim pMapEnvelope As IEnvelope
        Dim pPaperEnvelope As IEnvelope
        Dim pAv As IActiveView
        Dim pMap As IMap
        Dim pProjectReference As IProjectedCoordinateSystem
        pMap = pMapelement.Map
        pAv = pMap
        pMapEnvelope = pAv.Extent
        pElement = pMapelement
        pPaperEnvelope = pElement.Geometry.Envelope
        pProjectReference = pMap.SpatialReference

        For i = 1 To IntLineCount - 1
            '获得经纬坐标
            If blnWidth = True Then
                lngMapX = lngMin + i * 60
                lngMapY = lngFZ
            Else
                lngMapY = lngMin + i * 60
                lngMapX = lngFZ
            End If

            '获得平面坐标
            GetPanleByCoordinate(lngMapX / 3600, lngMapY / 3600, pProjectReference, dblMapx, dblMapY)
            pPoint1 = New Point
            pPoint1.X = dblMapx
            pPoint1.Y = dblMapY

            Dim pPaperPoint As IPoint
            '获得图纸坐标
            pPaperPoint = GetPaperPoint(pMapEnvelope.LowerLeft, pMapEnvelope.UpperRight, pPaperEnvelope.LowerLeft, pPaperEnvelope.UpperRight, pPoint1)

            '找到距离该点最近的直线上的点 并且构造一个点
            pProximityOperator = pBaseLine
            pPointNormal = pProximityOperator.ReturnNearestPoint(pPaperPoint, esriSegmentExtension.esriNoExtension)

            '构造一条垂线
            pNormalLine = New Line
            pNormalLine.FromPoint = pPaperPoint
            pNormalLine.ToPoint = pPointNormal

            '获得交点
            pConstructPoint = GetIntersectionSeg(pLine, pNormalLine, True)

            '得到格网小短线
            pShortLine = New Polyline
            pShortLine.FromPoint = pConstructPoint
            pShortLine.ToPoint = pPointNormal

            '画ELEMENT
            pLineElement = New LineElement
            pLineElement.Symbol = pSimpleSymbol
            pElement = pLineElement
            pElement.Geometry = pShortLine
            pGrapContainer.AddElement(pElement, 0)
        Next i
    End Sub

    '获得平面坐标
    Public Sub GetPanleByCoordinate(ByVal dblMapx As Double, ByVal dblMapY As Double, ByVal pProjectReference As IProjectedCoordinateSystem, ByVal dblX As Double, ByVal dblY As Double)
        Dim pPoint As WKSPoint
        If pProjectReference Is Nothing Then
            dblX = dblMapx
            dblY = dblMapY
        Else
            pPoint.x = dblMapx
            pPoint.y = dblMapY

            pProjectReference.Forward(1, pPoint)

            dblX = pPoint.x
            dblY = pPoint.y
        End If

    End Sub
    '写格网的标市
    Private Function DrawGridText(ByVal pPoint As IPoint, ByVal strText As String, ByVal dblTextSize As Double, ByVal h_alignment As esriTextHorizontalAlignment, ByVal v_alignment As esriTextVerticalAlignment) As ITextElement
        Dim pRGBColor As IRgbColor
        Dim pTextElement As ITextElement
        Dim pTextSymbol As ITextSymbol
        'Dim fnt As stdole.IFontDisp
        Dim pElement As IElement
        pRGBColor = New RgbColor
        pRGBColor.Blue = 0
        pRGBColor.Red = 0
        pRGBColor.Green = 0
        pTextElement = New TextElement
        pElement = pTextElement
        pElement.Geometry = pPoint
        Dim pFontDisp As stdole.IFontDisp
        pFontDisp = New stdole.StdFont
        If type_ZT = 0 Then
            pFontDisp.Name = "黑体"
        ElseIf type_ZT = 1 Then
            pFontDisp.Name = "宋体"     'yjl20110915 add
        End If

        pFontDisp.Bold = True
        'pFontDisp.Underline = True
        pTextSymbol = New TextSymbol
        pTextSymbol.Font = pFontDisp
        pTextSymbol.Color = pRGBColor
        pTextSymbol.Size = dblTextSize
        pTextSymbol.HorizontalAlignment = h_alignment
        pTextSymbol.VerticalAlignment = v_alignment
        pTextElement.Symbol = pTextSymbol
        pTextElement.Text = strText
        DrawGridText = pTextElement
    End Function


    '由MAP上的GEOMETRY转到图纸坐标上来
    Private Function GetPaperXY(ByVal pMapPoint1 As IPoint, ByVal pMapPoint2 As IPoint, _
        ByVal pPaperPoint1 As IPoint, ByVal pPaperPoint2 As IPoint, _
        ByVal dblX As Double, ByVal dblY As Double) As Double

        If dblY = 0 Then
            GetPaperXY = (pPaperPoint1.x - pPaperPoint2.x) * (dblX - pMapPoint1.X) / (pMapPoint1.X - pMapPoint2.X) + pPaperPoint1.x
        ElseIf dblX = 0 Then
            GetPaperXY = (pPaperPoint1.y - pPaperPoint2.y) * (dblY - pMapPoint1.Y) / (pMapPoint1.Y - pMapPoint2.Y) + pPaperPoint1.y
        Else
            Exit Function
        End If
    End Function

    '得到要素的交点
    Private Function GetIntersectionSeg(ByVal pSegment1 As ISegment, ByVal pSegment2 As ISegment, ByVal blnEx As Boolean) As IPoint
        Dim pTcoll As IPointCollection
        Dim pConstructMultipoint As IConstructMultipoint
        Dim i As Integer

        pConstructMultipoint = New Multipoint
        If blnEx = False Then
            pConstructMultipoint.ConstructIntersection(pSegment1, esriSegmentExtension.esriExtendEmbedded, pSegment2, esriSegmentExtension.esriNoExtension)
        Else
            pConstructMultipoint.ConstructIntersection(pSegment1, esriSegmentExtension.esriExtendEmbedded, pSegment2, esriSegmentExtension.esriExtendEmbedded)
        End If
        pTcoll = pConstructMultipoint
        For i = 0 To pTcoll.PointCount - 1
            GetIntersectionSeg = pTcoll.Point(i)
            Exit Function
        Next i
        GetIntersectionSeg = Nothing
    End Function
    '得到要素的交点
    Private Function GetIntersection(ByVal pPolyline1 As IPolyline, ByVal pPolyline2 As IPolyline) As IPoint
        Dim pTcoll As IPointCollection
        Dim pConstructMultipoint As IConstructMultipoint
        Dim pSegment1 As ISegment
        Dim pSegment2 As ISegment
        Dim pSegmentCol As ISegmentCollection
        GetIntersection = Nothing
        pSegmentCol = New Polyline

        pSegmentCol = pPolyline1
        pSegment1 = pSegmentCol.Segment(0)

        pSegmentCol = Nothing
        pSegmentCol = pPolyline2
        pSegment2 = pSegmentCol.Segment(0)

        Dim i As Integer
        pConstructMultipoint = New Multipoint
        pConstructMultipoint.ConstructIntersection(pSegment1, esriSegmentExtension.esriExtendEmbedded, pSegment2, esriSegmentExtension.esriExtendEmbedded)
        pTcoll = pConstructMultipoint
        For i = 0 To pTcoll.PointCount - 1
            GetIntersection = pTcoll.Point(i)
            Exit For
        Next i
        Return GetIntersection
    End Function

    Private Sub CopyToPageLayout(ByVal pGeometry As IGeometry, ByVal vPagelayout As IPageLayoutControl, ByVal lngScale As Long)
        '向pagelayerout加图层
        Dim pMap As IMap

        '得到 IActiveView 接口
        Dim pActiveView As IActiveView
        Dim pSymbolBorder As ISymbolBorder
        Dim pSimpleLineSymbol As ISimpleLineSymbol

        pMap = vPagelayout.ActiveView.FocusMap

        pActiveView = pMap
        pActiveView.Extent = pGeometry.Envelope

        pMap.MapScale = lngScale


        pSimpleLineSymbol = New SimpleLineSymbol
        pSimpleLineSymbol.Width = 0
        pSymbolBorder = New SymbolBorder
        pSymbolBorder.LineSymbol = pSimpleLineSymbol
        pMap.ClipGeometry = pGeometry
        pMap.ClipBorder = pSymbolBorder


    End Sub

    '移动图幅周边的一些表示xinxi
    Private Sub MovePageLyrOutlbl(ByVal pElement As IElement, ByVal dblX As Double, ByVal dblY As Double)
        'Dim pEnvelope As IEnvelope
        'Dim pPoint As IPoint
        'If pElement.Geometry.GeometryType = esriGeometryType.esriGeometryPoint Then
        '    pPoint = pElement.Geometry
        '    pPoint.X = pPoint.X + dblX
        '    pPoint.Y = pPoint.Y + dblY

        '    pElement.Geometry = pPoint
        'Else
        '    pEnvelope = pElement.Geometry.Envelope
        '    pEnvelope.Offset(dblX, dblY)
        '    pElement.Geometry = pEnvelope
        'End If
        Dim pTran As ITransform2D
        pTran = pElement.Geometry
        pTran.Move(dblX, dblY)
        pElement.Geometry = pTran

    End Sub
End Class
