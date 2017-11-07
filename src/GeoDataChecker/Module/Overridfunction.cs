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
using ESRI.ArcGIS.Display;

namespace GeoDataChecker
{
    /// <summary>
    /// 给双击事件的委托事件定义不同的执行方法，以实现不同功能下的定位操作
    /// 编写人：王冰
    /// </summary>
    public class Overridfunction
    {
        public static Plugin.Application.IAppGISRef _AppHk;
        static ILayer m_CurLayer = null;//共用一个层的变量
        public static string name = "";
        public static void DataCheckGridDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (view.RowCount == 0) return;
            if (view.SelectedCells.Count == 0) return;
            switch (name)
            {
                case "自交线":
                    LineSelf(sender);
                    break;
                case "线重复":
                    DoubleLine(sender);
                    break;
                case "重复点":
                    DoublePoint(sender);
                    break;
                case "面重复":
                    DoublePolygon(sender);
                    break;
                case "代码图层":
                    CodeLayer(sender);
                    break;
                case "代码标准":
                    Code(sender);
                    break;
                case "悬挂点":
                    HangPoint_click(sender);
                    break;
                case "接边检查":
                    JoinCheck(sender);
                    break;

            }
        }

        /// <summary>
        /// 双击事件，双击后，定位到指定的层要素 NAME+“：”+OID+点 接边检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void JoinCheck(object sender)
        {
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.CurrentRow.Cells[0].Value.ToString();//得到点击行的第一个单元格内容
            int DesID = Convert.ToInt32(view.CurrentRow.Cells[1].Value.ToString());//选定行的第二个单元格
            IPoint pErrPnt = view.CurrentRow.Cells[2].Value as IPoint;
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            string[] org = para[0].Split(s);//得到源ID串
            className = org[0];
            OrginID = Convert.ToInt32(org[1]);

            IFeature fu = null;//得到源要素
            IFeature Des_feature = null;//目标要素
            int n = _AppHk.MapControl.Map.LayerCount;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                Des_feature = F_layer.FeatureClass.GetFeature(DesID);//目标要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null && Des_feature != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, Des_feature);//在对应的层上选择指定的元素
                //SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层

                ITopologicalOperator pTop = pErrPnt as ITopologicalOperator;
                IGeometry pGeometry = pTop.Buffer(30);
                IEnvelope pEnvelope = pGeometry.Envelope;

                if (pEnvelope == null) return;
                pEnvelope.Expand(12, 0, false);
                IActiveView pActiveView = _AppHk.MapControl.Map as IActiveView;
                pActiveView.Extent = pEnvelope;
                pActiveView.Refresh();
                Application.DoEvents();
                //画出对应的点
                if (pErrPnt != null)
                {
                    _AppHk.MapControl.ActiveView.ScreenDisplay.StartDrawing(_AppHk.MapControl.ActiveView.ScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
                    esriSimpleMarkerStyle pStyle = esriSimpleMarkerStyle.esriSMSCircle;
                    _AppHk.MapControl.ActiveView.ScreenDisplay.SetSymbol(SetSnapSymbol(pStyle) as ISymbol);
                    _AppHk.MapControl.ActiveView.ScreenDisplay.DrawPoint(pErrPnt);
                    _AppHk.MapControl.ActiveView.ScreenDisplay.FinishDrawing();
                }
            }
        }

