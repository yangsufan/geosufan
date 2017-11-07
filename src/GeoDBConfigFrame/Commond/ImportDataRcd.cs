using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//添加记录
namespace GeoDBConfigFrame
{
    public class ImportDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        //   private Plugin.Application.IAppFormRef m_frmhook;
        public ImportDataRcd()
        {
            base._Name = "GeoDBConfigFrame.ImportDataRcd";
            base._Caption = "导入记录";
            base._Tooltip = "导入记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入记录";
        }
        //添加记录菜单响应
        public override void OnClick()
        {
            if (m_Hook.GridCtrl == null)
                return;
            FaceControl pFaceControl = (FaceControl)m_Hook.MainUserControl;
            DataGridView pGridControl = m_Hook.GridCtrl;
            if (pFaceControl.getEditable() == false)
                return;
            string  Tablename="";
            //获取数据库连接串和表名
            Tablename = pFaceControl.m_TableName;
            OpenFileDialog pOpenFileDlg = new OpenFileDialog();
            pOpenFileDlg.Title = "选择导入的Excel文件";
            pOpenFileDlg.Filter = "Excel文件(*.xls)|*.xls|文本文档(*.txt)|*.txt";
            if (pOpenFileDlg.ShowDialog() != DialogResult.OK)
                return;
            string strFileName = pOpenFileDlg.FileName;
            DialogResult pResult = MessageBox.Show("是覆盖已有记录(Y)还是追加导入(N)？", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            bool isCovered = true;
            if (pResult == DialogResult.Yes)
            {
                isCovered = true;
            }
            else if (pResult == DialogResult.No)
            {
                isCovered = false;
            }
            else
            {
                return;
            }
            ///增加导入文本文档功能  ZQ 20111101
            switch (System.IO.Path.GetExtension(strFileName))
            {
                case ".xls":
                    ModDBOperate.ImportExcelToTableEx(strFileName, Tablename, isCovered);
                    break;
                case".txt":
                    ModDBOperate.ImportTextToTable(strFileName, Tablename, isCovered);
                    break;   
            }
       
            //if (result == DialogResult.OK)//changed by xisheng 2011.06.16
            //{ //记录添加后再次初始化dataview控件
            pFaceControl.InitDataInfoList(Tablename, true);
            //}
            //else
            //{
            //    pFaceControl.InitDataInfoList(Tablename, false);
            //}
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
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
