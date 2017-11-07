using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    public partial class FrmGetGeometry : DevComponents.DotNetBar.Office2007Form
    {
        //**************************************************************
        //guozheng 2011-1-24 added 
        // 该类实现在图层控件中加载一个图层，并在图层中选择一个要素作为图幅批量更新的范围
        //
        //**************************************************************
        private IHookHelper m_HookHelp = null;/////////////主运用程序HOOK
        private IGeometry m_ResMapFrame = null;////////////获取的图幅范围
        public IGeometry ResMapFrame
        {
            get { return this.m_ResMapFrame; }
        }
        private IFeatureClass m_MapFrameClass = null;//////图幅结合图层的FeatureClass
        public IFeatureClass MapFrameClass
        {
            get{ return this.m_MapFrameClass;}
        }
        private List<IFeatureClass> m_Layers = null;///////图层集合
        private IFeatureDataset m_WorkFeatureDataset = null;/////////////工作库的Workspace

        public string IP
        {
            get { return this.txt_Ip.Text.Trim(); }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="in_Layers">图层集合</param>
        /// <param name="in_Hook">主程序Hook</param>
        /// <param name="in_WorkSpace">工作库的IFeatureDataset</param>
        public FrmGetGeometry(List<IFeatureClass> in_Layers, IHookHelper in_Hook, IFeatureDataset in_FeatureDataset)
        {
            InitializeComponent();
            /////////将图层添加至下拉列表中
            this.m_HookHelp = in_Hook;
            this.m_Layers = new List<IFeatureClass>();
            foreach (IFeatureClass GetFeaCls in in_Layers)
            {
                if (GetFeaCls.ShapeType != esriGeometryType.esriGeometryPolygon) continue;
                this.combox_layers.Items.Add((GetFeaCls as IDataset).Name);
                this.m_Layers.Add(GetFeaCls);
            }
            ////////默认选中一个
            if (this.combox_layers.Items.Count > 0) this.combox_layers.SelectedIndex = 0;
            string sSelectLayerName = this.combox_layers.Text.Trim();
            IFeatureClass GetLayerFeacls = GetFeaClsByName(sSelectLayerName);
            AddALayer(GetLayerFeacls);
            this.m_WorkFeatureDataset = in_FeatureDataset;

        }
        public FrmGetGeometry()
        {
            InitializeComponent();
        }

        private void AddALayer(IFeatureClass in_FeaCls)
        {
            if (in_FeaCls == null) return;
            IFeatureLayer pFeaLayer = new FeatureLayerClass();
            pFeaLayer.FeatureClass = in_FeaCls;
            ILayer pNewLayer = pFeaLayer as ILayer;
            pNewLayer.Name = (in_FeaCls as IDataset).Name;
            /////////加载图层
            this.axMapControl1.ClearLayers();
            this.axMapControl1.AddLayer(pNewLayer);

        }
        /// <summary>
        /// 选择图层变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combox_layers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelectIndex = this.combox_layers.SelectedIndex;
            string sLayerName = this.combox_layers.Text.Trim();
            try
            {
                IFeatureClass GetLayerFeacls = GetFeaClsByName(sLayerName);
                AddALayer(GetLayerFeacls);
            }
            catch (Exception ex)
            {
                //******************************************
                //系统运行日志
                if (ModData.SysLog == null)
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(ex);
                //******************************************
            }
        }
        /// <summary>
        /// 通过名称获取图层列表中的图层
        /// </summary>
        /// <param name="in_sLayerName">图层名</param>
        /// <returns>没有返回NULL</returns>
        private IFeatureClass GetFeaClsByName(string in_sLayerName)
        {
            foreach (IFeatureClass GetFeaCls in m_Layers)
            {
                ///////循环所有名称
                if ((GetFeaCls as IDataset).Name == in_sLayerName) return GetFeaCls;
            }
            return null;
        }
        /// <summary>
        /// OK按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            //////在界面的图层中获取选中的图幅的Feature的Geometry
            if (string.IsNullOrEmpty(this.txt_Ip.Text.Trim()))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","请输入发布服务器的IP地址");
                this.txt_Ip.Focus();
                return;
            }
            if (m_ResMapFrame == null)
            {
                ////////没有导入范围
                IFeatureLayer pFeaLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                IFeatureSelection pFeaSel = pFeaLayer as IFeatureSelection;
                this.m_MapFrameClass = pFeaLayer.FeatureClass;
                if (pFeaSel.SelectionSet == null || pFeaSel.SelectionSet.Count == 0)
                {
                    MessageBox.Show("请在图上选择范围！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (pFeaSel.SelectionSet.Count > 1)
                {
                    MessageBox.Show("请只选择一个图幅", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                m_ResMapFrame = GetFeaLayerGeometry(pFeaSel, pFeaLayer);
                if (m_ResMapFrame == null)
                {
                    MessageBox.Show("请设置图幅数据范围！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
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

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
        /// <summary>
        /// 从TxT文件导入图副范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_getRangeByText_Click(object sender, EventArgs e)
        {
            Exception ex = null;
            if (string.IsNullOrEmpty(this.txt_Ip.Text.Trim()))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入发布服务器的IP地址");
                this.txt_Ip.Focus();
                return;
            }
            string m_sTxtRangeFileName = string.Empty;/////////导入的txt文件名
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择图形范围坐标txt";
            OpenFile.Filter = "图形范围坐标文本(*.txt)|*.txt";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                m_sTxtRangeFileName = OpenFile.FileName;
                StringBuilder sb = new StringBuilder();
                try
                {
                    StreamReader sr = new StreamReader(OpenFile.FileName);
                    while (sr.Peek() >= 0)
                    {
                        string[] strTemp = sr.ReadLine().Split(',');
                        if (sb.Length != 0)
                        {
                            sb.Append(",");
                        }
                        for (int j = 0; j < strTemp.Length - 1; j=j+2)
                        {
                            //////循环所有坐标值添加到记录中
                            sb.Append(strTemp[j] + "@" + strTemp[j+1]);
                            if (j < strTemp.Length - 2)
                            {
                                sb.Append(',');
                            }
                        }
                    }
                }
                catch (Exception eError)
                {
                    //******************************************
                    //系统运行日志
                    if (ModData.SysLog == null)
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eError);
                    //******************************************
                    MessageBox.Show("导入范围失败\n:原因：" + eError.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (sb.Length == 0) return;
                m_ResMapFrame = GetPolygonByCol(sb.ToString(),out ex) as IGeometry;
                if (ex != null)
                {
                    MessageBox.Show("导入的文件获取范围信息失败\n:原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
                if (m_ResMapFrame == null)
                {
                    MessageBox.Show("导入的文件获取范围信息失败", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (m_ResMapFrame.IsEmpty)
                {
                    MessageBox.Show("获取的几何范围为空，范围文件格式有误", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //CreateMapFrameLayerInWorkSpace(m_ResMapFrame, out ex);
                if (ex != null)
                {
                    MessageBox.Show("将范围记录至图副范围图层失败\n:原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
           
        }


        /// <summary>
        /// 从坐标字符串得到范围Polygon
        /// </summary>
        /// <param name="strCoor">坐标字符串,格式为X@Y,以逗号分割</param>
        /// <returns></returns>
        private IPolygon GetPolygonByCol(string strCoor,out Exception ex)
        {
            ex = null;
            try
            {
                object after = Type.Missing;
                object before = Type.Missing;
                IPolygon polygon = new PolygonClass();
                IPointCollection pPointCol = (IPointCollection)polygon;
                string[] strTemp = strCoor.Split(',');
                for (int index = 0; index < strTemp.Length; index++)
                {
                    string CoorLine = strTemp[index];
                    string[] coors = CoorLine.Split('@');

                    double X = Convert.ToDouble(coors[0]);
                    double Y = Convert.ToDouble(coors[1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(X, Y);
                    pPointCol.AddPoint(pPoint, ref before, ref after);
                }

                polygon = (IPolygon)pPointCol;
                polygon.Close();

                ITopologicalOperator pTopo = (ITopologicalOperator)polygon;
                pTopo.Simplify();

                return polygon;
            }
            catch (Exception e)
            {
                //******************************************
                //系统运行日志
                if (ModData.SysLog == null)
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(e);
                //******************************************
                ex = e;
                return null;
            }
        }

        /// <summary>
        /// 将导入的范围在工作库中建立一个图副范围图层，
        /// </summary>
        /// <param name="in_pMapRange"></param>
        /// <param name="ex"></param>
        private void CreateMapFrameLayerInWorkSpace(IGeometry in_pMapRange, out Exception ex)
        {
            ex = null;
            ////////第一步：获取工作库的Workspace
            //if (this.m_WorkFeatureDataset == null) { ex = new Exception("工作库的工作空间不能为空"); return; }            
            ////////第二步：在工作空间中建立一个图副范围图层
            //try
            //{
            //    IFields fields = new FieldsClass();
            //    IFieldsEdit fsEdit = fields as IFieldsEdit;
            //    //添加Object字段
            //    IField newfield2 = new FieldClass();
            //    IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
            //    fieldEdit2.Name_2 = "OBJECTID";
            //    fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
            //    fieldEdit2.AliasName_2 = "OBJECTID";
            //    newfield2 = fieldEdit2 as IField;
            //    fsEdit.AddField(newfield2);

            //    //添加Geometry字段
            //    IField newfield1 = new FieldClass();
            //    IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
            //    fieldEdit1.Name_2 = "SHAPE";
            //    fieldEdit1.Type_2 = esriFieldType.esriFieldTypeGeometry;
            //    IGeometryDef geoDef = new GeometryDefClass();

                
            //    IGeometryDefEdit geoDefEdit = geoDef as IGeometryDefEdit;
            //    geoDefEdit.SpatialReference_2 = (this.m_WorkFeatureDataset as IGeoDataset).SpatialReference;
            //    geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            //    fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
            //    newfield1 = fieldEdit1 as IField;
            //    fsEdit.AddField(newfield1);
            //    fields = fsEdit as IFields;
            //    ////////建立要素集
            //    IFeatureClass pMapFrameFeaCls = null;
            //    pMapFrameFeaCls = ModDBOperator.GetFeaClsSetInEnum("MapFrameLayer", this.m_WorkFeatureDataset.Subsets);
            //    if (pMapFrameFeaCls == null)
            //    {
            //        //////不存在则建立
            //        pMapFrameFeaCls = this.m_WorkFeatureDataset.CreateFeatureClass("MapFrameLayer", fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
            //    }

            //    ////////第三部将图副范写入图副范围图层////////
            //    IWorkspaceEdit WsEdit = this.m_WorkFeatureDataset.Workspace as IWorkspaceEdit;
            //    WsEdit.StartEditing(true);
            //    WsEdit.StartEditOperation();

            //    IFeature NewFea = pMapFrameFeaCls.CreateFeature();
            //    NewFea.Shape = in_pMapRange;
            //    NewFea.Store();

            //    WsEdit.StopEditOperation();
            //    WsEdit.StopEditing(true);    
            //    ////////第四步：记录此图层，加载至图层控件中
            //    this.m_MapFrameClass = pMapFrameFeaCls;
            //    DeleteLayerByName("MapFrameLayer");
            //    IFeatureLayer NewMapFrameLayer = new FeatureLayerClass();
            //    NewMapFrameLayer.FeatureClass = pMapFrameFeaCls;
            //    NewMapFrameLayer.Name = "MapFrameLayer";
            //    this.m_HookHelp.FocusMap.AddLayer(NewMapFrameLayer as ILayer);
            //    this.m_HookHelp.FocusMap.MoveLayer((NewMapFrameLayer as ILayer),this.m_HookHelp.FocusMap.LayerCount);
            //}
            //catch (Exception eError)
            //{
            //    //******************************************
            //    //系统运行日志
            //    if (ModData.SysLog == null)
            //        ModData.SysLog = new clsWriteSystemFunctionLog();
            //    ModData.SysLog.Write(eError);
            //    //******************************************
            //    ex = eError;
            //}
        }

        /// <summary>
        /// 根据名称移除制定图层
        /// </summary>
        /// <param name="LayerName">图层名</param>
        private void DeleteLayerByName(string LayerName)
        {
            try
            {
                for (int i = 0; i < this.m_HookHelp.FocusMap.LayerCount; i++)
                {
                    ILayer GetLayer = this.m_HookHelp.FocusMap.get_Layer(i);
                    if (GetLayer.Name == LayerName)
                    {
                        this.m_HookHelp.FocusMap.DeleteLayer(GetLayer);
                        return;
                    }
                }
            }
            catch(Exception eError)
            {
                //******************************************
                //系统运行日志
                if (ModData.SysLog == null)
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError);
                //******************************************
            }
        }

        /// <summary>
        /// 导入图层(导入一个MDB中的所有图层)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InputLayer_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenMDBFrm = new OpenFileDialog();
            OpenMDBFrm.Filter = "ArcGis PDB 文件|*.mdb";
            if (DialogResult.OK == OpenMDBFrm.ShowDialog())
            {
                string sGetPDBFileName = OpenMDBFrm.FileName;
                try
                {
                    IWorkspace pWS = null;
                    IPropertySet pPropSet = new PropertySetClass();
                    AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                    pPropSet.SetProperty("DATABASE", sGetPDBFileName);
                    pWS = pAccessFact.Open(pPropSet, 0);
                    pAccessFact = null;
                    IEnumDatasetName pEnumDatasetName = pWS.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                    IDatasetName pDataSetName = pEnumDatasetName.Next();
                    if (m_Layers == null) m_Layers = new List<IFeatureClass>();
                    this.combox_layers.Items.Clear();
                    while (pDataSetName != null)
                    {
                        IFeatureClass pGetFeaCls = (pWS as IFeatureWorkspace).OpenFeatureClass(pDataSetName.Name);
                        if (pGetFeaCls != null)
                        {
                            m_Layers.Add(pGetFeaCls);
                            this.combox_layers.Items.Add(pDataSetName.Name);
                        }
                        pDataSetName = pEnumDatasetName.Next();
                    }
                    ////////将这些图层加载到界面上////////
                    ////////默认选中一个
                    if (this.combox_layers.Items.Count > 0) this.combox_layers.SelectedIndex = 0;
                    string sSelectLayerName = this.combox_layers.Text.Trim();
                    IFeatureClass GetLayerFeacls = GetFeaClsByName(sSelectLayerName);
                    AddALayer(GetLayerFeacls);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pEnumDatasetName);
                }
                catch(Exception eError)
                {
                    if (ModData.SysLog == null) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eError);
                }
            }
        }

    }
}
