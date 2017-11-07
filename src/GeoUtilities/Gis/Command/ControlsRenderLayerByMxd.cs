using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoUtilities
{
    public class ControlsRenderLayerByMxd : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsRenderLayerByMxd()
        {
            base._Name = "GeoUtilities.ControlsRenderLayerByMxd";
            base._Caption = "数据符号化";
            base._Tooltip = "数据符号化";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据符号化";
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk == null) return false;
                    if (_AppHk.MapControl == null) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

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
            //OpenFileDialog OpenFile = new OpenFileDialog();
            //OpenFile.Title = "选择ArcGis MXD文档";
            //OpenFile.Filter = "MXD文档(*.mxd)|*.mxd";
            //OpenFile.CheckFileExists = true;


            //if (OpenFile.ShowDialog() == DialogResult.OK)
            //{
                Exception err=null;
                SysCommon.Gis.ModGisPub.RenderLayerByMxd(Application.StartupPath + "\\..\\Res\\rule\\符号显示规则\\Symbol.mxd", _AppHk.MapControl, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", err.Message);
                    return;
                }

                _AppHk.MapControl.ActiveView.Refresh();
                _AppHk.TOCControl.Update();
            //}      
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
