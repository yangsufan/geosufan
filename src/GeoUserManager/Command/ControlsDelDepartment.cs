using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Authorize;
using SysCommon.Error;
using SysCommon.Gis;

namespace GeoUserManager
{
    public class ControlsDelDepartment : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public ControlsDelDepartment()
        {
            base._Name = "GeoUserManager.ControlsDelDepartment";
            base._Caption = "删除科室";
            base._Tooltip = "删除科室";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除科室";
        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook != null)
                {
                    if (m_Hook.MainForm.Controls[0] is UCRole)
                    {
                        UCRole pRole = m_Hook.MainForm.Controls[0] as UCRole;
                        if (pRole.UCtag == "User" && m_Hook.UserTree.SelectedNode != null)
                        {
                            if (ModuleOperator.GroupByName == "科室")
                            {
                                if (m_Hook.UserTree.SelectedNode.Level == 1)
                                    return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (m_Hook.UserTree.SelectedNode != null)
            {
                Exception eError;
               string DeID  = m_Hook.UserTree.SelectedNode.TagString;
               string DePartmentName = m_Hook.UserTree.SelectedNode.Text;
               SysGisTable ksTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
                Exception error=null ;
               if (DeID == null||DeID =="") return;
               if (m_Hook.ConnUser.Name.ToLower() != "admin")
               {
                   MessageBox.Show("只有管理员用户才能进行此操作！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Stop);
                   return;
               }
               if (DePartmentName == "遥感室")
               {
                   MessageBox.Show("该科室下包含管理员用户，不可删除！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                   return;
               }
               if (MessageBox.Show("该操作将会删除该科室下全部用户，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
               {
                   return;
               }
               else
               {
                   //删除该科室下所有用户
                   List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
                   listDic = ksTable.GetRows("USER_INFO", "USERDEPARTMENT='" + DeID + "'", out error);
                   for (int i = 0; i < listDic.Count; i++)
                   {
                       Dictionary<string ,object > newdic =new Dictionary<string,object> ();
                       newdic =listDic [i];
                       string userID = newdic["USERID"].ToString ();
                       ksTable.DeleteRows("USER_EXPORT", "USERID='" + userID + "'", out error);
                   }
                       if (!ksTable.DeleteRows("USER_INFO", "USERDEPARTMENT='" + DeID + "'", out error))
                       {
                           MessageBox.Show("删除该科室下用户出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                           return;
                       }
                   //删除该科室信息
                   if (!ksTable.DeleteRows("USER_DEPARTMENT", "DEPARTMENTID='" + DeID + "'", out error))
                   {
                       MessageBox.Show("删除该科室出差！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                       return;
                   }
               }
                //刷新用户树
               ModuleOperator.DisplayUserTree("", m_Hook.UserTree, ref ModData.gisDb, out eError);
               if (this.WriteLog)
               {
                   Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
               }

            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }

    }
}
