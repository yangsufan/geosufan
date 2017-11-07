﻿using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using System.Xml;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoStatistics
{
    public class CommandEcostatistics : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;

        public CommandEcostatistics()
        {
            base._Name = "GeoStatistics.CommandEcostatistics";
            base._Caption = "底表统计";
            base._Tooltip = "底表统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "底表统计";

        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (_AppHk != null)
                {
                    if (_AppHk.MapControl != null)
                    {
                        if (_AppHk.MapControl.Map.LayerCount > 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private void GetConfig(string ConfigPath, out string LinBanLayerKey, out string LinBanCodeField, out string XzqField)
        {
            LinBanLayerKey = "";
            LinBanCodeField = "";
            XzqField = "";

            if (!System.IO.File.Exists(ConfigPath))
            {
                return;
            }
            try
            {
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(ConfigPath);
                string strSearch = "//QueryConfig/QueryItem[@ItemText=" + "'最大林斑号查询'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode == null)
                {
                    return;
                }
                XmlNodeList pNodeList = pNode.SelectNodes(".//LayerItem");
                if (pNodeList.Count > 0)
                {
                    XmlNode pXZnode = pNodeList[0];
                    LinBanLayerKey = pXZnode.Attributes["NodeKey"].Value;//行政地名图层名

                }
                XmlNode pFieldNode = pNode.SelectSingleNode(".//FieldItem[@LabelText='小斑号：']");
                if (pFieldNode != null)
                {
                    LinBanCodeField = pFieldNode.Attributes["FieldName"].Value;
                }
                pFieldNode = pNode.SelectSingleNode(".//FieldItem[@LabelText='行政区：']");
                if (pFieldNode != null)
                {
                    XzqField = pFieldNode.Attributes["FieldName"].Value;
                }
            }
            catch
            { }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MainUserControl == null) return;
            if (_AppHk.MapControl.LayerCount == 0) { DevComponents.DotNetBar.MessageBoxEx.Show("未加载数据！", "提示！"); return; }

            string LinBanLayerKey = "", LinBanCodeField = "", XzqField = "";

            string playerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树0.xml";
            string strConfigPath = Application.StartupPath + "\\..\\res\\xml\\林班号查询.xml";

            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace, "最大林斑号", strConfigPath);
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath);
            GetConfig(strConfigPath, out LinBanLayerKey, out LinBanCodeField, out XzqField);

            IFeatureClass _LinBanFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath, LinBanLayerKey);
            try
            {
                System.IO.File.Delete(strConfigPath);
                System.IO.File.Delete(playerTreePath);
            }
            catch
            { }
            if (_LinBanFeatureClass == null) { DevComponents.DotNetBar.MessageBoxEx.Show("未添加林斑数据！", "提示！"); return; }
            //添加进度条 ygc 2012-10-8
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = false;
            vProgress.EnableUserCancel(false);
            vProgress.MaxValue = 8;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            vProgress.SetProgress("正在进行八大类总体统计");
            ModCommon.DoEcostatistic(_LinBanFeatureClass, "xbmj", "xjl", "jjlzs", vProgress);
            vProgress.Close();
            //MessageBox.Show("统计成功！", "提示");
            DevComponents.DotNetBar.MessageBoxEx.Show("统计成功！", "提示");
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;

            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
    }
}
