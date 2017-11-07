using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.esriSystem;
namespace GeoEdit
{
    public class ControlsDeploy : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        Plugin.Application.IAppArcGISRef m_Hook;
        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsDeploy()
        {

            base._Name = "GeoEdit.ControlsDeploy";
            base._Caption = "属性修改";
            base._Tooltip = "属性修改";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "属性修改";

        }

        /// <summary>
        /// 控制融合按钮是否为可用，在Map上的存在图层，并且选中了要素，并且编辑状态为打开时，该按钮可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
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

        public override void OnClick()
        {
            if (myHook == null) return;
            if (myHook.MapControl == null) return;

            if (_tool == null || _cmd == null || m_Hook == null) return;
            if (m_Hook.MapControl == null) return;

            m_Hook.MapControl.CurrentTool = _tool;
            m_Hook.CurrentTool = this.Name;
            
            
            
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            if (myHook.MapControl == null) return;
            
            m_Hook = myHook as Plugin.Application.IAppArcGISRef;
            _tool = new ChangsFeature();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(m_Hook);
            


        }

    }
    /// <summary>
    /// 使用TOOL工具操作 让属性修改点击时就是可选择的工具
    /// </summary>
    public class ChangsFeature:BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private Plugin.Application.IAppFormRef myHook ;
        private Plugin.Application.IAppGISRef smpdHook;
        public ChangsFeature()
        {
            base.m_category = "GeoCommon";
            base.m_caption = "ChangsFeature";
            base.m_message = "移动选择要素";
            base.m_toolTip = "移动选择要素";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.Select.cur");
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
            }
        }

        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();
            myHook = hook as Plugin.Application.IAppFormRef;
            Plugin.Application.IAppArcGISRef HookGis = hook as Plugin.Application.IAppArcGISRef;
            smpdHook = hook as Plugin.Application.IAppGISRef;
            m_hookHelper.Hook = HookGis.MapControl;
            m_MapControl = HookGis.MapControl as IMapControlDefault;
            
        }
        public override void OnClick()
        {
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
                if (pLayer.Visible == false)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                if (pFeatureLayer.Selectable == false)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }

                GetSelctionSet(pFeatureLayer, pGeometry, bjustone, Shift);

                pLayer = pEnumLayer.Next();
            }

            //触发Map选择发生变化事件
            ISelectionEvents pSelectionEvents = m_hookHelper.FocusMap as ISelectionEvents;
            pSelectionEvents.SelectionChanged();

            //刷新
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_hookHelper.ActiveView.Extent);
            AttributeShow();
        }
        private void GetSelctionSet(IFeatureLayer pFeatureLayer, IGeometry pGeometry, bool bjustone, int Shift)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            //没开启编辑的不可选择
            IDataset pDataset = pFeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
            if (!pWorkspaceEdit.IsBeingEdited()) return;
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
        }


        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
        }
        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        /// <summary>
        /// 显示属性 主程序入口
        /// </summary>
        public void AttributeShow()
        {
            EnterSelectValueCount();//首先确定地图有没有选择的要素
            ///当我们的属性窗体没有显示过，就显示，否则就给它激活
            if (AttributeShow_state.state_value)
            {
                if (!AttributeShow_state.show_state)
                {
                    AttributeShow_state.Temp_frm = new FrmAttribute(GetData_tree(), GetData_View(), smpdHook);//传一个值到窗体当中
                    AttributeShow_state.Temp_frm.Owner = myHook.MainForm;
                    AttributeShow_state.Temp_frm.ShowInTaskbar = false;
                    AttributeShow_state.Temp_frm.Show();
                    AttributeShow_state.show_state = true;//更新它的状态
                    ControlsDeploy d = new ControlsDeploy();
                    smpdHook.ArcGisMapControl.OnSelectionChanged += new EventHandler(Select);
                }
                
            }
        }
        /// <summary>
        /// MAP上选择集更改时发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Select(object sender, EventArgs e)
        {
            if (AttributeShow_state.show_state == true && AttributeShow_state.state_value == true)
            {
                Hashtable hs_tree = GetData_tree();//新TREE的值
                Hashtable hs_show = GetData_View();//新显示控件属性值

                //在MAP上，可能会选择一个空的要素，所以判别它是否为空
                if (hs_show != null && hs_tree != null)
                {
                    AttributeShow_state.Temp_frm.hs_table_tree = hs_tree;//给属性修改窗体重新赋值 tree值
                    AttributeShow_state.Temp_frm.hs_table_attribute = hs_show;//给属性修改窗体重新赋值 attribute值
                    AttributeShow_state.Temp_frm.DataMain();
                }
                else
                {
                    AttributeShow_state.Temp_frm.DT_VIEW_Attriubte.DataSource = null;//如果传过来的值为空就给窗体赋值一个空
                    AttributeShow_state.Temp_frm.treeview_Name.Nodes.Clear();//清除树上的节点
                }

            }
        }

        /// <summary>
        /// 确定地图上是否有选择的要素
        /// </summary>
        private void EnterSelectValueCount()
        {
            IEnumFeature features = smpdHook.ArcGisMapControl.Map.FeatureSelection as IEnumFeature;
            features.Reset();
            IFeature feature = features.Next();
            if (feature != null)
            {
                AttributeShow_state.state_value = true;//确定里面有选中的值
            }
        }

        /// <summary>
        /// 从MAP上得到选择的要素数据
        /// <para>形式如：name oid，其中NAME是指的要素类的名字，OID指的是要素的ID</para>
        /// <remarks>主要是用来邦定界面上的树</remarks>
        /// </summary>
        private Hashtable GetData_tree()
        {
            Hashtable table = new Hashtable();//建立一个KEY VALUE
            IEnumFeature f = m_hookHelper.FocusMap.FeatureSelection as IEnumFeature;//行到被选择的要素的数据集
            f.Reset();
            IFeature feature = f.Next();
            string name = "";
            string oid = "";
            #region 通过遍历组合KEY为：要素类名，VALUE为：OID，如果要素名存在KEY中，那么就累加到之前的值后面
            while (feature != null)
            {
                IObjectClass obj = feature.Class;
                name = feature.Class.AliasName;
                oid = feature.OID.ToString();
                if (table.Count == 0)
                {
                    table.Add(name, oid);
                }
                else
                {
                    if (table.ContainsKey(name))
                    {
                        string temp = table[name].ToString() + " " + oid;
                        table[name] = temp;
                    }
                    else
                    {
                        table.Add(name, oid);
                    }
                }
                feature = f.Next();

            }
            #endregion

            int K_count = table.Count;//判定数据字典是不是空的
            if (K_count == 0)
            {
                table = null;
            }
            return table;
        }
        /// <summary>
        /// 得到所选择的要素不可修改的字段
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="feature"></param>
        private void GetOnlyReadAttribute(string layer, IFeature feature)
        {
            ArrayList list = new ArrayList();//存放不可修改的属性名
            IFields fields = feature.Fields;
            for (int n = 0; n < fields.FieldCount; n++)
            {
                IField field = feature.Fields.get_Field(n);
                string name = field.Name;
                if (field.Editable && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeOID)
                {
                    continue;
                }
                else
                {
                    list.Add(name);
                }
            }
            MoData.GetOnlyReadAtt.Add(layer, list);//以名字和属性列表集存入


        }
        /// <summary>
        /// 主要是用来显示对应OID的属性值 给属性显示控件提供数据
        /// <para>key=name+oid value=field +value</para>
        /// <remarks>利用数据字典，将要素类的名称和单个的OID作为一个KEY，然后把OID里的字段和值取出来当作值</remarks>
        /// </summary>
        /// <returns></returns>
        private Hashtable GetData_View()
        {

            Hashtable table = new Hashtable();
            IEnumFeature f_dataset = m_hookHelper.FocusMap.FeatureSelection as IEnumFeature;//取得地图控件上被选择的要素集

            f_dataset.Reset();
            IFeature feature = f_dataset.Next();//取得下一个要素
            string name = "";
            string oid = "";
            #region 遍历对应要素类下对应OID要素的记录值
            while (feature != null)
            {
                IDataset ds = feature.Class as IDataset;
                name = ds.Name;
                if (MoData.GetOnlyReadAtt == null)
                {
                    GetOnlyReadAttribute(name, feature);//调用只属性的方法
                }
                else
                {
                    if (!MoData.GetOnlyReadAtt.ContainsKey(name))
                    {
                        GetOnlyReadAttribute(name, feature);//调用只属性的方法
                    }
                }
                oid = feature.OID.ToString();

                string key = name + oid;//使用KEY值,把要素类的名称加上单个的OID组合成一个KEY
                string Value = "";//使用VALUE

                string shape = "";//确定要素的SHAPE是什么
                #region 得到要素的SHAPE类型：注记，面，线，点
                if (feature.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    shape = "注记";
                }
                else
                {
                    IGeometry geometry = feature.Shape;//得到要素的几何图形

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

                IFields fields = feature.Fields;
                int k = fields.FieldCount;
                #region 此遍历主要是用来得到一整条要素的记录，把它进行组合成一个VALUE
                for (int s = 0; s < k; s++)
                {

                    IField field = fields.get_Field(s);
                    string str = "";
                    string f_value = feature.get_Value(s).ToString();//得到对应列值
                    if (field.Name.ToLower() == "shape")
                    {
                        str = field.Name + " " + shape;//得到对应的SHAPE类型
                    }
                    else if (field.Name.ToLower() == "element")
                    {
                        str = field.Name + " blob";
                    }
                    else
                    {
                        if (f_value != string.Empty)
                        {
                            str = field.Name + " " + f_value;
                        }
                        else
                        {
                            str = field.Name + " " + "null";
                        }
                    }
                    Value += str + ",";
                }
                #endregion
                #region 把KEY和VALUE加入到数据字典当中，以备后面使用
                table.Add(key, Value);
                #endregion
                feature = f_dataset.Next();

            }
            #endregion

            if (table.Count == 0)
            {
                table = null;
            }
            return table;
        }
    }
}
