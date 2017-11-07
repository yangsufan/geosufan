using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using SysCommon.Gis;

namespace GeoSysSetting
{
    public class CommandSaveInitState : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;
        private Plugin.Application.IAppDBIntegraRef _pDBIntegra;
        public CommandSaveInitState()
        {
            base._Name = "GeoSysSetting.CommandSaveInitState";
            base._Caption = "保存为初始状态";
            base._Tooltip = "保存为初始状态";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "保存为初始状态";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return false;
                if (_hook.MainForm.Controls[0] is SubControl.UCDataSourceManger)
                {
                    return true;
                }
                return false;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            string strTmpPath=Application.StartupPath+"\\..\\Temp\\Tmpmxd"+System.DateTime.Now.ToString("yyyyMMddHHmmss")+".mxd";
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\..\\Temp"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\..\\Temp");
            }
            IMxdContents pMxdC;

            pMxdC = _pDBIntegra.MapControl.Map as IMxdContents;

            IMapDocument pMapDocument = new MapDocumentClass();
            //创建地图文档
            pMapDocument.New(strTmpPath);

            //保存信息
            IActiveView pActiveView = _pDBIntegra.MapControl.Map as IActiveView;

            pMapDocument.ReplaceContents(pMxdC);
            try//yjl20110817 防止保存时，其他程序也在打开这个文档而导致共享冲突从而使系统报错
            {
                pMapDocument.Save(true, true);
            }
            catch (Exception ex)
            {
            }
            pMapDocument.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapDocument);
            pMapDocument = null;
            if (System.IO.File.Exists(strTmpPath))
            {
                SysGisTable mSystable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace );
                Dictionary<string, object> pDic = new Dictionary<string, object>();
                //参数名
                pDic.Add("SETTINGNAME", "初始加载地图文档");
                //参数数据类型
                pDic.Add("DATATYPE", "MxdFile");
                //参数描述
                //pDic.Add("DESCRIPTION", textBoxSettingDescription.Text);
                //参数值（分简单参数值和文件型参数值）

                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                pBlobStream.LoadFromFile(strTmpPath);
                pDic.Add("SETTINGVALUE2", pBlobStream);

                Exception err = null;
                bool bRes = false;
                if (mSystable.ExistData("SYSSETTING", "SETTINGNAME='初始加载地图文档'"))
                {
                    bRes = mSystable.UpdateRow("SYSSETTING", "SETTINGNAME='初始加载地图文档'", pDic, out err);
                }
                else
                {
                    bRes = mSystable.NewRow("SYSSETTING", pDic, out err);
                }
                if (!bRes)
                {
                    MessageBox.Show("保存出错："+err.Message);
                }
                try 
                {
                    System.IO.File.Delete(strTmpPath);
                }
                catch(Exception err2)
                {
                }
            }            

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;
            _pDBIntegra = hook as Plugin.Application.IAppDBIntegraRef;

        }
    }
}
