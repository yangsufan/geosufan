using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
namespace GeoDataEdit
{
    public class SaveFeatureEdit : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SaveFeatureEdit()
        {
            base._Name = "GeoDataEdit.SaveFeatureEdit";
            base._Caption = "保存图元编辑";
            base._Tooltip = "保存图元编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "保存图元编辑";
        }
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook.MapControl.Map.LayerCount == 0)
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
        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("鹰眼图设置");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                IWorkspaceEdit iWE = ((getEditLayer.isExistLayer(m_Hook.ArcGisMapControl.Map) as IFeatureLayer).FeatureClass as IDataset).Workspace as IWorkspaceEdit;
                if(iWE.IsBeingEdited())
                {
                    bool hasEdits=false;
                    iWE.HasEdits(ref hasEdits);
                    if (hasEdits && MessageBox.Show("是否保存编辑？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        iWE.StopEditing(true);
                        Plugin.LogTable.Writelog("保存编辑");
                    }
                    else
                    {
                        iWE.StopEditing(false);
                        Plugin.LogTable.Writelog("未保存编辑");
                    }
                }
                m_Hook.ArcGisMapControl.ActiveView.Refresh();
                iWE = null;
             
            }
            catch
            { 
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}