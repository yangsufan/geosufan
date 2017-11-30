using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// System Config Static Class
/// Set The System Config Item
/// </summary>
namespace SysCommon
{
  public static  class ModuleConfig
    {
        /// <summary>
        /// Connect File Name
        /// </summary>
        public static string m_ConnectFileName = string.Format("{0}\\{1}", System.Windows.Forms.Application.StartupPath,"conn.dat");
    }
}
