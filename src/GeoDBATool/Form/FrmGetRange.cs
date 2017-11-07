using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using SysCommon.Gis;

namespace GeoDBATool
{
    public partial class FrmGetRange : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppGISRef m_AppGIS;               //主功能应用APP
        private IGeometry m_Geometry;
        private IList<IDataset> List;
        public IGeometry DrawGeometry
        {
            set
            {
                m_Geometry = value;
            }
        }
        IFeatureLayer m_FeaLayer = null;
        public FrmGetRange(Plugin.Application.IAppGISRef pAppGIS)
        {
            InitializeComponent();
            m_AppGIS = pAppGIS;
            RadioBtnSelectRange.Checked = false;
            RadioBtnInputRange.Checked = false;
            RadioBtnDrawRange.Checked = false;
        }


        private void btnSelectRange_Click(object sender, EventArgs e)
        {
           ITool  _tool = new ControlsSelectFeaturesToolClass();
           ICommand _cmd = _tool as ICommand;
            _cmd.OnCreate(axMapControl1.Object as IMapControlDefault);
            if (_tool == null || _cmd == null) return;
            axMapControl1.CurrentTool = _tool;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            m_Geometry = null;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;

            m_FeaLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureSelection pFeaSel = m_FeaLayer as IFeatureSelection;
            if (pFeaSel.SelectionSet == null || pFeaSel.SelectionSet.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请在图上选择范围！");
                return;
            }
            else
            {
                m_Geometry = GetFeaLayerGeometry(pFeaSel, m_FeaLayer);
            }
            if (m_Geometry == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置图幅数据范围！");
                return;
            }
            //对范围进行检查看该范围是否是本地工作库图幅数据的总范围……

            //将范围信息解析后写入XML中
            byte[] xmlByte = xmlSerializer(m_Geometry);
            string base64String = Convert.ToBase64String(xmlByte);

            //XmlDocument DocXml = (m_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode)
            //DocXml.Load(ModData.v_projectXML);
            XmlNode ProNode = m_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode;//DocXml.SelectSingleNode(".//工程[@名称='" + m_AppGIS.ProjectTree.SelectedNode.Name + "']");
            XmlElement RangeElem = ProNode.SelectSingleNode(".//内容//图幅工作库//范围信息") as XmlElement;
            RangeElem.SetAttribute("范围", base64String);
            ProNode.OwnerDocument.Save(ModData.v_projectDetalXML);   //cyf 20110628

            //cyf 20110621 modify ;将范围信息存储到系统维护库当中
            #region 获取系统维护库连接信息，并连接系统维护库，将工作空间保存起来
            if (ModData.TempWks == null)
            {
                bool blnCanConnect = false;
                SysCommon.Gis.SysGisDB vgisDb = new SysCommon.Gis.SysGisDB();
                if (File.Exists(ModData.v_ConfigPath))
                {
                    //获得系统维护库连接信息
                    SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModData.v_ConfigPath, out ModData.Server, out ModData.Instance, out ModData.Database, out ModData.User, out ModData.Password, out ModData.Version, out ModData.dbType);
                    //连接系统维护库
                    blnCanConnect = ModDBOperator.CanOpenConnect(vgisDb, ModData.dbType, ModData.Server, ModData.Instance, ModData.Database, ModData.User, ModData.Password, ModData.Version);
                }
                else
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + ModData.v_ConfigPath + "/n请重新配置");
                    return;
                }
                if (!blnCanConnect)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                    return;
                }
                ModData.TempWks = vgisDb.WorkSpace;
            }
            if (ModData.TempWks == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败，请检查!");
                return;
            }
            #endregion
            ////将范围信息写入数据库
            //if (!File.Exists(ModData.v_DbInterConn)) return;

            ////读取系统维护库连接信息
            //XmlDocument xmlConnDoc = new XmlDocument();
            //xmlConnDoc.Load(ModData.v_DbInterConn);
            //XmlElement ele = xmlConnDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
            //if (ele == null) return;
            //string sConnect = ele.GetAttribute("连接字符串");//系统维护库连接字符串
            ////连接系统维护库
            //SysCommon.DataBase.SysTable pTable = new SysCommon.DataBase.SysTable();
            //pTable.SetDbConnection(sConnect, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提示，连接系统维护库失败！");
            //    return;
            //}
            if (m_AppGIS.ProjectTree.SelectedNode.Name.ToString().Trim() == "") return;
           
            ///原有更新ROW的方法
            //string upStr = "update DATABASEMD set DBPARA='" + base64String + "' where ID=" + Convert.ToInt32(m_AppGIS.ProjectTree.SelectedNode.Name.ToString().Trim());
            //pTable.UpdateTable(upStr, out eError);
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提示，更新图幅范围信息失败！");
            //    return;
            //}

            //现在的更新ROW xisheng changed 20111018
            SysGisTable sysTable = new SysGisTable(ModData.TempWks);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData.Add("DBPARA",base64String);
                try { sysTable.UpdateRow("DATABASEMD", "ID='" + Convert.ToInt32(m_AppGIS.ProjectTree.SelectedNode.Name.ToString().Trim()) + "'", dicData, out eError); }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提示，更新图幅范围信息失败！+详细信息:" + eError.Message);
                    return;
                }

            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "完成范围信息的获取和存储！");
            //end
            this.Close();
        }

        //获得选择的图层的范围
        public IGeometry GetFeaLayerGeometry(IFeatureSelection pFeatureSel, IFeatureLayer pMapLayer)
        {
            IEnumIDs pEnumIDs = pFeatureSel.SelectionSet.IDs;
            int id = pEnumIDs.Next();
            IGeometry pGeo = null;
            while (id != -1)
            {
                IFeature pFeat = pMapLayer.FeatureClass.GetFeature(id);
                if (pGeo == null)
                {
                    pGeo = pFeat.Shape;
                }
                else
                {
                    ITopologicalOperator pTop = pGeo as ITopologicalOperator;
                    pGeo = pTop.Union(pFeat.Shape);
                }
                id = pEnumIDs.Next();
            }
            return pGeo;
        }

        /// <summary>
        /// 序列化(将对象序列化成字符串)
        /// </summary>
        /// <param name="xmlByte">序列化字节</param>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static byte[] xmlSerializer(object obj)
        {
            try
            {
                byte[] xmlByte = null;//保存序列化后的字节
                //判断是否支持IPersistStream接口,只有支持该接口的对象才能进行序列化
                if (obj is ESRI.ArcGIS.esriSystem.IPersistStream)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    pStream.Save(xmlStream as ESRI.ArcGIS.esriSystem.IStream, 0);

                    xmlByte = xmlStream.SaveToBytes();
                }
                return xmlByte;
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************

                return null;
            }
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            Exception eError=null;
            SysCommon.Gis.SysGisDataSet pSysGISDT = new SysCommon.Gis.SysGisDataSet();
            axMapControl1.ClearLayers();
            //加载范围数据
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择图形范围";
            OpenFile.Filter = "图幅范围数据(*.mdb)|*.mdb";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                pSysGISDT.SetWorkspace(OpenFile.FileName, SysCommon.enumWSType.PDB, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据刻录出错！");
                    return;
                }
                IList<IDataset> LstDT = pSysGISDT.GetAllFeatureClass();
                List = LstDT;
                FeaturecomboBox.Items.Clear();
                FeaturecomboBox.Enabled = true;
                this.DataLoad.Enabled = true;
                foreach (IDataset pDT in LstDT)
                {
                    IFeatureClass pFeatureCls = pDT as IFeatureClass;
                    if (pFeatureCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureLayer pFeaLayer = new FeatureLayerClass();
                        ILayer pLayer = null;
                        pFeaLayer.FeatureClass = pFeatureCls;
                        pLayer = pFeaLayer as ILayer;

                        FeaturecomboBox.Items.Add(pFeatureCls.AliasName);
                    }
                }
                if (FeaturecomboBox.Items.Count > 0)
                    FeaturecomboBox.SelectedIndex = 0;
                axMapControl1.ActiveView.Refresh();
            }

            //ICommand _cmd = new ControlsAddDataCommandClass();
            //if (_cmd == null) return;
            //_cmd.OnCreate(axMapControl1.Object as IMapControlDefault);

            //_cmd.OnClick();
            //axMapControl1.ActiveView.Refresh();
        }
        
        private void RadioBtnDrawRange_Click(object sender, EventArgs e)
        {
            //RadioBtnDrawRange.Checked = !RadioBtnDrawRange.Checked;
           
        }
        
        private void RadioBtnInputRange_Click(object sender, EventArgs e)
        {
            //RadioBtnInputRange.Checked = !RadioBtnInputRange.Checked;
          
        }

        
        private void RadioBtnSelectRange_Click(object sender, EventArgs e)
        {
            //RadioBtnSelectRange.Checked = !RadioBtnSelectRange.Checked;
            
        }
        //从外部TXT文件导入范围
        private void RadioBtnInputRange_CheckedChanged(object sender, EventArgs e)
        {
            Exception outError=null;
            if (RadioBtnInputRange.Checked)
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.CheckFileExists = true;
                OpenFile.CheckPathExists = true;
                OpenFile.Title = "选择图形范围坐标txt";
                OpenFile.Filter = "图形范围坐标文本(*.txt)|*.txt";
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        StreamReader sr = new StreamReader(OpenFile.FileName);
                        while (sr.Peek() >= 0)
                        {
                            string[] strTemp = sr.ReadLine().Split(',');
                            //cyf 20110621 add:根据文本文件获取几何范围
                            for (int i = 0; i < strTemp.Length-1; i = i + 2)
                            {
                                if (sb.Length != 0)
                                {
                                    sb.Append(",");
                                }
                                sb.Append(strTemp[i] + "@" + strTemp[i+1]);
                            }
                            //end
                        }
                    }
                    catch(Exception er)
                    {
                        //*******************************************************************
                        //guozheng added
                        if (ModData.SysLog != null)
                        {
                            ModData.SysLog.Write(er, null, DateTime.Now);
                        }
                        else
                        {
                            ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            ModData.SysLog.Write(er, null, DateTime.Now);
                        }
                        //********************************************************************

                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "图形范围坐标txt格式不正确!\n文本每行为点坐标且以','分割");
                        return;
                    }

                    if (sb.Length == 0) return;
                    m_Geometry = ModDBOperator.GetPolygonByCol(sb.ToString()) as IGeometry;
                    //获得几何范围的空间参考
                    //if (ModData.TempWks != null)
                    //{
                    //    IFeatureWorkspace pFeaWs = ModData.TempWks as IFeatureWorkspace;
                    //    if (pFeaWs == null)
                    //    {
                    //        //获取系统维护库连接信息失败
                    //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败！");
                    //        return;
                    //    }
                    //}
                    //m_Geometry.SpatialReference=

                    //cyf 20110621 在临时工作库中创建一个范围图层
                    //CreateMapFrameLayerInWorkSpace(m_Geometry, out outError);
                    //if(outError!=null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建临时的范围图层失败！\n原因："+outError.Message);
                    //    return;
                    //}
                    //end
                }
            }
        }

        /// <summary>
        /// 将导入的范围在工作库中建立一个图副范围图层，  cyf 20110621
        /// </summary>
        /// <param name="in_pMapRange"></param>
        /// <param name="ex"></param>
        //private void CreateMapFrameLayerInWorkSpace(IGeometry in_pMapRange, out Exception ex)
        //{
        //    ex = null;
        //    //////第一步：获取工作库的Workspace
        //    if (this.m_WorkFeatureDataset == null) { ex = new Exception("工作库的工作空间不能为空"); return; }
        //    //////第二步：在工作空间中建立一个图副范围图层
        //    try
        //    {
        //        IFields fields = new FieldsClass();
        //        IFieldsEdit fsEdit = fields as IFieldsEdit;
        //        //添加Object字段
        //        IField newfield2 = new FieldClass();
        //        IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
        //        fieldEdit2.Name_2 = "OBJECTID";
        //        fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
        //        fieldEdit2.AliasName_2 = "OBJECTID";
        //        newfield2 = fieldEdit2 as IField;
        //        fsEdit.AddField(newfield2);

        //        //添加Geometry字段
        //        IField newfield1 = new FieldClass();
        //        IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
        //        fieldEdit1.Name_2 = "SHAPE";
        //        fieldEdit1.Type_2 = esriFieldType.esriFieldTypeGeometry;
        //        IGeometryDef geoDef = new GeometryDefClass();


        //        IGeometryDefEdit geoDefEdit = geoDef as IGeometryDefEdit;
        //        geoDefEdit.SpatialReference_2 = (this.m_WorkFeatureDataset as IGeoDataset).SpatialReference;
        //        geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
        //        fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
        //        newfield1 = fieldEdit1 as IField;
        //        fsEdit.AddField(newfield1);
        //        fields = fsEdit as IFields;
        //        ////////建立要素集
        //        IFeatureClass pMapFrameFeaCls = null;
        //        pMapFrameFeaCls = ModData.GetFeaClsSetInEnum("MapFrameLayer", this.m_WorkFeatureDataset.Subsets);
        //        if (pMapFrameFeaCls == null)
        //        {
        //            //////不存在则建立
        //            pMapFrameFeaCls = this.m_WorkFeatureDataset.CreateFeatureClass("MapFrameLayer", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
        //        }

        //        ////////第三部将图副范写入图副范围图层////////
        //        IWorkspaceEdit WsEdit = this.m_WorkFeatureDataset.Workspace as IWorkspaceEdit;
        //        WsEdit.StartEditing(true);
        //        WsEdit.StartEditOperation();

        //        IFeature NewFea = pMapFrameFeaCls.CreateFeature();
        //        NewFea.Shape = in_pMapRange;
        //        NewFea.Store();

        //        WsEdit.StopEditOperation();
        //        WsEdit.StopEditing(true);
        //        ////////第四步：记录此图层，加载至图层控件中
        //        this.m_MapFrameClass = pMapFrameFeaCls;
        //        DeleteLayerByName("MapFrameLayer");
        //        IFeatureLayer NewMapFrameLayer = new FeatureLayerClass();
        //        NewMapFrameLayer.FeatureClass = pMapFrameFeaCls;
        //        NewMapFrameLayer.Name = "MapFrameLayer";
        //        this.m_HookHelp.FocusMap.AddLayer(NewMapFrameLayer as ILayer);
        //        this.m_HookHelp.FocusMap.MoveLayer((NewMapFrameLayer as ILayer), this.m_HookHelp.FocusMap.LayerCount);
        //    }
        //    catch (Exception eError)
        //    {
        //        //******************************************
        //        //系统运行日志
        //        if (ModData.SysLog == null)
        //            ModData.SysLog = new clsWriteSystemFunctionLog();
        //        ModData.SysLog.Write(eError);
        //        //******************************************
        //        ex = eError;
        //    }
        //}
        //依照底图划范围
        private void RadioBtnDrawRange_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioBtnDrawRange.Checked)
            {
                RadioBtnDrawRange.Checked = true;
                DrawPolygonToolClass drawPolygon = new DrawPolygonToolClass(true, this);
                drawPolygon.OnCreate(axMapControl1.Object as IMapControlDefault);
                axMapControl1.CurrentTool = drawPolygon as ITool;
            }
            else
            {

                RadioBtnDrawRange.Checked = false;
            }
        }
        //从界面上选择范围
        private void RadioBtnSelectRange_CheckedChanged(object sender, EventArgs e)
        {
            //if (RadioBtnSelectRange.Checked)
            //{
            //    btnSelectRange.Enabled = true;
            //}
            //else
            //{
            //    btnSelectRange.Enabled = false;
            //}
        }

        private void axMapControl1_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            if (axMapControl1.Map.LayerCount == 0)
            {
                RadioBtnSelectRange.Checked = false;
                RadioBtnSelectRange.Enabled = false;
                btnSelectRange.Enabled = false;
            }
            else
            {
                IFeatureLayer pFeaLayer = null;
                for (int i = 0; i < axMapControl1.Map.LayerCount; i++)
                {
                    pFeaLayer = axMapControl1.Map.get_Layer(i) as IFeatureLayer;
                    if (pFeaLayer == null)
                    {
                        continue;
                    }
                    if (pFeaLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        m_FeaLayer = pFeaLayer;
                        break;
                    }
                }
                if (m_FeaLayer == null)
                {
                    RadioBtnSelectRange.Enabled = false;
                    btnSelectRange.Enabled = false;
                }
                else
                {
                    RadioBtnSelectRange.Enabled = true;
                }
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.FeaturecomboBox.Text))
                return;
            foreach (IDataset pDT in List)
            {
                IFeatureClass pFeatureCls = pDT as IFeatureClass;
                if (pFeatureCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    IFeatureLayer pFeaLayer = new FeatureLayerClass();
                    ILayer pLayer = null;

                    if (pFeatureCls.AliasName == this.FeaturecomboBox.Text.Trim())
                    {
                        axMapControl1.Map.ClearLayers();
                        axMapControl1.Refresh();
                        pFeaLayer.FeatureClass = pFeatureCls;
                        pLayer = pFeaLayer as ILayer;
                        axMapControl1.Map.AddLayer(pLayer);
                        IFeatureCursor cursor = pFeatureCls.Search(null, false);
                        IFeature pFeature = cursor.NextFeature();
                        IFeatureSelection pFeatureSelection = pFeaLayer as IFeatureSelection;
                        pFeatureSelection.Add(pFeature);
                        axMapControl1.ActiveView.Refresh();
                        btnSelectRange.Enabled = true;
                        break;
                    }
                    
                }
            }
        }
    }
}