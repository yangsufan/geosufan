using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SpatialAnalyst;
using SysCommon;

namespace GeoDataExport
{
    public partial class frmExportEx:DevComponents.DotNetBar.Office2007Form
    {
        //added by chulili  选择图层控件新增的属性
        private XmlDocument _LayerTreeXmldoc = null;
        private Dictionary<string, string> _DicLayerList = null;
        private int m_m = 0;//设置分类，是查询还是统计，判断是不是需要判断图层是否加载或者可查询
        private bool m_checkbox = false;//设置是否有checkbox
        private IMap m_Map = null;//设置Imap;
        private IMap m_Mapold = null;
        private List<DevComponents.AdvTree.Node> ListNode = new List<DevComponents.AdvTree.Node>();
        private bool flag = true;
        private bool flag2 = true;
        private bool tbclick = false;//是否点击同步按钮
        private string m_TempGDBPath = Application.StartupPath + "\\..\\Template\\TempXBM.gdb";//ygc 20130325 国家标准模板数据库

        public Dictionary<string, ILayer> m_DicLayer = new Dictionary<string, ILayer>(); //xisheng 用字典将图层和NodeKey绑到字典里，方便获取 20111203
        public IWorkspace m_TmpWorkSpace = null;//added by chulili 2011-11-30 从外部获取
        public IMap m_returnMap = new MapClass();
        public string m_NodeKey = "";//设置图层NodeKey属性。、
        public string m_NodeText = "";//图层名
        private SysCommon.SelectLayer.ClsSelectLayerByTree _ClsSelectLayerByTree = null;
        bool remove = false;//判断是否有移除
        //added by chulili end 

        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }

        //ZQ  20110804  add   读取图层属性信息
        private String _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树2.xml";
        //ygc 20130411 国标转换配置文件
        private string m_ChangeConfigPath = Application.StartupPath + "\\..\\res\\xml\\ChangeConfig.xml";
        //yjl20111012 add 行政区数据提取
        public string XZQCode
        {
            get;
            set;
        }
        //end
        #region   坐标转换的
        //定义几个类变量 这里主要考虑到里面的代码比较乱 不好每部都传变量 所以就定义了
        private bool m_blnEditCoor = false;//是否进行坐标的变换
        private double m_dblXoffset = 10;  //平移x
        private double m_dblYoffset = 10;  //平移y
        private double m_dblRote = 1;    //旋转角度
        private double m_dblCentX = 1;   //旋转原点的x坐标
        private double m_dblCentY = 1;   //选择原点的y坐标
        private bool boolGeometryExport = true;//added by chulili 2012-07-05 是否属于范围提取（山西新增不按范围提取）

        //对图形进行坐标放射变换
        private IGeometry EditCoor(IGeometry pGeometry, double dblXoff, double dblYoff, double dblRote, double dblPntX, double dblPntY)
        {
            IClone pClone = pGeometry as IClone;
            IGeometry pNewGeo = pClone.Clone() as IGeometry;

            try
            {
                ITransform2D pTrans = pNewGeo as ITransform2D;

                //进行下平移
                if (dblXoff != 0 || dblYoff != 0)
                {
                    pTrans.Move(dblXoff, dblYoff);
                }

                if (dblRote != 0)
                {
                    double dblTemp = dblRote * Math.PI / 180.0;
                    IPoint pPntCent = new PointClass();
                    pPntCent.PutCoords(dblPntX, dblPntY);

                    pTrans.Rotate(pPntCent, dblTemp);
                }

                IGeometry pGeo = pTrans as IGeometry;

                return pGeo;
            }
            catch
            {
                return pNewGeo;
            }
        }

        //调用函数
        private IGeometry GetNewGeometry(IGeometry pGeometry)
        {
            if (!m_blnEditCoor) return pGeometry;
            return EditCoor(pGeometry, m_dblXoffset, m_dblYoffset, m_dblRote, m_dblCentX, m_dblCentY);
        }
        #endregion
        //changed by chulili 2012-07-05 山西项目，全图提取，不传入范围
        public frmExportEx(IMap pmap, IGeometry pgeometry)
        {
            InitializeComponent();
            //Mapcontrol,geometry值传递
            pMap = pmap;
            pGeometry = pgeometry;
            int intIndex = GetColumnIndexOfdataGrid("剪裁");
            if (pGeometry != null)
            {
                dataGridLayers.Columns[intIndex].Visible = true;
                boolGeometryExport = true;//按照范围提取
            }
            else
            {
                dataGridLayers.Columns[intIndex].Visible = false;//不按范围提取，隐藏“剪裁”列
                boolGeometryExport = false;//不按照范围提取，即全图提取
            }
            m_m = 1;
            m_checkbox = true;
            m_Mapold = m_Map = pMap;
            m_TmpWorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            this.btn_TbCatalog.Visible = true;
            InitClsSelectLayerTree();
        }
        private int GetColumnIndexOfdataGrid(string strHeaderCell)
        {
            if (strHeaderCell == "")
            {
                return -1;
            }
            for (int i = 0; i < dataGridLayers.Columns.Count; i++)
            {
                if (dataGridLayers.Columns[i].HeaderCell.Value.ToString() == strHeaderCell)
                {
                    return i;
                }
            }
            return -1;
        }
        public IMap pMap;
        public IGeometry pGeometry;
        public int cellclickindex =new int();
         public   int layercount = 0;
        string saveworkspace = "";
        string savefilename = "";
        string savefiletype = "";
        public int rowindex = 0;
        public int shift = 0;
        public bool ctrl = false;
        public frmAddData addDataDialog;
        private IFeatureLayer pjiancaiFeaturelayer;
        public double m_area=0;

        private void frmExportEx_Load(object sender, EventArgs e)
        {
            LoadSelectLayerByTree();
        }
        
        /// <summary>
        /// 查询该图层是否可查询 added by xisheng 20110802
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns></returns>
        public bool GetIsQuery(ILayer layer)
        {
            ILayerGeneralProperties pLayerGenPro = layer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (strNodeXml.Equals(""))
            {
                return true;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//AboutShow");
            if (pxmlnode == null)
            {
                pXmldoc = null;
                return true;
            }
            string strNodeKey = pxmlnode.Attributes["IsQuery"].Value.ToString();
            if (strNodeKey.Trim().ToUpper() == "FALSE")
            {
                pXmldoc = null;
                return false;
            }
            else
            {
                pXmldoc = null;
                return true;
            }

        }
        /// <summary>
        /// 查询图层的父节点   ZQ  20110804   add
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private XmlNode GetNode(IFeatureLayer layer)
        {
            XmlNode xmlNode = null;
            ILayerGeneralProperties pLayerGenPro = layer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (strNodeXml.Equals(""))
            {
                return xmlNode = null;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//Layer");
            if (pxmlnode == null)
            {

                return xmlNode = null; ;
            }
            string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
            pXmldoc.Load(_layerTreePath);
            pxmlnode = null;
            pxmlnode = pXmldoc.SelectSingleNode("//Layer[@NodeKey='" + strNodeKey + "']");
            xmlNode = pxmlnode.ParentNode;
            return xmlNode;
        }
        private XmlNode GetNode(ILayer  layer)
        {
            XmlNode xmlNode = null;
            ILayerGeneralProperties pLayerGenPro = layer as ILayerGeneralProperties;
            //读取该图层的描述信息，转成xml节点
            string strNodeXml = pLayerGenPro.LayerDescription;

            if (strNodeXml.Equals(""))
            {
                return xmlNode = null;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.LoadXml(strNodeXml);
            //获取节点的NodeKey信息
            XmlNode pxmlnode = pXmldoc.SelectSingleNode("//Layer");
            if (pxmlnode == null)
            {

                return xmlNode = null; ;
            }
            string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
            pXmldoc.Load(_layerTreePath);
            pxmlnode = null;
            pxmlnode = pXmldoc.SelectSingleNode("//Layer[@NodeKey='"+strNodeKey+"']");
            //xmlNode = pxmlnode.ParentNode;
            return pxmlnode;//changed by chulili 20110823 返回该节点本身

        }
        /// <summary>
        /// 根据配置文件读 图层比例尺信息    ZQ  20110804  add
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private string GetScale(ILayer layer)
        {
            string strScale = "";
            XmlNode pXmlNode = GetNode(layer);
            if (pXmlNode == null)
            {
                return strScale = "";
            }
            XmlElement pXmlele = pXmlNode as XmlElement;
            while (pXmlele != null)
            {
                if (pXmlele.HasAttribute("DataScale"))
                {
                    strScale = pXmlele.GetAttribute("DataScale");
                    return strScale;
                }
                else
                {
                    pXmlele = pXmlele.ParentNode as XmlElement;
                }
            }
            //strScale = pXmlNode.ParentNode.Attributes["DataScale"].Value.ToString();
            return strScale;
        }
        /// <summary>
        /// 根据展示目录树都图层  类型信息   ZQ  20110804  add
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private string GetDatatype(ILayer layer)
        {
            string strType = "";
            XmlNode pXmlNode = GetNode(layer);
            if (pXmlNode == null)
            {
                return strType = "";
            }
            XmlElement pXmlele = pXmlNode.ParentNode as XmlElement;//从父节点开始找，Layer节点也具有DataType属性
            while (pXmlele != null)
            {
                if (pXmlele.HasAttribute("DataType"))
                {
                    strType = pXmlele.GetAttribute("DataType");
                    return strType;
                }
                else
                {
                    pXmlele = pXmlele.ParentNode as XmlElement;
                }
            }
            //strType = pXmlNode.ParentNode.Attributes["DataType"].Value.ToString();
            return strType;
        }
        //end
        //输出路径设置
        private void btnOutPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "(*.gdb)|*.gdb";//ygc 20130325 山西全库抽取数据，数据量较大，只保存为GDB格式

            if (dlg.ShowDialog() != DialogResult.OK) return;
            string filepath = dlg.FileName;
            if (filepath == "") return;
            string[] savearry = new string[100];
            string[] savearry1 = new string[100];
            string str = "";
           savearry = filepath.Split(new char[] { '\\' });
           if (savearry.Length == 0) return;
           
            for (int i = 0; i < savearry.Length - 1; i++)
            {
                str = str + savearry[i]+"\\\\";
            }
            string str1 = savearry[savearry.Length - 1];
            saveworkspace = str;
            savefilename = str1;
            if (str1 != "")
            {
                savearry1 = str1.Split(new char[] { '.' });
                savefiletype = savearry1[1];
            }
            if (savefiletype == "shp")
            {
                txtPath.Text = filepath.Substring(0, filepath.LastIndexOf("."));
            }
            else
            {
                txtPath.Text = filepath;
            }
        }
        //执行输出操作
        private void btnOK_Click(object sender, EventArgs e)
        {
            AddSelectLayer();
            try
            {
                layercount = 0;
                string[] strbool = new string[dataGridLayers.Rows.Count];
                string str1 = "True";
                string str2 = "";
                string[] arrycut = new string[dataGridLayers.Rows.Count];
                try
                {
                    for (int i = 0; i < dataGridLayers.Rows.Count; i++)
                    {
                        str2 = dataGridLayers.Rows[i].Cells[0].Value.ToString();
                        if (str2 == str1)
                        {

                            strbool[layercount] = dataGridLayers.Rows[i].Cells[3].Value.ToString();
                            arrycut[layercount] = dataGridLayers.Rows[i].Cells[1].Value.ToString();
                           
                            layercount++;

                        }
                    }
                }
                catch
                {
                    MessageBox.Show("你没有选择图层");
                    return;
                }
                int selectcount = 0;
                for (int j = 0; j < layercount; j++)
                {
                    if (strbool[j] != "")
                    {
                        selectcount++;
                    }
                }
                if (pGeometry == null & pjiancaiFeaturelayer == null & boolGeometryExport )
                {
                    MessageBox.Show("请选择输出范围：");
                    return;
                }
                if (selectcount == 0)
                {
                    MessageBox.Show("没有选择图层，请选择图层");
                    return;
                }
                if (saveworkspace == "")
                {
                    MessageBox.Show("请设置存放路径");
                    return;
                }

                //输出格式判断
                if (pGeometry != null)
                {
                    IArea pArea = pGeometry as IArea;
                    if (m_area != 0)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("开始提取数据,提取范围面积为" + m_area + "平方米，存储位置为：" + txtPath.Text);
                        }
                    }
                    else
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("开始提取数据,提取范围面积为" + pArea.Area + "，存储位置为：" + txtPath.Text);
                        }
                    }
                }
                if (savefiletype == "mdb")
                {
                    IWorkspace pWorkspace = CreatePDBWorkSpace(savefilename);
                    DataExport(strbool, pWorkspace, arrycut);
                }
                if (savefiletype == "gdb")
                {
                    IWorkspace pWorkspace = CreateFileGDBWorkSpace(savefilename);
                    DataExport(strbool, pWorkspace, arrycut);
                }
                if (savefiletype == "shp")
                {
                    IWorkspace pWorkspace = CreateShapeFileWorkSpace(savefilename);
                    DataExport(strbool, pWorkspace, arrycut);
                }
                //deleted by chulili 2012-07-05 山西允许不传入范围，整图层提取
                //if (pGeometry == null)
                //{
                //    IWorkspace pWorkspace = null;
                //    DataExport(strbool, pWorkspace, arrycut);
                //}

