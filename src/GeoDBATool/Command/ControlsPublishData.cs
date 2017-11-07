using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public class ControlsPublishData : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsPublishData()
        {
            base._Name = "GeoDBATool.ControlsPublishData";
            base._Caption = "更新发布范围内的数据";
            base._Tooltip = "更新发布范围内的数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "更新发布范围内的数据";
        }
        public override bool Enabled
        {
            get
            {
                return true;
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

        private delegate void Clear();
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
        public override void OnClick()
        {
            //////////////////////////////////获取数据范围进行发布///////////////////////////////////
            FrmGetGeometry GetGeometryFrm = new FrmGetGeometry();
            if (System.Windows.Forms.DialogResult.OK == GetGeometryFrm.ShowDialog())
            {
                IGeometry pPublishArea = GetGeometryFrm.ResMapFrame;
                string sIp = GetGeometryFrm.IP;
                if (pPublishArea == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有获取到发布的数据范围");
                    return;
                }
                if (pPublishArea.IsEmpty)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有获取到发布的数据范围");
                    return;
                }
                try
                {
                    ///////////////////////////////获取工程名称，进行发布/////////////////////////////////////////
                    //若发布更新类为空，则初始化发布更新类
                    string strProjectName = ModData.v_AppGIS.ProjectTree.Nodes[0].Text;
                    Exception eError = null;
                    if (ModData.v_RemoteMsg == null)
                    {
                        //连接本地socket
                        ModData.v_RemoteMsg = new CSendUpdateMsg("", eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            return;
                        }
                    }
                    ModData.v_RemoteSesion = ModData.v_RemoteMsg.CSendUpdateMsgConn(sIp, out eError);
                    if (eError != null || ModData.v_RemoteSesion == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接远程发布更新服务器失败！");
                        return;
                    }
                    //等待
                    System.Threading.Thread.Sleep(300);
                    double pMinX = 0;        //发布更新范围的最小x坐标
                    double pMaxX = 0;        //发布更新范围的最大X坐标
                    double pMinY = 0;        //发布更新范围的最小Y坐标
                    double pMaxY = 0;        //发布更新范围的最大y坐标
                    IEnvelope pEnvelo = pPublishArea.Envelope;

                    pMinX = pEnvelo.XMin;
                    pMaxX = pEnvelo.XMax;
                    pMinY = pEnvelo.YMin;
                    pMaxY = pEnvelo.YMax;

                    //第一种情况  矩形  按照矩形范围发送消息

                    if (ModData.v_RemoteMsg.SendEnvelopUpdateMsg(strProjectName, pMinX, pMinY, pMaxX, pMaxY))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送成功！");
                    }
                    else
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送失败！");
                    }
                    //释放连接，清空SESSION
                    if (ModData.v_Msg != null)
                    {
                        ModData.v_Msg.DisConnect();
                        ModData.v_Sesion = null;
                    }
                    if (ModData.v_RemoteMsg != null)
                    {
                        ModData.v_RemoteMsg.DisConnect();
                        ModData.v_RemoteSesion = null;
                        //重新初始化发布更新类，创建本地socket
                        ModData.v_RemoteMsg = new CSendUpdateMsg("", eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            return;
                        }
                    }
                }
                catch(Exception eR)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送失败！具体信息请查看系统运行日志");
                    if (ModData.SysLog == null) ModData.SysLog.Write(eR);
                    return;
                }
            }

        }

      
        
    }
}
