using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    public class ControlsLineBufferQueryToolex : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;
        public ControlsLineBufferQueryToolex()
        {
            base._Name = "GeoDataCenterFunLib.ControlsLineBufferQueryToolex";
            base._Caption = "线缓冲查询";
            base._Tooltip = "线缓冲查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "线缓冲查询";

            //base._Cursor=new Cursor(GetType(), "Resources.BufferQuery.cur");
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.Map.LayerCount == 0)
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
            Plugin.Application.AppGIS phook = _AppHk as Plugin.Application.AppGIS;
            SysCommon.BottomQueryBar pBar= phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            LineBufferQueryToolClass pTool = _cmd as LineBufferQueryToolClass;
            pTool.WriteLog = this.WriteLog; //ygc 2012-9-12 是否写日志
            pTool.GetQueryBar(pBar);
            _cmd.OnClick();
            //if (_AppHk.CurrentControl is IMapControl2)
            //{
               _AppHk.MapControl.CurrentTool = _tool;
            //}
            //else
            //{
            //    _AppHk.PageLayoutControl.CurrentTool = _tool;
            //}

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new LineBufferQueryToolClass(pAppForm.MainForm);
            LineBufferQueryToolClass TempTool = _tool as LineBufferQueryToolClass;
            TempTool.WriteLog = WriteLog;
            _cmd = TempTool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }
}
