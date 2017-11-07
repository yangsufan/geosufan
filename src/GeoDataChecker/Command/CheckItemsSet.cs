using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using System.Collections;
namespace GeoDataChecker
{
    /// <summary>
    /// 开启检查操作
    /// 编写人:王冰
    /// </summary>
    class CheckItemsSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        Plugin.Application.IAppFormRef pAppFormRef;
        public bool OpenState = false;//确定是否点击过开启检查
        ArrayList list;//存库体
        public CheckItemsSet()
        {
            base._Name = "GeoDataChecker.CheckItemsSet";
            base._Caption = "开启检查";
            base._Tooltip = "开启检查";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "开启检查";
            //base._Image = "";
            //base._Category = "";
        }

        /// <summary>
        /// 图层中存在数据时才变为可用

        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {

                    ///首先判断编辑是否打开，如果打开了，检查将不可用
                    if (CheckEditWorkspace() == 0)
                    {
                        #region MyRegion
                        int state = 0;//状态
                        list = ControlState(_AppHk.MapControl);//调用可以库体的状态方法
                        if (list.Count > 0)
                        {
                            state = 1;
                            #region 当地图控件上有我们可以检查的数据库，就高亮
                            Plugin.Application.IAppGISRef apphk = _AppHk as Plugin.Application.IAppGISRef;//转换对象,使它转换后能得到我们所需要的当前操作线程
                            //判别当前线程是不是空的,是才进行,不是则显示为不可用.
                            if (apphk.CurrentThread == null)
                            {
                                //如果开启过检查并存在我们检查的数据，就让检查可用
                                if (OpenState && state == 1)
                                {

                                    SetCheckState.CheckState = true;
                                    return true;
                                }
                                else
                                {

                                    //如果没有开启过检查，我们就判别有没有满足我们检查的数据
                                    if (state == 1)
                                    {
                                        base._Enabled = true;
                                        return true;
                                    }
                                    else
                                    {
                                        ClearCheckDataGridView(apphk);
                                        SetCheckState.CheckState = false;
                                        OpenState = false;
                                        base._Enabled = false;
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                SetCheckState.CheckState = false;//如果有线程在操作,则将检查状态为不可用
                                return false;
                            }
                            #endregion
                        }
                        else
                        {
                            base._Checked = false;
                            SetCheckState.CheckState = false;
                            OpenState = false;
                            return false;
                        }
                        #endregion
                    }
                    //如果编辑打开了，并且在检查功能里没有使用过编辑
                    else if (SetCheckState.GeoCor == false)
                    {
                        base._Checked = false;
                        SetCheckState.CheckState = false;
                        OpenState = false;
                        return false;
                    }
                    //在检查功能当中开启过编辑 因为拓扑修正时用到过编辑
                    else
                    {
                        base._Checked = true;
                        SetCheckState.CheckState = false;
                        return false;
                    }

                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// 当前版本我们确定可以检查的有更新修编数据、工作库、现势库,分别存在的可能性有：三种同时存在，二种，一种等。
        /// 规定：检查时，只能选择一种进行，即如果工作库和现势库同时存在，那么须选择一个进行检查,因些，将地图上拥有的图层库体列出来
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private ArrayList ControlState(ESRI.ArcGIS.Controls.IMapControlDefault map)
        {
            ArrayList list = new ArrayList();//用来存当前可被查询的库体有那些
            int Count = map.LayerCount;//总图层数
            for (int n = 0; n < Count; n++)
            {
                ILayer layer = map.Map.get_Layer(n);
                string LayerName = layer.Name;//层名
                if (LayerName == "更新修编数据" || LayerName == "工作库数据" || LayerName == "现势库数据" && layer is IGroupLayer)
                {
                    list.Add(LayerName);
                }

            }
            return list;
        }
        /// <summary>
        /// 清除检查的出错显示栏
        /// </summary>
        /// <param name="AppHk"></param>
        private void ClearCheckDataGridView(Plugin.Application.IAppGISRef AppHk)
        {
            if (AppHk.DataCheckGrid.Rows.Count > 0)
            {
                AppHk.DataCheckGrid.DataSource = null;
            }
        }
        public override string Message
        {
            get
            {
                pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        /// <summary>
        /// 检查开启入口
        /// </summary>
        public override void OnClick()
        {

            SetCheckState.pT = null;
            SetCheckState.Geodatabase = null;//设置一个集合
            SetCheckState.pRuleCont = null;//一个拓扑规则检查的集合容器
            SetCheckState.CheckDataBaseName = "";
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            base._Checked = true;
            //高亮显示处理结果栏
            DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
            PanelTip.DockContainerItem.Selected = true;
            Overridfunction._AppHk = _AppHk;
            int ListCount = list.Count;
            if (ListCount > 0)
            {
                if (ListCount > 1)
                {
                    frmCheckDataBase ShowCheck = new frmCheckDataBase(list);
                    ShowCheck.ShowDialog();
                }
                else if (ListCount == 1)
                {
                    SetCheckState.CheckDataBaseName = list[0].ToString();
                }
                if (SetCheckState.CheckDataBaseName == "")
                {
                    base._Checked = false;
                    return;
                }
                SetCheckTopo();//设置拓扑
                OpenState = true;
                SetCheckState.CheckState = true;//如果加载的数据符合我们要检查的数据要求，我们就改变状态
            }
            else
            {
                SetCheckState.Message(pAppFormRef, "提示", "请加载数据进行检查！");
            }



        }
        /// <summary>
        /// 判断编辑操作是否打开
        /// </summary>
        private int CheckEditWorkspace()
        {

            int state = 0;//初始为没开启
            int count = _AppHk.MapControl.Map.LayerCount;//总层数
            for (int n = 0; n < count; n++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(n);//得到一个层
                if (layer is IGroupLayer && layer.Name == SetCheckState.CheckDataBaseName)
                {
                    ICompositeLayer Com_layer = layer as ICompositeLayer;
                    IFeatureLayer F_layer = Com_layer.get_Layer(0) as IFeatureLayer;
                    IFeatureClass F_class = F_layer.FeatureClass;
                    IWorkspaceEdit space = F_class.FeatureDataset.Workspace as IWorkspaceEdit;
                    if (space.IsBeingEdited())
                    {
                        state = 1;//确定为开启状态
                        break;
                    }
                }
            }
            return state;

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 设置检查用到的拓扑
        /// </summary>
        private void SetCheckTopo()
        {
            #region 从MAP上获取其要素操作空间及数据集名称
            string FeatureDataset_Name = "";//要素数据集的名字
            IFeatureWorkspace pFeatureWorkspace;//定义要素操作空间
            ///通过MAP上的图层获取我们所需要的数据集
            ILayer layer = null;//定义用来接收指定组下面的图层
            int L_count = _AppHk.MapControl.LayerCount;//MAP上总图层数
            if (L_count == 0) return;//当MAP上是空，就返回
            for (int G = 0; G < L_count; G++)
            {
                ILayer temp_layer = _AppHk.MapControl.get_Layer(G);
                //判别是不是组，如果是，就从组中取一个层
                if (temp_layer is IGroupLayer && temp_layer.Name == SetCheckState.CheckDataBaseName)
                {
                    ICompositeLayer grouplayer = temp_layer as ICompositeLayer;//把组图层转成组合图层
                    if (grouplayer.Count == 0) return;
                    layer = grouplayer.get_Layer(0);//得到MAP中其中一个层
                    break;
                }
            }
            IFeatureLayer f_layer = layer as IFeatureLayer;
            IFeatureClass cl = f_layer.FeatureClass;//得到对应的要素类
            if (cl.FeatureDataset == null)
            {
                SetCheckState.Message(pAppFormRef, "提示", "您所操作的数据是离散的，请先做数据预处理！");
                return;
            }
            else
            {
                pFeatureWorkspace = cl.FeatureDataset.Workspace as IFeatureWorkspace;
                FeatureDataset_Name = cl.FeatureDataset.Name;//得到要素类的数据集名称
            }
            #endregion
            IFeatureDataset pfd = pFeatureWorkspace.OpenFeatureDataset(FeatureDataset_Name);//打开一个DATASET数据集

            IGeoDataset pGeoDataset = pfd as IGeoDataset;
            if (pGeoDataset != null)
            {
                SetCheckState.Geodatabase = pGeoDataset;
            }
            ITopologyContainer pTc = pfd as ITopologyContainer;//转成创建拓扑需要的对应对象接口 
            #region 创建拓扑，后面的检查都使用这个拓扑
            if (SetCheckState.pT == null)
            {
                ITopology pT_temp = null;//定义一个临时的拓扑
                try
                {
                    if (SetCheckState.CheckDataBaseName == "更新修编数据" || SetCheckState.CheckDataBaseName == "工作库数据")
                    {
                        pT_temp = pTc.get_TopologyByName("Rule");//查找拓扑
                    }
                    else if (SetCheckState.CheckDataBaseName == "现势库数据")
                    {
                        pT_temp = pTc.get_TopologyByName("Rule1");//查找拓扑
                    }
                    if (pT_temp != null)
                    {
                        SetCheckState.pT = pT_temp;//如果拓扑已存在，我们就把原先的拓扑赋值给定义的全局拓扑
                        SetCheckState.pRuleCont = SetCheckState.pT as ITopologyRuleContainer;

                    }
                }
                catch
                {
                    if (SetCheckState.CheckDataBaseName == "更新修编数据" || SetCheckState.CheckDataBaseName == "工作库数据")
                    {
                        SetCheckState.pT = pTc.CreateTopology("Rule", pTc.DefaultClusterTolerance, -1, "");//创建了个名叫Rule的拓扑。
                    }
                    else if (SetCheckState.CheckDataBaseName == "现势库数据")
                    {
                        SetCheckState.pT = pTc.CreateTopology("Rule1", pTc.DefaultClusterTolerance, -1, "");//创建了个名叫Rule的拓扑。
                    }
                    SetCheckState.pRuleCont = SetCheckState.pT as ITopologyRuleContainer;
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
                                #region 将所有的要素类加入到拓扑中
                                if (pFeatureClass.Search(null, false).NextFeature() == null || pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation) continue;
                                SetCheckState.pT.AddClass(pFeatureClass, 5, 1, 1, false);//将要验证的要线素类放入拓扑中。
                                #endregion
                            }
                            break;
                        }
                        #endregion
                    }
                }
            }
            #endregion

        }

    }
}
