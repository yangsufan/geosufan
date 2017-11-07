using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei 20110314 add content:删除角色或者用户
    /// </summary>
    public partial class DelGroup : DevComponents.DotNetBar.Office2007Form
    {
        bool m_beSuccedd=true;   //用来标注界面初始化是否成功
        public bool BeSuccedd
        {
            get {return m_beSuccedd;}
            set {m_beSuccedd=value;}
        }
        bool m_BeRole = true;    //是否删除角色
        public DelGroup(bool beRole)
        {
            InitializeComponent();
            m_BeRole = beRole;
            //初始化角色列表框
            InitialForm();

            if (beRole)
            {
                this.Text = "删除角色";
            }
            else
            {
                this.Text = "删除用户";
            }
        }

        /// <summary>
        /// 初始化列表框
        /// </summary>
        private void InitialForm()
        {
            Exception outError = null;        //异常
            SysCommon.DataBase.SysTable pSysDB = null; //连接数据库类 
            //连接数据库
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                return;
            }
            string str = "";              //查询字符串
            if (m_BeRole)
            {
                //查询角色基本信息表
               // str = "select * from rolebaseinfo";  //定义查询字符串  3为系统管理员
                str = "select * from rolebaseinfo r join (select roletypeid from roletypeinfo ) t on (r.roletypeid=t.roletypeid) where t.roletypeid<> 3";
            }
            else
            {
                //查询用户基本信息表
                // str = "select * from userbaseinfo";//定义查询字符串  3为系统管理员
                str = "select * from userbaseinfo u join  userrolerelationinfo r on (u.userid=r.userid) join (select roleid,roletypeid from rolebaseinfo) b on (r.roleid=b.roleid) where b.roletypeid<>3";

            }
            //查询角色信息表
            DataTable pTable = pSysDB.GetSQLTable(str, out outError);
            if (outError != null || pTable==null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色信息表失败");
                pSysDB.CloseDbConnection();
                m_beSuccedd = false;
                return;
            }
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                int pRoleID =Convert.ToInt32(pTable.Rows[i][0].ToString().Trim());   //角色ID
                string pRoleName = pTable.Rows[i][1].ToString().Trim();              //角色名称
                ListViewItem pItem = new ListViewItem();
                pItem.Text = pRoleName;
                pItem.Tag = pRoleID;
                pItem.Checked = false;
                if (!LstViewRole.Items.Contains(pItem))
                {
                    LstViewRole.Items.Add(pItem);
                }
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            //遍历列表框，设置全选 
            for (int i = 0; i < LstViewRole.Items.Count; i++)
            {
                ListViewItem pItem = new ListViewItem();
                pItem = LstViewRole.Items[i];
                pItem.Checked = true;
            }
        }

        private void btnConSel_Click(object sender, EventArgs e)
        {
            //遍历列表框，设置反选
            for (int i = 0; i < LstViewRole.Items.Count; i++)
            {
                ListViewItem pItem = new ListViewItem();
                pItem = LstViewRole.Items[i];
                if (pItem.Checked)
                {
                    pItem.Checked = false;
                }
                else
                {
                    pItem.Checked = true;
                }
                
            }
        }

        private void btnDelSel_Click(object sender, EventArgs e)
        {
            if (LstViewRole.CheckedItems.Count == 0) return;
            if (m_BeRole)
            {
                #region 删除角色及其相关信息，1、删除角色用户关系表，2、删除角色表
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "是否删除选中角色及其对应的所有用户？"))
                {
                    //删除
                    SysCommon.DataBase.SysTable pSysDB = null;
                    Exception outError = null;
                    //连接数据库
                    ModDBOperate.ConnectDB(out pSysDB, out outError);
                    if (outError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                        return;
                    }

                    //开启事物
                    pSysDB.StartTransaction();
                    //遍历列表框，执行删除操作
                    for (int i = 0; i < LstViewRole.Items.Count; i++)
                    {

                        int pRoleID = -1;//角色ID
                        //***********************************//
                        //guozheng added
                        if (pRoleID == 3) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统管理员角色：" + LstViewRole.Items[i].Text + " 不能删除"); continue; }
                        ListViewItem pItem = new ListViewItem();
                        pItem = LstViewRole.Items[i];
                        if (!pItem.Checked) continue;
                        pRoleID = Convert.ToInt32(pItem.Tag.ToString());
                        string delStr = "";   //删除字符串

                        //删除角色用户关系表
                        delStr = "delete from userrolerelationinfo where ROLEID=" + pRoleID;
                        pSysDB.UpdateTable(delStr, out outError);
                        if (outError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //删除角色信息表
                        delStr = "delete from rolebaseinfo where ROLEID=" + pRoleID;
                        pSysDB.UpdateTable(delStr, out outError);
                        if (outError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除角色信息表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                    }

                    pSysDB.EndTransaction(true);
                    pSysDB.CloseDbConnection();

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除成功！");
                }
                #endregion
            }
            else
            {
                #region 删除用户及其相关信息,1、删除角色用户关系表，2、删除用户基本信息表
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "是否删除选中用户？"))
                {
                    //删除
                    SysCommon.DataBase.SysTable pSysDB = null;
                    Exception outError = null;
                    //连接数据库
                    ModDBOperate.ConnectDB(out pSysDB, out outError);
                    if (outError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                        return;
                    }

                    //开启事物
                    pSysDB.StartTransaction();
                    //遍历列表框，执行删除操作
                    for (int i = 0; i < LstViewRole.Items.Count; i++)
                    {

                        int pUserID = -1;//角色ID
                        ListViewItem pItem = new ListViewItem();
                        pItem = LstViewRole.Items[i];
                        if (!pItem.Checked) continue;
                        pUserID = Convert.ToInt32(pItem.Tag.ToString());
                        string delStr = "";   //删除字符串

                        //删除角色用户关系表
                        delStr = "delete from userrolerelationinfo where USERID=" + pUserID;
                        pSysDB.UpdateTable(delStr, out outError);
                        if (outError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        //删除该用户的作业分配区域
                        delStr = "delete from UPDATEINFO where USERID=" + pUserID;
                        pSysDB.UpdateTable(delStr, out outError);
                        if (outError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }

                        //删除角色信息表
                        delStr = "delete from userbaseinfo where USERID=" + pUserID;
                        pSysDB.UpdateTable(delStr, out outError);
                        if (outError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除角色信息表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                    }

                    pSysDB.EndTransaction(true);
                    pSysDB.CloseDbConnection();
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除成功！");
                }
                #endregion
            }
           
            this.Close();
        }
    }
}
