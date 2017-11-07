using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class ControlsDataSource : Plugin.Interface.CommandRefBase
    {
        private GeoSysSetting.SubControl.UCDataSourceManger ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlsDataSource()
        {
            base._Name = "GeoSysSetting.ControlsDataSource";
            base._Caption = "目录管理";
            base._Tooltip = "目录管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "目录管理";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return true;
                //根据时间刷新一下改空间是否显示
                if (_hook.Visible == false && this.ucCtl != null)
                {
                    this.ucCtl.Visible = false;
                }
                return true;
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
            
            if (ucCtl == null)
            {
                ucCtl = new SubControl.UCDataSourceManger(_hook as Plugin.Application.IAppFormRef);
                ucCtl.Dock = DockStyle.Fill;
                ucCtl.m_TmpWorkSpace = _hook.TempWksInfo.Wks;

                _hook.MainForm.Controls.Add(ucCtl);
            }
            else
            {
                ucCtl.RefreshLayerTree();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            ucCtl.Enabled = true;
            ucCtl.Visible = true;
            _hook.MainForm.Controls.SetChildIndex(ucCtl, 0);

            
            _hook.CurScaleCmb.SelectedIndexChanged += new EventHandler(CurScaleCmb_SelectedIndexChanged);

            //添加回车事件自定义比例尺
            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = _hook.CurScaleCmb.ComboBoxEx;
            vComboEx.KeyDown += new KeyEventHandler(vComboEx_KeyDown);

            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
        //响应回车时间 改变当前显示比例尺
        void vComboEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = sender as DevComponents.DotNetBar.Controls.ComboBoxEx;
            string strScale = vComboEx.Text;
            double dblSacle = 0;
            Plugin.Application.IAppDBIntegraRef pDBIntegraRef = _hook as Plugin.Application.IAppDBIntegraRef;
            try
            {
                if (double.TryParse(strScale, out dblSacle))
                {
                    pDBIntegraRef.MapControl.Map.MapScale = dblSacle;
                    pDBIntegraRef.MapControl.ActiveView.Refresh();
                }
                else
                {
                    vComboEx.Text = pDBIntegraRef.MapControl.Map.MapScale.ToString();
                }
            }
            catch
            {
            }
        }
        //参考比例尺事件　陈亚飞添加

        private void CurScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            Plugin.Application.IAppDBIntegraRef pDBIntegraRef = _hook as Plugin.Application.IAppDBIntegraRef;
            try
            {
                pDBIntegraRef.MapControl.Map.MapScale = Convert.ToDouble(_hook.CurScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                _hook.CurScaleCmb.Text = pDBIntegraRef.MapControl.Map.MapScale.ToString("0");
            }
            pDBIntegraRef.MapControl.ActiveView.Refresh();
        }

    }
}