        //设置画捕捉时候的符号
        private static ISimpleMarkerSymbol SetSnapSymbol(esriSimpleMarkerStyle pStyle)
        {
            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
            ISymbol pSymbol = pMarkerSymbol as ISymbol;

            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Transparency = 0;
            //采用异或方式绘制，擦除以前画的符号
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            pMarkerSymbol.Color = pRgbColor;
            pMarkerSymbol.Style = pStyle;

            //设置轮廓线样式
            pRgbColor.Red = 255;
            pRgbColor.Blue = 0;
            pRgbColor.Green = 0;
            pRgbColor.Transparency = 230;
            pMarkerSymbol.Outline = true;
            pMarkerSymbol.OutlineColor = pRgbColor;
            pMarkerSymbol.OutlineSize = 1;
            pMarkerSymbol.Size = 12;

            return pMarkerSymbol;
        }
        /// <summary>
        /// 线自相交
        /// </summary>
        /// <param name="sender"></param>
        private static void LineSelf(object sender)
        {
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.SelectedCells[0].Value.ToString();//得到点击行的第一个单元格内容
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            className = para[0];//名称
            string[] org = para[1].Split(s);//得到源ID串
            OrginID = Convert.ToInt32(org[1]);//源ID
            IFeature fu = null;//得到要素
            int n = _AppHk.MapControl.Map.LayerCount;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                //IFeatureLayer f_layer = temp_layer as IFeatureLayer;//转成对应的要素层
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素

                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }
        }
        /// <summary>
        /// 线重复
        /// </summary>
        /// <param name="sender"></param>
        private static void DoubleLine(object sender)
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
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {

                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }

        }
        /// <summary>
        /// 点重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DoublePoint(object sender)
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
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {

                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }


        }
        /// <summary>
        /// 面重复
        /// </summary>
        /// <param name="sender"></param>
        private static void DoublePolygon(object sender)
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
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                //IFeatureLayer f_layer = temp_layer as IFeatureLayer;//转成对应的要素层
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }

        }
        /// <summary>
        /// 代码图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CodeLayer(object sender)
        {
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.SelectedCells[0].Value.ToString();//得到点击行的第一个单元格内容
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            className = para[0];//名称
            string[] org = para[1].Split(s);//得到源ID串
            OrginID = Convert.ToInt32(org[1]);//源ID
            IFeature fu = null;//得到要素
            int n = _AppHk.MapControl.Map.LayerCount;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }


        }
        /// <summary>
        /// 代码标准
        /// </summary>
        /// <param name="sender"></param>
        private static void Code(object sender)
        {
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.SelectedCells[0].Value.ToString();//得到点击行的第一个单元格内容
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            className = para[0];//名称
            string[] org = para[1].Split(s);//得到源ID串
            OrginID = Convert.ToInt32(org[1]);//源ID
            IFeature fu = null;//得到要素
            int n = _AppHk.MapControl.Map.LayerCount;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                //IFeatureLayer f_layer = temp_layer as IFeatureLayer;//转成对应的要素层
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }


        }
        /// <summary>
        /// 悬挂点
        /// </summary>
        /// <param name="sender"></param>
        private static void HangPoint_click(object sender)
        {
            #region MyRegion
            DataGridView view = (DataGridView)sender;//将当前操作的对象转成控件对象，以方便操作
            string temp = view.SelectedCells[0].Value.ToString();//得到点击行的第一个单元格内容
            string className = "";//要素类的名称
            int OrginID = 0;//源要素类的要素ID

            char[] p = { ' ' };//以空格为分割点
            char[] s ={ '：' };//以：为分割点
            string[] para = temp.Split(p);
            className = para[0];//名称
            string[] org = para[1].Split(s);//得到源ID串
            OrginID = Convert.ToInt32(org[1]);//源ID
            IFeature fu = null;//得到要素
            int n = _AppHk.MapControl.Map.LayerCount;
            if (n == 0) return;
            //遍历找出我们指定层
            for (int S = 0; S < n; S++)
            {
                ILayer layer = _AppHk.MapControl.Map.get_Layer(S);
                //判别是不是组，如果是，就从组中取一个层
                if (layer is IGroupLayer)
                {
                    if (layer.Name == "更新修编数据" || layer.Name == "工作库数据" || layer.Name == "现势库数据")
                    {
                        ICompositeLayer C_layer = layer as ICompositeLayer;//得到组合图层
                        for (int c = 0; c < C_layer.Count; c++)
                        {
                            ILayer temp_layer = C_layer.get_Layer(c);
                            IFeatureLayer F_layer = temp_layer as IFeatureLayer;
                            IDataset set = F_layer.FeatureClass as IDataset;
                            if (className == set.Name)
                            {
                                m_CurLayer = temp_layer;
                                //IFeatureLayer f_layer = temp_layer as IFeatureLayer;//转成对应的要素层
                                fu = F_layer.FeatureClass.GetFeature(OrginID);//得到要素
                                break;
                            }

                        }
                    }
                }

            }
            if (fu != null)
            {
                _AppHk.MapControl.Map.ClearSelection();//每次进来前先清除之前选择过的
                _AppHk.MapControl.Map.SelectFeature(m_CurLayer, fu);//在对应的层上选择指定的元素
                SysCommon.Gis.ModGisPub.ZoomToFeature(_AppHk.MapControl, fu);//定位到相应的层
            }
            #endregion
        }
    }
}
