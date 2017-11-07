using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
//using GeoProperties;

namespace GeoUtilities 
{
    public class ControlsLayerAtt : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        ICommand m_pCommand=null;

        public ControlsLayerAtt()
        {
            base._Name = "GeoUtilities.ControlsLayerAtt";
            base._Caption = "图层属性";
            base._Tooltip = "图层属性";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "图层属性";

            m_pCommand=new GeoProperties.LayerPropertiesTool();
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer ;
                if (mLayer == null) return false;
                if (mLayer is IGroupLayer) return false;
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
            if(m_pCommand==null) return;
            (m_pCommand as GeoProperties.LayerPropertiesTool).CurLayer=_AppHk.MapControl.CustomProperty as ILayer;

            // shduan 20110721 增加RasterLayer和RasterCatalog图层属性*************************
            if (_AppHk.MapControl.CustomProperty is IFeatureLayer)
            {
                IFeatureLayer pFeatLayer = _AppHk.MapControl.CustomProperty as IFeatureLayer;
                IFeatureLayerDefinition pFLDefinition = pFeatLayer as IFeatureLayerDefinition;

                IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
            }
            else if (_AppHk.MapControl.CustomProperty is  IRasterLayer )
            {
                IRasterLayer pRasterLayer = _AppHk.MapControl.CustomProperty as IRasterLayer;

            }
            else if (_AppHk.MapControl.CustomProperty is IRasterCatalog )
            {
                IRasterCatalog pRC = _AppHk.MapControl.CustomProperty as IRasterCatalog;
            }
            //end ******************************************************************************
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("打开" + Message);//xisheng 2011.07.08 增加日志
            }
            m_pCommand.OnClick();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            
            m_pCommand.OnCreate(_AppHk.MapControl);
        }
    }
}
