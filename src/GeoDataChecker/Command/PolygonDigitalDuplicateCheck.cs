using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Data;
namespace GeoDataChecker
{
    /// <summary>
    /// 功能：重复面的检查
    /// 编写人：王冰
    /// </summary>
    class PolygonDigitalDuplicateCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private ILayer m_CurLayer;//定义一个层，用来接收我们指定的对比层
        public PolygonDigitalDuplicateCheck()
        {
            base._Name = "GeoDataChecker.PolygonDigitalDuplicateCheck";
            base._Caption = "面重复数字化检查";
            base._Tooltip = "检查图面中的面要素是否有与之重复的面要素（位置相同、属性相同）";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "面重复数字化检查";
            //base._Image = "";
            //base._Category = "";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    else
                    {
                        if (SetCheckState.CheckState)
                        {
                            base._Enabled = true;
                            return true;
                        }
                        else
                        {
                            base._Enabled = false;
                            return false;
                        }
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Overridfunction.name = "面重复";
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行面重复数字化检查主函数
            ExcutePolygonDigitalDuplicateCheck(_AppHk);
        }



        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 定义一个委托，主要是因为要把它标识为安全的窗体控件
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="AppHk"></param>
        private delegate void Update_data(DataTable tb, Plugin.Application.IAppGISRef AppHk);//定义一个委托

        /// <summary>
        /// 具体的实现委托的方法
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="AppHk"></param>
        private void Bind(DataTable tb, Plugin.Application.IAppGISRef AppHk)
        {
            AppHk.DataCheckGrid.DataSource = tb;//将数据邦定到控件上
            AppHk.DataCheckGrid.Columns[0].Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 80;//调整列宽度
            if (AppHk.DataCheckGrid.DataSource != null)
            {
                AppHk.DataCheckGrid.MouseDoubleClick += new MouseEventHandler(Overridfunction.DataCheckGridDoubleClick);//加入双击事件
            }
        }

        /// <summary>
        /// 进度条委托，并传值
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="step"></param>
        private delegate void ProcessBar(int max, int min, int step, int current, Plugin.Application.IAppFormRef AppHk);
        /// <summary>
        /// 对应委托的方法，进度条
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="step"></param>
        private void ControlProcessBar(int max, int min, int step, int current, Plugin.Application.IAppFormRef AppHk)
        {

            AppHk.ProgressBar.Maximum = max;//最大上限
            AppHk.ProgressBar.Minimum = min;//最低下限
            AppHk.ProgressBar.Step = step;//间值
            AppHk.ProgressBar.Value = current;//当前值

        }
        /// <summary>
        /// 用来是否显示进度条
        /// </summary>
        /// <param name="par"></param>
        private delegate void ShowPrcoessBar(bool par, Plugin.Application.IAppFormRef AppHk);
        /// <summary>
        /// 如果为真就显示进度条，否则不显示，默认不显示
        /// </summary>
        /// <param name="par"></param>
        /// <param name="AppHk"></param>
        private void Show_processBar(bool par, Plugin.Application.IAppFormRef AppHk)
        {
            if (par)
            {
                AppHk.ProgressBar.Visible = true;
            }
            else
            {
                AppHk.ProgressBar.Visible = false;
            }
        }
        
        /// <summary>
        /// 面重复数字化检查主函数
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcutePolygonDigitalDuplicateCheck(Plugin.Application.IAppGISRef _AppHk)
        {


            #region 取得进度条对象
            Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;
            //取得进度条对象
            //选中数据处理树图项
            DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
            if (PanelTip != null)
            {
                PanelTip.DockContainerItem.Selected = true;
            }
            //使用带参的多线程操作
            try
            {
                System.Threading.ParameterizedThreadStart start = new System.Threading.ParameterizedThreadStart(Cover);
                System.Threading.Thread thread = new System.Threading.Thread(start);
                _AppHk.CurrentThread = thread;
                thread.Start(pAppForm);
            }
            catch (Exception ex)
            {
                if (_AppHk.CurrentThread != null)
                    _AppHk.CurrentThread = null;
                SetCheckState.Message(pAppForm, "错误", ex.ToString());
                return;
            }

            #endregion

        }


        /// <summary>
        /// 重复面
        /// </summary>
        private void Cover(object para)
        {
            Plugin.Application.IAppFormRef pAppForm = para as Plugin.Application.IAppFormRef;
            System.Data.DataTable Datatable = new System.Data.DataTable();//手动建立一个数据表，将得到的数据邦定到检查结果当中显示

            ///如果检查结果提示内有内容就清除
            ClearDataCheckGrid ClearGrid = new ClearDataCheckGrid();
            ClearGrid.Operate(pAppForm, _AppHk);
            Datatable.Columns.Add("重复面", typeof(string));//创建一列

            #region 打开要操作的数据集

            int L_count = _AppHk.MapControl.LayerCount;//MAP上总图层数
            if (L_count == 0) return;//当MAP上是空，就返回
            SetCheckState.CheckShowTips(pAppForm,"面重复检查开始......");
            IGeoDataset pGeoDataset = SetCheckState.Geodatabase;
            ITopologyRuleContainer pRuleCont = SetCheckState.pRuleCont;//引入拓扑规则接口对象，将拓扑转成对应的规则接口对象  
            #endregion
            try
            {

                #region 遍历整个数据集要素
                ArrayList list_line = new ArrayList();//用动态的数组来接收满足线要素类的对象，以后面实现遍历拓扑检查
                for (int l = 0; l < L_count; l++)
                {
                    ILayer layer_1 = _AppHk.MapControl.get_Layer(l);//通过索引得到对应的层
                    #region //判别是不是组，如果是，就从组中取一个层
                    if (layer_1 is IGroupLayer && layer_1.Name == SetCheckState.CheckDataBaseName)
                    {
                        ICompositeLayer grouplayer_1 = layer_1 as ICompositeLayer;//把组图层转成组合图层
                        int group_count = grouplayer_1.Count;
                        if (group_count == 0) return;
                        for (int g = 0; g < group_count; g++)
                        {
                            ILayer layer_temp = grouplayer_1.get_Layer(g);
                            IFeatureLayer pFeaturelayer = layer_temp as IFeatureLayer;
                            IFeatureClass pFeatureClass = pFeaturelayer.FeatureClass;
                            #region 将符合线要素的类加入到拓扑中
                            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon && pFeatureClass.FeatureType != esriFeatureType.esriFTAnnotation && pFeatureClass.Search(null, false).NextFeature() != null)
                            {
                                list_line.Add(pFeatureClass);//将符合的要素类加入动态数组当中

                            }
                            #endregion
                        }
                    }
                    #endregion

                }
                #endregion
                int Count = list_line.Count;
                if (Count == 0)
                {
                    SetCheckState.CheckShowTips(pAppForm, "重复面检查完成！");
                    _AppHk.CurrentThread = null;
                    SetCheckState.Message(pAppForm, "提示", "面重复检查完成！");
                    //选中检查出错列表
                    ClearGrid.CheckDataGridShow(pAppForm, _AppHk);
                    return;
                }
                if (Count == 0) return;
                SetCheckState.TreeIni_Fun(list_line, _AppHk);//初始化树图
                SetCheckState.CheckShowTips(pAppForm, "检查数据准备完毕，请稍后........");
                #region 通过过滤后的要素类来进行拓扑检查
                for (int N = 0; N < Count; N++)
                {
                    int num = N + 1;//由于层的索引是从0开始的，所以得加1

                    #region 创建拓扑并验证拓扑
                    IFeatureClass TempClass = list_line[N] as IFeatureClass;//将对象转成相应的要素类
                    IDataset ds = TempClass as IDataset;
                    SetCheckState.TreeCheckFun(ds.Name, N, _AppHk);//调用树节点选中方法
                    ITopologyRule topologyRule = new TopologyRuleClass();//实例一个拓扑规则类
                    topologyRule.TopologyRuleType = esriTopologyRuleType.esriTRTAreaNoOverlap;//使用规则的类型(面不相交)
                    topologyRule.Name = "www";//给规则取个名
                    topologyRule.OriginClassID = TempClass.FeatureClassID;
                    topologyRule.AllOriginSubtypes = true;
                    pRuleCont.AddRule(topologyRule);


                    ISegmentCollection pLocation = new PolygonClass();//使用多边形接口
                    pLocation.SetRectangle(pGeoDataset.Extent);//将我们用来操作验证的要素类利用SETRECTANGLE来构造一个完整的几何形体
                    IPolygon pPoly = SetCheckState.pT.get_DirtyArea(pLocation as IPolygon);
                    IEnvelope pAreaValidated = SetCheckState.pT.ValidateTopology(pPoly.Envelope);//验证拓扑

                    SetCheckState.CheckShowTips(pAppForm, "正在进行拓扑分析，请稍后........");
                    IErrorFeatureContainer pErrFeatCon = SetCheckState.pT as IErrorFeatureContainer;//实例一个拓扑出错的容器
                    IEnumTopologyErrorFeature pEnumTopoErrFeat = pErrFeatCon.get_ErrorFeatures(pGeoDataset.SpatialReference, topologyRule, pGeoDataset.Extent, true, false);//将所有的拓扑出错进行枚举 
                    #endregion

                    ITopologyErrorFeature Topo_ErrFeat = pEnumTopoErrFeat.Next();//开始遍历拓扑错误，表示下一个s
                    ArrayList TempTopList = new ArrayList();
                    //将当前层的所有出错要素放入动态数组当中
                    while (Topo_ErrFeat != null)
                    {
                        TempTopList.Add(Topo_ErrFeat);//将出错的要素存入动态数组
                        Topo_ErrFeat = pEnumTopoErrFeat.Next();
                    }
                    int P_count = TempTopList.Count;//每个层的总出错记录
                    if (P_count == 0)
                    {
                        SetCheckState.CheckShowTips(pAppForm, "图层完好,准备进行下一个图层，请稍后........");
                        continue;
                    }
                    pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { true, pAppForm });//是否显示进度条,加载让它显示
                    #region 遍历出错
                    for (int p = 0; p < P_count; p++)
                    {
                        int i = p + 1;//方便计算
                        pAppForm.MainForm.Invoke(new ProcessBar(ControlProcessBar), new object[] { P_count, 0, 1, i, pAppForm });//给进度条传需要的值
                        ITopologyErrorFeature pTopoErrFeat = TempTopList[p] as ITopologyErrorFeature;
                        int OriginOID = pTopoErrFeat.OriginOID;//源ID
                        int DestinationOID = pTopoErrFeat.DestinationOID;//目标ID
                        IFeature source_fe = TempClass.GetFeature(OriginOID);//源要素
                        IFeature des_fe = TempClass.GetFeature(DestinationOID);//目标要素 
                        #region 通过拓扑出来的错进行再次分析求重复面
                        ///求几何空间差值，必须要两种情况考虑：a->b  b->a，如果同时都为空，说明两个重复
                        ITopologicalOperator Origin_0 = des_fe.Shape as ITopologicalOperator;//空间几何运算
                        IGeometry difference_0 = Origin_0.Difference(source_fe.Shape as IGeometry);//求两个面的差值

                        ITopologicalOperator Origin = source_fe.Shape as ITopologicalOperator;//空间几何运算
                        IGeometry difference = Origin.Difference(des_fe.Shape as IGeometry);//求两个面的差值

                        
                        if (difference.IsEmpty && difference_0.IsEmpty)
                        {
                            #region MyRegion
                            //    StringBuilder origin_temp = new StringBuilder();//除了SHAPE外的字段对比源
                            //    StringBuilder destination_temp = new StringBuilder();//除了SHAPE外的字段对比目标
                            //    //IDataset ds = TempClass as IDataset;
                            //    ArrayList list = Foreach();//调用要对比的XML文件里的字段名
                            //    int count = list.Count;
                            //    #region 通过拓扑出错及对出错的对象再次的进行差值及字段对比，得出最终的结果是否一样
                            //    if (count > 0)
                            //    {
                            //        #region 得到要实际比较的字段值
                            //        for (int n = 0; n < count; n++)
                            //        {
                            //            int index = TempClass.GetFeature(OriginOID).Fields.FindFieldByAliasName(list[n].ToString());//得到源的列ID

                            //            int index1 = TempClass.GetFeature(DestinationOID).Fields.FindFieldByAliasName(list[n].ToString());//得到目标的列ID
                            //            if (index > 0 && index1 > 0)
                            //            {
                            //                origin_temp.Append(TempClass.GetFeature(OriginOID).get_Value(index));//在对应的要素类当中取出源对应ID列的值，加入BUILDER中
                            //                destination_temp.Append(TempClass.GetFeature(DestinationOID).get_Value(index1));//在对应的要素类当中取出目标对应的ID列值，加入目录的BUILDER中
                            //            }
                            //        } 
                            //        #endregion
                            //        #region 比较两个值是否一样
                            //        if (origin_temp.ToString() == destination_temp.ToString() && origin_temp != null && destination_temp != null)//将两个列的值进行对比
                            //        { 
                            #endregion
                            string temp = ds.Name + " ID：" + OriginOID.ToString() + " 目标ID：" + DestinationOID.ToString();
                            System.Data.DataRow Row = Datatable.NewRow();//新创建一个行
                            Row[0] = temp;
                            Datatable.Rows.Add(Row);//将行加入到表数据集中
                            SetCheckState.CheckShowTips(pAppForm, temp);
                            #region
                            //        }
                        //        else
                        //        {
                        //            pTopoErrFeat = pEnumTopoErrFeat.Next();//遍历下一个错误
                        //            continue;
                        //        } 
                        //        #endregion
                        //    }
                        //    else
                        //    {
                        //        string temp = ds.Name + " ID：" + OriginOID.ToString() + " 目标ID：" + DestinationOID.ToString();//将结果组织起来，放到一个字符串当中
                        //        System.Data.DataRow Row = Datatable.NewRow();//新创建一个行
                        //        Row[0] = temp;
                        //        Datatable.Rows.Add(Row);//将行加入到表数据集中
                        //    }
                            #endregion
                        }   
                        #endregion
                    }

                    pRuleCont.DeleteRule(topologyRule);
                    #endregion
                    if (num == Count)
                    {
                        string Case = "马上完成请稍后.....";//状态栏显示
                        SetCheckState.CheckShowTips(pAppForm, Case);
                    }
                }
                #endregion
                pAppForm.MainForm.Invoke(new Update_data(Bind), new object[] { Datatable, _AppHk });//将窗体控件使用委托
                pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                _AppHk.CurrentThread = null;
                SetCheckState.Message(pAppForm,"提示", "面重复检查完成！");
                //选中检查出错列表
                SetCheckState.CheckShowTips(pAppForm, "面重复检查完成！");
                ClearGrid.CheckDataGridShow(pAppForm, _AppHk);

            }
            catch (Exception ex)
            {
                _AppHk.CurrentThread = null;
                SetCheckState.Message(pAppForm, "错误", ex.ToString());
                return;
            }
        }
        /// <summary>
        /// 遍历XML
        /// </summary>
        /// <returns></returns>
        private ArrayList Foreach()
        {
            ArrayList list = new ArrayList();
            XmlDataDocument doc = new XmlDataDocument();
            string path = Application.StartupPath + "\\..\\Res\\checker\\area.xml";
            doc.Load(path);//加载一个xml文件
            XmlNode node = doc.DocumentElement;
            int count = node.ChildNodes.Count;
            if (count != 0)
            {
                for (int n = 0; n < count; n++)
                {
                    list.Add(node.ChildNodes[n].InnerText);
                }
                return list;
            }
            else
            {
                list = null;
                return list;
            }
        }
        /// <summary>
        /// 双击后，通过显示的记录查找出指定层的指定无素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataCheckGridDoubleClick(object sender, System.EventArgs e)
        {
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.SelectedCells[0].Value.ToString();//得到点击行的第一个单元格内容
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID
            int DestID = 0;//目标要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            className = para[0];//名称
            string[] org = para[1].Split(s);//得到源ID串
            OrginID = Convert.ToInt32(org[1]);//源ID
            string[] des = para[2].Split(s);//目标源ID串
            DestID = Convert.ToInt32(des[1]);//目标ID
            IFeature fu = null;//得到要素
            int n = _AppHk.MapControl.Map.LayerCount;
            ISpatialReference pSpatialRef = null;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                    for (int c = 0; c < C_layer.Count; c++)
                    {
                        ILayer temp_layer = C_layer.get_Layer(c);
                        IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                        IDataset set = F_layer.FeatureClass as IDataset;
                        if (className == set.Name)
                        {
                            if (pSpatialRef == null)
                            {
                                pSpatialRef = (set as IGeoDataset).SpatialReference;
                            }
                            m_CurLayer = temp_layer;
                            //IFeatureLayer f_layer = temp_layer as IFeatureLayer;//转成对应的要素层
                            fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                            break;
                        }

                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu, pSpatialRef);//定位到相应的层
            }

        }
    }
}
