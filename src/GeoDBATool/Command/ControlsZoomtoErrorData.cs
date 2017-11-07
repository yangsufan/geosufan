using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public class ControlsZoomtoErrorData: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        private IFeatureLayer  m_CurLayer;
        int m_OID = -1;
        public ControlsZoomtoErrorData()
        {
            base._Name = "GeoDBATool.ControlsZoomtoErrorData";
            base._Caption = "定位到出错要素";
            base._Tooltip = "定位到出错要素";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "根据要素OID定位到出错要素位置";
            
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    string pFeaclsName = "";
                    m_CurLayer = null;
                    if (m_Hook == null) return false;
                    if (m_Hook.MapControl == null || m_Hook.DataCheckGrid == null) return false;
                    if (m_Hook.DataCheckGrid.SelectedRows.Count == 0 || m_Hook.DataCheckGrid.SelectedRows.Count > 1) return false;
                    if (m_Hook.DataCheckGrid.SelectedRows.Count == 1)
                    {
                        pFeaclsName = m_Hook.DataCheckGrid.SelectedRows[0].Cells["数据图层1"].FormattedValue.ToString().Trim();
                        m_OID = Convert.ToInt32(m_Hook.DataCheckGrid.SelectedRows[0].Cells["要素OID1"].FormattedValue.ToString().Trim());

                    }
                    if (pFeaclsName == "") return false;
                    if (m_OID == -1) return false;

                    List<IFeatureLayer> fLayers = new List<IFeatureLayer>();
                    fLayers = GetAllFLayers(m_Hook);
                    if (fLayers.Count == 0) return false;
                    if (fLayers == null) return false;
                    for (int i = 0; i < fLayers.Count; i++)
                    {
                        IFeatureClass pfeaCls = fLayers[i].FeatureClass;
                        IDataset pDT = pfeaCls as IDataset;
                        string tempName = ""; //图层名
                        tempName = pDT.Name;
                        if (tempName.Contains("."))
                        {
                            tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                        }
                        if (tempName == pFeaclsName)
                        {
                            m_CurLayer = fLayers[i];
                            break;
                        }
                    }
                    if (m_CurLayer == null) return false;
                    return true;
                }
                catch(Exception e)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    //********************************************************************
                    return false;
                }
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
           
            IFeatureClass pFeatCls = m_CurLayer.FeatureClass;
            
            try
            {
                IGeoDataset pGeoDT = pFeatCls as IGeoDataset;
                ISpatialReference pSpatialRef = null;
                if (pGeoDT != null)
                {
                    pSpatialRef = pGeoDT.SpatialReference;
                }
                IFeature pFeature = pFeatCls.GetFeature(m_OID);
                m_Hook.MapControl.Map.ClearSelection();
                m_Hook.MapControl.Map.SelectFeature(m_CurLayer, pFeature);
                SysCommon.Gis.ModGisPub.ZoomToFeature(m_Hook.MapControl, pFeature, pSpatialRef);
                IGraphicsContainer pGra = m_Hook.MapControl.Map as IGraphicsContainer;
                MakeSymbol(pGra, pFeature);
            }
            catch (Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示",ex.Message);
                return;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }

        private List<IFeatureLayer> GetAllFLayers(Plugin.Application.IAppGISRef hook)
        {
            List<IFeatureLayer> LstFLayer = new List<IFeatureLayer>();
            IFeatureLayer pFLayer = null;
            for (int i = 0; i < hook.MapControl.Map.LayerCount; i++)
            {
                ILayer pLayer = hook.MapControl.Map.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        IFeatureLayer ppFLayer = mLayer as IFeatureLayer;
                        if (ppFLayer == null)
                        {
                            continue;
                        }
                        LstFLayer.Add(ppFLayer);
                    }
                }
                else
                {
                    pFLayer = pLayer as IFeatureLayer;
                    if (pFLayer == null)
                    {
                        continue;
                    }
                    LstFLayer.Add(pFLayer);
                }
            }
            return LstFLayer;
        }

        /// <summary>
        /// 通过添加Element方式高亮显示错误要素(解决点在特定比例尺中未显示问题)
        /// </summary>
        /// <param name="pGra"></param>
        /// <param name="in_fea"></param>
        private void MakeSymbol(IGraphicsContainer pGra, IFeature in_fea)
        {
            if (in_fea == null) return;
            pGra.DeleteAllElements();
            IActiveView pAcitveView = pGra as IActiveView;
            IElement pEle;
            IRgbColor pColor = new RgbColorClass();
            pColor.Blue = 255;
            pColor.Green = 200;
            pColor.Red = 0;

            if (in_fea.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                IMarkerElement pMakEle = new MarkerElementClass();
                pEle = pMakEle as IElement;
                IMarkerSymbol pMakSym = new SimpleMarkerSymbolClass();
                pMakSym.Color = pColor;
                pMakEle.Symbol = pMakSym;
                pEle.Geometry = in_fea.Shape as ESRI.ArcGIS.Geometry.IGeometry;
                pGra.AddElement(pEle, 0);
                pAcitveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }           
            
        }
    }

}
