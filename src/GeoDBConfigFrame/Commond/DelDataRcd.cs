using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;

using SysCommon.Error;
using ESRI.ArcGIS.Geodatabase;
//删除记录
namespace GeoDBConfigFrame
{
    public class DelDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public DelDataRcd()
        {
            base._Name = "GeoDBConfigFrame.DelDataRcd";
            base._Caption = "删除记录";
            base._Tooltip = "删除记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除记录";
        }
        //删除记录菜单响应
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
            //如果未选中记录...
            if (pGridControl.SelectedRows.Count == 0)
            { 
                DevComponents.DotNetBar.MessageBoxEx.Show("未选中记录！");
                return;
            }
            int k = pGridControl.SelectedRows.Count;
            //删除数据时，要询问一次
            if (pGridControl.SelectedRows.Count > 0)
            {
                if (DevComponents.DotNetBar.MessageBoxEx.Show("确定要删除选中的记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            //OleDbConnection mycon = new OleDbConnection(connstr);   //定义OleDbConnection对象实例并连接数据库
            //string strExp = "";
            //OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            //mycon.Open();
            //int i = 0, j = 0;
            if (GeoDataCenterFunLib.LogTable.m_sysTable == null)
                return;
            Exception err;
            ITable pTable =LogTable.m_sysTable.OpenTable(Tablename, out err);
            if (pTable == null)
            {
                //MessageBox.Show(err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ErrorHandle.ShowFrmErrorHandle("提示", err.Message);
                return;//若日志表不存在，返回null
            }
            if (pGridControl.SelectedRows.Count > 0)
            {
                string objectID = pTable.OIDFieldName;
                string strExp = objectID+" IN (";
                for (int h = 0; h < pGridControl.SelectedRows.Count; h++)
                {   
                    //构造删除记录的语句
                    
                    for (int i = 0; i < pGridControl.ColumnCount; i++)
                    {
                        if (pGridControl.Columns[i].Name.ToUpper().Equals("ID"))
                        {
                            
                            strExp += pGridControl.SelectedRows[h].Cells[i].Value.ToString()+",";
                        }
                    }
                }
                strExp = strExp.Substring(0, strExp.Length - 1);
                strExp += ")";
                //执行删除记录的语句段
                IWorkspace pWorkspace = LogTable.m_gisDb.WorkSpace;
                ITransactions pTransactions = (ITransactions)pWorkspace;
                try
                {

                    if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                }
                catch (Exception eX)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eX.Message);
                    return;
                }
                Exception exError;
                if (!LogTable.m_sysTable.DeleteRows(Tablename, strExp, out exError))
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "删除记录失败！" + exError.Message);
                    return;
                }
              
                try
                {
                    if (pTransactions.InTransaction) pTransactions.CommitTransaction();
                }
                catch (Exception eX)
                {
                }
            }
        
           
            //再次初始化datagridview控件
            pfacecontrol.InitDataInfoList(Tablename );
            
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
