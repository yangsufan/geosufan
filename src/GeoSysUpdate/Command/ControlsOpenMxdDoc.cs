using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;

namespace GeoSysUpdate
{
    public class ControlsOpenMxdDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsOpenMxdDoc()
        {
            base._Name = "GeoSysUpdate.ControlsOpenMxdDoc";
            base._Caption = "打开地图文档";
            base._Tooltip = "打开地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "打开地图文档";
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
        {
            if (!ModMxd._MxdPath.Equals(""))
            {
                DialogResult pResult = MessageBox.Show("是否保存当前的地图文档?", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (pResult)
                {
                    case DialogResult.Cancel:
                        return;
                        break;
                    case DialogResult.Yes:
                        {
                            IMxdContents pMxdC;

                            pMxdC = _AppHk.MapControl.Map as IMxdContents;

                            IMapDocument pMapDocument = new MapDocumentClass();
                            //打开地图文档
                            if (File.Exists(ModMxd._MxdPath))
                            {
                                pMapDocument.Open(ModMxd._MxdPath, "");
                            }
                            else
                            {
                                pMapDocument.New(ModMxd._MxdPath);
                            }
                            //保存信息
                            IActiveView pActiveView = _AppHk.MapControl.Map as IActiveView;

                            pMapDocument.ReplaceContents(pMxdC);

                            pMapDocument.Save(true, true);

                            break;
                        }
                    case DialogResult.No:
                        break;
                }
            }
            OpenFileDialog pOpendlg = new OpenFileDialog();
            pOpendlg.Title = "打开地图文档";

            pOpendlg.Filter = "(*.mxd)|*.mxd";
            if (pOpendlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string strMxdName = pOpendlg.FileName;
            if (_AppHk.MapControl.CheckMxFile(strMxdName))
            {
                _AppHk.MapControl.LoadMxFile(strMxdName, "", "");
            }
            UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            if (pUserControl != null)
            {
                pUserControl.OpenMxdDocDeal();
            }
            ModMxd._MxdPath = strMxdName;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
