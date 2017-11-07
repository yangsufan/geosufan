using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    //=============================
    //作者：席胜
    //时间：2011-2-22
    //增加进度条开始结束调用方法
    //=============================
    public class GeoProgressBar
    {
        //设置进度条窗口为全局变量，等会设置状态时调用
        ProgressBarStatDlg prgfm;
       //无状态显示的进度条调用
        public void ProgressBar_Start(Form fm)
        {
            ProgressBarDlg prgfm = new ProgressBarDlg();
            prgfm.Show(fm);
        }
       
        //无状态显示的进度条关闭
        public void ProgressBar_End(Form fm)
        {
            fm.OwnedForms[0].Close();
            //Form.ActiveForm.Close();
        }

        //有状态显示的进度条调用
        public void ProgressBarStat_Start(Form fm)
        {
            prgfm = new ProgressBarStatDlg();
            prgfm.Show(fm);
        }


        //有状态显示的进度条关闭
        public void ProgressBarStat_End(Form fm)
        {
            fm.OwnedForms[0].Close();
            //Form.ActiveForm.Close();
        }

        //设置进度状态值
        public void SetState(string value)
        {
           
            if (Form.ActiveForm.Equals(prgfm))
                prgfm.Value = value;

            
           
        }
    }

}
