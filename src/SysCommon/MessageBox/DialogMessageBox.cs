using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 * 提示信息框类
 */
namespace Fan.Common
{
    public class DialogMessageBox
    {
        public static DialogResult Show(string msg, string title, MessageBoxButtons buttons)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, buttons);
        }
        public static DialogResult Show(string msg, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, buttons, icon);
        }
        public static DialogResult Show(string msg, string title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, buttons, icon, defaultButton);
        }
        #region 信息提示ShowInfo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowInfo(string msg)
        {
            return ShowInfo(msg, "系统提示");
        }
        public static DialogResult ShowInfo(string msg, string title)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region 信息提示ShowInfoError
        public static DialogResult ShowInfoError(string msg)
        {
            return ShowInfoError(msg, "系统提示");
        }
        public static DialogResult ShowInfoError(string msg, string title)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region 信息提示ShowInfoQuestion
        public static DialogResult ShowInfoQuestion(string msg)
        {
            return ShowInfoQuestion(msg, "系统提示");
        }
        public static DialogResult ShowInfoQuestion(string msg, string title)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        #endregion

        #region 信息提示ShowInfoWarning
        public static DialogResult ShowInfoWarning(string msg)
        {
            return ShowInfoWarning(msg, "系统提示");
        }
        public static DialogResult ShowInfoWarning(string msg, string title)
        {
            Cursor.Current = Cursors.Default;
            return XtraMessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion
    }
}
