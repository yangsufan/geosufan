using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Xml;

namespace GeoDataCenterFunLib
{
    public partial class frmQueryDomain : DevComponents.DotNetBar.Office2007Form
    {
        /// <summary>
        /// 作者：yjl
        /// 日期：20110730
        /// 说明：地名查询窗体
        /// </summary>
        private string xzdmBH;//行政代码编号AH  AI AJ
        private Dictionary<string, string> zrdm1 = null;//对应关系
        private string strSqlWhere = "";
        //private string fdDMFL = "";//地名分类字段     //changed by chulili 20110825 地名查询现在用不到地名分类字段
        private string fdDM = "";//地名字段
        public string _QueryTag = "";//added by chulili 20110731标明是自然还是行政地名 取值"ZR" "XZ"

        private string _XZLayerName = "";//行政地名图层名

        private string _ZRLayername = "";//自然地名图层名
        List<string> m_ListField = null;
        private Form _OwnerFrm = null;
        private IMapControlDefault _MapControl = null;
        private IFeatureClass _QueryFeaClass_XZ = null;
        private IFeatureClass _QueryFeaClass_ZR = null;
        private frmQuerytoTable m_frmQuery = null;
        public SysCommon.BottomQueryBar QueryBar
        {
            get;
            set;
        }
        private IQueryFilter _QueryFilterAll = null;
        private IQueryFilter _QueryFilterPart1 = null;
        private IQueryFilter _QueryFilterPart2 = null;
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
        private List<ILayer> _ListLayers = null;
        private List<IFeatureClass> _ListFeatureClasses = null;
        private List<string> _ListLayerNames = null;
        private List<string> _ListNodeKeys = null;
        private string m_SelectXZQ;
        //public frmQueryDomain(IWorkspace pTmpwks, Form pFrm, IMapControlDefault pMapControl, IFeatureClass pXZFeatureClass,IFeatureClass pZRFeatureClass, string XZLayername,string ZRLayername, string inFdDM)
        //{
        //    InitializeComponent();
        //    m_ListField = new List<string>();
        //    _OwnerFrm = pFrm;
        //    _MapControl = pMapControl;
        //    _QueryFeaClass_XZ = pXZFeatureClass;
        //    _QueryFeaClass_ZR = pZRFeatureClass;

        //    _XZLayerName = XZLayername;//行政地名图层名
        //    _ZRLayername = ZRLayername;//自然地名图层名
        //    //radioXZDM.Checked = true;
        //    //fdDMFL = inFdDMFL;
        //    fdDM = inFdDM;
        //    txtLayer.Text = XZLayername;
        //    if (SysCommon.ModField._DicFieldName.Count == 0)
        //    {
        //        SysCommon.ModField.InitNameDic(pTmpwks, SysCommon.ModField._DicFieldName, "属性对照表");
        //    }
        //    if (_QueryFeaClass_XZ != null)
        //    {
        //        IntialComBox(_QueryFeaClass_XZ);
        //    }

