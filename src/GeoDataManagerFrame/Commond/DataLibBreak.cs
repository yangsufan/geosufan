using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using System.Threading;

//断开数据连接
namespace GeoDataManagerFrame
{
    public class DataLibBreak : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public Plugin.Application.IAppFormRef m_frmhook;
        public DataLibBreak()
        {
            base._Name = "GeoDataManagerFrame.DataLibBreak";
            base._Caption = "退出目录";
            base._Tooltip = "退出数据目录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "退出目录";
        }

        public override void OnClick()
        {
          //  GeoProgressBar dd = new GeoProgressBar();
          //  dd.ProgressBar_Start(m_frmhook.MainForm);

           
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("退出数据目录");

                }
                //清空图层
                m_Hook.MapControl.ActiveView.Clear();
                m_Hook.MapControl.ActiveView.Refresh();

                //清空文档视图
             //   m_Hook.RichBoxWordDoc.text = "";

                //清空地图文档树
                m_Hook.MapDocTree.Nodes.Clear();

        //        m_Hook.TextDocTree.Nodes.Clear();

                m_Hook.DocControl.Clear();
                //清空m_tparent
                SetControl.m_tparent = null;//added by xisheng 2011.04.02
               
                //清空数据索引
           //     m_Hook.DataIndexTree.Nodes.Clear();
                m_Hook.DataUnitTree.Nodes.Clear();

                //关闭工作区
                if (m_Hook.gisDataSet != null)
                {
                    if (m_Hook.gisDataSet.WorkSpace != null)
                    {
                        m_Hook.gisDataSet.CloseWorkspace(true);
                    }
                }
                
               
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
