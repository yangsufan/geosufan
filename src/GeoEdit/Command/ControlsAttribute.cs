using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoEdit
{
    /// <summary>
    /// 属性刷
    /// </summary>
    public class ControlsAttribute : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        private ControlsAttributeCopy _ControlsAttributeCopy;
        private ITool _tool = null;
        private ICommand _cmd = null;

        public static IFeatureClass m_CurFeatCls;         //当前源要素的featureclass

        /// <summary>
        /// 构造函数 用来控件是否显示及相关的文字说明
        /// </summary>
        public ControlsAttribute()
        {

            base._Name = "GeoEdit.ControlsAttribute";
            base._Caption = "属性刷";
            base._Tooltip = "属性刷";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "属性刷";


        }

        /// <summary>
        /// 控制融合按钮是否为可用，在Map上的存在图层，并且选中了要素，并且编辑状态为打开时，该按钮可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (_ControlsAttributeCopy == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                return true;
            }
        }

        public override bool Checked
        {
            get
            {
                if (_ControlsAttributeCopy == null) return false;
                if (_ControlsAttributeCopy.Exit == true)
                {
                    myHook.CurrentTool = null;
                    return false;
                }
                if (myHook.CurrentTool != this.Name) return false;
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }
        /// <summary>
        /// 主要单击入口
        /// </summary>
        public override void OnClick()
        {
            if (myHook == null) return;
            if (myHook.MapControl == null) return;

            if (myHook.MapControl.Map.SelectionCount == 1)
            {
                AttributeShow_state.hs_Feature = new Hashtable();
                GetMapFeature();//得到源要素
            }
            else
            {
                myHook.CurrentTool = null;
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择参考要素,个数必须为1！");
                return;
            }

            myHook.MapControl.CurrentTool = _tool;
            myHook.CurrentTool = this.Name;
        }

        /// <summary>
        /// 初始化我们用到的主窗体里的控件
        /// </summary>
        /// <param name="hook"></param>
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            if (myHook.MapControl == null) return;
            _ControlsAttributeCopy = new ControlsAttributeCopy();
            _tool = _ControlsAttributeCopy as ITool;
            _cmd = _tool as ICommand;
            _cmd.OnCreate(myHook.MapControl);

        }

        /// <summary>
        /// 获取MAP上选择的源要素
        /// </summary>
        private void GetMapFeature()
        {

            AttributeShow_state.state_brush = true;//当确定是选择一个要素源时，才改变状态

            IEnumFeature IEnum_dataset = myHook.MapControl.Map.FeatureSelection as IEnumFeature;//行到被选择的要素的数据集
            IEnum_dataset.Reset();
            IFeature Feature = IEnum_dataset.Next();
            IWorkspace space = MoData.v_CurWorkspaceEdit as IWorkspace;//得到相应的操作空间
            while (Feature != null)
            {
                m_CurFeatCls = Feature.Class as IFeatureClass;
                IDataset dataset_space = m_CurFeatCls as IDataset;//中转成一个要素集合
                if (!space.Equals(dataset_space.Workspace))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "编辑空间不一致，请重新选择要素！");
                    myHook.MapControl.Map.ClearSelection();
                    return;
                    
                }

                string shape = "";//确定要素的SHAPE是什么
                string Name = Feature.Class.AliasName;//层名
                AttributeShow_state.OID = Feature.OID.ToString();//得到源OID
                #region 得到要素的SHAPE类型：注记，面，线，点
                if (Feature.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    shape = "注记";
                }
                else
                {
                    IGeometry geometry = Feature.Shape;//得到要素的几何图形

                    switch (geometry.GeometryType.ToString())//确定它的几何要素类型
                    {
                        case "esriGeometryPolygon":
                            shape = "面";
                            break;
                        case "esriGeometryPolyline":
                            shape = "线";
                            break;
                        case "esriGeometryPoint":
                            shape = "点";
                            break;
                    }
                }
                #endregion

                IFields fields = Feature.Fields;
                int count = fields.FieldCount;//得到该要素共有多少个属性
                AttributeShow_state.feature_count = count;//将源要素的字段数赋值给总的全局数
                string value = "";//接收该要素的值
                for (int n = 0; n < count; n++)
                {
                    string name = fields.get_Field(n).Name.ToLower();
                    if (name == "shape")
                    {
                        value += shape + ",";
                    }
                    else
                    {
                        string F_value = Feature.get_Value(n).ToString();
                        if (F_value == string.Empty)
                        {
                            F_value = "null";
                        }
                        value += name + " " + F_value + ",";
                    }
                }

                string processStr = value.Substring(0, value.Length - 1);

                AttributeShow_state.hs_Feature.Add(Name, processStr);

                Feature = IEnum_dataset.Next();
            }
            myHook.MapControl.Map.ClearSelection();
            myHook.MapControl.ActiveView.Refresh();

        }
    }


    public class ControlsAttributeCopy : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private bool m_bExit;
        public bool Exit
        {
            get
            {
                return m_bExit;
            }
        }

        //类的方法
        public ControlsAttributeCopy()
        {
            base.m_category = "GeoCommon";
            base.m_caption = "AttributeCopy";
            base.m_message = "属性刷";
            base.m_toolTip = "属性刷";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.AttributeCopy.cur");
            }
            catch(Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            //支持快捷键
            m_MapControl.KeyIntercept = 1;  //esriKeyInterceptArrowKeys
        }

        public override void OnKeyUp(int keyCode, int Shift)
        {
            if (m_MapControl.Map.SelectionCount > 0)
            {
                AttributeShow_state.state_brush = false;//只有当有选择的时候才会把状态改回来
            }
            if (!AttributeShow_state.state_brush)//首先判别是否选择了源要素
            {
                if (keyCode == 13)   //再判别当前是不是单击的是回车
                {
                    CopyAttribute();//进行要素属性的COPY
                }
            }

            if (keyCode == 27) //退出
            {
                m_MapControl.Map.ClearSelection();
                m_MapControl.ActiveView.Refresh();
                m_MapControl.CurrentTool = null;
                m_bExit = true;
            }
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
            //设置点选择容差
            ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
            double Length = ModPublic.ConvertPixelsToMapUnits(m_hookHelper.ActiveView, pSelectEnv.SearchTolerance);

            IPoint pPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            IGeometry pGeometry = pPoint as IGeometry;
            ITopologicalOperator pTopo = pGeometry as ITopologicalOperator;
            IGeometry pBuffer = pTopo.Buffer(Length);

            //仅与框架别界相交地物会被选取
            pGeometry = m_MapControl.TrackRectangle() as IGeometry;
            bool bjustone = true;
            if (pGeometry != null)
            {
                if (pGeometry.IsEmpty)
                {
                    pGeometry = pBuffer;
                }
                else
                {
                    bjustone = false;
                }
            }
            else
            {
                pGeometry = pBuffer;
            }

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer = m_MapControl.Map.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;

                if (pFeatureClass.ObjectClassID != ControlsAttribute.m_CurFeatCls.ObjectClassID)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }

                switch (Shift)
                {
                    case 1:   //增加选择结果集
                        ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultAdd, bjustone);
                        break;
                    case 4:   //减少选择结果集
                        ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultSubtract, bjustone);
                        break;
                    case 2:
                        ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultXOR, bjustone);
                        break;
                    default:   //新建选择结果集
                        ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultNew, bjustone);
                        break;
                }

                pLayer = pEnumLayer.Next();
            }

            //刷新
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_hookHelper.ActiveView.Extent);
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
        }

        public override void OnDblClick()
        {

        }
        public override bool Deactivate()
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 将源要素的属性COPY给同层选中的要素
        /// </summary>
        private void CopyAttribute()
        {

            char[] sp ={ ' ' };//以空格分割
            char[] sp1 ={ ',' };//以逗号分割
            string Feature_value = "";
            foreach (DictionaryEntry de in AttributeShow_state.hs_Feature)
            {
                Feature_value = de.Value.ToString();//得到源OID的属性值
            }
            string[] Field = Feature_value.Split(sp1);//以逗号分割

            IEnumFeature F_dateset = m_MapControl.Map.FeatureSelection as IEnumFeature;//得到MAP上选中的要素集
            F_dateset.Reset();
            IFeature Feature = F_dateset.Next();//取得下一个要素
            if (Feature == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "您没有选择任何要修改的目标要素属性！");
                return;
            }
            MoData.v_CurWorkspaceEdit.StartEditOperation();//开启编辑操作
            while (Feature != null)
            {
                IFeatureClass F_class = Feature.Class as IFeatureClass;

                //MoData.v_CurWorkspaceEdit.RedoEditOperation();//开启可回退
                if (Feature.FeatureType != esriFeatureType.esriFTAnnotation)//判别不是注记时发生操作
                {

                    //COPY时，将源要素属性除外
                    if (Feature.OID.ToString() != AttributeShow_state.OID)
                    {
                        for (int n = 0; n < AttributeShow_state.feature_count; n++)
                        {

                            string value = Feature.Fields.get_Field(n).Name.ToLower();
                            if (value == "objectid" || value == "shape" || value == "shape_length" || value == "shape_area" || value == "element")//确定控件上不能更改的属性
                            {
                                continue;//如果是这些固定不变的字段那么属性值是不能COPY，进行一下一个字段赋值
                            }
                            else
                            {
                                string[] tempStr = Field[n].Split(sp);//以空格分割，得到字段
                                string Value = tempStr[1];//值
                                Feature.set_Value(n, Value);

                            }
                        }
                    }
                }
                Feature.Store();

                Feature = F_dateset.Next();//遍历下一个要素
            }
            MoData.v_CurWorkspaceEdit.StopEditOperation();//结束编辑操作

            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改成功！");
            m_MapControl.Map.ClearSelection();//清除MAP上的选择集
            m_MapControl.ActiveView.Refresh();
            AttributeShow_state.state_brush = true;//确定我们已操作过属性刷，并改变状态，让后面的操作得以进行不出错。

        }
    }
}
