using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//全部删除
namespace GeoDBConfigFrame
{
    public class DelAllDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public DelAllDataRcd()
        {
            base._Name = "GeoDBConfigFrame.DelAllDataRcd";
            base._Caption = "全部删除";
            base._Tooltip = "全部删除";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "全部删除";
        }
        //全部删除菜单响应
        public override void OnClick()
        {
            
            if (m_Hook.GridCtrl == null)
                return;
            FaceControl pfacecontrol = (FaceControl)m_Hook.MainUserControl;
            DataGridView pGridControl = m_Hook.GridCtrl;
            if (pfacecontrol.getEditable() == false)
                return;
            string connstr, Tablename;
            //获取数据库连接串和表名
            connstr = pfacecontrol.m_connstr;
            Tablename = pfacecontrol.m_TableName;
            if (Tablename.Contains("."))
                Tablename = Tablename.Split('.')[1];//处理SDE表
            FaceControl pFaceControl = (FaceControl)(m_Hook.MainUserControl);
            if (pGridControl.DataSource == null)
                return;
            //删除数据时要询问一次
            if (DevComponents.DotNetBar.MessageBoxEx.Show("确定要全部删除吗？删除后不可恢复！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            try
            {
                if (GeoDataCenterFunLib.LogTable.m_sysTable == null)
                    return;
                Exception ex = null;
                LogTable.m_sysTable.DeleteRows(Tablename, "", out ex);//删除
                if (ex != null)
                    throw new Exception("删除全部数据失败！", ex);
            }
            catch (Exception pEx)
            {
                DevComponents.DotNetBar.MessageBoxEx.Show(pEx.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pfacecontrol.InitDataInfoList(Tablename);
            ////ZQ   20111017   add  及时更新数据字典
            switch (Tablename)
            {
                case "属性对照表":
                    SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
                    break;
                case "标准图层代码表":
                    SysCommon.ModField.InitLayerNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicLayerName);
                    break;
                //default:
                //    ///ZQ 20111020 add 增加重启提示
                //    MessageBox.Show("删除的记录只有应用系统重启以后才能生效！", "提示！");
                //    break;
            }
            /////
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            //OleDbConnection mycon = new OleDbConnection(connstr);   //定义OleDbConnection对象实例并连接数据库
            ////构造删除数据的语句
            //string strExp = "delete from " + Tablename ;
            //OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            //mycon.Open();
            ////执行删除语句
            //aCommand.ExecuteNonQuery();
            //mycon.Close();
            ////再次初始化datagridview控件
            //pfacecontrol.InitDataInfoList(Tablename );

            //if (m_Hook != null)
            //{
            //    LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
            //    if (log != null)
            //    {
            //        log.Writelog("数据全部删除");

            //    }
            //}

            /*       Exception eError;
                   AddGroup frmGroup = new AddGroup();
                   if (frmGroup.ShowDialog() == DialogResult.OK)
                   {
                       ModuleOperator.DisplayRoleTree("", m_Hook.RoleTree, ref ModData.gisDb, out eError);
                       if (eError != null)
                       {
                           ErrorHandle.ShowFrmError("提示", eError.Message);
                           return;
                       }
                   }
             */
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
