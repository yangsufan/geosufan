Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Controls
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.GeoDatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem


Public Class clsGetGeoInfo
    Public m_pPointLT1 As IPoint                                '左上角点坐标(平面）
    Public m_pPointLB2 As IPoint                                '左下角点坐标(平面）
    Public m_pPointRB3 As IPoint                                '右下角点坐标(平面）
    Public m_pPointRT4 As IPoint                                '右上角点坐标(平面）

    Public m_pGeoPointLT1 As IPoint                             '左上角点坐标(经纬度）
    Public m_pGeoPointLB2 As IPoint                             '左下角点坐标(经纬度）
    Public m_pGeoPointRB3 As IPoint                             '右下角点坐标(经纬度）
    Public m_pGeoPointRT4 As IPoint                             '右上角点坐标(经纬度）

    Public m_pSheetMapGeometry As IGeometry                     '得到的返回的图幅图形
    Public m_pTopLineGeometry As IGeometry                      '上边线的几何图形（插入点后 图纸坐标）
    Public m_pBottomLineGeometry As IGeometry                   '下边线的几何图形（插入点后 图纸坐标）

    '输入信息
    Public m_lngMapScale As Long                                '比例尺
    Public m_strMapNO As String                                 '图幅号
    Public m_pPrjCoor As IProjectedCoordinateSystem            '投影信息
    Public m_intInsertCount As Integer                          '插入点数

    '该模块用到的模块级变量
    Private m_pTopLine As IGeometry
    Private m_pBottomLine As IGeometry

    '由图幅号和比例尺　投影计算出所有的图幅形状信息
    Public Sub ComputerAllGeoInfo()
        Dim lngMapScale As Long
        Dim strMapNO As String
        Dim pPrjCoor As IProjectedCoordinateSystem
        Dim dblX As Double      '西南角Ｘ坐标
        Dim dblY As Double      '西南角Ｙ坐标
        Dim dblXoff As Double   '图幅的横向宽度（单位为秒）
        Dim dblYoff As Double   '图幅的纵向高度（单位为秒）
        Dim pLTpoint As IPoint
        Dim pLBpoint As IPoint
        Dim pRBpoint As IPoint
        Dim pRTpoint As IPoint
        Dim intPointCount As Integer    '要插入点的个数
        Dim pLeftLine As ILine          '图幅的左边线
        Dim pRightLine As ILine         '图幅的右边线
        Dim pTopLine As IGeometry       '图幅的上边线
        Dim pBottomLine As IGeometry    '图幅的下边线
        'Dim pGeoClip As IGeometry       '反出去一个图幅的图形
        Dim pGeoclipCol As IPointCollection
        Dim pPolyline As IPolyline
        Dim pPolygon As IPolygon
        Dim pTempPntCol As IPointCollection
        Dim i As Integer

        lngMapScale = m_lngMapScale
        strMapNO = m_strMapNO
        pPrjCoor = m_pPrjCoor

        intPointCount = m_intInsertCount
        If strMapNO = "" Or pPrjCoor Is Nothing Then
            '可以加入日志信息
            Exit Sub
        End If
        '
        '得到一个图幅当中ＸＹ向的经纬度偏差
        If lngMapScale = 5000 Then
            dblXoff = 112.5
            dblYoff = 75
        End If

        If lngMapScale = 10000 Then
            dblXoff = 225
            dblYoff = 150
        End If
        If lngMapScale = 25000 Then
            dblXoff = 450
            dblYoff = 300
        End If

        If lngMapScale = 50000 Then
            dblXoff = 900
            dblYoff = 600
        End If

        If lngMapScale = 250000 Then
            dblXoff = 4500
            dblYoff = 3000
        End If

        '对于比例尺的情况 直接返该图幅的GEOMETRY
        If lngMapScale < 5000 Then
            Select Case lngMapScale
                Case 500

                    Dim XMin As Double
                    Dim YMin As Double
                    Dim XYarray() As String
                    XYarray = strMapNO.Split("-")
                    XMin = Convert.ToDouble(XYarray(0))

                    YMin = Convert.ToDouble(XYarray(1))

                    Dim pEnvelope As IEnvelope
                    pEnvelope = New Envelope()
                    pEnvelope.PutCoords(XMin, YMin, XMin + 250, YMin + 250)
                    m_pSheetMapGeometry = pEnvelope
                    m_pSheetMapGeometry.Project(pPrjCoor)

                Case 1000
                    Dim XMin As Double
                    Dim YMin As Double
                    Dim XYarray() As String
                    XYarray = strMapNO.Split("-")
                    XMin = Convert.ToDouble(XYarray(0))

                    YMin = Convert.ToDouble(XYarray(1))

                    Dim pEnvelope As IEnvelope
                    pEnvelope = New Envelope()
                    pEnvelope.PutCoords(XMin, YMin, XMin + 500, YMin + 500)
                    m_pSheetMapGeometry = pEnvelope
                    m_pSheetMapGeometry.Project(pPrjCoor)
                Case 2000
                    Dim XMin As Double
                    Dim YMin As Double
                    Dim XYarray() As String
                    XYarray = strMapNO.Split("-")
                    XMin = Convert.ToDouble(XYarray(0))

                    YMin = Convert.ToDouble(XYarray(1))

                    Dim pEnvelope As IEnvelope
                    pEnvelope = New Envelope()
                    pEnvelope.PutCoords(XMin, YMin, XMin + 1000, YMin + 1000)
                    m_pSheetMapGeometry = pEnvelope
                    m_pSheetMapGeometry.Project(pPrjCoor)
            End Select

            Exit Sub
        End If

        '得到西南角坐标点 不用上面的函数的原因是要获得四个角说有的坐标点信息
        Call GetCoordinateFromNewCode(strMapNO, dblX, dblY)

        '下面根据西南角坐标和经纬度偏差来计算四个顶角点
        '坐上角点
        pLTpoint = New Point
        pLTpoint.x = dblX / 3600
        pLTpoint.y = (dblY + dblYoff) / 3600
        m_pGeoPointLT1 = pLTpoint
        m_pPointLT1 = GetxyVal(pLTpoint, pPrjCoor)

        '左下角点
        pLBpoint = New Point
        pLBpoint.x = dblX / 3600
        pLBpoint.y = dblY / 3600
        m_pGeoPointLB2 = pLBpoint
        m_pPointLB2 = GetxyVal(pLBpoint, pPrjCoor)

        '右下角点
        pRBpoint = New Point
        pRBpoint.x = (dblX + dblXoff) / 3600
        pRBpoint.y = dblY / 3600
        m_pGeoPointRB3 = pRBpoint
        m_pPointRB3 = GetxyVal(pRBpoint, pPrjCoor)

        '右上角点
        pRTpoint = New Point
        pRTpoint.x = (dblX + dblXoff) / 3600
        pRTpoint.y = (dblY + dblYoff) / 3600
        m_pGeoPointRT4 = pRTpoint
        m_pPointRT4 = GetxyVal(pRTpoint, pPrjCoor)

        '四条边线
        '左边线
        pLeftLine = New Line
        pLeftLine.FromPoint = pLTpoint
        pLeftLine.ToPoint = pLBpoint

        '右边线
        pRightLine = New Line
        pRightLine.FromPoint = pRTpoint
        pRightLine.ToPoint = pRBpoint

        '下面是根据需求进行点的插入
        pTopLine = GetTopOrBotLine(pLTpoint, pRTpoint, pPrjCoor, intPointCount)
        pBottomLine = GetTopOrBotLine(pLBpoint, pRBpoint, pPrjCoor, intPointCount)
        m_pTopLine = pTopLine
        m_pBottomLine = pBottomLine

        '将图幅的图形返回出去
        pGeoclipCol = New Polygon

        pGeoclipCol.AddPointCollection(pBottomLine)

        pPolyline = pTopLine
        pTempPntCol = pPolyline
        For i = pTempPntCol.PointCount - 1 To 0 Step -1
            pGeoclipCol.AddPoint(pTempPntCol.Point(i))
        Next i

        pPolygon = pGeoclipCol
        pPolygon.Close()

        m_pSheetMapGeometry = pPolygon
        m_pSheetMapGeometry.Project(pPrjCoor)

    End Sub

    Public Sub ChangeMapToPaper()
        '返回图纸坐标中的上下边线
        m_pTopLineGeometry = ChangeLineToPaper(m_pTopLine)
        m_pBottomLineGeometry = ChangeLineToPaper(m_pBottomLine)
    End Sub
    '将平面坐标线转到图纸坐标中去 主要是针对上下两条边线
    Private Function ChangeLineToPaper(ByVal pPolyline As IPolyline) As IPolyline
        Dim pTempPoint As IPoint
        Dim pPointCol As IPointCollection
        Dim pPaperPntCol As IPointCollection
        Dim i As Integer
        ChangeLineToPaper = Nothing
        pPaperPntCol = New Polyline
        pPointCol = pPolyline

        For i = 0 To pPointCol.PointCount - 1
            pTempPoint = pPointCol.Point(i)
            pPaperPntCol.AddPoint(GetPaperPoint(g_pMapEnvelope.LowerLeft, g_pMapEnvelope.UpperRight, g_pPaperEnvelope.LowerLeft, g_pPaperEnvelope.UpperRight, pTempPoint))
        Next i

        If pPaperPntCol.PointCount > 1 Then
            ChangeLineToPaper = pPaperPntCol
        End If
        Return ChangeLineToPaper
    End Function
End Class
