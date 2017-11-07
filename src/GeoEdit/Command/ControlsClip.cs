using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoEdit
{
    public class ControlsClip : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        public ControlsClip()
        {
            base._Name = "GeoEdit.ControlsClip";
            base._Caption = "剪切";
            base._Tooltip = "剪切";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "剪切";
            
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
                if (myHook.MapControl.Map.SelectionCount == 0) return false;
                return true;
            }
        }

        public override void OnClick()
        {

            Dictionary<IGeometry, IElement> ges = new Dictionary<IGeometry, IElement>();

            MoData.v_CurWorkspaceEdit.StartEditOperation();

            IEnumFeature features = myHook.MapControl.Map.FeatureSelection as IEnumFeature;
            features.Reset();
            IFeature feature = features.Next();
            while (feature != null)
            {
                IDataset pDataset = feature.Class as IDataset;
                IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit.IsBeingEdited())
                {
                    IElement ele = null;
                    if (feature is IAnnotationFeature)
                    {
                        IAnnotationFeature af = feature as IAnnotationFeature;
                        ele = af.Annotation;
                    }

                    ges.Add(feature.ShapeCopy, ele);
                    try
                    {
                        feature.Delete();
                    }
                    catch(Exception eError)
                    {
                        //******************************************
                        //guozheng added System Exception log
                        if (SysCommon.Log.Module.SysLog == null)
                            SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        SysCommon.Log.Module.SysLog.Write(eError);
                        //******************************************

                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "剪切失败！\n请使用‘数据编辑’中的‘要素编辑’选择要素进行剪切操作。");
                    }
                }
                feature = features.Next();
            }

            MoData.v_CurWorkspaceEdit.StopEditOperation();
            ControlsPaste.GeometriesToBePasted = ges;

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
            if (this.Enabled && e.shift == 2 && e.keyCode == 88)
            {
                this.OnClick();
            }
        }
    }
}
