using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GeoHistory
{
    public class CommandExtentView : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private FrmHistoryMapView m_frmHistoryMapView;
        private DevComponents.AdvTree.AdvTree _ProjectTree = null;
        public CommandExtentView()
        {
            base._Name = "GeoHistory.CommandExtentView";
            base._Caption = "历史数据管理";
            base._Tooltip = "历史数据管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "历史数据管理";
            
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return true;
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
            //如果工作库数据group图层已经存在 就不让再加了
            bool hasCurData = false, hasHisData = false; ;
            for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
            {
                ILayer mLayer = _AppHk.MapControl.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    if (mLayer.Name == "现势库数据")
                    {
                        hasCurData = true;
                    }
                    else if (mLayer.Name == "历史库数据")
                    {
                        hasHisData = true;
                    }
                }
            }
            if (!hasCurData)
            {
                ControlsAddCurrentDataBase cacdb = new ControlsAddCurrentDataBase();
                cacdb.OnCreate(_AppHk);
                cacdb.OnClick();
            }
            if (!hasHisData)
            {
                ControlsAddHistoryDataBase cahdb = new ControlsAddHistoryDataBase();
                cahdb.OnCreate(_AppHk);
                cahdb.OnClick();
            }

            Plugin.Application.AppGIS pApp = _AppHk as Plugin.Application.AppGIS;
            if (pApp != null)
            {
                _ProjectTree = pApp.ProjectTree;
            }
            showHistoryMapView();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
        private void showHistoryMapView()
        {
            if (m_frmHistoryMapView == null || m_frmHistoryMapView.IsDisposed)
            {
                AxMapControl MapMain = _AppHk.ArcGisMapControl;
                IObjectCopy pOC = new ObjectCopyClass();
                IMap pCopy = pOC.Copy(MapMain.Map) as IMap;
                ILayer pLL = null;
                for (int i = 0; i < pCopy.LayerCount; i++)
                { 
                    ILayer pLayer=pCopy.get_Layer(i);
                    if (pLayer.Name == "示意图")
                    {
                        pLL = pLayer;
                        ICompositeLayer pCL = pLayer as ICompositeLayer;
                        IGroupLayer pGL = pLayer as IGroupLayer;
                        for (int j = 0; j < pCL.Count; j++)
                        {
                            ILayer pL = pCL.get_Layer(j);
                            if (pL.Name.ToUpper() == "NJTDT.ZONE")
                            {
                                IFeatureRenderer pSR = new SimpleRendererClass();
                                IGeoFeatureLayer pGEOL = pL as IGeoFeatureLayer;
                                pGEOL.Renderer = pSR;
                            }
                            //else if (pL.Name.ToUpper() == "NJTDT.JFB")
                            //{
                            //    IFeatureClass pFC = (ModData.v_SysDataSet.WorkSpace as IFeatureWorkspace).OpenFeatureClass("JFB");
                            //    if (pFC != null)
                            //        (pL as IFeatureLayer).FeatureClass = pFC;
                            //}
                            else
                            {
                                pGL.Delete(pL);
                            }
                        }
                    }
                }
                //pCopy.DeleteLayer(pLL);
                m_frmHistoryMapView = new FrmHistoryMapView(MapMain.Extent, pCopy, _ProjectTree);
                //m_frmHistoryMapView.clsMain = clsMain;
                //m_frmHistoryMapView.MainMapExtent = MapMain.Extent;

                //m_frmHistoryMapView.HistoryMap = pCopy;
                m_frmHistoryMapView.Show();
                ModHistory.SetForegroundWindow(m_frmHistoryMapView.Handle);
            }
            else
            {
                m_frmHistoryMapView.Visible = true;
                ModHistory.SetForegroundWindow(m_frmHistoryMapView.Handle);
            }
        }

    }
}
