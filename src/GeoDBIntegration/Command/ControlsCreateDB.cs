using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SysCommon.Authorize;

namespace GeoDBIntegration
{
    public class ControlsCreateDB: Plugin.Interface.CommandRefBase
    {
      
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //added by chulili 20110624 为了判断可用状态添加变量
        private Plugin.Application.IAppFormRef _hook;
        public ControlsCreateDB()
        {
            base._Name = "ControlsCreateDB.ControlsCreateDB";
            base._Caption = "创建数据库体";
            base._Tooltip = "创建数据库体";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "创建数据库体";

        }
        public override bool Enabled
        {
            get
            {
                ////若没有登录系统，则按钮不可用
                //if (ModuleData.m_User == null) return false;
                ////若用户不是管理员，则按钮不可用
                //if (ModuleData.m_User.RoleTypeID != EnumRoleType.管理员.GetHashCode()) return false;

                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;

                //cyf 20110602 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                //added by chulili 20110624 若不处于数据源管理界面  菜单不可用
                if (!(_hook.MainForm.Controls[0] is UserControlDBIntegra))
                {
                    return false;
                }
                //added by chulili 20110705界面不可见，菜单不可用
                if(!_hook.MainForm.Controls[0].Visible )
                {
                    return false;
                }

                //end add
                foreach (Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
                {
                    if (pRole.TYPEID == EnumRoleType.管理员.GetHashCode().ToString())
                    {
                        beAdmin = true;
                        break;
                    }
                }
                return true;
                //end
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
           //为不同的子系统创建不同的数据库
            if (m_Hook.ProjectTree.SelectedNode == null) return;
            if (m_Hook.ProjectTree.SelectedNode.Tag == null) return;
            XmlElement ele = m_Hook.ProjectTree.SelectedNode.Tag as XmlElement;
            if (ele == null) return;
            //cyf 20110628  add
            //string sDBType = string.Empty;
            //try
            //{
            //    sDBType = ele.GetAttribute("数据库类型");
            //}
            //catch
            //{
            //    return;
            //}
            string pDBTypeID = "";        //数据库类型ID  
            try { pDBTypeID = ele.GetAttribute("数据库类型ID").Trim(); }
            catch { }
            //end
            if (pDBTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
            {
                frmCreateDB CreateDbOper = new frmCreateDB(m_Hook.ProjectTree.SelectedNode);
                CreateDbOper.ShowDialog();
            }
            else if (pDBTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
            {
                FrmCreateRasterDB RasterCreateDB = new FrmCreateRasterDB(m_Hook.ProjectTree.SelectedNode, pDBTypeID);
                RasterCreateDB.ShowDialog();
            }
            else if (pDBTypeID == enumInterDBType.高程数据库.GetHashCode().ToString())
            {
                FrmCreateRasterDB RasterCreateDB = new FrmCreateRasterDB(m_Hook.ProjectTree.SelectedNode, pDBTypeID);
                RasterCreateDB.ShowDialog();
            }
            else if (pDBTypeID == enumInterDBType.地名数据库.GetHashCode().ToString())
            {

            }
            else if (pDBTypeID == enumInterDBType.地理编码数据库.GetHashCode().ToString())
            {

            }
            else if (pDBTypeID == enumInterDBType.成果文件数据库.GetHashCode().ToString())
            {
                frmCreateDB CreateDbOper = new frmCreateDB(m_Hook.ProjectTree.SelectedNode);
                CreateDbOper.ShowDialog();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
            if (m_Hook == null) return;
        }
    }
}
