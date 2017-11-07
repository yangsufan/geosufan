using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using System.Xml;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormAddLayer : DevComponents.DotNetBar.Office2007Form
    {
        private bool _isAdd = true;
        private IWorkspace _tmpWorkspace = null;
        private DevComponents.AdvTree.Node _node=null ;
        private string _xmlpath = "";
        private IWorkspace _dataWorkspace = null;
        private ImageList _ImgList;
        private int _iDataType; //shduan 20110612 =1添加图层，=2添加数据集

        //private IDictionary<int, string> _DicFeaClass = new Dictionary<int, string>();

        private string m_strRasterType;  //shduan add 20110629 ="DEM"代表高程数据，=“DOM”代表影像数据
        public Boolean m_bIsModify = false; //shduan 20110625 用于记录是否确定
        private FontStyle newFont;
        private FontFamily newFamily = new FontFamily("宋体");
        private float newSize = 10;

        private IDictionary<int, string> _DicDataType = new Dictionary<int, string>();
        private string _DirectoryFieldname = "";
        private string _RenderFieldname = "";

        private string _MatchConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchConfig.xml";
        //当前Map中添加后又删除的图层(用于关闭对话框后，将该节点传递出去，最终为了在map中显示刚刚更改的图层)

        private IFeatureClass _CurFeatureClass = null;//用来保存当前处理的FeatureClass
        
        private DevComponents.AdvTree.Node _curNode = null;

        private UcDataLib _pUC = null;  //added by chulili 20110909 添加控件变量，用来调用卸载节点的函数，节点修改了，老节点对应的图层要从视图中卸载掉

        private IFeatureClass _OldFeatureClass = null;  //added by chulili 2012-11-09 用来保存修改前的图层连接的要素类
        public DevComponents.AdvTree.Node CurNode
        {
            get { return _curNode; }
        }
        public FormAddLayer()
        {
            InitializeComponent();
            if (ModuleMap._ListHideFields == null)
            {
                ModuleMap.GetHideFields();
            }
        }
        public FormAddLayer(UcDataLib pUC, string xmlpath,IWorkspace pWKS, DevComponents.AdvTree.Node pnode, bool isadd,ImageList pList,int iDataType)
        {
            InitializeComponent();
            _pUC = pUC;
            _xmlpath = xmlpath;
            _tmpWorkspace = pWKS;
            //cyf 20110613 add
            if (_tmpWorkspace == null) return;
            //end
            _isAdd = isadd;
            _node = pnode;
            _ImgList = pList;
            _iDataType = iDataType;
            if (ModuleMap._ListHideFields == null)
            {
                ModuleMap.GetHideFields();
            }
        }
        // *---------------------------------------------------------------------------------------
        // *开 发 者：chenyafei
        // *开发时间：20110622
        // *功能函数：根据执行的条件查询数据库中的信息
        // *参    数：表名称（支持多表，以逗号隔开）、字段名称(支持多只段，以逗号隔开)、where字句，异常(输出)
        // *返 回 值：返回查询表格的游标
        //public static ICursor GetCursor(IFeatureWorkspace pFeaWS, string tableName, string fieldName, string whereStr, out Exception outError)
        //{
        //    outError = null;
        //    ICursor pCursor = null;
        //    IQueryDef pQueryDef = pFeaWS.CreateQueryDef();
        //    pQueryDef.Tables = tableName;
        //    pQueryDef.SubFields = fieldName;
        //    pQueryDef.WhereClause = whereStr;
        //    try
        //    {
        //        pCursor = pQueryDef.Evaluate();
        //        if (pCursor == null)
        //        {
        //            outError = new Exception("查询数据库失败！");
        //            return null; ;
        //        }
        //    }
        //    catch
        //    {
        //        outError = new Exception("查询数据库失败！");
        //        return null;
        //    }
        //    return pCursor;
        //}
        private void FormAddLayer_Load(object sender, EventArgs e)
        {
            ModuleMap.GetMatchConfig(out _DirectoryFieldname, out _RenderFieldname);
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            //Dictionary<string, object> dicData = new Dictionary<string, object>();
            Exception eError;
            //初始化数据源
            List<object> ListDatasource = sysTable.GetFieldValues("DATABASEMD", "DATABASENAME", "", out eError);
            this.comboBoxDataSource.Items.Clear();
            foreach (object datasource in ListDatasource)
            {
                this.comboBoxDataSource.Items.Add(datasource.ToString());
            }
            //初始化符号化下拉框
            List<object> ListRender = sysTable.GetFieldValues("Render", _RenderFieldname , "", out eError);
            this.comboBoxRender.Items.Clear();
            this.comboBoxRender.Items.Add("自动匹配");//shduan 201106添加，用于根据FC名称自动匹配符号
            foreach (object datasource in ListRender)
            {
                this.comboBoxRender.Items.Add(datasource.ToString());
            }
            //cyf 20110613 add 
            if (comboBoxRender.Items.Count > 0)
            {
                comboBoxRender.SelectedIndex = 0;
            }
            //end
            //初始化字体 shduan 20110623 *************************************************
            //获取字体名称
            System.Drawing.Text.InstalledFontCollection FontsCol = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in FontsCol.Families)
            {
                CmbFontName.Items.Add(family.Name);
            }
            //获取字体大小
            for (int i = 0; i < 11; i++)
            {
                CmbFontSize.Items.Add(i + 4);
            }
            for (int i = 0; i < 9; i++)
            {
                CmbFontSize.Items.Add(12 + 2 * i);
            }
            for (int i = 0; i < 9; i++)
            {
                CmbFontSize.Items.Add(32 + 4 * i);
            }

            IFeatureClass ptmpFeaClass = null;

            if (_isAdd == true)//若添加图层，将数据源一栏设置成上次选择的数据源
            {
                this.btnAddLayer.Text = "添加";
                if (_iDataType == 1)
                {
                    this.Text = "添加图层";
                }
                else
                {
                    this.Text = "添加数据集";
                    this.comboBoxRender.Enabled = false;//添加数据集时只允许自动匹配
                }

                if (!ModuleMap._DefaultDBsource.Equals(""))
                {
                    //this.comboBoxDataSource.Text = ModuleMap._DefaultDBsource;
                    for (int i = 0; i < this.comboBoxDataSource.Items.Count; i++)
                    {
                        if (this.comboBoxDataSource.Items[i].ToString().Equals(ModuleMap._DefaultDBsource))
                        {
                            this.comboBoxDataSource.SelectedIndex = i;
                            break;
                        }
                    }
                }

                //获取字段
                IFields pFields = null;
                string FieldName = "";
                if (ptmpFeaClass != null)
                {
                    //没有进行联表查询，则直接将字段的名称加载到CmbFields中
                    pFields = ptmpFeaClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        FieldName = pFields.get_Field(i).Name;
                        if (FieldName.ToLower().Contains("shape")) continue;
                        CmbFields.Items.Add(FieldName);
                        //设置主显字段时，排除隐藏字段
                        if (!ModuleMap._ListHideFields.Contains(FieldName))
                        {
                            cmbKeyField.Items.Add(FieldName);//主显字段
                        }
                    }
                    CmbFields.SelectedIndex = 0;
                    cmbKeyField.SelectedIndex = 0;//主显字段
                }
                //end  ***********************************************************************
            }
            else    //若修改图层，则按照图层的属性进行对话框设置
            {
                this.Text = "修改图层";
                this.btnAddLayer.Text = "修改";
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(_xmlpath);
                //获取图层节点
                _curNode = _node;
                string strSearch = "//Layer[@NodeKey='" + _node.Name + "']";
                XmlNode pxmlnode = xmldoc.SelectSingleNode(strSearch);
                XmlElement pxmlele = pxmlnode as XmlElement;
                if (pxmlnode == null)
                    return;
                //获取图层的渲染节点
                XmlNode vNodeShow = pxmlnode["AboutShow"];
                XmlNode vNodeLabel = pxmlnode["AttrLabel"];

                XmlElement vShowElement = vNodeShow as XmlElement;
                XmlElement vLabelElement = vNodeLabel as XmlElement;

                string strDataSourceID = "";
                string feaclscode = "";
                string strDatasetName = "";
                string strRenderID = "";
                if (pxmlele.HasAttribute("ConnectKey"))
                {
                    strDataSourceID=pxmlele.GetAttribute("ConnectKey");
                }
                if (pxmlele.HasAttribute("Code"))
                {
                    feaclscode = pxmlele.GetAttribute("Code");
                }
                if (pxmlele.HasAttribute("FeatureDatasetName"))
                {
                    strDatasetName=pxmlele.GetAttribute("FeatureDatasetName");
                }
                if (vShowElement.HasAttribute("Renderer"))
                {
                    strRenderID = vShowElement.GetAttribute("Renderer");
                }
                object objDataSource = sysTable.GetFieldValue("DATABASEMD", "DATABASENAME", "ID=" + strDataSourceID, out eError);
                object objRender = sysTable.GetFieldValue("Render", "Layer", "ID='" + strRenderID + "'", out eError);

                string DataSourcename = "";
                string strRender = "";
                if (objDataSource != null) DataSourcename = objDataSource.ToString();

                


                if (objRender != null) strRender = objRender.ToString();
                //设置数据源
                for (int i = 0; i < this.comboBoxDataSource.Items.Count; i++)
                {
                    if (this.comboBoxDataSource.Items[i].ToString().Equals(DataSourcename))
                    {
                        this.comboBoxDataSource.SelectedIndex = i;
                        break;
                    }
                }
                //设置数据集
                for (int i = 0; i < this.comboBoxFeaDS.Items.Count; i++)
                {
                    if (this.comboBoxFeaDS.Items[i].ToString().Equals(strDatasetName))
                    {
                        this.comboBoxFeaDS.SelectedIndex = i;
                        break;
                    }
                }
                //设置地物类
                string strTrueFeaclscode = feaclscode;
                if (feaclscode.Contains("."))
                {
                    strTrueFeaclscode = feaclscode.Substring(feaclscode.IndexOf(".") + 1);
                }
                for (int i = 0; i < this.comboBoxFeaClass.Items.Count; i++)
                {
                    string strTrueItem = comboBoxFeaClass.Items[i].ToString();
                    if (strTrueItem.Contains("."))
                    {
                        strTrueItem = strTrueItem.Substring(strTrueItem.IndexOf(".")+1);
                    }
                    if (strTrueItem.Equals(strTrueFeaclscode))
                    {
                        this.comboBoxFeaClass.SelectedIndex = i;
                        break;
                    }
                }

                //added by chulili 2012-11-09 获取修改前的要素类
                string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "DATABASENAME='" + DataSourcename + "'", out eError).ToString();
                int type = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourcename + "'", out eError).ToString());
                IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
                try
                {
                    _OldFeatureClass = (pWorkspace as IFeatureWorkspace).OpenFeatureClass(feaclscode);
                }
                catch
                { }

                //设置符号
                for (int i = 0; i < this.comboBoxRender.Items.Count; i++)
                {
                    if (this.comboBoxRender.Items[i].ToString().Equals(strRender))
                    {
                        this.comboBoxRender.SelectedIndex = i;
                        this.chkAutoMatchRender.Checked = true;
                        break;
                    }
                }
                

                //设置图层属性和标注
                GetShowAttriOfNode(vShowElement, pxmlele);
                if (vLabelElement != null)
                {
                    GetLabelAtrriOfNode(vLabelElement);
                }
                XmlNode vNodeDefine = pxmlnode["ShowDefine"];
                if (vNodeDefine != null)
                {
                    GetDefineAtrriOfNode(vNodeDefine as XmlElement );
                }
            }
        }
        //获取节点的Show方面的属性
        private void GetShowAttriOfNode(XmlElement vShowElement,XmlElement pxmlele)
        {
            string strFCAliaseName = "";
            //string strDataType = "";
            string strLoad = "";
            string strView = "";
            string strEdit = "";
            string strQuery = "";
            string strSelect = "";
            string strKeyField = "";
            if (pxmlele.HasAttribute("NodeText"))
            {
                strFCAliaseName = pxmlele.GetAttribute("NodeText");
            }
            //strDataType=pxmlele.GetAttribute("DataType");  
            if (pxmlele.HasAttribute("Load"))
            {
                strLoad = pxmlele.GetAttribute("Load");
            }
            if (pxmlele.HasAttribute("View"))
            {
                strView = pxmlele.GetAttribute("View");
            }
            //added by chulili 20110713
            if (vShowElement.HasAttribute("IsEdit"))
            {
                strEdit = vShowElement.GetAttribute("IsEdit");
            }
            if (vShowElement.HasAttribute("IsQuery"))
            {
                strQuery = vShowElement.GetAttribute("IsQuery");
            }
            if (vShowElement.HasAttribute("IsSelect"))
            {
                strSelect = vShowElement.GetAttribute("IsSelect");
            }
            //end add
            string strMaxScale = "";
            string strMinScale = "";
            if (vShowElement.HasAttribute("MaxScale"))
            {
                strMaxScale = vShowElement.GetAttribute("MaxScale");
            }
            if (vShowElement.HasAttribute("MinScale"))
            {
                strMinScale = vShowElement.GetAttribute("MinScale");
            }
            if (vShowElement.HasAttribute("KeyField"))
            {
                strKeyField = vShowElement.GetAttribute("KeyField");
            }
            for (int i = 0; i < cmbKeyField.Items.Count; i++)
            {
                if (cmbKeyField.Items[i].ToString() == strKeyField)
                {
                    cmbKeyField.SelectedIndex = i;
                    break;
                }
            }
            this.txtLayer.Text = strFCAliaseName;
            this.txtMaxScale.Text = strMaxScale;
            this.txtMinScale.Text = strMinScale;
            //是否加载
            if (strLoad == "0" ||strLoad=="")
            {
                this.chkLoad.Checked = false;
            }
            else
            {
                this.chkLoad.Checked = true;
            }
            //是否可见
            if (strView == "0"||strLoad=="")
            {
                this.chkView.Checked = false;
            }
            else
            {
                this.chkView.Checked = true;
            }
            //是否可编辑
            if (strEdit.ToUpper() == "TRUE")
            { 
                this.chkEdit.Checked = true; 
            }
            else
            { 
                this.chkEdit.Checked = false; 
            }
            //是否可查询
            if (strQuery.ToUpper() == "TRUE")
            { 
                this.chkQuery.Checked = true; 
            }
            else
            { 
                this.chkQuery.Checked = false; 
            }
            //是否可选择
            if (strSelect.ToUpper() == "TRUE")
            { 
                this.chkSelected.Checked = true; 
            }
            else
            {
                this.chkSelected.Checked = false;
            }
        }
        //获取节点的定义显示方面的属性
        private void GetDefineAtrriOfNode(XmlElement peleDefine)
        {
            if (peleDefine == null) return;
            string strDefineExpression = peleDefine.GetAttribute("Express");
            this.textBoxFilter.Text = strDefineExpression;
        }
        //获取节点的标注方面的属性
        private void GetLabelAtrriOfNode(XmlElement vLabelElement)
        {            
            string strIsLabel = vLabelElement.GetAttribute("IsLabel");
            string strHideSymbol = "";
            if (vLabelElement.HasAttribute("HideSymbol"))
            {
                strHideSymbol = vLabelElement.GetAttribute("HideSymbol");
            }
            string strLabelField = vLabelElement.GetAttribute("Expression");
            string strFontName = vLabelElement.GetAttribute("FontName");
            string strFontSize = vLabelElement.GetAttribute("FontSize");
            string strColor = vLabelElement.GetAttribute("FontBoldColor");
            //added by chulili 20110713 补充label属性
            string strFontUnderLine = vLabelElement.GetAttribute("FontUnderLine");
            string strFontFontBold = vLabelElement.GetAttribute("FontBold");
            string strFontItalic = vLabelElement.GetAttribute("FontItalic");
            string strNumLabelsOption = vLabelElement.GetAttribute("NumLabelsOption");
            //end add
            string strMaxScale = vLabelElement.GetAttribute("MaxScale");
            string strMinScale = vLabelElement.GetAttribute("MinScale");

            if (strIsLabel.ToUpper()  == "TRUE")
            {
                this.chkIsLabel.Checked = true;
            }
            else
            {
                this.chkIsLabel.Checked = false;//changed by chulili 20110712修改bug true->false
            }

            if (strHideSymbol.ToUpper() == "TRUE")
            {
                this.chkHideSymbol.Checked = true;
            }
            else
            {
                this.chkHideSymbol.Checked = false;//changed by chulili 20110712修改bug true->false
            }
            for (int i = 0; i < CmbFields.Items.Count; i++)
            {
                if (CmbFields.Items[i].ToString() == strLabelField)
                {
                    CmbFields.SelectedIndex = i;
                    break;
                }
            }
            //this.CmbFields.Text = strLabelField;
            this.txtMaxLabelScale.Text = strMaxScale;
            this.txtMinLabelScale.Text = strMinScale;
            for (int i = 0; i < CmbFontName.Items.Count; i++)
            {
                if (CmbFontName.Items[i].ToString() == strFontName)
                {
                    CmbFontName.SelectedIndex = i;
                }
                //end
                //IFeatureWorkspace pFeaWorkspace = pWorkspace as IFeatureWorkspace;
                //20110630 褚丽丽删除取地物类别名的代码

            }

            for (int i = 0; i < CmbFontSize.Items.Count; i++)
            {
                if (CmbFontSize.Items[i].ToString() == strFontSize)
                {
                    CmbFontSize.SelectedIndex = i;
                }

            }
            if (strFontSize != "")
            {
                newSize = (float)Convert.ToDouble(strFontSize);
            }
            else
            {
                newSize = 10;
            }
            if (strFontName != "")
            {
                newFamily = new FontFamily(strFontName);
            }
            else
            {
                strFontName = "宋体";
            }
            try
            {
                int intColor = Convert.ToInt32(strColor);
                LabelText.ForeColor  = ColorTranslator.FromOle(intColor); ;
            }
            catch
            { }
            newFont = FontStyle.Regular;//普通文本
            //是否粗体
            if (strFontFontBold.ToUpper() == "TRUE")
            {
                this.btnBold.Checked = true;
                newFont = newFont ^ FontStyle.Bold;
            }
            else
            {
                this.btnBold.Checked = false;
            }
            //是否斜体
            if (strFontItalic.ToUpper() == "TRUE")
            {
                this.btnItalic.Checked = true;
                newFont = newFont ^ FontStyle.Italic;
            }
            else
            {
                this.btnItalic.Checked = false;
            }
            //是否下划线
            if (strFontUnderLine.ToUpper() == "TRUE")
            {
                this.btnUnderLine.Checked = true;
                newFont = newFont ^ FontStyle.Underline;
            }
            else
            {
                this.btnUnderLine.Checked = false;
            }
            setFont();
            switch (strNumLabelsOption)
            {
                case"esriOneLabelPerName":
                    this.rdbPerName.Checked = true;
                    break;
                case "esriOneLabelPerPart":
                    this.rdbPerPart.Checked = true;
                    break;
                case "esriOneLabelPerShape":
                    this.rdbPerShape.Checked = true;
                    break;
                default:
                    this.rdbPerName.Checked = true;
                    break;
                    
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bIsModify = false;
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            m_bIsModify = true;
            Exception eError;

            //cyf 20110613 add:数据集
#region 获得工作空间
            IFeatureWorkspace pfws = _dataWorkspace as IFeatureWorkspace;
            if (pfws == null)
                pfws = _tmpWorkspace as IFeatureWorkspace;
            if (pfws == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工作空间失败，请检查！");
                return;
            }
#endregion
            if (this.comboBoxFeaDS.Text.Equals(""))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未选择数据集！");
                return;
            }
            //end

            //将默认数据源名称记下，以备用户下次添加图层时使用
            ModuleMap._DefaultDBsource = this.comboBoxDataSource.Text;


            XmlDocument layerxmldoc = new XmlDocument();
            layerxmldoc.Load(_xmlpath);
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);

            object objDbsource = sysTable.GetFieldValue("DATABASEMD", "ID", "DATABASENAME='"+this.comboBoxDataSource.Text +"'", out eError);
            string strRender="";
            string strDBsource = "";
            if (objDbsource != null)
            {
                strDBsource = objDbsource.ToString();
            }
            else
            {
                MessageBox.Show("数据源连接失败！");
                return;
            }
 
            int indexFeaClass = -1;
            if (m_strRasterType == "")
            {
                indexFeaClass = this.comboBoxFeaClass.SelectedIndex;  //矢量
            }
            else
            {
                indexFeaClass = this.comboBoxFeaDS.SelectedIndex;//影像
            }
            string SelectedFeaClsName = "";
            string SelectedDataType = "";
            if (_iDataType == 2)//添加数据集
            {
                if (this.comboBoxFeaClass.Text == "所有要素类")
                {
                    SelectedDataType = "FD";
                }
                else
                {
                    if (indexFeaClass >= 0)
                    {
                        SelectedDataType = _DicDataType[indexFeaClass];
                        SelectedFeaClsName = this.comboBoxFeaDS.Text ;
                    }
                }
            }
            else//添加或修改图层
            {
                if (indexFeaClass >= 0)
                {
                    SelectedDataType = _DicDataType[indexFeaClass];
                    if (SelectedDataType == "FC")
                    {
                        SelectedFeaClsName = this.comboBoxFeaClass.Text.Trim();
                    }
                    else
                    {
                        SelectedFeaClsName = this.comboBoxFeaDS.Text.Trim();
                    }
                }
            }

            //若属于添加图层或数据集
            if (_isAdd == true)
            {               
                //***********************************************************************************
                if (SelectedDataType == "FD")
                {
                    //shduan 20110613首先增加数据集节点
                    DevComponents.AdvTree.Node addNode = new DevComponents.AdvTree.Node();
                    addNode.Text = this.txtLayer.Text;
                    addNode.Tag = "DataDIR";
                    addNode.Image = _ImgList.Images["DataDIROpen"];
                    addNode.CheckBoxVisible = true;
                    //changed by chulili 20110712根据用户的选择判断节点是否选中
                    if (this.chkLoad.Checked)
                    {
                        addNode.CheckState = CheckState.Checked;//加载
                    }
                    else
                    {
                        addNode.CheckState = CheckState.Unchecked;//不加载
                    }
                    string nodekey = Guid.NewGuid().ToString();
                    addNode.Name = nodekey;
                    _node.Nodes.Add(addNode);
                    _curNode = addNode;
                    string strTag = _node.Tag.ToString();
                    string NodeName = strTag;
                    if (strTag.Contains("DataDIR"))
                    {
                        NodeName = "DataDIR";
                    }
                    string strSearch = "//" + NodeName + "[@NodeKey='" + _node.Name + "']";
                    XmlNode pxmlnode = layerxmldoc.SelectSingleNode(strSearch);
                    XmlElement childele = layerxmldoc.CreateElement("DataDIR");


                    childele.SetAttribute("NodeKey", nodekey);
                    childele.SetAttribute("NodeText", this.txtLayer.Text);
                    //childele.SetAttribute("Description", pFrm._DataSetScrip);
                    childele.SetAttribute("Enabled", "true");
                    childele.SetAttribute("Expand", "100");
                    pxmlnode.AppendChild(childele as XmlNode);

                    ModuleMap.SetDataKey(addNode, childele as XmlNode);
                    //addNode.DataKey = childele as object;  //added by chulili 20110701视图连动
                    //addNode.Parent.DataKey = pxmlnode as object;
                    //XMLDoc.Save(_LayerXmlPath);
                    //cyf 20110613 add:获得数据下面所有要素类的名称
                    SysCommon.Gis.SysGisDataSet pSysDt = new SysGisDataSet();
                    pSysDt.WorkSpace = _dataWorkspace;
                    List<string> LstFeaClsName = new List<string>();  //数据集下面所有的要素类名称
                    //获取数据集下面的所有要素类名称集合
                    string strFeatDSName = this.comboBoxFeaDS.Text.Trim();
                    //shduan 20110718 delete**************************************************
                    //if (strFeatDSName.Contains("."))
                    //{
                    //    strFeatDSName = strFeatDSName.Substring(strFeatDSName.IndexOf(".") + 1);
                    //}
                    //shduan 20110718 delete**************************************************
                    LstFeaClsName = pSysDt.GetFeatureClassNames(strFeatDSName, false, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集下面所有要素类名称失败！");
                        layerxmldoc.Save(_xmlpath);
                        return;
                    }
                    SelectedDataType = "FC";
                    //遍历FD下所有的FC加载到当前节点下
                    foreach (string pFeaClaName in LstFeaClsName)
                    {
                        IFeatureClass pFeatClass = pfws.OpenFeatureClass(pFeaClaName);
                        esriGeometryType ptype = pFeatClass.ShapeType;
                        string strFeatureType = ptype.ToString();
                        _CurFeatureClass = pFeatClass;
                        //changed by chulili 添加输入参数：layername 地物类的别名作为图层名
                        GetFCFromWS(pfws, SelectedDataType, addNode, pFeaClaName, strDBsource, childele as XmlNode, layerxmldoc);
                    }
                }
                else
                {
                    //changed by chulili 添加输入参数  用地物类别名作为图层名称
                    GetFCFromWS(pfws, SelectedDataType, _node, SelectedFeaClsName, strDBsource, null, layerxmldoc);
                }
            }
            //若属于修改图层
            else
            {
                #region 
                if (_pUC != null)
                {
                    _pUC.RemoveLayer(_node);
                }
                _node.Text = this.txtLayer.Text;
                string strTag = _node.Tag.ToString();
                string xmlNodename = strTag;
                if (strTag.Contains("DataDIR"))
                {
                    xmlNodename = "DataDIR";
                }
                string strSearch = "//" + xmlNodename + "[@NodeKey='" + _node.Name + "']";

                XmlNode pxmlnode = layerxmldoc.SelectSingleNode(strSearch);
                XmlElement pele = pxmlnode as XmlElement;
                pele.SetAttribute("NodeText", this.txtLayer.Text);
                pfws = _dataWorkspace as IFeatureWorkspace;
                if (pfws == null)
                    pfws = _tmpWorkspace as IFeatureWorkspace;
                if (pfws == null)
                {
                    layerxmldoc.Save(_xmlpath);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
                //IFeatureClass paddfc = pfws.OpenFeatureClass(this.comboBoxFeaClass.Text );
                string strCode = "";//地物类的名称或者RC、RD的名称
                string strFeatureType = "";
                switch (SelectedDataType)
                {
                    case "FC":
                        IFeatureClass paddfc = pfws.OpenFeatureClass(this.comboBoxFeaClass.Text);
                        IDataset paddDataset = paddfc as IDataset;
                        esriGeometryType ptype = paddfc.ShapeType;
                        strCode = this.comboBoxFeaClass.Text;
                        strFeatureType = ptype.ToString();
                        //added by chulili 20111024 标注地物类需获取FeatureType属性
                        if (paddfc.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            strFeatureType = "esriFTAnnotation";
                        }
                        else if (paddfc.FeatureType == esriFeatureType.esriFTDimension)
                        {
                            strFeatureType = "esriFTAnnotation";
                        }
                        //end added by chulili 20111024
                        pele.SetAttribute("DataType", "FC");
                        pele.SetAttribute("FeatureType", strFeatureType);
                        break;
                    case "RC":
                        strCode = this.comboBoxFeaClass.Text;
                        pele.SetAttribute("DataType", "RC");
                        pele.SetAttribute("FeatureType", "");
                        break;
                    case "RD":
                        strCode = this.comboBoxFeaClass.Text;
                        pele.SetAttribute("DataType", "RD");
                        pele.SetAttribute("FeatureType", "");
                        break;
                }
                //为节点设置image
                SetNodeImage(_node, SelectedDataType, strFeatureType);
                _node.CheckBoxVisible = true;
                if (this.chkLoad.Checked)
                {
                    pele.SetAttribute("Load","1");
                    _node.CheckState = CheckState.Checked;
                }
                else
                {
                    pele.SetAttribute("Load","0");
                    _node.CheckState = CheckState.Unchecked;
                }
                if (this.chkView.Checked)
                {
                    pele.SetAttribute("View","1");
                }
                else
                {
                    pele.SetAttribute("View","0");
                }

                pele.SetAttribute("NodeText", this.txtLayer.Text);
                pele.SetAttribute("ConnectKey", strDBsource);
                pele.SetAttribute("FeatureDatasetName", this.comboBoxFeaDS.Text);

                //判断名称如果包含用户名就去掉 xisheng20110923
                //if (strCode.Contains("."))
                //    strCode = strCode.Substring(strCode.IndexOf(".") + 1);
                pele.SetAttribute("Code", strCode);
                pele.SetAttribute("Enabled", "true");
                #region 褚丽丽注释掉20110712 在SetOtherAttriOfNode函数里设置renderer和最大、最小比例尺
                //设置符号化属性
                //XmlNode nodeRender = pxmlnode["AboutShow"];
                //XmlElement eleRender = null;
                //if (nodeRender == null)
                //{
                //    eleRender = layerxmldoc.CreateElement("AboutShow");
                //    nodeRender = pxmlnode.AppendChild(eleRender as XmlNode);

                //}
                //eleRender = nodeRender as XmlElement;
                //eleRender.SetAttribute("Renderer", strRender);
                //eleRender.SetAttribute("MaxScale", strMaxScale);
                //eleRender.SetAttribute("MinScale", strMinScale);
                #endregion
                string pTrueFeaClsName = this.comboBoxFeaClass.Text;
                if (this.comboBoxFeaClass.Text.Contains("."))
                {
                    pTrueFeaClsName = this.comboBoxFeaClass.Text.Substring(this.comboBoxFeaClass.Text.IndexOf(".") + 1);
                }
                //shduan add 20110629
                //设置匹配的值，图层名或地物类名
                string strMatchName = "";
                if (_DirectoryFieldname == "Layer")
                {
                    strMatchName = this.txtLayer.Text;
                }
                else
                {
                    strMatchName = pTrueFeaClsName;
                }
                SetOtherAttriOfNode(pxmlnode as XmlNode, layerxmldoc, SelectedDataType, strFeatureType, pTrueFeaClsName, strMatchName);
                if (SelectedDataType == "FC")
                {
                    IFeatureClass pTmpaddfc = pfws.OpenFeatureClass(this.comboBoxFeaClass.Text);
                    if (!pTmpaddfc.Equals(_OldFeatureClass))
                    {
                        foreach (XmlNode pFiledNode in pxmlnode.ChildNodes)
                        {
                            if (pFiledNode.Name == "Field")
                            {
                                pxmlnode.RemoveChild(pFiledNode);
                            }
                        }
                        AddFieldOfLayerNode(pTmpaddfc, pxmlnode, layerxmldoc);
                    }
                }
                //设置定义显示    //changed by chulili 定义显示，暂时屏蔽
                //XmlNode nodeDefine = pxmlnode["ShowDefine"];
                //XmlElement eleDefine = nodeDefine as XmlElement;
                //eleDefine.SetAttribute("Express", strDefineExpression);
                //eleDefine.SetAttribute("IsDefineDisplay", "1");

                layerxmldoc.Save(_xmlpath);

                XmlNode pTMPnode = layerxmldoc.SelectSingleNode("//Layer[@NodeKey='" + _node.Name + "']");

                ModuleMap.SetDataKey(_node, pTMPnode);  //changed by chulili 20111011设置节点及其所有父节点的DataKey
                //_node.DataKey = pTMPnode as object;

                #endregion

                _curNode = _node;


            }
            this.DialogResult = DialogResult.OK;
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            this.Close();
        }

        private void GetFCFromWS(IFeatureWorkspace pfws, string SelectedDataType, DevComponents.AdvTree.Node addNode, string SelectedFeaClsName,  string strDBsource, XmlNode pxmlChildnode, XmlDocument layerxmldoc)
        {
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            Exception eError = null;
            string strLayerName = SelectedFeaClsName;//shduan 20110718先默认给FC名称，后面获取FC别名
            //deleted by chulili 20110816 统一使用图层字典进行中英文映射
            //if (this.chkAutoMatchScale.Checked)
            //{
            //    object objLayerName = sysTable.GetFieldValue("Render", "Layer", "FeatureClass='" + SelectedFeaClsName + "'", out eError);
            //    if (objLayerName != null)
            //    {
            //        strLayerName = objLayerName.ToString();
            //    }
            //}
            //else
            //{
            if (_iDataType == 1)
            {
                strLayerName = this.txtLayer.Text;//added by chulili 20110816图层名允许用户修改
            }
            else
            {
                strLayerName = GetChineseNameOfFeaCls(SelectedFeaClsName);
            }
            //}
            //deleted by chulili 20110816
            DevComponents.AdvTree.Node addSubNode = new DevComponents.AdvTree.Node();
            addSubNode.Text = strLayerName;
            addSubNode.Tag = "Layer";
            string nodekey = Guid.NewGuid().ToString();
            addSubNode.Name = nodekey;
            //string strCode = SelectedFeaClsName;//地物类的名称或者RC、RD的名称
            //strCode = SelectedFeaClsName;  //shduan 20110615 add
            string strFeatureType = "";//shduan 20110615 add 这个地方的值从哪儿获得？？？？？
            //added by chulili 20110630获取最大、最小比例尺
            //string strTrueFeaClsName = SelectedFeaClsName;
            //if (SelectedFeaClsName.Contains("."))
            //{
            //    strTrueFeaClsName = SelectedFeaClsName.Substring(SelectedFeaClsName.IndexOf(".") + 1);
            //}
            //object objMaxScale = sysTable.GetFieldValue("Render", "MaxScale", "FeatureClass='" + strTrueFeaClsName + "'", out eError);
            //object objMinScale = sysTable.GetFieldValue("Render", "MinScale", "FeatureClass='" + strTrueFeaClsName + "'", out eError);

            #region 设置图标
            switch (SelectedDataType)
            {               
                case "FC":
                    IFeatureClass paddfc = null;
                    try {paddfc= pfws.OpenFeatureClass(SelectedFeaClsName); }
                    catch { }
                    esriGeometryType ptype = paddfc.ShapeType;
                    strFeatureType = ptype.ToString();
                    //added by chulili 20111024 标注地物类需获取FeatureType属性
                    if (paddfc.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        strFeatureType = "esriFTAnnotation";
                    }
                    else if (paddfc.FeatureType == esriFeatureType.esriFTDimension)
                    {
                        strFeatureType = "esriFTAnnotation";
                    }
                    //end added by chulili 20111024
                    if (_ImgList != null)
                    {
                       
                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                addSubNode.Image = _ImgList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                addSubNode.Image = _ImgList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                addSubNode.Image = _ImgList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                addSubNode.Image = _ImgList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                addSubNode.Image = _ImgList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                addSubNode.Image = _ImgList.Images["_MultiPatch"];
                                break;
                            default:
                                addSubNode.Image = _ImgList.Images["Layer"];
                                break;
                        }
                    }
                    break;
                case "RC":
                    if (_ImgList != null)
                        addSubNode.Image = _ImgList.Images["Layer"];
                    strFeatureType = m_strRasterType;
                    break;
                case "RD":
                    if (_ImgList != null)
                        addSubNode.Image = _ImgList.Images["Layer"];
                    strFeatureType = m_strRasterType;
                    break;
            }
            #endregion
            addSubNode.CheckBoxVisible = true;
            //if (_isAdd == true)
            //{//changed by chulili 20110707修改节点时，不改变加载状态
            //    addSubNode.CheckState = CheckState.Unchecked;//cyf 20110613 modify CheckState.Unchecked;
            //}

            XmlElement childele = layerxmldoc.CreateElement("Layer");
            #region 设置节点的普通属性
            childele.SetAttribute("NodeText", strLayerName);
            childele.SetAttribute("NodeKey", nodekey);
            childele.SetAttribute("Enabled", "true");
            childele.SetAttribute("ConnectKey", strDBsource);
            //if (SelectedDataType == "FC")
            //{
                childele.SetAttribute("FeatureDatasetName", this.comboBoxFeaDS.Text);
            //}
            if (SelectedDataType == "RC" || SelectedDataType == "RD")
            {
                SelectedFeaClsName = this.comboBoxFeaDS.Text;
            }
            childele.SetAttribute("DataType", SelectedDataType);
            childele.SetAttribute("FeatureType", strFeatureType);
            //if (SelectedFeaClsName.Contains("."))  //判断名称如果包含用户名就去掉 xisheng20110923
            //{
            //    SelectedFeaClsName = SelectedFeaClsName.Substring(SelectedFeaClsName.LastIndexOf(".") + 1);
            //}
            childele.SetAttribute("Code", SelectedFeaClsName);
            //是否加载changed by chulili 20110709
            if (this.chkLoad.Checked)
            {
                childele.SetAttribute("Load", "1");
                addSubNode.CheckState = CheckState.Checked;
            }
            else
            {
                childele.SetAttribute("Load", "0");
                addSubNode.CheckState = CheckState.Unchecked;
            }
            //是否可见changed by chulili 20110709
            if (this.chkView.Checked)
            {
                childele.SetAttribute("View", "1");
            }
            else
            {
                childele.SetAttribute("View", "0");
            }
            
            #endregion
            if (pxmlChildnode != null)
            {
                pxmlChildnode.AppendChild(childele as XmlNode);
            }
            else
            {
                string strTag = addNode.Tag.ToString();
                //string NodeName = strTag;  //shduan 20110615**********************
                string NodeName ;
                if (strTag.Contains("DataDIR"))
                {
                    NodeName = "DataDIR";
                }
                else 
                {
                    NodeName = strTag;
                }
                //end***************************************************************
                string strSearch = "//" + NodeName + "[@NodeKey='" + addNode.Name + "']";
                pxmlChildnode = layerxmldoc.SelectSingleNode(strSearch);
                pxmlChildnode.AppendChild(childele as XmlNode);
            }
            //changed by chulili 20110711添加一个参数，在该函数里设置标注等属性
            //设置与符号库匹配的值，图层名或者地物类名称
            string strMatchName = "";
            if (_DirectoryFieldname == "Layer")
            {
                strMatchName = strLayerName;
            }
            else
            {
                strMatchName = SelectedFeaClsName;
                if (SelectedFeaClsName.Contains("."))
                {
                    strMatchName = SelectedFeaClsName.Substring(SelectedFeaClsName.IndexOf(".") + 1);
                }
            }
            SetOtherAttriOfNode(childele as XmlNode, layerxmldoc, SelectedDataType, strFeatureType, SelectedFeaClsName, strMatchName);
            
            //added by chulili 2012-11-09 如果是featureclass，则添加字段到xml中
            if (SelectedDataType == "FC")
            {
                IFeatureClass pTmpaddfc = null;
                try { pTmpaddfc = pfws.OpenFeatureClass(SelectedFeaClsName); }
                catch { }
                AddFieldOfLayerNode(pTmpaddfc,childele as XmlNode,layerxmldoc);
            }

            #region changed by chulili 20110711这些属性统一在SetOtherAttriOfNode函数里面设置

            #endregion
            layerxmldoc.Save(_xmlpath);

            XmlNode pTMPnode = layerxmldoc.SelectSingleNode("//Layer[@NodeKey='" + nodekey + "']");
            addSubNode.DataKey = pTMPnode as object;
            addNode.Nodes.Add(addSubNode);//在树图中添加advnode
            addNode.DataKey = pTMPnode.ParentNode as object ;
            if (_curNode == null)
            {
                _curNode = addSubNode;
            }
        }
        private void AddFieldOfLayerNode(IFeatureClass pFeatureClass,XmlNode pXmlNode,XmlDocument pXmldoc)
        {
            if (pFeatureClass == null)
            {
                return;
            }
            IFields pFields = pFeatureClass.Fields;
            if (SysCommon.ModField._DicFieldName.Keys.Count == 0)
            {
                SysCommon.ModField.InitNameDic(_tmpWorkspace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);
                string strName = pField.Name;
                string strAliasName = pField.AliasName;
                string strGUID = Guid.NewGuid().ToString();
                XmlElement childele = pXmldoc.CreateElement("Field");
                childele.SetAttribute("Name", strName);
                childele.SetAttribute("AliasName", strAliasName);
                string strChinesename = SysCommon.ModField.GetChineseNameOfField(strName);
                childele.SetAttribute("NodeText", strChinesename);
                childele.SetAttribute("NodeKey", strGUID);
                childele.SetAttribute("Visible", "true");
                pXmlNode.AppendChild(childele as XmlNode);
            }
        }
        private void SetNodeImage(DevComponents.AdvTree.Node pNode, string DataType,string esriType)
        {
            switch (DataType)
            {
                case "FC":
                    if (_ImgList != null)
                    {
                        switch (esriType)
                        {
                            case "esriGeometryPoint":
                                pNode.Image = _ImgList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                pNode.Image = _ImgList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                pNode.Image = _ImgList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                pNode.Image = _ImgList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                pNode.Image = _ImgList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                pNode.Image = _ImgList.Images["_MultiPatch"];
                                break;
                            default:
                                pNode.Image = _ImgList.Images["Layer"];
                                break;
                        }
                    }
                    break;
                case "RC":
                    if (_ImgList != null)
                        pNode.Image = _ImgList.Images["Layer"];//RC->Layer 没有名称为RC的图片
                    break;
                case "RD":
                    if (_ImgList != null)
                        pNode.Image = _ImgList.Images["Layer"];//RC->Layer  没有名称为RD的图片
                    break;
            }
        }
        //设置节点的Show属性
        private void setShowAttriOfNode(XmlNode pnodeShow, ILayer pLayer, string strDataType,object objRender)
        {
            #region 设置renderer、最大、最小比例尺
            if (pnodeShow == null) return;
            XmlElement eleShow = pnodeShow as XmlElement;
            if (eleShow == null) return;
            if (objRender != null)//shduan 20110615 add判断
            {
                eleShow.SetAttribute("Renderer", objRender.ToString());
            }
            else
            {
                eleShow.SetAttribute("Renderer", "");
            }
            string strMaxScale = "";
            string strMinScale = "";
            if (this.chkAutoMatchScale.Checked)
            {
                if (pLayer != null)
                {
                    strMaxScale = pLayer.MaximumScale.ToString();
                    strMinScale = pLayer.MinimumScale.ToString();
                }
            }
            else
            {
                strMaxScale= this.txtMaxScale.Text;
                strMinScale= this.txtMinScale.Text;
            }
            string strKeyField = this.cmbKeyField.Text;
            if (strKeyField != "")
            {
                eleShow.SetAttribute("KeyField", strKeyField);
            }
            else
            {
                if (_CurFeatureClass != null)
                {
                    for (int i = 0; i < _CurFeatureClass.Fields.FieldCount; i++)
                    {
                        IField pField = _CurFeatureClass.Fields.get_Field(i);
                        if (pField.Type != esriFieldType.esriFieldTypeGeometry && pField.Type != esriFieldType.esriFieldTypeBlob && pField.Type != esriFieldType.esriFieldTypeDate)
                        {
                            if (ModuleMap._ListHideFields != null)
                            {
                                if (!ModuleMap._ListHideFields.Contains(pField.Name))
                                {
                                    eleShow.SetAttribute("KeyField", pField.Name);
                                    break;
                                }
                            }
                            else
                            {
                                eleShow.SetAttribute("KeyField", pField.Name);
                                break;
                            }
                        }
                    }
                }
            }
            eleShow.SetAttribute("MaxScale", strMaxScale);
            eleShow.SetAttribute("MinScale", strMinScale);
            //shduan 20110721 只有FC时才有其他显示参数
            if (strDataType == "FC")
            {
                eleShow.SetAttribute("IsEdit", this.chkEdit.Checked.ToString());
                eleShow.SetAttribute("IsQuery", this.chkQuery.Checked.ToString());
                eleShow.SetAttribute("IsSnap", "False");
                eleShow.SetAttribute("IsSelect", this.chkSelected.Checked.ToString());
                eleShow.SetAttribute("LayerTransparency", "0");
                eleShow.SetAttribute("IsReferScale", "true");
            }
            #endregion
        }
        //设置节点的Label属性
        private void setLabelAttriOfNode(XmlNode pnodeLabel, ILayer pLayer, string strDataType, string strFeatureType)
        {
            if (pnodeLabel == null) return;
            XmlElement peleLabel = pnodeLabel as XmlElement;
            if (peleLabel == null) return;
            if (strDataType == "FC")
            {
                //为AttrLabel节点写属性

                #region 写标注相关属性
                string strIsLabel = "";
                string strExpression = "";
                string strFontName = "";
                string strFontSize = "";
                string strFontUnderLine = "";
                string strFontBold = "";
                string strFontItalic = "";
                string strFontBoldColor = "";
                string strLabelMaxScale = "";
                string strLabelMinScale = "";
                string strNumLabelsOption = "";
                if (this.chkAutoMatchLabel.Checked)
                {
                    IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                    if (pGeoFeatureLayer != null)
                    {
                        IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatureLayer.AnnotationProperties;
                        //定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
                        IAnnotateLayerProperties pAnnoLayerProperties = null;
                        IElementCollection pElementCollection = null;
                        IElementCollection pElementCollection1 = null;
                        //获取标注渲染对象
                        pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
                        //ILabelEngineLayerProperties pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties;
                        //IBasicOverposterLayerProperties4 pBasicOverposterLayerProperties = pLabelEngineLayerPro.BasicOverposterLayerProperties as IBasicOverposterLayerProperties4;
                        ILabelEngineLayerProperties2 pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties2;
                        IBasicOverposterLayerProperties pBasicOverposterLayerProperties = pLabelEngineLayerPro.OverposterLayerProperties as IBasicOverposterLayerProperties;
                        ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;
                        if (pAnnoLayerProperties != null)
                        {

                            strExpression = pLabelEngineLayerPro.Expression.ToString();
                            //处理字段名  Expression中字段名格式是：[字段名]
                            if (strExpression.StartsWith("["))
                            {
                                strExpression = strExpression.Substring(1);
                            }
                            if (strExpression.EndsWith("]"))
                            {
                                strExpression = strExpression.Substring(0, strExpression.Length - 1);
                            }

                            strIsLabel = pGeoFeatureLayer.DisplayAnnotation.ToString();
                            strFontName = pTextSymbol.Font.Name;
                            strFontSize = Convert.ToInt32(pTextSymbol.Font.Size).ToString();
                            strFontUnderLine = pTextSymbol.Font.Underline.ToString();
                            strFontBold = pTextSymbol.Font.Bold.ToString();
                            strFontItalic = pTextSymbol.Font.Italic.ToString();
                            //IRgbColor pRGBcolor= ColorTranslator.FromOle(pTextSymbol.Color.RGB) as IRgbColor;
                            strFontBoldColor = pTextSymbol.Color.RGB.ToString();
                            strLabelMaxScale = pAnnoLayerProperties.AnnotationMaximumScale.ToString();
                            strLabelMinScale = pAnnoLayerProperties.AnnotationMinimumScale.ToString();
                            if (pBasicOverposterLayerProperties != null)
                            {
                                switch (pBasicOverposterLayerProperties.NumLabelsOption)
                                {
                                    case esriBasicNumLabelsOption.esriOneLabelPerName:
                                        strNumLabelsOption = "esriOneLabelPerName";
                                        break;
                                    case esriBasicNumLabelsOption.esriOneLabelPerPart:
                                        strNumLabelsOption = "esriOneLabelPerPart";
                                        break;
                                    case esriBasicNumLabelsOption.esriOneLabelPerShape:
                                        strNumLabelsOption = "esriOneLabelPerShape";
                                        break;
                                    case esriBasicNumLabelsOption.esriNoLabelRestrictions:
                                        strNumLabelsOption = "esriNoLabelRestrictions";
                                        break;
                                }
                            }
                            else
                            {
                                strNumLabelsOption = "esriOneLabelPerName";
                            }
                        }
                    }
                }
                else
                {
                    IRgbColor pColor = new RgbColorClass();
                    pColor.Red = Convert.ToInt32(LabelText.ForeColor.R);
                    pColor.Green = Convert.ToInt32(LabelText.ForeColor.G);
                    pColor.Blue = Convert.ToInt32(LabelText.ForeColor.B);

                    strFontBoldColor = pColor.RGB.ToString();
                    System.Drawing.Font pFont = LabelText.Font;

                    strIsLabel = this.chkIsLabel.Checked.ToString();
                    strExpression = this.CmbFields.Text;
                    strFontName = pFont.Name;
                    strFontSize = pFont.Size.ToString();
                    strFontUnderLine = pFont.Underline.ToString();
                    strFontBold = pFont.Bold.ToString();
                    strFontItalic = pFont.Italic.ToString();


                    strLabelMaxScale = this.txtMaxLabelScale.Text;
                    strLabelMinScale = this.txtMinLabelScale.Text;
                    if (this.rdbPerName.Checked)
                    {
                        strNumLabelsOption = "esriOneLabelPerName";
                    }
                    else if (this.rdbPerPart.Checked)
                    {
                        strNumLabelsOption = "esriOneLabelPerPart";
                    }
                    else if (this.rdbPerShape.Checked)
                    {
                        strNumLabelsOption = "esriOneLabelPerShape";
                    }
                }
                peleLabel.SetAttribute("IsLabel", strIsLabel);
                peleLabel.SetAttribute("HideSymbol", chkHideSymbol.Checked.ToString());
                peleLabel.SetAttribute("Expression", strExpression);
                peleLabel.SetAttribute("FontName", strFontName);
                peleLabel.SetAttribute("FontSize", strFontSize);
                peleLabel.SetAttribute("FontUnderLine", strFontUnderLine);
                peleLabel.SetAttribute("FontBold", strFontBold);
                peleLabel.SetAttribute("FontItalic", strFontItalic);
                peleLabel.SetAttribute("FontBoldColor", strFontBoldColor);
                peleLabel.SetAttribute("MaxScale", strLabelMaxScale);
                peleLabel.SetAttribute("MinScale", strLabelMinScale);
                peleLabel.SetAttribute("NumLabelsOption", strNumLabelsOption);

                peleLabel.SetAttribute("RotationField", "");
                peleLabel.SetAttribute("RotationType", "");

                //shduan 20110623 修改XML节点属性名称，并区分点、线、面*****************************************
                if (strFeatureType == "esriGeometryPoint")
                {
                    peleLabel.SetAttribute("PointPlacementAngles", "");
                    peleLabel.SetAttribute("PointPlacementOnTop", "true");
                    peleLabel.SetAttribute("PointPlacementMethod", "esriOnTopPoint");
                    peleLabel.SetAttribute("PointPlacementPriorities", "22122333");
                }
                else if (strFeatureType == "esriGeometryPolyline")
                {
                    peleLabel.SetAttribute("PointPlacementAngles", "");
                    peleLabel.SetAttribute("PointPlacementOnTop", "true");
                    peleLabel.SetAttribute("PointPlacementMethod", "esriOnTopPoint");
                }
                else if (strFeatureType == "esriGeometryPolygon")
                {
                    peleLabel.SetAttribute("PolygonPlacementMethod", "esriAlwaysHorizontal");
                    peleLabel.SetAttribute("PlaceOnlyInsidePolygon", "false");
                }
                //end ******************************************************************************************************
                #endregion
            }
        }
        private void setDefineAttriOfNode(XmlNode pnodeDefine, ILayer pLayer, string strDataType)
        {
            if (pnodeDefine == null) return;
            XmlElement peleDefine = pnodeDefine as XmlElement;
            if (peleDefine == null) return;

            if (this.chkAutoMatchFilter.Checked )
            {
                IFeatureLayerDefinition pLayerDefine = pLayer as IFeatureLayerDefinition;
                if (pLayerDefine != null)
                {
                    peleDefine.SetAttribute("Express", pLayerDefine.DefinitionExpression);
                    if (!pLayerDefine.DefinitionExpression.Equals(string.Empty ))
                    {
                        peleDefine.SetAttribute("IsDefineDisplay", "True");
                    }
                    else
                    {
                        peleDefine.SetAttribute("IsDefineDisplay", "False");
                    }
                }
            }
            else
            {
                peleDefine.SetAttribute("Express", this.textBoxFilter.Text);

                if (!this.textBoxFilter.Text.Equals(string.Empty )) //changed by chulili 20120830 修改bug
                {
                    peleDefine.SetAttribute("IsDefineDisplay", "True");
                }
                else
                {
                    peleDefine.SetAttribute("IsDefineDisplay", "False");
                }
            }
        }
        //添加参数strMatchName 表示与符号库中匹配的值  如果用图层名匹配，该参数就是图层名；如果用地物类名匹配，该参数就是地物类名
        private void SetOtherAttriOfNode(XmlNode pNode, XmlDocument pXmlDoc,string strDataType, string strFeatureType,string strTrueFeaClsName,string strMatchName)
        {
            Exception eError = null;
            if (pNode == null) return;

            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            object objLayer = null;
            if (this.comboBoxRender.Text.Equals("自动匹配"))
            {
                objLayer = ModuleRenderer.GetLayerConfigFromBlob(_RenderFieldname +"='" + strMatchName + "'", _tmpWorkspace);
            }
            else
            {
                objLayer = ModuleRenderer.GetLayerConfigFromBlob(_RenderFieldname +"='" + this.comboBoxRender.Text + "'", _tmpWorkspace);
            }
            object objRender = null;
            if (this.chkAutoMatchRender.Checked)//是否自动匹配符号
            {
                if (this.comboBoxRender.Text.Equals("自动匹配"))
                {
                    objRender = sysTable.GetFieldValue("Render", "ID", _RenderFieldname +"='" + strMatchName + "'", out eError);
                }
                else
                {
                    objRender = sysTable.GetFieldValue("Render", "ID", _RenderFieldname + "='" + this.comboBoxRender.Text + "'", out eError);
                }
            }
            //shduan 20110801
            ILayer pLayer = null;
            if (objLayer!=null)
            {
                pLayer = objLayer as ILayer;
            }

            XmlNodeList pList = pNode.ChildNodes;
            XmlNode nodeShow=null;
            XmlNode nodeLabel = null;
            XmlNode nodeDefine = null;
            XmlNode nodeCAD = null;
            //判断是否已有子节点，有则取出子节点ode
            XmlElement pNodeEle = pNode as XmlElement;
            if (pNodeEle != null)//添加OrderID属性
            {
                if (!pNodeEle.HasAttribute("OrderID"))
                {
                    pNodeEle.SetAttribute("OrderID", "-1");
                }
            }
            //changed by chulili 20110711用这种简单方法获得子节点
            nodeLabel = pNode["AttrLabel"];
            nodeDefine = pNode["ShowDefine"];
            nodeShow = pNode["AboutShow"];            
            nodeCAD = pNode["AboutCADShow"];
            //为AboutShow节点写属性
            XmlElement eleShow = null;
            if (nodeShow == null)
            {
                eleShow = pXmlDoc.CreateElement("AboutShow");
                nodeShow = pNode.AppendChild(eleShow as XmlNode);
            }
            setShowAttriOfNode(nodeShow, pLayer, strDataType, objRender);

            //shduan 20110721 只有FC时才有其他显示参数
            if (strDataType == "FC")
            {
                //为标注节点写属性
                XmlElement eleLabel = null;
                if (nodeLabel == null)
                {
                    eleLabel = pXmlDoc.CreateElement("AttrLabel");
                    nodeLabel = pNode.AppendChild(eleLabel as XmlNode);
                }
                setLabelAttriOfNode(nodeLabel, pLayer, strDataType, strFeatureType);
                //为ShowDefine节点写属性
                XmlElement eleDefine = null;
                if (nodeDefine == null)
                {
                    eleDefine = pXmlDoc.CreateElement("ShowDefine");
                    nodeDefine = pNode.AppendChild(eleDefine as XmlNode);
                }
                setDefineAttriOfNode(nodeDefine, pLayer, strDataType);

                //为AboutCADShow节点写属性
                XmlElement eleCAD = null;
                if (nodeCAD == null)
                {
                    eleCAD = pXmlDoc.CreateElement("AboutCADShow");
                    nodeCAD = pNode.AppendChild(eleCAD as XmlNode);
                }
            }
        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
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

        private void comboBoxDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110613 add
            this.comboBoxFeaClass.Items.Clear();
            this.comboBoxFeaDS.Items.Clear();
            //end
            SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            Exception eError;
            string DataSourceName = this.comboBoxDataSource.Text;
            string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
            int type = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
            int iDataType = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATABASETYPEID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
            string strDBPara = sysTable.GetFieldValue("DATABASEMD", "DBPARA", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
            if (iDataType == 4)
            {
                m_strRasterType = "DOM";
                this.comboBoxFeaClass.Text = "";
                this.comboBoxFeaClass.Enabled = false;
                this.groupPanel1.Enabled = false;
                this.chkEdit.Enabled = false;
                this.chkQuery.Enabled = false;
                this.chkSelected.Enabled = false;
                this.comboBoxRender.Enabled = false;
            }
            else if (iDataType == 3)
            {
                m_strRasterType = "DEM";
                this.comboBoxFeaClass.Text = "";
                this.comboBoxFeaClass.Enabled = false;
                this.groupPanel1.Enabled = false;
                this.chkEdit.Enabled = false;
                this.chkQuery.Enabled = false;
                this.chkSelected.Enabled = false;
                this.comboBoxRender.Enabled = true ;
            }
            else
            {
                m_strRasterType = "";
                this.comboBoxFeaClass.Enabled = true;
                //this.groupPanel1.Enabled = true;
                this.chkEdit.Enabled = true;
                this.chkQuery.Enabled = true;
                this.chkSelected.Enabled = true;
                //this.comboBoxRender.Enabled = true;
            }
            int index6 = conninfostr.LastIndexOf("|");
            IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null)  return;
            _dataWorkspace = pWorkspace;
            //shduan 20110625 ***********************************************************************
            //_DicFeaClass.Clear();
            _DicDataType.Clear();

            string strdataset = "";
            string strDatasets = conninfostr.Substring(index6 + 1);
            string[] strTemp = strDatasets.Split(new char[] { ',' });
            string[] strDBParas = strDBPara.Split(new char[] { ',' }); 
            for (int i = 0; i < strTemp.Length; i++)
            {
                this.comboBoxFeaDS.Items.Add(strTemp[i]);
                if (iDataType == 3 || iDataType == 4)
                {
                    //_DicFeaClass.Add(i, strTemp[i]);
                    if (strDBParas[i].Contains("栅格数据集"))
                    {
                        _DicDataType.Add(i, "RD");
                    }
                    else if (strDBParas[i].Contains("栅格编目"))
                    {
                        _DicDataType.Add(i, "RC");
                    }
                }
            }
            if (this.comboBoxFeaDS.Items.Count > 0)
            {
                comboBoxFeaDS.SelectedIndex = 0;
            }            //end ***********************************************************************************
        }
        private string  GetChineseNameOfFeaCls(string pFeaClsName)
        {
            string pTrueFeaClsName = pFeaClsName;
            if (pFeaClsName.Contains("."))
            {
                pTrueFeaClsName = pFeaClsName.Substring(pFeaClsName.IndexOf(".") + 1);
            }
            string pLayerName = pTrueFeaClsName;
            if (SysCommon.ModField._DicLayerName.Keys.Contains(pTrueFeaClsName))
            {
                pLayerName = SysCommon.ModField._DicLayerName[pTrueFeaClsName];
            }
            return pLayerName;
        }
        private void comboBoxFeaClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFCName = comboBoxFeaClass.Text;
            string pLayerName = GetChineseNameOfFeaCls(strFCName);
            
            //shduan 20110615
            //if (this.txtLayer.Text.Equals(""))
            //{
            //    this.txtLayer.Text = comboBoxFeaClass.Text;
            //}
            //if(_isAdd)    //changed by chulili 20110701
            //{
                if (strFCName == "所有要素类")
                {
                    this.txtLayer.Text = this.comboBoxFeaDS.Text;
                    return;
                }
                else
                {
                    this.txtLayer.Text = pLayerName;
                }
            //}
            //shduan 20110624 根据图层获取对应的字段，放在标注的下拉列表中

            if (_dataWorkspace == null)
            {
                this.txtLayer.Text = pLayerName;

                return;

            }
            //end
            IFeatureWorkspace pFeaWorkspace = _dataWorkspace as IFeatureWorkspace;
            IFeatureClass  ptmpFeaClass = pFeaWorkspace.OpenFeatureClass(strFCName);

            //获取字段
            IFields pFields = null;
            string FieldName = "";
            if (ptmpFeaClass != null)
            {
                _CurFeatureClass = ptmpFeaClass;
                //没有进行联表查询，则直接将字段的名称加载到CmbFields中
                pFields = ptmpFeaClass.Fields;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    FieldName = pFields.get_Field(i).Name;
                    if (FieldName.ToLower() == "shape") continue;
                    CmbFields.Items.Add(FieldName);
                    //设置主显字段时，排除隐藏字段
                    if (ModuleMap._ListHideFields != null)
                    {
                        if (!ModuleMap._ListHideFields.Contains(FieldName))
                        {
                            cmbKeyField.Items.Add(FieldName);
                        }
                    }
                    else
                    {
                        cmbKeyField.Items.Add(FieldName);
                    }
                }
                CmbFields.SelectedIndex = 0;
                cmbKeyField.SelectedIndex = 0;
            }
        }

        private void comboBoxFeaDS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110613 add
            //if (_isAdd)
            //{
                this.comboBoxFeaClass.Items.Clear();
            //}

            if (_iDataType == 2 && m_strRasterType == "")
            {
                //说明添加的是数据集
                this.comboBoxFeaClass.Items.Add("所有要素类");
                this.comboBoxFeaClass.Text = "所有要素类";
                comboBoxFeaClass.Enabled = false;
            }
            else
            {
                comboBoxFeaClass.Enabled = true;
                string strdataset = this.comboBoxFeaDS.Text.Trim();  //当前数据集名称
                if (_dataWorkspace == null) return;
                IFeatureWorkspace pFeaWorkspace = _dataWorkspace as IFeatureWorkspace;
                IDataset pSourcedataset = null;
                //_DicFeaClass.Clear();
                //_DicDataType.Clear();
                //if (!strdataset.Equals(""))

                if (m_strRasterType == "DEM" || m_strRasterType == "DOM")
                {
                    comboBoxFeaClass.Text = "";
                    comboBoxFeaClass.Enabled = false;
                    this.txtLayer.Text = comboBoxFeaDS.Text;
                }
                else
                {
                    //comboBoxFeaClass.Enabled = true;
                    //string strdataset = this.comboBoxFeaDS.Text.Trim();  //当前数据集名称
                    //if (_dataWorkspace == null) return;
                    //IFeatureWorkspace pFeaWorkspace = _dataWorkspace as IFeatureWorkspace;
                    //IDataset pSourcedataset = null;
                    //_DicFeaClass.Clear();
                    _DicDataType.Clear();
                    if (!strdataset.Equals(""))
                    {
                        try
                        {//打开已知的地物类集合
                            pSourcedataset = pFeaWorkspace.OpenFeatureDataset(strdataset);
                        }
                        catch
                        { }

                    }
                    if (pSourcedataset == null)
                    {
                        comboBoxFeaClass.Enabled = true;
                        //枚举所有的地物类集合
                        IEnumDataset pDatasets = _dataWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                        IDataset ptmpDataset = pDatasets.Next();
                        while (ptmpDataset != null)
                        {
                            if (ptmpDataset.Name.Substring(ptmpDataset.Name.Length - 4).Equals("_GOH"))
                            {
                                ptmpDataset = pDatasets.Next();
                                continue;
                            }
                            IEnumDataset ptmpFeaclasses = ptmpDataset.Subsets;
                            IDataset ptmpFeaClsDataset = ptmpFeaclasses.Next();

                            while (ptmpFeaClsDataset != null)
                            {
                                IFeatureClass pFeaCls = ptmpFeaClsDataset as IFeatureClass;
                                //_DicFeaClass.Add(this.comboBoxFeaClass.Items.Count, ptmpFeaClsDataset.Name);
                                _DicDataType.Add(this.comboBoxFeaClass.Items.Count, "FC");
                                this.comboBoxFeaClass.Items.Add(ptmpFeaClsDataset.Name);
                                ptmpFeaClsDataset = ptmpFeaclasses.Next();
                            }
                            ptmpDataset = pDatasets.Next();
                        }
                        IEnumDataset pDatasets2 = _dataWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                        IDataset ptmpDataset2 = pDatasets2.Next();

                        while (ptmpDataset2 != null)
                        {
                            IFeatureClass pFeaCls2 = ptmpDataset2 as IFeatureClass;
                            //_DicFeaClass.Add(this.comboBoxFeaClass.Items.Count, ptmpDataset2.Name);
                            _DicDataType.Add(this.comboBoxFeaClass.Items.Count, "FC");
                            this.comboBoxFeaClass.Items.Add(ptmpDataset2.Name);
                            ptmpDataset2 = pDatasets2.Next();
                        }
                    }
                    else
                    {
                        //shduan20110613 增加所有要素类
                        //this.comboBoxFeaClass.Items.Add("所有要素类");

                        IEnumDataset ptmpFeaclasses = pSourcedataset.Subsets;
                        IDataset ptmpFeaClsDataset = ptmpFeaclasses.Next();
                        while (ptmpFeaClsDataset != null)
                        {
                            IFeatureClass pTmpFeaCls = ptmpFeaClsDataset as IFeatureClass;
                            //_DicFeaClass.Add(this.comboBoxFeaClass.Items.Count, ptmpFeaClsDataset.Name);
                            _DicDataType.Add(this.comboBoxFeaClass.Items.Count, "FC");
                            this.comboBoxFeaClass.Items.Add(ptmpFeaClsDataset.Name);

                            ptmpFeaClsDataset = ptmpFeaclasses.Next();
                        }
                    }
                    ////枚举所有的RC
                    //IEnumDataset pDatasetsRC = _dataWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                    //IDataset ptmpDatasetRC = pDatasetsRC.Next();

                    //while (ptmpDatasetRC != null)
                    //{
                    //    //IRasterCatalog ptmpRC = ptmpDatasetRC as IRasterCatalog;
                    //    _DicFeaClass.Add(this.comboBoxFeaClass.Items.Count, ptmpDatasetRC.Name);
                    //    _DicDataType.Add(this.comboBoxFeaClass.Items.Count, "RC");
                    //    this.comboBoxFeaClass.Items.Add(ptmpDatasetRC.Name);
                    //    ptmpDatasetRC = pDatasetsRC.Next();
                    //}
                    ////枚举所有的RD
                    //IEnumDataset pDatasetsRD = _dataWorkspace.get_Datasets(esriDatasetType.esriDTRasterDataset);
                    //IDataset ptmpDatasetRD = pDatasetsRD.Next();
                    //while (ptmpDatasetRD != null)
                    //{
                    //    _DicFeaClass.Add(this.comboBoxFeaClass.Items.Count, ptmpDatasetRD.Name);
                    //    _DicDataType.Add(this.comboBoxFeaClass.Items.Count, "RD");
                    //    this.comboBoxFeaClass.Items.Add(ptmpDatasetRD.Name);
                    //    ptmpDatasetRD = pDatasetsRD.Next();
                    //}
                }
            }
           
            if (comboBoxFeaClass.Items.Count > 0)
            {   //若数据源中有对于地物类集合的限定，则使用该地物类集合
                //comboBoxFeaClass.SelectedIndex = 1;
            }
            //end
        }
        //改变选择的地物类或影像
        private void comboBoxRender_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Exception eError = null;
            //SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            //object objLayer = ModuleRenderer.GetLayerConfigFromBlob("Layer='" + this.comboBoxRender.Text + "'", _tmpWorkspace);
            ////object objMaxScale = sysTable.GetFieldValue("Render", "MaxScale", "FeatureClass='" + this.comboBoxRender.Text + "'", out eError);
            ////object objMinScale = sysTable.GetFieldValue("Render", "MinScale", "FeatureClass='" + this.comboBoxRender.Text + "'", out eError);
            //string strMaxScale = "";
            //string strMinScale = "";
            //ILayer pLayer = null;
            ////changed by chulili 20110711最大最小比例尺皆从Layer中获取
            ////if (objMaxScale != null)
            ////    strMaxScale = objMaxScale.ToString();
            ////if (objMinScale != null)
            ////    strMinScale = objMinScale.ToString();
            ////end changed
            //if (objLayer == null)
            //{
            //    return;
            //}
            //pLayer = objLayer as ILayer;

            //strMaxScale = pLayer.MaximumScale.ToString();
            //strMinScale = pLayer.MinimumScale.ToString();
            
            ////changed by chulili 20110711 符号化切换图层，最大最小比例尺自动切换
            ////if (this.txtMaxScale.Text.Equals("") || this.txtMaxScale.Text.Equals("0"))
            ////{
            //    this.txtMaxScale.Text = strMaxScale;
            ////}
            ////if (this.txtMinScale.Text.Equals("") || this.txtMinScale.Text.Equals("0"))
            ////{
            //    this.txtMinScale.Text = strMinScale;
            ////}
            ////end changed 
            //#region 从图层中读取标注信息 褚丽丽添加 20110711
            //IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            //if (pGeoFeatureLayer == null)
            //    return;
            ////是否标注
            //this.chkIsLabel.Checked = pGeoFeatureLayer.DisplayAnnotation;
            //IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatureLayer.AnnotationProperties;
            ////定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
            //IAnnotateLayerProperties pAnnoLayerProperties = null;
            //IElementCollection pElementCollection = null;
            //IElementCollection pElementCollection1 = null;
            ////获取标注渲染对象
            //pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
            //ILabelEngineLayerProperties pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties;
            //ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;

            //if (pAnnoLayerProperties == null)
            //    return;
            //string strLabelMaxScale = "";
            //string strLabelMinScale = "";
            //string strLabelField = "";
            //string strLabelFont = "";
            //string strLabelSize = "";
            //IColor pLabelColor = null;
            ////比例尺
            //strLabelMaxScale = pAnnoLayerProperties.AnnotationMaximumScale.ToString();
            //strLabelMinScale = pAnnoLayerProperties.AnnotationMinimumScale.ToString();
            //IBasicOverposterLayerProperties4 pBasicOverposterLayerProperties =pLabelEngineLayerPro.BasicOverposterLayerProperties as IBasicOverposterLayerProperties4;
            //strLabelField = pLabelEngineLayerPro.Expression;
            ////处理字段名  Expression中字段名格式是：[字段名]
            //if (strLabelField.StartsWith("["))
            //{
            //    strLabelField = strLabelField.Substring(1);
            //}
            //if (strLabelField.EndsWith("]"))
            //{
            //    strLabelField = strLabelField.Substring(0, strLabelField.Length - 1);
            //}

            //strLabelFont = pTextSymbol.Font.Name;
            //strLabelSize = pTextSymbol.Size.ToString();
            //pLabelColor = pTextSymbol.Color;
            //this.txtMaxLabelScale.Text  = strLabelMaxScale;
            //this.txtMinLabelScale.Text = strLabelMinScale;
            //this.CmbFields.Text = strLabelField;
            //for (int i = 0; i < CmbFontName.Items.Count; i++)
            //{
            //    if (CmbFontName.Items[i].ToString() == strLabelFont)
            //    {
            //        CmbFontName.SelectedIndex = i;
            //        break;
            //    }

            //}
            //if (CmbFontSize.Items.Contains(strLabelSize))
            //{
            //    for (int i = 0; i < CmbFontSize.Items.Count; i++)
            //    {
            //        if (CmbFontSize.Items[i].ToString() == strLabelSize)
            //        {
            //            CmbFontSize.SelectedIndex = i;
            //            break;
            //        }

            //    }
            //}
            //else
            //{
            //    CmbFontSize.Items.Add(strLabelSize);
            //    CmbFontSize.SelectedIndex = CmbFontSize.Items.Count - 1;
            //}
            
            ////处理标注格式
            //this.btnUnderLine.Checked = pTextSymbol.Font.Underline;
            //this.btnBold.Checked = pTextSymbol.Font.Bold;
            //this.btnItalic.Checked = pTextSymbol.Font.Italic;
            //this.FontColorPicker.SelectedColor =ColorTranslator.FromOle(pTextSymbol.Color.RGB);

            //newSize = (float)Convert.ToDouble(strLabelSize);
            //newFamily = new FontFamily(strLabelFont);
            //newFont = FontStyle.Regular;
            //if (pTextSymbol.Font.Underline)
            //    newFont = newFont ^ FontStyle.Underline;
            //if (pTextSymbol.Font.Bold)
            //    newFont = newFont ^ FontStyle.Bold;
            //if (pTextSymbol.Font.Italic)
            //    newFont = newFont ^ FontStyle.Italic;
            //setFont();
            //LabelText.ForeColor = ColorTranslator.FromOle(pTextSymbol.Color.RGB);
            //if (pBasicOverposterLayerProperties != null)
            //{
            //    switch (pBasicOverposterLayerProperties.NumLabelsOption)
            //    {
            //        case esriBasicNumLabelsOption.esriOneLabelPerName:
            //            this.rdbPerName.Checked = true;
            //            break;
            //        case esriBasicNumLabelsOption.esriOneLabelPerPart:
            //            this.rdbPerPart.Checked = true;
            //            break;
            //        case esriBasicNumLabelsOption.esriOneLabelPerShape:
            //            this.rdbPerShape.Checked = true;
            //            break;
            //        case esriBasicNumLabelsOption.esriNoLabelRestrictions:
            //            this.rdbPerName.Checked = true;
            //            break;
            //    }
            //}
            //#endregion
        }

        private void textBoxMinScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";

            if (!char.IsControl(e.KeyChar)&&(!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true ;
            }
        }

        private void textBoxMaxScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void CmbFontSize_ComboBoxTextChanged(object sender, EventArgs e)
        {
            if (CmbFontSize.ControlText.Trim() == string.Empty || !IsNumeric(CmbFontSize.ControlText))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "字体大小请输入数字!");
                return;
            }

            newSize = (float)Convert.ToDouble(CmbFontSize.ControlText);
            setFont();
        }

        private bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
        } 

        private void btnUnderLine_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Underline == true)
            {
                btnUnderLine.Checked = false;
            }
            else
            {
                btnUnderLine.Checked = true;
            }
            newFont = newFont ^ FontStyle.Underline;
            setFont();
        }

        private void setFont()
        {
            this.LabelText.Font = new System.Drawing.Font(newFamily, newSize, newFont);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Italic == true)
            {
                btnItalic.Checked = false;
            }
            else
            {
                btnItalic.Checked = true;
            }
            newFont = newFont ^ FontStyle.Italic;
            setFont();
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            if (LabelText.Font.Bold == true)
            {
                btnBold.Checked = false;
            }
            else
            {
                btnBold.Checked = true;
            }
            newFont = newFont ^ FontStyle.Bold;
            setFont();
        }

        private void CmbFontName_ComboBoxTextChanged(object sender, EventArgs e)
        {
            if (CmbFontSize.SelectedItem != null)
            {
                newSize = (float)Convert.ToDouble(CmbFontSize.SelectedItem.ToString());
                setFont();
            }
        }

        private void FontColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            LabelText.ForeColor = FontColorPicker.SelectedColor;
        }

        private void CmbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            newFamily = new FontFamily(CmbFontName.SelectedItem.ToString());
            setFont();
        }

        private void txtMinLabelScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";

            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void txtMaxLabelScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";

            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void chkAutoMatchScale_CheckedChanged(object sender, EventArgs e)
        {
            string strMaxScale = "";
            string strMinScale = "";

            if (chkAutoMatchScale.Checked)
            {
                if (_iDataType == 1)
                {
                    //SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
                    //string strFeaClsName = this.comboBoxFeaClass.Text;
                    //string strTrueFeaClsName = strFeaClsName;
                    //if (strFeaClsName.Contains("."))
                    //{
                    //    strTrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
                    //}
                    //object objLayer = ModuleRenderer.GetLayerConfigFromBlob("Layer='" + strTrueFeaClsName + "'", _tmpWorkspace);
                    //if (objLayer != null)
                    //{
                    //    ILayer pLayer = objLayer;
                    //    strMaxScale = pLayer.MaximumScale.ToString();
                    //    strMinScale = pLayer.MinimumScale.ToString();
                    //}
                    //this.txtMinScale.Text = strMinScale;
                    //this.txtMaxScale.Text = strMaxScale;
                    this.txtMinScale.Enabled = false;
                    this.txtMaxScale.Enabled = false;
                }
                else
                {
                    this.txtMinScale.Enabled = false;
                    this.txtMaxScale.Enabled = false;
                }
            }
            else
            {
                if (_iDataType == 1)
                {
                    this.txtMinScale.Enabled = true;
                    this.txtMaxScale.Enabled = true;
                }
                else
                {
                    this.txtMinScale.Enabled = true;
                    this.txtMaxScale.Enabled = true;
                }
            }
        }
        //符号设置的含义改成匹配项，用来从下拉框中选择一项，用来匹配符号、比例尺、显示、标注等
        private void chkAutoMatchRender_CheckedChanged(object sender, EventArgs e)
        {
            //string strRender = "";
            //if (chkAutoMatchRender.Checked)
            //{
            //    if (_iDataType == 1)
            //    {
            //        //SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
            //        //string strFeaClsName = this.comboBoxFeaClass.Text;
            //        //string strTrueFeaClsName = strFeaClsName;
            //        //if (strFeaClsName.Contains("."))
            //        //{
            //        //    strTrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
            //        //}
            //        //object objRender = sysTable.GetFieldValue("Render", "ID", "Layer='" + strTrueFeaClsName + "'", out eError);
            //        //if (objRender != null) strRender = strTrueFeaClsName;
            //        //for (int i = 0; i < this.comboBoxRender.Items.Count; i++)
            //        //{
            //        //    if (this.comboBoxRender.Items[i].ToString() == strRender)
            //        //    {
            //        //        this.comboBoxRender.SelectedIndex = i;
            //        //        break;
            //        //    }
            //        //}
            //        this.comboBoxRender.Enabled = false;
                    
            //    }
            //    else
            //    {
            //        this.comboBoxRender.Enabled = false;
            //    }
            //}
            //else
            //{
            //    if (_iDataType == 1)
            //    {
            //        this.comboBoxRender.Enabled = true;
            //    }
            //    else
            //    {
            //        this.comboBoxRender.Enabled = true;
            //    }
            //}
        }

        private void chkAutoMatchLabel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoMatchLabel.Checked)
            {
                if (_iDataType == 1)
                {
                    //SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
                    //string strFeaClsName = this.comboBoxFeaClass.Text;
                    //string strTrueFeaClsName = strFeaClsName;
                    //if (strFeaClsName.Contains("."))
                    //{
                    //    strTrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
                    //}
                    //object objLayer = ModuleRenderer.GetLayerConfigFromBlob("Layer='" + strTrueFeaClsName + "'", _tmpWorkspace);
                    //if (objLayer != null)
                    //{
                    //    ILayer pLayer = objLayer as ILayer;
                    //    IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                    //    if (pGeoFeatureLayer != null)
                    //    {
                    //        IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = pGeoFeatureLayer.AnnotationProperties;
                    //        //定义IAnnotateLayerPropertiesCollection.QueryItem方法中调用的对象
                    //        IAnnotateLayerProperties pAnnoLayerProperties = null;
                    //        IElementCollection pElementCollection = null;
                    //        IElementCollection pElementCollection1 = null;
                    //        //获取标注渲染对象
                    //        pAnnotateLayerPropertiesCollection.QueryItem(0, out  pAnnoLayerProperties, out pElementCollection, out pElementCollection1);
                    //        ILabelEngineLayerProperties pLabelEngineLayerPro = pAnnoLayerProperties as ILabelEngineLayerProperties;
                    //        IBasicOverposterLayerProperties4 pBasicOverposterLayerProperties = pLabelEngineLayerPro.BasicOverposterLayerProperties as IBasicOverposterLayerProperties4;
                    //        ITextSymbol pTextSymbol = pLabelEngineLayerPro.Symbol;
                    //        if (pAnnoLayerProperties != null)
                    //        {
                    //            strExpression = pLabelEngineLayerPro.Expression.ToString();
                    //            //处理字段名  Expression中字段名格式是：[字段名]
                    //            if (strExpression.StartsWith("["))
                    //            {
                    //                strExpression = strExpression.Substring(1);
                    //            }
                    //            if (strExpression.EndsWith("]"))
                    //            {
                    //                strExpression = strExpression.Substring(0, strExpression.Length - 1);
                    //            }

                    //            if (pGeoFeatureLayer.DisplayAnnotation)
                    //            { this.chkIsLabel.Checked = true; }
                    //            else
                    //            { this.chkIsLabel.Checked = false; }
                    //            strFontName = pTextSymbol.Font.Name;
                    //            strFontSize = pTextSymbol.Font.Size.ToString();
                    //            strFontUnderLine = pTextSymbol.Font.Underline.ToString();
                    //            strFontBold = pTextSymbol.Font.Bold.ToString();
                    //            strFontItalic = pTextSymbol.Font.Italic.ToString();
                    //            strFontBoldColor = pTextSymbol.Color.RGB.ToString();

                    //            strLabelMaxScale = pAnnoLayerProperties.AnnotationMaximumScale.ToString();
                    //            strLabelMinScale = pAnnoLayerProperties.AnnotationMinimumScale.ToString();
                    //            if (pBasicOverposterLayerProperties != null)
                    //            {
                    //                switch (pBasicOverposterLayerProperties.NumLabelsOption)
                    //                {
                    //                    case esriBasicNumLabelsOption.esriOneLabelPerName:
                    //                        this.rdbPerName.Checked = true;
                    //                        break;
                    //                    case esriBasicNumLabelsOption.esriOneLabelPerPart:
                    //                        this.rdbPerPart.Checked = true;
                    //                        break;
                    //                    case esriBasicNumLabelsOption.esriOneLabelPerShape:
                    //                        this.rdbPerShape.Checked = true;
                    //                        break;
                    //                    case esriBasicNumLabelsOption.esriNoLabelRestrictions:
                    //                        this.rdbPerName.Checked = true;
                    //                        break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                this.rdbPerName.Checked = true;
                    //            }
                    //        }
                    //    }
                    //}
                    this.groupPanel1.Enabled = false;
                }
                else
                {
                    this.groupPanel1.Enabled = false;
                }
            }
            else
            {
                if (_iDataType == 1)
                {
                    this.groupPanel1.Enabled = true;
                }
                else
                {
                    this.groupPanel1.Enabled = true;
                }
            }
        }

        private void chkAutoMatchFilter_CheckedChanged(object sender, EventArgs e)
        {
            string strFilter = "";
            if (chkAutoMatchFilter.Checked)
            {
                if (_iDataType == 1)
                {
                    //SysGisTable sysTable = new SysGisTable(_tmpWorkspace);
                    //string strFeaClsName = this.comboBoxFeaClass.Text;
                    //string strTrueFeaClsName = strFeaClsName;
                    //if (strFeaClsName.Contains("."))
                    //{
                    //    strTrueFeaClsName = strFeaClsName.Substring(strFeaClsName.IndexOf(".") + 1);
                    //}
                    //object objLayer = ModuleRenderer.GetLayerConfigFromBlob("Layer='" + strTrueFeaClsName + "'", _tmpWorkspace);
                    //if (objLayer != null)
                    //{
                    //    ILayer pLayer = objLayer as ILayer;
                    //    IFeatureLayerDefinition pLayerDefine = pLayer as IFeatureLayerDefinition;
                    //    if (pLayerDefine != null)
                    //    {
                    //        strFilter = pLayerDefine.DefinitionExpression;
                    //    }
                    //}
                    //this.textBoxFilter.Text = strFilter;
                    this.textBoxFilter.Enabled = false;
                }
                else
                {
                    this.textBoxFilter.Enabled = false;
                }
            }
            else
            {
                if (_iDataType == 1)
                {
                    this.textBoxFilter.Enabled = true;
                }
                else
                {
                    this.textBoxFilter.Enabled = true;
                }
            }
        }


    }
}
