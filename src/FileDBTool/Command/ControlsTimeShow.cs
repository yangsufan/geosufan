using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 显示时间
    /// </summary>
    public class ControlsTimeShow: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef myHook;
        private DevComponents.DotNetBar.ComboBoxItem _ComboBoxTime;
        DevComponents.AdvTree.Node v_ProNode = null;      //项目节点

        public ControlsTimeShow()
        {
            base._Name = "FileDBTool.ControlsTimeShow";
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
                if (_ComboBoxTime == null)
                {
                    //加载下拉列表框，并进行事件绑定
                    foreach (KeyValuePair<DevComponents.DotNetBar.BaseItem, string> kvCmd in Plugin.ModuleCommon.DicBaseItems)
                    {
                        if (kvCmd.Value == "FileDBTool.ControlsTimeShow")
                        {
                            DevComponents.DotNetBar.ComboBoxItem aComboBoxItem = kvCmd.Key as DevComponents.DotNetBar.ComboBoxItem;
                            if (aComboBoxItem != null)
                            {
                                aComboBoxItem.ComboWidth = 200;
                                _ComboBoxTime = aComboBoxItem;
                                _ComboBoxTime.SelectedIndexChanged += new EventHandler(ComboBoxTime_SelectedIndexChanged);
                                ModData.v_ComboxTime = _ComboBoxTime;
                                break;
                            }
                        }
                    }
                }
                bool b = false;
                if (myHook.ProjectTree.SelectedNode != null)
                { //根据时间和其他组合条件来查询

                    string treeNodeType = myHook.ProjectTree.SelectedNode.DataKey.ToString();
                    if(treeNodeType==EnumTreeNodeType.DATABASE.ToString())
                    {//根据时间进行查询

                        b = true;
                    }
                    else if(treeNodeType==EnumTreeNodeType.PROJECT.ToString())
                    {//根据项目、时间进行组合查询

                        v_ProNode=myHook.ProjectTree.SelectedNode;
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.DATAFORMAT.ToString())
                    {//根据项目、数据类型（DLG\DEM\DOM\DRG）和时间来组合查询

                        v_ProNode = myHook.ProjectTree.SelectedNode.Parent;
                        b = true;
                    }
                    else if (treeNodeType==EnumTreeNodeType.PRODUCT.ToString())
                    { //根据产品和时间来组合查询

                        v_ProNode = myHook.ProjectTree.SelectedNode.Parent.Parent;
                        b = true;
                    }
                    else if (treeNodeType==EnumTreeNodeType.PRODUCTPYPE.ToString())
                    {  //根据产品类型（标准图幅、非标准图幅、属性表）和时间来组合查询

                        v_ProNode = myHook.ProjectTree.SelectedNode.Parent.Parent.Parent;
                        b = true;
                    }
                }
                return b;
            }
        }

        public override void OnClick()
        {

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppFileRef;


            //添加项目
        }
        private void ComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
       {
           //执行事件
       }
    }
}
