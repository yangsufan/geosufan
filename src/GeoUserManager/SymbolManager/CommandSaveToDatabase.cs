using System;
using System.Collections.Generic;
using System.Text;
//using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;

namespace GeoUserManager
{
    public class CommandSaveToDatabase : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;

        public CommandSaveToDatabase()
        {
            base._Name = "GeoUserManager.CommandSaveToDatabase";
            base._Caption = "保存符号(上传)";
            base._Tooltip = "保存符号(上传)";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "保存符号(上传)";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                //if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                //if (_AppHk.MapControl.LayerCount == 0) return false;
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
        {
            try
            {
                System.Xml.XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                if (pXmlDoc == null) return;

                //上传到符号库中去
                Exception Err;
                bool result = false;
                SysCommon.Gis.SysGisDB vGisdb = new SysCommon.Gis.SysGisDB();
                result = vGisdb.SetWorkspace(SdeConfig.Server, SdeConfig.Instance, SdeConfig.Database, SdeConfig.User, SdeConfig.Password, SdeConfig.Version, out Err);
                if (!result) return;

                IWorkspace pWks = vGisdb.WorkSpace;
                if (pWks == null) return;

                ESRI.ArcGIS.esriSystem.IMemoryBlobStream pBlobStream = new ESRI.ArcGIS.esriSystem.MemoryBlobStreamClass();
                byte[] bytes = Encoding.Default.GetBytes(pXmlDoc.OuterXml);
                pBlobStream.ImportFromMemory(ref bytes[0], (uint)bytes.GetLength(0));

                IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
                ITable pTable = pFeaWks.OpenTable("SYMBOLINFO");
                IQueryFilter pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
                pQueryFilter.WhereClause = "SYMBOLNAME='ALLSYMBOL'";

                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                if (pRow == null)
                {
                    pRow = pTable.CreateRow();
                }

                pRow.set_Value(pRow.Fields.FindField("SYMBOLNAME"), "ALLSYMBOL");
                pRow.set_Value(pRow.Fields.FindField("SYMBOL"), pBlobStream);
                pRow.set_Value(pRow.Fields.FindField("UPDATETIME"), System.DateTime.Now);

                pRow.Store();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "符号信息上传服务器成功！");
            }
            catch (Exception ex)
            {

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "符号信息上传服务器出现错误！" + ex.Message);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;

        }
    }
}
