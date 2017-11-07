using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

/*---------------------------------------------------------------
 added by xisheng 20110730 坐标定位菜单文件 ControlsXYLocation.cs
 ---------------------------------------------------------------*/
namespace GeoDataCenterFunLib
{
    public class ControlsXYLocation : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;

        public ControlsXYLocation()
        {
            base._Name = "GeoDataCenterFunLib.ControlsXYLocation";
            base._Caption = "坐标定位";
            base._Tooltip = "坐标定位";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "坐标定位";
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
            if (_AppHk == null) 
                return;
           frmXYLocation frm=new frmXYLocation(_AppHk.ArcGisMapControl);
           frm.WriteLog = this.WriteLog;
           frm.p1 = _AppHk.p1; frm.p2 = _AppHk.p2; frm.p3 = _AppHk.p3; frm.p4 = _AppHk.p4;
           frm.Show((_AppHk as Plugin.Application.IAppFormRef ).MainForm );
           
           
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;
            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
       



        }



    }

