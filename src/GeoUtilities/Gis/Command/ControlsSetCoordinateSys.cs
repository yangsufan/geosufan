using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
namespace GeoUtilities
{
    public class ControlsSetCoordinateSys : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;


        public ControlsSetCoordinateSys()
        {
            base._Name = "GeoUtilities.ControlsSetCoordinateSys";
            base._Caption = "设置空间参考";
            base._Tooltip = "设置空间参考";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置空间参考";
            //base._Image = "";
            //base._Category = "";
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
            if ( _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            //OpenFileDialog OpenFile = new OpenFileDialog();
            //OpenFile.Title = "选择空间参考";
            //OpenFile.Filter = "空间参考文件(*.prj)|*.prj";
            ////设置map的空间参考
            //if (OpenFile.ShowDialog() == DialogResult.OK)
            //{
            //    Plugin.LogTable.Writelog(base._Tooltip);//xisheng 2011.07.08 增加日志
            //    string prjFile = OpenFile.FileName;
            //    try
            //    {
            //        ISpatialReferenceFactory pRef = new SpatialReferenceEnvironmentClass();
            //        ISpatialReference pSpatialRef = pRef.CreateESRISpatialReferenceFromPRJFile(prjFile);
            //        if (pSpatialRef != null)
            //        {
            //            _AppHk.MapControl.Map.SpatialReference = pSpatialRef;
            //            _AppHk.MapControl.ActiveView.Refresh();
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    catch(Exception e)
            //    {
            //        MessageBox.Show("设置失败，问题原因：'" + e.Message + "'", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    MessageBox.Show("设置成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            FrmDisplayPrj fmSetCoorSys = new FrmDisplayPrj(_AppHk.MapControl.Map);
            fmSetCoorSys.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            //点击确认后地图刷新 20110801
            if (fmSetCoorSys.ShowDialog() == DialogResult.OK && fmSetCoorSys.hasSet)
            {
                _AppHk.MapControl.ActiveView.Refresh();
            }
            //xisheng end 
            

            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

        }

    }
}
