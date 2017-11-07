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
    /// 功能：重复点检查
    /// 编写：王冰
    /// </summary>
    class PointDigitalDuplicateCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private ArrayList list = new ArrayList();//用来遍历时，处理重复点过滤
        public PointDigitalDuplicateCheck()
        {
            base._Name = "GeoDataChecker.PointDigitalDuplicateCheck";
            base._Caption = "点重复数字化检查";
            base._Tooltip = "检查图面中的点要素是否有与之重复的点要素（位置相同、属性相同）";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "点重复数字化检查";

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
            Overridfunction.name = "重复点";
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            Plugin.Application.IAppFormRef AppFrom = _AppHk as Plugin.Application.IAppFormRef;
            //选中数据处理树图项
            DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
            if (PanelTip != null)
            {
                PanelTip.DockContainerItem.Selected = true;
            }
            //点重复数字化检查主函数
            try
            {
                System.Threading.ParameterizedThreadStart start = new System.Threading.ParameterizedThreadStart(ExcutePointDigitalDuplicateCheck);
                System.Threading.Thread thread = new System.Threading.Thread(start);
                _AppHk.CurrentThread = thread;
                thread.Start(_AppHk as object);
            }
            catch (Exception ex)
            {
                if (_AppHk.CurrentThread != null) _AppHk.CurrentThread = null;
                SetCheckState.Message(AppFrom, "错误", ex.ToString());
                return;
            }

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
            AppHk.DataCheckGrid.DataSource = tb;
            AppHk.DataCheckGrid.Columns[0].Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 80;
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
        /// 点重复数字化检查主函数
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcutePointDigitalDuplicateCheck(object Hook)
        {
            Plugin.Application.IAppGISRef _AppHk = Hook as Plugin.Application.IAppGISRef;

            #region 取得进度条对象

            //取得进度条对象

            Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;
            #endregion
            System.Data.DataTable Datatable = new System.Data.DataTable();//手动建立一个数据表，将得到的数据邦定到检查结果当中显示
            Datatable.Columns.Add("重复点", typeof(string));//创建一列

            ///如果检查结果提示内有内容就清除
            ClearDataCheckGrid ClearGrid = new ClearDataCheckGrid();
            ClearGrid.Operate(pAppForm, _AppHk);
            //判断图层个数是否为0
            if (_AppHk.MapControl.LayerCount == 0) return;
            SetCheckState.CheckShowTips(pAppForm, "重复点检查开始.....");
            int Count = _AppHk.MapControl.LayerCount;
            for (int i = 0; i < Count; i++)
            {

                ILayer temp_layer = _AppHk.MapControl.get_Layer(i);
                //判别是不是组，如果是，就从组中取一个层
                if (temp_layer is IGroupLayer && temp_layer.Name == SetCheckState.CheckDataBaseName)
                {
                    ICompositeLayer grouplayer = temp_layer as ICompositeLayer;//把组图层转成组合图层
                    int C_count = grouplayer.Count;//组合图层数
                    if (C_count == 0) return;
                    #region 遍历组下面的重复点
                    ArrayList LayerList = new ArrayList();//存要素类
                    //将所有可用的图层存入动态数组
                    for (int c = 0; c < C_count; c++)
                    {
                        //图层转换
                        pAppForm.MainForm.Invoke(new ProcessBar(ControlProcessBar), new object[] { C_count, 0, 1, c, pAppForm });//给进度条传需要的值
                        int num = c + 1;//由于层的索引是从0开始的，所以得加1
                        ESRI.ArcGIS.Carto.ILayer pLayer = grouplayer.get_Layer(c);
                        ESRI.ArcGIS.Carto.IFeatureLayer pFeatLayer = pLayer as ESRI.ArcGIS.Carto.IFeatureLayer;
                        //如果图层不为空，就转成对应的要素类
                        if (pFeatLayer == null) continue;
                        IFeatureClass pFeatureClass = pFeatLayer.FeatureClass;
                        if (pFeatureClass == null) continue;
                        //只取点层
                        if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPoint && pFeatureClass.FeatureType != esriFeatureType.esriFTAnnotation)
                        {
                            LayerList.Add(pFeatureClass);//加入动态数组中
                        }
                    }
                    SetCheckState.TreeIni_Fun(LayerList, _AppHk);//初始化树
                    //重复点检查操作
                    for (int L = 0; L < LayerList.Count; L++)
                    {
                        IFeatureClass pFeatureClass = LayerList[L] as IFeatureClass;
                        RePoint(pFeatureClass, pAppForm, Datatable,L);//开始遍历重复点
                        if (L == LayerList.Count - 1)
                        {
                            SetCheckState.CheckShowTips(pAppForm, "重复点检查马上完成，请稍后...");
                        }
                    }
                    #endregion
                    break;
                }

            }
            pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
            SetCheckState.CheckShowTips(pAppForm, "重复点检查完成！");
            _AppHk.CurrentThread = null;//线程使用完置空

            pAppForm.MainForm.Invoke(new Update_data(Bind), new object[] { Datatable, _AppHk });
            SetCheckState.Message(pAppForm, "提示", "重复点检查完成！");
            //选中检查出错列表
            ClearGrid.CheckDataGridShow(pAppForm, _AppHk);

        }

        /// <summary>
        /// 判断重复点
        /// </summary>
        private void RePoint(IFeatureClass pFeatureclass, Plugin.Application.IAppFormRef pAppForm, System.Data.DataTable Datatable,int index)
        {
            #region point
            try
            {
                #region 读XML内容
                string[] para;
                ArrayList list = new ArrayList();
                list = Foreach();
                if (list != null)
                {
                    int list_count = list.Count;
                    para = new string[list_count];
                    for (int num = 0; num < list_count; num++)
                    {
                        para[num] = list[num].ToString();
                    }
                }
                else
                {
                    para = new string[0];
                }
                #endregion
                list = null;
                Hashtable Foreach_hs = new Hashtable();//用一个哈希来遍历重复
                IFeatureClass pFeatureClass = pFeatureclass;//要素类
                IDataset set = pFeatureClass as IDataset;
                SetCheckState.TreeCheckFun(set.Name, index, _AppHk);//选中节点
                //开始遍历
                #region 遍历要素类
                IFeatureCursor cour = pFeatureClass.Search(null, false);
                IFeature fu = cour.NextFeature();
                ArrayList FeatureList = new ArrayList();//存要素动态数组
                //将当前要素类里的所有要素存入动态数组
                while (fu != null)
                {
                    FeatureList.Add(fu);
                    fu = cour.NextFeature();
                }

                //释放cursor 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cour);

                //如果为空，就返回
                int FeatureCount = FeatureList.Count;
                if (FeatureCount == 0)
                {
                    pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                    SetCheckState.CheckShowTips(pAppForm, "图层完好，无重复点！");
                    return;
                }
                #region 遍历要素类里的重复点
                pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { true, pAppForm });//是否显示进度条,加载让它显示
                for (int F = 0; F < FeatureCount; F++)
                {
                    fu = FeatureList[F] as IFeature;
                    int i = F + 1;//方便计算
                    pAppForm.MainForm.Invoke(new ProcessBar(ControlProcessBar), new object[] { FeatureCount, 0, 1, i, pAppForm });//给进度条传需要的值
                    #region 总体上得到一个比较值，然后加入到LIST当中
                    IPoint point = fu.Shape as IPoint;
                    double x = point.X;
                    double y = point.Y;
                    string temp = x.ToString() + "," + y.ToString();
                    #region 从XML里提取要比较字段
                    int C = para.Length;
                    if (C > 0)
                    {
                        for (int n = 0; n < C; n++)
                        {
                            int ID = fu.Fields.FindField(para[n]);//得到OID
                            if (ID >= 0)
                            {
                                string content = fu.get_Value(ID).ToString();//得到对应字段的值
                                temp = temp + "," + content;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    #endregion
                    #endregion
                    string soruce = set.Name + " 源OID：" + fu.OID.ToString();
                    if (Foreach_hs == null)
                    {

                        Foreach_hs.Add(temp, soruce);

                    }
                    else
                    {
                        if (!Foreach_hs.Contains(temp))
                        {
                            Foreach_hs.Add(temp, soruce);
                        }
                        else
                        {
                            string values = Foreach_hs[temp].ToString();//取得源重复的OID和NAME

                            string temps = values + " 重复的OID：" + fu.OID.ToString();//组合信息
                            System.Data.DataRow Row = Datatable.NewRow();//新创建一个行
                            Row[0] = temps;
                            Datatable.Rows.Add(Row);//将行加入到表数据集中
                            SetCheckState.CheckShowTips(pAppForm, temps);

                        }
                    }
                } 
                #endregion

                SetCheckState.CheckShowTips(pAppForm, "正在进行下一个图层操作，请稍后....");
                #endregion
            }
            catch (Exception ex)
            {
                _AppHk.CurrentThread = null;
                MessageBox.Show(ex.ToString());
                return;
            }
            #endregion

        }
        /// <summary>
        /// 遍历XML
        /// </summary>
        /// <returns></returns>
        private ArrayList Foreach()
        {
            ArrayList list = new ArrayList();
            XmlDataDocument doc = new XmlDataDocument();
            string path = Application.StartupPath + "\\..\\Res\\checker\\point.xml";//通过获取程序的运行路径再返到上层得到我们所需要的XML路径
            doc.Load(path);//加载XML文件
            XmlNode node = doc.DocumentElement;//得到XML的节点
            int count = node.ChildNodes.Count;
            if (count != 0)
            {
                for (int n = 0; n < count; n++)
                {
                    list.Add(node.ChildNodes[n].InnerText);//读取XML节点的内容
                }
                return list;
            }
            else
            {
                list = null;
                return list;
            }
        }
    }
}
