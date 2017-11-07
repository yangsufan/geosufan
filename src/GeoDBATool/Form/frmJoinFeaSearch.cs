using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public partial class frmJoinFeaSearch : DevComponents.DotNetBar.Office2007Form
    {
        private double _dDisTo;
        private double _dSeacherTo;
        private double _dAngleTo;
        private double _dLengthTo;
        private XmlElement _ConnectInfo;
        private　SysCommon.Gis.SysGisDataSet pGisDT = null;        //////接边图层连接对象
        private　SysCommon.Gis.SysGisDataSet MapFramepGisDT = null;//////图幅结合表连接对象
        private Dictionary<string ,List<string>> m_DicFieldsConctrolList;/////接边控制属性字段
        private List<string> _JoinLayerName;/////////////////////参与接边图层名称列表
        public List<string> JoinLayerName
        {
            get { return this._JoinLayerName; }
        }
        private string _MapFrameName;///////////////////////////图幅结合表图层名
        public string MapFrameName
        {
            get { return this._MapFrameName; }
        }

        private string _MapFrameField=string.Empty;///////////////图幅字段
        public string MapFrameField
        {
            get { return this._MapFrameField; }
        }

        private Dictionary<string ,List<string>> m_FieldDic;////////接边控制属性字段
        public Dictionary<string, List<string>> FieldDic
        {
            get { return this.m_FieldDic; }
        }

        public frmJoinFeaSearch()
        {
            InitializeComponent();
            this._JoinLayerName = null;
            this._MapFrameName = string.Empty;
            _JoinLayerName = new List<string>();
            _MapFrameName = string.Empty;
            rbServer.Checked = true;
            rbExistdata.Checked = true;
            this.list_JoinLayer.Items.Clear();
            this.com_Project.Items.Clear();
            this.com_DataBase.Items.Clear();
            ////////获取库体信息
            //XmlDocument XmlDoc = new XmlDocument();
            //if (File.Exists(ModData.v_projectXML))
            //{
            //    XmlDoc.Load(ModData.v_projectXML);
            //    if (null == XmlDoc)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工程信息失败！");
            //        return;
            //    }
            //    XmlNodeList ProjectNodeList = XmlDoc.SelectSingleNode(".//工程管理").ChildNodes;
            //    if (null != ProjectNodeList)
            //    {
            //        for (int i = 0; i < ProjectNodeList.Count; i++)
            //        {
            //            XmlElement nodeele = ProjectNodeList[i] as XmlElement;
            //            string projectName = nodeele.GetAttribute("名称");
            //            this.com_Project.Items.Add(projectName);
            //        }
            //    }
            //}
            try
            {
                DevComponents.AdvTree.Node ProjectNode = ModData.v_AppGIS.DataTree.Nodes[0];
                string projectName = ProjectNode.Text;
                this.com_Project.Items.Add(projectName);
                this.com_DataBase.Items.Add("现势库");
                this.com_DataBase.Items.Add("历史库");
            }
            catch
            {
            }
           
            this.m_DicFieldsConctrolList = new Dictionary<string, List<string>>();
        }

        private void frmConSet_Load(object sender, EventArgs e)
        {
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
            double dDisTo = -1;
            double dSeacherTo = -1;
            double dAngleTo = -1;
            double dLengthTo = -1;
            try
            {
                dDisTo = Convert.ToDouble(sDisTo);
                dSeacherTo = Convert.ToDouble(sSeacherTo);
                dAngleTo = Convert.ToDouble(sAngleTo);
                dLengthTo = Convert.ToDouble(sLengthTo);
            }
            catch (Exception er)
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边参数配置文件中参数不正确！");
                return;
            }
            this._dAngleTo = dAngleTo;
            this._dDisTo = dDisTo;
            this._dLengthTo = dLengthTo;
            this._dSeacherTo = dSeacherTo;
            try
            {
                this.com_Project.Items.Clear();
                this.com_DataBase.Items.Clear();
                DevComponents.AdvTree.Node ProjectNode = ModData.v_AppGIS.ProjectTree.Nodes[0];
                string projectName = ProjectNode.Text;
                this.com_Project.Items.Add(projectName);
                this.com_DataBase.Items.Add("现势库");
                this.com_DataBase.Items.Add("历史库");
            }
            catch
            {
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            Exception ex = null;
            GetConnectInfo(out ex);
            if (ex!=null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return;
            }
            GetDataBaseLayer();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           //////从库体中选择图幅结合表
            if (rbExistdata.Checked == true)
            {
                this.com_MapNoField.Items.Clear();
                this.com_MapFrameList.Text = "";
                this.com_MapFrameList.Items.Clear();
                if (this.list_JoinLayer.Items.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
                {
                    this.com_MapFrameList.Items.Add(this.list_JoinLayer.Items[i].ToString());
                }
                MapFramepGisDT = pGisDT;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            /////从外部mdb文件选择图幅结合表
            Exception ex = null;
            this.com_MapFrameList.Text = "";
            if (rbOutdata.Checked == true)
            {
                this.com_MapNoField.Items.Clear();
                this.com_MapFrameList.Text = "";
                this.com_MapFrameList.Items.Clear();
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Title = "选择任务范围";
                openFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                if (openFile.ShowDialog() != DialogResult.OK)
                {
                    rbExistdata.Checked = true;
                    return;
                }

                label_loadState.Visible = true;
                label_loadState.Text = "正在获取图层";
                System.Windows.Forms.Application.DoEvents();
                string smdbPah = openFile.FileName;
                MapFramepGisDT = new SysCommon.Gis.SysGisDataSet();
                MapFramepGisDT.SetWorkspace(smdbPah, SysCommon.enumWSType.PDB, out ex);
                List<IDataset> LstDT = MapFramepGisDT.GetAllFeatureClass();
                List<ILayer> LstLayer = new List<ILayer>();
                for (int i = 0; i < LstDT.Count; i++)
                {
                    IFeatureClass pFeaCls = LstDT[i] as IFeatureClass;
                    if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation) continue;
                    if (pFeaCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        //只添面图层
                        IFeatureLayer pFeaLayer = new FeatureLayerClass();
                        pFeaLayer.FeatureClass = pFeaCls;
                        ILayer pLayer = pFeaLayer as ILayer;
                        if (!LstLayer.Contains(pLayer))
                        {
                            LstLayer.Add(pLayer);
                            label_loadState.Text = "正在添加图层：" + pLayer.Name;
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }
                }
                if (LstLayer.Count == 0)
                {
                    return;
                }
                //在图层列表中添加待选择图层

                for (int j = 0; j < LstLayer.Count; j++)
                {
                    string layername = ((LstLayer[j] as IFeatureLayer).FeatureClass as IDataset).Name;
                    this.com_MapFrameList.Items.Add(layername);
                }
                if (com_MapFrameList.Items.Count > 0) com_MapFrameList.SelectedIndex = 0;//默认选择第一个 xisheng
                label_loadState.Visible = false;

            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (this.list_JoinLayer.Items.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有接边图层!");
                return;
            }

            bool IsSelect = false;
            for (int i = 0; i < list_JoinLayer.Items.Count; i++)
            {
                CheckState ChecStste = list_JoinLayer.GetItemCheckState(i);
                if (ChecStste == CheckState.Checked)
                {
                    IsSelect = true;
                    break;
                }
            }
            if (!IsSelect)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有接边图层!");
                return;
            }

            if (string.IsNullOrEmpty(this.com_MapFrameList.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有选择图幅范围数据！");
                // this.DialogResult = DialogResult.Abort;
                return;
            }
            if (string.IsNullOrEmpty(this.com_MapNoField.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有选择图幅范围数据的图幅号字段！");
                // this.DialogResult = DialogResult.Abort;
                return;
            }
            this._MapFrameField = this.com_MapNoField.Text.Trim();
            //////将图层添加到地图控件上
            Exception ex = null; 
            this._JoinLayerName = new List<string>();
            string Fname = this.com_MapFrameList.Text.Trim();
           
            //if (rbServer.Checked == true)/////将库体中的图层添加到地图控件上
            //{
            //    #region 显示接边图层
            //    ModData.v_AppGIS.ArcGisMapControl.ClearLayers();
            //    for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
            //    {
            //        if (this.list_JoinLayer.GetItemChecked(i) == true)
            //        {
                        
            //            string layerName=this.list_JoinLayer.Items[i].ToString().Trim();                        
            //            IFeatureClass FeaCls = this.pGisDT.GetFeatureClass(layerName,out ex);
            //            if (null == ex)
            //            {
            //                IFeatureLayer Fealayer = new FeatureLayerClass();
            //                Fealayer.FeatureClass = FeaCls;
            //                Fealayer.Name = (FeaCls as IDataset).Name;
            //                this._JoinLayerName.Add(layerName);
            //                ModData.v_AppGIS.ArcGisMapControl.AddLayer(Fealayer as ILayer);
            //            }
            //        }
            //    }
            //    #endregion
            //    #region 显示图幅范围图层

            //    if (this._JoinLayerName.Contains(Fname))
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边图层中存在与图幅范围图层同名图层！");
            //        //this.DialogResult = DialogResult.Abort;
            //        return;
            //    }
            //    IFeatureClass MapFeaCls = null;
            //    if (rbExistdata.Checked == true)
            //    {
            //        MapFeaCls = this.pGisDT.GetFeatureClass(Fname, out ex);
            //    }
            //    else
            //    {
            //        MapFeaCls = this.MapFramepGisDT.GetFeatureClass(Fname, out ex);
            //    }
            //    if (null == ex)
            //    {
            //        IFeatureLayer Fealayer = new FeatureLayerClass();
            //        Fealayer.FeatureClass = MapFeaCls;
            //        Fealayer.Name = (MapFeaCls as IDataset).Name;
            //        ILayerEffects EffLayer = Fealayer as ILayerEffects;
            //        if (EffLayer.SupportsTransparency)
            //            EffLayer.Transparency = 30;
            //        ModData.v_AppGIS.ArcGisMapControl.AddLayer(Fealayer as ILayer, 0);
            //    }
           
            //    #endregion
            //}
            //else if (rbMapLayer.Checked == true)//////在地图控件上获取接边图层
            //{
                #region 在地图控件上获取接边图层
                this._JoinLayerName = new List<string>();
                for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
                {
                    if (this.list_JoinLayer.GetItemChecked(i) == true)
                    {
                        string layerName = this.list_JoinLayer.Items[i].ToString().Trim();
                        this._JoinLayerName.Add(layerName);                       
                    }
                }
                #endregion
                #region 显示图幅范围图层
                if (rbExistdata.Checked == true)
                {
                    this._MapFrameName = Fname;
                }
                else if (rbOutdata.Checked == true)
                {
                    for (int i = 0; i < this.list_JoinLayer.Items.Count;i++ )
                    {
                        if (this.list_JoinLayer.Items[i].ToString().Trim() == Fname)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边图层中存在与图幅范围图层同名图层！");
                            return;
                        }
                    }
                    IFeatureClass MapFeaCls = this.MapFramepGisDT.GetFeatureClass(Fname, out ex);
                    if (null == ex)
                    {
                        IFeatureLayer Fealayer = new FeatureLayerClass();
                        Fealayer.FeatureClass = MapFeaCls;
                        Fealayer.Name = (MapFeaCls as IDataset).Name;
                        ILayerEffects EffLayer = Fealayer as ILayerEffects;
                        if (EffLayer.SupportsTransparency)
                            EffLayer.Transparency = 30;
                        ModData.v_AppGIS.ArcGisMapControl.AddLayer(Fealayer as ILayer, 0);
                    }
                }


                #endregion
            //}
            this._MapFrameName = Fname;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
        /// <summary>
        /// 通过工程获取图层列表
        /// </summary>
        /// <param name="ex"></param>
        private void GetConnectInfo(out Exception ex)
        {
            ex = null;
            if (string.IsNullOrEmpty(this.com_Project.Text))
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个工程！");
                return;
            }
            if (string.IsNullOrEmpty(this.com_DataBase.Text))
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个库体!");
                return;
            }
            string ProjectName = this.com_Project.Text.Trim();
            string DataBaseName = this.com_DataBase.Text.Trim();
            //////////////获取相应的库体的连接信息
            string type = string.Empty;
            string server = string.Empty;
            string servername = string.Empty;
            string databasepath = string.Empty;
            string user = string.Empty;
            string password = string.Empty;
            string version = string.Empty;
            /////////////////
            XmlDocument XmlDoc = new XmlDocument();
            //cyf 20110713 modify
            XmlDoc.Load(ModData.v_projectDetalXML);
            //XmlDoc.Load(ModData.v_projectXML);
            if (null == XmlDoc)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工程信息失败！");
                return;
            }
            XmlElement ProjectEle = XmlDoc.SelectSingleNode(".//工程管理/工程[@名称='" + ProjectName + "']") as XmlElement;
            if (ProjectEle == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库工程记录文件中无法找到工程：" + ProjectName);
                return;
            }
            XmlElement DataBaseEle = ProjectEle.SelectSingleNode(".//内容/" + DataBaseName) as XmlElement;
            if (null == DataBaseEle)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库工程记录文件中无法找到工程：" + ProjectName + "中库体：" + DataBaseName);
                return;
            }
            XmlElement ConnectInfo = DataBaseEle.SelectSingleNode(".//连接信息") as XmlElement;
            this._ConnectInfo = ConnectInfo;
            XmlElement Base = DataBaseEle.SelectSingleNode(".//连接信息/库体") as XmlElement;
            type = ConnectInfo.GetAttribute("类型");
            server = ConnectInfo.GetAttribute("服务器");
            servername = ConnectInfo.GetAttribute("服务名");
            databasepath = ConnectInfo.GetAttribute("数据库");
            user = ConnectInfo.GetAttribute("用户");
            password = ConnectInfo.GetAttribute("密码");
            version = ConnectInfo.GetAttribute("版本");
            /////////////////////////////使用连接信息进行连接数据库
            pGisDT = new SysCommon.Gis.SysGisDataSet();
            if (string.IsNullOrEmpty(type))
            {
                ex = new Exception("库体未初始化！");
                return;
            }
            if (type == "SDE")
            {
                pGisDT.SetWorkspace(server, servername, databasepath, user, password, version, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else if (type == "GDB")
            {
                pGisDT.SetWorkspace(databasepath, SysCommon.enumWSType.GDB, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else if (type == "PDB")
            {
                pGisDT.SetWorkspace(databasepath, SysCommon.enumWSType.PDB, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }          
        }
        /// <summary>
        /// 获取数据库的图层列表
        /// </summary>
        private void GetDataBaseLayer()
        {
            if (this._ConnectInfo == null)
                return;
            //////////////获取相应的库体的连接信息
            string type = string.Empty;
            string server = string.Empty;
            string servername = string.Empty;
            string databasepath = string.Empty;
            string user = string.Empty;
            string password = string.Empty;
            string version = string.Empty;
            /////////////////

            XmlElement ConnectInfo = this._ConnectInfo;
            try
            {
                type = ConnectInfo.GetAttribute("类型");
                server = ConnectInfo.GetAttribute("服务器");
                servername = ConnectInfo.GetAttribute("服务名");
                databasepath = ConnectInfo.GetAttribute("数据库");
                user = ConnectInfo.GetAttribute("用户");
                password = ConnectInfo.GetAttribute("密码");
                version = ConnectInfo.GetAttribute("版本");
            }
            catch (Exception e)
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取连接信息失败！");
                return;
            }
            /////////////////////////////使用连接信息进行连接数据库
            pGisDT = new SysCommon.Gis.SysGisDataSet();
            if (string.IsNullOrEmpty(type))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "库体未初始化！");
                return;
            }
            Exception ex = null;
            if (type == "SDE")
            {
                pGisDT.SetWorkspace(server, servername, databasepath, user, password, version, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else if (type == "GDB")
            {
                pGisDT.SetWorkspace(databasepath, SysCommon.enumWSType.GDB, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else if (type == "PDB")
            {
                pGisDT.SetWorkspace(databasepath, SysCommon.enumWSType.PDB, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }

            if (rbServer.Checked == true)
            {
                //获得数据库中的所有要素类添加到图层列表中供用户选择
                List<IDataset> LstDT = pGisDT.GetAllFeatureClass();
                List<ILayer> LstLayer = new List<ILayer>();
                for (int i = 0; i < LstDT.Count; i++)
                {
                    IFeatureClass pFeaCls = LstDT[i] as IFeatureClass;
                    //*************************************************
                    //////判断用户库与历史库
                    if (this.com_DataBase.Text == "现势库")
                    {
                        if (LstDT[i].Name.EndsWith("_GOH") || LstDT[i].Name.EndsWith("_GOH"))
                            continue;
                    }
                    else if (this.com_DataBase.Text == "历史库")
                    {
                        if (!LstDT[i].Name.EndsWith("_GOH") && !LstDT[i].Name.EndsWith("_GOH"))
                            continue;
                    }
                    else continue;
                    //************************************************

                    if (pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation) continue;
                    if (pFeaCls.ShapeType == esriGeometryType.esriGeometryPolyline || pFeaCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        //只添加线与面的图层
                        IFeatureLayer pFeaLayer = new FeatureLayerClass();
                        pFeaLayer.FeatureClass = pFeaCls;
                        ILayer pLayer = pFeaLayer as ILayer;
                        if (!LstLayer.Contains(pLayer))
                        {
                            LstLayer.Add(pLayer);
                        }
                    }
                }
                if (LstLayer.Count == 0)
                {
                    return;
                }
                //在图层列表中添加待选择图层
                this.list_JoinLayer.Items.Clear();
                for (int j = 0; j < LstLayer.Count; j++)
                {
                    string layername = ((LstLayer[j] as IFeatureLayer).FeatureClass as IDataset).Name;
                    this.list_JoinLayer.Items.Add(layername);
                }
                if (rbExistdata.Checked == true)
                {
                    this.com_MapFrameList.Items.Clear();
                    if (this.list_JoinLayer.Items.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
                    {
                        this.com_MapFrameList.Items.Add(this.list_JoinLayer.Items[i].ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 图幅结合表图层来源选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com_MapFrameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Exception ex=null;
            if (string.IsNullOrEmpty(this.com_MapFrameList.Text))
                return;
            string MapFName = this.com_MapFrameList.Text.Trim();
            IFeatureClass MapFeacls = null;
            if (rbExistdata.Checked == true)///已有数据中获取图幅结合图层
            {
                if (rbServer.Checked == true)////库体数据中获取
                {
                   MapFeacls= this.pGisDT.GetFeatureClass(MapFName, out ex);
                    if (null != ex)
                        return;
                }
                else if (rbMapLayer.Checked == true)////图层数据中获取
                {
                    ILayer GetLayer = GetLayAtMapByName(MapFName);
                    IFeatureLayer GetFeaLayer=GetLayer as IFeatureLayer;
                    if (null==GetFeaLayer){ex =new Exception("获取图幅结合表图层失败");return;}
                    MapFeacls = GetFeaLayer.FeatureClass;
                }               
            }
            else if (rbOutdata.Checked == true) /////外部数据中获取
            {
                if (this.MapFramepGisDT == null)
                    return;
                MapFeacls = this.MapFramepGisDT.GetFeatureClass(MapFName, out ex);
                if (null != ex)
                    return;
            }
            /////遍历获取图幅结合表图层的字段，添加到列表中
            if (null != MapFeacls)
            {
                this.com_MapNoField.Items.Clear();
                for (int i = 0; i < MapFeacls.Fields.FieldCount; i++)
                {
                    if (MapFeacls.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeOID || MapFeacls.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                        continue;
                    this.com_MapNoField.Items.Add(MapFeacls.Fields.get_Field(i).Name.Trim());
                }
            }
        }

        private void addMapFrameLayer()//////将图幅结合表的图层显示在地图控件上
        {
            Exception ex = null;
            if (string.IsNullOrEmpty(this.com_MapFrameList.Text))
                return;
            string MapFName = this.com_MapFrameList.Text.Trim();
            IFeatureClass MapFeacls = null;
            if (rbExistdata.Checked == true)///已有数据中获取图幅结合图层
            {
                if (rbServer.Checked == true)////库体数据中获取
                {
                    MapFeacls = this.pGisDT.GetFeatureClass(MapFName, out ex);
                    if (null != ex)
                        return;
                }
                else if (rbMapLayer.Checked == true)////图层数据中获取
                {
                    int layercount = ModData.v_AppGIS.ArcGisMapControl.LayerCount;
                    if (layercount == 0)
                        return;
                    for (int i = 0; i < layercount; i++)
                    {
                        IFeatureLayer Fealayer = ModData.v_AppGIS.ArcGisMapControl.get_Layer(i) as IFeatureLayer;
                        if ((Fealayer.FeatureClass as IDataset).Name == MapFName)
                        {
                            MapFeacls = Fealayer.FeatureClass;
                            break;
                        }
                    }
                }
            }
            else if (rbOutdata.Checked == true) /////外部数据中获取
            {
                if (this.MapFramepGisDT == null)
                    return;
                MapFeacls = this.MapFramepGisDT.GetFeatureClass(MapFName, out ex);
                if (null != ex)
                    return;
            }
            /////加载图层
            if (null != MapFeacls)
            {
                
            }
        }

        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            if (this.list_JoinLayer.Items.Count == 0)
                return;
            for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
            {
                this.list_JoinLayer.SetItemChecked(i, true);
            }
        }

        private void btn_Reverse_Click(object sender, EventArgs e)
        {
            if (this.list_JoinLayer.Items.Count == 0)
                return;
            for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
            {
                if (this.list_JoinLayer.GetItemChecked(i))
                    this.list_JoinLayer.SetItemChecked(i, false);
                else
                    this.list_JoinLayer.SetItemChecked(i, true);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (this.list_JoinLayer.Items.Count == 0)
                return;
            for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
            {
                this.list_JoinLayer.SetItemChecked(i, false);
            }
        }

        private void btn_ControlFields_Click(object sender, EventArgs e)
        {
            List<IFeatureClass> list_Fea = new List<IFeatureClass>();
            Exception ex = null;
            //////////获取选中的接边图层///////////
            //if (rbServer.Checked == true)/////库体图层
            //{
            //    #region 获取图层
            //    for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
            //    {
            //        if (this.list_JoinLayer.GetItemChecked(i) == true)
            //        {

            //            string layerName = this.list_JoinLayer.Items[i].ToString().Trim();
            //            IFeatureClass FeaCls = this.pGisDT.GetFeatureClass(layerName, out ex);
            //            if (null == ex)
            //            {
            //                list_Fea.Add(FeaCls);
            //            }
            //        }
            //    }
            //    #endregion              
            //}
            //else if (rbMapLayer.Checked == true)//////地图控件图层
            //{
                #region 获取图层
                this._JoinLayerName = new List<string>();
                for (int i = 0; i < this.list_JoinLayer.Items.Count; i++)
                {
                    if (this.list_JoinLayer.GetItemChecked(i) == true)
                    {
                        string layerName = this.list_JoinLayer.Items[i].ToString().Trim();
                        for (int j = 0; j < ModData.v_AppGIS.MapControl.LayerCount; j++)
                        {
                            if (layerName == ModData.v_AppGIS.MapControl.get_Layer(j).Name)
                            {
                                ILayer getlayer = ModData.v_AppGIS.MapControl.get_Layer(j);
                                IFeatureLayer fea = getlayer as IFeatureLayer;
                                if (fea != null)
                                {
                                    IFeatureClass getFeaCls = fea.FeatureClass;
                                    if (getFeaCls != null) list_Fea.Add(getFeaCls);
                                }
                                
                            }
                        }
                    }
                }
                #endregion
            //}
            ///////////获取每个图层的控制属性字段///////
            if (list_Fea.Count > 0)
            {
                Dictionary<string, List<string>> GetFields = new Dictionary<string, List<string>>();
                frmSetControlFields setFrm = new frmSetControlFields(list_Fea, this.m_FieldDic);
                if (DialogResult.Yes == setFrm.ShowDialog())
                {
                    this.m_FieldDic = setFrm.FieldDic;
                }

            }
        }
        //***************************************************************************
        //guozheng added 2010-12-30
        //***************************************************************************
        /// <summary>
        /// 通过一个名称获取地图控件上的一个图层
        /// </summary>
        /// <param name="LayerName">图层名</param>
        /// <returns>找到返回ILayer，找不到返回NULL</returns>
        private ILayer GetLayAtMapByName(string LayerName)
        {
            for (int i = 0; i < ModData.v_AppGIS.ArcGisMapControl.LayerCount; i++)
            {
                ILayer player = ModData.v_AppGIS.ArcGisMapControl.get_Layer(i);
                if (null == player) continue;
                if (player is IGroupLayer)
                {
                    ILayer GetLayer= GetLayerAtGropLayer(LayerName, player as IGroupLayer);
                    if (GetLayer != null) return GetLayer;
                 }
                else 
                {
                    IFeatureLayer GetFeaLayer = player as IFeatureLayer;
                    if (GetFeaLayer == null) continue;
                    else if ((GetFeaLayer.FeatureClass as IDataset).Name == LayerName)
                        return player;
                }
            }
            return null;
        }
        //***************************************************************************
        //guozheng added 2010-12-30
        //***************************************************************************
        /// <summary>
        /// 在GroupLayer中通过吗名称获取一个图层（递归）
        /// </summary>
        /// <param name="LayerName">图层名</param>
        /// <param name="pGroupLayer">IGroupLayer对象</param>
        /// <returns>找到返回ILayer，找不到返回NULL</returns>
        private ILayer GetLayerAtGropLayer(string LayerName,IGroupLayer pGroupLayer)
        {
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            if (null == pComLayer) return null;
            int ComCount = pComLayer.Count;
            for (int i = 0; i < ComCount; i++)
            {
                ILayer pLayer = pComLayer.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ILayer GetLayer = GetLayerAtGropLayer(LayerName, pLayer as IGroupLayer);
                    if (null != GetLayer) return GetLayer;
                }
                else
                {
                    IFeatureLayer GetFeaLayer = pLayer as IFeatureLayer;
                    if (null == GetFeaLayer) continue;
                    if ((GetFeaLayer.FeatureClass as IDataset).Name == LayerName)
                        return pLayer;
                }
            }
            return null;
        }

        private void list_JoinLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int CruIndex = this.list_JoinLayer.SelectedIndex;
                if (this.list_JoinLayer.GetItemChecked(CruIndex))
                    this.list_JoinLayer.SetItemChecked(CruIndex, false);
                else
                    this.list_JoinLayer.SetItemChecked(CruIndex, true);
            }
            catch
            {
            }
        }

        private void btn_Loaddata_Click(object sender, EventArgs e)
        {
            this.list_JoinLayer.Items.Clear();
            this.com_MapNoField.Items.Clear();
            this.com_MapFrameList.Items.Clear();
            if (rbExistdata.Checked == true)
            {
                this.com_MapFrameList.Items.Clear();
            }
            int layerCont = ModData.v_AppGIS.ArcGisMapControl.LayerCount;
            rbMapLayer.Checked = true;
            if (layerCont == 0)
            {
                MessageBox.Show("请先加载临时库数据到地图中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int count = 0;
            if (layerCont > 0)
            {
                for (int j = 0; j < layerCont; j++)
                {
                    ILayer layer = ModData.v_AppGIS.ArcGisMapControl.get_Layer(j);
                    if (layer is IGroupLayer)
                    {
                        ICompositeLayer pComLayer = (ICompositeLayer)layer;
                        int ComCount = pComLayer.Count;
                        for (int i = 0; i < ComCount; i++)
                        {
                            IFeatureLayer pLayer = pComLayer.get_Layer(i) as IFeatureLayer;
                            if (null == pLayer) continue;
                            IFeatureClass FeaClss = pLayer.FeatureClass;
                            if (null == FeaClss)
                                continue;
                            if (FeaClss.ShapeType == esriGeometryType.esriGeometryPolygon || FeaClss.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                this.list_JoinLayer.Items.Add((FeaClss as IDataset).Name);
                                this.list_JoinLayer.SetItemChecked(count, true);
                                count++;
                                if (rbExistdata.Checked == true)
                                {
                                    this.com_MapFrameList.Items.Add((FeaClss as IDataset).Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        IFeatureLayer FeaLayer = layer as IFeatureLayer;
                        if (null == FeaLayer) continue;
                        IFeatureClass FeaClss = FeaLayer.FeatureClass;
                        if (null == FeaClss)
                            continue;
                        if (FeaClss.ShapeType == esriGeometryType.esriGeometryPolygon || FeaClss.ShapeType == esriGeometryType.esriGeometryPolyline)
                        {
                            this.list_JoinLayer.Items.Add((FeaClss as IDataset).Name);
                            this.list_JoinLayer.SetItemChecked(count, true);
                            count++;
                            if (rbExistdata.Checked == true)
                            {
                                this.com_MapFrameList.Items.Add((FeaClss as IDataset).Name); ;
                            }
                        }
                    }
                }

            }
        }

        private void com_MapNoField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}