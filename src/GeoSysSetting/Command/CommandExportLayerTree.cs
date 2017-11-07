using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class CommandExportLayerTree : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        //Plugin.Application.IAppPrivilegesRef  m_Hook;
        //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //end added by chulili
        private Plugin.Application.IAppFormRef _hook;

        public CommandExportLayerTree()
        {
            base._Name = "GeoSysSetting.CommandExportLayerTree";
            base._Caption = "导出";
            base._Tooltip = "导出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导出";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return false ;
                if (_hook.MainForm.Controls[0] is SubControl.UCDataSourceManger)
                {
                    return true;
                }
                return false;
            }
        }

        public override string Message
        {
            get
            {//changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppFormRef
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppFormRef
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            //弹出对话框供用户选择导出路径
            SaveFileDialog pSaveFileDlg = new SaveFileDialog();
            pSaveFileDlg.Title = "保存图层目录";
            pSaveFileDlg.Filter = "XML数据(*.xml)|*.xml";
            if (pSaveFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("目录" + Caption);//xisheng 2011.07.09 增加日志
                }
                string xmlpath = pSaveFileDlg.FileName;
                //调用函数导出图层目录（由数据库导出到选定路径）
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, xmlpath);
                MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

    
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            //m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
            //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            _hook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
