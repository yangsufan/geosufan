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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using GeoDataEdit.Tools;

namespace GeoDataEdit
{
    public class ControlsDrawRangePolygon : Plugin.Interface.ToolRefBase
    {

        private ITool _tool = null;
        private ICommand _cmd = null;
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        private ESRI.ArcGIS.SystemUI.ICommand m_pCommand;

        public ControlsDrawRangePolygon()
        {
            base._Name = "GeoDataEdit.ControlsDrawRangePolygon";
            base._Caption = "·¶Î§Ãè»æ";
            base._Tooltip = "·¶Î§Ãè»æ";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "·¶Î§Ãè»æ";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerIdentify;
            //base._Image = "";
            //base._Category = "";
            m_pCommand = new GeoDataEdit.Tools.ToolDrawPolygons();
        }

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
                    if (ToolDrawPolygons.ListGeometrys.Count > 0)
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
            if (_AppHk == null) return;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Caption);//xisheng ÈÕÖ¾¼ÇÂ¼
            }
            frmSelectType frm = new frmSelectType();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _AppHk.MapControl.CurrentTool = m_pCommand as ESRI.ArcGIS.SystemUI.ITool;
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;
            m_pCommand.OnCreate(_AppHk);
            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;

        }

    }

    
}
