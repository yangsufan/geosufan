using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Collections;
namespace GeoDataChecker
{
    class CodeLayerCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        public CodeLayerCheck()
        {
            base._Name = "GeoDataChecker.CodeLayerCheck";
            base._Caption = "代码图层检查";
            base._Tooltip = "检查加载的图层中要素是否处在正确的图层中";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "代码图层检查";
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

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            Overridfunction.name = "代码图层";
            //执行代码图层检查处理

            ExcuteCodeLayerCheck(_AppHk);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

        }

        private void ExcuteCodeLayerCheck(Plugin.Application.IAppGISRef AppHk)
        {
            #region 代码标准化图层检查主程序

            try
            {
                //高亮显示处理结果栏
                DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
                PanelTip.DockContainerItem.Selected = true;

                Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;

                System.Threading.ParameterizedThreadStart start = new System.Threading.ParameterizedThreadStart(CheckLayer);
                System.Threading.Thread thread = new System.Threading.Thread(start);
                _AppHk.CurrentThread = thread;
                thread.Start(pAppForm);//调用图层标准检查方法
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
        /// 定义一个委托，主要是因为要把它标识为安全的窗体控件 用来邦定数据显示
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="AppHk"></param>
        private delegate void Update_data(DataTable tb, Plugin.Application.IAppGISRef AppHk);//定义一个委托

        /// <summary>
        /// 具体的实现委托的方法 邦定数据并邦定双击事件
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
        /// 代码图层标准化检查
        /// </summary>
        private void CheckLayer(object para)
        {
            Plugin.Application.IAppFormRef pAppForm = para as Plugin.Application.IAppFormRef;
            System.Data.DataTable Datatable = new System.Data.DataTable();//手动建立一个数据表，将得到的数据邦定到检查结果当中显示
            Datatable.Columns.Add("错误图层及相关要素", typeof(string));//创建一列
            ///如果检查结果提示内有内容就清除
            ClearDataCheckGrid ClearGrid = new ClearDataCheckGrid();
            ClearGrid.Operate(pAppForm, _AppHk);

            int count = _AppHk.MapControl.Map.LayerCount;//得以MAP上图层的总数

            if (count == 0) return;//图层为0，返回
            SetCheckState.CheckShowTips(pAppForm, "代码图层标准检查开始.....");
            for (int n = 0; n < count; n++)
            {
                
                //遍历整个MAP上的图层 
                ILayer temp_layer = _AppHk.MapControl.get_Layer(n);

                #region //判别是不是组，如果是，就从组中取一个层
                if (temp_layer is IGroupLayer)
                {
                    ICompositeLayer grouplayer = temp_layer as ICompositeLayer;//把组图层转成组合图层
                    int G_count = grouplayer.Count;//组合图层的层数
                    if (G_count == 0) return;
                    string Value_temp = G_count.ToString();

                    #region 初始化树
                    ArrayList LayerList = new ArrayList();//用来初始化树图的动态数组
                    //将所有不为空的要素类放入数组中
                    for (int L = 0; L < G_count; L++)
                    {
                        ILayer layer = grouplayer.get_Layer(L);////定义用来接收指定组下面的图层
                        IFeatureLayer F_layer = layer as IFeatureLayer;//转成要素层
                        if (F_layer == null) continue;//如果图层为空则返回
                        IFeatureClass F_class = F_layer.FeatureClass;//得到相应的要素类
                        if (F_class == null) continue;
                        LayerList.Add(F_class);//将要素类放入动态数组
                    }
                    SetCheckState.TreeIni_Fun(LayerList, _AppHk);//初始化树 
                    #endregion

                    for (int G = 0; G < LayerList.Count; G++)
                    {
                        
                        ArrayList FeatureList = new ArrayList();//存放每个要素类下面的所有要素
                        int g = G + 1;//由于层的索引是从0开始的，所以得加1
                        IFeatureClass F_class = LayerList[G] as IFeatureClass;//得到相应的要素类
                        IDataset set = F_class as IDataset;//将要素类恩成数据集，以用来取得名称
                        SetCheckState.TreeCheckFun(set.Name, G, _AppHk);

                        #region 当前层共有多少要素
                        IFeatureCursor Cursor = F_class.Search(null, false);//提供游标，用来遍历要素 
                        IFeature Feature = Cursor.NextFeature();//开始遍历要素,取出下一个要素，这里是第一次，所以指第一个
                        while (Feature != null)
                        {
                            FeatureList.Add(Feature);
                            Feature = Cursor.NextFeature();//下一个要素
                        } 

                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(Cursor);

                        #endregion
                        if (FeatureList.Count == 0)
                        {
                            pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
                            SetCheckState.CheckShowTips(pAppForm, "空图层！");
                            continue;
                        }
                        #region 图层标准化检查
                        pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { true, pAppForm });//是否显示进度条,加载让它显示
                        for(int F = 0;F < FeatureList.Count;F++)
                        {
                            pAppForm.MainForm.Invoke(new ProcessBar(ControlProcessBar), new object[] { FeatureList.Count, 0, 1, F+1, pAppForm });//给进度条传需要的值
                            IFeature FeatureTemp = FeatureList[F] as IFeature;
                            int index = -1;
                            for (int f = 0; f < FeatureTemp.Fields.FieldCount; f++)
                            {
                                string code = FeatureTemp.Fields.get_Field(f).Name.ToLower();//将字段名换成小写
                                if (code == "code")
                                {
                                    index = f;
                                    break;
                                }
                            }
                            if (index >= 0)//必须确定表中有CODE字段列，如果没有就不执行
                            {
                                #region 图层标准化查询
                                try
                                {
                                    string value = FeatureTemp.get_Value(index).ToString();//根据列索引得到对应的值
                                    
                                    string name = set.Name;//取得要素类的名称
                                    int ret = BindAccess_Layer(value, name);//将要素得到的列值进行数据库查询
                                    if (ret == -1)
                                    {
                                        string content = name + " OID：" + FeatureTemp.OID.ToString();
                                        System.Data.DataRow Row = Datatable.NewRow();
                                        Row[0] = content;
                                        Datatable.Rows.Add(Row);
                                        SetCheckState.CheckShowTips(pAppForm,content);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _AppHk.CurrentThread = null;//如果中间出错，就结束线程
                                    MessageBox.Show(ex.ToString());
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                break;//由于要素类的结构列是一样的，一条要素找不到对应的列，其它的也一样，所以跳出这个要素类的遍历
                            }

                        }

                        if (G == LayerList.Count)
                        {
                            SetCheckState.CheckShowTips(pAppForm, "马上完成，请稍后...");
                        }
                        #endregion
                        
                    }
                    break;
                } 
                #endregion
            }
            pAppForm.MainForm.Invoke(new Update_data(Bind), new object[] { Datatable, _AppHk });
            pAppForm.MainForm.Invoke(new ShowPrcoessBar(Show_processBar), new object[] { false, pAppForm });//是否显示进度条,加载让它显示
            _AppHk.CurrentThread = null;
            SetCheckState.CheckShowTips(pAppForm, "代码图层标准检查完成！");
            SetCheckState.Message(pAppForm,"提示", "图层标准化检查完成！");
            //选中检查出错列表
            ClearGrid.CheckDataGridShow(pAppForm, _AppHk);
        }

        /// <summary>
        /// 代码图层标准化检查，检索MDB
        /// </summary>
        private int BindAccess_Layer(string code, string name)
        {
            string path = Application.StartupPath + "\\..\\Res\\checker\\GeoTemplate.mdb";
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + "");//创建数据库连接
            string sql = "select GeoCode from GeoCheckerDwgTemplate where GeoCode='" + code + "' and 图层='" + name + "'";//数据库语句,查询有没有对应的CODE
            OleDbCommand cmd = new OleDbCommand(sql, con);//执行数据查询语句
            OleDbDataAdapter Datapter = new OleDbDataAdapter(cmd);//适配器
            DataSet ds = new DataSet();
            Datapter.Fill(ds);
            int count = ds.Tables[0].Rows.Count;//得到查询的行数
            int temp = 0;//临时过渡的变量
            if (count == 0)
            {
                temp = -1;//表示没有查询到
            }
            else if (count > 0)
            {
                temp = 1;//表示查询到了
            }
            return temp;//返回最终的结果
        }

    }
}
