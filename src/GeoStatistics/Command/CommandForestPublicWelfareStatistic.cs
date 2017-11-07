using System;
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
    public class CommandForestPublicWelfareStatistic : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;

        public CommandForestPublicWelfareStatistic()
        {
            base._Name = "GeoStatistics.CommandForestPublicWelfareStatistic";
            base._Caption = "国家级公益林地规划面积";
            base._Tooltip = "国家级公益林地规划面积";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "国家级公益林地规划面积";

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
            if (_AppHk.MapControl.LayerCount == 0) { MessageBox.Show("未加载数据！", "提示！"); return; }

            string LinBanLayerKey = "", LinBanCodeField = "", XzqField = "";

            string layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
            string strConfigPath = Application.StartupPath + "\\..\\res\\xml\\林班号查询.xml";

            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace, "最大林斑号", strConfigPath);
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, layerTreePath);
            GetConfig(strConfigPath, out LinBanLayerKey, out LinBanCodeField, out XzqField);

            IFeatureClass _LinBanFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, layerTreePath, LinBanLayerKey);
            if (_LinBanFeatureClass == null) { MessageBox.Show("未添加林斑数据！", "提示！"); return; }
            frmExportEightStatisticalTables newfrm = new frmExportEightStatisticalTables();
            newfrm.m_Caption = base.Caption;
            newfrm.m_pFeatrueClass = _LinBanFeatureClass;
            newfrm.ShowDialog();
            try
            {
                System.IO.File.Delete(strConfigPath);
                System.IO.File.Delete(layerTreePath);
            }
            catch
            { }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("八类统计--国家级公益林地规划面积"); //ygc 2012-9-14 写日志
            }

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
