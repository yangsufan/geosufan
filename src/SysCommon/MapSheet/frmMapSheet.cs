using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoDatabaseDistributed;
using ESRI.ArcGIS.Geometry;
using System.Collections;


namespace SysCommon.MapSheet
{
    public partial class frmMapSheet : Form
    {
        private bool m_Res;
        int pPicindex = 0;
        string MapPath;
        IWorkspace pWorkSpace;
        private ITool _tool = null;
        private ICommand _cmd = null;
        private string[] pMapNumSelected = null;
        private string pMapSheetName = null;

        string mapFrameName = "";
        public string MapFrameName
        {
            set { mapFrameName = value; }
        }
        //返回结果
        public bool Res
        {
            get { return m_Res; }
        }

        //数据入库时的已入库范围
        private Dictionary<string, string[]> pMapNumExist4DBIMP = null;

        //数据入库时检查已经入库的图幅范围，是否与现在选择范围重叠，避免重复导入
        public Dictionary<string, string[]> MapNumExist4DBIMP
        {
            set { pMapNumExist4DBIMP = value; }
        }

        //数据更新时已提取进行更新的范围
        private Dictionary<string, string[]> pMapNumExist4DBUpdate = null;

        //数据更新提取时检查已经提取的图幅范围，是否与现在选择范围重叠，避免重复提取进行更新
        public Dictionary<string, string[]> MapNumExist4DBUpdate
        {
            set { pMapNumExist4DBUpdate = value; }
        }

        //选择图幅号时，是否进行入库或者更新提取的检查
        private bool pMapNumCheck = false;

        //是否进行查看图幅号重叠情况，写入分析日志
        public bool MapNumCheck
        {
            set { pMapNumCheck = value; }
        }

        //MapSheetName属性，获取图幅结合表图层名称
        public string MapSheetName
        {
            get { return pMapSheetName; }
        }

        //MapNumSelected属性，获取选择的图幅号数组
        public string[] MapNumSelected
        {
            get { return pMapNumSelected; }
            
        }

