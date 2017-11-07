using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.IO;
using System.Xml;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

namespace GeoDataCenterFunLib
{
    public partial class FrmGetXZQLocation : DevComponents.DotNetBar.Office2007Form
    {
        public FrmGetXZQLocation()
        {
            InitializeComponent();
        }
        private string _XZQpath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";//行政区XML文件
        public static string _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\行政区定位图层树.xml";
        private List<string> m_listProvince;
        private List<string> m_listCity;
        private List<string> m_listCounty;
        private List<string> m_listXiang;
        private List<string> m_listCun;

        private string m_Province;
        private string m_City;
        private string m_Country;
        private string m_Xiang;
        //定位地图
        public IMapControlDefault m_DefaultMap
        {
            get;
            set;
        }
        //是否定位后关闭
        private bool IsClose = false;
        public bool m_IsClose
        {
            get { return IsClose; }
            set { IsClose =value; }
        }
        public string m_XZQCode
        {
            get;
            set;
        }
        private bool IsLocation = true;
        public bool m_IsLocation
        {
            get { return IsLocation; }
            set { IsLocation = value; }
        }//是否定位
        public string m_XZQ
        {
            get;
            set;
        }
        //选择行政区后的范围
        public IGeometry m_pGeometry
        {
            get;
            set;
        }
        private void FrmGetXZQLocation_Load(object sender, EventArgs e)
        {
            InitializeCbProvince();
        }
        private void InitializeCbProvince()
        {
            m_listProvince = new List<string>();
            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 1 + "'";
            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;
            IRow pRow = pCursor.NextRow();
            m_listProvince.Clear();
            while (pRow != null)
            {
                cbsheng.Items.Add(pRow.get_Value(ndx).ToString());
                m_listProvince.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (m_listProvince.Count <= 0)
            {
                MessageBox.Show("无省级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cbsheng.SelectedIndex = 0;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        private void cbsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_listCity = new List<string>();
            cbShi.Items.Clear();
            cbShi.Text = "";
            cbXian.Items.Clear();
            cbXian.Text = "";
            cbXiang.Items.Clear();
            cbXiang.Text = "";
            cbcun.Items.Clear();
            cbcun.Text = "";
            int ProvinceIndex = cbsheng.SelectedIndex;
            m_Province = m_listProvince[ProvinceIndex];

            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 2 + "'and substr(code,1,2)= '" + m_Province + "' and substr(code,1,3)<>'149'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();


            while (pRow != null)
            {
                cbShi.Items.Add(pRow.get_Value(ndx).ToString());
                m_listCity.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (m_listCity.Count <= 0)
            {
                MessageBox.Show("无市级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        private void cbShi_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_listCounty = new List<string>();
            cbXian.Items.Clear();
            cbXian.Text = "";
            cbXiang.Items.Clear();
            cbXiang.Text = "";
            cbcun.Items.Clear();
            cbcun.Text = "";
            int cityIndex = cbShi.SelectedIndex;

            m_City = m_listCity[cityIndex];

            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 3 + "' and substr(code,1,4)='" + m_City + "'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();


            while (pRow != null)
            {
                cbXian.Items.Add(pRow.get_Value(ndx).ToString());
                m_listCounty.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (m_listCounty.Count <= 0)
            {
                MessageBox.Show("无县级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        //点击确定按钮；完成定位
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_IsLocation)
            {
                string xiangCode = "";
                if (cbcun.SelectedItem != null)
                {
                    int CunIndex = cbcun.SelectedIndex;
                    xiangCode = m_listCun[CunIndex];
                }
                else if (m_Xiang != null && m_Xiang != "" && cbXiang.SelectedItem != null)
                {
                    xiangCode = m_Xiang;
                }
                else if (m_Country != null && m_Country != "" && cbXian.SelectedItem != null)
                {
                    xiangCode = m_Country;
                }
                else if (m_City != null && m_City != "" && cbShi.SelectedItem != null)
                {
                    xiangCode = m_City;
                }
                else
                {
                    xiangCode = m_Province;
                }
                if (xiangCode == "") return;
                string strXZQFieldName = GetXZQFieldName(xiangCode.Length);
                string config = GetConfig(xiangCode.Length);
                if (config == "" || strXZQFieldName == "") return;
                IFeatureClass pFeatureClass = getExtentByXZQ(config);
                if (pFeatureClass == null)
                {
                    MessageBox.Show("无法获取图层信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                IGeometry pGeometry = null;
                if (cbXiang.SelectedItem == null)
                {
                    if (xiangCode.Length == 4)
                    {
                        xiangCode = xiangCode.Substring(2, 2);
                    }//2013-02-28ygc 市级行政区界限特殊处理
                    pGeometry = getXZQExtentFromFL(pFeatureClass, xiangCode, strXZQFieldName);
                }
                else if (cbXiang.SelectedItem != null && cbcun.SelectedItem == null)
                {
                    string xianFieldName = GetXZQFieldName(xiangCode.Length - 2);
                    pGeometry = getXZQExtentFromFL(pFeatureClass, "0" + xiangCode.Substring(6, 2), strXZQFieldName, xianFieldName, xiangCode.Substring(0, 6));
                }
                else if (cbcun.SelectedItem != null)
                {
                    //ygc 20130417村级定位处理
                    pGeometry = getXZQExtentFromFL(pFeatureClass, xiangCode, strXZQFieldName);
                }
                if (pGeometry == null)
                {
                    MessageBox.Show("未找到对应的行政区！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m_pGeometry = pGeometry;
                if (m_DefaultMap == null) return;
                //定位
                IEnvelope pExtent = pGeometry.Envelope;
                ///ZQ 20111020 定位范围扩大1.5倍
                SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
                (m_DefaultMap.Map as IActiveView).Extent = pExtent;
                //ZQ    20110914    modify   改变显示方式
                //drawPolygonElement(pGeometry as IPolygon, psGra);
                m_DefaultMap.Refresh(esriViewDrawPhase.esriViewAll, null, null);
                m_DefaultMap.ActiveView.ScreenDisplay.UpdateWindow();
                //end
                ITopologicalOperator pTopologicalOperator = pGeometry as ITopologicalOperator;
                m_DefaultMap.FlashShape(pGeometry, 3, 200, null);
                drawgeometryXOR(pTopologicalOperator.Boundary);

                m_XZQCode = xiangCode;
                if (m_IsClose)
                {
                    this.Close();
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                m_XZQ = cbsheng.Text + cbShi.Text + cbXian.Text + cbXiang.Text + cbcun.Text;
                if (cbcun.SelectedItem != null)
                {
                    int CunIndex = cbcun.SelectedIndex;
                    m_XZQCode = m_listCun[CunIndex];
                }
                else if (m_Xiang != null && m_Xiang != "" && cbXiang.SelectedItem != null)
                {
                    m_XZQCode = m_Xiang;
                }
                else if (m_Country != null && m_Country != "" && cbXian.SelectedItem != null)
                {
                    m_XZQCode = m_Country;
                }
                else if (m_City != null && m_City != "" && cbShi.SelectedItem != null)
                {
                    m_XZQCode = m_City;
                }
                else
                {
                    m_XZQCode = m_Province;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        //关闭窗口
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private IFeatureClass getExtentByXZQ(string layerConfig)
        {
            if (File.Exists(_XZQpath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_XZQpath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//LayerConfig/" + layerConfig);
                XmlElement pEle = pNode as XmlElement;
                if (pEle != null)
                {
                    string strNodeKey = "";
                    string strXZBM = "";
                    if (pEle.HasAttribute("NodeKey"))
                    {
                        strNodeKey = pEle.GetAttribute("NodeKey");
                    }
                    if (pEle.HasAttribute("XZBMField"))
                    {
                        strXZBM = pEle.GetAttribute("XZBMField");
                    }
                    pXmldoc = null;
                    IFeatureClass pFeatureClass =GetFeatureClassByNodeKey(strNodeKey);
                    return pFeatureClass;
                }
                pXmldoc = null;
            }
            return null;
        }

        //根据行政区图层，行政编码字段名称和字段值，获取对应的地物几何数据
        private IGeometry getXZQExtentFromFL(IFeatureClass pXZQFeaCls, string strXZQBM, string strFieldXZQBM)
        {

            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (pXZQFeaCls != null)
                {//查找行政编码属性列
                    int iIndex = pXZQFeaCls.Fields.FindField(strFieldXZQBM);
                    IField pField = pXZQFeaCls.Fields.get_Field(iIndex);
                    if (strXZQBM.Length == 10)
                    {
                        pQueryFilter.WhereClause = "xian='" + strXZQBM.Substring(0, 6) + "' and xiang='0" + strXZQBM.Substring (6,2)+"' and "+strFieldXZQBM + "='0" + strXZQBM.Substring(8,2)+ "'";
                    }
                    else
                    {
                        //构造过滤条件
                        if (pField.Type.ToString() == "esriFieldTypeString")
                        {

                            pQueryFilter.WhereClause = strFieldXZQBM + "='" + strXZQBM + "'";
                        }
                        else if (pField.Type.ToString() == "esriFieldTypeDouble")
                        {
                            pQueryFilter.WhereClause = strFieldXZQBM + "=" + strXZQBM;
                        }
                    }
                    //end
                    //查找
                    IFeatureCursor pFCursor = pXZQFeaCls.Search(pQueryFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    //只获取找到的第一个地物
                    if (pFeature != null)
                    {
                        pFCursor = null;
                        pQueryFilter = null;
                        return pFeature.ShapeCopy;
                    }
                    pFCursor = null;
                    pQueryFilter = null;
                }

            }
            catch
            {

            }
            return null;
        }

        private IGeometry getXZQExtentFromFL(IFeatureClass pXZQFeaCls, string strXZQBM, string strFieldXZQBM,string xianFieldName,string xianFieldValue)
        {

            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (pXZQFeaCls != null)
                {//查找行政编码属性列
                    int iIndex = pXZQFeaCls.Fields.FindField(strFieldXZQBM);
                    IField pField = pXZQFeaCls.Fields.get_Field(iIndex);
                    //构造过滤条件
                    if (pField.Type.ToString() == "esriFieldTypeString")
                    {

                        pQueryFilter.WhereClause = strFieldXZQBM + "='" + strXZQBM + "' and  " + xianFieldName + "='" + xianFieldValue+"'";
                    }
                    else if (pField.Type.ToString() == "esriFieldTypeDouble")
                    {
                        pQueryFilter.WhereClause = strFieldXZQBM + "=" + strXZQBM + " and " + xianFieldName + "= " + xianFieldValue;
                    }
                    //end
                    //查找
                    IFeatureCursor pFCursor = pXZQFeaCls.Search(pQueryFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    //只获取找到的第一个地物
                    if (pFeature != null)
                    {
                        pFCursor = null;
                        pQueryFilter = null;
                        return pFeature.ShapeCopy;
                    }
                    pFCursor = null;
                    pQueryFilter = null;
                }

            }
            catch
            {

            }
            return null;
        }
        //根据村级行政区代码获取村级范围
        private void cbXian_SelectedIndexChanged(object sender, EventArgs e)
        { 
            m_listXiang = new List<string>();
            cbXiang.Items.Clear();
            cbXiang.Text = "";
            cbcun.Items.Clear();
            cbcun.Text = "";

            int cityIndex = cbXian.SelectedIndex;
            m_Country = "";
            m_Country = m_listCounty[cityIndex];

            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 4 + "' and substr(code,1,6)='" + m_Country + "'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();


            while (pRow != null)
            {
                cbXiang.Items.Add(pRow.get_Value(ndx).ToString());
                m_listXiang.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (m_listXiang.Count <= 0)
            {
                MessageBox.Show("无乡级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        private string GetXZQFieldName(int codeLen)
        {
            if (codeLen == 0) return "";
            switch (codeLen)
            {
                case 2:
                    return "sheng";
                case 4:
                    return "shi";
                case 6:
                    return "xian";
                case 8:
                    return "xiang";
                case 10:
                    return "cun";
                default :
                    return "";
            }
        }
        //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(string strNodeKey)
        {
            if (strNodeKey.Equals(""))
            {
                return null;
            }
            //目录树路径变量:_layerTreePath
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(_layerTreePath))
            {
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, _layerTreePath);
            }
            else
            {
                File.Delete(_layerTreePath);
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, _layerTreePath);
            }
            if (!File.Exists(_layerTreePath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(_layerTreePath);
            string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return null;
            }
            //获取图层名,数据源id
            string strFeaClassName = "";
            string strDBSourceID = "";
            try
            {
                strFeaClassName = pNode.Attributes["Code"].Value;
                strDBSourceID = pNode.Attributes["ConnectKey"].Value;
            }
            catch
            { }
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            object objConnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDBSourceID, out eError);
            string conninfostr = "";
            if (objConnstr != null)
            {
                conninfostr = objConnstr.ToString();
            }
            object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "ID=" + strDBSourceID, out eError);
            int type = -1;
            if (objType != null)
            {
                type = int.Parse(objType.ToString());
            }
            //根据数据源连接信息,获取数据源连接
            IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null)
            {
                return null;
            }
            //打开地物类
            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeaClass = null;
            try
            {
                pFeaClass = pFeaWorkSpace.OpenFeatureClass(strFeaClassName);
            }
            catch
            { }
            if (File.Exists(_layerTreePath))
            {
                File.Delete(_layerTreePath);
            }
            return pFeaClass;

        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
            //end added by chulili 20111109
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }
        private string GetConfig(int CodeLen)
        {
            if (CodeLen == 0) return "";
            switch (CodeLen)
            {
                case 2:
                    return "Province";
                case 4:
                    return "City";
                case 6:
                    return "County";
                case 8:
                    return "Town";
                case 10:
                    return "Village";
                default :
                    return "";
            }
        }
        public void drawgeometryXOR(IGeometry pGeometry)
        {
            if (pGeometry == null)//如果窗体关闭或者取消 就不绘制 xisheng 2011.06.28
            {
                return;
            }
            IScreenDisplay pScreenDisplay = m_DefaultMap.ActiveView.ScreenDisplay;
            ISymbol pSymbol = null;
            //颜色对象
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;
            pRGBColor = getRGB(255, 0, 0);
            pRGBColor.Transparency = 255;
            try
            {
                switch (pGeometry.GeometryType.ToString())
                {
                    case "esriGeometryPoint"://点要素
                        ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                        pMarkerSymbol.Size = 2.0;
                        pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                        pMarkerSymbol.Color = pRGBColor;
                        pSymbol = (ISymbol)pMarkerSymbol;
                        break;
                    case "esriGeometryPolyline"://线要素
                        ISimpleLineSymbol pPolyLineSymbol = new SimpleLineSymbolClass();
                        pPolyLineSymbol.Color = pRGBColor;
                        pPolyLineSymbol.Width = 2.5;
                        pPolyLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        pSymbol = (ISymbol)pPolyLineSymbol;
                        pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;
                        break;
                    case "esriGeometryPolygon"://面要素
                        ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                        ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

                        pSymbol = (ISymbol)pFillSymbol;
                        pSymbol.ROP2 = esriRasterOpCode.esriROPCopyPen;

                        pLineSymbol.Color = pRGBColor;
                        pLineSymbol.Width = 1.5;
                        pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        pFillSymbol.Outline = pLineSymbol;

                        pFillSymbol.Color = pRGBColor;
                        pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                        break;
                }


                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);
                switch (pGeometry.GeometryType.ToString())
                {
                    case "esriGeometryPoint"://点要素
                        pScreenDisplay.DrawPoint(pGeometry);
                        break;
                    case "esriGeometryPolyline"://线要素
                        pScreenDisplay.DrawPolyline(pGeometry);
                        break;
                    case "esriGeometryPolygon"://面要素
                        pScreenDisplay.DrawPolygon(pGeometry);
                        break;
                }
                pScreenDisplay.FinishDrawing();

            }
            catch
            { }
            finally
            {
                pSymbol = null;
                pRGBColor = null;
            }
        }
        private static IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Red = r;
            pRgbColor.Green = g;
            pRgbColor.Blue = b;
            return pRgbColor;
        }

        private void cbXiang_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_listCun = new List<string>();
            cbcun.Items.Clear();
            cbcun.Text = "";
            int xiangIndex = cbXiang.SelectedIndex;
            m_Xiang = "";
            m_Xiang =m_listXiang [xiangIndex];

            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 5 + "' and substr(code,1,8)='" + m_Xiang + "'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();


            while (pRow != null)
            {
                cbcun.Items.Add(pRow.get_Value(ndx).ToString());
                m_listCun.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (m_listXiang.Count <= 0)
            {
                MessageBox.Show("无村级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

    }
}
