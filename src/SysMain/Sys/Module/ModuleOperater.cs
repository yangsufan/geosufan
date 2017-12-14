using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SysCommon;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.DataBase;
using System.Data.OracleClient;
using System.Data.OleDb;
namespace GDBM
{
    public static class ModuleOperator
    {
        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Plugin.ModuleCommon.DicTabs == null || Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Plugin.Interface.IControlRef> keyValue in Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevExpress.XtraBars.Ribbon.RibbonPage, string> keyValue in Plugin.ModuleCommon.DicTabs)
            {
                //if (keyValue.Value == pSysName)
                //{
                //    i = i + 1;
                //    keyValue.Key.Visible = true;
                //    keyValue.Key.Enabled = true;
                //    if (i == 1)
                //    {
                //        //默认选中第一项
                //        keyValue.Key.Checked = true;
                //    }
                //}
                //else
                //{
                //    keyValue.Key.Visible = false;
                //    keyValue.Key.Enabled = false;
                //}
            }
        }

    }
}
