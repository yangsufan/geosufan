using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
using System.Xml;

namespace GeoDataManagerFrame
{
    public class CommandMaxCadastraQuery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        //private IFeatureClass _JFFeatureClass = null;
        private IFeatureClass _LinBanFeatureClass = null;

        public CommandMaxCadastraQuery()
        {
            base._Name = "GeoDataManagerFrame.CommandMaxCadastraQuery";
            base._Caption = "最大林斑号查询";
            base._Tooltip = "最大林斑号查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "最大林斑号查询";
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;

                return true;
            }
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
        {//XZQLocation
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;
            if (_AppHk.MapControl.LayerCount == 0) { MessageBox.Show("未加载数据！", "提示！"); return; }

            string LinBanLayerKey = "", LinBanCodeField = "", XzqField = "";

            string playerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树0.xml";
            string strConfigPath = Application.StartupPath + "\\..\\res\\xml\\林班号查询0.xml";
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath);
            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace,"最大林斑号", strConfigPath);

            GetConfig(strConfigPath, out LinBanLayerKey, out LinBanCodeField, out XzqField);

            _LinBanFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, playerTreePath, LinBanLayerKey);
            if (_LinBanFeatureClass == null) { MessageBox.Show("未添加林斑数据！", "提示！"); return; }

            frmMaxCadastraQuery pfrmMaxCadastraQuery = new frmMaxCadastraQuery(_LinBanFeatureClass,  LinBanCodeField, XzqField, _AppHk.ArcGisMapControl as AxMapControl, Plugin.ModuleCommon.TmpWorkSpace);
            pfrmMaxCadastraQuery.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
            pfrmMaxCadastraQuery.Show();
            try
            {
                System.IO.File.Delete(strConfigPath);
                System.IO.File.Delete(playerTreePath);
            }
            catch
            { }

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

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }


    }
}
