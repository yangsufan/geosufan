using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace GeoEdit
{
    public class ControlsMerge : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        private IFeatureLayer  m_MergeLayer;
        static int m_SavedOID = 0;

        ICommand _cmd = null;      //动态加载的清除的按钮

        public static int SavedOID
        {
            set { ControlsMerge.m_SavedOID = value; }
        }

        public ControlsMerge()
        {
            base._Name = "GeoEdit.ControlsMerge";
            base._Caption = "融合";
            base._Tooltip = "融合";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "融合";
            
        }

        //===================================================================================================
        //陈胜鹏  2009-08-19 添加
        /// <summary>
        /// 控制融合按钮是否为可用，在Map上的存在图层，并且选中了要素，并且编辑状态为打开时，该按钮可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                try
                {

                    if (MoData.v_CurWorkspaceEdit == null) return false;

                    //判断是否加载了图层
                    if (myHook.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    //判断是否选中两个及两个以上的要素要素
                    if (myHook.MapControl.Map.SelectionCount < 2)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    //获取被选中要素的数据集，判断是否编辑已经打开
                    bool isEditing = GetDatasetEditState(myHook.MapControl.Map);
                    if (!isEditing)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    //所有条件都满足则设置为可用
                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        //======================================================================================================

        #region 判断要素是否在同一图层以及图层的编辑状态
        //===================================================================================================
        //陈胜鹏  2009-08-19 添加
        /// <summary>
        /// 从图面上获取选中要素所在数据集的工作空间，判断是否开启编辑
        /// </summary>
        /// <param name="pMap">当前加载了数据的地图对象</param>
        /// <returns></returns>
        private bool GetDatasetEditState(ESRI.ArcGIS.Carto.IMap pMap)
        {

            int pSameLyr = 0;  //记录要素是否为同层
            ILayer pLayer = null;
            //判断选择的要素是否处于同一个图层
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                IFeatureLayer pFeatLyr = null;
                IFeatureSelection pFeatSel = null;
                pLayer = pMap.get_Layer(i);
                if(pLayer is IGroupLayer )
                {
                    if (pLayer.Name == "示意图")
                    {
                        continue;
                    }
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        pFeatLyr = mLayer as IFeatureLayer;
                        if (pFeatLyr != null)
                        {
                            pFeatSel = pFeatLyr as IFeatureSelection;
                            if (pFeatSel.SelectionSet.Count > 0)
                            {
                                pSameLyr = pSameLyr + 1;
                                m_MergeLayer = pFeatLyr;//当只有一个图层被选中时，pFeatLyr就是要进行融合的目标图层
                            }
                        }
                    }
                }
                
                pFeatLyr = pLayer as IFeatureLayer;
                if (pFeatLyr != null)
                {
                    pFeatSel = pFeatLyr as IFeatureSelection;
                    if (pFeatSel.SelectionSet.Count > 0)
                    {
                        pSameLyr = pSameLyr + 1;
                        m_MergeLayer = pFeatLyr;//当只有一个图层被选中时，pFeatLyr就是要进行融合的目标图层
                    }
                }
            }
            //如果选择的要素所在的层数不是1，即要素不在同一个层
            if (pSameLyr != 1)
            {
                return false;
            }
            
            //一系列的QI后获得IWorkspaceEdit接口对象pWSE
            IFeatureClass pFeatCls = m_MergeLayer.FeatureClass as IFeatureClass;
            IDataset pDS = pFeatCls as IDataset;
            IWorkspace pWs = pDS.Workspace as IWorkspace;
            IWorkspaceEdit pWSE = pWs as IWorkspaceEdit;

            return pWSE.IsBeingEdited();  //返回编辑状态
            
        }
        //=================================================================================================== 
        #endregion

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
        /// 执行融合功能
        /// </summary>
        public override void OnClick()
        {
            if (m_MergeLayer!=null)
            {
                IFeatureSelection pFeatSel = m_MergeLayer as IFeatureSelection;
                ISelectionSet pSelectionSet = pFeatSel.SelectionSet;
      
                //如果选择的要素多余一个，则可以开始融合
                if (pSelectionSet.Count>1)
                {
                    //开启操作
                    MoData.v_CurWorkspaceEdit.StartEditOperation();

                    #region 弹出属性选择界面
                    Plugin.Application.IAppFormRef pAppform = myHook as Plugin.Application.IAppFormRef;
                    AttributeShow_state.Temp_frm4Merge = new FrmAttribute4Merge(GetData_tree(), GetData_View(), myHook);//传一个值到窗体当中
                    AttributeShow_state.Temp_frm4Merge.Owner = pAppform.MainForm;
                    AttributeShow_state.Temp_frm4Merge.ShowInTaskbar = false;
                    AttributeShow_state.Temp_frm4Merge.ShowDialog(); 
                    #endregion

                    //确认了保留哪个要素才能进行融合
                    if (m_SavedOID != 0)
                    {
                        MergeSelectedFeature(pSelectionSet, m_MergeLayer, m_SavedOID);
                        m_SavedOID = 0;   //至为零
                    }

                    //结束操作
                    MoData.v_CurWorkspaceEdit.StopEditOperation();
                }

                _cmd.OnClick();  //执行清除选择

                //刷新图面
                IActiveView pActiveView = myHook.MapControl.Map as IActiveView;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);  
            }
        }

        #region 要素融合的主函数   
        //========================================================================================
        //陈胜鹏  2009-08-19  添加
        /// <summary>
        /// 融合选择集中的要素
        /// </summary>
        /// <param name="pSelectionSet">选择集对象，从中取出需要融合的要素</param>
        /// <param name="pMergeLayer">发生融合的要素所在图层</param>
        /// <param name="OID">被保留的要素OID</param>
        private void MergeSelectedFeature(ISelectionSet pSelectionSet, IFeatureLayer pMergeLayer, int OID)
        {
            //判断是否满足融合条件
            bool CanMerge = JudgeCondition();
            //OID = 0;
            //如果满足融合的判定条件才能开始进行融合（这里主要分几何条件和属性条件）
            if (CanMerge)
            {
                IEnumIDs pEnumIDs;     //枚举要素变量
                Dictionary<int, IFeature> pDicFeature =new Dictionary<int,IFeature>();   //存储多个需要融合的要素的字典
                int FtID = 0;         //要素OID
                IGeometry pGeoTemp=null;    //存储融合后的要素图形
                ITopologicalOperator pTopoOperator;
                IFeature pFt = null;
                int pTempFeatID = 0;

                IFeatureClass pFeatCls = pMergeLayer.FeatureClass;

                pEnumIDs = pSelectionSet.IDs;
                FtID = pEnumIDs.Next();

                //初始化临时要素图形
                pTempFeatID = FtID;
                pGeoTemp = pFeatCls.GetFeature(pTempFeatID).Shape;
                pDicFeature.Add(FtID, pFeatCls.GetFeature(pTempFeatID));  //记录遍历到的要素ID

                FtID = pEnumIDs.Next();
                while (FtID!=-1)
                {
                    //获得需要融合的第二个要素
                    pFt = pFeatCls.GetFeature(FtID);
                    pDicFeature.Add(FtID, pFt);  //记录遍历到的要素ID

                    //融合要素的图形
                    pTopoOperator = pGeoTemp as ITopologicalOperator;
                    pGeoTemp = pTopoOperator.Union(pFt.ShapeCopy);

                    //临时图形进行简单化
                    pTopoOperator = pGeoTemp as ITopologicalOperator;
                    pTopoOperator.Simplify();

                    //下一个要素
                    FtID = pEnumIDs.Next();
                }

                //获取第一个保留的要素，赋给它融合后的图形，并保存
                pFt = pFeatCls.GetFeature(OID);
                pFt.Shape = pGeoTemp;

                //处理设置FID、state不一致问题 wjj 20090903
                IDisplayRelationshipClass pDisResCls = pMergeLayer as IDisplayRelationshipClass;
                if (pDisResCls.RelationshipClass != null)     //是否与日志记录表做了联表查询
                {
                    //获取图幅结合表
                    IFeatureLayer pFeatLay = ModPublic.GetLayerOfGroupLayer(myHook.MapControl, "范围", "图幅范围") as IFeatureLayer;
                    if (pFeatLay != null)
                    {
                        ChangeFeatureState(pDisResCls.RelationshipClass, pFt, pDicFeature, pFeatLay.FeatureClass);
                    }
                }
                
                pFt.Store();

                //删除被融合的要素
                foreach (KeyValuePair<int, IFeature> var in pDicFeature)
                {
                    if (var.Key != OID)
                    {
                        pFt = pFeatCls.GetFeature(var.Key);
                        pFt.Delete();
                    }

                }

                pDicFeature = null;
            }
        }

        #region wjj 20090903 处理设置融合时FID、state不一致问题并修改核心日志记录表
        /// <summary> 
        /// 处理设置融合时FID、state不一致问题并修改核心日志记录表
        /// </summary>
        /// <param name="pRelationshipClass">融合要素关系类</param>
        /// <param name="pTargetFeat">保留属性的要素</param>
        /// <param name="pDicFeature">融合要素集合</param>
        /// <param name="pRangeClss">图幅结合表</param>
        private void ChangeFeatureState(IRelationshipClass pRelationshipClass, IFeature pTargetFeat, Dictionary<int, IFeature> pDicFeature,IFeatureClass pRangeClss)
        {
            if (MoData.v_LogTable == null) return;
            if (MoData.v_LogTable.DbConn == null) return;  //日志记录表是否连接

            if (pDicFeature == null || pRelationshipClass == null || pRangeClss==null) return;
            Exception exError = null;
            IRelQueryTable pJionRelQueryTable = SysCommon.Gis.ModGisPub.GetRelQueryTable(pRelationshipClass, true, null, "STATE", false, true, out exError);
            IFeatureClass pFeatCls=pJionRelQueryTable as IFeatureClass;
            if (pFeatCls == null) return;

            IDataset pOriginIDataset = pRelationshipClass.OriginClass as IDataset;    //要素类
            if (pOriginIDataset == null) return;

            int indexOID = pFeatCls.Fields.FindField(pOriginIDataset.Name+".OBJECTID");
            int indexFID = pFeatCls.Fields.FindField(pOriginIDataset.Name + ".GOFID");
            int indexState = pFeatCls.Fields.FindField( pRelationshipClass.DestinationClass.AliasName+ ".STATE");
            if (indexOID == -1 || indexFID == -1 || indexState == -1) return;

            object temp = pFeatCls.GetFeature(pTargetFeat.OID).get_Value(indexState);
            if (temp is DBNull) temp = -1;          //表示未变化
            int valueTargetState = Convert.ToInt32(temp);
            int valueTargetFID = Convert.ToInt32(pFeatCls.GetFeature(pTargetFeat.OID).get_Value(indexFID));
            if (valueTargetState == 1 || valueTargetState==2)    //为发生变化的要素      
            {
                foreach (KeyValuePair<int, IFeature> keyValue in pDicFeature)
                {
                    if (keyValue.Key == pTargetFeat.OID) continue;   //为保留属性的要素则跳到下个要素
                    IFeature pTempFeature=pFeatCls.GetFeature(keyValue.Key);
                    temp = pTempFeature.get_Value(indexState);
                    if (temp is DBNull) temp = -1;          //表示未变化
                    int valueState = Convert.ToInt32(temp);
                    int valueFID = Convert.ToInt32(pTempFeature.get_Value(indexFID));

                    if (valueState == 1)
                    {//该要素的State为新建,则删除该要素日志记录表中对应内容
                        string strCon = "LAYERNAME='" + pOriginIDataset.Name + "'and OID=" + pTempFeature.OID + " and STATE=1 and SAVE=1";
                        MoData.v_LogTable.UpdateTable("delete * from UpdateLog where " + strCon, out exError);
                    }
                    else if (valueState == 2)
                    {//该要素的State为修改
                        string strCon = "GOFID=" + valueFID + " and SAVE=1";
                        if (valueFID != valueTargetFID)
                        {//该要素的FID与保留属性要素FID不同,则将该要素日志记录表中对应内容状态修改为删除
                            MoData.v_LogTable.UpdateTable("update UpdateLog set STATE=3 where " + strCon, out exError);
                        }
                        else
                        {//则删除该要素日志记录表中对应内容
                            MoData.v_LogTable.UpdateTable("delete * from UpdateLog where " + strCon, out exError);
                        }
                    }
                    else
                    {//该要素的State为未变化
                        if (valueFID != valueTargetFID)
                        {//该要素的FID与保留属性要素FID不同,则在日志记录表中增加该要素对应内容状态为删除
                            string strSQL = "insert into UpdateLog(GOFID,STATE,SAVE) values(" + valueFID.ToString() + ",3,1)";
                            MoData.v_LogTable.UpdateTable( strSQL, out exError);
                        }
                    }
                }
            }
            else    //为未发生变化的要素   
            {
                bool bAddLog = false;
                foreach (KeyValuePair<int, IFeature> keyValue in pDicFeature)
                {
                    if (keyValue.Key == pTargetFeat.OID) continue;   //为保留属性的要素则跳到下个要素
                    IFeature pTempFeature = pFeatCls.GetFeature(keyValue.Key);
                    temp = pTempFeature.get_Value(indexState);
                    if (temp is DBNull) temp = -1;          //表示未变化
                    int valueState = Convert.ToInt32(temp);
                    int valueFID = Convert.ToInt32(pTempFeature.get_Value(indexFID));

                    if (valueState == 1)
                    {//该要素的State为新建,则删除该要素日志记录表中对应内容,并在日志记录表中增加保留属性要素对应内容状态为修改
                        string strCon = "LAYERNAME='" + pOriginIDataset.Name + "'and OID=" + pTempFeature.OID + " and STATE=1 and SAVE=1";
                        MoData.v_LogTable.UpdateTable("delete * from UpdateLog where " + strCon, out exError);

                        if (bAddLog == false) //(只增加一次)在日志记录表中增加保留属性要素对应内容状态为修改
                        {
                            string strSQL = "insert into UpdateLog(GOFID,LAYERNAME,OID,STATE,SAVE) values(" + valueTargetFID.ToString() + ",'" + pOriginIDataset.Name + "'," + pTargetFeat.OID + ",2,1)";
                            MoData.v_LogTable.UpdateTable(strSQL, out exError);
                            bAddLog = true;
                        }
                    }
                    else if (valueState == 2)
                    {//该要素的State为修改
                        string strCon = "GOFID=" + valueFID + " and SAVE=1";
                        if (valueFID != valueTargetFID)
                        {//该要素的FID与保留属性要素FID不同,则将该要素日志记录表中对应内容状态修改为删除
                            MoData.v_LogTable.UpdateTable("update UpdateLog set STATE=3 where " + strCon, out exError);
                        }
                        else
                        {//则删除该要素日志记录表中对应内容
                            MoData.v_LogTable.UpdateTable("delete * from UpdateLog where " + strCon, out exError);
                        }

                        if (bAddLog == false) //(只增加一次)在日志记录表中增加保留属性要素对应内容状态为修改
                        {
                            string strSQL = "insert into UpdateLog(GOFID,LAYERNAME,OID,STATE,SAVE) values(" + valueTargetFID.ToString() + ",'" + pOriginIDataset.Name + "'," + pTargetFeat.OID + ",2,1)";
                            MoData.v_LogTable.UpdateTable(strSQL, out exError);
                            bAddLog = true;
                        }
                    }
                    else
                    {//该要素的State为未变化,则增加日志记录表中对应内容状态为删除
                        if (valueFID != valueTargetFID)
                        {//该要素的FID与保留属性要素FID不同,则在日志记录表中增加该要素应内容状态为删除
                            string strSQL = "insert into UpdateLog(GOFID,STATE,SAVE) values(" + valueFID.ToString() + ",3,1)";
                            MoData.v_LogTable.UpdateTable(strSQL, out exError);
                        }

                        if (bAddLog == false) //(只增加一次)在日志记录表中增加保留属性要素对应内容状态为修改
                        {
                            string strSQL = "insert into UpdateLog(GOFID,LAYERNAME,OID,STATE,SAVE) values(" + valueTargetFID.ToString() + ",'" + pOriginIDataset.Name + "'," + pTargetFeat.OID + ",2,1)";
                            MoData.v_LogTable.UpdateTable(strSQL, out exError);
                            bAddLog = true;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 判断融合后采用哪个要素的属性
        /// </summary>
        /// <returns></returns>
        private bool JudgeCondition()
        {

            return true;
        }
        //======================================================================================== 
        #endregion

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            if (myHook.MapControl == null) return;

            //创建清除选择按钮
            _cmd = new ControlsClearSelectionCommandClass();
            _cmd.OnCreate(myHook.MapControl);

            myHook.ArcGisMapControl.OnKeyDown += new IMapControlEvents2_Ax_OnKeyDownEventHandler(ArcGisMapControl_OnKeyDown);
        }

        private void ArcGisMapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (this.Enabled && e.shift == 2 && e.keyCode == 77)
            {
                this.OnClick();
            }
        }


        //==========================================================================================================
        //陈胜鹏   2009-08-20   添加
        /// <summary>
        /// 从MAP上得到选择的要素数据
        /// <para>形式如：name oid，其中NAME是指的要素类的名字，OID指的是要素的ID</para>
        /// <remarks>主要是用来邦定界面上的树</remarks>
        /// </summary>
        private Hashtable GetData_tree()
        {

            Hashtable table = new Hashtable();//建立一个KEY VALUE
            IEnumFeature f = myHook.MapControl.Map.FeatureSelection as IEnumFeature;//行到被选择的要素的数据集
            f.Reset();
            IFeature feature = f.Next();
            string name = "";
            string oid = "";
            #region 通过遍历组合KEY为：要素类名，VALUE为：OID，如果要素名存在KEY中，那么就累加到之前的值后面
            while (feature != null)
            {
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
        //=================================================================================================================


        //=================================================================================================================
        //陈胜鹏   2009-08-20  添加
        /// <summary>
        /// 主要是用来显示对应OID的属性值 给属性显示控件提供数据
        /// <para>key=name+oid value=field +value</para>
        /// <remarks>利用数据字典，将要素类的名称和单个的OID作为一个KEY，然后把OID里的字段和值取出来当作值</remarks>
        /// </summary>
        /// <returns></returns>
        private Hashtable GetData_View()
        {

            Hashtable table = new Hashtable();
            IEnumFeature f_dataset = myHook.MapControl.Map.FeatureSelection as IEnumFeature;//取得地图控件上被选择的要素集

            f_dataset.Reset();
            IFeature feature = f_dataset.Next();//取得下一个要素
            string name = "";
            string oid = "";
            #region 遍历对应要素类下对应OID要素的记录值
            while (feature != null)
            {
                name = feature.Class.AliasName;
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
                        str = "SHAPE " + shape;//得到对应的SHAPE类型
                    }
                    else if (field.Name.ToLower() == "element")
                    {
                        str = "Element blob";
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
                if (table.Count == 0)
                {
                    table.Add(key, Value);
                }
                else
                {
                    table.Add(key, Value);
                }
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
        //=================================================================================================================
    }
}
