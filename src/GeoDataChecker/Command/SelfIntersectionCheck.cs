﻿using System;
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
    /// 功能：自相交的线检查
    /// 编写人：王冰
    /// </summary>
    class SelfIntersectionCheck : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef _AppHk;
        public SelfIntersectionCheck()
        {
            base._Name = "GeoDataChecker.SelfIntersectionCheck";
            base._Caption = "自相交检查";
            base._Tooltip = "检查加载的线要素中是否存在自相交的要素";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "自相交检查";

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
            Overridfunction.name = "自交线";
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行自相交检查

            ExcuteSelfIntersectionCheck(_AppHk);
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
        /// 自相交检查主程序
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcuteSelfIntersectionCheck(Plugin.Application.IAppGISRef _AppHk)
        {
            #region 取得进度条对象
            Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;
            //取得进度条对象
            try
            {
                //高亮显示处理结果栏
                DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
                PanelTip.DockContainerItem.Selected = true;

                System.Threading.ParameterizedThreadStart start = new System.Threading.ParameterizedThreadStart(Line_self);
                System.Threading.Thread thread = new System.Threading.Thread(start);
                _AppHk.CurrentThread = thread;//得到当前的进程
                thread.Start(pAppForm);
            }
            catch (Exception ex)
            {
                if (_AppHk.CurrentThread != null) _AppHk.CurrentThread = null;
                
                MessageBox.Show(ex.ToString());
                return;
            }
            #endregion

        }

        /// <summary>
        /// 自相交的线
        /// </summary>
        private void Line_self(object para)
        {
            Plugin.Application.IAppFormRef pAppForm = para as Plugin.Application.IAppFormRef;
            System.Data.DataTable Datatable = new System.Data.DataTable();//手动建立一个数据表，将得到的数据邦定到检查结果当中显示

            Datatable.Columns.Add("自交线", typeof(string));//创建一列
            ///如果检查结果提示内有内容就清除
            ClearDataCheckGrid ClearGrid = new ClearDataCheckGrid();
            ClearGrid.Operate(pAppForm, _AppHk);
            #region 打开要操作的数据集

            if (_AppHk.MapControl.LayerCount == 0) return;//如果MAP上打开的图层为空，就返回
            SetCheckState.CheckShowTips(pAppForm, "自交线检查开始.....");
           
            int L_count = _AppHk.MapControl.LayerCount;//MAP上总图层数
            if (L_count == 0) return;//当MAP上是空，就返回

            IGeoDataset pGeoDataset = SetCheckState.Geodatabase;
            ITopologyRuleContainer pRuleCont = SetCheckState.pT as ITopologyRuleContainer;//引入拓扑规则接口对象，将拓扑转成对应的规则接口对象
            #endregion
            try
            {

                #region 遍历整个数据集要素
                ArrayList list_line = new ArrayList();//用动态的数组来接收满足线要素类的对象，以后面实现遍历拓扑检查
                for (int l = 0; l < L_count; l++)
                {
                    ILayer layer_1 = _AppHk.MapControl.get_Layer(l);//通过索引得到对应的层
                    #region //判别是不是组，如果是，就从组中取一个层
                    if (layer_1 is IGroupLayer)
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
                            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline && pFeatureClass.FeatureType != esriFeatureType.esriFTAnnotation && pFeatureClass.Search(null, false).NextFeature() != null)
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
                if (Count == 0) return;
                SetCheckState.TreeIni_Fun(list_line, _AppHk);//初始化树图
                #region 通过过滤后的要素类来进行拓扑检查
                for (int N = 0; N < Count; N++)
                {

                    int num = N + 1;//由于层的索引是从0开始的，所以得加1
                    

                    IFeatureClass TempClass = list_line[N] as IFeatureClass;//将对象转成相应的要素类
                    IDataset ds_line = TempClass as IDataset;
                    SetCheckState.TreeCheckFun(ds_line.Name, N, _AppHk);
                    ITopologyRule topologyRule = new TopologyRuleClass();//实例一个拓扑规则类
                    topologyRule.TopologyRuleType = esriTopologyRuleType.esriTRTLineNoSelfIntersect;//使用规则的类型(自相交)
                    topologyRule.Name = "www";//给规则取个名
                    topologyRule.OriginClassID = TempClass.FeatureClassID;
                    topologyRule.AllOriginSubtypes = true;
                    pRuleCont.AddRule(topologyRule);


                    ISegmentCollection pLocation = new PolygonClass();//使用多边形接口
                    pLocation.SetRectangle(pGeoDataset.Extent);//将我们用来操作验证的要素类利用SETRECTANGLE来构造一个完整的几何形体
                    IPolygon pPoly = SetCheckState.pT.get_DirtyArea(pLocation as IPolygon);
                    IEnvelope pAreaValidated = SetCheckState.pT.ValidateTopology(pPoly.Envelope);

                    IErrorFeatureContainer pErrFeatCon = SetCheckState.pT as IErrorFeatureContainer;//实例一个拓扑出错的容器
                    IEnumTopologyErrorFeature pEnumTopoErrFeat = pErrFeatCon.get_ErrorFeatures(pGeoDataset.SpatialReference, topologyRule, pGeoDataset.Extent, true, false);//将所有的拓扑出错进行枚举
                    ITopologyErrorFeature Topo_ErrFeat = pEnumTopoErrFeat.Next();
                    ArrayList TempTopList = new ArrayList();
                    while (Topo_ErrFeat != null)
                    {
                        TempTopList.Add(Topo_ErrFeat);//将出错的要素存入动态数组
                        Topo_ErrFeat = pEnumTopoErrFeat.Next();
                    }
                    #region 遍历出错
                    int P_count = TempTopList.Count;//每个层的总出错记录
                    if (P_count == 0)
                    {
                        pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                        SetCheckState.CheckShowTips(pAppForm, "图层完好，无自交线！");
                        continue;
                    } 
                    pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { true, pAppForm });//是否显示进度条,加载让它显示
                    for (int p = 0; p < P_count; p++)
                    { 
                        int i = p + 1;//方便计算
                        pAppForm.MainForm.Invoke(new ProcessBar(ControlProcessBar), new object[] { P_count, 0, 1, i, pAppForm });//给进度条传需要的值
                        ITopologyErrorFeature pTopoErrFeat = TempTopList[p] as ITopologyErrorFeature;
                        int OriginOID = pTopoErrFeat.OriginOID;//源ID
                       
                        if (i % 2 != 0)
                        {
                            string temp = ds_line.Name + " 自相交线ID：" + OriginOID.ToString();
                            System.Data.DataRow Row = Datatable.NewRow();//创建一个行
                            Row[0] = temp;//对行里的列索引赋值
                            Datatable.Rows.Add(Row);//将行加入到表数据集当中
                             SetCheckState.CheckShowTips(pAppForm, temp);
                        }

                    }
                    #endregion

                    pRuleCont.DeleteRule(topologyRule);//删除规则
                    if (num == Count)
                    {
                        SetCheckState.CheckShowTips(pAppForm, "马上完成，请稍后......");//状态栏显示
                    }
                }
                SetCheckState.CheckShowTips(pAppForm, "自相交线检查完成！");
                pAppForm.MainForm.Invoke(new Update_data(Bind), new object[] { Datatable, _AppHk });//将窗体控件使用委托
                pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                _AppHk.CurrentThread = null;
                SetCheckState.Message(pAppForm, "提示", "自相交检查完成！");
                //选中检查出错列表
                ClearGrid.CheckDataGridShow(pAppForm, _AppHk);

                #endregion

            }
            catch (Exception ex)
            {
                _AppHk.CurrentThread = null;
                if (pAppForm.ProgressBar.Visible)
                {
                    pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                }
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
