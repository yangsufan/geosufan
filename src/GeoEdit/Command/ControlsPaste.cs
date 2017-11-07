using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    public class ControlsPaste : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;

        private static Dictionary<IGeometry, IElement> _GeometriesToBePasted;
        public static Dictionary<IGeometry, IElement> GeometriesToBePasted
        {
            get { return _GeometriesToBePasted; }
            set { _GeometriesToBePasted=value;}
        }

        public ControlsPaste()
        {
            base._Name = "GeoEdit.ControlsPaste";
            base._Caption = "粘贴";
            base._Tooltip = "粘贴";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "粘贴";

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

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                if (_GeometriesToBePasted == null) return false;
                if (_GeometriesToBePasted.Count== 0) return false;
                return true;
            }
        }

        public override void OnClick()
        {
            IFeatureClass fc = (MoData.v_CurLayer as IFeatureLayer).FeatureClass;
            foreach (KeyValuePair<IGeometry, IElement> item in GeometriesToBePasted)
            {
                if (fc.ShapeType!= item.Key.GeometryType)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "几何类型不匹配！");
                    return;
                }
            }

            MoData.v_CurWorkspaceEdit.StartEditOperation();
            IFeatureCursor cursor = fc.Insert(true);

            IFeatureBuffer buffer = fc.CreateFeatureBuffer();
            IFeature feature = buffer as IFeature;

            foreach (KeyValuePair<IGeometry,IElement> item in GeometriesToBePasted)
            {
                Exception Err = null;
                try
                {
                    feature.Shape = item.Key;
                }
                catch (Exception exErr)
                {
                    //******************************************
                    //guozheng added System Exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(exErr);
                    //******************************************

                    Err = exErr;
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", exErr.Message);
                }

                if (Err == null)
                {
                    if (feature.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        IAnnotationFeature af = feature as IAnnotationFeature;
                        af.Annotation = item.Value;

                        feature = af as IFeature;
                        
                    }

                    buffer = feature as IFeatureBuffer;
                    cursor.InsertFeature(buffer);
                }
            }

            cursor.Flush();
            MoData.v_CurWorkspaceEdit.StopEditOperation();
            myHook.MapControl.ActiveView.Refresh();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;
            myHook.ArcGisMapControl.OnKeyDown += new IMapControlEvents2_Ax_OnKeyDownEventHandler(ArcGisMapControl_OnKeyDown);
        }

        private void ArcGisMapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (this.Enabled && e.shift == 2 && e.keyCode == 86)
            {
                this.OnClick();
            }
        }
    }
}
