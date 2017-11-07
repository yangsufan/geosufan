using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Authorize;
using SysCommon.Gis;
using System.IO;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    class ControlsDeleteDBType : Plugin.Interface.CommandRefBase
    {
                private Plugin.Application.IAppDBIntegraRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;
        public ControlsDeleteDBType()
        {
            base._Name = "GeoDBIntegration.ControlsDeleteDBType";
            base._Caption = "删除数据库类型";
            base._Tooltip = "删除数据库类型";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除数据库类型";

        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;

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

                if (_hook.MainForm.Controls[0].Visible == false)
                {
                    return false;
                }

                //end add
                foreach(Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
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
            //判断配置文件是否存在
            bool blnCanConnect = false;
            SysGisTable DeleteTable = new SysGisTable(ModuleData.TempWks);
            SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
            Exception ex = null;
            if (File.Exists(ModuleData.v_ConfigPath))
            {
                //获得系统维护库连接信息
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModuleData.v_ConfigPath, out ModuleData.Server, out ModuleData.Instance, out ModuleData.Database, out ModuleData.User, out ModuleData.Password, out ModuleData.Version, out ModuleData.dbType);
                //连接系统维护库
                blnCanConnect = CanOpenConnect(vgisDb, ModuleData.dbType, ModuleData.Server, ModuleData.Instance, ModuleData.Database, ModuleData.User, ModuleData.Password, ModuleData.Version);
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + ModuleData.v_ConfigPath + "/n请重新配置");
                return;
            }
            if (!blnCanConnect)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                return;
            }
            ModuleData.TempWks = vgisDb.WorkSpace;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            //判断是否选择要删除的数据库 ygc 2012-9-3
            if (m_Hook.ProjectTree.SelectedNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择要删除的数据库！");
                return;
            }
            //判断选择的数据库类型下是否存在数据库连接 ygc 2012-9-3
             string IdValues="";
            try
            {
               IdValues = DeleteTable.GetFieldValue("DATABASETYPEMD", "ID", "databasetype='" + m_Hook.ProjectTree.SelectedNode.Text + "'", out ex).ToString();
            }
            catch
            {
                MessageBox.Show("请选择正确的数据库！","错误");
                return;
            }
                //string Id = DeleteTable.GetFieldValue("DATABASEMD", "ID", "databasetypeid='" + IdValues + "'",out ex).ToString ();
                if (IdValues == "" && IdValues == null)
                {
                    MessageBox.Show("","");
                    return;
                }
                if (DeleteTable.ExistData("DATABASEMD", "databasetypeid='" + IdValues + "'"))
                {
                   DialogResult result= MessageBox.Show(" 要删除的数据库类型下存在数据库连接，是否一起删除这些连接?","提示",MessageBoxButtons .OKCancel ,MessageBoxIcon.Question);
                   List<object> listOid = DeleteTable.GetFieldValues("DATABASEMD", "ID", "databasetypeid='" + IdValues + "'", out ex);
                   if (result == DialogResult.OK)
                   {
                       DialogResult result1 = MessageBox.Show("确定删除数据库类型" + m_Hook.ProjectTree.SelectedNode.Text + "?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                       if (result1 == DialogResult.OK)
                       {
                           try
                           {
                               bool flag1 = DeleteTable.DeleteRows("DATABASEMD", "databasetypeid='" + IdValues + "'", out ex);
                               //卸载数据库连接
                               for (int i = 0; i < listOid.Count; i++)
                               {
                                   ModuleData.v_DataBaseProPanel.RemoveDataBasePro(m_Hook.ProjectTree.SelectedNode.Text, Convert.ToInt64(listOid[i].ToString ()));
                               }
                               bool flag2 = DeleteTable.DeleteRows("DATABASETYPEMD", "databasetype='" + m_Hook.ProjectTree.SelectedNode.Text + "'", out ex);
                               if (flag1 && flag2)
                               {
                                   (ModuleData.v_AppDBIntegra.MainUserControl as UserControlDBIntegra).InitProjectTree();
                                   MessageBox.Show("成功删除数据库类型!", "提示");
                               }
                           }
                           catch
                           {
                               SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.ToString());
                           }

                       }
                       else return;

                   }
                   else return;

                }
                else
                {
                    DialogResult result1 = MessageBox.Show("确定删除数据库类型" + m_Hook.ProjectTree.SelectedNode.Text + "?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result1 == DialogResult.OK)
                    {
                        try
                        {
                            bool flag = DeleteTable.DeleteRows("DATABASETYPEMD", "databasetype='" + m_Hook.ProjectTree.SelectedNode.Text + "'", out ex);
                            if (flag)
                            {
                                (ModuleData.v_AppDBIntegra.MainUserControl as UserControlDBIntegra).InitProjectTree();
                                MessageBox.Show("成功删除数据库类型!", "提示");
                            }
                        }
                        catch
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.ToString());
                        }

                    }
                    else return;

                }
            Plugin.LogTable.Writelog(" 删除数据库类型【" + m_Hook.ProjectTree.SelectedNode.Text + "】成功！");       

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
            if (m_Hook == null) return;
        }


        //测试链接信息是否可用
        private bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }

    }
}
