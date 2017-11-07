using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataChecker
{
    /// <summary>
    /// 功能：面套面检查 一个面完全的包含另一个面
    /// 编写人：王晶晶
    /// 修整：王冰
    /// 开发思想：利用拓扑规则的面不相交得出相交的面，然后利用空间过滤关系的包含关系得出当前操作的要素是否在包含面
    /// </summary>
    class PolygonOverlapTreat:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        public PolygonOverlapTreat()
        {
            base._Name = "GeoDataChecker.PolygonOverlapTreat";
            base._Caption = "面域重叠拓扑处理";
            base._Tooltip = "对面套面的的要素进行处理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "面域重叠拓扑处理";
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
            /// <summary>
            /// 主入口函数
            /// </summary>
  
        public override void OnClick()
        {

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行面要素标准化
            Thread aThread = new Thread(new ThreadStart(ExcutePolygonOverlapTreat));
            _AppHk.CurrentThread = aThread;
            aThread.Start();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 面套面处理主函数
        /// </summary>
        /// <param name="_AppHk"></param>
        private void ExcutePolygonOverlapTreat()
        {
            Plugin.Application.IAppFormRef pAppFrm = _AppHk as Plugin.Application.IAppFormRef;

            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "正在获取所有面层..." });
            //获取所有面层
            List<ILayer> listLayers = GetCheckLayers(_AppHk.MapControl.Map, "更新修编数据");
            if (listLayers == null) return;
            List<ILayer> listCheckLays = new List<ILayer>();
            //遍历更新库体里的所有层
            foreach (ILayer pTempLay in listLayers)
            {
                IFeatureLayer pFeatLay = pTempLay as IFeatureLayer;
                if (pFeatLay == null) continue;
                if (pFeatLay.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation) continue;
                //接边针对的是线和面，所以我们操作时只需对线和面层进行即可
                if (pFeatLay.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon) continue;

                listCheckLays.Add(pTempLay);
            }
            if (listCheckLays.Count == 0) return;

            //遍历各个面层并与其它面层建立拓扑规则以获取所有存在重叠的最小面
            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "正在建立拓扑规则..." });
            Dictionary<ITopologyRule,IFeatureClass> dicTemp=new Dictionary<ITopologyRule,IFeatureClass>();
            ITopologyRuleContainer pTopologyRuleContainer = SetCheckState.pT as ITopologyRuleContainer;//使用建立好的拓扑
            //遍历我们得出的所有面层并操作
            foreach (ILayer pLay in listCheckLays)
            {
                IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                if (pFeatLay == null) continue;
                ITopologyRule topologyRule = new TopologyRuleClass();//实例一个拓扑规则类
                topologyRule.TopologyRuleType = esriTopologyRuleType.esriTRTAreaNoOverlap;//使用规则的类型(面不相交)
                topologyRule.OriginClassID = pFeatLay.FeatureClass.FeatureClassID;
                topologyRule.AllOriginSubtypes = true;
                topologyRule.Name = pFeatLay.FeatureClass.AliasName;
                if (pTopologyRuleContainer.get_CanAddRule(topologyRule) == true)//提示拓扑规则是否可以被添加
                {
                    pTopologyRuleContainer.AddRule(topologyRule);
                    dicTemp.Add(topologyRule, pFeatLay.FeatureClass);
                } 

                foreach (ILayer pOtherLay in listCheckLays)
                {
                    if (pOtherLay == pLay) continue;
                    IFeatureLayer pOtherFeatLay = pOtherLay as IFeatureLayer;
                    if (pOtherFeatLay == null) continue;
                    topologyRule = new TopologyRuleClass();
                    topologyRule.TopologyRuleType = esriTopologyRuleType.esriTRTAreaNoOverlapArea;
                    topologyRule.OriginClassID = pFeatLay.FeatureClass.FeatureClassID;
                    topologyRule.AllOriginSubtypes = true;
                    topologyRule.DestinationClassID = pOtherFeatLay.FeatureClass.FeatureClassID;
                    topologyRule.AllDestinationSubtypes = true;
                    topologyRule.Name = pFeatLay.FeatureClass.AliasName + " AreaNoOverlapArea with " + pOtherFeatLay.FeatureClass.AliasName;
                    if (pTopologyRuleContainer.get_CanAddRule(topologyRule) == true)
                    {
                        pTopologyRuleContainer.AddRule(topologyRule);
                        dicTemp.Add(topologyRule, pFeatLay.FeatureClass);
                    } 
                }
            }

            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "正在验证拓扑..." });
            ISegmentCollection pLocation = new PolygonClass();//使用多边形接口
            pLocation.SetRectangle(SetCheckState.Geodatabase.Extent);//将我们用来操作验证的要素类利用SETRECTANGLE来构造一个完整的几何形体
            IPolygon pPoly = SetCheckState.pT.get_DirtyArea(pLocation as IPolygon);
            IEnvelope pAreaValidated = SetCheckState.pT.ValidateTopology(pPoly.Envelope);//验证拓扑
           
            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "正在分析获取要处理的要素..." });
            pAppFrm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { pAppFrm, true });
            pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, 0, dicTemp.Count, 0 });
            //获取所有存在重叠的最小面
            Dictionary<IFeature, IFeatureClass> dicCheckFeats = new Dictionary<IFeature, IFeatureClass>();
            IErrorFeatureContainer pErrFeatCon = SetCheckState.pT as IErrorFeatureContainer;
            IEnumRule pEnumRule = pTopologyRuleContainer.Rules;
            pEnumRule.Reset();
            ITopologyRule pTempTopologyRule = pEnumRule.Next() as ITopologyRule;
            int intcnt = 0;
            while (pTempTopologyRule != null)
            {
                intcnt++;
                pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, -1, -1, intcnt });
                if (!(pTempTopologyRule.TopologyRuleType == esriTopologyRuleType.esriTRTAreaNoOverlap || pTempTopologyRule.TopologyRuleType == esriTopologyRuleType.esriTRTAreaNoOverlapArea))
                    continue;
                IEnumTopologyErrorFeature pEnumTopoErrFeat = pErrFeatCon.get_ErrorFeatures(SetCheckState.Geodatabase.SpatialReference, pTempTopologyRule, SetCheckState.Geodatabase.Extent, true, false);
                ITopologyErrorFeature pTopoErrFeat = pEnumTopoErrFeat.Next();

                IFeatureClass pOrgFeatCls=null;
                if(dicTemp.ContainsKey(pTempTopologyRule))
                {
                     pOrgFeatCls=dicTemp[pTempTopologyRule];
                }
                if(pOrgFeatCls==null) break;
                IDataset pDatasetTemp = pOrgFeatCls as IDataset;
                while (pTopoErrFeat != null)
                {
                    IFeature orgFeat = pOrgFeatCls.GetFeature(pTopoErrFeat.OriginOID);
                    bool bHasFeat = false;
                    foreach (ILayer pLay in listCheckLays)
                    {
                        IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                        if (pFeatLay == null) continue;
                        IDataset pDataset = pFeatLay.FeatureClass as IDataset;
                        string strCon="";
                        //排除指定要素
                        if (pDataset.Name == pDatasetTemp.Name)
                        {
                            strCon = "OBJECTID<>" + orgFeat.OID.ToString();
                        }

                        //判断面orgFeat内是否包含有面
                        if (HasFeatureWithInGeometry(pFeatLay.FeatureClass, orgFeat.Shape, strCon) == true)
                        {
                            bHasFeat = true;
                            break;
                        }
                    }

                    if (bHasFeat == false)
                    {
                        dicCheckFeats.Add(orgFeat, pOrgFeatCls);
                    }
                    pTopoErrFeat = pEnumTopoErrFeat.Next();
                }

                pTempTopologyRule = pEnumRule.Next() as ITopologyRule;
            }
                        
            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "处理数据中..." });
            pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, 0, dicCheckFeats.Count, 0 });

            Exception err = null;
            //开启编辑  wjj 20090921
            IDataset pFeatDataset = SetCheckState.Geodatabase as IDataset;
            IWorkspaceEdit pWorkspaceEdit = pFeatDataset.Workspace as IWorkspaceEdit;
            if (pWorkspaceEdit.IsBeingEdited() == false)
            {
                pWorkspaceEdit.StartEditing(false);
            }
            
            //遍历上步所获得到的重叠最小面，分析获取包含该面的所有面并根据面积排序
            intcnt = 0;
            foreach (KeyValuePair<IFeature,IFeatureClass> keyvlue in dicCheckFeats)
            {
                intcnt++;
                pAppFrm.MainForm.Invoke(new ChangeProgress(ChangeProgressBar), new object[] { pAppFrm.ProgressBar, -1, -1, intcnt });
                IFeature pFeat = keyvlue.Key;
                Dictionary<IFeature, IFeatureClass> dicFeats = GetFeaturesByGeometry(pFeat, listCheckLays);
                bool bDel = false;
                if (dicFeats.Count > 0)
                {
                    //从外到内做处理,处理原则分类代码相同则利用小面套空大面并删除小面,不同则小面套空大面
                    IFeature[] pFeatArray = new IFeature[dicFeats.Count];
                    dicFeats.Keys.CopyTo(pFeatArray, 0);
                    for (int i = 0; i < dicFeats.Count - 1;i++ )
                    {
                        if (bDel == false)
                        {
                            IFeature pBigFeat = pFeatArray[i];
                            IFeature pLowFeat = pFeatArray[i + 1];

                            //小面掏空大面
                            UpdateFeatureByOverlapArea(dicFeats[pBigFeat], "OBJECTID=" + pBigFeat.OID.ToString(), pLowFeat.Shape, out err);
                            if (pBigFeat.get_Value(pBigFeat.Fields.FindField("CODE")).ToString() == pLowFeat.get_Value(pLowFeat.Fields.FindField("CODE")).ToString())
                            {
                                SysCommon.Gis.ModGisPub.DelFeature(dicFeats[pLowFeat], "OBJECTID=" + pLowFeat.OID.ToString(), out err);
                                bDel = true;
                            }
                            else
                                bDel = false;
                        }
                        else
                            bDel = false;
                    }


                    IFeature pLastFeat = pFeatArray[dicFeats.Count - 1];
                    //小面掏空大面
                    UpdateFeatureByOverlapArea(dicFeats[pLastFeat], "OBJECTID=" + pLastFeat.OID.ToString(), pFeat.Shape, out err);
                    if (pLastFeat.get_Value(pLastFeat.Fields.FindField("CODE")).ToString() == pFeat.get_Value(pFeat.Fields.FindField("CODE")).ToString())
                    {
                        SysCommon.Gis.ModGisPub.DelFeature(dicCheckFeats[pFeat], "OBJECTID=" + pFeat.OID.ToString(), out err);
                        bDel = true;
                    }
                    else
                        bDel = false;
                }
            }

            pEnumRule.Reset();
            pTempTopologyRule = pEnumRule.Next() as ITopologyRule;
            while (pTempTopologyRule != null)
            {
                pTopologyRuleContainer.DeleteRule(pTempTopologyRule);
                pTempTopologyRule = pEnumRule.Next() as ITopologyRule;
            }

            //结束编辑 wjj 20090921
            if (pWorkspaceEdit.IsBeingEdited() == true)
            {
                pWorkspaceEdit.StopEditing(true);
            }

            _AppHk.CurrentThread = null;
            pAppFrm.MainForm.Invoke(new ShowTips(ShowStatusTips), new object[] { pAppFrm, "" });
            pAppFrm.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { pAppFrm, false });
            pAppFrm.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "处理完毕!" });
        }

        /// <summary>
        ///  利用图形对指定要素进行图形求差（小面掏空大面）
        /// </summary>
        /// <param name="pFeatueCls">要素类</param>
        /// <param name="strCon">指定要素条件</param>
        /// <param name="pGeometry">求差图形</param>
        private void UpdateFeatureByOverlapArea(IFeatureClass pFeatueCls, string strCon, IGeometry pGeometry,out Exception err)
        {
            err = null;
            try
            {
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = strCon;
                IFeatureCursor pFcursor = pFeatueCls.Update(pQf, false);
                IFeature pFeatTemp = pFcursor.NextFeature();
                if (pFeatTemp != null)
                {
                    ITopologicalOperator pTop = pFeatTemp.Shape as ITopologicalOperator;
                    pTop.Simplify();
                    pFeatTemp.Shape = pTop.Difference(pGeometry);
                    pFcursor.UpdateFeature(pFeatTemp);
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFcursor);
            }
            catch(Exception exError)
            {
                err = exError;
            }
        }

        /// <summary>
        /// 获取包含指定要素的所有要素，并根据面积排序
        /// </summary>
        /// <param name="pFeat">指定要素</param>
        /// <param name="listFeatCls">所有要素类集合</param>
        private Dictionary<IFeature, IFeatureClass> GetFeaturesByGeometry(IFeature pFeat, List<ILayer> listLayers)
        {
            IDataset pDatasetTemp = pFeat.Class as IDataset;
            //获取包含指定要素pFeat的所有要素
            Dictionary<IFeature, IFeatureClass> dicValueTemp = new Dictionary<IFeature, IFeatureClass>();
            foreach (ILayer pLay in listLayers)
            {
                IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                if (pFeatLay == null) continue;
                IFeatureClass pFeatCls = pFeatLay.FeatureClass;
                IDataset pDataset = pFeatCls as IDataset;
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pFeat.Shape;
                pSpatialFilter.GeometryField = "SHAPE";
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                //排除指定要素
                if (pDataset.Name == pDatasetTemp.Name)
                {
                    pSpatialFilter.WhereClause = "OBJECTID<>" + pFeat.OID.ToString();
                }

                IFeatureCursor pFeatCursor = pFeatCls.Search(pSpatialFilter, false);
                IFeature pFeature = pFeatCursor.NextFeature();
                while (pFeature != null)
                {
                    dicValueTemp.Add(pFeature, pFeatCls);
                    pFeature = pFeatCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCursor);
            }

            //对获取到的要素进行面积排序
            IFeature[] pFeatArray=new IFeature[dicValueTemp.Count];
            dicValueTemp.Keys.CopyTo(pFeatArray, 0);
            for (int i = 0; i < pFeatArray.Length; i++)
            {
                for (int j = i + 1; j < pFeatArray.Length; j++)
                {
                    double valueI=System.Math.Abs(Convert.ToDouble(pFeatArray[i].get_Value(pFeatArray[i].Fields.FindField("SHAPE_Area"))));
                    double valueJ=System.Math.Abs(Convert.ToDouble(pFeatArray[j].get_Value(pFeatArray[j].Fields.FindField("SHAPE_Area"))));
                    if (valueI < valueJ)
                    {
                        IClone pClone = pFeatArray[i] as IClone;
                        IFeature pFeatTemp = pClone.Clone() as IFeature;
                        pFeatArray[i] = pFeatArray[j];
                        pFeatArray[j] = pFeatTemp;
                    }
                }
            }

            Dictionary<IFeature, IFeatureClass> dicValue = new Dictionary<IFeature, IFeatureClass>();
            for (int i = 0; i < pFeatArray.Length; i++)
            {
                dicValue.Add(pFeatArray[i], dicValueTemp[pFeatArray[i]]);
            }

            return dicValue;
        }

        /// <summary>
        /// 判断要素类中是否包含图形（面内是否包含有面）
        /// </summary>
        /// <param name="pFeatureCls">要素类</param>
        /// <param name="pGeometry">图形</param>
        /// <param name="strCon">条件</param>
        /// <returns></returns>
        private bool HasFeatureWithInGeometry(IFeatureClass pFeatureCls,IGeometry pGeometry,string strCon)
        {
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.GeometryField = "SHAPE";
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            if (strCon != "")
            {
                pSpatialFilter.WhereClause = strCon;
            }

            int intCnt = pFeatureCls.FeatureCount(pSpatialFilter);
            if (intCnt == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取某Group层下的所有图层
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="strGroupLayName"></param>
        /// <returns></returns>
        public List<ILayer> GetCheckLayers(IMap pMap, string strGroupLayName)
        {
            List<ILayer> listLays = new List<ILayer>();
            for (int n = 0; n < pMap.LayerCount; n++)
            {
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer && layer.Name == strGroupLayName)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        listLays.Add(Comlayer.get_Layer(c));
                    }
                }
            }

            if (listLays.Count == 0) return null;
            return listLays;
        }

        #region 进程与界面控件响应实现
        //弹出提示对话框
        private delegate void ShowForm(string strCaption, string strText);
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }

        //改变进度条
        private delegate void ChangeProgress(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value);
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }

        //控制进度条显示
        private delegate void ShowProgress(Plugin.Application.IAppFormRef pAppFrm, bool bVisible);
        private void ShowProgressBar(Plugin.Application.IAppFormRef pAppFrm, bool bVisible)
        {
            if (bVisible == true)
            {
                pAppFrm.ProgressBar.Visible = true;
            }
            else
            {
                pAppFrm.ProgressBar.Visible = false;
            }
        }

        //改变状态栏提示内容
        private delegate void ShowTips(Plugin.Application.IAppFormRef pAppFrm, string strText);
        private void ShowStatusTips(Plugin.Application.IAppFormRef pAppFrm, string strText)
        {
            pAppFrm.OperatorTips = strText;
        }
        #endregion
    }
}