        //    //ComField.Items.Add(SysCommon.ModField.GetChineseNameOfField(fdDM));
        //    //m_ListField.Add(inFdDM);
        //    ComField.Text = SysCommon.ModField.GetChineseNameOfField(fdDM);
        //}
        public frmQueryDomain(IWorkspace pTmpwks, Form pFrm, IMapControlDefault pMapControl,List<string> pListNodeKeys, List<ILayer> pListLayers,List<IFeatureClass> pListFeatureClasses,List<string> pListLayerNames, string inFdDM)
        {
            InitializeComponent();
            m_ListField = new List<string>();
            _OwnerFrm = pFrm;
            _MapControl = pMapControl;
            _ListLayers = pListLayers;
            _ListFeatureClasses = pListFeatureClasses;
            _ListLayerNames = pListLayerNames;
            _ListNodeKeys = pListNodeKeys;
            //radioXZDM.Checked = true;
            //fdDMFL = inFdDMFL;
            fdDM = inFdDM;
            txtLayer.Text = pListLayerNames[0];
            if (SysCommon.ModField._DicFieldName.Count == 0)
            {
                SysCommon.ModField.InitNameDic(pTmpwks, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (pListFeatureClasses[0] != null)
            {
                IntialComBox(pListFeatureClasses[0]);
            }

            //ComField.Items.Add(SysCommon.ModField.GetChineseNameOfField(fdDM));
            //m_ListField.Add(inFdDM);
            ComField.Text = SysCommon.ModField.GetChineseNameOfField(fdDM);
        }
        public string SqlWhere
        {
            get
            {
                return strSqlWhere;
            }
        }
        #region
        //private void radioXZDM_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (radioXZDM.Checked)
        //    {
        //        _QueryTag = "XZ";
        //        //cmbClass1.Items.Clear();
        //        //cmbClass2.Items.Clear();
        //        //cmbClass2.Text = "";
        //        //cmbClass1.Items.Add("省、直辖市、自治区");//AB
        //        //cmbClass1.Items.Add("地级市、自治州、地区");//AC  AD
        //        //cmbClass1.Items.Add("县、县级市");// AE  AF  AG
        //        //cmbClass1.Items.Add("乡镇、街道办"); //AH  AI AJ
        //        //cmbClass1.Items.Add("行政村");// AK
        //        //cmbClass1.Items.Add("自然村");//BA、BB、BC、BD
        //        //cmbClass1.Items.Add("企事业单位");//CA-CH

        //        //cmbClass2.Enabled = false;
        //        //cmbClass1.SelectedIndex = 0;
        //    }
        //}

        //private void radioZRDM_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (radioZRDM.Checked)
        //    {
        //        _QueryTag = "ZR";
        //        //cmbClass2.Enabled = true;
        //        //cmbClass1.Items.Clear();
        //        //cmbClass2.Items.Clear();
        //        //cmbClass1.Items.Add("交通要素");
        //        //cmbClass1.Items.Add("纪念地和古迹名");
        //        //cmbClass1.Items.Add("山名");
        //        //cmbClass1.Items.Add("陆地水域");
        //        //cmbClass1.Items.Add("海洋水域");
        //        //cmbClass1.Items.Add("自然地域");
        //        //cmbClass1.Items.Add("境界标志");
        //        //cmbClass1.SelectedIndex = 0;
        //    }
        //}

        //private void cmbClass1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    #region radioZRDM.Checked
        //    if (radioZRDM.Checked)
        //    {
        //        switch (cmbClass1.Text)
        //        {
        //            case "交通要素":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("空运站、机场名", "DA");
        //                    zrdm1.Add("海运航线名", "DB");
        //                    zrdm1.Add("海港名", "DC");
        //                    zrdm1.Add("内河航线名", "DD");
        //                    zrdm1.Add("内河港口名", "DE");
        //                    zrdm1.Add("渡口名", "DF");
        //                    zrdm1.Add("铁路线路名", "DG");
        //                    zrdm1.Add("铁路车站名", "DH");
        //                    zrdm1.Add("公路及乡村路名", "DI");
        //                    zrdm1.Add("公路站名", "DJ");
        //                    zrdm1.Add("桥梁、涵洞、隧道名", "DK");
        //                    zrdm1.Add("城市路、街、巷等名", "DL");
        //                    zrdm1.Add("管线、索道名", "DM");
        //                    zrdm1.Add("地铁线路名", "DN");
        //                    zrdm1.Add("地铁站名", "DO");
        //                    zrdm1.Add("其它", "DP");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "纪念地和古迹名":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("具有历史意义的纪念地", "EA");
        //                    zrdm1.Add("公园、风景名胜名", "EB");
        //                    zrdm1.Add("古建筑名（包括钟楼、鼓楼、城楼、关塞、庙宇、塔、宫殿、官邸、府衙、牌坊、碑、寺、石窟、祠、古桥等）", "EC");
        //                    zrdm1.Add("古长城名", "ED");
        //                    zrdm1.Add("古石刻名、摩岩名", "EE");
        //                    zrdm1.Add("古遗址名", "EF");
        //                    zrdm1.Add("古墓葬名", "EG");
        //                    zrdm1.Add("古战场名", "EH");
        //                    zrdm1.Add("其它", "EI");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "山名":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("山体名（包括山脉、山岭、火山、冰山、雪山等）", "HA");
        //                    zrdm1.Add("山峰名（山丘、崮等）", "HB");
        //                    zrdm1.Add("山坡名", "HC");
        //                    zrdm1.Add("谷地名", "HD");
        //                    zrdm1.Add("山崖名", "HE");
        //                    zrdm1.Add("洞穴名", "HF");
        //                    zrdm1.Add("山口名（包括垭口、关口、隘口等）", "HG");
        //                    zrdm1.Add("台地名（塬、坝子名）", "HH");
        //                    zrdm1.Add("其它", "HI");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "陆地水域":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("常年河流名", "IA");
        //                    zrdm1.Add("季节性河流名", "IB");
        //                    zrdm1.Add("消失河名", "IC");
        //                    zrdm1.Add("伏流河名", "ID");
        //                    zrdm1.Add("运河名", "IE");
        //                    zrdm1.Add("渠道名", "IF");
        //                    zrdm1.Add("湖泊名", "IG");
        //                    zrdm1.Add("水库名", "IH");
        //                    zrdm1.Add("蓄洪区名", "II");
        //                    zrdm1.Add("瀑布名", "IJ");
        //                    zrdm1.Add("泉名", "IK");
        //                    zrdm1.Add("井名", "IL");
        //                    zrdm1.Add("干涸河名", "IM");
        //                    zrdm1.Add("干涸湖名", "IN");
        //                    zrdm1.Add("冰川名", "IO");
        //                    zrdm1.Add("河口名", "IP");
        //                    zrdm1.Add("河滩名", "IQ");
        //                    zrdm1.Add("河曲、河湾、峡名", "IR");
        //                    zrdm1.Add("洲岛名", "IS");
        //                    zrdm1.Add("沼泽、湿地名", "IT");
        //                    zrdm1.Add("水利设施名(包括堤坝、水闸、输水隧道等)", "IU");
        //                    zrdm1.Add("其它", "IV");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "海洋水域":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("海洋名", "JA");
        //                    zrdm1.Add("海湾、港湾名", "JB");
        //                    zrdm1.Add("海峡名", "JC");
        //                    zrdm1.Add("水道名", "JD");
        //                    zrdm1.Add("岛、礁名", "JE");
        //                    zrdm1.Add("群岛、列岛名", "JF");
        //                    zrdm1.Add("半岛、岬角名", "JG");
        //                    zrdm1.Add("滩涂名", "JH");
        //                    zrdm1.Add("海盆名", "JI");
        //                    zrdm1.Add("海沟名", "JJ");
        //                    zrdm1.Add("海底山脉名", "JK");
        //                    zrdm1.Add("海岸名", "JL");
        //                    zrdm1.Add("海槽名", "JM");
        //                    zrdm1.Add("海底断裂带名", "JN");
        //                    zrdm1.Add("海底峡谷名", "JO");
        //                    zrdm1.Add("海底高原名", "JP");
        //                    zrdm1.Add("海底平原名", "JQ");
        //                    zrdm1.Add("大陆架、大陆坡名", "JR");
        //                    zrdm1.Add("其它", "JS");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "自然地域":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("平原名", "KA");
        //                    zrdm1.Add("凹地、盆地名", "KB");
        //                    zrdm1.Add("山地、丘陵名", "KC");
        //                    zrdm1.Add("高原名", "KD");
        //                    zrdm1.Add("草原名", "KE");
        //                    zrdm1.Add("绿洲名", "KF");
        //                    zrdm1.Add("荒漠、沙漠名", "KH");
        //                    zrdm1.Add("森林名", "KI");
        //                    zrdm1.Add("三角洲名", "KJ");
        //                    zrdm1.Add("盐田名", "KK");
        //                    zrdm1.Add("自然保护区名", "KL");
        //                    zrdm1.Add("其它", "KM");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //            case "境界标志":
        //                {
        //                    zrdm1 = new Dictionary<string, string>();
        //                    zrdm1.Clear();
        //                    zrdm1.Add("界碑名", "LA");
        //                    zrdm1.Add("界桩名", "LB");
        //                    zrdm1.Add("其它", "LC");
        //                    cmbClass2.Items.Clear();
        //                    foreach (KeyValuePair<string, string> kvp in zrdm1)
        //                    {
        //                        cmbClass2.Items.Add(kvp.Key);
        //                    }
        //                    cmbClass2.SelectedIndex = 0;
        //                    break;
        //                }
        //        }
        //    }
        //    #endregion radioZRDM.Checked
        //    #region else
        //    else
        //    {
        //        switch (cmbClass1.Text)
        //        {
        //            case "省、直辖市、自治区":
        //                {
        //                    xzdmBH = "'AB'";
        //                    break;
        //                }
        //            case "地级市、自治州、地区":
        //                {
        //                    xzdmBH = "'AC','AD'";
        //                    break;
        //                }
        //            case "县、县级市":
        //                {
        //                    xzdmBH = "'AE','AF','AG'";
        //                    break;
        //                }
        //            case "乡镇、街道办":
        //                {
        //                    xzdmBH = "'AH','AI','AJ'";
        //                    break;
        //                }
        //            case "行政村":
        //                {
        //                    xzdmBH = "'AK'";
        //                    break;
        //                }
        //            case "自然村":
        //                {
        //                    xzdmBH = "'BA','BB','BC','BD'";
        //                    break;
        //                }
        //            case "企事业单位":
        //                {
        //                    xzdmBH = "'CA','CB','CC','CD','CE','CF','CG','CH'";
        //                    break;
        //                }
        //        }
        //        #endregion
        //    }
        //}
        #endregion
        private void btnQuery_Click(object sender, EventArgs e)
        {    
            
            //if (radioZRDM.Checked)
            //{
            //    _QueryTag = "ZR";
            //    //strSqlWhere = fdDMFL + " =  '" + zrdm1[cmbClass2.Text] + "' AND " + fdDM + " LIKE '%" + txtName.Text.Trim() + "%'";
            //}
            //else
            //{
            //    _QueryTag = "XZ";
            //    //strSqlWhere = fdDMFL + " IN(" + xzdmBH + ") AND " + fdDM + " LIKE '%" + txtName.Text.Trim() + "%'";
            //}
            int Index = ComField.SelectedIndex;
            if (Index > 0)
            {
                fdDM = m_ListField[Index];
            }
            if (_ListFeatureClasses == null)
            {
                return;
            }
            if (_ListFeatureClasses.Count == 0)
            {
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("林班查询");//xisheng 日志记录 0928;
            }
            string strWKSdescrip = ModQuery.GetDescriptionOfWorkspace((_ListFeatureClasses[0] as IDataset).Workspace);

            int indexName = _ListFeatureClasses[0].Fields.FindField(fdDM);
            if (indexName < 0)
            {
                MessageBox.Show("找不到名称字段，请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            esriFieldType typeName = _ListFeatureClasses[0].Fields.get_Field(indexName).Type;

            string strLike = "";
            switch (strWKSdescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            string strSqlWhere2 = "", strSqlWhere3 = "";
            if (txtBoxName.Text.Trim().Equals(""))
            {
                if (this.rdbAccurate.Checked)
                {
                    MessageBox.Show("精确查询,需输入小班号!","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation );
                    return;
                }
                if (MessageBox.Show("该操作将会查询全库数据，查询速度较慢，是否继续？","提示",MessageBoxButtons .YesNo ,MessageBoxIcon.Asterisk) != DialogResult.Yes) return;
                strSqlWhere = "";
                strSqlWhere2 = "";
                strSqlWhere3 = "1=0";
            }
            else
            {
                if (this.rdbAccurate.Checked)
                {
                    if (m_SelectXZQ != "")
                    {
                        string tempString = GetWhereCase(m_SelectXZQ);
                        strSqlWhere = fdDM + " = '" + txtBoxName.Text.Trim() + "' and " + tempString;
                    }
                    else
                    {
                        strSqlWhere = fdDM + " = '" + txtBoxName.Text.Trim() + "'";
                    }
                    if (typeName.Equals(esriFieldType.esriFieldTypeString))
                    {
                        strSqlWhere2 = fdDM + " = '" + txtBoxName.Text.Trim() + "'";
                    }
                    else
                    {
                        strSqlWhere2 = fdDM + " = " + txtBoxName.Text.Trim();
                    }

                    strSqlWhere3 = "1=0";
                }
                else
                {
                    if (m_SelectXZQ != "")
                    {
                        string temp = GetWhereCase(m_SelectXZQ);
                        strSqlWhere = fdDM + " LIKE '" + strLike + txtBoxName.Text.Trim() + strLike + "' and " + temp;
                    }
                    else
                    {
                        strSqlWhere = fdDM + " LIKE '" + strLike + txtBoxName.Text.Trim() + strLike + "'";
                    }
                    //strSqlWhere = " contains(" + fdDM + ",'" + txtBoxName.Text.Trim() + "')>0";
                    if (typeName.Equals(esriFieldType.esriFieldTypeString))
                    {
                        strSqlWhere2 = fdDM + " = '" + txtBoxName.Text.Trim() + "'";
                        strSqlWhere3 = fdDM + "<> '" + txtBoxName.Text.Trim() + "'";
                    }
                    else 
                    {
                        strSqlWhere2 = fdDM + " = " + txtBoxName.Text.Trim();
                        strSqlWhere3 = fdDM + "<> " + txtBoxName.Text.Trim();
                    }
                }                
            }
            this.Hide();

            if (_QueryFilterAll == null) _QueryFilterAll = new QueryFilterClass();
            if (_QueryFilterPart1 == null) _QueryFilterPart1 = new QueryFilterClass();
            if (_QueryFilterPart2 == null) _QueryFilterPart2 = new QueryFilterClass();
            //deleted by chulili 20110802 查询道路地物类,不再查询自然地名注记,就不必增加查询条件
            ////added by chulili 20110801 
            //if (RoadWhere.Equals(""))
            //{
            //    RoadWhere = "CLASS ='DI' OR CLASS='DG'";
            //}
            //else
            //{
            //    RoadWhere = RoadWhere + " AND (CLASS ='DI' OR CLASS='DG')";
            //}

            ////end added by chulili
            _QueryFilterAll.WhereClause = strSqlWhere;
            _QueryFilterPart1.WhereClause = strSqlWhere2;
            _QueryFilterPart2.WhereClause = strSqlWhere3;
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuerytoTable(_MapControl);
            }
            //用查询地物类,查询条件填充查询窗体
            //if (this.rdbAccurate.Checked)
            //{
            //    m_frmQuery.FillData(_QueryFeaClass_XZ, _QueryFilterPart1, true);
            //}
            //else
            //{
            //    m_frmQuery.FillData(_QueryFeaClass_XZ, _QueryFilterAll, _QueryFilterPart1, _QueryFilterPart2, true);
            //    //m_frmQuery.FillData(_QueryFeaClass_ZR, pQF, pQF2, pQF3, true);
            //}

            SysCommon.CProgress vProgress = new SysCommon.CProgress("林班查询");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("进行查询");
            try
            {
               // m_frmQuery.FillData(ListLayerName, ListFeatureClass, _QueryFilterAll, _QueryFilterPart1, _QueryFilterPart2, true);
               // QueryBar.EmergeQueryData(_ListNodeKeys, _ListLayers, _ListLayerNames, _ListFeatureClasses, _QueryFilterAll, _QueryFilterPart1, _QueryFilterPart2, true);
                QueryBar.EmergeQueryData(_ListLayers[0] as IFeatureLayer, _QueryFilterAll, vProgress);
                QueryBar.m_pMapControl = _MapControl;
                try
                {
                    DevComponents.DotNetBar.Bar pBar = QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                    if (pBar != null)
                    {
                        pBar.AutoHide = false;
                        //pBar.SelectedDockTab = 1;
                        int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                        pBar.SelectedDockTab = tmpindex;
                    }
                }
                catch
                { }
            }
            catch(Exception err2)
            {
                vProgress.Close();
            }
            vProgress.Close();
            //m_frmQuery.Show(_OwnerFrm);
           // m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
        }
        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmQuery = null;
            _QueryFilterAll = null;
            _QueryFilterPart1 = null;
            _QueryFilterPart2 = null;
            this.Dispose();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void IntialComBox(IFeatureClass pFeatureClass)
        {
            if (pFeatureClass == null)
                return;
            m_ListField = new List<string>();
            this.ComField.Items.Clear();
            this.ComField.Text = "";
          
            //循环得到每一个字段并根据字段类型进行对应的操作
            //此处以后还要更改增加更多判断和对应操作
            for (int iIndex = 0; iIndex < pFeatureClass.Fields.FieldCount; iIndex++)
            {
                IField pField = pFeatureClass.Fields.get_Field(iIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        break;
                    case esriFieldType.esriFieldTypeString:
                        ListViewItem newItem = new ListViewItem(new string[] {  SysCommon.ModField.GetChineseNameOfField(pField.Name)});
                        m_ListField.Add(pField.Name);
                        newItem.Tag = pField.Name;
                        //////处理字段名过长显示问题
                        newItem.ToolTipText = SysCommon.ModField.GetChineseNameOfField(pField.Name);
                        this.ComField.Items.Add(newItem);
                        break;
                    default:
                        break;
                }
            }
        }

        private void txtLayer_Click(object sender, EventArgs e)
        {
            Plugin.SelectLayerByTree frm = new Plugin.SelectLayerByTree(1);
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.m_NodeKey.Trim() != "")
                {
                    _ListLayers.Clear();
                    _ListFeatureClasses.Clear();
                    _ListLayerNames.Clear();
                    IMap pMap = _MapControl.Map;
                    XmlDocument pXmldoc = new XmlDocument();
                    pXmldoc.Load(frm._LayerTreePath);
                    ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, pMap, frm.m_NodeKey, pXmldoc);
                    pXmldoc = null;
                    IFeatureClass pFeatureClass = null;
                    string strLayerName = "";
                    if (pLayer != null)
                    {
                        strLayerName = pLayer.Name;
                        try
                        {
                            pFeatureClass = (pLayer as IFeatureLayer).FeatureClass;
                        }
                        catch
                        { }

                    }
                    else
                    {
                        pFeatureClass =ModQuery.GetFeatureClassByNodeKey(frm._LayerTreePath, frm.m_NodeKey,out strLayerName);
                    }
                    _ListLayers.Add(pLayer);
                    _ListLayerNames.Add(strLayerName);
                    _ListFeatureClasses.Add(pFeatureClass);
                    txtLayer.Text = strLayerName;
                }

                if (_ListFeatureClasses != null)
                {
                    if (_ListFeatureClasses.Count > 0)
                    {
                        IntialComBox(_ListFeatureClasses[0]);
                    }
                }
            }
        }

        private void txtLayer_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSelectXZQ_Click(object sender, EventArgs e)
        {
            FrmGetXZQLocation newfrm = new FrmGetXZQLocation();
            newfrm.m_IsLocation = false;
            if (newfrm.ShowDialog() != DialogResult.OK) return;
            txtXZQ.Text = newfrm.m_XZQ;
            m_SelectXZQ = newfrm.m_XZQCode;
        }
        //ygc 20130417 根据行政区代码长度合成条件字符串
        private string GetWhereCase(string xzqCode)
        {
            string newWhere = "";
            switch (xzqCode.Length)
            {
                case 2:
                    newWhere = "sheng='" + xzqCode + "'";
                    break;
                case 4:
                    newWhere = "shi='" + xzqCode + "'";
                    break;
                case 6:
                    newWhere = "xian='" + xzqCode + "'";
                    break;
                case 8:
                    newWhere = "xiang='" + xzqCode + "'";
                    break;
                case 10:
                    newWhere = "cun='" + xzqCode + "'";
                    break;
            }
            return newWhere;
        }
    }
}
