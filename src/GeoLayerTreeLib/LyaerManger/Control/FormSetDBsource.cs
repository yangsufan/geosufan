using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormSetDBsource : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace _tmpWorkspace = null;        //工作库连接
        private List<IDataset> _ListDataset = new List<IDataset>();//added by chulili 20110928 数据源要具体到地物类集合
        private List<string> _ListTypeOfDataset = new List<string>();
        private DevComponents.AdvTree.Node _node = null;    //用户所要更改的节点
        private IWorkspace _dataWorkspace = null;   //用户选中的空间数据库连接
        private string _xmlpath = "";
        private UcDataLib _pUC = null;//added by chulili 20110909 添加控件变量，用来调用卸载节点的函数，节点修改了，老节点对应的图层要从视图中卸载掉
        public FormSetDBsource()
        {
            InitializeComponent();
        }
        public FormSetDBsource(UcDataLib pUC, string xmlpath, IWorkspace pWKS, DevComponents.AdvTree.Node pnode)
        {
            InitializeComponent();
            _pUC = pUC;
            _xmlpath = xmlpath;
            _tmpWorkspace = pWKS;
            _node = pnode;
        }

        private void FormSetDBsource_Load(object sender, EventArgs e)
        {
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            Exception eError;
            //初始化空间数据源列表
            List<object> ListDatasource = sysTable.GetFieldValues("DATABASEMD", "DATABASENAME", "", out eError);
            this.comboBoxDataSource.Items.Clear();
            foreach (object datasource in ListDatasource)
            {
                this.comboBoxDataSource.Items.Add(datasource.ToString());
            }
        }

        private void comboBoxDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110613 add
            //this.comboBoxFeaClass.Items.Clear();
            //end
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            Exception eError;
            //根据用户选择的数据源，得到数据源的工作空间
            string DataSourceName = this.comboBoxDataSource.Text;
            string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
            int type = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
            string strDBPara = sysTable.GetFieldValue("DATABASEMD", "DBPARA", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
            int index6 = conninfostr.LastIndexOf("|");
            //string strdataset = conninfostr.Substring(index6 + 1);

            IWorkspace pWorkspace = ModuleMap.GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null) return;
            _ListDataset.Clear();
            _ListTypeOfDataset.Clear();
            string strDatasets = conninfostr.Substring(index6 + 1);
            string[] strTemp = strDatasets.Split(new char[] { ',' });
            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
            if (pFeaWorkSpace != null)
            {
                for (int k = 0; k < strTemp.Length; k++)
                {
                    IDataset pTmpdataset = null;
                    if (strDBPara.Contains("栅格数据集"))
                    {
                        IRasterWorkspaceEx rasterWorkspace = (IRasterWorkspaceEx)pFeaWorkSpace;
                        IRasterDataset pRDataset = rasterWorkspace.OpenRasterDataset(strTemp[k]);
                        pTmpdataset = pRDataset as IDataset;
                        if (pTmpdataset != null)
                        {
                            _ListTypeOfDataset.Add("RD");
                        }
                    }
                    else if (strDBPara.Contains("栅格编目"))
                    {
                        IRasterWorkspaceEx rasterWorkspace = (IRasterWorkspaceEx)pFeaWorkSpace;
                        IRasterCatalog pRCatalog = rasterWorkspace.OpenRasterCatalog(strTemp[k]);
                        pTmpdataset = pRCatalog as IDataset;
                        if (pTmpdataset != null)
                        {
                            _ListTypeOfDataset.Add("RC");
                        }
                    }
                    else
                    {
                        pTmpdataset = pFeaWorkSpace.OpenFeatureDataset(strTemp[k]) as IDataset;
                        if (pTmpdataset != null)
                        {
                            _ListTypeOfDataset.Add("FC");
                        }
                    }
                    if (pTmpdataset != null)
                    {
                        _ListDataset.Add(pTmpdataset);
                    }
                }
            }
            _dataWorkspace = pWorkspace;
            
            if (pWorkspace == null)
                return;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            if (_pUC != null)
            {
                _pUC.RemoveNodeFromMap(_node);
            }
            //记录默认的数据源，以备下次设置时默认显示
            ModuleMap._DefaultDBsource = this.comboBoxDataSource.Text;
            //打开xml文档
            XmlDocument layerxmldoc = new XmlDocument();
            layerxmldoc.Load(_xmlpath);
            //查找xml中的对应节点
            string strTag = _node.Tag.ToString();
            string NodeName = strTag;
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            string strSearch = "//" + NodeName + "[@NodeKey='" + _node.Name + "']";
            XmlNode pxmlnode = layerxmldoc.SelectSingleNode(strSearch);
            
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            //end
            object objDbsource = sysTable.GetFieldValue("DATABASEMD", "ID", "DATABASENAME='" + this.comboBoxDataSource.Text + "'", out eError);

            string strDBsource = "";

            if (objDbsource != null)
                strDBsource = objDbsource.ToString();
            SetDbsourceOfXmlnode(_node, layerxmldoc, strDBsource, _dataWorkspace, _ListDataset,_ListTypeOfDataset );
            layerxmldoc.Save(_xmlpath);
            if (_pUC != null)
            {
                _pUC.AddNodeToMap(_node);
            }
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }
        //为xml节点赋数据源，递归调用
        private bool SetDbsourceOfXmlnode(DevComponents.AdvTree.Node pNode,XmlDocument pXmldoc, string strDbsource,IWorkspace pWorkspace,List<IDataset > pListDataset,List<string> pListTypeOfDataset)
        {
            //判断参数是否有效
            if (pNode == null)
                return false;
            if (strDbsource.Equals(""))
                return false;
            if (pWorkspace == null)
                return false;
            string strTag = pNode.Tag.ToString();
            string strNodeName = strTag;
            if (strNodeName.Contains("DataDIR"))
            {
                strNodeName = "DataDIR";
            }
            switch (strNodeName)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    if (pNode.Nodes.Count > 0)
                    {
                        for (int i = 0; i < pNode.Nodes.Count; i++)
                        {
                            DevComponents.AdvTree.Node pTmpNode=pNode.Nodes[i];
                            SetDbsourceOfXmlnode(pTmpNode, pXmldoc, strDbsource, pWorkspace, pListDataset,pListTypeOfDataset );
                        }
                        //string strNodeKey = pNode.Name;
                        //XmlNode pNewXmlNode = pXmldoc.SelectSingleNode("//" + strNodeName + "[@NodeKey='" + strNodeKey + "']");
                        //if (pNewXmlNode != null)
                        //{
                        //    pNode.DataKey = pNewXmlNode as object;
                        //}
                    }
                    break;
                case "Layer":
                    try
                    {
                        bool tag = false;
                        string strSearch = "//" + strNodeName + "[@NodeKey='" + pNode.Name + "']";
                        XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
                        if (pXmlnode == null)
                        {
                            return false;
                        }
                        if (!(pXmlnode is XmlElement))
                        {
                            return false;
                        }
                        XmlElement pNodeEle = pXmlnode as XmlElement;
                        //为节点设置数据源，后续还应添加数据集（数据集根据数据源、地物类名称得到）
                        //added by chulili 20110725 从数据源获取地物类，得到地物类所在数据集
                        string strFeaClsName = pNodeEle.Attributes["Code"].Value.ToString();
                        string strDataType = pNodeEle.Attributes["DataType"].Value.ToString();
                        IFeatureWorkspace pFeaWks = pWorkspace as IFeatureWorkspace;
                        if (pFeaWks != null)
                        {
                            //IFeatureClass pFeaCls = pFeaWks.OpenFeatureClass(strFeaClsName);
                            //if (pFeaCls != null)
                            //{
                            //    IFeatureDataset pDataSet = pFeaCls.FeatureDataset;
                            //    if (pDataSet != null)
                            //    {
                            //        pNodeEle.SetAttribute("FeatureDatasetName", pDataSet.Name);
                            //    }
                            //    else
                            //    {
                            //        pNodeEle.SetAttribute("FeatureDatasetName", "");
                            //    }
                            //}
                            //else//直接根据名称获取不到
                            //{
                                string TrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
                                
                                if (pListDataset != null)
                                {
                                    if (strDataType.Equals("FC"))
                                    {
                                        for (int i = 0; i < pListDataset.Count; i++)//循环地物类集合
                                        {
                                            if (tag)
                                            {
                                                break;  //如果已经匹配上，跳出循环
                                            }
                                            if (pListTypeOfDataset[i].Equals("FC"))
                                            {
                                                IDataset pDataset = pListDataset[i];
                                                IEnumDataset pEnumDataset = pDataset.Subsets;
                                                IDataset ptmpDataset = pEnumDataset.Next();
                                                while (ptmpDataset != null) //循环集合中的地物类
                                                {
                                                    string pFeaClsName = ptmpDataset.Name;
                                                    if (pFeaClsName.Equals(strFeaClsName))  //先对整个地物类名称进行匹配
                                                    {
                                                        pNodeEle.SetAttribute("FeatureDatasetName", pDataset.Name);
                                                        pNodeEle.SetAttribute("Code", ptmpDataset.Name);
                                                        tag = true;
                                                        break;
                                                    }
                                                    ptmpDataset = pEnumDataset.Next();
                                                }
                                            }
                                        }
                                        if (!tag)
                                        {
                                            for (int i = 0; i < pListDataset.Count; i++)//循环地物类集合
                                            {
                                                if (tag)
                                                {
                                                    break;  //如果已经匹配上，跳出循环
                                                }
                                                if (pListTypeOfDataset[i].Equals("FC"))
                                                {
                                                    IDataset pDataset = pListDataset[i];
                                                    IEnumDataset pEnumDataset = pDataset.Subsets;
                                                    IDataset ptmpDataset = pEnumDataset.Next();
                                                    while (ptmpDataset != null) //循环集合中的地物类
                                                    {
                                                        string pFeaClsName = ptmpDataset.Name;
                                                        if (pFeaClsName.Substring(pFeaClsName.IndexOf(".") + 1) == TrueFeaClsName) //再去掉用户名进行匹配
                                                        {
                                                            pNodeEle.SetAttribute("FeatureDatasetName", pDataset.Name);
                                                            pNodeEle.SetAttribute("Code", ptmpDataset.Name);
                                                            tag = true;
                                                            break;
                                                        }
                                                        ptmpDataset = pEnumDataset.Next();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (strDataType.Equals("RC") || strDataType.Equals("RD"))
                                    {
                                        for (int i = 0; i < pListDataset.Count; i++)//循环地物类集合
                                        {
                                            if (tag)
                                            {
                                                break;  //如果已经匹配上，跳出循环
                                            }
                                            if (pListTypeOfDataset[i].Equals("RC") || pListTypeOfDataset[i].Equals("RD"))
                                            {
                                                IDataset pDataset = pListDataset[i];
                                                if (pDataset != null) //循环集合中的地物类
                                                {
                                                    string RCname = pDataset.Name;
                                                    if (RCname.Equals(strFeaClsName))  //先对整个地物类名称进行匹配
                                                    {
                                                        pNodeEle.SetAttribute("FeatureDatasetName", pDataset.Name);
                                                        pNodeEle.SetAttribute("Code", pDataset.Name);
                                                        pNodeEle.SetAttribute("DataType", pListTypeOfDataset[i]);
                                                        tag = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!tag)
                                        {
                                            for (int i = 0; i < pListDataset.Count; i++)//循环地物类集合
                                            {
                                                if (tag)
                                                {
                                                    break;  //如果已经匹配上，跳出循环
                                                }
                                                if (pListTypeOfDataset[i].Equals("FC") || pListTypeOfDataset[i].Equals("RD"))
                                                {
                                                    IDataset pDataset = pListDataset[i];
                                                    if (pDataset != null) //循环集合中的地物类
                                                    {
                                                        string pFeaClsName = pDataset.Name;
                                                        if (pFeaClsName.Substring(pFeaClsName.IndexOf(".") + 1) == TrueFeaClsName) //再去掉用户名进行匹配
                                                        {
                                                            pNodeEle.SetAttribute("FeatureDatasetName", pDataset.Name);
                                                            pNodeEle.SetAttribute("Code", pDataset.Name);
                                                            pNodeEle.SetAttribute("DataType", pListTypeOfDataset[i]);
                                                            tag = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }                                    
                                }
                            //}
                        }
                        //end added by chulili
                        if (tag)
                        {
                            pNodeEle.SetAttribute("ConnectKey", strDbsource);
                        }
                        //added by chulili 20110630 非常重要，只有这样关联上，图层节点才能在视图浏览中正确显示
                        //pNode.DataKey = pXmlnode as object;
                        ModuleMap.SetDataKey(pNode, pXmlnode);
                    }
                    catch (Exception e)
                    {
                        string strinfo = e.Message;
                    }
                    break;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
