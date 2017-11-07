using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GeoDBConfigFrame
{
    public static class ModSystemExport
    {
        public static void SDEExport()
        {
            //exp sde/sde@ OriSDE file=c:\sde.dmp
            //Exp RasterData/RasterData@ OriSDE file=c: RasterData.dmp

        }
        public static string ExeCommand(string commandText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                p.StandardInput.WriteLine(commandText);
                p.StandardInput.WriteLine("exit");
                //strOutput = p.StandardOutput.ReadToEnd();
                //p.WaitForExit(); //执行这句有某些问题，所以不执行，让cmd.exe在后台执行
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }

    }
}