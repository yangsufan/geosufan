using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;

namespace GeoDBATool
{
    class ControlsDataJoinSearch : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        private DataTable m_PolyLineSearchTable;
        private DataTable m_PolygonSearchTable;

        public ControlsDataJoinSearch()
        {
            base._Name = "GeoDBATool.ControlsDataJoinSetting";
            base._Caption = "接边要素搜索";
            base._Tooltip = "";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "接边要素搜索";

        }

        public override bool Enabled
        {
            get
            {
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            /*  执行接边要素的搜索
             *  基本过程：
             *    1.获取基本参数信息：图幅结合表的图层、参与接边图层列表、接边参数（从配置xml文件中获取）；
             *    2.实现IMapframe接口，通过接口获取接边的源、目标缓冲区域、接边边界；
             *    3.（接边要素粗选）实现IDestinatDataset接口，使用（2）返回的缓冲区域通过接口获取待接边源要素、目标要素的OID集合；
             *    4.（接边要素精选）实现ICheckOperation接口，通过接口在步骤（3）的OID集合中判断接边情况和属性匹配情况（若FieldsControlList不赋值，则不进行属性控制）；
             *    5.将接口ICheckOperation返回的信息反映到界面上。
             */
            frmJoinFeaSearch FeaSearch = new frmJoinFeaSearch();
            List<string> JoinFeaClsName = null;
            Dictionary<string, List<string>> JoinField = new Dictionary<string, List<string>>();//////接边控制字段
            string MapFrameName = string.Empty;
            string MapFrameField = "";
            if (System.Windows.Forms.DialogResult.OK == FeaSearch.ShowDialog())
            {
                JoinFeaClsName = FeaSearch.JoinLayerName;
                JoinField = FeaSearch.FieldDic;
                MapFrameName = FeaSearch.MapFrameName;
                MapFrameField = FeaSearch.MapFrameField;
                IFeatureClass MapFrameFeaClss = null;
                #region 获取图幅范围
                int layercount = m_Hook.ArcGisMapControl.LayerCount;
                for (int i = 0; i < layercount; i++)
                {
                    ILayer getlayer = m_Hook.ArcGisMapControl.get_Layer(i);
                    if (getlayer.Name == MapFrameName)
                    {
                        IFeatureLayer FeaLayer = getlayer as IFeatureLayer;
                        MapFrameFeaClss = FeaLayer.FeatureClass;
                    }
                }
                #endregion
                List<IFeatureClass> JoinFeaCls = new List<IFeatureClass>();
                m_Hook.PolylineSearchGrid.Tag = JoinFeaCls;//////将待接边的图层列表挂在PolylineSearchGrid.Tag上供使用
                #region 获取接边图层列表
                layercount = m_Hook.ArcGisMapControl.LayerCount;
                for (int i = 0; i < layercount; i++)
                {
                    ILayer getlayer = m_Hook.ArcGisMapControl.get_Layer(i);
                    if (JoinFeaClsName.Contains(getlayer.Name))
                    {
                        IFeatureLayer FeaLayer = getlayer as IFeatureLayer;
                        IFeatureClass getFeaClss = FeaLayer.FeatureClass;
                        JoinFeaCls.Add(getFeaClss);
                    }
                }
                #endregion
                double dDisTo = -1;
                double dSeacherTo = -1;
                double dAngleTo = -1;
                double dLengthTo = -1;
                #region 获取接边参数
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(ModData.v_JoinSettingXML);
                if (null == XmlDoc)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取接边参数配置文件失败！");
                    return;
                }
                XmlElement ele = XmlDoc.SelectSingleNode(".//接边设置") as XmlElement;

                string sDisTo = ele.GetAttribute("距离容差");
                string sSeacherTo = ele.GetAttribute("搜索容差");
                string sAngleTo = ele.GetAttribute("角度容差");
                string sLengthTo = ele.GetAttribute("长度容差");
                string sJoinType = ele.GetAttribute("接边类型");
                string sIsRemovePnt = ele.GetAttribute("删除多边形多余点").Trim();
                string sIsSimplify = ele.GetAttribute("简单化要素").Trim();
                try
                {
                    dDisTo = Convert.ToDouble(sDisTo);
                    dSeacherTo = Convert.ToDouble(sSeacherTo);
                    dAngleTo = Convert.ToDouble(sAngleTo);
                    dLengthTo = Convert.ToDouble(sLengthTo);
                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边参数配置文件中参数不正确！");
                    return;
                }
                #endregion
                if (MapFrameFeaClss == null)
                    return;
                int max = MapFrameFeaClss.FeatureCount(null);
                IFeatureCursor JoinFrameCur = MapFrameFeaClss.Search(null, false);
                IFeature MapFrameFea = JoinFrameCur.NextFeature();

                IMapframe pMapframe = null;//////////接边图幅类初始化（标准图幅接边或非标准图幅接边）
                if (sJoinType == "标准图幅")
                    pMapframe = new ClsMapFrame();
                else
                    pMapframe = new ClsTaskFrame();
                pMapframe.MapFrameFea = MapFrameFeaClss;
                FrmProcessBar ProcessBar = new FrmProcessBar(max);
                ProcessBar.Show();
                ProcessBar.SetFrmProcessBarText( "正在搜索接边要素");
                long value = 0;
                this.m_PolyLineSearchTable.Rows.Clear();
                this.m_PolygonSearchTable.Rows.Clear();
                IDestinatDataset DesData = null;
                if (sJoinType == "标准图幅")
                    DesData = new ClsDestinatDataset(true);/////标准图幅接边搜索
                else
                    DesData = new ClsDestinatDataset(false);/////非标准图幅接边搜索
                /////////////////////////////////
                DesData.Angle_to = dAngleTo;
                if (sIsRemovePnt == "Y")//////删除多边形上多余的点
                    DesData.IsRemoveRedundantPnt = true;
                else
                    DesData.IsRemoveRedundantPnt = false;

                if (sIsSimplify == "Y")///////要素简单化（针对有多个geometry的要素）
                    DesData.IsGeometrySimplify = true;
                else
                    DesData.IsGeometrySimplify = false;
                //////////////////////////////////////
                ele = XmlDoc.SelectSingleNode(".//日志设置") as XmlElement;
                string sLogPath = ele.GetAttribute("日志路径").Trim();



                DesData.JoinFeatureClass = JoinFeaCls;
                //////遍历每一个图幅搜索接边要素/////
                if (!string.IsNullOrEmpty(sLogPath))
                {
                    IJoinLOG JoinLog = new ClsJoinLog();
                    Exception ex = null;
                    JoinLog.onDataJoin_Start(0, out ex);
                }
                while (MapFrameFea != null)
                {
                    value += 1;
                    ProcessBar.SetFrmProcessBarValue(value);
                    Application.DoEvents();
                    int index = MapFrameFea.Fields.FindField(MapFrameField);
                    string No = string.Empty;
                    try
                    {
                        if (index > 0)
                            No = MapFrameFea.get_Value(index).ToString();
                    }
                    catch
                    {
                        No = string.Empty;
                    }
                    ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No);
                    Application.DoEvents();
                    pMapframe.OriMapFrame = MapFrameFea;

                    IGeometry OriArea = null;
                    IGeometry DesArea = null;
                    ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + "正在生成缓冲区");
                    Application.DoEvents();
                    try
                    {
                        pMapframe.GetBufferArea(dDisTo, out OriArea, out DesArea);
                    }
                    catch
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "生成接边搜索缓冲区失败！\n请检查图幅范围图层是否设置正确。");
                        ProcessBar.Close();
                        return;
                    }
                    ////////////////////////////////////
                    //IElement ele2 = null;
                    //IPolygonElement pPolElemnt = new PolygonElementClass();
                    //IFillShapeElement pFillShapeElement = (IFillShapeElement)pPolElemnt;
                    //pFillShapeElement.Symbol = GetDrawSymbol(0, 255, 0);
                    //ele2 = pFillShapeElement as IElement;
                    //ele2.Geometry = DesArea;
                    //IGraphicsContainer pMapGraphics = (IGraphicsContainer)m_Hook.ArcGisMapControl.Map;
                    //pMapGraphics.AddElement(ele2, 0);
                    //m_Hook.ArcGisMapControl.ActiveView.Refresh();

                    //IElement ele3 = null;
                    //pPolElemnt = new PolygonElementClass();
                    //pFillShapeElement = (IFillShapeElement)pPolElemnt;
                    //pFillShapeElement.Symbol = GetDrawSymbol(255, 0, 0);
                    //ele3 = pFillShapeElement as IElement;
                    //ele3.Geometry = OriArea;
                    ////IGraphicsContainer pMapGraphics = (IGraphicsContainer)this.axMapControl.Map;
                    //pMapGraphics.AddElement(ele3, 0);

                    /////////////////////////////////////
                    if (!string.IsNullOrEmpty(No))
                        ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + ",正在获取接边要素");
                    else
                        ProcessBar.SetFrmProcessBarText("正在获取接边要素");
                    Application.DoEvents();
                    Dictionary<string, List<long>> OriOidDic = DesData.GetFeaturesByGeometry(OriArea, true);////接边源要素记录
                    Dictionary<string, List<long>> DesOidDic = DesData.GetFeaturesByGeometry(DesArea, false);////接边目标要素记录
                    ICheckOperation CheckOper = new ClsCheckOperationer();
                    CheckOper.Angel_Tolerrance = dAngleTo;/////////////角度容差
                    CheckOper.borderline = pMapframe.Getborderline();//接边边界
                    CheckOper.Dis_Tolerance = dDisTo;//////////////////距离容差
                    CheckOper.Search_Tolerrance = dSeacherTo;//////////搜索容差
                    CheckOper.Length_Tolerrance = dLengthTo;///////////长度容差
                    CheckOper.DesBufferArea = DesArea;/////////////////目标搜索缓冲区
                    CheckOper.OriBufferArea = OriArea;/////////////////源搜索缓冲区

                    if (!string.IsNullOrEmpty(sLogPath))
                        CheckOper.CreatLog = true;
                    else
                        CheckOper.CreatLog = false;
                    if (null != OriOidDic)
                    {

                        foreach (KeyValuePair<string, List<long>> item in OriOidDic)
                        {
                            string OriFeaName = item.Key;
                            if (!string.IsNullOrEmpty(No))
                                ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + ",正在搜索图层：" + OriFeaName);
                            else
                                ProcessBar.SetFrmProcessBarText("正在搜索图层：" + OriFeaName);
                            Application.DoEvents();
                            List<long> OriFeaOIDL = item.Value;
                            List<long> DesFeaOIDL = null;
                            if (DesOidDic == null) continue;
                            if (DesOidDic.ContainsKey(OriFeaName))
                            {
                                DesFeaOIDL = DesOidDic[OriFeaName];
                            }
                            if (null != OriFeaOIDL && null != DesFeaOIDL)
                            {
                                CheckOper.DesFeaturesOID = DesFeaOIDL;
                                CheckOper.OriFeaturesOID = OriFeaOIDL;
                                IFeatureClass JoinFea = DesData.TargetFeatureClass(OriFeaName);
                                if (null != JoinFeaCls)
                                {
                                    CheckOper.DestinatFeaCls = JoinFea;
                                    if (null != JoinField)
                                    {
                                        foreach (KeyValuePair<string, List<string>> getitem in JoinField)
                                        {
                                            if (getitem.Key == (JoinFea as IDataset).Name)
                                                CheckOper.FieldsControlList = getitem.Value;
                                        }
                                    }
                                    esriGeometryType GeoType = CheckOper.GetDatasetGeometryType();
                                    if (GeoType == esriGeometryType.esriGeometryPolyline)
                                    {
                                        if (!string.IsNullOrEmpty(No))
                                            ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + ",正在搜索图层：" + OriFeaName + ",操作：线型要素记录");
                                        else
                                            ProcessBar.SetFrmProcessBarText("正在搜索图层：" + OriFeaName + ",操作：线型要素记录");
                                        Application.DoEvents();
                                        DataTable table = CheckOper.GetPolylineDesFeatureOIDByOriFeature();
                                        if (null != table)
                                        {
                                            for (int i = 0; i < table.Rows.Count; i++)
                                            {
                                                if (!string.IsNullOrEmpty(No))
                                                    ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + "正在搜索图层：" + OriFeaName + "，操作：添加记录到列表");
                                                else
                                                    ProcessBar.SetFrmProcessBarText("正在搜索图层：" + OriFeaName + "，操作：添加记录到列表");
                                                Application.DoEvents();
                                                this.m_PolyLineSearchTable.Rows.Add(table.Rows[i].ItemArray);
                                            }

                                        }

                                    }
                                    if (GeoType == esriGeometryType.esriGeometryPolygon)
                                    {
                                        if (!string.IsNullOrEmpty(No))
                                            ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + "正在搜索图层：" + OriFeaName + "，操作：多边形要素记录");
                                        else
                                            ProcessBar.SetFrmProcessBarText("正在搜索图层：" + OriFeaName + "，操作：多边形要素记录");
                                        Application.DoEvents();
                                        DataTable table = CheckOper.GetPolygonDesFeatureOIDByOriFeature();
                                        if (null != table)
                                        {
                                            for (int i = 0; i < table.Rows.Count; i++)
                                            {
                                                if (!string.IsNullOrEmpty(No))
                                                    ProcessBar.SetFrmProcessBarText("正在处理图幅：" + No + "正在搜索图层：" + OriFeaName + "，操作：添加记录到列表");
                                                else
                                                    ProcessBar.SetFrmProcessBarText("正在搜索图层：" + OriFeaName + "，操作：添加记录到列表");
                                                Application.DoEvents();
                                                this.m_PolygonSearchTable.Rows.Add(table.Rows[i].ItemArray);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    MapFrameFea = JoinFrameCur.NextFeature();
                }
                if (!string.IsNullOrEmpty(sLogPath))
                {
                    IJoinLOG JoinLog = new ClsJoinLog();
                    Exception ex = null;
                    JoinLog.onDataJoin_Terminate(0, out ex);
                }
                m_Hook.PolygonSearchGrid.DataSource = null;
                m_Hook.PolylineSearchGrid.DataSource = null;
                m_Hook.PolygonSearchGrid.DataSource = this.m_PolygonSearchTable;
                m_Hook.PolylineSearchGrid.DataSource = this.m_PolyLineSearchTable;
                SelectALL(m_Hook.PolylineSearchGrid);
                SelectALL(m_Hook.PolygonSearchGrid);
                MessageBox.Show("线记录个数：" + this.m_PolyLineSearchTable.Rows.Count + ";面记录个数：" + this.m_PolygonSearchTable.Rows.Count);
                ProcessBar.Close();
            }
        }

        /// <summary>
        /// 将列表中第一列选中 xisheng 20110829
        /// </summary>
        public static void SelectALL(DevComponents.DotNetBar.Controls.DataGridViewX GetDataGrid)
        {
            try
            {
                for (int i = 0; i < GetDataGrid.Rows.Count; i++)
                {
                    GetDataGrid.Rows[i].Cells[0].Value = true;
                }
                if (GetDataGrid.Columns.Contains("OriPtn"))
                {
                    GetDataGrid.Columns["OriPtn"].Visible = false;
                    GetDataGrid.Columns["DesPtn"].Visible = false;
                }
                if (GetDataGrid.Columns.Contains("OriLineIndex"))
                {
                    GetDataGrid.Columns["OriLineIndex"].Visible = false;
                    GetDataGrid.Columns["DesLineIndex"].Visible = false;
                }
            }
            catch { }
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
            /////////////////初始化线型接边搜索表
            m_PolyLineSearchTable = new DataTable();
            m_PolyLineSearchTable.TableName = "PolylineSearchTable";
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            //DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            //DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            //DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("OriPtn", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            //DataColumn dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc5.DefaultValue = -1;
            DataColumn dc6 = new DataColumn("DesPtn", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
            m_PolyLineSearchTable.Columns.Add(dc1);
            m_PolyLineSearchTable.Columns.Add(dc2);
            m_PolyLineSearchTable.Columns.Add(dc3);
            m_PolyLineSearchTable.Columns.Add(dc4);
            m_PolyLineSearchTable.Columns.Add(dc5);
            m_PolyLineSearchTable.Columns.Add(dc6);
            m_PolyLineSearchTable.Columns.Add(dc7);
            //////////////////////////////////////
            ////初始化多边形接边搜索表
            m_PolygonSearchTable = new DataTable();
            m_PolygonSearchTable.TableName = "PolygonSearchTable";
            dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            dc4 = new DataColumn("OriLineIndex", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc5.DefaultValue = -1;
            dc6 = new DataColumn("DesLineIndex", Type.GetType("System.Int64"));
            dc6.DefaultValue = -1;
            dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
            m_PolygonSearchTable.Columns.Add(dc1);
            m_PolygonSearchTable.Columns.Add(dc2);
            m_PolygonSearchTable.Columns.Add(dc3);
            m_PolygonSearchTable.Columns.Add(dc4);
            m_PolygonSearchTable.Columns.Add(dc5);
            m_PolygonSearchTable.Columns.Add(dc6);
            m_PolygonSearchTable.Columns.Add(dc7);
            ////////////////////////////////////////////////////
        }
        public static ISimpleFillSymbol GetDrawSymbol(int intRed, int intGreen, int intBlue)
        {
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;

            ISymbol pSymbol = (ISymbol)pFillSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pRGBColor.Red = intRed;
            pRGBColor.Green = intGreen;
            pRGBColor.Blue = intBlue;
            pLineSymbol.Color = pRGBColor;

            pLineSymbol.Width = 0.8;
            pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pFillSymbol.Outline = pLineSymbol;

            pFillSymbol.Color = pRGBColor;
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

            return pFillSymbol;
        }
    }
}
