using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class CommandImportLayerTree : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        //changed by chulili 20110722启动于数据源界面 IAppPrivilegesRef->IAppDBIntegraRef
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public CommandImportLayerTree()
        {
            base._Name = "GeoSysSetting.CommandImportLayerTree";
            base._Caption = "导入";
            base._Tooltip = "导入";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入";
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
            SubControl.UCDataSourceManger pUCdatasource = _hook.MainForm.Controls[0] as SubControl.UCDataSourceManger;
            if (pUCdatasource == null)
            {
                return;
            }
            //弹出对话框供用户选择导入的xml文件
            OpenFileDialog pOpenFileDlg = new OpenFileDialog();
            pOpenFileDlg.Title = "选择图层目录";
            pOpenFileDlg.Filter = "XML数据(*.xml)|*.xml";
            if (pOpenFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("目录" + Caption);//xisheng 2011.07.09 增加日志
                }
                string xmlpath = pOpenFileDlg.FileName;
				//added by chulili 20110729 判断导入目录的文件格式是否正确
                if (!GeoLayerTreeLib.LayerManager.ModuleMap.IsLayerTreeXmlRight(xmlpath))
                {
                    MessageBox.Show("选择的xml文件格式不正确!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (GeoLayerTreeLib.LayerManager.ModuleMap.SaveLayerTree(Plugin.ModuleCommon.TmpWorkSpace, xmlpath))
                {   //调用函数，将本地xml形式的图层目录导入到数据库中
                    SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, ModPublicFun._layerTreePath);
					//changed by chulili 20110722 导入后刷新图层RefreshLayerTree-》RefreshLayerTreeEx
                    pUCdatasource.RefreshLayerTreeEx();
                    SysCommon.ModSysSetting.IsConfigLayerTreeChanged = true;
                    MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("导入失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
