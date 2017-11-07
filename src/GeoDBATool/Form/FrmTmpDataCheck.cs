using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Data.OracleClient;
namespace GeoDBATool
{
    public partial class FrmTmpDataCheck : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.AdvTree _ProjectTree = null;
        private List<DevComponents.AdvTree.Node> _ListTmpDBList = new List<DevComponents.AdvTree.Node>();
        private IFeatureDataset _CurFeatureDataset = null;
        private IFeatureClass _CurFeatureClass = null;
        private System.Xml.XmlNode _ConnectXmlNode = null;  //DatabaseDetalProject.xml中连接信息的节点
        private List<string> _ListRegions = null;   //行政区列表
        private List<string> _ListRegionNames = null;   //行政区名列表
        private Dictionary<string, enumTmpDBCheckState> _DicCheckStates = null;
        private OracleConnection _Conn = null;
        //数据库审核状态
        private enum enumTmpDBCheckState
        {
            NULL=0,     //空值
            Yes = 1,    //全部通过审核
            No = 2,     //全部未通过审核
            Part = 3    //部分通过审核
        }
        public FrmTmpDataCheck(DevComponents.AdvTree.AdvTree pProjectTree)
        {
            InitializeComponent();
            if (pProjectTree != null)
            {
                _ProjectTree = pProjectTree;
            }
        }
        private string GetRegionName(string RegionCode)
        {
            string RegionName = RegionCode;
            SysCommon.Gis.SysGisTable pTable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception err = null;
            object objName = pTable.GetFieldValue("行政区字典表", "NAME", "CODE='"+RegionCode+"'", out err);
            pTable = null;
            if (objName != null)
            {
                RegionName = objName.ToString();
            }
            return RegionName;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTmpDataCheck_Load(object sender, EventArgs e)
        {
            //加进度条 xisheng 2011.06.28
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("初始化审核状态界面...");
            InitTmpDBList();
            vProgress.Close();
            //this.Focus();
        }
        private void InitTmpDBList()
        {
            this.lblTips.Text = "读取临时库列表...";
            Application.DoEvents();
            if (_ProjectTree != null)
            {
                _ListTmpDBList.Clear();
                for (int i = 0; i < _ProjectTree.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pRootNode = _ProjectTree.Nodes[0];
                    GetTmpDBList(pRootNode, ref _ListTmpDBList);
                }                
                if (_ListTmpDBList.Count > 0)
                {
                    for (int i = 0; i < _ListTmpDBList.Count; i++)
                    {
                        DevComponents.AdvTree.Node pNode = _ListTmpDBList[i];
                        this.cmbTmpDB.Items.Add(pNode.Text);
                    }
                }
                if (this.cmbTmpDB.Items.Count > 0)
                {
                    this.cmbTmpDB.SelectedIndex = 0;
                }
            }
        }
        private void GetTmpDBList(DevComponents.AdvTree.Node pNode, ref List<DevComponents.AdvTree.Node> ListTmpDBList)
        {
           
            for (int i = 0; i < pNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pChildNode = pNode.Nodes[i];
                if (pChildNode.DataKeyString == "DB" && pChildNode.Text == "临时库")
                {
                    for (int j = 0; j < pChildNode.Nodes.Count; j++)
                    {
                        DevComponents.AdvTree.Node pTmpnode = pChildNode.Nodes[j];
                        try
                        {
                            _ConnectXmlNode = (pTmpnode.Parent.Tag as System.Xml.XmlNode).SelectSingleNode(".//连接信息");
                        }
                        catch
                        { }
                        ListTmpDBList.Add(pTmpnode);
                    }
                }
                else 
                {
                    GetTmpDBList(pChildNode,ref ListTmpDBList);
                }
            }
        }

        private void cmbTmpDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbTmpDB.SelectedIndex;
            if (index >= 0)
            {
                try
                {
                    DevComponents.AdvTree.Node pNode = _ListTmpDBList[index];
                    if (pNode.DataKeyString == "FD")
                    {
                        Exception err = null;
                        SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(Plugin.ModuleCommon.TmpWorkSpace);
                        IFeatureDataset featureDataset = null;        //数据集
                        featureDataset = sysGisDataset.GetFeatureDataset(pNode.Text, out err);
                        sysGisDataset = null;
                        _CurFeatureDataset = featureDataset;
                        InitFeatureClassList(featureDataset);
                    }
                }
                catch
                { }
            }
        }
        private void InitFeatureClassList(IFeatureDataset pFeaDataset)
        {
            this.lblTips.Text = "读取临时库中图层列表...";
            Application.DoEvents();
            this.cmbLayer.Items.Clear();
            this.cmbLayer.Items.Add("所有图层");
            IEnumDataset pEnumDs = pFeaDataset.Subsets;
            pEnumDs.Reset();
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                IFeatureClass pFeatureClass = pDs as IFeatureClass;
                this.cmbLayer.Items.Add(pFeatureClass.AliasName);

                pDs = pEnumDs.Next();
            }
            this.cmbLayer.SelectedIndex = 0;
        }
        private string GetRegionField()
        {
            if (this.rdbSheng.Checked)
            {
                return "sheng";
            }
            if (this.rdbShi.Checked)
            {
                return "shi";
            }
            if (this.rdbXian.Checked)
            {
                return "xian";
            }
            return "xian";
        }
        private void InitContentGrid()
        {
            this.lblTips.Text = "读取审核状态...";
            Application.DoEvents();
            this.dGridCheckRes.Rows.Clear();
            if (_CurFeatureDataset == null)
            {
                return;
            }
            if (this.cmbLayer.Text=="所有图层")
            {
                _CurFeatureClass = null;
            }
            string strRegionField = GetRegionField();
            
            if (_Conn == null)
            {
                if (_ConnectXmlNode != null)
                {
                    string Server = _ConnectXmlNode.Attributes["服务器"].Value.ToString();
                    string Database = _ConnectXmlNode.Attributes["数据库"].Value.ToString();
                    string strUser = _ConnectXmlNode.Attributes["用户"].Value.ToString();
                    string strPassword = _ConnectXmlNode.Attributes["密码"].Value.ToString();
                    _Conn = ModOracle.GetOracleConnection(Server, Database, strUser, strPassword);
                }
            }
            
            if (_Conn != null)
            {
                if (_Conn.State == System.Data.ConnectionState.Closed)
                {
                    _Conn.Open();
                }
                if (_CurFeatureClass == null)
                {
                    this.dGridCheckRes.Rows.Clear();
                    progressBarX1.Visible = true;
                    progressBarX1.Maximum = this.cmbLayer.Items.Count;
                    progressBarX1.Minimum = 1;
                    progressBarX1.Value = 1;
                    progressBarX1.Step = 1;

                    IEnumDataset pEnumDs = null;
                    pEnumDs = _CurFeatureDataset.Subsets;                        
                    pEnumDs.Reset();
                    IDataset pDs = pEnumDs.Next();
                    while (pDs != null)
                    {
                        IFeatureClass pFeatureClass = pDs as IFeatureClass;
                        progressBarX1.PerformStep();
                        this.lblTips.Text = "读取'"+pFeatureClass.AliasName+"'图层审核状态...";
                        Application.DoEvents();
                        InitContentGridByFeatureClass(_Conn, pFeatureClass, strRegionField);
                        pDs = pEnumDs.Next();
                    }
                }
                else
                {
                    this.dGridCheckRes.Rows.Clear();
                    this.lblTips.Text = "读取'" + _CurFeatureClass.AliasName + "'图层审核状态...";
                    Application.DoEvents();
                    InitContentGridByFeatureClass(_Conn, _CurFeatureClass, strRegionField);
                }
                _Conn.Close();
                if (_ListRegionNames == null)
                {
                    _ListRegionNames = new List<string>();
                }
                _ListRegionNames.Clear();
                for (int i = 0; i < _ListRegions.Count; i++)
                {
                    string strRegion = _ListRegions[i];
                    string strRegionName = GetRegionName(strRegion);
                    _ListRegionNames.Add(strRegionName);
                }
                for (int i = 0; i < _ListRegions.Count; i++)
                {
                    string strRegion = _ListRegions[i];
                    enumTmpDBCheckState CurState = _DicCheckStates[strRegion];
                    string strState = "";
                    switch (CurState)
                    {
                        case enumTmpDBCheckState.Yes:
                            strState = "通过";
                            break;
                        case enumTmpDBCheckState.No:
                            strState = "未通过";
                            break;
                        case enumTmpDBCheckState.Part:
                            strState = "部分通过";
                            break;
                        case enumTmpDBCheckState.NULL:
                            strState = "无数据";
                            break;
                    }
                    this.dGridCheckRes.Rows.Add(_ListRegionNames[i], strState);
                }
                _Conn.Close();
                progressBarX1.Visible = false;
                this.lblTips.Text = "获取审核状态完毕。";
                Application.DoEvents();

            }
            else
            {
                this.lblTips.Text = "获取临时库连接失败!";
                Application.DoEvents();
            }
            
            
        }
        private void InitContentGridByFeatureClass(OracleConnection conn, IFeatureClass pFeatureclass,string strRegionField)
        {
            if (pFeatureclass == null)
            {
                return;
            }
            string strTableName = (pFeatureclass as IDataset).Name;

            enumTmpDBCheckState state = enumTmpDBCheckState.NULL;
            if (_ListRegions == null)
            {
                _ListRegions = new List<string>();
            }
            if (_DicCheckStates == null)
            {
                _DicCheckStates = new Dictionary<string, enumTmpDBCheckState>();
            }
            OracleDataReader pReader = ModOracle.GetReader(conn, "select distinct " + strRegionField + " from " + strTableName);
            if (pReader != null)
            {
                while (pReader.Read())
                {
                    if (pReader.GetValue(0) != null)
                    {
                        string strRegion = pReader.GetValue(0).ToString();
                        if (!_ListRegions.Contains(strRegion))
                        {
                            _ListRegions.Add(strRegion);
                        }
                    }
                }
                pReader.Close();
            }
            for (int i = 0; i < _ListRegions.Count; i++)
            {
                string CurRegion = _ListRegions[i];
                enumTmpDBCheckState CurState = enumTmpDBCheckState.NULL;
                if (_DicCheckStates.ContainsKey(CurRegion))
                {
                    CurState = _DicCheckStates[CurRegion];
                }
                else
                {
                    _DicCheckStates.Add(CurRegion, CurState);
                }
                if (CurState == enumTmpDBCheckState.Part)
                {
                    continue;
                }
                pReader = ModOracle.GetReader(conn, "select distinct CheckState from " + strTableName+" where "+strRegionField+"='"+CurRegion+"'");
                if (pReader != null)
                {
                    while (pReader.Read())
                    {
                        if (CurState == enumTmpDBCheckState.Part)
                        {
                            break;
                        }
                        object objValue = pReader.GetValue(0);
                        if (objValue == null)
                        {
                            if(CurRegion=="140211")
                            {
                                int tmp=0;
                            }
                            CurState = ModifyState(CurState, enumTmpDBCheckState.No);
                        }
                        else
                        {
                            if (objValue.ToString() == "Y")
                            {
                                CurState = ModifyState(CurState, enumTmpDBCheckState.Yes);
                            }
                            else
                            {
                                if(CurRegion=="140211")
                                {
                                    int tmp=0;
                                }
                                CurState = ModifyState(CurState, enumTmpDBCheckState.No);
                            }
                        }
                    }
                    pReader.Close();
                    _DicCheckStates[CurRegion] = CurState;
                }
            }
        }
        private enumTmpDBCheckState ModifyState(enumTmpDBCheckState state, enumTmpDBCheckState CurState)
        {
            enumTmpDBCheckState resState = state;
            if (state == enumTmpDBCheckState.NULL)
            {
                resState = CurState;
            }
            else
            {
                if (state == enumTmpDBCheckState.Yes)
                {
                    if (CurState == enumTmpDBCheckState.No)
                    {
                        resState = enumTmpDBCheckState.Part;
                    }
                }
                else if (state == enumTmpDBCheckState.No)
                {
                    if (CurState == enumTmpDBCheckState.Yes) 
                    {
                        resState = enumTmpDBCheckState.Part;
                    }
 
                }
                else if (state == enumTmpDBCheckState.Part)
                {
                    return state;
                }
            }
            return resState;
        }
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblTips.Text = "";
            Application.DoEvents();
            if (this.cmbLayer.Text == "所有图层")
            {
                _CurFeatureClass = null;
            }
            else
            {
                string strCurName = this.cmbLayer.Text;
                IEnumDataset pEnumDs = _CurFeatureDataset.Subsets;
                pEnumDs.Reset();
                IDataset pDs = pEnumDs.Next();
                while (pDs != null)
                {
                    IFeatureClass pFeatureClass = pDs as IFeatureClass;
                    if (pFeatureClass.AliasName == strCurName)
                    {
                        _CurFeatureClass = pFeatureClass;
                        break;
                    }
                    pDs = pEnumDs.Next();
                }
            }
            InitContentGrid();
        }
        private void rdbRegion_CheckedChanged(object sender, EventArgs e)
        {
            this.lblTips.Text = "";
            Application.DoEvents();
            if (_ListRegions != null)
            {
                _ListRegions.Clear();
            }
            if (_DicCheckStates != null)
            {
                _DicCheckStates.Clear();
            }
            InitContentGrid();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            this.lblTips.Text = "";
            Application.DoEvents();
            if (_Conn == null)
            {
                this.lblTips.Text = "获取临时库连接失败!";
                Application.DoEvents();
            }
            if (_Conn != null)
            {
                if (_Conn.State == System.Data.ConnectionState.Closed)
                {
                    _Conn.Open();
                }
            }
            for (int i = 0; i < this.dGridCheckRes.Rows.Count; i++)
            {
                DataGridViewRow pRow = dGridCheckRes.Rows[i];
                if (!pRow.Selected)
                {
                    continue;
                }
                string strRegion = pRow.Cells[0].Value.ToString();
                string strState = pRow.Cells[1].Value.ToString();
                bool bRes = true;
                if (strState != "通过")
                {
                    string RegionCode = _ListRegions[i];
                    string RegionField = GetRegionField();
                    if (_CurFeatureClass == null)
                    {
                        progressBarX1.Visible = true;
                        progressBarX1.Maximum = this.cmbLayer.Items.Count;
                        progressBarX1.Minimum = 1;
                        progressBarX1.Value = 1;
                        progressBarX1.Step = 1;

                        IEnumDataset pEnumDs = null;
                        pEnumDs = _CurFeatureDataset.Subsets;
                        pEnumDs.Reset();
                        IDataset pDs = pEnumDs.Next();
                        while (pDs != null)
                        {
                            IFeatureClass pFeatureClass = pDs as IFeatureClass;
                            progressBarX1.PerformStep();
                            this.lblTips.Text = "改变'"+strRegion+"'的'" + pFeatureClass.AliasName + "'图层审核状态...";
                            Application.DoEvents();
                            bool bTmpRes = MakeChecked(_Conn, pFeatureClass, RegionField, RegionCode);
                            if (!bTmpRes)
                            {
                                bRes = false;
                            }
                            pDs = pEnumDs.Next();
                        }
                        progressBarX1.Visible = false;
                        if (bRes)
                        {
                            pRow.Cells[1].Value = "通过";
                        }
                    }
                    else
                    {
                        bRes = MakeChecked(_Conn, _CurFeatureClass, RegionField, RegionCode);
                        if (bRes)
                        {
                            pRow.Cells[1].Value = "通过";
                        }
                    }

                }
            }
            this.lblTips.Text = "改变审核状态完毕。";
            Application.DoEvents();
            _Conn.Close();
        }
        private bool MakeChecked(OracleConnection conn, IFeatureClass pFeatureclass, string strRegionField,string CurRegion)
        {
            if (pFeatureclass == null)
            {
                return false;
            }
            if (conn == null)
            {
                return false;
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                string strTableName = (pFeatureclass as IDataset).Name;
                if (pFeatureclass.Fields.FindField(strRegionField) >= 0)
                {
                    OracleCommand pCommand = conn.CreateCommand();
                    
                    pCommand.CommandText = "update " + strTableName + " set CheckState='Y' where " + strRegionField + "='" + CurRegion + "'";
                    pCommand.ExecuteNonQuery();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void FrmTmpDataCheck_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_Conn != null)
            {
                if (_Conn.State == ConnectionState.Open)
                {
                    _Conn.Close();                
                }
                _Conn = null;
            }
            if (_ListRegions != null)
            {
                _ListRegions.Clear();
                _ListRegions = null;
            }
            if (_ListRegionNames != null)
            {
                _ListRegionNames.Clear();
                _ListRegionNames = null;
            }
            if (_DicCheckStates != null)
            {
                _DicCheckStates.Clear();
                _DicCheckStates = null;
            }
            if (_ListTmpDBList != null)
            {
                _ListTmpDBList.Clear();
                _ListTmpDBList = null;
            }
            if (_ConnectXmlNode != null)
            {
                _ConnectXmlNode = null;
            }

        }
    }
}
