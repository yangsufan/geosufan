using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

using SysCommon.Gis;
using SysCommon;
//编辑命名规则
namespace GeoDBConfigFrame
{
    public class InitSystem : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public InitSystem()
        {
            base._Name = "GeoDBConfigFrame.InitSystem";
            base._Caption = "系统初始化";
            base._Tooltip = "系统初始化";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "系统初始化";
        }
        public override void OnClick()
        {
            if (MessageBox.Show("确定要系统初始化吗？本操作会覆盖原来的系统业务数据", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            if (File.Exists(Mod.v_ConfigPath))
            {
                //工作库
                SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(Mod.v_ConfigPath, out Mod.Server, out Mod.Instance, out Mod.Database, out Mod.User, out Mod.Password, out Mod.Version, out Mod.dbType);
                bool blnCanConnect = CanOpenConnect(vgisDb, Mod.dbType, Mod.Server, Mod.Instance, Mod.Database, Mod.User, Mod.Password, Mod.Version);
            }
            
            SysGisDB gisDb = new SysGisDB();
            bool result=false;
            Exception eError;
            enumWSType wsType;
            switch (Mod.dbType)
            {
                case "SDE":
                    result = gisDb.SetWorkspace(Mod.Server, Mod.Instance, Mod.Database, Mod.User, Mod.Password, Mod.Version, out eError);
                    break;
                case "PDB":
                    wsType = enumWSType.PDB;
                    result = gisDb.SetWorkspace(Mod.Server, wsType, out eError);
                    break;
                case "GDB":
                    wsType = enumWSType.GDB;
                    result = gisDb.SetWorkspace(Mod.Server, wsType, out eError);
                    break;
                default:
                    break;
            }
            if (result == false)
                return;

            IWorkspaceFactory pWorkspaceFactory = null;
            IWorkspace pWorkspace = null;
            IPropertySet pPropertySet = new PropertySetClass();

            string dataPath = Application.StartupPath + "\\..\\Template\\DbInfoTemplate.gdb";
            pPropertySet.SetProperty("DATABASE", dataPath);
            pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
            if (pWorkspace == null) return;
            InitSystemByXML(pWorkspace, gisDb.WorkSpace);

        }
        private void InitSystemByXML(IWorkspace sourceWorkspace,IWorkspace targetWorkspace)
        {
            string xmlpath = Application.StartupPath + "\\..\\Template\\InitSystemConfig.Xml";
            XmlDocument doc=new XmlDocument();
            doc.Load(xmlpath );
            string strSearch = "//InitSystemRoot";
            XmlNode pInitSystemnode = doc.SelectSingleNode(strSearch );
            XmlNodeList pInitSystemlist = pInitSystemnode.ChildNodes;
            foreach (XmlNode pnode in pInitSystemlist)
            {
                if (pnode.NodeType == XmlNodeType.Element)
                {
                    XmlElement pEle = pnode as XmlElement;
                    string strTableName = pEle.GetAttribute("Name");
                    try
                    {
                        targetWorkspace.ExecuteSQL("drop table " + strTableName);
                    }
                    catch(Exception e)
                    {
                    }
                    CopyPasteGDBData.CopyPasteGeodatabaseData(sourceWorkspace, targetWorkspace, strTableName, esriDatasetType.esriDTTable);

                }
            }
            
        }
        private   bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "SDE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "PDB")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "GDB")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
