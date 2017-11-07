using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon;

namespace GeoSysSetting
{
    public class CommandImprtMxd : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public CommandImprtMxd()
        {
            base._Name = "GeoSysSetting.CommandImprtMxd";
            base._Caption = "导入符号模板";
            base._Tooltip = "导入符号模板";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入符号模板";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return false;
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
            {
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
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Exception eError;
            //弹出对话框供用户选择mxd文档
            OpenFileDialog pofd = new OpenFileDialog();
            pofd.Title = "选择符号文档";
            pofd.Filter = "Mxd文件|*mxd";
            pofd.RestoreDirectory = false;
            if (pofd.ShowDialog() == DialogResult.OK)
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("目录" + Caption);//xisheng 2011.07.09 增加日志
                }
                string filepath = pofd.FileName;
                bool iscover = false;
                if (MessageBox.Show("是否覆盖原有符号库？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    iscover = true;
                }
                else
                {
                    iscover = false;
                }
                //调用函数读取mxd符号文档到数据库表格中
                CProgress vProg = new CProgress("正在导入符号库，请稍候...");
                vProg.EnableCancel = false;
                vProg.ShowDescription = true;
                vProg.FakeProgress = true;
                vProg.TopMost = true;
                vProg.ShowProgress();
                if (GeoLayerTreeLib.LayerManager.ModuleMap.ReadmxdToDataBaseEx(filepath, "", Plugin.ModuleCommon.TmpWorkSpace, iscover, vProg))
                {
                    vProg.Close();
                    MessageBox.Show("导入成功！");
                }
                else
                {
                    vProg.Close();
                    MessageBox.Show("导入失败，请检查选择的符号文档！");
                }
                vProg = null;
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
