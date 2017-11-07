using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
    public class CommandModifyPassword : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef _AppHk;

         public CommandModifyPassword()
        {
            base._Name = "GeoDataManagerFrame.CommandModifyPassword";
            base._Caption = "修改密码";
            base._Tooltip = "修改密码";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改密码";
        }
         public override bool Enabled
         {
             get
             {
                 return true;
             }
         }
         public override void OnClick()
         {
             if (_AppHk.ConnUser == null) { return; }
             frmModifyPassword pfrmModifyPassword = new frmModifyPassword(_AppHk, Plugin.ModuleCommon.TmpWorkSpace);
             pfrmModifyPassword.ShowDialog();
         }


         public override void OnCreate(Plugin.Application.IApplicationRef hook)
         {
             if (hook == null) return;
             _AppHk = hook as Plugin.Application.IAppFormRef;
             if (_AppHk.MapControl == null) return;
         }








    }
}
