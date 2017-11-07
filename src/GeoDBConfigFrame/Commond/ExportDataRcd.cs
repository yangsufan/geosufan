using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//添加记录
namespace GeoDBConfigFrame
{
    public class ExportDataRcd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        //   private Plugin.Application.IAppFormRef m_frmhook;
        public ExportDataRcd()
        {
            base._Name = "GeoDBConfigFrame.ExportDataRcd";
            base._Caption = "导出记录";
            base._Tooltip = "导出记录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导出记录";
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
            string Tablename = "";
            //获取数据库连接串和表名
            Tablename = pFaceControl.m_TableName;
            SaveFileDialog pOpenFileDlg = new SaveFileDialog();
            pOpenFileDlg.Title = "设置导出Excel文件的名称";
            pOpenFileDlg.Filter = "Excel文件(*.xls)|*.xls";
            if (pOpenFileDlg.ShowDialog() != DialogResult.OK)
                return;
            string strFileName = pOpenFileDlg.FileName;
            //加进度条 chulili 2013-01-11
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);


            vProgress.ShowDescription = true;
            vProgress.FakeProgress = false;
            vProgress.TopMost = true;

            bool bRes =ModExcel.ExportTableToExcel(Plugin.ModuleCommon.TmpWorkSpace, Tablename, strFileName, vProgress);
            vProgress.Close();
            if (bRes)
            {
                MessageBox.Show("导出成功!");
            }
            
            //ModDBOperate.ExportTableToExcel(Tablename, strFileName);
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
