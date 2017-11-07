using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;

//修改记录
namespace GeoDBConfigFrame
{
    public class ModifyDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public ModifyDataRcd()
        {
            base._Name = "GeoDBConfigFrame.ModifyDataRcd";
            base._Caption = "修改记录";
            base._Tooltip = "修改记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改记录";
        }
        //添加记录菜单响应
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
            FaceControl pFaceControl = (FaceControl)(m_Hook.MainUserControl);
            if (pGridControl.DataSource == null)
                return;
            //如果未选中记录。。。
            if (pGridControl.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选中记录！");
                return;
            }
            //如果选中多行记录。。。
            if (pGridControl.SelectedRows.Count > 1)
            {
                MessageBox.Show("仅可修改一行！");
                return;
            }
            int idx = pGridControl.SelectedRows[0].Index;//yjl20111103 add
            //初始化修改记录对话框
            TableForm myTableForm = new TableForm(pGridControl,connstr,Tablename);
            myTableForm.InitForm("MODIFY");
            myTableForm.ShowDialog();
            //记录修改后再次初始化dataview控件
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
                //    MessageBox.Show("修改的记录只有应用系统重启以后才能生效！", "提示！");
                //    break;
            }
            
            
            //yjl20111103 add 
            if (pGridControl.Rows.Count > idx)
            {
                //取消默认的第1行选中
                pGridControl.Rows[0].Selected = false;
                //设置选择行
                pGridControl.Rows[idx].Selected = true;
                //设置当前行
                pGridControl.CurrentCell = pGridControl.Rows[idx].Cells[0];
            }
            /////
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
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
