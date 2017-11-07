using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//添加记录
namespace GeoDBConfigFrame
{
    public class AddDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
     //   private Plugin.Application.IAppFormRef m_frmhook;
        public AddDataRcd()
        {
            base._Name = "GeoDBConfigFrame.AddDataRcd";
            base._Caption = "添加记录";
            base._Tooltip = "添加记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加记录";
        }
        //添加记录菜单响应
        public override void OnClick()
        {


            if (m_Hook.GridCtrl  == null)
                return;
            FaceControl pfacecontrol = (FaceControl)m_Hook.MainUserControl;
            DataGridView pGridControl = m_Hook.GridCtrl;
            if (pfacecontrol.getEditable() == false)
                return;
            string connstr, Tablename;
            //获取数据库连接串和表名
            connstr = pfacecontrol.m_connstr;
            Tablename = pfacecontrol.m_TableName;
            FaceControl pFaceControl =( FaceControl )(m_Hook.MainUserControl);
            if (pGridControl.DataSource == null)
                return;
            //初始化添加记录对话框
            TableForm myTableForm = new TableForm(pGridControl, connstr, Tablename);
            myTableForm.InitForm("ADD");
           DialogResult result= myTableForm.ShowDialog();
           if (result == DialogResult.OK)//changed by xisheng 2011.06.16
            { //记录添加后再次初始化dataview控件
                pfacecontrol.InitDataInfoList(Tablename,true);
            }
            else
            {
                pfacecontrol.InitDataInfoList(Tablename,false);
            }
           ////ZQ   20111017   add  及时更新数据字典
           switch (Tablename)
           {
               case "属性对照表":
                   SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
                   break;
               case"标准图层代码表":
                   SysCommon.ModField.InitLayerNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicLayerName);
                   break;
               //default:
               //    ///ZQ 20111020 add 增加重启提示
               //    MessageBox.Show("添加的记录只有应用系统重启以后才能生效！","提示！");
               //    break;
           }
           /////
           if (this.WriteLog)
           {
               Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
           }
            //OleDbCommandBuilder builder = new OleDbCommandBuilder(pFaceControl.m_Adapter );
            ////将数据提交到数据库更新
            //try
            //{
            //    pFaceControl.m_Adapter.Update(pFaceControl.m_dataTable);
            //    //提交后重新获取数据
            //    pFaceControl.m_dataTable.Clear();
            //    pFaceControl.m_Adapter.Fill(pFaceControl.m_dataTable);
            //    pGridControl.DataSource = null;
            //    pGridControl.DataSource = pFaceControl.m_dataTable;
            //    //gridControl.Update();
            //}
            //catch (System.Exception m)
            //{
            //    Console.WriteLine(m.Message);
            //}
            //pGridControl = null;
            //pFaceControl = null;
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
         //   m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
