using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
namespace GeoSysSetting
{
    public partial class FrmStatisticsConfig : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace _TmpWorkSpace=null;//业务库工作空间
        private string _LayerTreePath = Application.StartupPath + "\\..\\res\\xml\\查询图层树.xml";     //图层目录文件路径
        private string _QueryConfigPath = Application.StartupPath + "\\..\\Template\\StatisticsConfig.xml";
        private XmlDocument _LayerTreeXmldoc = null;
        private Dictionary<string, string> _DicLayerList = null;
        public FrmStatisticsConfig(IWorkspace pTmpWorkSpace)
        {
            _TmpWorkSpace = pTmpWorkSpace;
            if (pTmpWorkSpace != null)
            {
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(pTmpWorkSpace, _LayerTreePath);
                SysGisTable mSystable = new SysCommon.Gis.SysGisTable(pTmpWorkSpace);
                Exception err = null;
                Dictionary<string, object> pDic = mSystable.GetRow("SYSSETTING", "SETTINGNAME='统计配置'", out err);
                if (pDic != null)
                {
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                        {
                            object tempObj = pDic["SETTINGVALUE2"];
                            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = tempObj as IMemoryBlobStreamVariant;
                            IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
                            if (pMemoryBlobStream != null)
                            {
                                try
                                {
                                    pMemoryBlobStream.SaveToFile(_QueryConfigPath);
                                }
                                catch { return; }
                            }
                        }
                    }
                }
            }
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Exception exError = null;
            ITransactions pTransactions = null;
            //保存查询配置文件（由本地向数据库保存）
            try
            {
                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();

                pBlobStream.LoadFromFile(_QueryConfigPath );
                //启动事务
                pTransactions = (ITransactions)_TmpWorkSpace;
                if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                SysGisTable sysTable = new SysGisTable(_TmpWorkSpace);
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData.Add("SETTINGVALUE2", pBlobStream);
                dicData.Add("SETTINGNAME", "统计配置");
                //判断是更新还是添加
                //不存在则添加，已存在则更新
                if (!sysTable.ExistData("SYSSETTING", "SETTINGNAME='统计配置'"))
                {
                    if (!sysTable.NewRow("SYSSETTING", dicData, out exError))
                    {
                        MessageBox.Show(exError.Message);
                        return;
                    }
                }
                else
                {
                    if (!sysTable.UpdateRow("SYSSETTING", "SETTINGNAME='统计配置'", dicData, out exError))
                    {
                        MessageBox.Show(exError.Message);
                        return;
                    }
                }
                //提交事务
                if (pTransactions.InTransaction) pTransactions.CommitTransaction();
            }
            catch (Exception ex)
            {
                //出错则放弃提交
                if (pTransactions.InTransaction) pTransactions.AbortTransaction();
                MessageBox.Show(exError.Message);
                return;
            }
            this.DialogResult  = DialogResult.OK;
        }

        private void FrmQueryConfig_Load(object sender, EventArgs e)
        {
            
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            if (_DicLayerList == null)
            {
                _DicLayerList=new Dictionary<string,string >();
            }
            //初始化图层树列表
            if (File.Exists(_LayerTreePath))
            {
                if (_LayerTreeXmldoc == null)
                {
                    _LayerTreeXmldoc = new XmlDocument();
                }
                _LayerTreeXmldoc.Load(_LayerTreePath);
                advTreeLayerList.Nodes.Clear();

                //获取Xml的根节点并作为根节点加到UltraTree上
                XmlNode xmlnodeRoot = _LayerTreeXmldoc.DocumentElement;
                XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

                xmlelementRoot.SetAttribute("NodeKey", "Root");
                string sNodeText = xmlelementRoot.GetAttribute("NodeText");

                //创建并设定树的根节点
                DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
                treenodeRoot.Name = "Root";
                treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                this.advTreeLayerList.Nodes.Add(treenodeRoot);

                treenodeRoot.Image =this.ImageList.Images["Root"];
                InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);


            }            
            //根据查询配置文件初始化界面

            //初始化查询类型下拉框
            if (File.Exists(_QueryConfigPath))
            {
                XmlDocument pQueryXmldoc = new XmlDocument();
                try
                {
                    pQueryXmldoc.Load(_QueryConfigPath);
                }
                catch { }
                //查询节点
                XmlNodeList pQueryItems = pQueryXmldoc.SelectNodes("//StatisticsConfig/StatisticsItem");
                for (int i = 0; i < pQueryItems.Count; i++)
                {
                    XmlNode pNode = pQueryItems[i];
                    XmlElement pEle = pNode as XmlElement;
                    if (pEle.HasAttribute("ItemText"))
                    {
                        string strQueryType = pEle.GetAttribute("ItemText").ToString();
                        this.comboBoxQueryType.Items.Add(strQueryType);
                    }
                }
                if (this.comboBoxQueryType.Items.Count > 0)
                {
                    this.comboBoxQueryType.SelectedIndex = 0;
                }
                pQueryXmldoc = null;

            }
            //根据查询配置文件初始化界面 end
            //初始化当前查询类型

        }
        //根据配置文件显示图层树
        private void InitLayerTreeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode)
        {

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
                XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
                if (xmlElementChild == null)
                {
                    continue;
                }
                else if (xmlElementChild.Name == "ConfigInfo")
                {
                    continue;
                }
                //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                string sNodeKey = xmlElementChild.GetAttribute("NodeKey");

                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;

                treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;

                treenode.Nodes.Add(treenodeChild);

                //递归
                if(xmlElementChild.Name!="Layer")
                {
                    InitLayerTreeByXmlNode(treenodeChild, xmlElementChild as XmlNode);
                }

                InitializeNodeImage(treenodeChild);
            }

        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        private void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image = this.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = this.ImageList.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = this.ImageList.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = this.ImageList.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = this.ImageList.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = this.ImageList.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = this.ImageList.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = this.ImageList.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = this.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = this.ImageList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = this.ImageList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = this.ImageList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = this.ImageList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = this.ImageList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = this.ImageList.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = this.ImageList.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = this.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = this.ImageList.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = this.ImageList.Images["RD"];
                    break;
                case "SubType":
                    treenode.Image = this.ImageList.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }

        private void buttonAddLayer_Click(object sender, EventArgs e)
        {
            //找到选中的图层或更高级别节点
            DevComponents.AdvTree.Node pSelNode = advTreeLayerList.SelectedNode;
            if (pSelNode == null) return;

            AddNodeToListLayer(pSelNode);
            SaveLayerList();
            RefreshBtn();
            
        }
        /// <summary>
        /// 刷新按钮可用性
        /// </summary>
        private void RefreshBtn()
        {
            int icount = listViewQueryLayer.Items.Count;
            switch(comboBoxQueryType.Text.Trim())
            {
                case"林业用地分布":
                    //if (icount >= 2)
                    //{
                    //    buttonAddLayer.Enabled = false;
                    //    buttonDelAll.Enabled = true;
                    //    buttonDelLayer.Enabled = true;
                    //}
                    //else
                    //{
                    //    buttonAddLayer.Enabled = true;
                    //}
               
                    //break;
                case "林地保护等级分布":
                case "主导功能区分布":
                case "林地质量等级分布":
                case "林地规划统计":
                case "林地结构统计":
                case "林地利用分布统计":
                case "林场用地分布统计":
                    if (icount >= 1)
                    {
                        buttonAddLayer.Enabled = false;
                        buttonDelAll.Enabled = true;
                        buttonDelLayer.Enabled = true;
                    }
                    else
                    {
                        buttonAddLayer.Enabled = true;
                    }
                    break;
            }
            if (icount == 0)
            {
                buttonDelAll.Enabled = false;
                buttonDelLayer.Enabled = false;
            }
        }

        //将图层列表保存到配置文件中
        private void SaveLayerList()
        {
            if (File.Exists(_QueryConfigPath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_QueryConfigPath);
                //找到当前查询类型节点
                XmlNode pNode = pXmldoc.SelectSingleNode("//StatisticsConfig/StatisticsItem[@ItemText='" + this.comboBoxQueryType.Text + "']");
                if (pNode != null)
                {
                    XmlNodeList pLayerList = pNode.SelectNodes(".//LayerItem");
                    //把旧的图层列表全部删掉（可能修改旧的会比较好，但难以实现）
                    for (int i = pLayerList.Count - 1;i>=0 ; i--)
                    {
                        XmlNode pTmpNode = pLayerList[i];
                        pNode.RemoveChild(pTmpNode);

                    }
                    //将新的图层列表添加进去
                    for (int j = 0; j < this.listViewQueryLayer.Items.Count; j++)
                    {
                        XmlElement pLayerEle = pXmldoc.CreateElement("LayerItem");
                        ListViewItem pItem = listViewQueryLayer.Items[j];
                        pLayerEle.SetAttribute("NodeText", pItem.Text);
                        pLayerEle.SetAttribute("NodeKey", pItem.Tag.ToString());
                        pNode.AppendChild(pLayerEle as XmlNode);
                    }

                }
                pXmldoc.Save(_QueryConfigPath );
                pXmldoc = null;
            }
        }
        //将选中节点包含的图层节点加入到图层列表中
        private void AddNodeToListLayer(DevComponents.AdvTree.Node pNode)
        {
            if (pNode == null) return;
            switch (pNode.Tag.ToString())
            {
                case "DIR":
                case "DataDIR":
                case "Root":
                    if (pNode.Nodes.Count > 0)
                    {
                        for (int i = 0; i < pNode.Nodes.Count; i++)
                        {
                            DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                            AddNodeToListLayer(pTmpNode);
                        }
                    }
                    break;
                case "Layer":
                    if (!_DicLayerList.ContainsKey(pNode.Name))
                    {
                        ListViewItem pItem = new ListViewItem();
                        pItem.Text = pNode.Text;
                        pItem.Tag = pNode.Name;
                        this.listViewQueryLayer.Items.Add(pItem);
                        _DicLayerList.Add(pNode.Name,pNode.Text );
                    }
                    break;
            }
        }
        private void buttonDelLayer_Click(object sender, EventArgs e)
        {
            //找到选中的图层
            if (listViewQueryLayer.SelectedItems.Count > 0)
            {
                for (int i = listViewQueryLayer.SelectedItems.Count - 1; i >= 0; i--)
                {
                    ListViewItem pItem = listViewQueryLayer.SelectedItems[i];
                    _DicLayerList.Remove(pItem.Tag.ToString());
                    listViewQueryLayer.Items.Remove(pItem);
                    
                }
                SaveLayerList();
            }
            RefreshBtn();
            
        }

        private void buttonDelAll_Click(object sender, EventArgs e)
        {
            this.listViewQueryLayer.Items.Clear();
            _DicLayerList.Clear();
            SaveLayerList();
            RefreshBtn();
        }
        //点击图层列表中某个图层，属性下拉框重新初始化
        private void listViewQueryLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewQueryLayer.SelectedItems.Count > 0)
            {
                ListViewItem pItem = listViewQueryLayer.SelectedItems[0];
                string strNodeKey = pItem.Tag.ToString();

                IFeatureClass pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(_TmpWorkSpace,_LayerTreePath, strNodeKey);
                string strFieldName1=comboBoxField1.Text;
                string strFieldName2=comboBoxField2.Text;
                comboBoxField1.Items.Clear();
                comboBoxField2.Items.Clear();
                if (pFeatureClass == null)
                {
                    return;
                }
                for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                {
                    string strFieldname = pFeatureClass.Fields.get_Field(i).Name;
                    string strChineseName = SysCommon.ModField.GetChineseNameOfField(strFieldname);
                    comboBoxField1.Items.Add(strFieldname + "【" + strChineseName + "】");
                    if (comboBoxField2.Visible)
                    {
                        comboBoxField2.Items.Add(strFieldname+"【"+ strChineseName+"】");
                    }

                }

                for (int j = 0; j < comboBoxField1.Items.Count; j++)
                {
                    string strTmp = comboBoxField1.Items[j].ToString();
                    if (strTmp.Contains(strFieldName1))
                    {
                        comboBoxField1.SelectedIndex = j;
                        break;
                    }
                }
                if(comboBoxField2.Visible)
                {
                    for (int j = 0; j < comboBoxField2.Items.Count; j++)
                    {
                        string strTmp = comboBoxField2.Items[j].ToString();
                        if (strTmp.Contains(strFieldName2))
                        {
                            comboBoxField2.SelectedIndex = j;
                            break;
                        }
                    }
                }
            }
        }

        private void comboBoxQueryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当前查询类型
            string strQueryType = this.comboBoxQueryType.Text;
            this.listViewQueryLayer.Items.Clear();
            _DicLayerList.Clear();
            this.comboBoxField1.Items.Clear();
            comboBoxField1.Text = "";
            this.comboBoxField2.Items.Clear();
            comboBoxField2.Text = "";
            if (File.Exists(_QueryConfigPath))
            {
                //打开查询配置文件
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_QueryConfigPath );

                XmlNode pXmlnode = pXmldoc.SelectSingleNode("//StatisticsConfig/StatisticsItem[@ItemText='" + strQueryType + "']");
                if (pXmlnode != null)
                {
                    XmlNodeList pLayerList = pXmlnode.SelectNodes(".//LayerItem");
                    //初始化该查询类型的图层列表
                    if (pLayerList != null)
                    {
                        for (int i = 0; i < pLayerList.Count; i++)
                        {
                            XmlNode pLayerNode = pLayerList[i];
                            XmlElement pLayerEle = pLayerNode as XmlElement;
                            if (pLayerEle != null)
                            {
                                if (pLayerEle.HasAttribute("NodeKey"))
                                {
                                    string strNodeKey = pLayerNode.Attributes["NodeKey"].Value.ToString();  //图层Key
                                    string strText = pLayerNode.Attributes["NodeText"].Value.ToString();    //图层中文名
                                    //判断列表中是否已经有该图层
                                    if (!_DicLayerList.ContainsKey("strNodeKey"))
                                    {
                                        ListViewItem pItem = new ListViewItem();
                                        pItem.Text = strText;
                                        pItem.Tag = strNodeKey;

                                        this.listViewQueryLayer.Items.Add(pItem);
                                        _DicLayerList.Add(strNodeKey, strText);

                                    }
                                    
                                }
                            }
                        }
                    }
                    //初始化该查询类型的字段
                    XmlNodeList pFieldList = pXmlnode.SelectNodes(".//FieldItem");

                    if (pFieldList != null)
                    {
                        if (pFieldList.Count > 0)
                        {
                            XmlNode pFieldNode = pFieldList[0];
                            XmlElement pFieldEle = pFieldNode as XmlElement;
                            if(pFieldEle.HasAttribute("LabelText"))
                            {
                                labelField1.Text = pFieldEle.GetAttribute("LabelText").ToString();
                                string strFieldName1 = pFieldEle.GetAttribute("FieldName").ToString();
                                string strChineseName1 = SysCommon.ModField.GetChineseNameOfField(strFieldName1);
                                this.comboBoxField1.Text = strFieldName1 + "【" + strChineseName1 + "】";

                            }
                            if (pFieldList.Count > 1)
                            {
                                pFieldNode = pFieldList[1];
                                pFieldEle = pFieldNode as XmlElement;
                                if (pFieldEle.HasAttribute("LabelText"))
                                {
                                    labelField2.Text = pFieldEle.GetAttribute("LabelText").ToString();
                                    string strFieldName2 = pFieldEle.GetAttribute("FieldName").ToString();
                                    string strChineseName2 = SysCommon.ModField.GetChineseNameOfField(strFieldName2);
                                    this.comboBoxField2.Text = strFieldName2 + "【" + strChineseName2 + "】";
                                }
                            }
                        }
                        if (pFieldList.Count > 1)
                        {
                            labelField2.Visible = true;
                            comboBoxField2.Visible = true;
                        }
                        else
                        {
                            labelField2.Visible = false;
                            comboBoxField2.Visible = false;
                        }
                    }
                }
                pXmldoc = null;
            }
            RefreshBtn();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult  = DialogResult.Cancel;
        }

        private void comboBoxField1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists(_QueryConfigPath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_QueryConfigPath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//StatisticsConfig/StatisticsItem[@ItemText='" + this.comboBoxQueryType.Text + "']");
                if (pNode != null)
                {
                    XmlNode pFieldnode = pNode.SelectSingleNode(".//FieldItem[@LabelText='"+this.labelField1.Text+"']");
                    XmlElement pFieldEle = pFieldnode as XmlElement;
                    string strFieldname=this.comboBoxField1.Text;
                    if(strFieldname.Contains("【"))
                    {
                        strFieldname=strFieldname.Substring(0,strFieldname.IndexOf("【"));
                    }
                    if (pFieldEle != null)
                    {
                        pFieldEle.SetAttribute("FieldName", strFieldname);
                    }                 
                }
                pXmldoc.Save(_QueryConfigPath);
            }
        }

        private void comboBoxField2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists(_QueryConfigPath))
            {
                XmlDocument pXmldoc = new XmlDocument();
                pXmldoc.Load(_QueryConfigPath);

                XmlNode pNode = pXmldoc.SelectSingleNode("//StatisticsConfig/StatisticsItem[@ItemText='" + this.comboBoxQueryType.Text + "']");
                if (pNode != null)
                {
                    XmlNode pFieldnode = pNode.SelectSingleNode(".//FieldItem[@LabelText='" + this.labelField2.Text + "']");
                    XmlElement pFieldEle = pFieldnode as XmlElement;
                    string strFieldname = this.comboBoxField2.Text;
                    if (strFieldname.Contains("【"))
                    {
                        strFieldname = strFieldname.Substring(0, strFieldname.IndexOf("【"));
                    }
                    if (pFieldEle != null)
                    {
                        pFieldEle.SetAttribute("FieldName", strFieldname);
                    }
                }
                pXmldoc.Save(_QueryConfigPath);
            }
        }

    }
}

        