                MessageBox.Show("数据成功导出！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("完成数据提取!");
                }
                progressStep.Value = 0;
                this.Close();
            }
            catch (Exception  ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
        //数据输出
        private void DataExport(string[] str, IWorkspace pWorkspace,string[] arrycut)
        {
            
            int count = 0;
            Exception ex=null ;
             for (int k = 0; k < this.dataGridLayers.Rows.Count; k++)
            {
                rowindex = k;
                if (dataGridLayers.Rows[k].Tag == null) continue;
                if (!((dataGridLayers.Rows[k].Tag as ILayer) is IFeatureLayer)) continue;
                string strExport = dataGridLayers.Rows[k].Cells[0].Value.ToString();
                if (strExport.ToUpper() == "FALSE") continue;

                string strcut = "";
                if (boolGeometryExport) //added by chulili 2012-07-05 如果按照范围提取（山西新增不按范围提取）
                {
                    strcut = dataGridLayers.Rows[k].Cells[1].Value.ToString();
                }
                IFeatureLayer pFeatureLayer = dataGridLayers.Rows[k].Tag as IFeatureLayer;

                 ISpatialFilter pSpatialFilter=new SpatialFilterClass();
                 if (pGeometry != null)
                 {
                     pSpatialFilter.Geometry = pGeometry;
                     pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                 }
                 count = pFeatureLayer.FeatureClass.FeatureCount(pSpatialFilter);
                this.lblTips.Text = "正在提取数据：" + pFeatureLayer.Name;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("提取" + pFeatureLayer.Name + "图层,提取" + count + "个要素");//xisheng 日志记录07.08
                }
                this.lblTips.Refresh();

                string name = "";
                if (pFeatureLayer.FeatureClass == null) continue;
                IDataset pdataset = pFeatureLayer.FeatureClass as IDataset;
                name = pdataset.Name;
                name = name.Substring(name.IndexOf('.') + 1);
                
                 //要素输出
                if (pGeometry != null)
                {
                    ///zq 2011-1219 当地图的空间参考与图层的不一致时进行转换
                    IClone pClone = (IClone)pGeometry;
                    IGeometry pTempGeometry = pClone.Clone() as IGeometry;
                    try
                    {
                        if (pTempGeometry.SpatialReference != (pFeatureLayer.FeatureClass as IGeoDataset).SpatialReference)
                        {
                            pTempGeometry.Project((pFeatureLayer.FeatureClass as IGeoDataset).SpatialReference);
                        }
                    }
                    catch { }
                    //end
                    if (savefiletype != "shp")
                    {
                        IFeatureClass targetFeatureclass=null ;
                        //ygc 20130325 判断提取图层是否为小班图层
                        int XbIndex = pFeatureLayer.FeatureClass.Fields.FindField("XBH");
                        if (XbIndex != -1)
                        {
                             IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                            //通过提取模板创建图层
                             IWorkspace pTempWorkspace = null;
                             try
                             {
                                 //pTempWorkspace = pGDBWorkSpace.OpenFromFile(m_TempGDBPath, 0);
                                 //获取模板图层
                                 IFeatureLayer pTempFeatureLayer = GetTempFeatureLayer(m_TempGDBPath, out ex);
                                 targetFeatureclass = CreateFeatureClass(name, pTempFeatureLayer, pWorkspace, null, null, pTempFeatureLayer.FeatureClass.ShapeType);
                             }
                             catch
                             { }
                        }
                        else
                        {
                            targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                        }
                        if (targetFeatureclass == null) continue;
                     
                        //注记输出
                        if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                            continue;
                        }

                        CopyFeatureToFeatureClass(pFeatureLayer, targetFeatureclass, pTempGeometry, strcut);
                    }
                    else
                    {
                        IFeatureClass targetFeatureclass = CreateFeatureClassSHP(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                        if (targetFeatureclass == null) continue;
                     
                        //注记输出
                        if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                            continue;
                        }

                        CopyFeatureToFeatureClassShp(pFeatureLayer, targetFeatureclass, pTempGeometry, strcut);
 
                    }
                }
                if (pGeometry == null && pjiancaiFeaturelayer!=null)
                {
                    IFeatureCursor pFeatureCursor1 = pjiancaiFeaturelayer.Search(null, false);
                    IFeature pfeature = pFeatureCursor1.NextFeature();
                    while (pfeature != null)
                    {
                        if (savefiletype == "mdb")
                        {
                            string workspacename = pFeatureLayer.Name + pfeature.get_Value(0).ToString()+".mdb";
                            pWorkspace = CreatePDBWorkSpace(workspacename);
                            IFeatureClass targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                            if (targetFeatureclass == null) return;
                            
                            if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                                pfeature = pFeatureCursor1.NextFeature();
                                continue;
                            }
                            CopyFeatureToFeatureClass(pFeatureLayer, targetFeatureclass, pfeature.Shape, strcut);
                        }
                        if (savefiletype == "gdb")
                        {
                            string workspacename = pFeatureLayer.Name + pfeature.get_Value(0).ToString() + ".gdb";
                            pWorkspace = CreateFileGDBWorkSpace(workspacename);
                             IFeatureClass targetFeatureclass=null ;
                            //ygc 20130325 判断提取图层是否为小班图层
                            int XbIndex = pFeatureLayer.FeatureClass.Fields.FindField("XBH");
                            if (XbIndex != -1)
                            {
                                IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                                //通过提取模板创建图层
                                IWorkspace pTempWorkspace = null;
                                try
                                {
                                    //pTempWorkspace = pGDBWorkSpace.OpenFromFile(m_TempGDBPath, 0);
                                    //获取模板图层
                                    IFeatureLayer pTempFeatureLayer = GetTempFeatureLayer(m_TempGDBPath, out ex);
                                    targetFeatureclass = CreateFeatureClass(name, pTempFeatureLayer, pWorkspace, null, null, pTempFeatureLayer.FeatureClass.ShapeType);
                                }
                                catch
                                { }
                            }
                            else
                            {
                                targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                            }
                             //CreateFeatureClassSHP(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                            if (targetFeatureclass == null) return;
                            
                            if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                                pfeature = pFeatureCursor1.NextFeature();
                                continue;
                            }
                            CopyFeatureToFeatureClass(pFeatureLayer, targetFeatureclass, pfeature.Shape, strcut);
                        }
                        if (savefiletype == "shp")
                        {
                            string workspacename = pFeatureLayer.Name + pfeature.get_Value(0).ToString() + ".shp";
                            pWorkspace = CreateShapeFileWorkSpace(workspacename);
                            IFeatureClass targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                            if (targetFeatureclass == null) return;
                            
                            if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                            {
                                CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                                pfeature = pFeatureCursor1.NextFeature();
                                continue;
                            }
                            CopyFeatureToFeatureClassShp(pFeatureLayer, targetFeatureclass, pfeature.Shape, strcut);
                        }

                        pfeature = pFeatureCursor1.NextFeature();
                    }
                 // CopyFeatureToFeatureClass1(pFeatureLayer, pWorkspace, strcut);
                }
                if (pGeometry == null && pjiancaiFeaturelayer == null & boolGeometryExport==false )
                {
                    if (savefiletype != "shp")
                    {
                        IFeatureClass targetFeatureclass = null;
                        //ygc 20130325 判断提取图层是否为小班图层
                        int XbIndex = pFeatureLayer.FeatureClass.Fields.FindField("XBH");
                        if (XbIndex != -1)
                        {
                            IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                            //通过提取模板创建图层
                            IWorkspace pTempWorkspace = null;
                            try
                            {
                                //pTempWorkspace = pGDBWorkSpace.OpenFromFile(m_TempGDBPath, 0);
                                //获取模板图层
                                IFeatureLayer pTempFeatureLayer = GetTempFeatureLayer(m_TempGDBPath, out ex);
                                targetFeatureclass = CreateFeatureClass(name, pTempFeatureLayer, pWorkspace, null, null, pTempFeatureLayer.FeatureClass.ShapeType);
                            }
                            catch
                            { }
                        }
                        else
                        {
                            targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                        }
                      //  IFeatureClass targetFeatureclass = CreateFeatureClass(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                        if (targetFeatureclass == null) continue;

                        //注记输出
                        if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                            continue;
                        }

                        CopyFeatureToFeatureClass(pFeatureLayer, targetFeatureclass, null, strcut);
                    }
                    else
                    {
                        IFeatureClass targetFeatureclass = CreateFeatureClassSHP(name, pFeatureLayer, pWorkspace, null, null, pFeatureLayer.FeatureClass.ShapeType);
                        if (targetFeatureclass == null) continue;

                        //注记输出
                        if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            CopyAnnoPropertyToFC(pFeatureLayer.FeatureClass, targetFeatureclass);
                            continue;
                        }

                        CopyFeatureToFeatureClassShp(pFeatureLayer, targetFeatureclass, null, strcut);

                    }                  // CopyFeatureToFeatureClass1(pFeatureLayer, pWorkspace, strcut);
                }
            }
      }
        //创建featureclass
        private IFeatureClass CreateFeatureClass(string name, IFeatureLayer pfeaturelayer, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType)
        {
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            for (int i = 0; i < pfeaturelayer.FeatureClass.Fields.FieldCount; i++)
            {
                IClone pClone = pfeaturelayer.FeatureClass.Fields.get_Field(i) as IClone;
                IField pTempField = pClone.Clone() as IField;
                IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                string strFieldName = pTempField.Name;
                string[] strFieldNames = strFieldName.Split('.');

                if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;

                pTempFieldEdit.Name_2=strFieldNames[strFieldNames.GetLength(0)-1];
                pFieldsEdit.AddField(pTempField);
            }

            string strShapeFieldName = pfeaturelayer.FeatureClass.ShapeFieldName;
            string[] strShapeNames=strShapeFieldName.Split('.');
            strShapeFieldName = strShapeNames[strShapeNames.GetLength(0)-1];


            IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", pFields, uidCLSID, uidCLSEXT, pfeaturelayer.FeatureClass.FeatureType, strShapeFieldName, "");

            return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.3的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }
        
        //创建featureclass针对shp，因其字段长度最长限制为10...yjl630
        private IFeatureClass CreateFeatureClassSHP(string name, IFeatureLayer pfeaturelayer, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType)
        {
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                //toShpField = new Dictionary<string, string>();
                for (int i = 0; i < pfeaturelayer.FeatureClass.Fields.FieldCount; i++)
                {
                    IClone pClone = pfeaturelayer.FeatureClass.Fields.get_Field(i) as IClone;
                    IField pTempField = pClone.Clone() as IField;
                    IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                    string strFieldName = pTempField.Name;
                    string[] strFieldNames = strFieldName.Split('.');

                    if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;
                    string fdName=strFieldNames[strFieldNames.GetLength(0) - 1];
                    pTempFieldEdit.Name_2 = fdName;
                    try
                    {
                        pFieldsEdit.AddField(pTempField);
                    }
                    catch(Exception err0)
                    { }
                }

                string strShapeFieldName = pfeaturelayer.FeatureClass.ShapeFieldName;
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];

                IFieldChecker fdCheker = new FieldCheckerClass();
                IEnumFieldError enumFdError=null;
                IFields validFds = null;
                fdCheker.ValidateWorkspace = pWorkspace;
                fdCheker.Validate(pFields, out enumFdError, out validFds);
                //List<string > tmp=new List<string>();
                //for (int s = 0; s < validFds.FieldCount; s++)
                //{
                //    tmp.Add(validFds.get_Field(s).Name);
                //}
                //if(EnumFieldError !=null)
                //     IFieldError fe = enumFdError.Next();
                //while (fe != null)
                //{
                //    fe = enumFdError.Next();
                //}
                if (File.Exists(pWorkspace.PathName+"\\"+name+".shp"))//如果不判断，则创建重名的要素会失败且将其删除，原因未知
                    return null;

                IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", validFds, uidCLSID, uidCLSEXT, pfeaturelayer.FeatureClass.FeatureType, strShapeFieldName, "");

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.3的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }
        

        private IWorkspace CreatePDBWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            if (System.IO.File.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }
            
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
            IWorkspace PDB_workspace = (IWorkspace)name.Open();
            return PDB_workspace;

        }
        public IWorkspace CreateFileGDBWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            if (System.IO.File.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace fileGDB_workspace = (IWorkspace)name.Open();
            return fileGDB_workspace;
        }

        public IWorkspace CreateShapeFileWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory() as IWorkspaceFactory;
            filename = filename.Substring(0, filename.LastIndexOf("."));
            if (System.IO.Directory.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace shapefile_workspace = (IWorkspace)name.Open();
            return shapefile_workspace;
        }
        //要素复制
        /* private void CopyFeatureToFeatureClass1(IFeatureLayer pfeaturelayer,IWorkspace pWorkspace ,string strcut)
         {
             int featurecount = 0;
             ISpatialFilter pSpatialFilter = new SpatialFilterClass();
             IFeatureCursor pFeatureCursor1 = pjiancaiFeaturelayer.Search(null, false);
             IFeature pfeature = pFeatureCursor1.NextFeature();
             while (pfeature != null)
             {
                 pSpatialFilter.Geometry = pfeature.Shape;
                 pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
                 pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                 if (dataGridLayers.Rows[rowindex].Cells[4].Value == null)
                 { }
                 else
                 {
                     pSpatialFilter.WhereClause = dataGridLayers.Rows[rowindex].Cells[4].Value.ToString();
                 }
                 IQueryFilter pQueryFilter = (ISpatialFilter)pSpatialFilter;
                 IFeatureCursor pFeatureCursor = pfeaturelayer.Search(pQueryFilter, false);
                 featurecount = pfeaturelayer.FeatureClass.FeatureCount(pQueryFilter);
                 //分别创建featureclass
                 string fcname = pfeaturelayer.Name + pfeature.get_Value(0).ToString();
                 IFeatureClass pfeatureclass = CreateFeatureClass(fcname, pfeaturelayer, pWorkspace, null, null, pfeaturelayer.FeatureClass.ShapeType);
                 //剪裁输出
                 if (strcut == "True")
                 {
                     cutExport(pFeatureCursor, pfeatureclass, pfeature.Shape, featurecount);
                 }
                 //不剪裁输出
                 else
                 {
                     notcutExport(pFeatureCursor, pfeatureclass, featurecount);
                 }
                 pfeature = pFeatureCursor1.NextFeature();
             }
           /*  pSpatialFilter.Geometry = pfeature.Shape;
             pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
             pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

             if (dataGridLayers.Rows[rowindex].Cells[4].Value == null)
             { }
             else
             {
                 pSpatialFilter.WhereClause = dataGridLayers.Rows[rowindex].Cells[4].Value.ToString();
             }
             IQueryFilter pQueryFilter = (ISpatialFilter)pSpatialFilter;
             IFeatureCursor pFeatureCursor = pfeaturelayer.Search(pQueryFilter, false);
             featurecount = pfeaturelayer.FeatureClass.FeatureCount(pQueryFilter);
             //剪裁输出
             if (strcut == "True")
             {
                 cutExport(pFeatureCursor, pfeatureclass, pfeature.Shape, featurecount);
             }
             //不剪裁输出
             else
             {
                 notcutExport(pFeatureCursor, pfeatureclass, featurecount);
             }
         }*/
        private void CopyFeatureToFeatureClass(IFeatureLayer pfeaturelayer, IFeatureClass pfeatureclass,IGeometry pgeometry,string strcut)
        {
            
                int featurecount = 0;
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                if (pgeometry != null)
                {
                    pSpatialFilter.Geometry = pgeometry;

                    //pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
                    switch (pgeometry.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                            break;

                        case esriGeometryType.esriGeometryPolyline:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            break;

                        case esriGeometryType.esriGeometryPolygon:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            break;
                        default:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            break;
                    }
                }

                if (dataGridLayers.Rows[rowindex].Cells["colWhere"].Value == null)
            { }
            else
            {
                pSpatialFilter.WhereClause = dataGridLayers.Rows[rowindex].Cells["colWhere"].Value.ToString();
            }

            featurecount = pfeaturelayer.FeatureClass.FeatureCount(pSpatialFilter);
            IFeatureCursor pFeatureCursor = pfeaturelayer.FeatureClass.Search(pSpatialFilter, false);
            
            //剪裁输出
             if (strcut == "True")
             {
                 cutExport(pFeatureCursor, pfeatureclass, pgeometry,featurecount);
             }
                 //不剪裁输出
             else
             {
                 notcutExport(pFeatureCursor, pfeatureclass,featurecount);
             }

             System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
             pFeatureCursor = null;
           
        }
        //for shp
        private void CopyFeatureToFeatureClassShp(IFeatureLayer pfeaturelayer, IFeatureClass pfeatureclass, IGeometry pgeometry, string strcut)
        {

            int featurecount = 0;
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            if (pgeometry != null)
            {
                pSpatialFilter.Geometry = pgeometry;

                //pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
                switch (pgeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;

                    case esriGeometryType.esriGeometryPolyline:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;

                    case esriGeometryType.esriGeometryPolygon:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                }
            }

            if (dataGridLayers.Rows[rowindex].Cells["colWhere"].Value == null)
            { }
            else
            {
                pSpatialFilter.WhereClause = dataGridLayers.Rows[rowindex].Cells["colWhere"].Value.ToString();
            }

            featurecount = pfeaturelayer.FeatureClass.FeatureCount(pSpatialFilter);
            IFeatureCursor pFeatureCursor = pfeaturelayer.FeatureClass.Search(pSpatialFilter, false);

            //剪裁输出
            if (strcut == "True")
            {
                cutExportShp(pFeatureCursor, pfeatureclass, pgeometry, featurecount);
            }
            //不剪裁输出
            else
            {
                notcutExportShp(pFeatureCursor, pfeatureclass, featurecount);
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;

        }
        
        //不剪裁输出
        private void notcutExport(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount; 
            progressStep.Step = 1;
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();
                //ygc 20130411 判断是否提取的图层为小班图层
                bool IsXBFeatureLayer = false;
                int XBIndex = pToFeatureClass.Fields.FindField("XIAO_BAN");
                if (XBIndex != -1) IsXBFeatureLayer = true;
                //ygc 20130411 获取国标转换配置信息
                XmlDocument ChangeDoc = null;
                if (IsXBFeatureLayer)
                {
                    ChangeDoc = new XmlDocument();
                    if (File.Exists(m_ChangeConfigPath))
                    {
                        ChangeDoc.Load(m_ChangeConfigPath);
                    }
                }
                string sFieldName = "";//源字段名称
                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    sFieldName = pFeature.Fields.get_Field(i).Name;
                    int TargetIndex = -1;
                    if (IsXBFeatureLayer)
                    {
                        //ygc 20130411 提取小班图层时，对小班图层的字段进行国标转换
                        if (ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']") != null && ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']").Attributes["IsChanged"].Value == "False")
                        {
                            TargetIndex = pFeatureBuffer.Fields.FindField(ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']").Attributes["TargetField"].Value.ToString());
                            if ((TargetIndex > -1) && (pFeatureBuffer.Fields.get_Field(TargetIndex).Editable == true))
                            {
                                pFeatureBuffer.set_Value(TargetIndex, pFeature.get_Value(i));
                            }
                        }
                        else if (ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']") != null && ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']").Attributes["IsChanged"].Value == "True")
                        {
                            TargetIndex = pFeatureBuffer.Fields.FindField(ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']").Attributes["TargetField"].Value.ToString());
                            if ((TargetIndex > -1) && (pFeatureBuffer.Fields.get_Field(TargetIndex).Editable == true))
                            {
                               // pFeatureBuffer.set_Value(TargetIndex, pFeature.get_Value(i));
                                //ygc 20130411 对省级字段值进行处理
                                string newValue = GetChangeValue(sFieldName, ChangeDoc.SelectSingleNode("//RootItems//Item[@SourceField='" + sFieldName + "']").Attributes["TargetField"].Value.ToString(), pFeature.get_Value(i).ToString(), pFeature);
                                pFeatureBuffer.set_Value(TargetIndex, (object)newValue);
                            }
                        }
                    }
                    else
                    {
                        int iIndex = pFeatureBuffer.Fields.FindField(sFieldName);

                        if ((iIndex > -1) && (pFeatureBuffer.Fields.get_Field(iIndex).Editable == true))
                        {
                            pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                        }
                    }
                }
                pFeatureBuffer.Shape = GetNewGeometry(pFeature.ShapeCopy);
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                progressStep.PerformStep();
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        //剪裁输出
        private void cutExport(IFeatureCursor pfeaturecursor, IFeatureClass pfeatureclass, IGeometry pgeometry, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pfeaturecursor.NextFeature();
            if (pFeature == null) return;

            IFeatureCursor pFeatureCursor = pfeatureclass.Insert(true);

            ISegmentCollection pSegmentCol = new PolygonClass();
            if (pgeometry.GeometryType == esriGeometryType.esriGeometryEnvelope)
            {
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope = pgeometry as IEnvelope;
                pSegmentCol.SetRectangle(pEnvelope);
                pgeometry = pSegmentCol as IGeometry;
            }
            else if (pgeometry.GeometryType == esriGeometryType.esriGeometryCircularArc)
            {
                ICircularArc pCircularArc = new CircularArcClass();
                pCircularArc = pgeometry as ICircularArc;
                object obj = System.Reflection.Missing.Value;
                pSegmentCol.AddSegment((ISegment)pCircularArc, ref obj, ref obj);
                pgeometry = pSegmentCol as IGeometry;
            }

            ITopologicalOperator pTopoOp = (ITopologicalOperator)pgeometry;
            IGeometry pBndGeom = pTopoOp.Boundary;

            esriGeometryDimension iDimension;

            IGeometry pAimGeometry = pFeature.ShapeCopy;
            if (pAimGeometry.Dimension < pgeometry.Dimension) iDimension = pAimGeometry.Dimension;
            else iDimension = pgeometry.Dimension;

            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pfeatureclass.CreateFeatureBuffer();
                IFeature pAimFeature = (IFeature)pFeatureBuffer;

                pAimGeometry = pFeature.ShapeCopy;
                IRelationalOperator pRelOpeator = (IRelationalOperator)pAimGeometry;

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = pAimFeature.Fields.FindField(sFieldName);
                    if (iIndex == -1) continue;
                    IField pFld = pAimFeature.Fields.get_Field(iIndex);
                    if ((iIndex > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                         pAimFeature.set_Value(iIndex, pFeature.get_Value(i));
                    }
                }

                //此处有错误，暂时加保护 xisheng 20111128
                try
                {
                    if (pAimGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        pAimFeature.Shape = GetNewGeometry(pFeature.ShapeCopy);
                    }
                    //判断是否相交或者包含关系,如果是则进行空间切割
                    else
                    {
                        bool bCross = false;
                        bool bContain = false;
                        try
                        {
                            bCross = pRelOpeator.Crosses(pBndGeom);    //changed by chulili 20111213 这句话可能报错，不要直接放在IF条件里                        
                        }
                        catch
                        { }
                        try
                        {
                            bContain = pRelOpeator.Contains(pBndGeom);  //changed by chulili 20111213 这句话可能报错
                        }
                        catch
                        { }
                        if (bCross || bContain)
                        {
                            pTopoOp = (ITopologicalOperator)pFeature.ShapeCopy;
                            pTopoOp.Simplify();
                            pAimFeature.Shape = GetNewGeometry(pTopoOp.Intersect(pgeometry, iDimension));
                        }
                        else
                        {
                            pAimFeature.Shape = GetNewGeometry(pFeature.ShapeCopy);
                        }
                    }
                }
                catch { }

                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount = iCount + 1;
                progressStep.PerformStep();
                pFeature = pfeaturecursor.NextFeature();
            }

            if (iCount > 0) pFeatureCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }

        //不剪裁输出forShp
        private void notcutExportShp(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    //string sFieldName = pFeature.Fields.get_Field(i).Name;

                    //int iIndex = pFeatureBuffer.Fields.FindField(toShpField[sFieldName]);
                    try
                    {
                        if (pFeatureBuffer.Fields.get_Field(i).Editable)
                        {
                            pFeatureBuffer.set_Value(i, pFeature.get_Value(i));
                        }
                    }
                    catch
                    { }
                }
                pFeatureBuffer.Shape = GetNewGeometry(pFeature.ShapeCopy);
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                progressStep.PerformStep();
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        //剪裁输出forShp
        private void cutExportShp(IFeatureCursor pfeaturecursor, IFeatureClass pfeatureclass, IGeometry pgeometry, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pfeaturecursor.NextFeature();
            if (pFeature == null) return;

            IFeatureCursor pFeatureCursor = pfeatureclass.Insert(true);

            ISegmentCollection pSegmentCol = new PolygonClass();
            if (pgeometry.GeometryType == esriGeometryType.esriGeometryEnvelope)
            {
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope = pgeometry as IEnvelope;
                pSegmentCol.SetRectangle(pEnvelope);
                pgeometry = pSegmentCol as IGeometry;
            }
            else if (pgeometry.GeometryType == esriGeometryType.esriGeometryCircularArc)
            {
                ICircularArc pCircularArc = new CircularArcClass();
                pCircularArc = pgeometry as ICircularArc;
                object obj = System.Reflection.Missing.Value;
                pSegmentCol.AddSegment((ISegment)pCircularArc, ref obj, ref obj);
                pgeometry = pSegmentCol as IGeometry;
            }

            ITopologicalOperator pTopoOp = (ITopologicalOperator)pgeometry;
            IGeometry pBndGeom = pTopoOp.Boundary;

            esriGeometryDimension iDimension;

            IGeometry pAimGeometry = pFeature.ShapeCopy;
            if (pAimGeometry.Dimension < pgeometry.Dimension) iDimension = pAimGeometry.Dimension;
            else iDimension = pgeometry.Dimension;

            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pfeatureclass.CreateFeatureBuffer();
                IFeature pAimFeature = (IFeature)pFeatureBuffer;

                pAimGeometry = pFeature.ShapeCopy;
                IRelationalOperator pRelOpeator = (IRelationalOperator)pAimGeometry;

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    //string sFieldName = pFeature.Fields.get_Field(i).Name;

                    //int iIndex = pAimFeature.Fields.FindField(toShpField[sFieldName]);
                    //if (iIndex == -1) continue;
                    IField pFld = pAimFeature.Fields.get_Field(i);
                    if ((pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        try
                        {
                            pAimFeature.set_Value(i, pFeature.get_Value(i));
                        }
                        catch
                        { }
                    }
                }

                if (pAimGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    pAimFeature.Shape = GetNewGeometry(pFeature.ShapeCopy);
                }
                //判断是否相交或者包含关系,如果是则进行空间切割
                else
                {
                    bool bCross = false;
                    bool bContain = false;
                    try
                    {
                        bCross = pRelOpeator.Crosses(pBndGeom);//changed by chulili 20111213 这句话可能报错，不要直接放在IF条件里                       
                    }
                    catch
                    { }
                    try
                    {
                        bContain = pRelOpeator.Contains(pBndGeom);//changed by chulili 20111213 这句话可能报错  
                    }
                    catch
                    { }
                    if (bCross || bContain)
                    {
                        pTopoOp = (ITopologicalOperator)pFeature.ShapeCopy;
                        pTopoOp.Simplify();
                        pAimFeature.Shape = GetNewGeometry(pTopoOp.Intersect(pgeometry, iDimension));
                    }
                    else
                    {
                        pAimFeature.Shape = GetNewGeometry(pFeature.ShapeCopy);
                    }
                }
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount = iCount + 1;
                progressStep.PerformStep();
                pFeature = pfeaturecursor.NextFeature();
            }

            if (iCount > 0) pFeatureCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        
        
        //注记复制
        private void CopyAnnoPropertyToFC(IFeatureClass pSourceFeatureClass, IFeatureClass pToFeatureClass)
        {
            IAnnoClass pSourceAnnoClass = (IAnnoClass)pSourceFeatureClass.Extension;
            IAnnoClass pTargerAnnoClass = (IAnnoClass)pToFeatureClass.Extension;

            IAnnotateLayerPropertiesCollection pSourceAnnoProperCollection = pSourceAnnoClass.AnnoProperties;
            IClone pAnnoCollection = (IClone)pSourceAnnoProperCollection;

            ISymbolCollection pSourceSymbolCollection = pSourceAnnoClass.SymbolCollection;
            IClone pAnnoSymbol = (IClone)pSourceSymbolCollection;

            IAnnoClassAdmin2 pAnnoClassAdmin = (IAnnoClassAdmin2)pTargerAnnoClass;

            pAnnoClassAdmin.ReferenceScale = pSourceAnnoClass.ReferenceScale;
            pAnnoClassAdmin.ReferenceScaleUnits = pSourceAnnoClass.ReferenceScaleUnits;

            pAnnoClassAdmin.AnnoProperties = (IAnnotateLayerPropertiesCollection)pAnnoCollection;
            pAnnoClassAdmin.SymbolCollection = (ISymbolCollection)pAnnoSymbol;

            pAnnoClassAdmin.UpdateProperties();
            pAnnoClassAdmin.UpdateOnShapeChange = true;

        }
        //全选
        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridLayers.RowCount; i++)
            {
                dataGridLayers.Rows[i].Cells[0].Value = true;
                dataGridLayers.Rows[i].Cells[1].Value = true;
            }
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消提取数据!");
            }
        }
        //反选
        private void btnNotSel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridLayers.RowCount; i++)
            {
                convert(dataGridLayers.Rows[i].Cells[0]);
                //changed by chulili 20111208 反选以及与裁剪同步
                dataGridLayers.Rows[i].Cells[1].Value = dataGridLayers.Rows[i].Cells[0].Value;
                //added by chulili 20110801 裁剪与图层选择同步
                //if (dataGridLayers.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                //{
                //    dataGridLayers.Rows[i].Cells[1].Value = true;
                //}
                //else
                //{
                //    dataGridLayers.Rows[i].Cells[1].Value = false;
                //}
                //end added by chulili
            }
        }
        //改变checkbox的值
        private void convert(DataGridViewCell cell)
        {
            string strT = "True";

            if (cell.Value.ToString() == strT)
            {
                cell.Value = false;
            }
            else 
            {
                cell.Value = true;
            }
        }
        // dataGridLayers_CellMouseClick事件
        private void dataGridLayers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            rowindex = e.RowIndex;
            //added by chulili 20110731 提取时选中一个层,默认选中裁剪
            if (e.ColumnIndex==0)
            {
                //if (dataGridLayers.Rows[rowindex].Cells[0].Value.ToString().ToLower() == "false" )
                //{
                //    dataGridLayers.Rows[rowindex].Cells[1].Value = true;
                //}
                //else
                //{
                //    dataGridLayers.Rows[rowindex].Cells[1].Value = false ;
                //}
            } 
            //end added by chulili
            DataGridViewRow pdatagridviewrow = dataGridLayers.Rows[e.RowIndex];
            if (e.RowIndex < 0) return;
            
            //checkbox选择
            //int intColumnIndex = e.ColumnIndex;
            //if (e.ColumnIndex == 0)
            //{
            //    bool blnSelect = (bool)dataGridLayers.Rows[e.RowIndex].Cells[intColumnIndex].Value;

            //    for (int i = 0; i < dataGridLayers.SelectedRows.Count; i++)
            //    {
            //        dataGridLayers.SelectedRows[0].Cells[intColumnIndex].Value = !blnSelect;
            //    }
            //}  
            //dataGridLayers.s

            //int seleectrowcount = dataGridLayers.SelectedRows.Count;
            //bool rowbool=dataGridLayers.SelectedRows.Contains(pdatagridviewrow);
            //if (seleectrowcount > 0&rowbool==true)
            //{
            //    for (int i = 0; i < seleectrowcount; i++)
            //    {
            //        dataGridLayers.SelectedRows[i].Cells[e.ColumnIndex].Value = true;
            //    }
            //}
            //string strcheck = dataGridLayers.Rows[e.RowIndex].Cells[0].Value.ToString();
            //if (strcheck == "False") return;
            //弹出SQL界面
            /// ZQ  20111116  modify
            if (!((pdatagridviewrow.Tag as ILayer) is IFeatureLayer)) return;
            if (dataGridLayers.Columns[e.ColumnIndex].HeaderCell.Value.ToString() =="过滤条件")
            {
                cellclickindex = e.RowIndex;
                SQLfrm frm = new SQLfrm(pdatagridviewrow);
                frm.Show();

            }
            ctrl = false;
        }
       /* private void dataGridLayers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                ctrl = true;
            }
        }

        private void dataGridLayers_KeyUp(object sender, KeyEventArgs e)
        {
            ctrl = false;
        }
        */
        private void btnjiancai_Click(object sender, EventArgs e)
        {
            addDataDialog = new frmAddData(pMap);
            if (addDataDialog.ShowDialog() == DialogResult.OK)
            {
                addDataDialog.FormAddData();
                pjiancaiFeaturelayer = addDataDialog.m_pFeaturelayer;
            }
            if (pjiancaiFeaturelayer.FeatureClass.ShapeType.ToString() != "esriGeometryPolygon")
            {
                MessageBox.Show("选择的范围必须是面状数据，请重新选择：");
                return;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridLayers.RowCount; i++)
            {
                dataGridLayers.Rows[i].Cells[1].Value = true;
                dataGridLayers.Rows[i].Cells[0].Value = true;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridLayers.RowCount; i++)
            {
                convert(dataGridLayers.Rows[i].Cells[1]);
            }
        }


        private void dataGridLayers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (this.dataGridLayers.SelectedRows.Count < 2)
            {
                this.colChecked.ReadOnly = false;
                this.colchecked1.ReadOnly = false;
                return;
            }
            else
            {
                this.colChecked.ReadOnly = true;
                this.colchecked1.ReadOnly = true;
            }

            //checkbox选择
            //int intColumnIndex = e.ColumnIndex;
            //if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            //{
            //    bool blnSelect = (bool)dataGridLayers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            //    for (int i = 0; i < dataGridLayers.SelectedRows.Count; i++)
            //    {
            //        dataGridLayers.SelectedRows[i].Cells[e.ColumnIndex].Value = !blnSelect;
            //    }
            //}  
        }

        private void cmdCoorSet_Click(object sender, EventArgs e)
        {
            frmSetting frmset = new frmSetting();
            frmset.g_dblXoff = m_dblXoffset;
            frmset.g_dblYoff = m_dblYoffset;
            frmset.g_dblCentX = m_dblCentX;
            frmset.g_dblCentY = m_dblCentY;
            frmset.g_dblRotate = m_dblRote;
            frmset.ShowDialog();

            m_dblXoffset = frmset.g_dblXoff;
            m_dblYoffset = frmset.g_dblYoff;
            m_dblCentX = frmset.g_dblCentX;
            m_dblCentY = frmset.g_dblCentY;
            m_dblRote = frmset.g_dblRotate;
        }

        private void chkTrans_CheckedChanged(object sender, EventArgs e)
        {
            this.m_blnEditCoor = this.chkTrans.Checked;
        }
        #region  ZQ  20110803    add    选择比例尺信息与数据类型信息
   
        /// <summary>
        /// 通过比例尺、数据类型信息来控制图层显隐
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="CellIndex"></param>
        /// <param name="pCheckBox"></param>
        private void SetRowValue(string strScale,string strType, int CellIndex, DevComponents.DotNetBar.Controls.CheckBoxX pCheckBox)
        {
            try
            {
                for (int i = 0; i < dataGridLayers.RowCount; i++)
                {
                    bool BVlaue = (dataGridLayers.Rows[i].Cells[CellIndex].Value.ToString() == strScale && dataGridLayers.Rows[i].Cells[CellIndex + 1].Value.ToString() == strType);
                    if (BVlaue && pCheckBox.Checked)
                    {
                        dataGridLayers.Rows[i].Cells[1].Value= true;
                        dataGridLayers.Rows[i].Cells[0].Value = true;
                        dataGridLayers.Rows[i].Visible = true;

                    }
                    else if (BVlaue && !pCheckBox.Checked)
                    {
                        dataGridLayers.Rows[i].Cells[1].Value = false;
                        dataGridLayers.Rows[i].Cells[0].Value = true;
                        dataGridLayers.Rows[i].Visible = false;

                    }
                }
            }
            catch
            {
            }
        }
        private void chckBoxEDLG_CheckedChanged(object sender, EventArgs e)
        {
            SetRowValue("250000","DLG",4,chckBoxEDLG);
        }

        private void chckBoxEDEM_CheckedChanged(object sender, EventArgs e)
        {
            SetRowValue("250000", "DEM", 4, chckBoxEDEM);
        }

        private void chckBoxEDOM_CheckedChanged(object sender, EventArgs e)
        {
            SetRowValue("250000", "DOM", 4, chckBoxEDOM);
        }
        private void chckBoxDLG_CheckedChanged(object sender, EventArgs e)
        {

            SetRowValue("50000", "DLG", 4, chckBoxDLG);
        }
        private void chckBoxDEM_CheckedChanged(object sender, EventArgs e)
        {
            SetRowValue("50000", "DEM", 4, chckBoxDEM);
        }

        private void chckBoxDOM_CheckedChanged(object sender, EventArgs e)
        {
            SetRowValue("50000", "DOM", 4, chckBoxDOM);
        }
        #endregion


        private  Dictionary<string, ILayer> mydic = new Dictionary<string, ILayer>(); //xisheng 用字典存储列表中的图层和NodeKey，方便获取 20111203
        //弹出图层目录选择提取图层 20111128 xisheng
        private void AddSelectLayer()
        {
                //dataGridLayers.Rows.Clear();
                //IMap map= frm.m_returnMap;
            GetSelectLayer();
                IFeatureLayer pFeatureLayer;
                IDataset pDataset = null;
                int intIndex = 0;

                //图层初始化
                foreach (KeyValuePair<string, ILayer> keyvalue in m_DicLayer)//更改循环方式 20111203 xisheng
                {
                    ILayer pLyr = keyvalue.Value;
                    if (pLyr is IGroupLayer)
                    {
                        ICompositeLayer pComLyr = pLyr as ICompositeLayer;
                        for (int j = 0; j < pComLyr.Count; j++)
                        {
                            ILayer pTempLyr = pComLyr.get_Layer(j);
                            if (pTempLyr is IFeatureLayer && !(pTempLyr is IGdbRasterCatalogLayer)) //特例这个也是IFeatureLayer xisheng 20111202
                            {
                                pFeatureLayer = pTempLyr as IFeatureLayer;
                                //ZQ   20110803    add 过滤接图表图层
                                if (!GetIsQuery(pFeatureLayer)) { continue; }
                                //end
                                pDataset = pFeatureLayer.FeatureClass as IDataset;
                                //yjl20111012 add 行政区数据提取
                                string strNewCode = XZQCode.Substring(0, 6);
                                if (XZQCode != null && !pDataset.Name.Contains(strNewCode))
                                {
                                    continue;

                                }
                                //增加判断是不是已经存在于列表中了，20111203 xisheng 
                                if (!mydic.ContainsKey(keyvalue.Key))
                                {
                                    mydic.Add(keyvalue.Key, keyvalue.Value);
                                }
                                else
                                {
                                    continue;
                                }
                                //ZQ  20110804    modify

                                dataGridLayers.Rows.Add(true, true, pDataset.Name, pFeatureLayer.Name, GetScale(pFeatureLayer).ToString(), GetDatatype(pFeatureLayer).ToString());

                                dataGridLayers.Rows[intIndex].Tag = pFeatureLayer;
                                //dataGridLayers.Rows[intIndex].Visible = false;

                                intIndex++;
                            }
                            else//栅格数据提取预留,暂未支持 xisheng 20111128
                            {
                                //if (!GetIsQuery(pTempLyr)) { continue; } 栅格不必判断是否可查询

                                //增加判断是不是已经存在于列表中了，20111203 xisheng 
                                if (!mydic.ContainsKey(keyvalue.Key))
                                {
                                    mydic.Add(keyvalue.Key, keyvalue.Value);
                                }
                                else
                                {
                                    continue;
                                }
                                pDataset = pTempLyr as IDataset;

                                dataGridLayers.Rows.Add(true, true, pDataset.Name, pTempLyr.Name, GetScale(pTempLyr).ToString(), GetDatatype(pTempLyr).ToString());

                                dataGridLayers.Rows[intIndex].Tag = pTempLyr;
                                //dataGridLayers.Rows[intIndex].Visible = false;

                                intIndex++;
                            }
                        }
                    }
                    else if (pLyr is IFeatureLayer && !(pLyr is IGdbRasterCatalogLayer))//特例这个也是IFeatureLayer xisheng 20111202
                    {
                        pFeatureLayer = (IFeatureLayer)pLyr;
                        //ZQ   20110803    add 过滤接图表图层
                        if (!GetIsQuery(pFeatureLayer))
                        { continue; }
                        //end
                        pDataset = pFeatureLayer.FeatureClass as IDataset;
                        if (pDataset == null)
                        {
                            return;
                        }
                        //yjl20111012 add 行政区数据提取
                        string strNewCode = "";
                        if (XZQCode != null) XZQCode.Substring(0, 6);
                        if (XZQCode != null && !pDataset.Name.Contains(strNewCode))
                        {
                            continue;

                        }

                        //增加判断是不是已经存在于列表中了，20111203 xisheng 
                        if (!mydic.ContainsKey(keyvalue.Key))
                        {
                            mydic.Add(keyvalue.Key, keyvalue.Value);
                        }
                        else
                        {
                            continue;
                        }
                        //ZQ  20110804    modify   增加比例尺与数据类型信息

                        dataGridLayers.Rows.Add(true, true, pDataset.Name, pFeatureLayer.Name, GetScale(pFeatureLayer).ToString(), GetDatatype(pFeatureLayer).ToString());

                        dataGridLayers.Rows[intIndex].Tag = pFeatureLayer;
                        //dataGridLayers.Rows[intIndex].Visible = true;
                        intIndex++;
                    }
                    else //栅格数据提取预留,暂未支持 xisheng 20111128
                    {

                        //if (!GetIsQuery(pLyr)) { continue; }//不必判断是否可查询
                        //增加判断是不是已经存在于列表中了，20111203 xisheng 
                        if (!mydic.ContainsKey(keyvalue.Key))
                        {
                            mydic.Add(keyvalue.Key, keyvalue.Value);
                        }
                        else
                        {
                            continue;
                        }
                        pDataset = pLyr as IDataset;
                        ///ZQ 2011 1202  判断pDataset是否为空
                        if (pDataset == null) { continue; }

                        dataGridLayers.Rows.Add(true, true, pDataset.Name, pLyr.Name, GetScale(pLyr).ToString(), GetDatatype(pLyr).ToString());

                        dataGridLayers.Rows[intIndex].Tag = pLyr;
                        //dataGridLayers.Rows[intIndex].Visible = false;
                        (dataGridLayers.Rows[intIndex].Cells["colWhere"] as DataGridViewButtonCell).ReadOnly = true;
                        intIndex++;

                    }
                }
        }

        //private void dataGridLayers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        //    rowindex = e.RowIndex;
        //    //added by chulili 20110731 提取时选中一个层,默认选中裁剪
        //    if (e.ColumnIndex == 0)
        //    {
        //        dataGridLayers.Rows[rowindex].Cells[1].Value = dataGridLayers.Rows[rowindex].Cells[0].Value;
        //        //if (dataGridLayers.Rows[rowindex].Cells[0].Value.ToString().ToLower() == "false" )
        //        //{
        //        //    dataGridLayers.Rows[rowindex].Cells[1].Value = true;
        //        //}
        //        //else
        //        //{
        //        //    dataGridLayers.Rows[rowindex].Cells[1].Value = false ;
        //        //}
        //    }
        //    if (e.ColumnIndex == 1)
        //    {
        //        if (dataGridLayers.Rows[rowindex].Cells[1].Value.ToString().ToLower() == "true")
        //        {
        //            dataGridLayers.Rows[rowindex].Cells[0].Value = dataGridLayers.Rows[rowindex].Cells[1].Value;
        //        }
        //    }
        //}

        private void dataGridLayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            rowindex = e.RowIndex;
            //added by chulili 20110731 提取时选中一个层,默认选中裁剪
            if (e.ColumnIndex == 0)
            {
                if (dataGridLayers.Rows[rowindex].Cells[0].Value.ToString().ToLower() == "true")
                {
                    dataGridLayers.Rows[rowindex].Cells[0].Value = false;
                    dataGridLayers.Rows[rowindex].Cells[1].Value = false;
                }
                else
                {
                    dataGridLayers.Rows[rowindex].Cells[0].Value = true;
                    dataGridLayers.Rows[rowindex].Cells[1].Value = true;
                }

            }
            if (e.ColumnIndex == 1)
            {
                if (dataGridLayers.Rows[rowindex].Cells[1].Value.ToString().ToLower() == "false")
                {
                    dataGridLayers.Rows[rowindex].Cells[1].Value = true;
                    dataGridLayers.Rows[rowindex].Cells[0].Value = true;
                }
            }
        }
        //清除列表
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            dataGridLayers.Rows.Clear();
            mydic.Clear();
        }
        private void InitClsSelectLayerTree()
        {
            if (_ClsSelectLayerByTree == null)
            {
                _ClsSelectLayerByTree = new SysCommon.SelectLayer.ClsSelectLayerByTree();
            }
            _ClsSelectLayerByTree.Imagelist = this.ImageList;
            _ClsSelectLayerByTree.CheckBox = m_checkbox;
            _ClsSelectLayerByTree.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;

        }
        private void LoadSelectLayerByTree()
        {
            if (m_TmpWorkSpace != null)
            {
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(m_TmpWorkSpace, _layerTreePath);
            }
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(m_TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (_DicLayerList == null)
            {
                _DicLayerList = new Dictionary<string, string>();
            }
            _ClsSelectLayerByTree.CreatRootNode(m_Map, _layerTreePath, this.advTreeLayerList, tbclick);//创建根节点
            _ClsSelectLayerByTree.RemoveChild(advTreeLayerList.Nodes[0], ref remove);

        }
        //双击选择叶子节点 
        private void advTreeLayerList_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (m_checkbox)
                return;
            m_NodeKey = "";
            if (e.Node == null)
                return;
            if (e.Node.Tag.ToString() != "Layer")//不是叶子节点 返回
            {
                return;
            }

            _ClsSelectLayerByTree.GetNodeKey(e.Node, labelErr, m_m, ref m_NodeKey, ref m_NodeText);
            if (string.IsNullOrEmpty(m_NodeKey))
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GetSelectLayer()
        {

            if (!m_checkbox)//不支持多选的
            {
                m_NodeKey = "";
                if (advTreeLayerList.SelectedNode == null)
                    return;
                if (advTreeLayerList.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
                {
                    return;
                }

                _ClsSelectLayerByTree.GetNodeKey(advTreeLayerList.SelectedNode, labelErr, m_m, ref m_NodeKey, ref m_NodeText);
                if (string.IsNullOrEmpty(m_NodeKey))
                    return;
            }
            else//支持多选的 
            {

                if (advTreeLayerList.Nodes.Count > 0)
                {
                    ListNode.Clear();//清空节点
                    _ClsSelectLayerByTree.GetCheckedNodetoMap(advTreeLayerList.Nodes[0], ListNode);
                    try
                    {
                        for (int i = 0; i < ListNode.Count; i++)
                        {
                            // IFeatureLayer pfeaturelayer = new FeatureLayerClass();
                            // pfeaturelayer.Name = ListNode[i].Text;
                            //pfeaturelayer.FeatureClass=SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace,_LayerTreePath,GetNodeKey1(ListNode[i]));

                            ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(m_TmpWorkSpace, m_Mapold, _ClsSelectLayerByTree.GetNodeKey1(ListNode[i], labelErr, m_m), _LayerTreeXmldoc, false);
                            if (pLayer == null) continue;
                            //m_returnMap.AddLayer(pLayer);
                            m_DicLayer.Add(_ClsSelectLayerByTree.GetNodeKey1(ListNode[i], labelErr, m_m), pLayer);
                        }
                    }
                    catch { }
                }
            }
        }

        //同步目录所有节点
        private void btn_TbCatalog_Click(object sender, EventArgs e)
        {
            tbclick = true;
            _ClsSelectLayerByTree.CreatRootNode(m_Map, _layerTreePath, this.advTreeLayerList, tbclick);
        }
        private void advTreeLayerList_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            DevComponents.AdvTree.AdvTree tree = sender as DevComponents.AdvTree.AdvTree;
            DevComponents.AdvTree.Node node = tree.SelectedNode;

            if (node == null) return;
            //如果是叶子节点
            if (flag2 && flag)
            {
                if (flag && node.Nodes.Count == 0)
                {
                    //flag2 = false;
                    //node.Parent.Checked = true;
                    //flag2 = true;
                    _ClsSelectLayerByTree.ChangeParentCheck(node, ref flag2);
                }
                else if (flag2)//不是叶子节点 
                {

                    //foreach (DevComponents.AdvTree.Node item in node.Nodes)
                    //{
                    //    flag = false;
                    //    item.Checked = node.Checked;
                    //}
                    //flag = true;
                    _ClsSelectLayerByTree.ChangeParentCheck(node, ref flag2);
                    _ClsSelectLayerByTree.ChangeChildCheck(node, ref flag);
                }
            }
        }
        //ygc 20130409 通过
        private IFeatureLayer GetTempFeatureLayer(string TempPath, out Exception ex)
        {
            ex = null;
            IFeatureLayer pFeatureLayer = null;
            IWorkspace pWorkspace = null;
            IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
            try
            {
                pWorkspace = pGDBWorkSpace.OpenFromFile(TempPath, 0);
                pFeatureLayer = new FeatureLayerClass();
                IFeatureClass pFeatureClass = GetFeatureClass(pWorkspace, "xbm", "");
                if (pFeatureClass != null) pFeatureLayer.FeatureClass = pFeatureClass;
            }
            catch (Exception e)
            {
                ex = e;
                return null;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pGDBWorkSpace);
            }
            return pFeatureLayer;
        }
        //通过IWorkspace获取图层
        public static IFeatureClass GetFeatureClass(IWorkspace pWorkspace, string LayerName, string DatasetName)
        {
            if (pWorkspace == null)
            {
                return null;
            }
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                if (pDataset.Name == LayerName)
                {
                    return pDataset as IFeatureClass;
                }
                pDataset = pEnumDataset.Next();
            }
            pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            pDataset = pEnumDataset.Next();
            //  IFeatureDataset pFeaDataset = pEnumDataset.Next() as IFeatureDataset;
            while (pDataset != null)
            {
                if (DatasetName == "")
                {
                    IFeatureClassContainer pFeatureClassContainer = pDataset as IFeatureClassContainer;
                    IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                    IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                    while (pFeatureClass != null)
                    {
                        if (pFeatureClass.AliasName.Contains(LayerName))
                        {
                            return pFeatureClass;
                        }
                        pFeatureClass = pEnumFeatureClass.Next();
                    }
                }
                else
                {
                    if (pDataset.Name == DatasetName)
                    {
                        IFeatureClassContainer pFeatureClassContainer = pDataset as IFeatureClassContainer;
                        IEnumFeatureClass pEnumFeatureClass = pFeatureClassContainer.Classes;
                        IFeatureClass pFeatureClass = pEnumFeatureClass.Next();
                        while (pFeatureClass != null)
                        {
                            if (pFeatureClass.AliasName.Contains(LayerName))
                            {
                                return pFeatureClass;
                            }
                            pFeatureClass = pEnumFeatureClass.Next();
                        }
                    }
                }
                pDataset = pEnumDataset.Next() as IFeatureDataset;
            }
            return null;
        }
        //ygc 20130411 省级标准向国家标准转换
        private string GetChangeValue(string SourceFieldName, string TargetFieldName, string SourceValue,IFeature pFeature)
        {
            string newValue = "";
            switch (TargetFieldName)
            {
                case "SHENG":
                    newValue = SourceValue.Substring(0, 2);
                    break;
                case "XIANG":
                    newValue = "0"+SourceValue.Substring(6,2);
                    break;
                case "CUN":
                    newValue = "0" + SourceValue.Substring(8, 2);
                    break;
                case "LIN_CHANG":
                    if (SourceValue.Trim ()!= string.Empty)
                    {
                        newValue = SourceValue.Substring(SourceValue.Length -3,3);
                    }
                    break;
                case "XIAO_BAN":
                    if (SourceValue.Trim ()!= string .Empty && SourceValue.Length != 4)
                    {
                        SourceValue = "0000" + SourceValue;
                        newValue = SourceValue.Substring(SourceValue.Length -4,4);
                    }
                    break;
                case "PO_DU":
                    if (SourceValue.Trim() != string.Empty)
                    {
                        newValue = DisposePD(SourceValue);
                    }
                    break;
                case "TU_RANG_LX":
                    if (SourceValue.Trim() != string.Empty) { newValue = DisposeTR(SourceValue); }
                    break;
                case "TU_CENG_HD":
                    newValue = DisposeTCHD(SourceValue);
                    break;
                case "MIAN_JI":
                    if (SourceValue.Trim() != string.Empty)
                    {
                        try
                        {
                            double tempMJ = Convert.ToDouble(SourceValue);
                            newValue = Math.Round(tempMJ, 2).ToString();
                        }
                        catch { }
                    }
                    break;
                case "LD_QS":
                    if (SourceValue == "15" || SourceValue == "16")
                    {
                        newValue = "";
                    }
                    else { newValue = SourceValue; }
                    break;
                case "DI_LEI":
                    newValue = DisposeDL(SourceValue);
                    break;
                case "YU_BI_DU":
                    newValue = DisposeYBD(SourceValue, pFeature);
                    break;
                case "LIN_ZHONG":
                    newValue = DisposeLZ(SourceValue, pFeature);
                    break;
                case "SEN_LIN_LB":
                    if (SourceValue.Trim() != string.Empty)
                    {
                        newValue = "0" + SourceValue;
                    }
                    break;
                case "SHI_QUAN_D":
                    if (SourceValue.Trim() != string.Empty)
                    {
                        newValue = SourceValue + "0";
                    }
                    break;
                case "GJGYL_BHDJ":
                    newValue = DisposeGYLBHDJ(SourceValue, pFeature);
                    break;
                case "G_CHENG_LB":
                    newValue = DisposeGCLB(SourceValue);
                    break;
                case "YOU_SHI_SZ":
                    newValue = DisposeYSSZ(SourceValue, pFeature);
                    break;
            }
            return newValue;
        }
      //ygc 20130411 坡度值处理
        private string DisposePD(string OldValue)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt16(OldValue);
            }
            catch { }
            if (temp < 5)
            {
                return "1";
            }
            else if (5 <= temp && temp < 15)
            {
                return "2";
            }
            else if (15 <= temp && temp < 25)
            {
                return "3";
            }
            else if (25 <= temp && temp < 35)
            {
                return "4";
            }
            else if (35 <= temp && temp < 45)
            {
                return "5";
            }
            else if (45 <= temp)
            {
                return "6";
            }
            return "";
        }
        //ygc 20130411 土壤字段值处理
        private string DisposeTR(string OldValue)
        {
            string newValue = "";
            if (OldValue == "1" || OldValue == "2")
            {
                newValue = "112";
            }
            else if (OldValue == "3" || OldValue == "4" || OldValue == "5" || OldValue == "6")
            {
                newValue = "122";
            }
            else if (OldValue == "7" || OldValue == "8" || OldValue == "9")
            {
                newValue = "133";
            }
            else if (OldValue == "10" || OldValue == "11" || OldValue == "12" || OldValue == "13" || OldValue == "14")
            {
                newValue = "124";
            }
            else if (OldValue == "15" || OldValue == "16")
            {
                newValue = "185";
            }
            else if (OldValue == "17" || OldValue == "18")
            {
                newValue = "133";
            }
            else if (OldValue == "19")
            {
                newValue = "192";
            }
            else if (OldValue == "20" || OldValue == "21")
            {
                newValue = "191";
            }
            else if (OldValue == "22" || OldValue == "23" || OldValue == "24") { newValue = "163"; }
            else if (OldValue == "25") { newValue = "151"; }
            else if (OldValue == "26" || OldValue == "27") { newValue = "171"; }
            else if (OldValue == "28") { newValue = "152"; }
            return newValue;
        }
        //ygc 20130411 图层厚度值处理
        private string DisposeTCHD(string OldValue)
        {
            string newValue = "";
            if (OldValue == "1") { newValue = "8"; }
            else if (OldValue == "2") { newValue = "25"; }
            else if (OldValue == "3") { newValue = "40"; }
            else if (OldValue == "4") { newValue = "75"; }
            else if (OldValue == "5") { newValue = "110"; }
            else { newValue = "0"; }
            return newValue;
        }
        //ygc 20130412 地类值处理
        private string DisposeDL(string OldValue)
        {
            string newValue = "";
            if (OldValue == "112" || OldValue == "114") { newValue = "111"; }
            else if (OldValue == "174") { newValue = "173"; }
            else if (OldValue == "211" || OldValue == "212") { newValue = "210"; }
            else if (OldValue == "221" || OldValue == "222" || OldValue == "223" || OldValue == "241") { newValue = "220"; }
            else if (OldValue == "231" || OldValue == "232" || OldValue == "233" || OldValue == "234" || OldValue == "246") { newValue = "230"; }
            else if (OldValue == "242" || OldValue == "243" || OldValue == "244" || OldValue == "245" || OldValue == "247" || OldValue == "254") { newValue = "240"; }
            else if (OldValue == "253" || OldValue == "252" || OldValue == "251") { newValue = "250"; }
            else { newValue = OldValue; }
            if (newValue .Trim ()!=string .Empty)
            {
                newValue = "0000" + newValue;
                newValue = newValue.Substring(newValue.Length - 4, 4);
            }
            return newValue;
        }
        //ygc 20130412 林种值处理
        private string DisposeLZ(string OldValue, IFeature pFeature)
        {
            string newValue = "";
            string DLValue = "";
            int DLindex = pFeature.Fields.FindField("DL");
            if (DLindex > -1)
            {
                DLValue = pFeature.get_Value(DLindex).ToString();
            }
            if (DLValue == "111" || DLValue == "112" || DLValue == "114" || DLValue == "120" || DLValue == "131" || DLValue == "132")
            {
                newValue = OldValue;
            }
            else
            {
                newValue = "";
            }
            if (newValue == "300") { newValue = ""; }
            return newValue;
        }
        //ygc 20130412 公益林保护等级处理
        private string DisposeGYLBHDJ(string OldValue, IFeature pFeature)
        {
            string newValue = "";
            int sqIndex = pFeature.Fields.FindField("SQ");
            string SQValue = "";
            if (sqIndex > -1)
            {
                SQValue = pFeature.get_Value(sqIndex).ToString();
            }
            if (SQValue == "1")
            {
                newValue = OldValue;
            }
            return newValue;
        }
        //ygc 20130412 工程类别处理
        private string DisposeGCLB(string OldValue)
        {
            string newValue = "";
            if (OldValue == "12" || OldValue == "21" || OldValue == "26" ||
                OldValue == "27" || OldValue == "30" || OldValue == "51" ||
                OldValue == "52" || OldValue == "53" || OldValue == "54" || OldValue == "60")
            {
                newValue = OldValue;
            }
            else if (OldValue == "40") { newValue = "41"; }
            else if (OldValue == "61" || OldValue == "62" || OldValue == "63" || OldValue == "64" || OldValue == "65" || OldValue == "66")
            { newValue = "90"; }
            else { newValue = string.Empty; }
            return newValue;
        }
        //ygc 20130412 郁闭度处理
        private string DisposeYBD(string OldValue, IFeature pFeature)
        {
            string newValue = "0";
            int DLIndex = pFeature.Fields.FindField("DL");
            string DLValue = "";
            if (DLIndex > -1)
            {
                DLValue = pFeature.get_Value(DLIndex).ToString();
            }
            if (DLValue == "131" || DLValue == "132")
            {
                int GMGDindex = pFeature.Fields.FindField("GMGD");
                if (GMGDindex > -1)
                {
                    newValue = pFeature.get_Value(GMGDindex).ToString();
                }
            }
            else if (DLValue == "111" || DLValue == "112" || DLValue == "113" || DLValue == "114" || DLValue == "120")
            {
                newValue = OldValue;
            }
            double temp = Convert.ToDouble(newValue);
            newValue = Math.Round(temp / 100, 2).ToString();
            return newValue;
        }
        //ygc 20130412 优势树种处理
        private string DisposeYSSZ(string OldValue, IFeature pFeature)
        {
            string newValue = "";
            int dlIndex = pFeature.Fields.FindField("DL");
            if (dlIndex > -1)
            {
                string dlValue = pFeature.get_Value(dlIndex).ToString();
                if (dlValue == "131" || dlValue == "132")
                {
                    if (OldValue == "909") newValue = "712000";
                    else if (OldValue == "910") newValue = "762000";
                    else if (OldValue == "911") newValue = "920100";
                    else if (OldValue == "912") newValue = "920200";
                    else if (OldValue == "913") newValue = "920300";
                    else if (OldValue == "914") newValue = "932000";
                    else if (OldValue == "919") newValue = "807000";
                    else if (OldValue == "925") newValue = "982000";
                    else if (OldValue == "927") newValue = "988000";
                    else if (OldValue == "930") newValue = "923000";
                    else if (OldValue == "932") newValue = "941000";
                    else if (OldValue == "720") newValue = "719000";
                    else if (OldValue == "722") newValue = "712000";
                    else if (OldValue == "350") newValue = "351000";
                    else if (OldValue == "411") newValue = "828000";
                    else if (OldValue == "414") newValue = "490400";
                    else if (OldValue == "420") newValue = "428000";
                    else if (OldValue == "491") newValue = "465000";
                    else if (OldValue == "492") newValue = "492100";
                    else if (OldValue == "493") newValue = "490000";
                    else if (OldValue == "494") newValue = "460000";
                    else if (OldValue == "531") newValue = "539100";
                    else if (OldValue == "545") newValue = "591000";
                    else if (OldValue == "591") newValue = "531000";
                    else if (OldValue == "130") newValue = "110000";
                    else if (OldValue == "360") newValue = "350100";
                    else if (OldValue == "380") newValue = "360000";
                }
                else
                {
                    int zyszIndex = pFeature.Fields.FindField("ZYSZ");
                    if (zyszIndex > -1)
                    {
                        string zyszValue = pFeature.get_Value(zyszIndex).ToString();
                        if (zyszValue .Trim ()!=string .Empty)
                        {
                            newValue = zyszValue + "000";
                        }
                    }
                }
            }
            return newValue;
        }
    }
}