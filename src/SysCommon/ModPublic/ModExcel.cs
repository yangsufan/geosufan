using System;
using System.Runtime.InteropServices;
namespace SysCommon
{
    public static class ModExcel
    {
        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

            int k = 0;
            try
            {
                GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
                p.Kill();     //关闭进程k
            }
            catch
            { }
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
    }
}