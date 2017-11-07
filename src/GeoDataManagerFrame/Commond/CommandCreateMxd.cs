using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;
namespace GeoDataManagerFrame
{
    public class CommandCreateMxd : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandCreateMxd()
        {
            base._Name = "GeoDataManagerFrame.CommandCreateMxd";
            base._Caption = "创建显示方案";
            base._Tooltip = "创建显示方案";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "创建显示方案";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentControl == null) return false;
                if (m_Hook.CurrentControl is ISceneControl) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("创建书签");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FrmSaveMxd pFrm = new FrmSaveMxd(Plugin.ModuleCommon.TmpWorkSpace);
            DialogResult pRes= pFrm.ShowDialog();
            if (pRes == DialogResult.OK)
            {
                Dictionary<string, object> pDic = new Dictionary<string, object>();
                pDic.Add(SysCommon.ModSysSetting._MxdListTable_NameField, pFrm.m_Name);
                pDic.Add(SysCommon.ModSysSetting._MxdListTable_DescripField, pFrm.m_Description);
                pDic.Add(SysCommon.ModSysSetting._MxdListTable_UserField, pFrm.user);
                pDic.Add(SysCommon.ModSysSetting._MxdListTable_ShareField, pFrm.m_Share.ToString());
                WriteMapToDB(m_Hook.ArcGisMapControl.Map, pDic, pFrm.m_Condition);
                pFrm = null;
            }
        }
        private void WriteMapToDB(IMap pMap, Dictionary<string,object> pDic, string strCondition)
        {
            //linyand add set map SpatialReference
            //pMap.SpatialReference = LoadProjectedCoordinateSystem();

            IPersistStream pPersistStream = pMap as IPersistStream;
            IStream pStream = new XMLStreamClass();
            pPersistStream.Save(pStream, 0);

            IXMLStream pXMLStream = pStream as IXMLStream;
            byte[] RenderByte = pXMLStream.SaveToBytes();
            IMemoryBlobStream pMemoryBlobStream = new MemoryBlobStreamClass();
            pMemoryBlobStream.ImportFromMemory(ref RenderByte[0], (uint)RenderByte.GetLength(0));


            pDic.Add(SysCommon.ModSysSetting._MxdListTable_MapField, pMemoryBlobStream);
            //采用更新blob字段的方法
            SysGisTable sysTable=new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            object objValue = sysTable.GetFieldValue(SysCommon.ModSysSetting._MxdListTable, SysCommon.ModSysSetting._MxdListTable_MapField, strCondition, out eError);
            bool flag = false;
            if (objValue == null)
            {
               flag= sysTable.NewRow(SysCommon.ModSysSetting._MxdListTable, pDic, out eError);
            }
            else
            {
                flag=sysTable.UpdateRow(SysCommon.ModSysSetting._MxdListTable, strCondition, pDic, out eError);
            }
            sysTable = null;
            if (flag)
            {
                MessageBox.Show("保存显示方案成功！", "提示");
            }
            else
            {
                MessageBox.Show("保存显示方案失败","提示");
            }
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}