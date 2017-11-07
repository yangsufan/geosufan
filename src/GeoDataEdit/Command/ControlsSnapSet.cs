using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;

namespace GeoDataEdit
{
    public class ControlsSnapSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef myHook;
        public ControlsSnapSet()
        {
            base._Name = "GeoDataEdit.ControlsSnapSet";
            base._Caption = "≤∂◊Ω…Ë÷√";
            base._Tooltip = "≤∂◊Ω…Ë÷√";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "≤∂◊Ω…Ë÷√";

        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (myHook.MapControl == null) return false;
                //if (MoData.v_CurWorkspaceEdit == null) return false;
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
            Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
            IToolbarControl pTC = new ToolbarControlClass();
            ClsEditorMain clsEM = new ClsEditorMain(pTC, myHook.ArcGisMapControl, pAppFormRef.MainForm);
            IHookHelper pHH = new HookHelperClass();
            pHH.Hook = myHook.ArcGisMapControl.Object;
            clsEM.HookHelper = pHH;
            frmEditSnapAttri fmESA = new frmEditSnapAttri(clsEM);
            fmESA.ShowDialog(pAppFormRef.MainForm);
            ICommand createPolygon = new CreatePolygonTool();
            (createPolygon as CreatePolygonTool).setClsEditorMain(clsEM);
            createPolygon.OnCreate(myHook.ArcGisMapControl.Object);
            createPolygon.OnClick();
            myHook.ArcGisMapControl.CurrentTool = createPolygon as ITool;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
         
            myHook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
