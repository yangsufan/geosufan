using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Fan.Common.Error
{
    public static class ErrorHandle
    {
        public static void ShowFrmErrorHandleEx(string sCaption, string sErrDescription,List<string> pListErrInfo)
        {
            try
            {
                frmErrorHandleEx newFrm = new frmErrorHandleEx(sCaption, sErrDescription, pListErrInfo);
                //暂时取第2个窗体(主窗体).....有待修改
                FormCollection frmCol = Application.OpenForms;
                newFrm.Owner = frmCol[0];

                newFrm.ShowDialog();
            }
            catch (Exception eX)
            {
                //******************************************
                //guozheng added System Exception log
                if (Fan.Common.Log.Module.SysLog == null)
                    Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
                Fan.Common.Log.Module.SysLog.Write(eX);
                //******************************************
            }
        }
        public static void ShowFrmErrorHandle(string sCaption, string sErrDescription)
        {
            try
            {
                frmErrorHandle newFrm = new frmErrorHandle(sCaption, sErrDescription);
                //暂时取第2个窗体(主窗体).....有待修改
                FormCollection frmCol = Application.OpenForms;
                newFrm.Owner = frmCol[0];

                newFrm.ShowDialog();
            }
            catch (Exception eX)
            {
                //******************************************
                //guozheng added System Exception log
                if (Fan.Common.Log.Module.SysLog == null)
                    Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
                Fan.Common.Log.Module.SysLog.Write(eX);
                //******************************************
            }
        }
        /// <summary>
        /// 显示提示窗体
        /// </summary>
        /// <param name="sCaption"></param>
        /// <param name="sErrDescription"></param>
        public static void ShowInform(string sCaption, string sErrDescription)
        {
            try
            {
                frmErrorHandle newFrm = new frmErrorHandle(sCaption, sErrDescription);
                //暂时取第2个窗体(主窗体).....有待修改
                FormCollection frmCol = Application.OpenForms;
                newFrm.Owner = frmCol[0];

                newFrm.ShowDialog();
            }
            catch (Exception eX)
            {
                //******************************************
                //guozheng added System Exception log
                if (Fan.Common.Log.Module.SysLog == null)
                    Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
                Fan.Common.Log.Module.SysLog.Write(eX);
                //******************************************
            }
        }
        public static bool ShowFrmInformation(string strOkName, string strCancelName, string sErrDescription)
        {
            try
            {
                frmInformation newFrm = new frmInformation(strOkName, strCancelName, sErrDescription);

                //暂时取第2个窗体(主窗体).....有待修改
                FormCollection frmCol = Application.OpenForms;
                newFrm.Owner = frmCol[0];

                newFrm.ShowDialog();
                if (newFrm.DialogResult == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception eX)
            {
                //******************************************
                //guozheng added System Exception log
                if (Fan.Common.Log.Module.SysLog == null)
                    Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
                Fan.Common.Log.Module.SysLog.Write(eX);
                //******************************************
                return false;
            }
        }
    }
}
