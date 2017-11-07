using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;

namespace GeoSysSetting
{
    public class CommandSaveLayerTree : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public CommandSaveLayerTree()
        {
            base._Name = "GeoSysSetting.CommandSaveLayerTree";
            base._Caption = "保存";
            base._Tooltip = "保存";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "保存";
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
            Exception eError;
            if (_hook == null)
            {
                return;
            }
            if (_hook.MainForm == null)
            {
                return;
            }
            SubControl.UCDataSourceManger pUcDataSource=_hook.MainForm.Controls[0] as SubControl.UCDataSourceManger;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("保存图层目录");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            
            if (pUcDataSource != null)
            {
                vProgress.SetProgress("保存图层顺序");
                pUcDataSource.DealLayerOrderID();   //对map中图层的顺序号进行处理，确保前后顺序正确
                pUcDataSource.SetOrderIDofAllLayer("Layer");   //对xml中的图层顺序号进行重新赋值，确保xml中所有顺序号前后关系正确，且都是整型
                pUcDataSource.RefreshOrderIDofAllLayer();
            }
            vProgress.SetProgress("保存图层目录配置文件");
            GeoLayerTreeLib.LayerManager.ModuleMap.SaveLayerTree(Plugin.ModuleCommon.TmpWorkSpace , ModPublicFun._layerTreePath );
            if (SysCommon.ModSysSetting.IsLayerTreeChanged)
            {
                SysCommon.ModSysSetting.IsConfigLayerTreeChanged = true;
            }
            SysCommon.ModSysSetting.IsLayerTreeChanged = false;
            vProgress.SetProgress("保存为初始状态");
            SaveInitState();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("目录" + Caption);//xisheng 2011.07.09 增加日志
            }
            vProgress.Close();
            MessageBox.Show("目录保存成功！");
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
        private void SaveInitState()
        {
            Plugin.Application.IAppDBIntegraRef _pDBIntegra = _hook as Plugin.Application.IAppDBIntegraRef;
            string strTmpPath = Application.StartupPath + "\\..\\Temp\\Tmpmxd" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".mxd";
            string strTmpPath2 = Application.StartupPath + "\\..\\Temp\\Tmpmxd" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "_.mxd";
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
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapDocument);
                pMapDocument = null;
                pMapDocument = new MapDocumentClass();
                pMapDocument.Open(strTmpPath, "");
                GeoLayerTreeLib.LayerManager.ModuleMap.SetLayersVisibleAttri(pMapDocument,true);
                ModPublicFun.WriteLog("pMapDocument.Save start");
                pMapDocument.SaveAs(strTmpPath2, true, true);
                ModPublicFun.WriteLog("pMapDocument.Save false");
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    ModPublicFun.WriteLog(ex.Message);
                }
            }
            pMapDocument.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapDocument);
            pMapDocument = null;
            try
            {
                System.IO.File.Delete(strTmpPath);
            }
            catch (Exception err2)
            {
            }
            if (System.IO.File.Exists(strTmpPath2))
            {
                SysGisTable mSystable = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
                Dictionary<string, object> pDic = new Dictionary<string, object>();
                //参数名
                pDic.Add("SETTINGNAME", "初始加载地图文档");
                //参数数据类型
                pDic.Add("DATATYPE", "MxdFile");
                //参数描述
                //pDic.Add("DESCRIPTION", textBoxSettingDescription.Text);
                //参数值（分简单参数值和文件型参数值）

                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                pBlobStream.LoadFromFile(strTmpPath2);
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
                    MessageBox.Show("保存出错：" + err.Message);
                }
                try
                {
                    System.IO.File.Delete(strTmpPath2);
                }
                catch (Exception err2)
                {
                }
            }            
        }
    }
}
