using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 比例尺
    /// </summary>
   public class ControlsScaleSel: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef myHook;
        private DevComponents.DotNetBar.ComboBoxItem _ComboBoxScale;

       public ControlsScaleSel()
        {
            base._Name = "FileDBTool.ControlsScaleSel";
            base._Caption = "比例尺选择";
            base._Tooltip = "比例尺选择";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "比例尺选择";
        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (_ComboBoxScale == null)
                {
                    foreach (KeyValuePair<DevComponents.DotNetBar.BaseItem, string> kvCmd in Plugin.ModuleCommon.DicBaseItems)
                    {
                        if (kvCmd.Value == "FileDBTool.ControlsScaleSel")
                        {
                            DevComponents.DotNetBar.ComboBoxItem aComboBoxItem = kvCmd.Key as DevComponents.DotNetBar.ComboBoxItem;
                            if (aComboBoxItem != null)
                            {
                                aComboBoxItem.ComboWidth = 150;
                                _ComboBoxScale = aComboBoxItem;
                                _ComboBoxScale.SelectedIndexChanged += new EventHandler(ComboBoxScale_SelectedIndexChanged);
                                break;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public override void OnClick()
        {

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppFileRef;

            //获得选中的树节点的类型
            //若节点类型为EnumTreeNodeType.DATAFORMAT，则加载比例尺信息

            //_ComboBoxScale.Items.Clear();
            //_ComboBoxScale.Items.AddRange(new object[] { "不选择", "500", "1000", "2000", "5000", "10000", "20000" });
            //_ComboBoxScale.SelectedIndex = 0;

        }
       private void ComboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
       {
           //执行事件
           //string a = this._ComboBoxScale.Items[0].ToString();
       }
    }
}
