using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDataChecker
{
    /// <summary>
    /// 拓扑修正功能，主要检查出如零长度，零面积，空图形，负面逆向修正等功能
    /// 编写人：陈胜朋
    /// 修整：王冰
    /// </summary>
    public class GeometryCorrector : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;


        public GeometryCorrector()
        {
            base._Name = "GeoDataChecker.GeometryCorrector";
            base._Caption = "模型拓扑修正";
            base._Tooltip = "Geodatabase模型的拓扑修正";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "模型拓扑修正";
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
                        //if (SetCheckState.CheckState)
                        //{
                        base._Enabled = true;
                        return true;
                        //}
                        //else
                        //{
                        //    base._Enabled = false;
                        //    return false;
                        //}
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

            //执行修正图形的处理
            Plugin.Application.IAppGISRef Apphook = _AppHk as Plugin.Application.IAppGISRef;

            try
            {
                System.Threading.ParameterizedThreadStart start = new System.Threading.ParameterizedThreadStart(ExcuteCorrect);
                System.Threading.Thread thread = new System.Threading.Thread(start);
                if (Apphook.DataCheckGrid.Rows.Count > 0)
                {
                    Apphook.DataCheckGrid.DataSource = null;//拓扑是不显示出错的，直接修正0或没用的要素
                }
                Apphook.CurrentThread = thread;
                thread.Start(_AppHk as object);

            }
            catch (Exception ex)
            {
                if (Apphook.CurrentThread != null) Apphook.CurrentThread = null;
                MessageBoxEx.Show(ex.ToString());
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
        /// 修正处理函数，参数框架为主窗体，以获得图层（取得待处理的要素）和进度条

        /// </summary>
        /// <param name="AppHook"></param>

        private void ExcuteCorrect(object hook)
        {
            SetCheckState.GeoCor = true;//拓扑修正里用到开启和关闭编辑
            Plugin.Application.IAppGISRef AppHook = hook as Plugin.Application.IAppGISRef;
            #region 取得进度条对象

            //取得进度条对象

            Plugin.Application.IAppFormRef pAppForm = AppHook as Plugin.Application.IAppFormRef;
            #endregion
            Plugin.Application.IAppGISRef appHook = hook as Plugin.Application.IAppGISRef;

            #region 获得待处理的图层

            //判断图层个数是否为0
            if (AppHook.MapControl.LayerCount == 0) return;
            IWorkspaceEdit pWorkspaceEdit = null;

            #region 循环图层进行处理，并控制进度条

            for (int i = 0; i < AppHook.MapControl.LayerCount; i++)
            {
                ESRI.ArcGIS.Carto.ILayer pLayer = AppHook.MapControl.get_Layer(i);
                bool isShpFile = false;

                //操作更新库体下的层 && pLayer.Name == SetCheckState.CheckDataBaseName

                #region 针对的是IGrouplayer下的层进行拓扑修正
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer C_layer = pLayer as ICompositeLayer;
                    int count = C_layer.Count;//得到组合图层数
                    if (count == 0) return;
                    for (int c = 0; c < count; c++)
                    {
                        ESRI.ArcGIS.Carto.IFeatureLayer pFeatLayer = C_layer.get_Layer(c) as IFeatureLayer;
                        #region 图形简单化，删除空图形，删除零长度线，删除零面积面，负面积面逆向
                        if (pFeatLayer == null) continue;
                        IFeatureClass pFeatureClass = pFeatLayer.FeatureClass;

                        //开启编辑  wjj 20090921
                        IDataset pDataset = pFeatureClass as IDataset;
                        pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                        if (pWorkspaceEdit.IsBeingEdited() == false)
                        {
                            pWorkspaceEdit.StartEditing(false);
                        }

                        //判断图层类型，线或者面才进行处理

                        if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                        {
                            IFeatureCursor pFeatCur = pFeatureClass.Search(null, false);

                            if (pFeatCur != null)
                            {
                                IFeature pFeat = pFeatCur.NextFeature();

                                if (pFeat != null)
                                {
                                    do
                                    {
                                        //删除空几何图形

                                        if (pFeat.Shape.IsEmpty)
                                        {
                                            pFeat.Delete();
                                            //写状态栏信息
                                            string del = pFeatureClass.AliasName + ": 删除空几何图形,OID:" + pFeat.OID;
                                            SetCheckState.CheckShowTips(pAppForm, del);
                                        }
                                        else
                                        {
                                            ITopologicalOperator pTopo = pFeat.Shape as ITopologicalOperator;
                                            if (pTopo != null)
                                            {
                                                //图形简单化
                                                pTopo.Simplify();
                                                //写状态栏信息
                                                string simplify = pFeatureClass.AliasName + ": 图形简单化,OID:" + pFeat.OID;
                                                SetCheckState.CheckShowTips(pAppForm, simplify);
                                            }
                                        }

                                        pFeat = pFeatCur.NextFeature();

                                    } while (pFeat != null);
                                }
                            }

                            #region 删除长度为零的线要素
                            //删除长度为零的要素

                            IQueryFilter pFilter = new QueryFilterClass();
                            pFilter.WhereClause = pFeatureClass.LengthField.Name + "=0 or " + pFeatureClass.LengthField.Name + " is null";

                            pFeatCur = pFeatureClass.Search(pFilter, false);

                            if (pFeatCur != null)
                            {
                                IFeature pFeat = pFeatCur.NextFeature();

                                if (pFeat != null)
                                {
                                    do
                                    {
                                        pFeat.Delete();
                                        //写状态栏信息
                                        string zero = pFeatureClass.AliasName + ": 删除长度为零的线要素,OID:" + pFeat.OID;
                                        SetCheckState.CheckShowTips(pAppForm, zero);
                                        pFeat = pFeatCur.NextFeature();

                                    } while (pFeat != null);
                                }
                            }

                            #endregion

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);
                        }
                        else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            IFeatureCursor pFeatCur = pFeatureClass.Search(null, false);
                            if (pFeatCur != null)
                            {
                                IFeature pFeat = pFeatCur.NextFeature();

                                if (pFeat != null)
                                {
                                    do
                                    {
                                        //删除空几何图形

                                        if (pFeat.Shape.IsEmpty)
                                        {
                                            pFeat.Delete();
                                            //写状态栏信息
                                            string delps = pFeatureClass.AliasName + ": 删除空几何图形,OID:" + pFeat.OID;
                                            SetCheckState.CheckShowTips(pAppForm, delps);
                                        }
                                        else
                                        {
                                            ITopologicalOperator pTopo = pFeat.Shape as ITopologicalOperator;
                                            if (pTopo != null)
                                            {
                                                //图形简单化
                                                pTopo.Simplify();
                                                //写状态栏信息
                                                string simplifyps = pFeatureClass.AliasName + ": 图形简单化,OID:" + pFeat.OID;
                                                SetCheckState.CheckShowTips(pAppForm, simplifyps);
                                            }
                                        }

                                        pFeat = pFeatCur.NextFeature();

                                    } while (pFeat != null);
                                }
                            }

                            #region 负面积面点序逆向
                            IQueryFilter pFilter = new QueryFilterClass();
                            pFilter.WhereClause = pFeatureClass.AreaField.Name + "<0";

                            pFeatCur = pFeatureClass.Search(pFilter, false);

                            if (pFeatCur != null)
                            {
                                IFeature pFeat = pFeatCur.NextFeature();
                                if (pFeat != null)
                                {
                                    do
                                    {
                                        //点序逆向
                                        ICurve pCurv = pFeat.Shape as ICurve;

                                        if (pCurv != null)
                                        {
                                            pCurv.ReverseOrientation();
                                            pFeat.Shape = pCurv;
                                            pFeat.Store();
                                            //写状态栏信息
                                            string reverse = pFeatureClass.AliasName + ": 点序逆向,OID:" + pFeat.OID;
                                            SetCheckState.CheckShowTips(pAppForm, reverse);
                                        }

                                        pFeat = pFeatCur.NextFeature();
                                    } while (pFeat != null);

                                }

                            }

                            #endregion

                            //删除面积为零的要素

                            pFilter.WhereClause = pFeatureClass.AreaField.Name + "=0 or " + pFeatureClass.AreaField.Name + " is null";

                            pFeatCur = pFeatureClass.Search(pFilter, false);

                            if (pFeatCur != null)
                            {
                                IFeature pFeat = pFeatCur.NextFeature();

                                if (pFeat != null)
                                {
                                    do
                                    {
                                        pFeat.Delete();
                                        //写状态栏信息
                                        string delarea = pFeatureClass.AliasName + ": 删除面积为零的要素,OID:" + pFeat.OID;
                                        SetCheckState.CheckShowTips(pAppForm, delarea);
                                        pFeat = pFeatCur.NextFeature();

                                    } while (pFeat != null);
                                }
                            }

                            //释放cursor
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);

                        #endregion
                        }

                        //结束编辑 wjj 20090921
                        if (pWorkspaceEdit.IsBeingEdited() == true)
                        {
                            pWorkspaceEdit.StopEditing(true);
                        }
                    }
                    SetCheckState.CheckShowTips(pAppForm, "拓扑修正检查完成！");
                    appHook.CurrentThread = null;
                    SetCheckState.Message(pAppForm, "提示", "拓扑修正检查完成！");
                    SetCheckState.GeoCor = false;//用完判断是否有编辑操作状态后还原
                    break;
                }
                #endregion
                else
                {
                    ESRI.ArcGIS.Carto.IFeatureLayer pFeatLayer = pLayer as IFeatureLayer;
                    #region 图形简单化，删除空图形，删除零长度线，删除零面积面，负面积面逆向
                    if (pFeatLayer == null) continue;
                    IFeatureClass pFeatureClass = pFeatLayer.FeatureClass;

                    //开启编辑  wjj 20090921
                    IDataset pDataset = pFeatureClass as IDataset;

                    #region 陈胜鹏添加shp文件类型的判断 2010-3-10
                    if (pFeatLayer.DataSourceType == "Shapefile Feature Class")
                    {
                        isShpFile = true;
                    }
                    #endregion

                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(false);
                    }

                    //判断图层类型，线或者面才进行处理

                    if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        IFeatureCursor pFeatCur = pFeatureClass.Search(null, false);

                        if (pFeatCur != null)
                        {
                            IFeature pFeat = pFeatCur.NextFeature();

                            if (pFeat != null)
                            {
                                do
                                {
                                    //删除空几何图形

                                    if (pFeat.Shape.IsEmpty)
                                    {
                                        pFeat.Delete();
                                        //写状态栏信息
                                        string del = pFeatureClass.AliasName + ": 删除空几何图形,OID:" + pFeat.OID;
                                        SetCheckState.CheckShowTips(pAppForm, del);
                                    }
                                    else
                                    {
                                        ITopologicalOperator pTopo = pFeat.Shape as ITopologicalOperator;
                                        if (pTopo != null)
                                        {
                                            //图形简单化
                                            pTopo.Simplify();
                                            //写状态栏信息
                                            string simplify = pFeatureClass.AliasName + ": 图形简单化,OID:" + pFeat.OID;
                                            SetCheckState.CheckShowTips(pAppForm, simplify);
                                        }
                                    }

                                    pFeat = pFeatCur.NextFeature();

                                } while (pFeat != null);
                            }
                        }

                        #region 删除长度为零的线要素
                        //删除长度为零的要素

                        IQueryFilter pFilter = new QueryFilterClass();

                        if (isShpFile)
                        {
                            pFilter.WhereClause = "SHAPE_Leng" + "=0 or " + "SHAPE_Leng" + " is null";

                        }
                        else
                        {
                            pFilter.WhereClause = pFeatureClass.LengthField.Name + "=0 or " + pFeatureClass.LengthField.Name + " is null";
                        }

                        pFeatCur = pFeatureClass.Search(pFilter, false);

                        if (pFeatCur != null)
                        {
                            IFeature pFeat = pFeatCur.NextFeature();

                            if (pFeat != null)
                            {
                                do
                                {
                                    pFeat.Delete();
                                    //写状态栏信息
                                    string zero = pFeatureClass.AliasName + ": 删除长度为零的线要素,OID:" + pFeat.OID;
                                    SetCheckState.CheckShowTips(pAppForm, zero);
                                    pFeat = pFeatCur.NextFeature();

                                } while (pFeat != null);
                            }
                        }

                        #endregion

                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);

                    }
                    else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pFeatCur = pFeatureClass.Search(null, false);
                        if (pFeatCur != null)
                        {
                            IFeature pFeat = pFeatCur.NextFeature();

                            if (pFeat != null)
                            {
                                do
                                {
                                    //删除空几何图形

                                    if (pFeat.Shape.IsEmpty)
                                    {
                                        pFeat.Delete();
                                        //写状态栏信息
                                        string delps = pFeatureClass.AliasName + ": 删除空几何图形,OID:" + pFeat.OID;
                                        SetCheckState.CheckShowTips(pAppForm, delps);
                                    }
                                    else
                                    {
                                        ITopologicalOperator pTopo = pFeat.Shape as ITopologicalOperator;
                                        if (pTopo != null)
                                        {
                                            //图形简单化
                                            pTopo.Simplify();
                                            //写状态栏信息
                                            string simplifyps = pFeatureClass.AliasName + ": 图形简单化,OID:" + pFeat.OID;
                                            SetCheckState.CheckShowTips(pAppForm, simplifyps);
                                        }
                                    }

                                    pFeat = pFeatCur.NextFeature();

                                } while (pFeat != null);
                            }
                        }

                        #region 负面积面点序逆向
                        IQueryFilter pFilter = new QueryFilterClass();

                        if (isShpFile)
                        {
                            pFilter.WhereClause = "SHAPE_Area" + "<0";
                        }
                        else
                        {
                            pFilter.WhereClause = pFeatureClass.AreaField.Name + "<0";
                        }

                        pFeatCur = pFeatureClass.Search(pFilter, false);

                        if (pFeatCur != null)
                        {
                            IFeature pFeat = pFeatCur.NextFeature();
                            if (pFeat != null)
                            {
                                do
                                {
                                    //点序逆向
                                    ICurve pCurv = pFeat.Shape as ICurve;

                                    if (pCurv != null)
                                    {
                                        pCurv.ReverseOrientation();
                                        pFeat.Shape = pCurv;
                                        pFeat.Store();
                                        //写状态栏信息
                                        string reverse = pFeatureClass.AliasName + ": 点序逆向,OID:" + pFeat.OID;
                                        SetCheckState.CheckShowTips(pAppForm, reverse);
                                    }

                                    pFeat = pFeatCur.NextFeature();
                                } while (pFeat != null);

                            }

                        }

                        #endregion

                        //删除面积为零的要素

                        if (isShpFile)
                        {
                            pFilter.WhereClause = "SHAPE_Area" + "=0 or " + "SHAPE_Area" + " is null";
                        }
                        else
                        {
                            pFilter.WhereClause = pFeatureClass.AreaField.Name + "=0 or " + pFeatureClass.AreaField.Name + " is null";
                        }

                        pFeatCur = pFeatureClass.Search(pFilter, false);

                        if (pFeatCur != null)
                        {
                            IFeature pFeat = pFeatCur.NextFeature();

                            if (pFeat != null)
                            {
                                do
                                {
                                    pFeat.Delete();
                                    //写状态栏信息
                                    string delarea = pFeatureClass.AliasName + ": 删除面积为零的要素,OID:" + pFeat.OID;
                                    SetCheckState.CheckShowTips(pAppForm, delarea);
                                    pFeat = pFeatCur.NextFeature();

                                } while (pFeat != null);
                            }
                        }
                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCur);
                    }
                    #endregion
                }
            }
            //结束编辑 wjj 20090921
            if (pWorkspaceEdit.IsBeingEdited() == true)
            {
                pWorkspaceEdit.StopEditing(true);
                SetCheckState.CheckShowTips(pAppForm, "拓扑修正检查完成！");
                appHook.CurrentThread = null;
                SetCheckState.Message(pAppForm, "提示", "拓扑修正检查完成！");
                SetCheckState.GeoCor = false;//用完判断是否有编辑操作状态后还原
            }
            #endregion
            #endregion




        }

    }

}