        public frmMapSheet(string vMapPath)
        {
            MapPath = vMapPath;
            InitializeComponent();
        }
        //新增一个构造，方便其它的窗体继承它
        public frmMapSheet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载图幅结合表
        /// </summary>
        public void AddMap()
        {
            IWorkspaceFactory pWorkSpaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkSpace;
            IDataset pDataSet;
            IFeatureClass pFeatureClass=null ;
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            ILayer pLayer;
            string LayerName;
            if (MapPath != "" && MapPath != null)
            {
                pWorkSpace = pWorkSpaceFactory.OpenFromFile(MapPath, 0);
                if (pWorkSpace != null)
                {
                    pFeatureWorkSpace = pWorkSpace as IFeatureWorkspace;

                    //假如传进了比例尺信息，则只能添加这个比例尺的图幅结合表，并把comboBoxScale置为不可用
                    if (vStrScale == "")
                    {
                        //列举要素类名称，加载到控件中
                        IEnumDataset pEnumDataset = pWorkSpace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
                        pEnumDataset.Reset();
                        IDataset pDataset = pEnumDataset.Next();
                        string pFirstMapName = null;
                        do
                        {
                            if (pDataset != null)
                            {
                                //如果是数据集
                                if (pDataset is IFeatureDataset)
                                {
                                    //IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset ;
                                    //if (pFeatureDataset != null)
                                    //{
                                    //    pEnumDataset = pFeatureDataset.Subsets();
                                    //}
                                }
                                //如果是要素类
                                else if (pDataset is IFeatureClass)
                                {
                                    this.comboBoxScale.Items.Add(pDataset.Name);
                                    if (pFirstMapName == null)
                                    {
                                        pFirstMapName = pDataset.Name;
                                        this.comboBoxScale.Text = pDataset.Name;
                                    }
                                    pDataset = pEnumDataset.Next();
                                }
                            }
                        } while (pDataset != null);

                        pFeatureClass = pFeatureWorkSpace.OpenFeatureClass(pFirstMapName);
                    }
                    else
                    {
                        //string MapFrameName = GetMapFrameName(vStrScale);
                        if (mapFrameName != "")
                        {
                            this.comboBoxScale.Items.Add(mapFrameName);
                            this.comboBoxScale.Text = mapFrameName;
                            this.comboBoxScale.Enabled = false;
                            pFeatureClass = pFeatureWorkSpace.OpenFeatureClass(mapFrameName);
                        }
                    }
                    if (pFeatureClass != null)
                    {
                        //图层显示标注
                        LabelingFeatureLayer(pFeatureLayer, "map_newno");

                        //图层透明
                        pDataSet = pFeatureClass as IDataset;
                        LayerName = pDataSet.Name;
                        pFeatureLayer.FeatureClass = pFeatureClass;
                        pFeatureLayer.Name = LayerName;
                        pLayer = pFeatureLayer as ILayer;
                        MapSheetControl.AddLayer(pLayer);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 标注指定图层的指定字段
        /// </summary>
        /// <param name="vFeatureLayer"></param>
        /// <param name="vLabelField"></param>
        private void LabelingFeatureLayer(IFeatureLayer vFeatureLayer, string vLabelField)
        {
            IAnnotateLayerPropertiesCollection pAnnotateLayerPropertiesCollection = null;
            IGeoFeatureLayer pGeoFeatureLayer = vFeatureLayer as IGeoFeatureLayer;

            pAnnotateLayerPropertiesCollection = pGeoFeatureLayer.AnnotationProperties as IAnnotateLayerPropertiesCollection;

        }

        private void frmMapSheet_Load(object sender, EventArgs e)
        {
            AddMap();
        }

        #region 放大、缩小、选择工具
        /// <summary>
        /// 漫游工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolPan_Click(object sender, EventArgs e)
        {
            _tool = new ControlsMapPanToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }

        /// <summary>
        /// 选择元素工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSelect_Click(object sender, EventArgs e)
        {
            _tool = new ControlsSelectToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 拉框放大工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolZoomIn_Click(object sender, EventArgs e)
        {
            _tool = new ControlsMapZoomInToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 拉框缩小工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolZoomOut_Click(object sender, EventArgs e)
        {
            _tool = new ControlsMapZoomOutToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 后景视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandLastExtendBack_Click(object sender, EventArgs e)
        {
            _cmd = new ControlsMapZoomToLastExtentBackCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 前景视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandLastExtendForward_Click(object sender, EventArgs e)
        {
            _cmd = new ControlsMapZoomToLastExtentForwardCommand();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 选择要素工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSelectFeatures_Click(object sender, EventArgs e)
        {
            _tool = new ControlsSelectFeaturesToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 清除选择集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolClearSelect_Click(object sender, EventArgs e)
        {
            _cmd = new ControlsClearSelectionCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 刷新图面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandRefresh_Click(object sender, EventArgs e)
        {
            _cmd = new ControlsMapRefreshViewCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 全图浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandFullExtend_Click(object sender, EventArgs e)
        {
            _cmd = new ControlsMapFullExtentCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        } 
        #endregion

        /// <summary>
        /// 比例尺下拉框控件中的内容发生改变时的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass = null;
            IFeatureLayer pFeaturelayer = new FeatureLayerClass();
            ILayer pLayer = null;
            IFeatureWorkspace pFeatureWorkspace = pWorkSpace as IFeatureWorkspace;
            if (pFeatureWorkspace != null)
            {
                MapSheetControl.Map.ClearLayers();

                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(comboBoxScale.Text);
                if (pFeatureClass != null)
                {
                    pFeaturelayer.FeatureClass= pFeatureClass;
                    pFeaturelayer.Name = comboBoxScale.Text;
                    pLayer = pFeaturelayer as ILayer;

                    MapSheetControl.AddLayer(pLayer);
                    pMapSheetName = comboBoxScale.Text;
                    balloonTip2.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 地图控件中的选择集发生变化时的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapSheetControl_OnSelectionChanged(object sender, EventArgs e)
        {
            
            IFeatureWorkspace pFeatureWorkspace = pWorkSpace as IFeatureWorkspace;
            IActiveView pActiview = MapSheetControl.Map as IActiveView;
            IFeatureLayer pFeaturelayer=null;
            IFeatureSelection pFeatureSelection=null;
            IFeatureClass pFeatureClass=null;
            IFeatureCursor pFeatureCur=null;
            ICursor pCur = null;
            IFields pFields=null;
            IFeature pFeat=null;
            string MapNum = null;

            //获得当前地图对象
            IMap pMap = pActiview.FocusMap as IMap;

            //获得当前图幅结合表层
            pFeaturelayer = pMap.get_Layer(0) as IFeatureLayer;
            pFeatureClass=pFeaturelayer.FeatureClass as IFeatureClass;
            pFields=pFeatureClass.Fields as IFields;
            Int32 i =pFields.FindField("map_newno");

            pFeatureSelection=pFeaturelayer as IFeatureSelection;

            //清除所有项，重新添加
            this.checkedListBoxNo.Items.Clear();
            this.richTextBox1.Text = "";
            this.pictureBox4.Visible = false;
            this.pictureBox3.Visible = false;
            //获取被选中的要素

            pFeatureSelection.SelectionSet.Search(null, false, out pCur);
            pFeatureCur = pCur as IFeatureCursor;

            pFeat = pFeatureCur.NextFeature();

            for (int j = 0; j < pFeatureSelection.SelectionSet.Count; j++)
            {
                MapNum = pFeat.get_Value(i).ToString();
                //以免重复加入（因为这个事件在第一次出发的时候，该函数总是被执行两次）
                if (this.checkedListBoxNo.Items.Contains(MapNum) == false)
                {
                    this.checkedListBoxNo.Items.Add(MapNum);
                    //新增的一条记录为选中
                    this.checkedListBoxNo.SetItemChecked(this.checkedListBoxNo.Items.Count-1, true);
                }
                pFeat = pFeatureCur.NextFeature();
            }

            pMapNumSelected=null;

            if(pFeatureSelection.SelectionSet.Count>0)
            {
                pMapNumSelected = new string[pFeatureSelection.SelectionSet.Count ];

                for (int k = 0; k <= pFeatureSelection.SelectionSet.Count - 1; k++)
                {
                    pMapNumSelected[k] = this.checkedListBoxNo.Items[k].ToString();
    			 
                }

                #region 选择图幅时的分析判断函数

                if(pMapNumCheck)
                {
                    MapOverlapAnanysis(pMapNumSelected,pMapNumExist4DBIMP,pMapNumExist4DBUpdate);
                }

                #endregion
                
            }
            //do
            //{
            //    pFeat = pFeatureCur.NextFeature();
            //    if (pFeat != null)
            //    {
            //        MapNum = pFeat.get_Value(i).ToString();
            //        //一面重复加入（因为这个事件在第一次出发的时候，该函数总是被执行两次）
            //        if (this.checkedListBoxNo.Items.Contains(MapNum) == false)
            //        {
            //            this.checkedListBoxNo.Items.Add(MapNum);
            //        }
            //    }
            //    pFeat = pFeatureCur.NextFeature();
            //} while (pFeat != null);
            

        }

        /// <summary>
        /// 图幅重叠性检查，将检查结果显示到日志列表框中
        /// </summary>
        /// <param name="pMapNumSelected"></param>
        /// <param name="pMapNumExist4DBIMP"></param>
        /// <param name="pMapNumExist4DBUpdate"></param>
        private void MapOverlapAnanysis(string[] pMapNumSelected, Dictionary<string, string[]> pMapNumExist4DBIMP, Dictionary<string, string[]> pMapNumExist4DBUpdate)
        {
            if (pMapNumSelected == null)
            {
                return;
            }
            else
            {
                //分析入库范围是否重叠
                if (pMapNumExist4DBIMP != null)
                {
                    object[] TheSameNum = null;
                    string[] MapNumExist = pMapNumExist4DBIMP[comboBoxScale.Text];   //获取当前比例尺图层下的已有图幅号集合

                    TheSameNum=CompareArrayNSelectSame(MapNumExist, pMapNumSelected).ToArray();

                    //有相同的内容则在日志中提示出来
                    if (TheSameNum.Length>0)
                    {
                        for (int i = 0; i <= TheSameNum.Length - 1; i++)
                        {
                            richTextBox1.Text = richTextBox1.Text + "\r\n" + "范围：" + TheSameNum[i] + "与已入库范围重叠！";
                        }
                        this.pictureBox4.Visible = true;
                        this.pictureBox3.Visible = false;
                    }
                    else
                    {
                        this.pictureBox3.Visible = true;
                        this.pictureBox4.Visible = false;
                    }
                }

                //分析更新提取范围是否重叠
                if (pMapNumExist4DBUpdate != null)
                {
                    object[] TheSameNum = null;
                    string[] MapNumExist = pMapNumExist4DBIMP[comboBoxScale.Text];   //获取当前比例尺图层下的已有图幅号集合

                    TheSameNum = CompareArrayNSelectSame(MapNumExist, pMapNumSelected).ToArray();

                    //有相同的内容则在日志中提示出来
                    if (TheSameNum.Length > 0)
                    {
                        for (int i = 0; i <= TheSameNum.Length - 1; i++)
                        {
                            richTextBox1.Text = richTextBox1.Text + "\r\n" + "范围：" + TheSameNum[i] + "与已提取范围重叠！";
                        }
                        this.pictureBox4.Visible = true;
                        this.pictureBox3.Visible = false;
                    }
                    else
                    {
                        this.pictureBox3.Visible = true;
                        this.pictureBox4.Visible = false;
                    }
                }
            }
        }

        private ArrayList CompareArrayNSelectSame(string[] MapNumExist, string[] pMapNumSelected)
        {
            ArrayList array1 = new ArrayList();
            ArrayList array2 = new ArrayList();

            for (int i = 0; i <= MapNumExist.Length - 1; i++)
            {
                array1.Add(MapNumExist[i]);

            }

            for (int j = 0; j <= MapNumSelected.Length - 1; j++)
            {

                if (array1.Contains(MapNumSelected[j]))
                {
                    array2.Add(MapNumSelected[j]);
                }

            }

            return array2;
        }




        /// <summary>
        /// 当按下的按钮为Enter时进行搜索，并选中高亮查找的图幅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchNumBox_KeyDown(object sender, KeyEventArgs e)
        {
            balloonTip2.Enabled = false;

            if (e.KeyCode == Keys.Enter)
            {
                if (SearchNumBox.Text != null)
                {
                    DoSearch(SearchNumBox.Text, comboBoxScale.Text);
                }
            }

            if (string.IsNullOrEmpty(SearchNumBox.Text))
            {
                balloonTip1.Enabled =true;
                pictureBox2.Visible = false;
                pictureBox1.Visible = false;
            }
            else
            {
                balloonTip1.Enabled =false;
            }
        }
        /// <summary>
        /// 根据图幅名称，查询并选中图符号
        /// </summary>
        /// <param name="vMapNum"></param>
        /// <param name="vFeaturecassName"></param>
        private void DoSearch(string vMapNum, string vFeaturecassName)
        {
            bool pFind=false;
            IFeatureClass pFeatureClass = null;
            IQueryFilter pQueryFilter=new QueryFilterClass();
            pQueryFilter.WhereClause="[map_newno]='"+vMapNum+"'";
            IFeature pFeature = null;

            IEnvelope pUnoinEvelope = new EnvelopeClass();

            IFeatureWorkspace pFeatureworspace = pWorkSpace as IFeatureWorkspace;
            pFeatureClass = pFeatureworspace.OpenFeatureClass(vFeaturecassName);

            if (pFeatureClass!=null)
            {
                ISelectionSet pSelectionset= pFeatureClass.Select(pQueryFilter,esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionNormal, pWorkSpace);
                IEnumIDs enumIDs = pSelectionset.IDs;
                int iD = enumIDs.Next();

                while (iD!=-1)
                {
                    pFind = true;
                    pFeature = pFeatureClass.GetFeature(iD);

                    pUnoinEvelope.Union(pFeature.Extent);

                    iD = enumIDs.Next();
                }

                if (pFind)
                {
                    pUnoinEvelope.Expand(2, 2, true);
                    MapSheetControl.Extent = pUnoinEvelope;
                }

                #region 根据是否找到结果来控制气泡和图片样式
                if (pFind)
                {
                    pictureBox2.Visible = false;
                    pictureBox1.Visible = true;
                    balloonTip2.Enabled = false;
                }
                else
                {
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = true;
                    balloonTip2.Enabled = true;
                } 
                #endregion
            }

        }

        private void cmdCancel_Click_1(object sender, EventArgs e)
        {
            pMapNumSelected = null;
            m_Res = false;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pPicindex += 1;
            if (pPicindex > 2)
            {
                pPicindex = 0;
            }
            if (pictureBox3.Visible)
            { pictureBox3.Load(Application.StartupPath + "\\..\\Res\\Pic\\SysCommom.MapSheet.NoDoubt" + pPicindex + ".png"); }
            if (pictureBox4.Visible)
            { pictureBox4.Load(Application.StartupPath + "\\..\\Res\\Pic\\SysCommom.MapSheet.Doubt" + pPicindex + ".png"); }
        }

        private void pictureBox4_VisibleChanged(object sender, EventArgs e)
        {
            if (pictureBox4.Visible)
            {
                timer1.Enabled = true;
                timer2.Enabled = true;

            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
            }
        }

        private void pictureBox3_VisibleChanged(object sender, EventArgs e)
        {
            if (pictureBox3.Visible)
            {
                pictureBox4.Visible = false;
                timer1.Enabled = true;
                timer2.Enabled = true;

            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (pictureBox3.Visible)
            { pictureBox3.Load(Application.StartupPath + "\\..\\Res\\Pic\\SysCommom.MapSheet.NoDoubt0.png"); }
            if (pictureBox4.Visible)
            { pictureBox4.Load(Application.StartupPath + "\\..\\Res\\Pic\\SysCommom.MapSheet.Doubt0.png"); }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            //返回当前的图幅结合表名称
            vSelectMapName = comboBoxScale.Text.Trim();
            
            //重新获得列表中选中的图幅号，因为有可能人工清除部分选中的图幅号
            pMapNumSelected = null;

            ArrayList pMapNumArray=new ArrayList();
            for (int k = 0; k <= this.checkedListBoxNo.Items.Count-1; k++)
            {
                if(this.checkedListBoxNo.GetItemChecked(k))
                {
                    pMapNumArray.Add(this.checkedListBoxNo.Items[k].ToString());
                }

            }
            pMapNumSelected=pMapNumArray.ToArray(typeof(string)) as string[];

            pMapNumArray = null;
            m_Res = true;
            this.Close();
        }
        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton4_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _tool = new ControlsSelectToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 漫游工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton3_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _tool = new ControlsMapPanToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 拉框放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton1_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _tool = new ControlsMapZoomInToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 拉框缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton2_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _tool = new ControlsMapZoomOutToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 前景视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton5_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _cmd = new ControlsMapZoomToLastExtentBackCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 后景视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton6_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _cmd = new ControlsMapZoomToLastExtentForwardCommand();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton10_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _tool = new ControlsSelectFeaturesToolClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(this.MapSheetControl.Object);
            //通知map控件当前工具
            this.MapSheetControl.CurrentTool = _tool;
            _cmd.OnClick();
        }
        /// <summary>
        /// 清除选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton8_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _cmd = new ControlsClearSelectionCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 刷新视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton9_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _cmd = new ControlsMapRefreshViewCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        /// <summary>
        /// 全图浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bubbleButton7_Click(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            _cmd = new ControlsMapFullExtentCommandClass();
            _cmd.OnCreate(this.MapSheetControl.Object);
            _cmd.OnClick();
        }
        
        //把当前的图幅结合表名称传出来   fangmiao  20090702
        private string vSelectMapName = "";
        public string SelectMapName
        {
            get { return vSelectMapName; }
        }

        //传进比例尺信息     fangmiao  20090706
        private string vStrScale = "";
        public string StrScale
        {
            set { vStrScale = value; }
        }

        //第二中方法是遍历判断法   fangmiao  20090707
       
        //添加标注  fangmiao  20090708
        private void AddLabel()
        {
            IGeoFeatureLayer pGeoFeatLayer;
            if (this.MapSheetControl.LayerCount == 0) return;
            pGeoFeatLayer = this.MapSheetControl.get_Layer(this.MapSheetControl.LayerCount - 1) as IGeoFeatureLayer;
            pGeoFeatLayer.DisplayField = "MAP_NEWNO";

            IAnnotateLayerPropertiesCollection pAnnoProps = null;
            pAnnoProps = pGeoFeatLayer.AnnotationProperties;

            ILineLabelPosition pPosition = null;
            pPosition = new LineLabelPositionClass();
            pPosition.Parallel = true;
            pPosition.Above = true;

            ILineLabelPlacementPriorities pPlacement = new LineLabelPlacementPrioritiesClass();
            IBasicOverposterLayerProperties4 pBasic = new BasicOverposterLayerPropertiesClass();
            pBasic.FeatureType = esriBasicOverposterFeatureType.esriOverposterPolyline;
            pBasic.LineLabelPlacementPriorities = pPlacement;
            pBasic.LineLabelPosition = pPosition;
            pBasic.BufferRatio = 0;
            pBasic.FeatureWeight = esriBasicOverposterWeight.esriHighWeight;
            pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;
            //pBasic.PlaceOnlyInsidePolygon = true;//仅在地物内部显示标注  deleted by chulili s20111018 界面上并没有这项设置，这句话应注释掉，否则像是错误

            ILabelEngineLayerProperties pLabelEngine = null;
            pLabelEngine = new LabelEngineLayerPropertiesClass();
            pLabelEngine.BasicOverposterLayerProperties = pBasic as IBasicOverposterLayerProperties;
            pLabelEngine.Expression = "[" + "MAP_NEWNO" + "]";
            pLabelEngine.Symbol.Size = 8;

            IAnnotateLayerProperties pAnnoLayerProps = null;
            pAnnoLayerProps = pLabelEngine as IAnnotateLayerProperties;
            pAnnoLayerProps.LabelWhichFeatures =esriLabelWhichFeatures.esriAllFeatures;
            pAnnoProps.Clear();
            pAnnoProps.Add(pAnnoLayerProps);
                    
            pGeoFeatLayer.DisplayAnnotation =true;
            this.MapSheetControl.ActiveView.Refresh();

        }
        //添加标注
        private void RemoveLabel()
        {
            IGeoFeatureLayer pGeoFeatLayer=null;
            pGeoFeatLayer = this.MapSheetControl.get_Layer(this.MapSheetControl.LayerCount - 1) as IGeoFeatureLayer;
            pGeoFeatLayer.DisplayAnnotation = false;
            this.MapSheetControl.ActiveView.Refresh();
        }
        private void bubbleButton5_Click_1(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            AddLabel();
        }
        //移除标注
        private void bubbleButton6_Click_1(object sender, DevComponents.DotNetBar.ClickEventArgs e)
        {
            RemoveLabel();
        }

    }
}